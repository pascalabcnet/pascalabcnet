using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace VisualPascalABCPlugins
{
    // Owns the process; the IDE owns its shared lifecycle through the optional callbacks.
    internal sealed class Net10ProgramRunner : IDisposable
    {
        private readonly Process process;
        private readonly bool redirectConsoleIO;
        private readonly bool pauseHost;
        private readonly SemaphoreSlim writes = new SemaphoreSlim(1, 1);
        private StreamWriter input;
        public bool WasStopped { get; private set; }

        public Net10ProgramRunner(string dotnetPath, string outputFile, string workingDirectory, string arguments,
            Func<string, string> prepareArguments = null, bool redirectConsoleIO = true,
            bool pause = false, string locale = "ru-RU")
        {
            if (!File.Exists(outputFile))
                throw new FileNotFoundException("Не найден результат компиляции .NET 10", outputFile);
            this.redirectConsoleIO = redirectConsoleIO;
            bool windowed = !redirectConsoleIO && Environment.OSVersion.Platform == PlatformID.Win32NT &&
                IsWindowsGuiExecutable(outputFile);
            pauseHost = !redirectConsoleIO && !windowed && pause;
            string commandArguments = (redirectConsoleIO ? "[REDIRECTIOMODE]" : "[RUNMODE]") +
                (string.IsNullOrWhiteSpace(arguments) ? "" : " " + arguments);
            if (prepareArguments != null) commandArguments = prepareArguments(commandArguments);
            string executable = dotnetPath;
            string processArguments = "\"" + outputFile + "\" " + commandArguments;
            if (pauseHost)
            {
                executable = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProgrammRunner.exe");
                if (!File.Exists(executable))
                    throw new FileNotFoundException("Не найден консольный запускатель", executable);
                processArguments = "--net10 \"" + dotnetPath + "\" \"" + outputFile +
                    "\" \"" + locale + "\" " + commandArguments;
            }
            var startInfo = new ProcessStartInfo(executable, processArguments)
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                // dotnet.exe is a console host even when the managed EXE is WinExe.
                CreateNoWindow = redirectConsoleIO || windowed,
                RedirectStandardInput = redirectConsoleIO,
                RedirectStandardOutput = redirectConsoleIO,
                RedirectStandardError = redirectConsoleIO
            };
            if (redirectConsoleIO)
            {
                startInfo.StandardOutputEncoding = new UTF8Encoding(false);
                startInfo.StandardErrorEncoding = new UTF8Encoding(false);
            }
            process = new Process { StartInfo = startInfo };
        }

        private static bool IsWindowsGuiExecutable(string fileName)
        {
            // Read the PE subsystem without loading the compiled assembly into the IDE.
            const int peOffsetField = 0x3c;
            const int optionalHeaderOffset = 24; // PE signature + COFF header
            const int subsystemOffset = 68; // same for PE32 and PE32+
            const ushort windowsGuiSubsystem = 2;
            try
            {
                using (var stream = File.OpenRead(fileName))
                using (var reader = new BinaryReader(stream))
                {
                    if (stream.Length < peOffsetField + sizeof(int) || reader.ReadUInt16() != 0x5a4d)
                        return false;
                    stream.Position = peOffsetField;
                    int peOffset = reader.ReadInt32();
                    if (peOffset < peOffsetField + sizeof(int) ||
                        stream.Length - peOffset < optionalHeaderOffset + subsystemOffset + sizeof(ushort))
                        return false;
                    stream.Position = peOffset;
                    if (reader.ReadUInt32() != 0x00004550)
                        return false;
                    stream.Position = (long)peOffset + 20; // COFF SizeOfOptionalHeader
                    if (reader.ReadUInt16() < subsystemOffset + sizeof(ushort))
                        return false;
                    stream.Position = (long)peOffset + optionalHeaderOffset;
                    ushort magic = reader.ReadUInt16();
                    if (magic != 0x10b && magic != 0x20b)
                        return false;
                    stream.Position = (long)peOffset + optionalHeaderOffset + subsystemOffset;
                    return reader.ReadUInt16() == windowsGuiSubsystem;
                }
            }
            catch (IOException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
        }

        public async Task<int> RunAsync(Action<string> output, Action readRequested,
            Action started = null, Action<RuntimeExceptionInfo> exception = null)
        {
            process.Start();
            if (!redirectConsoleIO)
            {
                started?.Invoke();
                await Task.Run(() => process.WaitForExit()).ConfigureAwait(false);
                return process.ExitCode;
            }
            input = new StreamWriter(process.StandardInput.BaseStream, new UTF8Encoding(false)) { AutoFlush = true };
            started?.Invoke();
            var protocol = new Net10RuntimeProtocol(output, readRequested, exception);
            Task stdout = PumpAsync(process.StandardOutput, output);
            Task stderr = PumpAsync(process.StandardError, protocol.Feed);
            await SendInputAsync("GO").ConfigureAwait(false);
            await Task.Run(() => process.WaitForExit()).ConfigureAwait(false);
            // Drain both pipes before reporting completion, including the final exception.
            await Task.WhenAll(stdout, stderr).ConfigureAwait(false);
            protocol.Complete(process.ExitCode != 0 && !WasStopped);
            return process.ExitCode;
        }

        private static async Task PumpAsync(StreamReader stream, Action<string> receive)
        {
            var buffer = new char[2048];
            int count;
            while ((count = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                receive(new string(buffer, 0, count));
        }

        public async Task SendInputAsync(string text)
        {
            await writes.WaitAsync().ConfigureAwait(false);
            try
            {
                if (input == null || process.HasExited) throw new IOException("Программа .NET 10 уже завершена.");
                await input.WriteLineAsync(text).ConfigureAwait(false);
                await input.FlushAsync().ConfigureAwait(false);
            }
            finally { writes.Release(); }
        }

        public void Stop()
        {
            try
            {
                if (!process.HasExited)
                {
                    WasStopped = true;
                    if (pauseHost && Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        using (var killer = Process.Start(new ProcessStartInfo("taskkill.exe",
                            "/T /F /PID " + process.Id)
                        {
                            UseShellExecute = false, CreateNoWindow = true
                        }))
                        {
                            if (killer.WaitForExit(1000) && killer.ExitCode == 0) return;
                        }
                    }
                    process.Kill();
                }
            }
            catch (InvalidOperationException) { }
            catch (Win32Exception) { }
        }

        public void Dispose()
        {
            Stop();
            input?.Dispose();
            process.Dispose();
        }
    }

    // stderr is a character stream: commands may span reads and have no newline.
    internal sealed class Net10RuntimeProtocol
    {
        private const string Read = "[READLNSIGNAL]";
        private const string CodePage = "[CODEPAGE";
        private const string ExceptionStart = "[EXCEPTION]";
        private const string FatalStart = "Process terminated.";
        private const string AssertionHeading = "Assertion failed.";
        private const int MaxFatalDiagnosticLength = 64 * 1024;
        private string pending = "";
        private bool reportedException;
        private readonly Action<string> output;
        private readonly Action readRequested;
        private readonly Action<RuntimeExceptionInfo> exception;

        public Net10RuntimeProtocol(Action<string> output, Action readRequested, Action<RuntimeExceptionInfo> exception = null)
        {
            this.output = output;
            this.readRequested = readRequested;
            this.exception = exception;
        }

        public void Feed(string text)
        {
            pending += text;
            while (pending.Length > 0)
            {
                if (pending.StartsWith(Read, StringComparison.Ordinal))
                {
                    pending = pending.Substring(Read.Length);
                    readRequested();
                }
                else if (pending.StartsWith(CodePage, StringComparison.Ordinal))
                {
                    int end = pending.IndexOf(']');
                    if (end < 0) return;
                    pending = pending.Substring(end + 1);
                }
                else if (pending.StartsWith(ExceptionStart, StringComparison.Ordinal))
                {
                    int end = pending.IndexOf("[END]", StringComparison.Ordinal);
                    if (end < 0) return;
                    string payload = pending.Substring(ExceptionStart.Length, end - ExceptionStart.Length);
                    RuntimeExceptionInfo error = null;
                    if (exception != null)
                    {
                        try { error = RuntimeExceptionInfo.Parse(payload); }
                        catch (FormatException) { }
                    }
                    if (error != null)
                    {
                        reportedException = true;
                        exception(error);
                    }
                    else output(Environment.NewLine + payload.Replace("[MESSAGE]", ": ")
                        .Replace("[STACK]", Environment.NewLine) + Environment.NewLine);
                    pending = pending.Substring(end + 5);
                }
                else if (CanReportFatalAssertion && pending.StartsWith(FatalStart, StringComparison.Ordinal) &&
                    pending.Length <= MaxFatalDiagnosticLength)
                {
                    int end = pending.IndexOf('\n');
                    if (end < 0) return;
                    int length = Math.Min(AssertionHeading.Length + 2, pending.Length - end - 1);
                    string assertion = pending.Substring(end + 1, length);
                    if (AssertionHeading.StartsWith(assertion, StringComparison.Ordinal) ||
                        assertion.StartsWith(AssertionHeading + "\n", StringComparison.Ordinal) ||
                        assertion.StartsWith(AssertionHeading + "\r", StringComparison.Ordinal))
                        return;
                    output(pending.Substring(0, end + 1));
                    pending = pending.Substring(end + 1);
                }
                else if (Read.StartsWith(pending, StringComparison.Ordinal) ||
                    CodePage.StartsWith(pending, StringComparison.Ordinal) ||
                    ExceptionStart.StartsWith(pending, StringComparison.Ordinal) ||
                    (CanReportFatalAssertion && FatalStart.StartsWith(pending, StringComparison.Ordinal))) return;
                else
                {
                    int next = pending.IndexOf('[', 1);
                    if (CanReportFatalAssertion)
                    {
                        int fatal = pending.IndexOf('P', 1);
                        if (fatal >= 0 && (next < 0 || fatal < next)) next = fatal;
                    }
                    int count = next < 0 ? pending.Length : next;
                    output(pending.Substring(0, count));
                    pending = pending.Substring(count);
                }
            }
        }

        private bool CanReportFatalAssertion => exception != null && !reportedException;

        public void Complete(bool failed = false)
        {
            var error = failed && CanReportFatalAssertion ? ParseFatalAssertion(pending) : null;
            if (error != null) exception(error);
            else if (pending.Length > 0) output(pending);
            pending = "";
        }

        private static RuntimeExceptionInfo ParseFatalAssertion(string text)
        {
            if (!text.StartsWith(FatalStart, StringComparison.Ordinal)) return null;
            var lines = text.Split('\n');
            if (lines.Length < 4 || lines[0].TrimEnd('\r') != FatalStart ||
                lines[1].TrimEnd('\r') != AssertionHeading)
                return null;

            int firstFrame = Array.FindIndex(lines, 2, line => line.TrimStart().StartsWith("at ", StringComparison.Ordinal));
            if (firstFrame < 0) firstFrame = lines.Length;
            var message = string.Join("\n", lines, 2, firstFrame - 2).Trim();
            var stack = firstFrame < lines.Length
                ? string.Join("\n", lines, firstFrame, lines.Length - firstFrame).TrimEnd()
                : "";
            var error = RuntimeExceptionInfo.Parse("Assertion failed[MESSAGE][STACK]" + stack);
            error.Message = message;
            if (error.Frames.Exists(frame => frame.Line > 0 &&
                frame.FileName?.EndsWith(".pas", StringComparison.OrdinalIgnoreCase) == true))
                return error;

            // An inlined assertion may only carry the source location in its Pascal message.
            var location = Regex.Match(message, @"\A(?:File|Файл) (.+\.pas), (?:line|строка) (\d+)(?=:|\r?$)",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (!location.Success || !int.TryParse(location.Groups[2].Value, out int lineNumber) || lineNumber <= 0)
                return null;
            error.Frames.Insert(0, new RuntimeStackFrame { FileName = location.Groups[1].Value, Line = lineNumber });
            return error;
        }
    }
}
