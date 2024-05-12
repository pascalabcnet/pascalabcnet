using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.SyntaxTree;

namespace AssignTupleDesugarAlgorithm
{
    public class Assign
    {
        public Symbol from;
        public Symbol to;
        public Assign(Symbol to, Symbol from)
        {
            this.to = to;
            this.from = from;
        }

        public static List<Assign> getAssignOrder(tuple_node tn, addressed_value_list vars, BindCollectLightSymInfo binder)
        {
            var left = new List<Symbol>();
            var right = new List<Symbol>();

            foreach (var ex in vars.variables)
            {
                var s = new Symbol(ex, binder);
                left.Add(s);
            }


            foreach (var ex in tn.el.expressions)
            {
                var s = new Symbol(ex, binder);
                right.Add(s);
            }

      
            var graph = GraphUtils.createAssignGraph(left, right);
            //graph.drawGraph();
            var order = graph.GetAssignOrder();
            return order.Select(elem => new Assign(to: elem.to.symbol, from: elem.from.symbol)).ToList();
        }

        

    }
}
