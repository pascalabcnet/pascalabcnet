using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

internal static class TextAreaToolTipFontTests
{
    internal static void Run(Assembly assembly, Action<bool, string> check)
    {
        bool mono = (bool)assembly.GetType("ICSharpCode.TextEditor.EditorPlatform", true)
            .GetField("IsMonoUnix", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
        using (var editor = (Control)Activator.CreateInstance(assembly.GetType("ICSharpCode.TextEditor.TextEditorControl", true)))
        using (var font = new Font(FontFamily.GenericSansSerif, 13))
        {
            object active = editor.GetType().GetProperty("ActiveTextAreaControl").GetValue(editor, null);
            object area = active.GetType().GetProperty("TextArea").GetValue(active, null);
            if (!mono)
            {
                using (var owner = new Form())
                {
                    owner.Controls.Add(editor);
                    area.GetType().GetMethod("SetToolTip", BindingFlags.Instance | BindingFlags.NonPublic)
                        .Invoke(area, new object[] { null, -1 });
                    var windows = area.GetType().GetField("windowsToolTip", BindingFlags.Static | BindingFlags.NonPublic);
                    var passive = area.GetType().GetField("toolTip", BindingFlags.Static | BindingFlags.NonPublic);
                    check(windows.GetValue(null) is Form && passive.GetValue(null) == null,
                        "Windows uses its own tooltip window and creates no Mono tooltip");
                    var show = area.GetType().GetMethod("SetToolTip", BindingFlags.Instance | BindingFlags.NonPublic);
                    var tip = (Form)windows.GetValue(null);
                    show.Invoke(area, new object[] { "Windows tooltip text", -1 });
                    check(tip.Visible && (string)tip.GetType().GetProperty("Description").GetValue(tip, null) == "Windows tooltip text"
                        && passive.GetValue(null) == null, "Windows tooltip displays text through the Windows implementation");
                    show.Invoke(area, new object[] { "Windows tooltip text", -1 });
                    check(ReferenceEquals(windows.GetValue(null), tip), "repeated Windows tooltip text reuses the same window");
                    show.Invoke(area, new object[] { null, -1 });
                    check(!tip.Visible, "null hides the Windows tooltip");
                    ((Form)windows.GetValue(null)).Dispose();
                    windows.SetValue(null, null);
                    owner.Controls.Remove(editor);
                }
            }
            var property = area.GetType().GetProperty("ToolTipFont");
            var field = area.GetType().GetField("toolTipFont", BindingFlags.NonPublic | BindingFlags.Instance);
            check(mono || field.GetValue(area) == null, "Windows does not resolve the tooltip font during construction");
            check(Equals(property.GetValue(area, null), Control.DefaultFont), "tooltip font defaults to the control default font");
            property.SetValue(area, font, null);
            check(ReferenceEquals(property.GetValue(area, null), font), "tooltip uses the caller-provided font");
            property.SetValue(area, null, null);
            check(mono ? property.GetValue(area, null) == null : Equals(property.GetValue(area, null), Control.DefaultFont),
                "null tooltip font preserves the platform policy");
        }
    }
}
