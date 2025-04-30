using Languages.Pascal.Frontend.Converters;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

namespace Languages.SPython.Frontend.Converters
{
    public class StandardSyntaxTreeConverterForIntellisense : BaseSyntaxTreeConverter
    {
        public override string Name { get; } = "StandardForIntellisense";

        protected override syntax_tree_node ApplyConversions(syntax_tree_node root)
        {
            // кидает ошибки за использование 
            // неподдерживаемых конструкций языка
            var ucv = new UnsupportedConstructsVisitor();
            ucv.ProcessNode(root);

            // добавление всех import ..., from ... import ...
            // как uses в начале программы для сбора имён из этих модулей
            // var ituv = new ImportToUsesVisitor();
            // ituv.ProcessNode(root);

            // замена return;       на { exit(); }
            // замена return expr;  на { result := expr; exit(); }
            var rdv = new ReturnDesugarVisitor();
            rdv.ProcessNode(root);

            // замена генерации списков на Select.Where.ToArray
            // (не работает из-за лямбд, если переместить в ConvertAfterUsedModulesCompilation)
            var ldv = new ListDesugarVisitor(root);
            ldv.ProcessNode(root);
            root = ldv.UpdatedRoot();

            // дешугаризация составных сравнительных операций (e.g. a == b == c)
            var ccdv = new CompoundComparisonDesugarVisitor();
            ccdv.ProcessNode(root);

            // замена типов из SPython на типы из PascalABC.NET
            var tcv = new TypeCorrectVisitorForIntellisense();
            tcv.ProcessNode(root);

            // вынос forward объявлений для всех функций в начало
            var afdv = new AddForwardDeclarationsVisitor();
            afdv.ProcessNode(root);

            return root;
        }

        public override syntax_tree_node ConvertAfterUsedModulesCompilation(syntax_tree_node root, in CompilationArtifactsUsedBySyntaxConverters compilationArtifacts)
        {
            // проверка корректности имён, разрешение неоднозначности
            // сохранение множества переменных, использующихся как глобальные в ncv.variablesUsedAsGlobal
            var ncv = new NameCorrectVisitor(compilationArtifacts.NamesFromUsedUnits);
            ncv.ProcessNode(root);

            // выносит объявлений переменных из ncv.variablesUsedAsGlobal на глобальный уровень
            // (в модулях все переменные, объявленные на глобальном уровне являются глобальными)
            var rugvv = new RetainUsedGlobalVariablesVisitor();
            rugvv.variablesUsedAsGlobal = ncv.variablesUsedAsGlobal;
            rugvv.ProcessNode(root);

            // замена вызова функций с именованными параметрами на вызов метода класса
            var fwnpdv = new FunctionsWithNamedParametersDesugarVisitor();
            fwnpdv.ProcessNode(root);

            // удаление специфичных синтаксических узлов Spython'a перед конвертацией в семантическое дерево
            // var esonv = new EraseSpythonOnlyNodesVisitor();
            // esonv.ProcessNode(root);

            // перестроение структуры дерева, для последующих этапов компиляции
            // итоговое представление:
            // 1) forward объявления функций
            // 2) объявления глобальных переменных (без присваиваний)
            // 3) объявление функции %%MAIN%%, содержащей компилируемую программу
            // 4) объявления функций, объявленных в программе
            // 5) begin %%MAIN%%() end.
            var tnrv = new TreeNodesRearrangementVisitor();
            tnrv.ProcessNode(root);

            return root;
        }
    }
}
