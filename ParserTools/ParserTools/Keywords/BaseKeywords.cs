// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PascalABCCompiler.Parsers
{
    public abstract class BaseKeywords
    {
        /// <summary>
        /// Соотвествие ключевых слов токенам (ключевые слова для парсера)
        /// </summary>
        public Dictionary<string, int> KeywordsToTokens { get; set; }

        /// <summary>
        /// Типы ключевых слов
        /// </summary>
        public Dictionary<string, KeywordKind> KeywordKinds { get; set; }

        /// <summary>
        /// Ключевые слова в контексте Intellisense (множество)
        /// </summary>
        public HashSet<string> KeywordsForIntellisenseSet { get; }

        /// <summary>
        /// Ключевые слова в контексте Intellisense (список)
        /// </summary>
        public List<string> KeywordsForIntellisenseList { get; set; } = new List<string>();

        /// <summary>
        /// Ключевые слова, соответствующие типам (array, set), либо классу, функции
        /// </summary>
        public List<string> TypeKeywords { get; set; } = new List<string>();

        /// <summary>
        /// Ключевые слова, которые должны восприниматься как функции при парсинге выражений
        /// </summary>
        public HashSet<string> KeywordsTreatedAsFunctions { get; set; }

        /// <summary>
        /// Словарь соответствий ключевых слов их эквивалентам (задается пользователем в специальном файле)
        /// </summary>
        private Dictionary<string, string> keymap = new Dictionary<string, string>();

        /// <summary>
        /// Название файла с информацией об эквивалентах ключевых слов
        /// </summary>
        protected abstract string FileName { get; }

        /// <summary>
        /// Функция загрузки эквивалентов ключевых слов из файла
        /// </summary>
        private void ReloadKeyMap()
        {
            try
            {
                var pathToKeymapFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName), FileName);
                
                if (System.IO.File.Exists(pathToKeymapFile))
                    keymap = System.IO.File.ReadLines(pathToKeymapFile, Encoding.Unicode).Select(s => s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(w => w[0], w => w[1]);
            }
            catch (Exception)
            {
                //var w = e.Message;
            }
        }

        /// <summary>
        /// Возвращает эквивалент ключевого слова, либо его самого, если эквивалента нет
        /// </summary>
        public string ConvertKeyword(string keyword)
        {
            if (keymap.Count() == 0 || !keymap.ContainsKey(keyword))
                return keyword;
            else 
                return keymap[keyword];
        }

        public BaseKeywords(bool caseSensitive)
        {
            ReloadKeyMap();

            StringComparer stringComparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

            KeywordsToTokens = new Dictionary<string, int>(stringComparer);

            KeywordsForIntellisenseSet = new HashSet<string>(stringComparer);

            KeywordsTreatedAsFunctions = new HashSet<string>(stringComparer);

            KeywordKinds = new Dictionary<string, KeywordKind>(stringComparer);
        }

        /// <summary>
        /// Возвращает токен, соответствующий идентификатору
        /// </summary>
        protected abstract int GetIdToken();

        /// <summary>
        /// Возвращает токен соответствующий ключевому слову, либо токен идентификатора, если такого ключевого слова нет
        /// </summary>
        public int KeywordOrIDToken(string keyword)
        {
            if (KeywordsToTokens.TryGetValue(keyword, out int token))
                return token;
            else
                return GetIdToken();
        }

        public void CreateNewKeyword(string name, Enum token = null, KeywordKind kind = KeywordKind.None, 
            bool isTypeKeyword = false, bool excludedInIntellisense = false, bool treatAsFunction = false)
        {
            name = ConvertKeyword(name);

            if (kind != KeywordKind.None)
                KeywordKinds[name] = kind;

            if (!excludedInIntellisense)
            {
                KeywordsForIntellisenseList.Add(name);
                KeywordsForIntellisenseSet.Add(name);
            }

            if (treatAsFunction)
                KeywordsTreatedAsFunctions.Add(name);

            // token null может передаваться, если это ключевое слово исключительно для Intellisense (например, break)
            if (token != null)
                KeywordsToTokens[name] = (int)(object)token;

            if (isTypeKeyword)
                TypeKeywords.Add(name);
        }
    }
}
