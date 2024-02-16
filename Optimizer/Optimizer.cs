// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using System.Text;
using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler
{
    

    public class Optimizer
    {
        private List<Errors.CompilerWarning> warns = new List<Errors.CompilerWarning>();
        private OptimizerHelper helper = new OptimizerHelper();
        private bool extended_mode = true;
        private common_type_node current_type;
        private bool condition_block = false;
        private bool has_returns = false;
        private bool has_goto = false;
        private Dictionary<common_method_node, common_method_node> processed_methods = new Dictionary<common_method_node, common_method_node>();
        private bool no_infinite_recursion;

        public Optimizer()
        {
        }

        public List<Errors.CompilerWarning> Optimize(program_node p)
        {
            try
            {
                foreach (common_namespace_node cnn in p.common_namespaces)
                {
                    VisitCommonNamespaceHeader(cnn);
                }
                foreach (common_namespace_node cnn in p.common_namespaces)
                {
                    VisitCommonNamespaceNode(cnn);
                }
                if (p.main_function != null)
                    VisitStatement(p.main_function.function_code);
                foreach (common_namespace_node cnn in p.common_namespaces)
                {
                    CollectInfoNamespaces(cnn);
                }
                return warns;
            }
            catch (Exception e)
            {
                return warns;
            }
        }

        private void AddWarning(string message, location loc)
        {
            warns.Add(new GenericWarning(message, loc));
        }
        private void AddWarningAssignWithoutUsing(string name, location loc)
        {
            if (name.Contains("$"))
                return;
            warns.Add(new AssignWithoutUsing(name,loc));
        }

        private void AddHint(string message, location loc)
        {
            warns.Add(new GenericHint(message, loc));
        }

        private void VisitCommonNamespaceHeader(common_namespace_node cnn)
        {
            if (!cnn.from_pcu)
            {
                foreach (namespace_variable vdn in cnn.variables)
                {
                    helper.AddVariable(vdn);
                    CheckType(vdn.type, vdn.inital_value, vdn.loc);
                    if (vdn.inital_value != null)
                        VisitExpression(vdn.inital_value);
                }
            }
        }

        private bool isUnused(VarInfo vi, var_definition_node vdn)
        {
            return vi.num_use == 0 && !vdn.is_special_name && !vdn.name.StartsWith("#");
        }

        private void CollectInfoNamespaces(common_namespace_node cnn)
        {
            if (!cnn.from_pcu)
            {
                if (cnn.is_main)
                foreach (var_definition_node vdn2 in cnn.variables)
                {
                	if (vdn2 is namespace_variable)
                	{
                		namespace_variable vdn = vdn2 as namespace_variable;
                		VarInfo vi = helper.GetVariable(vdn);
                    	if (isUnused(vi, vdn)) warns.Add(new UnusedVariable(vdn.name, vdn.loc));
                   	 	if (vi.num_ass > 0 && vi.act_num_use == 0) AddWarningAssignWithoutUsing(vdn.name, vi.last_ass_loc);
                    //if (vi.num_ass == 0 && vi.act_num_use > 0) helper.AddRealWarning(vdn, warns);
                	}
                	else if (vdn2 is local_block_variable)
                	{
                		local_block_variable vdn = vdn2 as local_block_variable;
                		VarInfo vi = helper.GetVariable(vdn);
                        if (isUnused(vi, vdn)) warns.Add(new UnusedVariable(vdn.name, vdn.loc));
                    	if (vi.num_ass > 0 && vi.act_num_use == 0) AddWarningAssignWithoutUsing(vdn.name, vi.last_ass_loc);
                	}
                }
                foreach (common_type_node ctn in cnn.types)
                    CollectInfoFields(ctn);
            }
        }

        private void VisitCommonNamespaceNode(common_namespace_node cnn)
        {
            if (!cnn.from_pcu)
            {
                foreach (common_type_node ctn in cnn.types)
                    VisitCommonTypeHeader(ctn);

                foreach (common_type_node ctn in cnn.types)
                    VisitCommonType(ctn);

                foreach (common_namespace_function_node cnfn in cnn.functions)
                {
                    VisitCommonNamespaceFunctionNode(cnfn);
                }
            }
        }

        private void VisitCommonType(common_type_node ctn)
        {
            foreach (common_method_node cmn in ctn.methods)
            {
                VisitMethod(cmn);
            }
        }

        private void VisitMethod(common_method_node cmn)
        {
            if (processed_methods.ContainsKey(cmn))
                return;
            processed_methods.Add(cmn, cmn);
            if (extended_mode)
                VisitVariables(cmn.var_definition_nodes_list);
            foreach (var_definition_node vdn in cmn.var_definition_nodes_list)
                helper.AddVariable(vdn);
            foreach (common_parameter prm in cmn.parameters)
                helper.AddParameter(prm);
            foreach (common_in_function_function_node nested in cmn.functions_nodes_list)
                VisitNestedFunction(nested);
            current_function = cmn;
            has_returns = false;
            has_goto = false;
            VisitStatement(cmn.function_code);
            foreach (var_definition_node vdn2 in cmn.var_definition_nodes_list)
            {
            	if (vdn2 is local_variable)
            	{
            		local_variable vdn = vdn2 as local_variable;
            		VarInfo vi = helper.GetVariable(vdn);
                    if (isUnused(vi, vdn))
                        warns.Add(new UnusedVariable(vdn.name, vdn.loc));
                	else if (vi.num_ass == 0 && 
                            vdn.is_ret_value && 
                            !cmn.name.StartsWith("<") && 
                            !cmn.cont_type.IsInterface && 
                            cmn.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract &&
                            cmn.return_value_type != null && 
                            !cmn.return_value_type.IsPointer && 
                            !cmn.is_constructor && 
                            cmn.return_value_type.type_special_kind != SemanticTree.type_special_kind.record && 
                            cmn.return_value_type.type_special_kind != SemanticTree.type_special_kind.array_wrapper
                            && !helper.IsExternal(cmn))
                        warns.Add(new UndefinedReturnValue(cmn.name, cmn.function_code.location));
            	}
            	else if (vdn2 is local_block_variable)
            	{
            		local_block_variable vdn = vdn2 as local_block_variable;
            		VarInfo vi = helper.GetVariable(vdn);
                    if (isUnused(vi, vdn))
                        warns.Add(new UnusedVariable(vdn.name, vdn.loc));
                    else if (vi.num_ass == 0 && 
                            vdn.is_ret_value && 
                            !cmn.name.StartsWith("<") && 
                            !cmn.cont_type.IsInterface && 
                            cmn.polymorphic_state != SemanticTree.polymorphic_state.ps_virtual_abstract && 
                            cmn.return_value_type != null && 
                            !cmn.return_value_type.IsPointer && 
                            !cmn.is_constructor && 
                            cmn.return_value_type.type_special_kind != SemanticTree.type_special_kind.record && 
                            cmn.return_value_type.type_special_kind != SemanticTree.type_special_kind.array_wrapper
                            && !helper.IsExternal(cmn))
                        warns.Add(new UndefinedReturnValue(cmn.name, cmn.function_code.location));
            	}
            	if (vdn2.inital_value != null)
                    VisitExpression(vdn2.inital_value);
            }
            foreach (common_parameter prm in cmn.parameters)
            {
                ParamInfo vi = helper.GetParameter(prm);
                if (prm.inital_value != null) VisitExpression(prm.inital_value);
                //if (vi.num_use == 0 && !prm.name.Contains("$")) warns.Add(new UnusedParameter(prm.name, prm.loc));
            }
            foreach (var_definition_node vdn in cmn.var_definition_nodes_list)
            {
            	VarInfo vi = helper.GetVariable(vdn);
            	if (vdn.inital_value != null) VisitExpression(vdn.inital_value);
            }
        }

        private void VisitCommonTypeHeader(common_type_node ctn)
        {
            if (ctn.base_type != null && ctn.base_type is common_type_node)
                VisitCommonTypeHeader(ctn.base_type as common_type_node);
            current_type = ctn;
            foreach (class_field fld in ctn.fields)
            {
                helper.AddField(fld);
                CheckType(fld.type, fld.inital_value, fld.loc);
            }
            current_type = null;    
        }

        private void CollectInfoFields(common_type_node ctn)
        {
            if (!ctn.comprehensive_namespace.from_pcu)
                foreach (class_field fld in ctn.fields)
                {
                    FldInfo fi = helper.GetField(fld);
                   // if (fi.num_use == 0 && !ctn.name.Contains("$") && fld.field_access_level == SemanticTree.field_access_level.fal_private) warns.Add(new UnusedField(fld.name, fld.loc));
                }
        }

        private common_function_node current_function=null;

        private void CheckType(type_node type, expression_node initial_value, location loc)
        {
            /*if (type.IsPointer && type.element_type.is_value_type && type.element_type is common_type_node)
            {
                AddHint("DO_NOT_USE_POINTERS_TO_RECORDS", loc);
            }*/
            if (type.type_special_kind == SemanticTree.type_special_kind.short_string && !(current_type != null && current_type.is_value))
            {
                AddHint("DO_NOT_USE_SHORT_STRINGS", loc);
            }
            else if (type.type_special_kind == SemanticTree.type_special_kind.array_kind || type.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
            {
                CheckType(type.element_type, initial_value, loc);
            }
        }

        private void VisitVariables(local_variable_list var_list)
        {
            foreach (local_variable lv in var_list)
            {
                CheckType(lv.type, lv.inital_value, lv.loc);
                if (lv.inital_value != null)
                    VisitExpression(lv.inital_value);
            }
        }

        private void VisitCommonNamespaceFunctionNode(common_namespace_function_node cnfn)
        {
            if (extended_mode)
                VisitVariables(cnfn.var_definition_nodes_list);
            foreach (var_definition_node vdn in cnfn.var_definition_nodes_list)
                helper.AddVariable(vdn);
            foreach (common_parameter prm in cnfn.parameters)
                helper.AddParameter(prm);
            foreach (common_in_function_function_node nested in cnfn.functions_nodes_list)
                VisitNestedFunction(nested);
            current_function = cnfn;
            has_returns = false;
            has_goto = false;
            VisitStatement(cnfn.function_code);
            foreach (var_definition_node vdn2 in cnfn.var_definition_nodes_list)
            {
            	if (vdn2 is local_variable)
            	{
            		local_variable vdn = vdn2 as local_variable;
            		VarInfo vi = helper.GetVariable(vdn);
                    if (isUnused(vi, vdn))
                        warns.Add(new UnusedVariable(vdn.name, vdn.loc));
                    else if (vi.num_ass == 0 && 
                            vdn.is_ret_value && 
                            !cnfn.name.StartsWith("<") && 
                            cnfn.return_value_type != null && 
                            cnfn.return_value_type.type_special_kind != SemanticTree.type_special_kind.record && 
                            !cnfn.return_value_type.IsPointer && 
                            cnfn.return_value_type.type_special_kind != SemanticTree.type_special_kind.array_wrapper && 
                            !helper.IsExternal(cnfn))
                        warns.Add(new UndefinedReturnValue(cnfn.name, cnfn.function_code.location));
                	if (vi.num_ass > 0 && vi.act_num_use == 0 && !vdn.is_special_name)
                        AddWarningAssignWithoutUsing(vdn.name, vi.last_ass_loc);
            	}
            	else if (vdn2 is local_block_variable)
            	{
            		local_block_variable vdn = vdn2 as local_block_variable;
            		VarInfo vi = helper.GetVariable(vdn);
                    if (isUnused(vi, vdn))
                        warns.Add(new UnusedVariable(vdn.name, vdn.loc));
                    else if (vi.num_ass == 0 && 
                             vdn.is_ret_value && 
                             !cnfn.name.StartsWith("<") && 
                             cnfn.return_value_type != null && 
                             cnfn.return_value_type.type_special_kind != SemanticTree.type_special_kind.record && 
                             !cnfn.return_value_type.IsPointer && 
                             cnfn.return_value_type.type_special_kind != SemanticTree.type_special_kind.array_wrapper && 
                             !helper.IsExternal(cnfn))
                        warns.Add(new UndefinedReturnValue(cnfn.name, cnfn.function_code.location));
                	if (vi.num_ass > 0 && vi.act_num_use == 0 && !vdn.is_special_name)
                        AddWarningAssignWithoutUsing(vdn.name, vi.last_ass_loc);
            	}
            	if (vdn2.inital_value != null)
                    VisitExpression(vdn2.inital_value);
            }
            foreach (common_parameter prm in cnfn.parameters)
            {
                ParamInfo vi = helper.GetParameter(prm);
                if (prm.inital_value != null)
                    VisitExpression(prm.inital_value);
                //if (vi.num_use == 0 && !prm.name.Contains("$")) warns.Add(new UnusedParameter(prm.name, prm.loc));
            }
            foreach (var_definition_node vdn in cnfn.var_definition_nodes_list)
            {
            	VarInfo vi = helper.GetVariable(vdn);
            	if (vdn.inital_value != null)
                    VisitExpression(vdn.inital_value);
            }
        }

        private void VisitNestedFunction(common_in_function_function_node cnfn)
        {
            if (extended_mode)
                VisitVariables(cnfn.var_definition_nodes_list);
            foreach (var_definition_node vdn in cnfn.var_definition_nodes_list)
                helper.AddVariable(vdn);
            foreach (common_parameter prm in cnfn.parameters)
                helper.AddParameter(prm);
            foreach (common_in_function_function_node nested in cnfn.functions_nodes_list)
                VisitNestedFunction(nested);
            current_function = cnfn;
            has_returns = false;
            has_goto = false;
            VisitStatement(cnfn.function_code);
            foreach (var_definition_node vdn2 in cnfn.var_definition_nodes_list)
            {
            	if (vdn2 is local_variable)
            	{
            		local_variable vdn = vdn2 as local_variable;
            		VarInfo vi = helper.GetVariable(vdn);
                    if (isUnused(vi, vdn))
                        warns.Add(new UnusedVariable(vdn.name, vdn.loc));
                    else if (vi.num_ass == 0 && 
                             vdn.is_ret_value && 
                             !cnfn.name.StartsWith("<") && 
                             cnfn.return_value_type.type_special_kind != SemanticTree.type_special_kind.record && 
                             !cnfn.return_value_type.IsPointer && 
                             cnfn.return_value_type.type_special_kind != SemanticTree.type_special_kind.array_wrapper && 
                             !helper.IsExternal(cnfn))
                        warns.Add(new UndefinedReturnValue(cnfn.name, cnfn.function_code.location));
                	if (vi.num_ass > 0 && vi.act_num_use == 0 && !vdn.is_special_name)
                        AddWarningAssignWithoutUsing(vdn.name, vi.last_ass_loc);
            	}
            	else if (vdn2 is local_block_variable)
            	{
            		local_block_variable vdn = vdn2 as local_block_variable;
            		VarInfo vi = helper.GetVariable(vdn);
                    if (isUnused(vi, vdn))
                        AddWarningAssignWithoutUsing(vdn.name, vdn.loc);
                    else if (vi.num_ass == 0 && 
                             vdn.is_ret_value && 
                             !cnfn.name.StartsWith("<") && 
                             cnfn.return_value_type.type_special_kind != SemanticTree.type_special_kind.record && 
                             !cnfn.return_value_type.IsPointer && 
                             cnfn.return_value_type.type_special_kind != SemanticTree.type_special_kind.array_wrapper && 
                             !helper.IsExternal(cnfn))
                        warns.Add(new UndefinedReturnValue(cnfn.name, cnfn.function_code.location));
                	if (vi.num_ass > 0 && vi.act_num_use == 0 && !vdn.is_special_name)
                        AddWarningAssignWithoutUsing(vdn.name, vi.last_ass_loc);
            	}
            	if (vdn2.inital_value != null)
                    VisitExpression(vdn2.inital_value);
            }
            foreach (common_parameter prm in cnfn.parameters)
            {
                ParamInfo vi = helper.GetParameter(prm);
                if (prm.inital_value != null)
                    VisitExpression(prm.inital_value);
                //if (vi.num_use == 0 && !prm.name.Contains("$")) warns.Add(new UnusedParameter(prm.name, prm.loc));
            }
            foreach (var_definition_node vdn in cnfn.var_definition_nodes_list)
            {
            	VarInfo vi = helper.GetVariable(vdn);
            	if (vdn.inital_value != null)
                    VisitExpression(vdn.inital_value);
            }
        }

        private void VisitStatement(statement_node sn)
        {
            if (sn == null) return;
            if (!(sn is statements_list) && is_break_stmt && !has_goto && !(sn is empty_statement))
            {
                warns.Add(new UnreachableCodeDetected(sn.location));
                is_break_stmt = false;
            }
            if (sn is expression_node)
            {
                VisitExpression((expression_node)sn);
               
                return;
            }
            switch (sn.semantic_node_type)
            {
                case semantic_node_type.if_node: VisitIf((if_node)sn); break;
                case semantic_node_type.while_node: VisitWhile((while_node)sn); break;
                case semantic_node_type.repeat_node: VisitRepeat((repeat_node)sn); break;
                case semantic_node_type.for_node: VisitFor((for_node)sn); break;
                case semantic_node_type.statements_list: VisitStatementList((statements_list)sn); break;
                case semantic_node_type.empty_statement: VisitEmpty((empty_statement)sn); break;
                case semantic_node_type.return_node: VisitReturnNode((return_node)sn); break;
                case semantic_node_type.switch_node: VisitSwitchNode((switch_node)sn); break;
                case semantic_node_type.external_statement_node: VisitExternalStatementNode((external_statement)sn); break;
                case semantic_node_type.pinvoke_node: VisitPInvokeStatementNode((pinvoke_statement)sn); break;
                case semantic_node_type.throw_statement: VisitThrow((throw_statement_node)sn); break;
                case semantic_node_type.runtime_statement: VisitRuntimeStatement((runtime_statement)sn); break;
                case semantic_node_type.try_block: VisitTryBlock((try_block)sn); break;
                case semantic_node_type.labeled_statement: VisitLabeledStatement((labeled_statement)sn); break;
                case semantic_node_type.goto_statement: VisitGoto((goto_statement)sn); break;
                case semantic_node_type.foreach_node: VisitForeach((foreach_node)sn); break;
                case semantic_node_type.lock_statement: VisitLockStatement((lock_statement)sn); break;
            }
           
        }

        enum RetVal
        {
            True,
            False,
            Undef
        }

        private RetVal GetConstantValue(expression_node en)
        {
            if (en is bool_const_node)
            {
                if ((en as bool_const_node).constant_value) return RetVal.True;
                else return RetVal.False;
            }
            else return RetVal.Undef;
        }

        private Stack<bool> condition_block_states = new Stack<bool>();

        private void CheckInfiniteRecursion(common_namespace_function_call cnfc)
        {
            //if (!condition_block && !has_returns && cnfc.function_node == current_function && !no_infinite_recursion)
            //    warns.Add(new InfiniteRecursion(cnfc.location));
        }

        private void CheckInfiniteRecursion(common_static_method_call cnfc)
        {
            //if (!condition_block && !has_returns && cnfc.function_node == current_function && !no_infinite_recursion)
            //    warns.Add(new InfiniteRecursion(cnfc.location));
        }

        private void SaveConditionBlock()
        {
            condition_block_states.Push(condition_block);
            condition_block = false;
        }

        private void RestoreConditionBlock()
        {
            condition_block = condition_block_states.Pop();
        }

        private void EnterConditionBlock()
        {
            condition_block = true;
        }

        private void VisitLockStatement(lock_statement stmt)
        {
            VisitExpression(stmt.lock_object);
            VisitStatement(stmt.body);
        }

        private void VisitForeach(foreach_node stmt)
        {
            SaveConditionBlock();
            IncreaseNumUseVar(stmt.ident);
        	VisitExpression(stmt.in_what);
            EnterConditionBlock();
            VisitStatement(stmt.what_do);
            RestoreConditionBlock();
        }

        private void VisitIf(if_node stmt)
        {
            SaveConditionBlock();
            RetVal rv = GetConstantValue(stmt.condition);
            VisitExpression(stmt.condition);
            if (rv == RetVal.False)
                is_break_stmt = true;
            if (rv != RetVal.True)
                EnterConditionBlock();
            VisitStatement(stmt.then_body);
            is_break_stmt = false;
            if (rv == RetVal.True)
                is_break_stmt = true;
            if (rv != RetVal.False)
                EnterConditionBlock();
            VisitStatement(stmt.else_body);
            is_break_stmt = false;
            RestoreConditionBlock();
        }

        private void VisitWhile(while_node stmt)
        {
            SaveConditionBlock();
            RetVal rv = GetConstantValue(stmt.condition);
            VisitExpression(stmt.condition);
            if (rv == RetVal.False)
                is_break_stmt = true;
            if (rv != RetVal.True)
                EnterConditionBlock();
            VisitStatement(stmt.body);
            RestoreConditionBlock();
        }

        private void VisitRepeat(repeat_node stmt)
        {
            SaveConditionBlock();
            RetVal rv = GetConstantValue(stmt.condition);
            if (rv != RetVal.False)
                EnterConditionBlock();
            VisitStatement(stmt.body);
            VisitExpression(stmt.condition);
            RestoreConditionBlock();
        }

        private void VisitFor(for_node stmt)
        {
            SaveConditionBlock();
            VisitStatement(stmt.init_while_expr);
            VisitStatement(stmt.initialization_statement);
            VisitStatement(stmt.increment_statement);
            VisitExpression(stmt.while_expr);
            EnterConditionBlock();
            VisitStatement(stmt.body);
            RestoreConditionBlock();
        }

        private bool is_break_stmt = false;

        private void VisitStatementList(statements_list stmt)
        {
            statement_node sn = null;
            bool tuple_decomp = false;
            foreach (local_block_variable lbv in stmt.local_variables)
            {
                if (lbv.name.StartsWith("#fpl") || lbv.name.StartsWith("#temp_var"))
                    tuple_decomp = true;
                helper.AddVariable(lbv);
                if (lbv.inital_value != null)
                    VisitExpression(lbv.inital_value);
                CheckType(lbv.type, lbv.inital_value, lbv.loc);
            }
            for (int i = 0; i < stmt.statements.Count; i++)
            {
                if (is_break_stmt && !has_goto && stmt.statements[i].semantic_node_type != semantic_node_type.empty_statement)
                    warns.Add(new UnreachableCodeDetected(stmt.statements[i].location));
                if (stmt.statements[i].semantic_node_type != semantic_node_type.empty_statement)
                    is_break_stmt = false;
                sn = stmt.statements[i];
                VisitStatement(sn);
                if (is_break_stmt && !has_goto && i < stmt.statements.Count - 1 && stmt.statements[i + 1].semantic_node_type != semantic_node_type.empty_statement && stmt.statements[i + 1].location != null)
                {
                    if (stmt.statements[i].location == null || stmt.statements[i + 1].location.ToString() != stmt.statements[i].location.ToString())
                        warns.Add(new UnreachableCodeDetected(stmt.statements[i + 1].location));
                }
                if (!(i < stmt.statements.Count - 1 && stmt.statements[i + 1].semantic_node_type == semantic_node_type.empty_statement))
                    is_break_stmt = false;
            }
            int tuples_num = 0;
            foreach (local_block_variable vdn in stmt.local_variables)
            {
                if (vdn.name.StartsWith("#fpl") || vdn.name.StartsWith("#temp_var"))
                {
                    tuple_decomp = true;
                    if (vdn.type is generic_instance_type_node)
                        tuples_num = (vdn.type as generic_instance_type_node).instance_params.Count;
                    else if (vdn.type is compiled_type_node)
                        tuples_num = (vdn.type as compiled_type_node).compiled_type.GetGenericArguments().Length;
                    continue;
                }
                else if (tuples_num > 0)
                {
                    tuples_num--;
                    continue;
                }    
                VarInfo vi = helper.GetVariable(vdn);
                if (isUnused(vi, vdn))
                    warns.Add(new UnusedVariable(vdn.name, vdn.loc));

                if (vi.num_ass > 0 && vi.act_num_use == 0 && !vdn.is_special_name)
                    AddWarningAssignWithoutUsing(vdn.name, vi.last_ass_loc);
            }
        }

        private void VisitExternalStatementNode(external_statement sn)
        {
            helper.MarkAsExternal(current_function);
        }
		
        private void VisitPInvokeStatementNode(pinvoke_statement sn)
        {
        	helper.MarkAsExternal(current_function);		
        }
        
        private void VisitSwitchNode(switch_node stmt)
        {
            SaveConditionBlock();
            VisitExpression(stmt.condition);
            EnterConditionBlock();
            for (int i = 0; i < stmt.case_variants.Count; i++)
            {
                VisitStatement(stmt.case_variants[i].case_statement);
                is_break_stmt = false;
            }
            VisitStatement(stmt.default_statement);
            RestoreConditionBlock();
        }

        private void VisitRuntimeStatement(runtime_statement sn)
        {
            
        }

        private void VisitReturnNode(return_node stmt)
        {
            VisitExpression(stmt.return_expr);
        }

        private void VisitEmpty(empty_statement stmt)
        {
            
        }

        private void VisitThrow(throw_statement_node stmt)
        {
            VisitExpression(stmt.excpetion);
            is_break_stmt = true;
            has_returns = true;
        }

        private void VisitTryBlock(try_block stmt)
        {
            VisitStatement(stmt.try_statements);
            for (int i = 0; i < stmt.filters.Count; i++)
            {
                if(stmt.filters[i].exception_var!=null)
                   IncreaseNumUseVar(stmt.filters[i].exception_var);
                VisitStatement(stmt.filters[i].exception_handler);
                is_break_stmt = false;
            }
            VisitStatement(stmt.finally_statements);
        }

        private void VisitLabeledStatement(labeled_statement stmt)
        {
            VisitStatement(stmt.statement);
        }

        private void VisitGoto(goto_statement stmt)
        {
        	has_goto = true;
        }
		
        private void IncreaseNumUseVar(var_definition_node lvr)
        {
            VarInfo vi = helper.GetVariable(lvr);
            vi.num_use++;
            vi.act_num_use++;
            vi.cur_use++;
            //if (vi.cur_ass == 0 && !lvr.var.name.Contains("$")) helper.AddTempWarning(lvr.var,new UseWithoutAssign(lvr.var.name, lvr.location));
        }
        
        private void IncreaseNumUseVar(local_variable_reference lvr)
        {
            VarInfo vi = helper.GetVariable(lvr.var);
            vi.num_use++;
            vi.act_num_use++;
            vi.cur_use++;
            //if (vi.cur_ass == 0 && !lvr.var.name.Contains("$")) helper.AddTempWarning(lvr.var,new UseWithoutAssign(lvr.var.name, lvr.location));
        }

        private void IncreaseNumAssVar(local_variable_reference lvr)
        {
            VarInfo vi = helper.GetVariable(lvr.var);
            vi.num_use++;
            vi.cur_ass++;
            vi.num_ass++;
            vi.last_ass_loc = lvr.location;
            if (vi.last_ass_loc == null && !lvr.var.is_ret_value) vi.num_ass--;
            if (vi.cur_use > 0) vi.cur_use--;

        }
		
        private void IncreaseNumAssVar(local_block_variable_reference lvr)
        {
            VarInfo vi = helper.GetVariable(lvr.var);
            vi.num_use++;
            vi.cur_ass++;
            vi.num_ass++;
            vi.last_ass_loc = lvr.location;
            if (vi.last_ass_loc == null) vi.num_ass--;
            if (vi.cur_use > 0) vi.cur_use--;

        }
        
        private void IncreaseNumUseVar(namespace_variable_reference lvr)
        {
            VarInfo vi = helper.GetVariable(lvr.var);
            if (vi == null) return;
            vi.num_use++;
            vi.act_num_use++;
            vi.cur_use++;
            //if (vi.cur_ass == 0 && !lvr.var.name.Contains("$")) helper.AddTempWarning(lvr.var,new UseWithoutAssign(lvr.var.name, lvr.location));
        }
		
        private void IncreaseNumUseVar(local_block_variable_reference lvr)
        {
            VarInfo vi = helper.GetVariable(lvr.var);
            if (vi == null)
            {
                helper.AddVariable(lvr.var);
                vi = helper.GetVariable(lvr.var);
            }
            if (vi == null) return;
            vi.num_use++;
            vi.act_num_use++;
            vi.cur_use++;
            //if (vi.cur_ass == 0 && !lvr.var.name.Contains("$")) helper.AddTempWarning(lvr.var,new UseWithoutAssign(lvr.var.name, lvr.location));
        }

        private void CheckVarParameter(expression_node expr, function_node func, int index)
        {
            if (index < func.parameters.Count && func.parameters[index].parameter_type == SemanticTree.parameter_type.var
                || (func is compiled_function_node && (func as compiled_function_node).cont_type.compiled_type == typeof(Array) && index == 0))
            {
                if (expr is local_block_variable_reference)
                    IncreaseNumAssVar(expr as local_block_variable_reference);
                else if (expr is local_variable_reference)
                    IncreaseNumAssVar(expr as local_variable_reference);
                else if (expr is namespace_variable_reference)
                    IncreaseNumAssVar(expr as namespace_variable_reference);
            }
        }
        
        private void IncreaseNumAssVar(namespace_variable_reference lvr)
        {
            VarInfo vi = helper.GetVariable(lvr.var);
            if (vi == null) return;
            vi.num_use++;
            vi.cur_ass++;
            vi.num_ass++;
            vi.last_ass_loc = lvr.location;
            if (vi.last_ass_loc == null) vi.num_ass--;
            if (vi.cur_use > 0) vi.cur_use--;

        }

        private void IncreaseNumUseParam(common_parameter_reference cpr)
        {
            ParamInfo vi = helper.GetParameter(cpr.par);
            if (vi == null) return;
            vi.num_use++;
            vi.cur_use++;
            //if (vi.cur_ass == 0) warns.Add(new UseWithoutAssign(lvr.var.name, lvr.location));
        }

        private void IncreaseNumAssParam(common_parameter_reference cpr)
        {
            ParamInfo vi = helper.GetParameter(cpr.par);
            if (vi == null) return;
            vi.num_use++;
            vi.cur_ass++;
            if (vi.cur_use > 0) vi.cur_use--;

        }

        private void IncreaseNumUseField(class_field_reference fld)
        {
            FldInfo vi = helper.GetField(fld.field);
            if (vi == null) return;
            vi.num_use++;
            vi.cur_use++;
            //if (vi.cur_ass == 0) warns.Add(new UseWithoutAssign(lvr.var.name, lvr.location));
        }

        private void IncreaseNumAssField(class_field_reference fld)
        {
            FldInfo vi = helper.GetField(fld.field);
            if (vi == null) 
                return;
            vi.num_use++;
            vi.cur_ass++;
            
            if (vi.cur_use > 0) vi.cur_use--;
        }

        private void IncreaseNumUseField(static_class_field_reference fld)
        {
            FldInfo vi = helper.GetField(fld.static_field);
            if (vi == null) return;
            vi.num_use++;
            vi.cur_use++;
            //if (vi.cur_ass == 0) warns.Add(new UseWithoutAssign(lvr.var.name, lvr.location));
        }

        private void IncreaseNumAssField(static_class_field_reference fld)
        {
            FldInfo vi = helper.GetField(fld.static_field);
            if (vi == null) return;
            vi.num_use++;
            vi.cur_ass++;
            if (vi.cur_use > 0) vi.cur_use--;
        }

        private void VisitExpression(expression_node en)
        {
            if (en == null) return;
            //WriteDebugInfo(en.location);
            switch (en.semantic_node_type)
            {
                case semantic_node_type.exit_procedure:
                    /*ничего писать не надо*/
                    has_returns = true;
                    break;
                case semantic_node_type.typeof_operator:
                    //VisitTypeOfOperator((typeof_operator)en); 
                    break;
                case semantic_node_type.statement_expression_node:
                    VisitStatementsExpressionNode((statements_expression_node)en); break;
                case semantic_node_type.question_colon_expression:
                    VisitQuestionColonExpression((question_colon_expression)en); break;
                case semantic_node_type.sizeof_operator:
                    VisitSizeOfOperator((sizeof_operator)en); break;
                case semantic_node_type.is_node:
                    VisitIsNode((is_node)en); break;
                case semantic_node_type.as_node:
                    VisitAsNode((as_node)en); break;
                case semantic_node_type.compiled_static_method_call_node_as_constant:
                    //VisitCompiledStaticMethodCallNodeAsConstant((compiled_static_method_call_as_constant)en); 
                    break;
                case semantic_node_type.array_initializer:
                    VisitArrayInitializer((array_initializer)en);
                    break;
                case semantic_node_type.record_initializer:
                    VisitRecordInitializer((record_initializer)en);
                    break;
                case semantic_node_type.array_const:
                    //VisitArrayConst((array_const)en);
                    break;
                case semantic_node_type.record_const:
                    //VisitRecordConst((record_constant)en); 
                    break;
                case semantic_node_type.float_const_node:
                    //VisitFloatConst((float_const_node)en); 
                    break;
                case semantic_node_type.byte_const_node:
                    //VisitByteConstNode((byte_const_node)en); 
                    break;
                case semantic_node_type.int_const_node:
                    //VisitIntConstNode((int_const_node)en); 
                    break;
                case semantic_node_type.sbyte_const_node:
                    //VisitSByteConstNode((sbyte_const_node)en); 
                    break;
                case semantic_node_type.short_const_node:
                    //VisitShortConstNode((short_const_node)en); 
                    break;
                case semantic_node_type.ushort_const_node:
                    //VisitUShortConstNode((ushort_const_node)en); 
                    break;
                case semantic_node_type.uint_const_node:
                    //VisitUIntConstNode((uint_const_node)en); 
                    break;
                case semantic_node_type.ulong_const_node:
                    //VisitULongConstNode((ulong_const_node)en); 
                    break;
                case semantic_node_type.long_const_node:
                    //VisitLongConstNode((long_const_node)en); 
                    break;
                case semantic_node_type.double_const_node:
                    //VisitDoubleConstNode((double_const_node)en); 
                    break;
                case semantic_node_type.char_const_node:
                    //VisitCharConstNode((char_const_node)en); 
                    break;
                case semantic_node_type.bool_const_node:
                    //VisitBoolConstNode((bool_const_node)en); 
                    break;
                case semantic_node_type.string_const_node:
                    //VisitStringConstNode((string_const_node)en); 
                    break;
                case semantic_node_type.local_variable_reference:
                    VisitLocalVariableReference((local_variable_reference)en); break;
                case semantic_node_type.namespace_variable_reference:
                    VisitNamespaceVariableReference((namespace_variable_reference)en); break;
                case semantic_node_type.basic_function_call:
                    VisitBasicFunctionCall((basic_function_call)en); break;
                case semantic_node_type.common_parameter_reference:
                    VisitCommonParameterReference((common_parameter_reference)en); 
                    break;
                case semantic_node_type.common_namespace_function_call:
                    VisitCommonNamespaceFunctionCall((common_namespace_function_call)en); break;
                case semantic_node_type.basic_function_call_node_as_constant:
                    VisitBasicFunctionCall((en as basic_function_call_as_constant).method_call); break;
                case semantic_node_type.common_namespace_function_call_node_as_constant:
                    VisitCommonNamespaceFunctionCallAsConstant((common_namespace_function_call_as_constant)en); break;
                case semantic_node_type.common_in_function_function_call:
                    VisitCommonInFuncFuncCall((common_in_function_function_call)en); break;
                case semantic_node_type.while_break_node:
                    //VisitWhileBreakNode((while_break_node)en); 
                    is_break_stmt = true;
                    break;
                case semantic_node_type.while_continue_node:
                    //VisitWhileContinueNode((while_continue_node)en); 
                    is_break_stmt = true;
                    break;
                case semantic_node_type.for_break_node:
                    //VisitForBreakNode((for_break_node)en); 
                    is_break_stmt = true;
                    break;
                case semantic_node_type.for_continue_node:
                    //VisitForContinueNode((for_continue_node)en); 
                    is_break_stmt = true;
                    break;
                case semantic_node_type.repeat_break_node:
                    //VisitRepeatBreakNode((repeat_break_node)en);
                    is_break_stmt = true;
                    break;
                case semantic_node_type.repeat_continue_node:
                    //VisitRepeatContinueNode((repeat_continue_node)en);
                    is_break_stmt = true;
                    break;
                case semantic_node_type.foreach_break_node:
                    is_break_stmt = true;
                    break;
                case semantic_node_type.foreach_continue_node:
                    is_break_stmt = true;
                    break;
                case semantic_node_type.common_static_method_call:
                    VisitCommonStaticMethodCall((common_static_method_call)en); break;
                case semantic_node_type.compiled_static_method_call:
                    VisitCompiledStaticMethodCall((compiled_static_method_call)en); break;
                case semantic_node_type.class_field_reference:
                    VisitClassFieldReference((class_field_reference)en); 
                    break;
                case semantic_node_type.deref_node:
                    VisitDerefNode((dereference_node)en); break;
                case semantic_node_type.common_method_call:
                    VisitCommonMethodCall((common_method_call)en); break;
                case semantic_node_type.compiled_function_call:
                    VisitCompiledFunctionCall((compiled_function_call)en); break;
                case semantic_node_type.get_addr_node:
                    VisitGetAddrNode((get_addr_node)en); break;
                case semantic_node_type.common_constructor_call:
                    VisitCommonConstructorCall((common_constructor_call)en); break;
                case semantic_node_type.compiled_constructor_call:
                    VisitCompiledConstructorCall((compiled_constructor_call)en); break;
                case semantic_node_type.compiled_variable_reference:
                    VisitCompiledVariableReference((compiled_variable_reference)en); break;
                case semantic_node_type.local_block_variable_reference:
                    VisitLocalBlockVariableReference((local_block_variable_reference)en); break;
                case semantic_node_type.static_compiled_variable_reference:
                    //VisitStaticCompiledVariableReference((static_compiled_variable_reference)en); 
                    break;
                case semantic_node_type.static_class_field_reference:
                    VisitStaticClassFieldReference((static_class_field_reference)en); 
                    break;
                case semantic_node_type.non_static_property_reference:
                    VisitNonStaticPropertyReference((non_static_property_reference)en); break;
                case semantic_node_type.simple_array_indexing:
                    VisitSimpleArrayIndexing((simple_array_indexing)en); break;
                case semantic_node_type.double_question_colon_expression:
                    VisitDoubleQuestionColon((double_question_colon_expression)en);
                    break;
                case semantic_node_type.this_node:
                    //VisitThisNode((this_node)en); 
                    break;
                case semantic_node_type.null_const_node:
                    //VisitNullConstNode((null_const_node)en); 
                    break;
                //default: ;//Console.WriteLine(en.semantic_node_type); throw new Exception("Unknown expression");
            }
        }
		
        
        private void VisitLocalBlockVariableReference(local_block_variable_reference en)
        {
        	IncreaseNumUseVar(en);
        }
        private void VisitCommonParameterReference(common_parameter_reference en)
        {
            IncreaseNumUseParam(en);
        }

        private void VisitStaticClassFieldReference(static_class_field_reference en)
        {
            IncreaseNumUseField(en);
        }

        private void VisitCompiledFunctionCall(compiled_function_call en)
        {
            VisitExpression(en.obj);
            for (int i = 0; i < en.parameters.Count; i++)
            {
                CheckVarParameter(en.parameters[i], en.function_node, i);
                VisitExpression(en.parameters[i]);
            }
        }

        private void VisitCompiledConstructorCall(compiled_constructor_call en)
        {
            for (int i = 0; i < en.parameters.Count; i++)
            {
                CheckVarParameter(en.parameters[i], en.function_node, i);
                VisitExpression(en.parameters[i]);
            }
        }

        private void VisitDoubleQuestionColon(double_question_colon_expression en)
        {
            VisitExpression(en.internal_condition);
            VisitExpression(en.internal_ret_if_null);
        }

        private void VisitSimpleArrayIndexing(simple_array_indexing en)
        {
            VisitExpression(en.simple_arr_expr);
            VisitExpression(en.ind_expr);
            if (en.expr_indices != null)
                foreach (expression_node expr in en.expr_indices)
                    VisitExpression(expr);
        }

        private void VisitNonStaticPropertyReference(non_static_property_reference en)
        {
            VisitExpression(en.expression);
            for (int i = 0; i < en.fact_parametres.Count; i++)
                VisitExpression(en.fact_parametres[i]);
        }

        private void VisitCompiledVariableReference(compiled_variable_reference en)
        {
            VisitExpression(en.obj);
        }

        private void VisitCommonConstructorCall(common_constructor_call en)
        {
            for (int i = 0; i < en.parameters.Count; i++)
            {
                CheckVarParameter(en.parameters[i], en.function_node, i);
                //if (!(en.function_node.function_code is runtime_statement))
                    VisitExpression(en.parameters[i]);
            }    
        }

        private void VisitGetAddrNode(get_addr_node en)
        {
            VisitExpression(en.addr_of);
            if (en.addr_of is local_variable_reference && (en.addr_of as local_variable_reference).var.is_ret_value)
                IncreaseNumAssVar(en.addr_of as local_variable_reference);
        }

        private void VisitClassFieldReference(class_field_reference en)
        {
            VisitExpression(en.obj);
            IncreaseNumUseField(en);
        }

        private void VisitCompiledStaticMethodCall(compiled_static_method_call en)
        {
            for (int i = 0; i < en.parameters.Count; i++)
            {
                CheckVarParameter(en.parameters[i], en.function_node, i);
                VisitExpression(en.parameters[i]);
            }  
        }

        private void VisitCommonInFuncFuncCall(common_in_function_function_call en)
        {
            for (int i = 0; i < en.parameters.Count; i++)
            {
                CheckVarParameter(en.parameters[i], en.function_node, i);
                VisitExpression(en.parameters[i]);
            }
                
        }

        private void VisitCommonNamespaceFunctionCall(common_namespace_function_call en)
        {
            CheckInfiniteRecursion(en);
            if ((en.function_node.name == "Reset" || en.function_node.name == "Rewrite" || en.function_node.name == "Assign") && en.function_node.comprehensive_namespace.namespace_name == compiler_string_consts.pascalSystemUnitName)
            {
                expression_node p = en.parameters[0];
                switch (p.semantic_node_type)
                {
                    case semantic_node_type.local_variable_reference: IncreaseNumAssVar((local_variable_reference)p); break;
                    case semantic_node_type.local_block_variable_reference: IncreaseNumAssVar((local_block_variable_reference)p); break;
                    case semantic_node_type.namespace_variable_reference: IncreaseNumAssVar((namespace_variable_reference)p); break;
                    case semantic_node_type.class_field_reference: VisitExpression((p as class_field_reference).obj); IncreaseNumAssField((class_field_reference)p); break;
                    case semantic_node_type.static_class_field_reference: IncreaseNumAssField((static_class_field_reference)p); break;
                    case semantic_node_type.common_parameter_reference: IncreaseNumAssParam((common_parameter_reference)p); break;
                    case semantic_node_type.deref_node: CheckAssign(((dereference_node)p).deref_expr); break;
                    case semantic_node_type.simple_array_indexing: VisitSimpleArrayIndexing((simple_array_indexing)p); break;
                }
            }
            for (int i = 0; i < en.parameters.Count; i++)
            {
                CheckVarParameter(en.parameters[i], en.function_node, i);
                VisitExpression(en.parameters[i]);
            }  
        }
		
        private void VisitCommonNamespaceFunctionCallAsConstant(common_namespace_function_call_as_constant en)
        {
            for (int i = 0; i < en.method_call.parameters.Count; i++)
            {
                CheckVarParameter(en.method_call.parameters[i], en.method_call.function_node, i);
                VisitExpression(en.method_call.parameters[i]);
            }
        }
        
        private void VisitCommonStaticMethodCall(common_static_method_call en)
        {
            CheckInfiniteRecursion(en);
            for (int i = 0; i < en.parameters.Count; i++)
            {
                CheckVarParameter(en.parameters[i], en.function_node, i);
                VisitExpression(en.parameters[i]);
            }
        }
        
        private void VisitCommonMethodCall(common_method_call en)
        {
            VisitExpression(en.obj);
            for (int i = 0; i < en.parameters.Count; i++)
            {
                CheckVarParameter(en.parameters[i], en.function_node, i);
                VisitExpression(en.parameters[i]);
            } 
        }

        private void VisitDerefNode(dereference_node en)
        {
            VisitExpression(en.deref_expr);
        }

        private void VisitStatementsExpressionNode(statements_expression_node en)
        {
            for (int i = 0; i < en.internal_statements.Count; i++)
                VisitStatement(en.internal_statements[i]);
            VisitExpression(en.internal_expression);
        }

        private void VisitQuestionColonExpression(question_colon_expression en)
        {
            SaveConditionBlock();
            RetVal rv = GetConstantValue(en.internal_condition);
            VisitExpression(en.internal_condition);
            if (rv != RetVal.True)
                EnterConditionBlock();
            VisitExpression(en.internal_ret_if_true);
            if (rv != RetVal.False)
                EnterConditionBlock();
            VisitExpression(en.internal_ret_if_false);
            RestoreConditionBlock();
        }

        private void VisitSizeOfOperator(sizeof_operator en)
        {
            
        }

        private void VisitIsNode(is_node en)
        {
            VisitExpression(en.left);
        }

        private void VisitAsNode(as_node en)
        {
            VisitExpression(en.left);
        }

        private void VisitLocalVariableReference(local_variable_reference en)
        {
            IncreaseNumUseVar(en);
        }

        private void VisitNamespaceVariableReference(namespace_variable_reference en)
        {
            IncreaseNumUseVar(en);
        }

        private void VisitAssignment(base_function_call en)
        {
            for (int i = 1; i < en.parameters.Count; i++)
                VisitExpression(en.parameters[i]);
            expression_node p = en.parameters[0];
            if (en.parameters[1] is common_parameter_reference && (en.parameters[1] as common_parameter_reference).par.name.StartsWith("<>"))
            {
                switch (p.semantic_node_type)
                {
                    case semantic_node_type.local_variable_reference: IncreaseNumUseVar((local_variable_reference)p); break;
                    case semantic_node_type.local_block_variable_reference: IncreaseNumUseVar((local_block_variable_reference)p); break;
                    case semantic_node_type.namespace_variable_reference: IncreaseNumUseVar((namespace_variable_reference)p); break;
                    case semantic_node_type.class_field_reference: VisitExpression((p as class_field_reference).obj); IncreaseNumUseField((class_field_reference)p); break;
                    case semantic_node_type.static_class_field_reference: IncreaseNumUseField((static_class_field_reference)p); break;
                    case semantic_node_type.common_parameter_reference: IncreaseNumUseParam((common_parameter_reference)p); break;
                    case semantic_node_type.deref_node: VisitDerefNode(((dereference_node)p)); break;
                    case semantic_node_type.simple_array_indexing: VisitSimpleArrayIndexing((simple_array_indexing)p); break;
                }
            }
            else
            switch (p.semantic_node_type)
            {
                case semantic_node_type.local_variable_reference: IncreaseNumAssVar((local_variable_reference)p); break;
                case semantic_node_type.local_block_variable_reference: IncreaseNumAssVar((local_block_variable_reference)p); break;
                case semantic_node_type.namespace_variable_reference: IncreaseNumAssVar((namespace_variable_reference)p); break;
                case semantic_node_type.class_field_reference: VisitExpression((p as class_field_reference).obj); IncreaseNumAssField((class_field_reference)p); break;
                case semantic_node_type.static_class_field_reference: IncreaseNumAssField((static_class_field_reference)p); break;
                case semantic_node_type.common_parameter_reference: IncreaseNumAssParam((common_parameter_reference)p); break;
                case semantic_node_type.deref_node: VisitDerefNode(((dereference_node)p)); break;
                case semantic_node_type.simple_array_indexing: VisitSimpleArrayIndexing((simple_array_indexing)p); break;
            }
            if (en.parameters[1] is local_block_variable_reference && (en.parameters[1] as local_block_variable_reference).var.name.StartsWith("$Ren"))
            {
                switch (p.semantic_node_type)
                {
                    case semantic_node_type.local_variable_reference: IncreaseNumUseVar((local_variable_reference)p); break;
                    case semantic_node_type.local_block_variable_reference: IncreaseNumUseVar((local_block_variable_reference)p); break;
                    case semantic_node_type.namespace_variable_reference: IncreaseNumUseVar((namespace_variable_reference)p); break;
                    case semantic_node_type.class_field_reference: VisitExpression((p as class_field_reference).obj); IncreaseNumUseField((class_field_reference)p); break;
                    case semantic_node_type.static_class_field_reference: IncreaseNumUseField((static_class_field_reference)p); break;
                    case semantic_node_type.common_parameter_reference: IncreaseNumUseParam((common_parameter_reference)p); break;
                    case semantic_node_type.deref_node: VisitDerefNode(((dereference_node)p)); break;
                    case semantic_node_type.simple_array_indexing: VisitSimpleArrayIndexing((simple_array_indexing)p); break;
                }
            }
        }

        private void CheckAssign(expression_node p)
        {
            switch (p.semantic_node_type)
            {
                case semantic_node_type.local_variable_reference: IncreaseNumAssVar((local_variable_reference)p); break;
                case semantic_node_type.namespace_variable_reference: IncreaseNumAssVar((namespace_variable_reference)p); break;
                case semantic_node_type.class_field_reference: VisitExpression((p as class_field_reference).obj); IncreaseNumAssField((class_field_reference)p); break;
                case semantic_node_type.static_class_field_reference: IncreaseNumAssField((static_class_field_reference)p); break;
                case semantic_node_type.common_parameter_reference: IncreaseNumAssParam((common_parameter_reference)p); break;
                case semantic_node_type.deref_node: CheckAssign(((dereference_node)p).deref_expr); break;
            }
        }
		
        private void VisitArrayInitializer(array_initializer en)
        {
        	foreach (expression_node expr in en.element_values)
        		VisitExpression(expr);
        }
        
        private void VisitRecordInitializer(record_initializer en)
        {
        	foreach (expression_node expr in en.field_values)
        		VisitExpression(expr);
        }
        
        private void VisitBasicFunctionCall(basic_function_call en)
        {
            switch (en.function_node.basic_function_type)
            {
                case SemanticTree.basic_function_type.iassign:
                case SemanticTree.basic_function_type.bassign:
                case SemanticTree.basic_function_type.boolassign:
                case SemanticTree.basic_function_type.fassign:
                case SemanticTree.basic_function_type.dassign:
                case SemanticTree.basic_function_type.sassign:
                case SemanticTree.basic_function_type.lassign:
                case SemanticTree.basic_function_type.charassign:
                case SemanticTree.basic_function_type.sbassign:
                case SemanticTree.basic_function_type.uiassign:
                case SemanticTree.basic_function_type.ulassign:
                case SemanticTree.basic_function_type.usassign:
                case SemanticTree.basic_function_type.objassign:
                    VisitAssignment(en); return;
            }
            bool tmp_no_infinite_recursion = no_infinite_recursion;
            for (int i = 0; i < en.parameters.Count; i++)
            {
                if (en.function_node.basic_function_type == SemanticTree.basic_function_type.booland
                    && en.parameters[i] is bool_const_node && (en.parameters[i] as bool_const_node).constant_value == false)
                    no_infinite_recursion = true;
                else if (en.function_node.basic_function_type == SemanticTree.basic_function_type.boolor
                    && en.parameters[0] is bool_const_node && (en.parameters[0] as bool_const_node).constant_value == true)
                    no_infinite_recursion = true;
                VisitExpression(en.parameters[i]);
            }
            if (!tmp_no_infinite_recursion)
                no_infinite_recursion = false;
        }

       
    }
}
