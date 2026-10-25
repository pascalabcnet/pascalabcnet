using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace VisualPascalABC
{
    internal sealed class GhostTextOverlay : IDisposable
    {
        private readonly TextArea textArea;
        private readonly Color suggestionColor = Color.FromArgb(190, 90, 90, 90);

        private string suggestion;

        public GhostTextOverlay(TextArea textArea)
        {
            this.textArea = textArea;
            textArea.Paint += OnPaint;
        }

        public bool HasSuggestion => !string.IsNullOrEmpty(suggestion);
        public string Suggestion => suggestion;

        public void ShowSuggestion(string value)
        {
            suggestion = value;
            textArea.Invalidate();
        }

        public void Clear()
        {
            suggestion = null;
            textArea.Invalidate();
        }


        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (string.IsNullOrEmpty(suggestion))
                return;

            var view = textArea.TextView;
            Point caretPosition = textArea.Caret.ScreenPosition;
            Font editorFont = view.TextEditorProperties.Font;

            using (var suggestionFont = new Font(
                editorFont, editorFont.Style | FontStyle.Italic))
            {
                const TextFormatFlags flags =
                    TextFormatFlags.NoPadding |
                    TextFormatFlags.NoPrefix |
                    TextFormatFlags.PreserveGraphicsClipping;

                int suggestionWidth = TextRenderer.MeasureText(
                    e.Graphics,
                    suggestion,
                    suggestionFont,
                    new Size(int.MaxValue, view.FontHeight),
                    flags).Width;

                LineSegment line = textArea.Document.GetLineSegment(
                    textArea.Caret.Line);

                string lineText = TextUtilities.GetLineAsString(
                    textArea.Document, textArea.Caret.Line);

                string suffix = textArea.Caret.Column < line.Length
                    ? lineText.Substring(textArea.Caret.Column)
                    : string.Empty;

                int suffixWidth = 0;

                if (!string.IsNullOrEmpty(suffix))
                {
                    suffixWidth = TextRenderer.MeasureText(
                        e.Graphics,
                        suffix,
                        editorFont,
                        new Size(int.MaxValue, view.FontHeight),
                        flags).Width;

                    HighlightBackground backgroundHighlight = 
                        (HighlightBackground)textArea.Document.HighlightingStrategy
                        .GetColorFor("Default");

                    // Очищаем область подсказки и исходного суффикса.
                    using (var background = new SolidBrush(backgroundHighlight.BackgroundColor))
                    {
                        e.Graphics.FillRectangle(
                            background,
                            caretPosition.X,
                            caretPosition.Y,
                            suggestionWidth + suffixWidth,
                            view.FontHeight);
                    }
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    suggestion,
                    suggestionFont,
                    caretPosition,
                    suggestionColor,
                    flags);

                if (!string.IsNullOrEmpty(suffix))
                {
                    TextRenderer.DrawText(
                        e.Graphics,
                        suffix,
                        editorFont,
                        new Point(
                            caretPosition.X + suggestionWidth,
                            caretPosition.Y),
                        textArea.ForeColor,
                        flags);
                }
            }
        }


        public void Dispose()
        {
            textArea.Paint -= OnPaint;
            textArea.Invalidate();
        }
    }
}
