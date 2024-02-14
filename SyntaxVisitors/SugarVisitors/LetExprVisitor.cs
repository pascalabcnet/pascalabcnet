// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    /// <summary>
    ///  А здесь не только замена кортежа - здесь оптимизация (a,b) := (1,2) и var (a,b) := (1,2)
    /// </summary>
    public class LetExprVisitor : BaseChangeVisitor
    {
        public static LetExprVisitor New
        {
            get { return new LetExprVisitor(); }
        }

        private int num = 0;
        public string UniqueName()
        {
            num++;
            return "#let_expr_vis"+num.ToString();
        }

        public override void visit(let_var_expr let_ex)
        {
            // Сгенерировать var_def_statement
            var vds = new var_statement(let_ex.id, let_ex.ex, let_ex.source_context);
            Replace(let_ex, let_ex.id);
            // Подняться до statement
            syntax_tree_node p = let_ex;
            while (p != null && !(p is statement))
                p = p.Parent;
            if (p == null)
                new SyntaxVisitorError("LET_EXPR_CANNOT_BE_IN_THIS_CONTEXT", let_ex);
            var stat = p as statement;
            var sl = new List<statement>();

            sl.Add(vds);
            sl.Add(stat);
            ReplaceStatementUsingParent(stat, sl);
            ProcessNode(vds);
            ProcessNode(stat);
        }
    }
}
