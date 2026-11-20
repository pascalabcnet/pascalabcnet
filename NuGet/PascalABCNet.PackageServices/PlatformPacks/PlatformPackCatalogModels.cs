using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PascalABCNet.PackageServices;

/// <summary>
/// Data from PlatformPacks/net10.0.json. These models do not resolve or install packages.
/// Version strings are preserved verbatim, including significant Windows platform revisions.
/// </summary>
public sealed record PlatformPackCatalog
{
    [JsonPropertyName("schemaVersion")]
    public required int SchemaVersion { get; init; }

    [JsonPropertyName("targetFramework")]
    public required string TargetFramework { get; init; }

    [JsonPropertyName("source")]
    public required PlatformPackCatalogSource Source { get; init; }

    [JsonPropertyName("notes")]
    public required IReadOnlyList<string> Notes { get; init; }

    [JsonPropertyName("frameworkReferences")]
    public required IReadOnlyList<FrameworkReferenceDefinition> FrameworkReferences { get; init; }

    [JsonPropertyName("windowsSdk")]
    public required WindowsSdkPackCatalog WindowsSdk { get; init; }

    [JsonPropertyName("excludedCategories")]
    public required IReadOnlyList<string> ExcludedCategories { get; init; }
}

/// <summary>Official data source and the rule used to extract this catalog.</summary>
public sealed record PlatformPackCatalogSource
{
    [JsonPropertyName("product")]
    public required string Product { get; init; }

    [JsonPropertyName("version")]
    public required string Version { get; init; }

    [JsonPropertyName("files")]
    public required IReadOnlyList<PlatformPackCatalogSourceFile> Files { get; init; }

    [JsonPropertyName("selection")]
    public required string Selection { get; init; }

    [JsonPropertyName("selectionReference")]
    public required string SelectionReference { get; init; }

    [JsonPropertyName("copyright")]
    public required string Copyright { get; init; }
}

/// <summary>A path relative to the source SDK and the checksum of that source file.</summary>
public sealed record PlatformPackCatalogSourceFile
{
    [JsonPropertyName("path")]
    public required string Path { get; init; }

    [JsonPropertyName("sha256")]
    public required string Sha256 { get; init; }
}

/// <summary>
/// Descriptive framework metadata, not a request to download a runtime.
/// Targeting assemblies and executable runtime assemblies are distinct assets.
/// </summary>
public sealed record FrameworkReferenceDefinition
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("runtimeFrameworkName")]
    public required string RuntimeFrameworkName { get; init; }

    [JsonPropertyName("defaultRuntimeFrameworkVersion")]
    public required string DefaultRuntimeFrameworkVersion { get; init; }

    [JsonPropertyName("latestRuntimeFrameworkVersion")]
    public required string LatestRuntimeFrameworkVersion { get; init; }

    [JsonPropertyName("targetingPackName")]
    public required string TargetingPackName { get; init; }

    [JsonPropertyName("targetingPackVersion")]
    public required string TargetingPackVersion { get; init; }

    [JsonPropertyName("runtimePackNamePattern")]
    public required string RuntimePackNamePattern { get; init; }

    [JsonPropertyName("isWindowsOnly")]
    public required bool IsWindowsOnly { get; init; }

    [JsonPropertyName("profile")]
    public string? Profile { get; init; }
}

/// <summary>Windows pack identity and the supported platform version entries.</summary>
public sealed record WindowsSdkPackCatalog
{
    [JsonPropertyName("packageId")]
    public required string PackageId { get; init; }

    [JsonPropertyName("runtimePackageId")]
    public required string RuntimePackageId { get; init; }

    [JsonPropertyName("runtimeIdentifier")]
    public required string RuntimeIdentifier { get; init; }

    [JsonPropertyName("isWindowsOnly")]
    public required bool IsWindowsOnly { get; init; }

    [JsonPropertyName("platformVersionsWithoutPack")]
    public required IReadOnlyList<string> PlatformVersionsWithoutPack { get; init; }

    [JsonPropertyName("platformVersions")]
    public required IReadOnlyList<WindowsSdkPlatformDefinition> PlatformVersions { get; init; }
}

/// <summary>
/// An exact Windows platform version and its package version.
/// The .0 and .1 platform revisions must remain separate entries.
/// </summary>
public sealed record WindowsSdkPlatformDefinition
{
    [JsonPropertyName("platformVersion")]
    public required string PlatformVersion { get; init; }

    [JsonPropertyName("minimumNetVersion")]
    public required string MinimumNetVersion { get; init; }

    [JsonPropertyName("packageVersion")]
    public required string PackageVersion { get; init; }
}
