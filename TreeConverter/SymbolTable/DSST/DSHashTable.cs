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
        public override string ToString() => dict.SkipWhile(x => x.Key != "").Skip(1).JoinIntoString(Environment.NewLine);

        public Dictionary<string, HashTableNode> dict;

        public SymbolsDictionary()
        {
            dict = new Dictionary<string, HashTableNode>();
        }

        public SymbolsDictionary(int start_size)
        {
            dict = new Dictionary<string, HashTableNode>(start_size);
        }

        public void ClearTable()
        {
            dict.Clear();
        }

        public HashTableNode Add(string name)
        {
            HashTableNode node = null;
            var b = dict.TryGetValue(name, out node);
            if (!b)
            {
                node = new HashTableNode(name);
                dict[name] = node;
            }
            return node;
        }

        public HashTableNode Find(string name)
        {
            HashTableNode node = null;
            dict.TryGetValue(name, out node);
            return node;
        }
    }
}
