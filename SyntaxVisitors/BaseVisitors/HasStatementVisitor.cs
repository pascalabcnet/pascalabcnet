// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
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
    public class HasStatementWithBarrierVisitor<T,Barrier> : BaseEnterExitVisitor
        where T : statement 
        where Barrier: syntax_tree_node
    {
        private bool foundT = false;

        public static bool Has(syntax_tree_node node)
        {
            HasStatementWithBarrierVisitor<T, Barrier> vis = new HasStatementWithBarrierVisitor<T, Barrier>();

            vis.ProcessNode(node);

            return vis.foundT;
        }

        public override void Enter(syntax_tree_node sn) // Искать всюду, но не заходить в узлы Barrier и вложенные
        {
            if (sn is Barrier)
            {
                visitNode = false;
            }
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
