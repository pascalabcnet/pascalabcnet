using Languages.SPython.Frontend.Converters;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Languages.Pascal.Frontend.Converters
{
    public class TypeCorrectVisitor : BaseChangeVisitor
    {
        public override void visit(named_type_reference _named_type_reference)
        {
            ident id = _named_type_reference.names[0];
            switch (id.name)
            {
                case "int":
                    id.name = "integer";
                    break;
                case "float":
                    id.name = "real";
                    break;
                case "str":
                    id.name = "string";
                    break;
                case "bool":
                    id.name = "boolean";
                    break;
            }
        }
    }
}
