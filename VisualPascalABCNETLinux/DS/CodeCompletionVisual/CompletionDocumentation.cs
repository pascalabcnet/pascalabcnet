using System;
using System.Threading;
using System.Windows.Forms;

namespace ICSharpCode.TextEditor.Gui.CompletionWindow
{
    // Linux shell only. One reader, at most one pending request, no UI-thread wait.
    // Cancellation drops publication immediately; a slow XML read may finish in
    // the background, but cannot change a closed/replaced completion list.
    internal sealed class CompletionDocumentation
    {
        private static readonly object gate = new object();
        private static CompletionDocumentation pending, active;
        private static bool reading;
        private readonly Control editor;
        private Action read, publish;
        private int cancelled;

        internal CompletionDocumentation(Control editor) { this.editor = editor; }
        internal bool IsCancelled { get { return Volatile.Read(ref cancelled) != 0; } }

        internal void Start(Action read, Action publish)
        {
            this.read = read;
            this.publish = publish;
            lock (gate)
            {
                active?.Cancel();
                pending?.Cancel();
                pending = this;
                if (reading) return;
                reading = true;
                ThreadPool.QueueUserWorkItem(_ => Drain());
            }
        }

        internal void Cancel()
        {
            Interlocked.Exchange(ref cancelled, 1);
            publish = null;
            read = null;
        }

        private static void Drain()
        {
            while (true)
            {
                CompletionDocumentation session;
                lock (gate)
                {
                    session = pending;
                    pending = null;
                    active = session;
                    if (session == null) { reading = false; return; }
                }
                try
                {
                    Action work = session.read;
                    if (!session.IsCancelled && work != null) work();
                    if (!session.IsCancelled && !session.editor.IsDisposed && session.editor.IsHandleCreated)
                        session.editor.BeginInvoke(new Action(session.Publish));
                }
                catch (Exception) { /* Documentation failure must not block the next request. */ }
                finally { session.read = null; }
            }
        }

        private void Publish()
        {
            Action apply = publish;
            publish = null;
            if (!IsCancelled && !editor.IsDisposed && apply != null) apply();
        }
    }
}
