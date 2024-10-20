
namespace PascalABCCompiler.SyntaxTree
{

	public class AbstractVisitor: IVisitor
	{
		public virtual void DefaultVisit(syntax_tree_node n)
		{
		}

		public virtual void visit(expression _expression)
		{
			DefaultVisit(_expression);
		}

		public virtual void visit(syntax_tree_node _syntax_tree_node)
		{
			DefaultVisit(_syntax_tree_node);
		}

		public virtual void visit(statement _statement)
		{
			DefaultVisit(_statement);
		}

		public virtual void visit(statement_list _statement_list)
		{
			DefaultVisit(_statement_list);
		}

		public virtual void visit(ident _ident)
		{
			DefaultVisit(_ident);
		}

		public virtual void visit(assign _assign)
		{
			DefaultVisit(_assign);
		}

		public virtual void visit(bin_expr _bin_expr)
		{
			DefaultVisit(_bin_expr);
		}

		public virtual void visit(un_expr _un_expr)
		{
			DefaultVisit(_un_expr);
		}

		public virtual void visit(const_node _const_node)
		{
			DefaultVisit(_const_node);
		}

		public virtual void visit(bool_const _bool_const)
		{
			DefaultVisit(_bool_const);
		}

		public virtual void visit(int32_const _int32_const)
		{
			DefaultVisit(_int32_const);
		}

		public virtual void visit(double_const _double_const)
		{
			DefaultVisit(_double_const);
		}

		public virtual void visit(subprogram_body _subprogram_body)
		{
			DefaultVisit(_subprogram_body);
		}

		public virtual void visit(addressed_value _addressed_value)
		{
			DefaultVisit(_addressed_value);
		}

		public virtual void visit(type_definition _type_definition)
		{
			DefaultVisit(_type_definition);
		}

		public virtual void visit(roof_dereference _roof_dereference)
		{
			DefaultVisit(_roof_dereference);
		}

		public virtual void visit(named_type_reference _named_type_reference)
		{
			DefaultVisit(_named_type_reference);
		}

		public virtual void visit(variable_definitions _variable_definitions)
		{
			DefaultVisit(_variable_definitions);
		}

		public virtual void visit(ident_list _ident_list)
		{
			DefaultVisit(_ident_list);
		}

		public virtual void visit(var_def_statement _var_def_statement)
		{
			DefaultVisit(_var_def_statement);
		}

		public virtual void visit(declaration _declaration)
		{
			DefaultVisit(_declaration);
		}

		public virtual void visit(declarations _declarations)
		{
			DefaultVisit(_declarations);
		}

		public virtual void visit(program_tree _program_tree)
		{
			DefaultVisit(_program_tree);
		}

		public virtual void visit(program_name _program_name)
		{
			DefaultVisit(_program_name);
		}

		public virtual void visit(string_const _string_const)
		{
			DefaultVisit(_string_const);
		}

		public virtual void visit(expression_list _expression_list)
		{
			DefaultVisit(_expression_list);
		}

		public virtual void visit(dereference _dereference)
		{
			DefaultVisit(_dereference);
		}

		public virtual void visit(indexer _indexer)
		{
			DefaultVisit(_indexer);
		}

		public virtual void visit(for_node _for_node)
		{
			DefaultVisit(_for_node);
		}

		public virtual void visit(repeat_node _repeat_node)
		{
			DefaultVisit(_repeat_node);
		}

		public virtual void visit(while_node _while_node)
		{
			DefaultVisit(_while_node);
		}

		public virtual void visit(if_node _if_node)
		{
			DefaultVisit(_if_node);
		}

		public virtual void visit(ref_type _ref_type)
		{
			DefaultVisit(_ref_type);
		}

		public virtual void visit(diapason _diapason)
		{
			DefaultVisit(_diapason);
		}

		public virtual void visit(indexers_types _indexers_types)
		{
			DefaultVisit(_indexers_types);
		}

		public virtual void visit(array_type _array_type)
		{
			DefaultVisit(_array_type);
		}

		public virtual void visit(label_definitions _label_definitions)
		{
			DefaultVisit(_label_definitions);
		}

		public virtual void visit(procedure_attribute _procedure_attribute)
		{
			DefaultVisit(_procedure_attribute);
		}

		public virtual void visit(typed_parameters _typed_parameters)
		{
			DefaultVisit(_typed_parameters);
		}

		public virtual void visit(formal_parameters _formal_parameters)
		{
			DefaultVisit(_formal_parameters);
		}

		public virtual void visit(procedure_attributes_list _procedure_attributes_list)
		{
			DefaultVisit(_procedure_attributes_list);
		}

		public virtual void visit(procedure_header _procedure_header)
		{
			DefaultVisit(_procedure_header);
		}

		public virtual void visit(function_header _function_header)
		{
			DefaultVisit(_function_header);
		}

		public virtual void visit(procedure_definition _procedure_definition)
		{
			DefaultVisit(_procedure_definition);
		}

		public virtual void visit(type_declaration _type_declaration)
		{
			DefaultVisit(_type_declaration);
		}

		public virtual void visit(type_declarations _type_declarations)
		{
			DefaultVisit(_type_declarations);
		}

		public virtual void visit(simple_const_definition _simple_const_definition)
		{
			DefaultVisit(_simple_const_definition);
		}

		public virtual void visit(typed_const_definition _typed_const_definition)
		{
			DefaultVisit(_typed_const_definition);
		}

		public virtual void visit(const_definition _const_definition)
		{
			DefaultVisit(_const_definition);
		}

		public virtual void visit(consts_definitions_list _consts_definitions_list)
		{
			DefaultVisit(_consts_definitions_list);
		}

		public virtual void visit(unit_name _unit_name)
		{
			DefaultVisit(_unit_name);
		}

		public virtual void visit(unit_or_namespace _unit_or_namespace)
		{
			DefaultVisit(_unit_or_namespace);
		}

		public virtual void visit(uses_unit_in _uses_unit_in)
		{
			DefaultVisit(_uses_unit_in);
		}

		public virtual void visit(uses_list _uses_list)
		{
			DefaultVisit(_uses_list);
		}

		public virtual void visit(program_body _program_body)
		{
			DefaultVisit(_program_body);
		}

		public virtual void visit(compilation_unit _compilation_unit)
		{
			DefaultVisit(_compilation_unit);
		}

		public virtual void visit(unit_module _unit_module)
		{
			DefaultVisit(_unit_module);
		}

		public virtual void visit(program_module _program_module)
		{
			DefaultVisit(_program_module);
		}

		public virtual void visit(hex_constant _hex_constant)
		{
			DefaultVisit(_hex_constant);
		}

		public virtual void visit(get_address _get_address)
		{
			DefaultVisit(_get_address);
		}

		public virtual void visit(case_variant _case_variant)
		{
			DefaultVisit(_case_variant);
		}

		public virtual void visit(case_node _case_node)
		{
			DefaultVisit(_case_node);
		}

		public virtual void visit(method_name _method_name)
		{
			DefaultVisit(_method_name);
		}

		public virtual void visit(dot_node _dot_node)
		{
			DefaultVisit(_dot_node);
		}

		public virtual void visit(empty_statement _empty_statement)
		{
			DefaultVisit(_empty_statement);
		}

		public virtual void visit(goto_statement _goto_statement)
		{
			DefaultVisit(_goto_statement);
		}

		public virtual void visit(labeled_statement _labeled_statement)
		{
			DefaultVisit(_labeled_statement);
		}

		public virtual void visit(with_statement _with_statement)
		{
			DefaultVisit(_with_statement);
		}

		public virtual void visit(method_call _method_call)
		{
			DefaultVisit(_method_call);
		}

		public virtual void visit(pascal_set_constant _pascal_set_constant)
		{
			DefaultVisit(_pascal_set_constant);
		}

		public virtual void visit(array_const _array_const)
		{
			DefaultVisit(_array_const);
		}

		public virtual void visit(write_accessor_name _write_accessor_name)
		{
			DefaultVisit(_write_accessor_name);
		}

		public virtual void visit(read_accessor_name _read_accessor_name)
		{
			DefaultVisit(_read_accessor_name);
		}

		public virtual void visit(property_accessors _property_accessors)
		{
			DefaultVisit(_property_accessors);
		}

		public virtual void visit(simple_property _simple_property)
		{
			DefaultVisit(_simple_property);
		}

		public virtual void visit(index_property _index_property)
		{
			DefaultVisit(_index_property);
		}

		public virtual void visit(class_members _class_members)
		{
			DefaultVisit(_class_members);
		}

		public virtual void visit(access_modifer_node _access_modifer_node)
		{
			DefaultVisit(_access_modifer_node);
		}

		public virtual void visit(class_body_list _class_body_list)
		{
			DefaultVisit(_class_body_list);
		}

		public virtual void visit(class_definition _class_definition)
		{
			DefaultVisit(_class_definition);
		}

		public virtual void visit(default_indexer_property_node _default_indexer_property_node)
		{
			DefaultVisit(_default_indexer_property_node);
		}

		public virtual void visit(known_type_definition _known_type_definition)
		{
			DefaultVisit(_known_type_definition);
		}

		public virtual void visit(set_type_definition _set_type_definition)
		{
			DefaultVisit(_set_type_definition);
		}

		public virtual void visit(record_const_definition _record_const_definition)
		{
			DefaultVisit(_record_const_definition);
		}

		public virtual void visit(record_const _record_const)
		{
			DefaultVisit(_record_const);
		}

		public virtual void visit(record_type _record_type)
		{
			DefaultVisit(_record_type);
		}

		public virtual void visit(enum_type_definition _enum_type_definition)
		{
			DefaultVisit(_enum_type_definition);
		}

		public virtual void visit(char_const _char_const)
		{
			DefaultVisit(_char_const);
		}

		public virtual void visit(raise_statement _raise_statement)
		{
			DefaultVisit(_raise_statement);
		}

		public virtual void visit(sharp_char_const _sharp_char_const)
		{
			DefaultVisit(_sharp_char_const);
		}

		public virtual void visit(literal_const_line _literal_const_line)
		{
			DefaultVisit(_literal_const_line);
		}

		public virtual void visit(string_num_definition _string_num_definition)
		{
			DefaultVisit(_string_num_definition);
		}

		public virtual void visit(variant _variant)
		{
			DefaultVisit(_variant);
		}

		public virtual void visit(variant_list _variant_list)
		{
			DefaultVisit(_variant_list);
		}

		public virtual void visit(variant_type _variant_type)
		{
			DefaultVisit(_variant_type);
		}

		public virtual void visit(variant_types _variant_types)
		{
			DefaultVisit(_variant_types);
		}

		public virtual void visit(variant_record_type _variant_record_type)
		{
			DefaultVisit(_variant_record_type);
		}

		public virtual void visit(procedure_call _procedure_call)
		{
			DefaultVisit(_procedure_call);
		}

		public virtual void visit(class_predefinition _class_predefinition)
		{
			DefaultVisit(_class_predefinition);
		}

		public virtual void visit(nil_const _nil_const)
		{
			DefaultVisit(_nil_const);
		}

		public virtual void visit(file_type_definition _file_type_definition)
		{
			DefaultVisit(_file_type_definition);
		}

		public virtual void visit(constructor _constructor)
		{
			DefaultVisit(_constructor);
		}

		public virtual void visit(destructor _destructor)
		{
			DefaultVisit(_destructor);
		}

		public virtual void visit(inherited_method_call _inherited_method_call)
		{
			DefaultVisit(_inherited_method_call);
		}

		public virtual void visit(typecast_node _typecast_node)
		{
			DefaultVisit(_typecast_node);
		}

		public virtual void visit(interface_node _interface_node)
		{
			DefaultVisit(_interface_node);
		}

		public virtual void visit(implementation_node _implementation_node)
		{
			DefaultVisit(_implementation_node);
		}

		public virtual void visit(diap_expr _diap_expr)
		{
			DefaultVisit(_diap_expr);
		}

		public virtual void visit(block _block)
		{
			DefaultVisit(_block);
		}

		public virtual void visit(proc_block _proc_block)
		{
			DefaultVisit(_proc_block);
		}

		public virtual void visit(array_of_named_type_definition _array_of_named_type_definition)
		{
			DefaultVisit(_array_of_named_type_definition);
		}

		public virtual void visit(array_of_const_type_definition _array_of_const_type_definition)
		{
			DefaultVisit(_array_of_const_type_definition);
		}

		public virtual void visit(literal _literal)
		{
			DefaultVisit(_literal);
		}

		public virtual void visit(case_variants _case_variants)
		{
			DefaultVisit(_case_variants);
		}

		public virtual void visit(diapason_expr _diapason_expr)
		{
			DefaultVisit(_diapason_expr);
		}

		public virtual void visit(var_def_list_for_record _var_def_list_for_record)
		{
			DefaultVisit(_var_def_list_for_record);
		}

		public virtual void visit(record_type_parts _record_type_parts)
		{
			DefaultVisit(_record_type_parts);
		}

		public virtual void visit(property_array_default _property_array_default)
		{
			DefaultVisit(_property_array_default);
		}

		public virtual void visit(property_interface _property_interface)
		{
			DefaultVisit(_property_interface);
		}

		public virtual void visit(property_parameter _property_parameter)
		{
			DefaultVisit(_property_parameter);
		}

		public virtual void visit(property_parameter_list _property_parameter_list)
		{
			DefaultVisit(_property_parameter_list);
		}

		public virtual void visit(inherited_ident _inherited_ident)
		{
			DefaultVisit(_inherited_ident);
		}

		public virtual void visit(format_expr _format_expr)
		{
			DefaultVisit(_format_expr);
		}

		public virtual void visit(initfinal_part _initfinal_part)
		{
			DefaultVisit(_initfinal_part);
		}

		public virtual void visit(token_info _token_info)
		{
			DefaultVisit(_token_info);
		}

		public virtual void visit(raise_stmt _raise_stmt)
		{
			DefaultVisit(_raise_stmt);
		}

		public virtual void visit(op_type_node _op_type_node)
		{
			DefaultVisit(_op_type_node);
		}

		public virtual void visit(file_type _file_type)
		{
			DefaultVisit(_file_type);
		}

		public virtual void visit(known_type_ident _known_type_ident)
		{
			DefaultVisit(_known_type_ident);
		}

		public virtual void visit(exception_handler _exception_handler)
		{
			DefaultVisit(_exception_handler);
		}

		public virtual void visit(exception_ident _exception_ident)
		{
			DefaultVisit(_exception_ident);
		}

		public virtual void visit(exception_handler_list _exception_handler_list)
		{
			DefaultVisit(_exception_handler_list);
		}

		public virtual void visit(exception_block _exception_block)
		{
			DefaultVisit(_exception_block);
		}

		public virtual void visit(try_handler _try_handler)
		{
			DefaultVisit(_try_handler);
		}

		public virtual void visit(try_handler_finally _try_handler_finally)
		{
			DefaultVisit(_try_handler_finally);
		}

		public virtual void visit(try_handler_except _try_handler_except)
		{
			DefaultVisit(_try_handler_except);
		}

		public virtual void visit(try_stmt _try_stmt)
		{
			DefaultVisit(_try_stmt);
		}

		public virtual void visit(inherited_message _inherited_message)
		{
			DefaultVisit(_inherited_message);
		}

		public virtual void visit(external_directive _external_directive)
		{
			DefaultVisit(_external_directive);
		}

		public virtual void visit(using_list _using_list)
		{
			DefaultVisit(_using_list);
		}

		public virtual void visit(jump_stmt _jump_stmt)
		{
			DefaultVisit(_jump_stmt);
		}

		public virtual void visit(loop_stmt _loop_stmt)
		{
			DefaultVisit(_loop_stmt);
		}

		public virtual void visit(foreach_stmt _foreach_stmt)
		{
			DefaultVisit(_foreach_stmt);
		}

		public virtual void visit(addressed_value_funcname _addressed_value_funcname)
		{
			DefaultVisit(_addressed_value_funcname);
		}

		public virtual void visit(named_type_reference_list _named_type_reference_list)
		{
			DefaultVisit(_named_type_reference_list);
		}

		public virtual void visit(template_param_list _template_param_list)
		{
			DefaultVisit(_template_param_list);
		}

		public virtual void visit(template_type_reference _template_type_reference)
		{
			DefaultVisit(_template_type_reference);
		}

		public virtual void visit(int64_const _int64_const)
		{
			DefaultVisit(_int64_const);
		}

		public virtual void visit(uint64_const _uint64_const)
		{
			DefaultVisit(_uint64_const);
		}

		public virtual void visit(new_expr _new_expr)
		{
			DefaultVisit(_new_expr);
		}

		public virtual void visit(where_type_specificator_list _where_type_specificator_list)
		{
			DefaultVisit(_where_type_specificator_list);
		}

		public virtual void visit(where_definition _where_definition)
		{
			DefaultVisit(_where_definition);
		}

		public virtual void visit(where_definition_list _where_definition_list)
		{
			DefaultVisit(_where_definition_list);
		}

		public virtual void visit(sizeof_operator _sizeof_operator)
		{
			DefaultVisit(_sizeof_operator);
		}

		public virtual void visit(typeof_operator _typeof_operator)
		{
			DefaultVisit(_typeof_operator);
		}

		public virtual void visit(compiler_directive _compiler_directive)
		{
			DefaultVisit(_compiler_directive);
		}

		public virtual void visit(operator_name_ident _operator_name_ident)
		{
			DefaultVisit(_operator_name_ident);
		}

		public virtual void visit(var_statement _var_statement)
		{
			DefaultVisit(_var_statement);
		}

		public virtual void visit(question_colon_expression _question_colon_expression)
		{
			DefaultVisit(_question_colon_expression);
		}

		public virtual void visit(expression_as_statement _expression_as_statement)
		{
			DefaultVisit(_expression_as_statement);
		}

		public virtual void visit(c_scalar_type _c_scalar_type)
		{
			DefaultVisit(_c_scalar_type);
		}

		public virtual void visit(c_module _c_module)
		{
			DefaultVisit(_c_module);
		}

		public virtual void visit(declarations_as_statement _declarations_as_statement)
		{
			DefaultVisit(_declarations_as_statement);
		}

		public virtual void visit(array_size _array_size)
		{
			DefaultVisit(_array_size);
		}

		public virtual void visit(enumerator _enumerator)
		{
			DefaultVisit(_enumerator);
		}

		public virtual void visit(enumerator_list _enumerator_list)
		{
			DefaultVisit(_enumerator_list);
		}

		public virtual void visit(c_for_cycle _c_for_cycle)
		{
			DefaultVisit(_c_for_cycle);
		}

		public virtual void visit(switch_stmt _switch_stmt)
		{
			DefaultVisit(_switch_stmt);
		}

		public virtual void visit(type_definition_attr_list _type_definition_attr_list)
		{
			DefaultVisit(_type_definition_attr_list);
		}

		public virtual void visit(type_definition_attr _type_definition_attr)
		{
			DefaultVisit(_type_definition_attr);
		}

		public virtual void visit(lock_stmt _lock_stmt)
		{
			DefaultVisit(_lock_stmt);
		}

		public virtual void visit(compiler_directive_list _compiler_directive_list)
		{
			DefaultVisit(_compiler_directive_list);
		}

		public virtual void visit(compiler_directive_if _compiler_directive_if)
		{
			DefaultVisit(_compiler_directive_if);
		}

		public virtual void visit(documentation_comment_list _documentation_comment_list)
		{
			DefaultVisit(_documentation_comment_list);
		}

		public virtual void visit(documentation_comment_tag _documentation_comment_tag)
		{
			DefaultVisit(_documentation_comment_tag);
		}

		public virtual void visit(documentation_comment_tag_param _documentation_comment_tag_param)
		{
			DefaultVisit(_documentation_comment_tag_param);
		}

		public virtual void visit(documentation_comment_section _documentation_comment_section)
		{
			DefaultVisit(_documentation_comment_section);
		}

		public virtual void visit(token_taginfo _token_taginfo)
		{
			DefaultVisit(_token_taginfo);
		}

		public virtual void visit(declaration_specificator _declaration_specificator)
		{
			DefaultVisit(_declaration_specificator);
		}

		public virtual void visit(ident_with_templateparams _ident_with_templateparams)
		{
			DefaultVisit(_ident_with_templateparams);
		}

		public virtual void visit(template_type_name _template_type_name)
		{
			DefaultVisit(_template_type_name);
		}

		public virtual void visit(default_operator _default_operator)
		{
			DefaultVisit(_default_operator);
		}

		public virtual void visit(bracket_expr _bracket_expr)
		{
			DefaultVisit(_bracket_expr);
		}

		public virtual void visit(attribute _attribute)
		{
			DefaultVisit(_attribute);
		}

		public virtual void visit(simple_attribute_list _simple_attribute_list)
		{
			DefaultVisit(_simple_attribute_list);
		}

		public virtual void visit(attribute_list _attribute_list)
		{
			DefaultVisit(_attribute_list);
		}

		public virtual void visit(function_lambda_definition _function_lambda_definition)
		{
			DefaultVisit(_function_lambda_definition);
		}

		public virtual void visit(function_lambda_call _function_lambda_call)
		{
			DefaultVisit(_function_lambda_call);
		}

		public virtual void visit(semantic_check _semantic_check)
		{
			DefaultVisit(_semantic_check);
		}

		public virtual void visit(lambda_inferred_type _lambda_inferred_type)
		{
			DefaultVisit(_lambda_inferred_type);
		}

		public virtual void visit(same_type_node _same_type_node)
		{
			DefaultVisit(_same_type_node);
		}

		public virtual void visit(name_assign_expr _name_assign_expr)
		{
			DefaultVisit(_name_assign_expr);
		}

		public virtual void visit(name_assign_expr_list _name_assign_expr_list)
		{
			DefaultVisit(_name_assign_expr_list);
		}

		public virtual void visit(unnamed_type_object _unnamed_type_object)
		{
			DefaultVisit(_unnamed_type_object);
		}

		public virtual void visit(semantic_type_node _semantic_type_node)
		{
			DefaultVisit(_semantic_type_node);
		}

		public virtual void visit(short_func_definition _short_func_definition)
		{
			DefaultVisit(_short_func_definition);
		}

		public virtual void visit(no_type_foreach _no_type_foreach)
		{
			DefaultVisit(_no_type_foreach);
		}

		public virtual void visit(matching_expression _matching_expression)
		{
			DefaultVisit(_matching_expression);
		}

		public virtual void visit(closure_substituting_node _closure_substituting_node)
		{
			DefaultVisit(_closure_substituting_node);
		}

		public virtual void visit(sequence_type _sequence_type)
		{
			DefaultVisit(_sequence_type);
		}

		public virtual void visit(modern_proc_type _modern_proc_type)
		{
			DefaultVisit(_modern_proc_type);
		}

		public virtual void visit(yield_node _yield_node)
		{
			DefaultVisit(_yield_node);
		}

		public virtual void visit(template_operator_name _template_operator_name)
		{
			DefaultVisit(_template_operator_name);
		}

		public virtual void visit(semantic_addr_value _semantic_addr_value)
		{
			DefaultVisit(_semantic_addr_value);
		}

		public virtual void visit(pair_type_stlist _pair_type_stlist)
		{
			DefaultVisit(_pair_type_stlist);
		}

		public virtual void visit(assign_tuple _assign_tuple)
		{
			DefaultVisit(_assign_tuple);
		}

		public virtual void visit(addressed_value_list _addressed_value_list)
		{
			DefaultVisit(_addressed_value_list);
		}

		public virtual void visit(tuple_node _tuple_node)
		{
			DefaultVisit(_tuple_node);
		}

		public virtual void visit(uses_closure _uses_closure)
		{
			DefaultVisit(_uses_closure);
		}

		public virtual void visit(dot_question_node _dot_question_node)
		{
			DefaultVisit(_dot_question_node);
		}

		public virtual void visit(slice_expr _slice_expr)
		{
			DefaultVisit(_slice_expr);
		}

		public virtual void visit(no_type _no_type)
		{
			DefaultVisit(_no_type);
		}

		public virtual void visit(yield_unknown_ident _yield_unknown_ident)
		{
			DefaultVisit(_yield_unknown_ident);
		}

		public virtual void visit(yield_unknown_expression_type _yield_unknown_expression_type)
		{
			DefaultVisit(_yield_unknown_expression_type);
		}

		public virtual void visit(yield_unknown_foreach_type _yield_unknown_foreach_type)
		{
			DefaultVisit(_yield_unknown_foreach_type);
		}

		public virtual void visit(yield_sequence_node _yield_sequence_node)
		{
			DefaultVisit(_yield_sequence_node);
		}

		public virtual void visit(assign_var_tuple _assign_var_tuple)
		{
			DefaultVisit(_assign_var_tuple);
		}

		public virtual void visit(slice_expr_question _slice_expr_question)
		{
			DefaultVisit(_slice_expr_question);
		}

		public virtual void visit(semantic_check_sugared_statement_node _semantic_check_sugared_statement_node)
		{
			DefaultVisit(_semantic_check_sugared_statement_node);
		}

		public virtual void visit(sugared_expression _sugared_expression)
		{
			DefaultVisit(_sugared_expression);
		}

		public virtual void visit(sugared_addressed_value _sugared_addressed_value)
		{
			DefaultVisit(_sugared_addressed_value);
		}

		public virtual void visit(double_question_node _double_question_node)
		{
			DefaultVisit(_double_question_node);
		}

		public virtual void visit(pattern_node _pattern_node)
		{
			DefaultVisit(_pattern_node);
		}

		public virtual void visit(type_pattern _type_pattern)
		{
			DefaultVisit(_type_pattern);
		}

		public virtual void visit(is_pattern_expr _is_pattern_expr)
		{
			DefaultVisit(_is_pattern_expr);
		}

		public virtual void visit(match_with _match_with)
		{
			DefaultVisit(_match_with);
		}

		public virtual void visit(pattern_case _pattern_case)
		{
			DefaultVisit(_pattern_case);
		}

		public virtual void visit(pattern_cases _pattern_cases)
		{
			DefaultVisit(_pattern_cases);
		}

		public virtual void visit(deconstructor_pattern _deconstructor_pattern)
		{
			DefaultVisit(_deconstructor_pattern);
		}

		public virtual void visit(pattern_parameter _pattern_parameter)
		{
			DefaultVisit(_pattern_parameter);
		}

		public virtual void visit(desugared_deconstruction _desugared_deconstruction)
		{
			DefaultVisit(_desugared_deconstruction);
		}

		public virtual void visit(var_deconstructor_parameter _var_deconstructor_parameter)
		{
			DefaultVisit(_var_deconstructor_parameter);
		}

		public virtual void visit(recursive_deconstructor_parameter _recursive_deconstructor_parameter)
		{
			DefaultVisit(_recursive_deconstructor_parameter);
		}

		public virtual void visit(deconstruction_variables_definition _deconstruction_variables_definition)
		{
			DefaultVisit(_deconstruction_variables_definition);
		}

		public virtual void visit(var_tuple_def_statement _var_tuple_def_statement)
		{
			DefaultVisit(_var_tuple_def_statement);
		}

		public virtual void visit(semantic_check_sugared_var_def_statement_node _semantic_check_sugared_var_def_statement_node)
		{
			DefaultVisit(_semantic_check_sugared_var_def_statement_node);
		}

		public virtual void visit(const_pattern _const_pattern)
		{
			DefaultVisit(_const_pattern);
		}

		public virtual void visit(tuple_pattern_wild_card _tuple_pattern_wild_card)
		{
			DefaultVisit(_tuple_pattern_wild_card);
		}

		public virtual void visit(const_pattern_parameter _const_pattern_parameter)
		{
			DefaultVisit(_const_pattern_parameter);
		}

		public virtual void visit(wild_card_deconstructor_parameter _wild_card_deconstructor_parameter)
		{
			DefaultVisit(_wild_card_deconstructor_parameter);
		}

		public virtual void visit(collection_pattern _collection_pattern)
		{
			DefaultVisit(_collection_pattern);
		}

		public virtual void visit(collection_pattern_gap_parameter _collection_pattern_gap_parameter)
		{
			DefaultVisit(_collection_pattern_gap_parameter);
		}

		public virtual void visit(collection_pattern_wild_card _collection_pattern_wild_card)
		{
			DefaultVisit(_collection_pattern_wild_card);
		}

		public virtual void visit(collection_pattern_var_parameter _collection_pattern_var_parameter)
		{
			DefaultVisit(_collection_pattern_var_parameter);
		}

		public virtual void visit(recursive_collection_parameter _recursive_collection_parameter)
		{
			DefaultVisit(_recursive_collection_parameter);
		}

		public virtual void visit(recursive_pattern_parameter _recursive_pattern_parameter)
		{
			DefaultVisit(_recursive_pattern_parameter);
		}

		public virtual void visit(tuple_pattern _tuple_pattern)
		{
			DefaultVisit(_tuple_pattern);
		}

		public virtual void visit(tuple_pattern_var_parameter _tuple_pattern_var_parameter)
		{
			DefaultVisit(_tuple_pattern_var_parameter);
		}

		public virtual void visit(recursive_tuple_parameter _recursive_tuple_parameter)
		{
			DefaultVisit(_recursive_tuple_parameter);
		}

		public virtual void visit(diapason_expr_new _diapason_expr_new)
		{
			DefaultVisit(_diapason_expr_new);
		}

		public virtual void visit(if_expr_new _if_expr_new)
		{
			DefaultVisit(_if_expr_new);
		}

		public virtual void visit(simple_expr_with_deref _simple_expr_with_deref)
		{
			DefaultVisit(_simple_expr_with_deref);
		}

		public virtual void visit(index _index)
		{
			DefaultVisit(_index);
		}

		public virtual void visit(array_const_new _array_const_new)
		{
			DefaultVisit(_array_const_new);
		}

		public virtual void visit(semantic_ith_element_of _semantic_ith_element_of)
		{
			DefaultVisit(_semantic_ith_element_of);
		}

		public virtual void visit(bigint_const _bigint_const)
		{
			DefaultVisit(_bigint_const);
		}

		public virtual void visit(foreach_stmt_formatting _foreach_stmt_formatting)
		{
			DefaultVisit(_foreach_stmt_formatting);
		}

		public virtual void visit(property_ident _property_ident)
		{
			DefaultVisit(_property_ident);
		}

		public virtual void visit(expression_with_let _expression_with_let)
		{
			DefaultVisit(_expression_with_let);
		}

		public virtual void visit(lambda_any_type_node_syntax _lambda_any_type_node_syntax)
		{
			DefaultVisit(_lambda_any_type_node_syntax);
		}

		public virtual void visit(ref_var_def_statement _ref_var_def_statement)
		{
			DefaultVisit(_ref_var_def_statement);
		}

		public virtual void visit(let_var_expr _let_var_expr)
		{
			DefaultVisit(_let_var_expr);
		}

		public virtual void visit(to_expr _to_expr)
		{
			DefaultVisit(_to_expr);
		}

		public virtual void visit(global_statement _global_statement)
		{
			DefaultVisit(_global_statement);
		}

		public virtual void visit(list_generator _list_generator)
		{
			DefaultVisit(_list_generator);
		}
	}


}

