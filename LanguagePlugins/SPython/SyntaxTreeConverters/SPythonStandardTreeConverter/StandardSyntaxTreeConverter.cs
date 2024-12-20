using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using System.Collections.Generic;

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

            // замена генерации списков на Select.Where.ToArray
            // (почему-то не работает если переместить в ConvertAfterUsedModulesCompilation)
            var ldv = new ListDesugarVisitor();
            ldv.ProcessNode(root);

            return root;
        }

        public override syntax_tree_node ConvertAfterUsedModulesCompilation(syntax_tree_node root, object data) 
        {
            // визитер проверящий корректность имён из модулей
            // и заменяющий первые присваивания переменных на объявление с инициализацией
            var niv = new NameInterpreterVisitor(data as Dictionary<string, HashSet<string>>);
            niv.ProcessNode(root);

            // вынос переменных самого внешнего уровня на глобальный
            // если они используются в функциях (являются глобальными)
            var rugvv = new RetainUsedGlobalVariablesVisitor();
            rugvv.ProcessNode(root);

            // замена вызова функций с именованными параметрами на вызов метода класса
            var fwnpdv = new FunctionsWithNamedParametersDesugarVisitor();
            fwnpdv.ProcessNode(root);

            // удаление узлов global
            var esonv = new EraseSpythonOnlyNodesVisitor();
            esonv.ProcessNode(root);

            return root;
        }
    }
}
