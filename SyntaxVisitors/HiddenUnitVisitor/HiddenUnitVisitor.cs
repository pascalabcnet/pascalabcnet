// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using PascalABCCompiler.SyntaxTree;

namespace SyntaxVisitors
{
    public class HiddenUnitVisitor : WalkingVisitorNew
    {
        public override void visit(method_name method)
        {
            method.meth_name.name = "@" + method.meth_name.name;
        }
    }
}
