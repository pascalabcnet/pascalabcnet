using System;

namespace VisualPascalABCPlugins
{
    internal static class Net10EditorDocument
    {
        public static Net10EditorSource Read(ICodeFileDocument document)
        {
            var editor = document.TextEditor;
            if (editor == null || editor.Document == null)
                throw new InvalidOperationException("Недоступен текст редактора: " + document.FileName);
            // ICodeFileDocument.Text is the DockContent caption, not Pascal source.
            return new Net10EditorSource { FileName = document.FileName,
                Text = editor.Document.TextContent,
                Changed = document.DocumentChanged, FromMetadata = document.FromMetadata };
        }
    }
}
