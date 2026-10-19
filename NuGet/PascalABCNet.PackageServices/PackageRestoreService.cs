using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.RuntimeModel;
using NuGet.Versioning;

namespace PascalABCNet.PackageServices;

/// <summary>Restores packages using NuGet libraries; never invokes SDK tools.</summary>
public sealed class PackageRestoreService
{
    /// <summary>Reports effective paths and sources without restoring or downloading packages.</summary>
    public PackageRestoreConfiguration GetConfiguration(PackageRestoreRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return RestoreEnvironment.Resolve(request).Configuration;
    }

    public async Task<ResolvedPackageGraph> RestoreAsync(PackageRestoreRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        ArgumentNullException.ThrowIfNull(request.Packages);
        if (!string.Equals(request.TargetFramework, "net10.0", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("This restore prototype supports net10.0 only.", nameof(request));

        var environment = RestoreEnvironment.Resolve(request);
        // The compiler and IntelliSense can restore the same program concurrently.
        // Protect writes AND consumption of the result, not just RestoreRunner:
        // another client may replace/delete project.assets.json before we read it.
        // Use NuGet's cross-process lock under its temp directory; no global gate
        // and no lock file beside user source. Waiting supports cancellation.
        return await ConcurrencyUtilities.ExecuteWithFileLockedAsync(
            Path.Combine(environment.Configuration.RestoreDirectory, "PascalABCNET.restore"),
            token => RestoreCoreAsync(request, environment, token), cancellationToken).ConfigureAwait(false);
    }

    private static async Task<ResolvedPackageGraph> RestoreCoreAsync(PackageRestoreRequest request,
        RestoreEnvironment environment, CancellationToken cancellationToken)
    {
        var framework = NuGetFramework.ParseFolder(request.TargetFramework);
        string? rid = request.RuntimeIdentifier ??
            (OperatingSystem.IsWindows() && RuntimeInformation.ProcessArchitecture == Architecture.X64 ? "win-x64" : null);
        if (rid is not null && rid != "win-x64")
            throw new ArgumentException("Native NuGet assets currently support win-x64 only.", nameof(request));
        var requirements = new Dictionary<string, NuGetVersion>(StringComparer.OrdinalIgnoreCase);
        foreach (var package in request.Packages)
        {
            ArgumentNullException.ThrowIfNull(package);
            ArgumentException.ThrowIfNullOrWhiteSpace(package.Id);
            if (!NuGet.Packaging.PackageIdValidator.IsValidPackageId(package.Id))
                throw new ArgumentException("Invalid package ID: " + package.Id, nameof(request));
            if (!NuGetVersion.TryParse(package.Version, out var version))
                throw new ArgumentException("An exact package version is required: " + package.Version, nameof(request));
            if (requirements.TryGetValue(package.Id, out var previous) && previous != version)
                throw new ArgumentException("Conflicting requested versions of " + package.Id, nameof(request));
            requirements[package.Id] = version;
        }
        string packagesDirectory = environment.Configuration.PackagesDirectory;
        string restoreDirectory = environment.Configuration.RestoreDirectory;
        Directory.CreateDirectory(restoreDirectory);
        // This is an identity for the in-memory project, not a generated csproj file.
        string projectIdentity = Path.Combine(restoreDirectory, "PascalPackageRestore.csproj");
        var dependencies = requirements.OrderBy(p => p.Key, StringComparer.OrdinalIgnoreCase)
            .Select(p => new LibraryDependency
            {
                LibraryRange = new LibraryRange(p.Key,
                    new VersionRange(p.Value, true, p.Value, true), LibraryDependencyTarget.Package),
                IncludeType = LibraryIncludeFlags.Compile | LibraryIncludeFlags.Runtime |
                    LibraryIncludeFlags.Native,
                SuppressParent = LibraryIncludeFlags.None
            }).ToImmutableArray();
        var spec = new PackageSpec(new[] { new TargetFrameworkInformation
        {
            FrameworkName = framework,
            TargetAlias = "net10.0",
            RuntimeIdentifierGraphPath = rid is null ? null : PrepareRuntimeGraphFile(restoreDirectory),
            Dependencies = dependencies
        } })
        {
            Name = "PascalPackageRestore",
            FilePath = projectIdentity,
            RuntimeGraph = rid is null ? RuntimeGraph.Empty : new RuntimeGraph(new[] { new RuntimeDescription(rid) }),
            RestoreMetadata = new ProjectRestoreMetadata
            {
                ProjectStyle = ProjectStyle.PackageReference,
                ProjectName = "PascalPackageRestore",
                ProjectPath = projectIdentity,
                ProjectUniqueName = projectIdentity,
                OutputPath = restoreDirectory,
                PackagesPath = packagesDirectory,
                Sources = environment.Sources.ToList(),
                ConfigFilePaths = environment.Configuration.ConfigurationFiles.ToList(),
                FallbackFolders = SettingsUtility.GetFallbackPackageFolders(environment.Settings).ToList(),
                TargetFrameworks = new[] { new ProjectRestoreMetadataFrameworkInfo(framework)
                    { TargetAlias = "net10.0" } },
                OriginalTargetFrameworks = new[] { "net10.0" },
                RestoreAuditProperties = new RestoreAuditProperties { EnableAudit = "false" },
                RestoreDoNotWriteDependencyGraphSpec = true
            }
        };
        var dependencyGraph = new DependencyGraphSpec();
        dependencyGraph.AddProject(spec);
        dependencyGraph.AddRestore(projectIdentity);
        var logger = new DiagnosticLogger();
        using var cache = new SourceCacheContext();
        var args = new RestoreArgs
        {
            Log = logger,
            CacheContext = cache,
            GlobalPackagesFolder = packagesDirectory,
            Sources = environment.Sources.Select(source => source.Source).ToList(),
            AllowNoOp = !request.ForceRestore,
            DisableParallel = true
        };
        args.PreLoadedRequestProviders.Add(new DependencyGraphSpecRequestProvider(
            new RestoreCommandProvidersCache(), dependencyGraph, environment.Settings));

        var summaries = await RestoreRunner.RunAsync(args, cancellationToken).ConfigureAwait(false);
        var summary = summaries.Single();
        var diagnostics = logger.Messages.ToArray();
        if (!summary.Success)
            throw new PackageRestoreException(diagnostics);

        string assetsPath = Path.Combine(restoreDirectory, LockFileFormat.AssetsFileName);
        var lockFile = new LockFileFormat().Read(assetsPath, logger)
            ?? throw new InvalidDataException("NuGet did not produce a valid assets file.");
        var target = lockFile.Targets.Single(t => t.TargetFramework.Equals(framework) &&
            (rid is null ? string.IsNullOrEmpty(t.RuntimeIdentifier) : t.RuntimeIdentifier == rid));
        var packages = target.Libraries.Where(l => l.Type == "package")
            .OrderBy(l => l.Name, StringComparer.OrdinalIgnoreCase).Select(library =>
            {
                string name = library.Name ?? throw new InvalidDataException("A restored package has no ID.");
                var version = library.Version ?? throw new InvalidDataException("A restored package has no version.");
                var entry = lockFile.Libraries.Single(l => l.Name == library.Name &&
                    l.Version == library.Version && l.Type == "package");
                string directory = lockFile.PackageFolders.Select(folder => GetPackagePath(folder.Path, entry.Path))
                    .FirstOrDefault(Directory.Exists)
                    ?? throw new DirectoryNotFoundException("Restored package directory is missing: " + name);
                return new ResolvedPackage(name, version.ToNormalizedString(), directory,
                    Array.AsReadOnly(library.Dependencies.Select(d =>
                        new ResolvedPackageDependency(d.Id, d.VersionRange.ToNormalizedString())).ToArray()),
                    FrameworkAssemblyPolicy.ExcludeProvidedAssemblies(GetAssemblies(directory, library.CompileTimeAssemblies)),
                    FrameworkAssemblyPolicy.ExcludeProvidedAssemblies(GetAssemblies(directory, library.RuntimeAssemblies)))
                {
                    NativeLibraries = GetAssets(directory, library.NativeLibraries)
                };
            }).ToArray();
        var installedIds = logger.InstalledPackageIds.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var installedPackages = packages.Where(package => installedIds.Contains(package.Id))
            .Select(package => new PackageRequirement(package.Id, package.Version)).ToArray();
        var portable = lockFile.Targets.Single(t => t.TargetFramework.Equals(framework) && string.IsNullOrEmpty(t.RuntimeIdentifier));
        bool platformSpecific = target.Libraries.Count != portable.Libraries.Count || target.Libraries.Any(library =>
        {
            var generic = portable.Libraries.FirstOrDefault(p => p.Name == library.Name && p.Version == library.Version);
            return generic is null || library.NativeLibraries.Count != 0 ||
                !library.RuntimeAssemblies.Select(a => a.Path).SequenceEqual(generic.RuntimeAssemblies.Select(a => a.Path));
        });
        return new ResolvedPackageGraph("net10.0", assetsPath, Array.AsReadOnly(packages),
            summary.NoOpRestore, summary.InstallCount, Array.AsReadOnly(diagnostics))
        {
            Configuration = environment.Configuration,
            // Ordinary managed programs remain portable, as before this feature.
            RuntimeIdentifier = request.RuntimeIdentifier ?? (platformSpecific ? rid : null),
            InstalledPackages = Array.AsReadOnly(installedPackages)
        };
    }

    private static IReadOnlyList<string> GetAssemblies(string directory, IEnumerable<LockFileItem> items)
        => GetAssets(directory, items.Where(item => item.Path.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)));

    private static IReadOnlyList<string> GetAssets(string directory, IEnumerable<LockFileItem> items)
    {
        var files = items.Where(item => Path.GetFileName(item.Path) != "_._")
            .Select(item => GetPackagePath(directory, item.Path)).ToArray();
        foreach (string file in files)
            if (!File.Exists(file))
                throw new FileNotFoundException("A selected NuGet assembly is missing.", file);
        return Array.AsReadOnly(files);
    }

    private static string PrepareRuntimeGraphFile(string restoreDirectory)
    {
        using var stream = typeof(PackageRestoreService).Assembly.GetManifestResourceStream(
            "PascalABCNet.PackageServices.PortableRuntimeIdentifierGraph.json")!;
        using var reader = new StreamReader(stream);
        string json = reader.ReadToEnd();
        string path = Path.Combine(restoreDirectory, "PortableRuntimeIdentifierGraph.json");
        // NuGet's API expects a file. Extract bundled data, never consult an
        // installed SDK, and preserve timestamps for no-op restores.
        if (!File.Exists(path) || File.ReadAllText(path) != json) File.WriteAllText(path, json);
        return path;
    }

    private static string GetPackagePath(string directory, string relativePath)
    {
        string root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory)) + Path.DirectorySeparatorChar;
        string path = Path.GetFullPath(Path.Combine(root, relativePath.Replace('/', Path.DirectorySeparatorChar)));
        var comparison = OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        if (!path.StartsWith(root, comparison))
            throw new InvalidDataException("NuGet asset path escapes its package directory: " + relativePath);
        return path;
    }

    private sealed class DiagnosticLogger : LoggerBase
    {
        internal ConcurrentQueue<PackageDiagnostic> Messages { get; } = new();
        internal ConcurrentQueue<string> InstalledPackageIds { get; } = new();

        public override void Log(ILogMessage message)
        {
            // NuGet reports a completed install at Information level. Keep only
            // the package ID; source URLs and cache paths must not enter IDE logs.
            if (message.Level == LogLevel.Information &&
                message.Message.StartsWith("Installed ", StringComparison.Ordinal))
            {
                string identity = message.Message.Substring("Installed ".Length);
                int space = identity.IndexOf(' ');
                if (space > 0)
                    InstalledPackageIds.Enqueue(identity.Substring(0, space));
            }
            if (message.Level is LogLevel.Warning or LogLevel.Error)
                Messages.Enqueue(new PackageDiagnostic(message.Level.ToString(),
                    message.Code.ToString(), message.Message));
        }

        public override Task LogAsync(ILogMessage message)
        {
            Log(message);
            return Task.CompletedTask;
        }
    }
}
