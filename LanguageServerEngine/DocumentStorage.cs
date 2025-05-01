using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Collections.Generic;
using System.Text;

namespace LanguageServerEngine
{
    internal class DocumentStorage
    {
        private readonly Dictionary<string, StringBuilder> documents = new Dictionary<string, StringBuilder>();

        public void UpdateDocument(string documentPath, TextDocumentContentChangeEvent change)
        {
            var sb = documents[documentPath];

            int startOffset = LspDataConvertor.GetOffsetFromPosition(sb, change.Range.Start);
            int endOffset = LspDataConvertor.GetOffsetFromPosition(sb, change.Range.End);

            // Удаляем старый текст (если диапазон не нулевой)
            if (startOffset != endOffset)
                sb.Remove(startOffset, endOffset - startOffset);

            // Вставляем новый текст
            sb.Insert(startOffset, change.Text);
        }

        public void AddDocument(string documentPath, string documentText)
        {
            documents[documentPath] = new StringBuilder(documentText);
        }

        public void RemoveDocument(string documentPath)
        {
            documents.Remove(documentPath);
        }

        public StringBuilder GetDocument(string documentPath)
        {
            return documents[documentPath];
        }
    }
}
