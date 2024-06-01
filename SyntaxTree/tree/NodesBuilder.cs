using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PascalABCCompiler.SyntaxTree
{
    #region Генерация новых узлов синтаксического дерева
    /// <summary>
    /// Класс для генерации новых узлов синтаксического дерева
    /// </summary>
    public class SyntaxTreeNodesConstructor
    {
        public static var_statement CreateVarStatementNode(string idName, type_definition varType, expression initValue)
        {
            var id = new ident(idName);
            var idlist = new ident_list(id);
            var vdef = new var_def_statement(idlist, varType, initValue, definition_attribute.None, false, null);
            return new var_statement(vdef, null);
        }

        public static var_statement CreateVarStatementNode(string idName, string varTypeName, expression initValue)
        {
            var id = new ident(idName);
            var idlist = new ident_list(id);
            var varType = new named_type_reference(varTypeName, null);
            var vdef = new var_def_statement(idlist, varType, initValue, definition_attribute.None, false, null);
            return new var_statement(vdef, null);
        }

        public static var_statement CreateVarStatementNode(ident_list idlist, type_definition varType, expression initValue)
        {
            var vdef = new var_def_statement(idlist, varType, initValue, definition_attribute.None, false, null);
            return new var_statement(vdef, null);
        }

        public static procedure_definition CreateProcedureDefinitionNode(method_name methName, formal_parameters formalPars, bool ofObject, bool classKeyword, statement procBody, SourceContext sc)
        {
            procedure_definition procDef = new procedure_definition();

            procedure_header procHeader = new procedure_header();
            procHeader.name = methName;
            procHeader.source_context = sc;
            if (procHeader.name.meth_name is template_type_name)
            {
                procHeader.template_args = (procHeader.name.meth_name as template_type_name).template_args;
                ident id = new ident(procHeader.name.meth_name.name);
                procHeader.name.meth_name = id;
            }
            procHeader.parameters = formalPars;
            procHeader.of_object = ofObject;
            procHeader.class_keyword = classKeyword;

            statement_list stmtList = new statement_list();
            stmtList.subnodes.Add(procBody);

            block bl = new block(null, null);
            bl.program_code = stmtList;

            procDef.proc_header = procHeader;
            procDef.proc_body = (proc_block)bl;

            return procDef;
        }

        public static procedure_definition CreateFunctionDefinitionNode(method_name methName, formal_parameters formalPars, bool ofObject, bool classKeyword, statement procBody, type_definition returnType, SourceContext sc)
        {
            procedure_definition procDef = new procedure_definition();

            function_header procHeader = new function_header();
            procHeader.name = methName;
            procHeader.source_context = sc;
            if (procHeader.name.meth_name is template_type_name)
            {
                procHeader.template_args = (procHeader.name.meth_name as template_type_name).template_args;
                ident id = new ident(procHeader.name.meth_name.name);
                procHeader.name.meth_name = id;
            }
            procHeader.parameters = formalPars;
            procHeader.of_object = ofObject;
            procHeader.class_keyword = classKeyword;
            procHeader.return_type = returnType;

            statement_list stmtList = new statement_list();
            stmtList.subnodes.Add(procBody);

            block bl = new block(null, null);
            bl.program_code = stmtList;

            procDef.proc_header = procHeader;
            procDef.proc_body = (proc_block)bl;

            return procDef;
        }
    }
    #endregion

    public class SubtreeCreator
    {
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
            return SyntaxTreeNodesConstructor.CreateProcedureDefinitionNode(new method_name(methName),
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
            return SyntaxTreeNodesConstructor.CreateFunctionDefinitionNode(new method_name(methName),
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
    public class SyntaxTreeBuilder
    {
        public static SourceContext BuildGenSC = new SourceContext(0, 777777, 0, 0, 0, 0);

        private static int GenIdNum = 0;
        public static ident GenIdentName()
        {
            GenIdNum++;
            return new ident("$GenId" + GenIdNum.ToString());
        }

        public static type_definition BuildSimpleType(string name)
        {
            return new named_type_reference(name, null);
        }

        public static type_definition BuildSameType(expression ex)
        {
            return new same_type_node(ex);
        }

        public static type_definition BuildSemanticType(Object t)
        {
            return new semantic_type_node(t) as type_definition;
        }

        public static var_def_statement BuildSimpleVarDef(string name, string type)
        {
            return new var_def_statement(name, BuildSimpleType(type));
        }

        public static class_members BuildClassFieldsSection(List<ident> names, List<type_definition> types)
        {
            var cm = new class_members(access_modifer.private_modifer);
            for (int i = 0; i < names.Count; i++)
                cm.Add(new var_def_statement(names[i], types[i]));
            return cm;
        }

        public static class_members BuildOneMemberSection(declaration m)
        {
            var cm = new class_members(access_modifer.public_modifer);
            cm.Add(m);
            return cm;
        }

        public static formal_parameters BuildFormalParameters(List<ident> names, List<type_definition> types)
        {
            var fp = new formal_parameters();
            for (int i = 0; i < names.Count; i++)
                fp.Add(new typed_parameters(names[i], types[i]));
            return fp;
        }

        public static formal_parameters BuildFormalVarParameters(List<ident> names, List<type_definition> types)
        {
            var fp = new formal_parameters();
            for (int i = 0; i < names.Count; i++) 
                fp.Add(new typed_parameters(new ident_list(names[i]), types[i],parametr_kind.var_parametr,null));
            return fp;
        }

        public static statement_list BuildSimpleAssignList(List<ident> lnames, List<ident> rnames)
        {
            var sl = new statement_list();
            for (int i = 0; i < lnames.Count; i++)
                sl.Add( new assign(lnames[i], rnames[i]));
            return sl;
        }

        public static procedure_definition BuildSimpleConstructor(List<ident> fields, List<ident> formal_names, List<type_definition> types)
        {
            var fp = SyntaxTreeBuilder.BuildFormalParameters(formal_names, types);
            if (fp.params_list.Count == 0)
                fp = null;
            var sl = SyntaxTreeBuilder.BuildSimpleAssignList(fields, formal_names);

            var c = new constructor(fp);
            return new procedure_definition(c, new block(sl));
        }

        public static procedure_definition BuildSimpleDeConstruct(List<ident> fields, List<ident> formal_names, List<type_definition> types)
        {
            var fp = SyntaxTreeBuilder.BuildFormalVarParameters(formal_names, types);
            if (fp.params_list.Count == 0)
                fp = null;
            var sl = SyntaxTreeBuilder.BuildSimpleAssignList(formal_names, fields);

            return new procedure_definition(new procedure_header("Deconstruct",fp), new block(sl));
        }

        public static class_members BuildSimpleConstructorSection(List<ident> fields, List<ident> formal_names, List<type_definition> types)
        {
            var cm = new class_members(access_modifer.public_modifer);
            cm.Add(BuildSimpleConstructor(fields,formal_names,types));
            return cm;
        }

        public static class_members BuildSimpleDeconstructSection(List<ident> fields, List<ident> formal_names, List<type_definition> types)
        {
            var cm = new class_members(access_modifer.public_modifer);
            cm.Add(BuildSimpleDeConstruct(fields, formal_names, types));
            return cm;
        }

        public static simple_property BuildSimpleReadWriteProperty(ident name, ident field, type_definition type)
        {
            return new simple_property(new property_ident(name.name, null, name.source_context), type, new property_accessors(new read_accessor_name(field,null,null), new write_accessor_name(field, null, null)));
        }

        public static class_members BuildSimpleReadPropertiesSection(List<ident> names, List<ident> fields, List<type_definition> types)
        {
            var cm = new class_members(access_modifer.public_modifer);
            for (int i = 0; i < names.Count; i++)
                cm.Add(BuildSimpleReadWriteProperty(names[i], fields[i], types[i]));
            return cm;
        }

        public static class_definition BuildClassOrRecordDefinition(bool is_class, params class_members[] cms)
        {
            var cb = new class_body_list();
            foreach (var cm in cms)
                cb.Add(cm);
            var cd = new class_definition(cb);
            if (!is_class)
                cd.keyword = class_keyword.Record;
            return cd;
        }

        public static class_definition BuildClassDefinition(params class_members[] cms)
        {
            return BuildClassOrRecordDefinition(true, cms);
        }

        public static class_definition BuildClassDefinition(named_type_reference_list parents, params class_members[] cms)
        {
            var cb = new class_body_list();
            foreach (var cm in cms)
                cb.Add(cm);
            var cd = new class_definition(parents,cb);
            return cd;
        }

        // frninja 23/04/16 - для шаблонных классов в yield
        public static class_definition BuildClassDefinition(named_type_reference_list parents, ident_list template_args, params class_members[] cms)
        {
            var cb = new class_body_list();
            foreach (var cm in cms)
                cb.Add(cm);

            var cd = new class_definition(parents, cb, class_keyword.Class, template_args, null, class_attribute.None, false, null);
            return cd;
        }
        // end frninja

        // names и types передаю во внешний мир на предмет анализа того, что они не указатели. Снаружи они инициализируются пустыми списками
        public static void AddMembersForAutoClass(class_definition cd, ref List<ident> names, ref List<type_definition> types) // SSM 24.03.14
        {
            //var types = new List<type_definition>();
            class_body_list cb = cd.body;
            bool HasToString = false;
            bool HasConstructor = false;
            bool HasDeconstruct = false;
            foreach (var l in cb.class_def_blocks)
            {
                foreach (var m in l.members)
                {
                    var mm = m as var_def_statement;
                    if (mm != null)
                    {
                        if (mm.var_attr != definition_attribute.Static)
                            foreach (var v in mm.vars.idents)
                            {
                                names.Add(v);
                                if (mm.vars_type != null)
                                    types.Add(mm.vars_type);    // во внешний мир для определения pointerов
                                else 
                                    types.Add(BuildSameType(mm.inital_value)); // почему-то только так хочет работать с рекурсивным типом Node<T>
                            }
                    }
                    else 
                    {
                        var ts = m as procedure_definition;
                        if (!HasConstructor)
                        {
                            if (ts != null && ts.proc_header is constructor 
                                && (ts.proc_header.parameters?.params_list.SelectMany(tp => tp.idents.idents).Count() ?? 0) == names.Count
                                )
                            {
                                HasConstructor = true; // на самом деле это означает, что есть конструктор с точно таким же количеством параметров
                            }
                        }

                        if (!HasDeconstruct)
                        {
                            if (ts != null && ts.proc_header.name != null && ts.proc_header.name.meth_name.name != null)
                            {
                                HasDeconstruct = ts.proc_header.name.meth_name.name.ToLower().Equals("deconstruct");
                            }
                        }

                        if (!HasToString)
                        {
                            if (ts != null && ts.proc_header.name != null && ts.proc_header.name.meth_name.name != null)
                            {
                                HasToString = ts.proc_header.name.meth_name.name.ToUpper().Equals("TOSTRING");
                            }
                        }
                    }
                }
            }

            // добавление свойств - временно убрал т.к. свойства нельзя передавать как var-параметры
            // По идее если это делать, то поля переименовывать везде в классе!!!
            /*for (var i=0; i<names.Count; i++)
            {
                if (names[i].name.StartsWith("#"))
                    continue;
                var propName = names[i].TypedClone();
                names[i].name = "!" + names[i].name;
                var simpleProp = BuildSimpleReadWriteProperty(propName, names[i].name, types[i]);
                var cm = BuildOneMemberSection(simpleProp);
                cb.Add(cm);
            }*/


            if (!HasConstructor)
            {
                var fnames = names.Select(x => new ident("f" + x.name.ToLower(), x.source_context)).ToList();
                if (fnames.Select(x=>x.name).Distinct().Count() != names.Count) // SSM 20/05/2020 #2126
                    return; // хак - мы не генерируем конструктор, потому что ошибка одинаковых имен выведется позже
                var cm = BuildSimpleConstructorSection(names, fnames, types);
                cb.Insert(0,cm);
                //cb.class_def_blocks.Insert(0, cm);
            }

            if (!HasDeconstruct)
            {
                var fnames = names.Select(x => new ident("f" + x.name, x.source_context)).ToList();
                var cm = BuildSimpleDeconstructSection(names, fnames, types);
                cb.Add(cm);
            }

            if (!HasToString)
            { 
                var tostr = BuildToStringFuncForAutoClass(names);
                var cm = BuildOneMemberSection(tostr);
                cb.Add(cm);
                //cb.class_def_blocks.Insert(0, BuildOneMemberSection(tostr));
            }
        }

        public static type_declaration BuildClassWithOneMethod(string class_name, List<ident> names, List<type_definition> types, procedure_definition pd)
        {
            var formnames = names.Select(x => new ident("form"+x.name, x.source_context)).ToList();

            var cm1 = BuildClassFieldsSection(names, types);
            var cm2 = BuildSimpleConstructorSection(names, formnames, types);
            var cm3 = BuildOneMemberSection(pd);

            return new type_declaration(class_name, BuildClassOrRecordDefinition(true, cm1, cm2, cm3), BuildGenSC);
        }

        public static type_declaration BuildAutoClass(string class_name, List<ident> names, List<type_definition> types, bool is_class)
        {
            var fnames = names.Select(x=>new ident("<f>"+x.name, x.source_context)).ToList();

            var cm1 = BuildClassFieldsSection(fnames,types);
            var cm2 = BuildSimpleConstructorSection(fnames,names,types);
            var cm3 = BuildSimpleReadPropertiesSection(names, fnames, types);

            return new type_declaration(class_name, BuildClassOrRecordDefinition(is_class, cm1, cm2, cm3), BuildGenSC);
        }

        public static type_declaration BuildClassWithFieldsOnly(string class_name, List<ident> names, List<type_definition> types, bool is_class)
        {
            var fnames = names.Select(x => new ident(x.name, x.source_context)).ToList();

            var cm1 = BuildClassFieldsSection(fnames, types);

            return new type_declaration(class_name, BuildClassOrRecordDefinition(is_class, cm1), BuildGenSC);
        }

        public static procedure_definition BuildShortFuncDefinition(formal_parameters fp, procedure_attributes_list att, method_name name, type_definition result, expression ex, SourceContext headsc)
        {
            var ff = new function_header(fp, att, name, null, result, headsc);
            procedure_definition pd = BuildShortProcFuncDefinition(ff, new assign("Result", ex, ex.source_context));
            return pd;
        }

        public static procedure_definition BuildShortProcDefinition(formal_parameters fp, procedure_attributes_list att, method_name name, statement st, SourceContext headsc)
        {
            var ff = new procedure_header(fp, att, name, null, headsc);
            return BuildShortProcFuncDefinition(ff, st);
        }
        public static procedure_definition BuildShortProcDefinition(formal_parameters fp, procedure_attributes_list att, method_name name, statement st, where_definition_list wdl, SourceContext headsc)
        {
            var ff = new procedure_header(fp, att, name, wdl, headsc);
            return BuildShortProcFuncDefinition(ff, st);
        }

        public static procedure_definition BuildShortProcFuncDefinition(procedure_header header, statement st)
        {
            var stlist = new statement_list(st, st.source_context);
            var b = new block(null, stlist, st.source_context);

            return new procedure_definition(header, b, true, new SourceContext(header.source_context,st.source_context));
        }

        public static procedure_definition BuildShortProcFuncDefinitionNoSC(procedure_header header, statement st)
        {
            var stlist = new statement_list(st, st.source_context);
            var b = new block(null, stlist, st.source_context);

            return new procedure_definition(header, b, true, BuildGenSC);
        }

        public static procedure_definition BuildToStringFuncForAutoClass(List<ident> names)
        {
            var pal = new procedure_attributes_list(proc_attribute.attr_override);
            var fp = new formal_parameters();
            var ff = new function_header("ToString", "string", fp, pal);

            var cleft = new char_const('(');
            var cright = new char_const(')');
            var ccomma = new char_const(',');
            bin_expr ex = new bin_expr(cleft, cright, Operators.Plus);
            for (var i = 0; i < names.Count; i++)
            { 
                var dn = new dot_node(names[i], new ident("ToString"));
                var asnode = new typecast_node(names[i], new named_type_reference("object"), op_typecast.as_op);
                var eqnode = new bin_expr(asnode, new nil_const(), Operators.Equal);
                var expr = new question_colon_expression(eqnode, new string_const("nil"), dn);
                ex.left = new bin_expr(ex.left, expr, Operators.Plus);
                if (i<names.Count-1)
                    ex.left = new bin_expr(ex.left, ccomma, Operators.Plus);
            }
            var ass = new assign("Result", ex);

            return BuildShortProcFuncDefinitionNoSC(ff, ass);
        }

        public static var_statement BuildVarStatementNodeFromAssignNode(assign _assign)
        {
            ident to = _assign.to as ident;
            if (to == null)
                throw new NotImplementedException();
            var _var_statement = SyntaxTreeNodesConstructor.CreateVarStatementNode(new ident_list(to.name), null, _assign.from);
            _var_statement.source_context = _assign.source_context;
            _var_statement.var_def.source_context = _assign.source_context;
            _var_statement.var_def.vars.source_context = _assign.to.source_context;
            return _var_statement;
        }
    }
}
