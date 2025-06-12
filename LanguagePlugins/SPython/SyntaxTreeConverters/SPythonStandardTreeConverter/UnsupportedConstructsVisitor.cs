using Languages.SPython.Frontend.Converters;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;

namespace Languages.Pascal.Frontend.Converters
{
    public class UnsupportedConstructsVisitor : BaseEnterExitVisitor, IPipelineVisitor
    {
        private int blockLevel = 0;
        // function returns a value
        private bool isInFunction = false;
        // procedure doesn't return value
        private bool isInProcedure = false;

        public UnsupportedConstructsVisitor() { }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            if (!context.Get<bool>("forIntellisense"))
                ProcessNode(root);

            next();
        }

        public override void Enter(syntax_tree_node st)
        {
            if (st is statement_list)
            {
                ++blockLevel;
            }
            if (st is procedure_definition pd)
            {
                if (pd.proc_header is function_header)
                    isInFunction = true;
                else
                    isInProcedure = true;
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
                isInProcedure = isInFunction = false;
            }
        }

        public override void visit(return_statement _return_statement)
        {
            // 'return' или 'return expr' вне функции
            if (!isInFunction && !isInProcedure)
            {
                throw new SPythonSyntaxVisitorError("RETURN_NOT_IN_FUNCTION",
                   _return_statement.source_context);
            }
            // 'return' внутри функции, которая должна возвращать значение
            if (isInFunction && _return_statement.expr == null)
            {
                throw new SPythonSyntaxVisitorError("RETURN_NOT_RETURN_VALUE",
                   _return_statement.source_context);
            }
            // 'return expr' внутри функции, которая не возвращает значение
            if (isInProcedure && _return_statement.expr != null)
            {
                throw new SPythonSyntaxVisitorError("RETURN_HAS_RETURN_VALUE",
                   _return_statement.expr.source_context);
            }

            base.visit(_return_statement);
        }

        public override void visit(global_statement _global_statement)
        {
            // конструкция  'global ...' используется вне функции
            if (!isInProcedure && !isInFunction)
            {
                throw new SPythonSyntaxVisitorError("GLOBAL_NOT_IN_FUNCTION",
                   _global_statement.source_context);
            }
            // конструкция  'global ...' используется
            // не на самом внешнем блоке внутри функции
            if (blockLevel != 2)
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
