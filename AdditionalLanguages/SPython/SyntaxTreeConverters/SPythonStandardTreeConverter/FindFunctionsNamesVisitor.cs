using PascalABCCompiler.SyntaxTree;
using System.Collections.Generic;

namespace Languages.SPython.Frontend.Converters
{
    internal class FindFunctionsNamesVisitor : BaseChangeVisitor
    {
        public HashSet<string> definedFunctionsNames = new HashSet<string>();

        public override void visit(procedure_definition pd)
        {
            definedFunctionsNames.Add(pd.proc_header.name.meth_name.name);
        }
    }
}
