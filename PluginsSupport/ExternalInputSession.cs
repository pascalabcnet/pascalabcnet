using System;

namespace VisualPascalABCPlugins
{
    // Optional extension: existing IWorkbenchRunService implementations/plugins stay compatible.
    public interface IExternalRunInputService
    {
        IExternalInputSession RegisterInput(ICodeFileDocument document, Action<string> send, Action stop);
        bool TryStopExternalInput();
    }

    public interface IExternalInputSession : IDisposable
    {
        void RequestInput();
    }

    // UI-thread state. The host owns the standard input panel; the plugin owns its process.
    public sealed class ExternalInputSession : IExternalInputSession
    {
        private readonly Action<string> send;
        private readonly Action stop;
        private readonly Action request;
        private readonly Action release;
        private bool disposed;
        public ICodeFileDocument Document { get; private set; }
        public bool IsWaiting { get; private set; }

        public ExternalInputSession(ICodeFileDocument document, Action<string> send, Action stop,
            Action request, Action release)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            this.send = send ?? throw new ArgumentNullException(nameof(send));
            this.stop = stop ?? throw new ArgumentNullException(nameof(stop));
            this.request = request ?? throw new ArgumentNullException(nameof(request));
            this.release = release ?? throw new ArgumentNullException(nameof(release));
        }

        public bool Owns(ICodeFileDocument document)
        {
            return !disposed && ReferenceEquals(Document, document);
        }

        public void RequestInput()
        {
            if (disposed || IsWaiting) return;
            IsWaiting = true;
            request();
        }

        public bool TrySend(ICodeFileDocument document, string text)
        {
            if (!Owns(document) || !IsWaiting) return false;
            IsWaiting = false;
            send(text);
            return true;
        }

        public bool TryStop(ICodeFileDocument document)
        {
            if (!Owns(document)) return false;
            stop();
            return true;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            IsWaiting = false;
            release();
        }
    }
}
