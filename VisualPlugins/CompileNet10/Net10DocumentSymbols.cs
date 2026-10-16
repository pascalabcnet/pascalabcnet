using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using VisualPascalABC;

namespace VisualPascalABCPlugins
{
    // The IDE owns the controls; the plugin only supplies detached LSP symbols.
    internal sealed class Net10DocumentSymbols : IDisposable
    {
        private readonly IWorkbenchDocumentEvents events;
        private readonly IWorkbenchDocumentService documentService;
        private readonly Net10LanguageDocuments documents;
        private readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 450 };
        private readonly Dictionary<ICodeFileDocument, IDocument> attached = new Dictionary<ICodeFileDocument, IDocument>();
        private ICodeFileDocument current;
        private CancellationTokenSource request;
        private long generation;
        private bool modern, enabled, dirty, disposed;

        internal Net10DocumentSymbols(IWorkbenchDocumentEvents events, IWorkbenchDocumentService documentService,
            Net10LanguageDocuments documents)
        {
            this.events = events;
            this.documentService = documentService;
            this.documents = documents;
            events.DocumentOpened += Opened;
            events.DocumentClosed += Closed;
            events.DocumentRenamed += Renamed;
            timer.Tick += Tick;
            timer.Start();
        }

        internal void SetActive(bool modernMode, bool showSymbols)
        {
            if (disposed) return;
            if (modern != modernMode)
            {
                modern = modernMode;
                foreach (var document in events.GetOpenDocuments())
                {
                    Attach(document);
                    Panel(document)?.SetModernMode(modern);
                }
                Cancel();
                current = null;
                dirty = true;
            }
            if (enabled != showSymbols)
            {
                enabled = showSymbols;
                Cancel();
                dirty = true;
            }
        }

        private static QuickClassBrowserPanel Panel(ICodeFileDocument document) =>
            (document?.TextEditor as CodeFileDocumentTextEditorControl)?.NavigationPanel;

        private static bool IsPascal(ICodeFileDocument document) => document != null && !document.FromMetadata &&
            string.Equals(Path.GetExtension(document.FileName), ".pas", StringComparison.OrdinalIgnoreCase);

        private void Attach(ICodeFileDocument document)
        {
            if (!IsPascal(document) || attached.ContainsKey(document)) return;
            var editor = document.TextEditor?.Document;
            if (editor == null) return;
            attached.Add(document, editor);
            editor.DocumentChanged += Changed;
        }

        private void Detach(ICodeFileDocument document)
        {
            if (!attached.TryGetValue(document, out var editor)) return;
            editor.DocumentChanged -= Changed;
            attached.Remove(document);
        }

        private void Opened(object sender, WorkbenchDocumentEventArgs e)
        {
            Attach(e.Document);
            Panel(e.Document)?.SetModernMode(modern);
            dirty = true;
        }
        private void Closed(object sender, WorkbenchDocumentEventArgs e)
        {
            Detach(e.Document);
            if (ReferenceEquals(current, e.Document)) { Cancel(); current = null; }
        }
        private void Renamed(object sender, WorkbenchDocumentEventArgs e)
        {
            Cancel();
            if (ReferenceEquals(current, e.Document)) current = null;
            dirty = true;
        }
        private void Changed(object sender, DocumentEventArgs e)
        {
            dirty = true;
            Cancel();
        }

        private void Tick(object sender, EventArgs e)
        {
            if (disposed || !modern) return;
            var selected = documentService.CurrentCodeFileDocument;
            if (!IsPascal(selected) || !attached.ContainsKey(selected)) return;
            if (!ReferenceEquals(current, selected))
            {
                Cancel();
                current = selected;
                dirty = true;
            }
            if (!enabled || !dirty || Panel(selected)?.Visible != true) return;
            dirty = false;
            var tokenSource = new CancellationTokenSource();
            request = tokenSource;
            var ticket = ++generation;
            var text = selected.TextEditor.Document.TextContent;
            _ = FetchAsync(selected, text, ticket, tokenSource.Token);
        }

        private async Task FetchAsync(ICodeFileDocument document, string text, long ticket, CancellationToken token)
        {
            try
            {
                var response = await documents.SymbolsAsync(document, token).ConfigureAwait(false);
                var symbols = Parse(response);
                var panel = Panel(document);
                if (panel == null || panel.IsDisposed || !panel.IsHandleCreated) return;
                panel.BeginInvoke(new Action(() =>
                {
                    if (!disposed && enabled && modern && ticket == generation && !token.IsCancellationRequested &&
                        ReferenceEquals(current, document) && document.TextEditor.Document.TextContent == text)
                        panel.SetModernSymbols(symbols);
                }));
            }
            catch (OperationCanceledException) { }
            catch (Exception error) { Trace.TraceWarning("LSP document symbols: {0}", error); }
        }

        internal static List<QuickClassBrowserPanel.ModernBrowserSymbol> Parse(object response)
        {
            var result = new List<QuickClassBrowserPanel.ModernBrowserSymbol>();
            if (!(response is IEnumerable items)) return result;
            foreach (var item in items)
                if (item is IDictionary<string, object> data && TryParse(data, out var symbol)) result.Add(symbol);
            return result;
        }

        private static bool TryParse(IDictionary<string, object> data,
            out QuickClassBrowserPanel.ModernBrowserSymbol symbol)
        {
            symbol = null;
            if (!data.TryGetValue("name", out var name) || !(name is string text) ||
                !data.TryGetValue("kind", out var kindValue) || !(kindValue is int kind) ||
                !data.TryGetValue("range", out var rangeValue) || !(rangeValue is IDictionary<string, object> range) ||
                !TryPoint(range, "start", out var startLine, out var startCharacter) ||
                !TryPoint(range, "end", out var endLine, out var endCharacter)) return false;
            string mappedKind;
            switch (kind)
            {
                case 5: mappedKind = "Class"; break;
                case 6: case 12: mappedKind = "Method"; break;
                case 7: mappedKind = "Property"; break;
                case 8: mappedKind = "Field"; break;
                case 10: mappedKind = "Enum"; break;
                case 11: mappedKind = "Interface"; break;
                case 14: mappedKind = "Constant"; break;
                case 23: mappedKind = "Struct"; break;
                case 24: mappedKind = "Event"; break;
                default: mappedKind = "Variable"; break;
            }
            symbol = new QuickClassBrowserPanel.ModernBrowserSymbol
            {
                Name = text,
                Detail = data.TryGetValue("detail", out var detail) ? detail as string : null,
                Kind = mappedKind,
                StartLine = startLine, StartCharacter = startCharacter,
                EndLine = endLine, EndCharacter = endCharacter,
                Children = data.TryGetValue("children", out var children) ? Parse(children) :
                    new List<QuickClassBrowserPanel.ModernBrowserSymbol>()
            };
            return true;
        }

        private static bool TryPoint(IDictionary<string, object> range, string key, out int line, out int character)
        {
            line = character = 0;
            return range.TryGetValue(key, out var value) && value is IDictionary<string, object> point &&
                point.TryGetValue("line", out var row) && row is int rowNumber && rowNumber >= 0 &&
                point.TryGetValue("character", out var column) && column is int columnNumber && columnNumber >= 0 &&
                (line = rowNumber) >= 0 && (character = columnNumber) >= 0;
        }

        private void Cancel()
        {
            generation++;
            request?.Cancel();
            request?.Dispose();
            request = null;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            timer.Stop(); timer.Dispose(); Cancel();
            foreach (var document in new List<ICodeFileDocument>(attached.Keys)) Detach(document);
            events.DocumentOpened -= Opened;
            events.DocumentClosed -= Closed;
            events.DocumentRenamed -= Renamed;
        }
    }
}
