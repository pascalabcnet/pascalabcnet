using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    // A plugin-owned input row: no interception of legacy Enter/Stop handlers.
    internal sealed class Net10OutputSession : IDisposable
    {
        private readonly IWorkbench workbench;
        private readonly RichTextBox output;
        private readonly Control document;
        private readonly Panel panel;
        private readonly TextBox input;
        private readonly Button send;
        private readonly Action stop;
        private readonly Func<string, System.Threading.Tasks.Task> submit;
        private bool disposed;

        public static RichTextBox CaptureOutput(IWorkbench workbench)
        {
            workbench.ServiceContainer.OperationsService.WriteToOutputBox("", true);
            var window = workbench.OutputWindow as Control;
            if (window == null) throw new InvalidOperationException("Недоступно окно вывода IDE.");
            var candidates = new List<RichTextBox>();
            FindOutput(window, candidates);
            if (candidates.Count != 1)
                throw new InvalidOperationException("Не удалось определить окно вывода текущей вкладки.");
            return candidates[0];
        }

        private static void FindOutput(Control root, List<RichTextBox> result)
        {
            foreach (Control control in root.Controls)
            {
                var text = control as RichTextBox;
                if (text != null && text.Visible && text.ReadOnly) result.Add(text);
                FindOutput(control, result);
            }
        }

        public Net10OutputSession(IWorkbench workbench, RichTextBox output, ICodeFileDocument source, string fileName,
            Func<string, System.Threading.Tasks.Task> submit, Action stop)
        {
            this.workbench = workbench;
            this.output = output;
            document = source as Control;
            this.submit = submit;
            this.stop = stop;
            if (document == null || document.IsDisposed || output.IsDisposed || output.Parent == null || output.Parent.Parent == null)
                throw new InvalidOperationException("Вкладка для запуска закрыта.");
            panel = new Panel { Dock = DockStyle.Bottom, Height = 30, Padding = new Padding(3), Visible = false };
            var label = new Label { Text = ".NET 10: " + Path.GetFileName(fileName),
                Dock = DockStyle.Left, Width = 155, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            input = new TextBox { Dock = DockStyle.Fill, Enabled = false, MaxLength = 32767 };
            send = new Button { Text = "Ввод", Dock = DockStyle.Right, Width = 65, Enabled = false };
            var stopButton = new Button { Text = "Stop10", Dock = DockStyle.Right, Width = 110 };
            send.Click += (sender, e) => Submit();
            stopButton.Click += (sender, e) => stop();
            input.KeyDown += (sender, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                e.SuppressKeyPress = true;
                Submit();
            };
            panel.Controls.Add(input);
            panel.Controls.Add(send);
            panel.Controls.Add(stopButton);
            panel.Controls.Add(label);
            // Retain the source tab's Control, not the mutable current-tab pointer.
            output.Parent.Parent.Controls.Add(panel);
            float zoom = output.ZoomFactor;
            output.Clear();
            output.ZoomFactor = zoom;
            output.Disposed += OutputDisposed;
            document.Disposed += OutputDisposed;
        }

        private void OutputDisposed(object sender, EventArgs e) { stop(); }

        public void Append(string text)
        {
            Post(() =>
            {
                output.AppendText(text);
                output.SelectionStart = output.TextLength;
                output.ScrollToCaret();
            });
        }

        public void RequestInput()
        {
            Post(() =>
            {
                panel.Visible = true;
                input.Enabled = send.Enabled = true;
                workbench.ServiceContainer.OperationsService.WriteToOutputBox("", true);
                input.Focus();
            });
        }

        private async void Submit()
        {
            if (disposed || !input.Enabled) return;
            string text = input.Text;
            input.Enabled = send.Enabled = false;
            input.Clear();
            panel.Visible = false;
            output.AppendText(text + Environment.NewLine);
            try { await submit(text); }
            catch (Exception error) { Append(Environment.NewLine + error.Message + Environment.NewLine); }
        }

        private void Post(Action action)
        {
            if (disposed || workbench.MainForm.IsDisposed) return;
            try
            {
                workbench.MainForm.BeginInvoke(new Action(() =>
                {
                    if (disposed) return;
                    if (output.IsDisposed) { stop(); return; }
                    action();
                }));
            }
            catch (InvalidOperationException) { }
        }

        public void Dispose()
        {
            disposed = true;
            output.Disposed -= OutputDisposed;
            document.Disposed -= OutputDisposed;
            panel.Dispose();
        }
    }
}
