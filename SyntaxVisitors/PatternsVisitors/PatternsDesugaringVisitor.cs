using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.SyntaxTreeConverters;

namespace SyntaxVisitors.PatternsVisitors
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

        public expression CollectionLengthCheck { get; set; }

        public List<semantic_check_sugared_statement_node> ElemTypeChecks { get; set; } = new List<semantic_check_sugared_statement_node>();
    }

    public class TupleDesugaringResult
    {
        public List<statement> VarParametersDeclarations { get; set; } = new List<statement>();

        public expression SuccessMatchingCheck { get; set; }

        public statement TupleLengthCheck { get; set; }

        public List<semantic_check_sugared_statement_node> ElemTypeChecks { get; set; } = new List<semantic_check_sugared_statement_node>();
    }

    public class PatternsDesugaringVisitor : BaseChangeVisitor, IPipelineVisitor
    {
        private enum PatternLocation { Unknown, IfCondition, Assign }

        private const string DeconstructMethodName = "deconstruct";
        private const string IsTestMethodName = "__TypeCheckAndAssignForIsMatch";
        private const string WildCardsTupleEqualFunctionName = "__WildCardsTupleEqual";
        private const string SeqFunctionName = "Seq";
        private const string CountPropertyName = "Count";
        private const string GeneratedPatternNamePrefix = "<>pattern";
        private const string GeneratedDeconstructParamPrefix = "<>deconstructParam";
        private const string GeneratedVisitElseBranchVariableName = "<>visitElseBranch";
        private const string GeneratedMatchExprVariableName = "<>matchExprVariable";

        private int generalVariableCounter = 0;
        private int successVariableCounter = 0;
        private int labelVariableCounter = 0;
        private static int deconstructParamVariableCounter = 0;
        private int matchExprVariableCounter = 0;

        private if_node _previousIf;
        private statement desugaredMatchWith;
        private List<if_node> processedIfNodes = new List<if_node>();
        private var_statement matchedExprVarDeclaration;

        //const matching
        private List<statement> typeChecks = new List<statement>();

        public static PatternsDesugaringVisitor New => new PatternsDesugaringVisitor();

        public void Visit(syntax_tree_node root, VisitorsContext context, Action next)
        {
            ProcessNode(root);

            next();
        }

        public override void visit(match_with matchWith)
        {
            desugaredMatchWith = null;
            _previousIf = null;

            var matchExprVariableName = NewMatchExprVariableId();
            matchedExprVarDeclaration = new var_statement(
                new ident(matchExprVariableName, matchWith.expr.source_context),
                matchWith.expr.Clone() as expression,
                matchWith.expr.source_context);
            ReplaceUsingParent(matchWith.expr, new ident(matchExprVariableName, matchWith.expr.source_context));

            /*if (matchWith.Parent is statement_list stl)
            {
                stl.InsertBefore(matchWith, matchedExprVarDeclaration);
            }
            else
            {
                var enclosingStl = new statement_list(matchedExprVarDeclaration, matchWith);
                enclosingStl.source_context = matchWith.source_context;
                ReplaceUsingParent(matchWith, enclosingStl);
            }*/
            
            // Кэшируем выражение для однократного вычисления
            //var cachedExpression = NewGeneralName();
            //AddDefinitionsInUpperStatementList(matchWith, new[] { new var_statement(cachedExpression, matchWith.expr) });

            // Преобразование из сахара в известную конструкцию каждого case
            var usedDeconstructionTypes = new HashSet<string>();
            foreach (var patternCase in matchWith.case_list.elements)
            {
                if (patternCase == null)
                    continue;
                DefaultDesugarPattern(matchWith.expr, patternCase);

                /*switch (patternCase.pattern)
                {
                    case deconstructor_pattern pattern:
                        {
                            // Проверяем встречался ли уже такой тип при деконструкции
                            // SSM 02.01.19 пока закомментировал этот кусок т.к. при этом коде падает стандартный пример ArithmSimplify.cs. #1408 снова открыл
                            var deconstructionType = (patternCase.pattern as deconstructor_pattern).
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
                            } 

                DefaultDesugarPattern(matchWith.expr, patternCase);
                            break;
                        }
                    case const_pattern p:
                        {
                            //DesugarConstPatternCase(matchWith.expr, patternCase);
                            DefaultDesugarPattern(matchWith.expr, patternCase);
                            break;
                        }
                    case collection_pattern p:
                        {
                            DefaultDesugarPattern(matchWith.expr, patternCase);
                            break;
                        }
                    case tuple_pattern p:
                        {
                            DefaultDesugarPattern(matchWith.expr, patternCase);
                            break;
                        }
                }*/
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

            desugaredMatchWith = new statement_list(matchedExprVarDeclaration, desugaredMatchWith);

            // Замена выражения match with на новое несахарное поддерево и его обход
            ReplaceUsingParent(matchWith, desugaredMatchWith);
            visit(desugaredMatchWith);
            typeChecks.Clear(); // SSM исправление ошибки #2276
        }

        private int num = 0;

        public string GenerateNewName(string name)
        {
            num += 1;
            return "$RenIsVarYield" + num + "$" + name;
        }

        public override void visit(procedure_definition pd)
        {
            //ReplaceNamesInIsVarVisitor.New.ProcessNode(pd);
            base.visit(pd);
        }

        public class StrBool
        {
            public string name;
            public bool needtorename;
            public StrBool(string name, bool b = false)
            {
                this.name = name;
                this.needtorename = b;
            }
        }

        public override void visit(is_pattern_expr isPatternExpr)
        {
            if (GetLocation(isPatternExpr) == PatternLocation.Unknown)
                throw new SyntaxVisitorError("PATTERN_MATHING_IS_NOT_SUPPORTED_IN_THIS_CONTEXT", isPatternExpr.source_context);

            Debug.Assert(GetAscendant<statement_list>(isPatternExpr) != null, "Couldn't find statement list in upper nodes");

            //syntax_tree_node stn = isPatternExpr;
            //while (stn != null && !(stn is procedure_definition) && !(stn is function_lambda_definition))
            //    stn = stn.Parent;
            //if (stn is procedure_definition pd && pd.has_yield) // SSM 11/07/20 Надо сделать обход с переименованием!!! Сейчас and не работает в yield + ... is A(var a1)
            //{
            // SSM 14/07/20
            var dict = new Dictionary<string, StrBool>(StringComparer.InvariantCultureIgnoreCase); // bool - надо ли начинать renaming

            var l = new List<statement>();
            var pp = isPatternExpr.right?.parameters;
            if (pp != null)
            {
                foreach (var p in pp)
                {
                    if (p is var_deconstructor_parameter vdp)
                    {
                        var idClone = vdp.identifier.TypedClone();
                        var idNew = vdp.identifier.TypedClone();
                        idNew.name = GenerateNewName(idNew.name);
                        var vs = new var_statement(idClone, idNew, idClone.source_context);
                        // проверять, что имя не начинается с $ и если начинается, то ошибка - уже переименовывали такое же!
                        if (vdp.identifier.name.StartsWith("$")) // SSM 14/07/20
                            throw new SyntaxVisitorError("Повторное объявление", vdp.identifier.source_context);
                        dict.Add(vdp.identifier.name, new StrBool(idNew.name,false));
                        //vdp.identifier.name = idNew.name; // в следующем цикле переименую
                        l.Add(vs);
                    }
                }
            }

            // SSM 14/07/20
            // подниматься пока выражение
            syntax_tree_node ex = isPatternExpr;
            syntax_tree_node ex1 = ex;
            while (ex is expression)
            {
                ex1 = ex;
                ex = ex.Parent;
            }
            ex = ex1; // саммое внешнее выражение. В нём и переименовать!
            var nams = ex.DescendantNodes().Where(stn => stn is ident).Cast<ident>();
            foreach (var n in nams)
            {
                // надо как-то пробросить до первой is_var
                if (dict.ContainsKey(n.name))
                {
                    var isvar = GetAscendant<is_pattern_expr>(n);
                    if (isvar != null)
                    {
                        dict[n.name].needtorename = true; // всё - можно переименовывать далее
                        n.name = dict[n.name].name; // переименовываем в is_var
                        continue;
                    }
                    if (!dict[n.name].needtorename) // если еще не дошли до is_var, то это какая-то внешняя и не переименовываем
                        continue;
                    if (n.Parent is dot_node dn && dn.left == n) // т.е. не переименовываем только если ident - в правой части dot_node
                    {
                        // переименовываем
                        n.name = dict[n.name].name;
                    }
                    else
                    {
                        // переименовываем
                        n.name = dict[n.name].name;
                    }
                }
            }


            var stat = GetAscendant<statement>(isPatternExpr);
            if (stat is if_node ifn)
            {
                //l.Add(ifn.then_body);
                if (!(ifn.then_body is statement_list))
                {
                    ifn.then_body = new statement_list(ifn.then_body);
                }
                (ifn.then_body as statement_list).list.InsertRange(0, l);
            }
            else
            {
                l.Insert(0, stat);
                foreach (var x in l)
                    x.Parent = stat.Parent;
                var stl = stat.Parent as statement_list;
                if (stl != null)
                    stl.ReplaceInList(stat, l);
                else if (stat is var_statement)
                    throw new SyntaxVisitorError("EMBEDDED_STATEMENT_CANNOT_BE_A_DECLARATION", stat.source_context);
            }
            //}


            //ReplaceStatement(stat, l); // тут нельзя. 

            DesugarIsExpression(isPatternExpr);
        }

        private void DefaultDesugarPattern(expression matchingExpression, pattern_case patternCase)
        {
            //var paramCheckExpr = DesugarDeconstructorPatternParameters(patternCase.pattern as deconstructor_pattern);

            var isExpression = new is_pattern_expr(matchingExpression, patternCase.pattern, patternCase.source_context);
            var ifCondition = patternCase.condition == null ? (expression)isExpression : bin_expr.LogicalAnd(isExpression, patternCase.condition);
            var ifCheck = SubtreeCreator.CreateIf(ifCondition, patternCase.case_action);

            // Добавляем полученные statements в результат
            AddDesugaredCaseToResult(ifCheck, ifCheck);
        }
        
        private type_definition GetTypeDefinitionForConstParam(expression constParamExpr)
        {
            /*switch (constParamExpr)
            {
                case string_const type:
                    return new named_type_reference("string");
                case char_const type:
                    return new named_type_reference("char");
                case int32_const type:
                    return new named_type_reference("integer");
                case int64_const type:
                    return new named_type_reference("integer");
                case double_const type:
                    return new named_type_reference("double");
            }*/
            if (constParamExpr is string_const)
                return new named_type_reference("string");
            if (constParamExpr is char_const)
                return new named_type_reference("char");
            if (constParamExpr is int32_const)
                return new named_type_reference("integer");
            if (constParamExpr is int64_const)
                return new named_type_reference("integer");
            if (constParamExpr is double_const)
                return new named_type_reference("double");
            return null;
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
                        GetTypeDefinitionForConstParam(constPattern.const_param),
                        false,
                        pattern.parameters[i].source_context);

                    paramCheckExpr = paramCheckExpr == null ? (expression)constParamCheck : bin_expr.LogicalAnd(paramCheckExpr, constParamCheck);
                }

                if (pattern.parameters[i] is wild_card_deconstructor_parameter)
                {
                    var wildCardGeneratedParamIdent = new ident(NewDeconstructParamId(), pattern.parameters[i].source_context);
                    pattern.parameters[i] = new var_deconstructor_parameter(
                        wildCardGeneratedParamIdent,
                        null,
                        false,
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
            var equalCalls = new List<bin_expr>();

            foreach (var patternExpression in patternExpressionNode.pattern_expressions.expressions)
            {
                statementsToAdd.Add(GetTypeCompatibilityCheck(matchingExpression, patternExpression));
                
                equalCalls.Add(
                    new bin_expr(matchingExpression, patternExpression, Operators.Equal, patternCase.source_context
                    )
                );
            }
            typeChecks.AddRange(statementsToAdd);

            expression orPatternCases = equalCalls[0];
            for (int i = 1; i < equalCalls.Count; ++i)
            {
                orPatternCases = bin_expr.LogicalOr(orPatternCases, equalCalls[i]);
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
                    desugaringResult.ElemTypeChecks.Add(GetTypeCompatibilityCheck(indexerCall, constParam.const_param));
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
                    desugaringResult.ElemTypeChecks.Add(GetTypeCompatibilityCheck(indexerCall, constParam.const_param));
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

        private ident NewGeneralName(SourceContext sc) => new ident(GeneratedPatternNamePrefix + "GenVar" + generalVariableCounter++, sc);

        private ident NewSuccessName(SourceContext sc) => new ident(GeneratedPatternNamePrefix + "Success" + successVariableCounter++, sc);

        private ident NewEndIfName(SourceContext sc) => new ident(GeneratedPatternNamePrefix + "EndIf" + labelVariableCounter++, sc);

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
            var castVariableName = NewGeneralName(pattern.source_context);
            desugarResult.CastVariableDefinition = new var_statement(castVariableName, pattern.type, pattern.type.source_context);

            var successVariableName = NewSuccessName(matchingExpression.source_context);
            desugarResult.SuccessVariableDefinition = new var_statement(successVariableName, new ident("false"), successVariableName.source_context);

            // делегирование проверки паттерна функции IsTest
            desugarResult.TypeCastCheck = SubtreeCreator.CreateSystemFunctionCall(IsTestMethodName, matchingExpression, castVariableName);

            var parameters = pattern.parameters.Cast<var_deconstructor_parameter>();
            foreach (var deconstructedVariable in parameters)
            {
                desugarResult.DeconstructionVariables.Add(
                    new var_def_statement(deconstructedVariable.identifier, deconstructedVariable.type, deconstructedVariable.identifier.source_context));
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
            desugaringResult.CollectionLengthCheck = new bin_expr(
                new dot_node(matchingExpression as addressed_value, new ident(CountPropertyName), pattern.source_context),
                new int32_const(exprBeforeGap.Count + exprAfterGap.Count),
                Operators.GreaterEqual,
                pattern.source_context
            );
            
            if (!gapItemMet)
            {
                var lengthWithoutGapCheck = new bin_expr(
                    new dot_node(matchingExpression as addressed_value, new ident(CountPropertyName), pattern.source_context),
                    new int32_const(exprBeforeGap.Count),
                    Operators.Equal,
                    pattern.source_context
                );
                successMatchingCheck = successMatchingCheck == null ? 
                                       lengthWithoutGapCheck :
                                       bin_expr.LogicalAnd(lengthWithoutGapCheck, successMatchingCheck);
            }

            desugaringResult.SuccessMatchingCheck = successMatchingCheck == null ?
                                                    new bool_const(true) :
                                                    successMatchingCheck;
            return desugaringResult;
        }

        private TupleDesugaringResult DesugarTuplePattern(tuple_pattern pattern, expression matchingExpression)
        {
            Debug.Assert(!pattern.IsRecursive, "All recursive patterns should be desugared into simple patterns at this point");
            var desugaringResult = new TupleDesugaringResult();
            var tupleItems = pattern.parameters;

            for (int i = 0; i < tupleItems.Count; ++i)
            {
                var tupleItemCall = new dot_node(
                                matchingExpression as addressed_value,
                                new ident("Item" + (i + 1).ToString(), matchingExpression.source_context), // SSM 24/06/20
                                matchingExpression.source_context);
                if (tupleItems[i] is tuple_pattern_var_parameter varParam)
                {
                    desugaringResult.VarParametersDeclarations.Add(
                        new var_statement(
                            varParam.identifier,
                            tupleItemCall,
                            matchingExpression.source_context
                        )
                    );
                }

                if (tupleItems[i] is const_pattern_parameter constParam)
                {
                    var eqParams = new expression_list(
                        new List<expression>()
                        {
                            tupleItemCall,
                            constParam.const_param
                        }
                    );
                    var equalCall = new method_call(
                        new dot_node(new ident("object"), new ident("Equals")),
                        eqParams,
                        pattern.source_context
                    );

                    desugaringResult.SuccessMatchingCheck = desugaringResult.SuccessMatchingCheck == null ?
                                                            (expression)equalCall :
                                                            bin_expr.LogicalAnd(desugaringResult.SuccessMatchingCheck, equalCall);
                    desugaringResult.ElemTypeChecks.Add(GetTypeCompatibilityCheck(tupleItemCall, constParam.const_param));
                }
            }

            desugaringResult.TupleLengthCheck = GetTypeCompatibilityCheck(matchingExpression, new int32_const(tupleItems.Count));
            
            if (desugaringResult.SuccessMatchingCheck == null)
            {
                desugaringResult.SuccessMatchingCheck = new bool_const(true);
            }
            return desugaringResult;
        }

        private void DesugarIsExpression(is_pattern_expr isPatternExpr)
        {
            if (isPatternExpr.right.IsRecursive)
            {
                var desugaredRecursiveIs = DesugarRecursiveParameters(isPatternExpr.left, isPatternExpr.right);
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

        private expression DesugarRecursiveParameters(expression expression, pattern_node pattern)
        {
            List<pattern_parameter> parameters = pattern.parameters;
            expression conjunction = new is_pattern_expr(expression, pattern, pattern.source_context);
            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] is recursive_pattern_parameter parameter)
                {
                    //var parameterType = (parameter.pattern as deconstructor_pattern).type;
                    var newName = NewGeneralName(parameter.source_context);
                    pattern_parameter varParameter = null;
                    if (pattern is deconstructor_pattern)
                    {
                        varParameter = new var_deconstructor_parameter(newName, null, false);
                    }
                    else if (pattern is collection_pattern)
                    {
                        varParameter = new collection_pattern_var_parameter(newName, null);
                    }
                    else if (pattern is tuple_pattern)
                    {
                        varParameter = new tuple_pattern_var_parameter(newName, null);
                    }
                    parameters[i] = varParameter;
                    varParameter.Parent = parameters[i];
                    conjunction = bin_expr.LogicalAnd(conjunction, DesugarRecursiveParameters(newName, parameter.pattern));
                }
            }
            return conjunction;
        }

        private void DesugarIsExpressionInAssignment(is_pattern_expr isExpression)
        {
            if (!(isExpression.right is deconstructor_pattern))
            {
                throw new SyntaxVisitorError("MATCHING_ASSIGN_NOT_SUPPORTED", isExpression.right.source_context);
            }
            var statementsToAdd = ProcessDesugaringForDeconstructorPattern(isExpression);
            AddDefinitionsInUpperStatementList(isExpression, statementsToAdd);
        }

        private void DesugarIsExpressionInIfCondition(is_pattern_expr isExpression)
        {
            List<statement> statementsToAdd = null;

            /*switch (isExpression.right)
            {
                case deconstructor_pattern dp:
                    if (dp.const_params_check != null)
                    {
                        var ifToAddConstParamsCheckTo = GetAscendant<if_node>(isExpression);
                        ifToAddConstParamsCheckTo.condition = 
                            bin_expr.LogicalAnd(ifToAddConstParamsCheckTo.condition, dp.const_params_check);
                    }
                    statementsToAdd = ProcessDesugaringForDeconstructorPattern(isExpression);
                    break;
                case collection_pattern cp:
                    statementsToAdd = ProcessDesugaringForCollectionPattern(isExpression);
                    break;
                case tuple_pattern cp:
                    statementsToAdd = ProcessDesugaringForTuplePattern(isExpression);
                    break;
                case const_pattern cp:
                    statementsToAdd = ProcessDesugaringForConstPattern(isExpression);
                    break;
            }*/
            if (isExpression.right is deconstructor_pattern dp)
            {
                if (dp.const_params_check != null)
                {
                    var ifToAddConstParamsCheckTo = GetAscendant<if_node>(isExpression);
                    ifToAddConstParamsCheckTo.condition =
                        bin_expr.LogicalAnd(ifToAddConstParamsCheckTo.condition, dp.const_params_check);
                }
                statementsToAdd = ProcessDesugaringForDeconstructorPattern(isExpression);
            }
            else if (isExpression.right is collection_pattern)
            {
                statementsToAdd = ProcessDesugaringForCollectionPattern(isExpression);
            }
            else if (isExpression.right is tuple_pattern)
            {
                statementsToAdd = ProcessDesugaringForTuplePattern(isExpression);
            }
            else if (isExpression.right is const_pattern)
            {
                statementsToAdd = ProcessDesugaringForConstPattern(isExpression);
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

        private List<statement> ProcessDesugaringForConstPattern(is_pattern_expr isExpression)
        {
            var patternExpressionNode = isExpression.right as const_pattern;

            var statementsToAdd = new List<statement>();
            var equalCalls = new List<bin_expr>();

            foreach (var patternExpression in patternExpressionNode.pattern_expressions.expressions)
            {
                statementsToAdd.Add(GetTypeCompatibilityCheck(isExpression.left, patternExpression));

                equalCalls.Add(
                    new bin_expr(isExpression.left, patternExpression, Operators.Equal, isExpression.source_context
                    )
                );
            }
            typeChecks.AddRange(statementsToAdd);
            expression orPatternCases = equalCalls[0];
            for (int i = 1; i < equalCalls.Count; ++i)
            {
                orPatternCases = bin_expr.LogicalOr(orPatternCases, equalCalls[i]);
            }

            ReplaceUsingParent(isExpression, orPatternCases);
            return statementsToAdd;
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
            statementsToAdd.AddRange(desugaringResult.ElemTypeChecks);
            return statementsToAdd;
        }

        private List<statement> ProcessDesugaringForTuplePattern(is_pattern_expr isExpression)
        {
            var pattern = isExpression.right as tuple_pattern;
            var desugaringResult = DesugarTuplePattern(pattern, isExpression.left);
            ReplaceUsingParent(isExpression, desugaringResult.SuccessMatchingCheck);

            var statementsToAdd = new List<statement>();
            statementsToAdd.Add(desugaringResult.TupleLengthCheck);
            statementsToAdd.AddRange(desugaringResult.VarParametersDeclarations);
            statementsToAdd.AddRange(desugaringResult.ElemTypeChecks);

            return statementsToAdd;
        }

        private semantic_check_sugared_statement_node GetMatchedExpressionCheck(expression matchedExpression)
        {
            return new semantic_check_sugared_statement_node(SemanticCheckType.MatchedExpression, new List<syntax_tree_node>() { matchedExpression });
        }

        private semantic_check_sugared_statement_node GetTypeCompatibilityCheck(is_pattern_expr expression)
        {
            return new semantic_check_sugared_statement_node(SemanticCheckType.MatchedExpressionAndType, new List<syntax_tree_node>() { expression.left, (expression.right as deconstructor_pattern).type });
        }

        private semantic_check_sugared_statement_node GetTypeCompatibilityCheck(expression expression1, expression expression2)
        {
            return new semantic_check_sugared_statement_node(SemanticCheckType.MatchedExpressionAndExpression, new List<syntax_tree_node>() { expression1, expression2 });
        }

        private semantic_check_sugared_statement_node GetTypeCompatibilityCheck(expression tuple, int32_const length)
        {
            return new semantic_check_sugared_statement_node(SemanticCheckType.MatchedTuple, new List<syntax_tree_node>() { tuple, length });
        }
        private bool IsNestedIfWithExtendedIs(if_node ifNode)
        {
            var parent = ifNode.Parent;
            while (parent != null)
            {
                if (parent is statement_list stList &&
                    IsVisitElseBranchStatementListDeclaration(stList))
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }

        private bool IsVisitElseBranchStatementListDeclaration(statement_list stList)
        {
            return stList.list != null && stList.list[0] != null &&
                   stList.list[0] is var_statement visitElseVarStatement &&
                   visitElseVarStatement.var_def.vars.idents[0].name.Equals(GeneratedVisitElseBranchVariableName);
        }

        private statement_list ConvertIfNode(if_node ifNode, List<statement> statementsBeforeIf, out statement elseBody)
        {
            // if e then <then> else <else>
            //
            // переводим в
            //
            // begin
            // var <>visitElseBranch := true;
            //   begin
            //     <>visitElseBranch := true;
            //     statementsBeforeIf
            //     if e then begin <then>; <>visitElseBranch := false; end;
            //   end
            //   if <>visitElseBranch then <else>
            // end

            // if e then <then>
            // 
            // переводим в
            //
            // begin
            //   statementsBeforeIf
            //   if e then <then>
            // end

            // Добавляем объявление <>visitElseBranch если мы находимся в первом в цепочке if, который не является вложенным
            List<statement> visitElseStatList = null;
            if (ifNode.else_body != null &&
                !(ifNode.Parent is if_node ifParentNode &&
                  ifParentNode.condition is ident ifParentNodeIdent &&
                  ifParentNodeIdent.name.Equals(GeneratedVisitElseBranchVariableName)) &&
                !IsNestedIfWithExtendedIs(ifNode))
            {
                visitElseStatList = new List<statement>();
                visitElseStatList.Add(
                    new var_statement(
                        new ident(GeneratedVisitElseBranchVariableName, ifNode.source_context),
                        new bool_const(true, ifNode.source_context),
                        ifNode.source_context)
                );
            }

            // Добавляем, чтобы на конвертировать еще раз, если потребуется
            processedIfNodes.Add(ifNode);

            if (ifNode.else_body != null)
            {
                statementsBeforeIf.Add(new assign(
                        new ident(GeneratedVisitElseBranchVariableName, ifNode.source_context),
                        new bool_const(true, ifNode.source_context),
                        Operators.Assignment,
                        ifNode.source_context));
            }
            var statementsBeforeAndIf = new statement_list();
            statementsBeforeAndIf.AddMany(statementsBeforeIf);
            statementsBeforeAndIf.Add(ifNode);

            if (ifNode.else_body == null)
            {
                elseBody = null;
                if (visitElseStatList != null)
                {
                    visitElseStatList.Add(statementsBeforeAndIf);
                    statementsBeforeAndIf = new statement_list(visitElseStatList);
                }
                return statementsBeforeAndIf;
            }
            else
            {
                var result = new statement_list();
                result.Add(statementsBeforeAndIf);
                //var endIfLabel = NewEndIfName();

                if (!(ifNode.then_body is statement_list))
                {
                    ifNode.then_body = new statement_list(ifNode.then_body, ifNode.then_body.source_context);
                    ifNode.then_body.Parent = ifNode;
                }

                var thenBody = ifNode.then_body as statement_list;
                
                thenBody.Add(new assign(
                    new ident(GeneratedVisitElseBranchVariableName, thenBody.source_context), 
                    new bool_const(false, thenBody.source_context), 
                    Operators.Assignment,
                    thenBody.source_context));

                // добавляем else
                result.Add(
                    new if_node(
                        new ident(GeneratedVisitElseBranchVariableName, ifNode.else_body.source_context),
                        ifNode.else_body,
                        null,
                        ifNode.else_body.source_context));
                
                // Возвращаем else для обхода, т.к. он уже не входит в if
                elseBody = ifNode.else_body;
                // удаляем else из if
                ifNode.else_body = null;

                if (visitElseStatList != null)
                {
                    visitElseStatList.Add(result);
                    result = new statement_list(visitElseStatList);
                }

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

            /*switch (firstStatement)
            {
                case if_node _: return PatternLocation.IfCondition;
                case var_statement _: return PatternLocation.Assign;
                case assign _: return PatternLocation.Assign;
                default: return PatternLocation.Unknown;
            }*/
            if (firstStatement is if_node)
                return PatternLocation.IfCondition;
            if (firstStatement is var_statement)
                return PatternLocation.Assign;
            if (firstStatement is assign)
                return PatternLocation.Assign;
            return PatternLocation.Unknown;
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

        private string NewMatchExprVariableId()
        {
            return GeneratedMatchExprVariableName + "matchExprVariableCounter" + deconstructParamVariableCounter++.ToString();
        }
    }
}

