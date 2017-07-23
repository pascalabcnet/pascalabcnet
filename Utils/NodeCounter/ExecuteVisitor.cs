// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
namespace PascalABCCompiler.SyntaxTree
{
	public class ExecuteVisitor:AbstractVisitor
	{
		private IVisitor executer;
		public ExecuteVisitor(IVisitor ex)
		{
			executer = ex;
		}
		public override void visit(syntax_tree_node _syntax_tree_node)
		{
			executer.visit(_syntax_tree_node);
		}
		public override void visit(expression _expression)
		{
			executer.visit(_expression);
			if (_expression.attributes != null)
				this.visit((dynamic)_expression.attributes);
		}
		public override void visit(statement _statement)
		{
			executer.visit(_statement);
			if (_statement.attributes != null)
				this.visit((dynamic)_statement.attributes);
		}
		public override void visit(statement_list _statement_list)
		{
			executer.visit(_statement_list);
			if (_statement_list.subnodes != null)
			foreach (dynamic x in _statement_list.subnodes)
				if(x != null)
					this.visit(x);
			if (_statement_list.left_logical_bracket != null)
				this.visit((dynamic)_statement_list.left_logical_bracket);
			if (_statement_list.right_logical_bracket != null)
				this.visit((dynamic)_statement_list.right_logical_bracket);
			if (_statement_list.attributes != null)
				this.visit((dynamic)_statement_list.attributes);
		}
		public override void visit(ident _ident)
		{
			executer.visit(_ident);
			if (_ident.attributes != null)
				this.visit((dynamic)_ident.attributes);
		}
		public override void visit(assign _assign)
		{
			executer.visit(_assign);
			if (_assign.to != null)
				this.visit((dynamic)_assign.to);
			if (_assign.from != null)
				this.visit((dynamic)_assign.from);
			if (_assign.attributes != null)
				this.visit((dynamic)_assign.attributes);
		}
		public override void visit(bin_expr _bin_expr)
		{
			executer.visit(_bin_expr);
			if (_bin_expr.left != null)
				this.visit((dynamic)_bin_expr.left);
			if (_bin_expr.right != null)
				this.visit((dynamic)_bin_expr.right);
			if (_bin_expr.attributes != null)
				this.visit((dynamic)_bin_expr.attributes);
		}
		public override void visit(un_expr _un_expr)
		{
			executer.visit(_un_expr);
			if (_un_expr.subnode != null)
				this.visit((dynamic)_un_expr.subnode);
			if (_un_expr.attributes != null)
				this.visit((dynamic)_un_expr.attributes);
		}
		public override void visit(const_node _const_node)
		{
			executer.visit(_const_node);
			if (_const_node.attributes != null)
				this.visit((dynamic)_const_node.attributes);
		}
		public override void visit(bool_const _bool_const)
		{
			executer.visit(_bool_const);
			if (_bool_const.attributes != null)
				this.visit((dynamic)_bool_const.attributes);
		}
		public override void visit(int32_const _int32_const)
		{
			executer.visit(_int32_const);
			if (_int32_const.attributes != null)
				this.visit((dynamic)_int32_const.attributes);
		}
		public override void visit(double_const _double_const)
		{
			executer.visit(_double_const);
			if (_double_const.attributes != null)
				this.visit((dynamic)_double_const.attributes);
		}
		public override void visit(subprogram_body _subprogram_body)
		{
			executer.visit(_subprogram_body);
			if (_subprogram_body.subprogram_code != null)
				this.visit((dynamic)_subprogram_body.subprogram_code);
			if (_subprogram_body.subprogram_defs != null)
				this.visit((dynamic)_subprogram_body.subprogram_defs);
		}
		public override void visit(addressed_value _addressed_value)
		{
			executer.visit(_addressed_value);
			if (_addressed_value.attributes != null)
				this.visit((dynamic)_addressed_value.attributes);
		}
		public override void visit(type_definition _type_definition)
		{
			executer.visit(_type_definition);
			if (_type_definition.attr_list != null)
				this.visit((dynamic)_type_definition.attr_list);
			if (_type_definition.attributes != null)
				this.visit((dynamic)_type_definition.attributes);
		}
		public override void visit(roof_dereference _roof_dereference)
		{
			executer.visit(_roof_dereference);
			if (_roof_dereference.dereferencing_value != null)
				this.visit((dynamic)_roof_dereference.dereferencing_value);
			if (_roof_dereference.attributes != null)
				this.visit((dynamic)_roof_dereference.attributes);
		}
		public override void visit(named_type_reference _named_type_reference)
		{
			executer.visit(_named_type_reference);
			if (_named_type_reference.names != null)
			foreach (dynamic x in _named_type_reference.names)
				if(x != null)
					this.visit(x);
			if (_named_type_reference.attr_list != null)
				this.visit((dynamic)_named_type_reference.attr_list);
			if (_named_type_reference.attributes != null)
				this.visit((dynamic)_named_type_reference.attributes);
		}
		public override void visit(variable_definitions _variable_definitions)
		{
			executer.visit(_variable_definitions);
			if (_variable_definitions.var_definitions != null)
			foreach (dynamic x in _variable_definitions.var_definitions)
				if(x != null)
					this.visit(x);
			if (_variable_definitions.attributes != null)
				this.visit((dynamic)_variable_definitions.attributes);
		}
		public override void visit(ident_list _ident_list)
		{
			executer.visit(_ident_list);
			if (_ident_list.idents != null)
			foreach (dynamic x in _ident_list.idents)
				if(x != null)
					this.visit(x);
		}
		public override void visit(var_def_statement _var_def_statement)
		{
			executer.visit(_var_def_statement);
			if (_var_def_statement.vars != null)
				this.visit((dynamic)_var_def_statement.vars);
			if (_var_def_statement.vars_type != null)
				this.visit((dynamic)_var_def_statement.vars_type);
			if (_var_def_statement.inital_value != null)
				this.visit((dynamic)_var_def_statement.inital_value);
			if (_var_def_statement.attributes != null)
				this.visit((dynamic)_var_def_statement.attributes);
		}
		public override void visit(declaration _declaration)
		{
			executer.visit(_declaration);
			if (_declaration.attributes != null)
				this.visit((dynamic)_declaration.attributes);
		}
		public override void visit(declarations _declarations)
		{
			executer.visit(_declarations);
			if (_declarations.defs != null)
			foreach (dynamic x in _declarations.defs)
				if(x != null)
					this.visit(x);
		}
		public override void visit(program_tree _program_tree)
		{
			executer.visit(_program_tree);
			if (_program_tree.compilation_units != null)
			foreach (dynamic x in _program_tree.compilation_units)
				if(x != null)
					this.visit(x);
		}
		public override void visit(program_name _program_name)
		{
			executer.visit(_program_name);
			if (_program_name.prog_name != null)
				this.visit((dynamic)_program_name.prog_name);
		}
		public override void visit(string_const _string_const)
		{
			executer.visit(_string_const);
			if (_string_const.attributes != null)
				this.visit((dynamic)_string_const.attributes);
		}
		public override void visit(expression_list _expression_list)
		{
			executer.visit(_expression_list);
			if (_expression_list.expressions != null)
			foreach (dynamic x in _expression_list.expressions)
				if(x != null)
					this.visit(x);
			if (_expression_list.attributes != null)
				this.visit((dynamic)_expression_list.attributes);
		}
		public override void visit(dereference _dereference)
		{
			executer.visit(_dereference);
			if (_dereference.dereferencing_value != null)
				this.visit((dynamic)_dereference.dereferencing_value);
			if (_dereference.attributes != null)
				this.visit((dynamic)_dereference.attributes);
		}
		public override void visit(indexer _indexer)
		{
			executer.visit(_indexer);
			if (_indexer.indexes != null)
				this.visit((dynamic)_indexer.indexes);
			if (_indexer.dereferencing_value != null)
				this.visit((dynamic)_indexer.dereferencing_value);
			if (_indexer.attributes != null)
				this.visit((dynamic)_indexer.attributes);
		}
		public override void visit(for_node _for_node)
		{
			executer.visit(_for_node);
			if (_for_node.loop_variable != null)
				this.visit((dynamic)_for_node.loop_variable);
			if (_for_node.initial_value != null)
				this.visit((dynamic)_for_node.initial_value);
			if (_for_node.finish_value != null)
				this.visit((dynamic)_for_node.finish_value);
			if (_for_node.statements != null)
				this.visit((dynamic)_for_node.statements);
			if (_for_node.increment_value != null)
				this.visit((dynamic)_for_node.increment_value);
			if (_for_node.type_name != null)
				this.visit((dynamic)_for_node.type_name);
			if (_for_node.attributes != null)
				this.visit((dynamic)_for_node.attributes);
		}
		public override void visit(repeat_node _repeat_node)
		{
			executer.visit(_repeat_node);
			if (_repeat_node.statements != null)
				this.visit((dynamic)_repeat_node.statements);
			if (_repeat_node.expr != null)
				this.visit((dynamic)_repeat_node.expr);
			if (_repeat_node.attributes != null)
				this.visit((dynamic)_repeat_node.attributes);
		}
		public override void visit(while_node _while_node)
		{
			executer.visit(_while_node);
			if (_while_node.expr != null)
				this.visit((dynamic)_while_node.expr);
			if (_while_node.statements != null)
				this.visit((dynamic)_while_node.statements);
			if (_while_node.attributes != null)
				this.visit((dynamic)_while_node.attributes);
		}
		public override void visit(if_node _if_node)
		{
			executer.visit(_if_node);
			if (_if_node.condition != null)
				this.visit((dynamic)_if_node.condition);
			if (_if_node.then_body != null)
				this.visit((dynamic)_if_node.then_body);
			if (_if_node.else_body != null)
				this.visit((dynamic)_if_node.else_body);
			if (_if_node.attributes != null)
				this.visit((dynamic)_if_node.attributes);
		}
		public override void visit(ref_type _ref_type)
		{
			executer.visit(_ref_type);
			if (_ref_type.pointed_to != null)
				this.visit((dynamic)_ref_type.pointed_to);
			if (_ref_type.attr_list != null)
				this.visit((dynamic)_ref_type.attr_list);
			if (_ref_type.attributes != null)
				this.visit((dynamic)_ref_type.attributes);
		}
		public override void visit(diapason _diapason)
		{
			executer.visit(_diapason);
			if (_diapason.left != null)
				this.visit((dynamic)_diapason.left);
			if (_diapason.right != null)
				this.visit((dynamic)_diapason.right);
			if (_diapason.attr_list != null)
				this.visit((dynamic)_diapason.attr_list);
			if (_diapason.attributes != null)
				this.visit((dynamic)_diapason.attributes);
		}
		public override void visit(indexers_types _indexers_types)
		{
			executer.visit(_indexers_types);
			if (_indexers_types.indexers != null)
			foreach (dynamic x in _indexers_types.indexers)
				if(x != null)
					this.visit(x);
			if (_indexers_types.attr_list != null)
				this.visit((dynamic)_indexers_types.attr_list);
			if (_indexers_types.attributes != null)
				this.visit((dynamic)_indexers_types.attributes);
		}
		public override void visit(array_type _array_type)
		{
			executer.visit(_array_type);
			if (_array_type.indexers != null)
				this.visit((dynamic)_array_type.indexers);
			if (_array_type.elemets_types != null)
				this.visit((dynamic)_array_type.elemets_types);
			if (_array_type.attr_list != null)
				this.visit((dynamic)_array_type.attr_list);
			if (_array_type.attributes != null)
				this.visit((dynamic)_array_type.attributes);
		}
		public override void visit(label_definitions _label_definitions)
		{
			executer.visit(_label_definitions);
			if (_label_definitions.labels != null)
				this.visit((dynamic)_label_definitions.labels);
			if (_label_definitions.attributes != null)
				this.visit((dynamic)_label_definitions.attributes);
		}
		public override void visit(procedure_attribute _procedure_attribute)
		{
			executer.visit(_procedure_attribute);
			if (_procedure_attribute.attributes != null)
				this.visit((dynamic)_procedure_attribute.attributes);
		}
		public override void visit(typed_parameters _typed_parameters)
		{
			executer.visit(_typed_parameters);
			if (_typed_parameters.idents != null)
				this.visit((dynamic)_typed_parameters.idents);
			if (_typed_parameters.vars_type != null)
				this.visit((dynamic)_typed_parameters.vars_type);
			if (_typed_parameters.inital_value != null)
				this.visit((dynamic)_typed_parameters.inital_value);
			if (_typed_parameters.attributes != null)
				this.visit((dynamic)_typed_parameters.attributes);
		}
		public override void visit(formal_parameters _formal_parameters)
		{
			executer.visit(_formal_parameters);
			if (_formal_parameters.params_list != null)
			foreach (dynamic x in _formal_parameters.params_list)
				if(x != null)
					this.visit(x);
		}
		public override void visit(procedure_attributes_list _procedure_attributes_list)
		{
			executer.visit(_procedure_attributes_list);
			if (_procedure_attributes_list.proc_attributes != null)
			foreach (dynamic x in _procedure_attributes_list.proc_attributes)
				if(x != null)
					this.visit(x);
		}
		public override void visit(procedure_header _procedure_header)
		{
			executer.visit(_procedure_header);
			if (_procedure_header.parameters != null)
				this.visit((dynamic)_procedure_header.parameters);
			if (_procedure_header.proc_attributes != null)
				this.visit((dynamic)_procedure_header.proc_attributes);
			if (_procedure_header.name != null)
				this.visit((dynamic)_procedure_header.name);
			if (_procedure_header.template_args != null)
				this.visit((dynamic)_procedure_header.template_args);
			if (_procedure_header.where_defs != null)
				this.visit((dynamic)_procedure_header.where_defs);
			if (_procedure_header.attr_list != null)
				this.visit((dynamic)_procedure_header.attr_list);
			if (_procedure_header.attributes != null)
				this.visit((dynamic)_procedure_header.attributes);
		}
		public override void visit(function_header _function_header)
		{
			executer.visit(_function_header);
			if (_function_header.return_type != null)
				this.visit((dynamic)_function_header.return_type);
			if (_function_header.parameters != null)
				this.visit((dynamic)_function_header.parameters);
			if (_function_header.proc_attributes != null)
				this.visit((dynamic)_function_header.proc_attributes);
			if (_function_header.name != null)
				this.visit((dynamic)_function_header.name);
			if (_function_header.template_args != null)
				this.visit((dynamic)_function_header.template_args);
			if (_function_header.where_defs != null)
				this.visit((dynamic)_function_header.where_defs);
			if (_function_header.attr_list != null)
				this.visit((dynamic)_function_header.attr_list);
			if (_function_header.attributes != null)
				this.visit((dynamic)_function_header.attributes);
		}
		public override void visit(procedure_definition _procedure_definition)
		{
			executer.visit(_procedure_definition);
			if (_procedure_definition.proc_header != null)
				this.visit((dynamic)_procedure_definition.proc_header);
			if (_procedure_definition.proc_body != null)
				this.visit((dynamic)_procedure_definition.proc_body);
			if (_procedure_definition.attributes != null)
				this.visit((dynamic)_procedure_definition.attributes);
		}
		public override void visit(type_declaration _type_declaration)
		{
			executer.visit(_type_declaration);
			if (_type_declaration.type_name != null)
				this.visit((dynamic)_type_declaration.type_name);
			if (_type_declaration.type_def != null)
				this.visit((dynamic)_type_declaration.type_def);
			if (_type_declaration.attributes != null)
				this.visit((dynamic)_type_declaration.attributes);
		}
		public override void visit(type_declarations _type_declarations)
		{
			executer.visit(_type_declarations);
			if (_type_declarations.types_decl != null)
			foreach (dynamic x in _type_declarations.types_decl)
				if(x != null)
					this.visit(x);
			if (_type_declarations.attributes != null)
				this.visit((dynamic)_type_declarations.attributes);
		}
		public override void visit(simple_const_definition _simple_const_definition)
		{
			executer.visit(_simple_const_definition);
			if (_simple_const_definition.const_name != null)
				this.visit((dynamic)_simple_const_definition.const_name);
			if (_simple_const_definition.const_value != null)
				this.visit((dynamic)_simple_const_definition.const_value);
			if (_simple_const_definition.attributes != null)
				this.visit((dynamic)_simple_const_definition.attributes);
		}
		public override void visit(typed_const_definition _typed_const_definition)
		{
			executer.visit(_typed_const_definition);
			if (_typed_const_definition.const_type != null)
				this.visit((dynamic)_typed_const_definition.const_type);
			if (_typed_const_definition.const_name != null)
				this.visit((dynamic)_typed_const_definition.const_name);
			if (_typed_const_definition.const_value != null)
				this.visit((dynamic)_typed_const_definition.const_value);
			if (_typed_const_definition.attributes != null)
				this.visit((dynamic)_typed_const_definition.attributes);
		}
		public override void visit(const_definition _const_definition)
		{
			executer.visit(_const_definition);
			if (_const_definition.const_name != null)
				this.visit((dynamic)_const_definition.const_name);
			if (_const_definition.const_value != null)
				this.visit((dynamic)_const_definition.const_value);
			if (_const_definition.attributes != null)
				this.visit((dynamic)_const_definition.attributes);
		}
		public override void visit(consts_definitions_list _consts_definitions_list)
		{
			executer.visit(_consts_definitions_list);
			if (_consts_definitions_list.const_defs != null)
			foreach (dynamic x in _consts_definitions_list.const_defs)
				if(x != null)
					this.visit(x);
			if (_consts_definitions_list.attributes != null)
				this.visit((dynamic)_consts_definitions_list.attributes);
		}
		public override void visit(unit_name _unit_name)
		{
			executer.visit(_unit_name);
			if (_unit_name.idunit_name != null)
				this.visit((dynamic)_unit_name.idunit_name);
		}
		public override void visit(unit_or_namespace _unit_or_namespace)
		{
			executer.visit(_unit_or_namespace);
			if (_unit_or_namespace.name != null)
				this.visit((dynamic)_unit_or_namespace.name);
		}
		public override void visit(uses_unit_in _uses_unit_in)
		{
			executer.visit(_uses_unit_in);
			if (_uses_unit_in.in_file != null)
				this.visit((dynamic)_uses_unit_in.in_file);
			if (_uses_unit_in.name != null)
				this.visit((dynamic)_uses_unit_in.name);
		}
		public override void visit(uses_list _uses_list)
		{
			executer.visit(_uses_list);
			if (_uses_list.units != null)
			foreach (dynamic x in _uses_list.units)
				if(x != null)
					this.visit(x);
		}
		public override void visit(program_body _program_body)
		{
			executer.visit(_program_body);
			if (_program_body.used_units != null)
				this.visit((dynamic)_program_body.used_units);
			if (_program_body.program_definitions != null)
				this.visit((dynamic)_program_body.program_definitions);
			if (_program_body.program_code != null)
				this.visit((dynamic)_program_body.program_code);
			if (_program_body.using_list != null)
				this.visit((dynamic)_program_body.using_list);
		}
		public override void visit(compilation_unit _compilation_unit)
		{
			executer.visit(_compilation_unit);
			if (_compilation_unit.compiler_directives != null)
			foreach (dynamic x in _compilation_unit.compiler_directives)
				if(x != null)
					this.visit(x);
		}
		public override void visit(unit_module _unit_module)
		{
			executer.visit(_unit_module);
			if (_unit_module.unit_name != null)
				this.visit((dynamic)_unit_module.unit_name);
			if (_unit_module.interface_part != null)
				this.visit((dynamic)_unit_module.interface_part);
			if (_unit_module.implementation_part != null)
				this.visit((dynamic)_unit_module.implementation_part);
			if (_unit_module.initialization_part != null)
				this.visit((dynamic)_unit_module.initialization_part);
			if (_unit_module.finalization_part != null)
				this.visit((dynamic)_unit_module.finalization_part);
			if (_unit_module.compiler_directives != null)
			foreach (dynamic x in _unit_module.compiler_directives)
				if(x != null)
					this.visit(x);
		}
		public override void visit(program_module _program_module)
		{
			executer.visit(_program_module);
			if (_program_module.program_name != null)
				this.visit((dynamic)_program_module.program_name);
			if (_program_module.used_units != null)
				this.visit((dynamic)_program_module.used_units);
			if (_program_module.program_block != null)
				this.visit((dynamic)_program_module.program_block);
			if (_program_module.using_namespaces != null)
				this.visit((dynamic)_program_module.using_namespaces);
			if (_program_module.compiler_directives != null)
			foreach (dynamic x in _program_module.compiler_directives)
				if(x != null)
					this.visit(x);
		}
		public override void visit(hex_constant _hex_constant)
		{
			executer.visit(_hex_constant);
			if (_hex_constant.attributes != null)
				this.visit((dynamic)_hex_constant.attributes);
		}
		public override void visit(get_address _get_address)
		{
			executer.visit(_get_address);
			if (_get_address.address_of != null)
				this.visit((dynamic)_get_address.address_of);
			if (_get_address.attributes != null)
				this.visit((dynamic)_get_address.attributes);
		}
		public override void visit(case_variant _case_variant)
		{
			executer.visit(_case_variant);
			if (_case_variant.conditions != null)
				this.visit((dynamic)_case_variant.conditions);
			if (_case_variant.exec_if_true != null)
				this.visit((dynamic)_case_variant.exec_if_true);
			if (_case_variant.attributes != null)
				this.visit((dynamic)_case_variant.attributes);
		}
		public override void visit(case_node _case_node)
		{
			executer.visit(_case_node);
			if (_case_node.param != null)
				this.visit((dynamic)_case_node.param);
			if (_case_node.conditions != null)
				this.visit((dynamic)_case_node.conditions);
			if (_case_node.else_statement != null)
				this.visit((dynamic)_case_node.else_statement);
			if (_case_node.attributes != null)
				this.visit((dynamic)_case_node.attributes);
		}
		public override void visit(method_name _method_name)
		{
			executer.visit(_method_name);
			if (_method_name.class_name != null)
				this.visit((dynamic)_method_name.class_name);
			if (_method_name.meth_name != null)
				this.visit((dynamic)_method_name.meth_name);
			if (_method_name.explicit_interface_name != null)
				this.visit((dynamic)_method_name.explicit_interface_name);
		}
		public override void visit(dot_node _dot_node)
		{
			executer.visit(_dot_node);
			if (_dot_node.left != null)
				this.visit((dynamic)_dot_node.left);
			if (_dot_node.right != null)
				this.visit((dynamic)_dot_node.right);
			if (_dot_node.attributes != null)
				this.visit((dynamic)_dot_node.attributes);
		}
		public override void visit(empty_statement _empty_statement)
		{
			executer.visit(_empty_statement);
			if (_empty_statement.attributes != null)
				this.visit((dynamic)_empty_statement.attributes);
		}
		public override void visit(goto_statement _goto_statement)
		{
			executer.visit(_goto_statement);
			if (_goto_statement.label != null)
				this.visit((dynamic)_goto_statement.label);
			if (_goto_statement.attributes != null)
				this.visit((dynamic)_goto_statement.attributes);
		}
		public override void visit(labeled_statement _labeled_statement)
		{
			executer.visit(_labeled_statement);
			if (_labeled_statement.label_name != null)
				this.visit((dynamic)_labeled_statement.label_name);
			if (_labeled_statement.to_statement != null)
				this.visit((dynamic)_labeled_statement.to_statement);
			if (_labeled_statement.attributes != null)
				this.visit((dynamic)_labeled_statement.attributes);
		}
		public override void visit(with_statement _with_statement)
		{
			executer.visit(_with_statement);
			if (_with_statement.what_do != null)
				this.visit((dynamic)_with_statement.what_do);
			if (_with_statement.do_with != null)
				this.visit((dynamic)_with_statement.do_with);
			if (_with_statement.attributes != null)
				this.visit((dynamic)_with_statement.attributes);
		}
		public override void visit(method_call _method_call)
		{
			executer.visit(_method_call);
			if (_method_call.parameters != null)
				this.visit((dynamic)_method_call.parameters);
			if (_method_call.dereferencing_value != null)
				this.visit((dynamic)_method_call.dereferencing_value);
			if (_method_call.attributes != null)
				this.visit((dynamic)_method_call.attributes);
		}
		public override void visit(pascal_set_constant _pascal_set_constant)
		{
			executer.visit(_pascal_set_constant);
			if (_pascal_set_constant.values != null)
				this.visit((dynamic)_pascal_set_constant.values);
			if (_pascal_set_constant.attributes != null)
				this.visit((dynamic)_pascal_set_constant.attributes);
		}
		public override void visit(array_const _array_const)
		{
			executer.visit(_array_const);
			if (_array_const.elements != null)
				this.visit((dynamic)_array_const.elements);
			if (_array_const.attributes != null)
				this.visit((dynamic)_array_const.attributes);
		}
		public override void visit(write_accessor_name _write_accessor_name)
		{
			executer.visit(_write_accessor_name);
			if (_write_accessor_name.accessor_name != null)
				this.visit((dynamic)_write_accessor_name.accessor_name);
		}
		public override void visit(read_accessor_name _read_accessor_name)
		{
			executer.visit(_read_accessor_name);
			if (_read_accessor_name.accessor_name != null)
				this.visit((dynamic)_read_accessor_name.accessor_name);
		}
		public override void visit(property_accessors _property_accessors)
		{
			executer.visit(_property_accessors);
			if (_property_accessors.read_accessor != null)
				this.visit((dynamic)_property_accessors.read_accessor);
			if (_property_accessors.write_accessor != null)
				this.visit((dynamic)_property_accessors.write_accessor);
		}
		public override void visit(simple_property _simple_property)
		{
			executer.visit(_simple_property);
			if (_simple_property.property_name != null)
				this.visit((dynamic)_simple_property.property_name);
			if (_simple_property.property_type != null)
				this.visit((dynamic)_simple_property.property_type);
			if (_simple_property.index_expression != null)
				this.visit((dynamic)_simple_property.index_expression);
			if (_simple_property.accessors != null)
				this.visit((dynamic)_simple_property.accessors);
			if (_simple_property.array_default != null)
				this.visit((dynamic)_simple_property.array_default);
			if (_simple_property.parameter_list != null)
				this.visit((dynamic)_simple_property.parameter_list);
			if (_simple_property.attributes != null)
				this.visit((dynamic)_simple_property.attributes);
		}
		public override void visit(index_property _index_property)
		{
			executer.visit(_index_property);
			if (_index_property.property_parametres != null)
				this.visit((dynamic)_index_property.property_parametres);
			if (_index_property.is_default != null)
				this.visit((dynamic)_index_property.is_default);
			if (_index_property.property_name != null)
				this.visit((dynamic)_index_property.property_name);
			if (_index_property.property_type != null)
				this.visit((dynamic)_index_property.property_type);
			if (_index_property.index_expression != null)
				this.visit((dynamic)_index_property.index_expression);
			if (_index_property.accessors != null)
				this.visit((dynamic)_index_property.accessors);
			if (_index_property.array_default != null)
				this.visit((dynamic)_index_property.array_default);
			if (_index_property.parameter_list != null)
				this.visit((dynamic)_index_property.parameter_list);
			if (_index_property.attributes != null)
				this.visit((dynamic)_index_property.attributes);
		}
		public override void visit(class_members _class_members)
		{
			executer.visit(_class_members);
			if (_class_members.members != null)
			foreach (dynamic x in _class_members.members)
				if(x != null)
					this.visit(x);
			if (_class_members.access_mod != null)
				this.visit((dynamic)_class_members.access_mod);
		}
		public override void visit(access_modifer_node _access_modifer_node)
		{
			executer.visit(_access_modifer_node);
		}
		public override void visit(class_body _class_body)
		{
			executer.visit(_class_body);
			if (_class_body.class_def_blocks != null)
			foreach (dynamic x in _class_body.class_def_blocks)
				if(x != null)
					this.visit(x);
		}
		public override void visit(class_definition _class_definition)
		{
			executer.visit(_class_definition);
			if (_class_definition.class_parents != null)
				this.visit((dynamic)_class_definition.class_parents);
			if (_class_definition.body != null)
				this.visit((dynamic)_class_definition.body);
			if (_class_definition.template_args != null)
				this.visit((dynamic)_class_definition.template_args);
			if (_class_definition.where_section != null)
				this.visit((dynamic)_class_definition.where_section);
			if (_class_definition.attr_list != null)
				this.visit((dynamic)_class_definition.attr_list);
			if (_class_definition.attributes != null)
				this.visit((dynamic)_class_definition.attributes);
		}
		public override void visit(default_indexer_property_node _default_indexer_property_node)
		{
			executer.visit(_default_indexer_property_node);
		}
		public override void visit(known_type_definition _known_type_definition)
		{
			executer.visit(_known_type_definition);
			if (_known_type_definition.unit_name != null)
				this.visit((dynamic)_known_type_definition.unit_name);
			if (_known_type_definition.attr_list != null)
				this.visit((dynamic)_known_type_definition.attr_list);
			if (_known_type_definition.attributes != null)
				this.visit((dynamic)_known_type_definition.attributes);
		}
		public override void visit(set_type_definition _set_type_definition)
		{
			executer.visit(_set_type_definition);
			if (_set_type_definition.of_type != null)
				this.visit((dynamic)_set_type_definition.of_type);
			if (_set_type_definition.attr_list != null)
				this.visit((dynamic)_set_type_definition.attr_list);
			if (_set_type_definition.attributes != null)
				this.visit((dynamic)_set_type_definition.attributes);
		}
		public override void visit(try_statement _try_statement)
		{
			executer.visit(_try_statement);
			if (_try_statement.statements != null)
				this.visit((dynamic)_try_statement.statements);
			if (_try_statement.attributes != null)
				this.visit((dynamic)_try_statement.attributes);
		}
		public override void visit(on_exception _on_exception)
		{
			executer.visit(_on_exception);
			if (_on_exception.exception_var_name != null)
				this.visit((dynamic)_on_exception.exception_var_name);
			if (_on_exception.exception_type_name != null)
				this.visit((dynamic)_on_exception.exception_type_name);
			if (_on_exception.stat != null)
				this.visit((dynamic)_on_exception.stat);
		}
		public override void visit(on_exception_list _on_exception_list)
		{
			executer.visit(_on_exception_list);
			if (_on_exception_list.on_exceptions != null)
			foreach (dynamic x in _on_exception_list.on_exceptions)
				if(x != null)
					this.visit(x);
		}
		public override void visit(try_finally_statement _try_finally_statement)
		{
			executer.visit(_try_finally_statement);
			if (_try_finally_statement.finally_statements != null)
				this.visit((dynamic)_try_finally_statement.finally_statements);
			if (_try_finally_statement.statements != null)
				this.visit((dynamic)_try_finally_statement.statements);
			if (_try_finally_statement.attributes != null)
				this.visit((dynamic)_try_finally_statement.attributes);
		}
		public override void visit(try_except_statement _try_except_statement)
		{
			executer.visit(_try_except_statement);
			if (_try_except_statement.on_except != null)
				this.visit((dynamic)_try_except_statement.on_except);
			if (_try_except_statement.else_statements != null)
				this.visit((dynamic)_try_except_statement.else_statements);
			if (_try_except_statement.statements != null)
				this.visit((dynamic)_try_except_statement.statements);
			if (_try_except_statement.attributes != null)
				this.visit((dynamic)_try_except_statement.attributes);
		}
		public override void visit(record_const_definition _record_const_definition)
		{
			executer.visit(_record_const_definition);
			if (_record_const_definition.name != null)
				this.visit((dynamic)_record_const_definition.name);
			if (_record_const_definition.val != null)
				this.visit((dynamic)_record_const_definition.val);
			if (_record_const_definition.attributes != null)
				this.visit((dynamic)_record_const_definition.attributes);
		}
		public override void visit(record_const _record_const)
		{
			executer.visit(_record_const);
			if (_record_const.rec_consts != null)
			foreach (dynamic x in _record_const.rec_consts)
				if(x != null)
					this.visit(x);
			if (_record_const.attributes != null)
				this.visit((dynamic)_record_const.attributes);
		}
		public override void visit(record_type _record_type)
		{
			executer.visit(_record_type);
			if (_record_type.parts != null)
				this.visit((dynamic)_record_type.parts);
			if (_record_type.base_type != null)
				this.visit((dynamic)_record_type.base_type);
			if (_record_type.attr_list != null)
				this.visit((dynamic)_record_type.attr_list);
			if (_record_type.attributes != null)
				this.visit((dynamic)_record_type.attributes);
		}
		public override void visit(enum_type_definition _enum_type_definition)
		{
			executer.visit(_enum_type_definition);
			if (_enum_type_definition.enumerators != null)
				this.visit((dynamic)_enum_type_definition.enumerators);
			if (_enum_type_definition.attr_list != null)
				this.visit((dynamic)_enum_type_definition.attr_list);
			if (_enum_type_definition.attributes != null)
				this.visit((dynamic)_enum_type_definition.attributes);
		}
		public override void visit(char_const _char_const)
		{
			executer.visit(_char_const);
			if (_char_const.attributes != null)
				this.visit((dynamic)_char_const.attributes);
		}
		public override void visit(raise_statement _raise_statement)
		{
			executer.visit(_raise_statement);
			if (_raise_statement.excep != null)
				this.visit((dynamic)_raise_statement.excep);
			if (_raise_statement.attributes != null)
				this.visit((dynamic)_raise_statement.attributes);
		}
		public override void visit(sharp_char_const _sharp_char_const)
		{
			executer.visit(_sharp_char_const);
			if (_sharp_char_const.attributes != null)
				this.visit((dynamic)_sharp_char_const.attributes);
		}
		public override void visit(literal_const_line _literal_const_line)
		{
			executer.visit(_literal_const_line);
			if (_literal_const_line.literals != null)
			foreach (dynamic x in _literal_const_line.literals)
				if(x != null)
					this.visit(x);
			if (_literal_const_line.attributes != null)
				this.visit((dynamic)_literal_const_line.attributes);
		}
		public override void visit(string_num_definition _string_num_definition)
		{
			executer.visit(_string_num_definition);
			if (_string_num_definition.num_of_symbols != null)
				this.visit((dynamic)_string_num_definition.num_of_symbols);
			if (_string_num_definition.name != null)
				this.visit((dynamic)_string_num_definition.name);
			if (_string_num_definition.attr_list != null)
				this.visit((dynamic)_string_num_definition.attr_list);
			if (_string_num_definition.attributes != null)
				this.visit((dynamic)_string_num_definition.attributes);
		}
		public override void visit(variant _variant)
		{
			executer.visit(_variant);
			if (_variant.vars != null)
				this.visit((dynamic)_variant.vars);
			if (_variant.vars_type != null)
				this.visit((dynamic)_variant.vars_type);
		}
		public override void visit(variant_list _variant_list)
		{
			executer.visit(_variant_list);
			if (_variant_list.vars != null)
			foreach (dynamic x in _variant_list.vars)
				if(x != null)
					this.visit(x);
		}
		public override void visit(variant_type _variant_type)
		{
			executer.visit(_variant_type);
			if (_variant_type.case_exprs != null)
				this.visit((dynamic)_variant_type.case_exprs);
			if (_variant_type.parts != null)
				this.visit((dynamic)_variant_type.parts);
		}
		public override void visit(variant_types _variant_types)
		{
			executer.visit(_variant_types);
			if (_variant_types.vars != null)
			foreach (dynamic x in _variant_types.vars)
				if(x != null)
					this.visit(x);
		}
		public override void visit(variant_record_type _variant_record_type)
		{
			executer.visit(_variant_record_type);
			if (_variant_record_type.var_name != null)
				this.visit((dynamic)_variant_record_type.var_name);
			if (_variant_record_type.var_type != null)
				this.visit((dynamic)_variant_record_type.var_type);
			if (_variant_record_type.vars != null)
				this.visit((dynamic)_variant_record_type.vars);
		}
		public override void visit(procedure_call _procedure_call)
		{
			executer.visit(_procedure_call);
			if (_procedure_call.func_name != null)
				this.visit((dynamic)_procedure_call.func_name);
			if (_procedure_call.attributes != null)
				this.visit((dynamic)_procedure_call.attributes);
		}
		public override void visit(class_predefinition _class_predefinition)
		{
			executer.visit(_class_predefinition);
			if (_class_predefinition.class_name != null)
				this.visit((dynamic)_class_predefinition.class_name);
			if (_class_predefinition.type_name != null)
				this.visit((dynamic)_class_predefinition.type_name);
			if (_class_predefinition.type_def != null)
				this.visit((dynamic)_class_predefinition.type_def);
			if (_class_predefinition.attributes != null)
				this.visit((dynamic)_class_predefinition.attributes);
		}
		public override void visit(nil_const _nil_const)
		{
			executer.visit(_nil_const);
			if (_nil_const.attributes != null)
				this.visit((dynamic)_nil_const.attributes);
		}
		public override void visit(file_type_definition _file_type_definition)
		{
			executer.visit(_file_type_definition);
			if (_file_type_definition.elem_type != null)
				this.visit((dynamic)_file_type_definition.elem_type);
			if (_file_type_definition.attr_list != null)
				this.visit((dynamic)_file_type_definition.attr_list);
			if (_file_type_definition.attributes != null)
				this.visit((dynamic)_file_type_definition.attributes);
		}
		public override void visit(constructor _constructor)
		{
			executer.visit(_constructor);
			if (_constructor.parameters != null)
				this.visit((dynamic)_constructor.parameters);
			if (_constructor.proc_attributes != null)
				this.visit((dynamic)_constructor.proc_attributes);
			if (_constructor.name != null)
				this.visit((dynamic)_constructor.name);
			if (_constructor.template_args != null)
				this.visit((dynamic)_constructor.template_args);
			if (_constructor.where_defs != null)
				this.visit((dynamic)_constructor.where_defs);
			if (_constructor.attr_list != null)
				this.visit((dynamic)_constructor.attr_list);
			if (_constructor.attributes != null)
				this.visit((dynamic)_constructor.attributes);
		}
		public override void visit(destructor _destructor)
		{
			executer.visit(_destructor);
			if (_destructor.parameters != null)
				this.visit((dynamic)_destructor.parameters);
			if (_destructor.proc_attributes != null)
				this.visit((dynamic)_destructor.proc_attributes);
			if (_destructor.name != null)
				this.visit((dynamic)_destructor.name);
			if (_destructor.template_args != null)
				this.visit((dynamic)_destructor.template_args);
			if (_destructor.where_defs != null)
				this.visit((dynamic)_destructor.where_defs);
			if (_destructor.attr_list != null)
				this.visit((dynamic)_destructor.attr_list);
			if (_destructor.attributes != null)
				this.visit((dynamic)_destructor.attributes);
		}
		public override void visit(inherited_method_call _inherited_method_call)
		{
			executer.visit(_inherited_method_call);
			if (_inherited_method_call.method_name != null)
				this.visit((dynamic)_inherited_method_call.method_name);
			if (_inherited_method_call.exprs != null)
				this.visit((dynamic)_inherited_method_call.exprs);
			if (_inherited_method_call.attributes != null)
				this.visit((dynamic)_inherited_method_call.attributes);
		}
		public override void visit(typecast_node _typecast_node)
		{
			executer.visit(_typecast_node);
			if (_typecast_node.expr != null)
				this.visit((dynamic)_typecast_node.expr);
			if (_typecast_node.type_def != null)
				this.visit((dynamic)_typecast_node.type_def);
			if (_typecast_node.attributes != null)
				this.visit((dynamic)_typecast_node.attributes);
		}
		public override void visit(interface_node _interface_node)
		{
			executer.visit(_interface_node);
			if (_interface_node.interface_definitions != null)
				this.visit((dynamic)_interface_node.interface_definitions);
			if (_interface_node.uses_modules != null)
				this.visit((dynamic)_interface_node.uses_modules);
			if (_interface_node.using_namespaces != null)
				this.visit((dynamic)_interface_node.using_namespaces);
		}
		public override void visit(implementation_node _implementation_node)
		{
			executer.visit(_implementation_node);
			if (_implementation_node.uses_modules != null)
				this.visit((dynamic)_implementation_node.uses_modules);
			if (_implementation_node.implementation_definitions != null)
				this.visit((dynamic)_implementation_node.implementation_definitions);
			if (_implementation_node.using_namespaces != null)
				this.visit((dynamic)_implementation_node.using_namespaces);
		}
		public override void visit(diap_expr _diap_expr)
		{
			executer.visit(_diap_expr);
			if (_diap_expr.left != null)
				this.visit((dynamic)_diap_expr.left);
			if (_diap_expr.right != null)
				this.visit((dynamic)_diap_expr.right);
			if (_diap_expr.attributes != null)
				this.visit((dynamic)_diap_expr.attributes);
		}
		public override void visit(block _block)
		{
			executer.visit(_block);
			if (_block.defs != null)
				this.visit((dynamic)_block.defs);
			if (_block.program_code != null)
				this.visit((dynamic)_block.program_code);
		}
		public override void visit(proc_block _proc_block)
		{
			executer.visit(_proc_block);
		}
		public override void visit(array_of_named_type_definition _array_of_named_type_definition)
		{
			executer.visit(_array_of_named_type_definition);
			if (_array_of_named_type_definition.type_name != null)
				this.visit((dynamic)_array_of_named_type_definition.type_name);
			if (_array_of_named_type_definition.attr_list != null)
				this.visit((dynamic)_array_of_named_type_definition.attr_list);
			if (_array_of_named_type_definition.attributes != null)
				this.visit((dynamic)_array_of_named_type_definition.attributes);
		}
		public override void visit(array_of_const_type_definition _array_of_const_type_definition)
		{
			executer.visit(_array_of_const_type_definition);
			if (_array_of_const_type_definition.attr_list != null)
				this.visit((dynamic)_array_of_const_type_definition.attr_list);
			if (_array_of_const_type_definition.attributes != null)
				this.visit((dynamic)_array_of_const_type_definition.attributes);
		}
		public override void visit(literal _literal)
		{
			executer.visit(_literal);
			if (_literal.attributes != null)
				this.visit((dynamic)_literal.attributes);
		}
		public override void visit(case_variants _case_variants)
		{
			executer.visit(_case_variants);
			if (_case_variants.variants != null)
			foreach (dynamic x in _case_variants.variants)
				if(x != null)
					this.visit(x);
		}
		public override void visit(diapason_expr _diapason_expr)
		{
			executer.visit(_diapason_expr);
			if (_diapason_expr.left != null)
				this.visit((dynamic)_diapason_expr.left);
			if (_diapason_expr.right != null)
				this.visit((dynamic)_diapason_expr.right);
			if (_diapason_expr.attributes != null)
				this.visit((dynamic)_diapason_expr.attributes);
		}
		public override void visit(var_def_list _var_def_list)
		{
			executer.visit(_var_def_list);
			if (_var_def_list.vars != null)
			foreach (dynamic x in _var_def_list.vars)
				if(x != null)
					this.visit(x);
		}
		public override void visit(record_type_parts _record_type_parts)
		{
			executer.visit(_record_type_parts);
			if (_record_type_parts.fixed_part != null)
				this.visit((dynamic)_record_type_parts.fixed_part);
			if (_record_type_parts.variant_part != null)
				this.visit((dynamic)_record_type_parts.variant_part);
		}
		public override void visit(property_array_default _property_array_default)
		{
			executer.visit(_property_array_default);
		}
		public override void visit(property_interface _property_interface)
		{
			executer.visit(_property_interface);
			if (_property_interface.parameter_list != null)
				this.visit((dynamic)_property_interface.parameter_list);
			if (_property_interface.property_type != null)
				this.visit((dynamic)_property_interface.property_type);
			if (_property_interface.index_expression != null)
				this.visit((dynamic)_property_interface.index_expression);
		}
		public override void visit(property_parameter _property_parameter)
		{
			executer.visit(_property_parameter);
			if (_property_parameter.names != null)
				this.visit((dynamic)_property_parameter.names);
			if (_property_parameter.type != null)
				this.visit((dynamic)_property_parameter.type);
		}
		public override void visit(property_parameter_list _property_parameter_list)
		{
			executer.visit(_property_parameter_list);
			if (_property_parameter_list.parameters != null)
			foreach (dynamic x in _property_parameter_list.parameters)
				if(x != null)
					this.visit(x);
		}
		public override void visit(inherited_ident _inherited_ident)
		{
			executer.visit(_inherited_ident);
			if (_inherited_ident.attributes != null)
				this.visit((dynamic)_inherited_ident.attributes);
		}
		public override void visit(format_expr _format_expr)
		{
			executer.visit(_format_expr);
			if (_format_expr.expr != null)
				this.visit((dynamic)_format_expr.expr);
			if (_format_expr.format1 != null)
				this.visit((dynamic)_format_expr.format1);
			if (_format_expr.format2 != null)
				this.visit((dynamic)_format_expr.format2);
			if (_format_expr.attributes != null)
				this.visit((dynamic)_format_expr.attributes);
		}
		public override void visit(initfinal_part _initfinal_part)
		{
			executer.visit(_initfinal_part);
			if (_initfinal_part.initialization_sect != null)
				this.visit((dynamic)_initfinal_part.initialization_sect);
			if (_initfinal_part.finalization_sect != null)
				this.visit((dynamic)_initfinal_part.finalization_sect);
		}
		public override void visit(token_info _token_info)
		{
			executer.visit(_token_info);
		}
		public override void visit(raise_stmt _raise_stmt)
		{
			executer.visit(_raise_stmt);
			if (_raise_stmt.expr != null)
				this.visit((dynamic)_raise_stmt.expr);
			if (_raise_stmt.address != null)
				this.visit((dynamic)_raise_stmt.address);
			if (_raise_stmt.attributes != null)
				this.visit((dynamic)_raise_stmt.attributes);
		}
		public override void visit(op_type_node _op_type_node)
		{
			executer.visit(_op_type_node);
		}
		public override void visit(file_type _file_type)
		{
			executer.visit(_file_type);
			if (_file_type.file_of_type != null)
				this.visit((dynamic)_file_type.file_of_type);
			if (_file_type.attr_list != null)
				this.visit((dynamic)_file_type.attr_list);
			if (_file_type.attributes != null)
				this.visit((dynamic)_file_type.attributes);
		}
		public override void visit(known_type_ident _known_type_ident)
		{
			executer.visit(_known_type_ident);
			if (_known_type_ident.attributes != null)
				this.visit((dynamic)_known_type_ident.attributes);
		}
		public override void visit(exception_handler _exception_handler)
		{
			executer.visit(_exception_handler);
			if (_exception_handler.variable != null)
				this.visit((dynamic)_exception_handler.variable);
			if (_exception_handler.type_name != null)
				this.visit((dynamic)_exception_handler.type_name);
			if (_exception_handler.statements != null)
				this.visit((dynamic)_exception_handler.statements);
		}
		public override void visit(exception_ident _exception_ident)
		{
			executer.visit(_exception_ident);
			if (_exception_ident.variable != null)
				this.visit((dynamic)_exception_ident.variable);
			if (_exception_ident.type_name != null)
				this.visit((dynamic)_exception_ident.type_name);
		}
		public override void visit(exception_handler_list _exception_handler_list)
		{
			executer.visit(_exception_handler_list);
			if (_exception_handler_list.handlers != null)
			foreach (dynamic x in _exception_handler_list.handlers)
				if(x != null)
					this.visit(x);
		}
		public override void visit(exception_block _exception_block)
		{
			executer.visit(_exception_block);
			if (_exception_block.stmt_list != null)
				this.visit((dynamic)_exception_block.stmt_list);
			if (_exception_block.handlers != null)
				this.visit((dynamic)_exception_block.handlers);
			if (_exception_block.else_stmt_list != null)
				this.visit((dynamic)_exception_block.else_stmt_list);
		}
		public override void visit(try_handler _try_handler)
		{
			executer.visit(_try_handler);
		}
		public override void visit(try_handler_finally _try_handler_finally)
		{
			executer.visit(_try_handler_finally);
			if (_try_handler_finally.stmt_list != null)
				this.visit((dynamic)_try_handler_finally.stmt_list);
		}
		public override void visit(try_handler_except _try_handler_except)
		{
			executer.visit(_try_handler_except);
			if (_try_handler_except.except_block != null)
				this.visit((dynamic)_try_handler_except.except_block);
		}
		public override void visit(try_stmt _try_stmt)
		{
			executer.visit(_try_stmt);
			if (_try_stmt.stmt_list != null)
				this.visit((dynamic)_try_stmt.stmt_list);
			if (_try_stmt.handler != null)
				this.visit((dynamic)_try_stmt.handler);
			if (_try_stmt.attributes != null)
				this.visit((dynamic)_try_stmt.attributes);
		}
		public override void visit(inherited_message _inherited_message)
		{
			executer.visit(_inherited_message);
			if (_inherited_message.attributes != null)
				this.visit((dynamic)_inherited_message.attributes);
		}
		public override void visit(external_directive _external_directive)
		{
			executer.visit(_external_directive);
			if (_external_directive.modulename != null)
				this.visit((dynamic)_external_directive.modulename);
			if (_external_directive.name != null)
				this.visit((dynamic)_external_directive.name);
		}
		public override void visit(using_list _using_list)
		{
			executer.visit(_using_list);
			if (_using_list.namespaces != null)
			foreach (dynamic x in _using_list.namespaces)
				if(x != null)
					this.visit(x);
		}
		public override void visit(oberon_import_module _oberon_import_module)
		{
			executer.visit(_oberon_import_module);
			if (_oberon_import_module.new_name != null)
				this.visit((dynamic)_oberon_import_module.new_name);
			if (_oberon_import_module.name != null)
				this.visit((dynamic)_oberon_import_module.name);
		}
		public override void visit(oberon_module _oberon_module)
		{
			executer.visit(_oberon_module);
			if (_oberon_module.first_name != null)
				this.visit((dynamic)_oberon_module.first_name);
			if (_oberon_module.second_name != null)
				this.visit((dynamic)_oberon_module.second_name);
			if (_oberon_module.import_list != null)
				this.visit((dynamic)_oberon_module.import_list);
			if (_oberon_module.definitions != null)
				this.visit((dynamic)_oberon_module.definitions);
			if (_oberon_module.module_code != null)
				this.visit((dynamic)_oberon_module.module_code);
			if (_oberon_module.compiler_directives != null)
			foreach (dynamic x in _oberon_module.compiler_directives)
				if(x != null)
					this.visit(x);
		}
		public override void visit(oberon_ident_with_export_marker _oberon_ident_with_export_marker)
		{
			executer.visit(_oberon_ident_with_export_marker);
			if (_oberon_ident_with_export_marker.attributes != null)
				this.visit((dynamic)_oberon_ident_with_export_marker.attributes);
		}
		public override void visit(oberon_exit_stmt _oberon_exit_stmt)
		{
			executer.visit(_oberon_exit_stmt);
			if (_oberon_exit_stmt.attributes != null)
				this.visit((dynamic)_oberon_exit_stmt.attributes);
		}
		public override void visit(jump_stmt _jump_stmt)
		{
			executer.visit(_jump_stmt);
			if (_jump_stmt.expr != null)
				this.visit((dynamic)_jump_stmt.expr);
			if (_jump_stmt.attributes != null)
				this.visit((dynamic)_jump_stmt.attributes);
		}
		public override void visit(oberon_procedure_receiver _oberon_procedure_receiver)
		{
			executer.visit(_oberon_procedure_receiver);
			if (_oberon_procedure_receiver.receiver_name != null)
				this.visit((dynamic)_oberon_procedure_receiver.receiver_name);
			if (_oberon_procedure_receiver.receiver_typename != null)
				this.visit((dynamic)_oberon_procedure_receiver.receiver_typename);
		}
		public override void visit(oberon_procedure_header _oberon_procedure_header)
		{
			executer.visit(_oberon_procedure_header);
			if (_oberon_procedure_header.receiver != null)
				this.visit((dynamic)_oberon_procedure_header.receiver);
			if (_oberon_procedure_header.first_name != null)
				this.visit((dynamic)_oberon_procedure_header.first_name);
			if (_oberon_procedure_header.second_name != null)
				this.visit((dynamic)_oberon_procedure_header.second_name);
			if (_oberon_procedure_header.return_type != null)
				this.visit((dynamic)_oberon_procedure_header.return_type);
			if (_oberon_procedure_header.parameters != null)
				this.visit((dynamic)_oberon_procedure_header.parameters);
			if (_oberon_procedure_header.proc_attributes != null)
				this.visit((dynamic)_oberon_procedure_header.proc_attributes);
			if (_oberon_procedure_header.name != null)
				this.visit((dynamic)_oberon_procedure_header.name);
			if (_oberon_procedure_header.template_args != null)
				this.visit((dynamic)_oberon_procedure_header.template_args);
			if (_oberon_procedure_header.where_defs != null)
				this.visit((dynamic)_oberon_procedure_header.where_defs);
			if (_oberon_procedure_header.attr_list != null)
				this.visit((dynamic)_oberon_procedure_header.attr_list);
			if (_oberon_procedure_header.attributes != null)
				this.visit((dynamic)_oberon_procedure_header.attributes);
		}
		public override void visit(oberon_withstmt_guardstat _oberon_withstmt_guardstat)
		{
			executer.visit(_oberon_withstmt_guardstat);
			if (_oberon_withstmt_guardstat.name != null)
				this.visit((dynamic)_oberon_withstmt_guardstat.name);
			if (_oberon_withstmt_guardstat.type_name != null)
				this.visit((dynamic)_oberon_withstmt_guardstat.type_name);
			if (_oberon_withstmt_guardstat.stmt != null)
				this.visit((dynamic)_oberon_withstmt_guardstat.stmt);
		}
		public override void visit(oberon_withstmt_guardstat_list _oberon_withstmt_guardstat_list)
		{
			executer.visit(_oberon_withstmt_guardstat_list);
			if (_oberon_withstmt_guardstat_list.guardstats != null)
			foreach (dynamic x in _oberon_withstmt_guardstat_list.guardstats)
				if(x != null)
					this.visit(x);
		}
		public override void visit(oberon_withstmt _oberon_withstmt)
		{
			executer.visit(_oberon_withstmt);
			if (_oberon_withstmt.quardstat_list != null)
				this.visit((dynamic)_oberon_withstmt.quardstat_list);
			if (_oberon_withstmt.else_stmt != null)
				this.visit((dynamic)_oberon_withstmt.else_stmt);
			if (_oberon_withstmt.attributes != null)
				this.visit((dynamic)_oberon_withstmt.attributes);
		}
		public override void visit(loop_stmt _loop_stmt)
		{
			executer.visit(_loop_stmt);
			if (_loop_stmt.stmt != null)
				this.visit((dynamic)_loop_stmt.stmt);
			if (_loop_stmt.attributes != null)
				this.visit((dynamic)_loop_stmt.attributes);
		}
		public override void visit(foreach_stmt _foreach_stmt)
		{
			executer.visit(_foreach_stmt);
			if (_foreach_stmt.identifier != null)
				this.visit((dynamic)_foreach_stmt.identifier);
			if (_foreach_stmt.type_name != null)
				this.visit((dynamic)_foreach_stmt.type_name);
			if (_foreach_stmt.in_what != null)
				this.visit((dynamic)_foreach_stmt.in_what);
			if (_foreach_stmt.stmt != null)
				this.visit((dynamic)_foreach_stmt.stmt);
			if (_foreach_stmt.attributes != null)
				this.visit((dynamic)_foreach_stmt.attributes);
		}
		public override void visit(addressed_value_funcname _addressed_value_funcname)
		{
			executer.visit(_addressed_value_funcname);
			if (_addressed_value_funcname.attributes != null)
				this.visit((dynamic)_addressed_value_funcname.attributes);
		}
		public override void visit(named_type_reference_list _named_type_reference_list)
		{
			executer.visit(_named_type_reference_list);
			if (_named_type_reference_list.types != null)
			foreach (dynamic x in _named_type_reference_list.types)
				if(x != null)
					this.visit(x);
		}
		public override void visit(template_param_list _template_param_list)
		{
			executer.visit(_template_param_list);
			if (_template_param_list.params_list != null)
			foreach (dynamic x in _template_param_list.params_list)
				if(x != null)
					this.visit(x);
			if (_template_param_list.dereferencing_value != null)
				this.visit((dynamic)_template_param_list.dereferencing_value);
			if (_template_param_list.attributes != null)
				this.visit((dynamic)_template_param_list.attributes);
		}
		public override void visit(template_type_reference _template_type_reference)
		{
			executer.visit(_template_type_reference);
			if (_template_type_reference.name != null)
				this.visit((dynamic)_template_type_reference.name);
			if (_template_type_reference.params_list != null)
				this.visit((dynamic)_template_type_reference.params_list);
			if (_template_type_reference.names != null)
			foreach (dynamic x in _template_type_reference.names)
				if(x != null)
					this.visit(x);
			if (_template_type_reference.attr_list != null)
				this.visit((dynamic)_template_type_reference.attr_list);
			if (_template_type_reference.attributes != null)
				this.visit((dynamic)_template_type_reference.attributes);
		}
		public override void visit(int64_const _int64_const)
		{
			executer.visit(_int64_const);
			if (_int64_const.attributes != null)
				this.visit((dynamic)_int64_const.attributes);
		}
		public override void visit(uint64_const _uint64_const)
		{
			executer.visit(_uint64_const);
			if (_uint64_const.attributes != null)
				this.visit((dynamic)_uint64_const.attributes);
		}
		public override void visit(new_expr _new_expr)
		{
			executer.visit(_new_expr);
			if (_new_expr.type != null)
				this.visit((dynamic)_new_expr.type);
			if (_new_expr.params_list != null)
				this.visit((dynamic)_new_expr.params_list);
			if (_new_expr.array_init_expr != null)
				this.visit((dynamic)_new_expr.array_init_expr);
			if (_new_expr.attributes != null)
				this.visit((dynamic)_new_expr.attributes);
		}
		public override void visit(type_definition_list _type_definition_list)
		{
			executer.visit(_type_definition_list);
			if (_type_definition_list.defs != null)
			foreach (dynamic x in _type_definition_list.defs)
				if(x != null)
					this.visit(x);
		}
		public override void visit(where_definition _where_definition)
		{
			executer.visit(_where_definition);
			if (_where_definition.names != null)
				this.visit((dynamic)_where_definition.names);
			if (_where_definition.types != null)
				this.visit((dynamic)_where_definition.types);
		}
		public override void visit(where_definition_list _where_definition_list)
		{
			executer.visit(_where_definition_list);
			if (_where_definition_list.defs != null)
			foreach (dynamic x in _where_definition_list.defs)
				if(x != null)
					this.visit(x);
		}
		public override void visit(sizeof_operator _sizeof_operator)
		{
			executer.visit(_sizeof_operator);
			if (_sizeof_operator.type_def != null)
				this.visit((dynamic)_sizeof_operator.type_def);
			if (_sizeof_operator.expr != null)
				this.visit((dynamic)_sizeof_operator.expr);
			if (_sizeof_operator.attributes != null)
				this.visit((dynamic)_sizeof_operator.attributes);
		}
		public override void visit(typeof_operator _typeof_operator)
		{
			executer.visit(_typeof_operator);
			if (_typeof_operator.type_name != null)
				this.visit((dynamic)_typeof_operator.type_name);
			if (_typeof_operator.attributes != null)
				this.visit((dynamic)_typeof_operator.attributes);
		}
		public override void visit(compiler_directive _compiler_directive)
		{
			executer.visit(_compiler_directive);
			if (_compiler_directive.Name != null)
				this.visit((dynamic)_compiler_directive.Name);
			if (_compiler_directive.Directive != null)
				this.visit((dynamic)_compiler_directive.Directive);
		}
		public override void visit(operator_name_ident _operator_name_ident)
		{
			executer.visit(_operator_name_ident);
			if (_operator_name_ident.attributes != null)
				this.visit((dynamic)_operator_name_ident.attributes);
		}
		public override void visit(var_statement _var_statement)
		{
			executer.visit(_var_statement);
			if (_var_statement.var_def != null)
				this.visit((dynamic)_var_statement.var_def);
			if (_var_statement.attributes != null)
				this.visit((dynamic)_var_statement.attributes);
		}
		public override void visit(question_colon_expression _question_colon_expression)
		{
			executer.visit(_question_colon_expression);
			if (_question_colon_expression.condition != null)
				this.visit((dynamic)_question_colon_expression.condition);
			if (_question_colon_expression.ret_if_true != null)
				this.visit((dynamic)_question_colon_expression.ret_if_true);
			if (_question_colon_expression.ret_if_false != null)
				this.visit((dynamic)_question_colon_expression.ret_if_false);
			if (_question_colon_expression.attributes != null)
				this.visit((dynamic)_question_colon_expression.attributes);
		}
		public override void visit(expression_as_statement _expression_as_statement)
		{
			executer.visit(_expression_as_statement);
			if (_expression_as_statement.expr != null)
				this.visit((dynamic)_expression_as_statement.expr);
			if (_expression_as_statement.attributes != null)
				this.visit((dynamic)_expression_as_statement.attributes);
		}
		public override void visit(c_scalar_type _c_scalar_type)
		{
			executer.visit(_c_scalar_type);
			if (_c_scalar_type.attr_list != null)
				this.visit((dynamic)_c_scalar_type.attr_list);
			if (_c_scalar_type.attributes != null)
				this.visit((dynamic)_c_scalar_type.attributes);
		}
		public override void visit(c_module _c_module)
		{
			executer.visit(_c_module);
			if (_c_module.defs != null)
				this.visit((dynamic)_c_module.defs);
			if (_c_module.used_units != null)
				this.visit((dynamic)_c_module.used_units);
			if (_c_module.compiler_directives != null)
			foreach (dynamic x in _c_module.compiler_directives)
				if(x != null)
					this.visit(x);
		}
		public override void visit(declarations_as_statement _declarations_as_statement)
		{
			executer.visit(_declarations_as_statement);
			if (_declarations_as_statement.defs != null)
				this.visit((dynamic)_declarations_as_statement.defs);
			if (_declarations_as_statement.attributes != null)
				this.visit((dynamic)_declarations_as_statement.attributes);
		}
		public override void visit(array_size _array_size)
		{
			executer.visit(_array_size);
			if (_array_size.max_value != null)
				this.visit((dynamic)_array_size.max_value);
			if (_array_size.attr_list != null)
				this.visit((dynamic)_array_size.attr_list);
			if (_array_size.attributes != null)
				this.visit((dynamic)_array_size.attributes);
		}
		public override void visit(enumerator _enumerator)
		{
			executer.visit(_enumerator);
			if (_enumerator.name != null)
				this.visit((dynamic)_enumerator.name);
			if (_enumerator.value != null)
				this.visit((dynamic)_enumerator.value);
		}
		public override void visit(enumerator_list _enumerator_list)
		{
			executer.visit(_enumerator_list);
			if (_enumerator_list.enumerators != null)
			foreach (dynamic x in _enumerator_list.enumerators)
				if(x != null)
					this.visit(x);
		}
		public override void visit(c_for_cycle _c_for_cycle)
		{
			executer.visit(_c_for_cycle);
			if (_c_for_cycle.expr1 != null)
				this.visit((dynamic)_c_for_cycle.expr1);
			if (_c_for_cycle.expr2 != null)
				this.visit((dynamic)_c_for_cycle.expr2);
			if (_c_for_cycle.expr3 != null)
				this.visit((dynamic)_c_for_cycle.expr3);
			if (_c_for_cycle.stmt != null)
				this.visit((dynamic)_c_for_cycle.stmt);
			if (_c_for_cycle.attributes != null)
				this.visit((dynamic)_c_for_cycle.attributes);
		}
		public override void visit(switch_stmt _switch_stmt)
		{
			executer.visit(_switch_stmt);
			if (_switch_stmt.condition != null)
				this.visit((dynamic)_switch_stmt.condition);
			if (_switch_stmt.stmt != null)
				this.visit((dynamic)_switch_stmt.stmt);
			if (_switch_stmt.attributes != null)
				this.visit((dynamic)_switch_stmt.attributes);
		}
		public override void visit(type_definition_attr_list _type_definition_attr_list)
		{
			executer.visit(_type_definition_attr_list);
			if (_type_definition_attr_list.attributes != null)
			foreach (dynamic x in _type_definition_attr_list.attributes)
				if(x != null)
					this.visit(x);
		}
		public override void visit(type_definition_attr _type_definition_attr)
		{
			executer.visit(_type_definition_attr);
			if (_type_definition_attr.attr_list != null)
				this.visit((dynamic)_type_definition_attr.attr_list);
			if (_type_definition_attr.attributes != null)
				this.visit((dynamic)_type_definition_attr.attributes);
		}
		public override void visit(lock_stmt _lock_stmt)
		{
			executer.visit(_lock_stmt);
			if (_lock_stmt.lock_object != null)
				this.visit((dynamic)_lock_stmt.lock_object);
			if (_lock_stmt.stmt != null)
				this.visit((dynamic)_lock_stmt.stmt);
			if (_lock_stmt.attributes != null)
				this.visit((dynamic)_lock_stmt.attributes);
		}
		public override void visit(compiler_directive_list _compiler_directive_list)
		{
			executer.visit(_compiler_directive_list);
			if (_compiler_directive_list.directives != null)
			foreach (dynamic x in _compiler_directive_list.directives)
				if(x != null)
					this.visit(x);
			if (_compiler_directive_list.Name != null)
				this.visit((dynamic)_compiler_directive_list.Name);
			if (_compiler_directive_list.Directive != null)
				this.visit((dynamic)_compiler_directive_list.Directive);
		}
		public override void visit(compiler_directive_if _compiler_directive_if)
		{
			executer.visit(_compiler_directive_if);
			if (_compiler_directive_if.if_part != null)
				this.visit((dynamic)_compiler_directive_if.if_part);
			if (_compiler_directive_if.elseif_part != null)
				this.visit((dynamic)_compiler_directive_if.elseif_part);
			if (_compiler_directive_if.Name != null)
				this.visit((dynamic)_compiler_directive_if.Name);
			if (_compiler_directive_if.Directive != null)
				this.visit((dynamic)_compiler_directive_if.Directive);
		}
		public override void visit(documentation_comment_list _documentation_comment_list)
		{
			executer.visit(_documentation_comment_list);
			if (_documentation_comment_list.sections != null)
			foreach (dynamic x in _documentation_comment_list.sections)
				if(x != null)
					this.visit(x);
		}
		public override void visit(documentation_comment_tag _documentation_comment_tag)
		{
			executer.visit(_documentation_comment_tag);
			if (_documentation_comment_tag.parameters != null)
			foreach (dynamic x in _documentation_comment_tag.parameters)
				if(x != null)
					this.visit(x);
		}
		public override void visit(documentation_comment_tag_param _documentation_comment_tag_param)
		{
			executer.visit(_documentation_comment_tag_param);
		}
		public override void visit(documentation_comment_section _documentation_comment_section)
		{
			executer.visit(_documentation_comment_section);
			if (_documentation_comment_section.tags != null)
			foreach (dynamic x in _documentation_comment_section.tags)
				if(x != null)
					this.visit(x);
		}
		public override void visit(token_taginfo _token_taginfo)
		{
			executer.visit(_token_taginfo);
		}
		public override void visit(declaration_specificator _declaration_specificator)
		{
			executer.visit(_declaration_specificator);
			if (_declaration_specificator.attr_list != null)
				this.visit((dynamic)_declaration_specificator.attr_list);
			if (_declaration_specificator.attributes != null)
				this.visit((dynamic)_declaration_specificator.attributes);
		}
		public override void visit(ident_with_templateparams _ident_with_templateparams)
		{
			executer.visit(_ident_with_templateparams);
			if (_ident_with_templateparams.name != null)
				this.visit((dynamic)_ident_with_templateparams.name);
			if (_ident_with_templateparams.template_params != null)
				this.visit((dynamic)_ident_with_templateparams.template_params);
			if (_ident_with_templateparams.attributes != null)
				this.visit((dynamic)_ident_with_templateparams.attributes);
		}
		public override void visit(template_type_name _template_type_name)
		{
			executer.visit(_template_type_name);
			if (_template_type_name.template_args != null)
				this.visit((dynamic)_template_type_name.template_args);
			if (_template_type_name.attributes != null)
				this.visit((dynamic)_template_type_name.attributes);
		}
		public override void visit(default_operator _default_operator)
		{
			executer.visit(_default_operator);
			if (_default_operator.type_name != null)
				this.visit((dynamic)_default_operator.type_name);
			if (_default_operator.attributes != null)
				this.visit((dynamic)_default_operator.attributes);
		}
		public override void visit(bracket_expr _bracket_expr)
		{
			executer.visit(_bracket_expr);
			if (_bracket_expr.expr != null)
				this.visit((dynamic)_bracket_expr.expr);
			if (_bracket_expr.attributes != null)
				this.visit((dynamic)_bracket_expr.attributes);
		}
		public override void visit(attribute _attribute)
		{
			executer.visit(_attribute);
			if (_attribute.qualifier != null)
				this.visit((dynamic)_attribute.qualifier);
			if (_attribute.type != null)
				this.visit((dynamic)_attribute.type);
			if (_attribute.arguments != null)
				this.visit((dynamic)_attribute.arguments);
		}
		public override void visit(simple_attribute_list _simple_attribute_list)
		{
			executer.visit(_simple_attribute_list);
			if (_simple_attribute_list.attributes != null)
			foreach (dynamic x in _simple_attribute_list.attributes)
				if(x != null)
					this.visit(x);
		}
		public override void visit(attribute_list _attribute_list)
		{
			executer.visit(_attribute_list);
			if (_attribute_list.attributes != null)
			foreach (dynamic x in _attribute_list.attributes)
				if(x != null)
					this.visit(x);
		}
		public override void visit(function_lambda_definition _function_lambda_definition)
		{
			executer.visit(_function_lambda_definition);
			if (_function_lambda_definition.ident_list != null)
				this.visit((dynamic)_function_lambda_definition.ident_list);
			if (_function_lambda_definition.return_type != null)
				this.visit((dynamic)_function_lambda_definition.return_type);
			if (_function_lambda_definition.formal_parameters != null)
				this.visit((dynamic)_function_lambda_definition.formal_parameters);
			if (_function_lambda_definition.proc_body != null)
				this.visit((dynamic)_function_lambda_definition.proc_body);
			if (_function_lambda_definition.proc_definition != null)
				this.visit((dynamic)_function_lambda_definition.proc_definition);
			if (_function_lambda_definition.parameters != null)
				this.visit((dynamic)_function_lambda_definition.parameters);
			if (_function_lambda_definition.defs != null)
			foreach (dynamic x in _function_lambda_definition.defs)
				if(x != null)
					this.visit(x);
			if (_function_lambda_definition.attributes != null)
				this.visit((dynamic)_function_lambda_definition.attributes);
		}
		public override void visit(function_lambda_call _function_lambda_call)
		{
			executer.visit(_function_lambda_call);
			if (_function_lambda_call.f_lambda_def != null)
				this.visit((dynamic)_function_lambda_call.f_lambda_def);
			if (_function_lambda_call.parameters != null)
				this.visit((dynamic)_function_lambda_call.parameters);
			if (_function_lambda_call.attributes != null)
				this.visit((dynamic)_function_lambda_call.attributes);
		}
		public override void visit(semantic_check _semantic_check)
		{
			executer.visit(_semantic_check);
			if (_semantic_check.param != null)
			foreach (dynamic x in _semantic_check.param)
				if(x != null)
					this.visit(x);
			if (_semantic_check.attributes != null)
				this.visit((dynamic)_semantic_check.attributes);
		}
		public override void visit(lambda_inferred_type _lambda_inferred_type)
		{
			executer.visit(_lambda_inferred_type);
			if (_lambda_inferred_type.attr_list != null)
				this.visit((dynamic)_lambda_inferred_type.attr_list);
			if (_lambda_inferred_type.attributes != null)
				this.visit((dynamic)_lambda_inferred_type.attributes);
		}
		public override void visit(same_type_node _same_type_node)
		{
			executer.visit(_same_type_node);
			if (_same_type_node.ex != null)
				this.visit((dynamic)_same_type_node.ex);
			if (_same_type_node.attr_list != null)
				this.visit((dynamic)_same_type_node.attr_list);
			if (_same_type_node.attributes != null)
				this.visit((dynamic)_same_type_node.attributes);
		}
	};
}
