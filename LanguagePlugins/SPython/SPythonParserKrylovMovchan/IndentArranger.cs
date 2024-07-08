using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SPythonParser
{   
    public class IndentArranger
    {
        private int lineCounter = -1;

        // количество пробелов соответствующее одному \t (в одном отступе)
        private const int indentSpaceNumber = 4;
        private const string indentToken = "#{";
        private const string unindentToken = "#}";
        private const string badIndentToken = "##";
        public const string createdFileNameAddition = "_processed";

        // регулярное выражение разбивающее строку программы на группы, последняя из которых - комментарий в конце строки
        private static readonly Regex programLineRegex = new Regex("^(([^\n\"\'#])+|(\'([^\'\n\\\\]|\\\\.)*\')|(\"([^\"\n\\\\]|\\\\.)*\"))*(#.*)?$");

        public IndentArranger() {}

        public void ProcessSourceText(ref string sourceText)
        {
            string[] programLines = sourceText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // удаляем комментарии в тексте программы
            EraseComments(ref programLines);

            // добавляем токены для начала/конца отступов и ; в конце stmt
            ArrangeIndents(ref programLines);

            sourceText = String.Join("\n", programLines);

            // создание файла с полученным текстом для дебага
            //File.WriteAllText("./processed_file.txt", sourceText);
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
        private bool IsFirstTokenElseOrElif(string line)
        {
            int beginIndex = 0;
            for (beginIndex = 0; beginIndex < line.Length; ++beginIndex)
                if (!Char.IsWhiteSpace(line[beginIndex]))
                    break;
            if (line.Length < beginIndex + 4) return false;

            // получаем строковое представление токена
            string firstToken = line.Substring(beginIndex, 4);
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

                bool isEmptyLine = true; // строка не содержит символов кроме \s\t\n\r
                int currentLineSpaceCounter = 0;

                for (int i = 0; i < line.Length; ++i)
                {
                    if (line[i] == '\t')
                    {
                        // один \t выравнивает до следующего отступа
                        currentLineSpaceCounter += indentSpaceNumber;
                        currentLineSpaceCounter &= ~(indentSpaceNumber - 1);
                    }
                    else if (char.IsWhiteSpace(line[i]))
                        currentLineSpaceCounter++;
                    else
                    {
                        isEmptyLine = false;
                        break;
                    }
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
                        AddSemicolonIfNeeded(ref programLines[lastNotEmptyLine]);
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
                        programLines[0] = badIndentToken + programLines[0];
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
                        bool isEndOfStatement = !IsFirstTokenElseOrElif(line);

                        unindentCounter--;
                        AddSemicolonIfNeeded(ref programLines[lastNotEmptyLine]);

                        programLines[lastNotEmptyLine] += unindentToken +
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


            if (lastNotEmptyLine != -1 && indentStack.Count() > 1)
            {
                // закрытие всех отступов в конце файла
                AddSemicolonIfNeeded(ref programLines[lastNotEmptyLine]);
                programLines[lastNotEmptyLine] += unindentToken +
                    string.Concat(Enumerable.Repeat(";" + unindentToken, indentStack.Count() - 2));
            }
        }

        private void AddSemicolonIfNeeded(ref string s)
        {
            int i = s.Length - 1;
            while (i > -1 && Char.IsWhiteSpace(s[i])) --i;
            if (i == -1 || s[i] != ';') s += ";";
        }
    }
}
