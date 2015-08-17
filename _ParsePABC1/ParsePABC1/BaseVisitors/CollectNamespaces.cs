using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class CollectUpperNamespacesVisitor : CollectUpperNodesVisitor
    {
        protected List<declaration> list = new List<declaration>();

        public bool PrintInfo = true;

        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            if (st is procedure_definition || st is class_definition)
            {
                list.Add(st as declaration);
                if (PrintInfo)
                    Console.Write("+: "+st.GetType().Name);
            }
        }
        public override void Exit(syntax_tree_node st)
        {
            if (st is procedure_definition || st is class_definition)
            {
                if (PrintInfo)
                    Console.WriteLine("-: " + st.GetType().Name);
                list.RemoveAt(list.Count - 1);
            }
            base.Exit(st);
        }

        public override void visit(procedure_definition p)
        {
            if (PrintInfo)
                Console.WriteLine(" " + p.proc_header.name.meth_name);

            var ld = new FindLocalDefsVisitor(); 
            p.visit(ld);

            base.visit(p);
        }
        public override void visit(class_definition cl)
        {
            type_declarations tds = UpperNode(2) as type_declarations;
            if (PrintInfo)
            {
                var f = tds.types_decl.Find(td => td.type_def == cl);
                Console.WriteLine(" " + f.type_name);
            }
            base.visit(cl);
        }
    }

}
