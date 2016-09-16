using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;


namespace SyntaxVisitors
{
    public class HasStatementVisitor<T> : WalkingVisitorNew
        where T : statement
    {
        private bool foundT = false;

        public static bool Has(syntax_tree_node node)
        {
            HasStatementVisitor<T> vis = new HasStatementVisitor<T>();

            vis.ProcessNode(node);

            return vis.foundT;
        }

        public override void DefaultVisit(syntax_tree_node node)
        {
            if (node is T)
                foundT = true;
            else 
                base.DefaultVisit(node);
        }
    }
}
