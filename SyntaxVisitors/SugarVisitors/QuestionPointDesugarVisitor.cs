// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;

namespace SyntaxVisitors.SugarVisitors
{
    public class QuestionPointDesugarVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        public static QuestionPointDesugarVisitor New
        {
            get { return new QuestionPointDesugarVisitor(); }
        }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
        }

        public question_colon_expression ConvertToQCE(dot_question_node dqn)
        {
            addressed_value left = dqn.left;
            addressed_value right = dqn.right;

            var eq = new bin_expr(left, new nil_const(), Operators.Equal, left.source_context);
            addressed_value dn = null;
            var dnleft = right; // Левая часть dn
            addressed_value rdqn = null;
            addressed_value ldqn = null;

            // Если right - это dot_question_node, то протащить внутрь него left. В силу ассоциирования слева направо достаточно на первый уровень.
            if (right.GetType() == typeof(dot_question_node))
            {
                var dqn_int = right as dot_question_node;
                ldqn = dqn_int.left;
                dnleft = ldqn;
                rdqn = dqn_int.right;
                // необходимо left протащить внутрь в ldqn
            }

            // Пока right - это dot_node, то необходимо протаскивать left чтобы присоединить его к первому не dot_node
            while (dnleft.GetType() == typeof(dot_node))
            {
                dn = dnleft;
                dnleft = (dnleft as dot_node).left;
            }
            // В итоге в dnleft - самый внутренний left, который уже не является dot_node
            dnleft = new dot_node(left, dnleft, left.source_context); // Прикрепили right к самому внутреннему левому узлу в нотации a.b.c.d
            if (dn != null)
                (dn as dot_node).left = dnleft;
            else if (rdqn == null)
                right = dnleft;
            else
            {
                right = new dot_question_node(dnleft, (right as dot_question_node).right, dnleft.source_context);
            }

            var q = new question_colon_expression(eq, new nil_const(), right, dqn.source_context);
            right.Parent = q;
            return q;
        }

        public addressed_value Into(addressed_value x, addressed_value v) // При возникновении новой конструкции в грамматике variable добавить обработку сюда
        {
            if (v.GetType() == typeof(dot_question_node))
            {
                var vv = v as dot_question_node;
                var res = new dot_question_node(Into(x, vv.left), vv.right, x.source_context);
                return res;
            }
            else if (v.GetType() == typeof(dot_node))
            {
                var vv = v as dot_node;
                var res = new dot_node(Into(x, vv.left), vv.right, x.source_context);
                return res;
            }
            else if (v.GetType() == typeof(indexer))
            {
                var vv = v as indexer;
                var res = new indexer(Into(x, vv.dereferencing_value), vv.indexes, x.source_context);
                return res;
            }
            else if (v.GetType() == typeof(slice_expr))
            {
                var vv = v as slice_expr;
                var res = new slice_expr(Into(x, vv.dereferencing_value), vv.from, vv.to, vv.step, x.source_context);
                return res;
            }
            else if (v.GetType() == typeof(slice_expr_question))
            {
                var vv = v as slice_expr_question;
                var res = new slice_expr_question(Into(x, vv.dereferencing_value), vv.from, vv.to, vv.step, x.source_context);
                return res;
            }
            else if (v.GetType() == typeof(method_call))
            {
                var vv = v as method_call;
                var res = new method_call(Into(x, vv.dereferencing_value), vv.parameters, x.source_context);
                return res;
            }
            else if (v.GetType() == typeof(roof_dereference))
            {
                var vv = v as roof_dereference;
                var res = new roof_dereference(Into(x, vv.dereferencing_value), x.source_context);
                return res;
            }
            else if (v.GetType() == typeof(ident_with_templateparams))
            {
                var vv = v as ident_with_templateparams;
                var res = new ident_with_templateparams(Into(x, vv.name), vv.template_params, x.source_context);
                return res;
            }
            else
            {
                var res = new dot_node(x, v, x.source_context);
                return res;
            }
        }

        private int num = 0;

        public string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }


        /*public question_colon_expression ConvertToQCE1(dot_question_node dqn)
        {
            addressed_value left = dqn.left;
            addressed_value right = dqn.right;

            var eq = new bin_expr(left, new nil_const(), Operators.Equal, left.source_context);

            var nr = Into(left, right);

            var q = new question_colon_expression(eq, new nil_const(), nr, dqn.source_context);
            nr.Parent = q;
            return q;
        }*/

        public question_colon_expression ConvertToQCE1(dot_question_node dqn, string name)
        {
            addressed_value right = dqn.right;

            var eq = new bin_expr(new ident(name,dqn.source_context), new nil_const(), Operators.Equal, dqn.left.source_context);

            var nr = Into(new ident(name), right);
            nr.source_context = dqn.source_context;
            var nc = new nil_const();
            nc.source_context = dqn.source_context;
            var q = new question_colon_expression(eq, nc, nr, dqn.source_context);
            nr.Parent = q;
            return q;
        }

        public override void visit(dot_question_node dqn)
        {
            var st = dqn.Parent;
            while ((st != null) && !(st is statement))
                st = st.Parent;
            if (st == null)
                throw new SyntaxVisitorError("?._CANNOT_BE_IN_THIS_CONTEXT", dqn.source_context);
            var tname = "#dqn_temp" + UniqueNumStr();

            dot_question_node rif = null;
            var qce = ConvertToQCE1(dqn, tname);
            if (qce.ret_if_false is dot_question_node dn)
            {
                rif = dn;
                var expr = new question_colon_expression(qce.condition, qce.ret_if_true, rif.left);
                rif.left.ExprToQCE = expr;
            }
            var sug = sugared_addressed_value.NewP(dqn, qce, dqn.source_context);
            ReplaceUsingParent(dqn, sug);
            //Replace(dqn, sug); // Этот не подходит!

            var dl = (dqn.left.ExprToQCE == null ? dqn.left : dqn.left.ExprToQCE) as addressed_value;
            dqn.Parent = null;
            var tt = new var_statement(new ident(tname), dl, dqn.source_context);
            tt.var_def.Parent = tt;
            var l = new List<statement>();
            l.Add(tt);
            l.Add(st as statement);

            ReplaceStatementUsingParent(st as statement, l);
            visit(qce);
            visit(tt);
        }
        /*public override void visit(dot_question_node dqn)
        {
            var qce = ConvertToQCE1(dqn);
            var sug = sugared_addressed_value.NewP(dqn, qce, dqn.source_context);
            ReplaceUsingParent(dqn, sug);
            visit(qce);
        }*/
    }
}
