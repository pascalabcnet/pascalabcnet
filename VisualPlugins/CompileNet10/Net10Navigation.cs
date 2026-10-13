using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10NavigationTarget
    {
        internal string FileName, MetadataTitle, MetadataText;
        internal int Line, Column; // IDE coordinates are one-based.
        internal static IEnumerable<IDictionary<string, object>> ReadLocations(object response)
        {
            if (response is IDictionary<string, object> single) yield return single;
            else if (response is IEnumerable items)
                foreach (var item in items) if (item is IDictionary<string, object> location) yield return location;
        }
        internal static List<Net10NavigationTarget> Read(object response)
        {
            var result = new List<Net10NavigationTarget>();
            foreach (var location in ReadLocations(response))
            {
                bool link = location.ContainsKey("targetUri");
                if (!location.TryGetValue(link ? "targetUri" : "uri", out var name) || !(name is string address) ||
                    !Uri.TryCreate(address, UriKind.Absolute, out var uri) ||
                    !location.TryGetValue(link ? "targetSelectionRange" : "range", out var range) ||
                    !(range is IDictionary<string, object> span) || !span.TryGetValue("start", out var start) ||
                    !(start is IDictionary<string, object> point) || !point.TryGetValue("line", out var line) || !(line is int row) ||
                    !point.TryGetValue("character", out var character) || !(character is int column) || row < 0 || column < 0 ||
                    row == int.MaxValue || column == int.MaxValue) continue;
                var target = new Net10NavigationTarget { Line = row + 1, Column = column + 1 };
                if (uri.IsFile && string.Equals(Path.GetExtension(uri.LocalPath), ".pas", StringComparison.OrdinalIgnoreCase))
                    target.FileName = uri.LocalPath;
                else if (uri.Scheme == "pascalabc-metadata" && location.TryGetValue("metadata", out var metadata) &&
                    metadata is IDictionary<string, object> data)
                {
                    target.MetadataTitle = Value(data, "title", "Title"); target.MetadataText = Value(data, "text", "Text");
                    if (string.IsNullOrEmpty(target.MetadataTitle) || string.IsNullOrEmpty(target.MetadataText)) continue;
                }
                else continue; // Never launch an external URL or write a virtual document to disk.
                result.Add(target);
            }
            return result;
        }
        private static string Value(IDictionary<string, object> data, string key, string alternative) =>
            data.TryGetValue(key, out var value) ? value as string : data.TryGetValue(alternative, out value) ? value as string : null;
    }

    internal sealed class Net10Navigation : IDisposable
    {
        private sealed class Subscription
        {
            internal MouseEventHandler Click, Move;
            internal EventHandler Cancel, Caret;
            internal System.Windows.Forms.KeyEventHandler Key, KeyUp;
            internal EventHandler Leave;
            internal DocumentEventHandler Changed;
        }
        private readonly IWorkbenchDocumentEvents events;
        private readonly Net10LanguageDocuments documents;
        private readonly Func<bool> enabled;
        private readonly Action<List<Net10NavigationTarget>> navigate;
        private readonly Func<Keys> modifiers;
        private readonly Dictionary<ICodeFileDocument, Subscription> subscriptions = new Dictionary<ICodeFileDocument, Subscription>();
        private CancellationTokenSource cancellation;
        private Net10HoverContext pending;
        private int caretOffset;
        private long generation;
        private bool active, disposed;
        private ICodeFileDocument markedDocument;
        private TextMarker marker;

        internal Net10Navigation(IWorkbenchDocumentEvents events, Net10LanguageDocuments documents, Func<bool> enabled,
            Action<List<Net10NavigationTarget>> navigate, Func<Keys> modifiers = null)
        {
            this.events = events; this.documents = documents; this.enabled = enabled; this.navigate = navigate;
            this.modifiers = modifiers ?? (() => Control.ModifierKeys);
            events.DocumentOpened += Opened; events.DocumentClosed += Closed; events.DocumentRenamed += Renamed;
        }
        internal void SetActive(bool value)
        {
            if (disposed || active == value) return;
            active = value;
            if (active) foreach (var document in events.GetOpenDocuments()) Attach(document);
            else { Cancel(); foreach (var document in new List<ICodeFileDocument>(subscriptions.Keys)) Detach(document); }
        }
        private void Attach(ICodeFileDocument document)
        {
            if (!active || document.FromMetadata || document.TextEditor == null || subscriptions.ContainsKey(document) ||
                !string.Equals(Path.GetExtension(document.FileName), ".pas", StringComparison.OrdinalIgnoreCase)) return;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            var handlers = new Subscription
            {
                Click = (sender, e) => Click(document, e), Move = (sender, e) => Mark(document, e),
                Cancel = (sender, e) => Cancel(), Leave = (sender, e) => ClearMark(),
                Key = (sender, e) => { if (e.KeyCode == Keys.Escape) Cancel(); else ClearMark(); },
                KeyUp = (sender, e) => ClearMark(),
                Caret = (sender, e) => { if (pending?.Document == document && area.Caret.Offset != caretOffset) Cancel(); },
                Changed = (sender, e) => Cancel()
            };
            subscriptions.Add(document, handlers);
            area.MouseClick += handlers.Click; area.MouseMove += handlers.Move; area.MouseLeave += handlers.Leave;
            area.LostFocus += handlers.Cancel; area.KeyDown += handlers.Key; area.KeyUp += handlers.KeyUp;
            area.Caret.PositionChanged += handlers.Caret; document.TextEditor.Document.DocumentChanged += handlers.Changed;
        }
        private void Detach(ICodeFileDocument document)
        {
            if (!subscriptions.TryGetValue(document, out var handlers)) return;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            area.MouseClick -= handlers.Click; area.MouseMove -= handlers.Move; area.MouseLeave -= handlers.Leave;
            area.LostFocus -= handlers.Cancel; area.KeyDown -= handlers.Key; area.KeyUp -= handlers.KeyUp;
            area.Caret.PositionChanged -= handlers.Caret; document.TextEditor.Document.DocumentChanged -= handlers.Changed;
            subscriptions.Remove(document);
        }
        private void Opened(object sender, WorkbenchDocumentEventArgs e) { Cancel(); Attach(e.Document); }
        private void Closed(object sender, WorkbenchDocumentEventArgs e) { Cancel(); Detach(e.Document); }
        private void Renamed(object sender, WorkbenchDocumentEventArgs e) { Cancel(); Detach(e.Document); Attach(e.Document); }
        private static Net10HoverContext AtMouse(ICodeFileDocument document, MouseEventArgs e)
        {
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            if (!area.TextView.DrawingPosition.Contains(e.Location)) return null;
            var position = area.TextView.GetLogicalPosition(e.X - area.TextView.DrawingPosition.X, e.Y - area.TextView.DrawingPosition.Y);
            return Net10HoverContext.Capture(document, position);
        }
        private void Click(ICodeFileDocument document, MouseEventArgs e)
        {
            Cancel();
            if (!active || !enabled() || e.Button != MouseButtons.Left || modifiers() != Keys.Control) return;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            if (area.SelectionManager.HasSomethingSelected) return;
            pending = AtMouse(document, e);
            if (pending == null) return;
            caretOffset = area.Caret.Offset;
            cancellation = new CancellationTokenSource();
            _ = RequestAsync(pending, generation, cancellation.Token);
        }
        private async Task RequestAsync(Net10HoverContext context, long ticket, CancellationToken token)
        {
            var area = context.Document.TextEditor.ActiveTextAreaControl.TextArea;
            var targets = new List<Net10NavigationTarget>();
            try
            {
                var response = await documents.NavigationAsync(context.Document, context.Position.Y, context.Position.X, token).ConfigureAwait(false);
                targets = Net10NavigationTarget.Read(response);
            }
            catch (OperationCanceledException) { }
            catch (Exception error) { Trace.TraceWarning("LSP navigation: {0}", error); }
            finally
            {
                // Mouse callbacks need not carry a SynchronizationContext. Never inspect
                // editor state or open a source tab on the transport thread.
                try
                {
                    if (!area.IsDisposed && area.IsHandleCreated) area.BeginInvoke(new Action(() =>
                    {
                        if (ticket != generation) return;
                        try
                        {
                            if (!token.IsCancellationRequested && active && enabled() && context.IsCurrent &&
                                area.Focused && area.Caret.Offset == caretOffset && targets.Count > 0) navigate(targets);
                        }
                        finally { if (ticket == generation) Cancel(); }
                    }));
                }
                catch (InvalidOperationException) { } // Editor was closed between response and UI dispatch.
            }
        }
        private void Mark(ICodeFileDocument document, MouseEventArgs e)
        {
            ClearMark();
            if (!active || !enabled() || modifiers() != Keys.Control) return;
            var context = AtMouse(document, e);
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            if (context == null || area.SelectionManager.HasSomethingSelected) return;
            int offset = document.TextEditor.Document.PositionToOffset(context.Position), start = offset, end = offset;
            string text = context.Text;
            while (start > 0 && Identifier(text[start - 1])) start--;
            while (end < text.Length && Identifier(text[end])) end++;
            // Own only this marker. Never remove breakpoints, diagnostics or another plugin's underline.
            marker = new TextMarker(start, end - start, TextMarkerType.Underlined, Color.Blue);
            markedDocument = document; area.Document.MarkerStrategy.AddMarker(marker);
            area.TextView.Cursor = Cursors.Hand; area.Invalidate();
        }
        private static bool Identifier(char ch) => char.IsLetterOrDigit(ch) || ch == '_' || ch == '&';
        private void ClearMark()
        {
            if (markedDocument != null && !markedDocument.TextEditor.IsDisposed)
            {
                var area = markedDocument.TextEditor.ActiveTextAreaControl.TextArea;
                area.Document.MarkerStrategy.RemoveMarker(marker); area.TextView.Cursor = Cursors.IBeam; area.Invalidate();
            }
            markedDocument = null; marker = null;
        }
        private void Cancel()
        {
            generation++; pending = null; cancellation?.Cancel(); cancellation?.Dispose(); cancellation = null; ClearMark();
        }
        public void Dispose()
        {
            if (disposed) return;
            SetActive(false); disposed = true;
            events.DocumentOpened -= Opened; events.DocumentClosed -= Closed; events.DocumentRenamed -= Renamed;
        }
    }
}
