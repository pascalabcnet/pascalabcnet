// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.TreeRealization;
using PascalABCCompiler.SyntaxTree;
using TreeConverter.LambdaExpressions;
using static PascalABCCompiler.StringConstants;

namespace PascalABCCompiler.TreeConverter
{
    #region Визитор поиска имен переменных (для поиска захваченных переменных в теле лямбды)
    public class FindMainIdentsVisitor : WalkingVisitorNew  // SSM
    {
        public ISet<ident> vars = new HashSet<ident>();
        public override void visit(ident id)
        {
            vars.Add(id);
        }
        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);
            if (dn.right.GetType()!=typeof(ident))
                ProcessNode(dn.right);
        }
    }
    #endregion

    #region Визитор поиска имен локальных переменных 
    public class FindLocalDefsVisitor : WalkingVisitorNew // SSM
    {
        public ISet<string> vars = new HashSet<string>();
        private bool indef = false;
        public override void visit(ident id)
        {
            if (indef)
                vars.Add(id.name);
        }
        public override void visit(SyntaxTree.var_statement defs)
        {
            indef = true;
            ProcessNode(defs.var_def.vars); // исключаем типы - 
              // просматриваем только имена переменных
            indef = false;
        }
    }
    #endregion

    public class LambdaHelper
    {
        public static string lambdaPrefix = "<>lambda";
        private const string nonPublicMembersNamePrefix = "<>nonPublic";
        private const string auxiliaryLambdaSuffix = "_$$$auxiliaryFuncName";
        private const string auxVarNamePrefix = "$$$auxVar$$$<>";
        private static int auxCounter = 0;
        private static int nonPublicMembersNameCounter = 0;
        private static int auxVarCounter = 0;

        public static string GetAuxVarName()
        {
            return auxVarNamePrefix + auxVarCounter++;
        }

        public static string GetAuxiliaryLambdaName(string lambdaName)
        {
            return lambdaName + auxiliaryLambdaSuffix + auxCounter++;
        }

        public static string GetNameForNonPublicMember(string memberName)
        {
            return nonPublicMembersNamePrefix + memberName + nonPublicMembersNameCounter++;
        }

        public static string GetLambdaNamePartWithoutGenerics(string name)
        {
            if (!name.StartsWith(lambdaPrefix))
            {
                return name;
            }

            var ind = name.IndexOf("<", lambdaPrefix.Length);
            if (ind < 0)
            {
                return name;
            }

            return name.Substring(0, ind);
        }

        public static void RemoveLambdaInfoFromCompilationContext(compilation_context context, function_lambda_definition lambdaDefinition)
        {
            if (context.converted_namespace != null
                            && context.converted_namespace.functions != null
                            && context.converted_namespace.functions.Any())
            // Если добавилось в глобальные функции, то удаляем оттуда
            {
                var lastIndex = context.converted_namespace.functions.Count - 1;
                if (context.converted_namespace.functions[lastIndex].name ==
                    lambdaDefinition.lambda_name)
                {
                    context.converted_namespace.functions.remove_at(lastIndex);
                }
            }
            //else SSM 27/04/16 - грубое исправление ошибки #147
            {
                // Если добавилась лямбда как метод класса, то удаляем оттуда
                if (context.converted_type != null
                    && context.converted_type.methods != null
                    && context.converted_type.methods.Any())
                {
                    var lastIndex = context.converted_type.methods.Count - 1;
                    if (context.converted_type.methods[lastIndex].name ==
                        lambdaDefinition.lambda_name)
                    {
                        context.converted_type.methods.remove_at(lastIndex);
                    }
                }
            }
        }

        public static type_definition ConvertSemanticTypeToSyntaxType(type_node semType)
        {
            // SSM 29/12/18 пробую скомбинировать
            //if (semType is compiled_type_node && !(semType as compiled_type_node).is_generic_type_definition && !(semType as compiled_type_node).is_generic_type_instance)
              //return new semantic_type_node(semType); // SSM 29/12/18 поменял на современное. Не получилось!
            //else 
            return OpenMP.ConvertToSyntaxType(semType); // Это же надо! Пользоваться для этого хреновиной из OpenMP!!!
        }

        public static int processingLambdaParametersForTypeInference = 0; // Счетчик для подсчета лямбд
        public static bool IsLambdaName(ident id)
        {
            if (id == null)
                return false;
            if (id.name == null)
                return false;
            return id.name.Contains(lambdaPrefix);
        }

        public static bool IsLambdaName(string id)
        {
            if (id == null)
                return false;
            return id.Contains(lambdaPrefix);
        }

        public static bool IsCapturedSelf(expression_node ex)
        {
            if (ex is class_field_reference)
            {
                class_field fld = (ex as class_field_reference).field;
                if (fld.name.Contains("<>local_variables_class"))
                    return true;
            }
            return false;
        }

        public static bool IsAuxiliaryLambdaName(ident id)
        {
            if (id == null)
                return false;
            if (id.name == null)
                return false;
            return id.name.Contains(auxiliaryLambdaSuffix);
        }

        public static bool IsAuxiliaryLambdaName(string id)
        {
            if (id == null)
                return false;
            return id.Contains(auxiliaryLambdaSuffix);
        }

        public static Stack<KeyValuePair<string, statement_list_stack>> StatementListStackStack = new Stack<KeyValuePair<string, statement_list_stack>>();

        public static void Reset()
        {
            CurrentLambdaScopeNum = -1;
            capturedVariables = new List<SymbolInfo>();
            captureCheck = false;
            processingLambdaParametersForTypeInference = 0;
            StatementListStackStack.Clear();
            auxCounter = 0;
            nonPublicMembersNameCounter = 0;
            auxVarCounter = 0;
        }
        public static bool captureCheck = false;
        public static List<SymbolInfo> capturedVariables = new List<SymbolInfo>();
        public static int CurrentLambdaScopeNum = -1;
        /// <summary>
        /// Фиктивный блок, представляющий лямбда-выражение. Используется для обхода с целью получения списка захватываемых переменных
        /// </summary>
        /// <param name="lambdaDef"></param>
        /// <returns></returns>
        public static block CreateFictiveBlockForLambda(function_lambda_definition lambdaDef)
        {
            statement_list stmtList = new statement_list();
            if (lambdaDef.formal_parameters != null)
                for (int i = 0; i < lambdaDef.formal_parameters.params_list.Count; i++)
                    stmtList.subnodes.Add(SyntaxTreeNodesConstructor.CreateVarStatementNode(lambdaDef.formal_parameters.params_list[i].idents,
                                                                                   lambdaDef.formal_parameters.params_list[i].vars_type,
                                                                                   null));
            if (lambdaDef.return_type != null)
                stmtList.subnodes.Add(SyntaxTreeNodesConstructor.CreateVarStatementNode(result_var_name, lambdaDef.return_type, null)); // переделать, не сработает, если тип возвращаемого значения не указан
            stmtList.subnodes.AddRange((lambdaDef.proc_body as statement_list).subnodes);
            block resBlock = new block();
            resBlock.program_code = stmtList;
            return resBlock;
        }

        /// <summary>
        /// Вывод типа возвращаемого значения лямбды
        /// </summary>
        public static void InferResultType(function_header funcHeader, proc_block procBody, syntax_tree_visitor visitor)
        {
            var retType = funcHeader.return_type as lambda_inferred_type;
            if (retType != null && retType.real_type is lambda_any_type_node)
                retType.real_type = new LambdaResultTypeInferrer(funcHeader, procBody, visitor).InferResultType();
        }

        /// <summary>
        /// Заменяет x -> begin result := x.Print end;  на  x -> begin x.Print end  в случае если это один оператор и вызов метода
        ///
        /// </summary>
        public static bool TryConvertFuncLambdaBodyWithMethodCallToProcLambdaBody(function_lambda_definition lambdaDef)
        {
            // SSM 12/12/16 - сделал чтобы всегда эта функция возвращала true.
            // Посмотрю далее на её поведение. Мне кажется, что если мы попали сюда, то мы хотим присвоить процедурному типу, 
            // и в любом случае это надо интерпретировать как процедуру
            var stl = lambdaDef.proc_body as SyntaxTree.statement_list;
            if (stl.expr_lambda_body)
            {
                // Очищаем от Result := , который мы создали на предыдущем этапе

                var ass = stl.list[0] as assign;
                if (ass != null && ass.to is ident && (ass.to as ident).name.ToLower() == result_var_name.ToLower())
                {
                    var f = ass.from;
                    if (f is method_call)
                    {
                        var ff = f as method_call;
                        stl.list[0] = new procedure_call(ff, ff.source_context); // заменить result := Print(x) на Print(x)
                        lambdaDef.return_type = null;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false; // Т.е. это - лямбда с коротким телом, но в результате сахарных преобразований Result := переменстился не на первое место - тогда преобразовать нельзя
            }

            lambdaDef.return_type = null;
            return true;
//            return false;
        }

        /// <summary>
        /// Вывод типа параметров лямбд и типа возвращаемого значения при присваивании лямбды переменной 
        /// </summary>
        public static void InferTypesFromVarStmt(type_node leftType, function_lambda_definition lambdaDef, syntax_tree_visitor visitor, Operators op = Operators.Undefined)
        {
            if (lambdaDef == null)
                return;
            if (leftType != null)
            {
                delegate_internal_interface dii_left =
                    (delegate_internal_interface)leftType.get_internal_interface(internal_interface_kind.delegate_interface);
                if (dii_left == null)
                {
                    if (leftType != SystemLibrary.SystemLibrary.system_delegate_type)
                    {
                        if (op != Operators.Undefined)
                        {
                            var sil = leftType.find_in_type(name_reflector.get_name(op));
                            if (sil != null && sil.Count > 0)
                            {
                                foreach (SymbolInfo si in sil)
                                {
                                    if (si.sym_info is function_node)
                                    {
                                        function_node fn = si.sym_info as function_node;
                                        if (fn.parameters.Count == 2)
                                        {
                                            dii_left = (delegate_internal_interface)fn.parameters[1].type.get_internal_interface(internal_interface_kind.delegate_interface);
                                            if (dii_left != null)
                                                break;
                                            if (fn.parameters[1].type.is_generic_parameter)
                                            {
                                                compiled_type_node ctn = leftType as compiled_type_node;
                                                common_type_node ctn2 = leftType as common_type_node;
                                                if (ctn != null && ctn.is_generic_type_instance && fn.parameters[0].type.is_generic_type_instance && ctn.original_generic == fn.parameters[0].type.original_generic)
                                                {
                                                    dii_left = (delegate_internal_interface)ctn.generic_params[0].get_internal_interface(internal_interface_kind.delegate_interface);
                                                    if (dii_left != null)
                                                        break;
                                                }
                                                if (ctn2 != null && ctn2.is_generic_type_instance && ctn2.instance_params.Count > 0 && fn.parameters[0].type.is_generic_type_instance && ctn2.original_generic == fn.parameters[0].type.original_generic)
                                                {
                                                    dii_left = (delegate_internal_interface)ctn2.instance_params[0].get_internal_interface(internal_interface_kind.delegate_interface);
                                                    if (dii_left != null)
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (dii_left == null)
                            visitor.AddError(visitor.get_location(lambdaDef), "ILLEGAL_LAMBDA_VARIABLE_TYPE");
                    } 
                    else
                        return;
                }
                    
                int leftTypeParamsNumber = dii_left.parameters.Count;
                int lambdaDefParamsCount = 0;
                if (lambdaDef.formal_parameters != null && lambdaDef.formal_parameters.params_list.Count != 0)
                {
                    for (int i = 0; i < lambdaDef.formal_parameters.params_list.Count; i++)
                        lambdaDefParamsCount += lambdaDef.formal_parameters.params_list[i].idents.idents.Count;
                    if (lambdaDefParamsCount != leftTypeParamsNumber)
                        visitor.AddError(visitor.get_location(lambdaDef), "ILLEGAL_LAMBDA_PARAMETERS_NUMBER");
                    bool flag = true;
                    SyntaxTree.formal_parameters lambdaDefParamsTypes = new formal_parameters();
                    for (int i = 0; i < lambdaDef.formal_parameters.params_list.Count; i++)
                        for (int j = 0; j < lambdaDef.formal_parameters.params_list[i].idents.idents.Count; j++)
                        {
                            var param = new SyntaxTree.typed_parameters();
                            param.idents = new ident_list();
                            param.idents.Add(lambdaDef.formal_parameters.params_list[i].idents.idents[j]);
                            param.vars_type = lambdaDef.formal_parameters.params_list[i].vars_type;
                            param.source_context = lambdaDef.formal_parameters.source_context;
                            lambdaDefParamsTypes.Add(param);
                        }
                    for (int i = 0; i < leftTypeParamsNumber && flag; i++)
                    {
                        if (lambdaDefParamsTypes.params_list[i].vars_type is SyntaxTree.lambda_inferred_type)
                        {
                            if ((lambdaDefParamsTypes.params_list[i].vars_type as SyntaxTree.lambda_inferred_type).real_type is lambda_any_type_node)
                            {
                                var curLeftParType = dii_left.parameters[i].type;
                                lambdaDefParamsTypes.params_list[i].vars_type = new SyntaxTree.lambda_inferred_type();
                                (lambdaDefParamsTypes.params_list[i].vars_type as SyntaxTree.lambda_inferred_type).real_type = curLeftParType;
                                continue;
                            }
                        }
                        var lambdaPar = visitor.convert_strong(lambdaDefParamsTypes.params_list[i].vars_type);
                        if (!convertion_data_and_alghoritms.eq_type_nodes(dii_left.parameters[i].type, lambdaPar))
                        {
                            visitor.AddError(visitor.get_location(lambdaDef), "ILLEGAL_LAMBDA_VARIABLE_TYPE");
                        }
                    }
                    lambdaDef.formal_parameters = lambdaDefParamsTypes;
                }
                if (lambdaDef.return_type != null && lambdaDef.return_type is lambda_inferred_type)
                {
                    if (dii_left.return_value_type != null)
                        (lambdaDef.return_type as lambda_inferred_type).real_type = dii_left.return_value_type;
                    else // SSM 23/07/16 - попытка бороться с var p: Shape->() := a->a.Print()
                    {
                        // lambdaDef.usedkeyword == 1 // function
                        var b = lambdaDef.usedkeyword == 0 && TryConvertFuncLambdaBodyWithMethodCallToProcLambdaBody(lambdaDef); // пытаться конвертировать только если мы явно не указали, что это функция
                        if (!b)
                            visitor.AddError(visitor.get_location(lambdaDef), "UNABLE_TO_CONVERT_FUNCTIONAL_TYPE_TO_PROCEDURAL_TYPE");
                    }
                }
            }
            else
            {
                if (lambdaDef.formal_parameters != null)
                    for (int i = 0; i < lambdaDef.formal_parameters.params_list.Count; i++)
                        if (lambdaDef.formal_parameters.params_list[i].vars_type is lambda_inferred_type)
                            visitor.AddError(visitor.get_location(lambdaDef), "IMPOSSIBLE_TO_INFER_TYPES_IN_LAMBDA");
            }
        }

        /// <summary>
        /// Временный узел, который используется при выведении типов параметров
        /// </summary>
        /// <param name="def"></param>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public static typed_expression GetTempFunctionNodeForTypeInference(SyntaxTree.function_lambda_definition def, syntax_tree_visitor visitor)
        {
            var res = new common_namespace_function_node(def.lambda_name, visitor.get_location(def), null, null);
            if (def.return_type != null)
                res.return_value_type = visitor.convert_strong(def.return_type);
            else
                res.return_value_type = null;
            res.parameters.Clear();
            if (def.formal_parameters != null && def.formal_parameters.params_list.Count != 0)
            {
                for (int i = 0; i < def.formal_parameters.params_list.Count; i++)
                { 
                    var tt = visitor.convert_strong(def.formal_parameters.params_list[i].vars_type); // SSM 29/12/18
                    for (int j = 0; j < def.formal_parameters.params_list[i].idents.idents.Count; j++)
                    {
                        var new_param = new common_parameter(null, SemanticTree.parameter_type.value, res, concrete_parameter_type.cpt_none, visitor.get_location(def.formal_parameters.params_list[i].idents.idents[0]));
                        new_param.type = tt;
                        res.parameters.AddElement(new_param);
                    }
                }
            }
            var hhh = new delegated_methods();
            hhh.proper_methods.AddElement(new common_namespace_function_call(res, visitor.get_location(def)));
            return new typed_expression(hhh, visitor.get_location(def));
        }

        public static procedure_definition ConvertLambdaNodeToProcDefNode(function_lambda_definition functionLambdaDef)
        {
            procedure_definition procDef = null;
            if (functionLambdaDef.return_type == null)
                procDef = SyntaxTreeNodesConstructor.CreateProcedureDefinitionNode(new method_name(null,null, new ident(functionLambdaDef.lambda_name, functionLambdaDef.source_context), null),
                                                              functionLambdaDef.formal_parameters,
                                                              false,
                                                              false,
                                                              functionLambdaDef.proc_body,
                                                              functionLambdaDef.source_context);
            else
                procDef = SyntaxTreeNodesConstructor.CreateFunctionDefinitionNode(new method_name(null, null, new ident(functionLambdaDef.lambda_name), null),
                                                             functionLambdaDef.formal_parameters,
                                                             false,
                                                             false,
                                                             functionLambdaDef.proc_body,
                                                             functionLambdaDef.return_type,
                                                             functionLambdaDef.source_context);
            procDef.proc_header.name.meth_name.source_context = procDef.proc_header.source_context;
            functionLambdaDef.proc_definition = procDef;
            return procDef;    
        }

        #region Поиск всех result - ов 
        public class ResultNodesSearcher : SyntaxTree.WalkingVisitorNew // Нигде в проекте не используется потому что expr могут использовать локальные переменные, тип которых неизвестен до семантического разбора
        {
            public List<expression> exprList = new List<expression>();
            public ResultNodesSearcher(syntax_tree_node root)
            {
                ProcessNode(root);
            }

            public override void visit(function_lambda_definition fd)
            {
                // не заходить во внутренние лямбды
            }
            public override void visit(assign value)
            {
                var to = value.to as ident;
                if (to != null && value.operator_type == Operators.Assignment && to.name.Equals(result_var_name, SemanticRulesConstants.SymbolTableCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                {
                    exprList.Add(value.from);
                }
            }
            /*public override void visit(function_lambda_definition fld)
            {
            }*/
        }
        #endregion
        /*
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
    */
    }
}
