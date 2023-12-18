using System.Collections.Generic;

namespace AssignTupleDesugarAlgorithm
{
    internal class Cycle
    {
        List<SymbolNode> cycle;


        public int Len => cycle.Count;

        public Cycle(List<SymbolNode> c)
        {
            cycle = c;
        }

        public SymbolNode this[int i] => cycle[i % Len];


        public override string ToString()
        {
            string res = "";
            foreach (var v in cycle)
            {
                res += v + "->";
            }

            return res + cycle[0];
        }
    }
}