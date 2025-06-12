using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Languages.SPython.Frontend.Converters
{
    internal class FindFunctionsNamesVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        private HashSet<string> definedFunctionsNames = new HashSet<string>();

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            context.Set("definedFunctionsNames", definedFunctionsNames);

            next();
        }

        public override void visit(procedure_definition pd)
        {
            definedFunctionsNames.Add(pd.proc_header.name.meth_name.name);
        }
    }
}
