
using PascalABCCompiler.SyntaxTree;
using System.Windows.Forms;
using System.Collections;

namespace VisualPascalABCPlugins
{



	class visualizator : AbstractVisitor
	{

		private System.Windows.Forms.TreeNodeCollection nodes;

		public visualizator(System.Windows.Forms.TreeNodeCollection nodes)
		{
			this.nodes=nodes;
		}

        public override void visit(syntax_tree_node _tree_node)
		{
			
		}


        private void prepare_node_with_text(syntax_tree_node subnode, string text)
		{
			if (subnode==null)
			{
				return;
			}
			TreeNode tn=new TreeNode();
			tn.Text=text;
			tn.Tag=subnode;
			string s=get_node_info.node(subnode);
			//get_async_info.node(subnode);
			//if (s!=null)
			//{
			//	if (s.EndsWith("Async"))
			//		tn.Text += "     " + s + "  ASYNCcount =" + get_async_info.AsyncCount;
			//	else
			//		tn.Text += "     " + s;

   //         }
            tn.Text += "     " + s;
            //tn.Nodes.Clear();
            visualizator vs=new visualizator(tn.Nodes);
			subnode.visit(vs);
			nodes.Add(tn);
		}

        public void prepare_node(syntax_tree_node subnode, string node_name)
		{
			if (subnode==null)
			{
				return;
			}
			prepare_node_with_text(subnode,node_name+": "+subnode.GetType().Name);
		}

		public void prepare_collection(IEnumerable inum,string collection_name)
		{
			int i=0;
            foreach (syntax_tree_node tn in inum)
			{
				prepare_node(tn,collection_name+"["+i.ToString()+"]");
				i++;
			}
		}

        public override void visit(statement_list _statement_list)
        {
            prepare_node(_statement_list.left_logical_bracket, "left_logical_bracket");
	        prepare_collection(_statement_list.subnodes,"subnodes");
            prepare_node(_statement_list.right_logical_bracket, "right_logical_bracket");
        }


        public override void visit(expression _expression)
        {
        	
        }


        public override void visit(assign _assign)
        {
	        prepare_node(_assign.to,"to");
	        prepare_node(_assign.from,"from");
        }

        public override void visit(assign_tuple _assign_tuple)
        {
            prepare_node(_assign_tuple.vars, "to");
            prepare_node(_assign_tuple.expr, "from");
        }

        public override void visit(bin_expr _bin_expr)
		{
			prepare_node(_bin_expr.left,"left");
			prepare_node(_bin_expr.right,"right");
		}


		public override void visit(un_expr _un_expr)
		{
			prepare_node(_un_expr.subnode,"subnode");
		}


		public override void visit(const_node _const_node)
		{
			
		}


		public override void visit(bool_const _bool_const)
		{
			
		}


		public override void visit(int32_const _int_const)
		{
			
		}
        public override void visit(int64_const _int_const)
        {

        }
        public override void visit(uint64_const _int_const)
        {

        }

       
		public override void visit(double_const _double_const)
		{
			
		}


		public override void visit(statement _statement)
		{
			
		}


		public override void visit(subprogram_body _subprogram_body)
		{
			prepare_node(_subprogram_body.subprogram_defs,"subprogram_defs");
			prepare_node(_subprogram_body.subprogram_code,"subprogram_code");
		}


		public override void visit(ident _ident)
		{
			
		}

        public override void visit(template_type_name node)
        {
            prepare_node(node.template_args, "template_args");
        }

		
		public override void visit(addressed_value _addressed_value)
		{
			
		}


		public override void visit(type_definition _type_definition)
		{
			
		}


		public override void visit(named_type_reference _named_type_reference)
		{
			prepare_collection(_named_type_reference.names,"names");	
		}


		public override void visit(variable_definitions _variable_definitions)
		{
			//for(int i=0;i<_variable_definitions.var_definitions.Count;i++)
			//{
			//	var_def_statement vdf=_variable_definitions.var_definitions[i];
			//	prepare_node(vdf,"var_definitions["+i.ToString()+"]");
			//}
			prepare_collection(_variable_definitions.var_definitions,"var_definitions");
		}


		public override void visit(ident_list _ident_list)
		{
			//for(int i=0;i<_ident_list.idents.Count;i++)
			//{
			//	ident id=_ident_list.idents[i];
			//	prepare_node(id,"idents["+i.ToString()+"]");
			//}
			prepare_collection(_ident_list.idents,"idents");
		}


		public override void visit(var_def_statement _var_def_statement)
		{
			prepare_node(_var_def_statement.vars,"vars");
			prepare_node(_var_def_statement.vars_type,"vars_type");
            prepare_node(_var_def_statement.inital_value, "inital_value");
		}


        public override void visit(declaration _subprogram_definition)
		{
			
		}


        public override void visit(declarations _subprogram_definitions)
		{
			//for(int i=0;i<_subprogram_definitions.defs.Count;i++)
			//{
			//	subprogram_definition sd=_subprogram_definitions.defs[i];
			//	prepare_node(sd,"defs["+i.ToString()+"]");
			//}
			prepare_collection(_subprogram_definitions.defs,"defs");
		}


		public override void visit(program_tree _program_tree)
		{
			//for(int i=0;i<_program_tree.compilation_units.Count;i++)
			//{
			//	subprogram_body sb=_program_tree.compilation_units[i];
			//	prepare_node(sb,"compilation_units["+i.ToString()+"]");
			//}
			prepare_collection(_program_tree.compilation_units,"compilation_units");
		}

		public override void visit(program_name _program_name)
		{
			prepare_node(_program_name.prog_name,"program name");
		}


		public override void visit(string_const _string_const)
		{
			
		}


		public override void visit(expression_list _expression_list)
		{
			prepare_collection(_expression_list.expressions,"expressions");
		}


		public override void visit(dereference _dereference)
		{
			prepare_node(_dereference.dereferencing_value,"dereferencing value");
		}


		public override void visit(roof_dereference _roof_dereference)
		{
			prepare_node(_roof_dereference.dereferencing_value,"dereferencing value");
		}


		public override void visit(indexer _indexer)
		{
			prepare_node(_indexer.dereferencing_value,"array");
			prepare_node(_indexer.indexes,"indexers");
		}


		public override void visit(for_node _for_node)
		{
			prepare_node(_for_node.loop_variable,"loop variable");
            prepare_node(_for_node.type_name, "type_name");
			prepare_node(_for_node.initial_value,"initial value");
			prepare_node(_for_node.finish_value,"finish_value");
			prepare_node(_for_node.increment_value,"increment_value");
			prepare_node(_for_node.statements,"body");
		}


		public override void visit(repeat_node _repeat_node)
		{
			prepare_node(_repeat_node.statements,"body");
			prepare_node(_repeat_node.expr,"condition");
		}


		public override void visit(while_node _while_node)
		{
			prepare_node(_while_node.expr,"condition");
			prepare_node(_while_node.statements,"body");
		}


		public override void visit(if_node _if_node)
		{
			prepare_node(_if_node.condition,"condition");
			prepare_node(_if_node.then_body,"then_body");
			prepare_node(_if_node.else_body,"else_body");
		}


		public override void visit(ref_type _ref_type)
		{
			prepare_node(_ref_type.pointed_to,"pointed to");
		}


		public override void visit(diapason _diapason)
		{
			prepare_node(_diapason.left,"left");
			prepare_node(_diapason.right,"right");
		}


		public override void visit(indexers_types _indexers_types)
		{
			prepare_collection(_indexers_types.indexers,"indexers");
		}


		public override void visit(array_type _array_type)
		{
			prepare_node(_array_type.elements_type,"elements type");
			prepare_node(_array_type.indexers,"indexers");
		}


		public override void visit(label_definitions _label_definitions)
		{
			prepare_node(_label_definitions.labels,"labels");
		}


		public override void visit(procedure_attribute _procedure_attribute)
		{
			
		}

		public override void visit(procedure_attributes_list _procedure_attributes_list)
		{
			prepare_collection(_procedure_attributes_list.proc_attributes,"attributes");
		}


		public override void visit(procedure_header _procedure_header)
		{
			prepare_node(_procedure_header.name,"name");
			prepare_node(_procedure_header.parameters,"parameters");
			prepare_node(_procedure_header.proc_attributes,"attributes");
            prepare_node(_procedure_header.template_args, "template_params");
            prepare_node(_procedure_header.where_defs, "where_defs");
		}


		public override void visit(function_header _function_header)
		{
			prepare_node(_function_header.name,"name");
			prepare_node(_function_header.parameters,"parameters");
			prepare_node(_function_header.proc_attributes,"attributes");
            prepare_node(_function_header.template_args, "template_params");
            prepare_node(_function_header.where_defs, "where_defs");
            prepare_node(_function_header.return_type, "return_type");
        }


		public override void visit(procedure_definition _procedure_definition)
		{
			prepare_node(_procedure_definition.proc_header,"procedure header");
			prepare_node(_procedure_definition.proc_body,"procedure body");
		}


		public override void visit(type_declaration _type_declaration)
		{
			prepare_node(_type_declaration.type_name,"type name");
			prepare_node(_type_declaration.type_def,"type definition");
		}


		public override void visit(type_declarations _type_declarations)
		{
			prepare_collection(_type_declarations.types_decl,"type definition");
		}


		public override void visit(simple_const_definition _simple_const_definition)
		{
			prepare_node(_simple_const_definition.const_name,"constant name");
			prepare_node(_simple_const_definition.const_value,"constant value");
		}


		public override void visit(typed_const_definition _typed_const_definition)
		{
			prepare_node(_typed_const_definition.const_name,"constant name");
			prepare_node(_typed_const_definition.const_type,"const type");
			prepare_node(_typed_const_definition.const_value,"constant value");
		}


		public override void visit(const_definition _const_definition)
		{
			prepare_node(_const_definition.const_name,"const name");
			prepare_node(_const_definition.const_value,"const value");
		}


		public override void visit(consts_definitions_list _consts_definitions_list)
		{
			prepare_collection(_consts_definitions_list.const_defs,"const definition");
		}


		public override void visit(unit_name _unit_name)
		{
			prepare_node(_unit_name.idunit_name,"name");
		}


        public override void visit(unit_or_namespace _uses_unit)
		{
			prepare_node(_uses_unit.name,"name");
		}


		public override void visit(uses_unit_in _uses_unit_in)
		{
			prepare_node(_uses_unit_in.name,"name");
			prepare_node(_uses_unit_in.in_file,"file name");
		}


		public override void visit(uses_list _uses_list)
		{
			prepare_collection(_uses_list.units,"uses");
		}


		public override void visit(program_body _program_body)
		{
			prepare_node(_program_body.used_units,"used units");
			prepare_node(_program_body.program_definitions,"definitions");
			prepare_node(_program_body.program_code,"program code");
		}


		public override void visit(compilation_unit _compilation_unit)
        {
            prepare_collection(_compilation_unit.compiler_directives, "compiler_directives");			
		}


		public override void visit(unit_module _unit_module)
		{
			prepare_node(_unit_module.unit_name,"unit name");
			prepare_node(_unit_module.interface_part,"interface");
			prepare_node(_unit_module.implementation_part,"implementation");
			prepare_node(_unit_module.initialization_part,"initialization");
			prepare_node(_unit_module.finalization_part,"finalization");
            prepare_collection(_unit_module.compiler_directives, "compiler_directives");
		}


		public override void visit(program_module _program_module)
		{
			prepare_node(_program_module.program_name,"program name");
			prepare_node(_program_module.used_units,"used_units");
			prepare_node(_program_module.using_namespaces,"using_namespaces");
			prepare_node(_program_module.program_block,"program block");
            prepare_collection(_program_module.compiler_directives, "compiler_directives");
        }

		public override void visit(typed_parameters _typed_parametres)
		{
			prepare_node(_typed_parametres.idents,"idents");
			prepare_node(_typed_parametres.vars_type,"type");		
			prepare_node(_typed_parametres.inital_value,"inital_value");
		}


		public override void visit(formal_parameters _formal_parametres)
		{
			prepare_collection(_formal_parametres.params_list,"parameters");
		}

		public override void visit(hex_constant _hex_constant)
		{

		}

		public override void visit(get_address _get_address)
		{
			prepare_node(_get_address.address_of,"address of");
		}

		public override void visit(case_variant _case_variant)
		{
			prepare_node(_case_variant.conditions,"conditions");
			prepare_node(_case_variant.exec_if_true,"statement");
		}

		public override void visit(case_node _case_node)
		{
			prepare_node(_case_node.param,"param");
			prepare_node(_case_node.conditions,"conditions");
			prepare_node(_case_node.else_statement,"else statement");
		}

		public override void visit(method_name _method_name)
		{
			prepare_node(_method_name.class_name,"class name");
			prepare_node(_method_name.meth_name,"method_name");
            prepare_node(_method_name.explicit_interface_name, "explicit_interface_name");
		}

		public override void visit(dot_node _dot_node)
		{
			prepare_node(_dot_node.left,"left");
			prepare_node(_dot_node.right,"right");
		}

		public override void visit(empty_statement _empty_statement)
		{
			
		}

		public override void visit(goto_statement _goto_statement)
		{
			prepare_node(_goto_statement.label,"to");
		}

		public override void visit(labeled_statement _labeled_statement)
		{
			prepare_node(_labeled_statement.label_name,"label name");
			prepare_node(_labeled_statement.to_statement,"labeled statement");
		}

		public override void visit(with_statement _with_statement)
		{
			prepare_node(_with_statement.do_with,"do with");
			prepare_node(_with_statement.what_do,"what do");
		}

		public override void visit(method_call _method_call)
		{
			prepare_node(_method_call.dereferencing_value,"dereferencing value");
			prepare_node(_method_call.parameters,"parameters");
		}

		public override void visit(pascal_set_constant _pascal_set_constant)
		{
			prepare_node(_pascal_set_constant.values,"values");	
		}

		public override void visit(array_const _array_const)
		{
			prepare_node(_array_const.elements,"elements");
		}

		public override void visit(write_accessor_name _write_accessor_name)
		{
			prepare_node(_write_accessor_name.accessor_name,"accessor name");
		}


		public override void visit(read_accessor_name _read_accessor_name)
		{
			prepare_node(_read_accessor_name.accessor_name,"accessor name");
		}


		public override void visit(property_accessors _property_accessors)
		{
			prepare_node(_property_accessors.read_accessor,"read accessor");
			prepare_node(_property_accessors.write_accessor,"write accessor");
		}


		public override void visit(simple_property _simple_property)
		{
			prepare_node(_simple_property.property_name,"property_name");
			prepare_node(_simple_property.parameter_list,"parameter_list");
			prepare_node(_simple_property.property_type,"property_type");
			prepare_node(_simple_property.index_expression,"index");
			prepare_node(_simple_property.accessors,"accessors");
			prepare_node(_simple_property.array_default,"array_default");
		}


		public override void visit(index_property _index_property)
		{
			prepare_node(_index_property.property_name,"property_name");
			prepare_node(_index_property.property_type,"property type");
			prepare_node(_index_property.index_expression,"indexers");
			prepare_node(_index_property.property_parametres,"index");
			prepare_node(_index_property.accessors,"accessors");
			prepare_node(_index_property.is_default,"default");
		}


		public override void visit(class_members _class_members)
		{
			prepare_node(_class_members.access_mod,"access_modifer");
			prepare_collection(_class_members.members,"members");
		}


		public override void visit(access_modifer_node _access_modifer_node)
		{
			
		}


		public override void visit(class_body_list _class_body)
		{
			prepare_collection(_class_body.class_def_blocks,"class definitions");
		}


		public override void visit(class_definition _class_definition)
		{
            prepare_node(_class_definition.attr_list, "attr_list");
			prepare_node(_class_definition.class_parents,"parents");
			prepare_node(_class_definition.body,"body");
            prepare_node(_class_definition.template_args, "template_args");
            prepare_node(_class_definition.where_section, "where_section");
		}

		public override void visit(default_indexer_property_node _default_indexer_property_node)
		{
			
		}

		public override void visit(known_type_definition _known_type_definition)
		{
			
		}

		public override void visit(set_type_definition _set_type_definition)
		{
			prepare_node(_set_type_definition.of_type,"of type");
		}

		public override void visit(record_const_definition _record_const_definition)
		{
			prepare_node(_record_const_definition.name,"name");
			prepare_node(_record_const_definition.val,"value");
		}

		public override void visit(record_const _record_const)
		{
			prepare_collection(_record_const.rec_consts,"consts");
		}

		public override void visit(record_type _record_type)
		{
			prepare_node(_record_type.base_type,"base_type");
			prepare_node(_record_type.parts,"parts");
		}
		public override void visit(record_type_parts _record_type_parts)
		{
			prepare_node(_record_type_parts.fixed_part,"fixed_part");
			prepare_node(_record_type_parts.variant_part,"variant_part");
		}

		public override void visit(enum_type_definition _enum_type_definition)
		{
            prepare_node(_enum_type_definition.attr_list, "attr_list");
            prepare_node(_enum_type_definition.enumerators, "enumerators");
		}

		public override void visit(char_const _char_const)
		{
			
		}

		public override void visit(raise_statement _raise_statement)
		{
			prepare_node(_raise_statement.excep,"exception");
		}

		public override void visit(sharp_char_const _sharp_char_const)
		{
			
		}

		public override void visit(literal_const_line _literal_const_line)
		{
			prepare_collection(_literal_const_line.literals,"literals");
		}

		public override void visit(string_num_definition _string_num_definition)
		{
            prepare_node(_string_num_definition.name, "name");
			prepare_node(_string_num_definition.num_of_symbols,"num_of_symbols");
		}

		public override void visit(variant _variant)
		{
			prepare_node(_variant.vars,"vars");
			prepare_node(_variant.vars_type,"vars_type");
		}

		public override void visit(variant_list _variant_list)
		{
			prepare_collection(_variant_list.vars,"vars");
		}

		public override void visit(variant_type _variant_type)
		{
			prepare_node(_variant_type.case_exprs,"case exprs");
			prepare_node(_variant_type.parts,"parts");
		}

		public override void visit(variant_types _variant_types)
		{
			prepare_collection(_variant_types.vars,"vars");
		}

		public override void visit(variant_record_type _variant_record_type)
		{
			prepare_node(_variant_record_type.var_name,"var_name");
			prepare_node(_variant_record_type.var_type,"var_type");
			prepare_node(_variant_record_type.vars,"variant types");
		}

		public override void visit(procedure_call _procedure_call)
		{
			prepare_node(_procedure_call.func_name,"func_name");
		}

		public override void visit(class_predefinition _class_predefinition)
		{
			prepare_node(_class_predefinition.class_name,"class_name");
		}

		public override void visit(nil_const _nil_const)
		{
			
		}

		public override void visit(file_type_definition _file_type_definition)
		{
			prepare_node(_file_type_definition.elem_type,"elem_type");
		}

		public override void visit(constructor _constructor)
		{
			prepare_node(_constructor.name,"name");
			prepare_node(_constructor.parameters,"parameters");
			prepare_node(_constructor.proc_attributes,"attributes");
		}

		public override void visit(destructor _destructor)
		{
			prepare_node(_destructor.name,"name");
			prepare_node(_destructor.parameters,"parameters");
			prepare_node(_destructor.proc_attributes,"attributes");
		}

		public override void visit(inherited_method_call _inherited_method_call)
		{
			prepare_node(_inherited_method_call.method_name,"method_name");
			prepare_node(_inherited_method_call.exprs,"exprs");
		}

		public override void visit(typecast_node _node)
		{
			prepare_node(_node.expr,"expr");
			prepare_node(_node.type_def,"type_def");
		}

		public override void visit(interface_node _interface_node)
		{
			prepare_node(_interface_node.uses_modules,"uses_modules");
			prepare_node(_interface_node.using_namespaces,"using_namespaces");
			prepare_node(_interface_node.interface_definitions,"interface_definitions");
		}

		public override void visit(implementation_node _implementation_node)
		{
			prepare_node(_implementation_node.uses_modules,"uses_modules");
			prepare_node(_implementation_node.using_namespaces,"using_namespaces");
			prepare_node(_implementation_node.implementation_definitions,"implementation_definitions");
		}
		public override void visit(diap_expr _diap_expr)
		{
			
		}
		public override void visit(block _block)
		{
			prepare_node(_block.defs,"defs");
			prepare_node(_block.program_code,"program code");
		}
		public override void visit(proc_block _proc_block)
		{
			
		}
		public override void visit(array_of_named_type_definition _array_of_named_type_definition)
		{
			prepare_node(_array_of_named_type_definition.type_name,"type_name");
		}
		public override void visit(array_of_const_type_definition _array_of_const_type_defenition)
		{
			
		}
		public override void visit(literal _literal)
		{
			
		}
		public override void visit(case_variants _case_variants)
		{
			prepare_collection(_case_variants.variants,"variants");
		}
		public override void visit(diapason_expr _diapason_expr)
		{
			prepare_node(_diapason_expr.left,"left");
			prepare_node(_diapason_expr.right,"right");
		}
		public override void visit(var_def_list_for_record _var_def_list)
		{
			prepare_collection(_var_def_list.vars,"vars");
		}
		public override void visit(property_array_default _property_array_default)
		{
		}
		public override void visit(property_interface _property_interface)
		{
		}
		public override void visit(property_parameter _property_parameter)
		{
			prepare_node(_property_parameter.names,"names");
			prepare_node(_property_parameter.type,"type");
		}
		public override void visit(property_parameter_list _property_parameter_list)
		{
			prepare_collection(_property_parameter_list.parameters,"parameters");
		}
		public override void visit(inherited_ident _inherited_ident)
		{
		}
		public override void visit(format_expr _format_expr)
		{
			prepare_node(_format_expr.expr,"expr");
			prepare_node(_format_expr.format1,"format1");
			prepare_node(_format_expr.format2,"format2");
		}
		public override void visit(initfinal_part _initfinal_part)
		{
			prepare_node(_initfinal_part.initialization_sect,"initialization_sect");
			prepare_node(_initfinal_part.finalization_sect,"finalization_sect");
		}
		public override void visit(token_info _token_info)
		{
		}
		public override void visit(raise_stmt _raise_stmt)
		{
			prepare_node(_raise_stmt.expr,"expr");
			prepare_node(_raise_stmt.address,"address");
		}
		public override void visit(op_type_node _op_type_node)
		{
		}
		public override void visit(file_type _file_type)
		{
			prepare_node(_file_type.file_of_type,"file_of_type");
		}
		public override void visit(known_type_ident __known_type_ident)
		{
		}
		public override void visit(exception_ident _exception_ident)
		{
			prepare_node(_exception_ident.variable,"variable");
			prepare_node(_exception_ident.type_name,"type_name");
		}
		public override void visit(exception_handler _exception_handler)
		{
			prepare_node(_exception_handler.variable,"variable");
			prepare_node(_exception_handler.type_name,"type_name");
			prepare_node(_exception_handler.statements,"statements");
		}
		public override void visit(exception_handler_list _exception_handler_list)
		{
			prepare_collection(_exception_handler_list.handlers,"handlers");
		}
		public override void visit(exception_block _exception_block)
		{
			prepare_node(_exception_block.stmt_list,"stmt_list");
			prepare_node(_exception_block.handlers,"handlers");
			prepare_node(_exception_block.else_stmt_list,"else_stmt_list");
		}
		public override void visit(try_handler _try_handler)
		{
		}
		public override void visit(try_handler_except _try_handler_except)
		{
			prepare_node(_try_handler_except.except_block,"except_block");
		}
		public override void visit(try_handler_finally _try_handler_finally)
		{
			prepare_node(_try_handler_finally.stmt_list,"stmt_list");
		}
		public override void visit(try_stmt _try_stmt)
		{
			prepare_node(_try_stmt.stmt_list,"stmt_list");
			prepare_node(_try_stmt.handler,"handler");
		}
		public override void visit(inherited_message _inherited_message)
		{
		}
		public override void visit(external_directive _external_directive)
		{
			prepare_node(_external_directive.modulename,"modulename");
			prepare_node(_external_directive.name,"name");
		}
		public override void visit(using_list _using_list)
		{
			prepare_collection(_using_list.namespaces,"usings");
		}
		public override void visit(jump_stmt node)
		{
            prepare_node(node.expr, "expr");
		}
		public override void visit(loop_stmt _loop_stmt)
		{
			prepare_node(_loop_stmt.stmt,"stmt");
		}
		public override void visit(foreach_stmt _foreach_stmt)
		{
			prepare_node(_foreach_stmt.identifier,"identifier");
			prepare_node(_foreach_stmt.type_name,"type_name");
			prepare_node(_foreach_stmt.in_what,"in_what");
			prepare_node(_foreach_stmt.stmt,"stmt");
		}
		public override void visit(addressed_value_funcname _addressed_value_funcname)
		{
		}
        public override void visit(named_type_reference_list _named_type_reference_list)
        {
            prepare_collection(_named_type_reference_list.types, "types");
        }
        public override void visit(template_param_list _template_param_list)
        {
            prepare_collection(_template_param_list.params_list, "params_list");
            prepare_node(_template_param_list.dereferencing_value, "dereferencing_value");
        }
        public override void visit(template_type_reference _template_type_reference)
        {
            prepare_node(_template_type_reference.name, "name");
            prepare_node(_template_type_reference.params_list, "params_list");
        }
        public override void visit(new_expr _new_expr)
        {
            prepare_node(_new_expr.type, "type");
            prepare_node(_new_expr.params_list, "params_list");
            prepare_node(_new_expr.array_init_expr, "array_init_expr");
        }
        public override void visit(where_type_specificator_list node)
        {
            prepare_collection(node.defs, "defs");
        }
        public override void visit(where_definition node)
        {
            prepare_node(node.names, "names");
            prepare_node(node.types, "types");
        }
        public override void visit(where_definition_list node)
        {
            prepare_collection(node.defs, "defs");
        }
        public override void visit(sizeof_operator node)
        {
            prepare_node(node.expr, "expr");
            prepare_node(node.type_def, "type_def");
        }
        public override void visit(typeof_operator node)
        {
            prepare_node(node.type_name, "type_name");
        }
        public override void visit(compiler_directive node)
        {
            prepare_node(node.Name, "Name");
            prepare_node(node.Directive, "Directive");
        }
        public override void visit(operator_name_ident node)
        {
            
        }
        public override void visit(var_statement node)
        {
            prepare_node(node.var_def, "var_def");
        }
        public override void visit(question_colon_expression node)
        {
            prepare_node(node.condition, "condition");
            prepare_node(node.ret_if_true, "ret_if_true");
            prepare_node(node.ret_if_false, "ret_if_false");
        }
        public override void visit(expression_as_statement node)
        {
            prepare_node(node.expr, "expr");
        }
        public override void visit(c_scalar_type node)
        {
            prepare_node(node.attr_list, "attr_list");
        }
        public override void visit(c_module node)
        {
            prepare_node(node.defs, "defs");
        }

        public override void visit(declarations_as_statement node)
        {
            prepare_node(node.defs, "var_defs");
        }
        public override void visit(array_size node)
        {
            prepare_node(node.max_value, "max_value");
        }

        public override void visit(enumerator node)
        {
            prepare_node(node.name, "name");
            prepare_node(node.value, "value");
        }
        public override void visit(enumerator_list node)
        {
            prepare_collection(node.enumerators, "enumerators");
        }
        public override void visit(c_for_cycle node)
        {
            prepare_node(node.expr1, "expr1");
            prepare_node(node.expr2, "expr2");
            prepare_node(node.expr3, "expr3");
            prepare_node(node.stmt, "stmt");
        }
        public override void visit(switch_stmt node)
        {
            prepare_node(node.condition, "condition");
            prepare_node(node.stmt, "stmt");
        }

        public override void visit(type_definition_attr node)
        {

        }

        public override void visit(type_definition_attr_list node)
        {
            prepare_collection(node.attributes, "attributes");            
        }
        public override void visit(lock_stmt node)
        {
            prepare_node(node.lock_object, "lock_object");
            prepare_node(node.stmt, "stmt");
        }
        public override void visit(compiler_directive_list node)
        {
            prepare_collection(node.directives, "directives");
        }
        public override void visit(compiler_directive_if node)
        {
            prepare_node(node.Name, "Name");
            prepare_node(node.Directive, "Directive");
            prepare_node(node.if_part, "if_part");
            prepare_node(node.elseif_part, "elseif_part");
        }
        public override void visit(documentation_comment_list node)
        {
            prepare_collection(node.sections, "sections");
        }
        public override void visit(documentation_comment_section node)
        {
            prepare_collection(node.tags,"tags");
        }
        public override void visit(documentation_comment_tag node)
        {
            prepare_collection(node.parameters, "parameters");
        }
        public override void visit(documentation_comment_tag_param node)
        {
        }
        public override void visit(token_taginfo node)
        {
        }
        public override void visit(declaration_specificator node)
        {

        }
        public override void visit(ident_with_templateparams node)
        {
            prepare_node(node.name, "name");
            prepare_node(node.template_params, "template_params");
        }
        
        public override void visit(default_operator node)
        {
            prepare_node(node.type_name, "type_name");
        }
        public override void visit(bracket_expr node)
        {
        	prepare_node(node.expr,"expr");
        }
        public override void visit(attribute _attribute)
        {
        	
        }
        
        public override void visit(attribute_list _attribute_list)
        {
        	
        }
        
        public override void visit(simple_attribute_list _simple_attribute_list)
        {
        	
        }

        public override void visit(function_lambda_definition node)
        {
            prepare_node(node.formal_parameters, "formal_parameters");
            prepare_node(node.proc_body, "proc_body");
            prepare_node(node.return_type, "return_type");
        }
        public override void visit(function_lambda_call node)
        {
            prepare_node(node.parameters, "real_parametres");
            prepare_node(node.f_lambda_def, "lambda");
        }
        public override void visit(semantic_check _semantic_check)
        {
        }
        public override void visit(matching_expression node)
        {
            prepare_node(node.left, "left");
            prepare_node(node.right, "right");
        }
        public override void visit(short_func_definition node)
        {
            prepare_node(node.proc_header, "proc_header");
            prepare_node(node.proc_body, "proc_body");
            prepare_node(node.procdef, "procdef");
        }
        public override void visit(name_assign_expr node)
        {
            prepare_node(node.name, "name");
            prepare_node(node.expr, "expr");
        }
    }

}

