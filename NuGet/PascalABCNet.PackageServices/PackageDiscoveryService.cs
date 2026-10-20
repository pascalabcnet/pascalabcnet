using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace PascalABCNet.PackageServices;

public sealed record PackageSuggestion(string Id, string Version, string? Description,
    long Downloads, string Source);

/// <summary>Explicit, cancellable discovery. Never restores or installs packages.</summary>
public sealed class PackageDiscoveryService
{
    public async Task<IReadOnlyList<PackageSuggestion>> SearchAsync(string programFilePath,
        string prefix, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(programFilePath);
        prefix = (prefix ?? string.Empty).Trim();
        if (prefix.Length < 3)
            return SearchLocal(programFilePath, prefix);

        var environment = RestoreEnvironment.Resolve(new PackageRestoreRequest {
            Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = programFilePath });
        var candidates = new List<PackageSuggestion>();
        Exception? failure = null;
        foreach (var source in environment.Sources)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var repository = Repository.Factory.GetCoreV3(source);
                var resource = await repository.GetResourceAsync<PackageSearchResource>(cancellationToken)
                    .ConfigureAwait(false);
                if (resource is null) continue;
                var found = await resource.SearchAsync(prefix, new SearchFilter(includePrerelease: false),
                    0, 100, NullLogger.Instance, cancellationToken).ConfigureAwait(false);
                foreach (var item in found)
                {
                    if (item.Identity?.Version is not NuGetVersion version || version.IsPrerelease)
                        continue;
                    candidates.Add(new PackageSuggestion(item.Identity.Id, version.ToNormalizedString(),
                        item.Description, item.DownloadCount.GetValueOrDefault(), source.Name));
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
            catch (Exception error) { failure = error; }
        }
        if (candidates.Count == 0 && failure is not null)
            throw new IOException("NuGet package search failed for all configured sources.", failure);
        return Rank(candidates, prefix);
    }

    public async Task<IReadOnlyList<string>> GetVersionsAsync(string programFilePath, string packageId,
        string prefix = "", CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(programFilePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(packageId);
        var environment = RestoreEnvironment.Resolve(new PackageRestoreRequest {
            Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = programFilePath });
        var versions = new HashSet<NuGetVersion>();
        Exception? failure = null;
        using var cache = new SourceCacheContext();
        foreach (var source in environment.Sources)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var repository = Repository.Factory.GetCoreV3(source);
                var resource = await repository.GetResourceAsync<FindPackageByIdResource>(cancellationToken)
                    .ConfigureAwait(false);
                if (resource is null) continue;
                foreach (var version in await resource.GetAllVersionsAsync(packageId, cache,
                    NullLogger.Instance, cancellationToken).ConfigureAwait(false))
                    if (!version.IsPrerelease && version.ToNormalizedString().StartsWith(prefix ?? "",
                            StringComparison.OrdinalIgnoreCase)) versions.Add(version);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
            catch (Exception error) { failure = error; }
        }
        if (versions.Count == 0 && failure is not null)
            throw new IOException("NuGet version lookup failed for all configured sources.", failure);
        return versions.OrderDescending().Take(30).Select(v => v.ToNormalizedString()).ToArray();
    }

    public static IReadOnlyList<PackageSuggestion> Rank(IEnumerable<PackageSuggestion> suggestions, string prefix) =>
        suggestions.GroupBy(s => s.Id, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.OrderByDescending(s => NuGetVersion.Parse(s.Version)).First())
            .OrderBy(s => s.Id.Equals(prefix, StringComparison.OrdinalIgnoreCase) ? 0 :
                s.Id.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ? 1 : 2)
            .ThenByDescending(s => s.Downloads)
            .ThenBy(s => s.Id, StringComparer.OrdinalIgnoreCase)
            .Take(20).ToArray();

    private static IReadOnlyList<PackageSuggestion> SearchLocal(string programFilePath, string prefix)
    {
        var environment = RestoreEnvironment.Resolve(new PackageRestoreRequest {
            Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = programFilePath,
            LocalPackagesOnly = true });
        string root = environment.Configuration.PackagesDirectory;
        if (!Directory.Exists(root)) return Array.Empty<PackageSuggestion>();
        return Directory.EnumerateDirectories(root)
            .Where(path => Path.GetFileName(path).StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(path => new {
                Id = Path.GetFileName(path),
                Versions = Directory.EnumerateDirectories(path).Select(Path.GetFileName)
                    .Select(name => NuGetVersion.TryParse(name, out var version) ? version : null)
                    .Where(version => version is not null && !version.IsPrerelease).ToArray()
            })
            .Where(item => item.Versions.Length > 0)
            .OrderBy(item => item.Id, StringComparer.OrdinalIgnoreCase)
            .Take(10)
            .Select(item => new PackageSuggestion(item.Id, item.Versions.Max()!.ToNormalizedString(),
                null, 0, string.Empty))
            .ToArray();
    }
}
