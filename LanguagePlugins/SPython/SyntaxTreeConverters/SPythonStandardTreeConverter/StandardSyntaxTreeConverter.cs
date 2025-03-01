using Languages.Pascal.Frontend.Converters;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

namespace Languages.SPython.Frontend.Converters
{
    public class StandardSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name { get; } = "Standard";

        protected override syntax_tree_node ApplyConversions(syntax_tree_node root)
        {
            // добавление всех import ..., from ... import ...
            // как uses в начале программы для сбора имён из этих модулей
            var ituv = new ImportToUsesVisitor();
            ituv.ProcessNode(root);

            // вынос forward объявлений для всех функций в начало
            var afdv = new AddForwardDeclarationsVisitor();
            afdv.ProcessNode(root);

            // замена генерации списков на Select.Where.ToArray
            // (не работает из-за лямбд, если переместить в ConvertAfterUsedModulesCompilation)
            var ldv = new ListDesugarVisitor();
            ldv.ProcessNode(root);

            // дешугаризация составных сравнительных операций
            var ccdv = new CompoundComparisonDesugarVisitor();
            ccdv.ProcessNode(root);

            return root;
        }

        public override syntax_tree_node ConvertAfterUsedModulesCompilation(syntax_tree_node root, in CompilationArtifactsUsedBySyntaxConverters compilationArtifacts)
        {
            // визитер проверящий корректность имён из модулей
            // и заменяющий первые присваивания переменных на объявление с инициализацией
            var ncv = new NameCorrectVisitor(compilationArtifacts.NamesFromUsedUnits);
            ncv.ProcessNode(root);

            // выносит глобальные переменные на локальный уровень
            // если они не используются в функциях (не являются глобальными)
            var rugvv = new RetainUsedGlobalVariablesVisitor();
            rugvv.ProcessNode(root);

            // замена вызова функций с именованными параметрами на вызов метода класса
            var fwnpdv = new FunctionsWithNamedParametersDesugarVisitor();
            fwnpdv.ProcessNode(root);

            // удаление специфичных синтаксических узлов Spython'a перед конвертацией в семантическое дерево
            var esonv = new EraseSpythonOnlyNodesVisitor();
            esonv.ProcessNode(root);

            // перестроение структуры дерева, для последующих этапов компиляции
            var tnrv = new TreeNodesRearrangementVisitor();
            tnrv.ProcessNode(root);

            return root;
        }
    }
}
