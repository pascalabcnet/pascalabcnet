using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConversion;

namespace SyntaxVisitors.SugarVisitors
{
    public class MatchWithVisitor : BaseChangeVisitor
    {
        private int variableCounter = 0;

        public static MatchWithVisitor New => new MatchWithVisitor();

        public override void visit(match_with matchWith)
        {
            var statements = new statement_list();

            foreach (var patternCase in matchWith.case_list.elements)
            {
                if (patternCase.pattern is type_pattern)
                    statements.AddMany(DesugarTypePatternCase(matchWith.expr, patternCase));
            }

            ReplaceUsingParent(matchWith, statements);
            visit(statements);
        }

        List<statement> DesugarTypePatternCase(expression matchingExpression, pattern_case patternCase)
        {
            Debug.Assert(patternCase.pattern is type_pattern);

            var result = new List<statement>();
            var pattern = (type_pattern) patternCase.pattern;
            var generatedVariable = GenerateIdent();
            result.Add(new var_statement(generatedVariable, pattern.type));
            var isTest = SubtreeCreator.CreateSystemFunctionCall("IsTest", matchingExpression, generatedVariable);
            var patternVarDef = new var_statement(pattern.identifier, generatedVariable);
            result.Add(SubtreeCreator.CreateIf(isTest, new statement_list(patternVarDef, patternCase.case_action)));

            return result;
        }

        private ident GenerateIdent()
        {
            return  new ident("gen" + variableCounter++);
        }
    }
}
