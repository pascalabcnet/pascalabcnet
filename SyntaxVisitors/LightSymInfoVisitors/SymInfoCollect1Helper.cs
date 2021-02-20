﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
    GlobalScopeSyntax
        BlockScopeSyntax
            ProcScopeSyntax (name)
                ParamsScopeSyntax
                    BlockScopeSyntax
                        StatListScopeSyntax (0)
                            StatListScopeSyntax (1)
            StatListScopeSyntax (0)
                StatListScopeSyntax (1)
*/


namespace PascalABCCompiler.SyntaxTree
{
    public partial class CollectLightSymInfoVisitor
    {
        public virtual void AddSymbol(ident name, SymKind kind, type_definition td = null, Attributes attr = 0)
        {
            Current.Symbols.Add(new SymInfoSyntax(name, kind, td, attr));
        }
        public void AddSymbols(List<ident> names, SymKind kind, type_definition td = null, Attributes attr = 0)
        {
            foreach (var n in names)
            {
                AddSymbol(n, kind, td, attr);
            }
        }
        public string Spaces(int n) => new string(' ', n);
        public void OutputString(string s) => System.IO.File.AppendAllText(fname, s);
        public void OutputlnString(string s = "") => System.IO.File.AppendAllText(fname, s + '\n');
        public void Output(string fname)
        {
#if DEBUG
            this.fname = fname;
            if (System.IO.File.Exists(fname))
                System.IO.File.Delete(fname);
            OutputElement(0, Root);
#endif
        }
        string fname;
        public void OutputElement(int d, ScopeSyntax s)
        {
            OutputString(Spaces(d));
            if (s == null)
                throw new Exception("ggggggggg");
            if (s is ParamsScopeSyntax)
            {
                OutputString(s.ToString()+": ");
                if (s.Symbols.Count > 0)
                    OutputlnString(string.Join(", ", s.Symbols.Select(x => x.ToString())));
            }
            else
            {
                OutputlnString(s.ToString());
                if (s.Symbols.Count > 0)
                    OutputlnString(Spaces(d + 2) + string.Join(", ", s.Symbols.Select(x => x.ToString())));
            }
            foreach (var sc in s.Children)
                OutputElement(d + 2, sc);
        }

    }
}

