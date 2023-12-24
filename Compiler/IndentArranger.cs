using System;
using System.Linq;
using System.IO;

namespace IndentArranger
{   
    public class IndentArranger
    {
        private int currentLineIndentLevel = 0;
        private int lineCounter = -1;
        private int previousIndentLevel = -1;

        public string CreatedFileName { get; private set; }
        public string CreatedFilePath { get; private set; }

        // количество пробелов соответствующее одному \t (в одном отступе)
        private const int indentSpaceNumber = 4;
        private const string indentToken = "#{";
        private const string unindentToken = "#}";
        public const string createdFileNameAddition = "_processed";

        public IndentArranger(string path)
        {
            CreatedFileName =
                Path.GetFileNameWithoutExtension(path) + 
                createdFileNameAddition + ".txt";

            CreatedFilePath = Path.GetDirectoryName(path) + "\\" + CreatedFileName;
        }

        public void ProcessSourceText(ref string sourceText)
        {
            // добавляем токены для начала/конца отступов и ; в конце stmt
            ArrangeIndents(ref sourceText);

            // создание файла с полученным текстом для дебага
            File.WriteAllText(CreatedFilePath, sourceText);
        }

        // возможные типы первого токена в строке
        private enum FirstTokenType { Id, If, Elif, Else, For, Def, While, Bad };

        // считывает первый токен в строке (с первого символа латиницы или _)
        // возвращает тип этого токена
        private FirstTokenType ReadFirstToken(string line, int beginIndex)
        {
            // узнаём длину токена
            int tokenLength = 1;
            for (int i = beginIndex + 1; i < line.Length; ++i)
            {
                if (Char.IsLetterOrDigit(line[i]) || line[i] == '_')
                    tokenLength++;
                else break;
            }

            // получаем тип токена по его строковому представлению
            string firstToken = line.Substring(beginIndex, tokenLength);
            switch (firstToken)
            {
                case "if":
                    return FirstTokenType.If;
                case "elif":
                    return FirstTokenType.Elif;
                case "else":
                    return FirstTokenType.Else;
                case "for":
                    return FirstTokenType.For;
                case "while":
                    return FirstTokenType.While;
                case "def":
                    return FirstTokenType.Def;
                default:
                    return FirstTokenType.Id;
            }
        }

        private void ArrangeIndents(ref string program)
        {
            string[] programLines = program.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int lastNotEmptyLine = -1;

            foreach (var line in programLines)
            {
                lineCounter++;

                bool isEmptyLine = true; // строка не содержит символов кроме 'space'\t\n\r
                bool readFirstToken = false;
                int currentLineSpaceCounter = 0;
                FirstTokenType currentFirstTokenType = FirstTokenType.Bad;

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
                                currentFirstTokenType = ReadFirstToken(line, i);
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

                // количество отступов в текущей строке 
                currentLineIndentLevel = currentLineSpaceCounter / indentSpaceNumber;

                // количество пробелов в строке является некорректным 
                // (не соответствует количеству пробелов в строке с любым отступом) 
                if (currentLineSpaceCounter % indentSpaceNumber != 0)
                {
                    throw new NotPossibleIndentException();
                }
                // текущий отступ соответствует увеличению на больше чем один отступ 
                else if (currentLineIndentLevel > previousIndentLevel + 1)
                {
                    throw new ManyIndentsAdditionException();
                }
                // текущий отступ соответствует увеличению на один отступ 
                else if (currentLineIndentLevel == previousIndentLevel + 1)
                {
                    if (lastNotEmptyLine != -1)
                        programLines[lastNotEmptyLine] += indentToken;
                    // закомментировать ветку else если нет блока оборачивающего всю программу
                    //else
                        //File.AppendAllText(CreatedFilePath, indentToken + "\n");
                }
                // текущий отступ соответствует уменьшению на один или несколько отступов
                else if (currentLineIndentLevel < previousIndentLevel)
                {
                    // если сейчас ветка elif/else, то это не конец команды
                    // поэтому ставить ; в конце не надо (она будет после блока elif/else)
                    bool isEndOfStatement = !(currentFirstTokenType == FirstTokenType.Elif || currentFirstTokenType == FirstTokenType.Else);

                    programLines[lastNotEmptyLine] +=
                        string.Concat(Enumerable.Repeat(";" + unindentToken, previousIndentLevel - currentLineIndentLevel))
                        + (isEndOfStatement ? ";" : "");
                }
                // текущий отступ  соответствует предыдущему отступу 
                else if (currentLineIndentLevel == previousIndentLevel)
                {
                    programLines[lastNotEmptyLine] += ";";
                }

                previousIndentLevel = currentLineIndentLevel;
                lastNotEmptyLine = lineCounter;
            }

            // закрытие всех отступов в конце файла
            // (заменить "currentLineIndentLevel + 1" на "currentLineIndentLevel" если есть блок оборачивающий всю программу)
            programLines[lastNotEmptyLine] +=
                string.Concat(Enumerable.Repeat(";" + unindentToken, currentLineIndentLevel));

            program = programLines.JoinIntoString("\n");
        }
    }
}
