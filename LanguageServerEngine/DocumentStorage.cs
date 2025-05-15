using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageServerEngine
{
    internal class DocumentStorage
    {
        private readonly Dictionary<string, TextBuffer> documents = new Dictionary<string, TextBuffer>();

        public void UpdateDocument(string documentPath, TextDocumentContentChangeEvent change)
        {
            var buffer = documents[documentPath];

            var range = change.Range;

            int startLine = (int)range.Start.Line;
            int startCol = (int)range.Start.Character;
            int endLine = (int)range.End.Line;
            int endCol = (int)range.End.Character;

            if (change.RangeLength > 0)
                buffer.Delete(startLine, startCol, endLine, endCol);

            buffer.Insert(startLine, startCol, change.Text);
        }

        public void AddDocument(string documentPath, string documentText)
        {
            documents[documentPath] = new TextBuffer(documentText);
        }

        public void RemoveDocument(string documentPath)
        {
            documents.Remove(documentPath);
        }

        public TextBuffer GetDocumentBuffer(string documentPath)
        {
            return documents[documentPath];
        }

        public string GetDocumentText(string documentPath)
        {
            return documents[documentPath].GetFullText();
        }
    }

    internal class TextBuffer
    {
        private List<string> lines;

        public TextBuffer(string initialText)
        {
            UpdateText(initialText);
        }

        /// <summary> Обновляет весь текст</summary>
        public void UpdateText(string text)
        {
            lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        /// <summary> Вставляет текст в указанную позицию. </summary>
        public void Insert(int line, int column, string text)
        {
            string originalLine = lines[line];

            // Если вставляемый текст содержит новые строки — разбиваем
            string[] insertedLines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if (insertedLines.Length == 1)
            {
                lines[line] = originalLine.Insert(column, insertedLines[0]);
            }
            else
            {
                string firstPart = originalLine.Substring(0, column);
                string lastPart = originalLine.Substring(column);

                var newLinesToInsert = new List<string>() { firstPart + insertedLines[0] };

                newLinesToInsert.AddRange(insertedLines.Skip(1).Take(insertedLines.Length - 2));

                newLinesToInsert.Add(insertedLines[insertedLines.Length - 1] + lastPart);

                lines.RemoveAt(line);
                lines.InsertRange(line, newLinesToInsert);
            }
        }

        /// <summary> Удаляет текст из указанного диапазона. </summary>
        public void Delete(int startLine, int startColumn, int endLine, int endColumn)
        {
            // Удаление в пределах одной строки
            if (startLine == endLine)
            {
                string line = lines[startLine];
                lines[startLine] = line.Remove(startColumn, endColumn - startColumn);
            }
            else
            {
                // Удаление нескольких строк
                string firstPart = lines[startLine].Substring(0, startColumn);
                string lastPart = lines[endLine].Substring(endColumn);

                int linesToRemove = endLine - startLine;

                lines.RemoveRange(startLine + 1, linesToRemove);

                // Объединяем оставшиеся части
                lines[startLine] = firstPart + lastPart;
            }
        }

        public int GetOffsetFromPosition(int line, int column)
        {
            int newLineLen = Environment.NewLine.Length;

            return lines.Take(line).Sum(l => l.Length + newLineLen) + column;
        }

        /// <summary> Возвращает строку по индексу. </summary>
        public string GetLine(int line) => lines[line];

        public List<string> GetLines() => lines;

        /// <summary> Возвращает весь текст. </summary>
        public string GetFullText() => string.Join(Environment.NewLine, lines);

        /// <summary> Количество строк. </summary>
        public int LineCount => lines.Count;
    }
}
