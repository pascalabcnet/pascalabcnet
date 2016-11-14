using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.SyntaxTreeConverters
{
    class StandardSyntaxTreeConverter: ISyntaxTreeConverter
    {
        public string Name { get; } = "Standard";
        public syntax_tree_node Convert(syntax_tree_node root)
        {
            FillParentNodeVisitor.New.ProcessNode(root); // прошивание ссылками на Parent nodes. Должно идти первым
            StandOutExprWithLambdaInForeachSequenceVisitor.New.ProcessNode(root); // выносим выражения с лямбдами из заголовка foreach

            return root;
        }
    }
}
