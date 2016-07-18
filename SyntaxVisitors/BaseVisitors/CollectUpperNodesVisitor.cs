using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class CollectUpperNodesVisitor : BaseEnterExitVisitor
    {
        protected List<syntax_tree_node> listNodes = new List<syntax_tree_node>();

        public syntax_tree_node CurrentNode
        {
            get
            {
                return listNodes[listNodes.Count - 1];
            }
        }

        public syntax_tree_node UpperNode(int up = 1)
        {
            if (listNodes.Count - 1 - up < 0)
                return null;
            return listNodes[listNodes.Count - 1 - up];
        }

        public override void Enter(syntax_tree_node st)
        {
            listNodes.Add(st);
        }
        public override void Exit(syntax_tree_node st)
        {
            listNodes.RemoveAt(listNodes.Count - 1);
        }
    }

}
