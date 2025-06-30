using System;
using System.Collections.Generic;
using System.Linq;

namespace PascalABCCompiler.SyntaxTree
{
    public abstract class CollectLightSymInfoVisitor : BaseEnterExitVisitor
    {
        public GlobalScopeSyntax Root;
        public ScopeSyntax Current;
        protected Dictionary<string, NamedScopeSyntax> classes = new Dictionary<string, NamedScopeSyntax>();
  

        protected bool inPrivate = false; 

        public CollectLightSymInfoVisitor(compilation_unit root)
        {
            Root = scopeCreator.GetScope(root) as GlobalScopeSyntax;
        }

        abstract protected AbstractScopeCreator scopeCreator
        { get; }


        public ScopeSyntax GetScope(syntax_tree_node node) => scopeCreator.GetScope(node);

        public abstract void PreExitScope(syntax_tree_node st);

        public override void visit(var_def_statement vd)
        {
            var attr = vd.var_attr == definition_attribute.Static ? SymbolAttributes.class_attr : 0;
            if (vd == null || vd.vars == null || vd.vars.list == null)
                return;
            AddSymbols(vd.vars.list, SymKind.var, vd.vars_type, attr);
            base.visit(vd);
        }

        public override void visit(assign_var_tuple node)
        {
            AddSymbols(node.idents.idents, SymKind.var);
            base.visit(node);
        }

        public override void visit(var_tuple_def_statement node)
        {
            var attr = node.var_attr == definition_attribute.Static ? SymbolAttributes.class_attr : 0;
            AddSymbols(node.vars.list, SymKind.var, node.vars_type, attr);
            base.visit(node);
        }

        public override void visit(formal_parameters fp)
        {
            foreach (var pg in fp.params_list)
            {
                 if (pg.param_kind == parametr_kind.var_parametr)                 
                    AddSymbols(pg.idents.idents, SymKind.param, null, SymbolAttributes.varparam_attr);             
                 else 
                    AddSymbols(pg.idents.idents, SymKind.param);
            }
            base.visit(fp);
        }

        public override void visit (simple_const_definition node)
        {
            AddSymbol(node.const_name, SymKind.const_var);
        }

        public override void visit(type_declaration td)
        {
            if (td.type_def is class_definition cd)
            {
                var tname = td.type_name;
                if (cd.keyword == class_keyword.Class || cd.keyword == class_keyword.TemplateClass)
                    AddSymbol(tname, SymKind.classname);
                else if (cd.keyword == class_keyword.Record || cd.keyword == class_keyword.TemplateRecord)
                    AddSymbol(tname, SymKind.recordname);
                else if (cd.keyword == class_keyword.Interface || cd.keyword == class_keyword.TemplateInterface)
                    AddSymbol(tname, SymKind.interfacename);                

                if (!classes.ContainsKey(td.type_name.name))
                {
                    classes.Add(td.type_name.name, scopeCreator.GetScope(td.type_def) as NamedScopeSyntax);
                    //Console.WriteLine("Added" + td.type_name.name + " " + (scopeCreator.GetScope(td.type_def) as ClassScopeSyntax));
                }
            }
            else if (td.type_def is named_type_reference)
            {
               AddSymbol(td.type_name, SymKind.type_alias);
            }
            base.visit(td);
        }

        public override void visit(class_members cm)
        {
            if (cm.access_mod.access_level == access_modifer.private_modifer)
            {
                inPrivate = true;
            }
            base.visit(cm);
            inPrivate = false;
        }

        public override void visit(for_node f)
        {
            if (f.create_loop_variable || f.type_name != null)
                AddSymbol(f.loop_variable, SymKind.var, f.type_name);
            base.visit(f);
        }



        public override void visit(procedure_definition node)
        {
            var name = node.proc_header?.name?.meth_name;
            var attr = node.proc_header.class_keyword ? SymbolAttributes.class_attr : 0;

            if (name != null)
                if (node.proc_header?.name?.class_name != null)
                {
                    var class_name = node.proc_header.name.class_name.name;
                    if (classes.ContainsKey(class_name))
                    {
                        (Current as ProcScopeSyntax).classParent = classes[class_name];
                        (Current as ProcScopeSyntax).classParent.AddSymbol(name, SymKind.funcname, null, attr);
                    }
                }
            
            
            base.visit(node);
        }

        public override void visit(class_definition cd)
        {
            var classScope = Current as NamedScopeSyntax;
            if (cd.class_parents != null )
                foreach (var parent in cd.class_parents.types)
                    if (parent.names.Count == 1)
                        if (classes.ContainsKey(parent.FirstIdent.name))
                            classScope.classParent = classes[parent.FirstIdent.name];
            base.visit(cd);
        }

        public override void visit(procedure_header node)
        {
            if ( node.template_args != null)
            {
                foreach (var template_name in node.template_args.idents)
                {
                    AddSymbol(template_name, SymKind.template_param);
                }
            }
            base.visit(node);
         }

        public override void visit(template_type_name node)
        {
            foreach (var name in node.template_args.idents)
            {
                AddSymbol(name, SymKind.template_param);
            }
            base.visit(node);
        }

        public override void visit(property_ident node)
        {
            AddSymbol(node, SymKind.property);
        }


        public virtual void AddSymbol(ident name, SymKind kind, type_definition td = null, SymbolAttributes attr = 0)
        {
            if (inPrivate)
                attr &= SymbolAttributes.private_attr;
            Current.AddSymbol(name, kind, td, attr);
        }
        public void AddSymbols(List<ident> names, SymKind kind, type_definition td = null, SymbolAttributes attr = 0)
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
                OutputString(s.ToString() + ": ");
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
