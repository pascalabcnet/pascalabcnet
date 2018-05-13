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

        public ident CastVariable
        {
            get
            {
                return CastVariableDefinition.var_def.vars.list[0];
            }
        }
    }

    public class PatternsDesugaringVisitor : BaseChangeVisitor
    {
        private const string DeconstructMethodName = compiler_string_consts.deconstruct_method_name;

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
                DesugarTypePattern(isPatternExpr);
        }

        void DesugarTypePatternCase(expression matchingExpression, pattern_case patternCase)
        {
            Debug.Assert(patternCase.pattern is type_pattern);

            //var result = new statement_list();
            //// создание фиктивной переменной необходимого типа
            //var pattern = (type_pattern) patternCase.pattern;

            //// Конструируем условие мэтчинга 
            //expression matchCondition = isTest;
            //if (patternCase.condition == null)
            //    matchCondition = isTest;
            //else
            //// Если есть секция when - добавляем ее в условие
            //{
            //    // Переименовываем идентификатор, т.к. мы сгенерировали внутреннее имя
            //    patternCase.condition.RenameIdentifierInDescendants(pattern.identifier.name, matchResultVariableName.name);
            //    matchCondition = new bin_expr(isTest, patternCase.condition, Operators.LogicalAND);
            //}

            //// Создание узла if с объявлением переменной из паттерна и присвоение ей значения 
            //// сгенерированной переменной
            //var patternVarDef = new var_statement(pattern.identifier, matchResultVariableName);
            //// Создание тела if из объявления переменной и соттветствующего тела case
            //var ifCheck = SubtreeCreator.CreateIf(matchCondition, new statement_list(patternVarDef, patternCase.case_action));
            //result.Add(ifCheck);

            //// Добавляем полученные statements в результат
            //AddDesugaredCaseToResult(result, ifCheck);
        }

        void DesugarDeconstructorPatternCase(expression matchingExpression, pattern_case patternCase)
        {
            Debug.Assert(patternCase.pattern is deconstructor_pattern);

            var result = new statement_list();
            var pattern = patternCase.pattern as deconstructor_pattern;
            // создание фиктивной переменной необходимого типа
            var desugaredPattern = DesugarPattern(pattern, matchingExpression);
            result.Add(desugaredPattern.CastVariableDefinition);

            

            // Создание тела if из объявлений переменных, вызова Deconstruct и соответствующего тела case
            var ifBody = new statement_list();
            ifBody.Add(new desugared_deconstruction(desugaredPattern.DeconstructionVariables, desugaredPattern.CastVariable, patternCase.pattern.source_context));
            ifBody.Add(desugaredPattern.DeconstructCall);

            if (patternCase.condition != null)
            {
                // Выполняем тело, если выполняется условие when
                ifBody.Add(SubtreeCreator.CreateIf(patternCase.condition, patternCase.case_action));
            }
            else
                ifBody.Add(patternCase.case_action);

            var ifCondition = desugaredPattern.TypeCastCheck;
            var ifCheck = SubtreeCreator.CreateIf(ifCondition, ifBody);

            result.Add(ifCheck);

            // Добавляем полученные statements в результат
            AddDesugaredCaseToResult(result, ifCheck);
        }

        private ident GenerateIdent()
        {
            return new ident("<>patternGenVar" + _variableCounter++);
        }

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
            //var desugarResult = new DeconstructionDesugaringResult();
            //var castVariableName = GenerateIdent();
            //desugarResult.CastVariableDefinition = new var_statement(castVariableName, pattern.type);

            //// делегирование проверки паттерна функции IsTest
            //desugarResult.TypeCastCheck = SubtreeCreator.CreateSystemFunctionCall("IsTest", matchingExpression, castVariableName);

            //foreach (var deconstructedVariable in pattern.parameters)
            //    desugarResult.DeconstructionVariables.Add(
            //        new var_def_statement(deconstructedVariable.identifier, deconstructedVariable.type));

            //var deconstructCall = new procedure_call();
            //deconstructCall.func_name = SubtreeCreator.CreateMethodCall(DeconstructMethodName, castVariableName.name, pattern.parameters.Select(x => x.identifier).ToArray());
            //desugarResult.DeconstructCall = deconstructCall;

            //return desugarResult;

            return null;
        }

        private void DesugarTypePattern(is_pattern_expr isPatternExpr)
        {
            Debug.Assert(isPatternExpr.right is type_pattern);

            // Замена is_pattern на вызов вспомогательной функции PABCSystem.IsTest 
            expression expression = isPatternExpr.left;
            type_pattern pattern = (type_pattern)isPatternExpr.right;
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

