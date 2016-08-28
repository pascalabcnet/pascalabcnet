using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PascalABCCompiler.SyntaxTree 
{
    public class FillParentNodeVisitor: CollectUpperNodesVisitor
    {
        public static FillParentNodeVisitor New
        {
            get
            {
                return new FillParentNodeVisitor();
            }
        }
        public override void Enter(syntax_tree_node st)
        {
            base.Enter(st);
            st.Parent = UpperNode();
        }
    }
}
