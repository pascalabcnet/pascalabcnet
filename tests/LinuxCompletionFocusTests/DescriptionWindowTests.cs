using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

internal static class DescriptionWindowTests
{
    internal static void Run(Assembly ide, Assembly editorAssembly, Action<bool, string> check, Action pump)
    {
        using (var owner = new Form { Text = "Linux hover popup test", Size = new Size(600, 300), ShowInTaskbar = false })
        using (var editor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        using (dynamic description = Activator.CreateInstance(ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.DeclarationWindow", true), owner))
        {
            editor.Dock = DockStyle.Fill; owner.Controls.Add(editor); owner.Show();
            object active = editor.GetType().GetProperty("ActiveTextAreaControl").GetValue(editor, null);
            var textArea = (Control)active.GetType().GetProperty("TextArea").GetValue(active, null);
            PropertyInfo text = description.GetType().GetProperty("Description");
            owner.Activate(); textArea.Focus(); pump();
            int focusLost = 0, painted = 0;
            textArea.LostFocus += (sender, args) => focusLost++;
            var nativeTip = (ToolTip)description.GetType().GetField("toolTip", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(description);
            nativeTip.Draw += (sender, args) => painted++;
            int forms = Application.OpenForms.Count;
            int hidden = 0;
            description.VisibleChanged += new EventHandler((sender, args) => { if (!description.Visible) hidden++; });
            description.Location = owner.PointToScreen(new Point(40, 80));
            text.SetValue(description, "function Test: integer;\nHover documentation.", null);
            pump(); pump();
            check(description.Visible && description.ClientSize.Width > 0 && description.ClientSize.Height > 0 &&
                (string)text.GetValue(description, null) ==
                "function Test: integer;\nHover documentation." && hidden == 0,
                "passive hover description has drawable size and remains visible [size=" + description.ClientSize +
                ", visible=" + description.Visible + ", hidden=" + hidden + ", lostFocus=" + focusLost + ", focused=" + textArea.Focused + "]");
            check(painted > 0 && textArea.Focused && focusLost == 0 && Application.OpenForms.Count == forms,
                "native tooltip is actually painted without a new Form or even a transient loss of editor focus");
            text.SetValue(description, null, null); pump();
            text.SetValue(description, "type Test\nAnother hover description.", null); pump(); pump();
            check(description.Visible && hidden == 1,
                "passive hover popup can be hidden and shown again without automatic closing");
            check(textArea.Focused && focusLost == 0, "showing another hover description does not take keyboard focus");
            for (int i = 0; i < 20; i++) {
                text.SetValue(description, "procedure Test" + i + ";\nRepeated documentation.", null);
                pump();
                text.SetValue(description, null, null);
            }
            check(textArea.Focused && focusLost == 0 && !description.Visible,
                "twenty native tooltip show/hide cycles preserve editor focus");
            text.SetValue(description, "procedure Test;\nFinal documentation.", null); pump();
            owner.GetType().GetMethod("OnDeactivate", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(owner, new object[] { EventArgs.Empty });
            check(!description.Visible, "leaving the owner hides the passive tooltip");
            int ownerHandlesCreated = 0;
            owner.HandleCreated += (sender, args) => ownerHandlesCreated++;
            owner.Dispose();
            check(description.IsDisposed && ownerHandlesCreated == 0 && !owner.IsHandleCreated,
                "owner disposal releases the tooltip without recreating an owner handle");
        }
        pump();
    }
}
