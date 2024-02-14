
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
    public enum SymKind { var, field, param, procname, funcname, classname,
        recordname, interfacename, template_param, type_alias, const_var, property };

    [Flags]
    public enum SymbolAttributes { class_attr = 1, varparam_attr = 2, private_attr = 4 };

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
        public SymbolAttributes Attr { get; set; }
        public SymInfoSyntax(ident Id, SymKind SK, type_definition Td = null, SymbolAttributes Attr = 0)
        {
            this.Id = Id;
            this.Td = Td;
            this.SK = SK;
            this.Attr = Attr;
        }
        public void AddAttribute(SymbolAttributes attr)
        {
            Attr |= attr;
        }
    }

    public class BindResult {

        public SymInfoSyntax symInfo;
        public Stack<ScopeSyntax> path;

        public BindResult(SymInfoSyntax s, Stack<ScopeSyntax> p)
        {
            symInfo = s;
            path = p;
        }

    }

    public abstract class ScopeSyntax
    {
        public syntax_tree_node node;

        public ScopeSyntax Parent { get; set; }
        public List<ScopeSyntax> Children = new List<ScopeSyntax>();
        public HashSet<SymInfoSyntax> Symbols = new HashSet<SymInfoSyntax>();
        public override string ToString() => GetType().Name.Replace("Syntax", "");

        protected virtual SymInfoSyntax searchSymbol(ident id)
        {
            foreach (var s in Symbols)
            {
           
                   if (s.Id.name == id.name && (id.source_context == null ||
                        s.Id.source_context == null ||
                        s.Id.source_context.Less(id.source_context)))
                   {
                    return s;
                   }
            }
            return null;
        }

        public void AddSymbol(ident name, SymKind kind, type_definition td = null, SymbolAttributes attr = 0)
        {
            Symbols.Add(new SymInfoSyntax(name, kind, td, attr));
        }
        public virtual SymInfoSyntax bind(ident id)
        {
            return searchSymbol(id);
        }

    }
    public class ScopeWithDefsSyntax : ScopeSyntax { } // 
    public abstract class GlobalScopeSyntax : ScopeWithDefsSyntax { } // program_module unit_module

    public class ProgramScopeSyntax : GlobalScopeSyntax { }

    public class UnitScopeSyntax : GlobalScopeSyntax { }

    public abstract class NamedScopeSyntax : ScopeWithDefsSyntax
    {
        public NamedScopeSyntax classParent;
        public ident Name { get; set; }
        public NamedScopeSyntax(ident Name) => this.Name = Name;
        public override string ToString() => base.ToString() + "(" + Name + ")";

        protected SymInfoSyntax searchInParentClass(ident id)
        {
            if (classParent == null) return null;
            foreach (var symbol in classParent.Symbols)
            {
                if (symbol.Id.name == id.name && !symbol.Attr.HasFlag(SymbolAttributes.private_attr))
                {
                    return symbol;
                }
            }
            return null;
        }

        public override SymInfoSyntax bind(ident id)
        {
            SymInfoSyntax res = searchSymbol(id);
            if (res != null) return res;
            var cur = this;
            while (cur != null)
            {
                res = cur.searchInParentClass(id);
                if (res != null) return res;
                cur = cur.classParent;
            }
            return res;
        }
    }

    public class ProcScopeSyntax : NamedScopeSyntax // procedure_definition
    { 
        
        public ProcScopeSyntax(ident Name) : base(Name) { }

        public override SymInfoSyntax bind(ident id)
        {

            var res = classParent?.bind(id);
            if (res != null) return res;
            return base.bind(id);
        }
    }
    public class ParamsScopeSyntax : ScopeSyntax { } // formal_parameters
    public class ClassScopeSyntax 
        : NamedScopeSyntax
    {
        public ClassScopeSyntax(ident Name) : base(Name) { }

        protected  SymInfoSyntax searchSymbol(ident id)
        {
            //Console.WriteLine("searching " + id.ToString() + " in " + Name);
            foreach (var s in Symbols)
            {
              //  Console.WriteLine("Checking " + s.Id);
                if (s.Id.name == id.name)
                {
                    return s;
                }
            }
            return null;
        }

      
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

