using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

namespace Languages.SPython.Frontend.Converters
{
    public class StandardSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name { get; } = "Standard";

        protected override syntax_tree_node ApplyConcreteConversions(syntax_tree_node root)
        {
            var sfvugv = new SPythonRetainUsedGlobalVariablesVisitor();
            sfvugv.ProcessNode(root);

            return root;
        }
    }
}
