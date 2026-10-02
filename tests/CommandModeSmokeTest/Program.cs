using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using PascalABCCompiler;

// Exercises the actual stdio protocol, including editor callbacks, without an IDE.
internal static class Program
{
    private const int Ready = ConsoleCompilerConstants.CommandStartNumber;
    private const int CompilationFinished = Ready + 14;
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(60);

    [DllImport("kernel32.dll")]
    private static extern uint SetErrorMode(uint mode);

    public static async Task<int> Main(string[] args)
    {
        // Child failures should report stderr, without opening a Windows error dialog.
        if (OperatingSystem.IsWindows())
            SetErrorMode(0x0002);
        if (args.Length == 0)
        {
            Console.Error.WriteLine("Usage: CommandModeSmokeTest <compiler.exe|compiler.dll> [...]");
            return 2;
        }

        try
        {
            foreach (string file in args)
                await CheckCompiler(Path.GetFullPath(file));
            return 0;
        }
        catch (Exception error)
        {
            Console.Error.WriteLine(error);
            return 1;
        }
    }

    private static async Task CheckCompiler(string compilerPath)
    {
        string temporaryDirectory = Path.Combine(Path.GetTempPath(), "pabc-commandmode-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryDirectory);
        var start = new ProcessStartInfo
        {
            FileName = compilerPath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) ? "dotnet" : compilerPath,
            WorkingDirectory = Path.GetDirectoryName(compilerPath)!,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardInputEncoding = new UTF8Encoding(false),
            StandardOutputEncoding = new UTF8Encoding(false),
            StandardErrorEncoding = new UTF8Encoding(false)
        };
        if (start.FileName == "dotnet")
            start.ArgumentList.Add(compilerPath);
        start.ArgumentList.Add("commandmode");
        using var process = new Process { StartInfo = start };
        Task<string>? errors = null;
        try
        {
            process.Start();
            errors = process.StandardError.ReadToEndAsync();
            int originalPid = process.Id;
            string sourcePath = Path.Combine(temporaryDirectory, "Несохранённая программа.pas");
            string outputPath = Path.ChangeExtension(sourcePath, ".exe");
            string sourceText = "begin Assert(2 + 2 = 4); end.";
            int sourceRequests = 0;
            long workingSet = 0;

            async Task<(int Command, string Argument)> ReadAndHandle()
            {
                string frame = await ReadFrame(process.StandardOutput);
                if (frame.Length < 3 || !int.TryParse(frame.AsSpan(0, 3), out int command))
                    throw new InvalidDataException("Invalid protocol frame: " + frame);
                string argument = frame.Length > 3 ? frame.Substring(4) : "";
                switch (command)
                {
                    case ConsoleCompilerConstants.SourceFileText:
                        if (string.Equals(argument, sourcePath, StringComparison.OrdinalIgnoreCase))
                        {
                            sourceRequests++;
                            SendText(process, ConsoleCompilerConstants.SourceFileText, sourceText);
                        }
                        else
                            Send(process, ConsoleCompilerConstants.Error);
                        break;
                    case ConsoleCompilerConstants.FileExsist:
                        Send(process, command, argument == sourcePath || File.Exists(argument));
                        break;
                    case ConsoleCompilerConstants.GetLastWriteTime:
                        Send(process, command, argument == sourcePath ? DateTime.UtcNow.Ticks : File.GetLastWriteTime(argument).Ticks);
                        break;
                    case ConsoleCompilerConstants.WorkingSet:
                        workingSet = long.Parse(argument);
                        break;
                }
                return (command, argument);
            }

            // Drain startup notifications before issuing the first command.
            while ((await ReadAndHandle()).Command != Ready) { }
            SendText(process, ConsoleCompilerConstants.CompilerOptionsFileName, sourcePath);
            SendText(process, ConsoleCompilerConstants.CompilerOptionsOutputDirectory, temporaryDirectory);
            Send(process, ConsoleCompilerConstants.CompilerOptionsDebug, false);
            Send(process, ConsoleCompilerConstants.CompilerOptionsRebuild, false);
            Send(process, ConsoleCompilerConstants.CompilerOptionsOutputType, 1);
            Send(process, ConsoleCompilerConstants.CompilerOptionsProjectCompiled, false);
            Send(process, ConsoleCompilerConstants.UseDllForSystemUnits, false);
            Send(process, ConsoleCompilerConstants.InternalDebugSavePCU, false);
            Send(process, ConsoleCompilerConstants.CompilerOptionsForDebugging, false);
            Send(process, ConsoleCompilerConstants.CompilerOptionsRunWithEnvironment, false);
            Send(process, ConsoleCompilerConstants.CompilerLocale, "en-US");
            Send(process, ConsoleCompilerConstants.IDELocale, Encoding.UTF8.GetString(Encoding.Unicode.GetBytes("en")));
            Send(process, ConsoleCompilerConstants.CompilerOptionsClearStandartModules);
            Send(process, ConsoleCompilerConstants.CompilerOptionsStandartModule,
                "PABCSystem" + ConsoleCompilerConstants.MessageSeparator + "0" + ConsoleCompilerConstants.MessageSeparator + "PascalABC.NET");

            async Task Compile(bool expectedSuccess)
            {
                var diagnostics = new List<string>();
                bool finished = false;
                int previousRequests = sourceRequests;
                Send(process, ConsoleCompilerConstants.CommandCompile);
                while (true)
                {
                    var frame = await ReadAndHandle();
                    if (frame.Command == ConsoleCompilerConstants.Error || frame.Command == ConsoleCompilerConstants.InternalError)
                        diagnostics.Add(frame.Argument);
                    if (frame.Command == CompilationFinished)
                        finished = true;
                    if (frame.Command == Ready)
                        break;
                }
                if (!finished || (diagnostics.Count == 0) != expectedSuccess)
                    throw new Exception($"Compilation result mismatch (success expected: {expectedSuccess}): {string.Join("\n", diagnostics)}");
                if (sourceRequests == previousRequests)
                    throw new Exception("Compiler did not request the current editor text.");
                if (process.Id != originalPid || process.HasExited)
                    throw new Exception("Compiler did not stay alive between compilations.");
                if (expectedSuccess && !File.Exists(outputPath))
                    throw new Exception("Compiler did not create its output file.");
                if (workingSet <= 0)
                    throw new Exception("Compiler did not report its working set.");
            }

            await Compile(true);
            byte[] firstAssembly = File.ReadAllBytes(outputPath);
            sourceText = "begin this_identifier_does_not_exist := 1; end.";
            await Compile(false);
            sourceText = "begin Assert('Привет' = 'Привет'); end.";
            await Compile(true);
            if (firstAssembly.SequenceEqual(File.ReadAllBytes(outputPath)))
                throw new Exception("Compiler reused the first output after the editor text changed.");
            Send(process, ConsoleCompilerConstants.CommandGCCollect);
            workingSet = 0;
            Send(process, ConsoleCompilerConstants.WorkingSet);
            while (workingSet == 0)
                await ReadAndHandle();
            Send(process, ConsoleCompilerConstants.CommandExit);
            if ((await ReadAndHandle()).Command != ConsoleCompilerConstants.CommandExit)
                throw new Exception("Missing shutdown acknowledgement.");
            await process.WaitForExitAsync().WaitAsync(Timeout);
            string stderr = await errors;
            if (process.ExitCode != 0 || stderr.Length != 0)
                throw new Exception($"Compiler exited with {process.ExitCode}: {stderr}");
            Console.WriteLine($"PASS: {compilerPath} (three compilations, editor text, Unicode paths, diagnostics, GC, working set, shutdown)");
        }
        catch (Exception error)
        {
            if (errors != null && process.HasExited)
                throw new Exception($"{compilerPath} exited with {process.ExitCode}: {(await errors).Replace("\0", "\\0")}", error);
            throw;
        }
        finally
        {
            if (errors != null && !process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
            }
            Directory.Delete(temporaryDirectory, recursive: true);
        }
    }

    private static void Send(Process process, int command, object? argument = null)
    {
        process.StandardInput.WriteLine(argument == null ? command.ToString() : $"{command} {argument}");
        process.StandardInput.Flush();
    }

    private static void SendText(Process process, int command, string text)
    {
        Send(process, command, text.Length);
        process.StandardInput.Write(text);
        process.StandardInput.Flush();
    }

    private static async Task<string> ReadFrame(StreamReader reader)
    {
        var text = new StringBuilder();
        char[] character = new char[1];
        using var cancellation = new CancellationTokenSource(Timeout);
        while (true)
        {
            if (await reader.ReadAsync(character.AsMemory(), cancellation.Token) == 0)
                throw new EndOfStreamException("Compiler closed the protocol stream: " + text);
            text.Append(character[0]);
            if (text.Length >= ConsoleCompilerConstants.DataSeparator.Length &&
                text.ToString(text.Length - ConsoleCompilerConstants.DataSeparator.Length, ConsoleCompilerConstants.DataSeparator.Length) == ConsoleCompilerConstants.DataSeparator)
                return text.ToString(0, text.Length - ConsoleCompilerConstants.DataSeparator.Length);
        }
    }
}
