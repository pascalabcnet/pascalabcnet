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
        public override string ToString() => dictCaseInsensitive.SkipWhile(x => x.Key != "").Skip(1).JoinIntoString(Environment.NewLine);

        private Dictionary<string, HashTableNode> dictCaseSensitive;

        public Dictionary<string, HashTableNode> DictCaseSensitive
        {
            get
            {
                if (dictCaseSensitive.Count == 0)
                {
                    dictCaseSensitive = new Dictionary<string, HashTableNode>(dictCaseInsensitive);
                }

                return dictCaseSensitive;
            }
        }

        private Dictionary<string, HashTableNode> dictCaseInsensitive;

        public SymbolsDictionary()
        {
            dictCaseSensitive = new Dictionary<string, HashTableNode>();

            dictCaseInsensitive = new Dictionary<string, HashTableNode>(StringComparer.OrdinalIgnoreCase);
        }

        public SymbolsDictionary(int start_size)
        {
            dictCaseInsensitive = new Dictionary<string, HashTableNode>(start_size, StringComparer.OrdinalIgnoreCase);
        }

        public void ClearTable()
        {
            dictCaseSensitive.Clear();

            dictCaseInsensitive.Clear();
        }

        public HashTableNode Add(string name, bool toCaseSensitive, PascalABCCompiler.TreeConverter.SymbolInfo info)
        {
            HashTableNode node;

            if (toCaseSensitive)
            {
                bool exists = dictCaseSensitive.TryGetValue(name, out node);

                if (!exists)
                {
                    node = new HashTableNode(name);

                    dictCaseSensitive[name] = node;

                    bool existsInInsensitive = dictCaseInsensitive.TryGetValue(name, out var node2);

                    if (existsInInsensitive)
                    {
                        node2.InfoList.Add(info);
                    }
                    else
                    {
                        var nodeCopy = new HashTableNode(name);

                        nodeCopy.InfoList.Add(info);

                        dictCaseInsensitive[name] = nodeCopy;
                    }
                }
            }
            else
            {
                bool exists = dictCaseInsensitive.TryGetValue(name, out node);

                if (!exists)
                {
                    node = new HashTableNode(name);

                    dictCaseInsensitive[name] = node;
                }
            }

            node.InfoList.Add(info);

            return node;
        }

        public HashTableNode Find(string name, bool inCaseSensitive)
        {
            HashTableNode node;
            
            if (inCaseSensitive)
            {

                DictCaseSensitive.TryGetValue(name, out node);
            }
            else
            {
                dictCaseInsensitive.TryGetValue(name, out node);
            }
            
            return node;
        }
    }
}
