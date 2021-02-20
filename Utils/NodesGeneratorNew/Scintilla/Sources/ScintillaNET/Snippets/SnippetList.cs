#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

#endregion Using Directives


namespace ScintillaNET
{
    public class SnippetList : KeyedCollection<string,Snippet>
    {
        #region Fields

        SnippetManager _manager;

        #endregion Fields


        #region Methods

        public Snippet Add(string shortcut, string code)
        {
            return Add(shortcut, code, _manager.DefaultDelimeter);
        }


        public Snippet Add(string shortcut, string code, bool isSurroundsWith)
        {
            return Add(shortcut, code, _manager.DefaultDelimeter, isSurroundsWith);
        }


        public Snippet Add(string shortcut, string code, char delimeter)
        {
            return Add(shortcut, code, delimeter, false);
        }


        public Snippet Add(string shortcut, string code, char delimeter, bool isSurroundsWith)
        {
            Snippet s = new Snippet(shortcut, code, delimeter, isSurroundsWith);
            Add(s);
            return s;
        }


        public void AddRange(IEnumerable<Snippet> snippets)
        {
            foreach (Snippet s in snippets)
                Add(s);
        }


        protected override string GetKeyForItem(Snippet item)
        {
            return item.Shortcut;
        }


        public void Sort()
        {
            
            Snippet[] a = new Snippet[Count];
            CopyTo(a, 0);
            Array.Sort<Snippet>(a);

            Clear();
            AddRange(a);
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Snippet s in this.Items)
                sb.Append(s.Shortcut).Append(" ");

            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }


        public bool TryGetValue(string key, out Snippet snippet)
        {
            if(this.Contains(key))
            {
                snippet = this[key];
                return true;
            }
            else
            {
                snippet = null;
                return false;
            }
        }

        #endregion Methods


        #region Constructors

        internal SnippetList(SnippetManager manager)
        {
            if (_manager != null)
            {
                _manager = manager;
            }
        }

        #endregion Constructors
    }
}