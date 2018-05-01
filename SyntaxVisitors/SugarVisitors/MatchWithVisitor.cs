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

            var result = new statement_list();
            // создание фиктивной переменной необходимого типа
            var pattern = (type_pattern) patternCase.pattern;
            var matchResultVariableName = GenerateIdent();
            result.Add(new var_statement(matchResultVariableName, pattern.type));
            // делегирование проверки паттерна функции IsTest
            var isTest = SubtreeCreator.CreateSystemFunctionCall("IsTest", matchingExpression, matchResultVariableName);

            // Конструируем условие мэтчинга 
            expression matchCondition = isTest;
            if (patternCase.condition == null)
                matchCondition = isTest;
            else
            // Если есть секция when - добавляем ее в условие
            {
                // Переименовываем идентификатор, т.к. мы сгенерировали внутреннее имя
                patternCase.condition.RenameIdentifierInDescendants(pattern.identifier.name, matchResultVariableName.name);
                matchCondition = new bin_expr(isTest, patternCase.condition, Operators.LogicalAND);
            }

            // Создание узла if с объявлением переменной из паттерна и присвоение ей значения 
            // сгенерированной переменной
            var patternVarDef = new var_statement(pattern.identifier, matchResultVariableName);
            // Создание тела if из объявления переменной и соттветствующего тела case
            var ifCheck = SubtreeCreator.CreateIf(matchCondition, new statement_list(patternVarDef, patternCase.case_action));
            result.Add(ifCheck);

            // Добавляем полученные statements в результат
            AddDesugaredCaseToResult(result, ifCheck);
        }

        void DesugarDeconstructorPatternCase(expression matchingExpression, pattern_case patternCase)
        {
            Debug.Assert(patternCase.pattern is deconstructor_pattern);

            var result = new statement_list();
            // создание фиктивной переменной необходимого типа
            var pattern = (deconstructor_pattern)patternCase.pattern;
            var matchResultVariableName = GenerateIdent();
            result.Add(new var_statement(matchResultVariableName, pattern.type));
            // делегирование проверки паттерна функции IsTest
            var isTest = SubtreeCreator.CreateSystemFunctionCall("IsTest", matchingExpression, matchResultVariableName);

            // Конструируем условие мэтчинга 
            expression matchCondition = isTest;
            if (patternCase.condition == null)
                matchCondition = isTest;
            else
            // Если есть секция when - добавляем ее в условие
            {
                // Переименовываем идентификатор, т.к. мы сгенерировали внутреннее имя
                patternCase.condition.RenameIdentifierInDescendants(pattern.parameters[0].identifier.name, matchResultVariableName.name);
                matchCondition = new bin_expr(isTest, patternCase.condition, Operators.LogicalAND);
            }

            // Создание узла if с объявлением переменной из паттерна и присвоение ей значения 
            // сгенерированной переменной
            var patternVarDef = new var_statement(pattern.parameters[0].identifier.name, matchResultVariableName);
            // Создание тела if из объявления переменной и соттветствующего тела case
            var ifCheck = SubtreeCreator.CreateIf(matchCondition, new statement_list(patternVarDef, patternCase.case_action));
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
    }
}
