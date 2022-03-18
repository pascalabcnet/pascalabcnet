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
    public enum SymKind { var, field, param, procname, funcname, classname, recordname, interfacename };

    [Flags]
    public enum Attributes { class_attr = 1, varparam_attr = 2};

    public class SymInfoSyntax
    {
        public override string ToString()
        {
            string typepart = "";
            if (SK == SymKind.var || SK == SymKind.field || SK == SymKind.param)
                typepart = ": " + (Td == null ? "NOTYPE" : Td.ToString());
            typepart = typepart.Replace("PascalABCCompiler.SyntaxTree.", "");
            var attrstr = Attr != 0 ? "[" + Attr.ToString() + "]" : "";
            var s = "(" + Id.ToString() + "{" + SK.ToString() + "}" + typepart + attrstr + ")";
            return s;
        }
        public ident Id { get; set; }
        public type_definition Td { get; set; }
        public SymKind SK { get; set; }
        public Attributes Attr { get; set; }
        public SymInfoSyntax(ident Id, SymKind SK, type_definition Td = null, Attributes Attr = 0)
        {
            this.Id = Id;
            this.Td = Td;
            this.SK = SK;
            this.Attr = Attr;
        }
        public void AddAttribute(Attributes attr)
        {
            Attr &= attr;
        }
    }

    public class ScopeSyntax
    {
        public ScopeSyntax Parent { get; set; }
        public List<ScopeSyntax> Children = new List<ScopeSyntax>();
        public List<SymInfoSyntax> Symbols = new List<SymInfoSyntax>();
        public override string ToString() => GetType().Name.Replace("Syntax", "");
    }
    public class ScopeWithDefsSyntax : ScopeSyntax { } // 
    public class GlobalScopeSyntax : ScopeWithDefsSyntax { } // program_module unit_module

    public class NamedScopeSyntax : ScopeWithDefsSyntax
    {
        public ident Name { get; set; }
        public NamedScopeSyntax(ident Name) => this.Name = Name;
        public override string ToString() => base.ToString() + "(" + Name + ")";
    }

    public class ProcScopeSyntax : NamedScopeSyntax // procedure_definition
    {
        public ProcScopeSyntax(ident Name) : base(Name) { }
    }
    public class ParamsScopeSyntax : ScopeSyntax { } // formal_parameters
    public class ClassScopeSyntax : NamedScopeSyntax
    {
        public ClassScopeSyntax(ident Name) : base(Name) { }
    } // 
    public class RecordScopeSyntax : NamedScopeSyntax
    {
        public RecordScopeSyntax(ident Name) : base(Name) { }
    } // 
    public class InterfaceScopeSyntax : NamedScopeSyntax
    {
        public InterfaceScopeSyntax(ident Name) : base(Name) { }
    } 
    public class LightScopeSyntax : ScopeSyntax // предок всех легковесных
    {
        public override string ToString()
        {
            var name = base.ToString();
            var level = 0;
            ScopeSyntax t = this;
            while (t.Parent is LightScopeSyntax)
            {
                level++;
                t = t.Parent;
            }
            return name+ "("+ level + ")";
        }
    }
    public class StatListScopeSyntax : LightScopeSyntax { } // statement_list
    public class RepeatScopeSyntax : LightScopeSyntax { } // statement_list
    public class CaseScopeSyntax : LightScopeSyntax { } // statement_list
    public class ForScopeSyntax : LightScopeSyntax { } // statement_list
    public class ForeachScopeSyntax : LightScopeSyntax { } // statement_list
    public class LambdaScopeSyntax : ScopeSyntax { }
}

