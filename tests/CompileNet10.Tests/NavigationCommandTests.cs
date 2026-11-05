using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using VisualPascalABCPlugins;
internal static partial class Program
{
    private static void CheckNavigationCommands()
    {
        using (var document = new EditorDocument { FileName = Path.GetFullPath("NavigationCommand.pas") })
        using (var parent = new Form { ShowInTaskbar = false, Opacity = 0 })
        using (var button = new Button())
        {
            document.TextEditor.Document.TextContent = "begin p1; end.";
            parent.Controls.Add(document.TextEditor); parent.Controls.Add(button);
            var events = new WorkbenchDocumentEventSource(() => new List<ICodeFileDocument> { document });
            var fake = new FakeLanguageClient { Request = (method, value, token) => Task.FromResult(
                NavigationResponse(document.FileName, method == "textDocument/definition" ? 2 : 5, 0)) };
            using (var sync = new Net10LanguageDocuments(events, () => fake, value => { }))
            {
                int jumps = 0; var targets = new List<Net10NavigationTarget>();
                using (var navigation = new Net10Navigation(events, sync, () => true, value => { targets = value; jumps++; }))
                {
                    parent.Show();
                    sync.SetActive(true); sync.WhenIdleAsync().GetAwaiter().GetResult(); navigation.SetActive(true);
                    var area = document.TextEditor.ActiveTextAreaControl.TextArea;
                    area.Caret.Position = new TextLocation(8, 0); // immediately after p1
                    button.Focus(); Application.DoEvents();
                    navigation.NavigateAtCaret(document, false); PumpNavigation(() => jumps == 1);
                    Check(targets[0].Line == 3, "definition command uses definition endpoint at the caret without editor focus");
                    navigation.NavigateAtCaret(document, true); PumpNavigation(() => jumps == 2);
                    Check(targets[0].Line == 6, "implementation command uses implementation endpoint");
                    navigation.SetActive(false);
                    int requests = fake.Messages.Count;
                    navigation.NavigateAtCaret(document, true); Application.DoEvents();
                    Check(fake.Messages.Count == requests && jumps == 2, "legacy mode does not dispatch external navigation commands");
                }
            }
        }
    }
}
