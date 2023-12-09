using PascalABCCompiler.SyntaxTree;
using System.Collections;
using System;

namespace VisualPascalABCPlugins
{

	class string_ref
	{
		public string s;
	}

	class get_node_info : AbstractVisitor
	{

		private string_ref sr;

		public get_node_info(string_ref sr)
		{
			this.sr=sr;
		}

		public string text
		{
			get
			{
				return sr.s;
			}
			set
			{
				sr.s=value;
			}
		}

		public static string node(syntax_tree_node tn)
		{
			string_ref sr=new string_ref();
			get_node_info gni=new get_node_info(sr);
			tn.visit(gni);
			return sr.s;
		}

		private void get_count(ICollection ic)
		{
			text="Count: "+ic.Count.ToString();
		}

        public override void visit(syntax_tree_node _tree_node)
		{
			
		}


		public override void visit(statement_list _statement_list)
		{
			//text="Count: "+_statement_list.subnodes.Count;
			get_count(_statement_list.subnodes);
		}


		public override void visit(expression _expression)
		{
			
		}


		public override void visit(assign _assign)
		{
            text = "operator_type: " + _assign.operator_type.ToString();
		}


		public override void visit(bin_expr _bin_expr)
		{
            text = "operator_type: " + _bin_expr.operation_type.ToString();
		}


		public override void visit(un_expr _un_expr)
		{
            text = "operator_type: " + _un_expr.operation_type.ToString();
		}


		public override void visit(const_node _const_node)
		{
			
		}


		public override void visit(bool_const _bool_const)
		{
			text="Value: "+_bool_const.val.ToString();
		}


		public override void visit(int32_const _int_const)
		{
			text="Value: "+_int_const.val.ToString();
		}
        public override void visit(int64_const _int_const)
        {
            text = "Value: " + _int_const.val.ToString();
        }
        public override void visit(uint64_const _int_const)
        {
            text = "Value: " + _int_const.val.ToString();
        }

		public override void visit(double_const _double_const)
		{
			text="Value: "+_double_const.val.ToString();
		}


		public override void visit(statement _statement)
		{
			
		}


		public override void visit(subprogram_body _subprogram_body)
		{
			
		}


		public override void visit(ident _ident)
		{
			text="Name: "+_ident.name;
		}

        public override void visit(template_type_name node)
        {
            text = "Name: " + node.name;
        }
		

		public override void visit(addressed_value _addressed_value)
		{
			
		}


		public override void visit(type_definition _type_definition)
		{
			
		}


		public override void visit(named_type_reference _named_type_reference)
		{
			//text="Type name: "+_named_type_reference.type_name;
		}


		public override void visit(variable_definitions _variable_definitions)
		{
			//text="Count: "+_variable_definitions.var_definitions.Count.ToString();
			get_count(_variable_definitions.var_definitions);
		}


		public override void visit(ident_list _ident_list)
		{
			//text="Count: "+_ident_list.idents.Count.ToString();
			get_count(_ident_list.idents);
		}


		public override void visit(var_def_statement _var_def_statement)
		{
            if (_var_def_statement.is_event)
                text = "event";
            if (_var_def_statement.var_attr != definition_attribute.None)
                text += _var_def_statement.var_attr.ToString();
        }


        public override void visit(declaration _subprogram_definition)
		{
		
		}


        public override void visit(declarations _subprogram_definitions)
		{
			get_count(_subprogram_definitions.defs);
		}


		public override void visit(program_tree _program_tree)
		{
			//text="Count: "+_program_tree.compilation_units.Count.ToString();
			get_count(_program_tree.compilation_units);
		}

		public override void visit(program_name _program_name)
		{
			text="program name: "+_program_name.prog_name.name;
		}


		public override void visit(string_const _string_const)
		{
			text="Value: "+_string_const.Value;
		}


		public override void visit(expression_list _expression_list)
		{
			get_count(_expression_list.expressions);
		}


		public override void visit(dereference _dereference)
		{
			
		}


		public override void visit(roof_dereference _roof_dereference)
		{
			
		}


		public override void visit(indexer _indexer)
		{
			
		}


		public override void visit(for_node _for_node)
		{
			text="Cycle type: "+_for_node.cycle_type.ToString();
		}


		public override void visit(repeat_node _repeat_node)
		{
			
		}


		public override void visit(while_node _while_node)
		{
            text = "Cycle type: " + _while_node.CycleType.ToString();
		}


		public override void visit(if_node _if_node)
		{
			
		}


		public override void visit(ref_type _ref_type)
		{
			
		}


		public override void visit(diapason _diapason)
		{
			
		}


		public override void visit(indexers_types _indexers_types)
		{
			get_count(_indexers_types.indexers);
		}


		public override void visit(array_type _array_type)
		{
			
		}


		public override void visit(label_definitions _label_definitions)
		{
			
		}


		public override void visit(procedure_attribute _procedure_attribute)
		{
			text="Attribute: "+_procedure_attribute.attribute_type.ToString();
		}

		public override void visit(procedure_attributes_list _procedure_attributes_list)
		{
			get_count(_procedure_attributes_list.proc_attributes);
		}


		public override void visit(procedure_header _procedure_header)
		{
			if (_procedure_header.of_object) text+="of object";
			if (_procedure_header.class_keyword) text+="class proc";
            if (_procedure_header.IsAsync) text += "isAsync";
        }


		public override void visit(function_header _function_header)
		{
            if (_function_header.of_object) text += "of object";
            if (_function_header.class_keyword) text += "class proc";
		}


		public override void visit(procedure_definition _procedure_definition)
		{
			
		}


		public override void visit(type_declaration _type_declaration)
		{
			
		}


		public override void visit(type_declarations _type_declarations)
		{
			get_count(_type_declarations.types_decl);
		}


		public override void visit(simple_const_definition _simple_const_definition)
		{
			
		}


		public override void visit(typed_const_definition _typed_const_definition)
		{
			
		}


		public override void visit(const_definition _const_definition)
		{
			
		}


		public override void visit(consts_definitions_list _consts_definitions_list)
		{
			get_count(_consts_definitions_list.const_defs);
		}


		public override void visit(unit_name _unit_name)
		{
            string keyword="";
            switch (_unit_name.HeaderKeyword)
            {
                case UnitHeaderKeyword.Unit:
                    keyword = "Unit";
                    break;
                case UnitHeaderKeyword.Library:
                    keyword = "Library";
                    break;
            }
            text = keyword + " name: " + _unit_name.idunit_name.name;			
		}


		public override void visit(unit_or_namespace node)
		{
			
		}


		public override void visit(uses_unit_in _uses_unit_in)
		{
			
		}


		public override void visit(uses_list _uses_list)
		{
			get_count(_uses_list.units);
		}


		public override void visit(program_body _program_body)
		{
			
		}


		public override void visit(compilation_unit _compilation_unit)
		{
			
		}


		public override void visit(unit_module _unit_module)
		{
            text = System.IO.Path.GetFileName(_unit_module.file_name);
		}


		public override void visit(program_module _program_module)
		{
			text=System.IO.Path.GetFileName(_program_module.file_name);
		}

		public override void visit(typed_parameters _typed_parametres)
		{
			text=_typed_parametres.param_kind.ToString();
		}


		public override void visit(formal_parameters _formal_parametres)
		{
			get_count(_formal_parametres.params_list);
		}

		public override void visit(hex_constant _hex_constant)
		{
			text="Value: "+_hex_constant.val;
		}

		public override void visit(get_address _get_address)
		{

		}

		public override void visit(case_variant _case_variant)
		{
			
		}

		public override void visit(case_node _case_node)
		{
			
		}

		public override void visit(method_name _method_name)
		{
			
		}

		public override void visit(dot_node _dot_node)
		{
			
		}

		public override void visit(empty_statement _empty_statement)
		{
			text="empty statement";
		}

		public override void visit(goto_statement _goto_statement)
		{
			
		}

		public override void visit(labeled_statement _labeled_statement)
		{
			
		}

		public override void visit(with_statement _with_statement)
		{
			
		}

		public override void visit(method_call _method_call)
		{
			
		}

		public override void visit(pascal_set_constant _pascal_set_constant)
		{
			
		}

		public override void visit(array_const _array_const)
		{
			
		}

		public override void visit(write_accessor_name _write_accessor_name)
		{
			
		}


		public override void visit(read_accessor_name _read_accessor_name)
		{
			
		}


		public override void visit(property_accessors _property_accessors)
		{
			
		}


        public override void visit(simple_property node)
		{
            if (node.attr != definition_attribute.None)
                text = node.attr.ToString();
		}


		public override void visit(index_property _index_property)
		{
			
		}


		public override void visit(class_members _class_members)
		{
			get_count(_class_members.members);
		}


		public override void visit(access_modifer_node _access_modifer_node)
		{
			text=_access_modifer_node.access_level.ToString();
		}


		public override void visit(class_body_list _class_body)
		{
			get_count(_class_body.class_def_blocks);
		}


		public override void visit(class_definition _class_definition)
		{
            if(_class_definition.attribute!= class_attribute.None)
                text += _class_definition.attribute.ToString()+" ";
            text += _class_definition.keyword.ToString();
		}

		public override void visit(default_indexer_property_node _default_indexer_property_node)
		{
			
		}

		public override void visit(known_type_definition _known_type_definition)
		{
			text=_known_type_definition.tp.ToString();
		}

		public override void visit(set_type_definition _set_type_definition)
		{
			
		}

		public override void visit(record_const_definition _record_const_definition)
		{
			
		}

		public override void visit(record_const _record_const)
		{
			get_count(_record_const.rec_consts);
		}

		public override void visit(record_type _record_type)
		{
			//get_count(_record_type.rec_vars);
		
		}
		public override void visit(record_type_parts _record_type_parts)
		{
			//get_count(_record_type.rec_vars);
		
		}

		public override void visit(enum_type_definition _enum_type_definition)
		{
			
		}

		public override void visit(char_const _char_const)
		{
			text="Value: "+_char_const.cconst;
		}

		public override void visit(raise_statement _raise_statement)
		{
			
		}

		public override void visit(sharp_char_const _sharp_char_const)
		{
			text="Char num: "+_sharp_char_const.char_num.ToString();
		}

		public override void visit(literal_const_line _sharp_const_line)
		{
			get_count(_sharp_const_line.literals);
		}

		public override void visit(string_num_definition _string_num_definition)
		{
			
		}

		public override void visit(variant _variant)
		{
			
		}

		public override void visit(variant_list _variant_list)
		{
			
		}

		public override void visit(variant_type _variant_type)
		{
			
		}

		public override void visit(variant_types _variant_types)
		{
			
		}

		public override void visit(variant_record_type _variant_record_type)
		{
			
		}

		public override void visit(procedure_call _procedure_call)
		{
			
		}

		public override void visit(class_predefinition _class_predefinition)
		{
			
		}

		public override void visit(nil_const _nil_const)
		{
			
		}

		public override void visit(file_type_definition _file_type_definition)
		{
			
		}

		public override void visit(constructor _constructor)
		{
            if (_constructor.of_object) text += "of object";
            if (_constructor.class_keyword) text += "class proc";
		}

		public override void visit(destructor _destructor)
		{
            if (_destructor.of_object) text += "of object";
            if (_destructor.class_keyword) text += "class proc";
		}

		public override void visit(inherited_method_call _inherited_method_call)
		{
			
		}

		public override void visit(typecast_node _node)
		{
            text = _node.cast_op.ToString();
		}

		public override void visit(interface_node _interface_node)
		{
			
		}

		public override void visit(implementation_node _implementation_node)
		{
			
		}
		public override void visit(diap_expr _diap_expr)
		{
			
		}
		public override void visit(block _block)
		{
			
		}
		public override void visit(proc_block _proc_block)
		{
			
		}
		public override void visit(array_of_named_type_definition _array_of_named_type_definition)
		{

		}
		public override void visit(array_of_const_type_definition _array_of_const_type_defenition)
		{
			text="array of const";
		}
		public override void visit(literal _literal)
		{
			
		}
		public override void visit(case_variants _case_variants)
		{
			get_count(_case_variants.variants);
		}
		public override void visit(diapason_expr _diapason_expr)
		{
			
		}
		public override void visit(var_def_list_for_record _var_def_list)
		{
			//text="Count: "+_statement_list.subnodes.Count;
			get_count(_var_def_list.vars);
		}
		public override void visit(property_array_default _property_array_default)
		{
			
		}
		public override void visit(property_interface _property_interface)
		{
			
		}
		public override void visit(property_parameter _property_parameter)
		{
			
		}
		public override void visit(property_parameter_list _property_parameter_list)
		{
			get_count(_property_parameter_list.parameters);
		}
		public override void visit(inherited_ident _inherited_ident)
		{
			text="inherited ident: "+_inherited_ident.name;		
		}
		public override void visit(format_expr _format_expr)
		{
		}
		public override void visit(initfinal_part _initfinal_part)
		{
		}
		public override void visit(token_info _token_info)
		{
            text = string.Format("text: '{0}'",_token_info.text);	
		}	
		public override void visit(raise_stmt _raise_stmt)
		{
		}
		public override void visit(op_type_node _op_type_node)
		{
		}
		public override void visit(file_type _file_type)
		{
		}		
		public override void visit(known_type_ident _known_type_ident)
		{
			text="Name: "+_known_type_ident.name+" type: "+_known_type_ident.type.ToString();

		}
		public override void visit(exception_ident _exception_ident)
		{
		}
		public override void visit(exception_handler _exception_handler)
		{
		}
		public override void visit(exception_handler_list _exception_handler_list)
		{
			get_count(_exception_handler_list.handlers);
		}
		public override void visit(exception_block _exception_block)
		{
		}
		public override void visit(try_handler _try_handler)
		{
		}
		public override void visit(try_handler_except _try_handler_except)
		{
		}
		public override void visit(try_handler_finally _try_handler_finally)
		{
		}
		public override void visit(try_stmt _try_stmt)
		{
		}
		public override void visit(inherited_message _inherited_message)
		{
		}
		public override void visit(external_directive _external_directive)
		{
		}
        	
		public override void visit(using_list _using_list)
		{
			get_count(_using_list.namespaces);
		}
        public override void visit(jump_stmt node)
		{
            text = node.JumpType.ToString();
		}
		public override void visit(loop_stmt _loop_stmt)
		{
		}
		public override void visit(foreach_stmt _foreach_stmt)
		{
		}
		public override void visit(addressed_value_funcname _addressed_value_funcname)
		{
		}
        public override void visit(named_type_reference_list _named_type_reference_list)
        {
            get_count(_named_type_reference_list.types);
        }
        public override void visit(template_param_list _template_param_list)
        {
            get_count(_template_param_list.params_list);            
        }
        public override void visit(template_type_reference _template_type_reference)
        {
            
        }
        public override void visit(new_expr _new_expr)
        {
            if (_new_expr.new_array)
                text = "new_array";
        }
        public override void visit(where_type_specificator_list node)
        {
            get_count(node.defs);
        }
        public override void visit(where_definition node)
        {
        }
        public override void visit(where_definition_list node)
        {
            get_count(node.defs);
        }
        public override void visit(sizeof_operator node)
        {
        }
        public override void visit(typeof_operator node)
        {
        }
        public override void visit(compiler_directive node)
        {
        }
        public override void visit(compiler_directive_list node)
        {
        }
        public override void visit(compiler_directive_if node)
        {
            text = string.Format("{0}={1}", node.Name, node.Directive);
        }
        public override void visit(operator_name_ident node)
        {
            text = node.operator_type.ToString();
        }
        public override void visit(var_statement node)
        {
        }
        public override void visit(question_colon_expression node)
        {
        }
        public override void visit(expression_as_statement node)
        {
        }
        public override void visit(c_scalar_type node)
        {
            text = node.scalar_name.ToString() + " sign:" + node.sign.ToString();
        }
        public override void visit(c_module node)
        {
            //text = "LanguageId:" + node.Language.ToString();
        }
        public override void visit(declarations_as_statement node)
        {
        }
        public override void visit(array_size node)
        {
        }
        public override void visit(enumerator node)
        {
        }
        public override void visit(enumerator_list node)
        {
        }
        public override void visit(c_for_cycle node)
        {
        }
        public override void visit(switch_stmt node)
        {
            text = node.Part.ToString();
        }

        public override void visit(type_definition_attr node)
        {
            if (node.attr !=  definition_attribute.None)
                text = node.attr.ToString();
        }

        public override void visit(type_definition_attr_list node)
        {
            if(node.attributes.Count>0)
                get_count(node.attributes);
        }
        public override void visit(lock_stmt node)
        {
        }
        public override void visit(documentation_comment_list node)
        {
        }
        public override void visit(documentation_comment_section node)
        {
            if (node.text != null)
                text = node.text;
        }
        public override void visit(documentation_comment_tag node)
        {
            text = string.Format("name={0} text={1}", node.name, node.text);
        }
        public override void visit(documentation_comment_tag_param node)
        {
            text = string.Format("name={0} value={1}", node.name, node.value);
        }
        public override void visit(token_taginfo node)
        {
        }
        public override void visit(declaration_specificator node)
        {
            text = String.Format("{0} ({1})", node.specificator, node.name);
        }
        public override void visit(ident_with_templateparams node)
        {
           
        }
        public override void visit(default_operator node)
        {
            
        }
        public override void visit(bracket_expr _bracket_expr)
        {
        	_bracket_expr.expr.visit(this);
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

        public override void visit(function_lambda_definition _function_lambda_definition)
        {

        }
        public override void visit(function_lambda_call _function_lambda_call)
        {
            //
        }
        public override void visit(semantic_check _semantic_check)
        {
            throw new NotImplementedException();
        }
		public override void visit(lambda_inferred_type lit) //lroman//
        {
            
        }
        public override void visit(same_type_node stn) //SS 22/06/13//
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

