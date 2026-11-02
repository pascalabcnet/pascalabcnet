using System;
using System.Reflection;
using System.Windows.Forms;

internal static class WindowTraceTests
{
    internal static void Run(Assembly ide, Action<bool, string> check)
    {
        Type trace = ide.GetType("VisualPascalABC.HoverTrace", true);
        if (!(bool)trace.GetField("Enabled", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null)) return;
        using (var window = new Form { ShowInTaskbar = false })
        {
            int created = 0, destroyed = 0;
            window.HandleCreated += (sender, args) => created++;
            window.HandleDestroyed += (sender, args) => destroyed++;
            trace.GetMethod("Observe", BindingFlags.Static | BindingFlags.NonPublic, null, new[] { typeof(Control), typeof(string) }, null)
                .Invoke(null, new object[] { window, "lifetime-test" });
            check(!window.IsHandleCreated && created == 0, "window diagnostics do not create an unrequested native handle");
            IntPtr handle = window.Handle;
            window.Dispose();
            check(handle != IntPtr.Zero && created == 1 && destroyed == 1 && !window.IsHandleCreated,
                "window diagnostics do not recreate a handle during destruction");
        }
    }
}
