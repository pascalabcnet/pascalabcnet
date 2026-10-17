using System;
using System.IO;
using System.Threading.Tasks;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static async Task CheckOutputCleanupRuns(string runtime)
    {
        string root = Path.Combine(Path.GetTempPath(), "pabc-output-cleanup-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        string source = Path.Combine(root, "main.pas");
        try
        {
            using (var compiler = new Net10ControllerClient(runtime, "dotnet"))
                foreach (string outputRoot in new[] { null, Path.Combine(root, "Selected output") })
                foreach (string scenario in new[] { "normal", "stop", "exception" })
                {
                    string directory = Net10OutputCleanup.GetDirectory(source, outputRoot);
                    string text = scenario == "normal" ? "begin Print(123); end." :
                        scenario == "stop" ? "begin var s := ReadString; end." :
                        "begin raise new System.Exception('cleanup test'); end.";
                    File.WriteAllText(source, text);
                    var response = await compiler.CompileAsync(source, directory, "__RedirectIOMode");
                    Check(response.success, "cleanup fixture compiles: " + scenario + ": " + response.message);
                    Check(string.Equals(Path.GetDirectoryName(Path.GetFullPath(response.outputFile)), directory,
                        StringComparison.OrdinalIgnoreCase), "real compiler uses selected output location");
                    RuntimeExceptionInfo error = null;
                    using (var runner = new Net10ProgramRunner("dotnet", response.outputFile, root, ""))
                    {
                        var running = runner.RunAsync(s => { }, () => runner.Stop(),
                            () => Check(Directory.Exists(directory), "output exists while process starts: " + scenario),
                            e => error = e);
                        if (await Task.WhenAny(running, Task.Delay(30000)) != running)
                        {
                            runner.Stop();
                            throw new TimeoutException("Cleanup run timed out: " + scenario);
                        }
                        int exitCode = await running;
                        Check(scenario != "normal" || exitCode == 0, "normal cleanup fixture exits successfully");
                        Check(scenario != "stop" || runner.WasStopped, "Stop completes before cleanup");
                        Check(scenario != "exception" || error != null, "runtime exception is delivered before cleanup");
                    }
                    Check(Net10OutputCleanup.TryDelete(source, response.outputFile, true, true, outputRoot) &&
                        !Directory.Exists(directory) && File.ReadAllText(source) == text,
                        "real process output is removed and source preserved: " + scenario);
                    Check(outputRoot == null || Directory.Exists(outputRoot), "selected parent output directory is preserved");
                }

            // An IDE-built library must survive deletion of the executable's
            // disposable output directory and remain usable by {$reference}.
            using (var compiler = new Net10ControllerClient(runtime, "dotnet"))
            foreach (bool configured in new[] { false, true })
            {
                string sourceDirectory = Path.Combine(root, configured ? "selected-case" : "default-case");
                Directory.CreateDirectory(sourceDirectory);
                string libraryDirectory = configured ? Path.Combine(root, "selected-libraries") : sourceDirectory;
                string outputRoot = configured ? libraryDirectory : null;
                string librarySource = Path.Combine(sourceDirectory, "MyLib.pas");
                string mainSource = Path.Combine(sourceDirectory, "main.pas");
                string libraryText = "library MyLib;\nprocedure ppp;\nbegin\n  Println(123);\nend;\nend.";
                File.WriteAllText(librarySource, libraryText);
                string outputDirectory = Net10OutputCleanup.GetDirectory(mainSource, outputRoot);
                var library = await compiler.CompileAsync(librarySource, outputDirectory,
                    libraryOutputDirectory: libraryDirectory);
                Check(library.success && Path.GetFullPath(library.outputFile) ==
                    Path.Combine(libraryDirectory, "MyLib.dll") && File.Exists(library.outputFile),
                    "Run10 library output stays outside disposable net10-output: " + library.message);
                Check(!File.Exists(Path.Combine(libraryDirectory, "MyLib.runtimeconfig.json")) &&
                    !File.Exists(Path.Combine(libraryDirectory, "MyLib.pdb")),
                    "release library creates neither runtime configuration nor debug symbols");

                string debugSource = Path.Combine(sourceDirectory, "DebugLib.pas");
                File.WriteAllText(debugSource, libraryText.Replace("MyLib", "DebugLib"));
                var debugLibrary = await compiler.CompileAsync(debugSource, outputDirectory,
                    libraryOutputDirectory: libraryDirectory, debug: true);
                Check(debugLibrary.success && File.Exists(Path.Combine(libraryDirectory, "DebugLib.dll")) &&
                    File.Exists(Path.Combine(libraryDirectory, "DebugLib.pdb")) &&
                    !File.Exists(Path.Combine(libraryDirectory, "DebugLib.runtimeconfig.json")),
                    "debug library creates PDB but no runtime configuration: " + debugLibrary.message);

                string mainText = "{$reference MyLib.dll} begin ppp; end.";
                File.WriteAllText(mainSource, mainText);
                for (int attempt = 0; attempt < 2; attempt++)
                {
                    var main = await compiler.CompileAsync(mainSource, outputDirectory, "__RedirectIOMode",
                        libraryOutputDirectory: libraryDirectory);
                    Check(main.success && File.Exists(Path.Combine(outputDirectory, "MyLib.dll")),
                        "main resolves and copies the persistent library: " + main.message);
                    Check(File.Exists(Path.Combine(outputDirectory, "main.runtimeconfig.json")),
                        "executable retains the required runtime configuration");
                    var output = new System.Text.StringBuilder();
                    using (var runner = new Net10ProgramRunner("dotnet", main.outputFile, sourceDirectory, ""))
                    {
                        int exitCode = await runner.RunAsync(text => output.Append(text), () => { });
                        Check(exitCode == 0 && output.ToString().Contains("123"),
                            "Run10 launches after copying the reference: exit=" + exitCode + ", output=" + output);
                    }
                    Check(Net10OutputCleanup.TryDelete(mainSource, main.outputFile, true, true, outputRoot) &&
                        File.Exists(library.outputFile) && !Directory.Exists(outputDirectory),
                        "Run10 cleanup removes only the disposable output and preserves the source library");
                }
            }
        }
        finally { DeleteOutputCleanupFixture(root); }
    }

    private static void CheckOutputCleanup()
    {
        string root = Path.Combine(Path.GetTempPath(), "pabc-output-cleanup-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        string source = Path.Combine(root, "main.pas");
        string directory = Path.Combine(root, "net10-output");
        string output = Path.Combine(directory, "main.exe");
        File.WriteAllText(source, "begin end.");
        try
        {
            Directory.CreateDirectory(Path.Combine(directory, "nested"));
            File.WriteAllText(output, "generated");
            File.WriteAllText(Path.Combine(directory, "main.pdb"), "symbols");
            File.WriteAllText(Path.Combine(directory, "main.runtimeconfig.json"), "{}");
            File.WriteAllText(Path.Combine(directory, "other.pcu"), "generated module");
            File.WriteAllText(Path.Combine(directory, "nested", "other.dll"), "generated library");
            foreach (var flags in new[] { new[] { false, false }, new[] { true, false }, new[] { false, true } })
                Check(!Net10OutputCleanup.TryDelete(source, output, flags[0], flags[1]) && File.Exists(output),
                    "output directory is preserved unless both deletion flags are enabled");
            Check(!Net10OutputCleanup.TryDelete(source, source, true, true) && File.Exists(source),
                "source directory cannot be used as an output cleanup target");
            Check(!Net10OutputCleanup.TryDelete(source, Path.Combine(root, "elsewhere", "main.exe"), true, true),
                "compiler output outside the expected net10-output directory is rejected");
            Check(!Net10OutputCleanup.TryDelete(source, Path.Combine(directory, "other.pcu"), true, true),
                "module-only compilation cannot authorize directory cleanup");
            Check(!Net10OutputCleanup.TryDelete("main.pas", output, true, true),
                "cleanup does not resolve relative source paths against a changing working directory");
            using (var locked = new FileStream(output, FileMode.Open, FileAccess.Read, FileShare.None))
                Check(!Net10OutputCleanup.TryDelete(source, output, true, true) && File.Exists(source),
                    "a locked output file does not throw or affect the source");
            // Legacy lifecycle cleanup may have already removed the EXE/PDB.
            File.Delete(output);
            Check(Net10OutputCleanup.TryDelete(source, output, true, true) && !Directory.Exists(directory),
                "whole generated directory is removed, including JSON, modules and nested files, even if EXE is gone");
            Check(File.Exists(source) && File.ReadAllText(source) == "begin end.", "source is preserved by recursive cleanup");
            Check(!Net10OutputCleanup.TryDelete(source, output, true, true), "cleanup is harmless when the directory is already gone");
            string selectedRoot = Path.Combine(root, "Selected output");
            string selectedDirectory = Net10OutputCleanup.GetDirectory(source, selectedRoot);
            string selectedExe = Path.Combine(selectedDirectory, "main.exe");
            Check(selectedDirectory == Path.Combine(selectedRoot, "net10-output") &&
                Net10OutputCleanup.GetDirectory(source) == directory, "both output locations resolve consistently");
            Directory.CreateDirectory(selectedDirectory);
            File.WriteAllText(selectedExe, "generated");
            string sentinel = Path.Combine(selectedRoot, "keep.txt");
            File.WriteAllText(sentinel, "keep");
            Check(!Net10OutputCleanup.TryDelete(source, selectedExe, true, true),
                "selected output cannot be deleted using source-adjacent settings");
            foreach (var flags in new[] { new[] { false, false }, new[] { true, false }, new[] { false, true } })
                Check(!Net10OutputCleanup.TryDelete(source, selectedExe, flags[0], flags[1], selectedRoot) && File.Exists(selectedExe),
                    "selected output is preserved unless both deletion flags are enabled");
            string insideSource = Path.Combine(selectedDirectory, "opened.pas");
            File.WriteAllText(insideSource, "begin end.");
            Check(!Net10OutputCleanup.TryDelete(insideSource, selectedExe, true, true, selectedRoot) && File.Exists(insideSource),
                "cleanup refuses to delete a source opened inside the generated directory");
            File.Delete(insideSource);
            Check(Net10OutputCleanup.TryDelete(source, selectedExe, true, true, selectedRoot) &&
                !Directory.Exists(selectedDirectory) && File.ReadAllText(sentinel) == "keep" && File.Exists(source),
                "only net10-output is removed, never the selected parent folder or its other files");

            string publishDirectory = Net10OutputCleanup.CreatePublishDirectory();
            try
            {
                File.WriteAllText(Path.Combine(publishDirectory, "main.exe"), "temporary build");
                Check(!Net10OutputCleanup.TryDeletePublishDirectory(root) && File.Exists(source),
                    "publish cleanup rejects unrelated directories");
                Check(Net10OutputCleanup.TryDeletePublishDirectory(publishDirectory) &&
                    !Directory.Exists(publishDirectory) && File.Exists(source),
                    "publish removes only its isolated temporary build directory");
            }
            finally { Net10OutputCleanup.TryDeletePublishDirectory(publishDirectory); }
        }
        finally { DeleteOutputCleanupFixture(root); }
    }

    private static void DeleteOutputCleanupFixture(string root)
    {
        string fullRoot = Path.GetFullPath(root);
        if (!fullRoot.StartsWith(Path.GetFullPath(Path.GetTempPath()).TrimEnd(Path.DirectorySeparatorChar) +
                Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
            !Path.GetFileName(fullRoot).StartsWith("pabc-output-cleanup-", StringComparison.Ordinal))
            throw new InvalidOperationException("Unsafe cleanup test directory");
        Directory.Delete(fullRoot, true);
    }
}
