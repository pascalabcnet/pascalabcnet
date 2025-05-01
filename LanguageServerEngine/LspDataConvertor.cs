using OmniSharp.Extensions.LanguageServer.Protocol.Models;
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

            return currentOffset + position.Character;
        }
    }
}
