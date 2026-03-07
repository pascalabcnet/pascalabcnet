using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.CoreUtils;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class DoubleQuestionDesugarVisitor : BaseChangeVisitor
    {
        /*private readonly GeneratedNamesManager generatedNamesManager;

        public DoubleQuestionDesugarVisitor(GeneratedNamesManager generatedNamesManager)
        {
            this.generatedNamesManager = generatedNamesManager;
        }

        public static DoubleQuestionDesugarVisitor Create(GeneratedNamesManager generatedNamesManager) => new DoubleQuestionDesugarVisitor(generatedNamesManager);

        public override void visit(double_question_node dqn)
        {
            var st = dqn.Parent;
            while (!(st is statement))
                st = st.Parent;
            var tname = generatedNamesManager.GenerateName("#temp");
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