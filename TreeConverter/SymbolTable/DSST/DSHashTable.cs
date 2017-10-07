// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Linq;
using System.Collections.Generic;

namespace SymbolTable
{

    /// <summary>
    /// Динамическая хеш таблица строк
    /// </summary>
    public class DSHashTable
    {
        public override string ToString() => dict.SkipWhile(x=>x.Key != "").Skip(1).JoinIntoString(Environment.NewLine);

        /*private int count;
        private int hash_size
        {
            get
            {
                return hash_arr.Length;
            }
        }

        private HashTableNode[] hash_arr;

        private int HashFunc(string s)
        {
            int n = 0;
            for (int i = 0; i < s.Length; i++)
                n = 127 * n + s[i] * 7;//137
            return Math.Abs(n % hash_size);
        }

        private System.Collections.Generic.Dictionary<int, object> ht = new System.Collections.Generic.Dictionary<int, object>();*/

        private Dictionary<string,HashTableNode> dict = new Dictionary<string, HashTableNode>();

        /*private int GetHash(string s)
        {
            int hash = HashFunc(s);
            int i = 1;
            ht.Clear();
            while (hash_arr[hash] != null)
            {
                if (hash_arr[hash].Name == s) return hash;
                i = i * 11;
                hash = Math.Abs((hash + i) % hash_size);
                if (ht.ContainsKey(hash))
                    Resize(hash_size * 2);
                else
                    ht[hash] = ht;
            }
            return hash;
        }

        private void Resize(int new_size)
        {
            HashTableNode[] ha = new HashTableNode[new_size];
            for (int i = 0; i < new_size; i++) ha[i] = null;
            count = 0;
            HashTableNode[] hat = hash_arr;
            hash_arr = ha;
            if (hat.Length > new_size) return;
            for (int i = 0; i < hat.Length; i++)
                if (hat[i] != null) Add(hat[i]);
        }*/


        public DSHashTable(int start_size)
        {
            //hash_arr = new HashTableNode[start_size];
            //ClearTable();
        }

        public void ClearTable()
        {
            dict.Clear();
            //count = 0;
            //for (int i = 0; i < hash_size; i++)
            //    hash_arr[i] = null;
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

            /*if (!dict.ContainsKey(name))
            {
                var node = new HashTableNode(name);
                dict[name] = node;
                return node;
            }
            return dict[name];*/

            /*if (count / hash_size * 100 > SymbolTableConstants.HashTable_StartResise)
                Resize(hash_size + hash_size / 100 * SymbolTableConstants.HashTable_ProcResize);
            int tn = GetHash(node.Name);
            if (hash_arr[tn] == null)
            {
                hash_arr[tn] = node;
                count++;
            }
            return hash_arr[tn];*/
        }

        public HashTableNode Find(string name)
        {
            HashTableNode node = null;
            dict.TryGetValue(name, out node);
            return node;
            /*if (dict.ContainsKey(name))
                return dict[name];
            else return null;*/

            /*int h = GetHash(name);
            //if (hash_arr[h] != null)
            //    return hash_arr[h];
            return hash_arr[h];*/
        }


    }
}
