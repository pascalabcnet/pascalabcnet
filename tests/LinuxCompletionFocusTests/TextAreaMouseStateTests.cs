using System;
using System.Reflection;
using System.Windows.Forms;

internal static class TextAreaMouseStateTests
{
    private static int requests;
    private static void Requested(object sender, object args) { requests++; }

    internal static void Run(Assembly assembly, Action<bool, string> check)
    {
        const BindingFlags instance = BindingFlags.Instance | BindingFlags.NonPublic;
        bool mono = (bool)assembly.GetType("ICSharpCode.TextEditor.EditorPlatform", true)
            .GetField("IsMonoUnix", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        using (var editor = (Control)Activator.CreateInstance(assembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        {
            var active = editor.GetType().GetProperty("ActiveTextAreaControl").GetValue(editor, null);
            var area = (Control)active.GetType().GetProperty("TextArea").GetValue(active, null);
            var type = area.GetType();
            var state = type.GetField("MyMouseButtons", instance);
            check((MouseButtons)state.GetValue(area) == MouseButtons.None, "mouse state starts released");
            int downs = 0, ups = 0;
            area.MouseDown += (s,e) => downs++;
            area.MouseUp += (s,e) => ups++;
            var down = type.GetMethod("OnMouseDown", instance);
            var up = type.GetMethod("OnMouseUp", instance);
            // A point outside all editor margins avoids selection changes.
            down.Invoke(area, new object[] { new MouseEventArgs(MouseButtons.Left, 1, -10, -10, 0) });
            check((MouseButtons)state.GetValue(area) == (mono ? MouseButtons.Left : MouseButtons.None),
                mono ? "Mono tracks local mouse down" : "Windows does not update Mono mouse state");
            if (mono)
            {
                var evt = type.GetEvent("ToolTipRequest");
                var handler = Delegate.CreateDelegate(evt.EventHandlerType, typeof(TextAreaMouseStateTests).GetMethod("Requested", BindingFlags.NonPublic | BindingFlags.Static));
                evt.AddEventHandler(area, handler);
                requests = 0;
                type.GetMethod("OnMouseHover", instance).Invoke(area, new object[] { EventArgs.Empty });
                check(requests == 0, "Mono hover while pressed does not request a tooltip");
                evt.RemoveEventHandler(area, handler);
            }
            up.Invoke(area, new object[] { new MouseEventArgs(MouseButtons.Left, 1, -10, -10, 0) });
            check((MouseButtons)state.GetValue(area) == MouseButtons.None && downs == 1 && ups == 1,
                "mouse release clears local state and base events fire once");
            down.Invoke(area, new object[] { new MouseEventArgs(MouseButtons.Right, 1, -10, -10, 0) });
            check((MouseButtons)state.GetValue(area) == (mono ? MouseButtons.Right : MouseButtons.None),
                "right mouse button follows the same platform policy");
            up.Invoke(area, new object[] { new MouseEventArgs(MouseButtons.Right, 1, -10, -10, 0) });
            check((MouseButtons)state.GetValue(area) == MouseButtons.None, "right mouse release leaves no stuck local button");
        }
    }
}
