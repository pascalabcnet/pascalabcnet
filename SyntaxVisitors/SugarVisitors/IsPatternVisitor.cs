using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConversion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SyntaxVisitors.SugarVisitors
{
    public class IsPatternVisitor : BaseChangeVisitor
    {
        public static IsPatternVisitor New => new IsPatternVisitor();

        public override void visit(is_pattern_expr isPatternExpr)
        {
            if (isPatternExpr.right is type_pattern)
                DesugarTypePattern(isPatternExpr);
        }

        private void DesugarTypePattern(is_pattern_expr isPatternExpr)
        {
            Debug.Assert(isPatternExpr.right is type_pattern);

            // Замена is_pattern на вызов вспомогательной функции PABCSystem.IsTest 
            expression expression = isPatternExpr.left;
            type_pattern pattern = (type_pattern) isPatternExpr.right;
            var isTestFunc = SubtreeCreator.CreateSystemFunctionCall("IsTest", expression, pattern.identifier);
            ReplaceUsingParent(isPatternExpr, isTestFunc);

            // Объявление переменной в ближайшем statement_list
            for (int i = listNodes.Count - 1; i >= 0; i--)
            {
                var statements = listNodes[i] as statement_list;
                statements?.InsertBefore(
                    listNodes[i + 1] as statement, 
                    new var_statement(pattern.identifier, pattern.type));
            }
        }
    }
}
