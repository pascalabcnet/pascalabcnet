// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using CodeCompletion;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanguageServerEngine
{
    internal static class LspDataConvertor
    {
        internal static string GetLineByOffset(string text, int offset)
        {
            int firstNewLine = text.LastIndexOf(Environment.NewLine, offset);

            if (firstNewLine == -1)
                firstNewLine = -Environment.NewLine.Length;

            int lastNewLine = text.IndexOf(Environment.NewLine, offset);

            if (lastNewLine == -1)
                lastNewLine = text.Length;

            int startIndex = firstNewLine + Environment.NewLine.Length;

            return text.Substring(startIndex, lastNewLine - startIndex);
        }


        internal static Range GetRangeForSymbolAtPos(string line, Position pos)
        {
            int i = (int)pos.Character;

            int j = 1;

            while (i - j >= 0 && (char.IsLetterOrDigit(line[i - j]) || line[i - j] == '_' || line[i - j] == '&' || line[i - j] == '!'))
            {
                j++;
            }

            int startColumn = i - j + 1;

            int endColumn;

            // для случая, когда символ слева от позиции
            if (i >= line.Length || !char.IsLetterOrDigit(line[i]))
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

        internal static List<SignatureInformation> GetLspMethodsInfoFromStringDescriptions(string[] descriptions, bool forIndexers)
        {
            var resultMethods = new List<SignatureInformation>();

            foreach (var description in descriptions)
            {
                int index = description.IndexOf('\n');

                string label = description.Substring(0, index != -1 ? index : description.Length);

                string documentation = index != -1 ? description.Substring(index + 1) : null;

                List<string> parameters = new List<string>();

                var languageInfo = CodeCompletionController.CurrentParser.LanguageInformation;

                // подсказка для индексов
                if (forIndexers)
                {
                    int startIndex = label.IndexOf('[') + 1;

                    var descriptionAfterBracket = label.Substring(startIndex);

                    int paramNum = 1;

                    int firstCharIndex = 0;

                    while (true)
                    {
                        int paramDelimIndex = languageInfo.FindParamDelimForIndexer(descriptionAfterBracket, paramNum);

                        if (paramDelimIndex == -1)
                        {
                            int closingIndex = languageInfo.FindClosingParenthesis(descriptionAfterBracket, ']');

                            if (closingIndex - firstCharIndex > 1)
                            {
                                parameters.Add(descriptionAfterBracket.Substring(firstCharIndex, closingIndex - firstCharIndex));
                            }
                            break;
                        }
                        else
                        {
                            parameters.Add(descriptionAfterBracket.Substring(firstCharIndex, paramDelimIndex - firstCharIndex));
                        }

                        paramNum++;

                        firstCharIndex = paramDelimIndex + languageInfo.DelimiterInIndexer.Length;
                    }
                }
                // подсказка для обычных методов
                else
                {
                    int startIndex = label.IndexOf("(", label.IndexOf("(" + PascalABCCompiler.StringResources.Get("CODE_COMPLETION_EXTENSION")) + 1) + 1;

                    var descriptionAfterBracket = label.Substring(startIndex);

                    int paramNum = 1;

                    int firstCharIndex = 0;

                    while (true)
                    {
                        int paramDelimIndex = languageInfo.FindParamDelim(descriptionAfterBracket, paramNum);

                        if (paramDelimIndex == -1)
                        {
                            int closingIndex = languageInfo.FindClosingParenthesis(descriptionAfterBracket, ')');

                            if (closingIndex - firstCharIndex > 1)
                            {
                                parameters.Add(descriptionAfterBracket.Substring(firstCharIndex, closingIndex - firstCharIndex));
                            }
                            break;
                        }
                        else
                        {
                            parameters.Add(descriptionAfterBracket.Substring(firstCharIndex, paramDelimIndex - firstCharIndex));
                        }

                        paramNum++;

                        firstCharIndex = paramDelimIndex + languageInfo.ParameterDelimiter.Length;
                    }
                }

                // Проверка нужна, потому что иначе Documentation сформируется неправильно при присвоении null
                if (documentation != null)
                {
                    var info = new SignatureInformation()
                    {
                        Documentation = documentation,
                        Label = label,
                        Parameters = parameters.Select(param => new ParameterInformation() { Label = param }).ToArray()
                    };
                    resultMethods.Add(info);
                }
                else
                {
                    var info = new SignatureInformation()
                    {
                        Label = label,
                        Parameters = parameters.Select(param => new ParameterInformation() { Label = param }).ToArray()
                    };
                    resultMethods.Add(info);
                }
            }

            return resultMethods;
        }
    }
}
