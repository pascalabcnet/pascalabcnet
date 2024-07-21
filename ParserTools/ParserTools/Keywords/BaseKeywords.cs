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
        /// Соотвествие ключевых слов токенам
        /// </summary>
        protected abstract Dictionary<string, int> KeywordsToTokens { get; set; }

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
            catch (Exception e)
            {
                //var w = e.Message;
            }
        }

        /// <summary>
        /// Возвращает само эквивалент ключевого слова, либо его само, если эквивалента нет
        /// </summary>
        public string ConvertKeyword(string keyword)
        {
            if (keymap.Count() == 0 || !keymap.ContainsKey(keyword))
                return keyword;
            else 
                return keymap[keyword];
        }

        public BaseKeywords()
        {
            ReloadKeyMap();
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
    }
}
