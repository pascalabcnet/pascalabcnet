using System.Security.Cryptography;
using System.Text;
using NuGet.Configuration;

namespace PascalABCNet.PackageServices;

internal sealed record RestoreEnvironment(
    ISettings Settings, IReadOnlyList<PackageSource> Sources, PackageRestoreConfiguration Configuration)
{
    internal static RestoreEnvironment Resolve(PackageRestoreRequest request)
    {
        string configurationDirectory = Path.GetFullPath(request.ConfigurationDirectory ??
            (request.ProgramFilePath is null ? Environment.CurrentDirectory :
                Path.GetDirectoryName(Path.GetFullPath(request.ProgramFilePath))!));
        ISettings settings;
        if (request.NuGetConfigFile is not null)
        {
            string file = Path.GetFullPath(request.NuGetConfigFile);
            if (!File.Exists(file)) throw new FileNotFoundException("NuGet configuration file is missing.", file);
            settings = NuGet.Configuration.Settings.LoadSpecificSettings(Path.GetDirectoryName(file)!, Path.GetFileName(file));
        }
        else
        {
            settings = NuGet.Configuration.Settings.LoadDefaultSettings(configurationDirectory,
                configFileName: null, machineWideSettings: new XPlatMachineWideSetting());
        }
        var sourceProvider = new PackageSourceProvider(settings);
        var configuredSources = sourceProvider.LoadPackageSources().ToArray();
        // A student needs only the runtime, not an SDK-created NuGet.Config.
        // Do not undo an explicit packageSources section (including <clear/>)
        // or a deliberate disabling of nuget.org. This fallback does not write settings.
        if (configuredSources.Length == 0 && settings.GetSection("packageSources") is null &&
            sourceProvider.IsPackageSourceEnabled("nuget.org"))
            configuredSources = new[] { new PackageSource("https://api.nuget.org/v3/index.json", "nuget.org") };
        var sources = request.Sources is null
            ? configuredSources.Where(source => source.IsEnabled).ToArray()
            : request.Sources.Select(source =>
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(source);
                string normalized = Uri.TryCreate(source, UriKind.Absolute, out var uri) && !uri.IsFile
                    ? source : Path.GetFullPath(source, configurationDirectory);
                return configuredSources.FirstOrDefault(configured => configured.IsEnabled &&
                    string.Equals(configured.Source, normalized, StringComparison.Ordinal)) ?? new PackageSource(normalized);
            }).DistinctBy(source => source.Source, StringComparer.Ordinal).ToArray();
        if (sources.Length == 0 && !request.LocalPackagesOnly)
            throw new ArgumentException("No enabled NuGet package sources. Configure NuGet.Config or provide Sources.", nameof(request));

        string packagesDirectory = Path.GetFullPath(request.PackagesDirectory ?? SettingsUtility.GetGlobalPackagesFolder(settings));
        string restoreDirectory = request.RestoreDirectory is null
            ? GetProgramRestoreDirectory(request.ProgramFilePath)
            : Path.GetFullPath(request.RestoreDirectory);
        var configuration = new PackageRestoreConfiguration(packagesDirectory, restoreDirectory,
            Array.AsReadOnly(sources.Select(source => new PackageSourceInfo(source.Name, source.Source)).ToArray()),
            Array.AsReadOnly(settings.GetConfigFilePaths().ToArray()));
        return new RestoreEnvironment(settings, sources, configuration);
    }

    private static string GetProgramRestoreDirectory(string? programFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(programFilePath);
        string identity = Path.GetFullPath(programFilePath);
        if (OperatingSystem.IsWindows()) identity = identity.ToUpperInvariant();
        string key = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(identity))).ToLowerInvariant();
        string cacheRoot = OperatingSystem.IsWindows()
            ? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            : Environment.GetEnvironmentVariable("XDG_CACHE_HOME") ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cache");
        return Path.Combine(cacheRoot, "PascalABC.NET", "NuGet", "Restore", key);
    }
}
