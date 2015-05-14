
namespace PascalABCCompiler.SyntaxTree
{

	public class AbstractVisitor: IVisitor
	{

		public virtual void visit(syntax_tree_node _syntax_tree_node)
		{
		}

		public virtual void visit(expression _expression)
		{
		}

		public virtual void visit(statement _statement)
		{
		}

		public virtual void visit(statement_list _statement_list)
		{
		}

		public virtual void visit(ident _ident)
		{
		}

		public virtual void visit(assign _assign)
		{
		}

		public virtual void visit(bin_expr _bin_expr)
		{
		}

		public virtual void visit(un_expr _un_expr)
		{
		}

		public virtual void visit(const_node _const_node)
		{
		}

		public virtual void visit(bool_const _bool_const)
		{
		}

		public virtual void visit(int32_const _int32_const)
		{
		}

		public virtual void visit(double_const _double_const)
		{
		}

		public virtual void visit(subprogram_body _subprogram_body)
		{
		}

		public virtual void visit(addressed_value _addressed_value)
		{
		}

		public virtual void visit(type_definition _type_definition)
		{
		}

		public virtual void visit(roof_dereference _roof_dereference)
		{
		}

		public virtual void visit(named_type_reference _named_type_reference)
		{
		}

		public virtual void visit(variable_definitions _variable_definitions)
		{
		}

		public virtual void visit(ident_list _ident_list)
		{
		}

		public virtual void visit(var_def_statement _var_def_statement)
		{
		}

		public virtual void visit(declaration _declaration)
		{
		}

		public virtual void visit(declarations _declarations)
		{
		}

		public virtual void visit(program_tree _program_tree)
		{
		}

		public virtual void visit(program_name _program_name)
		{
		}

		public virtual void visit(string_const _string_const)
		{
		}

		public virtual void visit(expression_list _expression_list)
		{
		}

		public virtual void visit(dereference _dereference)
		{
		}

		public virtual void visit(indexer _indexer)
		{
		}

		public virtual void visit(for_node _for_node)
		{
		}

		public virtual void visit(repeat_node _repeat_node)
		{
		}

		public virtual void visit(while_node _while_node)
		{
		}

		public virtual void visit(if_node _if_node)
		{
		}

		public virtual void visit(ref_type _ref_type)
		{
		}

		public virtual void visit(diapason _diapason)
		{
		}

		public virtual void visit(indexers_types _indexers_types)
		{
		}

		public virtual void visit(array_type _array_type)
		{
		}

		public virtual void visit(label_definitions _label_definitions)
		{
		}

		public virtual void visit(procedure_attribute _procedure_attribute)
		{
		}

		public virtual void visit(typed_parameters _typed_parameters)
		{
		}

		public virtual void visit(formal_parameters _formal_parameters)
		{
		}

		public virtual void visit(procedure_attributes_list _procedure_attributes_list)
		{
		}

		public virtual void visit(procedure_header _procedure_header)
		{
		}

		public virtual void visit(function_header _function_header)
		{
		}

		public virtual void visit(procedure_definition _procedure_definition)
		{
		}

		public virtual void visit(type_declaration _type_declaration)
		{
		}

		public virtual void visit(type_declarations _type_declarations)
		{
		}

		public virtual void visit(simple_const_definition _simple_const_definition)
		{
		}

		public virtual void visit(typed_const_definition _typed_const_definition)
		{
		}

		public virtual void visit(const_definition _const_definition)
		{
		}

		public virtual void visit(consts_definitions_list _consts_definitions_list)
		{
		}

		public virtual void visit(unit_name _unit_name)
		{
		}

		public virtual void visit(unit_or_namespace _unit_or_namespace)
		{
		}

		public virtual void visit(uses_unit_in _uses_unit_in)
		{
		}

		public virtual void visit(uses_list _uses_list)
		{
		}

		public virtual void visit(program_body _program_body)
		{
		}

		public virtual void visit(compilation_unit _compilation_unit)
		{
		}

		public virtual void visit(unit_module _unit_module)
		{
		}

		public virtual void visit(program_module _program_module)
		{
		}

		public virtual void visit(hex_constant _hex_constant)
		{
		}

		public virtual void visit(get_address _get_address)
		{
		}

		public virtual void visit(case_variant _case_variant)
		{
		}

		public virtual void visit(case_node _case_node)
		{
		}

		public virtual void visit(method_name _method_name)
		{
		}

		public virtual void visit(dot_node _dot_node)
		{
		}

		public virtual void visit(empty_statement _empty_statement)
		{
		}

		public virtual void visit(goto_statement _goto_statement)
		{
		}

		public virtual void visit(labeled_statement _labeled_statement)
		{
		}

		public virtual void visit(with_statement _with_statement)
		{
		}

		public virtual void visit(method_call _method_call)
		{
		}

		public virtual void visit(pascal_set_constant _pascal_set_constant)
		{
		}

		public virtual void visit(array_const _array_const)
		{
		}

		public virtual void visit(write_accessor_name _write_accessor_name)
		{
		}

		public virtual void visit(read_accessor_name _read_accessor_name)
		{
		}

		public virtual void visit(property_accessors _property_accessors)
		{
		}

		public virtual void visit(simple_property _simple_property)
		{
		}

		public virtual void visit(index_property _index_property)
		{
		}

		public virtual void visit(class_members _class_members)
		{
		}

		public virtual void visit(access_modifer_node _access_modifer_node)
		{
		}

		public virtual void visit(class_body _class_body)
		{
		}

		public virtual void visit(class_definition _class_definition)
		{
		}

		public virtual void visit(default_indexer_property_node _default_indexer_property_node)
		{
		}

		public virtual void visit(known_type_definition _known_type_definition)
		{
		}

		public virtual void visit(set_type_definition _set_type_definition)
		{
		}

		public virtual void visit(try_statement _try_statement)
		{
		}

		public virtual void visit(on_exception _on_exception)
		{
		}

		public virtual void visit(on_exception_list _on_exception_list)
		{
		}

		public virtual void visit(try_finally_statement _try_finally_statement)
		{
		}

		public virtual void visit(try_except_statement _try_except_statement)
		{
		}

		public virtual void visit(record_const_definition _record_const_definition)
		{
		}

		public virtual void visit(record_const _record_const)
		{
		}

		public virtual void visit(record_type _record_type)
		{
		}

		public virtual void visit(enum_type_definition _enum_type_definition)
		{
		}

		public virtual void visit(char_const _char_const)
		{
		}

		public virtual void visit(raise_statement _raise_statement)
		{
		}

		public virtual void visit(sharp_char_const _sharp_char_const)
		{
		}

		public virtual void visit(literal_const_line _literal_const_line)
		{
		}

		public virtual void visit(string_num_definition _string_num_definition)
		{
		}

		public virtual void visit(variant _variant)
		{
		}

		public virtual void visit(variant_list _variant_list)
		{
		}

		public virtual void visit(variant_type _variant_type)
		{
		}

		public virtual void visit(variant_types _variant_types)
		{
		}

		public virtual void visit(variant_record_type _variant_record_type)
		{
		}

		public virtual void visit(procedure_call _procedure_call)
		{
		}

		public virtual void visit(class_predefinition _class_predefinition)
		{
		}

		public virtual void visit(nil_const _nil_const)
		{
		}

		public virtual void visit(file_type_definition _file_type_definition)
		{
		}

		public virtual void visit(constructor _constructor)
		{
		}

		public virtual void visit(destructor _destructor)
		{
		}

		public virtual void visit(inherited_method_call _inherited_method_call)
		{
		}

		public virtual void visit(typecast_node _typecast_node)
		{
		}

		public virtual void visit(interface_node _interface_node)
		{
		}

		public virtual void visit(implementation_node _implementation_node)
		{
		}

		public virtual void visit(diap_expr _diap_expr)
		{
		}

		public virtual void visit(block _block)
		{
		}

		public virtual void visit(proc_block _proc_block)
		{
		}

		public virtual void visit(array_of_named_type_definition _array_of_named_type_definition)
		{
		}

		public virtual void visit(array_of_const_type_definition _array_of_const_type_definition)
		{
		}

		public virtual void visit(literal _literal)
		{
		}

		public virtual void visit(case_variants _case_variants)
		{
		}

		public virtual void visit(diapason_expr _diapason_expr)
		{
		}

		public virtual void visit(var_def_list _var_def_list)
		{
		}

		public virtual void visit(record_type_parts _record_type_parts)
		{
		}

		public virtual void visit(property_array_default _property_array_default)
		{
		}

		public virtual void visit(property_interface _property_interface)
		{
		}

		public virtual void visit(property_parameter _property_parameter)
		{
		}

		public virtual void visit(property_parameter_list _property_parameter_list)
		{
		}

		public virtual void visit(inherited_ident _inherited_ident)
		{
		}

		public virtual void visit(format_expr _format_expr)
		{
		}

		public virtual void visit(initfinal_part _initfinal_part)
		{
		}

		public virtual void visit(token_info _token_info)
		{
		}

		public virtual void visit(raise_stmt _raise_stmt)
		{
		}

		public virtual void visit(op_type_node _op_type_node)
		{
		}

		public virtual void visit(file_type _file_type)
		{
		}

		public virtual void visit(known_type_ident _known_type_ident)
		{
		}

		public virtual void visit(exception_handler _exception_handler)
		{
		}

		public virtual void visit(exception_ident _exception_ident)
		{
		}

		public virtual void visit(exception_handler_list _exception_handler_list)
		{
		}

		public virtual void visit(exception_block _exception_block)
		{
		}

		public virtual void visit(try_handler _try_handler)
		{
		}

		public virtual void visit(try_handler_finally _try_handler_finally)
		{
		}

		public virtual void visit(try_handler_except _try_handler_except)
		{
		}

		public virtual void visit(try_stmt _try_stmt)
		{
		}

		public virtual void visit(inherited_message _inherited_message)
		{
		}

		public virtual void visit(external_directive _external_directive)
		{
		}

		public virtual void visit(using_list _using_list)
		{
		}

		public virtual void visit(oberon_import_module _oberon_import_module)
		{
		}

		public virtual void visit(oberon_module _oberon_module)
		{
		}

		public virtual void visit(oberon_ident_with_export_marker _oberon_ident_with_export_marker)
		{
		}

		public virtual void visit(oberon_exit_stmt _oberon_exit_stmt)
		{
		}

		public virtual void visit(jump_stmt _jump_stmt)
		{
		}

		public virtual void visit(oberon_procedure_receiver _oberon_procedure_receiver)
		{
		}

		public virtual void visit(oberon_procedure_header _oberon_procedure_header)
		{
		}

		public virtual void visit(oberon_withstmt_guardstat _oberon_withstmt_guardstat)
		{
		}

		public virtual void visit(oberon_withstmt_guardstat_list _oberon_withstmt_guardstat_list)
		{
		}

		public virtual void visit(oberon_withstmt _oberon_withstmt)
		{
		}

		public virtual void visit(loop_stmt _loop_stmt)
		{
		}

		public virtual void visit(foreach_stmt _foreach_stmt)
		{
		}

		public virtual void visit(addressed_value_funcname _addressed_value_funcname)
		{
		}

		public virtual void visit(named_type_reference_list _named_type_reference_list)
		{
		}

		public virtual void visit(template_param_list _template_param_list)
		{
		}

		public virtual void visit(template_type_reference _template_type_reference)
		{
		}

		public virtual void visit(int64_const _int64_const)
		{
		}

		public virtual void visit(uint64_const _uint64_const)
		{
		}

		public virtual void visit(new_expr _new_expr)
		{
		}

		public virtual void visit(type_definition_list _type_definition_list)
		{
		}

		public virtual void visit(where_definition _where_definition)
		{
		}

		public virtual void visit(where_definition_list _where_definition_list)
		{
		}

		public virtual void visit(sizeof_operator _sizeof_operator)
		{
		}

		public virtual void visit(typeof_operator _typeof_operator)
		{
		}

		public virtual void visit(compiler_directive _compiler_directive)
		{
		}

		public virtual void visit(operator_name_ident _operator_name_ident)
		{
		}

		public virtual void visit(var_statement _var_statement)
		{
		}

		public virtual void visit(question_colon_expression _question_colon_expression)
		{
		}

		public virtual void visit(expression_as_statement _expression_as_statement)
		{
		}

		public virtual void visit(c_scalar_type _c_scalar_type)
		{
		}

		public virtual void visit(c_module _c_module)
		{
		}

		public virtual void visit(declarations_as_statement _declarations_as_statement)
		{
		}

		public virtual void visit(array_size _array_size)
		{
		}

		public virtual void visit(enumerator _enumerator)
		{
		}

		public virtual void visit(enumerator_list _enumerator_list)
		{
		}

		public virtual void visit(c_for_cycle _c_for_cycle)
		{
		}

		public virtual void visit(switch_stmt _switch_stmt)
		{
		}

		public virtual void visit(type_definition_attr_list _type_definition_attr_list)
		{
		}

		public virtual void visit(type_definition_attr _type_definition_attr)
		{
		}

		public virtual void visit(lock_stmt _lock_stmt)
		{
		}

		public virtual void visit(compiler_directive_list _compiler_directive_list)
		{
		}

		public virtual void visit(compiler_directive_if _compiler_directive_if)
		{
		}

		public virtual void visit(documentation_comment_list _documentation_comment_list)
		{
		}

		public virtual void visit(documentation_comment_tag _documentation_comment_tag)
		{
		}

		public virtual void visit(documentation_comment_tag_param _documentation_comment_tag_param)
		{
		}

		public virtual void visit(documentation_comment_section _documentation_comment_section)
		{
		}

		public virtual void visit(token_taginfo _token_taginfo)
		{
		}

		public virtual void visit(declaration_specificator _declaration_specificator)
		{
		}

		public virtual void visit(ident_with_templateparams _ident_with_templateparams)
		{
		}

		public virtual void visit(template_type_name _template_type_name)
		{
		}

		public virtual void visit(default_operator _default_operator)
		{
		}

		public virtual void visit(bracket_expr _bracket_expr)
		{
		}

		public virtual void visit(attribute _attribute)
		{
		}

		public virtual void visit(simple_attribute_list _simple_attribute_list)
		{
		}

		public virtual void visit(attribute_list _attribute_list)
		{
		}

		public virtual void visit(function_lambda_definition _function_lambda_definition)
		{
		}

		public virtual void visit(function_lambda_call _function_lambda_call)
		{
		}

		public virtual void visit(semantic_check _semantic_check)
		{
		}

		public virtual void visit(lambda_inferred_type _lambda_inferred_type)
		{
		}

		public virtual void visit(same_type_node _same_type_node)
		{
		}

		public virtual void visit(name_assign_expr _name_assign_expr)
		{
		}

		public virtual void visit(name_assign_expr_list _name_assign_expr_list)
		{
		}

		public virtual void visit(unnamed_type_object _unnamed_type_object)
		{
		}

		public virtual void visit(semantic_type_node _semantic_type_node)
		{
		}

		public virtual void visit(short_func_definition _short_func_definition)
		{
		}

		public virtual void visit(no_type_foreach _no_type_foreach)
		{
		}

		public virtual void visit(matching_expression _matching_expression)
		{
		}

		public virtual void visit(closure_substituting_node _closure_substituting_node)
		{
		}

		public virtual void visit(sequence_type _sequence_type)
		{
		}
	}


}

