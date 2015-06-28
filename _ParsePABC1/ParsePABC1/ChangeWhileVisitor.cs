using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler.Errors;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace ParsePABC1
{
    class ChangeWhileVisitor : BaseChangeVisitor
    {
        int lbnum = 0;

        public string newLabelName()
        {
            lbnum++;
            return "lb#" + lbnum.ToString();
        }
        public override void Exit(syntax_tree_node st)
        {
            if (st.GetType() == typeof(while_node))
            {
                var wn = st as while_node;

                var stl = new statement_list();

                var gt1 = new goto_statement(newLabelName());
                var gt2 = new goto_statement(newLabelName());
                var gt3 = new goto_statement(newLabelName());

                var if0 = new if_node(wn.expr, gt1, null);
                var lb3 = new labeled_statement(gt3.label, if0);
                var lb1 = new labeled_statement(gt1.label, wn.statements);
                var lb2 = new labeled_statement(gt2.label, new empty_statement());

                stl.Add(lb3).Add(gt2).Add(lb1).Add(gt3).Add(lb2);

                //var op = new if_node(wn.expr, wn.statements, null);
                Replace(wn, stl);
                // в declarations ближайшего блока добавить описание labels
                block bl = listNodes.FindLast(x => x is block) as block;

                var il = new ident_list();
                il.Add(gt1.label).Add(gt2.label).Add(gt3.label);
                var ld = new label_definitions(il);
                bl.defs.Add(ld);
            }
            base.Exit(st);
        }

    }

}
