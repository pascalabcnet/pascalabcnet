using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

/// <summary>
/// Этот визитор позволяет при переходе к семантике заменить синтаксический узел lambda_any_type_node_syntax
/// на семантический lambda_any_type_node
/// Это - единственное место, которое делало зависимость синтаксиса от семантики и этот визитор ее убирает
/// Мы вызываем этот визитор в компиляторе сразу после всех синтаксических визиторов
/// </summary>
namespace Languages.Pascal.Frontend.Converters
{
    public class LambdaAnyConverter : ISyntaxTreeConverter
    {
        public string Name { get; } = "LambdaAny";
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            LambdaAnyVisitor.New.ProcessNode(root);
            return root;
        }
    }

    public class LambdaAnyVisitor : BaseChangeVisitor
    {
        public static LambdaAnyVisitor New
        {
            get { return new LambdaAnyVisitor(); }
        }
        public override void visit(lambda_inferred_type lit)
        {
            if (lit.real_type is lambda_any_type_node_syntax)
                lit.real_type = new PascalABCCompiler.TreeRealization.lambda_any_type_node(); // семантический
        }
    }
}
