
using System.Collections.Generic;
using System.Linq;


namespace AssignTupleDesugar
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

        public static  List<Assign> getAssignOrder(List<Symbol> left, List<Symbol> right) {
            
            var graph = GraphUtils.createAssignGraph(left, right);
            var order = graph.GetAssignOrder();
            return order.Select(elem => new Assign(to: elem.to.symbol, from: elem.from.symbol)).ToList();
        }

    }
}
