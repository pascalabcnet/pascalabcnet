using System;
using System.Collections.Generic;
using System.IO;

namespace VisualPascalABCPlugins
{
    internal static class Net10EditorDocument
    {
        public static ICodeFileDocument ResolveRunTarget(ICodeFileDocument current, string outputFile,
            IWorkbenchDocumentService service)
        {
            string extension = Path.GetExtension(outputFile ?? "");
            var active = service.ActiveCodeFileDocument;
            // Match the legacy Run fallback only for a compiled unit/library, not another program.
            if ((string.Equals(extension, ".pcu", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(extension, ".dll", StringComparison.OrdinalIgnoreCase)) &&
                active != null && active != current && !active.FromMetadata && !active.Run &&
                ReferenceEquals(service.GetDocument(active.FileName), active))
                return active;
            return current;
        }

        public static List<Net10EditorSource> ReadOpen(IEnumerable<ICodeFileDocument> documents,
            IWorkbenchDocumentService service)
        {
            var result = new List<Net10EditorSource>();
            foreach (var document in documents)
                // ContainsTab(document) checks the debugger's TabStack, not OpenDocuments.
                if (!document.FromMetadata && ReferenceEquals(service.GetDocument(document.FileName), document))
                    result.Add(Read(document));
            return result;
        }

        public static Net10EditorSource Read(ICodeFileDocument document)
        {
            var editor = document.TextEditor;
            if (editor == null || editor.Document == null)
                throw new InvalidOperationException("Недоступен текст редактора: " + document.FileName);
            // ICodeFileDocument.Text is the DockContent caption, not Pascal source.
            return new Net10EditorSource { FileName = document.FileName,
                Text = editor.Document.TextContent,
                Changed = document.DocumentChanged, FromMetadata = document.FromMetadata };
        }
    }
}
