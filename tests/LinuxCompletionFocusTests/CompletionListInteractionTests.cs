using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

internal static class CompletionListInteractionTests
{
    private const BindingFlags Hidden = BindingFlags.Instance | BindingFlags.NonPublic;
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr window, int message, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr window, int message, IntPtr wParam, IntPtr lParam);
    private static object Read(object target, string name) { return target.GetType().GetProperty(name).GetValue(target, null); }
    private static void Write(object target, string name, object value) { target.GetType().GetProperty(name).SetValue(target, value, null); }
    private static object Call(object target, string name, params object[] args) { return target.GetType().GetMethod(name).Invoke(target, args); }
    private static IntPtr PointParam(int x, int y) { return new IntPtr((y << 16) | (x & 0xffff)); }

    internal static void Run(Assembly ide, Assembly editorAssembly, Action<bool, string> check, Action pump)
    {
        using (var owner = new Form { Text = "Linux passive completion list interaction test", Size = new Size(820, 500), ShowInTaskbar = false })
        using (var editor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        {
            editor.Dock = DockStyle.Fill; owner.Controls.Add(editor); owner.Show();
            object active = Read(editor, "ActiveTextAreaControl"), document = Read(editor, "Document");
            var area = (Control)Read(active, "TextArea");
            object caret = Read(active, "Caret");
            Action<string> setSource = source => {
                Write(document, "TextContent", source);
                Write(caret, "Position", document.GetType().GetMethod("OffsetToPosition").Invoke(document, new object[] { source.Length }));
            };
            setSource("s."); owner.Activate(); area.Focus(); pump();
            int lost = 0; area.LostFocus += (sender, args) => lost++;
            Type popupType = ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.PABCNETCodeCompletionWindow", true);
            object provider = Activator.CreateInstance(ide.GetType("VisualPascalABC.CodeCompletionProvider", true));
            provider.GetType().GetField("preSelection").SetValue(provider, string.Empty);
            Type itemType = ide.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.UserDefaultCompletionData", true);
            Array items = Array.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.Gui.CompletionWindow.ICompletionData", true), 40);
            for (int i = 0; i < items.Length; i++) items.SetValue(Activator.CreateInstance(itemType,
                "Method" + i.ToString("00"), "procedure Method" + i.ToString("00") + ";\nDocumentation.", 0, false, false), i);
            using (var popup = (ToolStripDropDown)popupType.GetConstructors(Hidden)[0].Invoke(new object[] { provider, items, owner, editor, false, true }))
            {
                var list = (Control)popupType.GetField("codeCompletionListView", Hidden).GetValue(popup);
                var bar = (VScrollBar)popupType.GetField("vScrollBar", Hidden).GetValue(popup);
                dynamic description = popupType.GetField("declarationViewWindow", Hidden).GetValue(popup);
                var sharedFont = (Font)ide.GetType("VisualPascalABC.Constants", true).GetField("CompletionFont", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                check(Equals(list.Font, sharedFont) && Equals(description.Font, sharedFont) && Equals(popup.Font, sharedFont),
                    "completion list, passive popup and hover-description renderer use the same font and units");
                popupType.GetMethod("ShowCompletionWindow", Hidden, null, Type.EmptyTypes, null).Invoke(popup, null);
                Call(popup, "SelectIndex", 0); pump();
                Func<string> selected = () => (string)Read(Read(list, "SelectedCompletionData"), "Text");
                check(popup.Visible && description.Visible && area.Focused && lost == 0 && !popup.ContainsFocus && !popup.Capture,
                    "a long dot-completion list and its description show without taking editor focus");
                check(list.Height == popup.ClientSize.Height && list.Width + bar.Width == popup.ClientSize.Width && bar.Visible,
                    "the hosted completion list fills the popup and keeps its scrollbar visible");
                var panel = list.Parent;
                check(panel.Bounds == popup.ClientRectangle && popup.Items[0].Bounds == popup.ClientRectangle && list.Top == 0 && bar.Bottom == panel.Height,
                    "completion host starts at the popup origin and its scrollbar reaches the bottom");
                panel.Bounds = new Rectangle(0, -60, panel.Width, Math.Max(1, panel.Height - 90));
                popup.PerformLayout(); pump();
                check(panel.Bounds == popup.ClientRectangle && popup.Items[0].Bounds == popup.ClientRectangle && list.Top == 0 && bar.Bottom == panel.Height,
                    "layout repairs a shifted or shortened host without clipping the first completion rows");
                check(!list.CanSelect && !bar.CanSelect, "neither completion rows nor the scrollbar can select keyboard focus");
                Call(area, "ExecuteDialogKey", Keys.End); pump();
                check(selected() == "Method39" && bar.Value > 0 && area.Focused, "End selects the last completion and scrolls it into view");
                Call(area, "ExecuteDialogKey", Keys.Home); Call(area, "ExecuteDialogKey", Keys.Down); pump();
                check(selected() == "Method01" && area.Focused, "Home and Down select completion rows through editor keys");
                Call(area, "ExecuteDialogKey", Keys.PageDown); pump();
                check(selected() != "Method01" && area.Focused, "PageDown navigates the long completion list");
                Call(area, "ExecuteDialogKey", Keys.Home); pump();
                int height = (int)Read(list, "ItemHeight");
                if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                    check(SendMessage(list.Handle, 0x21, owner.Handle, new IntPtr(0x02010001)) == new IntPtr(3),
                        "completion rows accept mouse clicks without activation");
                    SendMessage(list.Handle, 0x201, new IntPtr(1), PointParam(10, height + 3));
                    SendMessage(list.Handle, 0x202, IntPtr.Zero, PointParam(10, height + 3)); pump();
                } else {
                    list.GetType().GetMethod("OnMouseDown", Hidden).Invoke(list,
                        new object[] { new MouseEventArgs(MouseButtons.Left, 1, 10, height + 3, 0) }); pump();
                }
                check(selected() == "Method01" && area.Focused && lost == 0 && !list.Capture,
                    "clicking a completion row changes selection without stealing keyboard focus" +
                    " [selected=" + selected() + ", editorFocused=" + area.Focused + ", lost=" + lost + ", capture=" + list.Capture + ", visible=" + popup.Visible + "]");
                if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                    check(SendMessage(bar.Handle, 0x21, owner.Handle, new IntPtr(0x02010001)) == new IntPtr(3),
                        "completion scrollbar requests no activation");
                    int before = bar.Value;
                    // A native scrollbar tracks the held mouse in a nested loop.
                    // Queue both events so its tracking loop can receive MouseUp.
                    PostMessage(bar.Handle, 0x201, new IntPtr(1), PointParam(bar.Width / 2, bar.Height - 5));
                    PostMessage(bar.Handle, 0x202, IntPtr.Zero, PointParam(bar.Width / 2, bar.Height - 5)); pump();
                    check(bar.Value > before && area.Focused && lost == 0,
                        "native scrollbar arrow click scrolls without taking keyboard focus" +
                        " [before=" + before + ", value=" + bar.Value + ", focused=" + area.Focused + ", lost=" + lost + ", capture=" + bar.Capture + ", visible=" + popup.Visible + "]");
                    // A posted click has no matching physical button state. The
                    // standard VScrollBar also retains capture in this probe.
                    // End the artificial gesture before testing wheel/typing.
                    SendMessage(bar.Handle, 0x001f, IntPtr.Zero, IntPtr.Zero); // WM_CANCELMODE
                    bar.Capture = false;
                    int wheelBefore = bar.Value;
                    SendMessage(list.Handle, 0x20a, new IntPtr(unchecked(-120 << 16)), PointParam(0, 0)); pump();
                    check(bar.Value > wheelBefore && area.Focused && lost == 0, "native mouse wheel scrolls the completion list without taking focus");
                } else {
                    int before = bar.Value;
                    Call(popup, "HandleMouseWheel", new MouseEventArgs(MouseButtons.None, 0, 0, 0, -120)); pump();
                    check(bar.Value > before && area.Focused && lost == 0, "mouse wheel scrolls the completion list without taking focus");
                }
                EventInfo keys = area.GetType().GetEvent("KeyEventHandler");
                Delegate forward = Delegate.CreateDelegate(keys.EventHandlerType, popup, popupType.GetMethod("ProcessKeyEvent"));
                keys.AddEventHandler(area, forward);
                int closedEvents = 0;
                popup.Closed += (sender, args) => { closedEvents++; keys.RemoveEventHandler(area, forward); };
                try {
                    foreach (char ch in "Method2") Call(area, "SimulateKeyPress", ch);
                    pump();
                    check((string)Read(document, "TextContent") == "s.Method2" && selected() == "Method20" && lost == 0,
                        "typing a member prefix stays in the editor and selects a matching completion");
                    Call(area, "ExecuteDialogKey", Keys.Back); pump();
                    check((string)Read(document, "TextContent") == "s.Method" && popup.Visible && area.Focused,
                        "Backspace updates the completion prefix without losing editor input");
                    Call(popup, "SelectIndex", 0); pump();
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT) {
                        SendMessage(list.Handle, 0x203, new IntPtr(1), PointParam(10, 3));
                        if (!list.IsDisposed) SendMessage(list.Handle, 0x202, IntPtr.Zero, PointParam(10, 3));
                    } else {
                        list.GetType().GetMethod("OnDoubleClick", Hidden).Invoke(list, new object[] { EventArgs.Empty });
                    }
                    pump();
                    check(popup.IsDisposed && description.IsDisposed && (string)Read(document, "TextContent") == "s.Method00" && area.Focused && lost == 0,
                        "double-click inserts the chosen member and closes both passive popups without changing focus");
                    check(closedEvents == 1 && !(bool)Call(popup, "ProcessKeyEvent", ';'),
                        "completion closure raises one Closed event and a stale reference cannot intercept later typing");
                    Call(area, "SimulateKeyPress", ';');
                    check((string)Read(document, "TextContent") == "s.Method00;", "typing continues after double-click completion");
                }
                finally { keys.RemoveEventHandler(area, forward); }
            }
            setSource("s."); area.Focus();
            using (var popup = (ToolStripDropDown)popupType.GetConstructors(Hidden)[0].Invoke(new object[] { provider, items, owner, editor, false, true }))
            using (var other = new TextBox { Dock = DockStyle.Bottom })
            {
                owner.Controls.Add(other); other.BringToFront();
                popupType.GetMethod("ShowCompletionWindow", Hidden, null, Type.EmptyTypes, null).Invoke(popup, null);
                pump(); other.Focus(); pump();
                check(popup.IsDisposed && other.Focused && !area.Focused, "leaving the editor closes the completion list without pulling focus back");
            }
        }
        pump();
    }
}
