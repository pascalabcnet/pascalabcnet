#if PABCNET_MODERN
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private static HashSet<string> currentValidationPaths =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static ReferenceClosure validatedClosure;
        internal static int ReferenceClosureBuildCount { get; private set; }
        private static readonly Dictionary<string, CachedAssemblyIdentity> identityCache =
            new Dictionary<string, CachedAssemblyIdentity>(StringComparer.OrdinalIgnoreCase);
        // The installed platform is fixed for this process, just like its
        // already loaded platform assemblies. These caches contain no
        // Assembly or Type objects from a collectible context.
        private static readonly Dictionary<string, PlatformReferenceNode> platformReferenceGraph =
            new Dictionary<string, PlatformReferenceNode>(StringComparer.Ordinal);
        private static readonly HashSet<string> knownPlatformFiles = new HashSet<string>(StringComparer.Ordinal);
        internal static int PlatformReferenceGraphNodeCount => platformReferenceGraph.Count;
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
            validatedClosure = null;

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
            validatedClosure = null;
            currentCompilationPaths = new HashSet<string>(previousCompilationPaths,
                StringComparer.OrdinalIgnoreCase);
        }

        internal static void BeginCompilation()
        {
            previousCompilationPaths = currentCompilationPaths;
            currentCompilationPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            currentValidationPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            validatedClosure = null;
            ReferenceClosureBuildCount = 0;
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

        internal static string GetSimpleName(string assemblyPath)
        {
            return GetCachedIdentity(Path.GetFullPath(assemblyPath)).SimpleName;
        }

        internal static void ValidateNoConflictingAssemblies(IEnumerable<string> assemblyPaths, bool accumulate = true)
        {
            var rootPaths = new List<string>();
            var rootPathSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var rootDirectories = new List<string>();
            var rootDirectorySet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            // A later Pascal unit may add either packages or explicit references.
            // Validate against the whole current compilation, not only its own roots.
            foreach (string path in (accumulate ? currentValidationPaths : Enumerable.Empty<string>()).Concat(assemblyPaths))
            {
                string fullPath = Path.GetFullPath(path);
                if (!ReferenceFileExists(fullPath) || !rootPathSet.Add(fullPath))
                    continue;
                rootPaths.Add(fullPath);
                string directory = Path.GetDirectoryName(fullPath);
                if (rootDirectorySet.Add(directory))
                    rootDirectories.Add(directory);
            }

            ReferenceClosure closure = accumulate ? validatedClosure : null;
            bool reuseClosure = closure != null && closure.Matches(rootPaths);
            if (!reuseClosure)
            {
                closure = new ReferenceClosure(rootPaths, accumulate);
                var visitedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (string rootPath in rootPaths)
                    DiscoverReferenceClosure(rootPath, rootDirectories, closure, visitedPaths);
                if (accumulate)
                    ReferenceClosureBuildCount++;
            }

            var bySimpleName = new Dictionary<string, KeyValuePair<string, CachedAssemblyIdentity>>(
                StringComparer.OrdinalIgnoreCase);
            foreach (var file in closure.Files)
            {
                string fullPath = file.Key;
                // Matches has just checked the stamp of each cached file.
                CachedAssemblyIdentity identity = reuseClosure ? file.Value : GetCachedIdentity(fullPath);
                if (!bySimpleName.TryGetValue(identity.SimpleName, out var previous))
                {
                    bySimpleName.Add(identity.SimpleName,
                        new KeyValuePair<string, CachedAssemblyIdentity>(fullPath, identity));
                    continue;
                }

                string previousPath = previous.Key;
                CachedAssemblyIdentity previousIdentity = reuseClosure ? previous.Value : GetCachedIdentity(previousPath);
                if (!string.Equals(previousIdentity.FullName, identity.FullName,
                    StringComparison.Ordinal) || !FilesHaveSameContents(previousPath, fullPath))
                    throw new FileLoadException(
                        string.Format(StringResources.Get("COMPILATIONERROR_REFERENCE_ASSEMBLY_CONFLICT{0}{1}"),
                            previousPath, fullPath));
            }
            if (accumulate)
            {
                foreach (string rootPath in rootPaths)
                    currentValidationPaths.Add(rootPath);
                validatedClosure = closure;
            }
        }

        private sealed class ReferenceClosure
        {
            private readonly List<string> roots;
            internal readonly List<KeyValuePair<string, CachedAssemblyIdentity>> Files =
                new List<KeyValuePair<string, CachedAssemblyIdentity>>();
            private readonly List<KeyValuePair<string, CachedAssemblyIdentity>> mutableFiles =
                new List<KeyValuePair<string, CachedAssemblyIdentity>>();
            private readonly Dictionary<string, HashSet<string>> missingNames;

            internal ReferenceClosure(List<string> roots, bool cacheable)
            {
                this.roots = roots;
                if (cacheable)
                    missingNames = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
            }

            internal void RememberMissing(string path)
            {
                if (missingNames == null)
                    return;
                string directory = Path.GetDirectoryName(path);
                if (NetCoreSystemReferences.IsPlatformAssemblyDirectory(directory))
                    return;
                if (!missingNames.TryGetValue(directory, out var names))
                    missingNames[directory] = names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                names.Add(Path.GetFileName(path));
            }

            internal void AddFile(string path, CachedAssemblyIdentity identity, bool platform)
            {
                var file = new KeyValuePair<string, CachedAssemblyIdentity>(path, identity);
                Files.Add(file);
                if (!platform && missingNames != null)
                    mutableFiles.Add(file);
            }

            internal bool Matches(List<string> rootPaths)
            {
                if (!roots.SequenceEqual(rootPaths, StringComparer.OrdinalIgnoreCase))
                    return false;

                try
                {
                    foreach (var pair in mutableFiles)
                    {
                        var file = new FileInfo(pair.Key);
                        if (!file.Exists || file.Length != pair.Value.Length ||
                            file.LastWriteTimeUtc != pair.Value.LastWriteTimeUtc)
                            return false;
                    }

                    // A missing dependency can appear later (for example after a
                    // package restore) and take precedence over an existing one.
                    // Enumerate each affected directory once instead of probing
                    // every missing candidate on every validation.
                    foreach (var pair in missingNames)
                        foreach (string path in Directory.EnumerateFiles(pair.Key, "*.dll"))
                            if (pair.Value.Contains(Path.GetFileName(path)))
                                return false;
                }
                catch (IOException) { return false; }
                catch (UnauthorizedAccessException) { return false; }

                return true;
            }
        }

        private static void DiscoverReferenceClosure(string assemblyPath,
            List<string> searchDirectories, ReferenceClosure closure, HashSet<string> visitedPaths)
        {
            string fullPath = Path.GetFullPath(assemblyPath);
            if (!visitedPaths.Add(fullPath) || !ReferenceFileExists(fullPath))
                return;

            string localDirectory = Path.GetDirectoryName(fullPath);
            bool isPlatform = knownPlatformFiles.Contains(fullPath);
            PlatformReferenceNode platform = isPlatform ? GetPlatformReferenceNode(fullPath, localDirectory) : null;
            CachedAssemblyIdentity identity = platform?.Identity ?? GetCachedIdentity(fullPath);
            closure.AddFile(fullPath, identity, isPlatform);
            for (int i = 0; i < identity.ReferencedAssemblyNames.Length; i++)
            {
                string dependencyFileName = platform == null
                    ? identity.ReferencedAssemblyNames[i] + ".dll"
                    : platform.Dependencies[i].Key;
                string dependencyPath = platform == null
                    ? Path.Combine(localDirectory, dependencyFileName)
                    : platform.Dependencies[i].Value;
                if (platform == null && !ReferenceFileExists(dependencyPath))
                {
                    closure.RememberMissing(dependencyPath);
                    dependencyPath = null;
                }
                // Search outside the platform assembly's own directory using
                // this compilation's directories and their original order.
                if (dependencyPath == null)
                {
                    foreach (string directory in searchDirectories)
                    {
                        if (platform != null && string.Equals(directory, localDirectory, StringComparison.Ordinal))
                            continue; // The platform node already recorded this local miss.
                        string candidate = Path.Combine(directory, dependencyFileName);
                        if (ReferenceFileExists(candidate))
                        {
                            dependencyPath = candidate;
                            break;
                        }
                        closure.RememberMissing(candidate);
                    }
                }
                if (dependencyPath != null)
                    DiscoverReferenceClosure(dependencyPath, searchDirectories, closure, visitedPaths);
            }
        }

        private static bool ReferenceFileExists(string path)
        {
            if (knownPlatformFiles.Contains(path))
                return true;
            if (!File.Exists(path))
                return false;
            if (NetCoreSystemReferences.IsPlatformAssemblyDirectory(Path.GetDirectoryName(path)))
                knownPlatformFiles.Add(path);
            return true;
        }

        private static PlatformReferenceNode GetPlatformReferenceNode(string path, string directory)
        {
            if (platformReferenceGraph.TryGetValue(path, out var node))
                return node;

            CachedAssemblyIdentity identity = GetCachedIdentity(path);
            var dependencies = new KeyValuePair<string, string>[identity.ReferencedAssemblyNames.Length];
            for (int i = 0; i < dependencies.Length; i++)
            {
                string name = identity.ReferencedAssemblyNames[i] + ".dll";
                string candidate = Path.Combine(directory, name);
                dependencies[i] = new KeyValuePair<string, string>(name,
                    ReferenceFileExists(candidate) ? candidate : null);
            }
            node = new PlatformReferenceNode(identity, dependencies);
            platformReferenceGraph.Add(path, node);
            return node;
        }

        private sealed class PlatformReferenceNode
        {
            internal readonly CachedAssemblyIdentity Identity;
            internal readonly KeyValuePair<string, string>[] Dependencies;

            internal PlatformReferenceNode(CachedAssemblyIdentity identity,
                KeyValuePair<string, string>[] dependencies)
            {
                Identity = identity;
                Dependencies = dependencies;
            }
        }

        private static CachedAssemblyIdentity GetCachedIdentity(string path)
        {
            if (platformReferenceGraph.TryGetValue(path, out var platform))
                return platform.Identity;
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
                throw new FileLoadException(string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_METADATA_READ_FAILED{0}"), path),
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
                throw new FileLoadException(string.Format(StringResources.Get("COMPILATIONERROR_ASSEMBLY_METADATA_READ_FAILED{0}"), path),
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
                    StringResources.Get("COMPILATIONERROR_REFERENCE_CONTEXT_NOT_UNLOADED"));

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

                    // Keep platform types identical to those used by direct references.
                    if (NetCoreSystemReferences.IsPlatformAssemblyDirectory(directory))
                        return NetCoreSystemReferences.LoadRuntimeAssembly(dependencyPath);

                    RememberFile(dependencyPath);
                    using (FileStream stream = File.OpenRead(dependencyPath))
                        return LoadFromStream(stream);
                }
                if (assemblyName.Name.StartsWith("ReflectionCacheUnload", StringComparison.Ordinal))
                    throw new FileNotFoundException(string.Format(StringResources.Get(
                        "COMPILATIONERROR_REFERENCE_CONTEXT_CANNOT_RESOLVE{0}{1}"),
                        assemblyName.Name, string.Join("; ", searchDirectories)));
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
