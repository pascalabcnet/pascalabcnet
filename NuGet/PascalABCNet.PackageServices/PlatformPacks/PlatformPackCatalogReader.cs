using System.Text.Json;
using System.Text.Json.Serialization;
using NuGet.Packaging;
using NuGet.Versioning;

namespace PascalABCNet.PackageServices;

/// <summary>Reads catalog data only. Does not inspect the host SDK, resolve assets or access feeds.</summary>
public static class PlatformPackCatalogReader
{
    private const string Net10Resource = "PascalABCNet.PackageServices.PlatformPacks.net10.0.json";
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    /// <summary>Reads the catalog shipped with this library. No machine-specific file lookup is performed.</summary>
    public static PlatformPackCatalog ReadNet10()
    {
        using Stream stream = typeof(PlatformPackCatalogReader).Assembly.GetManifestResourceStream(Net10Resource)
            ?? throw new InvalidDataException("Embedded platform pack catalog is missing: " + Net10Resource);
        return Read(stream);
    }

    /// <summary>
    /// Reads and validates a schema version 1, net10.0 catalog.
    /// Leaves the caller's stream open. Malformed catalog data throws InvalidDataException.
    /// </summary>
    public static PlatformPackCatalog Read(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        PlatformPackCatalog catalog;
        try
        {
            using var document = JsonDocument.Parse(stream);
            CheckDuplicateProperties(document.RootElement, "$");
            catalog = document.RootElement.Deserialize<PlatformPackCatalog>(Options)
                ?? throw Invalid("$", "catalog must be an object, not null");
        }
        catch (JsonException error)
        {
            throw new InvalidDataException("Invalid platform pack catalog JSON: " + error.Message, error);
        }
        Validate(catalog);
        return catalog;
    }

    private static void Validate(PlatformPackCatalog catalog)
    {
        Require(catalog.SchemaVersion == 1, "$.schemaVersion", "only schema version 1 is supported");
        Require(catalog.TargetFramework == "net10.0", "$.targetFramework", "expected net10.0");
        var source = catalog.Source;
        Require(source is not null, "$.source", "must not be null");
        Text(source!.Product, "$.source.product");
        PackageVersion(source.Version, "$.source.version");
        Text(source.Selection, "$.source.selection");
        Text(source.Copyright, "$.source.copyright");
        Text(source.SelectionReference, "$.source.selectionReference");
        Require(Uri.TryCreate(source.SelectionReference, UriKind.Absolute, out var uri) &&
            uri.Scheme == Uri.UriSchemeHttps, "$.source.selectionReference", "expected an absolute HTTPS URL");
        var files = List(source.Files, "$.source.files", allowEmpty: false);
        var filePaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < files.Count; i++)
        {
            string path = "$.source.files[" + i + "]";
            string file = Text(files[i].Path, path + ".path");
            Require(!file.StartsWith('/') && !file.Contains('\\') && !file.Contains(':') &&
                file.Split('/').All(part => part.Length != 0 && part != "." && part != ".."),
                path + ".path", "expected a relative SDK path with forward slashes and no traversal");
            Require(filePaths.Add(file), path + ".path", "duplicate source file");
            string hash = Text(files[i].Sha256, path + ".sha256");
            Require(hash.Length == 64 && hash.All(char.IsAsciiHexDigit), path + ".sha256",
                "expected 64 hexadecimal SHA-256 digits");
        }
        Strings(catalog.Notes, "$.notes");
        Strings(catalog.ExcludedCategories, "$.excludedCategories", unique: true);

        var frameworks = List(catalog.FrameworkReferences, "$.frameworkReferences", allowEmpty: false);
        var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < frameworks.Count; i++)
        {
            var entry = frameworks[i];
            string path = "$.frameworkReferences[" + i + "]";
            PackageId(entry.Id, path + ".id");
            Require(ids.Add(entry.Id), path + ".id", "duplicate framework reference");
            PackageId(entry.RuntimeFrameworkName, path + ".runtimeFrameworkName");
            var defaultVersion = PackageVersion(entry.DefaultRuntimeFrameworkVersion, path + ".defaultRuntimeFrameworkVersion");
            var latestVersion = PackageVersion(entry.LatestRuntimeFrameworkVersion, path + ".latestRuntimeFrameworkVersion");
            Require(latestVersion.CompareTo(defaultVersion) >= 0, path + ".latestRuntimeFrameworkVersion",
                "must not precede the default runtime version");
            PackageId(entry.TargetingPackName, path + ".targetingPackName");
            PackageVersion(entry.TargetingPackVersion, path + ".targetingPackVersion");
            string pattern = Text(entry.RuntimePackNamePattern, path + ".runtimePackNamePattern");
            const string marker = "**RID**";
            int markerIndex = pattern.IndexOf(marker, StringComparison.Ordinal);
            Require(markerIndex >= 0 && markerIndex == pattern.LastIndexOf(marker, StringComparison.Ordinal),
                path + ".runtimePackNamePattern", "expected exactly one **RID** placeholder");
            PackageId(pattern.Replace(marker, "win-x64", StringComparison.Ordinal), path + ".runtimePackNamePattern");
            if (entry.Profile is not null)
                Text(entry.Profile, path + ".profile");
        }

        var windows = catalog.WindowsSdk;
        Require(windows is not null, "$.windowsSdk", "must not be null");
        PackageId(windows!.PackageId, "$.windowsSdk.packageId");
        PackageId(windows.RuntimePackageId, "$.windowsSdk.runtimePackageId");
        string rid = Text(windows.RuntimeIdentifier, "$.windowsSdk.runtimeIdentifier");
        Require(rid.All(c => char.IsAsciiLetterOrDigit(c) || c == '.' || c == '-') &&
            char.IsAsciiLetterOrDigit(rid[0]), "$.windowsSdk.runtimeIdentifier", "invalid runtime identifier");
        Require(windows.IsWindowsOnly, "$.windowsSdk.isWindowsOnly", "Windows SDK entries must be Windows-only");
        var versions = new HashSet<Version>();
        var withoutPack = List(windows.PlatformVersionsWithoutPack, "$.windowsSdk.platformVersionsWithoutPack");
        for (int i = 0; i < withoutPack.Count; i++)
        {
            string path = "$.windowsSdk.platformVersionsWithoutPack[" + i + "]";
            Require(versions.Add(NumericVersion(withoutPack[i], path)), path, "duplicate Windows platform version");
        }
        var platforms = List(windows.PlatformVersions, "$.windowsSdk.platformVersions", allowEmpty: false);
        for (int i = 0; i < platforms.Count; i++)
        {
            var entry = platforms[i];
            string path = "$.windowsSdk.platformVersions[" + i + "]";
            Require(versions.Add(NumericVersion(entry.PlatformVersion, path + ".platformVersion")),
                path + ".platformVersion", "duplicate or overlapping Windows platform version");
            var minimum = NumericVersion(entry.MinimumNetVersion, path + ".minimumNetVersion");
            Require(minimum <= new Version(10, 0, 0, 0), path + ".minimumNetVersion",
                "minimum .NET version exceeds the catalog's net10.0 target");
            PackageVersion(entry.PackageVersion, path + ".packageVersion");
        }
    }

    private static void CheckDuplicateProperties(JsonElement element, string path)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            var names = new HashSet<string>(StringComparer.Ordinal);
            foreach (var property in element.EnumerateObject())
            {
                string childPath = path + "." + property.Name;
                Require(names.Add(property.Name), childPath, "duplicate JSON property");
                CheckDuplicateProperties(property.Value, childPath);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            int i = 0;
            foreach (var child in element.EnumerateArray())
                CheckDuplicateProperties(child, path + "[" + i++ + "]");
        }
    }

    private static IReadOnlyList<T> List<T>(IReadOnlyList<T>? values, string path, bool allowEmpty = true)
        where T : class
    {
        Require(values is not null, path, "must not be null");
        Require(allowEmpty || values!.Count != 0, path, "must not be empty");
        for (int i = 0; i < values!.Count; i++)
            Require(values[i] is not null, path + "[" + i + "]", "must not be null");
        return values;
    }

    private static void Strings(IReadOnlyList<string>? values, string path, bool unique = false)
    {
        var items = List(values, path);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < items.Count; i++)
        {
            string itemPath = path + "[" + i + "]";
            Text(items[i], itemPath);
            Require(!unique || seen.Add(items[i]), itemPath, "duplicate entry");
        }
    }

    private static string Text(string? value, string path)
    {
        Require(!string.IsNullOrWhiteSpace(value) && value == value.Trim(), path,
            "expected nonempty text with no surrounding whitespace");
        return value!;
    }

    private static void PackageId(string? value, string path)
    {
        Text(value, path);
        Require(PackageIdValidator.IsValidPackageId(value!), path, "invalid package or framework ID");
    }

    private static NuGetVersion PackageVersion(string? value, string path)
    {
        Text(value, path);
        Require(NuGetVersion.TryParse(value, out var version), path, "invalid NuGet version");
        return version!;
    }

    private static Version NumericVersion(string? value, string path)
    {
        Text(value, path);
        Require(Version.TryParse(value, out var version), path, "invalid numeric version");
        // Normalize omitted components for duplicate checks, but preserve a nonzero revision.
        return new Version(version!.Major, version.Minor, Math.Max(0, version.Build), Math.Max(0, version.Revision));
    }

    private static void Require(bool condition, string path, string message)
    {
        if (!condition)
            throw Invalid(path, message);
    }

    private static InvalidDataException Invalid(string path, string message) =>
        new("Invalid platform pack catalog at " + path + ": " + message + ".");
}
