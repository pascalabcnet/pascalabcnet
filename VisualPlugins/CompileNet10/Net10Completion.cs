using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using VisualPascalABC;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10CompletionContext
    {
        internal readonly ICodeFileDocument Document;
        internal readonly string FileName, Text;
        internal readonly int Offset;
        internal Net10CompletionContext(ICodeFileDocument document)
        {
            Document = document;
            FileName = document.FileName;
            Text = document.TextEditor.Document.TextContent;
            Offset = document.TextEditor.ActiveTextAreaControl.Caret.Offset;
        }

        // Permit typing a member-name prefix while awaiting LSP; reject every other edit.
        internal bool TryPrefix(string text, out string prefix)
        {
            prefix = null;
            int length = text.Length - Text.Length;
            if (length < 0 || Offset > text.Length || Offset + length > text.Length ||
                !text.StartsWith(Text.Substring(0, Offset), StringComparison.Ordinal) ||
                !text.EndsWith(Text.Substring(Offset), StringComparison.Ordinal)) return false;
            prefix = text.Substring(Offset, length);
            foreach (char ch in prefix) if (!char.IsLetterOrDigit(ch) && ch != '_') return false;
            return true;
        }

        internal bool IsCurrent(out string prefix)
        {
            prefix = null;
            return string.Equals(FileName, Document.FileName, StringComparison.Ordinal) &&
                TryPrefix(Document.TextEditor.Document.TextContent, out prefix) &&
                Document.TextEditor.ActiveTextAreaControl.Caret.Offset == Offset + prefix.Length;
        }
    }

    // Subscribed ONLY in modern mode. TextArea's multicast key event returns the LAST handler's result.
    internal sealed class Net10Completion : IDisposable
    {
        private sealed class Subscription
        {
            public ICSharpCode.TextEditor.KeyEventHandler Key;
            public DialogKeyProcessor Dialog;
            public EventHandler Caret, LostFocus;
            public DocumentEventHandler Changed;
        }

        private readonly Form parent;
        private readonly IWorkbenchDocumentEvents events;
        private readonly Net10LanguageDocuments documents;
        private readonly Func<bool> enabled;
        private readonly Func<TextArea, bool> packageDirectiveAtCaret;
        private readonly Func<TextArea, bool> packageCompletion;
        private readonly Dictionary<ICodeFileDocument, Subscription> subscriptions = new Dictionary<ICodeFileDocument, Subscription>();
        private CodeCompletionImagesProvider images;
        private PABCNETCodeCompletionWindow window;
        private Net10CompletionContext pending;
        private CancellationTokenSource cancellation;
        private ICodeFileDocument scheduled;
        private long generation;
        private bool active, disposed;
        internal event Action Opening;

        public Net10Completion(Form parent, IWorkbenchDocumentEvents events, Net10LanguageDocuments documents, Func<bool> enabled)
        {
            this.parent = parent; this.events = events; this.documents = documents; this.enabled = enabled;
            packageDirectiveAtCaret = IsPackageDirectiveAtCaret;
            packageCompletion = RequestPackageCompletion;
            events.DocumentOpened += Opened;
            events.DocumentClosed += Closed;
            events.DocumentRenamed += Renamed;
        }

        public void SetActive(bool value)
        {
            if (disposed || active == value) return;
            active = value;
            if (active)
            {
                CodeCompletionAllNamesAction.PackageDirectiveAtCaret = packageDirectiveAtCaret;
                CodeCompletionAllNamesAction.PackageCompletion = packageCompletion;
                foreach (var document in events.GetOpenDocuments()) Attach(document);
            }
            else
            {
                if (ReferenceEquals(CodeCompletionAllNamesAction.PackageDirectiveAtCaret, packageDirectiveAtCaret))
                    CodeCompletionAllNamesAction.PackageDirectiveAtCaret = null;
                if (ReferenceEquals(CodeCompletionAllNamesAction.PackageCompletion, packageCompletion))
                    CodeCompletionAllNamesAction.PackageCompletion = null;
                Invalidate(); CloseWindow();
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
                Key = ch => Key(document, ch),
                Dialog = key =>
                {
                    if (key == (Keys.Control | Keys.Space) && RequestPackageCompletion(document)) return true;
                    if (key == Keys.Escape || scheduled != null) Invalidate();
                    return false;
                },
                Caret = (sender, e) => { if (pending?.Document == document && !pending.IsCurrent(out _)) Invalidate(); },
                LostFocus = (sender, e) => { if (pending?.Document == document || scheduled == document) Invalidate(); },
                Changed = (sender, e) =>
                {
                    if (pending != null && (pending.Document != document || !pending.TryPrefix(document.TextEditor.Document.TextContent, out _)))
                        Invalidate();
                }
            };
            subscriptions.Add(document, handlers);
            area.KeyEventHandler += handlers.Key;
            area.DoProcessDialogKey += handlers.Dialog;
            area.Caret.PositionChanged += handlers.Caret;
            area.LostFocus += handlers.LostFocus;
            document.TextEditor.Document.DocumentChanged += handlers.Changed;
        }

        private void Detach(ICodeFileDocument document)
        {
            if (!subscriptions.TryGetValue(document, out var handlers)) return;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            area.KeyEventHandler -= handlers.Key;
            area.DoProcessDialogKey -= handlers.Dialog;
            area.Caret.PositionChanged -= handlers.Caret;
            area.LostFocus -= handlers.LostFocus;
            document.TextEditor.Document.DocumentChanged -= handlers.Changed;
            subscriptions.Remove(document);
        }

        private void Opened(object sender, WorkbenchDocumentEventArgs e) { Attach(e.Document); Invalidate(); }
        private void Closed(object sender, WorkbenchDocumentEventArgs e) { Invalidate(); CloseWindow(); Detach(e.Document); }
        private void Renamed(object sender, WorkbenchDocumentEventArgs e) { Invalidate(); CloseWindow(); Detach(e.Document); Attach(e.Document); }

        private bool Key(ICodeFileDocument document, char ch)
        {
            if (scheduled != null) Invalidate(); // Another keystroke overtook the posted trigger callback.
            if (!active || !enabled()) { Invalidate(); CloseWindow(); return false; }
            if (window != null && window.ProcessKeyEvent(ch)) return true;
            if (ch != '.') return false;
            if (Net10PackageDirectiveContext.Parse(document.TextEditor.Document.TextContent,
                document.TextEditor.ActiveTextAreaControl.Caret.Offset) != null) return false;
            Invalidate(); CloseWindow();
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            string before = document.TextEditor.Document.TextContent;
            long ticket = generation;
            scheduled = document;
            // The normal editor path inserts the dot once. Request its final text/UTF-16 caret afterwards.
            area.BeginInvoke(new Action(() =>
            {
                if (ticket != generation) return;
                scheduled = null;
                int offset = area.Caret.Offset;
                // Use the editor's actual result, including selection replacement/overwrite/virtual space.
                if (!CanShow(document) || offset < 1 || document.TextEditor.Document.TextContent == before ||
                    document.TextEditor.Document.GetCharAt(offset - 1) != '.') return;
                pending = new Net10CompletionContext(document);
                cancellation = new CancellationTokenSource();
                _ = RequestAsync(pending, ticket, cancellation.Token);
            }));
            return false;
        }

        private bool CanShow(ICodeFileDocument document) => !disposed && active && enabled() &&
            subscriptions.ContainsKey(document) && !document.TextEditor.IsDisposed && document.TextEditor.ActiveTextAreaControl.TextArea.Focused;

        private ICodeFileDocument DocumentFor(TextArea area)
        {
            foreach (var document in subscriptions.Keys)
                if (document.TextEditor.ActiveTextAreaControl.TextArea == area) return document;
            return null;
        }

        private bool IsPackageDirectiveAtCaret(TextArea area)
        {
            if (!active || !enabled()) return false;
            var document = DocumentFor(area);
            return document != null && Net10PackageDirectiveContext.Parse(
                area.Document.TextContent, area.Caret.Offset) != null;
        }

        private bool RequestPackageCompletion(TextArea area)
        {
            var document = DocumentFor(area);
            return document != null && RequestPackageCompletion(document);
        }

        private bool RequestPackageCompletion(ICodeFileDocument document)
        {
            if (!CanShow(document)) return false;
            string text = document.TextEditor.Document.TextContent;
            int offset = document.TextEditor.ActiveTextAreaControl.Caret.Offset;
            var directive = Net10PackageDirectiveContext.Parse(text, offset);
            if (directive == null) return false;
            Invalidate(); CloseWindow();
            var context = new Net10CompletionContext(document);
            pending = context;
            cancellation = new CancellationTokenSource();
            _ = RequestPackageAsync(context, directive, generation, cancellation.Token);
            return true;
        }

        private async Task RequestPackageAsync(Net10CompletionContext context,
            Net10PackageDirectiveContext directive, long ticket, CancellationToken token)
        {
            try
            {
                object response = await documents.DiscoverPackagesAsync(context.Document,
                    directive.Prefix, directive.PackageId, token);
                if (token.IsCancellationRequested || ticket != generation || !CanShow(context.Document) ||
                    !string.Equals(context.Text, context.Document.TextEditor.Document.TextContent,
                        StringComparison.Ordinal) ||
                    context.Document.TextEditor.ActiveTextAreaControl.Caret.Offset != context.Offset) return;
                images = images ?? new CodeCompletionImagesProvider();
                var provider = new Net10PackageCompletionProvider(response, images, directive);
                pending = null;
                if (provider.Items.Length == 0) return;
                Opening?.Invoke();
                window = PABCNETCodeCompletionWindow.ShowExternalCompletionWindow(parent,
                    context.Document.TextEditor, provider, provider.Items, images, true);
                if (window != null)
                {
                    var shown = window;
                    shown.Closed += (sender, e) => { if (ReferenceEquals(window, shown)) window = null; };
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception error) { Trace.TraceWarning("NuGet completion: {0}", error); }
            finally
            {
                if (ticket == generation) { pending = null; cancellation?.Dispose(); cancellation = null; }
            }
        }

        private async Task RequestAsync(Net10CompletionContext context, long ticket, CancellationToken token)
        {
            try
            {
                var position = context.Document.TextEditor.Document.OffsetToPosition(context.Offset);
                object response = await documents.CompleteAsync(context.Document, position.Y, position.X, token);
                // Captured WinForms context: no editor/control access on the transport thread.
                if (token.IsCancellationRequested || ticket != generation || !CanShow(context.Document) || !context.IsCurrent(out var prefix)) return;
                images = images ?? new CodeCompletionImagesProvider();
                var provider = new Net10CompletionProvider(response, images, prefix);
                pending = null;
                Opening?.Invoke(); // Avoid two dialog-key consumers for completion and parameter help.
                window = PABCNETCodeCompletionWindow.ShowExternalCompletionWindow(parent, context.Document.TextEditor, provider, provider.Items, images);
                if (window != null)
                {
                    var shown = window;
                    shown.Closed += (sender, e) => { if (ReferenceEquals(window, shown)) window = null; };
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception error) { Trace.TraceWarning("LSP completion: {0}", error); }
            finally
            {
                if (ticket == generation) { pending = null; cancellation?.Dispose(); cancellation = null; }
            }
        }

        private void Invalidate()
        {
            generation++;
            pending = null; scheduled = null;
            cancellation?.Cancel(); cancellation?.Dispose(); cancellation = null;
        }
        private void CloseWindow() { var old = window; window = null; old?.Close(); }
        public void Dispose()
        {
            if (disposed) return;
            SetActive(false); disposed = true;
            events.DocumentOpened -= Opened; events.DocumentClosed -= Closed; events.DocumentRenamed -= Renamed;
            images?.ImageList.Dispose();
        }
    }
}
