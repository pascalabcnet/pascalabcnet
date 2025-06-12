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
                        new ident(PascalABCCompiler.StringConstants.default_constructor_name, ind.source_context),
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
            if (ind.Parent != null && !(ind.Parent is expression_list))
                possibleIndexer = null;
            if (possibleIndexer != null && possibleIndexer is indexer indexer)
            {
                // x[1:^1][5] - не должно сюда заходить в этом случае !!! Но неожиданно possibleIndexer оказывается expr[5]
                // Сюда должно заходить только при x[^1,3].  То есть, ind.Parent д.б. expression_list и только в этом случае!!!

                // Надо подняться до узла indexer и запомнить индекс, под которым данный index входит
                // Идём по Parent - и смотрим, когда Parent = indexer
                // Находим у него expressions и ищем, под каким индексом в нем содержится текущий. 
                // Запоминаем это целое число и передаем его последним параметром в Reverse если expressions.Count > 1
                expression_list reverseIndexMethodParams = null;
                if (indexer.indexes.expressions.Count == 1)
                    reverseIndexMethodParams = new expression_list(indexer.dereferencing_value, ind.source_context);
                else
                {
                    var i = indexer.indexes.expressions.FindIndex(x => x == ind);
                    if (i >= 0) 
                        reverseIndexMethodParams = new expression_list(new List<expression> { indexer.dereferencing_value, new int32_const(i) }, ind.source_context);
                }
                    
                mc = new method_call(
                        new dot_node(indexCreation, new ident("Reverse", ind.source_context)),
                        reverseIndexMethodParams,
                        ind.source_context);
            }
            else // Это непонятная ветка. Она наступает в частности если индексер не находится на 2 уровня выше, что бывает в случае индекса вида a[1..^1]
            {
                mc = indexCreation;
            }
            //var sug = new sugared_expression(ind, mc, ind.source_context);
            ReplaceUsingParent(ind, mc);
            visit(mc);
        }

    }
}
