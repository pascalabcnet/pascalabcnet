using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.InsightWindow;
using VisualPascalABC;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10Signature : IDisposable
    {
        private sealed class Subscription
        {
            public DocumentEventHandler Changed;
            public EventHandler Caret, LostFocus;
            public System.Windows.Forms.KeyEventHandler Key;
        }
        private readonly Form parent;
        private readonly IWorkbenchDocumentEvents events;
        private readonly Net10LanguageDocuments documents;
        private readonly Func<bool> enabled;
        private readonly Dictionary<ICodeFileDocument, Subscription> subscriptions = new Dictionary<ICodeFileDocument, Subscription>();
        private PABCNETInsightWindow window;
        private ICodeFileDocument windowDocument;
        private Net10SignatureContext pending;
        private CancellationTokenSource cancellation;
        private long generation;
        private bool active, disposed;

        internal Net10Signature(Form parent, IWorkbenchDocumentEvents events, Net10LanguageDocuments documents, Func<bool> enabled)
        {
            this.parent = parent; this.events = events; this.documents = documents; this.enabled = enabled;
            events.DocumentOpened += Opened; events.DocumentClosed += Closed; events.DocumentRenamed += Renamed;
        }

        internal void SetActive(bool value)
        {
            if (disposed || active == value) return;
            active = value;
            if (active) foreach (var document in events.GetOpenDocuments()) Attach(document);
            else
            {
                CloseHints();
                foreach (var document in new List<ICodeFileDocument>(subscriptions.Keys)) Detach(document);
            }
        }
        private void Attach(ICodeFileDocument document)
        {
            if (!active || document.FromMetadata || document.TextEditor == null || subscriptions.ContainsKey(document) ||
                !string.Equals(Path.GetExtension(document.FileName), ".pas", StringComparison.OrdinalIgnoreCase)) return;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            var handlers = new Subscription
            {
                Changed = (sender, e) => Changed(document, e),
                Caret = (sender, e) => { if (pending?.Document == document && !pending.IsCurrent) Invalidate(); },
                LostFocus = (sender, e) => LostFocus(document),
                Key = (sender, e) => { if (e.KeyCode == Keys.Escape) CloseHints(); }
            };
            subscriptions.Add(document, handlers);
            document.TextEditor.Document.DocumentChanged += handlers.Changed;
            area.Caret.PositionChanged += handlers.Caret; area.LostFocus += handlers.LostFocus; area.KeyDown += handlers.Key;
            // No extra multicast KeyEventHandler: completion's handled return must stay intact.
        }
        private void Detach(ICodeFileDocument document)
        {
            if (!subscriptions.TryGetValue(document, out var handlers)) return;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            document.TextEditor.Document.DocumentChanged -= handlers.Changed;
            area.Caret.PositionChanged -= handlers.Caret; area.LostFocus -= handlers.LostFocus; area.KeyDown -= handlers.Key;
            subscriptions.Remove(document);
        }
        private void Opened(object sender, WorkbenchDocumentEventArgs e) { CloseHints(); Attach(e.Document); }
        private void Closed(object sender, WorkbenchDocumentEventArgs e) { CloseHints(); Detach(e.Document); }
        private void Renamed(object sender, WorkbenchDocumentEventArgs e) { CloseHints(); Detach(e.Document); Attach(e.Document); }

        private void LostFocus(ICodeFileDocument document)
        {
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            if (area.IsDisposed || !area.IsHandleCreated) return;
            long ticket = generation;
            // ShowInsightWindow calls TextEditorControl.Focus(), temporarily moving focus from
            // TextArea to its container and back. Check the settled focus, not that transient event.
            area.BeginInvoke(new Action(() =>
            {
                if (ticket == generation && !area.Focused && window?.ContainsFocus != true) CloseHints();
            }));
        }

        private void Changed(ICodeFileDocument document, DocumentEventArgs e)
        {
            if (!active || !enabled()) { CloseHints(); return; }
            if ((pending != null && pending.Document != document) || (windowDocument != null && windowDocument != document))
            { CloseHints(); return; }
            if (windowDocument == document && window?.GetInsightProvider() is Net10SignatureProvider current && !current.Context.RequestPrefixUnchanged)
                CloseHints(); // A same-position edit before the trigger may emit no caret event.
            if (pending != null && !pending.TryText(document.TextEditor.Document.TextContent)) Invalidate();
            if (string.IsNullOrEmpty(e.Text) || !Net10SignatureContext.IsTrigger(e.Text[e.Text.Length - 1])) return;
            var context = Net10SignatureContext.Capture(document, e.Offset + e.Text.Length);
            if (context == null) return; // Quotes/comments and commas without an open call do not trigger requests.
            Invalidate(); pending = context;
            cancellation = new CancellationTokenSource();
            long ticket = generation; var token = cancellation.Token;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            area.BeginInvoke(new Action(() =>
            {
                if (ticket != generation) return;
                if (!CanShow(document) || !context.IsCurrent) { Invalidate(); return; }
                _ = RequestAsync(context, ticket, token);
            }));
        }
        private bool CanShow(ICodeFileDocument document) => !disposed && active && enabled() && subscriptions.ContainsKey(document) &&
            !document.TextEditor.IsDisposed && document.TextEditor.ActiveTextAreaControl.TextArea.Focused;
        private async Task RequestAsync(Net10SignatureContext context, long ticket, CancellationToken token)
        {
            try
            {
                var position = context.Document.TextEditor.Document.OffsetToPosition(context.Offset);
                object response = await documents.SignatureAsync(context.Document, position.Y, position.X, token);
                if (ticket != generation || token.IsCancellationRequested || !CanShow(context.Document) || !context.IsCurrent) return;
                var provider = new Net10SignatureProvider(response, context);
                if (provider.InsightDataCount == 0) return;
                pending = null;
                if (window == null)
                {
                    windowDocument = context.Document;
                    window = new PABCNETInsightWindow(parent, context.Document.TextEditor);
                    window.Font = new Font(Constants.CompletionInsightWindowFontName, window.Font.Size);
                    var shown = window;
                    shown.Closed += (sender, e) => { if (ReferenceEquals(window, shown)) { window = null; windowDocument = null; Invalidate(); } };
                }
                var previous = window.InsightProviderStackLength > 0 ? window.GetInsightProvider() as Net10SignatureProvider : null;
                if (previous?.Context.OpenOffset == context.OpenOffset)
                {
                    if (window.GetCurrentData() != previous.DefaultIndex)
                        provider.DefaultIndex = Math.Min(window.GetCurrentData(), provider.InsightDataCount - 1);
                    window.ReplaceCurrentInsightDataProvider(provider, context.FileName);
                }
                else window.AddInsightDataProvider(provider, context.FileName); // Native stack restores the outer call after ')'.
                window.ShowInsightWindow();
            }
            catch (OperationCanceledException) { }
            catch (Exception error) { Trace.TraceWarning("LSP signature: {0}", error); }
            finally { if (ticket == generation) { pending = null; cancellation?.Dispose(); cancellation = null; } }
        }
        private void Invalidate()
        {
            generation++; pending = null;
            cancellation?.Cancel(); cancellation?.Dispose(); cancellation = null;
        }
        internal void CloseHints()
        {
            Invalidate(); var old = window; window = null; windowDocument = null; old?.Close();
        }
        public void Dispose()
        {
            if (disposed) return;
            SetActive(false); disposed = true;
            events.DocumentOpened -= Opened; events.DocumentClosed -= Closed; events.DocumentRenamed -= Renamed;
        }
    }
}
