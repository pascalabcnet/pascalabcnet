using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

internal static class EditorToolTipTests
{
    internal static void Run(Assembly ide, Assembly editorAssembly, Action<bool, string> check, Action pump)
    {
        using (var owner = new Form { Size = new Size(700, 400), ShowInTaskbar = false })
        using (var editor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        using (var other = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        {
            owner.Controls.Add(editor); editor.Dock = DockStyle.Fill;
            owner.Controls.Add(other); other.Visible = false;
            owner.Show(); owner.Activate();
            Func<Control, Control> areaOf = e => {
                object active = e.GetType().GetProperty("ActiveTextAreaControl").GetValue(e, null);
                return (Control)active.GetType().GetProperty("TextArea").GetValue(active, null);
            };
            Control area = areaOf(editor), inactive = areaOf(other);
            Type type = area.GetType();
            var font = (Font)ide.GetType("VisualPascalABC.Constants", true)
                .GetField("CompletionFont", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            type.GetProperty("ToolTipFont").SetValue(area, font, null);
            MethodInfo show = type.GetMethod("SetToolTip", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo field = type.GetField("toolTip", BindingFlags.Static | BindingFlags.NonPublic);
            area.Focus(); pump(); int lost = 0, drawn = 0;
            area.LostFocus += (sender, args) => lost++;
            show.Invoke(area, new object[] { "begin\n  Print('&текст');\nend;", -1 });
            var tip = (ToolTip)field.GetValue(null);
            check(type.GetField("windowsToolTip", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) == null,
                "Mono creates no Windows declaration tooltip window");
            tip.Draw += (sender, args) => drawn++;
            show.Invoke(area, new object[] { null, -1 });
            show.Invoke(area, new object[] { "begin\n  Print('&текст');\nend;", -1 }); pump();
            check(drawn > 0 && area.Focused && lost == 0 && tip.OwnerDraw,
                "built-in folded-code tooltip paints without taking editor focus");
            show.Invoke(inactive, new object[] { null, -1 });
            check(ReferenceEquals(type.GetField("toolTipOwner", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null), area),
                "an inactive editor cannot hide another editor's built-in tooltip");
            show.Invoke(area, new object[] { "Marker explanation & literal text", -1 }); pump();
            check(area.Focused && lost == 0 && (string)type.GetField("toolTipText", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) == "Marker explanation & literal text",
                "built-in marker tooltip preserves its text and leaves keyboard input in the editor");
            check(Equals(type.GetProperty("ToolTipFont").GetValue(area, null), font),
                "built-in editor tooltips use the shared completion-list font");
            show.Invoke(area, new object[] { "  \t", -1 });
            check(type.GetField("toolTipOwner", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) == null,
                "whitespace hides the Mono tooltip through the Mono implementation");
            show.Invoke(area, new object[] { "Marker explanation & literal text", -1 });
            editor.Dispose();
            check(field.GetValue(null) == null, "disposing the owning editor releases its built-in tooltip");
        }
        pump();
    }
}
