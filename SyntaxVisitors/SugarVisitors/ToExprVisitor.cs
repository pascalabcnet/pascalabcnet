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
    public class ToExprVisitor : BaseChangeVisitor
    {
        public static ToExprVisitor New
        {
            get { return new ToExprVisitor(); }
        }

        private int num = 0;
        public string UniqueName()
        {
            num++;
            return "#let_expr_vis"+num.ToString();
        }

        public override void visit(to_expr to_ex)
        {
            var pairMethod =
                new method_call(
                    new dot_node(
                        new ident("PABCSystem", to_ex.source_context),
                        new ident("Pair", to_ex.source_context),
                        to_ex.source_context),
                    new expression_list(
                        new List<expression>()
                        {
                                        to_ex.key,
                                        to_ex.value
                        },
                        to_ex.source_context),
                    to_ex.source_context);

            Replace(to_ex, pairMethod);
            ProcessNode(to_ex.key);
            ProcessNode(to_ex.value);
        }
    }
}
