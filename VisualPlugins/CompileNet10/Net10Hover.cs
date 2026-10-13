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
using VisualPascalABC;

namespace VisualPascalABCPlugins
{
    internal sealed class Net10HoverContext
    {
        internal readonly ICodeFileDocument Document;
        internal readonly string FileName, Text;
        internal readonly TextLocation Position;
        private readonly int start, end;

        private Net10HoverContext(ICodeFileDocument document, TextLocation position, int offset)
        {
            Document = document; FileName = document.FileName;
            Text = document.TextEditor.Document.TextContent; Position = position;
            start = offset; end = offset + 1;
            while (start > 0 && IsIdentifier(Text[start - 1])) start--;
            while (end < Text.Length && IsIdentifier(Text[end])) end++;
        }

        private static bool IsIdentifier(char ch) => char.IsLetterOrDigit(ch) || ch == '_';
        internal static Net10HoverContext Capture(ICodeFileDocument document, TextLocation position)
        {
            var text = document.TextEditor.Document;
            if (position.Y < 0 || position.Y >= text.TotalNumberOfLines) return null;
            var line = text.GetLineSegment(position.Y);
            if (position.X < 0 || position.X >= line.Length) return null;
            int offset = line.Offset + position.X;
            return IsIdentifier(text.GetCharAt(offset)) ? new Net10HoverContext(document, position, offset) : null;
        }

        internal bool IsCurrent => !Document.TextEditor.IsDisposed &&
            string.Equals(FileName, Document.FileName, StringComparison.Ordinal) && Text == Document.TextEditor.Document.TextContent;

        internal bool Contains(TextLocation position)
        {
            if (!IsCurrent || position.Y != Position.Y || position.X < 0) return false;
            var line = Document.TextEditor.Document.GetLineSegment(position.Y);
            int offset = line.Offset + position.X;
            return offset >= start && offset < end;
        }

        internal static string Description(object response)
        {
            if (!(response is IDictionary<string, object> hover) || !hover.TryGetValue("contents", out var contents)) return null;
            return Content(contents);
        }
        private static string Content(object value)
        {
            if (value is string text) return text;
            if (value is IDictionary<string, object> markup)
                return markup.TryGetValue("value", out var description) ? description as string : null;
            if (value is IEnumerable items)
            {
                var descriptions = new List<string>();
                foreach (var item in items) { string part = Content(item); if (!string.IsNullOrWhiteSpace(part)) descriptions.Add(part); }
                return string.Join(Environment.NewLine, descriptions);
            }
            return null;
        }
    }

    // Native ToolTipRequest supplies the editor's hover delay and folded-line coordinates.
    // Only descriptions cross this adapter; the standard IDE hint window remains the renderer.
    internal sealed class Net10Hover : IDisposable
    {
        private sealed class Subscription
        {
            public ToolTipRequestEventHandler Request;
            public MouseEventHandler Move, MouseCancel;
            public EventHandler Cancel;
            public System.Windows.Forms.KeyEventHandler Key;
            public DocumentEventHandler Changed;
        }

        private readonly Form parent;
        private readonly IWorkbenchDocumentEvents events;
        private readonly Net10LanguageDocuments documents;
        private readonly Func<bool> enabled;
        private readonly Dictionary<ICodeFileDocument, Subscription> subscriptions = new Dictionary<ICodeFileDocument, Subscription>();
        private Net10HoverContext context;
        private CancellationTokenSource cancellation;
        private long generation;
        private bool active, disposed, shown;

        public Net10Hover(Form parent, IWorkbenchDocumentEvents events, Net10LanguageDocuments documents, Func<bool> enabled)
        {
            this.parent = parent; this.events = events; this.documents = documents; this.enabled = enabled;
            events.DocumentOpened += Opened; events.DocumentClosed += Closed; events.DocumentRenamed += Renamed;
            parent.Deactivate += Cancel;
        }

        public void SetActive(bool value)
        {
            if (disposed || active == value) return;
            active = value;
            if (active) foreach (var document in events.GetOpenDocuments()) Attach(document);
            else
            {
                Invalidate();
                foreach (var document in new List<ICodeFileDocument>(subscriptions.Keys)) Detach(document);
            }
        }

        private void Attach(ICodeFileDocument document)
        {
            if (!active || document.TextEditor == null || subscriptions.ContainsKey(document) ||
                !string.Equals(Path.GetExtension(document.FileName), ".pas", StringComparison.OrdinalIgnoreCase)) return;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            var handlers = new Subscription
            {
                Request = (sender, e) => Requested(document, e),
                Move = (sender, e) =>
                {
                    if (context?.Document == document && (!enabled() || !ContainsMouse(context, e.Location))) Invalidate();
                },
                MouseCancel = (sender, e) => Invalidate(),
                Cancel = Cancel,
                Key = (sender, e) => Invalidate(),
                Changed = (sender, e) => Invalidate() // Another unsaved unit can also change the description.
            };
            subscriptions.Add(document, handlers);
            area.ToolTipRequest += handlers.Request;
            area.MouseMove += handlers.Move; area.MouseDown += handlers.MouseCancel; area.MouseWheel += handlers.MouseCancel;
            area.MouseLeave += handlers.Cancel; area.LostFocus += handlers.Cancel;
            area.Caret.PositionChanged += handlers.Cancel; area.KeyDown += handlers.Key;
            document.TextEditor.Document.DocumentChanged += handlers.Changed;
        }

        private void Detach(ICodeFileDocument document)
        {
            if (!subscriptions.TryGetValue(document, out var handlers)) return;
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            area.ToolTipRequest -= handlers.Request;
            area.MouseMove -= handlers.Move; area.MouseDown -= handlers.MouseCancel; area.MouseWheel -= handlers.MouseCancel;
            area.MouseLeave -= handlers.Cancel; area.LostFocus -= handlers.Cancel;
            area.Caret.PositionChanged -= handlers.Cancel; area.KeyDown -= handlers.Key;
            document.TextEditor.Document.DocumentChanged -= handlers.Changed;
            subscriptions.Remove(document);
        }

        private void Opened(object sender, WorkbenchDocumentEventArgs e) { Invalidate(); Attach(e.Document); }
        private void Closed(object sender, WorkbenchDocumentEventArgs e) { Invalidate(); Detach(e.Document); }
        private void Renamed(object sender, WorkbenchDocumentEventArgs e) { Invalidate(); Detach(e.Document); Attach(e.Document); }
        private void Cancel(object sender, EventArgs e) => Invalidate();

        private void Requested(ICodeFileDocument document, ToolTipRequestEventArgs e)
        {
            // Folded text, error markers and debugger hints retain priority.
            if (!active || !enabled() || !e.InDocument || e.ToolTipShown) { Invalidate(); return; }
            if (context?.Document == document && context.Contains(e.LogicalPosition)) return;
            Invalidate();
            context = Net10HoverContext.Capture(document, e.LogicalPosition);
            if (context == null) return;
            cancellation = new CancellationTokenSource();
            _ = RequestAsync(context, generation, cancellation.Token);
        }

        private static bool ContainsMouse(Net10HoverContext value, Point mouse)
        {
            var area = value.Document.TextEditor.ActiveTextAreaControl.TextArea;
            var bounds = area.TextView.DrawingPosition;
            return bounds.Contains(mouse) && value.Contains(area.TextView.GetLogicalPosition(mouse.X - bounds.X, mouse.Y - bounds.Y));
        }

        private async Task RequestAsync(Net10HoverContext value, long ticket, CancellationToken token)
        {
            try
            {
                object response = await documents.HoverAsync(value.Document, value.Position.Y, value.Position.X, token);
                // Resume on WinForms context. No control access on the JSON-RPC transport thread.
                if (token.IsCancellationRequested || ticket != generation || disposed || !active || !enabled() ||
                    !value.IsCurrent || !subscriptions.ContainsKey(value.Document) || !parent.ContainsFocus) return;
                var area = value.Document.TextEditor.ActiveTextAreaControl.TextArea;
                Point mouse = area.PointToClient(Control.MousePosition);
                if (!area.Visible || !ContainsMouse(value, mouse)) return;
                string description = Net10HoverContext.Description(response);
                if (string.IsNullOrWhiteSpace(description)) return;
                TooltipServiceManager.ShowExternalHint(area, mouse, value.Position, description);
                shown = true;
            }
            catch (OperationCanceledException) { }
            catch (Exception error) { Trace.TraceWarning("LSP hover: {0}", error); }
            finally { if (ticket == generation) { cancellation?.Dispose(); cancellation = null; } }
        }

        private void Invalidate()
        {
            generation++; context = null;
            cancellation?.Cancel(); cancellation?.Dispose(); cancellation = null;
            // Do not hide another service's hint after returning to the legacy platform.
            if (shown) { shown = false; TooltipServiceManager.hideToolTip(); }
        }

        public void Dispose()
        {
            if (disposed) return;
            SetActive(false); disposed = true;
            events.DocumentOpened -= Opened; events.DocumentClosed -= Closed; events.DocumentRenamed -= Renamed;
            parent.Deactivate -= Cancel;
        }
    }
}
