// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
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
                {
                    var ptc = type_table.get_convertions(resultExpressionsTypes[i].Item1, mostCommonType);
                    if (ptc.first == null)
                        syntaxTreeVisitor.AddError(new CanNotConvertTypes(resultExpressionsTypes[i].Item3, resultExpressionsTypes[i].Item1, mostCommonType, syntaxTreeVisitor.get_location(resultExpressionsTypes[i].Item2)));
                }
                
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

        public override void visit(PascalABCCompiler.SyntaxTree.for_node fn)
        {
            syntaxTreeVisitor.visit(fn);
        }

        public override void visit(PascalABCCompiler.SyntaxTree.foreach_stmt fn)
        {
            syntaxTreeVisitor.visit(fn);
        }

        /*public override void visit(PascalABCCompiler.SyntaxTree.exception_block fn)
        {
            syntaxTreeVisitor.visit(fn);
        }*/
        // Patterns
        public override void visit(desugared_deconstruction _desugared_deconstruction)
        {
            // позволяем вывести типы объявленных переменных
            syntaxTreeVisitor.visit(_desugared_deconstruction);
        }
        // !Patterns

        public override void visit(assign assignment)
        {
            var to = assignment.to as ident;
            var from = assignment.from;
            if (to != null &&
                assignment.operator_type == Operators.Assignment &&
                to.name.Equals(PascalABCCompiler.StringConstants.result_variable_name, SemanticRulesConstants.SymbolTableCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
            {
                var converted = syntaxTreeVisitor.convert_strong(from);
                if (converted is typed_expression)
                {
                    delegated_methods dm = converted.type as delegated_methods;
                    if (dm.empty_param_method != null)
                        converted = syntaxTreeVisitor.convert_typed_expression_to_function_call(converted as typed_expression);
                }
                resultExpressionsTypes.Add(new Tuple<type_node, expression, expression_node>(converted.type, from, converted));
                var si_list = syntaxTreeVisitor.context.find(PascalABCCompiler.StringConstants.result_variable_name);
                if (si_list != null && si_list.Count > 0 && si_list[0].sym_info == null)
                {
                    si_list[0].sym_info = new local_variable(PascalABCCompiler.StringConstants.result_variable_name, converted.type, syntaxTreeVisitor.context.top_function, null);
                }
            }
        }

        public override void visit(function_lambda_definition funcLamDef)
        {
        }

        public override void visit(semantic_check_sugared_statement_node st)
        {
            // Это единственная семантическая проверка в сахарной конструкции где порождаются новые переменные
            // В остальных случаях семантические проверки в этом визиторе пропускаются
            if (st.typ as System.Type == typeof(assign_var_tuple))
            {
                var idents = st.lst[0] as ident_list;
                var expr = st.lst[1] as expression;
                syntaxTreeVisitor.semantic_check_assign_var_tuple(idents, expr);
            }
        }
    }
}