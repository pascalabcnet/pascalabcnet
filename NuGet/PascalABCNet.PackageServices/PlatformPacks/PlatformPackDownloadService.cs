using System.Collections.Concurrent;
using System.Collections.Immutable;
using NuGet.Commands;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.LibraryModel;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace PascalABCNet.PackageServices;

/// <summary>
/// Downloads exact selected pack identities via NuGet PackageDownload, not PackageReference.
/// NuGet owns extraction, package verification and global-cache install locks.
/// Does not execute package build targets, resolve DLL assets or modify compiler references.
/// </summary>
public sealed class PlatformPackDownloadService
{
    public async Task<IReadOnlyList<CachedPlatformPackage>> DownloadAsync(PlatformPackDownloadRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Selection);
        ArgumentNullException.ThrowIfNull(request.Selection.Packages);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.ProgramFilePath);
        cancellationToken.ThrowIfCancellationRequested();
        var identities = GetIdentities(request.Selection);
        if (identities.Length == 0)
            return Array.Empty<CachedPlatformPackage>();

        var context = new PackageRestoreRequest
        {
            Packages = Array.Empty<PackageRequirement>(),
            ProgramFilePath = request.ProgramFilePath,
            Sources = request.Sources,
            PackagesDirectory = request.PackagesDirectory,
            ConfigurationDirectory = request.ConfigurationDirectory,
            NuGetConfigFile = request.NuGetConfigFile,
            RestoreDirectory = request.RestoreDirectory,
            // Resolve settings without rejecting an empty source list: an already cached
            // package is usable offline even when all feeds have been disabled.
            LocalPackagesOnly = true
        };
        var environment = RestoreEnvironment.Resolve(context);
        var resolver = new VersionFolderPathResolver(environment.Configuration.PackagesDirectory);
        if (identities.All(identity => IsComplete(resolver, identity)))
            return Describe(identities, resolver, identities.Select(p => p.Id).ToHashSet(StringComparer.OrdinalIgnoreCase));
        if (request.LocalPackagesOnly)
            throw new PackageRestoreRequiredException();
        if (environment.Sources.Count == 0)
            throw new ArgumentException("No enabled NuGet package sources for missing platform packages.", nameof(request));

        // Never overwrite the ordinary compilation's assets or restore lock.
        string work = Path.Combine(environment.Configuration.RestoreDirectory, "platform-packs");
        Directory.CreateDirectory(work);
        return await ConcurrencyUtilities.ExecuteWithFileLockedAsync(
            Path.Combine(work, "PascalABCNET.platform-packs"),
            token => DownloadCoreAsync(identities, request.Selection.TargetFramework, environment, resolver, work, token),
            cancellationToken).ConfigureAwait(false);
    }

    private static PackageIdentity[] GetIdentities(PlatformPackSelection selection)
    {
        var identities = new Dictionary<string, PackageIdentity>(StringComparer.OrdinalIgnoreCase);
        foreach (var package in selection.Packages)
        {
            if (package is null)
                throw new ArgumentException("A selected platform package must not be null.", nameof(selection));
            if (string.IsNullOrWhiteSpace(package.Version) ||
                !NuGetVersion.TryParse(package.Version, out var version))
                throw new ArgumentException("Invalid selected platform package version.", nameof(selection));
            void Add(string id)
            {
                if (string.IsNullOrWhiteSpace(id) || !PackageIdValidator.IsValidPackageId(id))
                    throw new ArgumentException("Invalid selected platform package ID.", nameof(selection));
                if (identities.TryGetValue(id, out var previous) && previous.Version != version)
                    throw new ArgumentException("Conflicting selected platform package versions: " + id, nameof(selection));
                identities[id] = new PackageIdentity(id, version);
            }
            Add(package.Id);
            // The catalog's targeting/runtime package identities share the selected version.
            // They are usually the same package; do not download it twice.
            Add(package.RuntimePackageId);
        }
        return identities.Values.OrderBy(p => p.Id, StringComparer.OrdinalIgnoreCase).ToArray();
    }

    private static async Task<IReadOnlyList<CachedPlatformPackage>> DownloadCoreAsync(PackageIdentity[] identities,
        string targetFramework, RestoreEnvironment environment, VersionFolderPathResolver resolver,
        string work, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cached = identities.Where(identity => IsComplete(resolver, identity))
            .Select(p => p.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);
        if (cached.Count == identities.Length)
            return Describe(identities, resolver, cached);
        var framework = NuGetFramework.ParseFolder(targetFramework);
        if (framework.IsUnsupported)
            throw new ArgumentException("Invalid platform download target framework: " + targetFramework);

        string projectPath = Path.Combine(work, "PlatformPackDownload.csproj");
        var spec = new PackageSpec(new[]
        {
            new TargetFrameworkInformation
            {
                FrameworkName = framework,
                TargetAlias = targetFramework,
                DownloadDependencies = identities.Where(p => !cached.Contains(p.Id)).Select(p =>
                    new DownloadDependency(p.Id, new VersionRange(p.Version, true, p.Version, true)))
                    .ToImmutableArray()
            }
        })
        {
            Name = "PascalPlatformPackDownload",
            FilePath = projectPath,
            RestoreMetadata = new ProjectRestoreMetadata
            {
                ProjectStyle = ProjectStyle.PackageReference,
                ProjectName = "PascalPlatformPackDownload",
                ProjectPath = projectPath,
                ProjectUniqueName = projectPath,
                OutputPath = work,
                PackagesPath = environment.Configuration.PackagesDirectory,
                Sources = environment.Sources.ToList(),
                ConfigFilePaths = environment.Configuration.ConfigurationFiles.ToList(),
                TargetFrameworks = new[]
                {
                    new ProjectRestoreMetadataFrameworkInfo(framework) { TargetAlias = targetFramework }
                },
                OriginalTargetFrameworks = new[] { targetFramework },
                RestoreAuditProperties = new RestoreAuditProperties { EnableAudit = "false" },
                RestoreDoNotWriteDependencyGraphSpec = true
            }
        };
        var graph = new DependencyGraphSpec();
        graph.AddProject(spec);
        graph.AddRestore(projectPath);
        var logger = new DownloadLogger();
        using var cache = new SourceCacheContext();
        var args = new RestoreArgs
        {
            Log = logger,
            CacheContext = cache,
            GlobalPackagesFolder = environment.Configuration.PackagesDirectory,
            Sources = environment.Sources.Select(p => p.Source).ToList(),
            // A previous download-only assets file is not proof that the package cache is intact.
            AllowNoOp = false,
            DisableParallel = true
        };
        args.PreLoadedRequestProviders.Add(new DependencyGraphSpecRequestProvider(
            new RestoreCommandProvidersCache(), graph, environment.Settings));
        var summaries = await RestoreRunner.RunAsync(args, cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        if (!summaries.Single().Success)
            throw new PackageRestoreException(logger.Messages.ToArray());
        foreach (var identity in identities)
            if (!IsComplete(resolver, identity))
                throw new InvalidDataException("NuGet did not produce a complete platform package: " + identity);
        return Describe(identities, resolver, cached);
    }

    private static bool IsComplete(VersionFolderPathResolver resolver, PackageIdentity identity) =>
        File.Exists(resolver.GetNupkgMetadataPath(identity.Id, identity.Version)) &&
        File.Exists(resolver.GetHashPath(identity.Id, identity.Version)) &&
        File.Exists(resolver.GetPackageFilePath(identity.Id, identity.Version)) &&
        File.Exists(resolver.GetManifestFilePath(identity.Id, identity.Version));

    private static IReadOnlyList<CachedPlatformPackage> Describe(PackageIdentity[] identities,
        VersionFolderPathResolver resolver, HashSet<string> cached) =>
        Array.AsReadOnly(identities.Select(p => new CachedPlatformPackage(p.Id, p.Version.ToNormalizedString(),
            resolver.GetInstallPath(p.Id, p.Version), cached.Contains(p.Id))).ToArray());

    private sealed class DownloadLogger : LoggerBase
    {
        internal ConcurrentQueue<PackageDiagnostic> Messages { get; } = new();
        public override void Log(ILogMessage message)
        {
            if (message.Level is LogLevel.Warning or LogLevel.Error)
                Messages.Enqueue(new PackageDiagnostic(message.Level.ToString(), message.Code.ToString(), message.Message)
                {
                    PackageId = (message as IRestoreLogMessage)?.LibraryId
                });
        }
        public override Task LogAsync(ILogMessage message)
        {
            Log(message);
            return Task.CompletedTask;
        }
    }
}
