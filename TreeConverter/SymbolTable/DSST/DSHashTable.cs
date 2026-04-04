// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;
using System.Collections.Generic;

namespace SymbolTable
{

    /// <summary>
    /// Динамическая хеш таблица строк
    /// </summary>
    public class SymbolsDictionary
    {
        public override string ToString() => namesToInfos.SkipWhile(x => x.Key != "").Skip(1).JoinIntoString(Environment.NewLine);

        private readonly Dictionary<string, HashTableNode> namesToInfos;

        private readonly bool caseSensitive;

        /// <summary>
        /// Инициализация таблицы символов с учетом регистрозависимости
        /// </summary>
        public SymbolsDictionary(bool caseSensitive)
        {
            var stringComparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

            namesToInfos = new Dictionary<string, HashTableNode>(stringComparer);

            this.caseSensitive = caseSensitive;
        }

        //public SymbolsDictionary(int start_size)
        //{
        //    dictCaseInsensitive = new Dictionary<string, HashTableNode>(start_size, StringComparer.OrdinalIgnoreCase);
        //}

        /// <summary>
        /// Очистка сохраненных символов
        /// </summary>
        public void ClearTable()
        {
            namesToInfos.Clear();
        }

        /// <summary>
        /// Добавить информацию info о символе с именем name
        /// </summary>
        public HashTableNode Add(string name, PascalABCCompiler.TreeConverter.SymbolInfo info)
        {
            bool exists = namesToInfos.TryGetValue(name, out var node);

            if (!exists)
            {
                node = new HashTableNode(name);

                namesToInfos[name] = node;
            }

            node.InfoList.Add(info);

            return node;
        }

        /// <summary>
        /// Найти информацию о символе с именем name.
        /// caseSensitiveSearch определяет регистрозависимость поиска
        /// </summary>
        public HashTableNode Find(string name, bool caseSensitiveSearch)
        {
            namesToInfos.TryGetValue(name, out var node);

            // Дополнительная проверка для случая, когда нужен регистрозависимый поиск в регистронезависимом словаре
            if (caseSensitiveSearch && !caseSensitive && node?.Name != name)
                node = null;
            
            return node;
        }

        /// <summary>
        /// Получить информацию обо всех сохраненных символах
        /// </summary>
        public HashTableNode[] GetAllSymbolInfos() => namesToInfos.Values.ToArray();
    }
}
