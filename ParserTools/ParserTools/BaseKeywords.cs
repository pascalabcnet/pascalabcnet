// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PascalABCCompiler.Contracts.Parsers;

namespace PascalABCCompiler.Contracts
{
    public abstract class BaseKeywords : IKeywords
    {
        public IDictionary<string, KeywordKind> KeywordKinds { get; }

        public ISet<string> KeywordsForIntellisenseSet { get; }

        public IList<string> KeywordsForIntellisenseList { get; } = new List<string>();

        public IList<string> TypeKeywords { get; } = new List<string>();

        public ISet<string> KeywordsTreatedAsFunctions { get; }

        /// <summary>
        /// Словарь соответствий ключевых слов их эквивалентам (задается пользователем в специальном файле)
        /// </summary>
        private Dictionary<string, string> keymap = new Dictionary<string, string>();

        /// <summary>
        /// Название файла с информацией об эквивалентах ключевых слов
        /// </summary>
        protected abstract string KeywordsMapFileName { get; }

        /// <summary>
        /// Соотвествие ключевых слов токенам (ключевые слова для парсера)
        /// </summary>
        private readonly Dictionary<string, int> keywordsToTokens;

        /// <summary>
        /// Функция загрузки эквивалентов ключевых слов из файла
        /// </summary>
        private void ReloadKeyMap()
        {
            try
            {
                var pathToKeymapFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName), KeywordsMapFileName);
                
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
        private string ConvertKeyword(string keyword)
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

            keywordsToTokens = new Dictionary<string, int>(stringComparer);

            KeywordsForIntellisenseSet = new HashSet<string>(stringComparer);

            KeywordsTreatedAsFunctions = new HashSet<string>(stringComparer);

            KeywordKinds = new Dictionary<string, KeywordKind>(stringComparer);
        }

        /// <summary>
        /// Возвращает токен, соответствующий идентификатору
        /// </summary>
        protected abstract int GetIdToken();

        public int KeywordOrIDToken(string keyword)
        {
            if (keywordsToTokens.TryGetValue(keyword, out int token))
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
                keywordsToTokens[name] = (int)(object)token;

            if (isTypeKeyword)
                TypeKeywords.Add(name);
        }
    }
}
