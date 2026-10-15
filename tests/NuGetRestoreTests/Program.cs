using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Xml.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using PascalABCNet.PackageServices;

internal static class Program
{
    private const string RootId = "PascalABC.TestRoot";
    private const string DependencyId = "PascalABC.TestDependency";
    private static readonly List<string> HiddenRestoreDirectories = new();

    private static async Task<int> Main(string[] args)
    {
        string work = Path.Combine(Path.GetTempPath(), "pabc-nuget-restore-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(work);
        try
        {
            await CheckRestore(work);
            await CheckConfiguration(work);
            if (args.Contains("--shared-cache"))
            {
                await CheckSharedCache(work);
                await CheckRealTransitivePackage(work);
            }
            if (!args.Contains("--runtime-only-child"))
                await CheckRuntimeOnly(work);
            Console.WriteLine("PASS: NuGet library restore, exact versions, transitive dependencies, ref/lib, cache and diagnostics.");
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
        finally
        {
            foreach (string file in Directory.GetFiles(work, "*", SearchOption.AllDirectories))
                File.SetAttributes(file, FileAttributes.Normal);
            Directory.Delete(work, recursive: true);
            foreach (string directory in HiddenRestoreDirectories)
                if (Directory.Exists(directory)) Directory.Delete(directory, recursive: true);
        }
    }

    private static async Task CheckRestore(string work)
    {
        string feed = Path.Combine(work, "feed");
        Directory.CreateDirectory(feed);
        CreatePackage(feed, DependencyId, "1.0.0", "TestDependency", false, false);
        CreatePackage(feed, RootId, "1.0.0", "TestRoot", true, true);
        // A higher version exists, but an exact request must still select 1.0.0.
        CreatePackage(feed, RootId, "1.1.0", "TestRoot", true, true);
        var service = new PackageRestoreService();
        string configFile = Path.Combine(work, "isolated.config");
        WriteConfiguration(configFile, feed);
        var request = new PackageRestoreRequest
        {
            Packages = new[] { new PackageRequirement(RootId, "1.0.0") },
            Sources = new[] { feed },
            PackagesDirectory = Path.Combine(work, "packages"),
            RestoreDirectory = Path.Combine(work, "restore"),
            NuGetConfigFile = configFile
        };
        var first = await service.RestoreAsync(request);
        Check(first.Packages.Count == 2, "Root and transitive dependency were not both restored.");
        Check(!first.UsedCachedRestore && first.InstalledPackageCount == 2, "First restore did not install two packages.");
        var root = first.Packages.Single(p => p.Id == RootId);
        var dependency = first.Packages.Single(p => p.Id == DependencyId);
        Check(root.Version == "1.0.0" && dependency.Version == "1.0.0", "Exact versions were not selected.");
        Check(root.Dependencies.Single().Id == DependencyId, "Dependency edge is missing.");
        Check(root.CompileAssemblies.Single().Replace('\\', '/').EndsWith("/ref/net10.0/TestRoot.dll"),
            "Compile asset did not select the reference assembly.");
        Check(root.RuntimeAssemblies.Single().Replace('\\', '/').EndsWith("/lib/net10.0/TestRoot.dll"),
            "Runtime asset did not select the implementation assembly.");
        Check(dependency.CompileAssemblies.SequenceEqual(dependency.RuntimeAssemblies),
            "lib fallback did not work for the dependency.");
        CheckRuntimeGraph(first);

        var packageFiles = Directory.GetFiles(request.PackagesDirectory, "*", SearchOption.AllDirectories)
            .ToDictionary(path => path, path => (File.GetLastWriteTimeUtc(path), Hash(path)));
        DateTime assetsTime = File.GetLastWriteTimeUtc(first.AssetsFilePath);
        var repeated = await service.RestoreAsync(request);
        Check(repeated.UsedCachedRestore && repeated.InstalledPackageCount == 0,
            "Unchanged restore did not use NuGet's no-op cache.");
        Check(File.GetLastWriteTimeUtc(first.AssetsFilePath) == assetsTime, "No-op restore rewrote assets.");
        foreach (var pair in packageFiles)
            Check(File.GetLastWriteTimeUtc(pair.Key) == pair.Value.Item1 && Hash(pair.Key) == pair.Value.Item2,
                "Repeated restore changed a cached package file.");

        // A different output has no prior assets file: cached packages must suffice without a feed.
        Directory.Move(feed, feed + "-unavailable");
        var offline = await service.RestoreAsync(request with { RestoreDirectory = Path.Combine(work, "offline") });
        Check(offline.Packages.Count == 2 && offline.InstalledPackageCount == 0,
            "Fresh offline restore did not use the installed package cache.");
        Directory.Move(feed + "-unavailable", feed);

        var updated = await service.RestoreAsync(request with
        {
            Packages = new[] { new PackageRequirement(RootId, "1.1.0") }
        });
        Check(updated.Packages.Single(p => p.Id == RootId).Version == "1.1.0",
            "Changing a requirement did not update the graph.");

        try
        {
            await service.RestoreAsync(request with
            {
                Packages = new[] { new PackageRequirement(RootId, "9.9.9") }
            });
            throw new Exception("A nonexistent exact version restored successfully.");
        }
        catch (PackageRestoreException error)
        {
            Check(error.Diagnostics.Any(d => d.Severity == "Error" && d.Code.StartsWith("NU")),
                "Failure did not preserve NuGet diagnostic codes.");
        }
        var recovered = await service.RestoreAsync(request);
        Check(recovered.Packages.Single(p => p.Id == RootId).Version == "1.0.0",
            "A failed restore poisoned the next request.");
        var forced = await service.RestoreAsync(request with { ForceRestore = true });
        Check(!forced.UsedCachedRestore && forced.InstalledPackageCount == 0,
            "Forced restore should reevaluate the graph without reinstalling cached packages.");

        using var canceled = new CancellationTokenSource();
        canceled.Cancel();
        try
        {
            await service.RestoreAsync(request, canceled.Token);
            throw new Exception("Cancellation was ignored.");
        }
        catch (OperationCanceledException) { }
        Check(!Directory.EnumerateFiles(work, "*.csproj", SearchOption.AllDirectories).Any(),
            "Restore generated a C# project.");
    }

    private static async Task CheckConfiguration(string work)
    {
        string parent = Path.Combine(work, "configuration");
        string child = Path.Combine(parent, "student");
        Directory.CreateDirectory(child);
        string config = Path.Combine(parent, "NuGet.Config");
        WriteConfiguration(config, Path.Combine(work, "feed"), "config-packages");
        var service = new PackageRestoreService();
        var request = new PackageRestoreRequest
        {
            Packages = new[] { new PackageRequirement(RootId, "1.0.0") },
            ProgramFilePath = Path.Combine(child, "a.pas"),
            RestoreDirectory = Path.Combine(work, "config-restore")
        };
        string? originalPackages = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        try
        {
            Environment.SetEnvironmentVariable("NUGET_PACKAGES", null);
            var selected = service.GetConfiguration(request);
            Check(Path.TrimEndingDirectorySeparator(selected.PackagesDirectory) == Path.Combine(parent, "config-packages"),
                "Relative globalPackagesFolder was not resolved beside its NuGet.Config.");
            Check(selected.Sources.Single().Name == "intranet" &&
                selected.Sources.Single().Source == Path.Combine(work, "feed"),
                "Inherited source, <clear/> or disabled sources were not respected.");
            Check(selected.ConfigurationFiles.Contains(config), "Configuration provenance is missing.");
            var configured = await service.RestoreAsync(request);
            Check(configured.Packages.Count == 2, "Restore through configured local source failed.");

            string overriddenCache = Path.Combine(work, "environment-packages");
            Environment.SetEnvironmentVariable("NUGET_PACKAGES", overriddenCache);
            Check(service.GetConfiguration(request).PackagesDirectory == overriddenCache,
                "NUGET_PACKAGES did not override NuGet.Config.");
            string explicitCache = Path.Combine(work, "explicit-packages");
            Check(service.GetConfiguration(request with { PackagesDirectory = explicitCache }).PackagesDirectory == explicitCache,
                "An explicit API cache directory did not override the default.");
            Environment.SetEnvironmentVariable("NUGET_PACKAGES", null);

            var hiddenRequest = request with { RestoreDirectory = null };
            var hidden = service.GetConfiguration(hiddenRequest);
            HiddenRestoreDirectories.Add(hidden.RestoreDirectory);
            Check(!hidden.RestoreDirectory.StartsWith(work, StringComparison.OrdinalIgnoreCase),
                "Default restore files must not be stored beside the source.");
            Check(hidden.RestoreDirectory == service.GetConfiguration(hiddenRequest).RestoreDirectory,
                "The per-program cache key is unstable.");
            Check(hidden.RestoreDirectory != service.GetConfiguration(hiddenRequest with
            {
                ProgramFilePath = Path.Combine(parent, "a.pas")
            }).RestoreDirectory, "Same-named programs in different directories share a cache.");
            var hiddenGraph = await service.RestoreAsync(hiddenRequest);
            Check(File.Exists(hiddenGraph.AssetsFilePath), "Hidden per-program restore did not write assets.");
            Check(!Directory.GetFiles(child).Any(), "Restore wrote service files beside the program.");

            var explicitSource = service.GetConfiguration(request with { Sources = new[] { Path.Combine(work, "feed") } });
            Check(explicitSource.Sources.Count == 1, "Explicit sources were not respected.");
            new XDocument(new XElement("configuration", new XElement("packageSources", new XElement("clear"))))
                .Save(config);
            try
            {
                service.GetConfiguration(request);
                throw new Exception("An explicit <clear/> silently re-enabled nuget.org.");
            }
            catch (ArgumentException) { }
        }
        finally { Environment.SetEnvironmentVariable("NUGET_PACKAGES", originalPackages); }
        Console.WriteLine("PASS: NuGet.Config hierarchy, intranet source, disabled sources, NUGET_PACKAGES and hidden per-program cache.");
    }

    private static async Task CheckSharedCache(string work)
    {
        var service = new PackageRestoreService();
        var request = new PackageRestoreRequest
        {
            Packages = new[] { new PackageRequirement("Newtonsoft.Json", "13.0.3") },
            ProgramFilePath = Path.Combine(work, "cached-program.pas"),
            // Deliberately unavailable: an already installed package must suffice.
            Sources = new[] { Path.Combine(work, "unavailable-source") }
        };
        var selected = service.GetConfiguration(request);
        HiddenRestoreDirectories.Add(selected.RestoreDirectory);
        string expectedCache = SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(work,
            configFileName: null, machineWideSettings: new XPlatMachineWideSetting()));
        Check(Path.TrimEndingDirectorySeparator(selected.PackagesDirectory) == Path.TrimEndingDirectorySeparator(expectedCache),
            "The automatic package cache differs from NuGet's standard cache.");
        string packageDirectory = Path.Combine(selected.PackagesDirectory, "newtonsoft.json", "13.0.3");
        Check(File.Exists(Path.Combine(packageDirectory, ".nupkg.metadata")),
            "The optional shared-cache test requires Newtonsoft.Json 13.0.3 to be installed already.");
        var dllFiles = Directory.GetFiles(packageDirectory, "*.dll", SearchOption.AllDirectories)
            .ToDictionary(file => file, file => (File.GetLastWriteTimeUtc(file), Hash(file)));
        var cached = await service.RestoreAsync(request);
        Check(cached.InstalledPackageCount == 0 && cached.Packages.Single().Id == "Newtonsoft.Json",
            "The shared cache was not reused.");
        foreach (var pair in dllFiles)
            Check(File.GetLastWriteTimeUtc(pair.Key) == pair.Value.Item1 && Hash(pair.Key) == pair.Value.Item2,
                "An existing shared-cache DLL was modified.");
        Console.WriteLine("PASS: existing Newtonsoft.Json 13.0.3 reused without a feed: " + selected.PackagesDirectory);

        var downloadRequest = request with
        {
            Packages = new[] { new PackageRequirement("Dapper", "2.1.66") },
            ProgramFilePath = Path.Combine(work, "download-program.pas"),
            Sources = null
        };
        var downloadConfiguration = service.GetConfiguration(downloadRequest);
        HiddenRestoreDirectories.Add(downloadConfiguration.RestoreDirectory);
        Check(downloadConfiguration.Sources.Any(source => source.Source == "https://api.nuget.org/v3/index.json"),
            "The optional online test requires nuget.org among configured sources.");
        string dapperDirectory = Path.Combine(downloadConfiguration.PackagesDirectory, "dapper", "2.1.66");
        bool alreadyInstalled = File.Exists(Path.Combine(dapperDirectory, ".nupkg.metadata"));
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        var downloaded = await service.RestoreAsync(downloadRequest, timeout.Token);
        Check(downloaded.Packages.Single(p => p.Id == "Dapper").Version == "2.1.66", "Dapper restore failed.");
        if (!alreadyInstalled) Check(downloaded.InstalledPackageCount >= 1, "An absent package was not installed.");
        Check(File.Exists(Path.Combine(dapperDirectory, ".nupkg.metadata")), "Download did not populate the shared cache.");
        var repeated = await service.RestoreAsync(downloadRequest);
        Check(repeated.UsedCachedRestore && repeated.InstalledPackageCount == 0, "Repeated online restore did not use the cache.");
        Console.WriteLine(alreadyInstalled
            ? "PASS: Dapper 2.1.66 already cached; repeat restore is a no-op."
            : "PASS: Dapper 2.1.66 downloaded from configured sources into the shared cache; repeat restore is a no-op.");
        // Shared packages are deliberately left installed; only this test's restore files are deleted.
    }

    private static async Task CheckRealTransitivePackage(string work)
    {
        var service = new PackageRestoreService();
        var request = new PackageRestoreRequest
        {
            Packages = new[] { new PackageRequirement("Microsoft.Extensions.Configuration.Json", "10.0.0") },
            ProgramFilePath = Path.Combine(work, "transitive-program.pas")
        };
        var configuration = service.GetConfiguration(request);
        HiddenRestoreDirectories.Add(configuration.RestoreDirectory);
        Check(configuration.Sources.Any(source => source.Source == "https://api.nuget.org/v3/index.json"),
            "The optional online test requires nuget.org among configured sources.");
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
        var graph = await service.RestoreAsync(request, timeout.Token);
        var root = graph.Packages.Single(p => p.Id == "Microsoft.Extensions.Configuration.Json");
        var fileExtensions = graph.Packages.Single(p => p.Id == "Microsoft.Extensions.Configuration.FileExtensions");
        var physicalProvider = graph.Packages.Single(p => p.Id == "Microsoft.Extensions.FileProviders.Physical");
        Check(root.Dependencies.Any(d => d.Id == fileExtensions.Id), "Real package's direct dependency is missing.");
        Check(fileExtensions.Dependencies.Any(d => d.Id == physicalProvider.Id),
            "Real package's second-level dependency is missing.");
        Check(graph.Packages.Count > 3, "The real package graph was truncated.");
        foreach (var package in graph.Packages)
        {
            Check(Directory.Exists(package.Directory), "A resolved package is absent from the cache: " + package.Id);
            Check(package.CompileAssemblies.All(File.Exists) && package.RuntimeAssemblies.All(File.Exists),
                "A selected compile/runtime DLL is absent: " + package.Id);
        }
        var repeated = await service.RestoreAsync(request);
        Check(repeated.UsedCachedRestore && repeated.InstalledPackageCount == 0,
            "Repeated real transitive restore did not use the cache.");
        Console.WriteLine($"PASS: Microsoft.Extensions.Configuration.Json 10.0.0: {graph.Packages.Count} packages, second-level dependency and cached repeat.");
    }

    private static void WriteConfiguration(string path, string feed, string? packagesDirectory = null)
    {
        var configuration = new XElement("configuration",
            new XElement("packageSources", new XElement("clear"),
                new XElement("add", new XAttribute("key", "intranet"), new XAttribute("value", feed)),
                new XElement("add", new XAttribute("key", "disabled"), new XAttribute("value", "https://invalid.example/v3/index.json"))),
            new XElement("disabledPackageSources", new XElement("add", new XAttribute("key", "disabled"), new XAttribute("value", "true"))),
            new XElement("packageSourceMapping", new XElement("clear")));
        if (packagesDirectory is not null)
            configuration.Add(new XElement("config", new XElement("add",
                new XAttribute("key", "globalPackagesFolder"), new XAttribute("value", packagesDirectory))));
        new XDocument(configuration).Save(path);
    }

    private static void CreatePackage(string feed, string id, string version, string assembly,
        bool referenceAssembly, bool dependency)
    {
        var builder = new PackageBuilder
        {
            Id = id,
            Version = NuGetVersion.Parse(version),
            Description = "Isolated PascalABC.NET NuGet restore fixture."
        };
        builder.Authors.Add("PascalABC.NET tests");
        string fixture = Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", assembly + ".dll");
        builder.Files.Add(new PhysicalPackageFile { SourcePath = fixture, TargetPath = "lib/net10.0/" + assembly + ".dll" });
        // Deliberately include a second TFM; restore must select only the net10 assets.
        builder.Files.Add(new PhysicalPackageFile { SourcePath = fixture, TargetPath = "lib/net8.0/" + assembly + ".dll" });
        if (referenceAssembly)
            builder.Files.Add(new PhysicalPackageFile
            {
                SourcePath = Path.Combine(AppContext.BaseDirectory, "fixtures", "ref", assembly + ".dll"),
                TargetPath = "ref/net10.0/" + assembly + ".dll"
            });
        if (dependency)
            builder.DependencyGroups.Add(new PackageDependencyGroup(NuGetFramework.ParseFolder("net10.0"),
                new[] { new PackageDependency(DependencyId, VersionRange.Parse("[1.0.0]")) }));
        using var stream = File.Create(Path.Combine(feed, id + "." + version + ".nupkg"));
        builder.Save(stream);
    }

    private static void CheckRuntimeGraph(ResolvedPackageGraph graph)
    {
        var context = new AssemblyLoadContext("NuGetRestoreFixture", isCollectible: true);
        var paths = graph.Packages.SelectMany(p => p.RuntimeAssemblies)
            .ToDictionary(path => AssemblyName.GetAssemblyName(path).Name!, StringComparer.OrdinalIgnoreCase);
        context.Resolving += (_, name) => paths.TryGetValue(name.Name!, out var path)
            ? LoadFromFile(context, path) : null;
        try
        {
            var root = LoadFromFile(context, paths["TestRoot"]);
            int result = (int)root.GetType("NuGetRestoreFixture.Root")!.GetProperty("Value")!.GetValue(null)!;
            Check(result == 43, "Selected runtime graph could not execute with its transitive dependency.");
        }
        finally { context.Unload(); }
    }

    private static Assembly LoadFromFile(AssemblyLoadContext context, string path)
    {
        using var stream = File.OpenRead(path);
        return context.LoadFromStream(stream);
    }

    private static string Hash(string path)
    {
        using var file = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(file));
    }

    private static async Task CheckRuntimeOnly(string work)
    {
        // Build a genuine runtime-only dotnet layout. No sdk/ directory is copied.
        string runtime = Path.TrimEndingDirectorySeparator(RuntimeEnvironment.GetRuntimeDirectory());
        string version = Path.GetFileName(runtime);
        string dotnetRoot = Directory.GetParent(runtime)!.Parent!.Parent!.FullName;
        string isolated = Path.Combine(work, "runtime-only");
        Directory.CreateDirectory(isolated);
        string executable = OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet";
        string host = Path.Combine(isolated, executable);
        File.Copy(Path.Combine(dotnetRoot, executable), host);
        if (!OperatingSystem.IsWindows())
            File.SetUnixFileMode(host, File.GetUnixFileMode(Path.Combine(dotnetRoot, executable)));
        CopyDirectory(Path.Combine(dotnetRoot, "host", "fxr", version),
            Path.Combine(isolated, "host", "fxr", version));
        CopyDirectory(runtime, Path.Combine(isolated, "shared", "Microsoft.NETCore.App", version));

        var sdks = await RunIsolated(host, new[] { "--list-sdks" });
        Check(sdks.ExitCode == 0 && string.IsNullOrWhiteSpace(sdks.Output),
            "Isolated dotnet unexpectedly found an SDK: " + sdks.Output);
        var test = await RunIsolated(host, new[] { typeof(Program).Assembly.Location, "--runtime-only-child" });
        Check(test.ExitCode == 0, "Runtime-only test failed: " + test.Output + test.Error);
        Console.WriteLine("PASS: the complete restore test ran with an isolated .NET runtime, no SDK and an empty PATH.");
    }

    private static async Task<(int ExitCode, string Output, string Error)> RunIsolated(string host, string[] arguments)
    {
        var start = new ProcessStartInfo(host)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        foreach (string argument in arguments) start.ArgumentList.Add(argument);
        start.Environment["PATH"] = "";
        start.Environment["DOTNET_ROOT"] = Path.GetDirectoryName(host);
        start.Environment["DOTNET_MULTILEVEL_LOOKUP"] = "0";
        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(2)); }
        catch { if (!process.HasExited) process.Kill(entireProcessTree: true); throw; }
        return (process.ExitCode, await output, await error);
    }

    private static void CopyDirectory(string source, string destination)
    {
        Directory.CreateDirectory(destination);
        foreach (string file in Directory.GetFiles(source))
            File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
        foreach (string directory in Directory.GetDirectories(source))
            CopyDirectory(directory, Path.Combine(destination, Path.GetFileName(directory)));
    }

    private static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception(message);
    }
}
