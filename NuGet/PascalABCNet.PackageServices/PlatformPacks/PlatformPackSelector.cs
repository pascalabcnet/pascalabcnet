using NuGet.Frameworks;

namespace PascalABCNet.PackageServices;

/// <summary>Pure target/catalog selection. Does not inspect the host, access files or contact NuGet.</summary>
public static class PlatformPackSelector
{
    /// <summary>
    /// Selects for a TFM (not the Pascal directive text "windows").
    /// Catalog must have passed PlatformPackCatalogReader validation.
    /// Optional platforms correspond to Pascal's core/wpf/winforms directives.
    /// They neither change the target nor enable Windows API packages by themselves.
    /// Selection is independent of the current OS; target execution checks belong to the caller.
    /// </summary>
    public static PlatformPackSelection Select(PlatformPackCatalog catalog, string targetFramework,
        IReadOnlyList<string>? platforms = null)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetFramework);
        if (catalog.SchemaVersion != 1 || catalog.TargetFramework != "net10.0")
            throw new InvalidDataException("Expected a validated schema version 1, net10.0 platform pack catalog.");
        if (targetFramework != targetFramework.Trim())
            throw new ArgumentException("Target framework must not contain surrounding whitespace.", nameof(targetFramework));

        var framework = NuGetFramework.ParseFolder(targetFramework);
        var baseFramework = NuGetFramework.ParseFolder(catalog.TargetFramework);
        if (!string.Equals(framework.Framework, baseFramework.Framework, StringComparison.OrdinalIgnoreCase) ||
            framework.Version != baseFramework.Version)
            throw new NotSupportedException("Target framework is not supported by this catalog: " + targetFramework);
        bool windows = string.Equals(framework.Platform, "windows", StringComparison.OrdinalIgnoreCase);
        if (!windows && !string.IsNullOrEmpty(framework.Platform))
            throw new NotSupportedException("Target platform is not supported by this catalog: " + framework.Platform);

        var references = new List<FrameworkReferenceDefinition>();
        var referenceIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        void AddReference(string id)
        {
            if (!referenceIds.Add(id))
                return;
            var matches = catalog.FrameworkReferences.Where(entry =>
                string.Equals(entry.Id, id, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (matches.Length != 1)
                throw new InvalidDataException("Catalog must contain exactly one framework reference: " + id);
            references.Add(matches[0]);
        }

        AddReference("Microsoft.NETCore.App");
        bool wpf = false, winforms = false;
        foreach (string platform in platforms ?? Array.Empty<string>())
        {
            if (string.Equals(platform, "core", StringComparison.OrdinalIgnoreCase))
                continue;
            if (string.Equals(platform, "wpf", StringComparison.OrdinalIgnoreCase))
                wpf = true;
            else if (string.Equals(platform, "winforms", StringComparison.OrdinalIgnoreCase))
                winforms = true;
            else
                throw new ArgumentException("Unknown platform directive: " + (platform ?? "<null>"), nameof(platforms));
        }
        // Stable ordering regardless of the order or repetition of platform directives.
        if (wpf) AddReference("Microsoft.WindowsDesktop.App.WPF");
        if (winforms) AddReference("Microsoft.WindowsDesktop.App.WindowsForms");

        var packages = new List<SelectedPlatformPackage>();
        string normalizedTarget = catalog.TargetFramework;
        if (windows)
        {
            Version requested = Normalize(framework.PlatformVersion);
            // A Windows TFM with no platform version defaults to Windows API 7.0.
            if (requested == new Version(0, 0, 0, 0))
            {
                if (!targetFramework.EndsWith("-windows", StringComparison.OrdinalIgnoreCase))
                    throw new NotSupportedException("Explicit Windows platform version 0.0 is not supported.");
                requested = new Version(7, 0, 0, 0);
            }
            var noPack = catalog.WindowsSdk.PlatformVersionsWithoutPack.Where(version =>
                Normalize(Version.Parse(version)) == requested).ToArray();
            var matches = catalog.WindowsSdk.PlatformVersions.Where(entry =>
                Normalize(Version.Parse(entry.PlatformVersion)) == requested).ToArray();
            if (noPack.Length + matches.Length == 0)
                throw new NotSupportedException("Windows platform version is absent from the catalog: " + requested);
            if (noPack.Length + matches.Length != 1)
                throw new InvalidDataException("Ambiguous Windows platform version in catalog: " + requested);
            if (noPack.Length == 1)
                normalizedTarget += "-windows" + noPack[0];
            else
            {
                var entry = matches[0];
                if (Normalize(Version.Parse(entry.MinimumNetVersion)) > Normalize(framework.Version))
                    throw new NotSupportedException("Windows platform version requires .NET " + entry.MinimumNetVersion);
                // Profiles follow Microsoft's Windows framework-reference rules. Retain the exact
                // platform revision: .1 is CsWinRT 3, not a synonym for .0 (CsWinRT 2).
                string profile = requested.Revision == 1 ? "CsWinRT3.Windows" : "Windows";
                packages.Add(new SelectedPlatformPackage(
                    catalog.WindowsSdk.PackageId, entry.PackageVersion,
                    catalog.WindowsSdk.RuntimePackageId, catalog.WindowsSdk.RuntimeIdentifier,
                    entry.PlatformVersion, profile));
                normalizedTarget += "-windows" + entry.PlatformVersion;
            }
        }
        return new PlatformPackSelection(normalizedTarget,
            Array.AsReadOnly(references.ToArray()), Array.AsReadOnly(packages.ToArray()));
    }

    private static Version Normalize(Version version) =>
        new(version.Major, version.Minor, Math.Max(0, version.Build), Math.Max(0, version.Revision));
}
