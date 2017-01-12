using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTreeConverters
{
    public class StandardSyntaxTreeConverter: ISyntaxTreeConverter
    {
        public string Name { get; } = "Standard";
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            // Прошивание ссылками на Parent nodes. Должно идти первым
            FillParentNodeVisitor.New.ProcessNode(root);

            // Выносим выражения с лямбдами из заголовка foreach
            StandOutExprWithLambdaInForeachSequenceVisitor.New.ProcessNode(root); 

            return root;
        }
    }
}
