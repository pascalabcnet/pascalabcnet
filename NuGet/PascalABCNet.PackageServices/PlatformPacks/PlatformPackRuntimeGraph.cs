using NuGet.Versioning;

namespace PascalABCNet.PackageServices;

/// <summary>Adapts selected platform implementations to the existing cache-based host manifest.</summary>
public static class PlatformPackRuntimeGraph
{
    public static ResolvedPackageGraph FromPacks(PlatformPackSelection selection,
        IReadOnlyList<CachedPlatformPackage> cached, PackageRestoreConfiguration configuration)
    {
        var packages = new List<ResolvedPackage>();
        foreach (var selected in selection.Packages)
        {
            var assets = PlatformPackAssemblyResolver.Resolve(selection with { Packages = new[] { selected } }, cached);
            var package = cached.Single(p => p.Id.Equals(selected.RuntimePackageId, StringComparison.OrdinalIgnoreCase)
                && NuGetVersion.Parse(p.Version) == NuGetVersion.Parse(selected.Version));
            var paths = assets.Select(a => a.Path).ToArray();
            packages.Add(new ResolvedPackage(package.Id, package.Version, package.Directory,
                Array.Empty<ResolvedPackageDependency>(), paths, paths));
        }
        return new ResolvedPackageGraph(selection.TargetFramework, "", packages, true, 0,
            Array.Empty<PackageDiagnostic>()) { Configuration = configuration };
    }

    public static ResolvedPackageGraph Merge(ResolvedPackageGraph? ordinary, ResolvedPackageGraph platform)
    {
        if (ordinary is null) return platform;
        var comparer = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        if (!ordinary.TargetFramework.Equals(platform.TargetFramework, StringComparison.OrdinalIgnoreCase) ||
            !comparer.Equals(Path.GetFullPath(ordinary.Configuration.PackagesDirectory),
                Path.GetFullPath(platform.Configuration.PackagesDirectory)))
            throw new InvalidDataException("Platform and NuGet runtime graphs must use the same target and package cache.");
        var packages = ordinary.Packages.ToDictionary(p => p.Id, StringComparer.OrdinalIgnoreCase);
        foreach (var package in platform.Packages)
        {
            if (packages.TryGetValue(package.Id, out var existing))
            {
                if (NuGetVersion.Parse(existing.Version) != NuGetVersion.Parse(package.Version) ||
                    !comparer.Equals(Path.GetFullPath(existing.Directory), Path.GetFullPath(package.Directory)))
                    throw new InvalidDataException("Conflicting platform runtime package: " + package.Id);
                packages[package.Id] = existing with {
                    RuntimeAssemblies = existing.RuntimeAssemblies.Concat(package.RuntimeAssemblies).Distinct(comparer).ToArray()
                };
            }
            else packages.Add(package.Id, package);
        }
        // Preserve the ordinary graph's RID, native assets and dependency edges.
        return ordinary with { Packages = packages.Values.ToArray() };
    }
}
