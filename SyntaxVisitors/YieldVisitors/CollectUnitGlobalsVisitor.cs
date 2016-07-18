using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class CollectUnitGlobalsVisitor : CollectUpperNodesVisitor
    {
        public ISet<ident> CollectedGlobals { get; private set; } 

        public CollectUnitGlobalsVisitor()
        {
            CollectedGlobals = new HashSet<ident>();
        }

        /*public override void visit(interface_node node)
        {
            var globals = node.interface_definitions.defs
                .Select(def => def as variable_definitions)
                .Where(varDefsObj => (object)varDefsObj != null)
                .SelectMany(varDefs => varDefs.var_definitions)
                .SelectMany(v => v.vars.idents);

            CollectedGlobals.UnionWith(globals);
        }*/

        public override void visit(declarations node)
        {
            var globals = node.defs
                .Select(def => def as variable_definitions)
                .Where(varDefsObj => (object)varDefsObj != null)
                .SelectMany(varDefs => varDefs.var_definitions)
                .SelectMany(v => v.vars.idents);

            CollectedGlobals.UnionWith(globals);
        }
    }
}
