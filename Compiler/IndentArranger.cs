using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IndentArranger
{   
    public class IndentArranger
    {
        private string unitPath;
        private int currentLineIndentLevel = 0;
        private int lineCounter = -1;
        private int previousIndentLevel = -1;

        public string CreatedFileName { get; private set; }
        public string CreatedFilePath { get; private set; }

        // количество пробелов соответствующее одному \t (в одном отступе)
        private const int indentSpaceNumber = 4;
        private const string indentToken = "{";
        private const string unindentToken = "}";
        public const string createdFileNameAddition = "_indented";

        public IndentArranger(string path)
        {
            unitPath = path;

            CreatedFileName =
                Path.GetFileNameWithoutExtension(path) + 
                createdFileNameAddition + Path.GetExtension(path);

            CreatedFilePath = Path.GetDirectoryName(path) + "\\" + CreatedFileName;
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
                    //Console.Write(FirstTokenType.If);
                    return FirstTokenType.If;
                case "elif":
                    //Console.Write(FirstTokenType.Elif);
                    return FirstTokenType.Elif;
                case "else":
                    //Console.Write(FirstTokenType.Else);
                    return FirstTokenType.Else;
                case "for":
                    //Console.Write(FirstTokenType.For);
                    return FirstTokenType.For;
                case "while":
                    //Console.Write(FirstTokenType.While);
                    return FirstTokenType.While;
                case "def":
                    //Console.Write(FirstTokenType.Def);
                    return FirstTokenType.Def;
                default:
                    //Console.Write(FirstTokenType.Id);
                    return FirstTokenType.Id;
            }
        }

        public void ArrangeIndents(ref string program)
        {
            //File.Delete(CreatedFilePath);
            //string[] programLines = File.ReadAllLines(unitPath);
            string[] programLines = program.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int lastNotEmptyLine = -1;
            Stack<FirstTokenType> firstTokensTypesStack = new Stack<FirstTokenType>();

            foreach (var line in programLines)
            {
                lineCounter++;
                // вывод номера текущей строки для дебага (+1 т.к. нумерация с нуля)
                //Console.Write($"{lineCounter + 1}:\t");

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
                    //Console.WriteLine("EmptyLine");
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
                    firstTokensTypesStack.Push(currentFirstTokenType);

                    //Console.Write(indentLiteral);
                    if (lastNotEmptyLine != -1)
                        programLines[lastNotEmptyLine] += indentToken;
                    // закомментировать ветку else если нет блока оборачивающего всю программу
                    //else
                        //File.AppendAllText(CreatedFilePath, indentToken + "\n");
                }
                // текущий отступ соответствует уменьшению на один или несколько отступов
                else if (currentLineIndentLevel < previousIndentLevel)
                {
                    //firstTokensTypesStack.Count - currentLineIndentLevel - 1
                    while (firstTokensTypesStack.Count > currentLineIndentLevel + 1)
                    {
                        firstTokensTypesStack.Pop();
                    }
                    FirstTokenType previousFirstTokenType = firstTokensTypesStack.Pop();

                    // если сейчас ветка elif/else, то это не конец команды
                    // поэтому ставить ; в конце не надо (она будет после блока elif/else)
                    bool isEndOfStatement = !(currentFirstTokenType == FirstTokenType.Elif || currentFirstTokenType == FirstTokenType.Else);

                    firstTokensTypesStack.Push(currentFirstTokenType);

                    //Console.Write(
                    //string.Concat(Enumerable.Repeat(unindentLiteral + " ", previousIndentLevel - currentLineIndentCounter)));
                    programLines[lastNotEmptyLine] +=
                        string.Concat(Enumerable.Repeat(";" + unindentToken, previousIndentLevel - currentLineIndentLevel))
                        + (isEndOfStatement ? ";" : "");
                }
                // текущий отступ  соответствует предыдущему отступу 
                else if (currentLineIndentLevel == previousIndentLevel)
                {
                    programLines[lastNotEmptyLine] += ";";
                    
                    // меняем токен на вершине стека на текущий
                    firstTokensTypesStack.Pop();
                    firstTokensTypesStack.Push(currentFirstTokenType);

                    //Console.Write("Nothing");
                }

                previousIndentLevel = currentLineIndentLevel;
                lastNotEmptyLine = lineCounter;

                //Console.Write(firstTokensTypesStack.Peek() + " #" + firstTokensTypesStack.Count);

                //Console.WriteLine();
            }

            // закрытие всех отступов в конце файла
            // (заменить "currentLineIndentLevel + 1" на "currentLineIndentLevel" если нет блока оборачивающего всю программу)
            programLines[lastNotEmptyLine] +=
                string.Concat(Enumerable.Repeat(";" + unindentToken, currentLineIndentLevel));

            //Console.WriteLine("EOF:\t" + 
            //    string.Concat(Enumerable.Repeat(unindentLiteral + " ", currentLineIndentCounter + 1)));

            program = programLines.JoinIntoString("\n");
            File.AppendAllText(CreatedFilePath, program);
        }
    }


    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        string path;

    //        // только для дебага
    //        if (args.Length == 0)
    //        {
    //            path = "../../test.txt";
    //        }
    //        else
    //        {
    //            path = args[0];
    //        }

    //        try
    //        {
    //            IndentArranger ia = new IndentArranger(path);
    //            ia.ArrangeIndents();
    //        }
    //        catch (IndentArrangerException iae)
    //        {
    //            Console.Write("\r");
    //            Console.WriteLine(iae.Message);
    //        }
    //        catch (IOException ioe)
    //        {
    //            Console.WriteLine(ioe.Message);
    //        }
    //    }
    //}
}
