using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;

namespace VisualPascalABCPlugins
{
    // UI-thread editor subscriptions. Only immutable text snapshots cross into the transport queue.
    internal sealed class Net10LanguageDocuments : IDisposable
    {
        private sealed class Snapshot
        {
            public string Uri, Text;
            public int Version;
        }

        private sealed class Batch
        {
            public Snapshot[] Items;
            public long Sequence;
        }

        private readonly IWorkbenchDocumentEvents events;
        private readonly Func<INet10LanguageClient> createClient;
        private readonly Action<string> report;
        private readonly System.Windows.Forms.Timer debounce = new System.Windows.Forms.Timer { Interval = 200 };
        private readonly Dictionary<ICodeFileDocument, IDocument> editors = new Dictionary<ICodeFileDocument, IDocument>();
        private readonly Dictionary<string, Snapshot> sent = new Dictionary<string, Snapshot>(StringComparer.OrdinalIgnoreCase);
        private readonly object queueLock = new object();
        private Batch desired;
        private long nextSequence, appliedSequence;
        private readonly SemaphoreSlim operations = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();
        private Task work = Task.CompletedTask;
        private bool working;
        private volatile bool active;
        private volatile bool disposed;
        private bool cleanupStarted;
        private INet10LanguageClient client;
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, bool> packageRequests =
            new System.Collections.Concurrent.ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        internal bool NeedsPackageRestore(ICodeFileDocument document) => document != null && !document.FromMetadata &&
            packageRequests.ContainsKey(new Uri(Path.GetFullPath(document.FileName)).AbsoluteUri);
        internal Task<object> RestorePackagesAsync(ICodeFileDocument document) =>
            RequestPositionAsync(document, 0, 0, "pascalabc/restoreNuGetPackages", CancellationToken.None);
        internal Task<object> RefreshPackagesAsync(ICodeFileDocument document) =>
            RequestPositionAsync(document, 0, 0, "pascalabc/refreshNuGetPackages", CancellationToken.None);
        internal event Action<Net10RestoreProgress> RestoreProgressChanged;
        internal bool IsRestoringPackages => client?.IsRestoringPackages == true;
        internal Task CancelPackageRestoreAsync() => client?.CancelPackageRestoreAsync() ?? Task.CompletedTask;

        public Net10LanguageDocuments(IWorkbenchDocumentEvents events, Func<INet10LanguageClient> createClient, Action<string> report)
        {
            this.events = events;
            this.createClient = createClient;
            this.report = report;
            events.DocumentOpened += Opened;
            events.DocumentClosed += Closed;
            events.DocumentRenamed += Renamed;
            debounce.Tick += (sender, e) => Flush();
        }

        public void SetActive(bool value)
        {
            if (disposed || active == value) return;
            active = value;
            debounce.Stop();
            if (active)
            {
                foreach (var document in events.GetOpenDocuments()) Attach(document);
                Flush(); // Initial snapshot includes inactive and unsaved tabs.
            }
            else
            {
                foreach (var editor in editors.Values) editor.DocumentChanged -= Changed;
                editors.Clear();
                Enqueue(new Snapshot[0]); // Keep server alive, but restore disk views until resume.
            }
        }

        private static bool IsPascal(ICodeFileDocument document) => !document.FromMetadata &&
            string.Equals(Path.GetExtension(document.FileName), ".pas", StringComparison.OrdinalIgnoreCase);

        private void Attach(ICodeFileDocument document)
        {
            if (!active || !IsPascal(document) || editors.ContainsKey(document)) return;
            var editor = document.TextEditor?.Document;
            if (editor == null) return;
            editors.Add(document, editor);
            editor.DocumentChanged += Changed;
        }

        private void Detach(ICodeFileDocument document)
        {
            if (!editors.TryGetValue(document, out var editor)) return;
            editor.DocumentChanged -= Changed;
            editors.Remove(document);
        }

        private void Opened(object sender, WorkbenchDocumentEventArgs e) { Attach(e.Document); if (active) Flush(); }
        private void Closed(object sender, WorkbenchDocumentEventArgs e) { Detach(e.Document); if (active) Flush(); }
        private void Renamed(object sender, WorkbenchDocumentEventArgs e)
        {
            Detach(e.Document);
            Attach(e.Document);
            if (active) Flush(); // diff sends didClose(old URI) before didOpen(new URI).
        }
        private void Changed(object sender, DocumentEventArgs e)
        {
            debounce.Stop();
            if (active && !disposed) debounce.Start();
        }

        // Also used by tests to flush the debounce without sleeping or a GUI message loop.
        internal void Flush()
        {
            debounce.Stop();
            if (disposed || !active) return;
            Enqueue(Capture());
        }

        private Snapshot[] Capture() => editors.Select(pair => new Snapshot
            {
                Uri = new Uri(Path.GetFullPath(pair.Key.FileName)).AbsoluteUri,
                Text = pair.Value.TextContent
            }).ToArray();

        private void Enqueue(Snapshot[] snapshots)
        {
            lock (queueLock)
            {
                desired = new Batch { Items = snapshots, Sequence = ++nextSequence }; // Coalesce editor changes.
                if (working) return;
                working = true;
                work = Task.Run(DrainAsync);
            }
        }

        internal Task WhenIdleAsync() { lock (queueLock) return work; }

        private async Task DrainAsync()
        {
            while (true)
            {
                Batch batch;
                lock (queueLock)
                {
                    batch = desired;
                    desired = null;
                    if (batch == null) { working = false; return; }
                }
                if (disposed) continue;
                await operations.WaitAsync().ConfigureAwait(false);
                try
                {
                    await ApplyAsync(batch).ConfigureAwait(false);
                }
                catch (Exception error) { ConnectionFailed(error); }
                finally { operations.Release(); }
            }
        }

        // Called on the IDE thread: capture ALL unsaved tabs before moving to the transport queue.
        internal Task<object> CompleteAsync(ICodeFileDocument document, int line, int character, CancellationToken cancellation)
            => RequestPositionAsync(document, line, character, "textDocument/completion", cancellation);

        // Discovery never enters the serialized document-analysis queue: a slow feed
        // must not delay edits, diagnostics or the ordinary member completion.
        internal Task<object> DiscoverPackagesAsync(ICodeFileDocument document, string prefix,
            string packageId, CancellationToken cancellation)
        {
            if (!active || disposed || !editors.ContainsKey(document) || client == null)
                return Task.FromResult<object>(null);
            string uri = new Uri(Path.GetFullPath(document.FileName)).AbsoluteUri;
            string method = packageId == null ? "pascalabc/searchNuGetPackages" :
                "pascalabc/nugetPackageVersions";
            return client.RequestAsync(method, new { uri, prefix, packageId }, cancellation);
        }

        internal Task<object> HoverAsync(ICodeFileDocument document, int line, int character, CancellationToken cancellation)
            => RequestPositionAsync(document, line, character, "textDocument/hover", cancellation);

        internal Task<object> SignatureAsync(ICodeFileDocument document, int line, int character, CancellationToken cancellation)
            => RequestPositionAsync(document, line, character, "textDocument/signatureHelp", cancellation);

        internal Task<object> NavigationAsync(ICodeFileDocument document, int line, int character, CancellationToken cancellation)
            => RequestPositionAsync(document, line, character, "textDocument/implementation", cancellation);

        internal Task<object> SymbolsAsync(ICodeFileDocument document, CancellationToken cancellation)
            => RequestPositionAsync(document, 0, 0, "textDocument/documentSymbol", cancellation);

        private Task<object> RequestPositionAsync(ICodeFileDocument document, int line, int character, string method, CancellationToken cancellation)
        {
            // Metadata is a generated view, not a source module. Analyze it on demand
            // for hover only, using a non-file URI so it cannot shadow String.pas on disk.
            bool metadata = document.FromMetadata && method == "textDocument/hover" &&
                events.GetOpenDocuments().Contains(document);
            if (!active || disposed || !editors.ContainsKey(document) && !metadata) return Task.FromResult<object>(null);
            debounce.Stop();
            string uri = metadata ? "pascalabc-metadata-view:///" + Uri.EscapeDataString(document.FileName) :
                new Uri(Path.GetFullPath(document.FileName)).AbsoluteUri;
            var snapshots = Capture();
            if (metadata) snapshots = snapshots.Concat(new[] { new Snapshot { Uri = uri, Text = document.TextEditor.Document.TextContent } }).ToArray();
            var batch = new Batch { Items = snapshots };
            lock (queueLock) batch.Sequence = ++nextSequence;
            return Task.Run(async () =>
            {
                using (var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellation, lifetime.Token))
                {
                    await operations.WaitAsync(linked.Token).ConfigureAwait(false);
                    try
                    {
                        linked.Token.ThrowIfCancellationRequested();
                        if (!active || disposed) return null;
                        await ApplyAsync(batch).ConfigureAwait(false);
                        linked.Token.ThrowIfCancellationRequested();
                        if (!active || disposed || client == null) return null;
                        var parameters = method.StartsWith("pascalabc/", StringComparison.Ordinal)
                            ? new Dictionary<string, object> { ["uri"] = uri }
                            : new Dictionary<string, object> { ["textDocument"] = new { uri } };
                        if (method != "textDocument/documentSymbol" && !method.StartsWith("pascalabc/", StringComparison.Ordinal))
                            parameters["position"] = new { line, character };
                        if (method == "textDocument/completion")
                            parameters["context"] = new { triggerKind = 2, triggerCharacter = "." };
                        var response = await client.RequestAsync(method, parameters, linked.Token).ConfigureAwait(false);
                        if (method == "textDocument/implementation")
                        {
                            if (response == null || response is System.Collections.ICollection empty && empty.Count == 0)
                                response = await client.RequestAsync("textDocument/definition", parameters, linked.Token).ConfigureAwait(false);
                            foreach (var location in Net10NavigationTarget.ReadLocations(response))
                                if (location.TryGetValue("uri", out var name) && name is string address &&
                                    Uri.TryCreate(address, UriKind.Absolute, out var target) && target.Scheme == "pascalabc-metadata")
                                    location["metadata"] = await client.RequestAsync("pascalabc/metadata", new { uri = address }, linked.Token).ConfigureAwait(false);
                        }
                        return response;
                    }
                    catch (OperationCanceledException) { throw; }
                    catch (Exception error) { ConnectionFailed(error); return null; }
                    finally { operations.Release(); }
                }
            });
        }

        private void ConnectionFailed(Exception error)
        {
            client?.Dispose();
            client = null;
            sent.Clear();
            if (!disposed) Report("LSP: ошибка — " + error.Message);
            // Retry only on the next editor/platform action, not an endless restart loop.
        }

        private async Task ApplyAsync(Batch batch)
        {
            if (disposed || batch.Sequence < appliedSequence) return;
            var snapshots = batch.Items;
            if (client != null && !client.IsConnected)
            {
                client.Dispose();
                client = null;
                sent.Clear();
            }
            if (client == null && snapshots.Length > 0)
            {
                Report("LSP: подключение…");
                client = createClient();
                if (disposed) { client.Dispose(); return; }
                var connection = client;
                packageRequests.Clear();
                connection.PackageRestoreRequiredChanged += value =>
                {
                    if (disposed || !ReferenceEquals(client, connection) || string.IsNullOrEmpty(value.uri)) return;
                    if (value.required) packageRequests[value.uri] = true;
                    else packageRequests.TryRemove(value.uri, out _);
                };
                connection.RestoreProgressChanged += value =>
                {
                    if (active && !disposed && ReferenceEquals(client, connection)) RestoreProgressChanged?.Invoke(value);
                };
                connection.ConnectionLost += error =>
                {
                    if (active && !disposed && ReferenceEquals(client, connection))
                        Report("LSP: связь потеряна — " + error.Message);
                };
                await client.StartAsync().ConfigureAwait(false);
            }
            if (disposed || client == null) return;
            var names = new HashSet<string>(snapshots.Select(item => item.Uri), StringComparer.OrdinalIgnoreCase);
            foreach (string uri in sent.Keys.Where(uri => !names.Contains(uri)).ToArray())
            {
                if (disposed) break;
                await client.NotifyAsync("textDocument/didClose", new { textDocument = new { uri } }).ConfigureAwait(false);
                sent.Remove(uri);
                packageRequests.TryRemove(uri, out _);
            }
            foreach (var snapshot in snapshots)
            {
                if (disposed) break;
                if (!sent.TryGetValue(snapshot.Uri, out var previous))
                {
                    snapshot.Version = 1;
                    await client.NotifyAsync("textDocument/didOpen", new
                    {
                        textDocument = new { uri = snapshot.Uri, languageId = "pascal", version = snapshot.Version, text = snapshot.Text }
                    }).ConfigureAwait(false);
                }
                else if (snapshot.Text != previous.Text)
                {
                    snapshot.Version = checked(previous.Version + 1);
                    await client.NotifyAsync("textDocument/didChange", new
                    {
                        textDocument = new { uri = snapshot.Uri, version = snapshot.Version },
                        contentChanges = new[] { new { text = snapshot.Text } }
                    }).ConfigureAwait(false);
                }
                else continue;
                sent[snapshot.Uri] = snapshot;
            }
            Report("LSP: " + sent.Keys.Count(uri => !uri.StartsWith("pascalabc-metadata-view:", StringComparison.Ordinal)) + " файлов");
            appliedSequence = batch.Sequence;
        }

        private void Report(string text)
        {
            if (disposed) return;
            try { report(text); }
            catch (Exception error) { System.Diagnostics.Trace.TraceWarning("LSP status: {0}", error.Message); }
        }

        internal async Task ShutdownAsync()
        {
            if (cleanupStarted) return;
            cleanupStarted = true;
            disposed = true;
            lifetime.Cancel();
            debounce.Stop();
            debounce.Dispose();
            events.DocumentOpened -= Opened;
            events.DocumentClosed -= Closed;
            events.DocumentRenamed -= Renamed;
            foreach (var editor in editors.Values) editor.DocumentChanged -= Changed;
            editors.Clear();
            Task queued = WhenIdleAsync();
            // An unresponsive server must not survive IDE closure or block the UI thread.
            Task shutdown = FinishAsync(queued);
            if (await Task.WhenAny(shutdown, Task.Delay(2000)).ConfigureAwait(false) != shutdown)
                client?.Dispose();
            await shutdown.ConfigureAwait(false);
        }

        private async Task FinishAsync(Task queued)
        {
            try { await queued.ConfigureAwait(false); if (client != null) await client.StopAsync().ConfigureAwait(false); }
            catch (Exception error) { System.Diagnostics.Trace.TraceWarning("LSP shutdown: {0}", error.Message); }
            finally { client?.Dispose(); }
        }

        public void Dispose() { _ = ShutdownAsync(); }
        // ApplicationExit may happen before asynchronous FormClosed continuations run.
        internal void AbortOnApplicationExit() { disposed = true; lifetime.Cancel(); client?.Dispose(); }
    }
}
