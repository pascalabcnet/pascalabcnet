using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PascalABCCompiler.SyntaxTree
{

    public abstract class AbstractScopeCreator
    {
        public static bool IsScopeCreator(syntax_tree_node st)
        {

            if (st is compilation_unit || st is procedure_definition || st is statement_list ||
                st is for_node || st is foreach_stmt || st is class_definition || st is record_type ||
                st is function_lambda_definition || st is case_node) return true;

            return false;
        }

        public virtual ScopeSyntax GetScope(syntax_tree_node st)
        {
            ScopeSyntax res = null;
            if (st is compilation_unit p) res = CreateScope(p);
            else if (st is procedure_definition pd) res = CreateScope(pd);
            else if (st is statement_list sl) res = CreateScope(sl);
            else if (st is for_node f) res = CreateScope(f);
            else if (st is foreach_stmt fe) res = CreateScope(fe);
            else if (st is class_definition cd) res = CreateScope(cd);
            else if (st is record_type rt) res = CreateScope(rt);
            else if (st is function_lambda_definition fld) res = CreateScope(fld);
            else if (st is case_node cas) res = CreateScope(cas);
            if (res != null) res.node = st;
            return res;
        }

        
        protected abstract ScopeSyntax CreateScope(compilation_unit node);
        protected abstract ScopeSyntax CreateScope(procedure_definition node);
        protected abstract ScopeSyntax CreateScope(statement_list node);
        protected abstract ScopeSyntax CreateScope(for_node node);
        protected abstract ScopeSyntax CreateScope(foreach_stmt node);
        protected abstract ScopeSyntax CreateScope(class_definition node);
        protected abstract ScopeSyntax CreateScope(record_type node);
        protected abstract ScopeSyntax CreateScope(function_lambda_definition node);
        protected abstract ScopeSyntax CreateScope(case_node node);
    }

    class CachingScopeCreator : AbstractScopeCreator
    {
        public Dictionary<syntax_tree_node, ScopeSyntax> map = new Dictionary<syntax_tree_node, ScopeSyntax>();

        public override ScopeSyntax GetScope(syntax_tree_node st)
        {
            if (st == null) return null;
            if (!map.ContainsKey(st))
            {
                var scope = base.GetScope(st);
                if (scope != null) map.Add(st, scope);
                else return null;
            }
            return map[st];
        }

        protected override ScopeSyntax CreateScope(compilation_unit node) => new GlobalScopeSyntax();
       

        protected override ScopeSyntax CreateScope(procedure_definition node) 
        {

            var name = node.proc_header?.name?.meth_name;
            if (name == null)
                name = "create";
            var res = new ProcScopeSyntax(name);
            if (node.proc_header is function_header fh)
            {
                res.AddSymbol(new ident("Result", node.source_context), SymKind.var, fh.return_type);
            }
            return res; 
        }

        protected override ScopeSyntax CreateScope(statement_list node) => new StatListScopeSyntax();

        protected override ScopeSyntax CreateScope(for_node node) => new ForScopeSyntax();

        protected override ScopeSyntax CreateScope(foreach_stmt node) => new ForeachScopeSyntax();

        protected override ScopeSyntax CreateScope(class_definition cd)
        {
            var td = cd.Parent as type_declaration;
            var tname = td == null ? "NONAME" : td.type_name;
            ScopeSyntax res = null;

            if (cd.keyword == class_keyword.Class)
            {
                res  = new ClassScopeSyntax(tname);
            }
            else if (cd.keyword == class_keyword.Record)
            {
                res = new RecordScopeSyntax(tname);
            }
            else if (cd.keyword == class_keyword.Interface)
            {
                res =  new InterfaceScopeSyntax(tname);
            }
            return res;
        }

        protected override ScopeSyntax CreateScope(record_type node)
        {
            var td = node.Parent as type_declaration;
            var tname = td == null ? "NONAME" : td.type_name;
            return new RecordScopeSyntax(tname);
        }

        protected override ScopeSyntax CreateScope(function_lambda_definition node) => new LambdaScopeSyntax();

        protected override ScopeSyntax CreateScope(case_node node) => new CaseScopeSyntax();
    }



}
