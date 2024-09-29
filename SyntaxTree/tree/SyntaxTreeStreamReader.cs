/********************************************************************************************
Этот файл создан программой
PascalABC.NET: syntax tree generator  v1.5(с) Водолазов Н., Ткачук А.В., Иванов С.О., 2007

Вручную не редактировать!
*********************************************************************************************/
using System;
using System.IO;
using System.Collections.Generic;

namespace PascalABCCompiler.SyntaxTree
{

	public class SyntaxTreeStreamReader:IVisitor
	{

		public BinaryReader br;

		public syntax_tree_node _construct_node(Int16 node_class_number)
		{
			switch(node_class_number)
			{
				case 0:
					return new expression();
				case 1:
					return new syntax_tree_node();
				case 2:
					return new statement();
				case 3:
					return new statement_list();
				case 4:
					return new ident();
				case 5:
					return new assign();
				case 6:
					return new bin_expr();
				case 7:
					return new un_expr();
				case 8:
					return new const_node();
				case 9:
					return new bool_const();
				case 10:
					return new int32_const();
				case 11:
					return new double_const();
				case 12:
					return new subprogram_body();
				case 13:
					return new addressed_value();
				case 14:
					return new type_definition();
				case 15:
					return new roof_dereference();
				case 16:
					return new named_type_reference();
				case 17:
					return new variable_definitions();
				case 18:
					return new ident_list();
				case 19:
					return new var_def_statement();
				case 20:
					return new declaration();
				case 21:
					return new declarations();
				case 22:
					return new program_tree();
				case 23:
					return new program_name();
				case 24:
					return new string_const();
				case 25:
					return new expression_list();
				case 26:
					return new dereference();
				case 27:
					return new indexer();
				case 28:
					return new for_node();
				case 29:
					return new repeat_node();
				case 30:
					return new while_node();
				case 31:
					return new if_node();
				case 32:
					return new ref_type();
				case 33:
					return new diapason();
				case 34:
					return new indexers_types();
				case 35:
					return new array_type();
				case 36:
					return new label_definitions();
				case 37:
					return new procedure_attribute();
				case 38:
					return new typed_parameters();
				case 39:
					return new formal_parameters();
				case 40:
					return new procedure_attributes_list();
				case 41:
					return new procedure_header();
				case 42:
					return new function_header();
				case 43:
					return new procedure_definition();
				case 44:
					return new type_declaration();
				case 45:
					return new type_declarations();
				case 46:
					return new simple_const_definition();
				case 47:
					return new typed_const_definition();
				case 48:
					return new const_definition();
				case 49:
					return new consts_definitions_list();
				case 50:
					return new unit_name();
				case 51:
					return new unit_or_namespace();
				case 52:
					return new uses_unit_in();
				case 53:
					return new uses_list();
				case 54:
					return new program_body();
				case 55:
					return new compilation_unit();
				case 56:
					return new unit_module();
				case 57:
					return new program_module();
				case 58:
					return new hex_constant();
				case 59:
					return new get_address();
				case 60:
					return new case_variant();
				case 61:
					return new case_node();
				case 62:
					return new method_name();
				case 63:
					return new dot_node();
				case 64:
					return new empty_statement();
				case 65:
					return new goto_statement();
				case 66:
					return new labeled_statement();
				case 67:
					return new with_statement();
				case 68:
					return new method_call();
				case 69:
					return new pascal_set_constant();
				case 70:
					return new array_const();
				case 71:
					return new write_accessor_name();
				case 72:
					return new read_accessor_name();
				case 73:
					return new property_accessors();
				case 74:
					return new simple_property();
				case 75:
					return new index_property();
				case 76:
					return new class_members();
				case 77:
					return new access_modifer_node();
				case 78:
					return new class_body_list();
				case 79:
					return new class_definition();
				case 80:
					return new default_indexer_property_node();
				case 81:
					return new known_type_definition();
				case 82:
					return new set_type_definition();
				case 83:
					return new record_const_definition();
				case 84:
					return new record_const();
				case 85:
					return new record_type();
				case 86:
					return new enum_type_definition();
				case 87:
					return new char_const();
				case 88:
					return new raise_statement();
				case 89:
					return new sharp_char_const();
				case 90:
					return new literal_const_line();
				case 91:
					return new string_num_definition();
				case 92:
					return new variant();
				case 93:
					return new variant_list();
				case 94:
					return new variant_type();
				case 95:
					return new variant_types();
				case 96:
					return new variant_record_type();
				case 97:
					return new procedure_call();
				case 98:
					return new class_predefinition();
				case 99:
					return new nil_const();
				case 100:
					return new file_type_definition();
				case 101:
					return new constructor();
				case 102:
					return new destructor();
				case 103:
					return new inherited_method_call();
				case 104:
					return new typecast_node();
				case 105:
					return new interface_node();
				case 106:
					return new implementation_node();
				case 107:
					return new diap_expr();
				case 108:
					return new block();
				case 109:
					return new proc_block();
				case 110:
					return new array_of_named_type_definition();
				case 111:
					return new array_of_const_type_definition();
				case 112:
					return new literal();
				case 113:
					return new case_variants();
				case 114:
					return new diapason_expr();
				case 115:
					return new var_def_list_for_record();
				case 116:
					return new record_type_parts();
				case 117:
					return new property_array_default();
				case 118:
					return new property_interface();
				case 119:
					return new property_parameter();
				case 120:
					return new property_parameter_list();
				case 121:
					return new inherited_ident();
				case 122:
					return new format_expr();
				case 123:
					return new initfinal_part();
				case 124:
					return new token_info();
				case 125:
					return new raise_stmt();
				case 126:
					return new op_type_node();
				case 127:
					return new file_type();
				case 128:
					return new known_type_ident();
				case 129:
					return new exception_handler();
				case 130:
					return new exception_ident();
				case 131:
					return new exception_handler_list();
				case 132:
					return new exception_block();
				case 133:
					return new try_handler();
				case 134:
					return new try_handler_finally();
				case 135:
					return new try_handler_except();
				case 136:
					return new try_stmt();
				case 137:
					return new inherited_message();
				case 138:
					return new external_directive();
				case 139:
					return new using_list();
				case 140:
					return new jump_stmt();
				case 141:
					return new loop_stmt();
				case 142:
					return new foreach_stmt();
				case 143:
					return new addressed_value_funcname();
				case 144:
					return new named_type_reference_list();
				case 145:
					return new template_param_list();
				case 146:
					return new template_type_reference();
				case 147:
					return new int64_const();
				case 148:
					return new uint64_const();
				case 149:
					return new new_expr();
				case 150:
					return new where_type_specificator_list();
				case 151:
					return new where_definition();
				case 152:
					return new where_definition_list();
				case 153:
					return new sizeof_operator();
				case 154:
					return new typeof_operator();
				case 155:
					return new compiler_directive();
				case 156:
					return new operator_name_ident();
				case 157:
					return new var_statement();
				case 158:
					return new question_colon_expression();
				case 159:
					return new expression_as_statement();
				case 160:
					return new c_scalar_type();
				case 161:
					return new c_module();
				case 162:
					return new declarations_as_statement();
				case 163:
					return new array_size();
				case 164:
					return new enumerator();
				case 165:
					return new enumerator_list();
				case 166:
					return new c_for_cycle();
				case 167:
					return new switch_stmt();
				case 168:
					return new type_definition_attr_list();
				case 169:
					return new type_definition_attr();
				case 170:
					return new lock_stmt();
				case 171:
					return new compiler_directive_list();
				case 172:
					return new compiler_directive_if();
				case 173:
					return new documentation_comment_list();
				case 174:
					return new documentation_comment_tag();
				case 175:
					return new documentation_comment_tag_param();
				case 176:
					return new documentation_comment_section();
				case 177:
					return new token_taginfo();
				case 178:
					return new declaration_specificator();
				case 179:
					return new ident_with_templateparams();
				case 180:
					return new template_type_name();
				case 181:
					return new default_operator();
				case 182:
					return new bracket_expr();
				case 183:
					return new attribute();
				case 184:
					return new simple_attribute_list();
				case 185:
					return new attribute_list();
				case 186:
					return new function_lambda_definition();
				case 187:
					return new function_lambda_call();
				case 188:
					return new semantic_check();
				case 189:
					return new lambda_inferred_type();
				case 190:
					return new same_type_node();
				case 191:
					return new name_assign_expr();
				case 192:
					return new name_assign_expr_list();
				case 193:
					return new unnamed_type_object();
				case 194:
					return new semantic_type_node();
				case 195:
					return new short_func_definition();
				case 196:
					return new no_type_foreach();
				case 197:
					return new matching_expression();
				case 198:
					return new closure_substituting_node();
				case 199:
					return new sequence_type();
				case 200:
					return new modern_proc_type();
				case 201:
					return new yield_node();
				case 202:
					return new template_operator_name();
				case 203:
					return new semantic_addr_value();
				case 204:
					return new pair_type_stlist();
				case 205:
					return new assign_tuple();
				case 206:
					return new addressed_value_list();
				case 207:
					return new tuple_node();
				case 208:
					return new uses_closure();
				case 209:
					return new dot_question_node();
				case 210:
					return new slice_expr();
				case 211:
					return new no_type();
				case 212:
					return new yield_unknown_ident();
				case 213:
					return new yield_unknown_expression_type();
				case 214:
					return new yield_unknown_foreach_type();
				case 215:
					return new yield_sequence_node();
				case 216:
					return new assign_var_tuple();
				case 217:
					return new slice_expr_question();
				case 218:
					return new semantic_check_sugared_statement_node();
				case 219:
					return new sugared_expression();
				case 220:
					return new sugared_addressed_value();
				case 221:
					return new double_question_node();
				case 222:
					return new pattern_node();
				case 223:
					return new type_pattern();
				case 224:
					return new is_pattern_expr();
				case 225:
					return new match_with();
				case 226:
					return new pattern_case();
				case 227:
					return new pattern_cases();
				case 228:
					return new deconstructor_pattern();
				case 229:
					return new pattern_parameter();
				case 230:
					return new desugared_deconstruction();
				case 231:
					return new var_deconstructor_parameter();
				case 232:
					return new recursive_deconstructor_parameter();
				case 233:
					return new deconstruction_variables_definition();
				case 234:
					return new var_tuple_def_statement();
				case 235:
					return new semantic_check_sugared_var_def_statement_node();
				case 236:
					return new const_pattern();
				case 237:
					return new tuple_pattern_wild_card();
				case 238:
					return new const_pattern_parameter();
				case 239:
					return new wild_card_deconstructor_parameter();
				case 240:
					return new collection_pattern();
				case 241:
					return new collection_pattern_gap_parameter();
				case 242:
					return new collection_pattern_wild_card();
				case 243:
					return new collection_pattern_var_parameter();
				case 244:
					return new recursive_collection_parameter();
				case 245:
					return new recursive_pattern_parameter();
				case 246:
					return new tuple_pattern();
				case 247:
					return new tuple_pattern_var_parameter();
				case 248:
					return new recursive_tuple_parameter();
				case 249:
					return new diapason_expr_new();
				case 250:
					return new if_expr_new();
				case 251:
					return new simple_expr_with_deref();
				case 252:
					return new index();
				case 253:
					return new array_const_new();
				case 254:
					return new semantic_ith_element_of();
				case 255:
					return new bigint_const();
				case 256:
					return new foreach_stmt_formatting();
				case 257:
					return new property_ident();
				case 258:
					return new expression_with_let();
				case 259:
					return new lambda_any_type_node_syntax();
				case 260:
					return new ref_var_def_statement();
				case 261:
					return new let_var_expr();
				case 262:
					return new to_expr();
				case 263:
					return new global_statement();
				case 264:
					return new list_generator();
			}
			return null;
		}
		public syntax_tree_node _read_node()
		{
			if (br.ReadByte() == 1)
			{
				syntax_tree_node ssyy_tmp = _construct_node(br.ReadInt16());
				ssyy_tmp.visit(this);
				return ssyy_tmp;
			}
			else
			{
				return null;
			}
		}

		public void visit(expression _expression)
		{
			read_expression(_expression);
		}

		public void read_expression(expression _expression)
		{
			read_declaration(_expression);
		}


		public void visit(syntax_tree_node _syntax_tree_node)
		{
			read_syntax_tree_node(_syntax_tree_node);
		}

		public void read_syntax_tree_node(syntax_tree_node _syntax_tree_node)
		{
			if (br.ReadByte() == 0)
			{
				_syntax_tree_node.source_context = null;
			}
			else
			{
				SourceContext ssyy_beg = null;
				SourceContext ssyy_end = null;
				if (br.ReadByte() == 1)
				{
					ssyy_beg = new SourceContext(br.ReadInt32(), br.ReadInt32(), 0, 0);
				}
				if (br.ReadByte() == 1)
				{
					ssyy_end = new SourceContext(0, 0, br.ReadInt32(), br.ReadInt32());
				}
				_syntax_tree_node.source_context = new SourceContext(ssyy_beg, ssyy_end);
			}
		}


		public void visit(statement _statement)
		{
			read_statement(_statement);
		}

		public void read_statement(statement _statement)
		{
			read_declaration(_statement);
		}


		public void visit(statement_list _statement_list)
		{
			read_statement_list(_statement_list);
		}

		public void read_statement_list(statement_list _statement_list)
		{
			read_statement(_statement_list);
			if (br.ReadByte() == 0)
			{
				_statement_list.subnodes = null;
			}
			else
			{
				_statement_list.subnodes = new List<statement>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_statement_list.subnodes.Add(_read_node() as statement);
				}
			}
			_statement_list.left_logical_bracket = _read_node() as token_info;
			_statement_list.right_logical_bracket = _read_node() as token_info;
			_statement_list.expr_lambda_body = br.ReadBoolean();
		}


		public void visit(ident _ident)
		{
			read_ident(_ident);
		}

		public void read_ident(ident _ident)
		{
			read_addressed_value_funcname(_ident);
			if (br.ReadByte() == 0)
			{
				_ident.name = null;
			}
			else
			{
				_ident.name = br.ReadString();
			}
		}


		public void visit(assign _assign)
		{
			read_assign(_assign);
		}

		public void read_assign(assign _assign)
		{
			read_statement(_assign);
			_assign.to = _read_node() as addressed_value;
			_assign.from = _read_node() as expression;
			_assign.operator_type = (Operators)br.ReadByte();
		}


		public void visit(bin_expr _bin_expr)
		{
			read_bin_expr(_bin_expr);
		}

		public void read_bin_expr(bin_expr _bin_expr)
		{
			read_addressed_value(_bin_expr);
			_bin_expr.left = _read_node() as expression;
			_bin_expr.right = _read_node() as expression;
			_bin_expr.operation_type = (Operators)br.ReadByte();
		}


		public void visit(un_expr _un_expr)
		{
			read_un_expr(_un_expr);
		}

		public void read_un_expr(un_expr _un_expr)
		{
			read_addressed_value(_un_expr);
			_un_expr.subnode = _read_node() as expression;
			_un_expr.operation_type = (Operators)br.ReadByte();
		}


		public void visit(const_node _const_node)
		{
			read_const_node(_const_node);
		}

		public void read_const_node(const_node _const_node)
		{
			read_addressed_value(_const_node);
		}


		public void visit(bool_const _bool_const)
		{
			read_bool_const(_bool_const);
		}

		public void read_bool_const(bool_const _bool_const)
		{
			read_const_node(_bool_const);
			_bool_const.val = br.ReadBoolean();
		}


		public void visit(int32_const _int32_const)
		{
			read_int32_const(_int32_const);
		}

		public void read_int32_const(int32_const _int32_const)
		{
			read_const_node(_int32_const);
			_int32_const.val = br.ReadInt32();
		}


		public void visit(double_const _double_const)
		{
			read_double_const(_double_const);
		}

		public void read_double_const(double_const _double_const)
		{
			read_const_node(_double_const);
			_double_const.val = br.ReadDouble();
		}


		public void visit(subprogram_body _subprogram_body)
		{
			read_subprogram_body(_subprogram_body);
		}

		public void read_subprogram_body(subprogram_body _subprogram_body)
		{
			read_syntax_tree_node(_subprogram_body);
			_subprogram_body.subprogram_code = _read_node() as statement_list;
			_subprogram_body.subprogram_defs = _read_node() as declarations;
		}


		public void visit(addressed_value _addressed_value)
		{
			read_addressed_value(_addressed_value);
		}

		public void read_addressed_value(addressed_value _addressed_value)
		{
			read_expression(_addressed_value);
		}


		public void visit(type_definition _type_definition)
		{
			read_type_definition(_type_definition);
		}

		public void read_type_definition(type_definition _type_definition)
		{
			read_declaration(_type_definition);
			_type_definition.attr_list = _read_node() as type_definition_attr_list;
		}


		public void visit(roof_dereference _roof_dereference)
		{
			read_roof_dereference(_roof_dereference);
		}

		public void read_roof_dereference(roof_dereference _roof_dereference)
		{
			read_dereference(_roof_dereference);
		}


		public void visit(named_type_reference _named_type_reference)
		{
			read_named_type_reference(_named_type_reference);
		}

		public void read_named_type_reference(named_type_reference _named_type_reference)
		{
			read_type_definition(_named_type_reference);
			if (br.ReadByte() == 0)
			{
				_named_type_reference.names = null;
			}
			else
			{
				_named_type_reference.names = new List<ident>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_named_type_reference.names.Add(_read_node() as ident);
				}
			}
		}


		public void visit(variable_definitions _variable_definitions)
		{
			read_variable_definitions(_variable_definitions);
		}

		public void read_variable_definitions(variable_definitions _variable_definitions)
		{
			read_declaration(_variable_definitions);
			if (br.ReadByte() == 0)
			{
				_variable_definitions.var_definitions = null;
			}
			else
			{
				_variable_definitions.var_definitions = new List<var_def_statement>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_variable_definitions.var_definitions.Add(_read_node() as var_def_statement);
				}
			}
		}


		public void visit(ident_list _ident_list)
		{
			read_ident_list(_ident_list);
		}

		public void read_ident_list(ident_list _ident_list)
		{
			read_syntax_tree_node(_ident_list);
			if (br.ReadByte() == 0)
			{
				_ident_list.idents = null;
			}
			else
			{
				_ident_list.idents = new List<ident>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_ident_list.idents.Add(_read_node() as ident);
				}
			}
		}


		public void visit(var_def_statement _var_def_statement)
		{
			read_var_def_statement(_var_def_statement);
		}

		public void read_var_def_statement(var_def_statement _var_def_statement)
		{
			read_declaration(_var_def_statement);
			_var_def_statement.vars = _read_node() as ident_list;
			_var_def_statement.vars_type = _read_node() as type_definition;
			_var_def_statement.inital_value = _read_node() as expression;
			_var_def_statement.var_attr = (definition_attribute)br.ReadByte();
			_var_def_statement.is_event = br.ReadBoolean();
		}


		public void visit(declaration _declaration)
		{
			read_declaration(_declaration);
		}

		public void read_declaration(declaration _declaration)
		{
			read_syntax_tree_node(_declaration);
			_declaration.attributes = _read_node() as attribute_list;
		}


		public void visit(declarations _declarations)
		{
			read_declarations(_declarations);
		}

		public void read_declarations(declarations _declarations)
		{
			read_syntax_tree_node(_declarations);
			if (br.ReadByte() == 0)
			{
				_declarations.defs = null;
			}
			else
			{
				_declarations.defs = new List<declaration>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_declarations.defs.Add(_read_node() as declaration);
				}
			}
		}


		public void visit(program_tree _program_tree)
		{
			read_program_tree(_program_tree);
		}

		public void read_program_tree(program_tree _program_tree)
		{
			read_syntax_tree_node(_program_tree);
			if (br.ReadByte() == 0)
			{
				_program_tree.compilation_units = null;
			}
			else
			{
				_program_tree.compilation_units = new List<compilation_unit>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_program_tree.compilation_units.Add(_read_node() as compilation_unit);
				}
			}
		}


		public void visit(program_name _program_name)
		{
			read_program_name(_program_name);
		}

		public void read_program_name(program_name _program_name)
		{
			read_syntax_tree_node(_program_name);
			_program_name.prog_name = _read_node() as ident;
		}


		public void visit(string_const _string_const)
		{
			read_string_const(_string_const);
		}

		public void read_string_const(string_const _string_const)
		{
			read_literal(_string_const);
			if (br.ReadByte() == 0)
			{
				_string_const.Value = null;
			}
			else
			{
				_string_const.Value = br.ReadString();
			}
		}


		public void visit(expression_list _expression_list)
		{
			read_expression_list(_expression_list);
		}

		public void read_expression_list(expression_list _expression_list)
		{
			read_syntax_tree_node(_expression_list);
			if (br.ReadByte() == 0)
			{
				_expression_list.expressions = null;
			}
			else
			{
				_expression_list.expressions = new List<expression>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_expression_list.expressions.Add(_read_node() as expression);
				}
			}
		}


		public void visit(dereference _dereference)
		{
			read_dereference(_dereference);
		}

		public void read_dereference(dereference _dereference)
		{
			read_addressed_value_funcname(_dereference);
			_dereference.dereferencing_value = _read_node() as addressed_value;
		}


		public void visit(indexer _indexer)
		{
			read_indexer(_indexer);
		}

		public void read_indexer(indexer _indexer)
		{
			read_dereference(_indexer);
			_indexer.indexes = _read_node() as expression_list;
		}


		public void visit(for_node _for_node)
		{
			read_for_node(_for_node);
		}

		public void read_for_node(for_node _for_node)
		{
			read_statement(_for_node);
			_for_node.loop_variable = _read_node() as ident;
			_for_node.initial_value = _read_node() as expression;
			_for_node.finish_value = _read_node() as expression;
			_for_node.statements = _read_node() as statement;
			_for_node.cycle_type = (for_cycle_type)br.ReadByte();
			_for_node.increment_value = _read_node() as expression;
			_for_node.type_name = _read_node() as type_definition;
			_for_node.create_loop_variable = br.ReadBoolean();
		}


		public void visit(repeat_node _repeat_node)
		{
			read_repeat_node(_repeat_node);
		}

		public void read_repeat_node(repeat_node _repeat_node)
		{
			read_statement(_repeat_node);
			_repeat_node.statements = _read_node() as statement;
			_repeat_node.expr = _read_node() as expression;
		}


		public void visit(while_node _while_node)
		{
			read_while_node(_while_node);
		}

		public void read_while_node(while_node _while_node)
		{
			read_statement(_while_node);
			_while_node.expr = _read_node() as expression;
			_while_node.statements = _read_node() as statement;
			_while_node.CycleType = (WhileCycleType)br.ReadByte();
		}


		public void visit(if_node _if_node)
		{
			read_if_node(_if_node);
		}

		public void read_if_node(if_node _if_node)
		{
			read_statement(_if_node);
			_if_node.condition = _read_node() as expression;
			_if_node.then_body = _read_node() as statement;
			_if_node.else_body = _read_node() as statement;
		}


		public void visit(ref_type _ref_type)
		{
			read_ref_type(_ref_type);
		}

		public void read_ref_type(ref_type _ref_type)
		{
			read_type_definition(_ref_type);
			_ref_type.pointed_to = _read_node() as type_definition;
		}


		public void visit(diapason _diapason)
		{
			read_diapason(_diapason);
		}

		public void read_diapason(diapason _diapason)
		{
			read_type_definition(_diapason);
			_diapason.left = _read_node() as expression;
			_diapason.right = _read_node() as expression;
		}


		public void visit(indexers_types _indexers_types)
		{
			read_indexers_types(_indexers_types);
		}

		public void read_indexers_types(indexers_types _indexers_types)
		{
			read_type_definition(_indexers_types);
			if (br.ReadByte() == 0)
			{
				_indexers_types.indexers = null;
			}
			else
			{
				_indexers_types.indexers = new List<type_definition>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_indexers_types.indexers.Add(_read_node() as type_definition);
				}
			}
		}


		public void visit(array_type _array_type)
		{
			read_array_type(_array_type);
		}

		public void read_array_type(array_type _array_type)
		{
			read_type_definition(_array_type);
			_array_type.indexers = _read_node() as indexers_types;
			_array_type.elements_type = _read_node() as type_definition;
		}


		public void visit(label_definitions _label_definitions)
		{
			read_label_definitions(_label_definitions);
		}

		public void read_label_definitions(label_definitions _label_definitions)
		{
			read_declaration(_label_definitions);
			_label_definitions.labels = _read_node() as ident_list;
		}


		public void visit(procedure_attribute _procedure_attribute)
		{
			read_procedure_attribute(_procedure_attribute);
		}

		public void read_procedure_attribute(procedure_attribute _procedure_attribute)
		{
			read_ident(_procedure_attribute);
			_procedure_attribute.attribute_type = (proc_attribute)br.ReadByte();
		}


		public void visit(typed_parameters _typed_parameters)
		{
			read_typed_parameters(_typed_parameters);
		}

		public void read_typed_parameters(typed_parameters _typed_parameters)
		{
			read_declaration(_typed_parameters);
			_typed_parameters.idents = _read_node() as ident_list;
			_typed_parameters.vars_type = _read_node() as type_definition;
			_typed_parameters.param_kind = (parametr_kind)br.ReadByte();
			_typed_parameters.inital_value = _read_node() as expression;
		}


		public void visit(formal_parameters _formal_parameters)
		{
			read_formal_parameters(_formal_parameters);
		}

		public void read_formal_parameters(formal_parameters _formal_parameters)
		{
			read_syntax_tree_node(_formal_parameters);
			if (br.ReadByte() == 0)
			{
				_formal_parameters.params_list = null;
			}
			else
			{
				_formal_parameters.params_list = new List<typed_parameters>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_formal_parameters.params_list.Add(_read_node() as typed_parameters);
				}
			}
		}


		public void visit(procedure_attributes_list _procedure_attributes_list)
		{
			read_procedure_attributes_list(_procedure_attributes_list);
		}

		public void read_procedure_attributes_list(procedure_attributes_list _procedure_attributes_list)
		{
			read_syntax_tree_node(_procedure_attributes_list);
			if (br.ReadByte() == 0)
			{
				_procedure_attributes_list.proc_attributes = null;
			}
			else
			{
				_procedure_attributes_list.proc_attributes = new List<procedure_attribute>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_procedure_attributes_list.proc_attributes.Add(_read_node() as procedure_attribute);
				}
			}
		}


		public void visit(procedure_header _procedure_header)
		{
			read_procedure_header(_procedure_header);
		}

		public void read_procedure_header(procedure_header _procedure_header)
		{
			read_type_definition(_procedure_header);
			_procedure_header.parameters = _read_node() as formal_parameters;
			_procedure_header.proc_attributes = _read_node() as procedure_attributes_list;
			_procedure_header.name = _read_node() as method_name;
			_procedure_header.of_object = br.ReadBoolean();
			_procedure_header.class_keyword = br.ReadBoolean();
			_procedure_header.template_args = _read_node() as ident_list;
			_procedure_header.where_defs = _read_node() as where_definition_list;
		}


		public void visit(function_header _function_header)
		{
			read_function_header(_function_header);
		}

		public void read_function_header(function_header _function_header)
		{
			read_procedure_header(_function_header);
			_function_header.return_type = _read_node() as type_definition;
		}


		public void visit(procedure_definition _procedure_definition)
		{
			read_procedure_definition(_procedure_definition);
		}

		public void read_procedure_definition(procedure_definition _procedure_definition)
		{
			read_declaration(_procedure_definition);
			_procedure_definition.proc_header = _read_node() as procedure_header;
			_procedure_definition.proc_body = _read_node() as proc_block;
			_procedure_definition.is_short_definition = br.ReadBoolean();
		}


		public void visit(type_declaration _type_declaration)
		{
			read_type_declaration(_type_declaration);
		}

		public void read_type_declaration(type_declaration _type_declaration)
		{
			read_declaration(_type_declaration);
			_type_declaration.type_name = _read_node() as ident;
			_type_declaration.type_def = _read_node() as type_definition;
		}


		public void visit(type_declarations _type_declarations)
		{
			read_type_declarations(_type_declarations);
		}

		public void read_type_declarations(type_declarations _type_declarations)
		{
			read_declaration(_type_declarations);
			if (br.ReadByte() == 0)
			{
				_type_declarations.types_decl = null;
			}
			else
			{
				_type_declarations.types_decl = new List<type_declaration>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_type_declarations.types_decl.Add(_read_node() as type_declaration);
				}
			}
		}


		public void visit(simple_const_definition _simple_const_definition)
		{
			read_simple_const_definition(_simple_const_definition);
		}

		public void read_simple_const_definition(simple_const_definition _simple_const_definition)
		{
			read_const_definition(_simple_const_definition);
		}


		public void visit(typed_const_definition _typed_const_definition)
		{
			read_typed_const_definition(_typed_const_definition);
		}

		public void read_typed_const_definition(typed_const_definition _typed_const_definition)
		{
			read_const_definition(_typed_const_definition);
			_typed_const_definition.const_type = _read_node() as type_definition;
		}


		public void visit(const_definition _const_definition)
		{
			read_const_definition(_const_definition);
		}

		public void read_const_definition(const_definition _const_definition)
		{
			read_declaration(_const_definition);
			_const_definition.const_name = _read_node() as ident;
			_const_definition.const_value = _read_node() as expression;
		}


		public void visit(consts_definitions_list _consts_definitions_list)
		{
			read_consts_definitions_list(_consts_definitions_list);
		}

		public void read_consts_definitions_list(consts_definitions_list _consts_definitions_list)
		{
			read_declaration(_consts_definitions_list);
			if (br.ReadByte() == 0)
			{
				_consts_definitions_list.const_defs = null;
			}
			else
			{
				_consts_definitions_list.const_defs = new List<const_definition>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_consts_definitions_list.const_defs.Add(_read_node() as const_definition);
				}
			}
		}


		public void visit(unit_name _unit_name)
		{
			read_unit_name(_unit_name);
		}

		public void read_unit_name(unit_name _unit_name)
		{
			read_syntax_tree_node(_unit_name);
			_unit_name.idunit_name = _read_node() as ident;
			_unit_name.HeaderKeyword = (UnitHeaderKeyword)br.ReadByte();
		}


		public void visit(unit_or_namespace _unit_or_namespace)
		{
			read_unit_or_namespace(_unit_or_namespace);
		}

		public void read_unit_or_namespace(unit_or_namespace _unit_or_namespace)
		{
			read_syntax_tree_node(_unit_or_namespace);
			_unit_or_namespace.name = _read_node() as ident_list;
		}


		public void visit(uses_unit_in _uses_unit_in)
		{
			read_uses_unit_in(_uses_unit_in);
		}

		public void read_uses_unit_in(uses_unit_in _uses_unit_in)
		{
			read_unit_or_namespace(_uses_unit_in);
			_uses_unit_in.in_file = _read_node() as string_const;
		}


		public void visit(uses_list _uses_list)
		{
			read_uses_list(_uses_list);
		}

		public void read_uses_list(uses_list _uses_list)
		{
			read_syntax_tree_node(_uses_list);
			if (br.ReadByte() == 0)
			{
				_uses_list.units = null;
			}
			else
			{
				_uses_list.units = new List<unit_or_namespace>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_uses_list.units.Add(_read_node() as unit_or_namespace);
				}
			}
		}


		public void visit(program_body _program_body)
		{
			read_program_body(_program_body);
		}

		public void read_program_body(program_body _program_body)
		{
			read_syntax_tree_node(_program_body);
			_program_body.used_units = _read_node() as uses_list;
			_program_body.program_definitions = _read_node() as declarations;
			_program_body.program_code = _read_node() as statement_list;
			_program_body.using_list = _read_node() as using_list;
		}


		public void visit(compilation_unit _compilation_unit)
		{
			read_compilation_unit(_compilation_unit);
		}

		public void read_compilation_unit(compilation_unit _compilation_unit)
		{
			read_syntax_tree_node(_compilation_unit);
			if (br.ReadByte() == 0)
			{
				_compilation_unit.file_name = null;
			}
			else
			{
				_compilation_unit.file_name = br.ReadString();
			}
			if (br.ReadByte() == 0)
			{
				_compilation_unit.compiler_directives = null;
			}
			else
			{
				_compilation_unit.compiler_directives = new List<compiler_directive>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_compilation_unit.compiler_directives.Add(_read_node() as compiler_directive);
				}
			}
			if (br.ReadByte() == 0)
			{
				_compilation_unit.Language = null;
			}
			else
			{
				_compilation_unit.Language = br.ReadString();
			}
		}


		public void visit(unit_module _unit_module)
		{
			read_unit_module(_unit_module);
		}

		public void read_unit_module(unit_module _unit_module)
		{
			read_compilation_unit(_unit_module);
			_unit_module.unit_name = _read_node() as unit_name;
			_unit_module.interface_part = _read_node() as interface_node;
			_unit_module.implementation_part = _read_node() as implementation_node;
			_unit_module.initialization_part = _read_node() as statement_list;
			_unit_module.finalization_part = _read_node() as statement_list;
			_unit_module.attributes = _read_node() as attribute_list;
		}


		public void visit(program_module _program_module)
		{
			read_program_module(_program_module);
		}

		public void read_program_module(program_module _program_module)
		{
			read_compilation_unit(_program_module);
			_program_module.program_name = _read_node() as program_name;
			_program_module.used_units = _read_node() as uses_list;
			_program_module.program_block = _read_node() as block;
			_program_module.using_namespaces = _read_node() as using_list;
		}


		public void visit(hex_constant _hex_constant)
		{
			read_hex_constant(_hex_constant);
		}

		public void read_hex_constant(hex_constant _hex_constant)
		{
			read_int64_const(_hex_constant);
		}


		public void visit(get_address _get_address)
		{
			read_get_address(_get_address);
		}

		public void read_get_address(get_address _get_address)
		{
			read_addressed_value_funcname(_get_address);
			_get_address.address_of = _read_node() as addressed_value;
		}


		public void visit(case_variant _case_variant)
		{
			read_case_variant(_case_variant);
		}

		public void read_case_variant(case_variant _case_variant)
		{
			read_statement(_case_variant);
			_case_variant.conditions = _read_node() as expression_list;
			_case_variant.exec_if_true = _read_node() as statement;
		}


		public void visit(case_node _case_node)
		{
			read_case_node(_case_node);
		}

		public void read_case_node(case_node _case_node)
		{
			read_statement(_case_node);
			_case_node.param = _read_node() as expression;
			_case_node.conditions = _read_node() as case_variants;
			_case_node.else_statement = _read_node() as statement;
		}


		public void visit(method_name _method_name)
		{
			read_method_name(_method_name);
		}

		public void read_method_name(method_name _method_name)
		{
			read_syntax_tree_node(_method_name);
			if (br.ReadByte() == 0)
			{
				_method_name.ln = null;
			}
			else
			{
				_method_name.ln = new List<ident>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_method_name.ln.Add(_read_node() as ident);
				}
			}
			_method_name.class_name = _read_node() as ident;
			_method_name.meth_name = _read_node() as ident;
			_method_name.explicit_interface_name = _read_node() as ident;
		}


		public void visit(dot_node _dot_node)
		{
			read_dot_node(_dot_node);
		}

		public void read_dot_node(dot_node _dot_node)
		{
			read_addressed_value_funcname(_dot_node);
			_dot_node.left = _read_node() as addressed_value;
			_dot_node.right = _read_node() as addressed_value;
		}


		public void visit(empty_statement _empty_statement)
		{
			read_empty_statement(_empty_statement);
		}

		public void read_empty_statement(empty_statement _empty_statement)
		{
			read_statement(_empty_statement);
		}


		public void visit(goto_statement _goto_statement)
		{
			read_goto_statement(_goto_statement);
		}

		public void read_goto_statement(goto_statement _goto_statement)
		{
			read_statement(_goto_statement);
			_goto_statement.label = _read_node() as ident;
		}


		public void visit(labeled_statement _labeled_statement)
		{
			read_labeled_statement(_labeled_statement);
		}

		public void read_labeled_statement(labeled_statement _labeled_statement)
		{
			read_statement(_labeled_statement);
			_labeled_statement.label_name = _read_node() as ident;
			_labeled_statement.to_statement = _read_node() as statement;
		}


		public void visit(with_statement _with_statement)
		{
			read_with_statement(_with_statement);
		}

		public void read_with_statement(with_statement _with_statement)
		{
			read_statement(_with_statement);
			_with_statement.what_do = _read_node() as statement;
			_with_statement.do_with = _read_node() as expression_list;
		}


		public void visit(method_call _method_call)
		{
			read_method_call(_method_call);
		}

		public void read_method_call(method_call _method_call)
		{
			read_dereference(_method_call);
			_method_call.parameters = _read_node() as expression_list;
		}


		public void visit(pascal_set_constant _pascal_set_constant)
		{
			read_pascal_set_constant(_pascal_set_constant);
		}

		public void read_pascal_set_constant(pascal_set_constant _pascal_set_constant)
		{
			read_addressed_value(_pascal_set_constant);
			_pascal_set_constant.values = _read_node() as expression_list;
		}


		public void visit(array_const _array_const)
		{
			read_array_const(_array_const);
		}

		public void read_array_const(array_const _array_const)
		{
			read_expression(_array_const);
			_array_const.elements = _read_node() as expression_list;
		}


		public void visit(write_accessor_name _write_accessor_name)
		{
			read_write_accessor_name(_write_accessor_name);
		}

		public void read_write_accessor_name(write_accessor_name _write_accessor_name)
		{
			read_syntax_tree_node(_write_accessor_name);
			_write_accessor_name.accessor_name = _read_node() as ident;
			_write_accessor_name.pr = _read_node() as procedure_definition;
			_write_accessor_name.statment_for_formatting = _read_node() as statement;
		}


		public void visit(read_accessor_name _read_accessor_name)
		{
			read_read_accessor_name(_read_accessor_name);
		}

		public void read_read_accessor_name(read_accessor_name _read_accessor_name)
		{
			read_syntax_tree_node(_read_accessor_name);
			_read_accessor_name.accessor_name = _read_node() as ident;
			_read_accessor_name.pr = _read_node() as procedure_definition;
			_read_accessor_name.expression_for_formatting = _read_node() as expression;
		}


		public void visit(property_accessors _property_accessors)
		{
			read_property_accessors(_property_accessors);
		}

		public void read_property_accessors(property_accessors _property_accessors)
		{
			read_syntax_tree_node(_property_accessors);
			_property_accessors.read_accessor = _read_node() as read_accessor_name;
			_property_accessors.write_accessor = _read_node() as write_accessor_name;
		}


		public void visit(simple_property _simple_property)
		{
			read_simple_property(_simple_property);
		}

		public void read_simple_property(simple_property _simple_property)
		{
			read_declaration(_simple_property);
			_simple_property.property_name = _read_node() as property_ident;
			_simple_property.property_type = _read_node() as type_definition;
			_simple_property.index_expression = _read_node() as expression;
			_simple_property.accessors = _read_node() as property_accessors;
			_simple_property.array_default = _read_node() as property_array_default;
			_simple_property.parameter_list = _read_node() as property_parameter_list;
			_simple_property.attr = (definition_attribute)br.ReadByte();
			_simple_property.virt_over_none_attr = (proc_attribute)br.ReadByte();
			_simple_property.is_auto = br.ReadBoolean();
			_simple_property.initial_value = _read_node() as expression;
		}


		public void visit(index_property _index_property)
		{
			read_index_property(_index_property);
		}

		public void read_index_property(index_property _index_property)
		{
			read_simple_property(_index_property);
			_index_property.property_parametres = _read_node() as formal_parameters;
			_index_property.is_default = _read_node() as default_indexer_property_node;
		}


		public void visit(class_members _class_members)
		{
			read_class_members(_class_members);
		}

		public void read_class_members(class_members _class_members)
		{
			read_syntax_tree_node(_class_members);
			if (br.ReadByte() == 0)
			{
				_class_members.members = null;
			}
			else
			{
				_class_members.members = new List<declaration>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_class_members.members.Add(_read_node() as declaration);
				}
			}
			_class_members.access_mod = _read_node() as access_modifer_node;
		}


		public void visit(access_modifer_node _access_modifer_node)
		{
			read_access_modifer_node(_access_modifer_node);
		}

		public void read_access_modifer_node(access_modifer_node _access_modifer_node)
		{
			read_syntax_tree_node(_access_modifer_node);
			_access_modifer_node.access_level = (access_modifer)br.ReadByte();
		}


		public void visit(class_body_list _class_body_list)
		{
			read_class_body_list(_class_body_list);
		}

		public void read_class_body_list(class_body_list _class_body_list)
		{
			read_syntax_tree_node(_class_body_list);
			if (br.ReadByte() == 0)
			{
				_class_body_list.class_def_blocks = null;
			}
			else
			{
				_class_body_list.class_def_blocks = new List<class_members>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_class_body_list.class_def_blocks.Add(_read_node() as class_members);
				}
			}
		}


		public void visit(class_definition _class_definition)
		{
			read_class_definition(_class_definition);
		}

		public void read_class_definition(class_definition _class_definition)
		{
			read_type_definition(_class_definition);
			_class_definition.class_parents = _read_node() as named_type_reference_list;
			_class_definition.body = _read_node() as class_body_list;
			_class_definition.keyword = (class_keyword)br.ReadByte();
			_class_definition.template_args = _read_node() as ident_list;
			_class_definition.where_section = _read_node() as where_definition_list;
			_class_definition.attribute = (class_attribute)br.ReadByte();
			_class_definition.is_auto = br.ReadBoolean();
		}


		public void visit(default_indexer_property_node _default_indexer_property_node)
		{
			read_default_indexer_property_node(_default_indexer_property_node);
		}

		public void read_default_indexer_property_node(default_indexer_property_node _default_indexer_property_node)
		{
			read_syntax_tree_node(_default_indexer_property_node);
		}


		public void visit(known_type_definition _known_type_definition)
		{
			read_known_type_definition(_known_type_definition);
		}

		public void read_known_type_definition(known_type_definition _known_type_definition)
		{
			read_type_definition(_known_type_definition);
			_known_type_definition.tp = (known_type)br.ReadByte();
			_known_type_definition.unit_name = _read_node() as ident;
		}


		public void visit(set_type_definition _set_type_definition)
		{
			read_set_type_definition(_set_type_definition);
		}

		public void read_set_type_definition(set_type_definition _set_type_definition)
		{
			read_type_definition(_set_type_definition);
			_set_type_definition.of_type = _read_node() as type_definition;
		}


		public void visit(record_const_definition _record_const_definition)
		{
			read_record_const_definition(_record_const_definition);
		}

		public void read_record_const_definition(record_const_definition _record_const_definition)
		{
			read_statement(_record_const_definition);
			_record_const_definition.name = _read_node() as ident;
			_record_const_definition.val = _read_node() as expression;
		}


		public void visit(record_const _record_const)
		{
			read_record_const(_record_const);
		}

		public void read_record_const(record_const _record_const)
		{
			read_expression(_record_const);
			if (br.ReadByte() == 0)
			{
				_record_const.rec_consts = null;
			}
			else
			{
				_record_const.rec_consts = new List<record_const_definition>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_record_const.rec_consts.Add(_read_node() as record_const_definition);
				}
			}
		}


		public void visit(record_type _record_type)
		{
			read_record_type(_record_type);
		}

		public void read_record_type(record_type _record_type)
		{
			read_type_definition(_record_type);
			_record_type.parts = _read_node() as record_type_parts;
			_record_type.base_type = _read_node() as type_definition;
		}


		public void visit(enum_type_definition _enum_type_definition)
		{
			read_enum_type_definition(_enum_type_definition);
		}

		public void read_enum_type_definition(enum_type_definition _enum_type_definition)
		{
			read_type_definition(_enum_type_definition);
			_enum_type_definition.enumerators = _read_node() as enumerator_list;
		}


		public void visit(char_const _char_const)
		{
			read_char_const(_char_const);
		}

		public void read_char_const(char_const _char_const)
		{
			read_literal(_char_const);
			_char_const.cconst = br.ReadChar();
		}


		public void visit(raise_statement _raise_statement)
		{
			read_raise_statement(_raise_statement);
		}

		public void read_raise_statement(raise_statement _raise_statement)
		{
			read_statement(_raise_statement);
			_raise_statement.excep = _read_node() as expression;
		}


		public void visit(sharp_char_const _sharp_char_const)
		{
			read_sharp_char_const(_sharp_char_const);
		}

		public void read_sharp_char_const(sharp_char_const _sharp_char_const)
		{
			read_literal(_sharp_char_const);
			_sharp_char_const.char_num = (int)br.ReadByte();
		}


		public void visit(literal_const_line _literal_const_line)
		{
			read_literal_const_line(_literal_const_line);
		}

		public void read_literal_const_line(literal_const_line _literal_const_line)
		{
			read_literal(_literal_const_line);
			if (br.ReadByte() == 0)
			{
				_literal_const_line.literals = null;
			}
			else
			{
				_literal_const_line.literals = new List<literal>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_literal_const_line.literals.Add(_read_node() as literal);
				}
			}
		}


		public void visit(string_num_definition _string_num_definition)
		{
			read_string_num_definition(_string_num_definition);
		}

		public void read_string_num_definition(string_num_definition _string_num_definition)
		{
			read_type_definition(_string_num_definition);
			_string_num_definition.num_of_symbols = _read_node() as expression;
			_string_num_definition.name = _read_node() as ident;
		}


		public void visit(variant _variant)
		{
			read_variant(_variant);
		}

		public void read_variant(variant _variant)
		{
			read_syntax_tree_node(_variant);
			_variant.vars = _read_node() as ident_list;
			_variant.vars_type = _read_node() as type_definition;
		}


		public void visit(variant_list _variant_list)
		{
			read_variant_list(_variant_list);
		}

		public void read_variant_list(variant_list _variant_list)
		{
			read_syntax_tree_node(_variant_list);
			if (br.ReadByte() == 0)
			{
				_variant_list.vars = null;
			}
			else
			{
				_variant_list.vars = new List<variant>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_variant_list.vars.Add(_read_node() as variant);
				}
			}
		}


		public void visit(variant_type _variant_type)
		{
			read_variant_type(_variant_type);
		}

		public void read_variant_type(variant_type _variant_type)
		{
			read_syntax_tree_node(_variant_type);
			_variant_type.case_exprs = _read_node() as expression_list;
			_variant_type.parts = _read_node() as record_type_parts;
		}


		public void visit(variant_types _variant_types)
		{
			read_variant_types(_variant_types);
		}

		public void read_variant_types(variant_types _variant_types)
		{
			read_syntax_tree_node(_variant_types);
			if (br.ReadByte() == 0)
			{
				_variant_types.vars = null;
			}
			else
			{
				_variant_types.vars = new List<variant_type>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_variant_types.vars.Add(_read_node() as variant_type);
				}
			}
		}


		public void visit(variant_record_type _variant_record_type)
		{
			read_variant_record_type(_variant_record_type);
		}

		public void read_variant_record_type(variant_record_type _variant_record_type)
		{
			read_syntax_tree_node(_variant_record_type);
			_variant_record_type.var_name = _read_node() as ident;
			_variant_record_type.var_type = _read_node() as type_definition;
			_variant_record_type.vars = _read_node() as variant_types;
		}


		public void visit(procedure_call _procedure_call)
		{
			read_procedure_call(_procedure_call);
		}

		public void read_procedure_call(procedure_call _procedure_call)
		{
			read_statement(_procedure_call);
			_procedure_call.func_name = _read_node() as addressed_value;
			_procedure_call.is_ident = br.ReadBoolean();
		}


		public void visit(class_predefinition _class_predefinition)
		{
			read_class_predefinition(_class_predefinition);
		}

		public void read_class_predefinition(class_predefinition _class_predefinition)
		{
			read_type_declaration(_class_predefinition);
			_class_predefinition.class_name = _read_node() as ident;
		}


		public void visit(nil_const _nil_const)
		{
			read_nil_const(_nil_const);
		}

		public void read_nil_const(nil_const _nil_const)
		{
			read_const_node(_nil_const);
		}


		public void visit(file_type_definition _file_type_definition)
		{
			read_file_type_definition(_file_type_definition);
		}

		public void read_file_type_definition(file_type_definition _file_type_definition)
		{
			read_type_definition(_file_type_definition);
			_file_type_definition.elem_type = _read_node() as type_definition;
		}


		public void visit(constructor _constructor)
		{
			read_constructor(_constructor);
		}

		public void read_constructor(constructor _constructor)
		{
			read_procedure_header(_constructor);
		}


		public void visit(destructor _destructor)
		{
			read_destructor(_destructor);
		}

		public void read_destructor(destructor _destructor)
		{
			read_procedure_header(_destructor);
		}


		public void visit(inherited_method_call _inherited_method_call)
		{
			read_inherited_method_call(_inherited_method_call);
		}

		public void read_inherited_method_call(inherited_method_call _inherited_method_call)
		{
			read_statement(_inherited_method_call);
			_inherited_method_call.method_name = _read_node() as ident;
			_inherited_method_call.exprs = _read_node() as expression_list;
		}


		public void visit(typecast_node _typecast_node)
		{
			read_typecast_node(_typecast_node);
		}

		public void read_typecast_node(typecast_node _typecast_node)
		{
			read_addressed_value(_typecast_node);
			_typecast_node.expr = _read_node() as addressed_value;
			_typecast_node.type_def = _read_node() as type_definition;
			_typecast_node.cast_op = (op_typecast)br.ReadByte();
		}


		public void visit(interface_node _interface_node)
		{
			read_interface_node(_interface_node);
		}

		public void read_interface_node(interface_node _interface_node)
		{
			read_syntax_tree_node(_interface_node);
			_interface_node.interface_definitions = _read_node() as declarations;
			_interface_node.uses_modules = _read_node() as uses_list;
			_interface_node.using_namespaces = _read_node() as using_list;
		}


		public void visit(implementation_node _implementation_node)
		{
			read_implementation_node(_implementation_node);
		}

		public void read_implementation_node(implementation_node _implementation_node)
		{
			read_syntax_tree_node(_implementation_node);
			_implementation_node.uses_modules = _read_node() as uses_list;
			_implementation_node.implementation_definitions = _read_node() as declarations;
			_implementation_node.using_namespaces = _read_node() as using_list;
		}


		public void visit(diap_expr _diap_expr)
		{
			read_diap_expr(_diap_expr);
		}

		public void read_diap_expr(diap_expr _diap_expr)
		{
			read_expression(_diap_expr);
			_diap_expr.left = _read_node() as expression;
			_diap_expr.right = _read_node() as expression;
		}


		public void visit(block _block)
		{
			read_block(_block);
		}

		public void read_block(block _block)
		{
			read_proc_block(_block);
			_block.defs = _read_node() as declarations;
			_block.program_code = _read_node() as statement_list;
		}


		public void visit(proc_block _proc_block)
		{
			read_proc_block(_proc_block);
		}

		public void read_proc_block(proc_block _proc_block)
		{
			read_syntax_tree_node(_proc_block);
		}


		public void visit(array_of_named_type_definition _array_of_named_type_definition)
		{
			read_array_of_named_type_definition(_array_of_named_type_definition);
		}

		public void read_array_of_named_type_definition(array_of_named_type_definition _array_of_named_type_definition)
		{
			read_type_definition(_array_of_named_type_definition);
			_array_of_named_type_definition.type_name = _read_node() as named_type_reference;
		}


		public void visit(array_of_const_type_definition _array_of_const_type_definition)
		{
			read_array_of_const_type_definition(_array_of_const_type_definition);
		}

		public void read_array_of_const_type_definition(array_of_const_type_definition _array_of_const_type_definition)
		{
			read_type_definition(_array_of_const_type_definition);
		}


		public void visit(literal _literal)
		{
			read_literal(_literal);
		}

		public void read_literal(literal _literal)
		{
			read_const_node(_literal);
		}


		public void visit(case_variants _case_variants)
		{
			read_case_variants(_case_variants);
		}

		public void read_case_variants(case_variants _case_variants)
		{
			read_syntax_tree_node(_case_variants);
			if (br.ReadByte() == 0)
			{
				_case_variants.variants = null;
			}
			else
			{
				_case_variants.variants = new List<case_variant>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_case_variants.variants.Add(_read_node() as case_variant);
				}
			}
		}


		public void visit(diapason_expr _diapason_expr)
		{
			read_diapason_expr(_diapason_expr);
		}

		public void read_diapason_expr(diapason_expr _diapason_expr)
		{
			read_expression(_diapason_expr);
			_diapason_expr.left = _read_node() as expression;
			_diapason_expr.right = _read_node() as expression;
		}


		public void visit(var_def_list_for_record _var_def_list_for_record)
		{
			read_var_def_list_for_record(_var_def_list_for_record);
		}

		public void read_var_def_list_for_record(var_def_list_for_record _var_def_list_for_record)
		{
			read_syntax_tree_node(_var_def_list_for_record);
			if (br.ReadByte() == 0)
			{
				_var_def_list_for_record.vars = null;
			}
			else
			{
				_var_def_list_for_record.vars = new List<var_def_statement>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_var_def_list_for_record.vars.Add(_read_node() as var_def_statement);
				}
			}
		}


		public void visit(record_type_parts _record_type_parts)
		{
			read_record_type_parts(_record_type_parts);
		}

		public void read_record_type_parts(record_type_parts _record_type_parts)
		{
			read_syntax_tree_node(_record_type_parts);
			_record_type_parts.fixed_part = _read_node() as var_def_list_for_record;
			_record_type_parts.variant_part = _read_node() as variant_record_type;
		}


		public void visit(property_array_default _property_array_default)
		{
			read_property_array_default(_property_array_default);
		}

		public void read_property_array_default(property_array_default _property_array_default)
		{
			read_syntax_tree_node(_property_array_default);
		}


		public void visit(property_interface _property_interface)
		{
			read_property_interface(_property_interface);
		}

		public void read_property_interface(property_interface _property_interface)
		{
			read_syntax_tree_node(_property_interface);
			_property_interface.parameter_list = _read_node() as property_parameter_list;
			_property_interface.property_type = _read_node() as type_definition;
			_property_interface.index_expression = _read_node() as expression;
		}


		public void visit(property_parameter _property_parameter)
		{
			read_property_parameter(_property_parameter);
		}

		public void read_property_parameter(property_parameter _property_parameter)
		{
			read_syntax_tree_node(_property_parameter);
			_property_parameter.names = _read_node() as ident_list;
			_property_parameter.type = _read_node() as type_definition;
		}


		public void visit(property_parameter_list _property_parameter_list)
		{
			read_property_parameter_list(_property_parameter_list);
		}

		public void read_property_parameter_list(property_parameter_list _property_parameter_list)
		{
			read_syntax_tree_node(_property_parameter_list);
			if (br.ReadByte() == 0)
			{
				_property_parameter_list.parameters = null;
			}
			else
			{
				_property_parameter_list.parameters = new List<property_parameter>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_property_parameter_list.parameters.Add(_read_node() as property_parameter);
				}
			}
		}


		public void visit(inherited_ident _inherited_ident)
		{
			read_inherited_ident(_inherited_ident);
		}

		public void read_inherited_ident(inherited_ident _inherited_ident)
		{
			read_ident(_inherited_ident);
		}


		public void visit(format_expr _format_expr)
		{
			read_format_expr(_format_expr);
		}

		public void read_format_expr(format_expr _format_expr)
		{
			read_addressed_value(_format_expr);
			_format_expr.expr = _read_node() as expression;
			_format_expr.format1 = _read_node() as expression;
			_format_expr.format2 = _read_node() as expression;
		}


		public void visit(initfinal_part _initfinal_part)
		{
			read_initfinal_part(_initfinal_part);
		}

		public void read_initfinal_part(initfinal_part _initfinal_part)
		{
			read_syntax_tree_node(_initfinal_part);
			_initfinal_part.initialization_sect = _read_node() as statement_list;
			_initfinal_part.finalization_sect = _read_node() as statement_list;
		}


		public void visit(token_info _token_info)
		{
			read_token_info(_token_info);
		}

		public void read_token_info(token_info _token_info)
		{
			read_syntax_tree_node(_token_info);
			if (br.ReadByte() == 0)
			{
				_token_info.text = null;
			}
			else
			{
				_token_info.text = br.ReadString();
			}
		}


		public void visit(raise_stmt _raise_stmt)
		{
			read_raise_stmt(_raise_stmt);
		}

		public void read_raise_stmt(raise_stmt _raise_stmt)
		{
			read_statement(_raise_stmt);
			_raise_stmt.expr = _read_node() as expression;
			_raise_stmt.address = _read_node() as expression;
		}


		public void visit(op_type_node _op_type_node)
		{
			read_op_type_node(_op_type_node);
		}

		public void read_op_type_node(op_type_node _op_type_node)
		{
			read_token_info(_op_type_node);
			_op_type_node.type = (Operators)br.ReadByte();
		}


		public void visit(file_type _file_type)
		{
			read_file_type(_file_type);
		}

		public void read_file_type(file_type _file_type)
		{
			read_type_definition(_file_type);
			_file_type.file_of_type = _read_node() as type_definition;
		}


		public void visit(known_type_ident _known_type_ident)
		{
			read_known_type_ident(_known_type_ident);
		}

		public void read_known_type_ident(known_type_ident _known_type_ident)
		{
			read_ident(_known_type_ident);
			_known_type_ident.type = (known_type)br.ReadByte();
		}


		public void visit(exception_handler _exception_handler)
		{
			read_exception_handler(_exception_handler);
		}

		public void read_exception_handler(exception_handler _exception_handler)
		{
			read_syntax_tree_node(_exception_handler);
			_exception_handler.variable = _read_node() as ident;
			_exception_handler.type_name = _read_node() as named_type_reference;
			_exception_handler.statements = _read_node() as statement;
		}


		public void visit(exception_ident _exception_ident)
		{
			read_exception_ident(_exception_ident);
		}

		public void read_exception_ident(exception_ident _exception_ident)
		{
			read_syntax_tree_node(_exception_ident);
			_exception_ident.variable = _read_node() as ident;
			_exception_ident.type_name = _read_node() as named_type_reference;
		}


		public void visit(exception_handler_list _exception_handler_list)
		{
			read_exception_handler_list(_exception_handler_list);
		}

		public void read_exception_handler_list(exception_handler_list _exception_handler_list)
		{
			read_syntax_tree_node(_exception_handler_list);
			if (br.ReadByte() == 0)
			{
				_exception_handler_list.handlers = null;
			}
			else
			{
				_exception_handler_list.handlers = new List<exception_handler>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_exception_handler_list.handlers.Add(_read_node() as exception_handler);
				}
			}
		}


		public void visit(exception_block _exception_block)
		{
			read_exception_block(_exception_block);
		}

		public void read_exception_block(exception_block _exception_block)
		{
			read_syntax_tree_node(_exception_block);
			_exception_block.stmt_list = _read_node() as statement_list;
			_exception_block.handlers = _read_node() as exception_handler_list;
			_exception_block.else_stmt_list = _read_node() as statement_list;
		}


		public void visit(try_handler _try_handler)
		{
			read_try_handler(_try_handler);
		}

		public void read_try_handler(try_handler _try_handler)
		{
			read_syntax_tree_node(_try_handler);
		}


		public void visit(try_handler_finally _try_handler_finally)
		{
			read_try_handler_finally(_try_handler_finally);
		}

		public void read_try_handler_finally(try_handler_finally _try_handler_finally)
		{
			read_try_handler(_try_handler_finally);
			_try_handler_finally.stmt_list = _read_node() as statement_list;
		}


		public void visit(try_handler_except _try_handler_except)
		{
			read_try_handler_except(_try_handler_except);
		}

		public void read_try_handler_except(try_handler_except _try_handler_except)
		{
			read_try_handler(_try_handler_except);
			_try_handler_except.except_block = _read_node() as exception_block;
		}


		public void visit(try_stmt _try_stmt)
		{
			read_try_stmt(_try_stmt);
		}

		public void read_try_stmt(try_stmt _try_stmt)
		{
			read_statement(_try_stmt);
			_try_stmt.stmt_list = _read_node() as statement_list;
			_try_stmt.handler = _read_node() as try_handler;
		}


		public void visit(inherited_message _inherited_message)
		{
			read_inherited_message(_inherited_message);
		}

		public void read_inherited_message(inherited_message _inherited_message)
		{
			read_statement(_inherited_message);
		}


		public void visit(external_directive _external_directive)
		{
			read_external_directive(_external_directive);
		}

		public void read_external_directive(external_directive _external_directive)
		{
			read_proc_block(_external_directive);
			_external_directive.modulename = _read_node() as expression;
			_external_directive.name = _read_node() as expression;
		}


		public void visit(using_list _using_list)
		{
			read_using_list(_using_list);
		}

		public void read_using_list(using_list _using_list)
		{
			read_syntax_tree_node(_using_list);
			if (br.ReadByte() == 0)
			{
				_using_list.namespaces = null;
			}
			else
			{
				_using_list.namespaces = new List<unit_or_namespace>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_using_list.namespaces.Add(_read_node() as unit_or_namespace);
				}
			}
		}


		public void visit(jump_stmt _jump_stmt)
		{
			read_jump_stmt(_jump_stmt);
		}

		public void read_jump_stmt(jump_stmt _jump_stmt)
		{
			read_statement(_jump_stmt);
			_jump_stmt.expr = _read_node() as expression;
			_jump_stmt.JumpType = (JumpStmtType)br.ReadByte();
		}


		public void visit(loop_stmt _loop_stmt)
		{
			read_loop_stmt(_loop_stmt);
		}

		public void read_loop_stmt(loop_stmt _loop_stmt)
		{
			read_statement(_loop_stmt);
			_loop_stmt.count = _read_node() as expression;
			_loop_stmt.stmt = _read_node() as statement;
		}


		public void visit(foreach_stmt _foreach_stmt)
		{
			read_foreach_stmt(_foreach_stmt);
		}

		public void read_foreach_stmt(foreach_stmt _foreach_stmt)
		{
			read_statement(_foreach_stmt);
			_foreach_stmt.identifier = _read_node() as ident;
			_foreach_stmt.type_name = _read_node() as type_definition;
			_foreach_stmt.in_what = _read_node() as expression;
			_foreach_stmt.stmt = _read_node() as statement;
			_foreach_stmt.index = _read_node() as ident;
		}


		public void visit(addressed_value_funcname _addressed_value_funcname)
		{
			read_addressed_value_funcname(_addressed_value_funcname);
		}

		public void read_addressed_value_funcname(addressed_value_funcname _addressed_value_funcname)
		{
			read_addressed_value(_addressed_value_funcname);
		}


		public void visit(named_type_reference_list _named_type_reference_list)
		{
			read_named_type_reference_list(_named_type_reference_list);
		}

		public void read_named_type_reference_list(named_type_reference_list _named_type_reference_list)
		{
			read_syntax_tree_node(_named_type_reference_list);
			if (br.ReadByte() == 0)
			{
				_named_type_reference_list.types = null;
			}
			else
			{
				_named_type_reference_list.types = new List<named_type_reference>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_named_type_reference_list.types.Add(_read_node() as named_type_reference);
				}
			}
		}


		public void visit(template_param_list _template_param_list)
		{
			read_template_param_list(_template_param_list);
		}

		public void read_template_param_list(template_param_list _template_param_list)
		{
			read_dereference(_template_param_list);
			if (br.ReadByte() == 0)
			{
				_template_param_list.params_list = null;
			}
			else
			{
				_template_param_list.params_list = new List<type_definition>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_template_param_list.params_list.Add(_read_node() as type_definition);
				}
			}
		}


		public void visit(template_type_reference _template_type_reference)
		{
			read_template_type_reference(_template_type_reference);
		}

		public void read_template_type_reference(template_type_reference _template_type_reference)
		{
			read_named_type_reference(_template_type_reference);
			_template_type_reference.name = _read_node() as named_type_reference;
			_template_type_reference.params_list = _read_node() as template_param_list;
		}


		public void visit(int64_const _int64_const)
		{
			read_int64_const(_int64_const);
		}

		public void read_int64_const(int64_const _int64_const)
		{
			read_const_node(_int64_const);
			_int64_const.val = br.ReadInt64();
		}


		public void visit(uint64_const _uint64_const)
		{
			read_uint64_const(_uint64_const);
		}

		public void read_uint64_const(uint64_const _uint64_const)
		{
			read_const_node(_uint64_const);
			_uint64_const.val = br.ReadUInt64();
		}


		public void visit(new_expr _new_expr)
		{
			read_new_expr(_new_expr);
		}

		public void read_new_expr(new_expr _new_expr)
		{
			read_addressed_value(_new_expr);
			_new_expr.type = _read_node() as type_definition;
			_new_expr.params_list = _read_node() as expression_list;
			_new_expr.new_array = br.ReadBoolean();
			_new_expr.array_init_expr = _read_node() as array_const;
		}


		public void visit(where_type_specificator_list _where_type_specificator_list)
		{
			read_where_type_specificator_list(_where_type_specificator_list);
		}

		public void read_where_type_specificator_list(where_type_specificator_list _where_type_specificator_list)
		{
			read_syntax_tree_node(_where_type_specificator_list);
			if (br.ReadByte() == 0)
			{
				_where_type_specificator_list.defs = null;
			}
			else
			{
				_where_type_specificator_list.defs = new List<type_definition>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_where_type_specificator_list.defs.Add(_read_node() as type_definition);
				}
			}
		}


		public void visit(where_definition _where_definition)
		{
			read_where_definition(_where_definition);
		}

		public void read_where_definition(where_definition _where_definition)
		{
			read_syntax_tree_node(_where_definition);
			_where_definition.names = _read_node() as ident_list;
			_where_definition.types = _read_node() as where_type_specificator_list;
		}


		public void visit(where_definition_list _where_definition_list)
		{
			read_where_definition_list(_where_definition_list);
		}

		public void read_where_definition_list(where_definition_list _where_definition_list)
		{
			read_syntax_tree_node(_where_definition_list);
			if (br.ReadByte() == 0)
			{
				_where_definition_list.defs = null;
			}
			else
			{
				_where_definition_list.defs = new List<where_definition>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_where_definition_list.defs.Add(_read_node() as where_definition);
				}
			}
		}


		public void visit(sizeof_operator _sizeof_operator)
		{
			read_sizeof_operator(_sizeof_operator);
		}

		public void read_sizeof_operator(sizeof_operator _sizeof_operator)
		{
			read_addressed_value(_sizeof_operator);
			_sizeof_operator.type_def = _read_node() as type_definition;
			_sizeof_operator.expr = _read_node() as expression;
		}


		public void visit(typeof_operator _typeof_operator)
		{
			read_typeof_operator(_typeof_operator);
		}

		public void read_typeof_operator(typeof_operator _typeof_operator)
		{
			read_addressed_value(_typeof_operator);
			_typeof_operator.type_name = _read_node() as named_type_reference;
		}


		public void visit(compiler_directive _compiler_directive)
		{
			read_compiler_directive(_compiler_directive);
		}

		public void read_compiler_directive(compiler_directive _compiler_directive)
		{
			read_syntax_tree_node(_compiler_directive);
			_compiler_directive.Name = _read_node() as token_info;
			_compiler_directive.Directive = _read_node() as token_info;
		}


		public void visit(operator_name_ident _operator_name_ident)
		{
			read_operator_name_ident(_operator_name_ident);
		}

		public void read_operator_name_ident(operator_name_ident _operator_name_ident)
		{
			read_ident(_operator_name_ident);
			_operator_name_ident.operator_type = (Operators)br.ReadByte();
		}


		public void visit(var_statement _var_statement)
		{
			read_var_statement(_var_statement);
		}

		public void read_var_statement(var_statement _var_statement)
		{
			read_statement(_var_statement);
			_var_statement.var_def = _read_node() as var_def_statement;
		}


		public void visit(question_colon_expression _question_colon_expression)
		{
			read_question_colon_expression(_question_colon_expression);
		}

		public void read_question_colon_expression(question_colon_expression _question_colon_expression)
		{
			read_addressed_value(_question_colon_expression);
			_question_colon_expression.condition = _read_node() as expression;
			_question_colon_expression.ret_if_true = _read_node() as expression;
			_question_colon_expression.ret_if_false = _read_node() as expression;
		}


		public void visit(expression_as_statement _expression_as_statement)
		{
			read_expression_as_statement(_expression_as_statement);
		}

		public void read_expression_as_statement(expression_as_statement _expression_as_statement)
		{
			read_statement(_expression_as_statement);
			_expression_as_statement.expr = _read_node() as expression;
		}


		public void visit(c_scalar_type _c_scalar_type)
		{
			read_c_scalar_type(_c_scalar_type);
		}

		public void read_c_scalar_type(c_scalar_type _c_scalar_type)
		{
			read_type_definition(_c_scalar_type);
			_c_scalar_type.scalar_name = (c_scalar_type_name)br.ReadByte();
			_c_scalar_type.sign = (c_scalar_sign)br.ReadByte();
		}


		public void visit(c_module _c_module)
		{
			read_c_module(_c_module);
		}

		public void read_c_module(c_module _c_module)
		{
			read_compilation_unit(_c_module);
			_c_module.defs = _read_node() as declarations;
			_c_module.used_units = _read_node() as uses_list;
		}


		public void visit(declarations_as_statement _declarations_as_statement)
		{
			read_declarations_as_statement(_declarations_as_statement);
		}

		public void read_declarations_as_statement(declarations_as_statement _declarations_as_statement)
		{
			read_statement(_declarations_as_statement);
			_declarations_as_statement.defs = _read_node() as declarations;
		}


		public void visit(array_size _array_size)
		{
			read_array_size(_array_size);
		}

		public void read_array_size(array_size _array_size)
		{
			read_type_definition(_array_size);
			_array_size.max_value = _read_node() as expression;
		}


		public void visit(enumerator _enumerator)
		{
			read_enumerator(_enumerator);
		}

		public void read_enumerator(enumerator _enumerator)
		{
			read_syntax_tree_node(_enumerator);
			_enumerator.name = _read_node() as type_definition;
			_enumerator.value = _read_node() as expression;
		}


		public void visit(enumerator_list _enumerator_list)
		{
			read_enumerator_list(_enumerator_list);
		}

		public void read_enumerator_list(enumerator_list _enumerator_list)
		{
			read_syntax_tree_node(_enumerator_list);
			if (br.ReadByte() == 0)
			{
				_enumerator_list.enumerators = null;
			}
			else
			{
				_enumerator_list.enumerators = new List<enumerator>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_enumerator_list.enumerators.Add(_read_node() as enumerator);
				}
			}
		}


		public void visit(c_for_cycle _c_for_cycle)
		{
			read_c_for_cycle(_c_for_cycle);
		}

		public void read_c_for_cycle(c_for_cycle _c_for_cycle)
		{
			read_statement(_c_for_cycle);
			_c_for_cycle.expr1 = _read_node() as statement;
			_c_for_cycle.expr2 = _read_node() as expression;
			_c_for_cycle.expr3 = _read_node() as expression;
			_c_for_cycle.stmt = _read_node() as statement;
		}


		public void visit(switch_stmt _switch_stmt)
		{
			read_switch_stmt(_switch_stmt);
		}

		public void read_switch_stmt(switch_stmt _switch_stmt)
		{
			read_statement(_switch_stmt);
			_switch_stmt.condition = _read_node() as expression;
			_switch_stmt.stmt = _read_node() as statement;
			_switch_stmt.Part = (SwitchPartType)br.ReadByte();
		}


		public void visit(type_definition_attr_list _type_definition_attr_list)
		{
			read_type_definition_attr_list(_type_definition_attr_list);
		}

		public void read_type_definition_attr_list(type_definition_attr_list _type_definition_attr_list)
		{
			read_syntax_tree_node(_type_definition_attr_list);
			if (br.ReadByte() == 0)
			{
				_type_definition_attr_list.attributes = null;
			}
			else
			{
				_type_definition_attr_list.attributes = new List<type_definition_attr>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_type_definition_attr_list.attributes.Add(_read_node() as type_definition_attr);
				}
			}
		}


		public void visit(type_definition_attr _type_definition_attr)
		{
			read_type_definition_attr(_type_definition_attr);
		}

		public void read_type_definition_attr(type_definition_attr _type_definition_attr)
		{
			read_type_definition(_type_definition_attr);
			_type_definition_attr.attr = (definition_attribute)br.ReadByte();
		}


		public void visit(lock_stmt _lock_stmt)
		{
			read_lock_stmt(_lock_stmt);
		}

		public void read_lock_stmt(lock_stmt _lock_stmt)
		{
			read_statement(_lock_stmt);
			_lock_stmt.lock_object = _read_node() as expression;
			_lock_stmt.stmt = _read_node() as statement;
		}


		public void visit(compiler_directive_list _compiler_directive_list)
		{
			read_compiler_directive_list(_compiler_directive_list);
		}

		public void read_compiler_directive_list(compiler_directive_list _compiler_directive_list)
		{
			read_compiler_directive(_compiler_directive_list);
			if (br.ReadByte() == 0)
			{
				_compiler_directive_list.directives = null;
			}
			else
			{
				_compiler_directive_list.directives = new List<compiler_directive>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_compiler_directive_list.directives.Add(_read_node() as compiler_directive);
				}
			}
		}


		public void visit(compiler_directive_if _compiler_directive_if)
		{
			read_compiler_directive_if(_compiler_directive_if);
		}

		public void read_compiler_directive_if(compiler_directive_if _compiler_directive_if)
		{
			read_compiler_directive(_compiler_directive_if);
			_compiler_directive_if.if_part = _read_node() as compiler_directive;
			_compiler_directive_if.elseif_part = _read_node() as compiler_directive;
		}


		public void visit(documentation_comment_list _documentation_comment_list)
		{
			read_documentation_comment_list(_documentation_comment_list);
		}

		public void read_documentation_comment_list(documentation_comment_list _documentation_comment_list)
		{
			read_syntax_tree_node(_documentation_comment_list);
			if (br.ReadByte() == 0)
			{
				_documentation_comment_list.sections = null;
			}
			else
			{
				_documentation_comment_list.sections = new List<documentation_comment_section>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_documentation_comment_list.sections.Add(_read_node() as documentation_comment_section);
				}
			}
		}


		public void visit(documentation_comment_tag _documentation_comment_tag)
		{
			read_documentation_comment_tag(_documentation_comment_tag);
		}

		public void read_documentation_comment_tag(documentation_comment_tag _documentation_comment_tag)
		{
			read_syntax_tree_node(_documentation_comment_tag);
			if (br.ReadByte() == 0)
			{
				_documentation_comment_tag.name = null;
			}
			else
			{
				_documentation_comment_tag.name = br.ReadString();
			}
			if (br.ReadByte() == 0)
			{
				_documentation_comment_tag.parameters = null;
			}
			else
			{
				_documentation_comment_tag.parameters = new List<documentation_comment_tag_param>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_documentation_comment_tag.parameters.Add(_read_node() as documentation_comment_tag_param);
				}
			}
			if (br.ReadByte() == 0)
			{
				_documentation_comment_tag.text = null;
			}
			else
			{
				_documentation_comment_tag.text = br.ReadString();
			}
		}


		public void visit(documentation_comment_tag_param _documentation_comment_tag_param)
		{
			read_documentation_comment_tag_param(_documentation_comment_tag_param);
		}

		public void read_documentation_comment_tag_param(documentation_comment_tag_param _documentation_comment_tag_param)
		{
			read_syntax_tree_node(_documentation_comment_tag_param);
			if (br.ReadByte() == 0)
			{
				_documentation_comment_tag_param.name = null;
			}
			else
			{
				_documentation_comment_tag_param.name = br.ReadString();
			}
			if (br.ReadByte() == 0)
			{
				_documentation_comment_tag_param.value = null;
			}
			else
			{
				_documentation_comment_tag_param.value = br.ReadString();
			}
		}


		public void visit(documentation_comment_section _documentation_comment_section)
		{
			read_documentation_comment_section(_documentation_comment_section);
		}

		public void read_documentation_comment_section(documentation_comment_section _documentation_comment_section)
		{
			read_syntax_tree_node(_documentation_comment_section);
			if (br.ReadByte() == 0)
			{
				_documentation_comment_section.tags = null;
			}
			else
			{
				_documentation_comment_section.tags = new List<documentation_comment_tag>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_documentation_comment_section.tags.Add(_read_node() as documentation_comment_tag);
				}
			}
			if (br.ReadByte() == 0)
			{
				_documentation_comment_section.text = null;
			}
			else
			{
				_documentation_comment_section.text = br.ReadString();
			}
		}


		public void visit(token_taginfo _token_taginfo)
		{
			read_token_taginfo(_token_taginfo);
		}

		public void read_token_taginfo(token_taginfo _token_taginfo)
		{
			read_token_info(_token_taginfo);
			_token_taginfo.tag = (object)br.ReadByte();
		}


		public void visit(declaration_specificator _declaration_specificator)
		{
			read_declaration_specificator(_declaration_specificator);
		}

		public void read_declaration_specificator(declaration_specificator _declaration_specificator)
		{
			read_type_definition(_declaration_specificator);
			_declaration_specificator.specificator = (DeclarationSpecificator)br.ReadByte();
			if (br.ReadByte() == 0)
			{
				_declaration_specificator.name = null;
			}
			else
			{
				_declaration_specificator.name = br.ReadString();
			}
		}


		public void visit(ident_with_templateparams _ident_with_templateparams)
		{
			read_ident_with_templateparams(_ident_with_templateparams);
		}

		public void read_ident_with_templateparams(ident_with_templateparams _ident_with_templateparams)
		{
			read_addressed_value_funcname(_ident_with_templateparams);
			_ident_with_templateparams.name = _read_node() as addressed_value;
			_ident_with_templateparams.template_params = _read_node() as template_param_list;
		}


		public void visit(template_type_name _template_type_name)
		{
			read_template_type_name(_template_type_name);
		}

		public void read_template_type_name(template_type_name _template_type_name)
		{
			read_ident(_template_type_name);
			_template_type_name.template_args = _read_node() as ident_list;
		}


		public void visit(default_operator _default_operator)
		{
			read_default_operator(_default_operator);
		}

		public void read_default_operator(default_operator _default_operator)
		{
			read_addressed_value(_default_operator);
			_default_operator.type_name = _read_node() as named_type_reference;
		}


		public void visit(bracket_expr _bracket_expr)
		{
			read_bracket_expr(_bracket_expr);
		}

		public void read_bracket_expr(bracket_expr _bracket_expr)
		{
			read_addressed_value(_bracket_expr);
			_bracket_expr.expr = _read_node() as expression;
		}


		public void visit(attribute _attribute)
		{
			read_attribute(_attribute);
		}

		public void read_attribute(attribute _attribute)
		{
			read_syntax_tree_node(_attribute);
			_attribute.qualifier = _read_node() as ident;
			_attribute.type = _read_node() as named_type_reference;
			_attribute.arguments = _read_node() as expression_list;
		}


		public void visit(simple_attribute_list _simple_attribute_list)
		{
			read_simple_attribute_list(_simple_attribute_list);
		}

		public void read_simple_attribute_list(simple_attribute_list _simple_attribute_list)
		{
			read_syntax_tree_node(_simple_attribute_list);
			if (br.ReadByte() == 0)
			{
				_simple_attribute_list.attributes = null;
			}
			else
			{
				_simple_attribute_list.attributes = new List<attribute>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_simple_attribute_list.attributes.Add(_read_node() as attribute);
				}
			}
		}


		public void visit(attribute_list _attribute_list)
		{
			read_attribute_list(_attribute_list);
		}

		public void read_attribute_list(attribute_list _attribute_list)
		{
			read_syntax_tree_node(_attribute_list);
			if (br.ReadByte() == 0)
			{
				_attribute_list.attributes = null;
			}
			else
			{
				_attribute_list.attributes = new List<simple_attribute_list>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_attribute_list.attributes.Add(_read_node() as simple_attribute_list);
				}
			}
		}


		public void visit(function_lambda_definition _function_lambda_definition)
		{
			read_function_lambda_definition(_function_lambda_definition);
		}

		public void read_function_lambda_definition(function_lambda_definition _function_lambda_definition)
		{
			read_expression(_function_lambda_definition);
			_function_lambda_definition.ident_list = _read_node() as ident_list;
			_function_lambda_definition.return_type = _read_node() as type_definition;
			_function_lambda_definition.formal_parameters = _read_node() as formal_parameters;
			_function_lambda_definition.proc_body = _read_node() as statement;
			_function_lambda_definition.proc_definition = (object)br.ReadByte();
			_function_lambda_definition.parameters = _read_node() as expression_list;
			if (br.ReadByte() == 0)
			{
				_function_lambda_definition.lambda_name = null;
			}
			else
			{
				_function_lambda_definition.lambda_name = br.ReadString();
			}
			if (br.ReadByte() == 0)
			{
				_function_lambda_definition.defs = null;
			}
			else
			{
				_function_lambda_definition.defs = new List<declaration>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_function_lambda_definition.defs.Add(_read_node() as declaration);
				}
			}
			_function_lambda_definition.lambda_visit_mode = (LambdaVisitMode)br.ReadByte();
			_function_lambda_definition.substituting_node = _read_node() as syntax_tree_node;
			_function_lambda_definition.usedkeyword = (int)br.ReadByte();
		}


		public void visit(function_lambda_call _function_lambda_call)
		{
			read_function_lambda_call(_function_lambda_call);
		}

		public void read_function_lambda_call(function_lambda_call _function_lambda_call)
		{
			read_expression(_function_lambda_call);
			_function_lambda_call.f_lambda_def = _read_node() as function_lambda_definition;
			_function_lambda_call.parameters = _read_node() as expression_list;
		}


		public void visit(semantic_check _semantic_check)
		{
			read_semantic_check(_semantic_check);
		}

		public void read_semantic_check(semantic_check _semantic_check)
		{
			read_statement(_semantic_check);
			if (br.ReadByte() == 0)
			{
				_semantic_check.CheckName = null;
			}
			else
			{
				_semantic_check.CheckName = br.ReadString();
			}
			if (br.ReadByte() == 0)
			{
				_semantic_check.param = null;
			}
			else
			{
				_semantic_check.param = new List<syntax_tree_node>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_semantic_check.param.Add(_read_node() as syntax_tree_node);
				}
			}
			_semantic_check.fictive = (int)br.ReadByte();
		}


		public void visit(lambda_inferred_type _lambda_inferred_type)
		{
			read_lambda_inferred_type(_lambda_inferred_type);
		}

		public void read_lambda_inferred_type(lambda_inferred_type _lambda_inferred_type)
		{
			read_type_definition(_lambda_inferred_type);
			_lambda_inferred_type.real_type = (object)br.ReadByte();
		}


		public void visit(same_type_node _same_type_node)
		{
			read_same_type_node(_same_type_node);
		}

		public void read_same_type_node(same_type_node _same_type_node)
		{
			read_type_definition(_same_type_node);
			_same_type_node.ex = _read_node() as expression;
		}


		public void visit(name_assign_expr _name_assign_expr)
		{
			read_name_assign_expr(_name_assign_expr);
		}

		public void read_name_assign_expr(name_assign_expr _name_assign_expr)
		{
			read_expression(_name_assign_expr);
			_name_assign_expr.name = _read_node() as ident;
			_name_assign_expr.expr = _read_node() as expression;
		}


		public void visit(name_assign_expr_list _name_assign_expr_list)
		{
			read_name_assign_expr_list(_name_assign_expr_list);
		}

		public void read_name_assign_expr_list(name_assign_expr_list _name_assign_expr_list)
		{
			read_syntax_tree_node(_name_assign_expr_list);
			if (br.ReadByte() == 0)
			{
				_name_assign_expr_list.name_expr = null;
			}
			else
			{
				_name_assign_expr_list.name_expr = new List<name_assign_expr>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_name_assign_expr_list.name_expr.Add(_read_node() as name_assign_expr);
				}
			}
		}


		public void visit(unnamed_type_object _unnamed_type_object)
		{
			read_unnamed_type_object(_unnamed_type_object);
		}

		public void read_unnamed_type_object(unnamed_type_object _unnamed_type_object)
		{
			read_addressed_value(_unnamed_type_object);
			_unnamed_type_object.ne_list = _read_node() as name_assign_expr_list;
			_unnamed_type_object.is_class = br.ReadBoolean();
			_unnamed_type_object.new_ex = _read_node() as new_expr;
		}


		public void visit(semantic_type_node _semantic_type_node)
		{
			read_semantic_type_node(_semantic_type_node);
		}

		public void read_semantic_type_node(semantic_type_node _semantic_type_node)
		{
			read_type_definition(_semantic_type_node);
			_semantic_type_node.type = (Object)br.ReadByte();
		}


		public void visit(short_func_definition _short_func_definition)
		{
			read_short_func_definition(_short_func_definition);
		}

		public void read_short_func_definition(short_func_definition _short_func_definition)
		{
			read_procedure_definition(_short_func_definition);
			_short_func_definition.procdef = _read_node() as procedure_definition;
		}


		public void visit(no_type_foreach _no_type_foreach)
		{
			read_no_type_foreach(_no_type_foreach);
		}

		public void read_no_type_foreach(no_type_foreach _no_type_foreach)
		{
			read_type_definition(_no_type_foreach);
		}


		public void visit(matching_expression _matching_expression)
		{
			read_matching_expression(_matching_expression);
		}

		public void read_matching_expression(matching_expression _matching_expression)
		{
			read_addressed_value(_matching_expression);
			_matching_expression.left = _read_node() as expression;
			_matching_expression.right = _read_node() as expression;
		}


		public void visit(closure_substituting_node _closure_substituting_node)
		{
			read_closure_substituting_node(_closure_substituting_node);
		}

		public void read_closure_substituting_node(closure_substituting_node _closure_substituting_node)
		{
			read_ident(_closure_substituting_node);
			_closure_substituting_node.substitution = _read_node() as dot_node;
		}


		public void visit(sequence_type _sequence_type)
		{
			read_sequence_type(_sequence_type);
		}

		public void read_sequence_type(sequence_type _sequence_type)
		{
			read_type_definition(_sequence_type);
			_sequence_type.elements_type = _read_node() as type_definition;
		}


		public void visit(modern_proc_type _modern_proc_type)
		{
			read_modern_proc_type(_modern_proc_type);
		}

		public void read_modern_proc_type(modern_proc_type _modern_proc_type)
		{
			read_type_definition(_modern_proc_type);
			_modern_proc_type.aloneparam = _read_node() as type_definition;
			_modern_proc_type.el = _read_node() as enumerator_list;
			_modern_proc_type.res = _read_node() as type_definition;
		}


		public void visit(yield_node _yield_node)
		{
			read_yield_node(_yield_node);
		}

		public void read_yield_node(yield_node _yield_node)
		{
			read_statement(_yield_node);
			_yield_node.ex = _read_node() as expression;
		}


		public void visit(template_operator_name _template_operator_name)
		{
			read_template_operator_name(_template_operator_name);
		}

		public void read_template_operator_name(template_operator_name _template_operator_name)
		{
			read_template_type_name(_template_operator_name);
			_template_operator_name.opname = _read_node() as operator_name_ident;
		}


		public void visit(semantic_addr_value _semantic_addr_value)
		{
			read_semantic_addr_value(_semantic_addr_value);
		}

		public void read_semantic_addr_value(semantic_addr_value _semantic_addr_value)
		{
			read_addressed_value(_semantic_addr_value);
			_semantic_addr_value.expr = (Object)br.ReadByte();
		}


		public void visit(pair_type_stlist _pair_type_stlist)
		{
			read_pair_type_stlist(_pair_type_stlist);
		}

		public void read_pair_type_stlist(pair_type_stlist _pair_type_stlist)
		{
			read_syntax_tree_node(_pair_type_stlist);
			_pair_type_stlist.tn = _read_node() as type_definition;
			_pair_type_stlist.exprs = _read_node() as statement_list;
		}


		public void visit(assign_tuple _assign_tuple)
		{
			read_assign_tuple(_assign_tuple);
		}

		public void read_assign_tuple(assign_tuple _assign_tuple)
		{
			read_statement(_assign_tuple);
			_assign_tuple.vars = _read_node() as addressed_value_list;
			_assign_tuple.expr = _read_node() as expression;
		}


		public void visit(addressed_value_list _addressed_value_list)
		{
			read_addressed_value_list(_addressed_value_list);
		}

		public void read_addressed_value_list(addressed_value_list _addressed_value_list)
		{
			read_syntax_tree_node(_addressed_value_list);
			if (br.ReadByte() == 0)
			{
				_addressed_value_list.variables = null;
			}
			else
			{
				_addressed_value_list.variables = new List<addressed_value>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_addressed_value_list.variables.Add(_read_node() as addressed_value);
				}
			}
		}


		public void visit(tuple_node _tuple_node)
		{
			read_tuple_node(_tuple_node);
		}

		public void read_tuple_node(tuple_node _tuple_node)
		{
			read_addressed_value(_tuple_node);
			_tuple_node.el = _read_node() as expression_list;
		}


		public void visit(uses_closure _uses_closure)
		{
			read_uses_closure(_uses_closure);
		}

		public void read_uses_closure(uses_closure _uses_closure)
		{
			read_uses_list(_uses_closure);
			if (br.ReadByte() == 0)
			{
				_uses_closure.listunitsections = null;
			}
			else
			{
				_uses_closure.listunitsections = new List<uses_list>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_uses_closure.listunitsections.Add(_read_node() as uses_list);
				}
			}
		}


		public void visit(dot_question_node _dot_question_node)
		{
			read_dot_question_node(_dot_question_node);
		}

		public void read_dot_question_node(dot_question_node _dot_question_node)
		{
			read_addressed_value_funcname(_dot_question_node);
			_dot_question_node.left = _read_node() as addressed_value;
			_dot_question_node.right = _read_node() as addressed_value;
		}


		public void visit(slice_expr _slice_expr)
		{
			read_slice_expr(_slice_expr);
		}

		public void read_slice_expr(slice_expr _slice_expr)
		{
			read_dereference(_slice_expr);
			_slice_expr.v = _read_node() as addressed_value;
			_slice_expr.from = _read_node() as expression;
			_slice_expr.to = _read_node() as expression;
			_slice_expr.step = _read_node() as expression;
		}


		public void visit(no_type _no_type)
		{
			read_no_type(_no_type);
		}

		public void read_no_type(no_type _no_type)
		{
			read_type_definition(_no_type);
		}


		public void visit(yield_unknown_ident _yield_unknown_ident)
		{
			read_yield_unknown_ident(_yield_unknown_ident);
		}

		public void read_yield_unknown_ident(yield_unknown_ident _yield_unknown_ident)
		{
			read_ident(_yield_unknown_ident);
			_yield_unknown_ident.UnknownID = _read_node() as ident;
		}


		public void visit(yield_unknown_expression_type _yield_unknown_expression_type)
		{
			read_yield_unknown_expression_type(_yield_unknown_expression_type);
		}

		public void read_yield_unknown_expression_type(yield_unknown_expression_type _yield_unknown_expression_type)
		{
			read_type_definition(_yield_unknown_expression_type);
		}


		public void visit(yield_unknown_foreach_type _yield_unknown_foreach_type)
		{
			read_yield_unknown_foreach_type(_yield_unknown_foreach_type);
		}

		public void read_yield_unknown_foreach_type(yield_unknown_foreach_type _yield_unknown_foreach_type)
		{
			read_type_definition(_yield_unknown_foreach_type);
		}


		public void visit(yield_sequence_node _yield_sequence_node)
		{
			read_yield_sequence_node(_yield_sequence_node);
		}

		public void read_yield_sequence_node(yield_sequence_node _yield_sequence_node)
		{
			read_statement(_yield_sequence_node);
			_yield_sequence_node.ex = _read_node() as expression;
		}


		public void visit(assign_var_tuple _assign_var_tuple)
		{
			read_assign_var_tuple(_assign_var_tuple);
		}

		public void read_assign_var_tuple(assign_var_tuple _assign_var_tuple)
		{
			read_statement(_assign_var_tuple);
			_assign_var_tuple.idents = _read_node() as ident_list;
			_assign_var_tuple.expr = _read_node() as expression;
		}


		public void visit(slice_expr_question _slice_expr_question)
		{
			read_slice_expr_question(_slice_expr_question);
		}

		public void read_slice_expr_question(slice_expr_question _slice_expr_question)
		{
			read_slice_expr(_slice_expr_question);
		}


		public void visit(semantic_check_sugared_statement_node _semantic_check_sugared_statement_node)
		{
			read_semantic_check_sugared_statement_node(_semantic_check_sugared_statement_node);
		}

		public void read_semantic_check_sugared_statement_node(semantic_check_sugared_statement_node _semantic_check_sugared_statement_node)
		{
			read_statement(_semantic_check_sugared_statement_node);
			_semantic_check_sugared_statement_node.typ = (object)br.ReadByte();
			if (br.ReadByte() == 0)
			{
				_semantic_check_sugared_statement_node.lst = null;
			}
			else
			{
				_semantic_check_sugared_statement_node.lst = new List<syntax_tree_node>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_semantic_check_sugared_statement_node.lst.Add(_read_node() as syntax_tree_node);
				}
			}
		}


		public void visit(sugared_expression _sugared_expression)
		{
			read_sugared_expression(_sugared_expression);
		}

		public void read_sugared_expression(sugared_expression _sugared_expression)
		{
			read_expression(_sugared_expression);
			_sugared_expression.sugared_expr = (object)br.ReadByte();
			_sugared_expression.new_expr = _read_node() as expression;
		}


		public void visit(sugared_addressed_value _sugared_addressed_value)
		{
			read_sugared_addressed_value(_sugared_addressed_value);
		}

		public void read_sugared_addressed_value(sugared_addressed_value _sugared_addressed_value)
		{
			read_addressed_value(_sugared_addressed_value);
			_sugared_addressed_value.sugared_expr = (object)br.ReadByte();
			_sugared_addressed_value.new_addr_value = _read_node() as addressed_value;
		}


		public void visit(double_question_node _double_question_node)
		{
			read_double_question_node(_double_question_node);
		}

		public void read_double_question_node(double_question_node _double_question_node)
		{
			read_addressed_value_funcname(_double_question_node);
			_double_question_node.left = _read_node() as expression;
			_double_question_node.right = _read_node() as expression;
		}


		public void visit(pattern_node _pattern_node)
		{
			read_pattern_node(_pattern_node);
		}

		public void read_pattern_node(pattern_node _pattern_node)
		{
			read_syntax_tree_node(_pattern_node);
			if (br.ReadByte() == 0)
			{
				_pattern_node.parameters = null;
			}
			else
			{
				_pattern_node.parameters = new List<pattern_parameter>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_pattern_node.parameters.Add(_read_node() as pattern_parameter);
				}
			}
		}


		public void visit(type_pattern _type_pattern)
		{
			read_type_pattern(_type_pattern);
		}

		public void read_type_pattern(type_pattern _type_pattern)
		{
			read_pattern_node(_type_pattern);
			_type_pattern.identifier = _read_node() as ident;
			_type_pattern.type = _read_node() as type_definition;
		}


		public void visit(is_pattern_expr _is_pattern_expr)
		{
			read_is_pattern_expr(_is_pattern_expr);
		}

		public void read_is_pattern_expr(is_pattern_expr _is_pattern_expr)
		{
			read_addressed_value(_is_pattern_expr);
			_is_pattern_expr.left = _read_node() as expression;
			_is_pattern_expr.right = _read_node() as pattern_node;
		}


		public void visit(match_with _match_with)
		{
			read_match_with(_match_with);
		}

		public void read_match_with(match_with _match_with)
		{
			read_statement(_match_with);
			_match_with.expr = _read_node() as expression;
			_match_with.case_list = _read_node() as pattern_cases;
			_match_with.defaultAction = _read_node() as statement;
		}


		public void visit(pattern_case _pattern_case)
		{
			read_pattern_case(_pattern_case);
		}

		public void read_pattern_case(pattern_case _pattern_case)
		{
			read_statement(_pattern_case);
			_pattern_case.pattern = _read_node() as pattern_node;
			_pattern_case.case_action = _read_node() as statement;
			_pattern_case.condition = _read_node() as expression;
		}


		public void visit(pattern_cases _pattern_cases)
		{
			read_pattern_cases(_pattern_cases);
		}

		public void read_pattern_cases(pattern_cases _pattern_cases)
		{
			read_statement(_pattern_cases);
			if (br.ReadByte() == 0)
			{
				_pattern_cases.elements = null;
			}
			else
			{
				_pattern_cases.elements = new List<pattern_case>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_pattern_cases.elements.Add(_read_node() as pattern_case);
				}
			}
		}


		public void visit(deconstructor_pattern _deconstructor_pattern)
		{
			read_deconstructor_pattern(_deconstructor_pattern);
		}

		public void read_deconstructor_pattern(deconstructor_pattern _deconstructor_pattern)
		{
			read_pattern_node(_deconstructor_pattern);
			_deconstructor_pattern.type = _read_node() as type_definition;
			_deconstructor_pattern.const_params_check = _read_node() as expression;
		}


		public void visit(pattern_parameter _pattern_parameter)
		{
			read_pattern_parameter(_pattern_parameter);
		}

		public void read_pattern_parameter(pattern_parameter _pattern_parameter)
		{
			read_syntax_tree_node(_pattern_parameter);
		}


		public void visit(desugared_deconstruction _desugared_deconstruction)
		{
			read_desugared_deconstruction(_desugared_deconstruction);
		}

		public void read_desugared_deconstruction(desugared_deconstruction _desugared_deconstruction)
		{
			read_statement(_desugared_deconstruction);
			_desugared_deconstruction.variables = _read_node() as deconstruction_variables_definition;
			_desugared_deconstruction.deconstruction_target = _read_node() as expression;
		}


		public void visit(var_deconstructor_parameter _var_deconstructor_parameter)
		{
			read_var_deconstructor_parameter(_var_deconstructor_parameter);
		}

		public void read_var_deconstructor_parameter(var_deconstructor_parameter _var_deconstructor_parameter)
		{
			read_pattern_parameter(_var_deconstructor_parameter);
			_var_deconstructor_parameter.identifier = _read_node() as ident;
			_var_deconstructor_parameter.type = _read_node() as type_definition;
			_var_deconstructor_parameter.var_keyword_used = br.ReadBoolean();
		}


		public void visit(recursive_deconstructor_parameter _recursive_deconstructor_parameter)
		{
			read_recursive_deconstructor_parameter(_recursive_deconstructor_parameter);
		}

		public void read_recursive_deconstructor_parameter(recursive_deconstructor_parameter _recursive_deconstructor_parameter)
		{
			read_recursive_pattern_parameter(_recursive_deconstructor_parameter);
		}


		public void visit(deconstruction_variables_definition _deconstruction_variables_definition)
		{
			read_deconstruction_variables_definition(_deconstruction_variables_definition);
		}

		public void read_deconstruction_variables_definition(deconstruction_variables_definition _deconstruction_variables_definition)
		{
			read_declaration(_deconstruction_variables_definition);
			if (br.ReadByte() == 0)
			{
				_deconstruction_variables_definition.definitions = null;
			}
			else
			{
				_deconstruction_variables_definition.definitions = new List<var_def_statement>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_deconstruction_variables_definition.definitions.Add(_read_node() as var_def_statement);
				}
			}
		}


		public void visit(var_tuple_def_statement _var_tuple_def_statement)
		{
			read_var_tuple_def_statement(_var_tuple_def_statement);
		}

		public void read_var_tuple_def_statement(var_tuple_def_statement _var_tuple_def_statement)
		{
			read_var_def_statement(_var_tuple_def_statement);
		}


		public void visit(semantic_check_sugared_var_def_statement_node _semantic_check_sugared_var_def_statement_node)
		{
			read_semantic_check_sugared_var_def_statement_node(_semantic_check_sugared_var_def_statement_node);
		}

		public void read_semantic_check_sugared_var_def_statement_node(semantic_check_sugared_var_def_statement_node _semantic_check_sugared_var_def_statement_node)
		{
			read_var_def_statement(_semantic_check_sugared_var_def_statement_node);
			_semantic_check_sugared_var_def_statement_node.typ = (object)br.ReadByte();
			if (br.ReadByte() == 0)
			{
				_semantic_check_sugared_var_def_statement_node.lst = null;
			}
			else
			{
				_semantic_check_sugared_var_def_statement_node.lst = new List<syntax_tree_node>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_semantic_check_sugared_var_def_statement_node.lst.Add(_read_node() as syntax_tree_node);
				}
			}
		}


		public void visit(const_pattern _const_pattern)
		{
			read_const_pattern(_const_pattern);
		}

		public void read_const_pattern(const_pattern _const_pattern)
		{
			read_pattern_node(_const_pattern);
			_const_pattern.pattern_expressions = _read_node() as expression_list;
		}


		public void visit(tuple_pattern_wild_card _tuple_pattern_wild_card)
		{
			read_tuple_pattern_wild_card(_tuple_pattern_wild_card);
		}

		public void read_tuple_pattern_wild_card(tuple_pattern_wild_card _tuple_pattern_wild_card)
		{
			read_pattern_parameter(_tuple_pattern_wild_card);
		}


		public void visit(const_pattern_parameter _const_pattern_parameter)
		{
			read_const_pattern_parameter(_const_pattern_parameter);
		}

		public void read_const_pattern_parameter(const_pattern_parameter _const_pattern_parameter)
		{
			read_pattern_parameter(_const_pattern_parameter);
			_const_pattern_parameter.const_param = _read_node() as expression;
		}


		public void visit(wild_card_deconstructor_parameter _wild_card_deconstructor_parameter)
		{
			read_wild_card_deconstructor_parameter(_wild_card_deconstructor_parameter);
		}

		public void read_wild_card_deconstructor_parameter(wild_card_deconstructor_parameter _wild_card_deconstructor_parameter)
		{
			read_pattern_parameter(_wild_card_deconstructor_parameter);
		}


		public void visit(collection_pattern _collection_pattern)
		{
			read_collection_pattern(_collection_pattern);
		}

		public void read_collection_pattern(collection_pattern _collection_pattern)
		{
			read_pattern_node(_collection_pattern);
		}


		public void visit(collection_pattern_gap_parameter _collection_pattern_gap_parameter)
		{
			read_collection_pattern_gap_parameter(_collection_pattern_gap_parameter);
		}

		public void read_collection_pattern_gap_parameter(collection_pattern_gap_parameter _collection_pattern_gap_parameter)
		{
			read_pattern_parameter(_collection_pattern_gap_parameter);
		}


		public void visit(collection_pattern_wild_card _collection_pattern_wild_card)
		{
			read_collection_pattern_wild_card(_collection_pattern_wild_card);
		}

		public void read_collection_pattern_wild_card(collection_pattern_wild_card _collection_pattern_wild_card)
		{
			read_pattern_parameter(_collection_pattern_wild_card);
		}


		public void visit(collection_pattern_var_parameter _collection_pattern_var_parameter)
		{
			read_collection_pattern_var_parameter(_collection_pattern_var_parameter);
		}

		public void read_collection_pattern_var_parameter(collection_pattern_var_parameter _collection_pattern_var_parameter)
		{
			read_pattern_parameter(_collection_pattern_var_parameter);
			_collection_pattern_var_parameter.identifier = _read_node() as ident;
			_collection_pattern_var_parameter.type = _read_node() as type_definition;
		}


		public void visit(recursive_collection_parameter _recursive_collection_parameter)
		{
			read_recursive_collection_parameter(_recursive_collection_parameter);
		}

		public void read_recursive_collection_parameter(recursive_collection_parameter _recursive_collection_parameter)
		{
			read_recursive_pattern_parameter(_recursive_collection_parameter);
		}


		public void visit(recursive_pattern_parameter _recursive_pattern_parameter)
		{
			read_recursive_pattern_parameter(_recursive_pattern_parameter);
		}

		public void read_recursive_pattern_parameter(recursive_pattern_parameter _recursive_pattern_parameter)
		{
			read_pattern_parameter(_recursive_pattern_parameter);
			_recursive_pattern_parameter.pattern = _read_node() as pattern_node;
		}


		public void visit(tuple_pattern _tuple_pattern)
		{
			read_tuple_pattern(_tuple_pattern);
		}

		public void read_tuple_pattern(tuple_pattern _tuple_pattern)
		{
			read_pattern_node(_tuple_pattern);
		}


		public void visit(tuple_pattern_var_parameter _tuple_pattern_var_parameter)
		{
			read_tuple_pattern_var_parameter(_tuple_pattern_var_parameter);
		}

		public void read_tuple_pattern_var_parameter(tuple_pattern_var_parameter _tuple_pattern_var_parameter)
		{
			read_pattern_parameter(_tuple_pattern_var_parameter);
			_tuple_pattern_var_parameter.identifier = _read_node() as ident;
			_tuple_pattern_var_parameter.type = _read_node() as type_definition;
		}


		public void visit(recursive_tuple_parameter _recursive_tuple_parameter)
		{
			read_recursive_tuple_parameter(_recursive_tuple_parameter);
		}

		public void read_recursive_tuple_parameter(recursive_tuple_parameter _recursive_tuple_parameter)
		{
			read_recursive_pattern_parameter(_recursive_tuple_parameter);
		}


		public void visit(diapason_expr_new _diapason_expr_new)
		{
			read_diapason_expr_new(_diapason_expr_new);
		}

		public void read_diapason_expr_new(diapason_expr_new _diapason_expr_new)
		{
			read_addressed_value(_diapason_expr_new);
			_diapason_expr_new.left = _read_node() as expression;
			_diapason_expr_new.right = _read_node() as expression;
		}


		public void visit(if_expr_new _if_expr_new)
		{
			read_if_expr_new(_if_expr_new);
		}

		public void read_if_expr_new(if_expr_new _if_expr_new)
		{
			read_expression(_if_expr_new);
			_if_expr_new.condition = _read_node() as expression;
			_if_expr_new.if_true = _read_node() as expression;
			_if_expr_new.if_false = _read_node() as expression;
		}


		public void visit(simple_expr_with_deref _simple_expr_with_deref)
		{
			read_simple_expr_with_deref(_simple_expr_with_deref);
		}

		public void read_simple_expr_with_deref(simple_expr_with_deref _simple_expr_with_deref)
		{
			read_expression(_simple_expr_with_deref);
			_simple_expr_with_deref.simple_expr = _read_node() as expression;
			_simple_expr_with_deref.has_deref = br.ReadBoolean();
		}


		public void visit(index _index)
		{
			read_index(_index);
		}

		public void read_index(index _index)
		{
			read_expression(_index);
			_index.index_expr = _read_node() as expression;
			_index.inverted = br.ReadBoolean();
		}


		public void visit(array_const_new _array_const_new)
		{
			read_array_const_new(_array_const_new);
		}

		public void read_array_const_new(array_const_new _array_const_new)
		{
			read_addressed_value(_array_const_new);
			_array_const_new.elements = _read_node() as expression_list;
		}


		public void visit(semantic_ith_element_of _semantic_ith_element_of)
		{
			read_semantic_ith_element_of(_semantic_ith_element_of);
		}

		public void read_semantic_ith_element_of(semantic_ith_element_of _semantic_ith_element_of)
		{
			read_expression(_semantic_ith_element_of);
			_semantic_ith_element_of.id = _read_node() as ident;
			_semantic_ith_element_of.index = _read_node() as expression;
		}


		public void visit(bigint_const _bigint_const)
		{
			read_bigint_const(_bigint_const);
		}

		public void read_bigint_const(bigint_const _bigint_const)
		{
			read_const_node(_bigint_const);
			_bigint_const.val = br.ReadUInt64();
		}


		public void visit(foreach_stmt_formatting _foreach_stmt_formatting)
		{
			read_foreach_stmt_formatting(_foreach_stmt_formatting);
		}

		public void read_foreach_stmt_formatting(foreach_stmt_formatting _foreach_stmt_formatting)
		{
			read_statement(_foreach_stmt_formatting);
			_foreach_stmt_formatting.il = _read_node() as ident_list;
			_foreach_stmt_formatting.in_what = _read_node() as expression;
			_foreach_stmt_formatting.stmt = _read_node() as statement;
			_foreach_stmt_formatting.index = _read_node() as ident;
		}


		public void visit(property_ident _property_ident)
		{
			read_property_ident(_property_ident);
		}

		public void read_property_ident(property_ident _property_ident)
		{
			read_ident(_property_ident);
			if (br.ReadByte() == 0)
			{
				_property_ident.ln = null;
			}
			else
			{
				_property_ident.ln = new List<ident>();
				Int32 ssyy_count = br.ReadInt32();
				for(Int32 ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
				{
					_property_ident.ln.Add(_read_node() as ident);
				}
			}
		}


		public void visit(expression_with_let _expression_with_let)
		{
			read_expression_with_let(_expression_with_let);
		}

		public void read_expression_with_let(expression_with_let _expression_with_let)
		{
			read_addressed_value(_expression_with_let);
			_expression_with_let.stat = _read_node() as statement_list;
			_expression_with_let.expr = _read_node() as expression;
		}


		public void visit(lambda_any_type_node_syntax _lambda_any_type_node_syntax)
		{
			read_lambda_any_type_node_syntax(_lambda_any_type_node_syntax);
		}

		public void read_lambda_any_type_node_syntax(lambda_any_type_node_syntax _lambda_any_type_node_syntax)
		{
			read_expression(_lambda_any_type_node_syntax);
		}


		public void visit(ref_var_def_statement _ref_var_def_statement)
		{
			read_ref_var_def_statement(_ref_var_def_statement);
		}

		public void read_ref_var_def_statement(ref_var_def_statement _ref_var_def_statement)
		{
			read_declaration(_ref_var_def_statement);
			_ref_var_def_statement.var = _read_node() as ident;
			_ref_var_def_statement.initial_value = _read_node() as addressed_value;
		}


		public void visit(let_var_expr _let_var_expr)
		{
			read_let_var_expr(_let_var_expr);
		}

		public void read_let_var_expr(let_var_expr _let_var_expr)
		{
			read_expression(_let_var_expr);
			_let_var_expr.id = _read_node() as ident;
			_let_var_expr.ex = _read_node() as expression;
		}


		public void visit(to_expr _to_expr)
		{
			read_to_expr(_to_expr);
		}

		public void read_to_expr(to_expr _to_expr)
		{
			read_expression(_to_expr);
			_to_expr.key = _read_node() as expression;
			_to_expr.value = _read_node() as expression;
		}


		public void visit(global_statement _global_statement)
		{
			read_global_statement(_global_statement);
		}

		public void read_global_statement(global_statement _global_statement)
		{
			read_statement(_global_statement);
			_global_statement.idents = _read_node() as ident_list;
		}


		public void visit(list_generator _list_generator)
		{
			read_list_generator(_list_generator);
		}

		public void read_list_generator(list_generator _list_generator)
		{
			read_expression(_list_generator);
			_list_generator._expr = _read_node() as expression;
			_list_generator._ident = _read_node() as ident;
			_list_generator._range = _read_node() as expression;
			_list_generator._condition = _read_node() as expression;
		}

	}


}

