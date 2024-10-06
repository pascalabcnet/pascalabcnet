/********************************************************************************************
Этот файл создан программой
PascalABC.NET: syntax tree generator  v1.5(с) Водолазов Н., Ткачук А.В., Иванов С.О., 2007

Вручную не редактировать!
*********************************************************************************************/
using System;
using System.IO;

namespace PascalABCCompiler.SyntaxTree
{

	public class SyntaxTreeStreamWriter:IVisitor
	{

		public BinaryWriter bw;


		public void visit(expression _expression)
		{
			bw.Write((Int16)0);
			write_expression(_expression);
		}

		public void write_expression(expression _expression)
		{
			write_declaration(_expression);
		}


		public void visit(syntax_tree_node _syntax_tree_node)
		{
			bw.Write((Int16)1);
			write_syntax_tree_node(_syntax_tree_node);
		}

		public void write_syntax_tree_node(syntax_tree_node _syntax_tree_node)
		{
			if (_syntax_tree_node.source_context == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				if (_syntax_tree_node.source_context.begin_position == null)
				{
					bw.Write((byte)0);
				}
				else
				{
					bw.Write((byte)1);
					bw.Write(_syntax_tree_node.source_context.begin_position.line_num);
					bw.Write(_syntax_tree_node.source_context.begin_position.column_num);
				}
				if (_syntax_tree_node.source_context.end_position == null)
				{
					bw.Write((byte)0);
				}
				else
				{
					bw.Write((byte)1);
					bw.Write(_syntax_tree_node.source_context.end_position.line_num);
					bw.Write(_syntax_tree_node.source_context.end_position.column_num);
				}
			}
		}


		public void visit(statement _statement)
		{
			bw.Write((Int16)2);
			write_statement(_statement);
		}

		public void write_statement(statement _statement)
		{
			write_declaration(_statement);
		}


		public void visit(statement_list _statement_list)
		{
			bw.Write((Int16)3);
			write_statement_list(_statement_list);
		}

		public void write_statement_list(statement_list _statement_list)
		{
			write_statement(_statement_list);
			if (_statement_list.subnodes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_statement_list.subnodes.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _statement_list.subnodes.Count; ssyy_i++)
				{
					if (_statement_list.subnodes[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_statement_list.subnodes[ssyy_i].visit(this);
					}
				}
			}
			if (_statement_list.left_logical_bracket == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_statement_list.left_logical_bracket.visit(this);
			}
			if (_statement_list.right_logical_bracket == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_statement_list.right_logical_bracket.visit(this);
			}
			bw.Write(_statement_list.expr_lambda_body);
		}


		public void visit(ident _ident)
		{
			bw.Write((Int16)4);
			write_ident(_ident);
		}

		public void write_ident(ident _ident)
		{
			write_addressed_value_funcname(_ident);
			if (_ident.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_ident.name);
			}
		}


		public void visit(assign _assign)
		{
			bw.Write((Int16)5);
			write_assign(_assign);
		}

		public void write_assign(assign _assign)
		{
			write_statement(_assign);
			if (_assign.to == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_assign.to.visit(this);
			}
			if (_assign.from == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_assign.from.visit(this);
			}
			bw.Write((byte)_assign.operator_type);
		}


		public void visit(bin_expr _bin_expr)
		{
			bw.Write((Int16)6);
			write_bin_expr(_bin_expr);
		}

		public void write_bin_expr(bin_expr _bin_expr)
		{
			write_addressed_value(_bin_expr);
			if (_bin_expr.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_bin_expr.left.visit(this);
			}
			if (_bin_expr.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_bin_expr.right.visit(this);
			}
			bw.Write((byte)_bin_expr.operation_type);
		}


		public void visit(un_expr _un_expr)
		{
			bw.Write((Int16)7);
			write_un_expr(_un_expr);
		}

		public void write_un_expr(un_expr _un_expr)
		{
			write_addressed_value(_un_expr);
			if (_un_expr.subnode == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_un_expr.subnode.visit(this);
			}
			bw.Write((byte)_un_expr.operation_type);
		}


		public void visit(const_node _const_node)
		{
			bw.Write((Int16)8);
			write_const_node(_const_node);
		}

		public void write_const_node(const_node _const_node)
		{
			write_addressed_value(_const_node);
		}


		public void visit(bool_const _bool_const)
		{
			bw.Write((Int16)9);
			write_bool_const(_bool_const);
		}

		public void write_bool_const(bool_const _bool_const)
		{
			write_const_node(_bool_const);
			bw.Write(_bool_const.val);
		}


		public void visit(int32_const _int32_const)
		{
			bw.Write((Int16)10);
			write_int32_const(_int32_const);
		}

		public void write_int32_const(int32_const _int32_const)
		{
			write_const_node(_int32_const);
			bw.Write(_int32_const.val);
		}


		public void visit(double_const _double_const)
		{
			bw.Write((Int16)11);
			write_double_const(_double_const);
		}

		public void write_double_const(double_const _double_const)
		{
			write_const_node(_double_const);
			bw.Write(_double_const.val);
		}


		public void visit(subprogram_body _subprogram_body)
		{
			bw.Write((Int16)12);
			write_subprogram_body(_subprogram_body);
		}

		public void write_subprogram_body(subprogram_body _subprogram_body)
		{
			write_syntax_tree_node(_subprogram_body);
			if (_subprogram_body.subprogram_code == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_subprogram_body.subprogram_code.visit(this);
			}
			if (_subprogram_body.subprogram_defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_subprogram_body.subprogram_defs.visit(this);
			}
		}


		public void visit(addressed_value _addressed_value)
		{
			bw.Write((Int16)13);
			write_addressed_value(_addressed_value);
		}

		public void write_addressed_value(addressed_value _addressed_value)
		{
			write_expression(_addressed_value);
		}


		public void visit(type_definition _type_definition)
		{
			bw.Write((Int16)14);
			write_type_definition(_type_definition);
		}

		public void write_type_definition(type_definition _type_definition)
		{
			write_declaration(_type_definition);
			if (_type_definition.attr_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_type_definition.attr_list.visit(this);
			}
		}


		public void visit(roof_dereference _roof_dereference)
		{
			bw.Write((Int16)15);
			write_roof_dereference(_roof_dereference);
		}

		public void write_roof_dereference(roof_dereference _roof_dereference)
		{
			write_dereference(_roof_dereference);
		}


		public void visit(named_type_reference _named_type_reference)
		{
			bw.Write((Int16)16);
			write_named_type_reference(_named_type_reference);
		}

		public void write_named_type_reference(named_type_reference _named_type_reference)
		{
			write_type_definition(_named_type_reference);
			if (_named_type_reference.names == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_named_type_reference.names.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _named_type_reference.names.Count; ssyy_i++)
				{
					if (_named_type_reference.names[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_named_type_reference.names[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(variable_definitions _variable_definitions)
		{
			bw.Write((Int16)17);
			write_variable_definitions(_variable_definitions);
		}

		public void write_variable_definitions(variable_definitions _variable_definitions)
		{
			write_declaration(_variable_definitions);
			if (_variable_definitions.var_definitions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_variable_definitions.var_definitions.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _variable_definitions.var_definitions.Count; ssyy_i++)
				{
					if (_variable_definitions.var_definitions[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_variable_definitions.var_definitions[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(ident_list _ident_list)
		{
			bw.Write((Int16)18);
			write_ident_list(_ident_list);
		}

		public void write_ident_list(ident_list _ident_list)
		{
			write_syntax_tree_node(_ident_list);
			if (_ident_list.idents == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_ident_list.idents.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _ident_list.idents.Count; ssyy_i++)
				{
					if (_ident_list.idents[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_ident_list.idents[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(var_def_statement _var_def_statement)
		{
			bw.Write((Int16)19);
			write_var_def_statement(_var_def_statement);
		}

		public void write_var_def_statement(var_def_statement _var_def_statement)
		{
			write_declaration(_var_def_statement);
			if (_var_def_statement.vars == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_var_def_statement.vars.visit(this);
			}
			if (_var_def_statement.vars_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_var_def_statement.vars_type.visit(this);
			}
			if (_var_def_statement.inital_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_var_def_statement.inital_value.visit(this);
			}
			bw.Write((byte)_var_def_statement.var_attr);
			bw.Write(_var_def_statement.is_event);
		}


		public void visit(declaration _declaration)
		{
			bw.Write((Int16)20);
			write_declaration(_declaration);
		}

		public void write_declaration(declaration _declaration)
		{
			write_syntax_tree_node(_declaration);
			if (_declaration.attributes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_declaration.attributes.visit(this);
			}
		}


		public void visit(declarations _declarations)
		{
			bw.Write((Int16)21);
			write_declarations(_declarations);
		}

		public void write_declarations(declarations _declarations)
		{
			write_syntax_tree_node(_declarations);
			if (_declarations.defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_declarations.defs.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _declarations.defs.Count; ssyy_i++)
				{
					if (_declarations.defs[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_declarations.defs[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(program_tree _program_tree)
		{
			bw.Write((Int16)22);
			write_program_tree(_program_tree);
		}

		public void write_program_tree(program_tree _program_tree)
		{
			write_syntax_tree_node(_program_tree);
			if (_program_tree.compilation_units == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_program_tree.compilation_units.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _program_tree.compilation_units.Count; ssyy_i++)
				{
					if (_program_tree.compilation_units[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_program_tree.compilation_units[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(program_name _program_name)
		{
			bw.Write((Int16)23);
			write_program_name(_program_name);
		}

		public void write_program_name(program_name _program_name)
		{
			write_syntax_tree_node(_program_name);
			if (_program_name.prog_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_name.prog_name.visit(this);
			}
		}


		public void visit(string_const _string_const)
		{
			bw.Write((Int16)24);
			write_string_const(_string_const);
		}

		public void write_string_const(string_const _string_const)
		{
			write_literal(_string_const);
			if (_string_const.Value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_string_const.Value);
			}
		}


		public void visit(expression_list _expression_list)
		{
			bw.Write((Int16)25);
			write_expression_list(_expression_list);
		}

		public void write_expression_list(expression_list _expression_list)
		{
			write_syntax_tree_node(_expression_list);
			if (_expression_list.expressions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_expression_list.expressions.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _expression_list.expressions.Count; ssyy_i++)
				{
					if (_expression_list.expressions[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_expression_list.expressions[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(dereference _dereference)
		{
			bw.Write((Int16)26);
			write_dereference(_dereference);
		}

		public void write_dereference(dereference _dereference)
		{
			write_addressed_value_funcname(_dereference);
			if (_dereference.dereferencing_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_dereference.dereferencing_value.visit(this);
			}
		}


		public void visit(indexer _indexer)
		{
			bw.Write((Int16)27);
			write_indexer(_indexer);
		}

		public void write_indexer(indexer _indexer)
		{
			write_dereference(_indexer);
			if (_indexer.indexes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_indexer.indexes.visit(this);
			}
		}


		public void visit(for_node _for_node)
		{
			bw.Write((Int16)28);
			write_for_node(_for_node);
		}

		public void write_for_node(for_node _for_node)
		{
			write_statement(_for_node);
			if (_for_node.loop_variable == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_for_node.loop_variable.visit(this);
			}
			if (_for_node.initial_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_for_node.initial_value.visit(this);
			}
			if (_for_node.finish_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_for_node.finish_value.visit(this);
			}
			if (_for_node.statements == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_for_node.statements.visit(this);
			}
			bw.Write((byte)_for_node.cycle_type);
			if (_for_node.increment_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_for_node.increment_value.visit(this);
			}
			if (_for_node.type_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_for_node.type_name.visit(this);
			}
			bw.Write(_for_node.create_loop_variable);
		}


		public void visit(repeat_node _repeat_node)
		{
			bw.Write((Int16)29);
			write_repeat_node(_repeat_node);
		}

		public void write_repeat_node(repeat_node _repeat_node)
		{
			write_statement(_repeat_node);
			if (_repeat_node.statements == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_repeat_node.statements.visit(this);
			}
			if (_repeat_node.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_repeat_node.expr.visit(this);
			}
		}


		public void visit(while_node _while_node)
		{
			bw.Write((Int16)30);
			write_while_node(_while_node);
		}

		public void write_while_node(while_node _while_node)
		{
			write_statement(_while_node);
			if (_while_node.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_while_node.expr.visit(this);
			}
			if (_while_node.statements == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_while_node.statements.visit(this);
			}
			bw.Write((byte)_while_node.CycleType);
		}


		public void visit(if_node _if_node)
		{
			bw.Write((Int16)31);
			write_if_node(_if_node);
		}

		public void write_if_node(if_node _if_node)
		{
			write_statement(_if_node);
			if (_if_node.condition == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_if_node.condition.visit(this);
			}
			if (_if_node.then_body == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_if_node.then_body.visit(this);
			}
			if (_if_node.else_body == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_if_node.else_body.visit(this);
			}
		}


		public void visit(ref_type _ref_type)
		{
			bw.Write((Int16)32);
			write_ref_type(_ref_type);
		}

		public void write_ref_type(ref_type _ref_type)
		{
			write_type_definition(_ref_type);
			if (_ref_type.pointed_to == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_ref_type.pointed_to.visit(this);
			}
		}


		public void visit(diapason _diapason)
		{
			bw.Write((Int16)33);
			write_diapason(_diapason);
		}

		public void write_diapason(diapason _diapason)
		{
			write_type_definition(_diapason);
			if (_diapason.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_diapason.left.visit(this);
			}
			if (_diapason.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_diapason.right.visit(this);
			}
		}


		public void visit(indexers_types _indexers_types)
		{
			bw.Write((Int16)34);
			write_indexers_types(_indexers_types);
		}

		public void write_indexers_types(indexers_types _indexers_types)
		{
			write_type_definition(_indexers_types);
			if (_indexers_types.indexers == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_indexers_types.indexers.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _indexers_types.indexers.Count; ssyy_i++)
				{
					if (_indexers_types.indexers[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_indexers_types.indexers[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(array_type _array_type)
		{
			bw.Write((Int16)35);
			write_array_type(_array_type);
		}

		public void write_array_type(array_type _array_type)
		{
			write_type_definition(_array_type);
			if (_array_type.indexers == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_array_type.indexers.visit(this);
			}
			if (_array_type.elements_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_array_type.elements_type.visit(this);
			}
		}


		public void visit(label_definitions _label_definitions)
		{
			bw.Write((Int16)36);
			write_label_definitions(_label_definitions);
		}

		public void write_label_definitions(label_definitions _label_definitions)
		{
			write_declaration(_label_definitions);
			if (_label_definitions.labels == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_label_definitions.labels.visit(this);
			}
		}


		public void visit(procedure_attribute _procedure_attribute)
		{
			bw.Write((Int16)37);
			write_procedure_attribute(_procedure_attribute);
		}

		public void write_procedure_attribute(procedure_attribute _procedure_attribute)
		{
			write_ident(_procedure_attribute);
			bw.Write((byte)_procedure_attribute.attribute_type);
		}


		public void visit(typed_parameters _typed_parameters)
		{
			bw.Write((Int16)38);
			write_typed_parameters(_typed_parameters);
		}

		public void write_typed_parameters(typed_parameters _typed_parameters)
		{
			write_declaration(_typed_parameters);
			if (_typed_parameters.idents == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_typed_parameters.idents.visit(this);
			}
			if (_typed_parameters.vars_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_typed_parameters.vars_type.visit(this);
			}
			bw.Write((byte)_typed_parameters.param_kind);
			if (_typed_parameters.inital_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_typed_parameters.inital_value.visit(this);
			}
		}


		public void visit(formal_parameters _formal_parameters)
		{
			bw.Write((Int16)39);
			write_formal_parameters(_formal_parameters);
		}

		public void write_formal_parameters(formal_parameters _formal_parameters)
		{
			write_syntax_tree_node(_formal_parameters);
			if (_formal_parameters.params_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_formal_parameters.params_list.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _formal_parameters.params_list.Count; ssyy_i++)
				{
					if (_formal_parameters.params_list[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_formal_parameters.params_list[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(procedure_attributes_list _procedure_attributes_list)
		{
			bw.Write((Int16)40);
			write_procedure_attributes_list(_procedure_attributes_list);
		}

		public void write_procedure_attributes_list(procedure_attributes_list _procedure_attributes_list)
		{
			write_syntax_tree_node(_procedure_attributes_list);
			if (_procedure_attributes_list.proc_attributes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_procedure_attributes_list.proc_attributes.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _procedure_attributes_list.proc_attributes.Count; ssyy_i++)
				{
					if (_procedure_attributes_list.proc_attributes[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_procedure_attributes_list.proc_attributes[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(procedure_header _procedure_header)
		{
			bw.Write((Int16)41);
			write_procedure_header(_procedure_header);
		}

		public void write_procedure_header(procedure_header _procedure_header)
		{
			write_type_definition(_procedure_header);
			if (_procedure_header.parameters == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_procedure_header.parameters.visit(this);
			}
			if (_procedure_header.proc_attributes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_procedure_header.proc_attributes.visit(this);
			}
			if (_procedure_header.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_procedure_header.name.visit(this);
			}
			bw.Write(_procedure_header.of_object);
			bw.Write(_procedure_header.class_keyword);
			if (_procedure_header.template_args == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_procedure_header.template_args.visit(this);
			}
			if (_procedure_header.where_defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_procedure_header.where_defs.visit(this);
			}
		}


		public void visit(function_header _function_header)
		{
			bw.Write((Int16)42);
			write_function_header(_function_header);
		}

		public void write_function_header(function_header _function_header)
		{
			write_procedure_header(_function_header);
			if (_function_header.return_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_header.return_type.visit(this);
			}
		}


		public void visit(procedure_definition _procedure_definition)
		{
			bw.Write((Int16)43);
			write_procedure_definition(_procedure_definition);
		}

		public void write_procedure_definition(procedure_definition _procedure_definition)
		{
			write_declaration(_procedure_definition);
			if (_procedure_definition.proc_header == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_procedure_definition.proc_header.visit(this);
			}
			if (_procedure_definition.proc_body == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_procedure_definition.proc_body.visit(this);
			}
			bw.Write(_procedure_definition.is_short_definition);
		}


		public void visit(type_declaration _type_declaration)
		{
			bw.Write((Int16)44);
			write_type_declaration(_type_declaration);
		}

		public void write_type_declaration(type_declaration _type_declaration)
		{
			write_declaration(_type_declaration);
			if (_type_declaration.type_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_type_declaration.type_name.visit(this);
			}
			if (_type_declaration.type_def == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_type_declaration.type_def.visit(this);
			}
		}


		public void visit(type_declarations _type_declarations)
		{
			bw.Write((Int16)45);
			write_type_declarations(_type_declarations);
		}

		public void write_type_declarations(type_declarations _type_declarations)
		{
			write_declaration(_type_declarations);
			if (_type_declarations.types_decl == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_type_declarations.types_decl.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _type_declarations.types_decl.Count; ssyy_i++)
				{
					if (_type_declarations.types_decl[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_type_declarations.types_decl[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(simple_const_definition _simple_const_definition)
		{
			bw.Write((Int16)46);
			write_simple_const_definition(_simple_const_definition);
		}

		public void write_simple_const_definition(simple_const_definition _simple_const_definition)
		{
			write_const_definition(_simple_const_definition);
		}


		public void visit(typed_const_definition _typed_const_definition)
		{
			bw.Write((Int16)47);
			write_typed_const_definition(_typed_const_definition);
		}

		public void write_typed_const_definition(typed_const_definition _typed_const_definition)
		{
			write_const_definition(_typed_const_definition);
			if (_typed_const_definition.const_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_typed_const_definition.const_type.visit(this);
			}
		}


		public void visit(const_definition _const_definition)
		{
			bw.Write((Int16)48);
			write_const_definition(_const_definition);
		}

		public void write_const_definition(const_definition _const_definition)
		{
			write_declaration(_const_definition);
			if (_const_definition.const_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_const_definition.const_name.visit(this);
			}
			if (_const_definition.const_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_const_definition.const_value.visit(this);
			}
		}


		public void visit(consts_definitions_list _consts_definitions_list)
		{
			bw.Write((Int16)49);
			write_consts_definitions_list(_consts_definitions_list);
		}

		public void write_consts_definitions_list(consts_definitions_list _consts_definitions_list)
		{
			write_declaration(_consts_definitions_list);
			if (_consts_definitions_list.const_defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_consts_definitions_list.const_defs.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _consts_definitions_list.const_defs.Count; ssyy_i++)
				{
					if (_consts_definitions_list.const_defs[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_consts_definitions_list.const_defs[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(unit_name _unit_name)
		{
			bw.Write((Int16)50);
			write_unit_name(_unit_name);
		}

		public void write_unit_name(unit_name _unit_name)
		{
			write_syntax_tree_node(_unit_name);
			if (_unit_name.idunit_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unit_name.idunit_name.visit(this);
			}
			bw.Write((byte)_unit_name.HeaderKeyword);
		}


		public void visit(unit_or_namespace _unit_or_namespace)
		{
			bw.Write((Int16)51);
			write_unit_or_namespace(_unit_or_namespace);
		}

		public void write_unit_or_namespace(unit_or_namespace _unit_or_namespace)
		{
			write_syntax_tree_node(_unit_or_namespace);
			if (_unit_or_namespace.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unit_or_namespace.name.visit(this);
			}
		}


		public void visit(uses_unit_in _uses_unit_in)
		{
			bw.Write((Int16)52);
			write_uses_unit_in(_uses_unit_in);
		}

		public void write_uses_unit_in(uses_unit_in _uses_unit_in)
		{
			write_unit_or_namespace(_uses_unit_in);
			if (_uses_unit_in.in_file == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_uses_unit_in.in_file.visit(this);
			}
		}


		public void visit(uses_list _uses_list)
		{
			bw.Write((Int16)53);
			write_uses_list(_uses_list);
		}

		public void write_uses_list(uses_list _uses_list)
		{
			write_syntax_tree_node(_uses_list);
			if (_uses_list.units == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_uses_list.units.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _uses_list.units.Count; ssyy_i++)
				{
					if (_uses_list.units[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_uses_list.units[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(program_body _program_body)
		{
			bw.Write((Int16)54);
			write_program_body(_program_body);
		}

		public void write_program_body(program_body _program_body)
		{
			write_syntax_tree_node(_program_body);
			if (_program_body.used_units == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_body.used_units.visit(this);
			}
			if (_program_body.program_definitions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_body.program_definitions.visit(this);
			}
			if (_program_body.program_code == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_body.program_code.visit(this);
			}
			if (_program_body.using_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_body.using_list.visit(this);
			}
		}


		public void visit(compilation_unit _compilation_unit)
		{
			bw.Write((Int16)55);
			write_compilation_unit(_compilation_unit);
		}

		public void write_compilation_unit(compilation_unit _compilation_unit)
		{
			write_syntax_tree_node(_compilation_unit);
			if (_compilation_unit.file_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_compilation_unit.file_name);
			}
			if (_compilation_unit.compiler_directives == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_compilation_unit.compiler_directives.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _compilation_unit.compiler_directives.Count; ssyy_i++)
				{
					if (_compilation_unit.compiler_directives[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_compilation_unit.compiler_directives[ssyy_i].visit(this);
					}
				}
			}
			if (_compilation_unit.Language == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_compilation_unit.Language);
			}
		}


		public void visit(unit_module _unit_module)
		{
			bw.Write((Int16)56);
			write_unit_module(_unit_module);
		}

		public void write_unit_module(unit_module _unit_module)
		{
			write_compilation_unit(_unit_module);
			if (_unit_module.unit_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unit_module.unit_name.visit(this);
			}
			if (_unit_module.interface_part == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unit_module.interface_part.visit(this);
			}
			if (_unit_module.implementation_part == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unit_module.implementation_part.visit(this);
			}
			if (_unit_module.initialization_part == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unit_module.initialization_part.visit(this);
			}
			if (_unit_module.finalization_part == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unit_module.finalization_part.visit(this);
			}
			if (_unit_module.attributes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unit_module.attributes.visit(this);
			}
		}


		public void visit(program_module _program_module)
		{
			bw.Write((Int16)57);
			write_program_module(_program_module);
		}

		public void write_program_module(program_module _program_module)
		{
			write_compilation_unit(_program_module);
			if (_program_module.program_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_module.program_name.visit(this);
			}
			if (_program_module.used_units == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_module.used_units.visit(this);
			}
			if (_program_module.program_block == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_module.program_block.visit(this);
			}
			if (_program_module.using_namespaces == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_program_module.using_namespaces.visit(this);
			}
		}


		public void visit(hex_constant _hex_constant)
		{
			bw.Write((Int16)58);
			write_hex_constant(_hex_constant);
		}

		public void write_hex_constant(hex_constant _hex_constant)
		{
			write_int64_const(_hex_constant);
		}


		public void visit(get_address _get_address)
		{
			bw.Write((Int16)59);
			write_get_address(_get_address);
		}

		public void write_get_address(get_address _get_address)
		{
			write_addressed_value_funcname(_get_address);
			if (_get_address.address_of == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_get_address.address_of.visit(this);
			}
		}


		public void visit(case_variant _case_variant)
		{
			bw.Write((Int16)60);
			write_case_variant(_case_variant);
		}

		public void write_case_variant(case_variant _case_variant)
		{
			write_statement(_case_variant);
			if (_case_variant.conditions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_case_variant.conditions.visit(this);
			}
			if (_case_variant.exec_if_true == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_case_variant.exec_if_true.visit(this);
			}
		}


		public void visit(case_node _case_node)
		{
			bw.Write((Int16)61);
			write_case_node(_case_node);
		}

		public void write_case_node(case_node _case_node)
		{
			write_statement(_case_node);
			if (_case_node.param == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_case_node.param.visit(this);
			}
			if (_case_node.conditions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_case_node.conditions.visit(this);
			}
			if (_case_node.else_statement == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_case_node.else_statement.visit(this);
			}
		}


		public void visit(method_name _method_name)
		{
			bw.Write((Int16)62);
			write_method_name(_method_name);
		}

		public void write_method_name(method_name _method_name)
		{
			write_syntax_tree_node(_method_name);
			if (_method_name.ln == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_method_name.ln.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _method_name.ln.Count; ssyy_i++)
				{
					if (_method_name.ln[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_method_name.ln[ssyy_i].visit(this);
					}
				}
			}
			if (_method_name.class_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_method_name.class_name.visit(this);
			}
			if (_method_name.meth_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_method_name.meth_name.visit(this);
			}
			if (_method_name.explicit_interface_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_method_name.explicit_interface_name.visit(this);
			}
		}


		public void visit(dot_node _dot_node)
		{
			bw.Write((Int16)63);
			write_dot_node(_dot_node);
		}

		public void write_dot_node(dot_node _dot_node)
		{
			write_addressed_value_funcname(_dot_node);
			if (_dot_node.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_dot_node.left.visit(this);
			}
			if (_dot_node.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_dot_node.right.visit(this);
			}
		}


		public void visit(empty_statement _empty_statement)
		{
			bw.Write((Int16)64);
			write_empty_statement(_empty_statement);
		}

		public void write_empty_statement(empty_statement _empty_statement)
		{
			write_statement(_empty_statement);
		}


		public void visit(goto_statement _goto_statement)
		{
			bw.Write((Int16)65);
			write_goto_statement(_goto_statement);
		}

		public void write_goto_statement(goto_statement _goto_statement)
		{
			write_statement(_goto_statement);
			if (_goto_statement.label == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_goto_statement.label.visit(this);
			}
		}


		public void visit(labeled_statement _labeled_statement)
		{
			bw.Write((Int16)66);
			write_labeled_statement(_labeled_statement);
		}

		public void write_labeled_statement(labeled_statement _labeled_statement)
		{
			write_statement(_labeled_statement);
			if (_labeled_statement.label_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_labeled_statement.label_name.visit(this);
			}
			if (_labeled_statement.to_statement == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_labeled_statement.to_statement.visit(this);
			}
		}


		public void visit(with_statement _with_statement)
		{
			bw.Write((Int16)67);
			write_with_statement(_with_statement);
		}

		public void write_with_statement(with_statement _with_statement)
		{
			write_statement(_with_statement);
			if (_with_statement.what_do == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_with_statement.what_do.visit(this);
			}
			if (_with_statement.do_with == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_with_statement.do_with.visit(this);
			}
		}


		public void visit(method_call _method_call)
		{
			bw.Write((Int16)68);
			write_method_call(_method_call);
		}

		public void write_method_call(method_call _method_call)
		{
			write_dereference(_method_call);
			if (_method_call.parameters == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_method_call.parameters.visit(this);
			}
		}


		public void visit(pascal_set_constant _pascal_set_constant)
		{
			bw.Write((Int16)69);
			write_pascal_set_constant(_pascal_set_constant);
		}

		public void write_pascal_set_constant(pascal_set_constant _pascal_set_constant)
		{
			write_addressed_value(_pascal_set_constant);
			if (_pascal_set_constant.values == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_pascal_set_constant.values.visit(this);
			}
		}


		public void visit(array_const _array_const)
		{
			bw.Write((Int16)70);
			write_array_const(_array_const);
		}

		public void write_array_const(array_const _array_const)
		{
			write_expression(_array_const);
			if (_array_const.elements == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_array_const.elements.visit(this);
			}
		}


		public void visit(write_accessor_name _write_accessor_name)
		{
			bw.Write((Int16)71);
			write_write_accessor_name(_write_accessor_name);
		}

		public void write_write_accessor_name(write_accessor_name _write_accessor_name)
		{
			write_syntax_tree_node(_write_accessor_name);
			if (_write_accessor_name.accessor_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_write_accessor_name.accessor_name.visit(this);
			}
			if (_write_accessor_name.pr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_write_accessor_name.pr.visit(this);
			}
			if (_write_accessor_name.statment_for_formatting == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_write_accessor_name.statment_for_formatting.visit(this);
			}
		}


		public void visit(read_accessor_name _read_accessor_name)
		{
			bw.Write((Int16)72);
			write_read_accessor_name(_read_accessor_name);
		}

		public void write_read_accessor_name(read_accessor_name _read_accessor_name)
		{
			write_syntax_tree_node(_read_accessor_name);
			if (_read_accessor_name.accessor_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_read_accessor_name.accessor_name.visit(this);
			}
			if (_read_accessor_name.pr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_read_accessor_name.pr.visit(this);
			}
			if (_read_accessor_name.expression_for_formatting == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_read_accessor_name.expression_for_formatting.visit(this);
			}
		}


		public void visit(property_accessors _property_accessors)
		{
			bw.Write((Int16)73);
			write_property_accessors(_property_accessors);
		}

		public void write_property_accessors(property_accessors _property_accessors)
		{
			write_syntax_tree_node(_property_accessors);
			if (_property_accessors.read_accessor == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_property_accessors.read_accessor.visit(this);
			}
			if (_property_accessors.write_accessor == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_property_accessors.write_accessor.visit(this);
			}
		}


		public void visit(simple_property _simple_property)
		{
			bw.Write((Int16)74);
			write_simple_property(_simple_property);
		}

		public void write_simple_property(simple_property _simple_property)
		{
			write_declaration(_simple_property);
			if (_simple_property.property_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_simple_property.property_name.visit(this);
			}
			if (_simple_property.property_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_simple_property.property_type.visit(this);
			}
			if (_simple_property.index_expression == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_simple_property.index_expression.visit(this);
			}
			if (_simple_property.accessors == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_simple_property.accessors.visit(this);
			}
			if (_simple_property.array_default == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_simple_property.array_default.visit(this);
			}
			if (_simple_property.parameter_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_simple_property.parameter_list.visit(this);
			}
			bw.Write((byte)_simple_property.attr);
			bw.Write((byte)_simple_property.virt_over_none_attr);
			bw.Write(_simple_property.is_auto);
			if (_simple_property.initial_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_simple_property.initial_value.visit(this);
			}
		}


		public void visit(index_property _index_property)
		{
			bw.Write((Int16)75);
			write_index_property(_index_property);
		}

		public void write_index_property(index_property _index_property)
		{
			write_simple_property(_index_property);
			if (_index_property.property_parametres == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_index_property.property_parametres.visit(this);
			}
			if (_index_property.is_default == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_index_property.is_default.visit(this);
			}
		}


		public void visit(class_members _class_members)
		{
			bw.Write((Int16)76);
			write_class_members(_class_members);
		}

		public void write_class_members(class_members _class_members)
		{
			write_syntax_tree_node(_class_members);
			if (_class_members.members == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_class_members.members.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _class_members.members.Count; ssyy_i++)
				{
					if (_class_members.members[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_class_members.members[ssyy_i].visit(this);
					}
				}
			}
			if (_class_members.access_mod == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_class_members.access_mod.visit(this);
			}
		}


		public void visit(access_modifer_node _access_modifer_node)
		{
			bw.Write((Int16)77);
			write_access_modifer_node(_access_modifer_node);
		}

		public void write_access_modifer_node(access_modifer_node _access_modifer_node)
		{
			write_syntax_tree_node(_access_modifer_node);
			bw.Write((byte)_access_modifer_node.access_level);
		}


		public void visit(class_body_list _class_body_list)
		{
			bw.Write((Int16)78);
			write_class_body_list(_class_body_list);
		}

		public void write_class_body_list(class_body_list _class_body_list)
		{
			write_syntax_tree_node(_class_body_list);
			if (_class_body_list.class_def_blocks == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_class_body_list.class_def_blocks.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _class_body_list.class_def_blocks.Count; ssyy_i++)
				{
					if (_class_body_list.class_def_blocks[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_class_body_list.class_def_blocks[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(class_definition _class_definition)
		{
			bw.Write((Int16)79);
			write_class_definition(_class_definition);
		}

		public void write_class_definition(class_definition _class_definition)
		{
			write_type_definition(_class_definition);
			if (_class_definition.class_parents == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_class_definition.class_parents.visit(this);
			}
			if (_class_definition.body == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_class_definition.body.visit(this);
			}
			bw.Write((byte)_class_definition.keyword);
			if (_class_definition.template_args == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_class_definition.template_args.visit(this);
			}
			if (_class_definition.where_section == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_class_definition.where_section.visit(this);
			}
			bw.Write((byte)_class_definition.attribute);
			bw.Write(_class_definition.is_auto);
		}


		public void visit(default_indexer_property_node _default_indexer_property_node)
		{
			bw.Write((Int16)80);
			write_default_indexer_property_node(_default_indexer_property_node);
		}

		public void write_default_indexer_property_node(default_indexer_property_node _default_indexer_property_node)
		{
			write_syntax_tree_node(_default_indexer_property_node);
		}


		public void visit(known_type_definition _known_type_definition)
		{
			bw.Write((Int16)81);
			write_known_type_definition(_known_type_definition);
		}

		public void write_known_type_definition(known_type_definition _known_type_definition)
		{
			write_type_definition(_known_type_definition);
			bw.Write((byte)_known_type_definition.tp);
			if (_known_type_definition.unit_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_known_type_definition.unit_name.visit(this);
			}
		}


		public void visit(set_type_definition _set_type_definition)
		{
			bw.Write((Int16)82);
			write_set_type_definition(_set_type_definition);
		}

		public void write_set_type_definition(set_type_definition _set_type_definition)
		{
			write_type_definition(_set_type_definition);
			if (_set_type_definition.of_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_set_type_definition.of_type.visit(this);
			}
		}


		public void visit(record_const_definition _record_const_definition)
		{
			bw.Write((Int16)83);
			write_record_const_definition(_record_const_definition);
		}

		public void write_record_const_definition(record_const_definition _record_const_definition)
		{
			write_statement(_record_const_definition);
			if (_record_const_definition.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_record_const_definition.name.visit(this);
			}
			if (_record_const_definition.val == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_record_const_definition.val.visit(this);
			}
		}


		public void visit(record_const _record_const)
		{
			bw.Write((Int16)84);
			write_record_const(_record_const);
		}

		public void write_record_const(record_const _record_const)
		{
			write_expression(_record_const);
			if (_record_const.rec_consts == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_record_const.rec_consts.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _record_const.rec_consts.Count; ssyy_i++)
				{
					if (_record_const.rec_consts[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_record_const.rec_consts[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(record_type _record_type)
		{
			bw.Write((Int16)85);
			write_record_type(_record_type);
		}

		public void write_record_type(record_type _record_type)
		{
			write_type_definition(_record_type);
			if (_record_type.parts == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_record_type.parts.visit(this);
			}
			if (_record_type.base_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_record_type.base_type.visit(this);
			}
		}


		public void visit(enum_type_definition _enum_type_definition)
		{
			bw.Write((Int16)86);
			write_enum_type_definition(_enum_type_definition);
		}

		public void write_enum_type_definition(enum_type_definition _enum_type_definition)
		{
			write_type_definition(_enum_type_definition);
			if (_enum_type_definition.enumerators == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_enum_type_definition.enumerators.visit(this);
			}
		}


		public void visit(char_const _char_const)
		{
			bw.Write((Int16)87);
			write_char_const(_char_const);
		}

		public void write_char_const(char_const _char_const)
		{
			write_literal(_char_const);
			bw.Write(_char_const.cconst);
		}


		public void visit(raise_statement _raise_statement)
		{
			bw.Write((Int16)88);
			write_raise_statement(_raise_statement);
		}

		public void write_raise_statement(raise_statement _raise_statement)
		{
			write_statement(_raise_statement);
			if (_raise_statement.excep == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_raise_statement.excep.visit(this);
			}
		}


		public void visit(sharp_char_const _sharp_char_const)
		{
			bw.Write((Int16)89);
			write_sharp_char_const(_sharp_char_const);
		}

		public void write_sharp_char_const(sharp_char_const _sharp_char_const)
		{
			write_literal(_sharp_char_const);
			bw.Write((byte)_sharp_char_const.char_num);
		}


		public void visit(literal_const_line _literal_const_line)
		{
			bw.Write((Int16)90);
			write_literal_const_line(_literal_const_line);
		}

		public void write_literal_const_line(literal_const_line _literal_const_line)
		{
			write_literal(_literal_const_line);
			if (_literal_const_line.literals == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_literal_const_line.literals.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _literal_const_line.literals.Count; ssyy_i++)
				{
					if (_literal_const_line.literals[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_literal_const_line.literals[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(string_num_definition _string_num_definition)
		{
			bw.Write((Int16)91);
			write_string_num_definition(_string_num_definition);
		}

		public void write_string_num_definition(string_num_definition _string_num_definition)
		{
			write_type_definition(_string_num_definition);
			if (_string_num_definition.num_of_symbols == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_string_num_definition.num_of_symbols.visit(this);
			}
			if (_string_num_definition.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_string_num_definition.name.visit(this);
			}
		}


		public void visit(variant _variant)
		{
			bw.Write((Int16)92);
			write_variant(_variant);
		}

		public void write_variant(variant _variant)
		{
			write_syntax_tree_node(_variant);
			if (_variant.vars == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_variant.vars.visit(this);
			}
			if (_variant.vars_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_variant.vars_type.visit(this);
			}
		}


		public void visit(variant_list _variant_list)
		{
			bw.Write((Int16)93);
			write_variant_list(_variant_list);
		}

		public void write_variant_list(variant_list _variant_list)
		{
			write_syntax_tree_node(_variant_list);
			if (_variant_list.vars == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_variant_list.vars.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _variant_list.vars.Count; ssyy_i++)
				{
					if (_variant_list.vars[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_variant_list.vars[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(variant_type _variant_type)
		{
			bw.Write((Int16)94);
			write_variant_type(_variant_type);
		}

		public void write_variant_type(variant_type _variant_type)
		{
			write_syntax_tree_node(_variant_type);
			if (_variant_type.case_exprs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_variant_type.case_exprs.visit(this);
			}
			if (_variant_type.parts == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_variant_type.parts.visit(this);
			}
		}


		public void visit(variant_types _variant_types)
		{
			bw.Write((Int16)95);
			write_variant_types(_variant_types);
		}

		public void write_variant_types(variant_types _variant_types)
		{
			write_syntax_tree_node(_variant_types);
			if (_variant_types.vars == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_variant_types.vars.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _variant_types.vars.Count; ssyy_i++)
				{
					if (_variant_types.vars[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_variant_types.vars[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(variant_record_type _variant_record_type)
		{
			bw.Write((Int16)96);
			write_variant_record_type(_variant_record_type);
		}

		public void write_variant_record_type(variant_record_type _variant_record_type)
		{
			write_syntax_tree_node(_variant_record_type);
			if (_variant_record_type.var_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_variant_record_type.var_name.visit(this);
			}
			if (_variant_record_type.var_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_variant_record_type.var_type.visit(this);
			}
			if (_variant_record_type.vars == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_variant_record_type.vars.visit(this);
			}
		}


		public void visit(procedure_call _procedure_call)
		{
			bw.Write((Int16)97);
			write_procedure_call(_procedure_call);
		}

		public void write_procedure_call(procedure_call _procedure_call)
		{
			write_statement(_procedure_call);
			if (_procedure_call.func_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_procedure_call.func_name.visit(this);
			}
			bw.Write(_procedure_call.is_ident);
		}


		public void visit(class_predefinition _class_predefinition)
		{
			bw.Write((Int16)98);
			write_class_predefinition(_class_predefinition);
		}

		public void write_class_predefinition(class_predefinition _class_predefinition)
		{
			write_type_declaration(_class_predefinition);
			if (_class_predefinition.class_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_class_predefinition.class_name.visit(this);
			}
		}


		public void visit(nil_const _nil_const)
		{
			bw.Write((Int16)99);
			write_nil_const(_nil_const);
		}

		public void write_nil_const(nil_const _nil_const)
		{
			write_const_node(_nil_const);
		}


		public void visit(file_type_definition _file_type_definition)
		{
			bw.Write((Int16)100);
			write_file_type_definition(_file_type_definition);
		}

		public void write_file_type_definition(file_type_definition _file_type_definition)
		{
			write_type_definition(_file_type_definition);
			if (_file_type_definition.elem_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_file_type_definition.elem_type.visit(this);
			}
		}


		public void visit(constructor _constructor)
		{
			bw.Write((Int16)101);
			write_constructor(_constructor);
		}

		public void write_constructor(constructor _constructor)
		{
			write_procedure_header(_constructor);
		}


		public void visit(destructor _destructor)
		{
			bw.Write((Int16)102);
			write_destructor(_destructor);
		}

		public void write_destructor(destructor _destructor)
		{
			write_procedure_header(_destructor);
		}


		public void visit(inherited_method_call _inherited_method_call)
		{
			bw.Write((Int16)103);
			write_inherited_method_call(_inherited_method_call);
		}

		public void write_inherited_method_call(inherited_method_call _inherited_method_call)
		{
			write_statement(_inherited_method_call);
			if (_inherited_method_call.method_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_inherited_method_call.method_name.visit(this);
			}
			if (_inherited_method_call.exprs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_inherited_method_call.exprs.visit(this);
			}
		}


		public void visit(typecast_node _typecast_node)
		{
			bw.Write((Int16)104);
			write_typecast_node(_typecast_node);
		}

		public void write_typecast_node(typecast_node _typecast_node)
		{
			write_addressed_value(_typecast_node);
			if (_typecast_node.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_typecast_node.expr.visit(this);
			}
			if (_typecast_node.type_def == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_typecast_node.type_def.visit(this);
			}
			bw.Write((byte)_typecast_node.cast_op);
		}


		public void visit(interface_node _interface_node)
		{
			bw.Write((Int16)105);
			write_interface_node(_interface_node);
		}

		public void write_interface_node(interface_node _interface_node)
		{
			write_syntax_tree_node(_interface_node);
			if (_interface_node.interface_definitions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_interface_node.interface_definitions.visit(this);
			}
			if (_interface_node.uses_modules == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_interface_node.uses_modules.visit(this);
			}
			if (_interface_node.using_namespaces == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_interface_node.using_namespaces.visit(this);
			}
		}


		public void visit(implementation_node _implementation_node)
		{
			bw.Write((Int16)106);
			write_implementation_node(_implementation_node);
		}

		public void write_implementation_node(implementation_node _implementation_node)
		{
			write_syntax_tree_node(_implementation_node);
			if (_implementation_node.uses_modules == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_implementation_node.uses_modules.visit(this);
			}
			if (_implementation_node.implementation_definitions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_implementation_node.implementation_definitions.visit(this);
			}
			if (_implementation_node.using_namespaces == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_implementation_node.using_namespaces.visit(this);
			}
		}


		public void visit(diap_expr _diap_expr)
		{
			bw.Write((Int16)107);
			write_diap_expr(_diap_expr);
		}

		public void write_diap_expr(diap_expr _diap_expr)
		{
			write_expression(_diap_expr);
			if (_diap_expr.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_diap_expr.left.visit(this);
			}
			if (_diap_expr.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_diap_expr.right.visit(this);
			}
		}


		public void visit(block _block)
		{
			bw.Write((Int16)108);
			write_block(_block);
		}

		public void write_block(block _block)
		{
			write_proc_block(_block);
			if (_block.defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_block.defs.visit(this);
			}
			if (_block.program_code == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_block.program_code.visit(this);
			}
		}


		public void visit(proc_block _proc_block)
		{
			bw.Write((Int16)109);
			write_proc_block(_proc_block);
		}

		public void write_proc_block(proc_block _proc_block)
		{
			write_syntax_tree_node(_proc_block);
		}


		public void visit(array_of_named_type_definition _array_of_named_type_definition)
		{
			bw.Write((Int16)110);
			write_array_of_named_type_definition(_array_of_named_type_definition);
		}

		public void write_array_of_named_type_definition(array_of_named_type_definition _array_of_named_type_definition)
		{
			write_type_definition(_array_of_named_type_definition);
			if (_array_of_named_type_definition.type_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_array_of_named_type_definition.type_name.visit(this);
			}
		}


		public void visit(array_of_const_type_definition _array_of_const_type_definition)
		{
			bw.Write((Int16)111);
			write_array_of_const_type_definition(_array_of_const_type_definition);
		}

		public void write_array_of_const_type_definition(array_of_const_type_definition _array_of_const_type_definition)
		{
			write_type_definition(_array_of_const_type_definition);
		}


		public void visit(literal _literal)
		{
			bw.Write((Int16)112);
			write_literal(_literal);
		}

		public void write_literal(literal _literal)
		{
			write_const_node(_literal);
		}


		public void visit(case_variants _case_variants)
		{
			bw.Write((Int16)113);
			write_case_variants(_case_variants);
		}

		public void write_case_variants(case_variants _case_variants)
		{
			write_syntax_tree_node(_case_variants);
			if (_case_variants.variants == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_case_variants.variants.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _case_variants.variants.Count; ssyy_i++)
				{
					if (_case_variants.variants[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_case_variants.variants[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(diapason_expr _diapason_expr)
		{
			bw.Write((Int16)114);
			write_diapason_expr(_diapason_expr);
		}

		public void write_diapason_expr(diapason_expr _diapason_expr)
		{
			write_expression(_diapason_expr);
			if (_diapason_expr.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_diapason_expr.left.visit(this);
			}
			if (_diapason_expr.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_diapason_expr.right.visit(this);
			}
		}


		public void visit(var_def_list_for_record _var_def_list_for_record)
		{
			bw.Write((Int16)115);
			write_var_def_list_for_record(_var_def_list_for_record);
		}

		public void write_var_def_list_for_record(var_def_list_for_record _var_def_list_for_record)
		{
			write_syntax_tree_node(_var_def_list_for_record);
			if (_var_def_list_for_record.vars == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_var_def_list_for_record.vars.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _var_def_list_for_record.vars.Count; ssyy_i++)
				{
					if (_var_def_list_for_record.vars[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_var_def_list_for_record.vars[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(record_type_parts _record_type_parts)
		{
			bw.Write((Int16)116);
			write_record_type_parts(_record_type_parts);
		}

		public void write_record_type_parts(record_type_parts _record_type_parts)
		{
			write_syntax_tree_node(_record_type_parts);
			if (_record_type_parts.fixed_part == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_record_type_parts.fixed_part.visit(this);
			}
			if (_record_type_parts.variant_part == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_record_type_parts.variant_part.visit(this);
			}
		}


		public void visit(property_array_default _property_array_default)
		{
			bw.Write((Int16)117);
			write_property_array_default(_property_array_default);
		}

		public void write_property_array_default(property_array_default _property_array_default)
		{
			write_syntax_tree_node(_property_array_default);
		}


		public void visit(property_interface _property_interface)
		{
			bw.Write((Int16)118);
			write_property_interface(_property_interface);
		}

		public void write_property_interface(property_interface _property_interface)
		{
			write_syntax_tree_node(_property_interface);
			if (_property_interface.parameter_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_property_interface.parameter_list.visit(this);
			}
			if (_property_interface.property_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_property_interface.property_type.visit(this);
			}
			if (_property_interface.index_expression == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_property_interface.index_expression.visit(this);
			}
		}


		public void visit(property_parameter _property_parameter)
		{
			bw.Write((Int16)119);
			write_property_parameter(_property_parameter);
		}

		public void write_property_parameter(property_parameter _property_parameter)
		{
			write_syntax_tree_node(_property_parameter);
			if (_property_parameter.names == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_property_parameter.names.visit(this);
			}
			if (_property_parameter.type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_property_parameter.type.visit(this);
			}
		}


		public void visit(property_parameter_list _property_parameter_list)
		{
			bw.Write((Int16)120);
			write_property_parameter_list(_property_parameter_list);
		}

		public void write_property_parameter_list(property_parameter_list _property_parameter_list)
		{
			write_syntax_tree_node(_property_parameter_list);
			if (_property_parameter_list.parameters == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_property_parameter_list.parameters.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _property_parameter_list.parameters.Count; ssyy_i++)
				{
					if (_property_parameter_list.parameters[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_property_parameter_list.parameters[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(inherited_ident _inherited_ident)
		{
			bw.Write((Int16)121);
			write_inherited_ident(_inherited_ident);
		}

		public void write_inherited_ident(inherited_ident _inherited_ident)
		{
			write_ident(_inherited_ident);
		}


		public void visit(format_expr _format_expr)
		{
			bw.Write((Int16)122);
			write_format_expr(_format_expr);
		}

		public void write_format_expr(format_expr _format_expr)
		{
			write_addressed_value(_format_expr);
			if (_format_expr.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_format_expr.expr.visit(this);
			}
			if (_format_expr.format1 == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_format_expr.format1.visit(this);
			}
			if (_format_expr.format2 == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_format_expr.format2.visit(this);
			}
		}


		public void visit(initfinal_part _initfinal_part)
		{
			bw.Write((Int16)123);
			write_initfinal_part(_initfinal_part);
		}

		public void write_initfinal_part(initfinal_part _initfinal_part)
		{
			write_syntax_tree_node(_initfinal_part);
			if (_initfinal_part.initialization_sect == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_initfinal_part.initialization_sect.visit(this);
			}
			if (_initfinal_part.finalization_sect == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_initfinal_part.finalization_sect.visit(this);
			}
		}


		public void visit(token_info _token_info)
		{
			bw.Write((Int16)124);
			write_token_info(_token_info);
		}

		public void write_token_info(token_info _token_info)
		{
			write_syntax_tree_node(_token_info);
			if (_token_info.text == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_token_info.text);
			}
		}


		public void visit(raise_stmt _raise_stmt)
		{
			bw.Write((Int16)125);
			write_raise_stmt(_raise_stmt);
		}

		public void write_raise_stmt(raise_stmt _raise_stmt)
		{
			write_statement(_raise_stmt);
			if (_raise_stmt.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_raise_stmt.expr.visit(this);
			}
			if (_raise_stmt.address == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_raise_stmt.address.visit(this);
			}
		}


		public void visit(op_type_node _op_type_node)
		{
			bw.Write((Int16)126);
			write_op_type_node(_op_type_node);
		}

		public void write_op_type_node(op_type_node _op_type_node)
		{
			write_token_info(_op_type_node);
			bw.Write((byte)_op_type_node.type);
		}


		public void visit(file_type _file_type)
		{
			bw.Write((Int16)127);
			write_file_type(_file_type);
		}

		public void write_file_type(file_type _file_type)
		{
			write_type_definition(_file_type);
			if (_file_type.file_of_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_file_type.file_of_type.visit(this);
			}
		}


		public void visit(known_type_ident _known_type_ident)
		{
			bw.Write((Int16)128);
			write_known_type_ident(_known_type_ident);
		}

		public void write_known_type_ident(known_type_ident _known_type_ident)
		{
			write_ident(_known_type_ident);
			bw.Write((byte)_known_type_ident.type);
		}


		public void visit(exception_handler _exception_handler)
		{
			bw.Write((Int16)129);
			write_exception_handler(_exception_handler);
		}

		public void write_exception_handler(exception_handler _exception_handler)
		{
			write_syntax_tree_node(_exception_handler);
			if (_exception_handler.variable == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_exception_handler.variable.visit(this);
			}
			if (_exception_handler.type_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_exception_handler.type_name.visit(this);
			}
			if (_exception_handler.statements == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_exception_handler.statements.visit(this);
			}
		}


		public void visit(exception_ident _exception_ident)
		{
			bw.Write((Int16)130);
			write_exception_ident(_exception_ident);
		}

		public void write_exception_ident(exception_ident _exception_ident)
		{
			write_syntax_tree_node(_exception_ident);
			if (_exception_ident.variable == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_exception_ident.variable.visit(this);
			}
			if (_exception_ident.type_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_exception_ident.type_name.visit(this);
			}
		}


		public void visit(exception_handler_list _exception_handler_list)
		{
			bw.Write((Int16)131);
			write_exception_handler_list(_exception_handler_list);
		}

		public void write_exception_handler_list(exception_handler_list _exception_handler_list)
		{
			write_syntax_tree_node(_exception_handler_list);
			if (_exception_handler_list.handlers == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_exception_handler_list.handlers.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _exception_handler_list.handlers.Count; ssyy_i++)
				{
					if (_exception_handler_list.handlers[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_exception_handler_list.handlers[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(exception_block _exception_block)
		{
			bw.Write((Int16)132);
			write_exception_block(_exception_block);
		}

		public void write_exception_block(exception_block _exception_block)
		{
			write_syntax_tree_node(_exception_block);
			if (_exception_block.stmt_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_exception_block.stmt_list.visit(this);
			}
			if (_exception_block.handlers == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_exception_block.handlers.visit(this);
			}
			if (_exception_block.else_stmt_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_exception_block.else_stmt_list.visit(this);
			}
		}


		public void visit(try_handler _try_handler)
		{
			bw.Write((Int16)133);
			write_try_handler(_try_handler);
		}

		public void write_try_handler(try_handler _try_handler)
		{
			write_syntax_tree_node(_try_handler);
		}


		public void visit(try_handler_finally _try_handler_finally)
		{
			bw.Write((Int16)134);
			write_try_handler_finally(_try_handler_finally);
		}

		public void write_try_handler_finally(try_handler_finally _try_handler_finally)
		{
			write_try_handler(_try_handler_finally);
			if (_try_handler_finally.stmt_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_try_handler_finally.stmt_list.visit(this);
			}
		}


		public void visit(try_handler_except _try_handler_except)
		{
			bw.Write((Int16)135);
			write_try_handler_except(_try_handler_except);
		}

		public void write_try_handler_except(try_handler_except _try_handler_except)
		{
			write_try_handler(_try_handler_except);
			if (_try_handler_except.except_block == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_try_handler_except.except_block.visit(this);
			}
		}


		public void visit(try_stmt _try_stmt)
		{
			bw.Write((Int16)136);
			write_try_stmt(_try_stmt);
		}

		public void write_try_stmt(try_stmt _try_stmt)
		{
			write_statement(_try_stmt);
			if (_try_stmt.stmt_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_try_stmt.stmt_list.visit(this);
			}
			if (_try_stmt.handler == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_try_stmt.handler.visit(this);
			}
		}


		public void visit(inherited_message _inherited_message)
		{
			bw.Write((Int16)137);
			write_inherited_message(_inherited_message);
		}

		public void write_inherited_message(inherited_message _inherited_message)
		{
			write_statement(_inherited_message);
		}


		public void visit(external_directive _external_directive)
		{
			bw.Write((Int16)138);
			write_external_directive(_external_directive);
		}

		public void write_external_directive(external_directive _external_directive)
		{
			write_proc_block(_external_directive);
			if (_external_directive.modulename == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_external_directive.modulename.visit(this);
			}
			if (_external_directive.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_external_directive.name.visit(this);
			}
		}


		public void visit(using_list _using_list)
		{
			bw.Write((Int16)139);
			write_using_list(_using_list);
		}

		public void write_using_list(using_list _using_list)
		{
			write_syntax_tree_node(_using_list);
			if (_using_list.namespaces == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_using_list.namespaces.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _using_list.namespaces.Count; ssyy_i++)
				{
					if (_using_list.namespaces[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_using_list.namespaces[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(jump_stmt _jump_stmt)
		{
			bw.Write((Int16)140);
			write_jump_stmt(_jump_stmt);
		}

		public void write_jump_stmt(jump_stmt _jump_stmt)
		{
			write_statement(_jump_stmt);
			if (_jump_stmt.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_jump_stmt.expr.visit(this);
			}
			bw.Write((byte)_jump_stmt.JumpType);
		}


		public void visit(loop_stmt _loop_stmt)
		{
			bw.Write((Int16)141);
			write_loop_stmt(_loop_stmt);
		}

		public void write_loop_stmt(loop_stmt _loop_stmt)
		{
			write_statement(_loop_stmt);
			if (_loop_stmt.count == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_loop_stmt.count.visit(this);
			}
			if (_loop_stmt.stmt == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_loop_stmt.stmt.visit(this);
			}
		}


		public void visit(foreach_stmt _foreach_stmt)
		{
			bw.Write((Int16)142);
			write_foreach_stmt(_foreach_stmt);
		}

		public void write_foreach_stmt(foreach_stmt _foreach_stmt)
		{
			write_statement(_foreach_stmt);
			if (_foreach_stmt.identifier == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt.identifier.visit(this);
			}
			if (_foreach_stmt.type_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt.type_name.visit(this);
			}
			if (_foreach_stmt.in_what == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt.in_what.visit(this);
			}
			if (_foreach_stmt.stmt == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt.stmt.visit(this);
			}
			if (_foreach_stmt.index == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt.index.visit(this);
			}
		}


		public void visit(addressed_value_funcname _addressed_value_funcname)
		{
			bw.Write((Int16)143);
			write_addressed_value_funcname(_addressed_value_funcname);
		}

		public void write_addressed_value_funcname(addressed_value_funcname _addressed_value_funcname)
		{
			write_addressed_value(_addressed_value_funcname);
		}


		public void visit(named_type_reference_list _named_type_reference_list)
		{
			bw.Write((Int16)144);
			write_named_type_reference_list(_named_type_reference_list);
		}

		public void write_named_type_reference_list(named_type_reference_list _named_type_reference_list)
		{
			write_syntax_tree_node(_named_type_reference_list);
			if (_named_type_reference_list.types == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_named_type_reference_list.types.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _named_type_reference_list.types.Count; ssyy_i++)
				{
					if (_named_type_reference_list.types[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_named_type_reference_list.types[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(template_param_list _template_param_list)
		{
			bw.Write((Int16)145);
			write_template_param_list(_template_param_list);
		}

		public void write_template_param_list(template_param_list _template_param_list)
		{
			write_dereference(_template_param_list);
			if (_template_param_list.params_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_template_param_list.params_list.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _template_param_list.params_list.Count; ssyy_i++)
				{
					if (_template_param_list.params_list[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_template_param_list.params_list[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(template_type_reference _template_type_reference)
		{
			bw.Write((Int16)146);
			write_template_type_reference(_template_type_reference);
		}

		public void write_template_type_reference(template_type_reference _template_type_reference)
		{
			write_named_type_reference(_template_type_reference);
			if (_template_type_reference.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_template_type_reference.name.visit(this);
			}
			if (_template_type_reference.params_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_template_type_reference.params_list.visit(this);
			}
		}


		public void visit(int64_const _int64_const)
		{
			bw.Write((Int16)147);
			write_int64_const(_int64_const);
		}

		public void write_int64_const(int64_const _int64_const)
		{
			write_const_node(_int64_const);
			bw.Write(_int64_const.val);
		}


		public void visit(uint64_const _uint64_const)
		{
			bw.Write((Int16)148);
			write_uint64_const(_uint64_const);
		}

		public void write_uint64_const(uint64_const _uint64_const)
		{
			write_const_node(_uint64_const);
			bw.Write(_uint64_const.val);
		}


		public void visit(new_expr _new_expr)
		{
			bw.Write((Int16)149);
			write_new_expr(_new_expr);
		}

		public void write_new_expr(new_expr _new_expr)
		{
			write_addressed_value(_new_expr);
			if (_new_expr.type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_new_expr.type.visit(this);
			}
			if (_new_expr.params_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_new_expr.params_list.visit(this);
			}
			bw.Write(_new_expr.new_array);
			if (_new_expr.array_init_expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_new_expr.array_init_expr.visit(this);
			}
		}


		public void visit(where_type_specificator_list _where_type_specificator_list)
		{
			bw.Write((Int16)150);
			write_where_type_specificator_list(_where_type_specificator_list);
		}

		public void write_where_type_specificator_list(where_type_specificator_list _where_type_specificator_list)
		{
			write_syntax_tree_node(_where_type_specificator_list);
			if (_where_type_specificator_list.defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_where_type_specificator_list.defs.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _where_type_specificator_list.defs.Count; ssyy_i++)
				{
					if (_where_type_specificator_list.defs[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_where_type_specificator_list.defs[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(where_definition _where_definition)
		{
			bw.Write((Int16)151);
			write_where_definition(_where_definition);
		}

		public void write_where_definition(where_definition _where_definition)
		{
			write_syntax_tree_node(_where_definition);
			if (_where_definition.names == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_where_definition.names.visit(this);
			}
			if (_where_definition.types == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_where_definition.types.visit(this);
			}
		}


		public void visit(where_definition_list _where_definition_list)
		{
			bw.Write((Int16)152);
			write_where_definition_list(_where_definition_list);
		}

		public void write_where_definition_list(where_definition_list _where_definition_list)
		{
			write_syntax_tree_node(_where_definition_list);
			if (_where_definition_list.defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_where_definition_list.defs.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _where_definition_list.defs.Count; ssyy_i++)
				{
					if (_where_definition_list.defs[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_where_definition_list.defs[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(sizeof_operator _sizeof_operator)
		{
			bw.Write((Int16)153);
			write_sizeof_operator(_sizeof_operator);
		}

		public void write_sizeof_operator(sizeof_operator _sizeof_operator)
		{
			write_addressed_value(_sizeof_operator);
			if (_sizeof_operator.type_def == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_sizeof_operator.type_def.visit(this);
			}
			if (_sizeof_operator.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_sizeof_operator.expr.visit(this);
			}
		}


		public void visit(typeof_operator _typeof_operator)
		{
			bw.Write((Int16)154);
			write_typeof_operator(_typeof_operator);
		}

		public void write_typeof_operator(typeof_operator _typeof_operator)
		{
			write_addressed_value(_typeof_operator);
			if (_typeof_operator.type_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_typeof_operator.type_name.visit(this);
			}
		}


		public void visit(compiler_directive _compiler_directive)
		{
			bw.Write((Int16)155);
			write_compiler_directive(_compiler_directive);
		}

		public void write_compiler_directive(compiler_directive _compiler_directive)
		{
			write_syntax_tree_node(_compiler_directive);
			if (_compiler_directive.Name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_compiler_directive.Name.visit(this);
			}
			if (_compiler_directive.Directive == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_compiler_directive.Directive.visit(this);
			}
		}


		public void visit(operator_name_ident _operator_name_ident)
		{
			bw.Write((Int16)156);
			write_operator_name_ident(_operator_name_ident);
		}

		public void write_operator_name_ident(operator_name_ident _operator_name_ident)
		{
			write_ident(_operator_name_ident);
			bw.Write((byte)_operator_name_ident.operator_type);
		}


		public void visit(var_statement _var_statement)
		{
			bw.Write((Int16)157);
			write_var_statement(_var_statement);
		}

		public void write_var_statement(var_statement _var_statement)
		{
			write_statement(_var_statement);
			if (_var_statement.var_def == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_var_statement.var_def.visit(this);
			}
		}


		public void visit(question_colon_expression _question_colon_expression)
		{
			bw.Write((Int16)158);
			write_question_colon_expression(_question_colon_expression);
		}

		public void write_question_colon_expression(question_colon_expression _question_colon_expression)
		{
			write_addressed_value(_question_colon_expression);
			if (_question_colon_expression.condition == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_question_colon_expression.condition.visit(this);
			}
			if (_question_colon_expression.ret_if_true == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_question_colon_expression.ret_if_true.visit(this);
			}
			if (_question_colon_expression.ret_if_false == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_question_colon_expression.ret_if_false.visit(this);
			}
		}


		public void visit(expression_as_statement _expression_as_statement)
		{
			bw.Write((Int16)159);
			write_expression_as_statement(_expression_as_statement);
		}

		public void write_expression_as_statement(expression_as_statement _expression_as_statement)
		{
			write_statement(_expression_as_statement);
			if (_expression_as_statement.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_expression_as_statement.expr.visit(this);
			}
		}


		public void visit(c_scalar_type _c_scalar_type)
		{
			bw.Write((Int16)160);
			write_c_scalar_type(_c_scalar_type);
		}

		public void write_c_scalar_type(c_scalar_type _c_scalar_type)
		{
			write_type_definition(_c_scalar_type);
			bw.Write((byte)_c_scalar_type.scalar_name);
			bw.Write((byte)_c_scalar_type.sign);
		}


		public void visit(c_module _c_module)
		{
			bw.Write((Int16)161);
			write_c_module(_c_module);
		}

		public void write_c_module(c_module _c_module)
		{
			write_compilation_unit(_c_module);
			if (_c_module.defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_c_module.defs.visit(this);
			}
			if (_c_module.used_units == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_c_module.used_units.visit(this);
			}
		}


		public void visit(declarations_as_statement _declarations_as_statement)
		{
			bw.Write((Int16)162);
			write_declarations_as_statement(_declarations_as_statement);
		}

		public void write_declarations_as_statement(declarations_as_statement _declarations_as_statement)
		{
			write_statement(_declarations_as_statement);
			if (_declarations_as_statement.defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_declarations_as_statement.defs.visit(this);
			}
		}


		public void visit(array_size _array_size)
		{
			bw.Write((Int16)163);
			write_array_size(_array_size);
		}

		public void write_array_size(array_size _array_size)
		{
			write_type_definition(_array_size);
			if (_array_size.max_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_array_size.max_value.visit(this);
			}
		}


		public void visit(enumerator _enumerator)
		{
			bw.Write((Int16)164);
			write_enumerator(_enumerator);
		}

		public void write_enumerator(enumerator _enumerator)
		{
			write_syntax_tree_node(_enumerator);
			if (_enumerator.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_enumerator.name.visit(this);
			}
			if (_enumerator.value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_enumerator.value.visit(this);
			}
		}


		public void visit(enumerator_list _enumerator_list)
		{
			bw.Write((Int16)165);
			write_enumerator_list(_enumerator_list);
		}

		public void write_enumerator_list(enumerator_list _enumerator_list)
		{
			write_syntax_tree_node(_enumerator_list);
			if (_enumerator_list.enumerators == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_enumerator_list.enumerators.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _enumerator_list.enumerators.Count; ssyy_i++)
				{
					if (_enumerator_list.enumerators[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_enumerator_list.enumerators[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(c_for_cycle _c_for_cycle)
		{
			bw.Write((Int16)166);
			write_c_for_cycle(_c_for_cycle);
		}

		public void write_c_for_cycle(c_for_cycle _c_for_cycle)
		{
			write_statement(_c_for_cycle);
			if (_c_for_cycle.expr1 == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_c_for_cycle.expr1.visit(this);
			}
			if (_c_for_cycle.expr2 == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_c_for_cycle.expr2.visit(this);
			}
			if (_c_for_cycle.expr3 == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_c_for_cycle.expr3.visit(this);
			}
			if (_c_for_cycle.stmt == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_c_for_cycle.stmt.visit(this);
			}
		}


		public void visit(switch_stmt _switch_stmt)
		{
			bw.Write((Int16)167);
			write_switch_stmt(_switch_stmt);
		}

		public void write_switch_stmt(switch_stmt _switch_stmt)
		{
			write_statement(_switch_stmt);
			if (_switch_stmt.condition == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_switch_stmt.condition.visit(this);
			}
			if (_switch_stmt.stmt == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_switch_stmt.stmt.visit(this);
			}
			bw.Write((byte)_switch_stmt.Part);
		}


		public void visit(type_definition_attr_list _type_definition_attr_list)
		{
			bw.Write((Int16)168);
			write_type_definition_attr_list(_type_definition_attr_list);
		}

		public void write_type_definition_attr_list(type_definition_attr_list _type_definition_attr_list)
		{
			write_syntax_tree_node(_type_definition_attr_list);
			if (_type_definition_attr_list.attributes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_type_definition_attr_list.attributes.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _type_definition_attr_list.attributes.Count; ssyy_i++)
				{
					if (_type_definition_attr_list.attributes[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_type_definition_attr_list.attributes[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(type_definition_attr _type_definition_attr)
		{
			bw.Write((Int16)169);
			write_type_definition_attr(_type_definition_attr);
		}

		public void write_type_definition_attr(type_definition_attr _type_definition_attr)
		{
			write_type_definition(_type_definition_attr);
			bw.Write((byte)_type_definition_attr.attr);
		}


		public void visit(lock_stmt _lock_stmt)
		{
			bw.Write((Int16)170);
			write_lock_stmt(_lock_stmt);
		}

		public void write_lock_stmt(lock_stmt _lock_stmt)
		{
			write_statement(_lock_stmt);
			if (_lock_stmt.lock_object == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_lock_stmt.lock_object.visit(this);
			}
			if (_lock_stmt.stmt == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_lock_stmt.stmt.visit(this);
			}
		}


		public void visit(compiler_directive_list _compiler_directive_list)
		{
			bw.Write((Int16)171);
			write_compiler_directive_list(_compiler_directive_list);
		}

		public void write_compiler_directive_list(compiler_directive_list _compiler_directive_list)
		{
			write_compiler_directive(_compiler_directive_list);
			if (_compiler_directive_list.directives == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_compiler_directive_list.directives.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _compiler_directive_list.directives.Count; ssyy_i++)
				{
					if (_compiler_directive_list.directives[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_compiler_directive_list.directives[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(compiler_directive_if _compiler_directive_if)
		{
			bw.Write((Int16)172);
			write_compiler_directive_if(_compiler_directive_if);
		}

		public void write_compiler_directive_if(compiler_directive_if _compiler_directive_if)
		{
			write_compiler_directive(_compiler_directive_if);
			if (_compiler_directive_if.if_part == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_compiler_directive_if.if_part.visit(this);
			}
			if (_compiler_directive_if.elseif_part == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_compiler_directive_if.elseif_part.visit(this);
			}
		}


		public void visit(documentation_comment_list _documentation_comment_list)
		{
			bw.Write((Int16)173);
			write_documentation_comment_list(_documentation_comment_list);
		}

		public void write_documentation_comment_list(documentation_comment_list _documentation_comment_list)
		{
			write_syntax_tree_node(_documentation_comment_list);
			if (_documentation_comment_list.sections == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_documentation_comment_list.sections.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _documentation_comment_list.sections.Count; ssyy_i++)
				{
					if (_documentation_comment_list.sections[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_documentation_comment_list.sections[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(documentation_comment_tag _documentation_comment_tag)
		{
			bw.Write((Int16)174);
			write_documentation_comment_tag(_documentation_comment_tag);
		}

		public void write_documentation_comment_tag(documentation_comment_tag _documentation_comment_tag)
		{
			write_syntax_tree_node(_documentation_comment_tag);
			if (_documentation_comment_tag.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_documentation_comment_tag.name);
			}
			if (_documentation_comment_tag.parameters == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_documentation_comment_tag.parameters.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _documentation_comment_tag.parameters.Count; ssyy_i++)
				{
					if (_documentation_comment_tag.parameters[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_documentation_comment_tag.parameters[ssyy_i].visit(this);
					}
				}
			}
			if (_documentation_comment_tag.text == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_documentation_comment_tag.text);
			}
		}


		public void visit(documentation_comment_tag_param _documentation_comment_tag_param)
		{
			bw.Write((Int16)175);
			write_documentation_comment_tag_param(_documentation_comment_tag_param);
		}

		public void write_documentation_comment_tag_param(documentation_comment_tag_param _documentation_comment_tag_param)
		{
			write_syntax_tree_node(_documentation_comment_tag_param);
			if (_documentation_comment_tag_param.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_documentation_comment_tag_param.name);
			}
			if (_documentation_comment_tag_param.value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_documentation_comment_tag_param.value);
			}
		}


		public void visit(documentation_comment_section _documentation_comment_section)
		{
			bw.Write((Int16)176);
			write_documentation_comment_section(_documentation_comment_section);
		}

		public void write_documentation_comment_section(documentation_comment_section _documentation_comment_section)
		{
			write_syntax_tree_node(_documentation_comment_section);
			if (_documentation_comment_section.tags == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_documentation_comment_section.tags.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _documentation_comment_section.tags.Count; ssyy_i++)
				{
					if (_documentation_comment_section.tags[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_documentation_comment_section.tags[ssyy_i].visit(this);
					}
				}
			}
			if (_documentation_comment_section.text == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_documentation_comment_section.text);
			}
		}


		public void visit(token_taginfo _token_taginfo)
		{
			bw.Write((Int16)177);
			write_token_taginfo(_token_taginfo);
		}

		public void write_token_taginfo(token_taginfo _token_taginfo)
		{
			write_token_info(_token_taginfo);
			bw.Write((byte)_token_taginfo.tag);
		}


		public void visit(declaration_specificator _declaration_specificator)
		{
			bw.Write((Int16)178);
			write_declaration_specificator(_declaration_specificator);
		}

		public void write_declaration_specificator(declaration_specificator _declaration_specificator)
		{
			write_type_definition(_declaration_specificator);
			bw.Write((byte)_declaration_specificator.specificator);
			if (_declaration_specificator.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_declaration_specificator.name);
			}
		}


		public void visit(ident_with_templateparams _ident_with_templateparams)
		{
			bw.Write((Int16)179);
			write_ident_with_templateparams(_ident_with_templateparams);
		}

		public void write_ident_with_templateparams(ident_with_templateparams _ident_with_templateparams)
		{
			write_addressed_value_funcname(_ident_with_templateparams);
			if (_ident_with_templateparams.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_ident_with_templateparams.name.visit(this);
			}
			if (_ident_with_templateparams.template_params == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_ident_with_templateparams.template_params.visit(this);
			}
		}


		public void visit(template_type_name _template_type_name)
		{
			bw.Write((Int16)180);
			write_template_type_name(_template_type_name);
		}

		public void write_template_type_name(template_type_name _template_type_name)
		{
			write_ident(_template_type_name);
			if (_template_type_name.template_args == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_template_type_name.template_args.visit(this);
			}
		}


		public void visit(default_operator _default_operator)
		{
			bw.Write((Int16)181);
			write_default_operator(_default_operator);
		}

		public void write_default_operator(default_operator _default_operator)
		{
			write_addressed_value(_default_operator);
			if (_default_operator.type_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_default_operator.type_name.visit(this);
			}
		}


		public void visit(bracket_expr _bracket_expr)
		{
			bw.Write((Int16)182);
			write_bracket_expr(_bracket_expr);
		}

		public void write_bracket_expr(bracket_expr _bracket_expr)
		{
			write_addressed_value(_bracket_expr);
			if (_bracket_expr.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_bracket_expr.expr.visit(this);
			}
		}


		public void visit(attribute _attribute)
		{
			bw.Write((Int16)183);
			write_attribute(_attribute);
		}

		public void write_attribute(attribute _attribute)
		{
			write_syntax_tree_node(_attribute);
			if (_attribute.qualifier == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_attribute.qualifier.visit(this);
			}
			if (_attribute.type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_attribute.type.visit(this);
			}
			if (_attribute.arguments == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_attribute.arguments.visit(this);
			}
		}


		public void visit(simple_attribute_list _simple_attribute_list)
		{
			bw.Write((Int16)184);
			write_simple_attribute_list(_simple_attribute_list);
		}

		public void write_simple_attribute_list(simple_attribute_list _simple_attribute_list)
		{
			write_syntax_tree_node(_simple_attribute_list);
			if (_simple_attribute_list.attributes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_simple_attribute_list.attributes.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _simple_attribute_list.attributes.Count; ssyy_i++)
				{
					if (_simple_attribute_list.attributes[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_simple_attribute_list.attributes[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(attribute_list _attribute_list)
		{
			bw.Write((Int16)185);
			write_attribute_list(_attribute_list);
		}

		public void write_attribute_list(attribute_list _attribute_list)
		{
			write_syntax_tree_node(_attribute_list);
			if (_attribute_list.attributes == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_attribute_list.attributes.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _attribute_list.attributes.Count; ssyy_i++)
				{
					if (_attribute_list.attributes[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_attribute_list.attributes[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(function_lambda_definition _function_lambda_definition)
		{
			bw.Write((Int16)186);
			write_function_lambda_definition(_function_lambda_definition);
		}

		public void write_function_lambda_definition(function_lambda_definition _function_lambda_definition)
		{
			write_expression(_function_lambda_definition);
			if (_function_lambda_definition.ident_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_lambda_definition.ident_list.visit(this);
			}
			if (_function_lambda_definition.return_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_lambda_definition.return_type.visit(this);
			}
			if (_function_lambda_definition.formal_parameters == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_lambda_definition.formal_parameters.visit(this);
			}
			if (_function_lambda_definition.proc_body == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_lambda_definition.proc_body.visit(this);
			}
			bw.Write((byte)_function_lambda_definition.proc_definition);
			if (_function_lambda_definition.parameters == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_lambda_definition.parameters.visit(this);
			}
			if (_function_lambda_definition.lambda_name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_function_lambda_definition.lambda_name);
			}
			if (_function_lambda_definition.defs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_function_lambda_definition.defs.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _function_lambda_definition.defs.Count; ssyy_i++)
				{
					if (_function_lambda_definition.defs[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_function_lambda_definition.defs[ssyy_i].visit(this);
					}
				}
			}
			bw.Write((byte)_function_lambda_definition.lambda_visit_mode);
			if (_function_lambda_definition.substituting_node == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_lambda_definition.substituting_node.visit(this);
			}
			bw.Write((byte)_function_lambda_definition.usedkeyword);
		}


		public void visit(function_lambda_call _function_lambda_call)
		{
			bw.Write((Int16)187);
			write_function_lambda_call(_function_lambda_call);
		}

		public void write_function_lambda_call(function_lambda_call _function_lambda_call)
		{
			write_expression(_function_lambda_call);
			if (_function_lambda_call.f_lambda_def == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_lambda_call.f_lambda_def.visit(this);
			}
			if (_function_lambda_call.parameters == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_function_lambda_call.parameters.visit(this);
			}
		}


		public void visit(semantic_check _semantic_check)
		{
			bw.Write((Int16)188);
			write_semantic_check(_semantic_check);
		}

		public void write_semantic_check(semantic_check _semantic_check)
		{
			write_statement(_semantic_check);
			if (_semantic_check.CheckName == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_semantic_check.CheckName);
			}
			if (_semantic_check.param == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_semantic_check.param.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _semantic_check.param.Count; ssyy_i++)
				{
					if (_semantic_check.param[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_semantic_check.param[ssyy_i].visit(this);
					}
				}
			}
			bw.Write((byte)_semantic_check.fictive);
		}


		public void visit(lambda_inferred_type _lambda_inferred_type)
		{
			bw.Write((Int16)189);
			write_lambda_inferred_type(_lambda_inferred_type);
		}

		public void write_lambda_inferred_type(lambda_inferred_type _lambda_inferred_type)
		{
			write_type_definition(_lambda_inferred_type);
			bw.Write((byte)_lambda_inferred_type.real_type);
		}


		public void visit(same_type_node _same_type_node)
		{
			bw.Write((Int16)190);
			write_same_type_node(_same_type_node);
		}

		public void write_same_type_node(same_type_node _same_type_node)
		{
			write_type_definition(_same_type_node);
			if (_same_type_node.ex == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_same_type_node.ex.visit(this);
			}
		}


		public void visit(name_assign_expr _name_assign_expr)
		{
			bw.Write((Int16)191);
			write_name_assign_expr(_name_assign_expr);
		}

		public void write_name_assign_expr(name_assign_expr _name_assign_expr)
		{
			write_expression(_name_assign_expr);
			if (_name_assign_expr.name == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_name_assign_expr.name.visit(this);
			}
			if (_name_assign_expr.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_name_assign_expr.expr.visit(this);
			}
		}


		public void visit(name_assign_expr_list _name_assign_expr_list)
		{
			bw.Write((Int16)192);
			write_name_assign_expr_list(_name_assign_expr_list);
		}

		public void write_name_assign_expr_list(name_assign_expr_list _name_assign_expr_list)
		{
			write_syntax_tree_node(_name_assign_expr_list);
			if (_name_assign_expr_list.name_expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_name_assign_expr_list.name_expr.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _name_assign_expr_list.name_expr.Count; ssyy_i++)
				{
					if (_name_assign_expr_list.name_expr[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_name_assign_expr_list.name_expr[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(unnamed_type_object _unnamed_type_object)
		{
			bw.Write((Int16)193);
			write_unnamed_type_object(_unnamed_type_object);
		}

		public void write_unnamed_type_object(unnamed_type_object _unnamed_type_object)
		{
			write_addressed_value(_unnamed_type_object);
			if (_unnamed_type_object.ne_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unnamed_type_object.ne_list.visit(this);
			}
			bw.Write(_unnamed_type_object.is_class);
			if (_unnamed_type_object.new_ex == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_unnamed_type_object.new_ex.visit(this);
			}
		}


		public void visit(semantic_type_node _semantic_type_node)
		{
			bw.Write((Int16)194);
			write_semantic_type_node(_semantic_type_node);
		}

		public void write_semantic_type_node(semantic_type_node _semantic_type_node)
		{
			write_type_definition(_semantic_type_node);
			bw.Write((byte)_semantic_type_node.type);
		}


		public void visit(short_func_definition _short_func_definition)
		{
			bw.Write((Int16)195);
			write_short_func_definition(_short_func_definition);
		}

		public void write_short_func_definition(short_func_definition _short_func_definition)
		{
			write_procedure_definition(_short_func_definition);
			if (_short_func_definition.procdef == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_short_func_definition.procdef.visit(this);
			}
		}


		public void visit(no_type_foreach _no_type_foreach)
		{
			bw.Write((Int16)196);
			write_no_type_foreach(_no_type_foreach);
		}

		public void write_no_type_foreach(no_type_foreach _no_type_foreach)
		{
			write_type_definition(_no_type_foreach);
		}


		public void visit(matching_expression _matching_expression)
		{
			bw.Write((Int16)197);
			write_matching_expression(_matching_expression);
		}

		public void write_matching_expression(matching_expression _matching_expression)
		{
			write_addressed_value(_matching_expression);
			if (_matching_expression.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_matching_expression.left.visit(this);
			}
			if (_matching_expression.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_matching_expression.right.visit(this);
			}
		}


		public void visit(closure_substituting_node _closure_substituting_node)
		{
			bw.Write((Int16)198);
			write_closure_substituting_node(_closure_substituting_node);
		}

		public void write_closure_substituting_node(closure_substituting_node _closure_substituting_node)
		{
			write_ident(_closure_substituting_node);
			if (_closure_substituting_node.substitution == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_closure_substituting_node.substitution.visit(this);
			}
		}


		public void visit(sequence_type _sequence_type)
		{
			bw.Write((Int16)199);
			write_sequence_type(_sequence_type);
		}

		public void write_sequence_type(sequence_type _sequence_type)
		{
			write_type_definition(_sequence_type);
			if (_sequence_type.elements_type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_sequence_type.elements_type.visit(this);
			}
		}


		public void visit(modern_proc_type _modern_proc_type)
		{
			bw.Write((Int16)200);
			write_modern_proc_type(_modern_proc_type);
		}

		public void write_modern_proc_type(modern_proc_type _modern_proc_type)
		{
			write_type_definition(_modern_proc_type);
			if (_modern_proc_type.aloneparam == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_modern_proc_type.aloneparam.visit(this);
			}
			if (_modern_proc_type.el == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_modern_proc_type.el.visit(this);
			}
			if (_modern_proc_type.res == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_modern_proc_type.res.visit(this);
			}
		}


		public void visit(yield_node _yield_node)
		{
			bw.Write((Int16)201);
			write_yield_node(_yield_node);
		}

		public void write_yield_node(yield_node _yield_node)
		{
			write_statement(_yield_node);
			if (_yield_node.ex == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_yield_node.ex.visit(this);
			}
		}


		public void visit(template_operator_name _template_operator_name)
		{
			bw.Write((Int16)202);
			write_template_operator_name(_template_operator_name);
		}

		public void write_template_operator_name(template_operator_name _template_operator_name)
		{
			write_template_type_name(_template_operator_name);
			if (_template_operator_name.opname == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_template_operator_name.opname.visit(this);
			}
		}


		public void visit(semantic_addr_value _semantic_addr_value)
		{
			bw.Write((Int16)203);
			write_semantic_addr_value(_semantic_addr_value);
		}

		public void write_semantic_addr_value(semantic_addr_value _semantic_addr_value)
		{
			write_addressed_value(_semantic_addr_value);
			bw.Write((byte)_semantic_addr_value.expr);
		}


		public void visit(pair_type_stlist _pair_type_stlist)
		{
			bw.Write((Int16)204);
			write_pair_type_stlist(_pair_type_stlist);
		}

		public void write_pair_type_stlist(pair_type_stlist _pair_type_stlist)
		{
			write_syntax_tree_node(_pair_type_stlist);
			if (_pair_type_stlist.tn == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_pair_type_stlist.tn.visit(this);
			}
			if (_pair_type_stlist.exprs == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_pair_type_stlist.exprs.visit(this);
			}
		}


		public void visit(assign_tuple _assign_tuple)
		{
			bw.Write((Int16)205);
			write_assign_tuple(_assign_tuple);
		}

		public void write_assign_tuple(assign_tuple _assign_tuple)
		{
			write_statement(_assign_tuple);
			if (_assign_tuple.vars == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_assign_tuple.vars.visit(this);
			}
			if (_assign_tuple.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_assign_tuple.expr.visit(this);
			}
		}


		public void visit(addressed_value_list _addressed_value_list)
		{
			bw.Write((Int16)206);
			write_addressed_value_list(_addressed_value_list);
		}

		public void write_addressed_value_list(addressed_value_list _addressed_value_list)
		{
			write_syntax_tree_node(_addressed_value_list);
			if (_addressed_value_list.variables == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_addressed_value_list.variables.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _addressed_value_list.variables.Count; ssyy_i++)
				{
					if (_addressed_value_list.variables[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_addressed_value_list.variables[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(tuple_node _tuple_node)
		{
			bw.Write((Int16)207);
			write_tuple_node(_tuple_node);
		}

		public void write_tuple_node(tuple_node _tuple_node)
		{
			write_addressed_value(_tuple_node);
			if (_tuple_node.el == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_tuple_node.el.visit(this);
			}
		}


		public void visit(uses_closure _uses_closure)
		{
			bw.Write((Int16)208);
			write_uses_closure(_uses_closure);
		}

		public void write_uses_closure(uses_closure _uses_closure)
		{
			write_uses_list(_uses_closure);
			if (_uses_closure.listunitsections == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_uses_closure.listunitsections.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _uses_closure.listunitsections.Count; ssyy_i++)
				{
					if (_uses_closure.listunitsections[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_uses_closure.listunitsections[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(dot_question_node _dot_question_node)
		{
			bw.Write((Int16)209);
			write_dot_question_node(_dot_question_node);
		}

		public void write_dot_question_node(dot_question_node _dot_question_node)
		{
			write_addressed_value_funcname(_dot_question_node);
			if (_dot_question_node.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_dot_question_node.left.visit(this);
			}
			if (_dot_question_node.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_dot_question_node.right.visit(this);
			}
		}


		public void visit(slice_expr _slice_expr)
		{
			bw.Write((Int16)210);
			write_slice_expr(_slice_expr);
		}

		public void write_slice_expr(slice_expr _slice_expr)
		{
			write_dereference(_slice_expr);
			if (_slice_expr.v == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_slice_expr.v.visit(this);
			}
			if (_slice_expr.from == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_slice_expr.from.visit(this);
			}
			if (_slice_expr.to == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_slice_expr.to.visit(this);
			}
			if (_slice_expr.step == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_slice_expr.step.visit(this);
			}
		}


		public void visit(no_type _no_type)
		{
			bw.Write((Int16)211);
			write_no_type(_no_type);
		}

		public void write_no_type(no_type _no_type)
		{
			write_type_definition(_no_type);
		}


		public void visit(yield_unknown_ident _yield_unknown_ident)
		{
			bw.Write((Int16)212);
			write_yield_unknown_ident(_yield_unknown_ident);
		}

		public void write_yield_unknown_ident(yield_unknown_ident _yield_unknown_ident)
		{
			write_ident(_yield_unknown_ident);
			if (_yield_unknown_ident.UnknownID == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_yield_unknown_ident.UnknownID.visit(this);
			}
		}


		public void visit(yield_unknown_expression_type _yield_unknown_expression_type)
		{
			bw.Write((Int16)213);
			write_yield_unknown_expression_type(_yield_unknown_expression_type);
		}

		public void write_yield_unknown_expression_type(yield_unknown_expression_type _yield_unknown_expression_type)
		{
			write_type_definition(_yield_unknown_expression_type);
		}


		public void visit(yield_unknown_foreach_type _yield_unknown_foreach_type)
		{
			bw.Write((Int16)214);
			write_yield_unknown_foreach_type(_yield_unknown_foreach_type);
		}

		public void write_yield_unknown_foreach_type(yield_unknown_foreach_type _yield_unknown_foreach_type)
		{
			write_type_definition(_yield_unknown_foreach_type);
		}


		public void visit(yield_sequence_node _yield_sequence_node)
		{
			bw.Write((Int16)215);
			write_yield_sequence_node(_yield_sequence_node);
		}

		public void write_yield_sequence_node(yield_sequence_node _yield_sequence_node)
		{
			write_statement(_yield_sequence_node);
			if (_yield_sequence_node.ex == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_yield_sequence_node.ex.visit(this);
			}
		}


		public void visit(assign_var_tuple _assign_var_tuple)
		{
			bw.Write((Int16)216);
			write_assign_var_tuple(_assign_var_tuple);
		}

		public void write_assign_var_tuple(assign_var_tuple _assign_var_tuple)
		{
			write_statement(_assign_var_tuple);
			if (_assign_var_tuple.idents == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_assign_var_tuple.idents.visit(this);
			}
			if (_assign_var_tuple.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_assign_var_tuple.expr.visit(this);
			}
		}


		public void visit(slice_expr_question _slice_expr_question)
		{
			bw.Write((Int16)217);
			write_slice_expr_question(_slice_expr_question);
		}

		public void write_slice_expr_question(slice_expr_question _slice_expr_question)
		{
			write_slice_expr(_slice_expr_question);
		}


		public void visit(semantic_check_sugared_statement_node _semantic_check_sugared_statement_node)
		{
			bw.Write((Int16)218);
			write_semantic_check_sugared_statement_node(_semantic_check_sugared_statement_node);
		}

		public void write_semantic_check_sugared_statement_node(semantic_check_sugared_statement_node _semantic_check_sugared_statement_node)
		{
			write_statement(_semantic_check_sugared_statement_node);
			bw.Write((byte)_semantic_check_sugared_statement_node.typ);
			if (_semantic_check_sugared_statement_node.lst == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_semantic_check_sugared_statement_node.lst.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _semantic_check_sugared_statement_node.lst.Count; ssyy_i++)
				{
					if (_semantic_check_sugared_statement_node.lst[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_semantic_check_sugared_statement_node.lst[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(sugared_expression _sugared_expression)
		{
			bw.Write((Int16)219);
			write_sugared_expression(_sugared_expression);
		}

		public void write_sugared_expression(sugared_expression _sugared_expression)
		{
			write_expression(_sugared_expression);
			bw.Write((byte)_sugared_expression.sugared_expr);
			if (_sugared_expression.new_expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_sugared_expression.new_expr.visit(this);
			}
		}


		public void visit(sugared_addressed_value _sugared_addressed_value)
		{
			bw.Write((Int16)220);
			write_sugared_addressed_value(_sugared_addressed_value);
		}

		public void write_sugared_addressed_value(sugared_addressed_value _sugared_addressed_value)
		{
			write_addressed_value(_sugared_addressed_value);
			bw.Write((byte)_sugared_addressed_value.sugared_expr);
			if (_sugared_addressed_value.new_addr_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_sugared_addressed_value.new_addr_value.visit(this);
			}
		}


		public void visit(double_question_node _double_question_node)
		{
			bw.Write((Int16)221);
			write_double_question_node(_double_question_node);
		}

		public void write_double_question_node(double_question_node _double_question_node)
		{
			write_addressed_value_funcname(_double_question_node);
			if (_double_question_node.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_double_question_node.left.visit(this);
			}
			if (_double_question_node.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_double_question_node.right.visit(this);
			}
		}


		public void visit(pattern_node _pattern_node)
		{
			bw.Write((Int16)222);
			write_pattern_node(_pattern_node);
		}

		public void write_pattern_node(pattern_node _pattern_node)
		{
			write_syntax_tree_node(_pattern_node);
			if (_pattern_node.parameters == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_pattern_node.parameters.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _pattern_node.parameters.Count; ssyy_i++)
				{
					if (_pattern_node.parameters[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_pattern_node.parameters[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(type_pattern _type_pattern)
		{
			bw.Write((Int16)223);
			write_type_pattern(_type_pattern);
		}

		public void write_type_pattern(type_pattern _type_pattern)
		{
			write_pattern_node(_type_pattern);
			if (_type_pattern.identifier == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_type_pattern.identifier.visit(this);
			}
			if (_type_pattern.type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_type_pattern.type.visit(this);
			}
		}


		public void visit(is_pattern_expr _is_pattern_expr)
		{
			bw.Write((Int16)224);
			write_is_pattern_expr(_is_pattern_expr);
		}

		public void write_is_pattern_expr(is_pattern_expr _is_pattern_expr)
		{
			write_addressed_value(_is_pattern_expr);
			if (_is_pattern_expr.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_is_pattern_expr.left.visit(this);
			}
			if (_is_pattern_expr.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_is_pattern_expr.right.visit(this);
			}
		}


		public void visit(match_with _match_with)
		{
			bw.Write((Int16)225);
			write_match_with(_match_with);
		}

		public void write_match_with(match_with _match_with)
		{
			write_statement(_match_with);
			if (_match_with.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_match_with.expr.visit(this);
			}
			if (_match_with.case_list == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_match_with.case_list.visit(this);
			}
			if (_match_with.defaultAction == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_match_with.defaultAction.visit(this);
			}
		}


		public void visit(pattern_case _pattern_case)
		{
			bw.Write((Int16)226);
			write_pattern_case(_pattern_case);
		}

		public void write_pattern_case(pattern_case _pattern_case)
		{
			write_statement(_pattern_case);
			if (_pattern_case.pattern == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_pattern_case.pattern.visit(this);
			}
			if (_pattern_case.case_action == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_pattern_case.case_action.visit(this);
			}
			if (_pattern_case.condition == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_pattern_case.condition.visit(this);
			}
		}


		public void visit(pattern_cases _pattern_cases)
		{
			bw.Write((Int16)227);
			write_pattern_cases(_pattern_cases);
		}

		public void write_pattern_cases(pattern_cases _pattern_cases)
		{
			write_statement(_pattern_cases);
			if (_pattern_cases.elements == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_pattern_cases.elements.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _pattern_cases.elements.Count; ssyy_i++)
				{
					if (_pattern_cases.elements[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_pattern_cases.elements[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(deconstructor_pattern _deconstructor_pattern)
		{
			bw.Write((Int16)228);
			write_deconstructor_pattern(_deconstructor_pattern);
		}

		public void write_deconstructor_pattern(deconstructor_pattern _deconstructor_pattern)
		{
			write_pattern_node(_deconstructor_pattern);
			if (_deconstructor_pattern.type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_deconstructor_pattern.type.visit(this);
			}
			if (_deconstructor_pattern.const_params_check == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_deconstructor_pattern.const_params_check.visit(this);
			}
		}


		public void visit(pattern_parameter _pattern_parameter)
		{
			bw.Write((Int16)229);
			write_pattern_parameter(_pattern_parameter);
		}

		public void write_pattern_parameter(pattern_parameter _pattern_parameter)
		{
			write_syntax_tree_node(_pattern_parameter);
		}


		public void visit(desugared_deconstruction _desugared_deconstruction)
		{
			bw.Write((Int16)230);
			write_desugared_deconstruction(_desugared_deconstruction);
		}

		public void write_desugared_deconstruction(desugared_deconstruction _desugared_deconstruction)
		{
			write_statement(_desugared_deconstruction);
			if (_desugared_deconstruction.variables == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_desugared_deconstruction.variables.visit(this);
			}
			if (_desugared_deconstruction.deconstruction_target == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_desugared_deconstruction.deconstruction_target.visit(this);
			}
		}


		public void visit(var_deconstructor_parameter _var_deconstructor_parameter)
		{
			bw.Write((Int16)231);
			write_var_deconstructor_parameter(_var_deconstructor_parameter);
		}

		public void write_var_deconstructor_parameter(var_deconstructor_parameter _var_deconstructor_parameter)
		{
			write_pattern_parameter(_var_deconstructor_parameter);
			if (_var_deconstructor_parameter.identifier == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_var_deconstructor_parameter.identifier.visit(this);
			}
			if (_var_deconstructor_parameter.type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_var_deconstructor_parameter.type.visit(this);
			}
			bw.Write(_var_deconstructor_parameter.var_keyword_used);
		}


		public void visit(recursive_deconstructor_parameter _recursive_deconstructor_parameter)
		{
			bw.Write((Int16)232);
			write_recursive_deconstructor_parameter(_recursive_deconstructor_parameter);
		}

		public void write_recursive_deconstructor_parameter(recursive_deconstructor_parameter _recursive_deconstructor_parameter)
		{
			write_recursive_pattern_parameter(_recursive_deconstructor_parameter);
		}


		public void visit(deconstruction_variables_definition _deconstruction_variables_definition)
		{
			bw.Write((Int16)233);
			write_deconstruction_variables_definition(_deconstruction_variables_definition);
		}

		public void write_deconstruction_variables_definition(deconstruction_variables_definition _deconstruction_variables_definition)
		{
			write_declaration(_deconstruction_variables_definition);
			if (_deconstruction_variables_definition.definitions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_deconstruction_variables_definition.definitions.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _deconstruction_variables_definition.definitions.Count; ssyy_i++)
				{
					if (_deconstruction_variables_definition.definitions[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_deconstruction_variables_definition.definitions[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(var_tuple_def_statement _var_tuple_def_statement)
		{
			bw.Write((Int16)234);
			write_var_tuple_def_statement(_var_tuple_def_statement);
		}

		public void write_var_tuple_def_statement(var_tuple_def_statement _var_tuple_def_statement)
		{
			write_var_def_statement(_var_tuple_def_statement);
		}


		public void visit(semantic_check_sugared_var_def_statement_node _semantic_check_sugared_var_def_statement_node)
		{
			bw.Write((Int16)235);
			write_semantic_check_sugared_var_def_statement_node(_semantic_check_sugared_var_def_statement_node);
		}

		public void write_semantic_check_sugared_var_def_statement_node(semantic_check_sugared_var_def_statement_node _semantic_check_sugared_var_def_statement_node)
		{
			write_var_def_statement(_semantic_check_sugared_var_def_statement_node);
			bw.Write((byte)_semantic_check_sugared_var_def_statement_node.typ);
			if (_semantic_check_sugared_var_def_statement_node.lst == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_semantic_check_sugared_var_def_statement_node.lst.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _semantic_check_sugared_var_def_statement_node.lst.Count; ssyy_i++)
				{
					if (_semantic_check_sugared_var_def_statement_node.lst[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_semantic_check_sugared_var_def_statement_node.lst[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(const_pattern _const_pattern)
		{
			bw.Write((Int16)236);
			write_const_pattern(_const_pattern);
		}

		public void write_const_pattern(const_pattern _const_pattern)
		{
			write_pattern_node(_const_pattern);
			if (_const_pattern.pattern_expressions == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_const_pattern.pattern_expressions.visit(this);
			}
		}


		public void visit(tuple_pattern_wild_card _tuple_pattern_wild_card)
		{
			bw.Write((Int16)237);
			write_tuple_pattern_wild_card(_tuple_pattern_wild_card);
		}

		public void write_tuple_pattern_wild_card(tuple_pattern_wild_card _tuple_pattern_wild_card)
		{
			write_pattern_parameter(_tuple_pattern_wild_card);
		}


		public void visit(const_pattern_parameter _const_pattern_parameter)
		{
			bw.Write((Int16)238);
			write_const_pattern_parameter(_const_pattern_parameter);
		}

		public void write_const_pattern_parameter(const_pattern_parameter _const_pattern_parameter)
		{
			write_pattern_parameter(_const_pattern_parameter);
			if (_const_pattern_parameter.const_param == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_const_pattern_parameter.const_param.visit(this);
			}
		}


		public void visit(wild_card_deconstructor_parameter _wild_card_deconstructor_parameter)
		{
			bw.Write((Int16)239);
			write_wild_card_deconstructor_parameter(_wild_card_deconstructor_parameter);
		}

		public void write_wild_card_deconstructor_parameter(wild_card_deconstructor_parameter _wild_card_deconstructor_parameter)
		{
			write_pattern_parameter(_wild_card_deconstructor_parameter);
		}


		public void visit(collection_pattern _collection_pattern)
		{
			bw.Write((Int16)240);
			write_collection_pattern(_collection_pattern);
		}

		public void write_collection_pattern(collection_pattern _collection_pattern)
		{
			write_pattern_node(_collection_pattern);
		}


		public void visit(collection_pattern_gap_parameter _collection_pattern_gap_parameter)
		{
			bw.Write((Int16)241);
			write_collection_pattern_gap_parameter(_collection_pattern_gap_parameter);
		}

		public void write_collection_pattern_gap_parameter(collection_pattern_gap_parameter _collection_pattern_gap_parameter)
		{
			write_pattern_parameter(_collection_pattern_gap_parameter);
		}


		public void visit(collection_pattern_wild_card _collection_pattern_wild_card)
		{
			bw.Write((Int16)242);
			write_collection_pattern_wild_card(_collection_pattern_wild_card);
		}

		public void write_collection_pattern_wild_card(collection_pattern_wild_card _collection_pattern_wild_card)
		{
			write_pattern_parameter(_collection_pattern_wild_card);
		}


		public void visit(collection_pattern_var_parameter _collection_pattern_var_parameter)
		{
			bw.Write((Int16)243);
			write_collection_pattern_var_parameter(_collection_pattern_var_parameter);
		}

		public void write_collection_pattern_var_parameter(collection_pattern_var_parameter _collection_pattern_var_parameter)
		{
			write_pattern_parameter(_collection_pattern_var_parameter);
			if (_collection_pattern_var_parameter.identifier == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_collection_pattern_var_parameter.identifier.visit(this);
			}
			if (_collection_pattern_var_parameter.type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_collection_pattern_var_parameter.type.visit(this);
			}
		}


		public void visit(recursive_collection_parameter _recursive_collection_parameter)
		{
			bw.Write((Int16)244);
			write_recursive_collection_parameter(_recursive_collection_parameter);
		}

		public void write_recursive_collection_parameter(recursive_collection_parameter _recursive_collection_parameter)
		{
			write_recursive_pattern_parameter(_recursive_collection_parameter);
		}


		public void visit(recursive_pattern_parameter _recursive_pattern_parameter)
		{
			bw.Write((Int16)245);
			write_recursive_pattern_parameter(_recursive_pattern_parameter);
		}

		public void write_recursive_pattern_parameter(recursive_pattern_parameter _recursive_pattern_parameter)
		{
			write_pattern_parameter(_recursive_pattern_parameter);
			if (_recursive_pattern_parameter.pattern == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_recursive_pattern_parameter.pattern.visit(this);
			}
		}


		public void visit(tuple_pattern _tuple_pattern)
		{
			bw.Write((Int16)246);
			write_tuple_pattern(_tuple_pattern);
		}

		public void write_tuple_pattern(tuple_pattern _tuple_pattern)
		{
			write_pattern_node(_tuple_pattern);
		}


		public void visit(tuple_pattern_var_parameter _tuple_pattern_var_parameter)
		{
			bw.Write((Int16)247);
			write_tuple_pattern_var_parameter(_tuple_pattern_var_parameter);
		}

		public void write_tuple_pattern_var_parameter(tuple_pattern_var_parameter _tuple_pattern_var_parameter)
		{
			write_pattern_parameter(_tuple_pattern_var_parameter);
			if (_tuple_pattern_var_parameter.identifier == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_tuple_pattern_var_parameter.identifier.visit(this);
			}
			if (_tuple_pattern_var_parameter.type == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_tuple_pattern_var_parameter.type.visit(this);
			}
		}


		public void visit(recursive_tuple_parameter _recursive_tuple_parameter)
		{
			bw.Write((Int16)248);
			write_recursive_tuple_parameter(_recursive_tuple_parameter);
		}

		public void write_recursive_tuple_parameter(recursive_tuple_parameter _recursive_tuple_parameter)
		{
			write_recursive_pattern_parameter(_recursive_tuple_parameter);
		}


		public void visit(diapason_expr_new _diapason_expr_new)
		{
			bw.Write((Int16)249);
			write_diapason_expr_new(_diapason_expr_new);
		}

		public void write_diapason_expr_new(diapason_expr_new _diapason_expr_new)
		{
			write_addressed_value(_diapason_expr_new);
			if (_diapason_expr_new.left == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_diapason_expr_new.left.visit(this);
			}
			if (_diapason_expr_new.right == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_diapason_expr_new.right.visit(this);
			}
		}


		public void visit(if_expr_new _if_expr_new)
		{
			bw.Write((Int16)250);
			write_if_expr_new(_if_expr_new);
		}

		public void write_if_expr_new(if_expr_new _if_expr_new)
		{
			write_expression(_if_expr_new);
			if (_if_expr_new.condition == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_if_expr_new.condition.visit(this);
			}
			if (_if_expr_new.if_true == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_if_expr_new.if_true.visit(this);
			}
			if (_if_expr_new.if_false == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_if_expr_new.if_false.visit(this);
			}
		}


		public void visit(simple_expr_with_deref _simple_expr_with_deref)
		{
			bw.Write((Int16)251);
			write_simple_expr_with_deref(_simple_expr_with_deref);
		}

		public void write_simple_expr_with_deref(simple_expr_with_deref _simple_expr_with_deref)
		{
			write_expression(_simple_expr_with_deref);
			if (_simple_expr_with_deref.simple_expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_simple_expr_with_deref.simple_expr.visit(this);
			}
			bw.Write(_simple_expr_with_deref.has_deref);
		}


		public void visit(index _index)
		{
			bw.Write((Int16)252);
			write_index(_index);
		}

		public void write_index(index _index)
		{
			write_expression(_index);
			if (_index.index_expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_index.index_expr.visit(this);
			}
			bw.Write(_index.inverted);
		}


		public void visit(array_const_new _array_const_new)
		{
			bw.Write((Int16)253);
			write_array_const_new(_array_const_new);
		}

		public void write_array_const_new(array_const_new _array_const_new)
		{
			write_addressed_value(_array_const_new);
			if (_array_const_new.elements == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_array_const_new.elements.visit(this);
			}
		}


		public void visit(semantic_ith_element_of _semantic_ith_element_of)
		{
			bw.Write((Int16)254);
			write_semantic_ith_element_of(_semantic_ith_element_of);
		}

		public void write_semantic_ith_element_of(semantic_ith_element_of _semantic_ith_element_of)
		{
			write_expression(_semantic_ith_element_of);
			if (_semantic_ith_element_of.id == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_semantic_ith_element_of.id.visit(this);
			}
			if (_semantic_ith_element_of.index == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_semantic_ith_element_of.index.visit(this);
			}
		}


		public void visit(bigint_const _bigint_const)
		{
			bw.Write((Int16)255);
			write_bigint_const(_bigint_const);
		}

		public void write_bigint_const(bigint_const _bigint_const)
		{
			write_const_node(_bigint_const);
			bw.Write(_bigint_const.val);
		}


		public void visit(foreach_stmt_formatting _foreach_stmt_formatting)
		{
			bw.Write((Int16)256);
			write_foreach_stmt_formatting(_foreach_stmt_formatting);
		}

		public void write_foreach_stmt_formatting(foreach_stmt_formatting _foreach_stmt_formatting)
		{
			write_statement(_foreach_stmt_formatting);
			if (_foreach_stmt_formatting.il == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt_formatting.il.visit(this);
			}
			if (_foreach_stmt_formatting.in_what == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt_formatting.in_what.visit(this);
			}
			if (_foreach_stmt_formatting.stmt == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt_formatting.stmt.visit(this);
			}
			if (_foreach_stmt_formatting.index == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_foreach_stmt_formatting.index.visit(this);
			}
		}


		public void visit(property_ident _property_ident)
		{
			bw.Write((Int16)257);
			write_property_ident(_property_ident);
		}

		public void write_property_ident(property_ident _property_ident)
		{
			write_ident(_property_ident);
			if (_property_ident.ln == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				bw.Write(_property_ident.ln.Count);
				for(Int32 ssyy_i = 0; ssyy_i < _property_ident.ln.Count; ssyy_i++)
				{
					if (_property_ident.ln[ssyy_i] == null)
					{
						bw.Write((byte)0);
					}
					else
					{
						bw.Write((byte)1);
						_property_ident.ln[ssyy_i].visit(this);
					}
				}
			}
		}


		public void visit(expression_with_let _expression_with_let)
		{
			bw.Write((Int16)258);
			write_expression_with_let(_expression_with_let);
		}

		public void write_expression_with_let(expression_with_let _expression_with_let)
		{
			write_addressed_value(_expression_with_let);
			if (_expression_with_let.stat == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_expression_with_let.stat.visit(this);
			}
			if (_expression_with_let.expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_expression_with_let.expr.visit(this);
			}
		}


		public void visit(lambda_any_type_node_syntax _lambda_any_type_node_syntax)
		{
			bw.Write((Int16)259);
			write_lambda_any_type_node_syntax(_lambda_any_type_node_syntax);
		}

		public void write_lambda_any_type_node_syntax(lambda_any_type_node_syntax _lambda_any_type_node_syntax)
		{
			write_expression(_lambda_any_type_node_syntax);
		}


		public void visit(ref_var_def_statement _ref_var_def_statement)
		{
			bw.Write((Int16)260);
			write_ref_var_def_statement(_ref_var_def_statement);
		}

		public void write_ref_var_def_statement(ref_var_def_statement _ref_var_def_statement)
		{
			write_declaration(_ref_var_def_statement);
			if (_ref_var_def_statement.var == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_ref_var_def_statement.var.visit(this);
			}
			if (_ref_var_def_statement.initial_value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_ref_var_def_statement.initial_value.visit(this);
			}
		}


		public void visit(let_var_expr _let_var_expr)
		{
			bw.Write((Int16)261);
			write_let_var_expr(_let_var_expr);
		}

		public void write_let_var_expr(let_var_expr _let_var_expr)
		{
			write_expression(_let_var_expr);
			if (_let_var_expr.id == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_let_var_expr.id.visit(this);
			}
			if (_let_var_expr.ex == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_let_var_expr.ex.visit(this);
			}
		}


		public void visit(to_expr _to_expr)
		{
			bw.Write((Int16)262);
			write_to_expr(_to_expr);
		}

		public void write_to_expr(to_expr _to_expr)
		{
			write_expression(_to_expr);
			if (_to_expr.key == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_to_expr.key.visit(this);
			}
			if (_to_expr.value == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_to_expr.value.visit(this);
			}
		}


		public void visit(global_statement _global_statement)
		{
			bw.Write((Int16)263);
			write_global_statement(_global_statement);
		}

		public void write_global_statement(global_statement _global_statement)
		{
			write_statement(_global_statement);
			if (_global_statement.idents == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_global_statement.idents.visit(this);
			}
		}


		public void visit(list_generator _list_generator)
		{
			bw.Write((Int16)264);
			write_list_generator(_list_generator);
		}

		public void write_list_generator(list_generator _list_generator)
		{
			write_expression(_list_generator);
			if (_list_generator._expr == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_list_generator._expr.visit(this);
			}
			if (_list_generator._ident == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_list_generator._ident.visit(this);
			}
			if (_list_generator._range == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_list_generator._range.visit(this);
			}
			if (_list_generator._condition == null)
			{
				bw.Write((byte)0);
			}
			else
			{
				bw.Write((byte)1);
				_list_generator._condition.visit(this);
			}
		}

	}


}

