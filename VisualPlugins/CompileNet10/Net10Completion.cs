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
#if !PABC_LINUX_PLUGIN
using VisualPascalABC;
#endif

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
#if !PABC_LINUX_PLUGIN
        private readonly Func<TextArea, bool> packageDirectiveAtCaret;
        private readonly Func<TextArea, bool> packageCompletion;
#endif
        private readonly Dictionary<ICodeFileDocument, Subscription> subscriptions = new Dictionary<ICodeFileDocument, Subscription>();
        private readonly IExternalIntelliSenseSession ui;
        private Net10CompletionContext pending;
        private CancellationTokenSource cancellation;
        private ICodeFileDocument scheduled;
        private long generation;
        private bool active, disposed;
        internal event Action Opening;

        public Net10Completion(Form parent, IWorkbenchDocumentEvents events, Net10LanguageDocuments documents, Func<bool> enabled, IExternalIntelliSenseSession ui)
        {
            this.ui = ui ?? throw new ArgumentNullException(nameof(ui));
            this.parent = parent; this.events = events; this.documents = documents; this.enabled = enabled;
#if !PABC_LINUX_PLUGIN
            packageDirectiveAtCaret = IsPackageDirectiveAtCaret;
            packageCompletion = RequestPackageCompletion;
#endif
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
#if !PABC_LINUX_PLUGIN
                CodeCompletionAllNamesAction.PackageDirectiveAtCaret = packageDirectiveAtCaret;
                CodeCompletionAllNamesAction.PackageCompletion = packageCompletion;
#endif
                foreach (var document in events.GetOpenDocuments()) Attach(document);
            }
            else
            {
#if !PABC_LINUX_PLUGIN
                if (ReferenceEquals(CodeCompletionAllNamesAction.PackageDirectiveAtCaret, packageDirectiveAtCaret))
                    CodeCompletionAllNamesAction.PackageDirectiveAtCaret = null;
                if (ReferenceEquals(CodeCompletionAllNamesAction.PackageCompletion, packageCompletion))
                    CodeCompletionAllNamesAction.PackageCompletion = null;
#endif
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
            if (ui.ProcessCompletionKey(ch)) return true;
            if (ch != '.' && ch != ' ') return false;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            if (ch == ' ' && !FollowsNew(area.Document, area.Caret.Offset)) return false;
            string before = area.Document.TextContent;
            if (Net10PackageDirectiveContext.Parse(before, area.Caret.Offset) != null) return false;
            Invalidate(); CloseWindow();
            long ticket = generation;
            scheduled = document;
            // Let the editor insert the trigger once, then query its actual text and caret.
            area.BeginInvoke(new Action(() =>
            {
                if (ticket != generation) return;
                scheduled = null;
                int offset = area.Caret.Offset;
                // Use the editor's actual result, including selection replacement/overwrite/virtual space.
                if (!CanShow(document) || offset < 1 || document.TextEditor.Document.TextContent == before ||
                    document.TextEditor.Document.GetCharAt(offset - 1) != ch) return;
                pending = new Net10CompletionContext(document);
                cancellation = new CancellationTokenSource();
                _ = RequestAsync(pending, ticket, cancellation.Token, ch);
            }));
            return false;
        }

        internal static bool FollowsNew(IDocument document, int offset)
        {
            int start = offset - 3;
            if (start < 0 || offset > document.TextLength ||
                !string.Equals(document.GetText(start, 3), "new", StringComparison.OrdinalIgnoreCase))
                return false;
            if (start == 0) return true;
            char previous = document.GetCharAt(start - 1);
            return !char.IsLetterOrDigit(previous) && previous != '_' && previous != '&' && previous != '.';
        }

        private bool CanShow(ICodeFileDocument document) => !disposed && active && enabled() &&
            subscriptions.ContainsKey(document) && !document.TextEditor.IsDisposed && document.TextEditor.ActiveTextAreaControl.TextArea.Focused;

        private ICodeFileDocument DocumentFor(TextArea area)
        {
            foreach (var document in subscriptions.Keys)
                if (document.TextEditor.ActiveTextAreaControl.TextArea == area) return document;
            return null;
        }

#if !PABC_LINUX_PLUGIN
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

#endif
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
            var area = context.Document.TextEditor.ActiveTextAreaControl.TextArea;
            object response = null;
            try
            {
                response = await documents.DiscoverPackagesAsync(context.Document,
                    directive.Prefix, directive.PackageId, token).ConfigureAwait(false);
            }
            catch (OperationCanceledException) { }
            catch (Exception error) { Trace.TraceWarning("NuGet completion: {0}", error); }
            PostCompletion(area, ticket, () =>
            {
                if (response == null || token.IsCancellationRequested || !CanShow(context.Document) ||
                    !string.Equals(context.Text, area.Document.TextContent, StringComparison.Ordinal) ||
                    area.Caret.Offset != context.Offset) return;
                var provider = new Net10PackageCompletionProvider(response, ui, directive);
                pending = null;
                if (provider.Items.Length == 0) return;
                Opening?.Invoke();
                ui.ShowCompletion(context.Document, provider, true);
            });
        }

        private async Task RequestAsync(Net10CompletionContext context, long ticket, CancellationToken token, char trigger)
        {
            var area = context.Document.TextEditor.ActiveTextAreaControl.TextArea;
            object response = null;
            try
            {
                var position = area.Document.OffsetToPosition(context.Offset);
                response = await documents.CompleteAsync(context.Document, position.Y, position.X, token, trigger).ConfigureAwait(false);
            }
            catch (OperationCanceledException) { }
            catch (Exception error) { Trace.TraceWarning("LSP completion: {0}", error); }
            PostCompletion(area, ticket, () =>
            {
                if (response == null || token.IsCancellationRequested || !CanShow(context.Document) || !context.IsCurrent(out var prefix)) return;
                var provider = new Net10CompletionProvider(response, ui, prefix);
                pending = null;
                if (provider.Items.Length == 0) return;
                Opening?.Invoke(); // Avoid two dialog-key consumers for completion and parameter help.
                ui.ShowCompletion(context.Document, provider, preserveOrder: provider.DefaultIndex >= 0);
            });
        }

        private void PostCompletion(TextArea area, long ticket, Action show)
        {
            // Passive popups can leave the caller without a WinForms synchronization context.
            try
            {
                if (area.IsDisposed || !area.IsHandleCreated) return;
                area.BeginInvoke(new Action(() =>
                {
                    if (ticket != generation) return;
                    try { show(); }
                    catch (Exception error) { Trace.TraceWarning("Completion popup: {0}", error); }
                    finally
                    {
                        if (ticket == generation) { pending = null; cancellation?.Dispose(); cancellation = null; }
                    }
                }));
            }
            catch (InvalidOperationException) { } // The editor closed before dispatch.
        }

        private void Invalidate()
        {
            generation++;
            pending = null; scheduled = null;
            cancellation?.Cancel(); cancellation?.Dispose(); cancellation = null;
        }
        private void CloseWindow() => ui.Hide(IntelliSenseHint.Completion);
        public void Dispose()
        {
            if (disposed) return;
            SetActive(false); disposed = true;
            events.DocumentOpened -= Opened; events.DocumentClosed -= Closed; events.DocumentRenamed -= Renamed;

        }
    }
}
