using System.Collections.Generic;
using System.Data;
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class TreeNodesRearrangementVisitor : BaseChangeVisitor
    {
        // declaration до объявления мейна (forward объявления функций и объявления переменных)
        private List<declaration> prefix = new List<declaration>();

        private procedure_definition mainFunction;

        // declaration после объявления мейна (объявления функций с определениями)
        private List<declaration> suffix = new List<declaration>();

        private declarations decls;

        private const string mainFunctionName = "%%MAIN%%";

        private bool isInFunction = false;
        private int scopeCounter = 0;

        public TreeNodesRearrangementVisitor() { }

        // нужны методы из BaseChangeVisitor, но порядок обхода из WalkingVisitorNew
        public override void DefaultVisit(syntax_tree_node n)
        {
            if (n == null) return;
            for (var i = 0; i < n.subnodes_count; i++)
                ProcessNode(n[i]);
        }

        public override void Enter(syntax_tree_node stn)
        {
            if (stn is procedure_definition)
            {
                isInFunction = true;
                scopeCounter++;
            }
            if (stn is statement_list)
            {
                scopeCounter++;
            }

            base.Enter(stn);
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        private void BuildMainFunction(statement_list _statement_list)
        {
            statement_list mainBody = _statement_list.TypedClone();
            procedure_header _procedure_header = new procedure_header(mainFunctionName);
            block _block = new block(null, mainBody, mainBody.source_context);
            //block _block = new block(null, new statement_list(), _statement_list.source_context);
            mainFunction = new procedure_definition(_procedure_header, _block);
        }

        public override void Exit(syntax_tree_node stn)
        {
            if (stn is procedure_definition)
            {
                isInFunction = false;
                scopeCounter--;
            }
            if (stn is statement_list sl)
            {
                scopeCounter--;
                // вышли из внешнего statement_list внутри begin end.
                if (!isInFunction && scopeCounter == 0)
                {
                    BuildMainFunction(sl);
                    decls.list.Clear();
                    foreach (declaration decl in prefix)
                        decls.list.Add(decl);
                    decls.list.Add(mainFunction);
                    foreach (declaration decl in suffix)
                        decls.list.Add(decl);
                    sl.list.Clear();
                    sl.Add(new procedure_call(new method_call(new ident(mainFunctionName), null), true));
                }
            }
            if (stn is declarations ds)
            {
                decls = ds;
            }

            base.Exit(stn);
        }

        public override void visit(var_def_statement _var_def_statement)
        {
            if (scopeCounter == 0)
                prefix.Add(_var_def_statement);
        }

        private bool IsForwardDeclaration(procedure_definition _procedure_definition)
        {
            foreach (procedure_attribute attr in _procedure_definition.proc_header.proc_attributes.proc_attributes)
                if (attr.attribute_type is proc_attribute.attr_forward) 
                    return true;

            return false;
        }

        public override void visit(procedure_definition _procedure_definition)
        {
            if (IsForwardDeclaration(_procedure_definition))
                prefix.Add(_procedure_definition);
            else suffix.Add(_procedure_definition);
        }
    }
}
