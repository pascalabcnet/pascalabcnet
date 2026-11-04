using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABC;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static ExternalIntelliSenseSession TestIntelliSenseUI(Form parent)
    {
        if (parent == null) parent = new Form();
        var session = new ExternalIntelliSenseSession(parent);
        parent.Disposed += (sender, e) => session.Dispose();
        testHintParents.Add(parent);
        return session;
    }
    private static readonly System.Collections.Generic.List<Form> testHintParents = new System.Collections.Generic.List<Form>();

    private static void CheckExternalCompletionKeys()
    {
        var singleton = typeof(VisualPABCSingleton).GetField("mainForm", BindingFlags.NonPublic | BindingFlags.Static);
        var previous = singleton.GetValue(null);
        var shell = (Form1)FormatterServices.GetUninitializedObject(typeof(Form1));
        shell.UserOptions = new UserOptions { ShowCompletionInfoByGroup = true, EnableSmartIntellisense = true };
        singleton.SetValue(null, shell);
        try
        {
            foreach (var key in new[] { Keys.Enter, Keys.Tab, Keys.Escape })
            using (var parent = new Form { ShowInTaskbar = false, Opacity = 0 })
            using (var document = new EditorDocument { FileName = System.IO.Path.GetFullPath("SessionCompletion.pas") })
            using (var session = new ExternalIntelliSenseSession(parent))
            {
                parent.Controls.Add(document.TextEditor);
                var area = document.TextEditor.ActiveTextAreaControl.TextArea;
                document.TextEditor.Document.TextContent = "s.Su";
                area.Caret.Position = document.TextEditor.Document.OffsetToPosition(4);
                int closed = 0;
                session.Closed += hint => { throw new InvalidOperationException("test subscriber failure"); };
                session.Closed += hint => { if (hint == IntelliSenseHint.Completion) closed++; };
                parent.Show(); area.Focus();
                session.ShowCompletion(document, new Net10CompletionProvider(CompletionResponse(), session, "Su"));
                Check(area.ExecuteDialogKey(key) && document.TextEditor.Document.TextContent ==
                    (key == Keys.Escape ? "s.Su" : "s.Substring") && closed == 1,
                    "session preserves native completion insertion/cancellation: " + key);
                session.Hide(IntelliSenseHint.Completion);
                Check(closed == 1 && !session.ProcessCompletionKey('x'), "closed completion leaves the next editor key unhandled");
            }
        }
        finally { singleton.SetValue(null, previous); }
    }

    private static void CheckExternalIntelliSenseOwnership()
    {
        using (var parent = new Form { ShowInTaskbar = false, Opacity = 0 })
        using (var document = new EditorDocument { FileName = System.IO.Path.GetFullPath("OwnedHints.pas") })
        using (var old = new ExternalIntelliSenseSession(parent))
        using (var current = new ExternalIntelliSenseSession(parent))
        {
            parent.Controls.Add(document.TextEditor);
            document.TextEditor.Document.TextContent = "begin end.";
            var area = document.TextEditor.ActiveTextAreaControl.TextArea;
            var field = typeof(TooltipServiceManager).GetField("dvw", BindingFlags.Static | BindingFlags.NonPublic);
            var position = new TextLocation(1, 0);
            old.ShowHover(document, position, Point.Empty, "old description");
            current.ShowHover(document, position, Point.Empty, "new description");
            old.Hide(IntelliSenseHint.Hover);
            var popup = (ICSharpCode.TextEditor.Gui.CompletionWindow.DeclarationViewWindow)field.GetValue(null);
            Check(popup.Description == "new description", "an old session cannot hide another session's hover");
            old.Dispose(); old.Dispose();
            old.ShowHover(document, position, Point.Empty, "late reply");
            Check(popup.Description == "new description" && !old.ProcessCompletionKey('\n'),
                "disposed session ignores late replies and consumes no editor keys");
            TooltipServiceManager.ShowExternalHint(area, Point.Empty, position, "legacy description");
            current.Dispose();
            Check(popup.Description == "legacy description", "disposing modern hints cannot hide a later legacy tooltip");
            TooltipServiceManager.hideToolTip();
            // Shared providers reference only the common contract/editor, not the old EXE interface.
            Check(typeof(Net10SignatureProvider).GetInterface(typeof(IExternalParameterInfo).FullName) != null &&
                typeof(Net10SignatureProvider).GetInterface("ICSharpCode.TextEditor.Gui.InsightWindow.IExternalInsightDataProvider") == null,
                "external signature provider uses only the shared parameter contract");
        }
    }
}
