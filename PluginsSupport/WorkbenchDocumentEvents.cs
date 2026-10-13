using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VisualPascalABCPlugins
{
    // Optional extension: do not add members to the historical document service.
    // All calls and notifications run on the IDE thread. Subscribers must not block it.
    public interface IWorkbenchDocumentEvents
    {
        ICodeFileDocument[] GetOpenDocuments();
        event EventHandler<WorkbenchDocumentEventArgs> DocumentOpened;
        event EventHandler<WorkbenchDocumentEventArgs> DocumentClosed;
        event EventHandler<WorkbenchDocumentEventArgs> DocumentRenamed;
    }

    public sealed class WorkbenchDocumentEventArgs : EventArgs
    {
        public ICodeFileDocument Document { get; }
        // Capture the names: an asynchronous consumer must not read a later rename.
        public string FileName { get; }
        public string PreviousFileName { get; }

        public WorkbenchDocumentEventArgs(ICodeFileDocument document, string previousFileName = null)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            FileName = document.FileName;
            PreviousFileName = previousFileName;
        }
    }

    // Reads the host's existing collection, not another registry of open documents.
    public sealed class WorkbenchDocumentEventSource : IWorkbenchDocumentEvents
    {
        private readonly Func<IEnumerable<ICodeFileDocument>> getDocuments;
        private readonly Action<Exception> reportError;

        public WorkbenchDocumentEventSource(Func<IEnumerable<ICodeFileDocument>> getDocuments,
            Action<Exception> reportError = null)
        {
            this.getDocuments = getDocuments ?? throw new ArgumentNullException(nameof(getDocuments));
            this.reportError = reportError ?? (error => Trace.TraceError("Document event subscriber: {0}", error));
        }

        public ICodeFileDocument[] GetOpenDocuments() => getDocuments().Distinct().ToArray();
        public event EventHandler<WorkbenchDocumentEventArgs> DocumentOpened;
        public event EventHandler<WorkbenchDocumentEventArgs> DocumentClosed;
        public event EventHandler<WorkbenchDocumentEventArgs> DocumentRenamed;

        public void Opened(ICodeFileDocument document) => Publish(DocumentOpened,
            new WorkbenchDocumentEventArgs(document));
        // Call only after removal from the open collection, but before disposing the editor.
        public void Closed(ICodeFileDocument document) => Publish(DocumentClosed,
            new WorkbenchDocumentEventArgs(document));
        public void Renamed(ICodeFileDocument document, string previousFileName)
        {
            if (string.Equals(document.FileName, previousFileName, StringComparison.Ordinal)) return;
            Publish(DocumentRenamed, new WorkbenchDocumentEventArgs(document, previousFileName));
        }

        private void Publish(EventHandler<WorkbenchDocumentEventArgs> handlers, WorkbenchDocumentEventArgs args)
        {
            if (handlers == null) return;
            foreach (EventHandler<WorkbenchDocumentEventArgs> handler in handlers.GetInvocationList())
                try { handler(this, args); }
                catch (Exception error)
                {
                    // A plugin failure must not cancel a completed open/save/close operation,
                    // or prevent other subscribers from receiving its notification.
                    try { reportError(error); }
                    catch (Exception) { }
                }
        }
    }
}
