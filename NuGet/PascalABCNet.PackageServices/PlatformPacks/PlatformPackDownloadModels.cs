namespace PascalABCNet.PackageServices;

/// <summary>Inputs for explicit download-only package work. Does not enable compilation or execution.</summary>
public sealed record PlatformPackDownloadRequest
{
    public required PlatformPackSelection Selection { get; init; }
    public required string ProgramFilePath { get; init; }
    /// <summary>Null uses enabled NuGet.Config sources, with the existing clean-profile fallback.</summary>
    public IReadOnlyList<string>? Sources { get; init; }
    public string? PackagesDirectory { get; init; }
    public string? ConfigurationDirectory { get; init; }
    public string? NuGetConfigFile { get; init; }
    /// <summary>Optional base for work files; download metadata is isolated in its platform-packs subdirectory.</summary>
    public string? RestoreDirectory { get; init; }
    /// <summary>Read only complete packages already in the global cache; never access feeds.</summary>
    public bool LocalPackagesOnly { get; init; }
}

/// <summary>
/// An extracted package in the NuGet cache, not a set of resolved compile/runtime assemblies.
/// UsedCachedPackage means the package was complete when this operation checked the cache.
/// </summary>
public sealed record CachedPlatformPackage(string Id, string Version, string Directory, bool UsedCachedPackage);
