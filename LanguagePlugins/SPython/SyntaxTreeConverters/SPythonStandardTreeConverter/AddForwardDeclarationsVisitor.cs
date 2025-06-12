using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System;

namespace Languages.SPython.Frontend.Converters
{
    internal class AddForwardDeclarationsVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        private declarations decls;

        public AddForwardDeclarationsVisitor() { }

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
        }

        public override void Enter(syntax_tree_node stn)
        {
            if (stn is program_module pm)
            {
                decls = pm.program_block.defs;
            }
            if (stn is interface_node intn)
            {
                decls = intn.interface_definitions;
            }

            base.Enter(stn);
        }

        public override void visit(procedure_header _procedure_header)
        {
            SourceContext context = _procedure_header.source_context;
            var pd = new procedure_definition(_procedure_header.TypedClone(), null, context);
            pd.proc_header.proc_attributes.Add(new procedure_attribute(proc_attribute.attr_forward, context));
            decls.Add(pd, context);
        }

        public override void visit(function_header _function_header)
        {
            visit(_function_header as procedure_header);
        }
    }
}
