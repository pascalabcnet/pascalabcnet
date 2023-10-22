 using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;


namespace AssignTupleOptimizer
{
    class AssignTupleFilterVisitor : WalkingVisitorNew
    {

        public HashSet<string> targets = new HashSet<string>();

        public int count = 0;

        public override void visit(assign_tuple node)
        {
            if (node.expr is tuple_node tn)
            {
                count++;
                foreach (var sym in node.vars.variables)
                {
                    if (sym is ident id) targets.Add(id.name);
                }

                foreach (var sym in tn.el.expressions)
                {
                    if (sym is ident id) targets.Add(id.name);
                }
            }
            base.visit(node);
        }
    }
}
