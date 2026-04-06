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

        // Регистронезависимый словарь символов
        private readonly Dictionary<string, HashTableNode> namesToInfos = new Dictionary<string, HashTableNode>(StringComparer.OrdinalIgnoreCase);

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
                node = new HashTableNode();

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
            HashTableNode node;

            // Если ищем регистрозависимо
            if (caseSensitiveSearch)
            {
                namesToInfos.TryGetValue(name, out node);

                // Если есть точные совпадения, то надо взять только их
                if (node != null && node.InfoList.Find(info => info.Name == name) != null)
                {
                    return new HashTableNode(node.InfoList.Where(info => info.Name == name).ToList());
                }
            }
            // Если ищем регистронезависимо
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
