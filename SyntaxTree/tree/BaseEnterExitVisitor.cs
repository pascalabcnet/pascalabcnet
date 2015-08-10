using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTree
{
    public class BaseEnterExitVisitor: WalkingVisitorNew
    {
        public BaseEnterExitVisitor()
        {
            OnEnter = Enter;
            OnLeave = Exit;
        }

        public virtual void Enter(syntax_tree_node st)
        {
        }
        public virtual void Exit(syntax_tree_node st)
        {
        }
    }
}
