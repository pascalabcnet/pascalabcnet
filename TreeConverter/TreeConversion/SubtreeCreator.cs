// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;

namespace PascalABCCompiler.TreeConversion
{
    /*
    public class SubtreeCreator
    {
        syntax_tree_visitor _syntaxTreeVisitor;

        public SubtreeCreator(syntax_tree_visitor visitor)
        {
            _syntaxTreeVisitor = visitor;
        }
        
        internal void AddError(location loc, string ErrString, params string[] values)
        {
            _syntaxTreeVisitor.AddError(loc, ErrString, values);
        }

        /// <summary>
        /// Создать var-выражение
        /// </summary>
        /// <param name="name">Имя переменной</param>
        /// <param name="initialValue">Начальное значение</param>
        /// <returns></returns>
        public static var_def_statement CreateVarDef(string name, expression initialValue)
        {
            ident_list list = new ident_list();
            list.idents.Add(new ident(name));
            var res = new var_def_statement();
            res.inital_value = initialValue;
            res.vars = list;
            return res;
        }

        /// <summary>
        /// Объявление пременной с именованным типом
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static var_statement NamedTypeVariableDefinition(string variableName, string typeName)
        {
            var_def_statement varDefStatement = new var_def_statement();
            varDefStatement.vars.Add(new ident(variableName));
            varDefStatement.vars_type = new named_type_reference(typeName);
            return new var_statement(varDefStatement);
        }

        /// <summary>
        /// Создать var-выражение с несколькими переменными
        /// </summary>
        /// <param name="type">Тип переменных</param>
        /// <param name="idents">Имена переменных</param>
        /// <returns></returns>
        public static var_def_statement CreateVarDef(named_type_reference type, params string[] names)
        {
            var_def_statement res = new var_def_statement();
            foreach (string x in names)
                res.vars.Add(new ident(x));
            res.vars_type = type;
            return res;
        }

        /// <summary>
        /// Создать цикл for
        /// </summary>
        /// <param name="varName">Имя переменной цикла</param>
        /// <param name="initValue">Начальное значение переменной цикла</param>
        /// <param name="finishValue">Конечное значение переменной цикла</param>
        /// <param name="body">Тело цикла</param>
        /// <param name="type">Тип цикла(to / downto)</param>
        /// <returns></returns>
        public static SyntaxTree.for_node CreateFor(string varName, expression initValue, expression finishValue, statement body, for_cycle_type type)
        {
            SyntaxTree.for_node res = new SyntaxTree.for_node();
            res.loop_variable = new ident(varName);
            res.initial_value = initValue;
            res.finish_value = finishValue;
            res.statements = body;
            res.cycle_type = type;
            return res;
        }

        /// <summary>
        /// Создать цикл while
        /// </summary>
        /// <param name="condition">Условие цикла</param>
        /// <param name="body">Тело цикла</param>
        /// <returns></returns>
        public static SyntaxTree.while_node CreateWhile(expression condition, statement body)
        {
            SyntaxTree.while_node res = new SyntaxTree.while_node();
            res.CycleType = WhileCycleType.While;
            res.expr = condition;
            res.statements = body;
            return res;
        }

        private static SyntaxTree.if_node CreateIfElse(expression condition, statement thenBody, statement elseBody)
        {
            return new SyntaxTree.if_node(condition, thenBody, elseBody);
        }

        /// <summary>
        /// Создать условный оператор
        /// </summary>
        /// <param name="condition">Условие</param>
        /// <param name="thenBody">Тело, выполняемое если условие истино</param>
        /// <returns></returns>
        public static SyntaxTree.if_node CreateIf(expression condition, statement thenBody)
        {
            return CreateIfElse(condition, thenBody, null);
        }

        /// <summary>
        /// Создать условный оператор
        /// </summary>
        /// <param name="condition">Условие</param>
        /// <param name="thenBody">Тело, выполняемое если условие истино</param>
        /// <param name="elseBody">Тело, выполняемое если условие ложно</param>
        /// <returns></returns>
        public static SyntaxTree.if_node CreateIf(expression condition, statement thenBody, statement elseBody)
        {
            return CreateIfElse(condition, thenBody, elseBody);
        }

        private static SyntaxTree.program_module InternalCreateProgramModule(statement_list statements, declarations defs)
        {
            block block = new block();
            if (defs != null)
                block.defs = defs;
            statements.Add(new SyntaxTree.empty_statement());
            statements.left_logical_bracket = new token_info("begin");
            statements.right_logical_bracket = new token_info("end");
            block.program_code = statements;

            program_module res = new program_module();
            res.program_block = block;
            //res.used_units = create_standard_uses_list();
            return res;
        }

        public static SyntaxTree.program_module CreateProgramModule(statement_list statements, declarations defs)
        {
            return InternalCreateProgramModule(statements, defs);
        }

        public static SyntaxTree.program_module CreateProgramModule(statement_list statements)
        {
            return InternalCreateProgramModule(statements, null);
        }

        //private SyntaxTree.uses_list CreateStandartUsesList()
        //{
        //    uses_list res = new uses_list();
        //    unit_or_namespace pabcsystem = new unit_or_namespace();
        //    pabcsystem.name = new ident_list("PABCSystem");
        //    res.units.Add(pabcsystem);
        //    return res;
        //}

        /// <summary>
        /// Создает узел вызова процедуры из PABCSystem
        /// </summary>
        /// <param name="procName">Имя процедуры</param>
        /// <param name="exprList">Параметры процедуры</param>
        /// <returns></returns>
        public static procedure_call CreateSystemProcedureCall(string procName, params expression[] exprList)
        {
            var procedureCall = CreateProcedureCall(procName, exprList);
            (procedureCall.func_name as method_call).dereferencing_value = new dot_node(new ident("PABCSystem"), new ident(procName));

            return procedureCall;
        }

        /// <summary>
        /// Создает узел вызова функции из PABCSystem
        /// </summary>
        /// <param name="funcName">Имя функции</param>
        /// <param name="exprList">Параметры функции</param>
        /// <returns></returns>
        public static method_call CreateSystemFunctionCall(string funcName, params expression[] exprList)
        {
            return CreateMethodCall(funcName, "PABCSystem", exprList);
        }

        public static method_call CreateMethodCall(string funcName, string qualifier, params expression[] exprList)
        {
            var methodCall = CreateMethodCall(funcName, exprList);
            methodCall.dereferencing_value = new dot_node(new ident(qualifier), new ident(funcName));
            return methodCall;
        }

        public static procedure_call CreateProcedureCall(string procName, params expression[] exprList)
        {
            SyntaxTree.procedure_call pc = new SyntaxTree.procedure_call();
            pc.func_name = CreateMethodCall(procName, exprList);
            return pc;
        }

        private static method_call CreateMethodCall(string methodName, params expression[] exprList)
        {
            SyntaxTree.method_call mc = new SyntaxTree.method_call();
            mc.dereferencing_value = new ident(methodName);
            SyntaxTree.expression_list exl = new PascalABCCompiler.SyntaxTree.expression_list();
            foreach (expression x in exprList)
                exl.Add(x);
            mc.parameters = exl;

            return mc;
        }

        // From LamdaHelper
        private static procedure_definition InternalCreateProcedureDefinitionNode(string methName, formal_parameters formalPars, bool classKeyword, statement procBody, SourceContext sc)
        {
            return PascalABCCompiler.TreeConverter.LambdaHelper.SyntaxTreeNodesConstructor.CreateProcedureDefinitionNode(new method_name(methName),
                                                                                                                         formalPars,
                                                                                                                         false,
                                                                                                                         classKeyword,
                                                                                                                         procBody,
                                                                                                                         sc);
        }
        
        public static procedure_definition CreateProcedureDefinitionNode(string methName, formal_parameters formalPars, bool classKeyword, statement procBody, SourceContext sc)
        {
            return InternalCreateProcedureDefinitionNode(methName, formalPars, classKeyword, procBody, sc);
        }

        public static procedure_definition CreateProcedureDefinitionNode(string methName, formal_parameters formalPars, bool classKeyword, statement procBody)
        {
            return InternalCreateProcedureDefinitionNode(methName, formalPars, classKeyword, procBody, null);
        }

        private static procedure_definition InternalCreateFunctionDefinitionNode(string methName, formal_parameters formalPars, bool classKeyword, statement procBody, type_definition returnType, SourceContext sc)
        {
            return PascalABCCompiler.TreeConverter.LambdaHelper.SyntaxTreeNodesConstructor.CreateFunctionDefinitionNode(new method_name(methName),
                                                                                                                        formalPars,
                                                                                                                        false,
                                                                                                                        classKeyword,
                                                                                                                        procBody,
                                                                                                                        returnType,
                                                                                                                        sc);
        }

        public static procedure_definition CreateFunctionDefinitionNode(string methName, formal_parameters formalPars, bool classKeyword, statement procBody, type_definition returnType, SourceContext sc)
        {
            return InternalCreateFunctionDefinitionNode(methName, formalPars, classKeyword, procBody, returnType, sc);
        }

        public static procedure_definition CreateFunctionDefinitionNode(string methName, formal_parameters formalPars, bool classKeyword, statement procBody, type_definition returnType)
        {
            return InternalCreateFunctionDefinitionNode(methName, formalPars, classKeyword, procBody, returnType, null);
        }
        // From !LamdaHelper

        public static declarations CreateDeclarations(params declaration[] decls)
        {
            declarations res = new declarations();
            foreach (declaration x in decls)
                res.Add(x);
            return res;
        }

        public static statement_list CreateStatementList(params statement[] stmts)
        {
            statement_list res = new statement_list();
            foreach (statement x in stmts)
                res.Add(x);
            return res;
        }
    }
    */
}
