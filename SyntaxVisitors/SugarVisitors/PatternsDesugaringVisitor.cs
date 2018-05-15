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
        /// Список объявлений, которые должны быть включены внутрь особых конструкций, для ограничения области видимости.
        /// Например внутрь тела then, если паттерн объявлен в условии if.
        /// </summary>
        public List<var_statement> InternalAssignments { get; private set; } = new List<var_statement>();

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
            var result = DeconstructionVariables
                  .Select(x => new desugared_deconstruction(DeconstructionVariables, CastVariable, patternContext))
                  .ToList<statement>();
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

    public class PatternsDesugaringVisitor : BaseChangeVisitor
    {
        private enum PatternLocation { Unknown, IfCondition, Assign }

        private const string DeconstructMethodName = compiler_string_consts.deconstruct_method_name;
        private const string IsTestMethodName = compiler_string_consts.is_test_function_name;
        private const string GeneratedPatternNamePrefix = "<>pattern";

        private int _variableCounter = 0;
        private if_node _previousIf;
        private statement desugaredMatchWith;

        public static PatternsDesugaringVisitor New => new PatternsDesugaringVisitor();

        public override void visit(match_with matchWith)
        {
            desugaredMatchWith = null;
            _previousIf = null;

            // Преобразование из сахара в известную конструкцию каждого case
            foreach (var patternCase in matchWith.case_list.elements)
            {
                if (patternCase == null)
                    continue;

                if (patternCase.pattern is deconstructor_pattern)
                    // TODO Patterns: introduce a variable for expression and cache it
                    DesugarDeconstructorPatternCase(matchWith.expr, patternCase);
            }

            if (matchWith.defaultAction != null)
                AddDefaultCase(matchWith.defaultAction as statement_list);

            if (desugaredMatchWith == null)
                desugaredMatchWith = new empty_statement();

            // Замена выражения match with на новое несахарное поддерево и его обход
            ReplaceUsingParent(matchWith, desugaredMatchWith);
            visit(desugaredMatchWith);
        }

        public override void visit(is_pattern_expr isPatternExpr)
        {
            if (isPatternExpr.right is type_pattern)
                DesugarIsExpression(isPatternExpr);
        }

        void DesugarDeconstructorPatternCase(expression matchingExpression, pattern_case patternCase)
        {
            Debug.Assert(patternCase.pattern is deconstructor_pattern);

            //var result = new statement_list();
            //var pattern = patternCase.pattern as deconstructor_pattern;
            //// создание фиктивной переменной необходимого типа
            //var desugaredPattern = DesugarPattern(pattern, matchingExpression);
            //result.Add(desugaredPattern.CastVariableDefinition);

            

            //// Создание тела if из объявлений переменных, вызова Deconstruct и соответствующего тела case
            //var ifBody = new statement_list();
            //ifBody.Add(new desugared_deconstruction(desugaredPattern.DeconstructionVariables, desugaredPattern.CastVariable, patternCase.pattern.source_context));
            //ifBody.Add(desugaredPattern.DeconstructCall);

            //if (patternCase.condition != null)
            //{
            //    // Выполняем тело, если выполняется условие when
            //    ifBody.Add(SubtreeCreator.CreateIf(patternCase.condition, patternCase.case_action));
            //}
            //else
            //    ifBody.Add(patternCase.case_action);

            //var ifCondition = desugaredPattern.TypeCastCheck;
            //var ifCheck = SubtreeCreator.CreateIf(ifCondition, ifBody);

            //result.Add(ifCheck);

            //// Добавляем полученные statements в результат
            //AddDesugaredCaseToResult(result, ifCheck);
        }

        private ident NewGeneralName()
        {
            return new ident(GeneratedPatternNamePrefix + "GenVar" + _variableCounter++);
        }

        private ident NewSuccessName()
        {
            return new ident(GeneratedPatternNamePrefix + "Success" + _variableCounter++);
        }

        private bool IsGenerated(string name) => name.StartsWith(GeneratedPatternNamePrefix);

        private void AddDefaultCase(statement_list statements)
        {
            AddDesugaredCaseToResult(statements, _previousIf);
        }

        private void AddDesugaredCaseToResult(statement_list statements, if_node newIf)
        {
            // Если результат пустой, значит это первый case
            if (desugaredMatchWith == null)
                desugaredMatchWith = statements;
            else
            {
                _previousIf.else_body = statements;
                _previousIf.FillParentsInDirectChilds();
            }

            // Запоминаем только что сгенерированный if
            _previousIf = newIf;
        }

        private DeconstructionDesugaringResult DesugarPattern(deconstructor_pattern pattern, expression matchingExpression)
        {
            Debug.Assert(!pattern.IsRecursive, "All recursive patterns should be desugared into simple patterns at this point");

            var desugarResult = new DeconstructionDesugaringResult();
            var castVariableName = NewGeneralName();
            desugarResult.CastVariableDefinition = new var_statement(castVariableName, pattern.type);

            var successVariableName = NewSuccessName();
            desugarResult.SuccessVariableDefinition = new var_statement(successVariableName, named_type_reference.Boolean);

            // делегирование проверки паттерна функции IsTest
            desugarResult.TypeCastCheck = SubtreeCreator.CreateSystemFunctionCall(IsTestMethodName, matchingExpression, castVariableName);

            var parameters = pattern.parameters.Cast<var_deconstructor_parameter>();
            foreach (var deconstructedVariable in parameters)
            {
                if (IsGenerated(deconstructedVariable.identifier.name))
                    desugarResult.DeconstructionVariables.Add(
                        new var_def_statement(deconstructedVariable.identifier, deconstructedVariable.type));
                else
                {
                    var newVariable = NewGeneralName();
                    desugarResult.DeconstructionVariables.Add(
                        new var_def_statement(newVariable, deconstructedVariable.type));
                    desugarResult.InternalAssignments.Add(
                        new var_statement(deconstructedVariable.identifier, newVariable));
                }
            }

            var deconstructCall = new procedure_call();
            deconstructCall.func_name = SubtreeCreator.CreateMethodCall(DeconstructMethodName, castVariableName.name, parameters.Select(x => x.identifier).ToArray());
            desugarResult.DeconstructCall = deconstructCall;

            return desugarResult;
        }

        private void DesugarIsExpression(is_pattern_expr isPatternExpr)
        {
            Debug.Assert(isPatternExpr.right is deconstructor_pattern);

            var pattern = isPatternExpr.right as deconstructor_pattern;

            var desugaringResult = DesugarPattern(pattern, isPatternExpr.left);

            var statementsToAdd = desugaringResult.GetDeconstructionDefinitions(pattern.source_context);
            statementsToAdd.Add(desugaringResult.GetPatternCheckWithDeconstrunctorCall());
            AddDefinitionsInUpperStatementList(statementsToAdd);

            ReplaceUsingParent(isPatternExpr, desugaringResult.SuccessVariable);

            
        }

        private void AddDefinitionsInUpperStatementList(IEnumerable<statement> statementsToAdd)
        {
            var definitionsAdded = false;

            // Объявление переменной в ближайшем statement_list
            for (int i = listNodes.Count - 1; i >= 0; i--)
            {
                var statements = listNodes[i] as statement_list;
                if (statements != null)
                {
                    statements.InsertBefore(
                        listNodes[i + 1] as statement,
                        statementsToAdd);
                    
                    definitionsAdded = true;
                    break;
                }
            }

            // TODO Patterns: convert to compilation error
            Debug.Assert(definitionsAdded, "Couldn't add definitions");
        }

        private PatternLocation GetCurrentLocation()
        {
            var firstStatement = GetAscendant<statement>();
        }

        private T GetAscendant<T>()
            where T : syntax_tree_node
        {
            for (int i = listNodes.Count - 1; i >= 0; i--)
            {
                if (listNodes[i] is T)
                    return (T)listNodes[i];
            }

            return null;
        }
    }
}

