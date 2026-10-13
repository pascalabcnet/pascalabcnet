using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace VisualPascalABCPlugins
{
    internal interface INet10LanguageClient : IDisposable
    {
        bool IsConnected { get; }
        event Action<Exception> ConnectionLost;
        Task StartAsync();
        Task NotifyAsync(string method, object parameters);
        Task<object> RequestAsync(string method, object parameters, CancellationToken cancellation = default(CancellationToken));
        Task StopAsync();
    }

    // LSP framing, not Controller JSONL. No compiler assemblies enter the IDE process.
    internal sealed class Net10LanguageClient : INet10LanguageClient
    {
        private const int MaximumMessageBytes = 16 * 1024 * 1024;
        private readonly string runtimeDirectory, dotnetPath, language;
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private readonly SemaphoreSlim writes = new SemaphoreSlim(1, 1);
        private readonly ConcurrentDictionary<int, TaskCompletionSource<object>> pending =
            new ConcurrentDictionary<int, TaskCompletionSource<object>>();
        private readonly JavaScriptSerializer json = new JavaScriptSerializer { MaxJsonLength = MaximumMessageBytes };
        private Process process;
        private readonly object processLock = new object();
        private int nextId;
        private volatile bool disposed;
        private volatile bool broken;
        public bool IsConnected => !disposed && !broken;
        public event Action<Exception> ConnectionLost;
        public int ProcessId => process?.Id ?? 0;

        public Net10LanguageClient(string runtimeDirectory, string dotnetPath, string language)
        {
            this.runtimeDirectory = Path.GetFullPath(runtimeDirectory);
            this.dotnetPath = dotnetPath;
            this.language = language == "ru" ? "ru" : "en";
        }

        public async Task StartAsync()
        {
            string assembly = Path.Combine(runtimeDirectory, "PascalABCNet.LanguageServer.dll");
            if (!File.Exists(assembly)) throw new FileNotFoundException("Не найден LanguageServer .NET 10", assembly);
            lock (processLock)
            {
                if (disposed) throw new ObjectDisposedException(nameof(Net10LanguageClient));
                process = new Process
                {
                    StartInfo = new ProcessStartInfo(dotnetPath, "\"" + assembly + "\" --stdio --documentation-language " + language)
                    {
                        WorkingDirectory = runtimeDirectory, UseShellExecute = false, CreateNoWindow = true,
                        RedirectStandardInput = true, RedirectStandardOutput = true, RedirectStandardError = true,
                        StandardErrorEncoding = Encoding.UTF8
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null) Trace.TraceWarning("LanguageServer: {0}", e.Data);
                };
                if (!process.Start()) throw new IOException("Не удалось запустить LanguageServer");
                process.BeginErrorReadLine();
            }
            // This task catches every failure and completes pending requests; it cannot fault unobserved.
            _ = ReadResponsesAsync();
            await RequestAsync("initialize", new
            {
                processId = Process.GetCurrentProcess().Id,
                rootUri = new Uri(runtimeDirectory + Path.DirectorySeparatorChar).AbsoluteUri,
                capabilities = new { }
            }).ConfigureAwait(false);
            await NotifyAsync("initialized", new { }).ConfigureAwait(false);
        }

        public async Task<object> RequestAsync(string method, object parameters, CancellationToken cancellation = default(CancellationToken))
        {
            cancellation.ThrowIfCancellationRequested();
            int id = Interlocked.Increment(ref nextId);
            var completion = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            pending[id] = completion;
            try
            {
                using (cancellation.Register(() => completion.TrySetCanceled()))
                {
                    await SendAsync(new { jsonrpc = "2.0", id, method, @params = parameters }).ConfigureAwait(false);
                    if (await Task.WhenAny(completion.Task, Task.Delay(10000, lifetime.Token)).ConfigureAwait(false) != completion.Task)
                        throw new TimeoutException("LanguageServer не ответил за 10 секунд");
                    return await completion.Task.ConfigureAwait(false);
                }
            }
            finally { pending.TryRemove(id, out _); }
        }

        public Task NotifyAsync(string method, object parameters) =>
            SendAsync(new { jsonrpc = "2.0", method, @params = parameters });

        private async Task SendAsync(object message)
        {
            Task sending = SendCoreAsync(message);
            if (await Task.WhenAny(sending, Task.Delay(10000, lifetime.Token)).ConfigureAwait(false) != sending)
            {
                Dispose(); // Also releases blocked pipe writes when the server stops reading stdin.
                _ = sending.ContinueWith(task => { var ignored = task.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
                throw new TimeoutException("LanguageServer не принимает сообщения");
            }
            await sending.ConfigureAwait(false);
        }

        private async Task SendCoreAsync(object message)
        {
            await writes.WaitAsync(lifetime.Token).ConfigureAwait(false);
            try
            {
                if (!IsConnected || process == null || process.HasExited) throw new IOException("LanguageServer завершился");
                byte[] body = Encoding.UTF8.GetBytes(json.Serialize(message));
                if (body.Length > MaximumMessageBytes) throw new IOException("Слишком большое сообщение LanguageServer");
                byte[] header = Encoding.ASCII.GetBytes("Content-Length: " + body.Length.ToString(CultureInfo.InvariantCulture) + "\r\n\r\n");
                var input = process.StandardInput.BaseStream;
                await input.WriteAsync(header, 0, header.Length, lifetime.Token).ConfigureAwait(false);
                await input.WriteAsync(body, 0, body.Length, lifetime.Token).ConfigureAwait(false);
                await input.FlushAsync(lifetime.Token).ConfigureAwait(false);
            }
            finally { writes.Release(); }
        }

        // Bound headers and payload; byte count is UTF-8 bytes, never character count.
        internal static async Task<string> ReadMessageAsync(Stream stream, CancellationToken cancellation)
        {
            var header = new List<byte>();
            var single = new byte[1];
            while (true)
            {
                if (await stream.ReadAsync(single, 0, 1, cancellation).ConfigureAwait(false) == 0)
                    throw new EndOfStreamException("LanguageServer закрыл stdout");
                header.Add(single[0]);
                int n = header.Count;
                if (n > 8192) throw new IOException("Слишком большой заголовок LSP");
                if (n >= 4 && header[n - 4] == 13 && header[n - 3] == 10 && header[n - 2] == 13 && header[n - 1] == 10) break;
            }
            int length = -1;
            foreach (string line in Encoding.ASCII.GetString(header.ToArray()).Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                if (line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase))
                {
                    if (length != -1 || !int.TryParse(line.Substring(15).Trim(), NumberStyles.None,
                        CultureInfo.InvariantCulture, out length)) throw new IOException("Некорректный Content-Length LSP");
                }
            if (length < 0 || length > MaximumMessageBytes) throw new IOException("Некорректный размер сообщения LSP");
            var body = new byte[length];
            int offset = 0;
            while (offset < length)
            {
                int read = await stream.ReadAsync(body, offset, length - offset, cancellation).ConfigureAwait(false);
                if (read == 0) throw new EndOfStreamException("Неполное сообщение LSP");
                offset += read;
            }
            return new UTF8Encoding(false, true).GetString(body);
        }

        private async Task ReadResponsesAsync()
        {
            Exception failure;
            try
            {
                while (!lifetime.IsCancellationRequested)
                {
                    string message = await ReadMessageAsync(process.StandardOutput.BaseStream, lifetime.Token).ConfigureAwait(false);
                    // Serializer instances are not shared with the writer thread.
                    var response = new JavaScriptSerializer { MaxJsonLength = MaximumMessageBytes }
                        .Deserialize<Dictionary<string, object>>(message);
                    if (!response.TryGetValue("id", out object value) || value == null) continue;
                    int id = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                    if (!pending.TryRemove(id, out var completion)) continue;
                    if (response.ContainsKey("error")) completion.TrySetException(new IOException("Ошибка запроса LanguageServer"));
                    else { response.TryGetValue("result", out var result); completion.TrySetResult(result); }
                }
                failure = new ObjectDisposedException(nameof(Net10LanguageClient));
            }
            catch (Exception error) { failure = error; }
            broken = true;
            foreach (var item in pending)
                if (pending.TryRemove(item.Key, out var completion)) completion.TrySetException(failure);
            if (!disposed)
                try { ConnectionLost?.Invoke(failure); }
                catch (Exception error) { Trace.TraceWarning("LSP connection subscriber: {0}", error.Message); }
        }

        public async Task StopAsync()
        {
            try
            {
                if (process == null || process.HasExited) return;
                await RequestAsync("shutdown", null).ConfigureAwait(false);
                await NotifyAsync("exit", null).ConfigureAwait(false);
                await Task.Run(() => process.WaitForExit(1500)).ConfigureAwait(false);
            }
            finally { Dispose(); }
        }

        public void Dispose()
        {
            lock (processLock)
            {
                if (disposed) return;
                disposed = true;
                lifetime.Cancel();
                try { if (process != null && !process.HasExited) process.Kill(); }
                catch (InvalidOperationException) { }
                catch (System.ComponentModel.Win32Exception) { }
                finally { process?.Dispose(); }
            }
            // Closing redirected streams also releases pending pipe I/O on .NET Framework.
        }
    }
}
