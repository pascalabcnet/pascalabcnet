using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private sealed class FakeLanguageClient : INet10LanguageClient
    {
        public event Action<Net10PackageRestoreRequired> PackageRestoreRequiredChanged;
        internal void RequirePackages(string uri, bool required) =>
            PackageRestoreRequiredChanged?.Invoke(new Net10PackageRestoreRequired { uri = uri, required = required });
        public event Action<Net10RestoreProgress> RestoreProgressChanged { add { } remove { } }
        public bool IsRestoringPackages => false;
        public Task CancelPackageRestoreAsync() => Task.CompletedTask;
        public readonly ConcurrentQueue<Tuple<string, Dictionary<string, object>>> Messages =
            new ConcurrentQueue<Tuple<string, Dictionary<string, object>>>();
        public int Starts, Stops;
        public bool Disposed, FailNext;
        public Func<object, CancellationToken, Task<object>> Completion;
        public Func<string, object, CancellationToken, Task<object>> Request;
        public bool IsConnected { get; private set; } = true;
        public event Action<Exception> ConnectionLost;
        public void LoseConnection() { IsConnected = false; ConnectionLost?.Invoke(new IOException("idle test crash")); }
        public Task StartAsync() { Starts++; return Task.CompletedTask; }
        public Task NotifyAsync(string method, object parameters)
        {
            if (FailNext) { FailNext = false; throw new IOException("test disconnection"); }
            var json = new JavaScriptSerializer();
            Messages.Enqueue(Tuple.Create(method, json.Deserialize<Dictionary<string, object>>(json.Serialize(parameters))));
            return Task.CompletedTask;
        }
        public Task StopAsync() { Stops++; return Task.CompletedTask; }
        public Task<object> RequestAsync(string method, object parameters, CancellationToken cancellation = default(CancellationToken))
        {
            cancellation.ThrowIfCancellationRequested();
            NotifyAsync(method, parameters);
            if (Request != null) return Request(method, parameters, cancellation);
            return Completion != null ? Completion(parameters, cancellation) : Task.FromResult<object>(null);
        }
        public void Dispose() { Disposed = true; IsConnected = false; }
    }

    private static void CheckManualPackageRequests()
    {
        using (var document = new EditorDocument { FileName = Path.Combine(Path.GetTempPath(), "Manual packages.pas") })
        {
            document.TextEditor.Document.TextContent = "begin end.";
            var open = new List<ICodeFileDocument> { document };
            var events = new WorkbenchDocumentEventSource(() => open);
            var fake = new FakeLanguageClient();
            var sync = new Net10LanguageDocuments(events, () => fake, _ => { });
            try
            {
                sync.SetActive(true);
                sync.WhenIdleAsync().GetAwaiter().GetResult();
                string uri = new Uri(document.FileName).AbsoluteUri;
                fake.RequirePackages(uri, true);
                Check(sync.NeedsPackageRestore(document), "server notification enables the action for the matching editor");
                CheckRestoreButtonTransitions(sync, fake, document);
                fake.RequirePackages(uri, true);
                sync.RestorePackagesAsync(document).GetAwaiter().GetResult();
                var request = fake.Messages.Last();
                Check(request.Item1 == "pascalabc/restoreNuGetPackages" && (string)request.Item2["uri"] == uri &&
                    !request.Item2.ContainsKey("textDocument") && !request.Item2.ContainsKey("position"),
                    "IDE button sends the custom restore request with the server's exact parameter shape");
                sync.RefreshPackagesAsync(document).GetAwaiter().GetResult();
                request = fake.Messages.Last();
                Check(request.Item1 == "pascalabc/refreshNuGetPackages" && (string)request.Item2["uri"] == uri,
                    "post-compilation refresh uses the same editor's URI without permitting downloads");
                fake.RequirePackages(uri, false);
                Check(!sync.NeedsPackageRestore(document), "successful preparation removes the action for this editor");
                fake.RequirePackages(uri, true);
                open.Clear(); events.Closed(document); sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(!sync.NeedsPackageRestore(document), "closing a document clears its pending action");
            }
            finally { sync.ShutdownAsync().GetAwaiter().GetResult(); }
        }
    }

    private static void CheckRestoreButtonTransitions(Net10LanguageDocuments sync, FakeLanguageClient client, EditorDocument document)
    {
        using (var form = new System.Windows.Forms.Form { Width = 850 })
        using (var strip = new System.Windows.Forms.StatusStrip { Name = "statusStrip1" })
        {
            var label = new System.Windows.Forms.ToolStripStatusLabel {
                Name = "toolStripStatusLabel5", Text = "Ready", Spring = true };
            strip.Items.Add(label); form.Controls.Add(strip);
            using (var status = new Net10StatusDisplay(form, ProgressResource))
            {
                ICodeFileDocument selected = document;
                string uri = new Uri(document.FileName).AbsoluteUri;
                status.SetPackageRestoreRequired(() => sync.NeedsPackageRestore(selected), null);
                var button = (System.Windows.Forms.ToolStripButton)strip.Items["Net10NuGetRestore"];
                Check(button.Available, "restore action starts visible for an editor with missing packages");
                int appearances = 0;
                button.AvailableChanged += (sender, e) => { if (button.Available) appearances++; };

                status.SetCompilationActive(true);
                // Real server notification reaches the adapter while compilation
                // still owns the UI; the 500ms language timer has not run again.
                client.RequirePackages(uri, false);
                status.ClearRestores("compiler");
                status.SetCompilationActive(false);
                Check(!button.Available && appearances == 0,
                    "ending compilation cannot briefly resurrect the pre-compilation restore action");
                for (int i = 0; i < 3; i++) status.SetLanguageRestoreVisible(true);
                Check(!button.Available && appearances == 0 && label.Text == "Ready",
                    "subsequent language timer ticks keep the action hidden without changing status text");

                client.RequirePackages(uri, true);
                status.SetLanguageRestoreVisible(true);
                Check(button.Available && appearances == 1,
                    "genuinely missing packages still offer restoration again");
                status.SetCompilationActive(true);
                status.SetCompilationActive(false);
                Check(button.Available && appearances == 2,
                    "failed compilation with packages still missing correctly restores the action");

                status.SetManualRestoreActive(true);
                client.RequirePackages(uri, false);
                int before = appearances;
                status.ClearRestores("language");
                status.SetManualRestoreActive(false);
                Check(!button.Available && appearances == before,
                    "manual restore completion also never flashes its obsolete action");

                client.RequirePackages(uri, true);
                selected = null;
                status.SetLanguageRestoreVisible(true);
                Check(!button.Available && appearances == before,
                    "a late notification for a different editor cannot show its restore action");
                client.RequirePackages(uri, false);
            }
        }
    }

    private sealed class FragmentedStream : MemoryStream
    {
        public FragmentedStream(byte[] data) : base(data) { }
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token) =>
            base.ReadAsync(buffer, offset, Math.Min(count, 1), token);
    }

    private sealed class StartingLanguageClient : INet10LanguageClient
    {
        public event Action<Net10PackageRestoreRequired> PackageRestoreRequiredChanged { add { } remove { } }
        public event Action<Net10RestoreProgress> RestoreProgressChanged { add { } remove { } }
        public bool IsRestoringPackages => false;
        public Task CancelPackageRestoreAsync() => Task.CompletedTask;
        public readonly TaskCompletionSource<bool> Starting = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource<bool> release = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        public bool IsConnected { get; private set; } = true;
        public event Action<Exception> ConnectionLost { add { } remove { } }
        public int Notifications;
        public Task StartAsync() { Starting.TrySetResult(true); return release.Task; }
        public Task NotifyAsync(string method, object parameters) { Notifications++; return Task.CompletedTask; }
        public Task StopAsync() => Task.CompletedTask;
        public Task<object> RequestAsync(string method, object parameters, CancellationToken cancellation = default(CancellationToken)) => Task.FromResult<object>(null);
        public void Dispose() { IsConnected = false; release.TrySetResult(true); }
    }

    private static Dictionary<string, object> ObjectValue(object value) => (Dictionary<string, object>)value;
    private static object[] ArrayValue(object value) => ((System.Collections.IEnumerable)value).Cast<object>().ToArray();
    private static object DocumentValue(Tuple<string, Dictionary<string, object>> message, string key) =>
        ObjectValue(message.Item2["textDocument"])[key];

    private static async Task CheckLanguageSynchronizationAsync(string runtime)
    {
        string text = "{Кириллица 😀}" + new string('я', 10000);
        byte[] body = Encoding.UTF8.GetBytes(text);
        byte[] wire = Encoding.ASCII.GetBytes("Content-Type: application/vscode-jsonrpc; charset=utf-8\r\nContent-Length: " +
            body.Length + "\r\n\r\n").Concat(body).ToArray();
        using (var stream = new FragmentedStream(wire))
            Check(await Net10LanguageClient.ReadMessageAsync(stream, CancellationToken.None) == text,
                "LSP framing accepts fragmented UTF-8, Cyrillic, surrogate pairs and large payloads");
        foreach (string header in new[] { "Content-Length: -1", "Content-Length: 16777217", "Other: 42", "Content-Length: 2\r\nContent-Length: 2" })
        {
            bool rejected = false;
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(header + "\r\n\r\n{}")))
                try { await Net10LanguageClient.ReadMessageAsync(stream, CancellationToken.None); }
                catch (IOException) { rejected = true; }
            Check(rejected, "invalid/bounded LSP header rejected: " + header.Replace("\r\n", "; "));
        }

        string unique = Path.Combine(Path.GetTempPath(), "LspSync-" + Guid.NewGuid().ToString("N"));
        using (var first = new EditorDocument { FileName = Path.Combine(unique, "Первая программа.pas") })
        using (var floating = new EditorDocument { FileName = Path.Combine(unique, "Floating.pas") })
        using (var other = new EditorDocument { FileName = Path.Combine(unique, "other.cs") })
        using (var metadata = new EditorDocument { FileName = Path.Combine(unique, "Metadata.pas"), FromMetadata = true })
        {
            first.TextEditor.Document.TextContent = "begin var s := 'кириллица'; s.Substring(1); end.";
            floating.TextEditor.Document.TextContent = "begin end.";
            var open = new List<ICodeFileDocument> { first, floating, other, metadata };
            var events = new WorkbenchDocumentEventSource(() => open);
            var clients = new List<FakeLanguageClient>();
            var sync = new Net10LanguageDocuments(events, () =>
            {
                var value = new FakeLanguageClient(); clients.Add(value); return value;
            }, value => { });
            try
            {
                sync.Flush();
                Check(clients.Count == 0, "legacy mode starts no LanguageServer");
                sync.SetActive(true);
                sync.WhenIdleAsync().GetAwaiter().GetResult();
                var fake = clients[0];
                Check(fake.Starts == 1 && fake.Messages.Count == 2 && fake.Messages.All(item => item.Item1 == "textDocument/didOpen"),
                    "activate opens unsaved and floating Pascal editors only, without a compiler DLL copy");
                Check(fake.Messages.Any(item => (string)DocumentValue(item, "text") == first.TextEditor.Document.TextContent),
                    "initial LSP snapshot contains unsaved editor text");
                first.TextEditor.Document.TextContent = "begin Print(2); end.";
                first.TextEditor.Document.TextContent = "begin Print(3); end.";
                sync.Flush();
                sync.WhenIdleAsync().GetAwaiter().GetResult();
                var change = fake.Messages.Last();
                Check(change.Item1 == "textDocument/didChange" && (int)DocumentValue(change, "version") == 2 &&
                    (string)ObjectValue(ArrayValue(change.Item2["contentChanges"])[0])["text"] == "begin Print(3); end.",
                    "debounced changes send latest full text and monotonic document version");
                int count = fake.Messages.Count;
                sync.Flush(); sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(fake.Messages.Count == count, "unchanged text and cancelled close emit no LSP notifications");
                string oldName = first.FileName;
                first.FileName = Path.Combine(unique, "После Save As.pas");
                events.Renamed(first, oldName);
                sync.WhenIdleAsync().GetAwaiter().GetResult();
                var last = fake.Messages.ToArray().Skip(count).ToArray();
                Check(last.Length == 2 && last[0].Item1 == "textDocument/didClose" && last[1].Item1 == "textDocument/didOpen" &&
                    (string)DocumentValue(last[0], "uri") == new Uri(oldName).AbsoluteUri &&
                    (string)DocumentValue(last[1], "uri") == new Uri(first.FileName).AbsoluteUri,
                    "Save As closes old URI before opening new URI with current text");
                open.Remove(floating); events.Closed(floating); sync.WhenIdleAsync().GetAwaiter().GetResult();
                count = fake.Messages.Count;
                floating.TextEditor.Document.TextContent = "begin Print(9); end.";
                sync.Flush(); sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(fake.Messages.Count == count && fake.Messages.Last().Item1 == "textDocument/didClose",
                    "successful close detaches editor and sends didClose, not later text changes");
                sync.SetActive(false); sync.WhenIdleAsync().GetAwaiter().GetResult();
                count = fake.Messages.Count;
                first.TextEditor.Document.TextContent = "begin Print(4); end.";
                sync.Flush(); sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(fake.Messages.Count == count && !fake.Disposed && fake.Stops == 0,
                    "return to net472 closes documents and suspends requests but retains server");
                sync.SetActive(true); sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(clients.Count == 1 && (string)DocumentValue(fake.Messages.Last(), "text") == "begin Print(4); end.",
                    "resume replays latest unsaved state without another server process");
                fake.FailNext = true;
                first.TextEditor.Document.TextContent = "begin Print(5); end.";
                sync.Flush(); sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(fake.Disposed && clients.Count == 1, "failed connection is released without endless automatic restarts");
                first.TextEditor.Document.TextContent = "begin Print(6); end.";
                sync.Flush(); sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(clients.Count == 2 && clients[1].Messages.First().Item1 == "textDocument/didOpen" &&
                    (string)DocumentValue(clients[1].Messages.First(), "text") == "begin Print(6); end.",
                    "next editor action reconnects and replays full snapshot after failure");
                clients[1].LoseConnection();
                sync.Flush(); sync.WhenIdleAsync().GetAwaiter().GetResult();
                Check(clients.Count == 3 && clients[1].Disposed && clients[2].Messages.First().Item1 == "textDocument/didOpen",
                    "idle server crash reconnects on next action, even with unchanged text");
            }
            finally { sync.ShutdownAsync().GetAwaiter().GetResult(); }
            Check(clients.Last().Stops == 1 && clients.Last().Disposed, "IDE shutdown detaches events and shuts down server once");
            int finalCount = clients.Last().Messages.Count;
            events.Opened(first);
            first.TextEditor.Document.TextContent = "begin Print(7); end.";
            sync.Flush();
            Check(clients.Last().Messages.Count == finalCount && !Directory.Exists(unique),
                "disposed synchronization ignores events and never writes unsaved source files");
        }

        // Close/exit while initialize is still pending: no callbacks may reopen the server afterwards.
        using (var document = new EditorDocument { FileName = Path.Combine(unique, "Closing.pas") })
        {
            var events = new WorkbenchDocumentEventSource(() => new[] { document });
            var starting = new StartingLanguageClient();
            var sync = new Net10LanguageDocuments(events, () => starting, value => { });
            sync.SetActive(true);
            starting.Starting.Task.GetAwaiter().GetResult();
            sync.AbortOnApplicationExit();
            sync.WhenIdleAsync().GetAwaiter().GetResult();
            Check(!starting.IsConnected && starting.Notifications == 0,
                "application exit during startup aborts server without sending late document events");
            sync.ShutdownAsync().GetAwaiter().GetResult();
        }

        await CheckRealLanguageClientAsync(runtime, unique).ConfigureAwait(false);
    }

    private static async Task CheckRealLanguageClientAsync(string runtime, string unique)
    {
        var client = new Net10LanguageClient(runtime, "dotnet", "ru");
        int pid = 0;
        try
        {
            await client.StartAsync().ConfigureAwait(false);
            pid = client.ProcessId;
            string uri = new Uri(Path.Combine(unique, "Несохранённый.pas")).AbsoluteUri;
            const string source = "begin var s := 'кириллица'; s.Substring(1); end.";
            await client.NotifyAsync("textDocument/didOpen", new
            {
                textDocument = new { uri, languageId = "pascal", version = 1, text = source }
            }).ConfigureAwait(false);
            object request = new { textDocument = new { uri }, position = new { line = 0, character = source.IndexOf("s.Substring", StringComparison.Ordinal) + 2 } };
            var result = ObjectValue(await client.RequestAsync("textDocument/completion", request).ConfigureAwait(false));
            Check(ArrayValue(result["items"]).Any(item => (string)ObjectValue(item)["label"] == "Substring"),
                "legacy net472 LSP transport synchronizes Cyrillic unsaved text with real net10 server");
            var hover = ObjectValue(await client.RequestAsync("textDocument/hover", new
            {
                textDocument = new { uri }, position = new { line = 0, character = source.IndexOf("Substring", StringComparison.Ordinal) + 1 }
            }).ConfigureAwait(false));
            Check(Net10HoverContext.Description(hover)?.Contains("Substring") == true,
                "real net10 server supplies hover signature through net472 transport for unsaved Cyrillic editor text");
            var signature = ObjectValue(await client.RequestAsync("textDocument/signatureHelp", new
            {
                textDocument = new { uri }, position = new { line = 0, character = source.IndexOf("Substring(", StringComparison.Ordinal) + "Substring(".Length }
            }).ConfigureAwait(false));
            Check(ArrayValue(signature["signatures"]).Length >= 2 && (int)signature["activeParameter"] == 0,
                "real server returns Substring overloads after opening bracket through net472 transport");
            string commaSource = source.Replace("Substring(1)", "Substring(1, 2)");
            await client.NotifyAsync("textDocument/didChange", new
            {
                textDocument = new { uri, version = 2 }, contentChanges = new[] { new { text = commaSource } }
            }).ConfigureAwait(false);
            signature = ObjectValue(await client.RequestAsync("textDocument/signatureHelp", new
            {
                textDocument = new { uri }, position = new { line = 0, character = commaSource.IndexOf("Substring(1,", StringComparison.Ordinal) + "Substring(1,".Length }
            }).ConfigureAwait(false));
            Check((int)signature["activeParameter"] == 1, "real server advances Substring parameter after comma");
            string incomplete = source.Substring(0, source.IndexOf("Substring(", StringComparison.Ordinal) + "Substring(".Length) + " end.";
            await client.NotifyAsync("textDocument/didChange", new
            {
                textDocument = new { uri, version = 3 }, contentChanges = new[] { new { text = incomplete } }
            }).ConfigureAwait(false);
            signature = ObjectValue(await client.RequestAsync("textDocument/signatureHelp", new
            {
                textDocument = new { uri }, position = new { line = 0, character = incomplete.IndexOf("Substring(", StringComparison.Ordinal) + "Substring(".Length }
            }).ConfigureAwait(false));
            Check(ArrayValue(signature["signatures"]).Length >= 2, "real server answers after interactively typed '(' in an unfinished call");
            await client.NotifyAsync("textDocument/didChange", new
            {
                textDocument = new { uri, version = 4 }, contentChanges = new[] { new { text = source.Replace("'кириллица'", "123") } }
            }).ConfigureAwait(false);
            // Query position points to the unchanged member-access offset in the NEW text.
            string changed = source.Replace("'кириллица'", "123");
            result = ObjectValue(await client.RequestAsync("textDocument/completion", new
            {
                textDocument = new { uri }, position = new { line = 0, character = changed.IndexOf("s.Substring", StringComparison.Ordinal) + 2 }
            }).ConfigureAwait(false));
            Check(!ArrayValue(result["items"]).Any(item => (string)ObjectValue(item)["label"] == "Substring"),
                "real server sees changed unsaved type, not a stale disk/editor model");
            await client.NotifyAsync("textDocument/didClose", new { textDocument = new { uri } }).ConfigureAwait(false);
            result = ObjectValue(await client.RequestAsync("textDocument/completion", request).ConfigureAwait(false));
            Check(ArrayValue(result["items"]).Length == 0, "real server removes closed unsaved document");
            await client.StopAsync().ConfigureAwait(false);
        }
        finally { client.Dispose(); }
        bool gone;
        try { using (var process = System.Diagnostics.Process.GetProcessById(pid)) gone = process.HasExited; }
        catch (ArgumentException) { gone = true; }
        Check(gone, "LSP shutdown/exit leaves no child server process");
    }
}
