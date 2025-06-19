using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace Languages.SPython.Frontend.Converters
{
    internal class ReturnDesugarVisitor : BaseChangeVisitor
    {
        public ReturnDesugarVisitor() { }

        public override void visit(return_statement _return_statement)
        {
            SourceContext sc = _return_statement.source_context;

            // return;
            if (_return_statement.expr == null)
            {
                procedure_call pc = new procedure_call(new ident("exit", sc), true, sc);
                Replace(_return_statement, pc);
            }
            // return expr;
            else
            {
                statement res_assign = new assign(new ident(StringConstants.result_var_name), _return_statement.expr, Operators.Assignment, sc);
                statement exit_call = new procedure_call(new ident("exit"), true, sc);
                statement_list new_statement = new statement_list(res_assign, sc);
                new_statement.Add(exit_call, sc);
                Replace(_return_statement, new_statement);
            }
        }
    }
}
