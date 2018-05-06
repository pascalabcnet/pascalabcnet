using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConversion;

namespace SyntaxVisitors.SugarVisitors
{
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

    public class MatchWithVisitor : BaseChangeVisitor
    {
        private const string DeconstructMethodName = "Deconstruct";

        private int _variableCounter = 0;
        private if_node _previousIf;
        private statement_list desugaredMatchWith;

        public static MatchWithVisitor New => new MatchWithVisitor();

        public override void visit(match_with matchWith)
        {
            desugaredMatchWith = null;
            _previousIf = null;

            // Преобразование из сахара в известную конструкцию каждого case
            foreach (var patternCase in matchWith.case_list.elements)
            {
                if (patternCase.pattern is type_pattern)
                    DesugarTypePatternCase(matchWith.expr, patternCase);

                if (patternCase.pattern is deconstructor_pattern)
                    // TODO: introduce a variable for expression and cache it
                    DesugarDeconstructorPatternCase(matchWith.expr, patternCase);
            }

            if (desugaredMatchWith == null)
                return;

            // Замена выражения match with на новое несахарное поддерево и его обход
            ReplaceUsingParent(matchWith, desugaredMatchWith);
            visit(desugaredMatchWith);
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

            if (patternCase.condition != null)
            // Если есть секция when - добавляем ее в условие
            {
                // TODO: generate check for 'when'
            }

            // Создание тела if из объявлений переменных, вызова Deconstruct и соответствующего тела case
            var ifBody = new statement_list();
            ifBody.Add(new desugared_deconstruction(desugaredPattern.DeconstructionVariables, desugaredPattern.CastVariable));
            ifBody.Add(desugaredPattern.DeconstructCall);
            ifBody.Add(patternCase.case_action);
            var ifCheck = SubtreeCreator.CreateIf(desugaredPattern.TypeCastCheck, ifBody);
            
            result.Add(ifCheck);

            // Добавляем полученные statements в результат
            AddDesugaredCaseToResult(result, ifCheck);
        }

        private ident GenerateIdent()
        {
            return  new ident("<>patternGenVar" + _variableCounter++);
        }

        private void AddDesugaredCaseToResult(statement_list statements, if_node newIf)
        {
            // Если результат пустой, значит это первый case
            if (desugaredMatchWith == null)
                desugaredMatchWith = statements;
            else
                _previousIf.else_body = statements;

            // Запоминаем только что сгенерированный if
            _previousIf = newIf;
        }

        private DeconstructionDesugaringResult DesugarPattern(deconstructor_pattern pattern, expression matchingExpression)
        {
            var desugarResult = new DeconstructionDesugaringResult();
            var castVariableName = GenerateIdent();
            desugarResult.CastVariableDefinition = new var_statement(castVariableName, pattern.type);
            
            // делегирование проверки паттерна функции IsTest
            desugarResult.TypeCastCheck = SubtreeCreator.CreateSystemFunctionCall("IsTest", matchingExpression, castVariableName);

            foreach (var deconstructedVariable in pattern.parameters)
                desugarResult.DeconstructionVariables.Add(
                    new var_def_statement(deconstructedVariable.identifier, deconstructedVariable.type));

            var deconstructCall = new procedure_call();
            deconstructCall.func_name = SubtreeCreator.CreateMethodCall(DeconstructMethodName, castVariableName.name, pattern.parameters.Select(x => x.identifier).ToArray());
            desugarResult.DeconstructCall = deconstructCall;

            return desugarResult;
        }
    }
}
