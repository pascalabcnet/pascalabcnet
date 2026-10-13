using System;
using System.Linq;
using VisualPascalABCPlugins;

namespace VisualPascalABC
{
    public partial class Form1 : IWorkbenchDocumentEvents
    {
        private WorkbenchDocumentEventSource documentEvents;
        private WorkbenchDocumentEventSource DocumentEvents => documentEvents ??
            (documentEvents = new WorkbenchDocumentEventSource(() =>
                OpenDocuments.Values.Where(document => !document.IsDisposed).Cast<ICodeFileDocument>()));

        ICodeFileDocument[] IWorkbenchDocumentEvents.GetOpenDocuments() => DocumentEvents.GetOpenDocuments();
        event EventHandler<WorkbenchDocumentEventArgs> IWorkbenchDocumentEvents.DocumentOpened
        {
            add { DocumentEvents.DocumentOpened += value; }
            remove { DocumentEvents.DocumentOpened -= value; }
        }
        event EventHandler<WorkbenchDocumentEventArgs> IWorkbenchDocumentEvents.DocumentClosed
        {
            add { DocumentEvents.DocumentClosed += value; }
            remove { DocumentEvents.DocumentClosed -= value; }
        }
        event EventHandler<WorkbenchDocumentEventArgs> IWorkbenchDocumentEvents.DocumentRenamed
        {
            add { DocumentEvents.DocumentRenamed += value; }
            remove { DocumentEvents.DocumentRenamed -= value; }
        }

        private void NotifyDocumentOpened(CodeFileDocumentControl document) => documentEvents?.Opened(document);
        private void NotifyDocumentClosed(CodeFileDocumentControl document) => documentEvents?.Closed(document);
        private void NotifyDocumentRenamed(CodeFileDocumentControl document, string previousFileName) =>
            documentEvents?.Renamed(document, previousFileName);
    }
}
