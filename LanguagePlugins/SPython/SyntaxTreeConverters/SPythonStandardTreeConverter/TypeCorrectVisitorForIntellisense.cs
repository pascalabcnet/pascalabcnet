using Languages.SPython.Frontend.Converters;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Languages.SPython.Frontend.Converters
{
    public class TypeCorrectVisitorForIntellisense : BaseChangeVisitor
    {
        public override void visit(named_type_reference _named_type_reference)
        {
            ident id = _named_type_reference.names[0];
            switch (id.name)
            {
                case "list":
                    id.name = "!list";
                    break;
                case "dict":
                    id.name = "!dict";
                    break;
                case "set":
                    id.name = "!set";
                    break;
            }
        }
    }
}
