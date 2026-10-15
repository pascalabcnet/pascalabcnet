using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text.Json;
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
            if (args.Contains("--compiler"))
                await CheckCompilerEarlyRestore(work);
            if (args.Contains("--controller"))
                await CheckControllerRun(work);
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
        Check(first.InstalledPackages.Count == 2 &&
              first.InstalledPackages.Any(p => p.Id == RootId && p.Version == "1.0.0") &&
              first.InstalledPackages.Any(p => p.Id == DependencyId && p.Version == "1.0.0"),
            "Installed package identities did not include the root and transitive dependency.");
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
        Check(repeated.InstalledPackages.Count == 0,
            "No-op restore incorrectly reported a downloaded package.");
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

    private static async Task CheckCompilerEarlyRestore(string work)
    {
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string compiler = Path.Combine(repository, "bin-net10", "pabcnetcclear.exe");
        Check(File.Exists(compiler), "Build pabcnetc_clear/PABCNETCclear.csproj for net10.0 Release before --compiler.");

        string directory = Path.Combine(work, "compiler");
        Directory.CreateDirectory(directory);
        string outputDirectory = Path.Combine(directory, "net10-output");
        string packageCache = Path.Combine(directory, "packages");
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), Path.Combine(work, "feed"), packageCache);
        var service = new PackageRestoreService();

        string success = Path.Combine(directory, "two-packages.pas");
        File.WriteAllText(success, $"{{$package {RootId} 1.0.0}}\n{{$package {DependencyId} 1.0.0}}\nbegin end.\n");
        var successfulRun = await RunCompiler(compiler, success, packageCache);
        Check(successfulRun.ExitCode == 0,
            "Compiler did not restore two packages: " + successfulRun.Output + successfulRun.Error);
        var successRequest = new PackageRestoreRequest
        {
            ProgramFilePath = success,
            Packages = new[] { new PackageRequirement(RootId, "1.0.0"), new PackageRequirement(DependencyId, "1.0.0") }
        };
        string assets = service.GetConfiguration(successRequest).RestoreDirectory;
        HiddenRestoreDirectories.Add(assets);
        string assetsFile = Path.Combine(assets, "project.assets.json");
        Check(File.Exists(assetsFile), "The compiler did not write a hidden per-program restore graph.");
        using (var document = JsonDocument.Parse(File.ReadAllText(assetsFile)))
        {
            var libraries = document.RootElement.GetProperty("libraries");
            Check(libraries.TryGetProperty(RootId + "/1.0.0", out _) &&
                libraries.TryGetProperty(DependencyId + "/1.0.0", out _),
                "The compiler did not restore both package directives in one graph.");
        }
        Check(Directory.Exists(Path.Combine(packageCache, RootId.ToLowerInvariant(), "1.0.0")) &&
            Directory.Exists(Path.Combine(packageCache, DependencyId.ToLowerInvariant(), "1.0.0")),
            "The compiler did not use the configured package cache.");

        string mainWithPackageType = Path.Combine(directory, "package-type-main.pas");
        File.WriteAllText(mainWithPackageType,
            $"{{$package {RootId} 1.0.0}}\nbegin\n  Println(NuGetRestoreFixture.Root.Value);\nend.\n");
        HiddenRestoreDirectories.Add(service.GetConfiguration(successRequest with
        {
            ProgramFilePath = mainWithPackageType,
            Packages = new[] { new PackageRequirement(RootId, "1.0.0") }
        }).RestoreDirectory);
        var mainWithPackageTypeRun = await RunCompiler(compiler, mainWithPackageType, packageCache, outputDirectory);
        Check(mainWithPackageTypeRun.ExitCode == 0,
            "Package type was not visible in the main program: " +
            mainWithPackageTypeRun.Output + mainWithPackageTypeRun.Error);
        string executable = Path.Combine(outputDirectory, "package-type-main.exe");
        string depsFile = Path.ChangeExtension(executable, ".deps.json");
        string runtimeConfiguration = Path.ChangeExtension(executable, ".runtimeconfig.json");
        Check(File.Exists(depsFile), "The compiler did not write a NuGet runtime deps file.");
        using (var deps = JsonDocument.Parse(File.ReadAllText(depsFile)))
        {
            var libraries = deps.RootElement.GetProperty("libraries");
            Check(libraries.TryGetProperty(RootId + "/1.0.0", out _) &&
                libraries.TryGetProperty(DependencyId + "/1.0.0", out _),
                "The runtime manifest omitted a transitive package.");
        }
        using (var configuration = JsonDocument.Parse(File.ReadAllText(runtimeConfiguration)))
            Check(configuration.RootElement.GetProperty("runtimeOptions")
                    .GetProperty("additionalProbingPaths").EnumerateArray()
                    .Any(path => path.GetString() == packageCache),
                "The runtime configuration does not point to the selected NuGet cache.");
        Check(!File.Exists(Path.Combine(directory, "TestRoot.dll")) &&
            !File.Exists(Path.Combine(outputDirectory, "TestRoot.dll")) &&
            !File.Exists(Path.Combine(outputDirectory, "TestDependency.dll")),
            "Compilation copied NuGet DLLs beside the program.");
        var launched = await RunDotnet(executable, directory);
        Check(launched.ExitCode == 0 && launched.Output.Trim() == "43",
            "A package program failed to run from the NuGet cache: " + launched.Output + launched.Error);

        string unit = Path.Combine(directory, "PackageConsumer.pas");
        File.WriteAllText(unit,
            "unit PackageConsumer;\ninterface\nfunction PackageValue: integer;\n" +
            "implementation\nfunction PackageValue: integer;\nbegin\n" +
            "  Result := NuGetRestoreFixture.Root.Value + NuGetRestoreFixture.Dependency.Value;\nend;\nend.\n");
        string mainWithUnit = Path.Combine(directory, "package-type-unit.pas");
        File.WriteAllText(mainWithUnit,
            $"{{$package {RootId} 1.0.0}}\nuses PackageConsumer;\n" +
            "begin\n  Println(PackageValue);\nend.\n");
        HiddenRestoreDirectories.Add(service.GetConfiguration(successRequest with
        {
            ProgramFilePath = mainWithUnit,
            Packages = new[] { new PackageRequirement(RootId, "1.0.0") }
        }).RestoreDirectory);
        var mainWithUnitRun = await RunCompiler(compiler, mainWithUnit, packageCache, outputDirectory);
        Check(mainWithUnitRun.ExitCode == 0,
            "Package type was not visible in the Pascal unit: " +
            mainWithUnitRun.Output + mainWithUnitRun.Error);
        var repeatedUnitRun = await RunCompiler(compiler, mainWithUnit, packageCache, outputDirectory);
        Check(repeatedUnitRun.ExitCode == 0,
            "Repeated compilation (possibly through PCU) lost the package references: " +
            repeatedUnitRun.Output + repeatedUnitRun.Error);
        var unitLaunch = await RunDotnet(Path.Combine(outputDirectory, "package-type-unit.exe"), directory);
        Check(unitLaunch.ExitCode == 0 && unitLaunch.Output.Trim() == "85",
            "A Pascal unit failed to run with its transitive package dependency: " +
            unitLaunch.Output + unitLaunch.Error);

        string secondVersion = Path.Combine(directory, "package-version-11.pas");
        File.WriteAllText(secondVersion,
            $"{{$package {RootId} 1.1.0}}\nbegin\n  Println(NuGetRestoreFixture.Root.Value);\nend.\n");
        HiddenRestoreDirectories.Add(service.GetConfiguration(successRequest with
        {
            ProgramFilePath = secondVersion,
            Packages = new[] { new PackageRequirement(RootId, "1.1.0") }
        }).RestoreDirectory);
        var secondVersionBuild = await RunCompiler(compiler, secondVersion, packageCache, outputDirectory);
        Check(secondVersionBuild.ExitCode == 0,
            "A second program with another package version did not compile: " +
            secondVersionBuild.Output + secondVersionBuild.Error);
        string secondVersionExe = Path.Combine(outputDirectory, "package-version-11.exe");
        using (var secondDeps = JsonDocument.Parse(File.ReadAllText(
                   Path.ChangeExtension(secondVersionExe, ".deps.json"))))
            Check(secondDeps.RootElement.GetProperty("libraries")
                    .TryGetProperty(RootId + "/1.1.0", out _),
                "The second program's runtime manifest selected the wrong package version.");
        var secondVersionLaunch = await RunDotnet(secondVersionExe, directory);
        var originalVersionLaunch = await RunDotnet(Path.Combine(outputDirectory, "package-type-unit.exe"), directory);
        Check(secondVersionLaunch.ExitCode == 0 && secondVersionLaunch.Output.Trim() == "43" &&
            originalVersionLaunch.ExitCode == 0 && originalVersionLaunch.Output.Trim() == "85",
            "Programs with different package versions interfered in one output directory: " +
            secondVersionLaunch.Error + originalVersionLaunch.Error);

        File.WriteAllText(mainWithPackageType, "begin Println(7); end.\n");
        var withoutPackage = await RunCompiler(compiler, mainWithPackageType, packageCache, outputDirectory);
        Check(withoutPackage.ExitCode == 0 && !File.Exists(depsFile),
            "Recompiling without {$package} kept a stale generated deps file: " +
            withoutPackage.Output + withoutPackage.Error);
        var withoutPackageLaunch = await RunDotnet(executable, directory);
        Check(withoutPackageLaunch.ExitCode == 0 && withoutPackageLaunch.Output.Trim() == "7",
            "A program recompiled without packages failed to run: " +
            withoutPackageLaunch.Output + withoutPackageLaunch.Error);

        string conflict = Path.Combine(directory, "conflicting-packages.pas");
        File.WriteAllText(conflict, $"{{$package {RootId} 1.0.0}}\n{{$package {RootId.ToLowerInvariant()} 1.1.0}}\nbegin end.\n");
        var conflictingRun = await RunCompiler(compiler, conflict, packageCache);
        Check(conflictingRun.ExitCode != 0 && conflictingRun.Output.Contains("[2,1]") &&
            conflictingRun.Output.Contains("1.0.0") && conflictingRun.Output.Contains("1.1.0"),
            "A version conflict did not report the second directive: " + conflictingRun.Output + conflictingRun.Error);

        string missing = Path.Combine(directory, "missing-package.pas");
        const string missingId = "PascalABC.Missing.NuGetRestoreTest";
        File.WriteAllText(missing, $"{{$package {missingId} 1.0.0}}\nbegin end.\n");
        HiddenRestoreDirectories.Add(service.GetConfiguration(new PackageRestoreRequest
        {
            ProgramFilePath = missing,
            Packages = new[] { new PackageRequirement(missingId, "1.0.0") }
        }).RestoreDirectory);
        var missingRun = await RunCompiler(compiler, missing, packageCache);
        Check(missingRun.ExitCode != 0 &&
            (missingRun.Output + missingRun.Error).Contains(missingId, StringComparison.OrdinalIgnoreCase),
            "A missing package did not produce a package-specific error: " + missingRun.Output + missingRun.Error);

        Console.WriteLine("PASS: compiler early restore, package types in main and Pascal unit, conflict and missing package.");
    }

    private static async Task<(int ExitCode, string Output, string Error)> RunCompiler(
        string compiler, string source, string packageCache, string? outputDirectory = null)
    {
        var start = new ProcessStartInfo(compiler)
        {
            WorkingDirectory = Path.GetDirectoryName(compiler)!,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        start.ArgumentList.Add(source);
        if (outputDirectory != null) start.ArgumentList.Add(outputDirectory);
        start.Environment["NUGET_PACKAGES"] = packageCache;
        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(45)); }
        catch { if (!process.HasExited) process.Kill(entireProcessTree: true); throw; }
        return (process.ExitCode, await output, await error);
    }

    private static async Task<(int ExitCode, string Output, string Error)> RunDotnet(
        string executable, string workingDirectory)
    {
        var start = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        start.ArgumentList.Add(executable);
        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(45)); }
        catch { if (!process.HasExited) process.Kill(entireProcessTree: true); throw; }
        return (process.ExitCode, await output, await error);
    }

    private static async Task CheckControllerRun(string work)
    {
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string runtime = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_RUNTIME")
            ?? Path.Combine(repository, "bin-net10");
        string controller = Path.Combine(runtime, "PABCCompilerController.dll");
        string worker = Path.Combine(runtime, "PABCCompilerWorker.dll");
        Check(File.Exists(controller) && File.Exists(worker),
            "Build the unified net10 runtime before --controller.");

        string sourceDirectory = Path.Combine(work, "controller");
        string outputDirectory = Path.Combine(sourceDirectory, "net10-output");
        string packageCache = Path.Combine(sourceDirectory, "packages");
        string source = Path.Combine(sourceDirectory, "PackageRun10.pas");
        Directory.CreateDirectory(sourceDirectory);
        WriteConfiguration(Path.Combine(sourceDirectory, "NuGet.Config"),
            Path.Combine(work, "feed"), packageCache);
        File.WriteAllText(source, "begin end.\n");
        var service = new PackageRestoreService();
        HiddenRestoreDirectories.Add(service.GetConfiguration(new PackageRestoreRequest
        {
            ProgramFilePath = source,
            Packages = new[] { new PackageRequirement(RootId, "1.0.0") }
        }).RestoreDirectory);

        var start = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = runtime,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        start.ArgumentList.Add(controller);
        start.ArgumentList.Add(worker);
        start.ArgumentList.Add("0"); // Keep one Worker alive across all compilations.
        start.ArgumentList.Add("0");
        start.Environment["NUGET_PACKAGES"] = packageCache;
        using var process = Process.Start(start) ?? throw new Exception("Controller did not start.");
        var controllerError = process.StandardError.ReadToEndAsync();
        try
        {
            var ping = await SendControllerAsync(process, new { id = 1, command = "ping" });
            Check(ping.GetProperty("success").GetBoolean(), "Controller did not answer ping.");
            int workerPid = ping.GetProperty("workerPid").GetInt32();

            for (int index = 0; index < 3; index++)
            {
                string version = index == 0 ? "1.0.0" : "1.1.0";
                string text = $"{{$package {RootId} {version}}}\n" +
                              "begin Println(NuGetRestoreFixture.Root.Value); end.\n";
                var events = new List<JsonElement>();
                var compiled = await SendControllerAsync(process, new
                {
                    id = index + 2,
                    command = "compile",
                    fileName = source,
                    outputDirectory,
                    runtimeModule = "__RedirectIOMode",
                    emitEvents = true,
                    sourceFiles = new[] { new { fileName = source, text } }
                }, events.Add);
                Check(compiled.GetProperty("success").GetBoolean(),
                    "Worker did not compile the editor snapshot with a package: " + compiled);
                Check(events.Any(value => value.GetProperty("state").GetString() == "NuGetRestoreStarting"),
                    "Worker did not report NuGet restore progress.");
                Check(events.Any(value => value.GetProperty("state").GetString() == "NuGetPackagesInstalled") ==
                      (index < 2), "Worker reported a package download incorrectly.");
                var installedEvents = events.Where(value =>
                    value.GetProperty("state").GetString() == "NuGetPackageInstalled")
                    .Select(value => value.GetProperty("fileName").GetString()).ToArray();
                Check(installedEvents.Length == (index == 0 ? 2 : index == 1 ? 1 : 0),
                    "Worker reported incorrect package identities.");
                if (index == 0)
                    Check(installedEvents.Contains(RootId + " 1.0.0") &&
                          installedEvents.Contains(DependencyId + " 1.0.0"),
                        "Worker omitted a downloaded transitive dependency.");
                Check(compiled.GetProperty("workerPid").GetInt32() == workerPid,
                    "Controller restarted Worker between package versions.");
                string executable = compiled.GetProperty("outputFile").GetString()!;
                Check(Path.GetDirectoryName(executable) == outputDirectory,
                    "Worker ignored the shared Run10 output directory.");
                using (var deps = JsonDocument.Parse(File.ReadAllText(
                           Path.ChangeExtension(executable, ".deps.json"))))
                    Check(deps.RootElement.GetProperty("libraries")
                            .TryGetProperty(RootId + "/" + version, out _),
                        "Worker used a stale package graph after changing the editor snapshot.");
                var run = await RunDotnetRedirected(executable, sourceDirectory);
                Check(run.ExitCode == 0 && run.Output.Trim() == "43",
                    "Run10-style launch failed for package " + version + ": " +
                    run.Output + run.Error);
            }

            // TEST-NET10-PACKAGE-ALC: deleting the installed package twice while
            // one Worker stays alive must not retain the first unloaded context.
            string repeatedText = $"{{$package {RootId} 1.1.0}}\n" +
                                  "uses NuGetRestoreFixture;\nbegin Println(Root.Value); end.\n";
            string installedVersion = Path.Combine(packageCache,
                RootId.ToLowerInvariant(), "1.1.0");
            for (int index = 0; index < 4; index++)
            {
                bool deleted = index == 1 || index == 3;
                if (deleted)
                    Directory.Delete(installedVersion, recursive: true);
                var events = new List<JsonElement>();
                var compiled = await SendControllerAsync(process, new
                {
                    id = index + 5,
                    command = "compile",
                    fileName = source,
                    outputDirectory,
                    runtimeModule = "__RedirectIOMode",
                    emitEvents = true,
                    sourceFiles = new[] { new { fileName = source, text = repeatedText } }
                }, events.Add);
                Check(compiled.GetProperty("success").GetBoolean(),
                    "Worker failed after repeated package-cache deletion: " + compiled);
                Check(compiled.GetProperty("workerPid").GetInt32() == workerPid,
                    "Controller restarted Worker after package-cache deletion.");
                Check(events.Any(value => value.GetProperty("state").GetString() ==
                    "NuGetPackagesInstalled") == deleted,
                    "Worker reported package download incorrectly after cache deletion.");
                Check(events.Count(value => value.GetProperty("state").GetString() ==
                    "NuGetPackageInstalled") == (deleted ? 1 : 0),
                    "Worker did not identify the reinstalled package.");
            }

            var plain = await SendControllerAsync(process, new
            {
                id = 9,
                command = "compile",
                fileName = source,
                outputDirectory,
                runtimeModule = "__RedirectIOMode",
                sourceFiles = new[] { new { fileName = source, text = "begin Println(7); end.\n" } }
            });
            Check(plain.GetProperty("success").GetBoolean(),
                "Worker did not recompile without a package: " + plain);
            string plainExecutable = plain.GetProperty("outputFile").GetString()!;
            Check(!File.Exists(Path.ChangeExtension(plainExecutable, ".deps.json")),
                "Worker retained a stale NuGet runtime manifest.");
            var plainRun = await RunDotnetRedirected(plainExecutable, sourceDirectory);
            Check(plainRun.ExitCode == 0 && plainRun.Output.Trim() == "7",
                "Run10-style launch after removing a package failed: " + plainRun.Error);

            var shutdown = await SendControllerAsync(process, new { id = 10, command = "shutdown" });
            Check(shutdown.GetProperty("success").GetBoolean(), "Controller did not shut down cleanly.");
            await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));
            Check(process.ExitCode == 0, "Controller exited with an error: " + await controllerError);
        }
        finally
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
            }
        }
        Console.WriteLine("PASS: Run10 Controller/Worker compiles editor snapshots and runs two package versions from the shared cache.");
    }

    private static async Task<JsonElement> SendControllerAsync(
        Process process, object request, Action<JsonElement>? progress = null)
    {
        await process.StandardInput.WriteLineAsync(JsonSerializer.Serialize(request));
        await process.StandardInput.FlushAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(90));
        while (true)
        {
            string line = await process.StandardOutput.ReadLineAsync(timeout.Token)
                ?? throw new EndOfStreamException("Controller closed stdout unexpectedly.");
            using var document = JsonDocument.Parse(line);
            var response = document.RootElement.Clone();
            if (!response.TryGetProperty("event", out _)) return response;
            if (progress == null)
                throw new InvalidDataException("Unexpected Controller progress event.");
            progress(response);
        }
    }

    private static async Task<(int ExitCode, string Output, string Error)> RunDotnetRedirected(
        string executable, string workingDirectory)
    {
        var start = new ProcessStartInfo("dotnet")
        {
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        start.ArgumentList.Add(executable);
        start.ArgumentList.Add("[REDIRECTIOMODE]");
        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        await process.StandardInput.WriteLineAsync("GO");
        await process.StandardInput.FlushAsync();
        try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(45)); }
        catch { if (!process.HasExited) process.Kill(entireProcessTree: true); throw; }
        return (process.ExitCode, await output, await error);
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
