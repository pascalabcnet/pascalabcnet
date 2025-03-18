using Languages.SPython.Frontend.Converters;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Languages.Pascal.Frontend.Converters
{
    public class UnsupportedConstructsVisitor : BaseEnterExitVisitor
    {
        private int statement_list_counter = 0;

        public UnsupportedConstructsVisitor() { }

        public override void Enter(syntax_tree_node st)
        {
            if (st is statement_list)
            {
                ++statement_list_counter;
            }
        }

        public override void Exit(syntax_tree_node st)
        {
            if (st is statement_list)
            {
                --statement_list_counter;
            }
        }

        public override void visit(import_statement _import_statement)
        {
            if (statement_list_counter > 1)
            {
                throw new SPythonSyntaxVisitorError("LOCAL_IMPORT_USE",
                   _import_statement.source_context);
            }

            base.visit(_import_statement);
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            if (statement_list_counter > 1)
            {
                throw new SPythonSyntaxVisitorError("LOCAL_FROM_IMPORT_USE",
                   _from_import_statement.source_context);
            }

            base.visit(_from_import_statement);
        }

        public override void visit(procedure_definition _procedure_definition)
        {
            if (statement_list_counter > 1)
            {
                throw new SPythonSyntaxVisitorError("LOCAL_FUNCTION_DECLARATION",
                   _procedure_definition.source_context);
            }

            base.visit(_procedure_definition);
        }
    }
}
