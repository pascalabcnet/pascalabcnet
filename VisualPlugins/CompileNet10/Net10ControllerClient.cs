using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace VisualPascalABCPlugins
{
    public sealed class Net10CompileResponse
    {
        public int id { get; set; }
        public bool success { get; set; }
        public string result { get; set; }
        public string message { get; set; }
        public string outputFile { get; set; }
        public int workerPid { get; set; }
        public List<Net10Diagnostic> diagnostics { get; set; }
    }

    public sealed class Net10Diagnostic
    {
        public string severity { get; set; }
        public string fileName { get; set; }
        public int line { get; set; }
        public int column { get; set; }
        public string message { get; set; }
    }

    public sealed class Net10CompilerEvent
    {
        public int id { get; set; }
        public string @event { get; set; }
        public int attempt { get; set; }
        public string state { get; set; }
        public string fileName { get; set; }
        public uint linesCompiled { get; set; }
        public int errorCount { get; set; }
        public int warningCount { get; set; }
        public double elapsedMilliseconds { get; set; }
    }

    public sealed class Net10SourceFile
    {
        public string fileName { get; set; }
        public string text { get; set; }
    }

    public sealed class Net10ControllerClient : IDisposable
    {
        public event Action<string> WorkerRestarted;
        private readonly string runtimeDirectory;
        private readonly string dotnetPath;
        private readonly SemaphoreSlim requests = new SemaphoreSlim(1, 1);
        private readonly JavaScriptSerializer json = new JavaScriptSerializer { MaxJsonLength = 4 * 1024 * 1024 };
        private readonly StringBuilder log = new StringBuilder();
        private Process controller;
        private StreamWriter input;
        private Process worker;
        private int nextId;
        private bool disposed;

        public Net10ControllerClient(string runtimeDirectory, string dotnetPath)
        {
            this.runtimeDirectory = Path.GetFullPath(runtimeDirectory);
            this.dotnetPath = dotnetPath;
        }

        public async Task<Net10CompileResponse> CompileAsync(string fileName, string outputDirectory,
            string runtimeModule = null, List<Net10SourceFile> sourceFiles = null,
            Func<Net10CompilerEvent, Task> progress = null, bool rebuild = false)
        {
            await requests.WaitAsync().ConfigureAwait(false);
            try
            {
                if (disposed) throw new ObjectDisposedException(nameof(Net10ControllerClient));
                if (controller == null || controller.HasExited)
                {
                    Stop();
                    string assembly = Path.Combine(runtimeDirectory, "PABCCompilerController.dll");
                    if (!File.Exists(assembly)) throw new FileNotFoundException("Не найден контроллер .NET 10", assembly);
                    lock (log) log.Clear();
                    controller = new Process
                    {
                        StartInfo = new ProcessStartInfo(dotnetPath, "\"" + assembly + "\"")
                        {
                            WorkingDirectory = runtimeDirectory,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            StandardOutputEncoding = Encoding.UTF8,
                            StandardErrorEncoding = Encoding.UTF8
                        }
                    };
                    controller.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null) return;
                        const string restartPrefix = "[CompilerRestarted]";
                        if (e.Data.StartsWith(restartPrefix, StringComparison.Ordinal))
                            WorkerRestarted?.Invoke(e.Data.Substring(restartPrefix.Length));
                        lock (log)
                        {
                            log.AppendLine(e.Data);
                            if (log.Length > 8192) log.Remove(0, log.Length - 8192);
                        }
                    };
                    controller.Start();
                    input = new StreamWriter(controller.StandardInput.BaseStream, new UTF8Encoding(false));
                    controller.BeginErrorReadLine();
                }
                var ping = await SendAsync("ping", null, null, 20000).ConfigureAwait(false);
                if (!ping.success || ping.result != "PONG") throw new IOException("Контроллер не ответил PONG.");
                TrackWorker(ping.workerPid);
                var response = await SendAsync("compile", fileName, outputDirectory, 120000, runtimeModule, sourceFiles, progress, rebuild).ConfigureAwait(false);
                TrackWorker(response.workerPid);
                return response;
            }
            catch (Exception error)
            {
                Stop();
                string details;
                lock (log) details = log.ToString();
                throw new IOException(error.Message + (details.Length == 0 ? "" : "\n" + details), error);
            }
            finally { requests.Release(); }
        }

        private async Task<Net10CompileResponse> SendAsync(string command, string fileName, string outputDirectory,
            int timeoutMs, string runtimeModule = null, List<Net10SourceFile> sourceFiles = null,
            Func<Net10CompilerEvent, Task> progress = null, bool rebuild = false)
        {
            int id = ++nextId;
            var request = new Dictionary<string, object> { { "id", id }, { "command", command } };
            if (fileName != null) request.Add("fileName", fileName);
            if (outputDirectory != null) request.Add("outputDirectory", outputDirectory);
            if (runtimeModule != null) request.Add("runtimeModule", runtimeModule);
            if (sourceFiles != null) request.Add("sourceFiles", sourceFiles);
            if (progress != null) request.Add("emitEvents", true);
            if (rebuild) request.Add("rebuild", true);
            input.WriteLine(json.Serialize(request));
            input.Flush();
            using (var cancellation = new CancellationTokenSource())
            {
                Task timeout = Task.Delay(timeoutMs, cancellation.Token);
                try
                {
                    while (true)
                    {
                        Task<string> read = controller.StandardOutput.ReadLineAsync();
                        if (await Task.WhenAny(read, timeout).ConfigureAwait(false) != read)
                            throw new TimeoutException("Истекло время ожидания контроллера .NET 10.");
                        string line = await read.ConfigureAwait(false);
                        if (line == null) throw new IOException("Контроллер .NET 10 завершил соединение.");
                        var notification = json.Deserialize<Net10CompilerEvent>(line);
                        if (notification == null || notification.id != id)
                            throw new IOException("Некорректный ответ контроллера .NET 10.");
                        if (notification.@event != null)
                        {
                            if (progress == null || notification.@event != "compilerState")
                                throw new IOException("Неожиданное событие контроллера .NET 10.");
                            await progress(notification).ConfigureAwait(false);
                            continue;
                        }
                        return json.Deserialize<Net10CompileResponse>(line);
                    }
                }
                finally { cancellation.Cancel(); }
            }
        }

        private void TrackWorker(int pid)
        {
            if (pid <= 0 || (worker != null && worker.Id == pid)) return;
            worker?.Dispose();
            worker = null;
            try
            {
                worker = Process.GetProcessById(pid);
                // Hold a process handle so cleanup cannot kill an unrelated reused PID.
                var handle = worker.Handle;
            }
            catch (ArgumentException) { worker = null; }
        }

        private void Stop()
        {
            if (controller != null)
            {
                try
                {
                    if (!controller.HasExited)
                    {
                        input.WriteLine(json.Serialize(new { id = ++nextId, command = "shutdown" }));
                        input.Close();
                        if (!controller.WaitForExit(2000))
                        {
                            if (worker != null && !worker.HasExited) worker.Kill();
                            controller.Kill();
                            controller.WaitForExit(1000);
                        }
                    }
                }
                catch (InvalidOperationException) { }
                catch (IOException) { }
                finally { input?.Dispose(); input = null; controller.Dispose(); controller = null; }
            }
            worker?.Dispose();
            worker = null;
        }

        public void Dispose()
        {
            disposed = true;
            // Serializes shutdown with an in-flight request without blocking the IDE thread.
            Task.Run(async () =>
            {
                await requests.WaitAsync().ConfigureAwait(false);
                try { Stop(); }
                finally { requests.Release(); }
            });
        }
    }
}
