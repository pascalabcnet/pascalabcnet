using Languages.SPython.Frontend.Converters;
using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Languages.SPython.Frontend.Converters
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
                case "bigint":
                    id.name = "biginteger";
                    break;
                case "list":
                    id.name = "!list";
                    break;
                case "dict":
                    id.name = "!dict";
                    break;
                case "set":
                    id.name = "!set";
                    break;

                case "integer":
                    throw new SPythonSyntaxVisitorError("PASCALABCNET_TYPE_{0}_INSTEAD_OF_SPYTHON_TYPE_{1}",
                   id.source_context, id.name, "int");
                case "real":
                    throw new SPythonSyntaxVisitorError("PASCALABCNET_TYPE_{0}_INSTEAD_OF_SPYTHON_TYPE_{1}",
                   id.source_context, id.name, "float");
                case "string":
                    throw new SPythonSyntaxVisitorError("PASCALABCNET_TYPE_{0}_INSTEAD_OF_SPYTHON_TYPE_{1}",
                   id.source_context, id.name, "str");
                case "boolean":
                    throw new SPythonSyntaxVisitorError("PASCALABCNET_TYPE_{0}_INSTEAD_OF_SPYTHON_TYPE_{1}",
                   id.source_context, id.name, "bool");
                case "biginteger":
                    throw new SPythonSyntaxVisitorError("PASCALABCNET_TYPE_{0}_INSTEAD_OF_SPYTHON_TYPE_{1}",
                   id.source_context, id.name, "bigint");
            }
        }
    }
}
