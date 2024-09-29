using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

namespace Languages.SPython.Frontend.Converters
{
    public class StandardSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name { get; } = "Standard";

        protected override syntax_tree_node ApplyConcreteConversions(syntax_tree_node root)
        {
            // замена генерации списков на Select.Where.ToArray
            var lgnv = new ListGeneratorNodesVisitor();
            lgnv.ProcessNode(root);

            // замена узлов assign на узлы var
            // (внутри ф-й основываясь на узлах global,
            // вне ф-й по первому появлению в symbolTable)
            var atvcv = new AssignToVarConverterVisitor();
            atvcv.ProcessNode(root);

            // удаление узлов global
            var egnv = new EraseGlobalNodesVisitor();
            egnv.ProcessNode(root);

            // вынос переменных самого внешнего уровня на глобальный
            // если они используются в функциях (являются глобальными)
            var rugvv = new RetainUsedGlobalVariablesVisitor();
            rugvv.ProcessNode(root);

            return root;
        }
    }
}
