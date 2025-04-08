// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Конструирует типы массивов и делегатов (и enum-ов).

//В этом файле присутсвует существенное дублирование кода(~500строк). 
//TODO Разобраться и избавиться от дублирования

using System;

using PascalABCCompiler.TreeRealization;
using System.Collections.Generic;
using System.Linq;

namespace PascalABCCompiler.TreeConverter
{
	public class type_constructor
	{
		private int pascal_arrays_num = 0;

		convertion_data_and_alghoritms convertion_data_and_alghoritms;

		private static type_constructor _instance;

		public static type_constructor instance
		{
			get
			{
				return _instance;
			}
		}

		public type_constructor(convertion_data_and_alghoritms _convertion_data_and_alghoritms)
		{
			convertion_data_and_alghoritms = _convertion_data_and_alghoritms;
			_instance = this;
		}

		private System.Collections.Generic.Dictionary<type_node, List<type_node>> types_unsized_arrays =
			new System.Collections.Generic.Dictionary<type_node, List<type_node>>();

		public List<common_type_node> get_generic_arrays(int rank)
		{
			List<common_type_node> lst = new List<common_type_node>();
			foreach (var key in types_unsized_arrays.Keys)
			{
				if (key.is_generic_parameter)
				{
					List<type_node> ret = types_unsized_arrays[key];
                    for (int i = 0; i < ret.Count; i++)
                        if (ret[i] is common_type_node ctn)
                        {
							if (ctn.rank == rank)
								lst.Add(ctn);
                        }
                }
			}
			return lst;
        }

		public type_node create_unsized_array(type_node element_type, int rank, location loc)
		{
			List<type_node> ret = null;
			if(types_unsized_arrays.TryGetValue(element_type,out ret))
			{
				for (int i=0; i<ret.Count; i++)
				if (ret[i] is common_type_node)
				{
					if ((ret[i] as common_type_node).rank == rank)
					return ret[i];
				}
				else
				if (ret[i] is compiled_type_node)
				{
					if ((ret[i] as compiled_type_node).rank == rank)
					return ret[i];
				}
			}
			array_internal_interface aii = new array_internal_interface(element_type, rank);

			compiled_type_node comp_element_type = element_type as compiled_type_node;
			if (comp_element_type != null)
			{
				compiled_type_node ctn=NetHelper.NetHelper.get_array_type(comp_element_type, rank);
				ctn.Location = loc;
				if (ctn.get_internal_interface(internal_interface_kind.unsized_array_interface) == null)
				{
					ctn.add_internal_interface(aii);
				}
				ctn.SetName(StringConstants.get_array_type_name(ctn.element_type.name,rank));
				if (!types_unsized_arrays.TryGetValue(element_type,out ret))
				{
					ret = new List<type_node>();
					types_unsized_arrays[element_type] = ret;
				}
				ret.Add(ctn);
				//types_unsized_arrays.Add(element_type, ctn);
				
				SystemLibrary.SystemLibrary.init_reference_type(ctn);
				return ctn;
			}

			common_type_node comtn = new common_type_node(SystemLibrary.SystemLibrary.array_base_type,
			                                              StringConstants.get_array_type_name(element_type.name,rank), SemanticTree.type_access_level.tal_public,
			                                              null, convertion_data_and_alghoritms.symbol_table.CreateClassScope(null, SystemLibrary.SystemLibrary.array_base_type.Scope), loc);
			comtn.internal_type_special_kind = SemanticTree.type_special_kind.array_kind;
			comtn.is_class = true;
			comtn.add_internal_interface(aii);
			if (!types_unsized_arrays.TryGetValue(element_type,out ret))
			{
				ret = new List<type_node>();
				types_unsized_arrays[element_type] = ret;
			}
			ret.Add(comtn);
            //(ssyy) Добавляем интерфейсы.
            comtn.ImplementingInterfaces.Add(SystemLibrary.SystemLibrary.icloneable_interface);
            comtn.ImplementingInterfaces.Add(SystemLibrary.SystemLibrary.ilist_interface);
            comtn.ImplementingInterfaces.Add(SystemLibrary.SystemLibrary.icollection_interface);
            comtn.ImplementingInterfaces.Add(SystemLibrary.SystemLibrary.ienumerable_interface);
            if (generic_parameter_eliminations.check_type_generic_useful(element_type, null) == null)
            {
                List<type_node> type_params = new List<type_node>(1);
                type_params.Add(element_type);
                comtn.ImplementingInterfaces.Add(SystemLibrary.SystemLibrary.ilist1_interface.get_instance(type_params));
                comtn.ImplementingInterfaces.Add(SystemLibrary.SystemLibrary.icollection1_interface.get_instance(type_params));
                comtn.ImplementingInterfaces.Add(SystemLibrary.SystemLibrary.ienumerable1_interface.get_instance(type_params));
				if (SystemLibrary.SystemLibrary.ireadonlycollection_interface != null)
					comtn.ImplementingInterfaces.Add(SystemLibrary.SystemLibrary.ireadonlycollection_interface.get_instance(type_params));
			}
            //SystemLibrary.SystemLibrary.ic
            //(ssyy) Убрал 18.05.08
			//if (this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace != null)
			//this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace.types.AddElement(comtn);
			SystemLibrary.SystemLibrary.init_reference_type(comtn);

			return comtn;
		}
		
		public static void make_array_interface(common_type_node ctn)
		{
			array_internal_interface aii = new array_internal_interface(ctn.element_type,1);
			ctn.add_internal_interface(aii);
			//types_unsized_arrays.Add(element_type, comtn);
		}
		
		//TODO: Вообще-то здесь не должно быть этого кода. Не должен type_constructor знать о этих типах. Доступ к ним должен идти через StaticSystemLib.
		//Здесь хэшируются типы, чтобы не создавать их каждый раз. При очистке compiled_type_node.compiled_types их желательно тоже очистить.
		//Но compiled_type_node.compiled_types все равно никто никогда не чистит. 
        // А зря! SSM 05/04/2022
		private static type_node _IAsyncResultType;
		private static type_node _AsyncCallbackType;
		
		private static type_node begin_invoke_result_type
		{
			get
			{
				if (_IAsyncResultType == null)
				{
					_IAsyncResultType = compiled_type_node.get_type_node(typeof(System.IAsyncResult));
				}
				return _IAsyncResultType;
			}
		}

		private static type_node begin_invoke_parameter_type
		{
			get
			{
				if (_AsyncCallbackType == null)
				{
					_AsyncCallbackType = compiled_type_node.get_type_node(typeof(System.AsyncCallback));
				}
				return _AsyncCallbackType;
			}
		}

		public expression_node delegate_add_compile_time_executor(location call_location, expression_node[] exprs)
		{
			if (exprs.Length != 2)
			{
				throw new CompilerInternalError("Invalid delegate addition call");
			}

			expression_node cfc = convertion_data_and_alghoritms.create_simple_function_call(
				SystemLibrary.SystemLibrary.delegate_combine_method, call_location, exprs[0], exprs[1]);
			cfc = convertion_data_and_alghoritms.explicit_convert_type(cfc, exprs[0].type);
			return cfc;
		}

		public expression_node delegate_sub_compile_time_executor(location call_location, expression_node[] exprs)
		{
			if (exprs.Length != 2)
			{
				throw new CompilerInternalError("Invalid delegate addition call");
			}

			expression_node cfc = convertion_data_and_alghoritms.create_simple_function_call(
				SystemLibrary.SystemLibrary.delegate_remove_method, call_location, exprs[0], exprs[1]);
			cfc = convertion_data_and_alghoritms.explicit_convert_type(cfc, exprs[0].type);
			return cfc;
		}

		public expression_node delegate_add_assign_compile_time_executor(location call_location, expression_node[] exprs)
		{
			if (exprs.Length != 2)
			{
				throw new CompilerInternalError("Invalid delegate addition call");
			}

			expression_node cfc = convertion_data_and_alghoritms.create_simple_function_call(
				SystemLibrary.SystemLibrary.delegate_combine_method, call_location, exprs[0], exprs[1]);
			cfc = convertion_data_and_alghoritms.explicit_convert_type(cfc, exprs[0].type);
			expressions_list el=new expressions_list();
			el.AddElement(exprs[0]);
			el.AddElement(cfc);

			SymbolInfo si = exprs[0].type.find_first_in_type(StringConstants.assign_name);
            if (si == null && exprs[0].type.original_generic != null)
                si = exprs[0].type.original_generic.find_first_in_type(StringConstants.assign_name);
            if (si == null)
			{
				throw new CompilerInternalError("Undefined delegate operation");
			}
			function_node fn = (function_node)si.sym_info;

			expression_node en = convertion_data_and_alghoritms.create_simple_function_call(fn,
			                                                                                call_location, el[0], el[1]);
			
			return en;
		}

		public expression_node delegate_sub_assign_compile_time_executor(location call_location, expression_node[] exprs)
		{
			if (exprs.Length != 2)
			{
				throw new CompilerInternalError("Invalid delegate addition call");
			}

			expression_node cfc = convertion_data_and_alghoritms.create_simple_function_call(
				SystemLibrary.SystemLibrary.delegate_remove_method, call_location, exprs[0], exprs[1]);
			cfc = convertion_data_and_alghoritms.explicit_convert_type(cfc, exprs[0].type);
			expressions_list el = new expressions_list();
			el.AddElement(exprs[0]);
			el.AddElement(cfc);

			SymbolInfo si = exprs[0].type.find_first_in_type(StringConstants.assign_name);
            if (si == null && exprs[0].type.original_generic != null)
                si = exprs[0].type.original_generic.find_first_in_type(StringConstants.assign_name);
            if (si == null)
			{
				throw new CompilerInternalError("Undefined delegate operation");
			}
			function_node fn = (function_node)si.sym_info;

			expression_node en = convertion_data_and_alghoritms.create_simple_function_call(fn,
			                                                                                call_location, el[0], el[1]);

			return en;
		}

        public common_type_node create_delegate_without_init(string name, common_namespace_node cmn, location loc)
        {
            SymbolTable.Scope top_scope = null;
            if (cmn != null)
            {
                top_scope = cmn.scope;
            }
            common_type_node ctn = new common_type_node(SystemLibrary.SystemLibrary.delegate_base_type, name,
                                                        SemanticTree.type_access_level.tal_public, cmn,
                                                        convertion_data_and_alghoritms.symbol_table.CreateClassScope(top_scope, SystemLibrary.SystemLibrary.delegate_base_type.Scope, "delegate " + name),
                                                        loc);
			ctn.IsDelegate = true;
			ctn.is_class = true;
            return ctn;
        }

        public common_type_node create_delegate(string name, type_node return_value_type, parameter_list parameters,
                                                common_namespace_node cmn, location loc)
        {
            common_type_node ctn = create_delegate_without_init(name, cmn, loc);
            init_delegate(ctn, return_value_type, parameters, loc);
            return ctn;
        }

		public void init_delegate(common_type_node ctn, type_node return_value_type, parameter_list parameters,
		                                        location loc)
		{
			common_method_node constructor=new common_method_node(StringConstants.default_constructor_name,null,loc,ctn,SemanticTree.polymorphic_state.ps_common,
			                                                      SemanticTree.field_access_level.fal_public,
			                                                      convertion_data_and_alghoritms.symbol_table.CreateScope(ctn.scope));
			constructor.is_constructor = true;
			constructor.return_value_type = ctn;
			constructor.function_code=new runtime_statement(SemanticTree.runtime_statement_type.ctor_delegate,loc);

			common_method_node invoke=new common_method_node(StringConstants.invoke_method_name,
			                                                 return_value_type,loc,ctn,SemanticTree.polymorphic_state.ps_virtual,
			                                                 SemanticTree.field_access_level.fal_public,
			                                                 convertion_data_and_alghoritms.symbol_table.CreateScope(ctn.scope));
			ctn.add_name(StringConstants.invoke_method_name,new SymbolInfo(invoke));
			for (int i=0; i<parameters.Count; i++)
			{
				if (parameters[i] is compiled_parameter)
				{
					parameters[i] = new common_parameter(parameters[i].name,parameters[i].type,parameters[i].parameter_type,invoke,concrete_parameter_type.cpt_none,null,null);
				}
			}
				
			invoke.parameters.AddRange(parameters);
			invoke.function_code = new runtime_statement(SemanticTree.runtime_statement_type.invoke_delegate, loc);

			common_method_node begin_invoke = new common_method_node(StringConstants.begin_invoke_method_name,
			                                                         begin_invoke_result_type, loc, ctn, SemanticTree.polymorphic_state.ps_virtual, SemanticTree.field_access_level.fal_public,
			                                                         convertion_data_and_alghoritms.symbol_table.CreateScope(ctn.scope));
			ctn.add_name(StringConstants.begin_invoke_method_name,new SymbolInfo(begin_invoke));
			parameter_list begin_invoke_params=new parameter_list();
			begin_invoke_params.AddRange(parameters);
			common_parameter cp=new common_parameter(StringConstants.callback_string,begin_invoke_parameter_type,
			                                         SemanticTree.parameter_type.value,begin_invoke,concrete_parameter_type.cpt_none,
			                                         null,loc);
			begin_invoke_params.AddElement(cp);
			cp = new common_parameter(StringConstants.object_in_par_string, SystemLibrary.SystemLibrary.object_type,
			                          SemanticTree.parameter_type.value,begin_invoke, concrete_parameter_type.cpt_none,
			                          null, loc);
			begin_invoke_params.AddElement(cp);
			begin_invoke.parameters.AddRange(begin_invoke_params);
			begin_invoke.function_code = new runtime_statement(SemanticTree.runtime_statement_type.begin_invoke_delegate, loc);

			common_method_node end_invoke = new common_method_node(StringConstants.end_invoke_method_name,
			                                                       return_value_type, loc, ctn, SemanticTree.polymorphic_state.ps_virtual, SemanticTree.field_access_level.fal_public,
			                                                       convertion_data_and_alghoritms.symbol_table.CreateScope(ctn.scope));
			ctn.add_name(StringConstants.end_invoke_method_name,new SymbolInfo(end_invoke));
			cp = new common_parameter(StringConstants.result_string, begin_invoke_result_type,
			                          SemanticTree.parameter_type.value, end_invoke, concrete_parameter_type.cpt_none,
			                          null, loc);
			end_invoke.parameters.AddElement(cp);
			end_invoke.function_code = new runtime_statement(SemanticTree.runtime_statement_type.end_invoke_delegate, loc);

			ctn.methods.AddElement(constructor);
			ctn.methods.AddElement(invoke);
			ctn.methods.AddElement(begin_invoke);
			ctn.methods.AddElement(end_invoke);

			SystemLibrary.SystemLibrary.init_reference_type(ctn);

			delegate_internal_interface dii = new delegate_internal_interface(return_value_type, invoke, constructor);
			dii.parameters.AddRange(parameters);

			ctn.add_internal_interface(dii);
			AddOperatorsToDelegate(ctn, loc);
		}

		//(ssyy) Добавляет +, +=, -, -=
		public void AddOperatorsToDelegate(common_type_node ctn, location loc)
		{
			//+
			common_method_node cmn_add = new common_method_node(StringConstants.plus_name, ctn, loc, ctn, SemanticTree.polymorphic_state.ps_static,
			                                                    SemanticTree.field_access_level.fal_public, ctn.scope);
			cmn_add.compile_time_executor = delegate_add_compile_time_executor;
			ctn.scope.AddSymbol(cmn_add.name, new SymbolInfo(cmn_add));
			common_parameter cp1 = new common_parameter(StringConstants.left_param_name, ctn, SemanticTree.parameter_type.value,
			                                            cmn_add, concrete_parameter_type.cpt_none, null, loc);
			common_parameter cp2 = new common_parameter(StringConstants.right_param_name, ctn, SemanticTree.parameter_type.value,
			                                            cmn_add, concrete_parameter_type.cpt_none, null, loc);
			cmn_add.parameters.AddElement(cp1);
			cmn_add.parameters.AddElement(cp2);

			//+=
			common_method_node cmn_add_assign = new common_method_node(StringConstants.plusassign_name, ctn, loc, ctn, SemanticTree.polymorphic_state.ps_static,
			                                                           SemanticTree.field_access_level.fal_public, ctn.scope);
			cmn_add_assign.compile_time_executor = delegate_add_assign_compile_time_executor;
			ctn.scope.AddSymbol(cmn_add_assign.name, new SymbolInfo(cmn_add_assign));
			common_parameter cp3 = new common_parameter(StringConstants.left_param_name, ctn, SemanticTree.parameter_type.value,
			                                            cmn_add_assign, concrete_parameter_type.cpt_none, null, loc);
			common_parameter cp4 = new common_parameter(StringConstants.right_param_name, ctn, SemanticTree.parameter_type.value,
			                                            cmn_add_assign, concrete_parameter_type.cpt_none, null, loc);
			cmn_add_assign.parameters.AddElement(cp3);
			cmn_add_assign.parameters.AddElement(cp4);

			//-
			common_method_node cmn_sub = new common_method_node(StringConstants.minus_name, ctn, loc, ctn, SemanticTree.polymorphic_state.ps_static,
			                                                    SemanticTree.field_access_level.fal_public, ctn.scope);
			cmn_sub.compile_time_executor = delegate_sub_compile_time_executor;
			ctn.scope.AddSymbol(cmn_sub.name, new SymbolInfo(cmn_sub));
			common_parameter cp12 = new common_parameter(StringConstants.left_param_name, ctn, SemanticTree.parameter_type.value,
			                                             cmn_sub, concrete_parameter_type.cpt_none, null, loc);
			common_parameter cp22 = new common_parameter(StringConstants.right_param_name, ctn, SemanticTree.parameter_type.value,
			                                             cmn_sub, concrete_parameter_type.cpt_none, null, loc);
			cmn_sub.parameters.AddElement(cp12);
			cmn_sub.parameters.AddElement(cp22);

			//-=
			common_method_node cmn_sub_assign = new common_method_node(StringConstants.minusassign_name, ctn, loc, ctn, SemanticTree.polymorphic_state.ps_static,
			                                                           SemanticTree.field_access_level.fal_public, ctn.scope);
			cmn_sub_assign.compile_time_executor = delegate_sub_assign_compile_time_executor;
			ctn.scope.AddSymbol(cmn_sub_assign.name, new SymbolInfo(cmn_sub_assign));
			common_parameter cp32 = new common_parameter(StringConstants.left_param_name, ctn, SemanticTree.parameter_type.value,
			                                             cmn_sub_assign, concrete_parameter_type.cpt_none, null, loc);
			common_parameter cp42 = new common_parameter(StringConstants.right_param_name, ctn, SemanticTree.parameter_type.value,
			                                             cmn_sub_assign, concrete_parameter_type.cpt_none, null, loc);
			cmn_sub_assign.parameters.AddElement(cp32);
			cmn_sub_assign.parameters.AddElement(cp42);
		}

		private string get_pascal_array_name()
		{
			pascal_arrays_num++;
			return (StringConstants.pascal_array_name + pascal_arrays_num.ToString());
		}

		private type_node create_array_type(ordinal_type_interface oti_indexer, type_node element_type,common_namespace_node _cmn, location loc)
		{
			int arr_length = oti_indexer.ordinal_type_to_int(oti_indexer.upper_value) -
				oti_indexer.ordinal_type_to_int(oti_indexer.lower_value) + 1;

			if (arr_length <= 0)
			{
				throw new SimpleSemanticError(loc, "NEGATIVE_ARRAY_LENGTH_({0})_NOT_ALLOWED", arr_length);
			}

			simple_array sa = new simple_array(element_type, arr_length);
			//sa.length = arr_length;
			//sa.element_type = element_type;

			SymbolTable.Scope top_scope = null;
			if (_cmn != null)
			{
				top_scope = _cmn.scope;
			}
			string name = get_pascal_array_name();
			//if (_cmn.namespace_name != null)
			//    name = _cmn.namespace_name + name;
			common_type_node ctn = new common_type_node(null, name, SemanticTree.type_access_level.tal_public,
			                                            _cmn, convertion_data_and_alghoritms.symbol_table.CreateClassScope(top_scope, null, "array_type " + name), loc);

			ctn.is_class = true;
			ctn.SetBaseType(SystemLibrary.SystemLibrary.object_type);
			//DarkStar Add
			//loc не нужно мне это!  и некому не нужно!
			//loc = null;
			
			//ctn.internal_is_value = true;

			class_constant_definition cdn1 = new class_constant_definition(StringConstants.lower_array_const_name,
			                                                               oti_indexer.lower_value, loc, ctn, SemanticTree.field_access_level.fal_public);
			ctn.scope.AddSymbol(cdn1.name, new SymbolInfo(cdn1));

			class_constant_definition cdn2 = new class_constant_definition(StringConstants.upper_array_const_name,
			                                                               oti_indexer.upper_value, loc, ctn, SemanticTree.field_access_level.fal_public);
			ctn.scope.AddSymbol(cdn2.name, new SymbolInfo(cdn2));

			class_field int_arr = new class_field(StringConstants.internal_array_name, sa, ctn,
			                                      SemanticTree.polymorphic_state.ps_common, SemanticTree.field_access_level.fal_public,loc);
			ctn.scope.AddSymbol(int_arr.name, new SymbolInfo(int_arr));
			ctn.fields.AddElement(int_arr);

			SystemLibrary.SystemLibrary.init_reference_type(ctn);

			ctn.const_defs.AddElement(cdn1);
			ctn.const_defs.AddElement(cdn2);

			common_method_node get_func = new common_method_node(StringConstants.get_val_pascal_array_name,
			                                                     element_type, /*loc*/new location(0xFFFFFF, 0, 0xFFFFFF, 0, loc.doc), ctn, SemanticTree.polymorphic_state.ps_common, SemanticTree.field_access_level.fal_private,
			                                                     convertion_data_and_alghoritms.symbol_table.CreateScope(ctn.scope, "get " + name));
			common_parameter get_param = new common_parameter(StringConstants.unary_param_name,
			                                                  oti_indexer.lower_value.type, SemanticTree.parameter_type.value, get_func, concrete_parameter_type.cpt_none,
			                                                  null, loc);
			get_func.parameters.AddElement(get_param);

			common_parameter_reference cpr = new common_parameter_reference(get_param, 0, loc);
			expression_node en1 = convertion_data_and_alghoritms.create_simple_function_call(oti_indexer.value_to_int,
			                                                                                 loc, cpr);
			expression_node en2 = new int_const_node(oti_indexer.ordinal_type_to_int(oti_indexer.lower_value), loc);
			expression_node sub_expr = convertion_data_and_alghoritms.create_simple_function_call(
				SystemLibrary.SystemLibrary.int_sub, loc, en1, en2);

			this_node thisnode = new this_node(ctn, loc);

			class_field_reference cfr1 = new class_field_reference(int_arr, thisnode, loc);

			expression_node index_expr = new simple_array_indexing(cfr1, sub_expr, element_type, loc);

			statement_node sn = new return_node(index_expr, /*loc*/new location(0xFFFFFF, 0, 0xFFFFFF, 0, loc.doc));

			get_func.function_code = sn;

			common_method_node set_func = new common_method_node(StringConstants.set_val_pascal_array_name,
			                                                     null, /*loc*/new location(0xFFFFFF, 0, 0xFFFFFF, 0, loc.doc), ctn, SemanticTree.polymorphic_state.ps_common, SemanticTree.field_access_level.fal_private,
			                                                     convertion_data_and_alghoritms.symbol_table.CreateScope(ctn.scope, "set " + name));
			common_parameter set_ind = new common_parameter(StringConstants.left_param_name,
			                                                oti_indexer.lower_value.type, SemanticTree.parameter_type.value, set_func, concrete_parameter_type.cpt_none,
			                                                null, loc);
			set_func.parameters.AddElement(set_ind);
			common_parameter set_val = new common_parameter(StringConstants.right_param_name,
			                                                element_type, SemanticTree.parameter_type.value, set_func, concrete_parameter_type.cpt_none, null, loc);
			set_func.parameters.AddElement(set_val);

			common_parameter_reference cpr2 = new common_parameter_reference(set_ind, 0, loc);
			expression_node en3 = convertion_data_and_alghoritms.create_simple_function_call(oti_indexer.value_to_int,
			                                                                                 loc, cpr2);
			expression_node en4 = new int_const_node(oti_indexer.ordinal_type_to_int(oti_indexer.lower_value), loc);
			expression_node sub_expr2 = convertion_data_and_alghoritms.create_simple_function_call(
				SystemLibrary.SystemLibrary.int_sub, loc, en3, en4);

			class_field_reference cfr2 = new class_field_reference(int_arr, thisnode, loc);

			expression_node index_expr2 = new simple_array_indexing(cfr2, sub_expr2, element_type,loc);

			SymbolInfo si = element_type.find_first_in_type(StringConstants.assign_name);
			if (si == null)
			{
				throw new NotSupportedError(loc);
				throw new CompilerInternalError("Varable of this type can not be assigned");
			}
			if (si.sym_info.general_node_type != general_node_type.function_node)
			{
				throw new CompilerInternalError("Invalid assign operator");
			}

			expression_node val_ref = new common_parameter_reference(set_val, 0, loc);

			function_node assign = (function_node)si.sym_info;
			statement_node assign_call = convertion_data_and_alghoritms.create_simple_function_call(assign,
			                                                                                        /*loc*/new location(0xFFFFFF, 0, 0xFFFFFF, 0, loc.doc), index_expr2, val_ref);

			set_func.function_code = assign_call;

			ctn.methods.AddElement(get_func);
			ctn.methods.AddElement(set_func);

			common_property_node cpn = new common_property_node(StringConstants.index_property_pascal_array_name,
			                                                    ctn, element_type, get_func, set_func, loc, SemanticTree.field_access_level.fal_public, SemanticTree.polymorphic_state.ps_common);

			common_parameter prop_cp = new common_parameter(StringConstants.unary_param_name, oti_indexer.lower_value.type,
			                                                SemanticTree.parameter_type.value, null, concrete_parameter_type.cpt_none, null, loc);
			cpn.parameters.AddElement(prop_cp);

			ctn.properties.AddElement(cpn);

			ctn.default_property = cpn;

			if (_cmn != null)
			{
				_cmn.types.AddElement(ctn);
			}

			bounded_array_interface bai = new bounded_array_interface(oti_indexer, element_type, cpn, oti_indexer.lower_value.type, int_arr);
			ctn.add_internal_interface(bai);
			ctn.type_special_kind = SemanticTree.type_special_kind.array_wrapper;

            if (element_type.type_special_kind != SemanticTree.type_special_kind.array_wrapper)
            {
                ctn.ImplementingInterfaces.Add(compiled_type_node.get_type_node(NetHelper.NetHelper.FindType(StringConstants.IEnumerableInterfaceName)));
                common_method_node en_cmn = new common_method_node(StringConstants.GetEnumeratorMethodName, compiled_type_node.get_type_node(NetHelper.NetHelper.FindType(StringConstants.IEnumeratorInterfaceName)), null, ctn, SemanticTree.polymorphic_state.ps_virtual, SemanticTree.field_access_level.fal_public, null);

                compiled_function_node en_fnc = NetHelper.NetHelper.FindName(NetHelper.NetHelper.FindType(StringConstants.IEnumerableInterfaceName), StringConstants.GetEnumeratorMethodName).FirstOrDefault().sym_info as compiled_function_node;
                statements_list sl = new statements_list(null);
                sl.statements.AddElement(new return_node(
                    new compiled_function_call(en_fnc, new class_field_reference(int_arr, new this_node(ctn, null), null), null), null));
                en_cmn.function_code = sl;
                en_cmn.newslot_awaited = true;
                ctn.methods.AddElement(en_cmn);

                if (!element_type.IsPointer)
                {
                    List<type_node> generic_args = new List<type_node>();
                    generic_args.Add(element_type);
                    type_node en_tn = compiled_type_node.get_type_node(NetHelper.NetHelper.FindType(StringConstants.IGenericEnumerableInterfaceName)).get_instance(generic_args);
                    ctn.ImplementingInterfaces.Add(en_tn);

                    en_cmn = new common_method_node(StringConstants.GetEnumeratorMethodName, compiled_type_node.get_type_node(NetHelper.NetHelper.FindType(StringConstants.IGenericEnumeratorInterfaceName)).get_instance(generic_args), null, ctn, SemanticTree.polymorphic_state.ps_virtual, SemanticTree.field_access_level.fal_public, null);
                    //en_fnc = en_tn.find_in_type("GetEnumerator").sym_info as function_node;//NetHelper.NetHelper.FindName(NetHelper.NetHelper.FindType(StringConstants.IGenericEnumerableInterfaceName), StringConstants.GetEnumeratorMethodName).sym_info as compiled_function_node;
                    List<SymbolInfo> en_sil = en_tn.find_in_type("GetEnumerator");
                    if (en_sil != null && en_sil.Count() > 1 && (en_sil[1].sym_info as function_node).return_value_type.is_generic_type_instance)
                        en_sil = en_sil.GetRange(1, en_sil.Count() - 1);
                    function_node en_fnc_inst = en_sil.FirstOrDefault().sym_info as function_node; ;//.get_instance(generic_args, true, loc);
                    sl = new statements_list(null);
                    if (en_fnc_inst is compiled_function_node)
                        sl.statements.AddElement(new return_node(
                            new compiled_function_call(en_fnc_inst as compiled_function_node, new class_field_reference(int_arr, new this_node(ctn, null), null), null), null));
                    else
                        sl.statements.AddElement(new return_node(
                            new common_method_call(en_fnc_inst as common_method_node, new class_field_reference(int_arr, new this_node(ctn, null), null), null), null));
                    en_cmn.function_code = sl;
                    en_cmn.newslot_awaited = true;
                    ctn.methods.AddElement(en_cmn);
                }
            }
			
			//= operation
			SymbolTable.ClassMethodScope scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(ctn.scope, _cmn.scope, null, "= operator from " + ctn.scope);
        	common_method_node cmn_eq = new common_method_node(StringConstants.GetNETOperName(StringConstants.eq_name),SystemLibrary.SystemLibrary.bool_type,null,ctn,
        	                                                SemanticTree.polymorphic_state.ps_static,SemanticTree.field_access_level.fal_public,scope);
        	cmn_eq.IsOperator = true;
        	common_parameter prm1 = new common_parameter("a",ctn,SemanticTree.parameter_type.value,cmn_eq,concrete_parameter_type.cpt_none,null,null);
        	common_parameter prm2 = new common_parameter("b",ctn,SemanticTree.parameter_type.value,cmn_eq,concrete_parameter_type.cpt_none,null,null);
        	cmn_eq.parameters.AddElement(prm1);
        	cmn_eq.parameters.AddElement(prm2);
        	statements_list body = new statements_list(null);
        	local_variable vdn = new local_variable("$i",SystemLibrary.SystemLibrary.integer_type,cmn_eq,null);//this.convertion_data_and_alghoritms.syntax_tree_visitor.context.create_for_temp_variable(SystemLibrary.SystemLibrary.integer_type, null);
        	cmn_eq.var_definition_nodes_list.AddElement(vdn);
        	expression_node var_ref = new local_variable_reference(vdn,0,null);//this.convertion_data_and_alghoritms.syntax_tree_visitor.create_variable_reference(vdn,null);
        	basic_function_call cond = new basic_function_call(SystemLibrary.SystemLibrary.int_sm as basic_function_node,null);
        	cond.parameters.AddElement(var_ref);
        	cond.parameters.AddElement(new int_const_node(arr_length,null));
        	while_node while_stmt = new while_node(cond,null);
        	statements_list while_body = new statements_list(null);
        	while_stmt.body = while_body;
        	simple_array_indexing left_sar = new simple_array_indexing(new class_field_reference(int_arr, new common_parameter_reference(prm1,0,null), null),var_ref,element_type,null);
        	simple_array_indexing right_sar = new simple_array_indexing(new class_field_reference(int_arr, new common_parameter_reference(prm2,0,null), null),var_ref,element_type,null);
        	expression_node cond2 = SystemLibrary.SystemLibrary.syn_visitor.find_operator(StringConstants.noteq_name,
        		                                                                            left_sar,right_sar,null);
        	if_node if_stmt = new if_node(cond2,new return_node(new bool_const_node(false,null),null),null,null);
        	while_body.statements.AddElement(if_stmt);
        	while_body.statements.AddElement(new basic_function_call(SystemLibrary.SystemLibrary.int_assign as basic_function_node,null
        	                                                   ,var_ref,new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,null,var_ref,new int_const_node(1,null))));
        	
        	body.statements.AddElement(while_stmt);
        	body.statements.AddElement(new return_node(new bool_const_node(true,null),null));
        	cmn_eq.function_code = body;
        	cmn_eq.is_overload = true;
        	ctn.methods.AddElement(cmn_eq);
        	ctn.Scope.AddSymbol(StringConstants.eq_name,new SymbolInfo(cmn_eq));
        	
        	//<> operation
			scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(ctn.scope, _cmn.scope, null, "<> operator from " + ctn.scope);
        	common_method_node cmn_noteq = new common_method_node(StringConstants.GetNETOperName(StringConstants.noteq_name),SystemLibrary.SystemLibrary.bool_type,null,ctn,
        	                                                SemanticTree.polymorphic_state.ps_static,SemanticTree.field_access_level.fal_public,scope);
        	cmn_noteq.IsOperator = true;
        	prm1 = new common_parameter("a",ctn,SemanticTree.parameter_type.value,cmn_noteq,concrete_parameter_type.cpt_none,null,null);
        	prm2 = new common_parameter("b",ctn,SemanticTree.parameter_type.value,cmn_noteq,concrete_parameter_type.cpt_none,null,null);
        	cmn_noteq.parameters.AddElement(prm1);
        	cmn_noteq.parameters.AddElement(prm2);
        	body = new statements_list(null);
        	vdn = new local_variable("$i",SystemLibrary.SystemLibrary.integer_type,cmn_noteq,null);//this.convertion_data_and_alghoritms.syntax_tree_visitor.context.create_for_temp_variable(SystemLibrary.SystemLibrary.integer_type, null);
        	cmn_noteq.var_definition_nodes_list.AddElement(vdn);
        	var_ref = new local_variable_reference(vdn,0,null);//this.convertion_data_and_alghoritms.syntax_tree_visitor.create_variable_reference(vdn,null);
        	cond = new basic_function_call(SystemLibrary.SystemLibrary.int_sm as basic_function_node,null);
        	cond.parameters.AddElement(var_ref);
        	cond.parameters.AddElement(new int_const_node(arr_length,null));
        	while_stmt = new while_node(cond,null);
        	while_body = new statements_list(null);
        	while_stmt.body = while_body;
        	left_sar = new simple_array_indexing(new class_field_reference(int_arr, new common_parameter_reference(prm1,0,null), null),var_ref,element_type,null);
        	right_sar = new simple_array_indexing(new class_field_reference(int_arr, new common_parameter_reference(prm2,0,null), null),var_ref,element_type,null);
        	cond2 = SystemLibrary.SystemLibrary.syn_visitor.find_operator(StringConstants.noteq_name,
        		                                                                            left_sar,right_sar,null);
        	if_stmt = new if_node(cond2,new return_node(new bool_const_node(true,null),null),null,null);
        	while_body.statements.AddElement(if_stmt);
        	while_body.statements.AddElement(new basic_function_call(SystemLibrary.SystemLibrary.int_assign as basic_function_node,null
        	                                                   ,var_ref,new basic_function_call(SystemLibrary.SystemLibrary.int_add as basic_function_node,null,var_ref,new int_const_node(1,null))));
        	
        	body.statements.AddElement(while_stmt);
        	body.statements.AddElement(new return_node(new bool_const_node(false,null),null));
        	cmn_noteq.function_code = body;
        	cmn_noteq.is_overload = true;
        	ctn.methods.AddElement(cmn_noteq);
        	ctn.Scope.AddSymbol(StringConstants.noteq_name,new SymbolInfo(cmn_noteq));
        	
        	//Equals
        	/*scope = convertion_data_and_alghoritms.symbol_table.CreateClassMethodScope(_cmn.scope,ctn.scope);
        	common_method_node cmn_equals = new common_method_node("Equals",SystemLibrary.SystemLibrary.bool_type,null,ctn,
        	                                                SemanticTree.polymorphic_state.ps_virtual,SemanticTree.field_access_level.fal_public,scope);
        	prm1 = new common_parameter("a",SystemLibrary.SystemLibrary.object_type,SemanticTree.parameter_type.value,cmn_equals,concrete_parameter_type.cpt_none,null,null);
        	
        	cmn_equals.parameters.AddElement(prm1);
        	body = new statements_list(null);
        	vdn = new local_variable("$i",ctn,cmn_equals,null);//this.convertion_data_and_alghoritms.syntax_tree_visitor.context.create_for_temp_variable(SystemLibrary.SystemLibrary.integer_type, null);
        	cmn_equals.var_definition_nodes_list.AddElement(vdn);
        	var_ref = new local_variable_reference(vdn,0,null);//this.convertion_data_and_alghoritms.syntax_tree_visitor.create_variable_reference(vdn,null);
        	as_node _as = new as_node(new common_parameter_reference(prm1,0,null),ctn,null);
        	base_function_call ass_bfc = new basic_function_call(SystemLibrary.SystemLibrary.object_type.find(StringConstants.assign_name).sym_info as basic_function_node,null);
        	ass_bfc.parameters.AddElement(var_ref);
        	ass_bfc.parameters.AddElement(_as);
        	body.statements.AddElement(ass_bfc);
        	common_static_method_call csmc = new common_static_method_call(ctn.find_in_type(StringConstants.eq_name).sym_info as common_method_node,null);
        	csmc.parameters.AddElement(new this_node(ctn,null));
        	csmc.parameters.AddElement(var_ref);
        	body.statements.AddElement(new return_node(csmc,null));
        	ctn.methods.AddElement(cmn_equals);
        	ctn.Scope.AddSymbol("Equals",new SymbolInfo(cmn_equals));*/
        	return ctn;
		}

		private string get_diap_type_name(constant_node low, constant_node high, string element_type_name)
		{
			if (low != null && high != null)
			{
				object o1 = low.get_object_value();
				object o2 = high.get_object_value();
				if (o1 is char) o1 = "'"+o1.ToString()+"'";
				if (o2 is char) o2 = "'"+o2.ToString()+"'";
				return o1.ToString()+".."+o2.ToString();
			}
			return (element_type_name + "_diap");
		}
		
		public static void add_convertions_to_diap(common_type_node ctn, constant_node lower_value, constant_node upper_value)
		{
			type_node element_type = ctn.base_type;
			if (element_type != SystemLibrary.SystemLibrary.char_type && element_type != SystemLibrary.SystemLibrary.bool_type)
			{
				//element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.iassign)));
				/*element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.bassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.sassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.sbassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.usassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.uiassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.lassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.ulassign)));*/
			}
			else if (element_type == SystemLibrary.SystemLibrary.char_type)
				element_type.add_generated_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.charassign)));
			else if (element_type == SystemLibrary.SystemLibrary.bool_type)
                element_type.add_generated_name(StringConstants.assign_name, new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn, PascalABCCompiler.SemanticTree.basic_function_type.boolassign)));
			else if (element_type.IsEnum)
			{
                element_type.add_generated_name(StringConstants.assign_name, new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn, PascalABCCompiler.SemanticTree.basic_function_type.iassign)));
                element_type.add_generated_name(StringConstants.assign_name, new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(element_type, PascalABCCompiler.SemanticTree.basic_function_type.iassign)));
			}
			
			if (element_type == SystemLibrary.SystemLibrary.integer_type)
			{
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoi,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoi,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.byte_type)
			{
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.btosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultob,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.chartob,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.sbyte_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtob,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.btosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultosb,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.chartosb,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.short_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stosb,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultos,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartos,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.ushort_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustos,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultous,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartous,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.uint_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoi,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoui,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoui,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.uint_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.uint_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.uint_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.uint_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.uint_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.uint_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.uint_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.uint_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.uint_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.uint_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.uint_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.uint_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.uint_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.uint_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.uint_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.uint_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.uint_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.uint_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.int64_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoui,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.long_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ltod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ltof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ultol,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartol,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.long_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.long_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.long_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.long_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.long_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.long_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.long_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.long_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.long_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.long_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.long_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.long_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.long_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.long_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.long_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.long_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.long_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.long_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.uint64_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ultol,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.ulong_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ultod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ultof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoul,false);
				
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.ulong_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.ulong_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.ulong_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.ulong_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.ulong_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.ulong_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.ulong_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.ulong_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.ulong_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.ulong_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.ulong_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.ulong_idiv);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.ulong_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.ulong_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.ulong_xor);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.ulong_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.ulong_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.ulong_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.char_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.chartob,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.chartosb,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartos,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartous,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoui,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartol,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoul,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartod,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartof,false);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.dtochar,false);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartof,false);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.char_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.char_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.char_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.char_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.char_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.char_noteq);
			}
			else if (element_type.IsEnum)
			{
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,element_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none);
                SystemLibrary.SystemLibrary.make_generated_type_conversion(element_type, ctn, type_compare.less_type, PascalABCCompiler.SemanticTree.basic_function_type.none,true);
                
                SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itob,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itosb,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itos,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.none,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itous,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itoui,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itol,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itoul,false);
				
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoi,false);
			}
			internal_interface ii = element_type.get_internal_interface(internal_interface_kind.ordinal_interface);
			ordinal_type_interface oti_old = (ordinal_type_interface)ii;

			//TODO: Переделать создание потомков oti.
			ordinal_type_interface oti_new = new ordinal_type_interface(oti_old.inc_method, oti_old.dec_method,
			                                                            oti_old.inc_value_method,oti_old.dec_value_method,
			                                                            oti_old.lower_eq_method, oti_old.greater_eq_method, oti_old.lower_method, oti_old.greater_method, lower_value, upper_value, oti_old.value_to_int, oti_old.ordinal_type_to_int);
			ctn.add_internal_interface(oti_new);
		}
		
		//TODO: Усовершенствовать. Как должны выглядеть типы-диапазоны для net-кода. Изменить методы inc и dec так, чтобы они генерировали исключения в случае выхода за границы типа диапазона.
		private type_node create_diap_type(type_node element_type, constant_node lower_value, constant_node upper_value,common_namespace_node _cmn)
		{
			SymbolTable.Scope top_scope = null;
			if (_cmn != null)
			{
				top_scope = _cmn.scope;
			}
			//basic_type_node btn=new basic_type_node(element_type.name+"_diap");
			common_type_node ctn = new common_type_node(element_type, get_diap_type_name(lower_value, upper_value, element_type.name), SemanticTree.type_access_level.tal_internal,
			                                            _cmn, convertion_data_and_alghoritms.symbol_table.CreateClassScope(top_scope, null, "diap_type " + element_type), null);
			ctn.type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.diap_type;
			ctn.internal_is_value = true;
			internal_interface ii = element_type.get_internal_interface(internal_interface_kind.ordinal_interface);
			ordinal_type_interface oti_old = (ordinal_type_interface)ii;

			//TODO: Переделать создание потомков oti.
			ordinal_type_interface oti_new = new ordinal_type_interface(oti_old.inc_method, oti_old.dec_method,
			                                                            oti_old.inc_value_method,oti_old.dec_value_method,
			                                                            oti_old.lower_eq_method, oti_old.greater_eq_method, 
			                                                            oti_old.lower_method, oti_old.greater_method,
			                                                            lower_value, upper_value, oti_old.value_to_int, oti_old.ordinal_type_to_int);
			//type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.integer_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
			//type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.integer_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
			//element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.iassign)));
			//type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.byte_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
			//type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.byte_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
			
			//type_table.add_type_conversion_from_defined(ctn,element_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,element_type,false,"diaptoord"),type_compare.less_type,true);
			//type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.integer_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
			
			if (element_type != SystemLibrary.SystemLibrary.char_type && element_type != SystemLibrary.SystemLibrary.bool_type)
			{
				element_type.add_generated_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.iassign)));
				/*element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.bassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.sassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.sbassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.usassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.uiassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.lassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.ulassign)));*/
			}
			else if (element_type == SystemLibrary.SystemLibrary.char_type)
                element_type.add_generated_name(StringConstants.assign_name, new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn, PascalABCCompiler.SemanticTree.basic_function_type.charassign)));
			else if (element_type == SystemLibrary.SystemLibrary.bool_type)
                element_type.add_generated_name(StringConstants.assign_name, new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn, PascalABCCompiler.SemanticTree.basic_function_type.boolassign)));
			else if (element_type.IsEnum)
			{
                element_type.add_generated_name(StringConstants.assign_name, new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn, PascalABCCompiler.SemanticTree.basic_function_type.iassign)));
                element_type.add_generated_name(StringConstants.assign_name, new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(element_type, PascalABCCompiler.SemanticTree.basic_function_type.iassign)));
			}
			if (element_type == SystemLibrary.SystemLibrary.integer_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoi,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoi,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
                SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.byte_type)
			{
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.btosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultob,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.chartob,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.sbyte_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtob,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.btosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultosb,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.chartosb,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.short_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stosb,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultos,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartos,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.ushort_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustos,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultous,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartous,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.int_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.int_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.int_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.int_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.int_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.int_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.int_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.int_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.int_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.int_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.int_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.int_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.int_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.int_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.int_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.int_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.int_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.uint_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoi,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoui,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoui,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.uint_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.uint_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.uint_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.uint_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.uint_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.uint_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.uint_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.uint_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.uint_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.uint_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.uint_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.uint_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.uint_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.uint_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.uint_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.uint_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.uint_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.uint_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.int64_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoui,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.long_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ltod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ltof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitol,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ultol,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartol,false);
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.long_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.long_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.long_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.long_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.long_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.long_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.long_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.long_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.long_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.long_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.long_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.long_idiv);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.long_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.long_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.long_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.long_xor);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.long_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.long_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.uint64_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultob,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultosb,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultos,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultous,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoi,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoui,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ultol,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.ulong_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ultod,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ultof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoul,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itod,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itof,true);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoul,false);
				
				
				//SystemLibrary.SystemLibrary.make_unary_empty_operator(StringConstants.plus_name, ctn, ctn);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.not_name, ctn, SystemLibrary.SystemLibrary.ulong_not);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.ulong_unmin);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.ulong_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.ulong_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.ulong_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.ulong_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.ulong_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.ulong_noteq);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.plus_name, ctn, SystemLibrary.SystemLibrary.ulong_add);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.minus_name, ctn, SystemLibrary.SystemLibrary.ulong_sub);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mul_name, ctn, SystemLibrary.SystemLibrary.ulong_mul);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.idiv_name, ctn, SystemLibrary.SystemLibrary.ulong_idiv);

				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.and_name, ctn, SystemLibrary.SystemLibrary.ulong_and);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.or_name, ctn, SystemLibrary.SystemLibrary.ulong_or);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.xor_name, ctn, SystemLibrary.SystemLibrary.ulong_xor);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.mod_name, ctn, SystemLibrary.SystemLibrary.ulong_mod);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shl_name, ctn, SystemLibrary.SystemLibrary.ulong_shl);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.shr_name, ctn, SystemLibrary.SystemLibrary.ulong_shr);
			}
			else if (element_type == SystemLibrary.SystemLibrary.char_type)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.chartob,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.chartosb,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartos,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartous,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoui,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartol,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartoul,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.double_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartod,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.float_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartof,false);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(tctn,SystemLibrary.SystemLibrary.char_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultochar,false);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.stochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ustochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.uitochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultochar,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.char_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.real_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.dtochar,false);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.float_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.chartof,false);
				
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.gr_name, ctn, SystemLibrary.SystemLibrary.char_gr);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.greq_name, ctn, SystemLibrary.SystemLibrary.char_greq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.sm_name, ctn, SystemLibrary.SystemLibrary.char_sm);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.smeq_name, ctn, SystemLibrary.SystemLibrary.char_smeq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.eq_name, ctn, SystemLibrary.SystemLibrary.char_eq);
				SystemLibrary.SystemLibrary.add_generated_funtion_to_type(StringConstants.noteq_name, ctn, SystemLibrary.SystemLibrary.char_noteq);
			}
			else if (element_type.IsEnum)
			{
				SystemLibrary.SystemLibrary.make_generated_type_conversion(element_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.none,true);
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.byte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itob,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.sbyte_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itosb,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.short_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itos,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.integer_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.none,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.ushort_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itous,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.itoui,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.int64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itol,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,SystemLibrary.SystemLibrary.uint64_type,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.itoul,false);
				
				
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.byte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.btoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.sbyte_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.sbtoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.short_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.stoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.ushort_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.ustoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.integer_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.uitoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.int64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ltoi,false);
				SystemLibrary.SystemLibrary.make_generated_type_conversion(SystemLibrary.SystemLibrary.uint64_type,ctn,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.ultoi,false);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(ctn,element_type,type_compare.greater_type,PascalABCCompiler.SemanticTree.basic_function_type.none);
				//SystemLibrary.SystemLibrary.make_generated_type_conversion(element_type,ctn,type_compare.less_type,PascalABCCompiler.SemanticTree.basic_function_type.none);
			}
			
			/*if (element_type != SystemLibrary.SystemLibrary.char_type && element_type != SystemLibrary.SystemLibrary.bool_type)
            {
            	//if (SystemLibrary.SystemLibrary.sbyte_type != element_type)
            	//type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.sbyte_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
            	if (SystemLibrary.SystemLibrary.byte_type != element_type)
				type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.byte_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
            	
            	if (SystemLibrary.SystemLibrary.short_type != element_type)
            	type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.short_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
            	if (SystemLibrary.SystemLibrary.ushort_type != element_type)
            	type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.ushort_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
            	if (SystemLibrary.SystemLibrary.integer_type != element_type)
            	type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.integer_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
            	if (SystemLibrary.SystemLibrary.uint_type != element_type)
            	type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.uint_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
            	if (SystemLibrary.SystemLibrary.long_type != element_type)
            	type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.long_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
            	if (SystemLibrary.SystemLibrary.ulong_type != element_type)
            	type_table.add_type_conversion_from_defined(ctn,SystemLibrary.SystemLibrary.ulong_type,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"diaptoord"),type_compare.less_type,true);
            }
             if (element_type != SystemLibrary.SystemLibrary.char_type && element_type != SystemLibrary.SystemLibrary.bool_type)
            {
            	//type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.integer_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            	//if (SystemLibrary.SystemLibrary.byte_type != element_type)
            	//type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.sbyte_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            	type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.byte_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            	//if (SystemLibrary.SystemLibrary.sbyte_type != element_type)
            	//if (SystemLibrary.SystemLibrary.short_type != element_type)
            	type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.short_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            	//if (SystemLibrary.SystemLibrary.ushort_type != element_type)
            	type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.ushort_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            	//if (SystemLibrary.SystemLibrary.integer_type != element_type)
            	type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.integer_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            	//if (SystemLibrary.SystemLibrary.uint_type != element_type)
            	type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.uint_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            	//if (SystemLibrary.SystemLibrary.long_type != element_type)
            	type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.long_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            	//if (SystemLibrary.SystemLibrary.ulong_type != element_type)
            	type_table.add_type_conversion_from_defined(SystemLibrary.SystemLibrary.ulong_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
            }
            else
            type_table.add_type_conversion_from_defined(element_type,ctn,new basic_function_node(PascalABCCompiler.SemanticTree.basic_function_type.none,ctn,true,"ordtodiap"),type_compare.less_type,true);
			 */
			//            common_namespace_function_node cnfn = new common_namespace_function_node("Inc$"+pascal_arrays_num++,null,this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace,null);
			//            cnfn.parameters.AddElement(new common_parameter("value",ctn,PascalABCCompiler.SemanticTree.parameter_type.var,cnfn,concrete_parameter_type.cpt_var,null,null));
			//            cnfn.is_overload = true;
			//this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace.functions.AddElement(cnfn);
			// if (SystemLibrary.SystemLibrary.system_unit.namespaces.Count > 0)
			// 	SystemLibrary.SystemLibrary.system_unit.namespaces[0].functions.AddElement(cnfn);
			// (SystemLibrary.SystemLibrary.system_unit.scope.Find(StringConstants.system_unit_name).sym_info as common_namespace_node).functions.AddElement(cnfn);
			// else
			// 	this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace.functions.AddElement(cnfn);
			//            statements_list sl = new statements_list(null);
			//            PascalABCCompiler.SemanticTree.basic_function_type bft = PascalABCCompiler.SemanticTree.basic_function_type.iinc;
			//            if (element_type == SystemLibrary.SystemLibrary.byte_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.binc;
			//            else if (element_type == SystemLibrary.SystemLibrary.sbyte_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.sbinc;
			//            else if (element_type == SystemLibrary.SystemLibrary.short_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.sinc;
			//            else if (element_type == SystemLibrary.SystemLibrary.ushort_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.usinc;
			//            else if (element_type == SystemLibrary.SystemLibrary.uint_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.uiinc;
			//            else if (element_type == SystemLibrary.SystemLibrary.long_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.linc;
			//            else if (element_type == SystemLibrary.SystemLibrary.ulong_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.ulinc;
			//            else if (element_type == SystemLibrary.SystemLibrary.char_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.cinc;
			//            else if (element_type == SystemLibrary.SystemLibrary.bool_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.boolinc;
			//            basic_function_call bfc = new basic_function_call(new basic_function_node(bft,element_type,true),null);
			//            bfc.parameters.AddElement(new common_parameter_reference(cnfn.parameters[0] as common_parameter,0,null));
			//            sl.statements.AddElement(bfc);
			//            cnfn.function_code = sl;
			//            SymbolInfo si = this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace.scope.Find("Inc");
			//            while (si.Next != null)
			//            	si = si.Next;
			//            si.Next = new SymbolInfo(cnfn);
			//            si.Next.access_level = access_level.al_internal;
			//            si.Next.symbol_kind = symbol_kind.sk_overload_procedure;
			//            //SystemLibrary.SystemLibrary.system_unit.scope.AddSymbol("Inc",new SymbolInfo(cnfn));
//
			//            cnfn = new common_namespace_function_node("Dec$"+pascal_arrays_num++,null,this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace,null);
			//            cnfn.parameters.AddElement(new common_parameter("value",ctn,PascalABCCompiler.SemanticTree.parameter_type.var,cnfn,concrete_parameter_type.cpt_var,null,null));
			//            //this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace.functions.AddElement(cnfn);
			//            if (SystemLibrary.SystemLibrary.system_unit.namespaces.Count > 0)
			//            	SystemLibrary.SystemLibrary.system_unit.namespaces[0].functions.AddElement(cnfn);
			//            else
			//            	this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace.functions.AddElement(cnfn);
			//            sl = new statements_list(null);
			//            bft = PascalABCCompiler.SemanticTree.basic_function_type.idec;
			//            if (element_type == SystemLibrary.SystemLibrary.byte_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.bdec;
			//            else if (element_type == SystemLibrary.SystemLibrary.sbyte_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.sbdec;
			//            else if (element_type == SystemLibrary.SystemLibrary.short_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.sdec;
			//            else if (element_type == SystemLibrary.SystemLibrary.ushort_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.usdec;
			//            else if (element_type == SystemLibrary.SystemLibrary.uint_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.uidec;
			//            else if (element_type == SystemLibrary.SystemLibrary.long_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.ldec;
			//            else if (element_type == SystemLibrary.SystemLibrary.ulong_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.uldec;
			//            else if (element_type == SystemLibrary.SystemLibrary.char_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.cdec;
			//            else if (element_type == SystemLibrary.SystemLibrary.bool_type)
			//            	bft = PascalABCCompiler.SemanticTree.basic_function_type.booldec;
			//            bfc = new basic_function_call(new basic_function_node(bft,element_type,true),null);
			//            bfc.parameters.AddElement(new common_parameter_reference(cnfn.parameters[0] as common_parameter,0,null));
			//            sl.statements.AddElement(bfc);
			//            cnfn.function_code = sl;
//
			//           SystemLibrary.SystemLibrary.system_unit.scope.AddSymbol("Dec",new SymbolInfo(cnfn));
			// if (element_type == SystemLibrary.SystemLibrary.integer_type)
			// element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.iassign)));
			// else if (element_type == SystemLibrary.SystemLibrary.byte_type)
			// element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.bassign)));
			/*element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.sassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.sbassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.usassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.uiassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.lassign)));
            	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.ulassign)));*/
			// else if (element_type == SystemLibrary.SystemLibrary.char_type)
			// 	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.charassign)));
			// else if (element_type == SystemLibrary.SystemLibrary.bool_type)
			// 	element_type.add_name(StringConstants.assign_name,new SymbolInfo(SystemLibrary.SystemLibrary.make_assign_operator(ctn,PascalABCCompiler.SemanticTree.basic_function_type.boolassign)));
			
			ctn.add_internal_interface(oti_new);
			this.convertion_data_and_alghoritms.syntax_tree_visitor.context.converted_namespace.types.AddElement(ctn);
			return ctn;
		}

		//Вот недостаток .NET 1. Пока класс не шаблонный и не видно сразу что это за ужасный тип (хэштаблица хэштаблиц хэштаблиц).
		//А стоит слелать шаблонный класс - сразу все понятно.
		//TODO: Создать шаблонный класс для представления ассоциативных многомерных массивов с автоматическим добавлением элементов. И переделать код ниже.
		//TODO: НИКОГДА БОЛЬШЕ НЕ СОЗДАВАТЬ ХЭШТАБЛИЦУ ХЭШТАБЛИЦ ХЭШТАБЛИЦ!!! НИКОГДА!!! НИКОГДА!!! НИКОГДА!!!
		private System.Collections.Generic.Dictionary<type_node, System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, type_node>>> pascal_diap =
			new System.Collections.Generic.Dictionary<type_node, System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, type_node>>>();
		private System.Collections.Generic.Dictionary<ordinal_type_interface, System.Collections.Generic.Dictionary<type_node, type_node>> pascal_arrays =
			new System.Collections.Generic.Dictionary<ordinal_type_interface, System.Collections.Generic.Dictionary<type_node, type_node>>();

		public type_node get_array_type(ordinal_type_interface oti_indexer, type_node element_type,common_namespace_node _cmn,location loc)
		{
			/*
            System.Collections.Generic.Dictionary<type_node, type_node> ht;
            if (!(pascal_arrays.TryGetValue(oti_indexer, out ht)))
            {
                ht = new System.Collections.Generic.Dictionary<type_node, type_node>();
                pascal_arrays[oti_indexer] = ht;
                type_node ret = create_array_type(oti_indexer, element_type, _cmn,loc);
                ht[element_type] = ret;
                return ret;
            }
            type_node tn;
            if (!(ht.TryGetValue(element_type, out tn)))
            {
                type_node ret = create_array_type(oti_indexer, element_type, _cmn,loc);
                ht[element_type] = ret;
                return ret;
            }
            return tn;
			 */
			type_node ret = create_array_type(oti_indexer, element_type, _cmn, loc);
			return ret;
		}
		
		public type_node get_need_ordinal_type(type_node tn)
		{
			if (tn == SystemLibrary.SystemLibrary.byte_type || tn == SystemLibrary.SystemLibrary.sbyte_type || tn == SystemLibrary.SystemLibrary.short_type
			    || tn == SystemLibrary.SystemLibrary.ushort_type || tn == SystemLibrary.SystemLibrary.integer_type) return SystemLibrary.SystemLibrary.integer_type;
			if (tn == SystemLibrary.SystemLibrary.uint_type || tn == SystemLibrary.SystemLibrary.int64_type) return SystemLibrary.SystemLibrary.int64_type;
			if (tn == SystemLibrary.SystemLibrary.uint64_type) return SystemLibrary.SystemLibrary.uint64_type;
			if (tn.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type) return get_need_ordinal_type(tn.base_type);
			return tn;
		}

        public constant_node create_need_constant_for_diapason(constant_node cn, type_node tn)
        {
            if (tn == SystemLibrary.SystemLibrary.integer_type)
            {
                switch (cn.semantic_node_type)
                {
                    case semantic_node_type.int_const_node: return cn;
                    case semantic_node_type.byte_const_node: return new int_const_node((cn as byte_const_node).constant_value, cn.location);
                    case semantic_node_type.sbyte_const_node: return new int_const_node((cn as sbyte_const_node).constant_value, cn.location);
                    case semantic_node_type.short_const_node: return new int_const_node((cn as short_const_node).constant_value, cn.location);
                    case semantic_node_type.ushort_const_node: return new int_const_node((cn as ushort_const_node).constant_value, cn.location);
                }
                return cn;
            }
            else if (tn == SystemLibrary.SystemLibrary.int64_type)
            {
                switch (cn.semantic_node_type)
                {
                    case semantic_node_type.long_const_node: return cn;
                    case semantic_node_type.uint_const_node: return new long_const_node((cn as uint_const_node).constant_value, cn.location);
                    case semantic_node_type.int_const_node: return new long_const_node((cn as int_const_node).constant_value, cn.location);
                    case semantic_node_type.byte_const_node: return new long_const_node((cn as byte_const_node).constant_value, cn.location);
                    case semantic_node_type.sbyte_const_node: return new long_const_node((cn as sbyte_const_node).constant_value, cn.location);
                    case semantic_node_type.short_const_node: return new long_const_node((cn as short_const_node).constant_value, cn.location);
                    case semantic_node_type.ushort_const_node: return new long_const_node((cn as ushort_const_node).constant_value, cn.location);
                }
                return cn;
            }
            else if (tn == SystemLibrary.SystemLibrary.uint64_type)
            {
                switch (cn.semantic_node_type)
                {
                    case semantic_node_type.ulong_const_node: return cn;
                    case semantic_node_type.uint_const_node: return new long_const_node((cn as uint_const_node).constant_value, cn.location);
                    //case semantic_node_type.int_const_node : return new long_const_node((cn as int_const_node).constant_value, cn.location);
                    case semantic_node_type.byte_const_node: return new long_const_node((cn as byte_const_node).constant_value, cn.location);
                    //case semantic_node_type.sbyte_const_node : return new long_const_node((cn as sbyte_const_node).constant_value, cn.location);
                    //case semantic_node_type.short_const_node : return new long_const_node((cn as short_const_node).constant_value, cn.location);
                    case semantic_node_type.ushort_const_node: return new long_const_node((cn as ushort_const_node).constant_value, cn.location);
                }
                return cn;
            }
            return cn;
        }
		
		public type_node get_pascal_diap(constant_node lower_value, constant_node upper_value,
		                                 location left_loc,common_namespace_node _cmn, location right_loc)
		{
			
			type_node elem_type = lower_value.type;
			internal_interface ii = lower_value.type.get_internal_interface(internal_interface_kind.ordinal_interface);
			if (ii == null)
			{
                throw new SimpleSemanticError(left_loc, "ORDINAL_TYPE_EXPECTED");
			}
			internal_interface iir = upper_value.type.get_internal_interface(internal_interface_kind.ordinal_interface);
			if (iir == null)
			{
                throw new SimpleSemanticError(right_loc, "ORDINAL_TYPE_EXPECTED");
			}
			expression_node lower_bound = lower_value;
			expression_node upper_bound = upper_value;
			if (lower_value.type != upper_value.type)
			{
				//throw new CompilerInternalError("Unsupported now. Need constant expression evaluator.");
				type_compare tc = type_table.compare_types(lower_value.type,upper_value.type);
				if (tc == type_compare.non_comparable_type)
                    throw new SimpleSemanticError(left_loc, "INCOMPATIBLE_ARRAY_BOUNDS_TYPES");
				if (lower_value.type == SystemLibrary.SystemLibrary.uint64_type)
					tc = type_compare.greater_type;
				else if (upper_value.type == SystemLibrary.SystemLibrary.uint64_type)
					tc = type_compare.less_type;
				
				if (tc == type_compare.greater_type)
				{
					type_intersection_node tin = lower_value.type.get_type_intersection(upper_value.type);
                    if (tin != null && tin.this_to_another.is_explicit) throw new SimpleSemanticError(left_loc, "INCOMPATIBLE_ARRAY_BOUNDS_TYPES");
					elem_type = get_need_ordinal_type(lower_value.type);
					if (elem_type == SystemLibrary.SystemLibrary.uint64_type)
						if (upper_value.type == SystemLibrary.SystemLibrary.sbyte_type || upper_value.type == SystemLibrary.SystemLibrary.short_type
						    || upper_value.type == SystemLibrary.SystemLibrary.integer_type || upper_value.type == SystemLibrary.SystemLibrary.int64_type)
                            throw new SimpleSemanticError(left_loc, "INCOMPATIBLE_ARRAY_BOUNDS_TYPES");
					upper_bound = convertion_data_and_alghoritms.convert_type(upper_value, elem_type);
					if (lower_bound.type != elem_type)
						lower_bound = convertion_data_and_alghoritms.convert_type(lower_value, elem_type);
				}
				else
				{
					type_intersection_node tin = upper_value.type.get_type_intersection(lower_value.type);
                    if (tin != null && tin.this_to_another.is_explicit) throw new SimpleSemanticError(left_loc, "INCOMPATIBLE_ARRAY_BOUNDS_TYPES");
					elem_type = get_need_ordinal_type(upper_value.type);
					if (elem_type == SystemLibrary.SystemLibrary.uint64_type)
						if (lower_value.type == SystemLibrary.SystemLibrary.sbyte_type || lower_value.type == SystemLibrary.SystemLibrary.short_type
						    || lower_value.type == SystemLibrary.SystemLibrary.integer_type || lower_value.type == SystemLibrary.SystemLibrary.int64_type)
                            throw new SimpleSemanticError(left_loc, "INCOMPATIBLE_ARRAY_BOUNDS_TYPES");
					lower_bound = convertion_data_and_alghoritms.convert_type(lower_value, elem_type);
					if (upper_bound.type != elem_type)
						upper_bound = convertion_data_and_alghoritms.convert_type(upper_value, elem_type);
				}
				/*expression_node en = null;
                try
                {
                    en = convertion_data_and_alghoritms.syntax_tree_visitor.find_operator(
                        StringConstants.eq_name, lower_value, upper_value, left_loc);
                }
                catch (NoFunctionWithSameArguments)
                {
                    throw new IncompatibleArrayBoundsTypes(left_loc);
                }
                base_function_call bfc = en as base_function_call;
                lower_bound = convertion_data_and_alghoritms.convert_type(lower_value, bfc.parameters[0].type);
                upper_bound = convertion_data_and_alghoritms.convert_type(upper_value, bfc.parameters[0].type);*/
				
			}
			//internal_interface iir2 = lower_bound.type.get_internal_interface(internal_interface_kind.ordinal_interface);
			internal_interface iir2 = elem_type.get_internal_interface(internal_interface_kind.ordinal_interface);
			if (iir2 == null)
			{
                throw new SimpleSemanticError(right_loc, "ORDINAL_TYPE_EXPECTED");
			}
			ordinal_type_interface oti_left = (ordinal_type_interface)ii;
			ordinal_type_interface oti_right = (ordinal_type_interface)iir;
			if (lower_value is common_namespace_function_call_as_constant
			    || lower_value is compiled_constructor_call_as_constant 
			    || lower_value is compiled_static_method_call_as_constant
			   	|| lower_value is common_constructor_call_as_constant
                || lower_value is basic_function_call_as_constant)
                throw new SimpleSemanticError(left_loc, "CONSTANT_EXPRESSION_EXPECTED");
			if (upper_value is common_namespace_function_call_as_constant
			    || upper_value is compiled_constructor_call_as_constant 
			    || upper_value is compiled_static_method_call_as_constant
			   	|| upper_value is common_constructor_call_as_constant
                || upper_value is basic_function_call_as_constant)
                throw new SimpleSemanticError(right_loc, "CONSTANT_EXPRESSION_EXPECTED");
			int left_int_value = oti_left.ordinal_type_to_int(lower_value);
			int right_int_value = oti_right.ordinal_type_to_int(upper_value);
			//lower_value = new int_const_node(left_int_value,lower_value.location);
			//upper_value = new int_const_node(right_int_value,upper_value.location);
			lower_value = create_need_constant_for_diapason(lower_value,elem_type);
			upper_value = create_need_constant_for_diapason(upper_value,elem_type);
			//ssyy
			if (left_int_value > right_int_value)
			{
                throw new SimpleSemanticError(right_loc, "LEFT_RANGE_GREATER_THEN_RIGHT");
			}
			//\ssyy
			/*
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, type_node>> ht;
            if (!(pascal_diap.TryGetValue(elem_type, out ht)))
            {
                ht = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.Dictionary<int, type_node>>();
                pascal_diap[elem_type] = ht;
            }
            System.Collections.Generic.Dictionary<int, type_node> ht2;
            if (!(ht.TryGetValue(left_int_value,out ht2)))
            {
                ht2 = new System.Collections.Generic.Dictionary<int, type_node>();
                ht[left_int_value] = ht2;
            }
            type_node tn;
            if (!((ht2.TryGetValue(right_int_value,out tn))))
            {
                tn = create_diap_type(elem_type, lower_value, upper_value,_cmn);
                ht2[right_int_value] = tn;
            }
            return tn;
			 */
			type_node tn = create_diap_type(elem_type, lower_value, upper_value, _cmn);
			return tn;
		}

		public void reset()
		{
			pascal_arrays_num = 0;
			//TODO: Что лучше Clear или создать новую?
			pascal_arrays.Clear();
			pascal_diap.Clear();
			types_unsized_arrays.Clear();
		}

	}
}
