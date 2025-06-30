// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace LanguageServerEngine
{
    internal class DocumentStorage : PascalABCCompiler.ISourceTextProvider
    {
        private bool incrementalModeOn;

        private readonly ConcurrentDictionary<string, ITextBuffer> documents = new ConcurrentDictionary<string, ITextBuffer>();

        public void SwitchOnIncrementalMode()
        {
            incrementalModeOn = true;

            foreach (var kv in documents.ToArray())
            {
                documents[kv.Key] = new TextBufferIncremental(kv.Value.GetText());
            }
        }

        public void UpdateDocument(string documentPath, TextDocumentContentChangeEvent change)
        {
            documents[documentPath].ApplyChange(change);
        }

        public void AddDocument(string documentPath, string documentText)
        {
            if (incrementalModeOn)
            {
                documents[documentPath] = new TextBufferIncremental(documentText);
            }
            else
            {
                documents[documentPath] = new TextBufferFull(documentText);
            }
        }

        public void RemoveDocument(string documentPath)
        {
            documents.TryRemove(documentPath, out _);
        }

        public ITextInfoProviderByPosition GetTextInfoProvider(string documentPath)
        {
            return documents[documentPath];
        }

        public string GetText(string filePath) => GetDocumentText(filePath);

        public string GetDocumentText(string documentPath)
        {
            return documents[documentPath].GetText();
        }

        private interface ITextBuffer : ITextInfoProviderByPosition
        {
            /// <summary> Применяет изменение текста. </summary>
            void ApplyChange(TextDocumentContentChangeEvent change);

            /// <summary> Возвращает весь текст. </summary>
            string GetText();
        }

        private class TextBufferIncremental : ITextBuffer
        {
            private List<string> lines;

            public TextBufferIncremental(string text)
            {
                UpdateText(text);
            }

            private void UpdateText(string text)
            {
                lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }

            public void ApplyChange(TextDocumentContentChangeEvent change)
            {
                var range = change.Range;

                int startLine = (int)range.Start.Line;
                int startCol = (int)range.Start.Character;
                int endLine = (int)range.End.Line;
                int endCol = (int)range.End.Character;

                if (change.RangeLength > 0)
                    Delete(startLine, startCol, endLine, endCol);

                Insert(startLine, startCol, change.Text);
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

            public char GetSymbolAtPos(int line, int pos) => lines[line][pos];

            public string GetText() => string.Join(Environment.NewLine, lines);
        }

        private class TextBufferFull : ITextBuffer
        {
            private string text;

            public TextBufferFull(string text)
            {
                this.text = text;
            }

            public void ApplyChange(TextDocumentContentChangeEvent change)
            {
                this.text = change.Text;
            }

            public int GetOffsetFromPosition(int line, int column)
            {
                int currentLine = 0;
                int offset = 0;

                while (currentLine < line && offset < text.Length)
                {
                    int newLinePos = text.IndexOf(Environment.NewLine, offset);

                    offset = newLinePos + Environment.NewLine.Length;
                    
                    currentLine++;
                }

                return offset + column;
            }

            public char GetSymbolAtPos(int line, int pos)
            {
                int offset = GetOffsetFromPosition(line, pos);

                return text[offset];
            }

            public string GetText() => text;
        }
    }

    internal interface ITextInfoProviderByPosition
    {
        /// <summary> Возвращает индекс символа по позиции (line, column). </summary>
        int GetOffsetFromPosition(int line, int column);

        /// <summary> Возвращает символ по позиции. </summary>
        char GetSymbolAtPos(int line, int pos);
    }
    
}
