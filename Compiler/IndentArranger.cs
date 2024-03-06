using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace IndentArranger
{   
    public class IndentArranger
    {
        private int currentLineIndentLevel = 0;
        private int lineCounter = -1;

        public string CreatedFileName { get; private set; }
        public string CreatedFilePath { get; private set; }

        // количество пробелов соответствующее одному \t (в одном отступе)
        private const int indentSpaceNumber = 4;
        private const string indentToken = "#{";
        private const string unindentToken = "#}";
        private const string badIndentToken = "##";
        public const string createdFileNameAddition = "_processed";

        // регулярное выражение разбивающее строку программы на группы, последняя из которых - комментарий в конце строки
        private static readonly Regex programLineRegex = new Regex("^(([^\n\"\'#])+|(\'([^\'\n\\\\]|\\\\.)*\')|(\"([^\"\n\\\\]|\\\\.)*\"))*(#.*)?$");

        public IndentArranger(string path)
        {
            CreatedFileName =
                Path.GetFileNameWithoutExtension(path) + 
                createdFileNameAddition + ".txt";

            CreatedFilePath = Path.GetDirectoryName(path) + "\\" + CreatedFileName;
        }

        public void ProcessSourceText(ref string sourceText)
        {
            string[] programLines = sourceText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // удаляем комментарии в тексте программы
            EraseComments(ref programLines);

            // добавляем токены для начала/конца отступов и ; в конце stmt
            ArrangeIndents(ref programLines);

            sourceText = programLines.JoinIntoString("\n");

            // создание файла с полученным текстом для дебага
            File.WriteAllText(CreatedFilePath, sourceText);
        }

        private void EraseComments(ref string[] programLines)
        {
            for (int i = 0; i < programLines.Length; ++i)
            {
                Match programLineMatch = programLineRegex.Match(programLines[i]);
                if (programLineMatch.Success)
                {
                    Group programLineComment = programLineMatch.Groups[programLineMatch.Groups.Count - 1];
                    if (programLineComment.Success)
                        programLines[i] = programLines[i].Substring(0, programLineComment.Index);
                }
            }
        }

        // считывает первый токен в строке (с первого символа латиницы или _)
        // возвращает тип этого токена
        private bool IsFirstTokenElseOrElif(string line, int beginIndex)
        {
            // узнаём длину токена
            int tokenLength = 1;
            for (int i = beginIndex + 1; i < line.Length; ++i)
            {
                if (Char.IsLetterOrDigit(line[i]) || line[i] == '_')
                    tokenLength++;
                else break;
            }

            // получаем строковое представление токена
            string firstToken = line.Substring(beginIndex, tokenLength);
            return firstToken == "else" || firstToken == "elif";
        }

        private void ArrangeIndents(ref string[] programLines)
        {
            int lastNotEmptyLine = -1;

            Stack<int> indentStack = new Stack<int>();
            indentStack.Push(0);

            foreach (var line in programLines)
            {
                lineCounter++;

                bool isEmptyLine = true; // строка не содержит символов кроме 'space'\t\n\r
                bool readFirstToken = false;
                int currentLineSpaceCounter = 0;
                bool isCurrentFirstTokenElseOrElif = false;

                for (int i = 0; i < line.Length; ++i)
                {
                    switch (line[i])
                    {
                        case ' ':
                            if (isEmptyLine)
                                currentLineSpaceCounter++;
                            break;
                        case '\t':
                            if (isEmptyLine)
                            {
                                // один \t выравнивает до следующего отступа
                                currentLineSpaceCounter += indentSpaceNumber;
                                currentLineSpaceCounter &= ~(indentSpaceNumber - 1);
                            }
                            break;
                        default:
                            if (char.IsWhiteSpace(line[i]))
                                break;

                            isEmptyLine = false;

                            // если символ латиницы или _ то это начало первого токена в строке
                            if (Char.IsLetter(line[i]) || line[i] == '_')
                            {
                                isCurrentFirstTokenElseOrElif = IsFirstTokenElseOrElif(line, i);
                                readFirstToken = true;
                            }
                            break;
                    }
                    if (readFirstToken)
                        break;
                }

                // пропуск строки не содержащей код 
                if (isEmptyLine)
                {
                    continue;
                }

                int previosSpaceCounter = indentStack.Peek();
                // текущий отступ соответствует предыдущему отступу
                if (currentLineSpaceCounter == previosSpaceCounter)
                {
                    if (lastNotEmptyLine != -1)
                        programLines[lastNotEmptyLine] += ";";
                }
                // текущий отступ соответствует увеличению
                else if (currentLineSpaceCounter > previosSpaceCounter)
                {
                    if (lastNotEmptyLine != -1)
                    {
                        programLines[lastNotEmptyLine] += indentToken;
                        indentStack.Push(currentLineSpaceCounter);
                    }
                    else
                    {
                        //ошибка (отступ в первой строке программы)
                    }
                }
                // оставшиеся случаи: отступ некорректный или уменьшение на один или несколько отступов
                else
                {
                    int unindentCounter = 0;
                    while (currentLineSpaceCounter < previosSpaceCounter)
                    { 
                        indentStack.Pop();
                        unindentCounter++;
                        previosSpaceCounter = indentStack.Peek();
                    }

                    // текущий отступ соответствует уменьшению на один или несколько отступов
                    if (currentLineSpaceCounter == previosSpaceCounter)
                    {
                        // если сейчас ветка elif/else, то это не конец команды
                        // поэтому ставить ; в конце не надо (она будет после блока elif/else)
                        bool isEndOfStatement = !isCurrentFirstTokenElseOrElif;

                        programLines[lastNotEmptyLine] +=
                            string.Concat(Enumerable.Repeat(";" + unindentToken, unindentCounter))
                            + (isEndOfStatement ? ";" : "");
                    }
                    // количество пробелов в строке является некорректным 
                    // (не соответствует количеству пробелов в строке с любым отступом) 
                    else // currentLineSpaceCounter > indentStack.Peek()
                    {
                        if (lastNotEmptyLine != -1)
                            programLines[lastNotEmptyLine] += badIndentToken;
                    }
                }

                lastNotEmptyLine = lineCounter;
            }

            // закрытие всех отступов в конце файла
            // (заменить "currentLineIndentLevel + 1" на "currentLineIndentLevel" если есть блок оборачивающий всю программу)
            programLines[lastNotEmptyLine] +=
                string.Concat(Enumerable.Repeat(";" + unindentToken, indentStack.Count() - 1));
        }
    }
}
