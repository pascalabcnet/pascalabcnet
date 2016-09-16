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
    public class ChangeWhileVisitor : BaseChangeVisitor
    {
        int lbnum = 0;

        public static ChangeWhileVisitor New
        {
            get { return new ChangeWhileVisitor();  }
        }

        public string newLabelName()
        {
            lbnum++;
            return "lb#" + lbnum.ToString();
        }
        public override void visit(while_node wn)
        {
            ProcessNode(wn.statements);
            var gt1 = new goto_statement(newLabelName());
            var gt2 = new goto_statement(newLabelName());
            var gt3 = new goto_statement(newLabelName());

            var if0 = new if_node(wn.expr, gt1, null);
            var lb3 = new labeled_statement(gt3.label, if0);
            var lb1 = new labeled_statement(gt1.label, wn.statements);
            var lb2 = new labeled_statement(gt2.label, new empty_statement());

            var stl = new statement_list(lb3, gt2, lb1, gt3, lb2);

            Replace(wn, stl);
            // в declarations ближайшего блока добавить описание labels
            block bl = listNodes.FindLast(x => x is block) as block;

            var ld = new label_definitions(gt1.label, gt2.label, gt3.label);
            bl.defs.Add(ld);
        }

    }

}
