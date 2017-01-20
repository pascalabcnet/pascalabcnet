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

            AssignTuplesDesugarVisitor.New.ProcessNode(root);

            // Пока не доделали
            //SliceQuestionDesugarVisitor.New.ProcessNode(root);

            // Всё, связанное с yield
            root.visit(new MarkMethodHasYieldAndCheckSomeErrorsVisitor());
            ProcessYieldCapturedVarsVisitor.New.ProcessNode(root);


            return root;
        }
    }
}
