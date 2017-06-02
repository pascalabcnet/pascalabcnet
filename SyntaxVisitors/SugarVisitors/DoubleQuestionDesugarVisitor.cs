using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class DoubleQuestionDesugarVisitor : BaseChangeVisitor
    {
        public static DoubleQuestionDesugarVisitor New
        {
            get { return new DoubleQuestionDesugarVisitor(); }
        }

        private int num = 0;

        public string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }

        public override void visit(double_question_node dqn)
        {
            var st = dqn.Parent;
            while (!(st is statement))
                st = st.Parent;
            var tname = "#temp" + UniqueNumStr();
            var tt = new var_statement(new ident(tname), dqn.left);
            tt.var_def.Parent = tt;
            var l = new List<statement>();
            l.Add(tt);
            l.Add(st as statement);
            expression right = dqn.right;
            var eq = new bin_expr(new ident(tname), new nil_const(), Operators.Equal, dqn.left.source_context);
            var qce = new question_colon_expression(eq, right, new ident(tname), dqn.source_context);
            ReplaceUsingParent(dqn, qce);
            visit(qce);
            ReplaceStatementUsingParent(st as statement, l);
            visit(tt);
        }
    }
}