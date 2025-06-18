
namespace PascalABCCompiler.SyntaxTree
{

	public interface IVisitor
	{
		///<summary>
		///Method to visit expression.
		///</summary>
		///<param name="_expression">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(expression _expression);
		///<summary>
		///Method to visit syntax_tree_node.
		///</summary>
		///<param name="_syntax_tree_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(syntax_tree_node _syntax_tree_node);
		///<summary>
		///Method to visit statement.
		///</summary>
		///<param name="_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(statement _statement);
		///<summary>
		///Method to visit statement_list.
		///</summary>
		///<param name="_statement_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(statement_list _statement_list);
		///<summary>
		///Method to visit ident.
		///</summary>
		///<param name="_ident">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(ident _ident);
		///<summary>
		///Method to visit assign.
		///</summary>
		///<param name="_assign">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(assign _assign);
		///<summary>
		///Method to visit bin_expr.
		///</summary>
		///<param name="_bin_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(bin_expr _bin_expr);
		///<summary>
		///Method to visit un_expr.
		///</summary>
		///<param name="_un_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(un_expr _un_expr);
		///<summary>
		///Method to visit const_node.
		///</summary>
		///<param name="_const_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(const_node _const_node);
		///<summary>
		///Method to visit bool_const.
		///</summary>
		///<param name="_bool_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(bool_const _bool_const);
		///<summary>
		///Method to visit int32_const.
		///</summary>
		///<param name="_int32_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(int32_const _int32_const);
		///<summary>
		///Method to visit double_const.
		///</summary>
		///<param name="_double_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(double_const _double_const);
		///<summary>
		///Method to visit subprogram_body.
		///</summary>
		///<param name="_subprogram_body">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(subprogram_body _subprogram_body);
		///<summary>
		///Method to visit addressed_value.
		///</summary>
		///<param name="_addressed_value">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(addressed_value _addressed_value);
		///<summary>
		///Method to visit type_definition.
		///</summary>
		///<param name="_type_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(type_definition _type_definition);
		///<summary>
		///Method to visit roof_dereference.
		///</summary>
		///<param name="_roof_dereference">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(roof_dereference _roof_dereference);
		///<summary>
		///Method to visit named_type_reference.
		///</summary>
		///<param name="_named_type_reference">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(named_type_reference _named_type_reference);
		///<summary>
		///Method to visit variable_definitions.
		///</summary>
		///<param name="_variable_definitions">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(variable_definitions _variable_definitions);
		///<summary>
		///Method to visit ident_list.
		///</summary>
		///<param name="_ident_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(ident_list _ident_list);
		///<summary>
		///Method to visit var_def_statement.
		///</summary>
		///<param name="_var_def_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(var_def_statement _var_def_statement);
		///<summary>
		///Method to visit declaration.
		///</summary>
		///<param name="_declaration">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(declaration _declaration);
		///<summary>
		///Method to visit declarations.
		///</summary>
		///<param name="_declarations">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(declarations _declarations);
		///<summary>
		///Method to visit program_tree.
		///</summary>
		///<param name="_program_tree">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(program_tree _program_tree);
		///<summary>
		///Method to visit program_name.
		///</summary>
		///<param name="_program_name">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(program_name _program_name);
		///<summary>
		///Method to visit string_const.
		///</summary>
		///<param name="_string_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(string_const _string_const);
		///<summary>
		///Method to visit expression_list.
		///</summary>
		///<param name="_expression_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(expression_list _expression_list);
		///<summary>
		///Method to visit dereference.
		///</summary>
		///<param name="_dereference">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(dereference _dereference);
		///<summary>
		///Method to visit indexer.
		///</summary>
		///<param name="_indexer">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(indexer _indexer);
		///<summary>
		///Method to visit for_node.
		///</summary>
		///<param name="_for_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(for_node _for_node);
		///<summary>
		///Method to visit repeat_node.
		///</summary>
		///<param name="_repeat_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(repeat_node _repeat_node);
		///<summary>
		///Method to visit while_node.
		///</summary>
		///<param name="_while_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(while_node _while_node);
		///<summary>
		///Method to visit if_node.
		///</summary>
		///<param name="_if_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(if_node _if_node);
		///<summary>
		///Method to visit ref_type.
		///</summary>
		///<param name="_ref_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(ref_type _ref_type);
		///<summary>
		///Method to visit diapason.
		///</summary>
		///<param name="_diapason">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(diapason _diapason);
		///<summary>
		///Method to visit indexers_types.
		///</summary>
		///<param name="_indexers_types">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(indexers_types _indexers_types);
		///<summary>
		///Method to visit array_type.
		///</summary>
		///<param name="_array_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(array_type _array_type);
		///<summary>
		///Method to visit label_definitions.
		///</summary>
		///<param name="_label_definitions">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(label_definitions _label_definitions);
		///<summary>
		///Method to visit procedure_attribute.
		///</summary>
		///<param name="_procedure_attribute">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(procedure_attribute _procedure_attribute);
		///<summary>
		///Method to visit typed_parameters.
		///</summary>
		///<param name="_typed_parameters">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(typed_parameters _typed_parameters);
		///<summary>
		///Method to visit formal_parameters.
		///</summary>
		///<param name="_formal_parameters">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(formal_parameters _formal_parameters);
		///<summary>
		///Method to visit procedure_attributes_list.
		///</summary>
		///<param name="_procedure_attributes_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(procedure_attributes_list _procedure_attributes_list);
		///<summary>
		///Method to visit procedure_header.
		///</summary>
		///<param name="_procedure_header">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(procedure_header _procedure_header);
		///<summary>
		///Method to visit function_header.
		///</summary>
		///<param name="_function_header">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(function_header _function_header);
		///<summary>
		///Method to visit procedure_definition.
		///</summary>
		///<param name="_procedure_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(procedure_definition _procedure_definition);
		///<summary>
		///Method to visit type_declaration.
		///</summary>
		///<param name="_type_declaration">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(type_declaration _type_declaration);
		///<summary>
		///Method to visit type_declarations.
		///</summary>
		///<param name="_type_declarations">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(type_declarations _type_declarations);
		///<summary>
		///Method to visit simple_const_definition.
		///</summary>
		///<param name="_simple_const_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(simple_const_definition _simple_const_definition);
		///<summary>
		///Method to visit typed_const_definition.
		///</summary>
		///<param name="_typed_const_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(typed_const_definition _typed_const_definition);
		///<summary>
		///Method to visit const_definition.
		///</summary>
		///<param name="_const_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(const_definition _const_definition);
		///<summary>
		///Method to visit consts_definitions_list.
		///</summary>
		///<param name="_consts_definitions_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(consts_definitions_list _consts_definitions_list);
		///<summary>
		///Method to visit unit_name.
		///</summary>
		///<param name="_unit_name">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(unit_name _unit_name);
		///<summary>
		///Method to visit unit_or_namespace.
		///</summary>
		///<param name="_unit_or_namespace">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(unit_or_namespace _unit_or_namespace);
		///<summary>
		///Method to visit uses_unit_in.
		///</summary>
		///<param name="_uses_unit_in">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(uses_unit_in _uses_unit_in);
		///<summary>
		///Method to visit uses_list.
		///</summary>
		///<param name="_uses_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(uses_list _uses_list);
		///<summary>
		///Method to visit program_body.
		///</summary>
		///<param name="_program_body">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(program_body _program_body);
		///<summary>
		///Method to visit compilation_unit.
		///</summary>
		///<param name="_compilation_unit">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(compilation_unit _compilation_unit);
		///<summary>
		///Method to visit unit_module.
		///</summary>
		///<param name="_unit_module">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(unit_module _unit_module);
		///<summary>
		///Method to visit program_module.
		///</summary>
		///<param name="_program_module">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(program_module _program_module);
		///<summary>
		///Method to visit hex_constant.
		///</summary>
		///<param name="_hex_constant">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(hex_constant _hex_constant);
		///<summary>
		///Method to visit get_address.
		///</summary>
		///<param name="_get_address">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(get_address _get_address);
		///<summary>
		///Method to visit case_variant.
		///</summary>
		///<param name="_case_variant">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(case_variant _case_variant);
		///<summary>
		///Method to visit case_node.
		///</summary>
		///<param name="_case_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(case_node _case_node);
		///<summary>
		///Method to visit method_name.
		///</summary>
		///<param name="_method_name">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(method_name _method_name);
		///<summary>
		///Method to visit dot_node.
		///</summary>
		///<param name="_dot_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(dot_node _dot_node);
		///<summary>
		///Method to visit empty_statement.
		///</summary>
		///<param name="_empty_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(empty_statement _empty_statement);
		///<summary>
		///Method to visit goto_statement.
		///</summary>
		///<param name="_goto_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(goto_statement _goto_statement);
		///<summary>
		///Method to visit labeled_statement.
		///</summary>
		///<param name="_labeled_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(labeled_statement _labeled_statement);
		///<summary>
		///Method to visit with_statement.
		///</summary>
		///<param name="_with_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(with_statement _with_statement);
		///<summary>
		///Method to visit method_call.
		///</summary>
		///<param name="_method_call">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(method_call _method_call);
		///<summary>
		///Method to visit pascal_set_constant.
		///</summary>
		///<param name="_pascal_set_constant">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(pascal_set_constant _pascal_set_constant);
		///<summary>
		///Method to visit array_const.
		///</summary>
		///<param name="_array_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(array_const _array_const);
		///<summary>
		///Method to visit write_accessor_name.
		///</summary>
		///<param name="_write_accessor_name">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(write_accessor_name _write_accessor_name);
		///<summary>
		///Method to visit read_accessor_name.
		///</summary>
		///<param name="_read_accessor_name">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(read_accessor_name _read_accessor_name);
		///<summary>
		///Method to visit property_accessors.
		///</summary>
		///<param name="_property_accessors">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(property_accessors _property_accessors);
		///<summary>
		///Method to visit simple_property.
		///</summary>
		///<param name="_simple_property">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(simple_property _simple_property);
		///<summary>
		///Method to visit index_property.
		///</summary>
		///<param name="_index_property">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(index_property _index_property);
		///<summary>
		///Method to visit class_members.
		///</summary>
		///<param name="_class_members">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(class_members _class_members);
		///<summary>
		///Method to visit access_modifer_node.
		///</summary>
		///<param name="_access_modifer_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(access_modifer_node _access_modifer_node);
		///<summary>
		///Method to visit class_body_list.
		///</summary>
		///<param name="_class_body_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(class_body_list _class_body_list);
		///<summary>
		///Method to visit class_definition.
		///</summary>
		///<param name="_class_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(class_definition _class_definition);
		///<summary>
		///Method to visit default_indexer_property_node.
		///</summary>
		///<param name="_default_indexer_property_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(default_indexer_property_node _default_indexer_property_node);
		///<summary>
		///Method to visit known_type_definition.
		///</summary>
		///<param name="_known_type_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(known_type_definition _known_type_definition);
		///<summary>
		///Method to visit set_type_definition.
		///</summary>
		///<param name="_set_type_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(set_type_definition _set_type_definition);
		///<summary>
		///Method to visit record_const_definition.
		///</summary>
		///<param name="_record_const_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(record_const_definition _record_const_definition);
		///<summary>
		///Method to visit record_const.
		///</summary>
		///<param name="_record_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(record_const _record_const);
		///<summary>
		///Method to visit record_type.
		///</summary>
		///<param name="_record_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(record_type _record_type);
		///<summary>
		///Method to visit enum_type_definition.
		///</summary>
		///<param name="_enum_type_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(enum_type_definition _enum_type_definition);
		///<summary>
		///Method to visit char_const.
		///</summary>
		///<param name="_char_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(char_const _char_const);
		///<summary>
		///Method to visit raise_statement.
		///</summary>
		///<param name="_raise_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(raise_statement _raise_statement);
		///<summary>
		///Method to visit sharp_char_const.
		///</summary>
		///<param name="_sharp_char_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(sharp_char_const _sharp_char_const);
		///<summary>
		///Method to visit literal_const_line.
		///</summary>
		///<param name="_literal_const_line">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(literal_const_line _literal_const_line);
		///<summary>
		///Method to visit string_num_definition.
		///</summary>
		///<param name="_string_num_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(string_num_definition _string_num_definition);
		///<summary>
		///Method to visit variant.
		///</summary>
		///<param name="_variant">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(variant _variant);
		///<summary>
		///Method to visit variant_list.
		///</summary>
		///<param name="_variant_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(variant_list _variant_list);
		///<summary>
		///Method to visit variant_type.
		///</summary>
		///<param name="_variant_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(variant_type _variant_type);
		///<summary>
		///Method to visit variant_types.
		///</summary>
		///<param name="_variant_types">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(variant_types _variant_types);
		///<summary>
		///Method to visit variant_record_type.
		///</summary>
		///<param name="_variant_record_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(variant_record_type _variant_record_type);
		///<summary>
		///Method to visit procedure_call.
		///</summary>
		///<param name="_procedure_call">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(procedure_call _procedure_call);
		///<summary>
		///Method to visit class_predefinition.
		///</summary>
		///<param name="_class_predefinition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(class_predefinition _class_predefinition);
		///<summary>
		///Method to visit nil_const.
		///</summary>
		///<param name="_nil_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(nil_const _nil_const);
		///<summary>
		///Method to visit file_type_definition.
		///</summary>
		///<param name="_file_type_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(file_type_definition _file_type_definition);
		///<summary>
		///Method to visit constructor.
		///</summary>
		///<param name="_constructor">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(constructor _constructor);
		///<summary>
		///Method to visit destructor.
		///</summary>
		///<param name="_destructor">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(destructor _destructor);
		///<summary>
		///Method to visit inherited_method_call.
		///</summary>
		///<param name="_inherited_method_call">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(inherited_method_call _inherited_method_call);
		///<summary>
		///Method to visit typecast_node.
		///</summary>
		///<param name="_typecast_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(typecast_node _typecast_node);
		///<summary>
		///Method to visit interface_node.
		///</summary>
		///<param name="_interface_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(interface_node _interface_node);
		///<summary>
		///Method to visit implementation_node.
		///</summary>
		///<param name="_implementation_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(implementation_node _implementation_node);
		///<summary>
		///Method to visit diap_expr.
		///</summary>
		///<param name="_diap_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(diap_expr _diap_expr);
		///<summary>
		///Method to visit block.
		///</summary>
		///<param name="_block">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(block _block);
		///<summary>
		///Method to visit proc_block.
		///</summary>
		///<param name="_proc_block">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(proc_block _proc_block);
		///<summary>
		///Method to visit array_of_named_type_definition.
		///</summary>
		///<param name="_array_of_named_type_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(array_of_named_type_definition _array_of_named_type_definition);
		///<summary>
		///Method to visit array_of_const_type_definition.
		///</summary>
		///<param name="_array_of_const_type_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(array_of_const_type_definition _array_of_const_type_definition);
		///<summary>
		///Method to visit literal.
		///</summary>
		///<param name="_literal">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(literal _literal);
		///<summary>
		///Method to visit case_variants.
		///</summary>
		///<param name="_case_variants">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(case_variants _case_variants);
		///<summary>
		///Method to visit diapason_expr.
		///</summary>
		///<param name="_diapason_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(diapason_expr _diapason_expr);
		///<summary>
		///Method to visit var_def_list_for_record.
		///</summary>
		///<param name="_var_def_list_for_record">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(var_def_list_for_record _var_def_list_for_record);
		///<summary>
		///Method to visit record_type_parts.
		///</summary>
		///<param name="_record_type_parts">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(record_type_parts _record_type_parts);
		///<summary>
		///Method to visit property_array_default.
		///</summary>
		///<param name="_property_array_default">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(property_array_default _property_array_default);
		///<summary>
		///Method to visit property_interface.
		///</summary>
		///<param name="_property_interface">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(property_interface _property_interface);
		///<summary>
		///Method to visit property_parameter.
		///</summary>
		///<param name="_property_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(property_parameter _property_parameter);
		///<summary>
		///Method to visit property_parameter_list.
		///</summary>
		///<param name="_property_parameter_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(property_parameter_list _property_parameter_list);
		///<summary>
		///Method to visit inherited_ident.
		///</summary>
		///<param name="_inherited_ident">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(inherited_ident _inherited_ident);
		///<summary>
		///Method to visit format_expr.
		///</summary>
		///<param name="_format_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(format_expr _format_expr);
		///<summary>
		///Method to visit initfinal_part.
		///</summary>
		///<param name="_initfinal_part">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(initfinal_part _initfinal_part);
		///<summary>
		///Method to visit token_info.
		///</summary>
		///<param name="_token_info">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(token_info _token_info);
		///<summary>
		///Method to visit raise_stmt.
		///</summary>
		///<param name="_raise_stmt">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(raise_stmt _raise_stmt);
		///<summary>
		///Method to visit op_type_node.
		///</summary>
		///<param name="_op_type_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(op_type_node _op_type_node);
		///<summary>
		///Method to visit file_type.
		///</summary>
		///<param name="_file_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(file_type _file_type);
		///<summary>
		///Method to visit known_type_ident.
		///</summary>
		///<param name="_known_type_ident">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(known_type_ident _known_type_ident);
		///<summary>
		///Method to visit exception_handler.
		///</summary>
		///<param name="_exception_handler">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(exception_handler _exception_handler);
		///<summary>
		///Method to visit exception_ident.
		///</summary>
		///<param name="_exception_ident">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(exception_ident _exception_ident);
		///<summary>
		///Method to visit exception_handler_list.
		///</summary>
		///<param name="_exception_handler_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(exception_handler_list _exception_handler_list);
		///<summary>
		///Method to visit exception_block.
		///</summary>
		///<param name="_exception_block">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(exception_block _exception_block);
		///<summary>
		///Method to visit try_handler.
		///</summary>
		///<param name="_try_handler">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(try_handler _try_handler);
		///<summary>
		///Method to visit try_handler_finally.
		///</summary>
		///<param name="_try_handler_finally">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(try_handler_finally _try_handler_finally);
		///<summary>
		///Method to visit try_handler_except.
		///</summary>
		///<param name="_try_handler_except">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(try_handler_except _try_handler_except);
		///<summary>
		///Method to visit try_stmt.
		///</summary>
		///<param name="_try_stmt">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(try_stmt _try_stmt);
		///<summary>
		///Method to visit inherited_message.
		///</summary>
		///<param name="_inherited_message">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(inherited_message _inherited_message);
		///<summary>
		///Method to visit external_directive.
		///</summary>
		///<param name="_external_directive">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(external_directive _external_directive);
		///<summary>
		///Method to visit using_list.
		///</summary>
		///<param name="_using_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(using_list _using_list);
		///<summary>
		///Method to visit jump_stmt.
		///</summary>
		///<param name="_jump_stmt">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(jump_stmt _jump_stmt);
		///<summary>
		///Method to visit loop_stmt.
		///</summary>
		///<param name="_loop_stmt">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(loop_stmt _loop_stmt);
		///<summary>
		///Method to visit foreach_stmt.
		///</summary>
		///<param name="_foreach_stmt">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(foreach_stmt _foreach_stmt);
		///<summary>
		///Method to visit addressed_value_funcname.
		///</summary>
		///<param name="_addressed_value_funcname">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(addressed_value_funcname _addressed_value_funcname);
		///<summary>
		///Method to visit named_type_reference_list.
		///</summary>
		///<param name="_named_type_reference_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(named_type_reference_list _named_type_reference_list);
		///<summary>
		///Method to visit template_param_list.
		///</summary>
		///<param name="_template_param_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(template_param_list _template_param_list);
		///<summary>
		///Method to visit template_type_reference.
		///</summary>
		///<param name="_template_type_reference">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(template_type_reference _template_type_reference);
		///<summary>
		///Method to visit int64_const.
		///</summary>
		///<param name="_int64_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(int64_const _int64_const);
		///<summary>
		///Method to visit uint64_const.
		///</summary>
		///<param name="_uint64_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(uint64_const _uint64_const);
		///<summary>
		///Method to visit new_expr.
		///</summary>
		///<param name="_new_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(new_expr _new_expr);
		///<summary>
		///Method to visit where_type_specificator_list.
		///</summary>
		///<param name="_where_type_specificator_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(where_type_specificator_list _where_type_specificator_list);
		///<summary>
		///Method to visit where_definition.
		///</summary>
		///<param name="_where_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(where_definition _where_definition);
		///<summary>
		///Method to visit where_definition_list.
		///</summary>
		///<param name="_where_definition_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(where_definition_list _where_definition_list);
		///<summary>
		///Method to visit sizeof_operator.
		///</summary>
		///<param name="_sizeof_operator">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(sizeof_operator _sizeof_operator);
		///<summary>
		///Method to visit typeof_operator.
		///</summary>
		///<param name="_typeof_operator">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(typeof_operator _typeof_operator);
		///<summary>
		///Method to visit compiler_directive.
		///</summary>
		///<param name="_compiler_directive">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(compiler_directive _compiler_directive);
		///<summary>
		///Method to visit operator_name_ident.
		///</summary>
		///<param name="_operator_name_ident">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(operator_name_ident _operator_name_ident);
		///<summary>
		///Method to visit var_statement.
		///</summary>
		///<param name="_var_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(var_statement _var_statement);
		///<summary>
		///Method to visit question_colon_expression.
		///</summary>
		///<param name="_question_colon_expression">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(question_colon_expression _question_colon_expression);
		///<summary>
		///Method to visit expression_as_statement.
		///</summary>
		///<param name="_expression_as_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(expression_as_statement _expression_as_statement);
		///<summary>
		///Method to visit c_scalar_type.
		///</summary>
		///<param name="_c_scalar_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(c_scalar_type _c_scalar_type);
		///<summary>
		///Method to visit c_module.
		///</summary>
		///<param name="_c_module">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(c_module _c_module);
		///<summary>
		///Method to visit declarations_as_statement.
		///</summary>
		///<param name="_declarations_as_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(declarations_as_statement _declarations_as_statement);
		///<summary>
		///Method to visit array_size.
		///</summary>
		///<param name="_array_size">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(array_size _array_size);
		///<summary>
		///Method to visit enumerator.
		///</summary>
		///<param name="_enumerator">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(enumerator _enumerator);
		///<summary>
		///Method to visit enumerator_list.
		///</summary>
		///<param name="_enumerator_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(enumerator_list _enumerator_list);
		///<summary>
		///Method to visit c_for_cycle.
		///</summary>
		///<param name="_c_for_cycle">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(c_for_cycle _c_for_cycle);
		///<summary>
		///Method to visit switch_stmt.
		///</summary>
		///<param name="_switch_stmt">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(switch_stmt _switch_stmt);
		///<summary>
		///Method to visit type_definition_attr_list.
		///</summary>
		///<param name="_type_definition_attr_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(type_definition_attr_list _type_definition_attr_list);
		///<summary>
		///Method to visit type_definition_attr.
		///</summary>
		///<param name="_type_definition_attr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(type_definition_attr _type_definition_attr);
		///<summary>
		///Method to visit lock_stmt.
		///</summary>
		///<param name="_lock_stmt">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(lock_stmt _lock_stmt);
		///<summary>
		///Method to visit compiler_directive_list.
		///</summary>
		///<param name="_compiler_directive_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(compiler_directive_list _compiler_directive_list);
		///<summary>
		///Method to visit compiler_directive_if.
		///</summary>
		///<param name="_compiler_directive_if">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(compiler_directive_if _compiler_directive_if);
		///<summary>
		///Method to visit documentation_comment_list.
		///</summary>
		///<param name="_documentation_comment_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(documentation_comment_list _documentation_comment_list);
		///<summary>
		///Method to visit documentation_comment_tag.
		///</summary>
		///<param name="_documentation_comment_tag">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(documentation_comment_tag _documentation_comment_tag);
		///<summary>
		///Method to visit documentation_comment_tag_param.
		///</summary>
		///<param name="_documentation_comment_tag_param">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(documentation_comment_tag_param _documentation_comment_tag_param);
		///<summary>
		///Method to visit documentation_comment_section.
		///</summary>
		///<param name="_documentation_comment_section">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(documentation_comment_section _documentation_comment_section);
		///<summary>
		///Method to visit token_taginfo.
		///</summary>
		///<param name="_token_taginfo">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(token_taginfo _token_taginfo);
		///<summary>
		///Method to visit declaration_specificator.
		///</summary>
		///<param name="_declaration_specificator">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(declaration_specificator _declaration_specificator);
		///<summary>
		///Method to visit ident_with_templateparams.
		///</summary>
		///<param name="_ident_with_templateparams">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(ident_with_templateparams _ident_with_templateparams);
		///<summary>
		///Method to visit template_type_name.
		///</summary>
		///<param name="_template_type_name">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(template_type_name _template_type_name);
		///<summary>
		///Method to visit default_operator.
		///</summary>
		///<param name="_default_operator">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(default_operator _default_operator);
		///<summary>
		///Method to visit bracket_expr.
		///</summary>
		///<param name="_bracket_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(bracket_expr _bracket_expr);
		///<summary>
		///Method to visit attribute.
		///</summary>
		///<param name="_attribute">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(attribute _attribute);
		///<summary>
		///Method to visit simple_attribute_list.
		///</summary>
		///<param name="_simple_attribute_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(simple_attribute_list _simple_attribute_list);
		///<summary>
		///Method to visit attribute_list.
		///</summary>
		///<param name="_attribute_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(attribute_list _attribute_list);
		///<summary>
		///Method to visit function_lambda_definition.
		///</summary>
		///<param name="_function_lambda_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(function_lambda_definition _function_lambda_definition);
		///<summary>
		///Method to visit function_lambda_call.
		///</summary>
		///<param name="_function_lambda_call">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(function_lambda_call _function_lambda_call);
		///<summary>
		///Method to visit semantic_check.
		///</summary>
		///<param name="_semantic_check">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(semantic_check _semantic_check);
		///<summary>
		///Method to visit lambda_inferred_type.
		///</summary>
		///<param name="_lambda_inferred_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(lambda_inferred_type _lambda_inferred_type);
		///<summary>
		///Method to visit same_type_node.
		///</summary>
		///<param name="_same_type_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(same_type_node _same_type_node);
		///<summary>
		///Method to visit name_assign_expr.
		///</summary>
		///<param name="_name_assign_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(name_assign_expr _name_assign_expr);
		///<summary>
		///Method to visit name_assign_expr_list.
		///</summary>
		///<param name="_name_assign_expr_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(name_assign_expr_list _name_assign_expr_list);
		///<summary>
		///Method to visit unnamed_type_object.
		///</summary>
		///<param name="_unnamed_type_object">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(unnamed_type_object _unnamed_type_object);
		///<summary>
		///Method to visit semantic_type_node.
		///</summary>
		///<param name="_semantic_type_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(semantic_type_node _semantic_type_node);
		///<summary>
		///Method to visit short_func_definition.
		///</summary>
		///<param name="_short_func_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(short_func_definition _short_func_definition);
		///<summary>
		///Method to visit no_type_foreach.
		///</summary>
		///<param name="_no_type_foreach">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(no_type_foreach _no_type_foreach);
		///<summary>
		///Method to visit matching_expression.
		///</summary>
		///<param name="_matching_expression">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(matching_expression _matching_expression);
		///<summary>
		///Method to visit closure_substituting_node.
		///</summary>
		///<param name="_closure_substituting_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(closure_substituting_node _closure_substituting_node);
		///<summary>
		///Method to visit sequence_type.
		///</summary>
		///<param name="_sequence_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(sequence_type _sequence_type);
		///<summary>
		///Method to visit modern_proc_type.
		///</summary>
		///<param name="_modern_proc_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(modern_proc_type _modern_proc_type);
		///<summary>
		///Method to visit yield_node.
		///</summary>
		///<param name="_yield_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(yield_node _yield_node);
		///<summary>
		///Method to visit template_operator_name.
		///</summary>
		///<param name="_template_operator_name">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(template_operator_name _template_operator_name);
		///<summary>
		///Method to visit semantic_addr_value.
		///</summary>
		///<param name="_semantic_addr_value">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(semantic_addr_value _semantic_addr_value);
		///<summary>
		///Method to visit pair_type_stlist.
		///</summary>
		///<param name="_pair_type_stlist">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(pair_type_stlist _pair_type_stlist);
		///<summary>
		///Method to visit assign_tuple.
		///</summary>
		///<param name="_assign_tuple">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(assign_tuple _assign_tuple);
		///<summary>
		///Method to visit addressed_value_list.
		///</summary>
		///<param name="_addressed_value_list">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(addressed_value_list _addressed_value_list);
		///<summary>
		///Method to visit tuple_node.
		///</summary>
		///<param name="_tuple_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(tuple_node _tuple_node);
		///<summary>
		///Method to visit uses_closure.
		///</summary>
		///<param name="_uses_closure">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(uses_closure _uses_closure);
		///<summary>
		///Method to visit dot_question_node.
		///</summary>
		///<param name="_dot_question_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(dot_question_node _dot_question_node);
		///<summary>
		///Method to visit slice_expr.
		///</summary>
		///<param name="_slice_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(slice_expr _slice_expr);
		///<summary>
		///Method to visit no_type.
		///</summary>
		///<param name="_no_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(no_type _no_type);
		///<summary>
		///Method to visit yield_unknown_ident.
		///</summary>
		///<param name="_yield_unknown_ident">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(yield_unknown_ident _yield_unknown_ident);
		///<summary>
		///Method to visit yield_unknown_expression_type.
		///</summary>
		///<param name="_yield_unknown_expression_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(yield_unknown_expression_type _yield_unknown_expression_type);
		///<summary>
		///Method to visit yield_unknown_foreach_type.
		///</summary>
		///<param name="_yield_unknown_foreach_type">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(yield_unknown_foreach_type _yield_unknown_foreach_type);
		///<summary>
		///Method to visit yield_sequence_node.
		///</summary>
		///<param name="_yield_sequence_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(yield_sequence_node _yield_sequence_node);
		///<summary>
		///Method to visit assign_var_tuple.
		///</summary>
		///<param name="_assign_var_tuple">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(assign_var_tuple _assign_var_tuple);
		///<summary>
		///Method to visit slice_expr_question.
		///</summary>
		///<param name="_slice_expr_question">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(slice_expr_question _slice_expr_question);
		///<summary>
		///Method to visit semantic_check_sugared_statement_node.
		///</summary>
		///<param name="_semantic_check_sugared_statement_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(semantic_check_sugared_statement_node _semantic_check_sugared_statement_node);
		///<summary>
		///Method to visit sugared_expression.
		///</summary>
		///<param name="_sugared_expression">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(sugared_expression _sugared_expression);
		///<summary>
		///Method to visit sugared_addressed_value.
		///</summary>
		///<param name="_sugared_addressed_value">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(sugared_addressed_value _sugared_addressed_value);
		///<summary>
		///Method to visit double_question_node.
		///</summary>
		///<param name="_double_question_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(double_question_node _double_question_node);
		///<summary>
		///Method to visit pattern_node.
		///</summary>
		///<param name="_pattern_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(pattern_node _pattern_node);
		///<summary>
		///Method to visit type_pattern.
		///</summary>
		///<param name="_type_pattern">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(type_pattern _type_pattern);
		///<summary>
		///Method to visit is_pattern_expr.
		///</summary>
		///<param name="_is_pattern_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(is_pattern_expr _is_pattern_expr);
		///<summary>
		///Method to visit match_with.
		///</summary>
		///<param name="_match_with">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(match_with _match_with);
		///<summary>
		///Method to visit pattern_case.
		///</summary>
		///<param name="_pattern_case">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(pattern_case _pattern_case);
		///<summary>
		///Method to visit pattern_cases.
		///</summary>
		///<param name="_pattern_cases">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(pattern_cases _pattern_cases);
		///<summary>
		///Method to visit deconstructor_pattern.
		///</summary>
		///<param name="_deconstructor_pattern">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(deconstructor_pattern _deconstructor_pattern);
		///<summary>
		///Method to visit pattern_parameter.
		///</summary>
		///<param name="_pattern_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(pattern_parameter _pattern_parameter);
		///<summary>
		///Method to visit desugared_deconstruction.
		///</summary>
		///<param name="_desugared_deconstruction">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(desugared_deconstruction _desugared_deconstruction);
		///<summary>
		///Method to visit var_deconstructor_parameter.
		///</summary>
		///<param name="_var_deconstructor_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(var_deconstructor_parameter _var_deconstructor_parameter);
		///<summary>
		///Method to visit recursive_deconstructor_parameter.
		///</summary>
		///<param name="_recursive_deconstructor_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(recursive_deconstructor_parameter _recursive_deconstructor_parameter);
		///<summary>
		///Method to visit deconstruction_variables_definition.
		///</summary>
		///<param name="_deconstruction_variables_definition">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(deconstruction_variables_definition _deconstruction_variables_definition);
		///<summary>
		///Method to visit var_tuple_def_statement.
		///</summary>
		///<param name="_var_tuple_def_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(var_tuple_def_statement _var_tuple_def_statement);
		///<summary>
		///Method to visit semantic_check_sugared_var_def_statement_node.
		///</summary>
		///<param name="_semantic_check_sugared_var_def_statement_node">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(semantic_check_sugared_var_def_statement_node _semantic_check_sugared_var_def_statement_node);
		///<summary>
		///Method to visit const_pattern.
		///</summary>
		///<param name="_const_pattern">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(const_pattern _const_pattern);
		///<summary>
		///Method to visit tuple_pattern_wild_card.
		///</summary>
		///<param name="_tuple_pattern_wild_card">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(tuple_pattern_wild_card _tuple_pattern_wild_card);
		///<summary>
		///Method to visit const_pattern_parameter.
		///</summary>
		///<param name="_const_pattern_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(const_pattern_parameter _const_pattern_parameter);
		///<summary>
		///Method to visit wild_card_deconstructor_parameter.
		///</summary>
		///<param name="_wild_card_deconstructor_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(wild_card_deconstructor_parameter _wild_card_deconstructor_parameter);
		///<summary>
		///Method to visit collection_pattern.
		///</summary>
		///<param name="_collection_pattern">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(collection_pattern _collection_pattern);
		///<summary>
		///Method to visit collection_pattern_gap_parameter.
		///</summary>
		///<param name="_collection_pattern_gap_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(collection_pattern_gap_parameter _collection_pattern_gap_parameter);
		///<summary>
		///Method to visit collection_pattern_wild_card.
		///</summary>
		///<param name="_collection_pattern_wild_card">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(collection_pattern_wild_card _collection_pattern_wild_card);
		///<summary>
		///Method to visit collection_pattern_var_parameter.
		///</summary>
		///<param name="_collection_pattern_var_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(collection_pattern_var_parameter _collection_pattern_var_parameter);
		///<summary>
		///Method to visit recursive_collection_parameter.
		///</summary>
		///<param name="_recursive_collection_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(recursive_collection_parameter _recursive_collection_parameter);
		///<summary>
		///Method to visit recursive_pattern_parameter.
		///</summary>
		///<param name="_recursive_pattern_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(recursive_pattern_parameter _recursive_pattern_parameter);
		///<summary>
		///Method to visit tuple_pattern.
		///</summary>
		///<param name="_tuple_pattern">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(tuple_pattern _tuple_pattern);
		///<summary>
		///Method to visit tuple_pattern_var_parameter.
		///</summary>
		///<param name="_tuple_pattern_var_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(tuple_pattern_var_parameter _tuple_pattern_var_parameter);
		///<summary>
		///Method to visit recursive_tuple_parameter.
		///</summary>
		///<param name="_recursive_tuple_parameter">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(recursive_tuple_parameter _recursive_tuple_parameter);
		///<summary>
		///Method to visit diapason_expr_new.
		///</summary>
		///<param name="_diapason_expr_new">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(diapason_expr_new _diapason_expr_new);
		///<summary>
		///Method to visit if_expr_new.
		///</summary>
		///<param name="_if_expr_new">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(if_expr_new _if_expr_new);
		///<summary>
		///Method to visit simple_expr_with_deref.
		///</summary>
		///<param name="_simple_expr_with_deref">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(simple_expr_with_deref _simple_expr_with_deref);
		///<summary>
		///Method to visit index.
		///</summary>
		///<param name="_index">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(index _index);
		///<summary>
		///Method to visit array_const_new.
		///</summary>
		///<param name="_array_const_new">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(array_const_new _array_const_new);
		///<summary>
		///Method to visit semantic_ith_element_of.
		///</summary>
		///<param name="_semantic_ith_element_of">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(semantic_ith_element_of _semantic_ith_element_of);
		///<summary>
		///Method to visit bigint_const.
		///</summary>
		///<param name="_bigint_const">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(bigint_const _bigint_const);
		///<summary>
		///Method to visit foreach_stmt_formatting.
		///</summary>
		///<param name="_foreach_stmt_formatting">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(foreach_stmt_formatting _foreach_stmt_formatting);
		///<summary>
		///Method to visit property_ident.
		///</summary>
		///<param name="_property_ident">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(property_ident _property_ident);
		///<summary>
		///Method to visit expression_with_let.
		///</summary>
		///<param name="_expression_with_let">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(expression_with_let _expression_with_let);
		///<summary>
		///Method to visit lambda_any_type_node_syntax.
		///</summary>
		///<param name="_lambda_any_type_node_syntax">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(lambda_any_type_node_syntax _lambda_any_type_node_syntax);
		///<summary>
		///Method to visit ref_var_def_statement.
		///</summary>
		///<param name="_ref_var_def_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(ref_var_def_statement _ref_var_def_statement);
		///<summary>
		///Method to visit let_var_expr.
		///</summary>
		///<param name="_let_var_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(let_var_expr _let_var_expr);
		///<summary>
		///Method to visit to_expr.
		///</summary>
		///<param name="_to_expr">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(to_expr _to_expr);
		///<summary>
		///Method to visit global_statement.
		///</summary>
		///<param name="_global_statement">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(global_statement _global_statement);
		///<summary>
		///Method to visit list_generator.
		///</summary>
		///<param name="_list_generator">Node to visit</param>
		///<returns> Return value is void </returns>
		void visit(list_generator _list_generator);
	}


}

