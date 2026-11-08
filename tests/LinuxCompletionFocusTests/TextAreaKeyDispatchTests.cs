using System;
using System.Reflection;
using System.Windows.Forms;

internal static class TextAreaKeyDispatchTests
{
    private static int firstCalls, laterCalls;
    private static bool First(char ch) { firstCalls++; return true; }
    private static bool Later(char ch) { laterCalls++; return false; }

    internal static void Run(Assembly editorAssembly, Action<bool, string> check)
    {
        var flags = BindingFlags.NonPublic | BindingFlags.Static;
        bool mono = (bool)editorAssembly.GetType("ICSharpCode.TextEditor.EditorPlatform", true)
            .GetField("IsMonoUnix", flags).GetValue(null);
        using (var editor = (Control)Activator.CreateInstance(editorAssembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        {
            object active = editor.GetType().GetProperty("ActiveTextAreaControl").GetValue(editor, null);
            object area = active.GetType().GetProperty("TextArea").GetValue(active, null);
            EventInfo keyEvent = area.GetType().GetEvent("KeyEventHandler");
            var first = Delegate.CreateDelegate(keyEvent.EventHandlerType, typeof(TextAreaKeyDispatchTests).GetMethod("First", flags));
            var later = Delegate.CreateDelegate(keyEvent.EventHandlerType, typeof(TextAreaKeyDispatchTests).GetMethod("Later", flags));
            MethodInfo handle = area.GetType().GetMethod("HandleKeyPress", BindingFlags.Instance | BindingFlags.NonPublic);
            firstCalls = laterCalls = 0;
            keyEvent.AddEventHandler(area, first);
            keyEvent.AddEventHandler(area, later);
            bool handled = (bool)handle.Invoke(area, new object[] { ';' });
            check(firstCalls == 1 && laterCalls == (mono ? 1 : 0) && handled == !mono,
                mono ? "Mono keeps the last multicast result and calls both subscribers" : "Windows stops at the first accepting subscriber");
            keyEvent.RemoveEventHandler(area, first);
            keyEvent.RemoveEventHandler(area, later);
            check(!(bool)handle.Invoke(area, new object[] { ';' }), "key without subscribers is not consumed");
        }
    }
}
