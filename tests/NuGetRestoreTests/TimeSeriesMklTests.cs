using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using PascalABCNet.LanguageServices;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckTimeSeriesMkl(string work)
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64)
            throw new PlatformNotSupportedException("The TimeSeries/MKL integration test requires Windows x64.");
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        string directory = Path.Combine(work, "timeseries-mkl");
        Directory.CreateDirectory(directory);
        string cache = Path.Combine(directory, "packages");
        string config = Path.Combine(directory, "NuGet.Config");
        WriteConfiguration(config, "https://api.nuget.org/v3/index.json", cache);
        string main = Path.Combine(directory, "TimeSeriesMkl.pas");
        string source = File.ReadAllText(Path.Combine(repository, "NuGet", "Samples", "TimeSeriesMkl.pas"));
        File.WriteAllText(main, source);
        var request = new PackageRestoreRequest {
            Packages = new[] { new PackageRequirement("Microsoft.ML.TimeSeries", "5.0.0") },
            ProgramFilePath = main, NuGetConfigFile = config
        };
        var service = new PackageRestoreService();
        HiddenRestoreDirectories.Add(service.GetConfiguration(request).RestoreDirectory);
        Console.WriteLine("Restoring Microsoft.ML.TimeSeries 5.0.0 into an isolated test cache...");
        var graph = await service.RestoreAsync(request);
        Check(graph.RuntimeIdentifier == "win-x64" &&
            new[] { "Microsoft.ML.TimeSeries", "Microsoft.ML", "Microsoft.ML.Mkl.Redist" }.All(id =>
                graph.Packages.Any(p => p.Id == id && p.Version == "5.0.0")), "The TimeSeries/MKL package graph is incomplete.");
        var native = graph.Packages.SelectMany(p => p.NativeLibraries).ToArray();
        var mkl = native.Where(path => Path.GetFileName(path).Contains("mkl", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileName(path).Equals("libiomp5md.dll", StringComparison.OrdinalIgnoreCase)).ToArray();
        Check(mkl.Length == 3 && native.All(path => path.Replace('\\', '/').Contains("/runtimes/win-x64/native/")),
            "MKL was not selected for Windows x64.");
        Console.WriteLine("Selected native files: " + string.Join(", ", native.Select(Path.GetFileName)));
        var cached = await service.RestoreAsync(request);
        Check(cached.UsedCachedRestore && cached.InstalledPackageCount == 0, "TimeSeries' unchanged restore was not cached.");
        var compiled = await RunCompiler(Path.Combine(kit, "pabcnetcclear.exe"), main, cache);
        Check(compiled.ExitCode == 0, "TimeSeries Pascal compilation failed: " + compiled.Output + compiled.Error);
        string executable = Path.ChangeExtension(main, ".exe");
        Check(!Directory.GetFiles(directory, "*.dll").Any(), "Compilation copied package DLLs next to the program.");

        static float[] Forecast(int exit, string output, string error, string marker)
        {
            Check(exit == 0 && output.Split('\n').Any(line => line.Trim() == marker),
                "TimeSeries " + marker + " failed: " + output + error);
            var values = output.Split('\n').Where(line => line.StartsWith("FORECAST=", StringComparison.Ordinal))
                .Select(line => float.Parse(line[9..].Trim(), CultureInfo.InvariantCulture)).ToArray();
            Check(values.Length == 6 && values.Select((value, index) => float.IsFinite(value) &&
                Math.Abs(value - (10 + 3 * Math.Sin(2 * Math.PI * index / 12))) < 0.2).All(ok => ok),
                "SSA did not forecast six held-out seasonal observations: " + output);
            Check(marker != "LOADED" || !output.Contains("TRAINED"), "Loading the checkpoint trained again.");
            return values;
        }
        static string[] NativePaths(string output) => output.Split('\n')
            .Where(line => line.StartsWith("NATIVE=", StringComparison.Ordinal)).Select(line => line[7..].Trim()).ToArray();
        void CheckNativePaths(string output, string[] expectedPaths, bool required)
        {
            var paths = NativePaths(output);
            Check((!required || expectedPaths.All(path => paths.Contains(path, StringComparer.OrdinalIgnoreCase))) &&
                paths.All(path => expectedPaths.Contains(path, StringComparer.OrdinalIgnoreCase)),
                "MKL was missing or loaded from an unexpected location: " + output);
        }
        var run = await RunDotnet(executable, directory);
        var expected = Forecast(run.ExitCode, run.Output, run.Error, "TRAINED");
        CheckNativePaths(run.Output, mkl, required: true);
        var loadedNative = NativePaths(run.Output);
        Console.WriteLine("PASS: Pascal trains SSA using MKL from exact package paths; forecast: " +
            string.Join(", ", expected.Select(value => value.ToString("F3", CultureInfo.InvariantCulture))));
        Console.WriteLine("Loaded native MKL files: " + string.Join(", ", loadedNative.Select(Path.GetFileName)));

        string host = CreateRuntimeOnlyHost(work);
        var sdks = await RunIsolated(host, new[] { "--list-sdks" });
        Check(sdks.ExitCode == 0 && string.IsNullOrWhiteSpace(sdks.Output), "The isolated TimeSeries host contains an SDK.");
        string model = Path.Combine(directory, "timeseries.zip");
        var saved = await RunIsolated(host, new[] { executable, "--save", model });
        Check(Forecast(saved.ExitCode, saved.Output, saved.Error, "TRAINED").SequenceEqual(expected) &&
            File.Exists(model) && new FileInfo(model).Length > 0, "Saving the SSA checkpoint failed.");
        CheckNativePaths(saved.Output, mkl, required: true);
        var loaded = await RunIsolated(host, new[] { executable, "--load", model });
        Check(Forecast(loaded.ExitCode, loaded.Output, loaded.Error, "LOADED").SequenceEqual(expected),
            "Reloading the checkpoint changed the forecast.");
        CheckNativePaths(loaded.Output, mkl, required: false);
        Check(NativePaths(loaded.Output).Length == 0, "Checkpoint prediction unexpectedly loaded MKL: " + loaded.Output);
        Console.WriteLine("PASS: SSA checkpoint and identical forecast in new processes without SDK and with empty PATH.");

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var language = new PascalLanguageService("en", Path.Combine(kit, "Lib"));
        string uri = new Uri(main).AbsoluteUri;
        var analysis = await language.OpenOrUpdateDocumentAsync(uri, main, source);
        Check(analysis.IsCompiled, "TimeSeries IntelliSense failed: " + analysis.LastConversionError);
        int caret = source.IndexOf("engine.CheckPoint", StringComparison.Ordinal) + "engine.".Length;
        Check((await language.GetCompletionAfterDotAsync(uri, caret)).Any(item => item.Label == "CheckPoint"),
            "TimeSeries completion omitted CheckPoint.");
        Check((await language.GetHoverAsync(uri, caret + 1))?.Contents.Contains("CheckPoint") == true, "TimeSeries hover failed.");
        int signature = source.IndexOf("engine.CheckPoint(", StringComparison.Ordinal) + "engine.CheckPoint(".Length;
        Check((await language.GetSignatureHelpAsync(uri, signature, '('))?.Signatures.Any(s => s.Label.Contains("CheckPoint")) == true,
            "TimeSeries signature help failed.");
        Check(!Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Any(module => native.Any(path =>
            string.Equals(Path.GetFileName(path), module.ModuleName, StringComparison.OrdinalIgnoreCase))),
            "IntelliSense loaded native MKL into its process.");
        Console.WriteLine("PASS: TimeSeries completion, hover and signature help without native execution in IntelliSense.");

        string published = PackagePublisher.Publish(executable, main, kit);
        Check(native.All(path => File.Exists(Path.Combine(published, Path.GetFileName(path)))), "Publication omitted native files.");
        Check(!File.Exists(Path.Combine(published, "TimeSeriesMkl.deps.json")), "Publication retained a cache-bound manifest.");
        string publishedModel = Path.Combine(published, "timeseries.zip");
        File.Copy(model, publishedModel); // Checkpoint is user data, not a NuGet asset.
        Directory.Move(cache, cache + ".hidden");
        try
        {
            string publishedExe = Path.Combine(published, "TimeSeriesMkl.exe");
            var training = await RunIsolated(host, new[] { publishedExe });
            Check(Forecast(training.ExitCode, training.Output, training.Error, "TRAINED").SequenceEqual(expected),
                "Published SSA training changed the forecast.");
            var publishedNative = mkl.Select(path => Path.Combine(published, Path.GetFileName(path))).ToArray();
            CheckNativePaths(training.Output, publishedNative, required: true);
            var prediction = await RunIsolated(host, new[] { publishedExe, "--load", publishedModel });
            Check(Forecast(prediction.ExitCode, prediction.Output, prediction.Error, "LOADED").SequenceEqual(expected),
                "Published checkpoint changed the forecast.");
            CheckNativePaths(prediction.Output, publishedNative, required: false);
            foreach (string path in loadedNative)
            {
                string missingPath = Path.Combine(published, Path.GetFileName(path));
                File.Move(missingPath, missingPath + ".hidden");
                try
                {
                    var missing = await RunIsolated(host, new[] { publishedExe });
                    Check(missing.ExitCode != 0 && !missing.Output.Contains("TRAINED") &&
                        (missing.Output + missing.Error).Contains("mkl", StringComparison.OrdinalIgnoreCase),
                        "SSA silently trained without " + Path.GetFileName(path) + ": " + missing.Output + missing.Error);
                    var withoutNative = await RunIsolated(host, new[] { publishedExe, "--load", publishedModel });
                    Check(Forecast(withoutNative.ExitCode, withoutNative.Output, withoutNative.Error, "LOADED").SequenceEqual(expected) &&
                        NativePaths(withoutNative.Output).Length == 0,
                        "Saved SSA prediction unexpectedly requires " + Path.GetFileName(path));
                }
                finally { File.Move(missingPath + ".hidden", missingPath); }
            }
            var recovery = await RunIsolated(host, new[] { publishedExe });
            Check(Forecast(recovery.ExitCode, recovery.Output, recovery.Error, "TRAINED").SequenceEqual(expected),
                "SSA training did not recover after restoring MKL.");
            CheckNativePaths(recovery.Output, publishedNative, required: true);
        }
        finally { Directory.Move(cache + ".hidden", cache); }
        Console.WriteLine("PASS: published SSA trains and reloads without cache/SDK/PATH; removing each MKL DLL stops training but not checkpoint prediction; restoration recovers.");
    }
}
