using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using PascalABCCompiler.Contracts;

namespace VisualPascalABC
{
    internal sealed class FullLineCompletionEditorController : IDisposable
    {
        private readonly CodeFileDocumentTextEditorControl editor;
        private TextArea textArea;
        private GhostTextOverlay ghostText;
        private readonly IInlineCompletionService manager;
        private CancellationTokenSource completionCancellation;

        public FullLineCompletionEditorController(
            CodeFileDocumentTextEditorControl editor, 
            IInlineCompletionService completionManager
            )
        {
            this.editor = editor;
            manager = completionManager;

            if (manager.Ready)
                OnCompletionManagerReady();
            else
                manager.OnReady += OnCompletionManagerReady;
        }

        private void OnCompletionManagerReady()
        {
            editor.Leave += OnEditorLeave;
            textArea = editor.ActiveTextAreaControl.TextArea;
            ghostText = new GhostTextOverlay(textArea);
            textArea.Caret.PositionChanged += OnCaretPositionChanged;
            textArea.DoProcessDialogKey += OnProcessDialogKey;
            textArea.Document.TextContentChanged += OnTextChanged;
        }

        private void OnEditorLeave(object sender, EventArgs e)
        {
            CancelPendingCompletion();
            ghostText.Clear();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            RequestCompletion();
        }

        private void OnCaretPositionChanged(object sender, EventArgs e)
        {
            RequestCompletion();
        }

        private bool OnProcessDialogKey(Keys keyData)
        {
            if ((keyData & Keys.KeyCode) != Keys.Tab || !ghostText.HasSuggestion)
                return false;

            string suggestion = ghostText.Suggestion;
            ghostText.Clear();
            int offset = textArea.Caret.Offset;
            textArea.Document.Insert(offset, suggestion);
            textArea.Caret.Position =
                textArea.Document.OffsetToPosition(offset + suggestion.Length);
            textArea.Refresh();
            return true;
        }

        private void RequestCompletion()
        {
            ghostText.Clear();
            CancelPendingCompletion();

            completionCancellation = new CancellationTokenSource();
            _ = RequestCompletionAsync(completionCancellation.Token);
        }

        private async Task RequestCompletionAsync(CancellationToken cancellationToken)
        {
            int caretOffset = textArea.Caret.Offset;
            string text = textArea.Document.TextContent;
            
            var context = new CodeCompletionContext
            (
                fileName: editor.FileName,
                fullText: text,
                caretOffset: caretOffset,
                line: textArea.Caret.Line,
                column: textArea.Caret.Column
            );

            try
            {
                string suggestion = await manager.TriggerCompletionAsync(context, cancellationToken);

                if (string.IsNullOrEmpty(suggestion) ||
                    textArea.IsDisposed)
                    return;

                ghostText.ShowSuggestion(suggestion);
            }
            catch (TaskCanceledException) { }
        }

        public void Dispose()
        {
            manager.OnReady -= OnCompletionManagerReady;
            editor.Leave -= OnEditorLeave;
            
            if (textArea != null)
            {
                textArea.Caret.PositionChanged -= OnCaretPositionChanged;
                textArea.DoProcessDialogKey -= OnProcessDialogKey;
                textArea.Document.TextContentChanged -= OnTextChanged;
            }

            CancelPendingCompletion();

            ghostText?.Dispose();
        }

        private void CancelPendingCompletion()
        {
            var cts = completionCancellation;
            if (cts == null) return;
            completionCancellation = null;

            try { cts.Cancel(); }
            catch (ObjectDisposedException) { }
            finally { cts.Dispose(); }
        }
    }
}
