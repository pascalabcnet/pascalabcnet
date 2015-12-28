
using System;

namespace TreeConverter
{

	public class system_lib_initializer
	{

		private compiled_type_node _bool_type;
		private compiled_type_node _integer_type;
		private compiled_type_node _real_type;
		private compiled_type_node _char_type;
		private compiled_type_node _string_type;

		private compiled_type_node _pointer_type;
		private basic_function_node _int_add;
		private basic_function_node _int_sub;

		private unit_node _system_unit;

		private bool_const_node _true_constant;
		private bool_const_node _false_constant;

		private constant_definition_node _true_constant_definition;
		private constant_definition_node _false_constant_definition;

		public unit_node system_unit
		{
			get
			{
				return _system_unit;
			}
		}

		public basic_function_node int_add
		{
			get
			{
				return _int_add;
			}
		}

		public basic_function_node int_sub
		{
			get
			{
				return _int_sub;
			}
		}

		//private base_interface_node _ipascal_enumerable;

		/*public base_interface_node ipascal_enumerable
		{
			get
			{
				return _ipascal_enumerable;
			}
		}*/

		public type_node bool_type
		{
			get
			{
				return _bool_type;
			}
		}

		public type_node integer_type
		{
			get
			{
				return _integer_type;
			}
		}

		public type_node real_type
		{
			get
			{
				return _real_type;
			}
		}

		public type_node char_type
		{
			get
			{
				return _char_type;
			}
		}

		public type_node string_type
		{
			get
			{
				return _string_type;
			}
		}

		public type_node pointer_type
		{
			get
			{
				return _pointer_type;
			}
		}
		private basic_function_node make_common_binary_operation(string operator_name,compiled_type_node def_type,compiled_type_node left,
			compiled_type_node right,SemanticTree.basic_function_type bft,type_node ret_value_type)
		{
			parameterArrayList pars=new parameterArrayList();
			basic_function_node bfn=new basic_function_node(bft,pars,ret_value_type);
			bfn.is_overload=true;
			basic_parameter par_left=new basic_parameter(compiler_string_consts.left_param_name,left,
				SemanticTree.parameter_type.value,bfn);
			basic_parameter par_right=new basic_parameter(compiler_string_consts.right_param_name,right,
				SemanticTree.parameter_type.value,bfn);
			pars.Add(par_left);
			pars.Add(par_right);
			def_type.add_additional_name(operator_name,new SymbolInfo(bfn));
			return bfn;
		}

		private void make_unary_operator(string operator_name,compiled_type_node to,SemanticTree.basic_function_type bft)
		{
			make_unary_operator(operator_name,to,bft,to);
		}

		private void make_unary_operator(string operator_name,compiled_type_node to,
			SemanticTree.basic_function_type bft,type_node ret_value_type)
		{
			parameterArrayList pars=new parameterArrayList();
			basic_function_node bfn=new basic_function_node(bft,pars,ret_value_type);
			bfn.is_overload=true;
			basic_parameter par=new basic_parameter(compiler_string_consts.unary_param_name,to,
				SemanticTree.parameter_type.value,bfn);
			pars.Add(par);
			to.add_additional_name(operator_name,new SymbolInfo(bfn));
		}

		private basic_function_node make_binary_operator(string operator_name,compiled_type_node to,SemanticTree.basic_function_type bft)
		{
			return make_binary_operator(operator_name,to,bft,to);
		}

		private basic_function_node make_binary_operator(string operator_name,compiled_type_node to,
			SemanticTree.basic_function_type bft,type_node ret_value_type)
		{
			/*parameterArrayList pars=new parameterArrayList();
			basic_function_node bfn=new basic_function_node(bft,pars,ret_value_type);
			basic_parameter par_left=new basic_parameter(compiler_string_consts.left_param_name,to,
				SemanticTree.parameter_type.value,bfn);
			basic_parameter par_right=new basic_parameter(compiler_string_consts.right_param_name,to,
				SemanticTree.parameter_type.value,bfn);
			pars.Add(par_left);
			pars.Add(par_right);
			to.add_additional_name(operator_name,new SymbolInfo(bfn));*/
			return make_common_binary_operation(operator_name,to,to,to,bft,ret_value_type);
		}

		private void make_type_conversion(compiled_type_node from,compiled_type_node to,type_compare tc,
			SemanticTree.basic_function_type bft)
		{
			parameterArrayList pars=new parameterArrayList();
			basic_function_node conv_method=new basic_function_node(bft,pars,to);
			basic_parameter bp=new basic_parameter(compiler_string_consts.unary_param_name,
				from,SemanticTree.parameter_type.value,conv_method);
			pars.Add(bp);

			type_intersection_node inter_node=new type_intersection_node(tc);
			inter_node.this_to_another=new type_conversion_factory(conv_method);
			from.add_intersection_node(to,inter_node);
		}

		private void make_assign_operator(compiled_type_node ctn,SemanticTree.basic_function_type bft)
		{
			make_compiled_object_operator(compiler_string_consts.assign_name,ctn,bft,SemanticTree.parameter_type.var);
		}

		private void make_compiled_object_operator(string name,compiled_type_node tp,SemanticTree.basic_function_type bft,
			SemanticTree.parameter_type first_param_type)
		{
			parameterArrayList pars=new parameterArrayList();
			basic_function_node bfn=new basic_function_node(bft,pars,tp);
			bfn.is_overload=true;
			basic_parameter to=new basic_parameter(compiler_string_consts.left_param_name,tp,
				first_param_type,bfn);
			basic_parameter from=new basic_parameter(compiler_string_consts.right_param_name,tp,
				SemanticTree.parameter_type.value,bfn);
			pars.Add(to);
			pars.Add(from);
			tp.add_additional_name(name,new SymbolInfo(bfn));
		}

		private void make_compiled_object_operator(string name,compiled_type_node tp,SemanticTree.basic_function_type bft)
		{
			make_compiled_object_operator(name,tp,bft,SemanticTree.parameter_type.value);
		}

		private void make_reference_object_assign_operator(compiled_type_node tp)
		{
			make_compiled_object_operator(compiler_string_consts.assign_name,tp,
				SemanticTree.basic_function_type.objassign,SemanticTree.parameter_type.var);
		}

		private void make_reference_object_eq_operator(compiled_type_node tp)
		{
			make_compiled_object_operator(compiler_string_consts.eq_name,tp,SemanticTree.basic_function_type.objeq);
		}

		private void make_reference_object_noteq_operator(compiled_type_node tp)
		{
			make_compiled_object_operator(compiler_string_consts.noteq_name,tp,SemanticTree.basic_function_type.objnoteq);
		}

		public void init_reference_type(compiled_type_node ctn)
		{
			make_reference_object_assign_operator(ctn);
			make_reference_object_eq_operator(ctn);
			make_reference_object_noteq_operator(ctn);
		}

		//No v drugom smisle reference.
		private void make_object_operator(ref_type_node ctn,SemanticTree.basic_function_type bas_ft,
			string name,type_node ret_type,SemanticTree.parameter_type first_parameter_type)
		{
			parameterArrayList pars=new parameterArrayList();
			basic_function_node bfn=new basic_function_node(bas_ft,pars,ret_type);
			bfn.is_overload=true;
			basic_parameter to=new basic_parameter(compiler_string_consts.left_param_name,ctn,
				first_parameter_type,bfn);
			basic_parameter from=new basic_parameter(compiler_string_consts.right_param_name,ctn,
				SemanticTree.parameter_type.value,bfn);
			pars.Add(to);
			pars.Add(from);
			ctn.add_additional_name(name,new SymbolInfo(bfn));
		}

		private void make_object_operator(ref_type_node ctn,SemanticTree.basic_function_type bas_ft,string name,
			type_node ret_type)
		{
			make_object_operator(ctn,bas_ft,name,ret_type,SemanticTree.parameter_type.value);
		}

		private void make_assign_operator(ref_type_node ctn)
		{
			make_object_operator(ctn,SemanticTree.basic_function_type.objassign,compiler_string_consts.assign_name,
				ctn,SemanticTree.parameter_type.var);
		}

		private void make_equivalence_operator(ref_type_node ctn)
		{
			make_object_operator(ctn,SemanticTree.basic_function_type.objeq,compiler_string_consts.eq_name,_bool_type);
		}

		private void make_not_equivalence_operator(ref_type_node ctn)
		{
			make_object_operator(ctn,SemanticTree.basic_function_type.objnoteq,compiler_string_consts.noteq_name,_bool_type);
		}

		public void init_reference_type(ref_type_node ctn)
		{
			make_assign_operator(ctn);
			make_equivalence_operator(ctn);
			make_not_equivalence_operator(ctn);
		}
		//End: No v drugom smisle reference.

		public void make_empty_operator(string name,compiled_type_node ctn)
		{
			parameterArrayList pars=new parameterArrayList();
			empty_function_node efn=new empty_function_node(pars,ctn);
			efn.is_overload=true;
			basic_parameter par=new basic_parameter(compiler_string_consts.unary_param_name,ctn,
				SemanticTree.parameter_type.value,null);
			pars.Add(par);
			ctn.add_additional_name(name,new SymbolInfo(efn));
		}

		private basic_function_node create_oti_method(SemanticTree.basic_function_type bft,type_node type)
		{
			parameterArrayList pal=new parameterArrayList();
			basic_function_node bfn=new basic_function_node(bft,pal,type);
			common_parameter cp=new common_parameter(compiler_string_consts.unary_param_name,type,
				SemanticTree.parameter_type.var,null);
			pal.Add(cp);
			bfn.is_overload=true;
			return bfn;
		}

		public int int_to_int(constant_node cn)
		{
#if (DEBUG)
			if (cn.semantic_node_type!=semantic_node_type.int_const_node)
			{
				throw new CompilerInternalError("Error in static type conversion");
			}
#endif
			int_const_node icn=(int_const_node)cn;
			return icn.const_value;
		}

		public int char_to_int(constant_node cn)
		{
#if (DEBUG)
			if (cn.semantic_node_type!=semantic_node_type.char_const_node)
			{
				throw new CompilerInternalError("Error in static type conversion");
			}
#endif
			char_const_node icn=(char_const_node)cn;
			return ((int)icn.const_value);
		}

		private void mark_int_as_ordinal()
		{
			ordinal_type_interface oti=new ordinal_type_interface();
			oti.inc_method=create_oti_method(SemanticTree.basic_function_type.iinc,_integer_type);
			oti.dec_method=create_oti_method(SemanticTree.basic_function_type.idec,_integer_type);
			
			SymbolInfo si=_integer_type.find_in_additional_names(compiler_string_consts.greq_name);
			basic_function_node bfn=(basic_function_node)si.sym_info;
			
			oti.greater_eq_method=bfn;

			si=_integer_type.find_in_additional_names(compiler_string_consts.smeq_name);
			bfn=(basic_function_node)si.sym_info;

			oti.lower_eq_method=bfn;

			constant_node cn_max=new int_const_node(int.MinValue,_integer_type);
			constant_node cn_min=new int_const_node(int.MinValue,_integer_type);

			oti.upper_value=cn_max;
			oti.lower_value=cn_min;

			parameterArrayList pal=new parameterArrayList();
			empty_function_node efn=new empty_function_node(pal,_integer_type);
			common_parameter cp=new common_parameter(compiler_string_consts.unary_param_name,_integer_type,
				SemanticTree.parameter_type.value,null);
			
			oti.value_to_int=efn;
			oti.ordinal_type_to_int=new ordinal_type_to_int(int_to_int);
			_integer_type.add_internal_interface(oti);
		}

		private void mark_char_as_ordinal()
		{
			ordinal_type_interface oti=new ordinal_type_interface();
			oti.inc_method=create_oti_method(SemanticTree.basic_function_type.cinc,_char_type);
			oti.dec_method=create_oti_method(SemanticTree.basic_function_type.cdec,_char_type);
			
			SymbolInfo si=_char_type.find_in_additional_names(compiler_string_consts.greq_name);
			basic_function_node bfn=(basic_function_node)si.sym_info;
			
			oti.greater_eq_method=bfn;

			si=_char_type.find_in_additional_names(compiler_string_consts.smeq_name);
			bfn=(basic_function_node)si.sym_info;

			oti.lower_eq_method=bfn;

			constant_node cn_max=new int_const_node(char.MaxValue,_char_type);
			constant_node cn_min=new int_const_node(char.MinValue,_char_type);

			oti.upper_value=cn_max;
			oti.lower_value=cn_min;

			parameterArrayList pal=new parameterArrayList();
			bfn=new basic_function_node(SemanticTree.basic_function_type.chartoi,pal,_integer_type);
			common_parameter cp=new common_parameter(compiler_string_consts.unary_param_name,_char_type,
				SemanticTree.parameter_type.value,null);
			
			oti.value_to_int=bfn;
			oti.ordinal_type_to_int=new ordinal_type_to_int(char_to_int);
			_char_type.add_internal_interface(oti);
		}

		//Спросить у Вани. Мой NetHelper не поддерживает такие операциию
		private void NetInitialize()
		{
			/*
			NetHelper.AddType(typeof(int), _integer_type);
			NetHelper.AddType(typeof(bool), _bool_type);
			NetHelper.AddType(typeof(double), _real_type);
			NetHelper.AddType(typeof(char), _char_type);
			NetHelper.AddType(typeof(string), _string_type);
			*/
		}

		private void initialize_types()
		{
			_integer_type=compiled_type_node.get_type_node(typeof(int));
			_bool_type=compiled_type_node.get_type_node(typeof(bool));
			_real_type=compiled_type_node.get_type_node(typeof(double));
			_char_type=compiled_type_node.get_type_node(typeof(char));
			_string_type=compiled_type_node.get_type_node(typeof(string));

			_pointer_type=compiled_type_node.get_type_node(Type.GetType("System.Void*"));
			NetInitialize();

			//Преобразования типов.
			make_type_conversion(_integer_type,_real_type,type_compare.less_type,SemanticTree.basic_function_type.itod);

			//Операции для типа integer.
			//Присваивание для integer.
			make_assign_operator(_integer_type,SemanticTree.basic_function_type.iassign);

			//Унарные операции.
			make_unary_operator(compiler_string_consts.minus_name,_integer_type,SemanticTree.basic_function_type.iunmin);
			make_unary_operator(compiler_string_consts.not_name,_integer_type,SemanticTree.basic_function_type.inot);
			make_empty_operator(compiler_string_consts.plus_name,_integer_type);
			
			//Арифметические операции.
			_int_add=make_binary_operator(compiler_string_consts.plus_name,_integer_type,SemanticTree.basic_function_type.iadd);
			_int_sub=make_binary_operator(compiler_string_consts.minus_name,_integer_type,SemanticTree.basic_function_type.isub);
			make_binary_operator(compiler_string_consts.mul_name,_integer_type,SemanticTree.basic_function_type.imul);
			make_binary_operator(compiler_string_consts.idiv_name,_integer_type,SemanticTree.basic_function_type.idiv);
			make_binary_operator(compiler_string_consts.mod_name,_integer_type,SemanticTree.basic_function_type.imod);
			//Операция / для integer.
			make_common_binary_operation(compiler_string_consts.div_name,_integer_type,_real_type,_real_type,
				SemanticTree.basic_function_type.ddiv,_real_type);

			//Опрерации сравнения.
			make_binary_operator(compiler_string_consts.gr_name,_integer_type,SemanticTree.basic_function_type.igr,_bool_type);
			make_binary_operator(compiler_string_consts.greq_name,_integer_type,SemanticTree.basic_function_type.igreq,_bool_type);
			make_binary_operator(compiler_string_consts.sm_name,_integer_type,SemanticTree.basic_function_type.ism,_bool_type);
			make_binary_operator(compiler_string_consts.smeq_name,_integer_type,SemanticTree.basic_function_type.ismeq,_bool_type);
			make_binary_operator(compiler_string_consts.eq_name,_integer_type,SemanticTree.basic_function_type.ieq,_bool_type);
			make_binary_operator(compiler_string_consts.noteq_name,_integer_type,SemanticTree.basic_function_type.inoteq,_bool_type);

			//Логические опреции.
			make_binary_operator(compiler_string_consts.and_name,_integer_type,SemanticTree.basic_function_type.iand);
			make_binary_operator(compiler_string_consts.or_name,_integer_type,SemanticTree.basic_function_type.ior);
			make_binary_operator(compiler_string_consts.xor_name,_integer_type,SemanticTree.basic_function_type.ixor);
			make_binary_operator(compiler_string_consts.shl_name,_integer_type,SemanticTree.basic_function_type.ishl);
			make_binary_operator(compiler_string_consts.shr_name,_integer_type,SemanticTree.basic_function_type.ishr);

			//real type.
			//Assign.
			make_assign_operator(_real_type,SemanticTree.basic_function_type.dassign);

			//Унарные операции.
			make_unary_operator(compiler_string_consts.minus_name,_real_type,SemanticTree.basic_function_type.dunmin);
			make_empty_operator(compiler_string_consts.plus_name,_real_type);

			//Арифметические операции.
			make_binary_operator(compiler_string_consts.plus_name,_real_type,SemanticTree.basic_function_type.dadd);
			make_binary_operator(compiler_string_consts.minus_name,_real_type,SemanticTree.basic_function_type.dsub);
			make_binary_operator(compiler_string_consts.mul_name,_real_type,SemanticTree.basic_function_type.dmul);
			make_binary_operator(compiler_string_consts.div_name,_real_type,SemanticTree.basic_function_type.ddiv);

			//Опрерации сравнения.
			make_binary_operator(compiler_string_consts.gr_name,_real_type,SemanticTree.basic_function_type.dgr,_bool_type);
			make_binary_operator(compiler_string_consts.greq_name,_real_type,SemanticTree.basic_function_type.dgreq,_bool_type);
			make_binary_operator(compiler_string_consts.sm_name,_real_type,SemanticTree.basic_function_type.dsm,_bool_type);
			make_binary_operator(compiler_string_consts.smeq_name,_real_type,SemanticTree.basic_function_type.dsmeq,_bool_type);
			make_binary_operator(compiler_string_consts.eq_name,_real_type,SemanticTree.basic_function_type.deq,_bool_type);
			make_binary_operator(compiler_string_consts.noteq_name,_real_type,SemanticTree.basic_function_type.dnoteq,_bool_type);

			//char type.
			//Assign.
			make_assign_operator(_char_type,SemanticTree.basic_function_type.charassign);

			//Опрерации сравнения.
			make_binary_operator(compiler_string_consts.gr_name,_char_type,SemanticTree.basic_function_type.chargr,_bool_type);
			make_binary_operator(compiler_string_consts.greq_name,_char_type,SemanticTree.basic_function_type.chargreq,_bool_type);
			make_binary_operator(compiler_string_consts.sm_name,_char_type,SemanticTree.basic_function_type.charsm,_bool_type);
			make_binary_operator(compiler_string_consts.smeq_name,_char_type,SemanticTree.basic_function_type.charsmeq,_bool_type);
			make_binary_operator(compiler_string_consts.eq_name,_char_type,SemanticTree.basic_function_type.chareq,_bool_type);
			make_binary_operator(compiler_string_consts.noteq_name,_char_type,SemanticTree.basic_function_type.charnoteq,_bool_type);

			//boolean type.
			//Assign.
			make_assign_operator(_bool_type,SemanticTree.basic_function_type.boolassign);

			//Логические операции.
			//Унарные операции.
			make_unary_operator(compiler_string_consts.not_name,_bool_type,SemanticTree.basic_function_type.bnot);

			//Логическме операции.
			make_binary_operator(compiler_string_consts.and_name,_bool_type,SemanticTree.basic_function_type.band);
			make_binary_operator(compiler_string_consts.or_name,_bool_type,SemanticTree.basic_function_type.bor);
			make_binary_operator(compiler_string_consts.xor_name,_bool_type,SemanticTree.basic_function_type.bxor);
			make_unary_operator(compiler_string_consts.not_name,_bool_type,SemanticTree.basic_function_type.boolnot);

			//Опрерации сравнения.
			make_binary_operator(compiler_string_consts.gr_name,_bool_type,SemanticTree.basic_function_type.boolgr);
			make_binary_operator(compiler_string_consts.greq_name,_bool_type,SemanticTree.basic_function_type.boolgreq);
			make_binary_operator(compiler_string_consts.sm_name,_bool_type,SemanticTree.basic_function_type.boolsm);
			make_binary_operator(compiler_string_consts.smeq_name,_bool_type,SemanticTree.basic_function_type.boolsmeq);
			make_binary_operator(compiler_string_consts.eq_name,_bool_type,SemanticTree.basic_function_type.booleq);
			make_binary_operator(compiler_string_consts.noteq_name,_bool_type,SemanticTree.basic_function_type.boolnoteq);

			mark_int_as_ordinal();
			mark_char_as_ordinal();
		}

		private void init_constants(SymbolTable.Scope sc)
		{
			_true_constant=new bool_const_node(true,_bool_type);
			_false_constant=new bool_const_node(false,_bool_type);

			_true_constant_definition=new constant_definition_node(compiler_string_consts.true_const_name,_true_constant);
			_false_constant_definition=new constant_definition_node(compiler_string_consts.false_const_name,_false_constant);

			sc.AddSymbol(compiler_string_consts.true_const_name,new SymbolInfo(_true_constant_definition));
			sc.AddSymbol(compiler_string_consts.false_const_name,new SymbolInfo(_false_constant_definition));
		}

		private void init_temp_methods(SymbolTable.Scope sc)
		{
			//TODO: Сделано по быстрому. Переделать.
			Type tp=typeof(Console);
			compiled_function_node cfn;
			System.Type[] arr=new System.Type[1];
			System.Reflection.MethodInfo mi;
			
			arr[0]=typeof(int);
			mi=tp.GetMethod("WriteLine",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_integer_type,SemanticTree.parameter_type.value,null));
			sc.AddSymbol("writeln",new SymbolInfo(cfn));

			arr[0]=typeof(double);
			mi=tp.GetMethod("WriteLine",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_real_type,SemanticTree.parameter_type.value,null));
			sc.AddSymbol("writeln",new SymbolInfo(cfn));

			arr[0]=typeof(char);
			mi=tp.GetMethod("WriteLine",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_char_type,SemanticTree.parameter_type.value,null));
			sc.AddSymbol("writeln",new SymbolInfo(cfn));

			arr[0]=typeof(bool);
			mi=tp.GetMethod("WriteLine",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_bool_type,SemanticTree.parameter_type.value,null));
			sc.AddSymbol("writeln",new SymbolInfo(cfn));

			arr[0]=typeof(string);
			mi=tp.GetMethod("WriteLine",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_string_type,SemanticTree.parameter_type.value,null));
			sc.AddSymbol("writeln",new SymbolInfo(cfn));

			mi=tp.GetMethod("ReadLine",new System.Type[0]);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.return_value_type=_string_type;
			sc.AddSymbol("readline",new SymbolInfo(cfn));

			make_assign_operator(_string_type,SemanticTree.basic_function_type.objassign);

			tp=typeof(int);
			arr[0]=typeof(string);
			mi=tp.GetMethod("Parse",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_string_type,SemanticTree.parameter_type.value,null));
			cfn.return_value_type=_integer_type;
			sc.AddSymbol("parseint",new SymbolInfo(cfn));

			tp=typeof(double);
			arr[0]=typeof(string);
			mi=tp.GetMethod("Parse",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_string_type,SemanticTree.parameter_type.value,null));
			cfn.return_value_type=_real_type;
			sc.AddSymbol("parsereal",new SymbolInfo(cfn));

			tp=typeof(bool);
			arr[0]=typeof(string);
			mi=tp.GetMethod("Parse",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_string_type,SemanticTree.parameter_type.value,null));
			cfn.return_value_type=_bool_type;
			sc.AddSymbol("parsebool",new SymbolInfo(cfn));

			tp=typeof(char);
			arr[0]=typeof(string);
			mi=tp.GetMethod("Parse",arr);
			cfn=new compiled_function_node(mi);
			cfn.parameters.Clear();
			cfn.parameters.Add(new common_parameter("val",_string_type,SemanticTree.parameter_type.value,null));
			cfn.return_value_type=_char_type;
			sc.AddSymbol("parsechar",new SymbolInfo(cfn));
			common_namespace_function_node cnfn = new common_namespace_function_node("New",null);
			cnfn.parameters.Clear();
			cnfn.parameters.Add(new common_parameter("ptr",_pointer_type,SemanticTree.parameter_type.value,null));
			sc.AddSymbol("new",new SymbolInfo(cnfn));
			
			cnfn = new common_namespace_function_node("Dispose",null);
			cnfn.parameters.Clear();
			cnfn.parameters.Add(new common_parameter("ptr",_pointer_type,SemanticTree.parameter_type.value,null));
			sc.AddSymbol("dispose",new SymbolInfo(cnfn));
		}

		private void make_types()
		{
			/*
			if (_integer_type!=null)
			{
				return;
			}
			*/
			initialize_types();
		}

		private void init_procedures(convertion_data_and_alghoritms conv,SymbolTable.Scope susc)
		{
			pseudo_function break_procedure=new pseudo_function(compiler_string_consts.break_procedure_name,
				new pseudo_function_algorithm(conv.syntax_tree_visitor.make_break_node));

			susc.AddSymbol(compiler_string_consts.break_procedure_name,new SymbolInfo(break_procedure));

			pseudo_function continue_procedure=new pseudo_function(compiler_string_consts.continue_procedure_name,
				new pseudo_function_algorithm(conv.syntax_tree_visitor.make_continue_node));

			susc.AddSymbol(compiler_string_consts.continue_procedure_name,new SymbolInfo(continue_procedure));
		}

		private void make_system_unit(convertion_data_and_alghoritms conv)
		{
			_system_unit=new unit_node();
			_system_unit.scope=conv.symbol_table.CreateUnitInterfaceScope(new SymbolTable.Scope[0]);

			SymbolTable.Scope susc=_system_unit.scope;
			//Добавляем типы.
			susc.AddSymbol(compiler_string_consts.integer_type_name,new SymbolInfo(_integer_type));
			susc.AddSymbol(compiler_string_consts.real_type_name,new SymbolInfo(_real_type));
			susc.AddSymbol(compiler_string_consts.char_type_name,new SymbolInfo(_char_type));
			susc.AddSymbol(compiler_string_consts.bool_type_name,new SymbolInfo(_bool_type));
			susc.AddSymbol(compiler_string_consts.string_type_name,new SymbolInfo(_string_type));

			susc.AddSymbol(compiler_string_consts.pointer_type_name,new SymbolInfo(_pointer_type));
			init_constants(susc);
			init_procedures(conv,susc);
			init_temp_methods(susc);
		}

		private void make_object_operator(common_type_node ctn,SemanticTree.basic_function_type bas_ft,
			string name,type_node ret_type,SemanticTree.parameter_type first_parameter_type)
		{
			parameterArrayList pars=new parameterArrayList();
			basic_function_node bfn=new basic_function_node(bas_ft,pars,ret_type);
			bfn.is_overload=true;
			basic_parameter to=new basic_parameter(compiler_string_consts.left_param_name,ctn,
				first_parameter_type,bfn);
			basic_parameter from=new basic_parameter(compiler_string_consts.right_param_name,ctn,
				SemanticTree.parameter_type.value,bfn);
			pars.Add(to);
			pars.Add(from);
			ctn.Scope.AddSymbol(name,new SymbolInfo(bfn));
		}

		private void make_object_operator(common_type_node ctn,SemanticTree.basic_function_type bas_ft,string name,
			type_node ret_type)
		{
			make_object_operator(ctn,bas_ft,name,ret_type,SemanticTree.parameter_type.value);
		}

		private void make_assign_operator(common_type_node ctn)
		{
			make_object_operator(ctn,SemanticTree.basic_function_type.objassign,compiler_string_consts.assign_name,
				ctn,SemanticTree.parameter_type.var);
		}

		private void make_equivalence_operator(common_type_node ctn)
		{
			make_object_operator(ctn,SemanticTree.basic_function_type.objeq,compiler_string_consts.eq_name,_bool_type);
		}

		private void make_not_equivalence_operator(common_type_node ctn)
		{
			make_object_operator(ctn,SemanticTree.basic_function_type.objnoteq,compiler_string_consts.noteq_name,_bool_type);
		}

		public void init_reference_type(common_type_node ctn)
		{
			make_assign_operator(ctn);
			make_equivalence_operator(ctn);
			make_not_equivalence_operator(ctn);
		}

		public bool is_initialized
		{
			get
			{
				return (_integer_type!=null);
			}
		}

		public void initialize(convertion_data_and_alghoritms conv)
		{
			if (is_initialized)
			{
				return;
			}
			make_types();
			make_system_unit(conv);
		}

		public system_lib_initializer()
		{
			compiled_type_node.set_system_lib_initializer(this);
			ref_type_node.set_system_lib_initializer(this);
		}


	}

}