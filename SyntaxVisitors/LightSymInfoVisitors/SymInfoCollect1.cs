using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
    ProgramScopeSyntax
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
    public enum SymKind { var, field, param };
    public class SymInfoSyntax
    {
        public override string ToString() => "("+Id.ToString()+"{"+SK.ToString()+"}"+": "+ Td.ToString()+")";
        public ident Id { get; set; }
        public type_definition Td { get; set; }
        public SymKind SK { get; set; }
        public SymInfoSyntax(ident Id, SymKind SK, type_definition Td = null)
        {
            this.Id = Id;
            this.Td = Td;
            this.SK = SK;
        }
    }

    public class ScopeSyntax
    {
        public ScopeSyntax Parent { get; set; }
        public List<ScopeSyntax> Children = new List<ScopeSyntax>();
        public List<SymInfoSyntax> Symbols = new List<SymInfoSyntax>();
        public override string ToString()
        {
            var s = GetType().Name;
            return s.Replace("Syntax", "");
        }
    }
    public class ScopeWithDefsSyntax : ScopeSyntax { } // 
    public class ProgramScopeSyntax : ScopeWithDefsSyntax { } // program_module
    public class ProcScopeSyntax : ScopeWithDefsSyntax // procedure_definition
    {
        public string Name { get; set; }
        public ProcScopeSyntax(string Name)
        {
            this.Name = Name;
        }
    } 
    public class ParamsScopeSyntax : ScopeSyntax { } // formal_parameters
    public class ClassScopeSyntax : ScopeWithDefsSyntax { } // 
    public class StatListScopeSyntax : ScopeSyntax { } // statement_list

    public class CollectLightSymInfoVisitor : BaseEnterExitVisitor
    {
        public ScopeSyntax Root;
        public ScopeSyntax Current;

        public static CollectLightSymInfoVisitor New => new CollectLightSymInfoVisitor();
        public override void Enter(syntax_tree_node st)
        {
            ScopeSyntax t = null;
            switch (st)
            {
                case program_module p:
                    t = new ProgramScopeSyntax();
                    Root = t;
                    break;
                case procedure_definition p:
                    t = new ProcScopeSyntax(p.proc_header.name.ToString());
                    break;
                case formal_parameters p:
                    t = new ParamsScopeSyntax();
                    break;
                case statement_list p:
                    t = new StatListScopeSyntax();
                    break;
            }
            if (t != null)
            {
                t.Parent = Current;
                if (Current != null)
                    Current.Children.Add(t);
                Current = t;
            }
        }
        public override void Exit(syntax_tree_node st)
        {
            switch (st)
            {
                case program_module p:
                case procedure_definition pd:
                case formal_parameters fp:
                case statement_list stl:
                    Current = Current.Parent;
                    break;
            }
        }
        public override void visit(var_def_statement vd)
        {
            if (vd == null || vd.vars == null || vd.vars.list == null)
                return;
            var type = vd.vars_type;
            Current.Symbols.AddRange(vd.vars.list.Select(x=>new SymInfoSyntax(x,SymKind.var,type)));
        }

        public override string ToString() => Root.ToString();

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
            OutputlnString(s.ToString());
            if (s.Symbols.Count > 0)
                OutputlnString(Spaces(d+2)+"Symbols: "+ string.Join(", ", s.Symbols.Select(x => x.ToString())));
            foreach (var sc in s.Children)
                OutputElement(d + 2, sc);
        }

    }
}

