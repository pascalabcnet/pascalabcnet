using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Languages.SPython.Frontend.Converters
{
    internal class AssignmentCharAsStringVisitor : BaseChangeVisitor
    {
        public AssignmentCharAsStringVisitor() { }

        public override void visit(assign _assign)
        {
            if (_assign.from is string_const sc
                && sc.Value.Length == 1)
            {
                _assign.from = new method_call(new ident("str"), new expression_list(sc, sc.source_context), sc.source_context);
            }
        }
    }
}
