using System.Text;
using System.Xml.Linq;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using PascalABCNet.PackageServices;

internal static class PlatformPackDownloadTests
{
    public static async Task<int> RunAsync()
    {
        string root = Path.Combine(Path.GetTempPath(), "pabc-platform-download-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        int passed = 0;
        void Check(bool condition, string message)
        {
            if (!condition) throw new Exception(message);
            passed++;
        }
        async Task Reject<T>(Func<Task> action, string message) where T : Exception
        {
            try { await action(); }
            catch (T) { passed++; return; }
            throw new Exception("Expected " + typeof(T).Name + ": " + message);
        }
        try
        {
            string feed = Path.Combine(root, "feed");
            string packages = Path.Combine(root, "packages");
            string work = Path.Combine(root, "restore");
            Directory.CreateDirectory(feed);
            Directory.CreateDirectory(work);
            string config = Path.Combine(root, "NuGet.Config");
            // Do not inherit the developer's feeds, global cache or package source settings.
            new XDocument(new XElement("configuration",
                new XElement("packageSources", new XElement("clear"),
                    new XElement("add", new XAttribute("key", "fixture"), new XAttribute("value", feed)))))
                .Save(config);
            string payload = Path.Combine(root, "fixture.bin");
            File.WriteAllText(payload, "platform-pack-fixture-not-an-executable", Encoding.UTF8);
            const string id = "PascalABC.Test.PlatformPack";
            const string runtimeId = "PascalABC.Test.PlatformRuntime";
            string MakePackage(string packageId, string version)
            {
                var builder = new PackageBuilder { Id = packageId, Version = NuGetVersion.Parse(version),
                    Description = "Local platform download fixture" };
                builder.Authors.Add("PascalABC.NET tests");
                builder.PackageTypes.Add(new PackageType("DotnetPlatform", new Version(1, 0)));
                // Deliberately not a normal net10.0 library. PackageDownload must extract
                // the package without checking compile compatibility or loading its DLL.
                builder.Files.Add(new PhysicalPackageFile {
                    SourcePath = payload, TargetPath = "ref/net8.0/Fixture.dll" });
                builder.DependencyGroups.Add(new PackageDependencyGroup(NuGetFramework.ParseFolder("net8.0"),
                    new[] { new PackageDependency("PascalABC.DeliberatelyMissingDependency", VersionRange.Parse("[1.0.0]")) }));
                string file = Path.Combine(feed, packageId + "." + version + ".nupkg");
                using var output = File.Create(file);
                builder.Save(output);
                return file;
            }
            string versionOne = MakePackage(id, "1.0.0");
            MakePackage(id, "2.0.0");
            MakePackage(runtimeId, "1.0.0");
            var selected = new SelectedPlatformPackage(id, "1.0.0", id, "any", "10.0.19041.0", "Windows");
            var plan = new PlatformPackSelection("net10.0-windows10.0.19041.0",
                Array.Empty<FrameworkReferenceDefinition>(), new[] { selected });
            var request = new PlatformPackDownloadRequest
            {
                Selection = plan,
                ProgramFilePath = Path.Combine(root, "main.pas"),
                PackagesDirectory = packages,
                NuGetConfigFile = config,
                RestoreDirectory = work
            };
            var service = new PlatformPackDownloadService();
            string normalAssets = Path.Combine(work, "project.assets.json");
            File.WriteAllText(normalAssets, "ordinary-restore-must-not-change");
            var downloaded = await service.DownloadAsync(request);
            Check(downloaded.Count == 1 && !downloaded[0].UsedCachedPackage &&
                downloaded[0].Id == id && downloaded[0].Version == "1.0.0",
                "the same targeting/runtime package is downloaded once at the exact version");
            string packageDirectory = downloaded[0].Directory;
            var resolver = new VersionFolderPathResolver(packages);
            Check(packageDirectory == resolver.GetInstallPath(id, NuGetVersion.Parse("1.0.0")),
                "package is extracted into NuGet's global-cache layout");
            string extracted = Path.Combine(packageDirectory, "ref", "net8.0", "Fixture.dll");
            Check(File.ReadAllBytes(extracted).SequenceEqual(File.ReadAllBytes(payload)),
                "complete package contents are extracted without loading the fake DLL");
            Check(File.Exists(resolver.GetHashPath(id, NuGetVersion.Parse("1.0.0"))) &&
                File.Exists(resolver.GetNupkgMetadataPath(id, NuGetVersion.Parse("1.0.0"))),
                "NuGet completion metadata and hash are present");
            Check(!Directory.Exists(Path.Combine(packages, "pascalabc.deliberatelymissingdependency")),
                "PackageDownload does not restore ordinary transitive library dependencies");
            Check(!Directory.Exists(resolver.GetInstallPath(id, NuGetVersion.Parse("2.0.0"))),
                "a newer feed version is not substituted");
            Check(File.ReadAllText(normalAssets) == "ordinary-restore-must-not-change" &&
                File.Exists(Path.Combine(work, "platform-packs", "project.assets.json")),
                "platform download metadata is isolated from ordinary restore");
            Check(!File.Exists(Path.Combine(root, "Fixture.dll")) &&
                !File.Exists(Path.Combine(root, "main.deps.json")) && !File.Exists(Path.Combine(root, "main.runtimeconfig.json")),
                "no DLL copy or execution manifests are produced beside the program");

            // Cache reuse must work with a feed that is no longer available.
            Directory.Move(feed, feed + "-offline");
            var reused = await service.DownloadAsync(request);
            Check(reused.Single().UsedCachedPackage && reused.Single().Directory == packageDirectory,
                "completed cache is used without contacting the unavailable feed");
            Check((await service.DownloadAsync(request with { LocalPackagesOnly = true })).Single().UsedCachedPackage,
                "cache-only mode reuses a complete package");
            var missing = request with {
                Selection = plan with { Packages = new[] { selected with { Version = "3.0.0" } } },
                LocalPackagesOnly = true
            };
            await Reject<PackageRestoreRequiredException>(() => service.DownloadAsync(missing), "offline cache miss");
            Check(!Directory.Exists(resolver.GetInstallPath(id, NuGetVersion.Parse("3.0.0"))),
                "cache-only miss does not install a package");
            Directory.Move(feed + "-offline", feed);

            var disabledSources = Path.Combine(root, "disabled.config");
            new XDocument(new XElement("configuration",
                new XElement("packageSources", new XElement("clear")))).Save(disabledSources);
            Check((await service.DownloadAsync(request with { NuGetConfigFile = disabledSources }))
                .Single().UsedCachedPackage, "cached packages do not require any enabled source");
            await Reject<ArgumentException>(() => service.DownloadAsync(missing with {
                NuGetConfigFile = disabledSources, LocalPackagesOnly = false
            }), "missing package with explicitly disabled sources");

            var versionTwo = request with { Selection = plan with {
                Packages = new[] { selected with { Version = "2.0.0" } }
            } };
            var results = await Task.WhenAll(service.DownloadAsync(versionTwo), service.DownloadAsync(versionTwo));
            Check(results.All(r => r.Count == 1 && r[0].Version == "2.0.0"),
                "concurrent download requests both get the requested package");
            Check(results.Count(r => r[0].UsedCachedPackage) == 1,
                "same-program downloads recheck cache after the work-directory lock");
            Check((await service.DownloadAsync(request)).Single().Version == "1.0.0" &&
                File.Exists(extracted), "downloading another version does not remove the old one");

            // A leftover directory without NuGet's completion markers is not a cache hit.
            string partialRoot = Path.Combine(root, "partial-cache");
            string partialDirectory = new VersionFolderPathResolver(partialRoot)
                .GetInstallPath(id, NuGetVersion.Parse("1.0.0"));
            Directory.CreateDirectory(partialDirectory);
            File.WriteAllText(Path.Combine(partialDirectory, "leftover.txt"), "incomplete");
            await Reject<PackageRestoreRequiredException>(() => service.DownloadAsync(request with {
                PackagesDirectory = partialRoot, LocalPackagesOnly = true
            }), "partial cache is rejected in offline mode");
            var repaired = await service.DownloadAsync(request with { PackagesDirectory = partialRoot });
            Check(!repaired.Single().UsedCachedPackage &&
                File.Exists(Path.Combine(repaired.Single().Directory, "ref", "net8.0", "Fixture.dll")),
                "NuGet installs a complete package over an unfinished cache directory");

            var separateRuntime = await service.DownloadAsync(request with {
                Selection = plan with { Packages = new[] { selected with { RuntimePackageId = runtimeId } } }
            });
            Check(separateRuntime.Count == 2 && separateRuntime.Any(p => p.Id == runtimeId && !p.UsedCachedPackage),
                "different targeting/runtime package IDs are both cached");
            var duplicate = await service.DownloadAsync(request with {
                Selection = plan with { Packages = new[] { selected, selected with { Id = id.ToUpperInvariant() } } }
            });
            Check(duplicate.Count == 1 && duplicate[0].UsedCachedPackage,
                "duplicate package identities are deduplicated ignoring ID case");
            await Reject<ArgumentException>(() => service.DownloadAsync(request with {
                Selection = plan with { Packages = new[] { selected, selected with { Version = "2.0.0" } } }
            }), "conflicting selected versions");
            await Reject<ArgumentException>(() => service.DownloadAsync(request with {
                Selection = plan with { Packages = new[] { selected with { Id = "../outside" } } }
            }), "invalid package ID is rejected before cache writes");
            await Reject<ArgumentException>(() => service.DownloadAsync(request with {
                Selection = plan with { Packages = new[] { selected with { Version = "../bad" } } }
            }), "invalid package version is rejected");

            using (var cancellation = new CancellationTokenSource())
            {
                cancellation.Cancel();
                await Reject<OperationCanceledException>(() => service.DownloadAsync(request, cancellation.Token),
                    "already cancelled download");
            }
            Check((await service.DownloadAsync(request)).Single().UsedCachedPackage,
                "cancellation does not damage already cached packages");
            var noPacks = request with {
                Selection = PlatformPackSelector.Select(PlatformPackCatalogReader.ReadNet10(), "net10.0"),
                NuGetConfigFile = Path.Combine(root, "must-not-be-opened.config"),
                PackagesDirectory = Path.Combine(root, "must-not-be-created")
            };
            Check((await service.DownloadAsync(noPacks)).Count == 0 &&
                !Directory.Exists(noPacks.PackagesDirectory), "empty selection performs no configuration or cache I/O");

            var missingOnline = missing with { LocalPackagesOnly = false };
            await Reject<PackageRestoreException>(() => service.DownloadAsync(missingOnline), "missing exact version");
            MakePackage(id, "3.0.0");
            Check((await service.DownloadAsync(missingOnline)).Single().Version == "3.0.0",
                "a failed download can be retried in the same process after the package appears");
            Check(File.Exists(versionOne), "test feed remains intact");
            Console.WriteLine($"PASS: {passed} platform package cache download checks (isolated local feed).");
            return passed;
        }
        finally { Directory.Delete(root, recursive: true); }
    }
}
