using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckEditorKeyDispatch()
    {
        foreach (bool modern in new[] { false, true })
        using (var document = new EditorDocument { FileName = Path.GetFullPath("EnterRegression.pas") })
        using (var parent = new Form())
        {
            var editor = document.TextEditor;
            var area = editor.ActiveTextAreaControl.TextArea;
            editor.Document.TextContent = "begin";
            area.Caret.Position = editor.Document.OffsetToPosition(5);
            // The shell's begin/end auto-insertion runs before plugin subscribers.
            int insertions = 0;
            area.KeyEventHandler += ch =>
            {
                if (ch != '\n') return false;
                insertions++;
                area.InsertString("\n  ");
                editor.Document.Insert(area.Caret.Offset, "\nend.");
                return true;
            };
            var events = new WorkbenchDocumentEventSource(() => new List<ICodeFileDocument> { document });
            using (var sync = new Net10LanguageDocuments(events, () => new FakeLanguageClient(), text => { }))
            using (var completion = new Net10Completion(parent, events, sync, () => true))
            {
                completion.SetActive(modern);
                new ICSharpCode.TextEditor.Actions.Return().Execute(area);
                Check(editor.Document.TextContent.Replace("\r\n", "\n") == "begin\n  \nend." &&
                    area.Caret.Line == 1 && area.Caret.Column == 2 && insertions == 1,
                    "Enter after begin inserts exactly one body line and keeps caret there; net10=" + modern);
            }
        }

        using (var editor = new TextEditorControl())
        {
            var area = editor.ActiveTextAreaControl.TextArea;
            int first = 0, consumer = 0, later = 0;
            area.KeyEventHandler += ch => { first++; return false; };
            area.KeyEventHandler += ch => { consumer++; return ch == 'x'; };
            area.KeyEventHandler += ch => { later++; return false; };
            area.SimulateKeyPress('x');
            Check(first == 1 && consumer == 1 && later == 0 && editor.Document.TextLength == 0,
                "handled key stops dispatch and is not inserted again");
            area.SimulateKeyPress('y');
            Check(first == 2 && consumer == 2 && later == 1 && editor.Document.TextContent == "y",
                "unhandled key reaches every subscriber and is inserted once");
            new ICSharpCode.TextEditor.Actions.Return().Execute(area);
            Check(editor.Document.TotalNumberOfLines == 2 && area.Caret.Line == 1,
                "ordinary Enter remains available when no handler consumes it");
        }
    }
}
