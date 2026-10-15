#if PABCNET_MODERN
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using System.Security.Cryptography;

namespace PascalABCCompiler.NetHelper
{
    /// <summary>
    /// Owns the collectible load context used for non-framework references.
    /// A new generation is created when a referenced assembly changes.
    /// </summary>
    internal static class CollectibleReferenceManager
    {
        private const string StatisticsFileEnvironmentVariable =
            "PABCNET_TEST_COLLECTIBLE_STATS_FILE";
        private static ReferenceAssemblyLoadContext currentContext;
        private static WeakReference pendingUnload;
        private static HashSet<string> previousCompilationPaths =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static HashSet<string> currentCompilationPaths =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static readonly Dictionary<string, CachedAssemblyIdentity> identityCache =
            new Dictionary<string, CachedAssemblyIdentity>(StringComparer.OrdinalIgnoreCase);
        private static bool generationResetThisCompilation;
        private static int metadataReadCount;
        private static int contextCreationCount;
        private static int collectibleLoadCount;

        internal static Assembly Load(string assemblyPath)
        {
            if (currentContext == null)
                CreateContext(Path.GetDirectoryName(assemblyPath));
            Assembly assembly = currentContext.LoadRootAssembly(assemblyPath);
            collectibleLoadCount++;
            WriteStatistics();
            return assembly;
        }

        internal static void AddSearchDirectory(string assemblyPath)
        {
            string directory = Path.GetDirectoryName(assemblyPath);
            if (currentContext == null)
                CreateContext(directory);
            else
                currentContext.AddSearchDirectory(directory);
            currentContext.RememberSourceFile(assemblyPath);
        }

        private static void CreateContext(string baseDirectory)
        {
            currentContext = new ReferenceAssemblyLoadContext(baseDirectory);
            contextCreationCount++;
        }

        internal static void WriteStatistics()
        {
            string path = Environment.GetEnvironmentVariable(StatisticsFileEnvironmentVariable);
            if (string.IsNullOrEmpty(path))
                return;
            File.WriteAllText(path, metadataReadCount + "|" + contextCreationCount + "|" +
                collectibleLoadCount);
        }

        internal static void StartNextGeneration()
        {
            VerifyPreviousGenerationWasUnloaded();
            identityCache.Clear();

            ReferenceAssemblyLoadContext oldContext = currentContext;
            currentContext = null;
            generationResetThisCompilation = true;
            if (oldContext == null)
                return;

            oldContext.Unload();
            pendingUnload = new WeakReference(oldContext);
        }

        internal static Assembly[] GetCurrentAssemblies()
        {
            if (currentContext == null)
                return Array.Empty<Assembly>();
            return new List<Assembly>(currentContext.Assemblies).ToArray();
        }

        internal static bool CurrentGenerationHasChanged(bool includeMissingFiles = true)
        {
            return currentContext != null && currentContext.HasChanged(includeMissingFiles);
        }

        internal static void MarkValidationFailure()
        {
            currentCompilationPaths = new HashSet<string>(previousCompilationPaths,
                StringComparer.OrdinalIgnoreCase);
        }

        internal static void BeginCompilation()
        {
            previousCompilationPaths = currentCompilationPaths;
            currentCompilationPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            generationResetThisCompilation = false;
        }

        internal static bool RegisterReferencePaths(IEnumerable<string> assemblyPaths)
        {
            bool pathChanged = false;
            foreach (string path in assemblyPaths)
            {
                string fullPath = Path.GetFullPath(path);
                currentCompilationPaths.Add(fullPath);
                if (!generationResetThisCompilation && !previousCompilationPaths.Contains(fullPath))
                {
                    string fileName = Path.GetFileName(fullPath);
                    foreach (string previousPath in previousCompilationPaths)
                    {
                        if (string.Equals(Path.GetFileName(previousPath), fileName,
                            StringComparison.OrdinalIgnoreCase) ||
                            HaveSameSimpleName(previousPath, fullPath))
                        {
                            pathChanged = true;
                            break;
                        }
                    }
                }
                AddSearchDirectory(fullPath);
            }
            return pathChanged;
        }

        private static bool HaveSameSimpleName(string firstPath, string secondPath)
        {
            if (!File.Exists(firstPath) || !File.Exists(secondPath))
                return false;
            try
            {
                return string.Equals(GetCachedIdentity(firstPath).SimpleName,
                    GetCachedIdentity(secondPath).SimpleName,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch (FileLoadException)
            {
                return false;
            }
        }

        internal static void ValidateNoConflictingAssemblies(IEnumerable<string> assemblyPaths)
        {
            var rootPaths = new List<string>();
            var rootPathSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var rootDirectories = new List<string>();
            var rootDirectorySet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string path in assemblyPaths)
            {
                string fullPath = Path.GetFullPath(path);
                if (!File.Exists(fullPath) || !rootPathSet.Add(fullPath))
                    continue;
                rootPaths.Add(fullPath);
                string directory = Path.GetDirectoryName(fullPath);
                if (rootDirectorySet.Add(directory))
                    rootDirectories.Add(directory);
            }

            var paths = new List<string>();
            var visitedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string rootPath in rootPaths)
                DiscoverReferenceClosure(rootPath, rootDirectories, paths, visitedPaths);

            var bySimpleName = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);
            foreach (string fullPath in paths)
            {
                CachedAssemblyIdentity identity = GetCachedIdentity(fullPath);
                string previousPath;
                if (!bySimpleName.TryGetValue(identity.SimpleName, out previousPath))
                {
                    bySimpleName.Add(identity.SimpleName, fullPath);
                    continue;
                }

                CachedAssemblyIdentity previousIdentity = GetCachedIdentity(previousPath);
                if (!string.Equals(previousIdentity.FullName, identity.FullName,
                    StringComparison.Ordinal) || !FilesHaveSameContents(previousPath, fullPath))
                    throw new FileLoadException(
                        "Assemblies '" + previousPath + "' and '" + fullPath +
                        "' have the same internal simple name but different contents or identities. " +
                        "PascalABC.NET cannot reference both assemblies in one compilation.");
            }
        }

        private static void DiscoverReferenceClosure(string assemblyPath,
            List<string> searchDirectories, List<string> paths, HashSet<string> visitedPaths)
        {
            string fullPath = Path.GetFullPath(assemblyPath);
            if (!visitedPaths.Add(fullPath) || !File.Exists(fullPath))
                return;

            paths.Add(fullPath);
            CachedAssemblyIdentity identity = GetCachedIdentity(fullPath);
            string localDirectory = Path.GetDirectoryName(fullPath);
            foreach (string referenceName in identity.ReferencedAssemblyNames)
            {
                string dependencyPath = Path.Combine(localDirectory, referenceName + ".dll");
                if (!File.Exists(dependencyPath))
                {
                    dependencyPath = null;
                    foreach (string directory in searchDirectories)
                    {
                        string candidate = Path.Combine(directory, referenceName + ".dll");
                        if (File.Exists(candidate))
                        {
                            dependencyPath = candidate;
                            break;
                        }
                    }
                }
                if (dependencyPath != null)
                    DiscoverReferenceClosure(dependencyPath, searchDirectories, paths, visitedPaths);
            }
        }

        private static CachedAssemblyIdentity GetCachedIdentity(string path)
        {
            var file = new FileInfo(path);
            CachedAssemblyIdentity cached;
            if (identityCache.TryGetValue(path, out cached) &&
                cached.Length == file.Length && cached.LastWriteTimeUtc == file.LastWriteTimeUtc)
                return cached;

            try
            {
                metadataReadCount++;
                AssemblyName identity = AssemblyName.GetAssemblyName(path);
                cached = new CachedAssemblyIdentity(file.Length, file.LastWriteTimeUtc,
                    identity.Name, identity.FullName, ReadAssemblyReferenceNames(path));
                identityCache[path] = cached;
                return cached;
            }
            catch (BadImageFormatException ex)
            {
                throw new FileLoadException("Cannot read assembly metadata from '" + path + "'.",
                    path, ex);
            }
        }

        private static string[] ReadAssemblyReferenceNames(string path)
        {
            try
            {
                using (FileStream stream = File.OpenRead(path))
                using (var peReader = new PEReader(stream))
                {
                    if (!peReader.HasMetadata)
                        return Array.Empty<string>();
                    MetadataReader metadata = peReader.GetMetadataReader();
                    var result = new List<string>();
                    foreach (AssemblyReferenceHandle handle in metadata.AssemblyReferences)
                    {
                        AssemblyReference reference = metadata.GetAssemblyReference(handle);
                        result.Add(metadata.GetString(reference.Name));
                    }
                    return result.ToArray();
                }
            }
            catch (BadImageFormatException ex)
            {
                throw new FileLoadException("Cannot read assembly metadata from '" + path + "'.",
                    path, ex);
            }
        }

        private static bool FilesHaveSameContents(string firstPath, string secondPath)
        {
            if (new FileInfo(firstPath).Length != new FileInfo(secondPath).Length)
                return false;
            using (FileStream first = File.OpenRead(firstPath))
            using (FileStream second = File.OpenRead(secondPath))
            {
                byte[] firstHash = SHA256.HashData(first);
                byte[] secondHash = SHA256.HashData(second);
                return CryptographicOperations.FixedTimeEquals(firstHash, secondHash);
            }
        }

        private static void VerifyPreviousGenerationWasUnloaded()
        {
            if (pendingUnload == null)
                return;

            for (int attempt = 0; pendingUnload.IsAlive && attempt < 10; attempt++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            if (pendingUnload.IsAlive)
                throw new InvalidOperationException(
                    "The previous reference AssemblyLoadContext is still referenced by the compiler.");

            pendingUnload = null;
        }

        private readonly struct CachedAssemblyIdentity
        {
            internal readonly long Length;
            internal readonly DateTime LastWriteTimeUtc;
            internal readonly string SimpleName;
            internal readonly string FullName;
            internal readonly string[] ReferencedAssemblyNames;

            internal CachedAssemblyIdentity(long length, DateTime lastWriteTimeUtc,
                string simpleName, string fullName, string[] referencedAssemblyNames)
            {
                Length = length;
                LastWriteTimeUtc = lastWriteTimeUtc;
                SimpleName = simpleName;
                FullName = fullName;
                ReferencedAssemblyNames = referencedAssemblyNames;
            }
        }

        private sealed class ReferenceAssemblyLoadContext : AssemblyLoadContext
        {
            private readonly List<string> searchDirectories = new List<string>();
            private readonly Dictionary<string, FileFingerprint> loadedFileFingerprints =
                new Dictionary<string, FileFingerprint>(StringComparer.OrdinalIgnoreCase);

            internal ReferenceAssemblyLoadContext(string baseDirectory)
                : base("PABC.ReferenceAssemblies", isCollectible: true)
            {
                AddSearchDirectory(baseDirectory);
            }

            internal Assembly LoadRootAssembly(string assemblyPath)
            {
                string fullPath = Path.GetFullPath(assemblyPath);
                string assemblyFullName = AssemblyName.GetAssemblyName(fullPath).FullName;
                foreach (Assembly loadedAssembly in Assemblies)
                    if (string.Equals(loadedAssembly.FullName, assemblyFullName,
                        StringComparison.Ordinal))
                        return loadedAssembly;

                AddSearchDirectory(Path.GetDirectoryName(fullPath));
                RememberFile(fullPath);
                using (FileStream stream = File.OpenRead(fullPath))
                    return LoadFromStream(stream);
            }

            internal bool HasChanged(bool includeMissingFiles)
            {
                foreach (KeyValuePair<string, FileFingerprint> pair in loadedFileFingerprints)
                {
                    if (!File.Exists(pair.Key))
                    {
                        if (includeMissingFiles)
                            return true;
                        continue;
                    }
                    if (pair.Value.HasChanged(pair.Key))
                        return true;
                }
                return false;
            }

            internal void RememberSourceFile(string path)
            {
                RememberFile(path);
            }

            protected override Assembly Load(AssemblyName assemblyName)
            {
                foreach (string directory in searchDirectories)
                {
                    string dependencyPath = Path.Combine(directory, assemblyName.Name + ".dll");
                    if (!File.Exists(dependencyPath))
                        continue;

                    RememberFile(dependencyPath);
                    using (FileStream stream = File.OpenRead(dependencyPath))
                        return LoadFromStream(stream);
                }
                if (assemblyName.Name.StartsWith("ReflectionCacheUnload", StringComparison.Ordinal))
                    throw new FileNotFoundException("Reference context could not resolve " + assemblyName.Name +
                        ". Search directories: " + string.Join("; ", searchDirectories));
                return null;
            }

            internal void AddSearchDirectory(string directory)
            {
                string fullPath = Path.GetFullPath(directory);
                if (!searchDirectories.Exists(path =>
                    string.Equals(path, fullPath, StringComparison.OrdinalIgnoreCase)))
                    searchDirectories.Add(fullPath);
            }

            private void RememberFile(string path)
            {
                string fullPath = Path.GetFullPath(path);
                if (!loadedFileFingerprints.ContainsKey(fullPath))
                    loadedFileFingerprints[fullPath] = GetFileFingerprint(fullPath);
            }

            private static FileFingerprint GetFileFingerprint(string path)
            {
                var file = new FileInfo(path);
                using (FileStream stream = File.OpenRead(path))
                    return new FileFingerprint(file.Length, file.LastWriteTimeUtc,
                        SHA256.HashData(stream));
            }

            private sealed class FileFingerprint
            {
                private readonly long length;
                private DateTime lastWriteTimeUtc;
                private readonly byte[] contentHash;

                internal FileFingerprint(long length, DateTime lastWriteTimeUtc,
                    byte[] contentHash)
                {
                    this.length = length;
                    this.lastWriteTimeUtc = lastWriteTimeUtc;
                    this.contentHash = contentHash;
                }

                internal bool HasChanged(string path)
                {
                    var file = new FileInfo(path);
                    if (file.Length != length)
                        return true;
                    if (file.LastWriteTimeUtc == lastWriteTimeUtc)
                        return false;

                    // TEST-NET10-PACKAGE-ALC: reinstalling an identical package
                    // changes timestamps, but must not create another generation.
                    using (FileStream stream = File.OpenRead(path))
                        if (!CryptographicOperations.FixedTimeEquals(
                            contentHash, SHA256.HashData(stream)))
                            return true;
                    lastWriteTimeUtc = file.LastWriteTimeUtc;
                    return false;
                }
            }
        }
    }
}
#endif
