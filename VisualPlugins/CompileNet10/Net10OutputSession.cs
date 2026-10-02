using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    // Output stays bound to the source tab; input uses the IDE's standard panel.
    internal sealed class Net10OutputSession : IDisposable
    {
        private readonly IWorkbench workbench;
        private readonly RichTextBox output;
        private readonly Control document;
        private readonly IExternalInputSession inputSession;
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

        public Net10OutputSession(IWorkbench workbench, RichTextBox output, ICodeFileDocument source,
            Func<string, System.Threading.Tasks.Task> submit, Action stop)
        {
            this.workbench = workbench;
            this.output = output;
            document = source as Control;
            this.submit = submit;
            this.stop = stop;
            if (document == null || document.IsDisposed || output.IsDisposed)
                throw new InvalidOperationException("Вкладка для запуска закрыта.");
            var inputService = workbench.ServiceContainer.RunService as IExternalRunInputService;
            if (inputService == null)
                throw new InvalidOperationException("Для стандартного ввода Run10 пересоберите IDE и PluginsSupport.dll.");
            inputSession = inputService.RegisterInput(source, Submit, stop);
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
            Post(() => inputSession.RequestInput());
        }

        private async void Submit(string text)
        {
            if (disposed) return;
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
            if (disposed) return;
            disposed = true;
            output.Disposed -= OutputDisposed;
            document.Disposed -= OutputDisposed;
            inputSession.Dispose();
        }
    }
}
