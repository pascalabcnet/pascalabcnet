using System;
using System.Windows.Forms;

namespace VisualPascalABCPlugins
{
    // Output stays bound to the source tab; input uses the IDE's standard panel.
    internal sealed class Net10OutputSession : IDisposable
    {
        private readonly IWorkbench workbench;
        private readonly Control document;
        private readonly IExternalInputSession inputSession;
        private readonly Action stop;
        private readonly Action<string> deliverOutput;
        private readonly Func<string, System.Threading.Tasks.Task> submit;
        private bool disposed;

        public Net10OutputSession(IWorkbench workbench, ICodeFileDocument source,
            Func<string, System.Threading.Tasks.Task> submit, Action stop, Action<string> deliverOutput)
        {
            this.workbench = workbench;
            document = source as Control;
            this.submit = submit;
            this.stop = stop;
            this.deliverOutput = deliverOutput ?? throw new ArgumentNullException(nameof(deliverOutput));
            if (document == null || document.IsDisposed)
                throw new InvalidOperationException("Вкладка для запуска закрыта.");
            var inputService = workbench.ServiceContainer.RunService as IExternalRunInputService;
            if (inputService == null)
                throw new InvalidOperationException("Для стандартного ввода Run10 пересоберите IDE и PluginsSupport.dll.");
            inputSession = inputService.RegisterInput(source, Submit, stop);
            document.Disposed += OutputDisposed;
        }

        private void OutputDisposed(object sender, EventArgs e) { stop(); }

        public void Append(string text)
        {
            if (!string.IsNullOrEmpty(text)) Post(() => deliverOutput(text));
        }

        public void RequestInput()
        {
            Post(() => inputSession.RequestInput());
        }

        public void ReportException(RuntimeExceptionInfo error, IExternalRunSession session)
        {
            Post(() => session.ReportException(error));
        }

        // Exit must follow all queued output/exception UI callbacks, even for a very short program.
        public System.Threading.Tasks.Task FlushAsync()
        {
            var completed = new System.Threading.Tasks.TaskCompletionSource<bool>();
            var form = workbench.MainForm;
            if (disposed || form.IsDisposed) return System.Threading.Tasks.Task.CompletedTask;
            FormClosedEventHandler closed = null;
            closed = (sender, e) => { form.FormClosed -= closed; completed.TrySetResult(true); };
            form.FormClosed += closed;
            try
            {
                form.BeginInvoke(new Action(() =>
                {
                    form.FormClosed -= closed;
                    completed.TrySetResult(true);
                }));
            }
            catch (InvalidOperationException)
            {
                form.FormClosed -= closed;
                completed.TrySetResult(true);
            }
            return completed.Task;
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
                    if (document.IsDisposed) { stop(); return; }
                    action();
                }));
            }
            catch (InvalidOperationException) { }
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            document.Disposed -= OutputDisposed;
            inputSession.Dispose();
        }
    }
}
