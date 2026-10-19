namespace PascalABCNet.PackageServices;

public sealed record PackageRequirement(string Id, string Version);

/// <summary>Restore inputs independent of an editor or compiler.</summary>
public sealed record PackageRestoreRequest
{
    public required IReadOnlyList<PackageRequirement> Packages { get; init; }
    /// <summary>Null uses configured enabled sources, or nuget.org when sources are not configured; an explicit list overrides them.</summary>
    public IReadOnlyList<string>? Sources { get; init; }
    /// <summary>Null uses NuGet's global cache, respecting NUGET_PACKAGES and NuGet.Config.</summary>
    public string? PackagesDirectory { get; init; }
    /// <summary>Null uses a hidden per-program directory in the PascalABC.NET user cache.</summary>
    public string? RestoreDirectory { get; init; }
    public string? ProgramFilePath { get; init; }
    /// <summary>Configuration lookup starts here, or beside ProgramFilePath when omitted.</summary>
    public string? ConfigurationDirectory { get; init; }
    /// <summary>Optional explicit config file, loaded without the normal configuration hierarchy.</summary>
    public string? NuGetConfigFile { get; init; }
    public string TargetFramework { get; init; } = "net10.0";
    /// <summary>Null selects win-x64 in a Windows x64 process, otherwise portable assets.
    /// An explicit RID is currently supported only for win-x64.</summary>
    public string? RuntimeIdentifier { get; init; }
    public bool ForceRestore { get; init; }
}

public sealed record PackageDiagnostic(string Severity, string Code, string Message);

public sealed record PackageSourceInfo(string Name, string Source);

public sealed record PackageRestoreConfiguration(
    string PackagesDirectory,
    string RestoreDirectory,
    IReadOnlyList<PackageSourceInfo> Sources,
    IReadOnlyList<string> ConfigurationFiles);

public sealed record ResolvedPackageDependency(string Id, string VersionRange);

public sealed record ResolvedPackage(
    string Id,
    string Version,
    string Directory,
    IReadOnlyList<ResolvedPackageDependency> Dependencies,
    IReadOnlyList<string> CompileAssemblies,
    IReadOnlyList<string> RuntimeAssemblies)
{
    /// <summary>Native files selected for this graph's RID; never semantic references.</summary>
    public IReadOnlyList<string> NativeLibraries { get; init; } = Array.Empty<string>();
}

public sealed record ResolvedPackageGraph(
    string TargetFramework,
    string AssetsFilePath,
    IReadOnlyList<ResolvedPackage> Packages,
    bool UsedCachedRestore,
    int InstalledPackageCount,
    IReadOnlyList<PackageDiagnostic> Diagnostics)
{
    public required PackageRestoreConfiguration Configuration { get; init; }
    public string? RuntimeIdentifier { get; init; }
    /// <summary>Packages actually installed into the global cache by this restore.</summary>
    public IReadOnlyList<PackageRequirement> InstalledPackages { get; init; } = Array.Empty<PackageRequirement>();
}

public sealed class PackageRestoreException : Exception
{
    public IReadOnlyList<PackageDiagnostic> Diagnostics { get; }

    public PackageRestoreException(IReadOnlyList<PackageDiagnostic> diagnostics)
        : base("NuGet restore failed: " + string.Join(Environment.NewLine,
            diagnostics.Where(d => d.Severity == "Error")
                .Select(d => d.Code + ": " + d.Message)))
    {
        Diagnostics = diagnostics;
    }
}
