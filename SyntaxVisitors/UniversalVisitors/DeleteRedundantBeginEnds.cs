// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class DeleteRedundantBeginEnds : BaseChangeVisitor
    {
        public static DeleteRedundantBeginEnds New
        {
            get { return new DeleteRedundantBeginEnds(); }
        }

        public static DeleteRedundantBeginEnds Accept(procedure_definition pd)
        {
            var n = New;
            n.ProcessNode(pd);
            return n;
        }

        public override void Exit(syntax_tree_node st)
        {
            var stl = st as statement_list;
            if (stl != null && UpperNode() is statement_list)
                ReplaceStatement(stl, stl.subnodes);

            var lst = st as labeled_statement;
            if (lst != null)
            {
                var sttl = lst.to_statement as statement_list;
                if (sttl != null)
                {
                    if (sttl.subnodes[0] is var_statement) // SSM - 17.07.16 - фикс - нельзя помечать меткой var_statement! 
                    {
                        sttl.AddFirst(empty_statement.New);
                    }
                    sttl.subnodes[0] = new labeled_statement(lst.label_name, sttl.subnodes[0]); // [0] элемент есть обязательно - в случае пустого begin end это empty_statement
                    ReplaceStatement(lst,sttl.subnodes);
                }
            }

            base.Exit(st);
        }

    }
}
