using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using PascalABCNet.LanguageServices;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckMLNetRegression(string work)
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64)
            throw new PlatformNotSupportedException("The ML.NET integration test requires Windows x64.");
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        string directory = Path.Combine(work, "mlnet-regression");
        Directory.CreateDirectory(directory);
        string cache = Path.Combine(directory, "packages");
        string config = Path.Combine(directory, "NuGet.Config");
        WriteConfiguration(config, "https://api.nuget.org/v3/index.json", cache);
        string main = Path.Combine(directory, "MLNetRegression.pas");
        string source = File.ReadAllText(Path.Combine(repository, "NuGet", "Samples", "MLNetRegression.pas"));
        File.WriteAllText(main, source);
        var request = new PackageRestoreRequest {
            Packages = new[] { new PackageRequirement("Microsoft.ML", "5.0.0") },
            ProgramFilePath = main, NuGetConfigFile = config
        };
        var service = new PackageRestoreService();
        HiddenRestoreDirectories.Add(service.GetConfiguration(request).RestoreDirectory);
        Console.WriteLine("Restoring Microsoft.ML 5.0.0 into an isolated test cache...");
        var graph = await service.RestoreAsync(request);
        Check(new[] { "Microsoft.ML", "Microsoft.ML.DataView", "Microsoft.ML.CpuMath" }.All(id =>
            graph.Packages.Any(p => p.Id == id && p.Version == "5.0.0")), "The ML.NET package graph is incomplete.");
        var immutable = graph.Packages.Single(p => p.Id == "System.Collections.Immutable");
        Check(immutable.CompileAssemblies.Count == 0 && immutable.RuntimeAssemblies.Count == 0,
            "ML.NET still brings a conflicting package copy of the core framework assembly.");
        var native = graph.Packages.SelectMany(p => p.NativeLibraries).ToArray();
        Console.WriteLine("Selected ML.NET native files: " + string.Join(", ", native.Select(Path.GetFileName)));
        var cached = await service.RestoreAsync(request);
        Check(cached.UsedCachedRestore && cached.InstalledPackageCount == 0, "ML.NET's unchanged restore was not cached.");
        var compiled = await RunCompiler(Path.Combine(kit, "pabcnetcclear.exe"), main, cache);
        Check(compiled.ExitCode == 0, "ML.NET Pascal compilation failed: " + compiled.Output + compiled.Error);
        string executable = Path.ChangeExtension(main, ".exe");
        Check(!Directory.GetFiles(directory, "*.dll").Any(), "Compilation copied package DLLs next to the program.");

        static float[] Scores(int exit, string output, string error, string marker)
        {
            Check(exit == 0 && output.Split('\n').Any(line => line.Trim() == marker),
                "ML.NET " + marker + " failed: " + output + error);
            var scores = output.Split('\n').Where(line => line.StartsWith("SCORE=", StringComparison.Ordinal))
                .Select(line => float.Parse(line[6..].Trim(), CultureInfo.InvariantCulture)).ToArray();
            float[] expected = { -1.5f, 2f, 4.5f };
            Check(scores.Length == expected.Length && scores.Zip(expected).All(p =>
                float.IsFinite(p.First) && Math.Abs(p.First - p.Second) < 0.1f),
                "Regression did not learn y = 2*x + 1: " + output);
            return scores;
        }
        var run = await RunDotnet(executable, directory);
        var expectedScores = Scores(run.ExitCode, run.Output, run.Error, "TRAINED");
        Console.WriteLine("PASS: Pascal trains SDCA regression and predicts three unseen inputs within tolerance: " +
            string.Join(", ", expectedScores.Select(score => score.ToString("F3", CultureInfo.InvariantCulture))));

        string host = CreateRuntimeOnlyHost(work);
        var sdks = await RunIsolated(host, new[] { "--list-sdks" });
        Check(sdks.ExitCode == 0 && string.IsNullOrWhiteSpace(sdks.Output), "The isolated ML.NET host contains an SDK.");
        string model = Path.Combine(directory, "regression.zip");
        var saved = await RunIsolated(host, new[] { executable, "--save", model });
        Check(Scores(saved.ExitCode, saved.Output, saved.Error, "TRAINED").SequenceEqual(expectedScores) &&
            File.Exists(model) && new FileInfo(model).Length > 0, "Saving the regression model failed.");
        var loaded = await RunIsolated(host, new[] { executable, "--load", model });
        Check(Scores(loaded.ExitCode, loaded.Output, loaded.Error, "LOADED").SequenceEqual(expectedScores) &&
            !loaded.Output.Contains("TRAINED"), "The loaded model changed predictions or trained again.");
        Console.WriteLine("PASS: training, saving and loading in a new process without SDK and with empty PATH.");

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var language = new PascalLanguageService("en", Path.Combine(kit, "Lib"));
        string uri = new Uri(main).AbsoluteUri;
        var analysis = await language.OpenOrUpdateDocumentAsync(uri, main, source);
        Check(analysis.IsCompiled, "ML.NET IntelliSense failed: " + analysis.LastConversionError);
        int caret = source.IndexOf("engine.Predict", StringComparison.Ordinal) + "engine.".Length;
        Check((await language.GetCompletionAfterDotAsync(uri, caret)).Any(item => item.Label == "Predict"), "ML.NET completion omitted Predict.");
        Check((await language.GetHoverAsync(uri, caret + 1))?.Contents.Contains("Predict") == true, "ML.NET hover failed.");
        int signature = source.IndexOf("engine.Predict(", StringComparison.Ordinal) + "engine.Predict(".Length;
        Check((await language.GetSignatureHelpAsync(uri, signature, '('))?.Signatures.Any(s => s.Label.Contains("Predict")) == true,
            "ML.NET signature help failed.");
        Check(!Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Any(module =>
            native.Any(path => string.Equals(Path.GetFileName(path), module.ModuleName, StringComparison.OrdinalIgnoreCase))),
            "IntelliSense executed native ML.NET code in its process.");
        Console.WriteLine("PASS: ML.NET completion, hover and signature help without native execution in IntelliSense.");

        string published = PackagePublisher.Publish(executable, main, kit);
        Check(native.All(path => File.Exists(Path.Combine(published, Path.GetFileName(path)))), "Publication omitted ML.NET native files.");
        // A trained model is user data, not a NuGet asset: copy it explicitly.
        string publishedModel = Path.Combine(published, "regression.zip");
        File.Copy(model, publishedModel);
        Directory.Move(cache, cache + ".hidden");
        try
        {
            string publishedExe = Path.Combine(published, "MLNetRegression.exe");
            var training = await RunIsolated(host, new[] { publishedExe });
            Scores(training.ExitCode, training.Output, training.Error, "TRAINED");
            var prediction = await RunIsolated(host, new[] { publishedExe, "--load", publishedModel });
            Check(Scores(prediction.ExitCode, prediction.Output, prediction.Error, "LOADED").SequenceEqual(expectedScores),
                "Publication changed the saved model's predictions.");
        }
        finally { Directory.Move(cache + ".hidden", cache); }
        Console.WriteLine("PASS: published ML.NET trains and loads the model without NuGet cache, SDK or PATH.");
    }
}
