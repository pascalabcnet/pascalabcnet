using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;

namespace AssignTupleOptimizer
{
    class MyCollectLightSymInfoVisitor : CollectLightSymInfoVisitor
    {

        AbstractLightScopeCreator scopeCreator = new LightScopeCreator();

        public Dictionary<syntax_tree_node, ScopeSyntax> map = new Dictionary<syntax_tree_node, ScopeSyntax>();

        public override void Enter(syntax_tree_node st)
        {
            var new_scope = scopeCreator.CreateScope(st);
            if (new_scope != null)
            {
                map.Add(st, new_scope);
                new_scope.Parent = Current;
                Current = new_scope;
            }

        }
        public virtual void PreExitScope(syntax_tree_node st)
        {

        }
        public override void Exit(syntax_tree_node st)
        {
            if (AbstractLightScopeCreator.IsScopeCreator(st))
            {
                PreExitScope(st);
                if (Current != null)
                    Current = Current.Parent;
            }
        }


        public override void visit(procedure_definition pd)
        {
            /*var name = pd.proc_header?.name?.meth_name;
            if (name == null)
                name = "create";
            var attr = pd.proc_header.class_keyword ? Attributes.class_attr : 0;
            if (name != null)
            {
                if (pd.proc_header is function_header)
                    AddSymbol(name, SymKind.funcname, null, attr);
                else AddSymbol(name, SymKind.procname, null, attr);
            }*/
            base.visit(pd);
        }

        public override void visit(class_definition cd)
        {
            var class_nm = Current as ClassScopeSyntax;
            Current = Current.Parent;

            foreach(var parent in cd.class_parents.types)
            {
                if(parent.names.Count == 1) 
                {
                    var res = Current.bind(parent.names[0]);
                    if(res != null)
                    {
                        class_nm.classParents.Add(map[res.Td] as ClassScopeSyntax);
                    }             
                }
            }

            var td = cd.Parent as type_declaration;
            var tname = td == null ? "NONAME" : td.type_name;
            if (cd.keyword == class_keyword.Class)
            {
                AddSymbol(tname, SymKind.classname, cd);
            }
            else if (cd.keyword == class_keyword.Record)
            {
                AddSymbol(tname, SymKind.recordname, cd);    
            }
            else if (cd.keyword == class_keyword.Interface)
            {
                AddSymbol(tname, SymKind.interfacename, cd);
            }
            Current = class_nm;
            
        }

        public override void visit(var_def_statement vd)
        {
            var attr = vd.var_attr == definition_attribute.Static ? Attributes.class_attr : 0;
            if (vd == null || vd.vars == null || vd.vars.list == null)
                return;
            //AddSymbols(vd.vars.list, SymKind.var, vd.vars_type, attr);
            base.visit(vd);
        }
        public override void visit(formal_parameters fp)
        {
            foreach (var pg in fp.params_list)
            {
                var type = pg.param_kind;
                if (type == parametr_kind.var_parametr) 
                    AddSymbols(pg.idents.idents, SymKind.param, pg.vars_type, Attributes.varparam_attr);
                else
                    AddSymbols(pg.idents.idents, SymKind.param, pg.vars_type);
            }
            base.visit(fp);
        }

        public override void visit(for_node f)
        {
            if (f.create_loop_variable || f.type_name != null)
                AddSymbol(f.loop_variable, SymKind.var, f.type_name);
            base.visit(f);
        }


       /* public SymInfoSyntax bind(ident id, ScopeSyntax ns)
        {
            var cur = ns;
            while (cur != null)
            {
                Console.WriteLine("Search in " + cur.ToString());
              foreach (var symbol in cur.Symbols)
                {
                    if (cur is ClassScopeSyntax c) 
                    {
                        var res = bindInClass(id, bind(c.Name, Root).Td as class_definition);
                        if (res != null) return res;
                    }
                    Console.WriteLine("checking symbol:" + symbol.ToString() + "names?: " + (symbol.Id.name == id.name).ToString()
                         + " source context?: " + id.source_context.Less(symbol.Id.source_context).ToString());
                    else
                    {
                        if (symbol.Id.name == id.name && symbol.Id.source_context.Less(id.source_context))
                        {
                            Console.WriteLine("Found!!");
                            return symbol;
                        } 
                    }
                }
                cur = cur.Parent;
            }
            return null;
        }

        private SymInfoSyntax bindInClass(ident id, class_definition clazz)
        {

            if (map.ContainsKey(clazz))
            {
                var ns = map[clazz];

                foreach (var s in ns.Symbols)
                {
                    if (s.Id.name == id.name) return s;
                }
                
                foreach(var parent in clazz.class_parents.types)
                {
                    SymInfoSyntax res_class = null;
                    foreach(var name in parent.names)
                    {
                        res_class = bind(name, Root);
                        if (res_class != null) break;
                    }
                    SymInfoSyntax res = null;
                    if (res_class != null)
                    {
                        res = bindInClass(id, res_class.Td as class_definition); 
                    }
                    if (res != null) return res;
                }
                return null;
            }
            else return null;

             
        }*/
    }
}
