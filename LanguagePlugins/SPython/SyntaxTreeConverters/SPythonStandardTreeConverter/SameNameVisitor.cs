using System.Collections.Generic;
using System.Data;
using System.Linq;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;

namespace Languages.SPython.Frontend.Converters
{
    internal class SameNameVisitor : BaseChangeVisitor
    {
        List<string> names = new List<string> { "time", "random" };

        public SameNameVisitor() { }

        public override void visit(dot_node _dot_node)
        {
            if (_dot_node.left is ident left_id &&
                _dot_node.right is ident right_id &&
                left_id.name == right_id.name &&
                names.Contains(left_id.name))
                left_id.name = left_id.name + '1';
            base.visit(_dot_node);
        }

        public override void visit(uses_list _uses_list)
        {
            for (int i = 0; i < _uses_list.units.Count; ++i)
            {
                string name = _uses_list.units[i].name.idents[0].name;
                if (names.Contains(name))
                    _uses_list.units.Insert(i++, 
                        new unit_or_namespace(new ident_list(new ident(name + '1'))));
            }
        }
    }
}
