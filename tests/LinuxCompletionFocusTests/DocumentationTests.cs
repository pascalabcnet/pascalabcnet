using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

internal static class DocumentationTests
{
    private static void Until(Func<bool> condition, Action pump)
    {
        var watch = Stopwatch.StartNew();
        while (!condition() && watch.ElapsedMilliseconds < 5000) pump();
        if (!condition()) throw new Exception("Timed out waiting for documentation test work.");
    }

    internal static void Run(Action<bool, string> check, Action pump)
    {
        using (var owner = new Form { Text = "Linux documentation cancellation test", Size = new Size(400, 180), ShowInTaskbar = false })
        using (var editor = new TextBox { Dock = DockStyle.Fill })
        using (var started = new ManualResetEvent(false))
        using (var release = new ManualResetEvent(false))
        {
            owner.Controls.Add(editor); owner.Show(); editor.Focus(); pump();
            int uiThread = Thread.CurrentThread.ManagedThreadId;
            int firstPublished = 0, secondRead = 0, lastRead = 0, lastPublished = 0, publishThread = 0;
            var first = new CompletionDocumentation(editor);
            first.Start(() => { started.Set(); if (!release.WaitOne(5000)) throw new Exception("Read gate timeout"); },
                () => firstPublished++);
            try
            {
                if (!started.WaitOne(3000)) throw new Exception("Documentation reader did not start.");
                var watch = Stopwatch.StartNew(); first.Cancel(); watch.Stop();
                bool heartbeat = false;
                editor.BeginInvoke(new Action(() => heartbeat = true));
                editor.AppendText(";x"); pump();
                check(watch.ElapsedMilliseconds < 250 && heartbeat && editor.Text == ";x" && !release.WaitOne(0),
                    "cancellation returns immediately and the UI keeps processing input while XML reading is blocked" +
                    " [cancelMs=" + watch.ElapsedMilliseconds + ", heartbeat=" + heartbeat + ", text=" + editor.Text + ", gate=" + release.WaitOne(0) + "]");

                var second = new CompletionDocumentation(editor);
                second.Start(() => Interlocked.Increment(ref secondRead), () => { throw new Exception("Obsolete request published"); });
                var last = new CompletionDocumentation(editor);
                last.Start(() => Interlocked.Increment(ref lastRead), () => { lastPublished++; publishThread = Thread.CurrentThread.ManagedThreadId; });
                release.Set();
                Until(() => lastPublished == 1, pump);
                check(firstPublished == 0 && secondRead == 0 && lastRead == 1 && publishThread == uiThread,
                    "only the latest request runs after a slow cancelled read and publishes on the UI thread");
            }
            finally { release.Set(); first.Cancel(); }

            int queuedPublished = 0;
            using (var readDone = new ManualResetEvent(false))
            {
                var queued = new CompletionDocumentation(editor);
                queued.Start(() => readDone.Set(), () => queuedPublished++);
                if (!readDone.WaitOne(3000)) throw new Exception("Documentation reader did not finish.");
                // Do not pump the UI until cancellation: publication may already be queued.
                Thread.Sleep(40); queued.Cancel(); pump();
                check(queuedPublished == 0, "cancellation discards a result already queued for UI publication");
            }

            using (var disposedEditor = new TextBox())
            using (var disposingRead = new ManualResetEvent(false))
            using (var disposingRelease = new ManualResetEvent(false))
            {
                owner.Controls.Add(disposedEditor); disposedEditor.CreateControl();
                int disposedPublished = 0, nextPublished = 0;
                var disposed = new CompletionDocumentation(disposedEditor);
                disposed.Start(() => { disposingRead.Set(); disposingRelease.WaitOne(5000); }, () => disposedPublished++);
                try
                {
                    if (!disposingRead.WaitOne(3000)) throw new Exception("Disposal reader did not start.");
                    disposedEditor.Dispose(); disposingRelease.Set();
                    var next = new CompletionDocumentation(editor);
                    next.Start(() => { }, () => nextPublished++);
                    Until(() => nextPublished == 1, pump);
                    check(disposedPublished == 0, "disposing the editor during a read is harmless and the next request succeeds");
                }
                finally { disposingRelease.Set(); disposed.Cancel(); }
            }

            int recovered = 0;
            var failed = new CompletionDocumentation(editor);
            failed.Start(() => { throw new InvalidOperationException("Simulated XML read failure"); },
                () => { throw new Exception("Failed read published"); });
            pump();
            var recovery = new CompletionDocumentation(editor);
            recovery.Start(() => { }, () => recovered++);
            Until(() => recovered == 1, pump);
            check(recovered == 1, "a failed documentation read does not block later completion requests");
        }
        pump();
    }
}
