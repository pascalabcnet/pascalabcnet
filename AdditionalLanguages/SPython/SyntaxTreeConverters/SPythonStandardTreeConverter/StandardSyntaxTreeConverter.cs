using Languages.Pascal.Frontend.Converters;
using PascalABCCompiler;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;
using SyntaxVisitors;
using SyntaxVisitors.SugarVisitors;
using System;

namespace Languages.SPython.Frontend.Converters
{
    public class StandardSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name { get; } = "Standard";

        protected override syntax_tree_node ApplyConversions(syntax_tree_node root, bool forIntellisense)
        {
            // кидает ошибки за использование
            // неподдерживаемых конструкций языка
            if (!forIntellisense)
                new UnsupportedConstructsVisitor().ProcessNode(root);

            // добавление всех import ..., from ... import ...
            // как uses в начале программы для сбора имён из этих модулей
            if (!forIntellisense)
                new ImportToUsesVisitor().ProcessNode(root);

            // замена
            // name bin_bit_op= expr
            // на
            // name = name bin_bit_op (expr)
            new TryCatchDecorator(new BitwiseAssignmentDesugarVisitor(), forIntellisense).ProcessNode(root);

            // замена return;       на { exit(); }
            // замена return expr;  на { result := expr; exit(); }
            new TryCatchDecorator(new ReturnDesugarVisitor(), forIntellisense).ProcessNode(root);

            // замена вызова функции map(func, sequence)
            // на func(element) for element in sequence
            new TryCatchDecorator(new MapDesugarVisitor(), forIntellisense).ProcessNode(root);

            // замена генерации последовательностей на Select.Where
            // (не работает из-за лямбд (скорее всего), если переместить в ConvertAfterUsedModulesCompilation)
            new TryCatchDecorator(new GeneratorObjectDesugarVisitor(root), forIntellisense).ProcessNode(root);

            // Выносим выражения с лямбдами из заголовка foreach + считаем максимум 10 вложенных лямбд
            // украл из паскаля
            StandOutExprWithLambdaInForeachSequenceAndNestedLambdasVisitor.New.ProcessNode(root);
            new VarNamesInMethodsWithSameNameAsClassGenericParamsReplacer(root as compilation_unit).ProcessNode(root);
            FindOnExceptVarsAndApplyRenameVisitor.New.ProcessNode(root);

            // дешугаризация составных сравнительных операций (e.g. a == b == c)
            new TryCatchDecorator(new CompoundComparisonDesugarVisitor(), forIntellisense).ProcessNode(root);

            return root;
        }

        public override syntax_tree_node ConvertAfterUsedModulesCompilation(syntax_tree_node root, bool forIntellisense, in CompilationArtifactsUsedBySyntaxConverters compilationArtifacts)
        {
            // украл из паскаля, нужны для работы 'for i1, i2 in expr' (работает с кортежными присваиваниями)
            var binder = new BindCollectLightSymInfo(root as compilation_unit);
            new TryCatchDecorator(binder, forIntellisense).ProcessNode(root);
            new TryCatchDecorator(new NewAssignTuplesDesugarVisitor(binder), forIntellisense).ProcessNode(root);

            // Заменяет 
            // variable_name = single_length_string
            // на
            // variable_name = str(single_length_string)
            // чтобы при выведении типа правильно вывел str, а не char
            new AssignmentCharAsStringVisitor().ProcessNode(root);

            // Сохраняет множество имён функций, которые объявлены в программе для NameCorrectVisitor
            var ffv = new FindFunctionsNamesVisitor();
            
            new TryCatchDecorator(ffv, forIntellisense).ProcessNode(root);

            // проверка корректности имён, разрешение неоднозначности
            // сохранение множества переменных, использующихся как глобальные в ncv.variablesUsedAsGlobal
            var ncv = new NameCorrectVisitor(System.IO.Path.GetFileNameWithoutExtension(((compilation_unit)root).file_name),
                compilationArtifacts.NamesFromUsedUnits, ffv.definedFunctionsNames);

            if (!new TryCatchDecorator(ncv, forIntellisense).ProcessNode(root))
                return root;

            // замена типов из SPython на типы из PascalABC.NET
            new TryCatchDecorator(new TypeCorrectVisitor(forIntellisense), forIntellisense).ProcessNode(root);

            // вынос forward объявлений для всех функций в начало
            new TryCatchDecorator(new AddForwardDeclarationsVisitor(), forIntellisense).ProcessNode(root);

            // выполняет генерацию кода для функций с kwarg-аргументами
            new TryCatchDecorator(new KwargsFunctionDesugarVisitor(), forIntellisense).ProcessNode(root);

            // замена вызова функций с kwarg-аргументами на вызов метода класса
            new TryCatchDecorator(new FunctionsWithNamedParametersDesugarVisitor(), forIntellisense).ProcessNode(root);

            // выносит объявлений переменных из ncv.variablesUsedAsGlobal на глобальный уровень
            // (в модулях все переменные, объявленные на глобальном уровне являются глобальными)
            new TryCatchDecorator(new RetainUsedGlobalVariablesVisitor(ncv.variablesUsedAsGlobal), forIntellisense).ProcessNode(root);

            // удаление специфичных синтаксических узлов Spython'a перед конвертацией в семантическое дерево
            if (!forIntellisense)
                new EraseSpythonOnlyNodesVisitor().ProcessNode(root);

            // перестроение структуры дерева, для последующих этапов компиляции
            // итоговое представление:
            // 1) forward объявления функций
            // 2) объявления глобальных переменных (без присваиваний)
            // 3) объявление функции %%MAIN%%, содержащей компилируемую программу
            // 4) объявления функций, объявленных в программе
            // 5) begin %%MAIN%%() end.
            new TryCatchDecorator(new TreeNodesRearrangementVisitor(), forIntellisense).ProcessNode(root);

            return root;
        }

        private class TryCatchDecorator
        {
            private WalkingVisitorNew internalVisitor;
            private bool forIntellisense;

            public TryCatchDecorator(WalkingVisitorNew internalVisitor, bool forIntellisense)
            {
                this.internalVisitor = internalVisitor;
                this.forIntellisense = forIntellisense;
            }

            public bool ProcessNode(syntax_tree_node root)
            {
                if (forIntellisense)
                {
                    try
                    {
                        internalVisitor.ProcessNode(root);
                    }
                    catch (Exception) 
                    {
                        return false;
                    }
                }
                else
                {
                    internalVisitor.ProcessNode(root);
                }

                return true;
            }
        }
    }
}
