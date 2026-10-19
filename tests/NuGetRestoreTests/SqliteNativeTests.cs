using System.Runtime.InteropServices;
using System.Text.Json;
using PascalABCNet.LanguageServices;
using PascalABCNet.PackageServices;

internal static partial class Program
{
    private static async Task CheckSqliteNative(string work)
    {
        if (!OperatingSystem.IsWindows() || RuntimeInformation.ProcessArchitecture != Architecture.X64)
            throw new PlatformNotSupportedException("The SQLite native integration test requires Windows x64.");
        string repository = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
        string kit = Environment.GetEnvironmentVariable("PABC_NUGET_TEST_COMPILER_DIR") ?? Path.Combine(repository, "bin-net10");
        string directory = Path.Combine(work, "sqlite-native");
        Directory.CreateDirectory(directory);
        string cache = Path.Combine(directory, "packages");
        string config = Path.Combine(directory, "NuGet.Config");
        WriteConfiguration(config, "https://api.nuget.org/v3/index.json", cache);
        string main = Path.Combine(directory, "SqliteNative.pas");
        string source = File.ReadAllText(Path.Combine(repository, "NuGet", "Samples", "SqliteNative.pas"));
        File.WriteAllText(main, source);
        var request = new PackageRestoreRequest {
            Packages = new[] { new PackageRequirement("Microsoft.Data.Sqlite", "10.0.13") },
            ProgramFilePath = main, NuGetConfigFile = config
        };
        var service = new PackageRestoreService();
        HiddenRestoreDirectories.Add(service.GetConfiguration(request).RestoreDirectory);
        Console.WriteLine("Restoring Microsoft.Data.Sqlite 10.0.13 for win-x64 from nuget.org into an isolated test cache...");
        var graph = await service.RestoreAsync(request);
        var native = graph.Packages.SelectMany(p => p.NativeLibraries).ToArray();
        Check(graph.RuntimeIdentifier == "win-x64" && native.Any(path => Path.GetFileName(path) == "e_sqlite3.dll"),
            "SQLite's Windows x64 native library was not selected.");
        Check(native.All(path => path.Replace('\\', '/').Contains("/runtimes/win-x64/native/")),
            "The SQLite graph contains native files for another platform.");
        var cached = await service.RestoreAsync(request);
        Check(cached.UsedCachedRestore && cached.InstalledPackageCount == 0, "SQLite was downloaded again on a no-op restore.");
        Console.WriteLine("PASS: real SQLite graph, Windows x64 native assets and no-op restore.");

        var compiled = await RunCompiler(Path.Combine(kit, "pabcnetcclear.exe"), main, cache);
        Check(compiled.ExitCode == 0, "SQLite Pascal compilation failed: " + compiled.Output + compiled.Error);
        string executable = Path.ChangeExtension(main, ".exe");
        Check(!File.Exists(Path.Combine(directory, "e_sqlite3.dll")) &&
            !File.Exists(Path.Combine(directory, "Microsoft.Data.Sqlite.dll")), "Compilation copied package DLLs next to the program.");
        var run = await RunDotnet(executable, directory);
        Check(run.ExitCode == 0 && run.Output.Trim() == "42", "SQLite native query from the cache failed: " + run.Output + run.Error);
        Console.WriteLine("PASS: compiled Pascal SQLite program executed native SQL from the NuGet cache.");

        string sqliteLibrary = native.Single(path => Path.GetFileName(path) == "e_sqlite3.dll");
        File.Move(sqliteLibrary, sqliteLibrary + ".hidden");
        try
        {
            var missing = await RunDotnet(executable, directory);
            Check(missing.ExitCode != 0 && (missing.Output + missing.Error).Contains("e_sqlite3", StringComparison.OrdinalIgnoreCase),
                "Missing native SQLite did not fail with a diagnostic identifying e_sqlite3.");
        }
        finally { File.Move(sqliteLibrary + ".hidden", sqliteLibrary); }
        var recovered = await RunDotnet(executable, directory);
        Check(recovered.ExitCode == 0 && recovered.Output.Trim() == "42", "SQLite did not recover after restoring its native library.");
        Console.WriteLine("PASS: missing native SQLite is diagnosed; restoring the DLL restores execution.");

        string isolatedHost = CreateRuntimeOnlyHost(work);
        var sdks = await RunIsolated(isolatedHost, new[] { "--list-sdks" });
        Check(sdks.ExitCode == 0 && string.IsNullOrWhiteSpace(sdks.Output), "SQLite's isolated host contains an SDK.");
        var isolatedRun = await RunIsolated(isolatedHost, new[] { executable });
        Check(isolatedRun.ExitCode == 0 && isolatedRun.Output.Trim() == "42",
            "Native SQLite requires more than the runtime: " + isolatedRun.Output + isolatedRun.Error);
        Console.WriteLine("PASS: native SQLite query executes with an isolated runtime, no SDK and empty PATH.");

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var language = new PascalLanguageService("en", Path.Combine(kit, "Lib"));
        string uri = new Uri(main).AbsoluteUri;
        var analysis = await language.OpenOrUpdateDocumentAsync(uri, main, source);
        Check(analysis.IsCompiled, "SQLite IntelliSense failed: " + analysis.LastConversionError);
        int caret = source.IndexOf("connection.Open", StringComparison.Ordinal) + "connection.".Length;
        Check((await language.GetCompletionAfterDotAsync(uri, caret)).Any(item => item.Label == "Open"), "SQLite completion omitted Open.");
        Check((await language.GetHoverAsync(uri, caret + 1))?.Contents.Contains("Open") == true, "SQLite hover failed.");
        Check(!System.Diagnostics.Process.GetCurrentProcess().Modules.Cast<System.Diagnostics.ProcessModule>()
            .Any(module => string.Equals(module.ModuleName, "e_sqlite3.dll", StringComparison.OrdinalIgnoreCase)),
            "IntelliSense loaded SQLite native code into its own process.");
        Console.WriteLine("PASS: SQLite completion and hover without executing native code inside IntelliSense.");

        string published = PackagePublisher.Publish(executable, main, kit);
        Check(File.Exists(Path.Combine(published, "e_sqlite3.dll")), "SQLite publication omitted e_sqlite3.dll.");
        Directory.Move(cache, cache + ".hidden");
        try
        {
            var publishedRun = await RunDotnet(Path.Combine(published, "SqliteNative.exe"), published);
            Check(publishedRun.ExitCode == 0 && publishedRun.Output.Trim() == "42",
                "Published SQLite program depended on the NuGet cache: " + publishedRun.Output + publishedRun.Error);
        }
        finally { Directory.Move(cache + ".hidden", cache); }
        Console.WriteLine("PASS: SQLite publication executes native SQL with the private NuGet cache unavailable.");
    }
}
