using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;

namespace LanguageServerEngine
{
    internal static class LspDataConvertor
    {

        internal static Range GetRangeForSymbolAtPos(string line, Position pos)
        {
            int i = (int)pos.Character;

            int j = 1;

            while (i - j > 0 && (char.IsLetterOrDigit(line[i - j]) || line[i - j] == '_' || line[i - j] == '&' || line[i - j] == '!'))
            {
                j++;
            }

            int startColumn = i - j + 1;

            int endColumn;

            // для случая, когда символ справа от позиции
            if (!(char.IsLetterOrDigit(line[i]) || line[i] == '_' || line[i] == '&' || line[i] == '!'))
            {
                endColumn = i;
                return new Range(new Position(pos.Line, startColumn), new Position(pos.Line, endColumn));
            }

            j = 1;

            while (i + j < line.Length && (char.IsLetterOrDigit(line[i + j]) || line[i + j] == '_' || line[i + j] == '&' || line[i + j] == '!'))
            {
                j++;
            }

            endColumn = i + j;

            return new Range(new Position(pos.Line, startColumn), new Position(pos.Line, endColumn));
        }

        internal static string GetNormalizedPathFromUri(Uri uri)
        {
            string path = uri.LocalPath;

            if (path.StartsWith("/") && path.Length > 2 && char.IsLetter(path[1]) && path[2] == ':')
            {
                path = path.Substring(1);
            }

            return path;
        }
    }
}
