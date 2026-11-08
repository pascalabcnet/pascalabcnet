using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace VisualPascalABC
{
    // Explicit local diagnostics. No UI actions, focus changes or persistent settings.
    internal static class HoverTrace
    {
        internal static readonly bool Enabled = Environment.GetEnvironmentVariable("PABCNET_HOVER_TRACE") == "1";

        internal static void Observe(DeclarationWindow window, string kind = "hover-description")
        {
            if (!Enabled) return;
            window.VisibleChanged += (sender, args) => Write(kind + " passive-tooltip visible=" + window.Visible + " bounds=" + window.Bounds);
            window.Disposed += (sender, args) => Write(kind + " passive-tooltip disposed");
        }
        private static readonly object gate = new object();
        private static string path;
        private static long lastMove;

        internal static void Write(string message)
        {
            if (!Enabled) return;
            try
            {
                lock (gate)
                {
                    if (path == null)
                    {
                        path = Path.Combine(Path.GetTempPath(), "PascalABCNET-hover-" + Process.GetCurrentProcess().Id + ".log");
                        Console.Error.WriteLine("PascalABC.NET hover log: " + path);
                        File.AppendAllText(path, "Trace=passive-completion-v9\nIDE=" + typeof(TooltipServiceManager).Assembly.Location +
                            " version=" + typeof(TooltipServiceManager).Assembly.GetName().Version +
                            "\nEditor=" + typeof(TextArea).Assembly.Location +
                            "\nCompletion=" + typeof(CodeCompletion.CodeCompletionController).Assembly.Location +
                            "\nRuntime=" + Environment.Version + " OS=" + Environment.OSVersion + Environment.NewLine);
                    }
                    if (new FileInfo(path).Length < 1024 * 1024)
                        File.AppendAllText(path, DateTime.UtcNow.ToString("o") + " " + message + Environment.NewLine);
                }
            }
            catch (Exception) { /* Diagnostics must never break the editor. */ }
        }

        internal static void MouseMove(TextArea area, MouseEventArgs e)
        {
            if (!Enabled) return;
            long now = Stopwatch.GetTimestamp();
            if (now - lastMove < Stopwatch.Frequency) return;
            lastMove = now;
            Write("mouse-move file=" + area.MotherTextEditorControl.FileName + " xy=" + e.Location +
                " editorFocused=" + area.ContainsFocus);
        }

        internal static void MouseHover(object sender, EventArgs e)
        {
            var area = (TextArea)sender;
            Write("mouse-hover file=" + area.MotherTextEditorControl.FileName + " editorFocused=" + area.ContainsFocus);
        }

        internal static void Observe(Control window, string kind = "hover-description")
        {
            if (!Enabled) return;
            // Only read Handle after creation. Reading it during destruction can
            // recreate a native window and would change the bug being diagnosed.
            IntPtr lastHandle = window.IsHandleCreated ? window.Handle : IntPtr.Zero;
            Action<string> record = action => Write(kind + " " + action + " handle=0x" + lastHandle.ToInt64().ToString("X") +
                " type=" + window.GetType().FullName);
            record("observed");
            window.HandleCreated += (sender, args) => {
                if (window.IsHandleCreated) lastHandle = window.Handle;
                record("handle-created");
            };
            window.HandleDestroyed += (sender, args) => record("handle-destroyed");
            window.Disposed += (sender, args) => record("disposed");
            window.VisibleChanged += (sender, args) => record("visible=" + window.Visible + " size=" + window.ClientSize + " bounds=" + window.Bounds);
            window.GotFocus += (sender, args) => record("got-focus");
            window.LostFocus += (sender, args) => record("lost-focus");
            window.MouseCaptureChanged += (sender, args) => record("mouse-capture-changed");
            window.KeyDown += (sender, args) => {
                if (args.KeyCode == Keys.Back || args.KeyCode == Keys.Delete || args.KeyCode == Keys.Escape ||
                    args.KeyCode == Keys.Enter || args.KeyCode == Keys.Tab)
                    record("key=" + args.KeyCode);
            };
            var form = window as Form;
            if (form != null)
            {
                form.Activated += (sender, args) => record("activated");
                form.Deactivate += (sender, args) => record("deactivated");
                form.FormClosing += (sender, args) => record("closing reason=" + args.CloseReason);
                form.FormClosed += (sender, args) => record("closed reason=" + args.CloseReason);
            }
        }
    }
}
