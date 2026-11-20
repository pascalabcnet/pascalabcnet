namespace PascalABCNet.PackageServices;

/// <summary>A plan only: framework references do not imply downloading another runtime.</summary>
public sealed record PlatformPackSelection(
    string TargetFramework,
    IReadOnlyList<FrameworkReferenceDefinition> FrameworkReferences,
    IReadOnlyList<SelectedPlatformPackage> Packages);

/// <summary>
/// A platform package identity selected from the catalog, not an installed package.
/// Profile identifies the Windows projection within the package; it is not a NuGet package ID.
/// No asset paths, feeds or host-specific cache locations are resolved at this stage.
/// </summary>
public sealed record SelectedPlatformPackage(
    string Id,
    string Version,
    string RuntimePackageId,
    string RuntimeIdentifier,
    string PlatformVersion,
    string Profile);
