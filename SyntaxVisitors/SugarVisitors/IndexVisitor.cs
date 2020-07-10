// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class IndexVisitor : BaseChangeVisitor
    {
        public static IndexVisitor New
        {
            get { return new IndexVisitor(); }
        }

        public override void visit(index ind)
        {
            var indexCreation =
                new method_call(
                    new dot_node(
                        new ident("SystemIndex", ind.source_context),
                        new ident("Create", ind.source_context),
                        ind.source_context),
                    new expression_list(
                        new List<expression>()
                        {
                            ind.index_expr,
                            new bool_const(ind.inverted, ind.source_context)
                        },
                        ind.source_context),
                    ind.source_context);
            syntax_tree_node mc = null;
            var possibleIndexer = ind.Parent?.Parent;
            if (possibleIndexer != null && possibleIndexer is indexer indexer)
            {
                var reverseIndexMethodParams = new expression_list(indexer.dereferencing_value, ind.source_context);
                mc = 
                    new method_call(
                        new dot_node(indexCreation, new ident("Reverse", ind.source_context)),
                        reverseIndexMethodParams,
                        ind.source_context);
            }
            else
            {
                mc = indexCreation;
            }
            //var sug = new sugared_expression(ind, mc, ind.source_context);
            Replace(ind, mc);
            visit(mc);
        }

    }
}
