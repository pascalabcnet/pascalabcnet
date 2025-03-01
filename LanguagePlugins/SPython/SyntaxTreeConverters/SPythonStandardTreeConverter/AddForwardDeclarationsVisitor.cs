using Languages.Pascal.Frontend.Converters;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

namespace Languages.SPython.Frontend.Converters
{
    internal class AddForwardDeclarationsVisitor : BaseChangeVisitor
    {
        private declarations decls;

        public AddForwardDeclarationsVisitor() { }

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
    }
}
