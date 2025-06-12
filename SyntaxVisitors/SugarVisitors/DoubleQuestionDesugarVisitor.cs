using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;

namespace SyntaxVisitors.SugarVisitors
{
    public class DoubleQuestionDesugarVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        public static DoubleQuestionDesugarVisitor New
        {
            get { return new DoubleQuestionDesugarVisitor(); }
        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
        }

        private int num = 0;

        public string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }

        /*public override void visit(double_question_node dqn)
        {
            var st = dqn.Parent;
            while (!(st is statement))
                st = st.Parent;
            var tname = "#temp" + UniqueNumStr();
            var tt = new var_statement(new ident(tname, dqn.source_context), dqn.left, dqn.source_context);
            tt.var_def.Parent = tt;
            var l = new List<statement>();
            l.Add(tt);
            l.Add(st as statement);
            expression right = dqn.right;
            var ni = new nil_const();
            ni.source_context = dqn.source_context;
            var eq = new bin_expr(new ident(tname, dqn.source_context), ni, Operators.NotEqual, dqn.left.source_context);
            var qce = new question_colon_expression(eq, new ident(tname, dqn.source_context), right, dqn.source_context);
            ReplaceUsingParent(dqn, qce);
            visit(qce);
            ReplaceStatementUsingParent(st as statement, l);
            visit(tt);
        }*/
    }
}