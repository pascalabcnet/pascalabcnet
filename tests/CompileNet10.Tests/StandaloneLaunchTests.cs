using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static async Task CheckStandaloneLaunch(string runtime)
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT) return;
        string root = Path.Combine(Path.GetTempPath(), "pabc-standalone-launch-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        using (var compiler = new Net10ControllerClient(runtime, "dotnet"))
        {
            string source = Path.Combine(root, "Windowed.pas");
            var windowed = await compiler.CompileAsync(source, root, "__RunMode", new List<Net10SourceFile>
            {
                new Net10SourceFile { fileName = source, text =
                    "{$apptype windows}\n" +
                    "function GetConsoleWindow: System.IntPtr; external 'kernel32.dll' name 'GetConsoleWindow';\n" +
                    "function IsWindowVisible(wnd: System.IntPtr): integer; external 'user32.dll' name 'IsWindowVisible';\n" +
                    "begin\n" +
                    "  if IsWindowVisible(GetConsoleWindow()) <> 0 then Halt(7);\n" +
                    "  System.IO.File.WriteAllText('windowed-argument.txt', CommandLineArgs[0]);\n" +
                    "end." }
            });
            Check(windowed.success, "compile a Windows GUI EXE: " + windowed.message);
            foreach (bool pause in new[] { false, true })
            {
                using (var runner = new Net10ProgramRunner("dotnet", windowed.outputFile, root,
                    pause ? "with-pause" : "without-pause", redirectConsoleIO: false, pause: pause))
                {
                    var start = GetLaunchInfo(runner);
                    Check(start.FileName == "dotnet" && start.CreateNoWindow && !start.RedirectStandardOutput,
                        "standalone GUI uses dotnet without a console or ProgrammRunner, pause=" + pause);
                    var run = runner.RunAsync(null, null);
                    if (await Task.WhenAny(run, Task.Delay(30000)) != run)
                    {
                        runner.Stop();
                        throw new TimeoutException("Windowed standalone run timed out");
                    }
                    int exitCode = await run;
                    Check(exitCode == 0, "Windows GUI process has no visible console and exits normally: " + exitCode);
                }
                Check(File.ReadAllText(Path.Combine(root, "windowed-argument.txt")) ==
                    (pause ? "with-pause" : "without-pause"), "standalone GUI keeps user arguments");
            }

            source = Path.Combine(root, "Console.pas");
            var console = await compiler.CompileAsync(source, root, "__RunMode", new List<Net10SourceFile>
            {
                new Net10SourceFile { fileName = source, text = "{$apptype console}\nbegin end." }
            });
            Check(console.success, "compile a console EXE: " + console.message);
            foreach (bool pause in new[] { false, true })
            {
                // Inspect the launch plan instead of opening a console and waiting for a key.
                using (var runner = new Net10ProgramRunner("dotnet", console.outputFile, root, "",
                    redirectConsoleIO: false, pause: pause))
                {
                    var start = GetLaunchInfo(runner);
                    Check(!start.CreateNoWindow && Path.GetFileName(start.FileName) ==
                        (pause ? "ProgrammRunner.exe" : "dotnet"),
                        "standalone console preserves its console and configured pause=" + pause);
                }
            }
        }
    }

    private static ProcessStartInfo GetLaunchInfo(Net10ProgramRunner runner)
    {
        var field = typeof(Net10ProgramRunner).GetField("process", BindingFlags.Instance | BindingFlags.NonPublic);
        return ((Process)field.GetValue(runner)).StartInfo;
    }
}
