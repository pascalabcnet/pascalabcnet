using System;
using System.Reflection;
using System.Windows.Forms;

internal static class TextAreaHoverTimerTests
{
    internal static void Run(Assembly assembly, Action<bool, string> check)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        bool mono = (bool)assembly.GetType("ICSharpCode.TextEditor.EditorPlatform", true)
            .GetField("IsMonoUnix", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        var editor = (Control)Activator.CreateInstance(assembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true));
        try
        {
            object active = editor.GetType().GetProperty("ActiveTextAreaControl").GetValue(editor, null);
            var area = (Control)active.GetType().GetProperty("TextArea").GetValue(active, null);
            var type = area.GetType();
            var timer = (Timer)type.GetField("HoverTimer", flags).GetValue(area);
            if (!mono)
            {
                check(timer == null, "Windows creates no Mono hover timer");
                return;
            }
            check(timer != null && timer.Enabled && timer.Interval == 400, "Mono hover timer starts at 400 ms");
            // Block tooltip requests; test only the timer's stationary-mouse decision.
            type.GetField("MyMouseButtons", flags).SetValue(area, MouseButtons.Left);
            int hovered = 0, disposed = 0;
            area.MouseHover += (s,e) => hovered++;
            timer.Disposed += (s,e) => disposed++;
            var move = type.GetMethod("OnMouseMove", flags);
            var tick = type.GetMethod("HoverTimerEventProcessor", flags);
            move.Invoke(area, new object[] { new MouseEventArgs(MouseButtons.Left, 0, -10, -10, 0) });
            tick.Invoke(area, new object[] { timer, EventArgs.Empty });
            check(hovered == 0, "moving mouse does not trigger synthetic hover");
            tick.Invoke(area, new object[] { timer, EventArgs.Empty });
            check(hovered == 1, "stationary mouse triggers synthetic hover");
            move.Invoke(area, new object[] { new MouseEventArgs(MouseButtons.Left, 0, -20, -20, 0) });
            tick.Invoke(area, new object[] { timer, EventArgs.Empty });
            check(hovered == 1, "movement resets the hover wait");
            editor.Dispose();
            check(!timer.Enabled && disposed == 1, "closing the editor stops and disposes its hover timer");
        }
        finally { editor.Dispose(); }
    }
}
