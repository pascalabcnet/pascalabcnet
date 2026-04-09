// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;
using System.Collections.Generic;
using PascalABCCompiler.TreeConverter;

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
        public void Add(string name, SymbolInfo info)
        {
            bool exists = namesToInfos.TryGetValue(name, out var node);

            if (!exists)
            {
                node = new HashTableNode();

                namesToInfos[name] = node;
            }

            node.InfoList.Add(info);
        }

        /// <summary>
        /// Найти информацию о символе с именем name.
        /// caseSensitiveSearch определяет регистрозависимость поиска
        /// </summary>
        public IEnumerable<SymbolInfo> Find(string name, bool caseSensitiveSearch)
        {
            // Если ищем регистрозависимо
            if (caseSensitiveSearch)
            {
                if (namesToInfos.TryGetValue(name, out var node))
                {
                    // Если есть точные совпадения, то надо взять только их
                    var infos = node.InfoList.Where(info => info.Name == name);

                    if (infos.Any())
                        return infos;
                }
            }
            // Если ищем регистронезависимо
            else
            {
                if (namesToInfos.TryGetValue(name, out var node))
                    return node.InfoList;
            }

            return null;
        }

        /// <summary>
        /// Получить информацию обо всех сохраненных символах
        /// </summary>
        public IEnumerable<HashTableNode> GetAllSymbolInfos() => namesToInfos.Values;
    }
}
