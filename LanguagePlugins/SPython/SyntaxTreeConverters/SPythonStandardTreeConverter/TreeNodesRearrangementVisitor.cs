using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;

namespace Languages.SPython.Frontend.Converters
{
    internal class TreeNodesRearrangementVisitor : BaseChangeVisitor
    {
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
            if (stn is program_module pm)
            {
                decls = pm.program_block.defs;
            }
            if (stn is interface_node intn)
            {
                decls = intn.interface_definitions;
            }

            base.Enter(stn);
        }

        private void BuildMainFunction(statement_list _statement_list)
        {
            statement_list mainBody = _statement_list.TypedClone();
            procedure_header _procedure_header = new procedure_header(mainFunctionName);
            block _block = new block(null, mainBody, mainBody.source_context);
            //block _block = new block(null, new statement_list(), _statement_list.source_context);
            mainFunction = new procedure_definition(_procedure_header, _block, false, _block.source_context);
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
                    decls.list.Add(mainFunction);
                    foreach (declaration decl in suffix)
                        decls.list.Add(decl);
                    sl.list.Clear();
                    sl.Add(new procedure_call(new method_call(new ident(mainFunctionName), null), true));
                }
            }

            base.Exit(stn);
        }

        public override void visit(declarations_as_statement _declarations_as_statement)
        {
            foreach (declaration decl in _declarations_as_statement.defs.defs)
            {
                suffix.Add(decl);
            }
            ReplaceStatement(_declarations_as_statement, new empty_statement());
        }
    }
}
