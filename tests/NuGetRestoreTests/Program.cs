using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Versioning;
using PascalABCNet.PackageServices;
using PascalABCNet.LanguageServices;

internal static partial class Program
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
            if (args.Contains("--source-defaults-clean-child"))
            {
                Check(!Directory.GetFiles(args[1], "NuGet.Config", SearchOption.AllDirectories).Any(),
                    "The clean profile already contains a NuGet.Config.");
                var configuration = new PackageRestoreService().GetConfiguration(new PackageRestoreRequest {
                    Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = Path.Combine(args[1], "student.pas") });
                Check(configuration.Sources.Count == 1 && configuration.Sources[0].Name == "nuget.org" &&
                    configuration.Sources[0].Source == "https://api.nuget.org/v3/index.json",
                    "A clean profile did not select nuget.org: " + string.Join(", ", configuration.Sources));
                Check(configuration.ConfigurationFiles.All(file => Path.GetFullPath(file).StartsWith(
                    Path.GetFullPath(args[1]) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)),
                    "Clean-profile test read the real user's config: " + string.Join(", ", configuration.ConfigurationFiles));
                Console.WriteLine("PASS: clean profile without a pre-existing NuGet.Config selects nuget.org.");
                return 0;
            }
            if (args.Contains("--source-defaults"))
            {
                await CheckSourceDefaults(work);
                return 0;
            }
            await CheckRestore(work);
            if (args.Contains("--module-intellisense"))
            {
                await CheckModuleIntelliSense(work);
                return 0;
            }
            if (args.Contains("--module-packages"))
            {
                await CheckModulePackages(work);
                return 0;
            }
            if (args.Contains("--package-directives"))
            {
                await CheckPackageDirectiveFixtures(work);
                return 0;
            }
            if (!args.Contains("--runtime-only-child"))
                await CheckLanguageServerProtocol(work);
            await CheckDirectReferenceReload(work);
            await CheckIntelliSense(work);
            await CheckModuleIntelliSense(work, !args.Contains("--runtime-only-child"));
            if (args.Contains("--compiler"))
            {
                await CheckCompilerEarlyRestore(work);
                await CheckModulePackagesInSeparateProcess();
            }
            if (args.Contains("--controller"))
                await CheckControllerRun(work);
            await CheckConfiguration(work);
            await CheckSourceDefaults(work);
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

    private static async Task CheckDirectReferenceReload(string work)
    {
        string directory = Path.Combine(work, "direct-reference");
        Directory.CreateDirectory(directory);
        string assemblyPath = Path.Combine(directory, "TestRoot.dll");
        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestRoot.dll"),
            assemblyPath);
        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestDependency.dll"),
            Path.Combine(directory, "TestDependency.dll"));

        string fileName = Path.Combine(directory, "DirectReference.pas");
        string source = "{$reference 'TestRoot.dll'}\n" +
            "begin\n  Println(NuGetRestoreFixture.Root.Value);\nend.\n";
        File.WriteAllText(fileName, source);
        string documentId = new Uri(fileName).AbsoluteUri;
        int dotOffset = source.IndexOf("Root.Value", StringComparison.Ordinal) + "Root.".Length;
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        var languageService = new PascalLanguageService("en",
            Path.Combine(repository, "bin-net10", "Lib"));

        var analysis = await languageService.OpenOrUpdateDocumentAsync(
            documentId, fileName, source, version: 1);
        Check(analysis.IsCompiled, "Direct-reference IntelliSense analysis failed: " +
            analysis.LastConversionError);
        var original = await languageService.GetCompletionAfterDotAsync(documentId, dotOffset);
        Check(original.Any(item => item.Label == "Value") &&
              !original.Any(item => item.Label == "OnlyInV2"),
            "Direct reference did not expose the original DLL.");

        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "v2", "lib", "TestRoot.dll"),
            assemblyPath, overwrite: true);
        File.SetLastWriteTimeUtc(assemblyPath, DateTime.UtcNow.AddSeconds(3));
        var replaced = await languageService.GetCompletionAfterDotAsync(documentId, dotOffset);
        Check(replaced.Any(item => item.Label == "OnlyInV2"),
            "IntelliSense retained the old direct-reference DLL after replacement without a Pascal edit.");

        var updatedAnalysis = await languageService.OpenOrUpdateDocumentAsync(
            documentId, fileName, source + "\n", version: 2);
        Check(updatedAnalysis.IsCompiled, "Direct-reference IntelliSense reanalysis failed: " +
            updatedAnalysis.LastConversionError);
        var afterEdit = await languageService.GetCompletionAfterDotAsync(documentId, dotOffset);
        Check(afterEdit.Any(item => item.Label == "OnlyInV2"),
            "IntelliSense retained the old direct-reference DLL after a Pascal edit.");

        File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestRoot.dll"),
            assemblyPath, overwrite: true);
        File.SetLastWriteTimeUtc(assemblyPath, DateTime.UtcNow.AddSeconds(6));
        var reverted = await languageService.GetCompletionAfterDotAsync(documentId, dotOffset);
        Check(reverted.Any(item => item.Label == "Value") &&
              !reverted.Any(item => item.Label == "OnlyInV2"),
            "IntelliSense did not return to the original direct-reference DLL.");
        Console.WriteLine("PASS: direct-reference IntelliSense reloads an overwritten DLL without a Pascal edit.");
    }

    private static async Task CheckIntelliSense(string work)
    {
        string directory = Path.Combine(work, "intellisense");
        Directory.CreateDirectory(directory);
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"),
            Path.Combine(work, "feed"), Path.Combine(work, "intellisense-packages"));

        string fileName = Path.Combine(directory, "PackageCompletion.pas");
        string source = $"{{$package {RootId} 1.0.0}}\n" +
            "begin\n  Println(NuGetRestoreFixture.Root.Value);\n" +
            "  Println(NuGetRestoreFixture.Dependency.Value);\n" +
            "  Println(NuGetRestoreFixture.Root.Combine(1, 2));\nend.\n";
        File.WriteAllText(fileName, source);
        var request = new PackageRestoreRequest
        {
            ProgramFilePath = fileName,
            Packages = new[] { new PackageRequirement(RootId, "1.0.0") }
        };
        HiddenRestoreDirectories.Add(new PackageRestoreService()
            .GetConfiguration(request).RestoreDirectory);

        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,
            "../../../../../"));
        var languageService = new PascalLanguageService("en",
            Path.Combine(repository, "bin-net10", "Lib"));
        string documentId = new Uri(fileName).AbsoluteUri;
        var analysis = await languageService.OpenOrUpdateDocumentAsync(
            documentId, fileName, source, version: 1);
        Check(analysis.IsCompiled,
            "IntelliSense could not analyze a package program: " + analysis.LastConversionError);

        int rootDot = source.IndexOf("Root.Value", StringComparison.Ordinal) + "Root.".Length;
        var rootItems = await languageService.GetCompletionAfterDotAsync(documentId, rootDot);
        Check(rootItems.Any(item => item.Label == "Value"),
            "IntelliSense did not expose the root package type's Value property.");

        int dependencyDot = source.IndexOf("Dependency.Value", StringComparison.Ordinal) +
            "Dependency.".Length;
        var dependencyItems = await languageService.GetCompletionAfterDotAsync(
            documentId, dependencyDot);
        Check(dependencyItems.Any(item => item.Label == "Value"),
            "IntelliSense did not expose the transitive package type's Value property.");

        int hoverOffset = source.IndexOf("Root.Value", StringComparison.Ordinal) +
            "Root.".Length + 1;
        var hover = await languageService.GetHoverAsync(documentId, hoverOffset);
        Check(hover?.Contents.Contains("Value", StringComparison.Ordinal) == true,
            "IntelliSense hover did not describe the NuGet package member.");

        int signatureOffset = source.IndexOf("Root.Combine(", StringComparison.Ordinal) +
            "Root.Combine(".Length;
        var signature = await languageService.GetSignatureHelpAsync(
            documentId, signatureOffset, '(');
        Check(signature?.Signatures.Any(item => item.Label.Contains("Combine",
            StringComparison.Ordinal)) == true,
            "IntelliSense did not provide NuGet package method signature help.");

        string withoutPackage = source.Replace($"{{$package {RootId} 1.0.0}}\n", "");
        var withoutPackageAnalysis = await languageService.OpenOrUpdateDocumentAsync(
            documentId, fileName, withoutPackage, version: 2);
        Check(withoutPackageAnalysis.IsCompiled,
            "IntelliSense could not reanalyze the program after removing its package.");
        int withoutPackageDot = withoutPackage.IndexOf("Root.Value", StringComparison.Ordinal) +
            "Root.".Length;
        var withoutPackageItems = await languageService.GetCompletionAfterDotAsync(
            documentId, withoutPackageDot);
        Check(!withoutPackageItems.Any(item => item.Label == "Value"),
            "A removed package remained visible in IntelliSense.");

        string missingVersion = source.Replace("1.0.0}", "9.9.9}");
        var missingAnalysis = await languageService.OpenOrUpdateDocumentAsync(
            documentId, fileName, missingVersion, version: 3);
        Check(!missingAnalysis.IsCompiled &&
            missingAnalysis.LastConversionError?.Contains("NuGet", StringComparison.OrdinalIgnoreCase) == true,
            "A missing package did not produce a diagnostic from IntelliSense.");

        var recoveredAnalysis = await languageService.OpenOrUpdateDocumentAsync(
            documentId, fileName, source, version: 4);
        Check(recoveredAnalysis.IsCompiled,
            "IntelliSense did not recover after a failed package restore: " +
            recoveredAnalysis.LastConversionError);
        string versionTwo = source.Replace("1.0.0}", "1.1.0}");
        var versionTwoAnalysis = await languageService.OpenOrUpdateDocumentAsync(
            documentId, fileName, versionTwo, version: 5);
        Check(versionTwoAnalysis.IsCompiled,
            "IntelliSense could not analyze the updated package: " + versionTwoAnalysis.LastConversionError);
        var versionTwoItems = await languageService.GetCompletionAfterDotAsync(documentId, rootDot);
        Check(versionTwoItems.Any(item => item.Label == "OnlyInV2"),
            "IntelliSense reused the old DLL after changing the package to version 1.1.0.");
        var versionOneAnalysis = await languageService.OpenOrUpdateDocumentAsync(
            documentId, fileName, source, version: 6);
        Check(versionOneAnalysis.IsCompiled,
            "IntelliSense could not switch back to the original package: " + versionOneAnalysis.LastConversionError);
        var versionOneItems = await languageService.GetCompletionAfterDotAsync(documentId, rootDot);
        Check(!versionOneItems.Any(item => item.Label == "OnlyInV2"),
            "IntelliSense retained a member from package version 1.1.0 after switching back.");

        // Two open programs with different versions share the long-lived service.
        // Their DOMs must be released and rebuilt lazily when the active DLL changes.
        string secondFile = Path.Combine(directory, "SecondProgram.pas");
        string secondUri = new Uri(secondFile).AbsoluteUri;
        File.WriteAllText(secondFile, versionTwo);
        HiddenRestoreDirectories.Add(new PackageRestoreService()
            .GetConfiguration(request with { ProgramFilePath = secondFile }).RestoreDirectory);
        var secondAnalysis = await languageService.OpenOrUpdateDocumentAsync(
            secondUri, secondFile, versionTwo, version: 1);
        Check(secondAnalysis.IsCompiled, "A second open program could not use package version 1.1.0.");
        var firstAgain = await languageService.GetCompletionAfterDotAsync(documentId, rootDot);
        Check(firstAgain.Any(item => item.Label == "Value") &&
              !firstAgain.Any(item => item.Label == "OnlyInV2"),
            "The first open program did not lazily recover its package version 1.0.0.");
        var secondAgain = await languageService.GetCompletionAfterDotAsync(secondUri, rootDot);
        Check(secondAgain.Any(item => item.Label == "OnlyInV2"),
            "The second open program did not lazily recover its package version 1.1.0.");

        var anotherService = new PascalLanguageService("en",
            Path.Combine(repository, "bin-net10", "Lib"));
        string thirdFile = Path.Combine(directory, "ThirdProgram.pas");
        string thirdUri = new Uri(thirdFile).AbsoluteUri;
        File.WriteAllText(thirdFile, source);
        HiddenRestoreDirectories.Add(new PackageRestoreService()
            .GetConfiguration(request with { ProgramFilePath = thirdFile }).RestoreDirectory);
        var thirdAnalysis = await anotherService.OpenOrUpdateDocumentAsync(
            thirdUri, thirdFile, source, version: 1);
        Check(thirdAnalysis.IsCompiled, "A second language-service instance could not use package version 1.0.0.");
        var firstServiceAgain = await languageService.GetCompletionAfterDotAsync(secondUri, rootDot);
        Check(firstServiceAgain.Any(item => item.Label == "OnlyInV2"),
            "The first language-service instance retained the other instance's package DLL.");
        var otherServiceAgain = await anotherService.GetCompletionAfterDotAsync(thirdUri, rootDot);
        Check(otherServiceAgain.Any(item => item.Label == "Value") &&
              !otherServiceAgain.Any(item => item.Label == "OnlyInV2"),
            "The second language-service instance could not reload its package DLL.");
        Console.WriteLine("PASS: NuGet IntelliSense, transitive assets and DLL version switching across open documents and services.");
    }

    private static async Task CheckLanguageServerProtocol(string work)
    {
        string directory = Path.Combine(work, "lsp");
        Directory.CreateDirectory(directory);
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"),
            Path.Combine(work, "feed"), Path.Combine(work, "lsp-packages"));
        string fileName = Path.Combine(directory, "PackageLsp.pas");
        string source = $"{{$package {RootId} 1.0.0}}\n" +
            "begin\n  Println(NuGetRestoreFixture.Root.Value);\n" +
            "  Println(NuGetRestoreFixture.Root.Combine(1, 2));\nend.\n";
        File.WriteAllText(fileName, source);
        HiddenRestoreDirectories.Add(new PackageRestoreService()
            .GetConfiguration(new PackageRestoreRequest
            {
                ProgramFilePath = fileName,
                Packages = new[] { new PackageRequirement(RootId, "1.0.0") }
            }).RestoreDirectory);

        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string serverAssembly = Path.Combine(Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR")
            ?? Path.Combine(repository, "bin-net10"), "PascalABCNet.LanguageServer.dll");
        Check(File.Exists(serverAssembly), "Build the net10 runtime distribution before the LSP package test.");
        string runtimeDirectory = Path.TrimEndingDirectorySeparator(RuntimeEnvironment.GetRuntimeDirectory());
        string dotnetRoot = Directory.GetParent(runtimeDirectory)!.Parent!.Parent!.FullName;
        string host = Path.Combine(dotnetRoot, OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        using var server = new Process
        {
            StartInfo = new ProcessStartInfo(host)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = Path.GetDirectoryName(serverAssembly)!
            }
        };
        server.StartInfo.ArgumentList.Add(serverAssembly);
        server.StartInfo.ArgumentList.Add("--stdio");
        Check(server.Start(), "The language server did not start.");
        Task<string> stderr = server.StandardError.ReadToEndAsync();
        try
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(90));
            Stream input = server.StandardInput.BaseStream;
            Stream output = server.StandardOutput.BaseStream;
            string uri = new Uri(fileName).AbsoluteUri;
            await WriteLspAsync(input, new
            {
                jsonrpc = "2.0", id = 1, method = "initialize",
                @params = new { processId = Environment.ProcessId,
                    rootUri = new Uri(directory + Path.DirectorySeparatorChar).AbsoluteUri,
                    capabilities = new { } }
            }, timeout.Token);
            using (await ReadLspResponseAsync(output, 1, timeout.Token)) { }
            await WriteLspAsync(input, new { jsonrpc = "2.0", method = "initialized", @params = new { } }, timeout.Token);

            string directFile = Path.Combine(directory, "DirectReferenceLsp.pas");
            string directAssembly = Path.Combine(directory, "TestRoot.dll");
            File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestRoot.dll"),
                directAssembly);
            File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "lib", "TestDependency.dll"),
                Path.Combine(directory, "TestDependency.dll"));
            string directSource = "{$reference 'TestRoot.dll'}\n" +
                "begin\n  Println(NuGetRestoreFixture.Root.Value);\nend.\n";
            File.WriteAllText(directFile, directSource);
            string directUri = new Uri(directFile).AbsoluteUri;
            int directDot = directSource.IndexOf("Root.Value", StringComparison.Ordinal) +
                "Root.".Length;
            await WriteLspAsync(input, new
            {
                jsonrpc = "2.0", method = "textDocument/didOpen",
                @params = new { textDocument = new
                    { uri = directUri, languageId = "pascal", version = 1, text = directSource } }
            }, timeout.Token);
            using (var originalDirect = await RequestLspPositionAsync(input, output, 10,
                       "textDocument/completion", directUri, directSource, directDot, timeout.Token))
                Check(!originalDirect.RootElement.GetProperty("result").GetProperty("items")
                    .EnumerateArray().Any(item => item.GetProperty("label").GetString() == "OnlyInV2"),
                    "LSP started with the wrong direct-reference DLL version.");

            File.Copy(Path.Combine(AppContext.BaseDirectory, "fixtures", "v2", "lib", "TestRoot.dll"),
                directAssembly, overwrite: true);
            File.SetLastWriteTimeUtc(directAssembly, DateTime.UtcNow.AddSeconds(3));
            using (var updatedDirect = await RequestLspPositionAsync(input, output, 11,
                       "textDocument/completion", directUri, directSource, directDot, timeout.Token))
                Check(updatedDirect.RootElement.GetProperty("result").GetProperty("items")
                    .EnumerateArray().Any(item => item.GetProperty("label").GetString() == "OnlyInV2"),
                    "LSP retained the old direct-reference DLL without a Pascal edit.");
            await WriteLspAsync(input, new
            {
                jsonrpc = "2.0", method = "textDocument/didClose",
                @params = new { textDocument = new { uri = directUri } }
            }, timeout.Token);

            await WriteLspAsync(input, new
            {
                jsonrpc = "2.0", method = "textDocument/didOpen",
                @params = new { textDocument = new { uri, languageId = "pascal", version = 1, text = source } }
            }, timeout.Token);

            int dotOffset = source.IndexOf("Root.Value", StringComparison.Ordinal) + "Root.".Length;
            using var completion = await RequestLspPositionAsync(input, output, 2,
                "textDocument/completion", uri, source, dotOffset, timeout.Token);
            Check(completion.RootElement.GetProperty("result").GetProperty("items")
                .EnumerateArray().Any(item => item.GetProperty("label").GetString() == "Value"),
                "LSP completion did not expose the NuGet package member.");

            using var hover = await RequestLspPositionAsync(input, output, 3,
                "textDocument/hover", uri, source, dotOffset + 1, timeout.Token);
            Check(hover.RootElement.GetProperty("result").GetProperty("contents")
                .GetProperty("value").GetString()?.Contains("Value", StringComparison.Ordinal) == true,
                "LSP hover did not describe the NuGet package member.");

            int signatureOffset = source.IndexOf("Root.Combine(", StringComparison.Ordinal) + "Root.Combine(".Length;
            using var signature = await RequestLspPositionAsync(input, output, 4,
                "textDocument/signatureHelp", uri, source, signatureOffset, timeout.Token);
            Check(signature.RootElement.GetProperty("result").GetProperty("signatures")
                .EnumerateArray().Any(item => item.GetProperty("label").GetString()?
                    .Contains("Combine", StringComparison.Ordinal) == true),
                "LSP signature help did not expose the NuGet package method.");

            string versionTwo = source.Replace("1.0.0}", "1.1.0}");
            await WriteLspAsync(input, new
            {
                jsonrpc = "2.0", method = "textDocument/didChange",
                @params = new { textDocument = new { uri, version = 2 },
                    contentChanges = new[] { new { text = versionTwo } } }
            }, timeout.Token);
            using var updatedCompletion = await RequestLspPositionAsync(input, output, 5,
                "textDocument/completion", uri, versionTwo, dotOffset, timeout.Token);
            Check(updatedCompletion.RootElement.GetProperty("result").GetProperty("items")
                .EnumerateArray().Any(item => item.GetProperty("label").GetString() == "OnlyInV2"),
                "LSP retained the old DLL after switching the package to version 1.1.0.");

            await WriteLspAsync(input, new
            {
                jsonrpc = "2.0", method = "textDocument/didChange",
                @params = new { textDocument = new { uri, version = 3 },
                    contentChanges = new[] { new { text = source } } }
            }, timeout.Token);
            using var revertedCompletion = await RequestLspPositionAsync(input, output, 6,
                "textDocument/completion", uri, source, dotOffset, timeout.Token);
            var revertedItems = revertedCompletion.RootElement.GetProperty("result").GetProperty("items")
                .EnumerateArray().Select(item => item.GetProperty("label").GetString()).ToArray();
            Check(revertedItems.Contains("Value") && !revertedItems.Contains("OnlyInV2"),
                "LSP did not restore the original package DLL after switching back to 1.0.0.");
            Console.WriteLine("PASS: NuGet completion, hover, signature help and DLL version switching over LSP stdio.");
        }
        finally
        {
            if (!server.HasExited) server.Kill(entireProcessTree: true);
            await stderr;
        }
    }

    private static async Task<JsonDocument> RequestLspPositionAsync(Stream input, Stream output,
        int id, string method, string uri, string source, int offset, CancellationToken cancellationToken)
    {
        string before = source[..offset];
        int line = before.Count(ch => ch == '\n');
        int lastBreak = before.LastIndexOf('\n');
        int character = offset - lastBreak - 1;
        await WriteLspAsync(input, new
        {
            jsonrpc = "2.0", id, method,
            @params = new { textDocument = new { uri }, position = new { line, character } }
        }, cancellationToken);
        return await ReadLspResponseAsync(output, id, cancellationToken);
    }

    private static async Task WriteLspAsync(Stream stream, object message, CancellationToken cancellationToken)
    {
        byte[] body = JsonSerializer.SerializeToUtf8Bytes(message);
        byte[] header = Encoding.ASCII.GetBytes($"Content-Length: {body.Length}\r\n\r\n");
        await stream.WriteAsync(header, cancellationToken);
        await stream.WriteAsync(body, cancellationToken);
        await stream.FlushAsync(cancellationToken);
    }

    private static async Task<JsonDocument> ReadLspResponseAsync(Stream stream, int id,
        CancellationToken cancellationToken)
    {
        while (true)
        {
            var header = new List<byte>();
            while (header.Count < 4 || header[^4] != '\r' || header[^3] != '\n' ||
                   header[^2] != '\r' || header[^1] != '\n')
            {
                byte[] one = new byte[1];
                if (await stream.ReadAsync(one, cancellationToken) == 0)
                    throw new EndOfStreamException("LSP server closed stdout before responding.");
                header.Add(one[0]);
            }
            string lengthHeader = Encoding.ASCII.GetString(header.ToArray()).Split("\r\n")
                .Single(line => line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase));
            int length = int.Parse(lengthHeader["Content-Length:".Length..].Trim());
            byte[] body = new byte[length];
            await stream.ReadExactlyAsync(body, cancellationToken);
            var message = JsonDocument.Parse(body);
            if (!message.RootElement.TryGetProperty("id", out var responseId) ||
                responseId.ValueKind != JsonValueKind.Number || responseId.GetInt32() != id)
            {
                message.Dispose();
                continue; // publishDiagnostics and other notifications may arrive first.
            }
            if (message.RootElement.TryGetProperty("error", out var error))
            {
                string description = error.ToString();
                message.Dispose();
                throw new InvalidOperationException("LSP request failed: " + description);
            }
            return message;
        }
    }

    private static async Task CheckCompilerEarlyRestore(string work)
    {
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string compiler = Path.Combine(Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR")
            ?? Path.Combine(repository, "bin-net10"), "pabcnetcclear.exe");
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
        Check(secondVersionLaunch.ExitCode == 0 && secondVersionLaunch.Output.Trim() == "44" &&
            originalVersionLaunch.ExitCode == 0 && originalVersionLaunch.Output.Trim() == "85",
            "Programs with different package versions interfered in one output directory: " +
            secondVersionLaunch.Error + originalVersionLaunch.Error);

        File.WriteAllText(Path.ChangeExtension(secondVersionExe, ".pdb"), "debug symbols fixture");
        string publishedV1 = PackagePublisher.Publish(
            Path.Combine(outputDirectory, "package-type-unit.exe"), mainWithUnit,
            Path.GetDirectoryName(compiler)!);
        string publishedV2 = PackagePublisher.Publish(secondVersionExe, secondVersion,
            Path.GetDirectoryName(compiler)!);
        Check(File.Exists(Path.Combine(publishedV1, "TestRoot.dll")) &&
              File.Exists(Path.Combine(publishedV1, "TestDependency.dll")) &&
              !File.Exists(Path.Combine(publishedV1, "package-type-unit.deps.json")) &&
              File.Exists(Path.Combine(publishedV2, "TestRoot.dll")) &&
              !File.Exists(Path.Combine(publishedV2, "package-version-11.pdb")) &&
              !File.Exists(Path.Combine(publishedV2, ".pascalabcnet-publish.json")),
            "Published folders do not contain the selected runtime dependency closure.");
        string userFile = Path.Combine(publishedV2, "user-notes.txt");
        File.WriteAllText(userFile, "keep me");
        PackagePublisher.Publish(secondVersionExe, secondVersion, Path.GetDirectoryName(compiler)!);
        Check(!File.Exists(userFile) && File.Exists(Path.Combine(publishedV2, "package-version-11.exe")),
            "Re-publishing did not replace the previous published folder.");
        string hiddenCache = packageCache + ".hidden";
        Directory.Move(packageCache, hiddenCache);
        try
        {
            var publishedFirst = await RunDotnet(Path.Combine(publishedV1, "package-type-unit.exe"), directory);
            var publishedSecond = await RunDotnet(Path.Combine(publishedV2, "package-version-11.exe"), directory);
            Check(publishedFirst.ExitCode == 0 && publishedFirst.Output.Trim() == "85" &&
                  publishedSecond.ExitCode == 0 && publishedSecond.Output.Trim() == "44",
                "Published programs still need the NuGet cache: " +
                publishedFirst.Error + publishedSecond.Error);
        }
        finally { Directory.Move(hiddenCache, packageCache); }

        File.WriteAllText(mainWithPackageType, "begin Println(7); end.\n");
        var withoutPackage = await RunCompiler(compiler, mainWithPackageType, packageCache, outputDirectory);
        Check(withoutPackage.ExitCode == 0 && !File.Exists(depsFile),
            "Recompiling without {$package} kept a stale generated deps file: " +
            withoutPackage.Output + withoutPackage.Error);
        var withoutPackageLaunch = await RunDotnet(executable, directory);
        Check(withoutPackageLaunch.ExitCode == 0 && withoutPackageLaunch.Output.Trim() == "7",
            "A program recompiled without packages failed to run: " +
            withoutPackageLaunch.Output + withoutPackageLaunch.Error);
        string plainPublish = PackagePublisher.Publish(executable, mainWithPackageType,
            Path.GetDirectoryName(compiler)!);
        Check(!File.Exists(Path.Combine(plainPublish, "package-type-main.deps.json")) &&
              !File.Exists(Path.Combine(plainPublish, "TestRoot.dll")),
            "Publishing a plain program retained old NuGet assets.");
        var plainPublishedRun = await RunDotnet(Path.Combine(plainPublish, "package-type-main.exe"), directory);
        Check(plainPublishedRun.ExitCode == 0 && plainPublishedRun.Output.Trim() == "7",
            "A plain published program failed to run: " + plainPublishedRun.Error);

        string directAssembly = Path.Combine(repository, "tests", "NuGetRestoreTests",
            "Fixtures", "TestDependency", "bin", "Release", "net10.0", "TestDependency.dll");
        File.Copy(directAssembly, Path.Combine(directory, "TestDependency.dll"));
        string directSource = Path.Combine(directory, "direct-reference.pas");
        File.WriteAllText(directSource,
            "{$reference TestDependency.dll}\nbegin Println(NuGetRestoreFixture.Dependency.Value); end.\n");
        var directBuild = await RunCompiler(compiler, directSource, packageCache, outputDirectory);
        Check(directBuild.ExitCode == 0,
            "A direct DLL reference did not compile: " + directBuild.Output + directBuild.Error);
        string directPublish = PackagePublisher.Publish(
            Path.Combine(outputDirectory, "direct-reference.exe"), directSource,
            Path.GetDirectoryName(compiler)!);
        Check(File.Exists(Path.Combine(directPublish, "TestDependency.dll")),
            "A direct DLL reference was omitted from the published folder.");
        var directPublishedRun = await RunDotnet(Path.Combine(directPublish, "direct-reference.exe"), directory);
        Check(directPublishedRun.ExitCode == 0 && directPublishedRun.Output.Trim() == "42",
            "A published direct DLL reference failed to run: " + directPublishedRun.Error);

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

        await CheckPackageDirectiveFixtures(work);
        Console.WriteLine("PASS: compiler early restore, package types in main and Pascal unit, conflict and missing package.");
    }

    private static async Task CheckPackageDirectiveFixtures(string work)
    {
        // TEST-NET10-PACKAGE-DIRECTIVES: reuse the legacy fixtures, but give their
        // synthetic package IDs real versions from our isolated local feed.
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string compiler = Path.Combine(Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR")
            ?? Path.Combine(repository, "bin-net10"), "pabcnetcclear.exe");
        Check(File.Exists(compiler), "Build the modern compiler before --package-directives.");
        string directory = Path.Combine(work, "package-directives");
        Directory.CreateDirectory(directory);
        string feed = Path.Combine(work, "feed");
        string packageCache = Path.Combine(directory, "packages");
        WriteConfiguration(Path.Combine(directory, "NuGet.Config"), feed, packageCache);
        var service = new PackageRestoreService();

        string AdaptFixture(string relativePath)
        {
            string text = File.ReadAllText(
                Path.Combine(repository, "TestSuite", relativePath.Replace('/', Path.DirectorySeparatorChar)))
            .Replace("Example.Package", RootId, StringComparison.Ordinal)
            .Replace("example.package", RootId.ToLowerInvariant(), StringComparison.Ordinal)
            .Replace("1.2.3", "1.0.0", StringComparison.Ordinal)
            .Replace("2.0.0", "1.1.0", StringComparison.Ordinal)
            .Replace(@"..\units\u_directive_package.pas", "u_directive_package.pas", StringComparison.Ordinal);
            // Legacy fixtures permit directives after uses/unit headers. Modern
            // copies obey the new header-only rule; original files stay untouched.
            string[] lines = text.Split('\n');
            bool IsPackage(string line) => line.TrimStart().StartsWith("{$package ", StringComparison.OrdinalIgnoreCase);
            return string.Join("\n", lines.Where(IsPackage).Concat(lines.Where(line => !IsPackage(line))));
        }

        foreach (string fixture in new[] {
            "directive_package_same_version.pas",
            "usesunits/use_directive_package.pas",
            "errors/err_directive_package_unit_conflict.pas"
        })
        {
            bool conflict = fixture.StartsWith("errors/", StringComparison.Ordinal);
            string caseDirectory = Path.Combine(directory, Path.GetFileNameWithoutExtension(fixture));
            Directory.CreateDirectory(caseDirectory);
            string source = Path.Combine(caseDirectory, Path.GetFileName(fixture));
            File.WriteAllText(source, AdaptFixture(fixture));
            File.WriteAllText(Path.Combine(caseDirectory, "u_directive_package.pas"),
                AdaptFixture("units/u_directive_package.pas"));
            string version = conflict ? "1.1.0" : "1.0.0";
            var request = new PackageRestoreRequest {
                ProgramFilePath = source,
                Packages = new[] { new PackageRequirement(RootId, version) }
            };
            string restoreDirectory = service.GetConfiguration(request).RestoreDirectory;
            HiddenRestoreDirectories.Add(restoreDirectory);
            var result = await RunCompiler(compiler, source, packageCache);
            string diagnostics = result.Output + result.Error;
            if (conflict)
            {
                Check(result.ExitCode != 0 && diagnostics.Contains(RootId) &&
                      diagnostics.Contains("1.0.0") && diagnostics.Contains("1.1.0") &&
                      !diagnostics.Contains("NU1102") && !diagnostics.Contains("NU1101"),
                    "The unit fixture did not report a package-version conflict: " + diagnostics);
            }
            else
            {
                Check(result.ExitCode == 0, "Package directive fixture failed: " + diagnostics);
                var repeated = await RunCompiler(compiler, source, packageCache);
                Check(repeated.ExitCode == 0,
                    "Repeated package directive compilation failed: " + repeated.Output + repeated.Error);
            }

            using var assets = JsonDocument.Parse(File.ReadAllText(Path.Combine(restoreDirectory, "project.assets.json")));
            var libraries = assets.RootElement.GetProperty("libraries");
            Check(libraries.EnumerateObject().Count() == 2 &&
                  libraries.TryGetProperty(RootId + "/" + version, out _) &&
                  libraries.TryGetProperty(DependencyId + "/1.0.0", out _),
                "Directive fixture did not use the expected root and transitive package versions.");
            var sources = assets.RootElement.GetProperty("project").GetProperty("restore")
                .GetProperty("sources").EnumerateObject().ToArray();
            Check(sources.Length == 1 &&
                  string.Equals(Path.GetFullPath(sources[0].Name), Path.GetFullPath(feed),
                      StringComparison.OrdinalIgnoreCase),
                "Directive fixture is not isolated from external NuGet sources.");
            Console.WriteLine("PASS: local-feed package directive fixture " + fixture);
        }
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
                Check(run.ExitCode == 0 && run.Output.Trim() == (version == "1.0.0" ? "43" : "44"),
                    "Run10-style launch failed for package " + version + ": " +
                    run.Output + run.Error);
            }

            var published = await SendControllerAsync(process, new
            {
                id = 11,
                command = "compile",
                fileName = source,
                outputDirectory,
                publish = true,
                sourceFiles = new[] { new { fileName = source,
                    text = $"{{$package {RootId} 1.1.0}}\nbegin Println(NuGetRestoreFixture.Root.Value); end.\n" } }
            });
            Check(published.GetProperty("success").GetBoolean(),
                "Controller/Worker did not publish the editor snapshot: " + published);
            string publishedDirectory = published.GetProperty("outputFile").GetString()!;
            Check(File.Exists(Path.Combine(publishedDirectory, Path.GetFileNameWithoutExtension(source) + ".exe")),
                "Controller/Worker did not report a published folder.");
            string hiddenCache = packageCache + ".hidden";
            Directory.Move(packageCache, hiddenCache);
            try
            {
                var publishedRun = await RunDotnet(
                    Path.Combine(publishedDirectory, Path.GetFileNameWithoutExtension(source) + ".exe"),
                    sourceDirectory);
                Check(publishedRun.ExitCode == 0 && publishedRun.Output.Trim() == "44",
                    "Controller/Worker publication still depends on the NuGet cache: " +
                    publishedRun.Output + publishedRun.Error);
            }
            finally { Directory.Move(hiddenCache, packageCache); }

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

    private static async Task CheckSourceDefaults(string work)
    {
        string directory = Path.Combine(work, "source-defaults");
        Directory.CreateDirectory(directory);
        string config = Path.Combine(directory, "NuGet.Config");
        var request = new PackageRestoreRequest {
            Packages = Array.Empty<PackageRequirement>(), ProgramFilePath = Path.Combine(directory, "a.pas"),
            NuGetConfigFile = config, RestoreDirectory = Path.Combine(directory, "restore")
        };
        var service = new PackageRestoreService();
        foreach (string xml in new[] { "<configuration/>",
            "<configuration><config><add key='globalPackagesFolder' value='packages'/></config></configuration>" })
        {
            File.WriteAllText(config, xml);
            var selected = service.GetConfiguration(request);
            Check(selected.Sources.Count == 1 && selected.Sources[0].Name == "nuget.org" &&
                selected.Sources[0].Source == "https://api.nuget.org/v3/index.json", "Missing sources must default to nuget.org.");
            Check(File.ReadAllText(config) == xml, "Selecting default sources rewrote NuGet.Config.");
        }
        foreach (string xml in new[] {
            "<configuration><packageSources><clear/></packageSources></configuration>",
            "<configuration><packageSources/></configuration>",
            "<configuration><disabledPackageSources><add key='nuget.org' value='true'/></disabledPackageSources></configuration>",
            "<configuration><packageSources><add key='school' value='https://school.invalid/v3/index.json'/></packageSources>" +
                "<disabledPackageSources><add key='school' value='true'/></disabledPackageSources></configuration>" })
        {
            File.WriteAllText(config, xml);
            try { service.GetConfiguration(request); throw new Exception("An explicit source restriction re-enabled nuget.org: " + xml); }
            catch (ArgumentException) { }
        }
        File.WriteAllText(config, "<configuration/>");
        var explicitSource = service.GetConfiguration(request with { Sources = new[] { directory } });
        Check(explicitSource.Sources.Count == 1 && explicitSource.Sources[0].Source == directory,
            "The default source overrode explicitly selected sources.");
        try { service.GetConfiguration(request with { Sources = Array.Empty<string>() }); throw new Exception("An explicit empty source list was ignored."); }
        catch (ArgumentException) { }
        string clean = Path.Combine(work, "clean-profile");
        Directory.CreateDirectory(clean);
        var start = new ProcessStartInfo(Environment.ProcessPath!) {
            UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true, CreateNoWindow = true,
            WorkingDirectory = clean
        };
        if (string.Equals(Path.GetFileNameWithoutExtension(Environment.ProcessPath), "dotnet", StringComparison.OrdinalIgnoreCase))
            start.ArgumentList.Add(typeof(Program).Assembly.Location);
        start.ArgumentList.Add("--source-defaults-clean-child");
        start.ArgumentList.Add(clean);
        foreach (string name in new[] { "APPDATA", "LOCALAPPDATA", "USERPROFILE", "HOME", "DOTNET_CLI_HOME",
            "NUGET_COMMON_APPLICATION_DATA", "ProgramData", "ProgramFiles(x86)", "ProgramFiles" })
        {
            string path = Path.Combine(clean, name.Replace("(", "").Replace(")", ""));
            Directory.CreateDirectory(path);
            start.Environment[name] = path;
        }
        start.Environment["NUGET_PACKAGES"] = Path.Combine(clean, "packages");
        start.Environment["PATH"] = "";
        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync();
        var errors = process.StandardError.ReadToEndAsync();
        try { await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(30)); }
        catch { if (!process.HasExited) process.Kill(entireProcessTree: true); throw; }
        Check(process.ExitCode == 0, "Clean-profile source selection failed: " + await output + await errors);
        Console.Write(await output);
        Console.WriteLine("PASS: default nuget.org, explicit sources, empty/disabled sources, no config rewrites; no network.");
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
        string variant = id == RootId && version == "1.1.0" ? "v2" : "";
        string fixture = Path.Combine(AppContext.BaseDirectory, "fixtures", variant, "lib", assembly + ".dll");
        builder.Files.Add(new PhysicalPackageFile { SourcePath = fixture, TargetPath = "lib/net10.0/" + assembly + ".dll" });
        // Deliberately include a second TFM; restore must select only the net10 assets.
        builder.Files.Add(new PhysicalPackageFile { SourcePath = fixture, TargetPath = "lib/net8.0/" + assembly + ".dll" });
        if (referenceAssembly)
            builder.Files.Add(new PhysicalPackageFile
            {
                SourcePath = Path.Combine(AppContext.BaseDirectory, "fixtures", variant, "ref", assembly + ".dll"),
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
