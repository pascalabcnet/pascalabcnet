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
        
        protected override syntax_tree_node ApplyConcreteConversions(syntax_tree_node root)
        {
            root.FillParentsInAllChilds();

            ExitParamVisitor.New.ProcessNode(root);

            var binder = new BindCollectLightSymInfo(root as compilation_unit);
#if DEBUG
            //            var stat = new ABCStatisticsVisitor();
            //            stat.ProcessNode(root);
#endif
            // SSM 02.01.24
            //LetExprVisitor.New.ProcessNode(root);

            // new range - до всего! До выноса выражения с лямбдой из foreach. 11.07 добавил поиск yields и присваивание pd.HasYield
            NewRangeDesugarAndFindHasYieldVisitor.New.ProcessNode(root);

            // Распаковка параметров в лямбдах
            UnpackLambdaParametersVisitor.New.ProcessNode(root);

            // Unnamed Records перенёс сюда
            UnnamedRecordsCheckVisitor.New.ProcessNode(root);

            // Выносим выражения с лямбдами из заголовка foreach + считаем максимум 10 вложенных лямбд
            StandOutExprWithLambdaInForeachSequenceAndNestedLambdasVisitor.New.ProcessNode(root);
            new VarNamesInMethodsWithSameNameAsClassGenericParamsReplacer(root as compilation_unit).ProcessNode(root); 
            FindOnExceptVarsAndApplyRenameVisitor.New.ProcessNode(root);

            // loop
            LoopDesugarVisitor.New.ProcessNode(root);
#if DEBUG
            //new SimplePrettyPrinterVisitor("D:/out.txt").ProcessNode(root);
#endif
            bool optimize_tuple_assign = true;
            // tuple_node
            TupleVisitor.Create(optimize_tuple_assign).ProcessNode(root);

            // index 
            IndexVisitor.New.ProcessNode(root);

            // slice_expr и slice_expr_question
            SliceDesugarVisitor.New.ProcessNode(root);
            // поставил раньше AssignTuplesDesugarVisitor из за var (a,b) := a[1:3];

            // теперь коллизия с (a[1:6], a[6:11]):= (a[6:11], a[1:6]);
            // assign_tuple и assign_var_tuple
            if (!optimize_tuple_assign)
                AssignTuplesDesugarVisitor.New.ProcessNode(root); // теперь это - на семантике
            else 
                NewAssignTuplesDesugarVisitor.Create(binder).ProcessNode(root);

            // question_point_desugar_visitor
            QuestionPointDesugarVisitor.New.ProcessNode(root);

            // double_question_desugar_visitor
            DoubleQuestionDesugarVisitor.New.ProcessNode(root);

            // Patterns
            // SingleDeconstructChecker.New.ProcessNode(root); // SSM 21.10.18 - пока разрешил множественные деконструкторы. Если будут проблемы - запретить
            ExtendedIsDesugaringVisitor.New.ProcessNode(root); // Десахаризация расширенного is, который используется в сложных логических выражениях
            PatternsDesugaringVisitor.New.ProcessNode(root);  // Обязательно в этом порядке.
#if DEBUG
            //new SimplePrettyPrinterVisitor("D:/out.txt").ProcessNode(root);
            // TestAssignIsDefVisitor.New.ProcessNode(root);
#endif

            // simple_property
            PropertyDesugarVisitor.New.ProcessNode(root);

            // Всё, связанное с yield
            CapturedNamesHelper.Reset();
            MarkMethodHasYieldAndCheckSomeErrorsVisitor.New.ProcessNode(root);
            ProcessYieldCapturedVarsVisitor.New.ProcessNode(root);

            CacheFunctionVisitor.New.ProcessNode(root);

            ToExprVisitor.New.ProcessNode(root);

            // При наличии файла lightpt.dat подключает модули LightPT и Tasks
            root = TeacherControlConverter.New.Convert(root);

#if DEBUG
            //new SimplePrettyPrinterVisitor("D:\\Tree.txt").ProcessNode(root);
            //FillParentNodeVisitor.New.ProcessNode(root);


            /*var cv = CollectLightSymInfoVisitor.New;
            cv.ProcessNode(root);
            cv.Output(@"Light1.txt");*/

            /*try
            {
                root.visit(new SimplePrettyPrinterVisitor(@"d:\\zzz1.txt"));
            }
            catch(Exception e)
            {

                System.IO.File.AppendAllText(@"d:\\zzz1.txt",e.Message);
            }*/


#endif
            return root;
        }
    }
}
