// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace SymbolTable
{

    /// <summary>
    /// Динамическая хеш таблица строк
    /// </summary>
    public class SymbolsDictionary
    {
        public override string ToString() => namesToInfos.SkipWhile(x => x.Key != "").Skip(1).JoinIntoString(Environment.NewLine);

        private readonly Dictionary<string, HashTableNode> namesToInfos;

        // Заполняется как регистронезависимый словарь для случая, когда основной словарь регистрозависим
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<string, HashTableNode> namesToInfosHelper;

        private readonly bool caseSensitive;

        /// <summary>
        /// Инициализация таблицы символов с учетом регистрозависимости
        /// </summary>
        public SymbolsDictionary(bool caseSensitive)
        {
            var stringComparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

            namesToInfos = new Dictionary<string, HashTableNode>(stringComparer);

            if (caseSensitive)
                namesToInfosHelper = new Dictionary<string, HashTableNode>(StringComparer.OrdinalIgnoreCase);

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
            var node = AddToSymbolsDict(namesToInfos, name, info);

            if (caseSensitive)
                AddToSymbolsDict(namesToInfosHelper, name, info);

            return node;
        }

        /// <summary>
        /// Добавить информацию info о символе с именем name в словарь dict
        /// </summary>
        private HashTableNode AddToSymbolsDict(Dictionary<string, HashTableNode> dict, string name, PascalABCCompiler.TreeConverter.SymbolInfo info)
        {
            bool exists = dict.TryGetValue(name, out var node);

            if (!exists)
            {
                node = new HashTableNode(name);

                dict[name] = node;
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
            HashTableNode node;

            // Если ищем регистрозависимо в регистронезависимом
            if (caseSensitiveSearch && !caseSensitive)
            {
                namesToInfos.TryGetValue(name, out node);

                // Если есть точные совпадения, то надо взять только их
                if (node != null && node.InfoList.Find(info => info.Name == name) != null)
                {
                    var nodeToReturn = new HashTableNode(name);

                    nodeToReturn.InfoList.AddRange(node.InfoList.Where(info => info.Name == name));

                    return nodeToReturn;
                }
            }
            // Если ищем регистронезависимо в регистрозависимом, то пользуемся вспомогательным словарем
            else if (!caseSensitiveSearch && caseSensitive)
            {
                namesToInfosHelper.TryGetValue(name, out node);
            }
            // В остальных случаях пользуемся основным словарем
            else
            {
                namesToInfos.TryGetValue(name, out node);
            }

            return node;
        }

        /// <summary>
        /// Получить информацию обо всех сохраненных символах
        /// </summary>
        public HashTableNode[] GetAllSymbolInfos() => namesToInfos.Values.ToArray();
    }
}
