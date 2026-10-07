using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisualPascalABCPlugins
{
    // Owns the process; the IDE owns its shared lifecycle through the optional callbacks.
    internal sealed class Net10ProgramRunner : IDisposable
    {
        private readonly Process process;
        private readonly SemaphoreSlim writes = new SemaphoreSlim(1, 1);
        private StreamWriter input;
        public bool WasStopped { get; private set; }

        public Net10ProgramRunner(string dotnetPath, string outputFile, string workingDirectory, string arguments,
            Func<string, string> prepareArguments = null)
        {
            if (!File.Exists(outputFile))
                throw new FileNotFoundException("Не найден результат компиляции .NET 10", outputFile);
            string commandArguments = "[REDIRECTIOMODE]" + (string.IsNullOrWhiteSpace(arguments) ? "" : " " + arguments);
            if (prepareArguments != null) commandArguments = prepareArguments(commandArguments);
            process = new Process
            {
                StartInfo = new ProcessStartInfo(dotnetPath, "\"" + outputFile + "\" " + commandArguments)
                {
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = new UTF8Encoding(false),
                    StandardErrorEncoding = new UTF8Encoding(false)
                }
            };
        }

        public async Task<int> RunAsync(Action<string> output, Action readRequested,
            Action started = null, Action<RuntimeExceptionInfo> exception = null)
        {
            process.Start();
            input = new StreamWriter(process.StandardInput.BaseStream, new UTF8Encoding(false)) { AutoFlush = true };
            started?.Invoke();
            var protocol = new Net10RuntimeProtocol(output, readRequested, exception);
            Task stdout = PumpAsync(process.StandardOutput, output);
            Task stderr = PumpAsync(process.StandardError, protocol.Feed);
            await SendInputAsync("GO").ConfigureAwait(false);
            await Task.Run(() => process.WaitForExit()).ConfigureAwait(false);
            // Drain both pipes before reporting completion, including the final exception.
            await Task.WhenAll(stdout, stderr).ConfigureAwait(false);
            protocol.Complete();
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
        private string pending = "";
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
                    if (error != null) exception(error);
                    else output(Environment.NewLine + payload.Replace("[MESSAGE]", ": ")
                        .Replace("[STACK]", Environment.NewLine) + Environment.NewLine);
                    pending = pending.Substring(end + 5);
                }
                else if (Read.StartsWith(pending, StringComparison.Ordinal) ||
                    CodePage.StartsWith(pending, StringComparison.Ordinal) ||
                    ExceptionStart.StartsWith(pending, StringComparison.Ordinal)) return;
                else
                {
                    int next = pending.IndexOf('[', 1);
                    int count = next < 0 ? pending.Length : next;
                    output(pending.Substring(0, count));
                    pending = pending.Substring(count);
                }
            }
        }

        public void Complete()
        {
            if (pending.Length > 0) output(pending);
            pending = "";
        }
    }
}
