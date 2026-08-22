// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

#if PABCNET_MODERN
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace PascalABCCompiler.NetHelper
{
    /// <summary>
    /// Calculates the small, implicit .NET system reference set used by both
    /// the compiler and semantic IntelliSense. The closure is read directly
    /// from CLR AssemblyRef metadata and cached for the lifetime of the process.
    /// </summary>
    public static class NetCoreSystemReferences
    {
        private static readonly string[] rootAssemblyNames =
        {
            "System.Private.CoreLib.dll",
            "System.Runtime.dll",
            "System.Runtime.Serialization.Formatters.dll",
            "System.Console.dll",
            "System.Diagnostics.Process.dll",
            "System.Collections.dll",
            "System.Collections.NonGeneric.dll",
            "System.ComponentModel.TypeConverter.dll",
            "System.Linq.dll",
            "System.IO.dll",
            "System.IO.FileSystem.dll",
            "System.IO.FileSystem.DriveInfo.dll",
            "System.Text.RegularExpressions.dll",
            "System.Threading.dll",
            "System.Threading.Tasks.dll",
            "System.Threading.Tasks.Parallel.dll",
            "System.Threading.Thread.dll",
            "System.Numerics.dll",
            "System.Numerics.Vectors.dll"
        };

        private static readonly ConcurrentDictionary<string, Lazy<IReadOnlyList<string>>> cache =
            new ConcurrentDictionary<string, Lazy<IReadOnlyList<string>>>(StringComparer.OrdinalIgnoreCase);
        private static readonly ConcurrentDictionary<string, Lazy<IReadOnlyList<string>>> facadeCache =
            new ConcurrentDictionary<string, Lazy<IReadOnlyList<string>>>(StringComparer.OrdinalIgnoreCase);

        public static IReadOnlyList<string> RootAssemblyNames
        {
            get { return new ReadOnlyCollection<string>(rootAssemblyNames); }
        }

        public static string RuntimeDirectory
        {
            get { return Path.GetDirectoryName(typeof(object).Assembly.Location); }
        }

        public static IReadOnlyList<string> AssemblyPaths
        {
            get { return GetAssemblyPaths(rootAssemblyNames); }
        }

        public static bool IsRuntimeAssembly(string assemblyFileName)
        {
            return !string.IsNullOrEmpty(assemblyFileName)
                && ResolveRuntimeAssemblyPath(RuntimeDirectory, Path.GetFileName(assemblyFileName)) != null;
        }

        public static IReadOnlyList<string> GetAssemblyPaths(IEnumerable<string> assemblyFileNames)
        {
            string runtimeDirectory = RuntimeDirectory;
            string[] roots = assemblyFileNames
                .Where(name => !string.IsNullOrEmpty(name))
                .Select(Path.GetFileName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            string rootsKey = string.Join("|", roots.OrderBy(name => name, StringComparer.OrdinalIgnoreCase));
            string cacheKey = runtimeDirectory + "|" + Environment.Version + "|" + rootsKey;

            return cache.GetOrAdd(
                cacheKey,
                _ => new Lazy<IReadOnlyList<string>>(
                    () => BuildDependencyClosure(runtimeDirectory, roots), true)).Value;
        }

        public static IReadOnlyList<string> GetFacadeAssemblyPaths(string assemblyFileName)
        {
            assemblyFileName = Path.GetFileName(assemblyFileName);
            string runtimeDirectory = RuntimeDirectory;
            string cacheKey = runtimeDirectory + "|" + Environment.Version + "|facade|" + assemblyFileName;

            return facadeCache.GetOrAdd(
                cacheKey,
                _ => new Lazy<IReadOnlyList<string>>(
                    () => BuildFacadeClosure(runtimeDirectory, assemblyFileName), true)).Value;
        }

        private static IReadOnlyList<string> BuildDependencyClosure(string runtimeDirectory, IEnumerable<string> roots)
        {
            if (string.IsNullOrEmpty(runtimeDirectory) || !Directory.Exists(runtimeDirectory))
                throw new DirectoryNotFoundException(
                    "The .NET runtime directory used by PascalABC.NET was not found: " + runtimeDirectory);

            var result = new List<string>();
            var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var pending = new Queue<string>(roots);

            while (pending.Count > 0)
            {
                string assemblyFileName = pending.Dequeue();
                if (!processed.Add(assemblyFileName))
                    continue;

                string assemblyPath = ResolveRuntimeAssemblyPath(runtimeDirectory, assemblyFileName);
                if (assemblyPath == null)
                    throw new FileNotFoundException(
                        "The .NET runtime assembly '" + assemblyFileName
                        + "' was not found in '" + runtimeDirectory + "'.",
                        Path.Combine(runtimeDirectory, assemblyFileName));

                result.Add(assemblyPath);

                using (var stream = File.OpenRead(assemblyPath))
                using (var peReader = new PEReader(stream))
                {
                    if (!peReader.HasMetadata)
                        throw new BadImageFormatException(
                            "The .NET runtime assembly has no CLR metadata: " + assemblyPath);

                    MetadataReader reader = peReader.GetMetadataReader();
                    foreach (AssemblyReferenceHandle handle in reader.AssemblyReferences)
                    {
                        AssemblyReference reference = reader.GetAssemblyReference(handle);
                        string dependencyFileName = reader.GetString(reference.Name) + ".dll";
                        if (!processed.Contains(dependencyFileName))
                            pending.Enqueue(dependencyFileName);
                    }
                }
            }

            return new ReadOnlyCollection<string>(result);
        }

        private static IReadOnlyList<string> BuildFacadeClosure(
            string runtimeDirectory,
            string rootAssemblyFileName)
        {
            var result = new List<string>();
            var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var pending = new Queue<string>();
            pending.Enqueue(rootAssemblyFileName);

            while (pending.Count > 0)
            {
                string assemblyFileName = pending.Dequeue();
                if (!processed.Add(assemblyFileName))
                    continue;

                string assemblyPath = ResolveRuntimeAssemblyPath(runtimeDirectory, assemblyFileName);
                if (assemblyPath == null)
                {
                    if (assemblyFileName.Equals(rootAssemblyFileName, StringComparison.OrdinalIgnoreCase))
                        throw new FileNotFoundException(
                            "The referenced .NET runtime assembly '" + assemblyFileName
                            + "' was not found in '" + runtimeDirectory + "'.",
                            Path.Combine(runtimeDirectory, assemblyFileName));

                    continue;
                }

                result.Add(assemblyPath);

                using (var stream = File.OpenRead(assemblyPath))
                using (var peReader = new PEReader(stream))
                {
                    MetadataReader reader = peReader.GetMetadataReader();
                    if (reader.TypeDefinitions.Count > 1)
                        continue;

                    foreach (ExportedTypeHandle handle in reader.ExportedTypes)
                    {
                        EntityHandle implementation = reader.GetExportedType(handle).Implementation;
                        while (implementation.Kind == HandleKind.ExportedType)
                            implementation = reader.GetExportedType((ExportedTypeHandle)implementation).Implementation;

                        if (implementation.Kind == HandleKind.AssemblyReference)
                        {
                            AssemblyReference reference = reader.GetAssemblyReference(
                                (AssemblyReferenceHandle)implementation);
                            string forwardedAssemblyFileName = reader.GetString(reference.Name) + ".dll";
                            if (!processed.Contains(forwardedAssemblyFileName))
                                pending.Enqueue(forwardedAssemblyFileName);
                        }
                    }
                }
            }

            return new ReadOnlyCollection<string>(result);
        }

        private static string ResolveRuntimeAssemblyPath(string runtimeDirectory, string assemblyFileName)
        {
            string runtimePath = Path.Combine(runtimeDirectory, assemblyFileName);
            if (File.Exists(runtimePath))
                return runtimePath;

            var runtimeVersionDirectory = new DirectoryInfo(runtimeDirectory);
            var runtimeRootDirectory = runtimeVersionDirectory.Parent;
            var sharedDirectory = runtimeRootDirectory?.Parent;
            if (sharedDirectory != null && runtimeRootDirectory.Name == "Microsoft.NETCore.App")
            {
                string windowsDesktopPath = Path.Combine(sharedDirectory.FullName,
                    "Microsoft.WindowsDesktop.App", runtimeVersionDirectory.Name, assemblyFileName);
                if (File.Exists(windowsDesktopPath))
                    return windowsDesktopPath;
            }

            return null;
        }
    }
}
#endif
