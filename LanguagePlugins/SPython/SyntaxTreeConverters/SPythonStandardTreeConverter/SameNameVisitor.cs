using System.Collections.Generic;
using System.Data;
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class SameNameVisitor : BaseChangeVisitor
    {
        public SameNameVisitor() { }

        public override void visit(dot_node _dot_node)
        {
            if (_dot_node.left is ident left_id &&
                _dot_node.right is ident right_id &&
                left_id.name == right_id.name &&
                left_id.name == "time")
                left_id.name = "time1";
        }
    }
}
