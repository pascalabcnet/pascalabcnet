using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class CountNodesVisitor : BaseEnterExitVisitor
    {
        public Dictionary<System.Type, int> d = new Dictionary<System.Type, int>();

        public int exprcount,statcount;
        public override void Enter(syntax_tree_node st)
        {
            if (d.ContainsKey(st.GetType()))
                d[st.GetType()] += 1;
            else d[st.GetType()] = 1;
            if (st is expression)
                exprcount++;
            if (st is statement)
                statcount++;
        }

        public void PrintSortedByValue()
        {
            var q = d.Select(x => new { x.Key, x.Value }).OrderByDescending(y => y.Value);
            foreach (var a in q)
                Console.WriteLine("{0}: {1} ", a.Key.Name, a.Value);
            Console.WriteLine();
            Console.WriteLine("Expressions Count: {0}", exprcount);
            Console.WriteLine("Statements Count: {0}", statcount);
        }

        public void Print()
        {
            foreach (var a in d)
                Console.WriteLine("{0}: {1} ", a.Key.Name, a.Value);
        }
    }
}
