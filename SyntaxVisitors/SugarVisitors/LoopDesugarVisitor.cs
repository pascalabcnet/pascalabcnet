// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors.SugarVisitors
{
    public class LoopDesugarVisitor : BaseChangeVisitor
    {
        public static LoopDesugarVisitor New
        {
            get { return new LoopDesugarVisitor(); }
        }

        private int num = 0;

        public string UniqueNumStr()
        {
            num++;
            return num.ToString();
        }

        public override void visit(loop_stmt loop)
        {
            // тут возможно ошибка более глубокая - в semantic_check_sugared_statement_node(asstup) возможно остаются во вложенных лямбдах другие assign_tuple
            var sl = new statement_list();
            sl.Add(new semantic_check_sugared_statement_node(typeof(loop_stmt), new List<syntax_tree_node> { loop.count }, loop.source_context));

            var tname = "#loop_var" + UniqueNumStr();
            var fn = new for_node(new ident(tname), new int32_const(1), loop.count, loop.stmt, loop.source_context);
            sl.Add(fn);

            ReplaceUsingParent(loop, sl);

            visit(fn);
        }
    }
}
