using System.Collections.Generic;
using System.Data;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    public class EraseGlobalNodesVisitor : BaseChangeVisitor
    {
        public EraseGlobalNodesVisitor() {}

        public override void visit(global_statement _global_statement)
        {
            DeleteInStatementList(_global_statement);
        }

        public override void visit(import_statement _import_statement)
        {
            DeleteInStatementList(_import_statement);
        }

        public override void visit(from_import_statement _from_import_statement)
        {
            DeleteInStatementList(_from_import_statement);
        }
    }
}