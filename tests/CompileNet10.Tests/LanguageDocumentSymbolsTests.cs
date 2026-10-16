using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using VisualPascalABCPlugins;

internal static partial class Program
{
    private static void CheckLanguageDocumentSymbols()
    {
        object point(int line, int character) => new Dictionary<string, object>
        {
            ["line"] = line, ["character"] = character
        };
        object range(int startLine, int endLine) => new Dictionary<string, object>
        {
            ["start"] = point(startLine, 0), ["end"] = point(endLine, 3)
        };
        var member = new Dictionary<string, object>
        {
            ["name"] = "Name", ["detail"] = "Name: string", ["kind"] = 7,
            ["range"] = range(2, 2)
        };
        var type = new Dictionary<string, object>
        {
            ["name"] = "Person", ["detail"] = "Person", ["kind"] = 5,
            ["range"] = range(1, 4), ["children"] = new object[] { member }
        };
        var symbols = Net10DocumentSymbols.Parse(new object[] { type });
        Check(symbols.Count == 1 && symbols[0].Kind == "Class" && symbols[0].StartLine == 1 &&
            symbols[0].Children.Count == 1 && symbols[0].Children[0].Kind == "Property" &&
            symbols[0].Children[0].Name == "Name", "document symbol response retains nested members and positions");
        Check(Net10DocumentSymbols.Parse(new object[] { new Dictionary<string, object> { ["name"] = "broken" } }).Count == 0,
            "malformed document symbols are ignored");

        using (var editor = new VisualPascalABC.CodeFileDocumentTextEditorControl())
        {
            var panel = editor.NavigationPanel;
            panel.SetModernMode(true);
            panel.SetModernSymbols(symbols);
            var flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var classes = (ComboBox)typeof(VisualPascalABC.QuickClassBrowserPanel).GetField("classComboBox", flags).GetValue(panel);
            var members = (ComboBox)typeof(VisualPascalABC.QuickClassBrowserPanel).GetField("membersComboBox", flags).GetValue(panel);
            Check(classes.Items.Count == 2, "modern panel lists global scope and class");
            classes.SelectedIndex = 1;
            Check(members.Items.Count == 1 && members.Items[0].ToString() == "Name: string",
                "modern panel lists selected class members");
            panel.SetModernMode(false);
            Check(classes.Items.Count == 0 && members.Items.Count == 0,
                "switching back restores legacy panel state");
        }
    }
}
