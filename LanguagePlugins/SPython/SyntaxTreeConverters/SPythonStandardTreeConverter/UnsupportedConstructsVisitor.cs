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
        private int blockLevel = 0;
        private bool isInFunction = false;

        public UnsupportedConstructsVisitor() { }

        public override void Enter(syntax_tree_node st)
        {
            if (st is statement_list)
            {
                ++blockLevel;
            }
            if (st is procedure_definition)
            {
                isInFunction = true;
            }
        }

        public override void Exit(syntax_tree_node st)
        {
            if (st is statement_list)
            {
                --blockLevel;
            }
            if (st is procedure_definition)
            {
                isInFunction = false;
            }
        }

        public override void visit(global_statement _global_statement)
        {
            // конструкция  'global ...' используется вне функции
            if (!isInFunction)
            {
                throw new SPythonSyntaxVisitorError("GLOBAL_NOT_IN_FUNCTION",
                   _global_statement.source_context);
            }
            // конструкция  'global ...' используется
            // не на самом внешнем блоке внутри функции
            if (blockLevel != 1)
            {
                throw new SPythonSyntaxVisitorError("GLOBAL_IN_NOT_OUTERMOST_BLOCK",
                   _global_statement.source_context);
            }

            base.visit(_global_statement);
        }

        public override void visit(import_statement _import_statement)
        {
            // конструкция 'import ...'
            // используется не на глобальном уровне
            if (blockLevel > 1)
            {
                throw new SPythonSyntaxVisitorError("LOCAL_IMPORT_USE",
                   _import_statement.source_context);
            }

            base.visit(_import_statement);
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            // конструкция 'from ... import ...'
            // используется не на глобальном уровне
            if (blockLevel > 1)
            {
                throw new SPythonSyntaxVisitorError("LOCAL_FROM_IMPORT_USE",
                   _from_import_statement.source_context);
            }

            base.visit(_from_import_statement);
        }

        public override void visit(procedure_definition _procedure_definition)
        {
            // объявление функции не на глобальном уровне
            if (blockLevel > 1)
            {
                throw new SPythonSyntaxVisitorError("LOCAL_FUNCTION_DECLARATION",
                   _procedure_definition.source_context);
            }

            base.visit(_procedure_definition);
        }
    }
}
