using Languages.Pascal.Frontend.Converters;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using SyntaxVisitors.SugarVisitors;

namespace Languages.SPython.Frontend.Converters
{
    public class StandardSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name { get; } = "Standard";

        protected override IPipelineVisitor[] VisitorsForConvert => new IPipelineVisitor[]
        {
            // кидает ошибки за использование 
            // неподдерживаемых конструкций языка
            new UnsupportedConstructsVisitor(),

            // добавление всех import ..., from ... import ...
            // как uses в начале программы для сбора имён из этих модулей
            new ImportToUsesVisitor(),

            // замена
            // name bin_bit_op= expr
            // на
            // name = name bin_bit_op (expr)
            new BitwiseAssignmentDesugarVisitor(),

            // замена return;       на { exit(); }
            // замена return expr;  на { result := expr; exit(); }
            new ReturnDesugarVisitor(),

            // замена вызова функции map(func, sequence)
            // на func(element) for element in sequence
            new MapDesugarVisitor(),

            // замена генерации последовательностей на Select.Where
            // (не работает из-за лямбд (скорее всего), если переместить в ConvertAfterUsedModulesCompilation)
            new GeneratorObjectDesugarVisitor(),

            // дешугаризация составных сравнительных операций (e.g. a == b == c)
            new CompoundComparisonDesugarVisitor(),
        };

        protected override IPipelineVisitor[] VisitorsForConvertAfterUsedModulesCompilation => new IPipelineVisitor[]
        {
            // Сохраняет множество имён функций, которые объявлены в программе для NameCorrectVisitor
            new FindFunctionsNamesVisitor(),

            // проверка корректности имён, разрешение неоднозначности
            // сохранение множества переменных, использующихся как глобальные в ncv.variablesUsedAsGlobal
            new NameCorrectVisitor(),

            // замена типов из SPython на типы из PascalABC.NET
            new TypeCorrectVisitor(),

            // украл из паскаля, нужны для работы 'for i1, i2 in expr' (работает с кортежными присваиваниями)
            new BindCollectLightSymInfo(),
            new NewAssignTuplesDesugarVisitor(),

            // вынос forward объявлений для всех функций в начало
            new AddForwardDeclarationsVisitor(),

            // выполняет генерацию кода для функций с kwarg-аргументами
            new KwargsFunctionDesugarVisitor(),

            // замена вызова функций с kwarg-аргументами на вызов метода класса
            new FunctionsWithNamedParametersDesugarVisitor(),

            // выносит объявлений переменных из ncv.variablesUsedAsGlobal на глобальный уровень
            // (в модулях все переменные, объявленные на глобальном уровне являются глобальными)
            new RetainUsedGlobalVariablesVisitor(),

            // удаление специфичных синтаксических узлов Spython'a перед конвертацией в семантическое дерево
            new EraseSpythonOnlyNodesVisitor(),

            // перестроение структуры дерева, для последующих этапов компиляции
            // итоговое представление:
            // 1) forward объявления функций
            // 2) объявления глобальных переменных (без присваиваний)
            // 3) объявление функции %%MAIN%%, содержащей компилируемую программу
            // 4) объявления функций, объявленных в программе
            // 5) begin %%MAIN%%() end.
            new TreeNodesRearrangementVisitor()
        };
    }
}
