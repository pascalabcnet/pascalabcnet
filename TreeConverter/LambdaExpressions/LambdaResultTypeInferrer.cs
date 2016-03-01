// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;

namespace TreeConverter.LambdaExpressions
{
    internal class LambdaResultTypeInferrer : WalkingVisitorNew
    {
        private const string RESULT_KEY_WORD = "result";
        private readonly List<Tuple<type_node, expression, expression_node>> resultExpressionsTypes;
        private readonly syntax_tree_visitor syntaxTreeVisitor;
        private readonly proc_block lambdaBody;
        private readonly function_header lambdaHeader;

        public LambdaResultTypeInferrer(function_header lambdaHeader, proc_block lambdaBody, syntax_tree_visitor syntaxTreeVisitor)
        {
            this.lambdaBody = lambdaBody;
            this.lambdaHeader = lambdaHeader;
            this.syntaxTreeVisitor = syntaxTreeVisitor;
            resultExpressionsTypes = new List<Tuple<type_node, expression, expression_node>>();
        }

        private type_node GetMostCommonType(int kind = 0)
        {
            if (resultExpressionsTypes.Count == 0)
                if (kind==0)
                    syntaxTreeVisitor.AddError(syntaxTreeVisitor.get_location(lambdaHeader), "IMPOSSIBLE_TO_INFER_RESULT_TYPE_IN_LAMBDA");
                else syntaxTreeVisitor.AddError(syntaxTreeVisitor.get_location(lambdaHeader), "IMPOSSIBLE_TO_INFER_RESULT_TYPE");

            var mostCommonType = resultExpressionsTypes[0].Item1;
            for (var i = 1; i < resultExpressionsTypes.Count; i++)
            {
                if (convertion_data_and_alghoritms.eq_type_nodes(mostCommonType, resultExpressionsTypes[i].Item1))
                    continue;
                var typeComparisonResult = type_table.compare_types(resultExpressionsTypes[i].Item1, mostCommonType);
                if (typeComparisonResult == type_compare.non_comparable_type)
                    syntaxTreeVisitor.AddError(new CanNotConvertTypes(resultExpressionsTypes[i].Item3, resultExpressionsTypes[i].Item1, mostCommonType, syntaxTreeVisitor.get_location(resultExpressionsTypes[i].Item2)));
                if (typeComparisonResult == type_compare.greater_type)
                    mostCommonType = resultExpressionsTypes[i].Item1;
            }

            return mostCommonType;
        }

        public type_node InferResultType(int kind = 0)
        {
            ProcessNode(lambdaBody);
            return GetMostCommonType(kind);
        }

        public override void visit(statement_list stmtList)
        {
            var stl = new statements_list(syntaxTreeVisitor.get_location(stmtList),
                                          syntaxTreeVisitor.get_location_with_check(stmtList.left_logical_bracket),
                                          syntaxTreeVisitor.get_location_with_check(stmtList.right_logical_bracket));
            syntaxTreeVisitor.convertion_data_and_alghoritms.statement_list_stack_push(stl);
            if (stmtList.subnodes != null)
                foreach (var stmt in stmtList.subnodes)
                    ProcessNode(stmt);
            syntaxTreeVisitor.convertion_data_and_alghoritms.statement_list_stack.pop();    
        }

        public override void visit(var_def_statement varStmt)
        {
            syntaxTreeVisitor.visit(varStmt);
        }

        public override void visit(assign assignment)
        {
            var to = assignment.to as ident;
            var from = assignment.from;
            if (to != null &&
                assignment.operator_type == Operators.Assignment &&
                to.name.ToLower() == RESULT_KEY_WORD)
            {
                var converted = syntaxTreeVisitor.convert_strong(from);
                resultExpressionsTypes.Add(new Tuple<type_node, expression, expression_node>(converted.type, from, converted));
            }
        }

        public override void visit(function_lambda_definition funcLamDef)
        {
        }
    }
}