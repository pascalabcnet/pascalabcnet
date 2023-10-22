using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;
using SyntaxVisitors.SugarVisitors;

namespace AssignTupleOptimizer
{
    class SymInfoFilterVisitor : MyCollectLightSymInfoVisitor
    {
        public HashSet<string> targets;    
        public SymInfoFilterVisitor(IEnumerable<string> t)
        {
            targets = new HashSet<string>();
            foreach (var target in t) targets.Add(target);
        }

        public override void AddSymbol(ident name, SymKind kind, type_definition td = null, Attributes attr = 0)
        {
            System.Console.Write("addSymbol: " + name + "?: ");
            if (targets.Contains(name.name))
            {
                System.Console.Write("Yes");
                base.AddSymbol(name, kind, td, attr);
            }
            else System.Console.Write("No");

            System.Console.WriteLine();
        }

    }
    
}
