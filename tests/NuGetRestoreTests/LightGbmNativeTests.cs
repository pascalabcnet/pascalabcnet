using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using PascalABCNet.LanguageServices;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckLightGbmNative(string work)
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64)
            throw new PlatformNotSupportedException("The LightGBM integration test requires Windows x64.");
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        string directory = Path.Combine(work, "lightgbm-native");
        Directory.CreateDirectory(directory);
        string cache = Path.Combine(directory, "packages");
        string config = Path.Combine(directory, "NuGet.Config");
        WriteConfiguration(config, "https://api.nuget.org/v3/index.json", cache);
        string main = Path.Combine(directory, "LightGbmRegression.pas");
        string source = File.ReadAllText(Path.Combine(repository, "NuGet", "Samples", "LightGbmRegression.pas"));
        File.WriteAllText(main, source);
        var request = new PackageRestoreRequest {
            Packages = new[] { new PackageRequirement("Microsoft.ML.LightGbm", "5.0.0") },
            ProgramFilePath = main, NuGetConfigFile = config
        };
        var service = new PackageRestoreService();
        HiddenRestoreDirectories.Add(service.GetConfiguration(request).RestoreDirectory);
        Console.WriteLine("Restoring Microsoft.ML.LightGbm 5.0.0 into an isolated test cache...");
        var graph = await service.RestoreAsync(request);
        Check(graph.RuntimeIdentifier == "win-x64" &&
            new[] { "Microsoft.ML.LightGbm", "Microsoft.ML", "Microsoft.ML.FastTree" }.All(id =>
                graph.Packages.Any(p => p.Id == id && p.Version == "5.0.0")) &&
            graph.Packages.Any(p => p.Id == "LightGBM" && p.Version == "4.6.0"), "The LightGBM package graph is incomplete.");
        var native = graph.Packages.SelectMany(p => p.NativeLibraries).ToArray();
        string library = native.Single(path => Path.GetFileName(path) == "lib_lightgbm.dll");
        Check(native.All(path => path.Replace('\\', '/').Contains("/runtimes/win-x64/native/")),
            "Native files for a different platform were selected.");
        Console.WriteLine("Selected native files: " + string.Join(", ", native.Select(Path.GetFileName)));
        var cached = await service.RestoreAsync(request);
        Check(cached.UsedCachedRestore && cached.InstalledPackageCount == 0, "LightGBM's unchanged restore was not cached.");
        var compiled = await RunCompiler(Path.Combine(kit, "pabcnetcclear.exe"), main, cache);
        Check(compiled.ExitCode == 0, "LightGBM Pascal compilation failed: " + compiled.Output + compiled.Error);
        string executable = Path.ChangeExtension(main, ".exe");
        Check(!Directory.GetFiles(directory, "*.dll").Any(), "Compilation copied package DLLs next to the program.");

        static float[] Scores(int exit, string output, string error, string marker, string nativePath)
        {
            Check(exit == 0 && output.Split('\n').Any(line => line.Trim() == marker),
                "LightGBM " + marker + " failed: " + output + error);
            var scores = output.Split('\n').Where(line => line.StartsWith("SCORE=", StringComparison.Ordinal))
                .Select(line => float.Parse(line[6..].Trim(), CultureInfo.InvariantCulture)).ToArray();
            float[] inputs = { -1.255f, 0.505f, 1.755f };
            Check(scores.Length == inputs.Length && scores.Zip(inputs).All(p =>
                float.IsFinite(p.First) && Math.Abs(p.First - p.Second * p.Second) < 0.15f),
                "LightGBM did not learn x*x at unseen inputs: " + output);
            string actual = output.Split('\n').Single(line => line.StartsWith("NATIVE=", StringComparison.Ordinal))[7..].Trim();
            Check(string.Equals(actual, nativePath, StringComparison.OrdinalIgnoreCase),
                "Unexpected loaded native LightGBM (path must be exact): " + output);
            Check(marker != "LOADED" || !output.Contains("TRAINED"), "Loading a saved model trained again.");
            return scores;
        }
        var run = await RunDotnet(executable, directory);
        var expected = Scores(run.ExitCode, run.Output, run.Error, "TRAINED", library);
        Console.WriteLine("PASS: Pascal trains native LightGBM from the exact package path and predicts x*x: " +
            string.Join(", ", expected.Select(score => score.ToString("F3", CultureInfo.InvariantCulture))));

        string host = CreateRuntimeOnlyHost(work);
        var sdks = await RunIsolated(host, new[] { "--list-sdks" });
        Check(sdks.ExitCode == 0 && string.IsNullOrWhiteSpace(sdks.Output), "The isolated LightGBM host contains an SDK.");
        string model = Path.Combine(directory, "lightgbm.zip");
        var saved = await RunIsolated(host, new[] { executable, "--save", model });
        Check(Scores(saved.ExitCode, saved.Output, saved.Error, "TRAINED", library).SequenceEqual(expected) &&
            File.Exists(model) && new FileInfo(model).Length > 0, "Saving the LightGBM model failed.");
        var loaded = await RunIsolated(host, new[] { executable, "--load", model });
        Check(Scores(loaded.ExitCode, loaded.Output, loaded.Error, "LOADED", "").SequenceEqual(expected),
            "The saved model changed predictions in a new process.");
        Console.WriteLine("PASS: native training and managed model loading in new processes without SDK and with empty PATH.");

        File.Move(library, library + ".hidden");
        try
        {
            var missing = await RunIsolated(host, new[] { executable });
            Check(missing.ExitCode != 0 && !missing.Output.Contains("TRAINED") &&
                (missing.Output + missing.Error).Contains("lib_lightgbm", StringComparison.OrdinalIgnoreCase),
                "Training without native LightGBM did not fail clearly: " + missing.Output + missing.Error);
        }
        finally { File.Move(library + ".hidden", library); }
        var recovered = await RunIsolated(host, new[] { executable });
        Check(Scores(recovered.ExitCode, recovered.Output, recovered.Error, "TRAINED", library).SequenceEqual(expected),
            "Native training did not recover after restoring the DLL.");
        Console.WriteLine("PASS: a missing manifest-listed cache DLL is rejected; restoring it recovers training.");

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var language = new PascalLanguageService("en", Path.Combine(kit, "Lib"));
        string uri = new Uri(main).AbsoluteUri;
        var analysis = await language.OpenOrUpdateDocumentAsync(uri, main, source);
        Check(analysis.IsCompiled, "LightGBM IntelliSense failed: " + analysis.LastConversionError);
        int caret = source.IndexOf("trainer.Fit", StringComparison.Ordinal) + "trainer.".Length;
        Check((await language.GetCompletionAfterDotAsync(uri, caret)).Any(item => item.Label == "Fit"), "LightGBM completion omitted Fit.");
        Check((await language.GetHoverAsync(uri, caret + 1))?.Contents.Contains("Fit") == true, "LightGBM hover failed.");
        int signature = source.IndexOf("trainer.Fit(", StringComparison.Ordinal) + "trainer.Fit(".Length;
        Check((await language.GetSignatureHelpAsync(uri, signature, '('))?.Signatures.Any(s => s.Label.Contains("Fit")) == true,
            "LightGBM signature help failed.");
        Check(!Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Any(module => native.Any(path =>
            string.Equals(Path.GetFileName(path), module.ModuleName, StringComparison.OrdinalIgnoreCase))),
            "IntelliSense loaded native ML.NET/LightGBM code into its process.");
        Console.WriteLine("PASS: LightGBM completion, hover and signature help without native execution in IntelliSense.");

        string published = PackagePublisher.Publish(executable, main, kit);
        Check(native.All(path => File.Exists(Path.Combine(published, Path.GetFileName(path)))), "Publication omitted native files.");
        Check(!File.Exists(Path.Combine(published, "LightGbmRegression.deps.json")),
            "Publication retained a cache-bound manifest; missing-DLL test would only check host startup.");
        string publishedModel = Path.Combine(published, "lightgbm.zip");
        File.Copy(model, publishedModel); // Model is user data, not a NuGet asset.
        Directory.Move(cache, cache + ".hidden");
        try
        {
            string publishedExe = Path.Combine(published, "LightGbmRegression.exe");
            var training = await RunIsolated(host, new[] { publishedExe });
            Check(Scores(training.ExitCode, training.Output, training.Error, "TRAINED",
                Path.Combine(published, "lib_lightgbm.dll")).SequenceEqual(expected), "Published training changed predictions.");
            var prediction = await RunIsolated(host, new[] { publishedExe, "--load", publishedModel });
            Check(Scores(prediction.ExitCode, prediction.Output, prediction.Error, "LOADED", "").SequenceEqual(expected),
                "Published model loading changed predictions.");
            // No cache-bound deps file here: prove the trainer itself requires the DLL,
            // not just the .NET host's startup validation of manifest assets.
            string publishedLibrary = Path.Combine(published, "lib_lightgbm.dll");
            File.Move(publishedLibrary, publishedLibrary + ".hidden");
            try
            {
                var missing = await RunIsolated(host, new[] { publishedExe });
                Check(missing.ExitCode != 0 && !missing.Output.Contains("TRAINED") &&
                    (missing.Output + missing.Error).Contains("lib_lightgbm", StringComparison.OrdinalIgnoreCase),
                    "Published training silently worked without native LightGBM: " + missing.Output + missing.Error);
                var managed = await RunIsolated(host, new[] { publishedExe, "--load", publishedModel });
                Check(Scores(managed.ExitCode, managed.Output, managed.Error, "LOADED", "").SequenceEqual(expected),
                    "Saved-tree prediction unexpectedly requires native LightGBM.");
            }
            finally { File.Move(publishedLibrary + ".hidden", publishedLibrary); }
            var recovery = await RunIsolated(host, new[] { publishedExe });
            Check(Scores(recovery.ExitCode, recovery.Output, recovery.Error, "TRAINED", publishedLibrary).SequenceEqual(expected),
                "Published training did not recover after restoring the DLL.");
        }
        finally { Directory.Move(cache + ".hidden", cache); }
        Console.WriteLine("PASS: published native training and saved-model prediction without cache, SDK or PATH.");
        Console.WriteLine("PASS: without the published native DLL only training fails; saved-tree prediction works; restoration recovers training.");
    }
}
