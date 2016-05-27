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
                    sttl.subnodes[0] = new labeled_statement(lst.label_name, sttl.subnodes[0]); // а если [0] элемента вообще нет?
                    ReplaceStatement(lst,sttl.subnodes);
                }
            }

            base.Exit(st);
        }

    }
}
