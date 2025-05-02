using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Text;

namespace LanguageServerEngine
{
    internal static class LspDataConvertor
    {
        // Конвертирует LSP-позицию (строка, символ) в индекс строки
        internal static int GetOffsetFromPosition(StringBuilder sb, Position position)
        {
            int currentLine = 0;
            int currentOffset = 0;

            while (currentLine < position.Line && currentOffset < sb.Length)
            {
                if (sb[currentOffset] == '\n')
                    currentLine++;
                currentOffset++;
            }

            return currentOffset + (int)position.Character;
        }

        public static string GetTextInRange(string documentText, Range range)
        {
            if (string.IsNullOrEmpty(documentText))
                return string.Empty;

            var lines = documentText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Проверка на выход за границы документа
            if (range.Start.Line >= lines.Length || range.End.Line >= lines.Length)
                return string.Empty;

            // Если диапазон начинается и заканчивается на одной строке
            if (range.Start.Line == range.End.Line)
            {
                string line = lines[range.Start.Line];
                int start = Math.Min((int)range.Start.Character, line.Length);
                int end = Math.Min((int)range.End.Character, line.Length);
                return line.Substring(start, end - start);
            }

            // Если диапазон охватывает несколько строк
            var result = new StringBuilder();

            // Первая строка (от Start.Character до конца строки)
            string firstLine = lines[range.Start.Line];
            int firstLineStart = (int)Math.Min(range.Start.Character, firstLine.Length);
            result.Append(firstLine.Substring(firstLineStart));

            // Промежуточные строки (если есть)
            for (int i = (int)range.Start.Line + 1; i < range.End.Line; i++)
            {
                result.AppendLine(); // или просто result.Append("\n"), если не нужны \r\n
                result.Append(lines[i]);
            }

            // Последняя строка (от начала до End.Character)
            if (range.Start.Line != range.End.Line)
            {
                result.AppendLine();
                string lastLine = lines[range.End.Line];
                int lastLineEnd = (int)Math.Min(range.End.Character, lastLine.Length);
                result.Append(lastLine.Substring(0, lastLineEnd));
            }

            return result.ToString();
        }
    }
}
