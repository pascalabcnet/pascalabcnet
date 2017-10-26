using System;
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
    public partial class CollectLightSymInfoVisitor : BaseEnterExitVisitor
    {
        public void AddSymbol(ident name, SymKind kind, type_definition td = null)
        {
            Current.Symbols.Add(new SymInfoSyntax(name, kind, td));
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

