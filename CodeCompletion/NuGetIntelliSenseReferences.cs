#if PABCNET_MODERN
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using PascalABCNet.PackageServices;
using NuGetPackageRequirement = PascalABCNet.PackageServices.PackageRequirement;

namespace CodeCompletion
{
    // Only the consumer-specific part lives here. Restore and transitive asset
    // selection are shared with the compiler through PackageServices.
    internal sealed class NuGetIntelliSenseReferences
    {
        private readonly PackageRestoreService restoreService = new PackageRestoreService();
        private readonly Dictionary<string, CacheEntry> cache =
            new Dictionary<string, CacheEntry>(StringComparer.OrdinalIgnoreCase);

        internal IReadOnlyList<string> Resolve(compilation_unit unit, Action beforeReferenceReload)
        {
            if (!(unit is program_module) || unit.compiler_directives == null)
                return Array.Empty<string>();

            var requirements = new Dictionary<string, NuGetPackageRequirement>(
                StringComparer.OrdinalIgnoreCase);
            foreach (compiler_directive directive in unit.compiler_directives)
            {
                if (directive == null || !string.Equals(directive.Name?.text, "package",
                    StringComparison.OrdinalIgnoreCase))
                    continue;

                string[] parts = (directive.Directive?.text ?? string.Empty)
                    .Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                // Incomplete directives are common while the user is typing.
                if (parts.Length != 2)
                    continue;
                NuGetPackageRequirement previous;
                if (requirements.TryGetValue(parts[0], out previous) &&
                    !string.Equals(previous.Version, parts[1], StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("Conflicting NuGet package versions for '" +
                        parts[0] + "': " + previous.Version + " and " + parts[1] + ".");
                requirements[parts[0]] = new NuGetPackageRequirement(parts[0], parts[1]);
            }

            string fileName = Path.GetFullPath(unit.file_name);
            if (requirements.Count == 0)
            {
                cache.Remove(fileName);
                return Array.Empty<string>();
            }

            var packages = requirements.Values.OrderBy(p => p.Id,
                StringComparer.OrdinalIgnoreCase).ToArray();
            string signature = string.Join("\n", packages.Select(p =>
                p.Id.ToLowerInvariant() + " " + p.Version.ToLowerInvariant()));
            CacheEntry previousEntry;
            if (cache.TryGetValue(fileName, out previousEntry) &&
                previousEntry.Signature == signature &&
                previousEntry.AssemblyPaths.All(File.Exists))
            {
                PascalABCCompiler.NetHelper.NetHelper.RegisterIntelliSenseReferencePaths(
                    previousEntry.AssemblyPaths, beforeReferenceReload);
                return previousEntry.AssemblyPaths;
            }

            var graph = restoreService.RestoreAsync(new PackageRestoreRequest
            {
                Packages = packages,
                ProgramFilePath = fileName
            }).GetAwaiter().GetResult();
            var assemblyPaths = new List<string>();
            var pathsByName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (ResolvedPackage package in graph.Packages)
            {
                if (package.CompileAssemblies.Count != 0 && package.RuntimeAssemblies.Count == 0)
                    throw new InvalidOperationException("NuGet package '" + package.Id +
                        "' has no runtime assembly for IntelliSense analysis.");
                foreach (string path in package.RuntimeAssemblies)
                {
                    string name = Path.GetFileName(path);
                    string existing;
                    if (pathsByName.TryGetValue(name, out existing))
                    {
                        if (!string.Equals(existing, path, StringComparison.OrdinalIgnoreCase))
                            throw new InvalidOperationException("NuGet packages contain two different '" +
                                name + "' assemblies.");
                        continue;
                    }
                    pathsByName.Add(name, path);
                    assemblyPaths.Add(path);
                }
            }

            PascalABCCompiler.NetHelper.NetHelper.RegisterIntelliSenseReferencePaths(
                assemblyPaths, beforeReferenceReload);
            string[] result = assemblyPaths.ToArray();
            cache[fileName] = new CacheEntry(signature, result);
            return result;
        }

        private sealed class CacheEntry
        {
            internal readonly string Signature;
            internal readonly string[] AssemblyPaths;

            internal CacheEntry(string signature, string[] assemblyPaths)
            {
                Signature = signature;
                AssemblyPaths = assemblyPaths;
            }
        }
    }
}
#endif
