using System.Diagnostics;
using System.Runtime.InteropServices;
using PascalABCNet.LanguageServices;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckMathNetMkl(string work)
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64)
            throw new PlatformNotSupportedException("The Math.NET MKL integration test requires Windows x64.");
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        string directory = Path.Combine(work, "mathnet-mkl");
        Directory.CreateDirectory(directory);
        string cache = Path.Combine(directory, "packages");
        string config = Path.Combine(directory, "NuGet.Config");
        WriteConfiguration(config, "https://api.nuget.org/v3/index.json", cache);
        string main = Path.Combine(directory, "MathNetMkl.pas");
        string source = File.ReadAllText(Path.Combine(repository, "NuGet", "Samples", "MathNetMkl.pas"));
        File.WriteAllText(main, source);
        var request = new PackageRestoreRequest {
            Packages = new[] { new PackageRequirement("MathNet.Numerics.MKL.Win-x64", "3.0.0") },
            ProgramFilePath = main, NuGetConfigFile = config
        };
        var service = new PackageRestoreService();
        HiddenRestoreDirectories.Add(service.GetConfiguration(request).RestoreDirectory);
        Console.WriteLine("Restoring MathNet.Numerics.MKL.Win-x64 3.0.0 into an isolated test cache...");
        var graph = await service.RestoreAsync(request);
        Check(graph.RuntimeIdentifier == "win-x64" && graph.Packages.Any(p => p.Id == "MathNet.Numerics" && p.Version == "5.0.0") &&
            graph.Packages.Any(p => p.Id == "MathNet.Numerics.Providers.MKL" && p.Version == "5.0.0"), "The Math.NET/MKL package graph is incomplete.");
        string[] names = { "libMathNetNumericsMKL.dll", "libiomp5md.dll" };
        var native = graph.Packages.SelectMany(p => p.NativeLibraries).ToArray();
        Check(names.All(name => native.Any(path => Path.GetFileName(path) == name)) &&
            native.All(path => path.Replace('\\', '/').Contains("/runtimes/win-x64/native/")), "MKL or OpenMP was not selected for Windows x64.");
        var cached = await service.RestoreAsync(request);
        Check(cached.UsedCachedRestore && cached.InstalledPackageCount == 0, "Math.NET's unchanged restore was not cached.");
        Console.WriteLine("PASS: Math.NET 5.0.0, MKL provider 5.0.0, both native DLLs and no-op restore.");

        var compiled = await RunCompiler(Path.Combine(kit, "pabcnetcclear.exe"), main, cache);
        Check(compiled.ExitCode == 0, "Math.NET Pascal compilation failed: " + compiled.Output + compiled.Error);
        string executable = Path.ChangeExtension(main, ".exe");
        Check(!names.Any(name => File.Exists(Path.Combine(directory, name))) && !File.Exists(Path.Combine(directory, "MathNet.Numerics.dll")),
            "Compilation copied package DLLs next to the program.");
        static bool Correct(int exit, string output) => exit == 0 && output.Contains("MKL", StringComparison.OrdinalIgnoreCase) &&
            output.Split('\n', StringSplitOptions.RemoveEmptyEntries).Last().Trim() == "19 22 43 50";
        var run = await RunDotnet(executable, directory);
        Check(Correct(run.ExitCode, run.Output), "Native matrix multiplication failed: " + run.Output + run.Error);
        Console.WriteLine("PASS: Pascal uses the MKL provider and multiplies matrices from the NuGet cache: 19 22 43 50.");

        string isolatedHost = CreateRuntimeOnlyHost(work);
        var sdks = await RunIsolated(isolatedHost, new[] { "--list-sdks" });
        Check(sdks.ExitCode == 0 && string.IsNullOrWhiteSpace(sdks.Output), "The isolated MKL host contains an SDK.");
        var isolated = await RunIsolated(isolatedHost, new[] { executable });
        Check(Correct(isolated.ExitCode, isolated.Output), "MKL requires an SDK or PATH: " + isolated.Output + isolated.Error);
        Console.WriteLine("PASS: MKL and its OpenMP dependency work without SDK and with empty PATH.");
        foreach (string name in names)
        {
            string path = native.Single(path => Path.GetFileName(path) == name);
            File.Move(path, path + ".hidden");
            try
            {
                var missing = await RunIsolated(isolatedHost, new[] { executable });
                Check(missing.ExitCode != 0 && (missing.Output + missing.Error).Contains("MKL", StringComparison.OrdinalIgnoreCase),
                    "Missing " + name + " silently fell back to managed math: " + missing.Output + missing.Error);
            }
            finally { File.Move(path + ".hidden", path); }
        }
        var recovered = await RunIsolated(isolatedHost, new[] { executable });
        Check(Correct(recovered.ExitCode, recovered.Output), "MKL did not recover after its libraries were restored.");
        Console.WriteLine("PASS: removal of either MKL or OpenMP fails instead of managed fallback; restoration recovers.");

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var language = new PascalLanguageService("en", Path.Combine(kit, "Lib"));
        string uri = new Uri(main).AbsoluteUri;
        var analysis = await language.OpenOrUpdateDocumentAsync(uri, main, source);
        Check(analysis.IsCompiled, "Math.NET IntelliSense failed: " + analysis.LastConversionError);
        int caret = source.IndexOf("a.Multiply", StringComparison.Ordinal) + "a.".Length;
        Check((await language.GetCompletionAfterDotAsync(uri, caret)).Any(item => item.Label == "Multiply"), "Math.NET completion omitted Multiply.");
        Check((await language.GetHoverAsync(uri, caret + 1))?.Contents.Contains("Multiply") == true, "Math.NET hover failed.");
        int signature = source.IndexOf("a.Multiply(", StringComparison.Ordinal) + "a.Multiply(".Length;
        Check((await language.GetSignatureHelpAsync(uri, signature, '('))?.Signatures.Any(s => s.Label.Contains("Multiply")) == true,
            "Math.NET signature help failed.");
        Check(!Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Any(module => names.Contains(module.ModuleName, StringComparer.OrdinalIgnoreCase)),
            "IntelliSense executed native MKL/OpenMP code in its process.");
        Console.WriteLine("PASS: Math.NET completion, hover and signature help without loading native MKL into IntelliSense.");

        string published = PackagePublisher.Publish(executable, main, kit);
        Check(names.All(name => File.Exists(Path.Combine(published, name))), "Publication omitted MKL or OpenMP.");
        Directory.Move(cache, cache + ".hidden");
        try
        {
            var publication = await RunIsolated(isolatedHost, new[] { Path.Combine(published, "MathNetMkl.exe") });
            Check(Correct(publication.ExitCode, publication.Output), "Published MKL program requires the cache: " + publication.Output + publication.Error);
        }
        finally { Directory.Move(cache + ".hidden", cache); }
        Console.WriteLine("PASS: published Math.NET/MKL program works without cache, SDK or PATH.");
    }
}
