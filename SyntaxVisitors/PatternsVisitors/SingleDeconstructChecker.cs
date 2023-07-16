using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxVisitors
{
    public class SingleDeconstructChecker : BaseEnterExitVisitor
    {
        private const string DeconstructMethodName = "deconstruct"; // в трех местах эту константу - если менять

        private int deconstructCount = 0;
        private bool isInClass = false;

        public static SingleDeconstructChecker New => new SingleDeconstructChecker();

        public override void Enter(syntax_tree_node st)
        {
            if (st is class_definition)
            {
                isInClass = true;
                deconstructCount = 0;
            }
        }

        public override void Exit(syntax_tree_node st)
        {
            if (st is class_definition)
                isInClass = false;
        }

        public override void visit(method_name _method_name)
        {
            if (!isInClass)
                return;

            if (_method_name?.meth_name?.name?.ToLower() == DeconstructMethodName)
                deconstructCount++;

            if (deconstructCount > 1)
                throw new SyntaxVisitorError("ONLY_ONE_DECONSTRUCT_ALLOWED", _method_name.Parent.source_context);
        }
    }
}
