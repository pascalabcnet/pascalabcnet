using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConversion;
using PascalABCCompiler.TreeConverter;

namespace SyntaxVisitors.SugarVisitors
{
    // Patterns

    public class DeconstructionDesugaringResult
    {
        /// <summary>
        /// Переменная, имеющая тип паттерна
        /// </summary>
        public var_statement CastVariableDefinition { get; set; }

        /// <summary>
        /// Переменная, в которую сохраняется результат мэтчинга
        /// </summary>
        public var_statement SuccessVariableDefinition { get; set; }

        /// <summary>
        /// Переменные, полученные в результате деконструкции
        /// </summary>
        public List<var_def_statement> DeconstructionVariables { get; private set; } = new List<var_def_statement>();

        /// <summary>
        /// Проверка соответствия типа выражения типу паттерна
        /// </summary>
        public expression TypeCastCheck { get; set; }

        /// <summary>
        /// Вызов Deconstruct
        /// </summary>
        public statement DeconstructCall { get; set; }

        public ident CastVariable => CastVariableDefinition.var_def.vars.list.First();

        public ident SuccessVariable => SuccessVariableDefinition.var_def.vars.list.First();

        public List<statement> GetDeconstructionDefinitions(SourceContext patternContext)
        {
            var result = new List<statement>();
            result.Add(CastVariableDefinition);
            result.Add(new desugared_deconstruction(DeconstructionVariables, CastVariable, patternContext));
            result.Add(SuccessVariableDefinition);

            return result;
        }

        public if_node GetPatternCheckWithDeconstrunctorCall()
        {
            return SubtreeCreator.CreateIf(
                TypeCastCheck,
                new statement_list(new assign(SuccessVariable.name, true), DeconstructCall));
        }
    }

    public class CollectionDesugaringResult
    {
        public List<statement> VarParametersDeclarations { get; set; } = new List<statement>();

        public expression SuccessMatchingCheck { get; set; }

        public List<statement> VarParametersAssignments { get; set; } = new List<statement>();
    }

    public class PatternsDesugaringVisitor : BaseChangeVisitor
    {
        private enum PatternLocation { Unknown, IfCondition, Assign }

        private const string DeconstructMethodName = compiler_string_consts.deconstruct_method_name;
        private const string IsTestMethodName = compiler_string_consts.is_test_function_name;
        private const string WildCardsTupleEqualFunctionName = compiler_string_consts.wild_cards_tuple_equal_function_name;
        private const string SeqFunctionName = compiler_string_consts.seq_function_name;
        private const string CountPropertyName = compiler_string_consts.count_property_name;
        private const string GeneratedPatternNamePrefix = "<>pattern";
        private const string GeneratedDeconstructParamPrefix = "<>deconstructParam";

        private int generalVariableCounter = 0;
        private int successVariableCounter = 0;
        private int labelVariableCounter = 0;
        private int deconstructParamVariableCounter = 0;

        private if_node _previousIf;
        private statement desugaredMatchWith;
        private List<if_node> processedIfNodes = new List<if_node>();

        //const matching
        private List<statement> typeChecks = new List<statement>();

        public static PatternsDesugaringVisitor New => new PatternsDesugaringVisitor();

        public override void visit(match_with matchWith)
        {
            desugaredMatchWith = null;
            _previousIf = null;

            // Кэшируем выражение для однократного вычисления
            //var cachedExpression = NewGeneralName();
            //AddDefinitionsInUpperStatementList(matchWith, new[] { new var_statement(cachedExpression, matchWith.expr) });

            // Преобразование из сахара в известную конструкцию каждого case
            var usedDeconstructionTypes = new HashSet<string>();
            foreach (var patternCase in matchWith.case_list.elements)
            {
                if (patternCase == null)
                    continue;

                switch (patternCase.pattern)
                {
                    case deconstructor_pattern pattern:
                        {
                            // Проверяем встречался ли уже такой тип при деконструкции
                            // SSM 02.01.19 пока закомментировал этот кусок т.к. при этом коде падает стандартный пример ArithmSimplify.cs. #1408 снова открыл
                            /*var deconstructionType = (patternCase.pattern as deconstructor_pattern).
                                type as named_type_reference;
                            if (deconstructionType != null &&
                                deconstructionType.names != null &&
                                deconstructionType.names.Count != 0)
                            {
                                var deconstructionTypeName = deconstructionType.names[0].name;
                                if (usedDeconstructionTypes.Contains(deconstructionTypeName))
                                {
                                    throw new SyntaxVisitorError("REPEATED_DECONSTRUCTION_TYPE",
                                                                 patternCase.pattern.source_context);
                                }
                                usedDeconstructionTypes.Add(deconstructionTypeName);
                            } */

                            DesugarDeconstructorPatternCase(matchWith.expr, patternCase);
                            break;
                        }
                    case const_pattern p:
                        {
                            DesugarConstPatternCase(matchWith.expr, patternCase);
                            break;
                        }
                    case collection_pattern p:
                        {
                            DesugarDeconstructorPatternCase(matchWith.expr, patternCase);
                            break;
                        }
                }
            }

            if (matchWith.defaultAction != null)
                AddDefaultCase(matchWith.defaultAction as statement_list);

            if (desugaredMatchWith == null)
                desugaredMatchWith = new empty_statement();

            if (typeChecks.Count != 0)
            {
                typeChecks.Add(desugaredMatchWith);
                desugaredMatchWith = new statement_list(typeChecks);
            }
            // Замена выражения match with на новое несахарное поддерево и его обход
            ReplaceUsingParent(matchWith, desugaredMatchWith);
            visit(desugaredMatchWith);
        }

        public override void visit(is_pattern_expr isPatternExpr)
        {
            if (GetLocation(isPatternExpr) == PatternLocation.Unknown)
                throw new SyntaxVisitorError("PATTERN_MATHING_IS_NOT_SUPPORTED_IN_THIS_CONTEXT", isPatternExpr.source_context);

            Debug.Assert(GetAscendant<statement_list>(isPatternExpr) != null, "Couldn't find statement list in upper nodes");

            DesugarIsExpression(isPatternExpr);
        }

        private void DesugarDeconstructorPatternCase(expression matchingExpression, pattern_case patternCase)
        {
            //var paramCheckExpr = DesugarDeconstructorPatternParameters(patternCase.pattern as deconstructor_pattern);

            var isExpression = new is_pattern_expr(matchingExpression, patternCase.pattern, patternCase.source_context);
            var ifCondition = patternCase.condition == null ? (expression)isExpression : bin_expr.LogicalAnd(isExpression, patternCase.condition);
            var ifCheck = SubtreeCreator.CreateIf(ifCondition, patternCase.case_action);

            // Добавляем полученные statements в результат
            AddDesugaredCaseToResult(ifCheck, ifCheck);
        }

        private expression DesugarDeconstructorPatternParameters(deconstructor_pattern pattern)
        {
            expression paramCheckExpr = null;

            for (int i = 0; i < pattern.parameters.Count; ++i)
            {
                if (pattern.parameters[i] is const_pattern_parameter constPattern)
                {
                    var constParamIdent = new ident(NewDeconstructParamId(), pattern.parameters[i].source_context);

                    var eqParams = new expression_list(
                        new List<expression>()
                        {
                            constPattern.const_param,
                            constParamIdent
                        }
                    );

                    var constParamCheck = new method_call(
                        new dot_node(new ident("object"), new ident("Equals")),
                        eqParams,
                        pattern.source_context
                    );

                    pattern.parameters[i] = new var_deconstructor_parameter(
                        constParamIdent,
                        null,
                        pattern.parameters[i].source_context);

                    paramCheckExpr = paramCheckExpr == null ? (expression)constParamCheck : bin_expr.LogicalAnd(paramCheckExpr, constParamCheck);
                }

                if (pattern.parameters[i] is wild_card_deconstructor_parameter)
                {
                    var wildCardGeneratedParamIdent = new ident(NewDeconstructParamId(), pattern.parameters[i].source_context);
                    pattern.parameters[i] = new var_deconstructor_parameter(
                        wildCardGeneratedParamIdent,
                        null,
                        pattern.parameters[i].source_context);
                }

                if (pattern.parameters[i] is recursive_deconstructor_parameter deconstructor_param)
                {
                    if (deconstructor_param.pattern is deconstructor_pattern deconstructor_pattern)
                    {
                        var recursiveChecks = DesugarDeconstructorPatternParameters(deconstructor_pattern);
                        paramCheckExpr = paramCheckExpr == null ?
                                         recursiveChecks :
                                         bin_expr.LogicalAnd(paramCheckExpr, recursiveChecks);
                    }
                }
            }
            return paramCheckExpr;
        }

        private void DesugarConstPatternCase(expression matchingExpression, pattern_case patternCase)
        {
            Debug.Assert(patternCase.pattern is const_pattern);
            var patternExpressionNode = patternCase.pattern as const_pattern;

            var statementsToAdd = new List<statement>();
            var equalCalls = new List<method_call>();

            var matchingWithFullWildCardTuple = false;
            foreach (var patternExpression in patternExpressionNode.pattern_expressions.expressions)
            {
                statementsToAdd.Add(GetTypeCompatibilityCheck(matchingExpression, patternExpression));

                // Matching tuples
                var possibleTupleCase = patternExpression as method_call;
                if (possibleTupleCase != null)
                {
                    var possibleTupleDotNode = (possibleTupleCase.dereferencing_value as dot_node);
                    if (possibleTupleDotNode != null)
                    {
                        var possibleTupleCallString = possibleTupleDotNode.ToString();
                        if (possibleTupleCallString.Contains("System.Tuple.Create"))
                        {
                            var elemsToCompare = new List<expression>();
                            for (int currentInd = 0; currentInd < possibleTupleCase.parameters.Count; ++currentInd)
                            {
                                var tupleElem = possibleTupleCase.parameters.expressions[currentInd];
                                if (!(tupleElem is tuple_wild_card))
                                {
                                    elemsToCompare.Add(currentInd);
                                }
                                else
                                {
                                    possibleTupleCase.parameters[currentInd] = new int32_const(0, patternCase.source_context);
                                }
                            }
                            matchingWithFullWildCardTuple = elemsToCompare.Count == 0;
                            if (matchingWithFullWildCardTuple)
                            {
                                break;
                            }
                            var seqCall = SubtreeCreator.CreateSystemFunctionCall(
                                SeqFunctionName, elemsToCompare.ToArray());

                            var tupleEqualsCall = SubtreeCreator.
                                CreateSystemFunctionCall(
                                    WildCardsTupleEqualFunctionName,
                                    possibleTupleCase,
                                    matchingExpression,
                                    seqCall
                                    );
                            equalCalls.Add(tupleEqualsCall);
                            continue;
                        }
                    }
                }

                // Matching not tuples
                var eqParams = new expression_list(
                    new List<expression>()
                    {
                    matchingExpression,
                    patternExpression
                    }
                );
                equalCalls.Add(
                    new method_call(
                        new dot_node(new ident("object"), new ident("Equals")),
                        eqParams,
                        patternCase.source_context
                    )
                );
            }
            typeChecks.AddRange(statementsToAdd);

            expression orPatternCases = null;
            // Если в одном из выражений есть тапл со всеми wild-card'ами (напр. (?, ?, ?))
            if (matchingWithFullWildCardTuple)
            {
                orPatternCases = new bool_const(true, patternCase.source_context);
            }
            else
            {
                orPatternCases = equalCalls[0];
                for (int i = 1; i < equalCalls.Count; ++i)
                {
                    orPatternCases = bin_expr.LogicalOr(orPatternCases, equalCalls[i]);
                }
            }
            var ifCondition = patternCase.condition == null ? orPatternCases : bin_expr.LogicalAnd(orPatternCases, patternCase.condition);
            var ifCheck = SubtreeCreator.CreateIf(ifCondition, patternCase.case_action);

            // Добавляем полученные statements в результат
            AddDesugaredCaseToResult(ifCheck, ifCheck);
        }

        private expression GetCollectionItemsEqualCheckBeforeGap(addressed_value matchingExpression, 
                                                                 List<pattern_parameter> toCompare, 
                                                                 CollectionDesugaringResult desugaringResult)
        {
            var fromIndex = 0;
            expression equalChecks = null;
            foreach (var param in toCompare)
            {
                if (param is const_pattern_parameter constParam)
                {
                    var indexerCall = new indexer(
                        matchingExpression,
                        new expression_list(
                            new int32_const(fromIndex, matchingExpression.source_context),
                            matchingExpression.source_context),
                        matchingExpression.source_context);

                    var eqParams = new expression_list(
                        new List<expression>()
                        {
                            indexerCall,
                            constParam.const_param
                        }
                    );

                    var equalCall = new method_call(
                        new dot_node(
                            new ident("object"),
                            new ident("Equals")),
                        eqParams,
                        matchingExpression.source_context
                    );

                    equalChecks = equalChecks == null ? (expression)equalCall : bin_expr.LogicalAnd(equalChecks, equalCall);
                }

                if (param is collection_pattern_var_parameter varParam)
                {
                    desugaringResult.VarParametersDeclarations.Add(
                        new var_statement(varParam.identifier, 
                        GetIndexerCallForCollectionPattern(matchingExpression as addressed_value, fromIndex)));
                }
                ++fromIndex;
            }
            return equalChecks;
        }

        private expression GetCollectionItemsEqualCheckAfterGap(addressed_value matchingExpression,
                                                                 List<pattern_parameter> toCompare,
                                                                 CollectionDesugaringResult desugaringResult)
        {
            var elemFromTail = 1;
            expression equalChecks = null;
            foreach (var param in toCompare)
            {
                var indexerCall = new indexer(
                        matchingExpression,
                        new expression_list(
                            new bin_expr(
                                new method_call(
                                    new dot_node(
                                        matchingExpression,
                                        new ident("Count", matchingExpression.source_context)),
                                    new expression_list()),
                                new int32_const(elemFromTail, matchingExpression.source_context),
                                Operators.Minus),
                            matchingExpression.source_context),
                        matchingExpression.source_context);

                if (param is const_pattern_parameter constParam)
                {
                    var eqParams = new expression_list(
                        new List<expression>()
                        {
                        indexerCall,
                        constParam.const_param
                        }
                    );

                    var equalCall = new method_call(
                        new dot_node(
                            new ident("object"),
                            new ident("Equals")),
                        eqParams,
                        matchingExpression.source_context
                    );

                    equalChecks = equalChecks == null ? (expression)equalCall : bin_expr.LogicalAnd(equalChecks, equalCall);
                }

                if (param is collection_pattern_var_parameter varParam)
                {
                    desugaringResult.VarParametersDeclarations.Add(
                        new var_statement(varParam.identifier, indexerCall));
                }
                ++elemFromTail;
            }
            return equalChecks;
        }

        private expression GetIndexerCallForCollectionPattern(addressed_value matchingExpression, int ind)
        {
            var indexerCall = new indexer(matchingExpression, new int32_const(ind), matchingExpression.source_context);
            return indexerCall;
        }

        private ident NewGeneralName() => new ident(GeneratedPatternNamePrefix + "GenVar" + generalVariableCounter++);

        private ident NewSuccessName() => new ident(GeneratedPatternNamePrefix + "Success" + successVariableCounter++);

        private ident NewEndIfName() => new ident(GeneratedPatternNamePrefix + "EndIf" + labelVariableCounter++);

        private bool IsGenerated(string name) => name.StartsWith(GeneratedPatternNamePrefix);

        private void AddDefaultCase(statement_list statements)
        {
            AddDesugaredCaseToResult(statements, _previousIf);
        }

        private void AddDesugaredCaseToResult(statement desugaredCase, if_node newIf)
        {
            // Если результат пустой, значит это первый case
            if (desugaredMatchWith == null)
                desugaredMatchWith = desugaredCase;
            else
            {
                _previousIf.else_body = desugaredCase;
                _previousIf.FillParentsInDirectChilds();
            }

            // Запоминаем только что сгенерированный if
            _previousIf = newIf;
        }

        private DeconstructionDesugaringResult DesugarDeconstructorPattern(deconstructor_pattern pattern, expression matchingExpression)
        {
            Debug.Assert(!pattern.IsRecursive, "All recursive patterns should be desugared into simple patterns at this point");

            var desugarResult = new DeconstructionDesugaringResult();
            var castVariableName = NewGeneralName();
            desugarResult.CastVariableDefinition = new var_statement(castVariableName, pattern.type);

            var successVariableName = NewSuccessName();
            desugarResult.SuccessVariableDefinition = new var_statement(successVariableName, new ident("false"));

            // делегирование проверки паттерна функции IsTest
            desugarResult.TypeCastCheck = SubtreeCreator.CreateSystemFunctionCall(IsTestMethodName, matchingExpression, castVariableName);

            var parameters = pattern.parameters.Cast<var_deconstructor_parameter>();
            foreach (var deconstructedVariable in parameters)
            {
                desugarResult.DeconstructionVariables.Add(
                    new var_def_statement(deconstructedVariable.identifier, deconstructedVariable.type));
            }

            var deconstructCall = new procedure_call();
            deconstructCall.func_name = SubtreeCreator.CreateMethodCall(DeconstructMethodName, castVariableName.name, parameters.Select(x => x.identifier).ToArray());
            desugarResult.DeconstructCall = deconstructCall;

            return desugarResult;
        }

        private CollectionDesugaringResult DesugarCollectionPattern(collection_pattern pattern, expression matchingExpression)
        {
            Debug.Assert(!pattern.IsRecursive, "All recursive patterns should be desugared into simple patterns at this point");

            var desugaringResult = new CollectionDesugaringResult();
            var collectionItems = pattern.parameters;
            var gapItemMet = false;
            var gapIndex = 0;
            var exprBeforeGap = new List<pattern_parameter>();
            var exprAfterGap = new List<pattern_parameter>();
            for (int i = 0; i < collectionItems.Count; ++i)
            {
                if (collectionItems[i] is collection_pattern_gap_parameter)
                {
                    if (gapItemMet)
                    {
                        throw new SyntaxVisitorError("REPEATED_DOTDOT_COLLECTION_PATTERN_EXPR",
                                                     pattern.source_context);
                    }
                    gapItemMet = true;
                    gapIndex = i;
                    continue;
                }

                if (gapItemMet)
                {
                    exprAfterGap.Insert(0, collectionItems[i]);
                }
                else
                {
                    exprBeforeGap.Add(collectionItems[i]);
                }
            }

            var successMatchingCheck = GetCollectionItemsEqualCheckBeforeGap(
                matchingExpression as addressed_value, exprBeforeGap, desugaringResult);

            if (gapItemMet && exprAfterGap.Count != 0)
            {
                var afterGapEqual = GetCollectionItemsEqualCheckAfterGap(
                    matchingExpression as addressed_value, exprAfterGap, desugaringResult);

                if (afterGapEqual != null)
                {
                    successMatchingCheck = successMatchingCheck == null ?
                                           afterGapEqual :
                                           bin_expr.LogicalAnd(successMatchingCheck, afterGapEqual);
                }
            }
            
            var collectionLengthCheck = new bin_expr(
                new dot_node(matchingExpression as addressed_value, new ident(CountPropertyName), pattern.source_context),
                new int32_const(exprBeforeGap.Count + exprAfterGap.Count),
                Operators.GreaterEqual,
                pattern.source_context
            );

            successMatchingCheck = successMatchingCheck == null ?
                                   collectionLengthCheck :
                                   bin_expr.LogicalAnd(collectionLengthCheck, successMatchingCheck);

            desugaringResult.SuccessMatchingCheck = successMatchingCheck == null ?
                                                    new bool_const(true) :
                                                    successMatchingCheck;
            return desugaringResult;
        }

        private void DesugarIsExpression(is_pattern_expr isPatternExpr)
        {
            if (isPatternExpr.right.IsRecursive)
            {
                var desugaredRecursiveIs = DesugarRecursiveDeconstructor(isPatternExpr.left, isPatternExpr.right);
                ReplaceUsingParent(isPatternExpr, desugaredRecursiveIs);
                desugaredRecursiveIs.visit(this);
                return;
            }
            var patternLocation = GetLocation(isPatternExpr);

            if (isPatternExpr.right is deconstructor_pattern pattern)
            {
                var constParamCheck = DesugarDeconstructorPatternParameters(isPatternExpr.right as deconstructor_pattern);
                pattern.const_params_check = constParamCheck;
            }
            //AddDefinitionsInUpperStatementList(isPatternExpr, new[] { GetTypeCompatibilityCheck(isPatternExpr) });
            switch (patternLocation)
            {
                case PatternLocation.IfCondition: DesugarIsExpressionInIfCondition(isPatternExpr); break;
                case PatternLocation.Assign: DesugarIsExpressionInAssignment(isPatternExpr); break;
            }
        }

        private expression DesugarRecursiveDeconstructor(expression expression, pattern_node pattern)
        {
            List<pattern_parameter> parameters = pattern.parameters;
            expression conjunction = new is_pattern_expr(expression, pattern, pattern.source_context);
            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] is recursive_deconstructor_parameter parameter)
                {
                    //var parameterType = (parameter.pattern as deconstructor_pattern).type;
                    var newName = NewGeneralName();
                    var varParameter = new var_deconstructor_parameter(newName, null);
                    parameters[i] = varParameter;
                    varParameter.Parent = parameters[i];
                    conjunction = bin_expr.LogicalAnd(conjunction, DesugarRecursiveDeconstructor(newName, parameter.pattern));
                }
            }
            return conjunction;
        }

        private void DesugarIsExpressionInAssignment(is_pattern_expr isExpression)
        {
            Debug.Assert(isExpression.right is deconstructor_pattern, "Only deconstructor patterns can be used in assignment");
            var statementsToAdd = ProcessDesugaringForDeconstructorPattern(isExpression);
            AddDefinitionsInUpperStatementList(isExpression, statementsToAdd);
        }

        private void DesugarIsExpressionInIfCondition(is_pattern_expr isExpression)
        {
            List<statement> statementsToAdd = null;

            switch (isExpression.right)
            {
                case deconstructor_pattern dp:
                    if (dp.const_params_check != null)
                    {
                        var ifToAddConstParamsCheckTo = GetAscendant<if_node>(isExpression);
                        ifToAddConstParamsCheckTo.condition = bin_expr.LogicalAnd(ifToAddConstParamsCheckTo.condition, dp.const_params_check);
                    }
                    statementsToAdd = ProcessDesugaringForDeconstructorPattern(isExpression);
                    break;
                case collection_pattern cp:
                    statementsToAdd = ProcessDesugaringForCollectionPattern(isExpression);
                    break;
            }

            var enclosingIf = GetAscendant<if_node>(isExpression);
            // Если уже обрабатывался ранее (второй встретившийся в том же условии is), то не изменяем if
            if (processedIfNodes.Contains(enclosingIf))
                AddDefinitionsInUpperStatementList(isExpression, statementsToAdd);
            // Иначе помещаем определения и if-then в отдельный блок, а else после этого блока
            else
            {
                // Сохраняем родителя, так как он поменяется при вызове ConvertIfNode
                var ifParent = enclosingIf.Parent;
                statement elseBody;
                var convertedIf = ConvertIfNode(enclosingIf, statementsToAdd, out elseBody);
                ifParent.ReplaceDescendantUnsafe(enclosingIf, convertedIf);
                convertedIf.Parent = ifParent;

                elseBody?.visit(this);
            }
        }

        private List<statement> ProcessDesugaringForDeconstructorPattern(is_pattern_expr isExpression)
        {
            var pattern = isExpression.right as deconstructor_pattern;
            var desugaringResult = DesugarDeconstructorPattern(pattern, isExpression.left);
            ReplaceUsingParent(isExpression, desugaringResult.SuccessVariable);
            var statementsToAdd = desugaringResult.GetDeconstructionDefinitions(pattern.source_context);
            statementsToAdd.Add(GetMatchedExpressionCheck(isExpression.left));
            statementsToAdd.Add(GetTypeCompatibilityCheck(isExpression));
            statementsToAdd.Add(desugaringResult.GetPatternCheckWithDeconstrunctorCall());

            return statementsToAdd;
        }

        private List<statement> ProcessDesugaringForCollectionPattern(is_pattern_expr isExpression)
        {
            var pattern = isExpression.right as collection_pattern;
            var desugaringResult = DesugarCollectionPattern(pattern, isExpression.left);
            ReplaceUsingParent(isExpression, desugaringResult.SuccessMatchingCheck);

            var statementsToAdd = new List<statement>();
            statementsToAdd.AddRange(desugaringResult.VarParametersDeclarations);
            //statementsToAdd.AddRange(desugaringResult.VarParametersAssignments);

            return statementsToAdd;
        }

        private semantic_check_sugared_statement_node GetMatchedExpressionCheck(expression matchedExpression)
        => new semantic_check_sugared_statement_node(SemanticCheckType.MatchedExpression, new List<syntax_tree_node>() { matchedExpression });

        private semantic_check_sugared_statement_node GetTypeCompatibilityCheck(is_pattern_expr expression) =>
            new semantic_check_sugared_statement_node(SemanticCheckType.MatchedExpressionAndType, new List<syntax_tree_node>() { expression.left, (expression.right as deconstructor_pattern).type });

        private semantic_check_sugared_statement_node GetTypeCompatibilityCheck(expression expression1, expression expression2) =>
            new semantic_check_sugared_statement_node(SemanticCheckType.MatchedExpressionAndExpression, new List<syntax_tree_node>() { expression1, expression2 });

        private statement_list ConvertIfNode(if_node ifNode, List<statement> statementsBeforeIf, out statement elseBody)
        {
            // if e then <then> else <else>
            //
            // переводим в
            //
            // begin
            //   statementsBeforeIf
            //   if e then begin <then>; goto end_if end;
            // end
            // <else>
            // end_if: empty_statement

            // if e then <then>
            // 
            // переводим в
            //
            // begin
            //   statementsBeforeIf
            //   if e then <then>
            // end

            // Добавляем, чтобы на конвертировать еще раз, если потребуется
            processedIfNodes.Add(ifNode);

            var statementsBeforeAndIf = new statement_list();
            statementsBeforeAndIf.AddMany(statementsBeforeIf);
            statementsBeforeAndIf.Add(ifNode);

            if (ifNode.else_body == null)
            {
                elseBody = null;
                return statementsBeforeAndIf;
            }
            else
            {
                var result = new statement_list();
                result.Add(statementsBeforeAndIf);
                var endIfLabel = NewEndIfName();
                // добавляем метку
                if (!(ifNode.then_body is statement_list))
                {
                    ifNode.then_body = new statement_list(ifNode.then_body, ifNode.then_body.source_context);
                    ifNode.then_body.Parent = ifNode;
                }

                var thenBody = ifNode.then_body as statement_list;
                thenBody.Add(new goto_statement(endIfLabel));
                // добавляем else и метку за ним
                result.Add(ifNode.else_body);
                result.Add(new labeled_statement(endIfLabel));
                // Возвращаем else для обхода, т.к. он уже не входит в if
                elseBody = ifNode.else_body;
                // удаляем else из if
                ifNode.else_body = null;
                // Добавляем метку
                AddLabel(endIfLabel);

                return result;
            }
        }

        private void AddLabel(ident label)
        {
            var block = listNodes.OfType<block>().Last();

            if (block.defs == null)
                block.defs = new declarations();

            block.defs.AddFirst(new label_definitions(label));
        }

        private void AddDefinitionsInUpperStatementList(syntax_tree_node currentNode, IEnumerable<statement> statementsToAdd)
        {
            var definitionsAdded = false;
            var ascendants = currentNode.AscendantNodes(true).ToArray();

            // Объявление переменной в ближайшем statement_list
            for (int i = 0; i < ascendants.Length; i++)
            {
                if (ascendants[i] is statement_list statements)
                {
                    statements.InsertBefore(
                        ascendants[i - 1] as statement,
                        statementsToAdd);

                    definitionsAdded = true;
                    break;
                }
            }

            Debug.Assert(definitionsAdded, "Couldn't add definitions");
        }

        private PatternLocation GetLocation(syntax_tree_node node)
        {
            var firstStatement = GetAscendant<statement>(node);

            switch (firstStatement)
            {
                case if_node _: return PatternLocation.IfCondition;
                case var_statement _: return PatternLocation.Assign;
                case assign _: return PatternLocation.Assign;
                default: return PatternLocation.Unknown;
            }
        }

        private T GetAscendant<T>(syntax_tree_node node)
            where T : syntax_tree_node
        {
            var current = node.Parent;
            while (current != null)
            {
                if (current is T res)
                    return res;

                current = current.Parent;
            }

            return null;
        }

        private string NewDeconstructParamId()
        {
            return GeneratedPatternNamePrefix + "DeconstructParam" + deconstructParamVariableCounter++.ToString();
        }
    }
}

