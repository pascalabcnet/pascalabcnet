using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;

namespace AssignTupleOptimizer
{
    interface ILightScopeCreator
    {
        ScopeSyntax CreateScope(syntax_tree_node node);
        ScopeSyntax CreateScope(program_module node);
        ScopeSyntax CreateScope(procedure_definition node);
        ScopeSyntax CreateScope(statement_list node);
        ScopeSyntax CreateScope(for_node node);
        ScopeSyntax CreateScope(foreach_stmt node);
        ScopeSyntax CreateScope(class_definition node);
        ScopeSyntax CreateScope(record_type node);
        ScopeSyntax CreateScope(function_lambda_definition node);
        ScopeSyntax CreateScope(case_node node);
    }


    abstract class AbstractLightScopeCreator : ILightScopeCreator
    {
        public static bool IsScopeCreator(syntax_tree_node st)
        {

            if (st is program_module || st is procedure_definition || st is statement_list ||
                st is for_node || st is foreach_stmt || st is class_definition || st is record_type ||
                st is function_lambda_definition || st is case_node) return true;

            return false;
        }

        public virtual ScopeSyntax CreateScope(syntax_tree_node st)
        {
            if (st is program_module p) return CreateScope(p);
            else if (st is procedure_definition pd) return CreateScope(pd);
            else if (st is statement_list sl) return CreateScope(sl);
            else if (st is for_node f) return CreateScope(f);
            else if (st is foreach_stmt fe) return CreateScope(fe);
            else if (st is class_definition cd) return CreateScope(cd);
            else if (st is record_type rt) return CreateScope(rt);
            else if (st is function_lambda_definition fld) return CreateScope(fld);
            else if (st is case_node cas) return CreateScope(cas);
            else return null;
        }

        public abstract ScopeSyntax CreateScope(program_module node);
        public abstract ScopeSyntax CreateScope(procedure_definition node);
        public abstract ScopeSyntax CreateScope(statement_list node);
        public abstract ScopeSyntax CreateScope(for_node node);
        public abstract ScopeSyntax CreateScope(foreach_stmt node);
        public abstract ScopeSyntax CreateScope(class_definition node);
        public abstract ScopeSyntax CreateScope(record_type node);
        public abstract ScopeSyntax CreateScope(function_lambda_definition node);
        public abstract ScopeSyntax CreateScope(case_node node);
    }

    class LightScopeCreator : AbstractLightScopeCreator
    {
        public override ScopeSyntax CreateScope(program_module node) => new GlobalScopeSyntax();


        public override ScopeSyntax CreateScope(procedure_definition node) 
        {
            var name = node.proc_header?.name?.meth_name;
            if (name == null)
                name = "create";
           var attr = node.proc_header.class_keyword ? Attributes.class_attr : 0;
           return new ProcScopeSyntax(name);
        }

        public override ScopeSyntax CreateScope(statement_list node) => new StatListScopeSyntax();

        public override ScopeSyntax CreateScope(for_node node) => new ForScopeSyntax();

        public override ScopeSyntax CreateScope(foreach_stmt node) => new ForeachScopeSyntax();

        public override ScopeSyntax CreateScope(class_definition node)
        {
            var td = node.Parent as type_declaration;
            var tname = td == null ? "NONAME" : td.type_name;
            if (node.keyword == class_keyword.Class)
            {
                //AddSymbol(tname, SymKind.classname);
                return new ClassScopeSyntax(tname);
            }
            else if (node.keyword == class_keyword.Record)
            {
                //AddSymbol(tname, SymKind.recordname);
                return new RecordScopeSyntax(tname);
            }
            else if (node.keyword == class_keyword.Interface)
            {
                //AddSymbol(tname, SymKind.interfacename);
                return new InterfaceScopeSyntax(tname);
            }
            else return null;
        }

        public override ScopeSyntax CreateScope(record_type node)
        {
            var td = node.Parent as type_declaration;
            var tname = td == null ? "NONAME" : td.type_name;
            return new RecordScopeSyntax(tname);
        }

        public override ScopeSyntax CreateScope(function_lambda_definition node) => new LambdaScopeSyntax();

        public override ScopeSyntax CreateScope(case_node node) => new CaseScopeSyntax();
    }

}
