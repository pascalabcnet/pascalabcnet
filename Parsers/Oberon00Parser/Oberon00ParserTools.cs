using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using QUT.Gppg;
using GPPGParserScanner;

namespace PascalABCCompiler.Oberon00Parser
{
    // Класс глобальных описаний и статических методов
    // для использования различными подсистемами парсера и сканера
    public static class PT // PT - parser tools
    {
        public static List<Error> Errors;
        /// Последний прочтенный идентификатор
        public static string LastIdentificator = "";
        //public static int max_errors;
        public static string CurrentFileName;
        /// Словарь стандартных типов с соответствующими внутренними именами
        public static Dictionary<string, string> standartTypes = new Dictionary<string, string>();

        /// Статический конструктор
        static PT() {
            standartTypes.Add("BOOLEAN", "boolean");
            standartTypes.Add("INTEGER", "integer");
            standartTypes.Add("SHORTINT", "byte");
            standartTypes.Add("LONGINT", "int64");
            standartTypes.Add("REAL", "real");
            standartTypes.Add("LONGREAL", "double");
            standartTypes.Add("CHAR", "char");
            standartTypes.Add("STRING", "string");
        }

        public static SourceContext ToSourceContext(LexLocation loc)
        {
            if (loc != null)
                return new SourceContext(loc.StartLine, loc.StartColumn + 1, loc.EndLine, loc.EndColumn);
            return null;
        }

        public static string CreateErrorString(params object[] args)
        {
            string[] ww = new string[args.Length - 1];
            for (int i = 1; i < args.Length; i++)
                ww[i - 1] = GetStrByTokenName((string)args[i]);
            string w = string.Join(" или ", ww);

            string got = (string)args[0];
            if (got.Equals("ID"))
                got = "'" + LastIdentificator + "'";
            else
                got = GetStrByTokenName(got);
            return string.Format("Синтаксическая ошибка: встречено {0}, а ожидалось {1}", got, w);
        }

        public static void AddError(string message, LexLocation loc)
        {
            Errors.Add(new SyntaxError(message, CurrentFileName, ToSourceContext(loc), null));
        }

        /// Определяет содержимое строки по ее представлению в тексте программы
        /// <param name="sourceStr">Строка с кавычками</param>
        /// <returns>Содержимое строки без кавычек</returns>
        public static string GetStringContent(string sourceStr) {
            bool hasStrType = (sourceStr.IndexOf('\'') != -1) ||
                (sourceStr.IndexOf('"') != -1);
            if (hasStrType)
                return sourceStr.Substring(1, sourceStr.Length - 2);
            else {      // символ представлен 16ным кодом
                string hexCode = sourceStr.Remove(sourceStr.Length - 1);
                int tryParseHex;
                if (int.TryParse(hexCode, System.Globalization.NumberStyles.AllowHexSpecifier, null, out tryParseHex))
                    return ((char)tryParseHex).ToString();
                else
                    throw new System.ArgumentException("Некорректный шестнадцатеричный код символа");
            }
        }

        /// Преобразует вещественный литерал, принятый в Обероне, к виду .NET
        /// <param name="sourceDoubleStr">Исходная строка вещественного числа</param>
        /// <returns>Корректную строку - вещественное число</returns>
        public static string GetCorrectDoubleStr(string sourceDoubleStr) {
            string correct = sourceDoubleStr.Replace(".",
                System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            correct = correct.Replace('D', 'E');
            return correct;
        }

        /// Определяет внутреннее имя типа по типу в программе
        /// <param name="typeName">Имя типа в программе</param>
        /// <returns>Корректное внутреннее имя типа</returns>
        public static string InternalTypeName(string typeName) {
            if (standartTypes.ContainsKey(typeName))
                return standartTypes[typeName];
            else
                return typeName;
        }


        // -----------------------------------------------------------------------------
        // Вспомогательные методы
        // -----------------------------------------------------------------------------
        /// Возвращает по имени токена представляющую его строку
        private static string GetStrByTokenName(string tokenName) {
            switch (tokenName) {
                case "ASSIGN": return "':='";
                case "SEMICOLUMN": return "';'";
                case "COLON": return "':'";
                case "COMMA": return "'.'";
                case "COLUMN": return "','";
                case "LPAREN": return "'('";
                case "RPAREN": return "')'";
                case "PLUS": return "'+'";
                case "MINUS": return "'-'";
                case "MULT": return "'*'";
                case "DIVIDE": return "'/'";
                case "LT": return "'<'";
                case "GT": return "'>'";
                case "LE": return "'<='";
                case "GE": return "'>='";
                case "EQ": return "'='";
                case "NE": return "'#'";
                case "NOT": return "'~'";
                case "AND": return "'&'";
                case "EXCLAMATION": return "'!'";
                case "ID":  return "идентификатор";
                case "EOF": return "конец программы";
                case "INTNUM": return "целое число";
                default: return tokenName;
            }
        }
    }
}