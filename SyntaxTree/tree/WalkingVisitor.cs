using System;
using System.Collections.Generic;


namespace PascalABCCompiler.SyntaxTree
{

    //Два варианта использования визитора:
    //1. Создать экземпляр этого класса, привязать к OnEnter и OnLeave обработчики посещений узла
    //и запустить обход. Визитор обойдет дерево и для каждого узла вызовет эти обработчики.
    //
    //2. Унаследовать от этого класса свой класс, в котором переорпеделить обработку нужных узлов.
    //Не забывать при этом о вызове visit'ов для подузлов (или base.visit), если это нужно.

	public delegate void VisitorDelegate(syntax_tree_node node);

    public class WalkingVisitor : AbstractVisitor
    {

		protected VisitorDelegate OnEnter;
        protected VisitorDelegate OnLeave;

        protected bool visitNode = true; // в OnEnter можно сделать false

        public virtual void ProcessNode(syntax_tree_node Node)
        {
            if (Node != null)
            {
                if (OnEnter != null)
                    OnEnter(Node);

                if (visitNode)
                    Node.visit(this);
                else visitNode = true;

                if (OnLeave != null)
                    OnLeave(Node);
            }
        }

		//0 
		public override void visit(default_operator _default_operator)
        {
			ProcessNode(_default_operator.type_name);
        }
        public override void visit(template_type_name _template_type_name)
        {
			ProcessNode(_template_type_name.template_args);
        }
        public override void visit(ident_with_templateparams _ident_with_templateparams)
        {
			ProcessNode(_ident_with_templateparams.template_params);
        }
        public override void visit(declaration_specificator _declaration_specificator)
        {
			ProcessNode(_declaration_specificator.attr_list);
        }
        public override void visit(token_taginfo _token_taginfo)
        {
        }
        public override void visit(documentation_comment_section _documentation_comment_section)
        {
			if (_documentation_comment_section.tags != null)
				for (int i = 0; i < _documentation_comment_section.tags.Count; ++i)
					ProcessNode(_documentation_comment_section.tags[i]);
        }
        public override void visit(documentation_comment_tag_param _documentation_comment_tag_param)
        {            
        }
        public override void visit(documentation_comment_tag _documentation_comment_tag)
        {
			if (_documentation_comment_tag.parameters != null)
				for (int i = 0; i < _documentation_comment_tag.parameters.Count; ++i)
					ProcessNode(_documentation_comment_tag.parameters[i]);
        }
        public override void visit(documentation_comment_list _documentation_comment_list)
        {
			if (_documentation_comment_list.sections != null)
				for (int i = 0; i < _documentation_comment_list.sections.Count; ++i)
					ProcessNode(_documentation_comment_list.sections[i]);
        }
        public override void visit(compiler_directive_if _compiler_directive_if)
        {
			ProcessNode(_compiler_directive_if.Directive);
			ProcessNode(_compiler_directive_if.Name);
			ProcessNode(_compiler_directive_if.if_part);
			ProcessNode(_compiler_directive_if.elseif_part);
        }                     //10
        public override void visit(compiler_directive_list _compiler_directive_list)
        {
            ProcessNode(_compiler_directive_list.Directive);
			ProcessNode(_compiler_directive_list.Name);
			ProcessNode(_compiler_directive_list.Directive);
			if (_compiler_directive_list.directives != null)
				for (int i = 0; i < _compiler_directive_list.directives.Count; ++i)
					ProcessNode(_compiler_directive_list.directives[i]);
        }
        public override void visit(lock_stmt _lock_stmt)
        {
			ProcessNode(_lock_stmt.stmt);
			ProcessNode(_lock_stmt.lock_object);
        }
        public override void visit(type_definition_attr _type_definition_attr)
        {
             ProcessNode(_type_definition_attr.attr_list);
        }
        public override void visit(type_definition_attr_list _type_definition_attr_list)
        {
			if (_type_definition_attr_list.attributes != null)
				for (int i = 0; i < _type_definition_attr_list.attributes.Count; ++i )
					ProcessNode(_type_definition_attr_list.attributes[i]);
        }
        public override void visit(switch_stmt _switch_stmt)
        {
			ProcessNode(_switch_stmt.condition);
			ProcessNode(_switch_stmt.stmt);
        }
        public override void visit(c_for_cycle _c_for_cycle)
        {
			ProcessNode(_c_for_cycle.expr1);
			ProcessNode(_c_for_cycle.expr2);
			ProcessNode(_c_for_cycle.expr3);
			ProcessNode(_c_for_cycle.stmt);
        }
        public override void visit(enumerator_list _enumerator_list)
        {
			if (_enumerator_list.enumerators != null)
				for (int i = 0; i < _enumerator_list.enumerators.Count; ++i)
					ProcessNode(_enumerator_list.enumerators[i]);
        }
        public override void visit(enumerator _enumerator)
        {
			ProcessNode(_enumerator.name);
			ProcessNode(_enumerator.value);
        }
        public override void visit(array_size _array_size)
        {
			ProcessNode(_array_size.attr_list);
			ProcessNode(_array_size.max_value);
        }                                           //20
        public override void visit(declarations_as_statement _declarations_as_statement)
        {
			ProcessNode(_declarations_as_statement.defs);
        }
        public override void visit(c_module _c_module)
        {
			if (_c_module.compiler_directives != null)
				for (int i = 0; i < _c_module.compiler_directives.Count; ++i)
					ProcessNode(_c_module.compiler_directives[i]);
			ProcessNode(_c_module.defs);
			ProcessNode(_c_module.used_units);
        }
        public override void visit(c_scalar_type _c_scalar_type)
        {
			ProcessNode(_c_scalar_type.attr_list);
        }
        public override void visit(expression_as_statement _expression_as_statement)
        {
			ProcessNode(_expression_as_statement.expr);
        }
        public override void visit(question_colon_expression _question_colon_expression)
        {
			ProcessNode(_question_colon_expression.condition);
			ProcessNode(_question_colon_expression.ret_if_true);
			ProcessNode(_question_colon_expression.ret_if_false);
        }
        public override void visit(var_statement _var_statement)
        {
			ProcessNode(_var_statement.var_def);
        }
        public override void visit(operator_name_ident _operator_name_ident)
        {
        }
        public override void visit(compiler_directive _compiler_directive)
        {
			ProcessNode(_compiler_directive.Directive);
			ProcessNode(_compiler_directive.Name);
        }
        public override void visit(typeof_operator _typeof_operator)
        {
			ProcessNode(_typeof_operator.type_name);
        }
        public override void visit(sizeof_operator _sizeof_operator)
        {
			ProcessNode(_sizeof_operator.expr);
			ProcessNode(_sizeof_operator.type_def);
        }                                 //30
        public override void visit(where_definition_list _where_definition_list)
        {
			if (_where_definition_list.defs != null)
				for (int i = 0; i < _where_definition_list.defs.Count; ++i)
					ProcessNode(_where_definition_list.defs[i]);
        }
        public override void visit(where_definition _where_definition)
        {
			ProcessNode(_where_definition.names);
			ProcessNode(_where_definition.types);
        }
        public override void visit(type_definition_list _type_definition_list)
        {
			if (_type_definition_list.defs != null)
				for (int i = 0; i < _type_definition_list.defs.Count; ++i)
					ProcessNode(_type_definition_list.defs[i]);
        }
        public override void visit(new_expr _new_expr)
        {
			ProcessNode(_new_expr.type);
			ProcessNode(_new_expr.params_list);
        }
        public override void visit(uint64_const _uint64_const)
        {           
        }
        public override void visit(int64_const _int64_const)
        {
        }
        public override void visit(template_type_reference _template_type_reference)
        {
			ProcessNode(_template_type_reference.attr_list);
			ProcessNode(_template_type_reference.name);
			if (_template_type_reference.names != null)
				for (int i = 0; i < _template_type_reference.names.Count; ++i)
					ProcessNode(_template_type_reference.names[i]);
			ProcessNode(_template_type_reference.params_list);
        }
        public override void visit(template_param_list _template_param_list)
        {
			ProcessNode(_template_param_list.dereferencing_value);
			if (_template_param_list.params_list != null)
				for (int i = 0; i < _template_param_list.params_list.Count; ++i)
					ProcessNode(_template_param_list.params_list[i]);
        }
        public override void visit(named_type_reference_list _named_type_reference_list)
        {
			if (_named_type_reference_list.types != null)
				for (int i = 0; i < _named_type_reference_list.types.Count; ++i)
					ProcessNode(_named_type_reference_list.types[i]);
        }
        public override void visit(addressed_value_funcname _addressed_value_funcname)
        {
        }               //40
        public override void visit(foreach_stmt _foreach_stmt)
        {
			ProcessNode(_foreach_stmt.identifier);
			ProcessNode(_foreach_stmt.in_what);
			ProcessNode(_foreach_stmt.stmt);
			ProcessNode(_foreach_stmt.type_name);
        }
        public override void visit(loop_stmt _loop_stmt)
        {
			ProcessNode(_loop_stmt.stmt);
        }
        public override void visit(oberon_withstmt _oberon_withstmt)
        {
			ProcessNode(_oberon_withstmt.quardstat_list);
			ProcessNode(_oberon_withstmt.else_stmt);
        }
        public override void visit(oberon_withstmt_guardstat_list _oberon_withstmt_guardstat_list)
        {
			if (_oberon_withstmt_guardstat_list.guardstats != null)
				for (int i = 0; i < _oberon_withstmt_guardstat_list.guardstats.Count; ++i)
					ProcessNode(_oberon_withstmt_guardstat_list.guardstats[i]);
        }
        public override void visit(oberon_withstmt_guardstat _oberon_withstmt_guardstat)
        {
			ProcessNode(_oberon_withstmt_guardstat.name);
			ProcessNode(_oberon_withstmt_guardstat.stmt);
			ProcessNode(_oberon_withstmt_guardstat.type_name);
        }
        public override void visit(oberon_procedure_header _oberon_procedure_header)
        {
			ProcessNode(_oberon_procedure_header.attr_list);
			ProcessNode(_oberon_procedure_header.first_name);
			ProcessNode(_oberon_procedure_header.name);
			ProcessNode(_oberon_procedure_header.parameters);
			ProcessNode(_oberon_procedure_header.proc_attributes);
			ProcessNode(_oberon_procedure_header.receiver);
			ProcessNode(_oberon_procedure_header.return_type);
			ProcessNode(_oberon_procedure_header.second_name);
			ProcessNode(_oberon_procedure_header.template_args);
			ProcessNode(_oberon_procedure_header.where_defs);
        }
        public override void visit(oberon_procedure_receiver _oberon_procedure_receiver)
        {
			ProcessNode(_oberon_procedure_receiver.receiver_name);
			ProcessNode(_oberon_procedure_receiver.receiver_typename);      
        }
        public override void visit(jump_stmt _jump_stmt)
        {
			ProcessNode(_jump_stmt.expr);
        }
        public override void visit(oberon_exit_stmt _oberon_exit_stmt)
        {
        }
        public override void visit(oberon_ident_with_export_marker _oberon_ident_with_export_marker)
        {           
        } //50
        public override void visit(oberon_module _oberon_module)
        {
			ProcessNode(_oberon_module.first_name);
			ProcessNode(_oberon_module.second_name);
			if (_oberon_module.compiler_directives != null)
				for (int i = 0; i < _oberon_module.compiler_directives.Count; ++i)
					ProcessNode(_oberon_module.compiler_directives[i]);
			ProcessNode(_oberon_module.import_list);
			ProcessNode(_oberon_module.definitions);
			ProcessNode(_oberon_module.module_code);        
        }
        public override void visit(oberon_import_module _oberon_import_module)
        {
			ProcessNode(_oberon_import_module.name);
			ProcessNode(_oberon_import_module.new_name);
        }
        public override void visit(using_list _using_list)
        {
			if (_using_list.namespaces != null)
				for (int i = 0; i < _using_list.namespaces.Count; ++i)
					ProcessNode(_using_list.namespaces[i]);
        }
        public override void visit(external_directive _external_directive)
        {
			ProcessNode(_external_directive.name);
			ProcessNode(_external_directive.modulename);
        }
        public override void visit(inherited_message _inherited_message)
        {
        }
        public override void visit(try_stmt _try_stmt)
        {
			ProcessNode(_try_stmt.stmt_list);
			ProcessNode(_try_stmt.handler);
        }
        public override void visit(try_handler_except _try_handler_except)
        {
			ProcessNode(_try_handler_except.except_block);
        }
        public override void visit(try_handler_finally _try_handler_finally)
        {
			ProcessNode(_try_handler_finally.stmt_list);
        }
        public override void visit(try_handler _try_handler)
        {
        }
        public override void visit(exception_block _exception_block)
        {
			ProcessNode(_exception_block.stmt_list);
			ProcessNode(_exception_block.else_stmt_list);
			ProcessNode(_exception_block.handlers);
        }                                 //60 
        public override void visit(exception_handler_list _exception_handler_list)
        {
			if (_exception_handler_list.handlers != null)
				for (int i = 0; i < _exception_handler_list.handlers.Count; ++i)
					ProcessNode(_exception_handler_list.handlers[i]);
        }
        public override void visit(exception_ident _exception_ident)
        {
			ProcessNode(_exception_ident.type_name);
			ProcessNode(_exception_ident.variable);
        }
        public override void visit(exception_handler _exception_handler)
        {
            ProcessNode(_exception_handler.type_name);
            ProcessNode(_exception_handler.variable);
			ProcessNode(_exception_handler.statements);
        }
        public override void visit(known_type_ident _known_type_ident)
        {
        }
        public override void visit(file_type _file_type)
        {
			ProcessNode(_file_type.file_of_type);
			ProcessNode(_file_type.attr_list);
        }
        public override void visit(op_type_node _op_type_node)
        {
        }
        public override void visit(raise_stmt _raise_stmt)
        {
			ProcessNode(_raise_stmt.address);
			ProcessNode(_raise_stmt.expr);
        }
        public override void visit(token_info _token_info)
        {
        }
        public override void visit(initfinal_part _initfinal_part)
        {
			ProcessNode(_initfinal_part.initialization_sect);
			ProcessNode(_initfinal_part.finalization_sect);
        }
        public override void visit(format_expr _format_expr)
        {
			ProcessNode(_format_expr.expr);
			ProcessNode(_format_expr.format1);
			ProcessNode(_format_expr.format2);
        }                                         //70
        public override void visit(inherited_ident _inherited_ident)
        {
        }
        public override void visit(property_parameter_list _property_parameter_list)
        {
			if (_property_parameter_list.parameters != null)
				for (int i = 0; i < _property_parameter_list.parameters.Count; ++i)
					ProcessNode(_property_parameter_list.parameters[i]);
        }
        public override void visit(property_parameter _property_parameter)
        {
			ProcessNode(_property_parameter.names);
			ProcessNode(_property_parameter.type);
        }
        public override void visit(property_interface _property_interface)
        {
			ProcessNode(_property_interface.index_expression);
			ProcessNode(_property_interface.parameter_list);
			ProcessNode(_property_interface.property_type);
        }
        public override void visit(property_array_default _property_array_default)
        {
        }
        public override void visit(record_type_parts _record_type_parts)
        {
			ProcessNode(_record_type_parts.fixed_part);
			ProcessNode(_record_type_parts.variant_part);
        }
        public override void visit(var_def_list _var_def_list)
        {
			if (_var_def_list.vars != null)
				for (int i = 0; i < _var_def_list.vars.Count; ++i)
					ProcessNode(_var_def_list.vars[i]);
        }
        public override void visit(diapason_expr _diapason_expr)
        {
			ProcessNode(_diapason_expr.left);
			ProcessNode(_diapason_expr.right);
        }
        public override void visit(case_variants _case_variants)
        {
			if (_case_variants.variants != null)
				for (int i = 0; i < _case_variants.variants.Count; ++i)
					ProcessNode(_case_variants.variants[i]);
        }
        public override void visit(literal _literal)
        {
        }                                                 //80 
        public override void visit(array_of_const_type_definition _array_of_const_type_definition)
        {
			ProcessNode(_array_of_const_type_definition.attr_list);
        }
        public override void visit(array_of_named_type_definition _array_of_named_type_definition)
        {
			ProcessNode(_array_of_named_type_definition.type_name);
			ProcessNode(_array_of_named_type_definition.attr_list);
        }
        public override void visit(proc_block _proc_block)
        {
        }
        public override void visit(block _block)
        {
			ProcessNode(_block.defs);
			ProcessNode(_block.program_code);
        }
        public override void visit(diap_expr _diap_expr)
        {
			ProcessNode(_diap_expr.left);
			ProcessNode(_diap_expr.right);
        }
        public override void visit(implementation_node _implementation_node)
        {
			ProcessNode(_implementation_node.uses_modules);
            ProcessNode(_implementation_node.using_namespaces);
			ProcessNode(_implementation_node.implementation_definitions);
        }
        public override void visit(interface_node _interface_node)
        {
            ProcessNode(_interface_node.uses_modules);
			ProcessNode(_interface_node.using_namespaces);
			ProcessNode(_interface_node.interface_definitions);
        }
        public override void visit(typecast_node _typecast_node)
        {
			ProcessNode(_typecast_node.expr);
			ProcessNode(_typecast_node.type_def);
        }
        public override void visit(inherited_method_call _inherited_method_call)
        {
            ProcessNode(_inherited_method_call.method_name);
			ProcessNode(_inherited_method_call.exprs);
        }
        public override void visit(destructor _destructor)
        {
			ProcessNode(_destructor.attr_list);
			ProcessNode(_destructor.name);
			ProcessNode(_destructor.parameters);
			ProcessNode(_destructor.proc_attributes);
			ProcessNode(_destructor.template_args);
			ProcessNode(_destructor.where_defs);
        }                                           //90
        public override void visit(constructor _constructor)
        {
			ProcessNode(_constructor.attr_list);
			ProcessNode(_constructor.name);
			ProcessNode(_constructor.parameters);
			ProcessNode(_constructor.proc_attributes);
			ProcessNode(_constructor.template_args);
			ProcessNode(_constructor.where_defs);
        }
        public override void visit(file_type_definition _file_type_definition)
        {
            ProcessNode(_file_type_definition.elem_type);
			ProcessNode(_file_type_definition.attr_list);
        }
        public override void visit(nil_const _nil_const)
        {
        }
        public override void visit(class_predefinition _class_predefinition)
        {
			ProcessNode(_class_predefinition.class_name);
            ProcessNode(_class_predefinition.type_def);
			ProcessNode(_class_predefinition.type_name);
        }
        public override void visit(procedure_call _procedure_call)
        {
			ProcessNode(_procedure_call.func_name);
        }
        public override void visit(variant_record_type _variant_record_type)
        {
			ProcessNode(_variant_record_type.var_name);
            ProcessNode(_variant_record_type.var_type);
			ProcessNode(_variant_record_type.vars);
        }
        public override void visit(variant_types _variant_types)
        {
			if (_variant_types.vars != null)
				for (int i = 0; i < _variant_types.vars.Count; ++i)
					ProcessNode(_variant_types.vars[i]);
        }
        public override void visit(variant_type _variant_type)
        {
			ProcessNode(_variant_type.case_exprs);
			ProcessNode(_variant_type.parts);
        }
        public override void visit(variant_list _variant_list)
        {
			if (_variant_list.vars != null)
				for (int i = 0; i < _variant_list.vars.Count; ++i)
					ProcessNode(_variant_list.vars[i]);
        }
        public override void visit(variant _variant)
        {
			ProcessNode(_variant.vars);
			ProcessNode(_variant.vars_type);
        }                                                 //100
        public override void visit(string_num_definition _string_num_definition)
        {
			ProcessNode(_string_num_definition.attr_list);
			ProcessNode(_string_num_definition.name);
			ProcessNode(_string_num_definition.num_of_symbols);
        }
        public override void visit(literal_const_line _literal_const_line)
        {
			if (_literal_const_line.literals != null)
				for (int i = 0; i < _literal_const_line.literals.Count; ++i)
					ProcessNode(_literal_const_line.literals[i]);
        }
        public override void visit(sharp_char_const _sharp_char_const)
        {
        }
        public override void visit(raise_statement _raise_statement)
        {
			ProcessNode(_raise_statement.excep);
        }
        public override void visit(char_const _char_const)
        {
        }
        public override void visit(enum_type_definition _enum_type_definition)
        {
			ProcessNode(_enum_type_definition.enumerators);
			ProcessNode(_enum_type_definition.attr_list);
        }
        public override void visit(record_type _record_type)
        {
			ProcessNode(_record_type.base_type);
            ProcessNode(_record_type.parts);
			ProcessNode(_record_type.attr_list);
        }
        public override void visit(record_const _record_const)
        {
			if (_record_const.rec_consts != null)
				for (int i = 0; i < _record_const.rec_consts.Count; ++i)
					ProcessNode(_record_const.rec_consts[i]);
        }
        public override void visit(record_const_definition _record_const_definition)
        {
			ProcessNode(_record_const_definition.name);
			ProcessNode(_record_const_definition.val);
        }
        public override void visit(try_except_statement _try_except_statement)
        {
			ProcessNode(_try_except_statement.statements);
            ProcessNode(_try_except_statement.on_except);
			ProcessNode(_try_except_statement.else_statements);
        }                       //110
        public override void visit(try_finally_statement _try_finally_statement)
        {
			ProcessNode(_try_finally_statement.statements);
			ProcessNode(_try_finally_statement.finally_statements);
        }
        public override void visit(on_exception_list _on_exception_list)
        {
			if (_on_exception_list.on_exceptions != null)
				for (int i = 0; i < _on_exception_list.on_exceptions.Count; ++i)
					ProcessNode(_on_exception_list.on_exceptions[i]);
        }
        public override void visit(on_exception _on_exception)
        {
			ProcessNode(_on_exception.exception_var_name);
            ProcessNode(_on_exception.exception_type_name);
			ProcessNode(_on_exception.stat);
        }
        public override void visit(try_statement _try_statement)
        {
			ProcessNode(_try_statement.statements);
        }
        public override void visit(set_type_definition _set_type_definition)
        {
			ProcessNode(_set_type_definition.attr_list);
			ProcessNode(_set_type_definition.of_type);
        }
        public override void visit(known_type_definition _known_type_definition)
        {
			ProcessNode(_known_type_definition.attr_list);
			ProcessNode(_known_type_definition.unit_name);
        }
        public override void visit(default_indexer_property_node _default_indexer_property_node)
        {
        }
        public override void visit(class_definition _class_definition)
        {
			ProcessNode(_class_definition.attr_list);
			ProcessNode(_class_definition.body);
			ProcessNode(_class_definition.class_parents);
			ProcessNode(_class_definition.template_args);
			ProcessNode(_class_definition.where_section);
        }
        public override void visit(class_body _class_body)
        {
			if (_class_body.class_def_blocks != null)
				for (int i = 0; i < _class_body.class_def_blocks.Count; ++i)
					ProcessNode(_class_body.class_def_blocks[i]);
        }
        public override void visit(access_modifer_node _access_modifer_node)
        {
        }                         //120
        public override void visit(class_members _class_members)
        {
			ProcessNode(_class_members.access_mod);
			if (_class_members.members != null)
				for (int i = 0; i < _class_members.members.Count; ++i)
					ProcessNode(_class_members.members[i]);
        }
        public override void visit(index_property _index_property)
        {
			ProcessNode(_index_property.accessors);
            ProcessNode(_index_property.array_default);
            ProcessNode(_index_property.index_expression);
            ProcessNode(_index_property.is_default);
            ProcessNode(_index_property.parameter_list);
            ProcessNode(_index_property.property_name);
            ProcessNode(_index_property.property_parametres);
			ProcessNode(_index_property.property_type);
        }
        public override void visit(simple_property _simple_property)
        {
            ProcessNode(_simple_property.accessors);
            ProcessNode(_simple_property.array_default);
            ProcessNode(_simple_property.index_expression);
            ProcessNode(_simple_property.parameter_list);
            ProcessNode(_simple_property.property_name);
			ProcessNode(_simple_property.property_type);
        }
        public override void visit(property_accessors _property_accessors)
        {
			ProcessNode(_property_accessors.read_accessor);
			ProcessNode(_property_accessors.write_accessor);
        }
        public override void visit(read_accessor_name _read_accessor_name)
        {
			ProcessNode(_read_accessor_name.accessor_name);
        }
        public override void visit(write_accessor_name _write_accessor_name)
        {
			ProcessNode(_write_accessor_name.accessor_name);
        }
        public override void visit(array_const _array_const)
        {
			ProcessNode(_array_const.elements);
        }
        public override void visit(pascal_set_constant _pascal_set_constant)
        {
			ProcessNode(_pascal_set_constant.values);
        }
        public override void visit(method_call _method_call)
        {
			ProcessNode(_method_call.dereferencing_value);
			ProcessNode(_method_call.parameters);
        }
        public override void visit(with_statement _with_statement)
        {
			ProcessNode(_with_statement.do_with);
			ProcessNode(_with_statement.what_do);
        }                                   //130
        public override void visit(labeled_statement _labeled_statement)
        {
			ProcessNode(_labeled_statement.label_name);
			ProcessNode(_labeled_statement.to_statement);
        }
        public override void visit(goto_statement _goto_statement)
        {
			ProcessNode(_goto_statement.label);
        }
        public override void visit(empty_statement _empty_statement)
        {
        }
        public override void visit(dot_node _dot_node)
        {
			ProcessNode(_dot_node.left);
			ProcessNode(_dot_node.right);
        }
        public override void visit(method_name _method_name)
        {
            ProcessNode(_method_name.class_name);
			ProcessNode(_method_name.explicit_interface_name);
            ProcessNode(_method_name.meth_name);
        }
        public override void visit(case_node _case_node)
        {
			ProcessNode(_case_node.param);
            ProcessNode(_case_node.conditions);
			ProcessNode(_case_node.else_statement);
        }
        public override void visit(case_variant _case_variant)
        {
			ProcessNode(_case_variant.conditions);
			ProcessNode(_case_variant.exec_if_true);
        }
        public override void visit(get_address _get_address)
        {
			ProcessNode(_get_address.address_of);
        }
        public override void visit(hex_constant _hex_constant)
        {
        }
        public override void visit(program_module _program_module)
        {
			ProcessNode(_program_module.program_name);
			ProcessNode(_program_module.used_units);
			ProcessNode(_program_module.using_namespaces);
			if (_program_module.compiler_directives != null)
				for (int i = 0; i < _program_module.compiler_directives.Count; ++i)
					ProcessNode(_program_module.compiler_directives[i]);
			ProcessNode(_program_module.program_block);
        }                                   //140
        public override void visit(unit_module _unit_module)
        {
			ProcessNode(_unit_module.unit_name);
			if (_unit_module.compiler_directives != null)
				for (int i = 0; i < _unit_module.compiler_directives.Count; ++i)
					ProcessNode(_unit_module.compiler_directives[i]);
			ProcessNode(_unit_module.interface_part);
			ProcessNode(_unit_module.implementation_part);
			ProcessNode(_unit_module.initialization_part);
            ProcessNode(_unit_module.finalization_part);

        }
        public override void visit(compilation_unit _compilation_unit)
        {
			if (_compilation_unit.compiler_directives != null)
				for (int i = 0; i < _compilation_unit.compiler_directives.Count; ++i)
					ProcessNode(_compilation_unit.compiler_directives[i]);
        }
        public override void visit(program_body _program_body)
        {
			ProcessNode(_program_body.used_units);
			ProcessNode(_program_body.using_list);
			ProcessNode(_program_body.program_definitions);
			ProcessNode(_program_body.program_code);
        }
        public override void visit(uses_list _uses_list)
        {
			if (_uses_list.units != null)
				for (int i = 0; i < _uses_list.units.Count; ++i)
					ProcessNode(_uses_list.units[i]);
        }
        public override void visit(uses_unit_in _uses_unit_in)
        {
			ProcessNode(_uses_unit_in.name);
        }
        public override void visit(unit_or_namespace _unit_or_namespace)
        {
			ProcessNode(_unit_or_namespace.name);
        }
        public override void visit(unit_name _unit_name)
        {
			ProcessNode(_unit_name.idunit_name);
        }
        public override void visit(consts_definitions_list _consts_definitions_list)
        {
			if (_consts_definitions_list.const_defs != null)
				for (int i = 0; i < _consts_definitions_list.const_defs.Count; ++i)
					ProcessNode(_consts_definitions_list.const_defs[i]);
        }
        public override void visit(const_definition _const_definition)
        {
			ProcessNode(_const_definition.const_name);
			ProcessNode(_const_definition.const_value);
        }
        public override void visit(typed_const_definition _typed_const_definition)
        {
			ProcessNode(_typed_const_definition.const_name);
            ProcessNode(_typed_const_definition.const_type);
			ProcessNode(_typed_const_definition.const_value);
        }                   //150
        public override void visit(simple_const_definition _simple_const_definition)
        {
			ProcessNode(_simple_const_definition.const_name);
			ProcessNode(_simple_const_definition.const_value);
        }
        public override void visit(type_declarations _type_declarations)
        {
			if (_type_declarations.types_decl != null)
				for (int i = 0; i < _type_declarations.types_decl.Count; ++i)
					ProcessNode(_type_declarations.types_decl[i]);
        }
        public override void visit(type_declaration _type_declaration)
        {
			ProcessNode(_type_declaration.type_name);
			ProcessNode(_type_declaration.type_def);
        }
        public override void visit(procedure_definition _procedure_definition)
        {
			ProcessNode(_procedure_definition.proc_header);
			ProcessNode(_procedure_definition.proc_body);
        }
        public override void visit(function_header _function_header)
        {
			ProcessNode(_function_header.attr_list);
			ProcessNode(_function_header.name);
			ProcessNode(_function_header.parameters);
			ProcessNode(_function_header.proc_attributes);
			ProcessNode(_function_header.return_type);
			ProcessNode(_function_header.template_args);
			ProcessNode(_function_header.where_defs);
        }
        public override void visit(procedure_header _procedure_header)
        {
			ProcessNode(_procedure_header.attr_list);
			ProcessNode(_procedure_header.name);
            ProcessNode(_procedure_header.parameters);
            ProcessNode(_procedure_header.proc_attributes);
            ProcessNode(_procedure_header.template_args);
            ProcessNode(_procedure_header.where_defs);
        }
        public override void visit(procedure_attributes_list _procedure_attributes_list)
        {
			if (_procedure_attributes_list.proc_attributes != null)
				for (int i = 0; i < _procedure_attributes_list.proc_attributes.Count; ++i)
					ProcessNode(_procedure_attributes_list.proc_attributes[i]);
        }
        public override void visit(formal_parameters _formal_parameters)
        {
            if (_formal_parameters.params_list != null)
                for (int i = 0; i < _formal_parameters.params_list.Count; ++i)
                    ProcessNode(_formal_parameters.params_list[i]);
        }
        public override void visit(typed_parameters _typed_parameters)
        {
            ProcessNode(_typed_parameters.idents);
            ProcessNode(_typed_parameters.inital_value);
            ProcessNode(_typed_parameters.vars_type);
        }
        public override void visit(procedure_attribute _procedure_attribute)
        {            
        }                         //160
        public override void visit(label_definitions _label_definitions)
        {
			ProcessNode(_label_definitions.labels);
        }
        public override void visit(array_type _array_type)
        {
			ProcessNode(_array_type.attr_list);
            ProcessNode(_array_type.elements_type);
			ProcessNode(_array_type.indexers);
        }
        public override void visit(indexers_types _indexers_types)
        {
            if (_indexers_types.attr_list != null)
                _indexers_types.attr_list.visit(this);
			if (_indexers_types.indexers != null)
				for (int i = 0; i < _indexers_types.indexers.Count; ++i)
					ProcessNode(_indexers_types.indexers[i]);
        }
        public override void visit(diapason _diapason)
        {
			ProcessNode(_diapason.attr_list);
            ProcessNode(_diapason.left);
            ProcessNode(_diapason.right);
        }
        public override void visit(ref_type _ref_type)
        {
			ProcessNode(_ref_type.attr_list) ;
			ProcessNode(_ref_type.pointed_to);
        }
        public override void visit(if_node _if_node)
        {
			ProcessNode(_if_node.condition);
            ProcessNode(_if_node.then_body);
			ProcessNode(_if_node.else_body);
        }
        public override void visit(while_node _while_node)
        {
            ProcessNode(_while_node.expr);
			ProcessNode(_while_node.statements);
        }
        public override void visit(repeat_node _repeat_node)
        {
			ProcessNode(_repeat_node.statements);
			ProcessNode(_repeat_node.expr);
        }
        public override void visit(for_node _for_node)
        {
			ProcessNode(_for_node.initial_value);
			ProcessNode(_for_node.finish_value);
			ProcessNode(_for_node.increment_value);
			ProcessNode(_for_node.loop_variable);
			ProcessNode(_for_node.type_name);
			ProcessNode(_for_node.statements);
        }
        public override void visit(indexer _indexer)
        {
			ProcessNode(_indexer.dereferencing_value);
			ProcessNode(_indexer.indexes);
        }                                                 //170
        public override void visit(roof_dereference _roof_dereference)
        {
			ProcessNode(_roof_dereference.dereferencing_value);
        }
        public override void visit(dereference _dereference)
        {
			ProcessNode(_dereference.dereferencing_value);
        }
        public override void visit(expression_list _expression_list)
        {
			if (_expression_list.expressions != null)
				for (int i = 0; i < _expression_list.expressions.Count; ++i)
					ProcessNode(_expression_list.expressions[i]);
        }
        public override void visit(string_const _string_const)
        {
        }
        public override void visit(program_name _program_name)
        {
			ProcessNode(_program_name.prog_name);
        }
        public override void visit(program_tree _program_tree)
        {
			if (_program_tree.compilation_units != null)
				for (int i = 0; i < _program_tree.compilation_units.Count; ++i)
					ProcessNode(_program_tree.compilation_units[i]);
        }
        public override void visit(declarations _declarations)
        {
			if (_declarations.defs != null)
				for (int i = 0; i < _declarations.defs.Count; ++i)
					ProcessNode(_declarations.defs[i]);
        }
        public override void visit(declaration _declaration)
        {
        }
        public override void visit(var_def_statement _var_def_statement)
        {
			ProcessNode(_var_def_statement.inital_value);
			ProcessNode(_var_def_statement.vars);
			ProcessNode(_var_def_statement.vars_type);
        }
        public override void visit(ident_list _ident_list)
        {
			if (_ident_list.idents != null)
				for (int i = 0; i < _ident_list.idents.Count; ++i)
					ProcessNode(_ident_list.idents[i]);
        }                                           //180
        public override void visit(variable_definitions _variable_definitions)
        {
			if (_variable_definitions.var_definitions != null)
				for (int i = 0; i < _variable_definitions.var_definitions.Count; ++i)
					ProcessNode(_variable_definitions.var_definitions[i]);
        }
        public override void visit(named_type_reference _named_type_reference)
        {
			ProcessNode(_named_type_reference.attr_list);
			if (_named_type_reference.names != null)
				for (int i = 0; i < _named_type_reference.names.Count; ++i)
					ProcessNode(_named_type_reference.names[i]);
        }
        public override void visit(type_definition _type_definition)
        {
			ProcessNode(_type_definition.attr_list);
        }
        public override void visit(addressed_value _addressed_value)
        {
        }
        public override void visit(ident _ident)
        {
        }
        public override void visit(subprogram_body _subprogram_body)
        {
            ProcessNode(_subprogram_body.subprogram_defs);
			ProcessNode(_subprogram_body.subprogram_code);
        }
        public override void visit(statement _statement)
        {
        }
        public override void visit(double_const _double_const)
        {
        }
        public override void visit(int32_const _int32_const)
        {
        }
        public override void visit(bool_const _bool_const)
        {
        }                                           //190
        public override void visit(const_node _const_node)
        {
        }
        public override void visit(un_expr _un_expr)
        {
            ProcessNode(_un_expr.subnode);
        }
        public override void visit(bin_expr _bin_expr)
        {
			ProcessNode(_bin_expr.left);
			ProcessNode(_bin_expr.right);
        }
        public override void visit(assign _assign)
        {
			ProcessNode(_assign.to);
			ProcessNode(_assign.from);
        }
        public override void visit(expression _expression)
        {
        }
        public override void visit(statement_list _statement_list)
        {
			ProcessNode(_statement_list.left_logical_bracket);
			if (_statement_list.subnodes != null)
				for (int i = 0; i < _statement_list.subnodes.Count; ++i)
					ProcessNode(_statement_list.subnodes[i]);
			ProcessNode(_statement_list.right_logical_bracket);
        }
        public override void visit(syntax_tree_node _syntax_tree_node)
        {
        }
		public override void visit(bracket_expr _bracket_expr)
		{
			ProcessNode(_bracket_expr.expr);
		}
        public override void visit(attribute _attribute)
        {
            ProcessNode(_attribute.arguments);
            ProcessNode(_attribute.qualifier);
            ProcessNode(_attribute.type);
        }
        public override void visit(simple_attribute_list _simple_attribute_list)
        {
            if (_simple_attribute_list.attributes != null)
                for (int i = 0; i < _simple_attribute_list.attributes.Count; ++i)
                    ProcessNode(_simple_attribute_list.attributes[i]);
        }                                           //200
        public override void visit(attribute_list _attribute_list)
        {
            if (_attribute_list.attributes != null)
                for (int i = 0; i < _attribute_list.attributes.Count; ++i)
                    ProcessNode(_attribute_list.attributes[i]);
        }
        public override void visit(function_lambda_definition _function_lambda_definition)
        {
            ProcessNode(_function_lambda_definition.attributes);
            //Непонятно, как обходить defs, так как они - object
            //if (_function_lambda_definition.defs != null)
            //    for (int i = 0; i < _function_lambda_definition.defs.Count; ++i)
            //        ProcessNode(_function_lambda_definition.defs[i]);
            ProcessNode(_function_lambda_definition.formal_parameters);
            ProcessNode(_function_lambda_definition.ident_list);
            ProcessNode(_function_lambda_definition.parameters);
            ProcessNode(_function_lambda_definition.proc_definition);
            ProcessNode(_function_lambda_definition.proc_body);
            ProcessNode(_function_lambda_definition.return_type);
        }
        public override void visit(function_lambda_call _function_lambda_call)
        {
            throw new NotImplementedException();
        }
        public override void visit(semantic_check _semantic_check)
        {

        }
		public override void visit(SyntaxTree.lambda_inferred_type lit) //lroman//
        {
        }
        public override void visit(SyntaxTree.same_type_node stn) //SSM 22/06/13//
        {
        }
        public override void visit(name_assign_expr _name_assign_expr) // SSM 27.06.13
        {
        }
        public override void visit(name_assign_expr_list _name_assign_expr_list) // SSM 27.06.13
        {
        }
        public override void visit(unnamed_type_object _unnamed_type_object) // SSM 27.06.13
        {
        }
        public override void visit(semantic_type_node stn) // SSM 
        {
        }
     
    }
 
}