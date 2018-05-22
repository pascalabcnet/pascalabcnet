// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Linq;
using PascalABCCompiler.SystemLibrary;
using PascalABCCompiler.SemanticTree;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;

namespace TreeConverter.LambdaExpressions.Closure
{
    internal class CapturedVariablesTreeBuilder : WalkingVisitorNew
    {
        private readonly syntax_tree_visitor _visitor;
        private CapturedVariablesTreeNode _currentTreeNode;
        private readonly Dictionary<int, CapturedVariablesTreeNode> _scopesCapturedVarsNodesDictionary;
        private Stack<CapturedVariablesTreeNodeLambdaScope> _currentLambdaScopeNodeStack = new Stack<CapturedVariablesTreeNodeLambdaScope>();
        private readonly Dictionary<SubstitutionKey, List<ident>> _identsReferences;
        private CapturedVariablesTreeNode _rootNode;
        private CapturedVariablesTreeNodeClassScope _classScope;
        private CapturedVariablesTreeNodeProcedureScope _procedureScope;

        public class CapturedVariablesInfo
        {
            public CapturedVariablesTreeNode RootNode
            {
                get;
                set;
            }

            public Dictionary<SubstitutionKey, List<ident>> IdentsReferences
            {
                get;
                set;
            }

            public Dictionary<int, CapturedVariablesTreeNode> CapturedVarsNodesDictionary
            {
                get; 
                set;
            }

            public CapturedVariablesTreeNodeProcedureScope ProcedureScope
            {
                get;
                set;
            }
        }

        private bool InLambdaContext
        {
            get
            {
                return _currentLambdaScopeNodeStack.Count > 0;
            }
        }
        public CapturedVariablesTreeBuilder(syntax_tree_visitor visitor)
        {
            _visitor = visitor;
            _scopesCapturedVarsNodesDictionary = new Dictionary<int, CapturedVariablesTreeNode>();
            _identsReferences = new Dictionary<SubstitutionKey, List<ident>>();
        }

        public override void visit(semantic_check_sugared_statement_node sn)
        {
            // Не обходить проверочные узлы в визиторе строительства дерева - SSM 1.05.17
        }

        public override void visit(var_def_statement varDefStmt)
        {
            _visitor.visit(varDefStmt);
            
            foreach (var id in varDefStmt.vars.idents)
            {
                SymbolInfo si = _visitor.context.find_first(id.name);
                _currentTreeNode.VariablesDefinedInScope.Add(new CapturedVariablesTreeNode.CapturedSymbolInfo(varDefStmt, si));
            }

            if (varDefStmt.inital_value != null)
            {
                ProcessNode(varDefStmt.inital_value);
            }
        }

        public override void visit(statement_list stmtList)
        {
            var stl = new statements_list(_visitor.get_location(stmtList),
                                          _visitor.get_location_with_check(stmtList.left_logical_bracket),
                                          _visitor.get_location_with_check(stmtList.right_logical_bracket));
            _visitor.convertion_data_and_alghoritms.statement_list_stack_push(stl);

            var newTreeNode = new CapturedVariablesTreeNodeBlockScope(_currentTreeNode, stl.Scope.ScopeNum, stmtList);

            if (_rootNode == null)
            {
                _rootNode = newTreeNode;
            }
            if (_currentTreeNode != null)
            {
                _currentTreeNode.ChildNodes.Add(newTreeNode);
            }
            _currentTreeNode = newTreeNode;

            _scopesCapturedVarsNodesDictionary.Add(stl.Scope.ScopeNum, _currentTreeNode);

            if (stmtList.subnodes != null)
            {
                foreach (var stmt in stmtList.subnodes)
                {
                    ProcessNode(stmt);
                }
            }

            _visitor.convertion_data_and_alghoritms.statement_list_stack.pop();

            _currentTreeNode = _currentTreeNode.ParentNode;
        }

        public override void visit(new_expr newExpr)
        {
            if (newExpr.params_list != null && newExpr.params_list.expressions != null)
            {
                foreach (var expr in newExpr.params_list.expressions)
                {
                    ProcessNode(expr);
                }
            }
        }

        public override void visit(ident id)
        {
            var idName = id.name.ToLower();

            SymbolInfo si = _visitor.context.find_first(idName);
            
            if (si == null)
            {
                if (InLambdaContext)
                {
                    _visitor.AddError(new ThisTypeOfVariablesCannotBeCaptured(_visitor.get_location(id)));
                    return;
                }
                return;
            }

            if (si.sym_info.semantic_node_type == semantic_node_type.namespace_variable ||
                si.sym_info.semantic_node_type == semantic_node_type.common_namespace_function_node ||
                si.sym_info.semantic_node_type == semantic_node_type.namespace_constant_definition ||
                si.sym_info.semantic_node_type == semantic_node_type.compiled_function_node ||
                si.sym_info.semantic_node_type == semantic_node_type.common_method_node || // SSM bug fix  #167
                si.sym_info.semantic_node_type == semantic_node_type.compiled_namespace_node ||
                si.sym_info.semantic_node_type == semantic_node_type.compiled_variable_definition ||
                si.sym_info.semantic_node_type == semantic_node_type.common_type_node ||
                si.sym_info.semantic_node_type == semantic_node_type.compiled_type_node ||
                si.sym_info.semantic_node_type == semantic_node_type.basic_interface_node ||
                si.sym_info.semantic_node_type == semantic_node_type.common_unit_node ||
                si.sym_info.semantic_node_type == semantic_node_type.compiled_unit_node ||
                si.sym_info.semantic_node_type == semantic_node_type.template_type ||
                si.sym_info.semantic_node_type == semantic_node_type.class_constant_definition ||
                si.sym_info.semantic_node_type == semantic_node_type.basic_function_node && idName == "exit")
            {
                return;
            }

            var acceptableVarType = si.sym_info.semantic_node_type == semantic_node_type.local_variable ||
                                    si.sym_info.semantic_node_type == semantic_node_type.local_block_variable ||
                                    si.sym_info.semantic_node_type == semantic_node_type.common_parameter ||
                                    si.sym_info.semantic_node_type == semantic_node_type.class_field
                                    
                                    ;

            if (!(acceptableVarType) && InLambdaContext) 
            {
                _visitor.AddError(new ThisTypeOfVariablesCannotBeCaptured(_visitor.get_location(id)));
                return;
            }

            /*if (si.sym_info.semantic_node_type == semantic_node_type.class_field && InLambdaContext)
            {
                var semClassField = (class_field)si.sym_info;
                if (semClassField.polymorphic_state != polymorphic_state.ps_common)
                {
                    _visitor.AddError(new ThisTypeOfVariablesCannotBeCaptured(_visitor.get_location(id)));
                    return;
                }
            }*/

            if (si.scope == null)
            {
                return;
            }

            var scopeIndex = si.scope.ScopeNum;
            var selfWordInClass = false;

            CapturedVariablesTreeNode scope;
            if (_scopesCapturedVarsNodesDictionary.TryGetValue(scopeIndex, out scope))
            {
                var prScope = scope as CapturedVariablesTreeNodeProcedureScope;
                if (prScope != null && acceptableVarType)
                {
                    if (si.sym_info.semantic_node_type == semantic_node_type.local_variable)
                    {
                        if (!(idName == compiler_string_consts.self_word && si.scope is SymbolTable.ClassMethodScope && _classScope != null) && InLambdaContext)
                        {
                            _visitor.AddError(new ThisTypeOfVariablesCannotBeCaptured(_visitor.get_location(id)));
                        }
                    }
                    if (si.sym_info.semantic_node_type == semantic_node_type.common_parameter && prScope.FunctionNode.parameters.First(v => v.name.ToLower() == idName).parameter_type != parameter_type.value && InLambdaContext)
                    {
                        _visitor.AddError(new CannotCaptureNonValueParameters(_visitor.get_location(id)));
                    }
                    
                    if (idName == compiler_string_consts.self_word && si.scope is SymbolTable.ClassMethodScope &&
                        _classScope != null)
                    {
                        var selfField = _classScope.VariablesDefinedInScope.Find(var => var.SymbolInfo == si);

                        if (selfField == null)
                        {
                            _classScope.VariablesDefinedInScope.Add(
                                new CapturedVariablesTreeNode.CapturedSymbolInfo(null, si));
                        }

                        selfWordInClass = true;
                    }
                    else
                    {
                        var field = prScope.VariablesDefinedInScope.Find(var => var.SymbolInfo == si);

                        if (field == null)
                        {
                            prScope.VariablesDefinedInScope.Add(new CapturedVariablesTreeNode.CapturedSymbolInfo(null,
                                                                                                                 si));
                        }
                    }
                }

                var clScope = scope as CapturedVariablesTreeNodeClassScope;
                if (clScope != null && acceptableVarType)
                {
                    var field = clScope.VariablesDefinedInScope.Find(var => var.SymbolInfo == si);

                    if (field == null)
                    {
                        clScope.VariablesDefinedInScope.Add(new CapturedVariablesTreeNode.CapturedSymbolInfo(null, si));
                    }

                    if (si.access_level != access_level.al_public)
                    {
                        if (!clScope.NonPublicMembersNamesMapping.ContainsKey(idName))
                        {
                            var name = LambdaHelper.GetNameForNonPublicMember(idName);
                            var semClassField = (class_field) si.sym_info;
                            var pn = new common_property_node(name, _visitor.context._ctn, null, field_access_level.fal_public, semClassField.polymorphic_state);
                            pn.internal_property_type = _visitor.convert_strong(id).type;
                            clScope.NonPublicMembersNamesMapping.Add(idName, new Tuple<string, class_field, semantic_node>(name, semClassField, pn));
                        }
                    }
                }

                var idRef = (selfWordInClass ? _classScope : scope)
                    .VariablesDefinedInScope
                    .Find(var => var.SymbolInfo == si);

                if (idRef == null)                                     //TODO: Осторожнее переделать
                {
                    {
                        return;
                    }
                } 

                if (_currentTreeNode.CorrespondingSyntaxTreeNode != null)
                {
                    var varName = ((IVAriableDefinitionNode)idRef.SymbolInfo.sym_info).name;                      //TODO: случай параметров и полей класса!!!!!!!!!!!!!!!!!!
                    var substKey = new SubstitutionKey(varName, idRef.SyntaxTreeNodeWithVarDeclaration, _currentTreeNode.CorrespondingSyntaxTreeNode);

                    if (!_identsReferences.ContainsKey(substKey))
                    {
                        _identsReferences.Add(substKey, new List<ident>());
                    }

                    _identsReferences[substKey].Add(id);
                }

                if (InLambdaContext && scope is CapturedVariablesTreeNodeLambdaScope &&
                    scopeIndex < _currentLambdaScopeNodeStack.Peek().ScopeIndex) //TODO: Захват параметров лямбды в другой лямбде
                {
                    _visitor.AddError(new ThisTypeOfVariablesCannotBeCaptured(_visitor.get_location(id)));
                    return;
                }

                if (!InLambdaContext || 
                    scopeIndex >= _currentLambdaScopeNodeStack.Peek().ScopeIndex)
                {
                    return;
                }

                var stackAsList = _currentLambdaScopeNodeStack.ToList();
                stackAsList.RemoveAt(0);

                if (!_currentLambdaScopeNodeStack.Peek().CapturedVarsSymbolInfo.Contains(si))
                {
                    _currentLambdaScopeNodeStack.Peek().CapturedVarsSymbolInfo.Add(si);
                    foreach (var capturedVariablesTreeNodeLambdaScope in stackAsList)
                    {
                        if (!capturedVariablesTreeNodeLambdaScope.CapturedVarsSymbolInfo.Contains(si))
                        {
                            capturedVariablesTreeNodeLambdaScope.CapturedVarsSymbolInfo.Add(si);
                        }
                    }
                }
                if (!idRef.ReferencingLambdas.Contains(_currentLambdaScopeNodeStack.Peek()))
                {
                    idRef.ReferencingLambdas.Add(_currentLambdaScopeNodeStack.Peek());
                    foreach (var capturedVariablesTreeNodeLambdaScope in stackAsList)
                    {
                        if (!idRef.ReferencingLambdas.Contains(capturedVariablesTreeNodeLambdaScope))
                        {
                            idRef.ReferencingLambdas.Add(capturedVariablesTreeNodeLambdaScope);
                        }
                    }
                }
            }
        }

        public override void visit(assign _assign)
        {
            var loc = _visitor.get_location(_assign);

            _visitor.internal_is_assign = true;
            var to = _visitor.convert_address_strong(_assign.to);
            _visitor.internal_is_assign = false;
            if (to == null)
                _visitor.AddError(_visitor.get_location(_assign.to), "CAN_NOT_ASSIGN_TO_LEFT_PART");

            bool flag;
            var node_type = general_node_type.constant_definition;
            if (convertion_data_and_alghoritms.check_for_constant_or_readonly(to, out flag, out node_type))
            {
                if (flag)
                    _visitor.AddError(to.location, "CAN_NOT_ASSIGN_TO_CONSTANT_OBJECT");
                else
                    _visitor.AddError(new CanNotAssignToReadOnlyElement(to.location, node_type));
            }

            if (to is class_field_reference)
            {
                if ((to as class_field_reference).obj.type.type_special_kind == type_special_kind.record &&
                    (to as class_field_reference).obj is base_function_call)
                {
                    //исключим ситуацию обращения к массиву
                    if (!(((to as class_field_reference).obj is common_method_call) &&
                    ((to as class_field_reference).obj as common_method_call).obj.type.type_special_kind == type_special_kind.array_wrapper))
                        _visitor.AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");
                }
            }
            else if (_visitor.context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.namespace_variable_reference)
            {
                if (_visitor.context.is_loop_variable((to as namespace_variable_reference).var))
                    _visitor.AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (_visitor.context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.local_variable_reference)
            {
                if (_visitor.context.is_loop_variable((to as local_variable_reference).var))
                    _visitor.AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (_visitor.context.is_in_cycle() && !SemanticRules.AllowChangeLoopVariable && to.semantic_node_type == semantic_node_type.local_block_variable_reference)
            {
                if (_visitor.context.is_loop_variable((to as local_block_variable_reference).var))
                    _visitor.AddError(to.location, "CANNOT_ASSIGN_TO_LOOP_VARIABLE");
            }
            else if (to is simple_array_indexing)
                if ((to as simple_array_indexing).simple_arr_expr is class_field_reference && ((to as simple_array_indexing).simple_arr_expr as class_field_reference).obj != null &&
                   ((to as simple_array_indexing).simple_arr_expr as class_field_reference).obj is constant_node)
                    _visitor.AddError(loc, "LEFT_SIDE_CANNOT_BE_ASSIGNED_TO");

            ProcessNode(_assign.to);
            ProcessNode(_assign.from);
        }

        public override void visit(dot_node dn)
        {
            ProcessNode(dn.left);

            if (!(dn.right is ident))
            {
                ProcessNode(dn.right);
            }
        }

        public override void visit(unnamed_type_object uto) // SSM 26/08/15 - Роман подсказал
        {
            ProcessNode(uto.ne_list);
        }

        public override void visit(foreach_stmt _foreach_stmt)
        {
            var loopIdentName = _foreach_stmt.identifier.name.ToLower();

            definition_node dn = null;
            var_definition_node vdn = null;

            var sl2 = new statements_list(_visitor.get_location(_foreach_stmt));
            _visitor.convertion_data_and_alghoritms.statement_list_stack_push(sl2);

            var newTreeNode = new CapturedVariablesTreeNodeForEachScope(_currentTreeNode, sl2.Scope.ScopeNum, _foreach_stmt);

            if (_currentTreeNode != null)
            {
                _currentTreeNode.ChildNodes.Add(newTreeNode);
            }
            _currentTreeNode = newTreeNode;

            _scopesCapturedVarsNodesDictionary.Add(sl2.Scope.ScopeNum, _currentTreeNode);

            var inWhat = _visitor.convert_strong(_foreach_stmt.in_what);
            var tmp = inWhat;
            if (inWhat is typed_expression)
                inWhat = _visitor.convert_typed_expression_to_function_call(inWhat as typed_expression);

            type_node elemType = null;
            if (inWhat.type == null)
                inWhat = tmp;
            bool bb; // здесь bb не нужно. Оно нужно в foreach
            _visitor.FindIEnumerableElementType(/*_foreach_stmt, */inWhat.type, ref elemType, out bb);

            if (_foreach_stmt.type_name == null)
            {
                var loc1 = _visitor.get_location(_foreach_stmt.identifier);
                dn = _visitor.context.check_name_node_type(loopIdentName, loc1, general_node_type.variable_node);
                vdn = (var_definition_node)dn;
            }
            else
            {
                vdn = _visitor.context.add_var_definition(loopIdentName, _visitor.get_location(_foreach_stmt.identifier));

                type_node tn;
                if (_foreach_stmt.type_name is no_type_foreach)
                {
                    tn = elemType;
                }
                else
                {
                    tn = _visitor.convert_strong(_foreach_stmt.type_name);
                    _visitor.check_for_type_allowed(tn, _visitor.get_location(_foreach_stmt.type_name));
                }


                _visitor.context.close_var_definition_list(tn, null);

                _currentTreeNode.VariablesDefinedInScope.Add(new CapturedVariablesTreeNode.CapturedSymbolInfo(_foreach_stmt, _visitor.context.find_first(loopIdentName)));
            }

            newTreeNode.SymbolInfoLoopVar = _visitor.context.find_first(loopIdentName);
            
            if (!(vdn.type is compiled_generic_instance_type_node))
                _visitor.convertion_data_and_alghoritms.check_convert_type_with_inheritance(vdn.type, elemType, _visitor.get_location(_foreach_stmt.identifier));

            var fn = new foreach_node(vdn, inWhat, null, _visitor.get_location(_foreach_stmt));
            _visitor.context.cycle_stack.push(fn);
            _visitor.context.loop_var_stack.Push(vdn);

            ProcessNode(_foreach_stmt.in_what);

            if (!(_foreach_stmt.stmt is statement_list))
            {
                var stmtList = new statement_list(_foreach_stmt.stmt, _foreach_stmt.stmt.source_context);
                _foreach_stmt.stmt = stmtList;
            }

            ProcessNode(_foreach_stmt.stmt);

            _visitor.context.loop_var_stack.Pop();
            _visitor.convertion_data_and_alghoritms.statement_list_stack.pop();
            _visitor.context.cycle_stack.pop();

            _currentTreeNode = _currentTreeNode.ParentNode;
        }

        public override void visit(PascalABCCompiler.SyntaxTree.for_node _for_node)
        {
            var loc1 = _visitor.get_location(_for_node.loop_variable);
            var loopIdentName = _for_node.loop_variable.name.ToLower();
            var nodesToProcess = new List<syntax_tree_node>();

            var_definition_node vdn;
            var initv = _visitor.convert_strong(_for_node.initial_value);
            var tmp = initv;

            if (initv is typed_expression)
            {
                initv = _visitor.convert_typed_expression_to_function_call(initv as typed_expression);
            }

            if (initv.type == null)
            {
                initv = tmp;
            }

            var headStmts = new statements_list(loc1);
            _visitor.convertion_data_and_alghoritms.statement_list_stack_push(headStmts);

            var newTreeNode = new CapturedVariablesTreeNodeForScope(_currentTreeNode, headStmts.Scope.ScopeNum, _for_node);

            if (_currentTreeNode != null)
            {
                _currentTreeNode.ChildNodes.Add(newTreeNode);
            }
            _currentTreeNode = newTreeNode;

            _scopesCapturedVarsNodesDictionary.Add(headStmts.Scope.ScopeNum, _currentTreeNode);


            if (_for_node.type_name == null && !_for_node.create_loop_variable)
            {
                var dn = _visitor.context.check_name_node_type(loopIdentName, loc1, general_node_type.variable_node);
                vdn = (var_definition_node)dn;
                nodesToProcess.Add(_for_node.loop_variable);
            }
            else
            {
                var tn = _for_node.type_name != null ? _visitor.convert_strong(_for_node.type_name) : initv.type;
                vdn = _visitor.context.add_var_definition(loopIdentName,
                                                          _visitor.get_location(_for_node.loop_variable), tn,
                                                          polymorphic_state.ps_common);

                _currentTreeNode.VariablesDefinedInScope.Add(new CapturedVariablesTreeNode.CapturedSymbolInfo(_for_node, _visitor.context.find_first(loopIdentName)));
            }


            newTreeNode.SymbolInfoLoopVar = _visitor.context.find_first(loopIdentName);
            
            var fn = new PascalABCCompiler.TreeRealization.for_node(null, null, null, null, null, _visitor.get_location(_for_node));
            if (vdn.type == SystemLibrary.bool_type)
            {
                fn.bool_cycle = true;
            }
            _visitor.context.cycle_stack.push(fn);
            _visitor.context.loop_var_stack.Push(vdn);

            nodesToProcess.Add(_for_node.initial_value);
            nodesToProcess.Add(_for_node.finish_value);
            nodesToProcess.Add(_for_node.increment_value);

            foreach (var n in nodesToProcess)
            {
                ProcessNode(n);
            }

            if (!(_for_node.statements is statement_list))
            {
                var stmtList = new statement_list(_for_node.statements, _for_node.statements.source_context);
                _for_node.statements = stmtList;
            }
            ProcessNode(_for_node.statements);

            _visitor.context.cycle_stack.pop();
            _visitor.context.loop_var_stack.Pop();
            _visitor.convertion_data_and_alghoritms.statement_list_stack.pop();

            _currentTreeNode = _currentTreeNode.ParentNode;
        }

        public override void visit(function_lambda_definition lambdaDefinition)
        {   
            if (lambdaDefinition.formal_parameters != null && lambdaDefinition.formal_parameters.params_list != null && lambdaDefinition.formal_parameters.params_list.Count != 0)
            {
                var varDefsList = new List<statement>();
                for (var i = 0; i < lambdaDefinition.formal_parameters.params_list.Count; i++)
                {
                    var varType = lambdaDefinition.formal_parameters.params_list[i].vars_type is lambda_inferred_type ?
                        LambdaHelper.ConvertSemanticTypeToSyntaxType((type_node)((lambda_inferred_type)lambdaDefinition.formal_parameters.params_list[i].vars_type).real_type) :
                        lambdaDefinition.formal_parameters.params_list[i].vars_type;
                    
                    for (var j = 0; j < lambdaDefinition.formal_parameters.params_list[i].idents.idents.Count; j++)
                    {
                        var varName = "<>" + lambdaDefinition.formal_parameters.params_list[i].idents.idents[j].name;
                        var vds = new var_def_statement(new ident(lambdaDefinition.formal_parameters.params_list[i].idents.idents[j].name), varType);
                        vds.inital_value = new ident(varName);
                        lambdaDefinition.formal_parameters.params_list[i].idents.idents[j].name = varName;
                        varDefsList.Add(new var_statement(vds));
                    }
                }

                ((statement_list)lambdaDefinition.proc_body).subnodes.InsertRange(0, varDefsList);
            }

            var procDecl = LambdaHelper.ConvertLambdaNodeToProcDefNode(lambdaDefinition);
            _visitor.body_exists = true;
            _visitor.hard_node_test_and_visit(procDecl.proc_header);
            
            var newTreeNode = new CapturedVariablesTreeNodeLambdaScope(_currentTreeNode, 
                lambdaDefinition, 
                _visitor.context.func_stack.top().scope.ScopeNum, lambdaDefinition);

            _currentTreeNode.LambdasDefinedInScope.Add(newTreeNode);
            _currentLambdaScopeNodeStack.Push(newTreeNode);

            if (_currentTreeNode != null)
            {
                _currentTreeNode.ChildNodes.Add(newTreeNode);
            }
            _currentTreeNode = newTreeNode;
            _scopesCapturedVarsNodesDictionary.Add(_currentTreeNode.ScopeIndex, _currentTreeNode);

            VisitProcParameters(procDecl.proc_header.parameters);
            ProcessNode(procDecl.proc_body);

            _visitor.context.leave_block();
            _visitor.context.remove_lambda_function(procDecl.proc_header.name.meth_name.name, false);

            _currentTreeNode = _currentTreeNode.ParentNode;
            _currentLambdaScopeNodeStack.Pop();

            LambdaHelper.RemoveLambdaInfoFromCompilationContext(_visitor.context, lambdaDefinition);
        }

        public override void visit(name_assign_expr nae) // Не надо захватывать явные имена полей в безымянных классах 
        {
            // ProcessNode(nae.name); - пропустить!
            ProcessNode(nae.expr);
        }

        public override void visit(with_statement _with_statement)
        {
            _visitor.AddError(_visitor.get_location(_with_statement), "WITH_AND_LAMBDAS_NOT_ALLOWED");
        }
        
        public override void visit(PascalABCCompiler.SyntaxTree.goto_statement _goto_statement)
        {
        	if (_goto_statement.source_context != null)
            	_visitor.AddError(_visitor.get_location(_goto_statement), "GOTO_AND_LAMBDAS_NOT_ALLOWED");
        	else
        		base.visit(_goto_statement);
        }
        
        private void VisitProcParameters(formal_parameters procParametres)
        {
            if (procParametres == null ||
                procParametres.params_list == null)
            {
                return;
            }

            foreach (var tp in procParametres.params_list)
            {
                foreach (var id in tp.idents.idents)
                {
                    SymbolInfo si = _visitor.context.find_first(id.name);
                    _currentTreeNode.VariablesDefinedInScope.Add(new CapturedVariablesTreeNode.CapturedSymbolInfo(tp, si));
                }
            }
        }

        private void CreateNodesForUpperScopesIfExist()
        {
            if (_visitor.context._ctn != null)
            {
                _rootNode = new CapturedVariablesTreeNodeClassScope(null, _visitor.context._ctn.Scope.ScopeNum, null, _visitor.context._ctn.name);
                _currentTreeNode = _rootNode;

                _scopesCapturedVarsNodesDictionary.Add(_visitor.context._ctn.Scope.ScopeNum, _currentTreeNode);

                _classScope = (CapturedVariablesTreeNodeClassScope) _rootNode;
            }

            if (_visitor.context.func_stack.top() != null)
            {
                var newTreeNode = new CapturedVariablesTreeNodeProcedureScope(_currentTreeNode, _visitor.context.func_stack.top(), _visitor.context.func_stack.top().scope.ScopeNum, null);

                if (_rootNode == null)
                {
                    _rootNode = newTreeNode;
                }
                if (_currentTreeNode != null)
                {
                    _currentTreeNode.ChildNodes.Add(newTreeNode);
                }
                _currentTreeNode = newTreeNode;

                _scopesCapturedVarsNodesDictionary.Add(_visitor.context.func_stack.top().scope.ScopeNum, _currentTreeNode);

                _procedureScope = (CapturedVariablesTreeNodeProcedureScope) _currentTreeNode;
            }
        }

        private CapturedVariablesTreeNode BuildTreeForStatementList(statement_list statementList)
        {
            CreateNodesForUpperScopesIfExist();

            ProcessNode(statementList);

            return _rootNode;
        }

        public CapturedVariablesInfo BuildTree(statement_list statementList)
        {
            return new CapturedVariablesInfo
                {
                    RootNode = BuildTreeForStatementList(statementList),
                    IdentsReferences = _identsReferences,
                    CapturedVarsNodesDictionary = _scopesCapturedVarsNodesDictionary,
                    ProcedureScope = _procedureScope
                };
        }
    }
}