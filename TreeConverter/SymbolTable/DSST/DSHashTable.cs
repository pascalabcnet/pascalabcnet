// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace SymbolTable
{

    /// <summary>
    /// Динамическая хеш таблица строк
    /// </summary>
    public class DSHashTable
    {
        private int count;
        private int hash_size
        {
            get
            {
                return hash_arr.Length;
            }
        }

        public HashTableNode[] hash_arr;

        private int HashFunc(string s)
        {
            int n = 0;
            for (int i = 0; i < s.Length; i++)
                n = 127 * n + s[i] * 7;//137
            return Math.Abs(n % hash_size);
        }

        private System.Collections.Generic.Dictionary<int, object> ht = new System.Collections.Generic.Dictionary<int, object>();

        private int GetHash(string s)
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
        }


        public DSHashTable(int start_size)
        {
            hash_arr = new HashTableNode[start_size];
            ClearTable();
        }

        public void ClearTable()
        {
            count = 0;
            for (int i = 0; i < hash_size; i++)
                hash_arr[i] = null;
        }

        public int Add(HashTableNode node)
        {
            if (count / hash_size * 100 > SymbolTableConstants.HashTable_StartResise)
                Resize(hash_size + hash_size / 100 * SymbolTableConstants.HashTable_ProcResize);
            int tn = GetHash(node.Name);
            if (hash_arr[tn] == null)
            {
                hash_arr[tn] = node;
                count++;
            }
            return tn;
        }

        public int Find(string name)
        {
            int h = GetHash(name);
            if (hash_arr[h] != null)
                return h;
            return -1;
        }


    }
}
