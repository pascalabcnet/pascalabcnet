using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckEditorReleaseConfiguration()
    {
        var attributes = typeof(TextEditorControl).Assembly.GetCustomAttributes(typeof(System.Diagnostics.DebuggableAttribute), false)
            .Cast<System.Diagnostics.DebuggableAttribute>();
        Check(!attributes.Any(value => value.IsJITOptimizerDisabled), "prepared editor is an optimized Release assembly, not Debug");
    }

    private sealed class UpdateTrackingEditor : TextEditorControl
    {
        public int Depth, UnbalancedEnds;
        public override void BeginUpdate() { Depth++; base.BeginUpdate(); }
        public override void EndUpdate()
        {
            // Record the defect even in Release; never show an interactive Assert dialog.
            if (Depth == 0) { UnbalancedEnds++; return; }
            Depth--;
            base.EndUpdate();
        }
    }

    private static void CheckEditorLoadingAndCaret()
    {
        var suspended = Assembly.Load("CodeCompletion").GetType("CodeCompletion.CodeCompletionController", true)
            .GetField("LegacyAnalysisSuspended");
        bool previous = (bool)suspended.GetValue(null);
        try
        {
            foreach (bool modern in new[] { false, true })
            using (var document = new EditorDocument(new UpdateTrackingEditor()) { FileName = Path.GetFullPath("Loaded.pas") })
            using (var parent = new Form())
            {
                suspended.SetValue(null, modern);
                var editor = (UpdateTrackingEditor)document.TextEditor;
                var area = editor.ActiveTextAreaControl.TextArea;
                area.Caret.PositionChanged += (sender, e) => VisualPascalABC.CodeCompletionHighlighter.Highlight(area);
                var events = new WorkbenchDocumentEventSource(() => new List<ICodeFileDocument> { document });
                using (var sync = new Net10LanguageDocuments(events, () => new FakeLanguageClient(), text => { }))
                using (var completion = new Net10Completion(parent, events, sync, () => true))
                {
                    completion.SetActive(modern);
                    foreach (string name in new[] { "EditorProgram.pas", "EditorUnit.pas", "EditorProgram.pas" })
                    {
                        document.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fixtures", name);
                        editor.LoadFile(document.FileName);
                        Check(editor.Depth == 0 && !editor.IsInUpdate && editor.UnbalancedEnds == 0,
                            "opening/reopening disk file keeps update balance; net10=" + modern + "; file=" + name);
                        string loadedText = editor.Document.TextContent;
                        for (int offset = 0; offset <= editor.Document.TextLength; offset++)
                        {
                            area.Caret.Position = editor.Document.OffsetToPosition(offset);
                            CheckBalance(editor, modern, offset);
                        }
                        Check(editor.Document.TextContent == loadedText,
                            "cursor movement does not modify loaded file text; net10=" + modern + "; file=" + name);
                    }
                    foreach (string source in new[] {
                        "begin\n  Print('Привет 😀');\nend.\n",
                        "unit U;\ninterface\nprocedure P;\nimplementation\nprocedure P;\nbegin\n  repeat\n  until true;\nend;\nend.\n",
                        "begin\n  if true then\n  begin\n    Print(1);\n  end;\nend.\n",
                        "type C = class\nend;\nbegin\nend.\n",
                        "// begin end\nbegin\n  Print('begin end');\nend.\n",
                        "begin", "", "type C = class;\nbegin end."
                    })
                    {
                        // Real editor LoadFile path, including caret callbacks during CommitUpdate.
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(source)))
                            editor.LoadFile(document.FileName, stream, true, true);
                        Check(editor.Depth == 0 && !editor.IsInUpdate && editor.UnbalancedEnds == 0,
                            "loading Pascal text keeps update balance; net10=" + modern + "; length=" + source.Length);
                        for (int offset = 0; offset <= editor.Document.TextLength; offset++)
                        {
                            area.Caret.Position = editor.Document.OffsetToPosition(offset);
                            VisualPascalABC.CodeCompletionHighlighter.Highlight(area);
                            CheckBalance(editor, modern, offset);
                        }
                        Check(true, "caret movement through every text position keeps update balance; net10=" + modern);
                        // Highlighting must not consume an update transaction owned by its caller.
                        editor.BeginUpdate();
                        VisualPascalABC.CodeCompletionHighlighter.Highlight(area);
                        Check(editor.Depth == 1 && editor.IsInUpdate && editor.UnbalancedEnds == 0,
                            "nested highlight preserves caller update; net10=" + modern);
                        editor.EndUpdate();
                    }
                    editor.Document.TextContent = "begin\nend.";
                    area.Caret.Position = editor.Document.OffsetToPosition(2);
                    VisualPascalABC.CodeCompletionHighlighter.Highlight(area);
                    Check(editor.Document.MarkerStrategy.TextMarker.Count() == 2,
                        "begin/end matching remains highlighted; net10=" + modern);
                    CheckBalance(editor, modern, 2);
                }
            }
        }
        finally { suspended.SetValue(null, previous); }
    }

    private static void CheckBalance(UpdateTrackingEditor editor, bool modern, int offset)
    {
        if (editor.Depth != 0 || editor.IsInUpdate || editor.UnbalancedEnds != 0)
            throw new Exception("Unbalanced editor update during caret movement; net10=" + modern + "; offset=" + offset);
    }
}
