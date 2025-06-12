// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;
using SyntaxVisitors.SugarVisitors;
using SyntaxVisitors.CheckingVisitors;
using SyntaxVisitors.PatternsVisitors;
using PascalABCCompiler.SyntaxTreeConverters;

namespace Languages.Pascal.Frontend.Converters
{
    public class StandardSyntaxTreeConverter : BaseSyntaxTreeConverter
    {
        public override string Name { get; } = "Standard";

        protected override IPipelineVisitor[] VisitorsForConvert => new IPipelineVisitor[]
        {
            new ExitParamVisitor(),

            new BindCollectLightSymInfo(),

            // new ABCStatisticsVisitor(),

            // SSM 02.01.24
            // new LetExprVisitor(),

            // new range - до всего! До выноса выражения с лямбдой из foreach. 11.07 добавил поиск yields и присваивание pd.HasYield
            new NewRangeDesugarAndFindHasYieldVisitor(),

            // Распаковка параметров в лямбдах
            new UnpackLambdaParametersVisitor(),

            // Unnamed Records перенёс сюда
            new UnnamedRecordsCheckVisitor(),

            // Выносим выражения с лямбдами из заголовка foreach + считаем максимум 10 вложенных лямбд
            new StandOutExprWithLambdaInForeachSequenceAndNestedLambdasVisitor(),

            new VarNamesInMethodsWithSameNameAsClassGenericParamsReplacer(),

            new FindOnExceptVarsAndApplyRenameVisitor(),

            // loop
            new LoopDesugarVisitor(),

            // new SimplePrettyPrinterVisitor("D:/out.txt"),

            // optimize tuple assign - true
            new TupleVisitor(optimize_tup_opt: true),

            // index
            new IndexVisitor(),

            // slice_expr и slice_expr_question
            // поставил раньше AssignTuplesDesugarVisitor из за var (a,b) := a[1:3];
            new SliceDesugarVisitor(),

            // теперь коллизия с (a[1:6], a[6:11]):= (a[6:11], a[1:6]);
            // assign_tuple и assign_var_tuple
            // if (!optimize_tuple_assign)
            // AssignTuplesDesugarVisitor.New.ProcessNode(root); // теперь это - на семантике
            // else // следующий визитор

            new NewAssignTuplesDesugarVisitor(),

            // question_point_desugar_visitor
            new QuestionPointDesugarVisitor(),

            // double_question_desugar_visitor
            new DoubleQuestionDesugarVisitor(),

            // Patterns
            // new SingleDeconstructChecker(), // SSM 21.10.18 - пока разрешил множественные деконструкторы. Если будут проблемы - запретить

            // Десахаризация расширенного is, который используется в сложных логических выражениях
            new ExtendedIsDesugaringVisitor(),

            // Обязательно в этом порядке.
            new PatternsDesugaringVisitor(),

            // new SimplePrettyPrinterVisitor("D:/out.txt"),

            // new TestAssignIsDefVisitor(),

            // simple_property
            new PropertyDesugarVisitor(),

            // Всё, связанное с yield
            // CapturedNamesHelper.Reset(); // вызывается внутри следующего визитора
            new MarkMethodHasYieldAndCheckSomeErrorsVisitor(),
            new ProcessYieldCapturedVarsVisitor(),

            new CacheFunctionVisitor(),

            new ToExprVisitor(),

            // При наличии файла lightpt.dat подключает модули LightPT и Tasks
            new TeacherControlConverter(),

            // new SimplePrettyPrinterVisitor("D:\\Tree.txt"),

            // new FillParentNodeVisitor(),

            // new CollectLightSymInfoVisitor(),
            // cv.Output(@"Light1.txt");

            // new SimplePrettyPrinterVisitor(@"d:\\zzz1.txt")
        };
    }
}
