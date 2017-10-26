// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using SyntaxVisitors;
using SyntaxVisitors.SugarVisitors;

namespace PascalABCCompiler.SyntaxTreeConverters
{
    public class StandardSyntaxTreeConverter: ISyntaxTreeConverter
    {
        public string Name { get; } = "Standard";
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            // Прошивание ссылками на Parent nodes. Должно идти первым
            // FillParentNodeVisitor расположен в SyntaxTree/tree как базовый визитор, отвечающий за построение дерева
            FillParentNodeVisitor.New.ProcessNode(root);

            // Выносим выражения с лямбдами из заголовка foreach
            StandOutExprWithLambdaInForeachSequenceVisitor.New.ProcessNode(root);

            //--- Обработка синтаксически сахарных узлов

            // loop
            LoopDesugarVisitor.New.ProcessNode(root);

            // tuple_node
            TupleVisitor.New.ProcessNode(root);

            // assign_tuple и assign_var_tuple
            AssignTuplesDesugarVisitor.New.ProcessNode(root);

            // slice_expr и slice_expr_question
            SliceDesugarVisitor.New.ProcessNode(root);

            // question_point_desugar_visitor
            QuestionPointDesugarVisitor.New.ProcessNode(root);

            // double_question_desugar_visitor
            DoubleQuestionDesugarVisitor.New.ProcessNode(root);


            // Всё, связанное с yield
            MarkMethodHasYieldAndCheckSomeErrorsVisitor.New.ProcessNode(root);
            ProcessYieldCapturedVarsVisitor.New.ProcessNode(root);

#if DEBUG
            
            /*var cv = CollectLightSymInfoVisitor.New;
            cv.ProcessNode(root);
            cv.Output(@"d:\\Light1.txt");*/
            
            /*try
            {
                //root.visit(new SimplePrettyPrinterVisitor(@"d:\\zzz4.txt"));
            }
            catch
            {

            }*/

#endif
            return root;
        }
    }
}
