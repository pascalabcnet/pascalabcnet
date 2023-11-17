using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;
using SyntaxVisitors.SugarVisitors;

namespace AssignTupleOptimizer
{
    class SymInfoFilterVisitor : CollectFullLightSymInfoVisitor
    {
        public HashSet<string> targets;    
        public SymInfoFilterVisitor(program_module root, IEnumerable<string> t) : base(root)
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
