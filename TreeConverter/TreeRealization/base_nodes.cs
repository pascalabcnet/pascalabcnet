// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Каждый узел семантического дерева должен уметь возвращать свой тип.
    /// Это список всех возможных типов узлов семантического дерева.
    /// </summary>
    public enum semantic_node_type
    {
        none, namespace_constant_definition, class_constant_definition, function_constant_definition, compiled_class_constant_definition,
        bool_const_node, long_const_node, byte_const_node, int_const_node, double_const_node, char_const_node,
        sbyte_const_node, short_const_node, ushort_const_node, uint_const_node, ulong_const_node, float_const_node,
        basic_function_call, common_namespace_function_call, common_in_function_function_call, common_method_call,
        common_static_method_call, compiled_function_call, compiled_static_method_call,
        basic_function_node, common_namespace_function_node, common_in_function_function_node, common_method_node,
        compiled_function_node, common_namespace_node, compiled_namespace_node, program_node, dll_node,
        common_property_node, basic_property_node, compiled_property_node, if_node, while_node, repeat_node, for_node,
        statements_list, basic_type_node, common_type_node, compiled_type_node, common_unit_node, compiled_unit_node, local_variable_reference,
        namespace_variable_reference, class_field_reference, static_class_field_reference, compiled_variable_reference,
        static_compiled_variable_reference, common_parameter_reference, local_variable, namespace_variable, class_field,
        compiled_variable_definition, basic_parameter, compiled_parameter, common_parameter, basic_interface_node,
        empty_statement, this_node, return_node, string_const_node, empty_function_node, common_constructor_call,
        compiled_constructor_call, pseudo_function, while_break_node, repeat_break_node, for_break_node,
        while_continue_node, repeat_continue_node, for_continue_node, compiled_constructor_node, simple_array,
        static_property_reference, non_static_property_reference, simple_array_indexing,
        external_statement_node, ref_type_node, get_addr_node, deref_node, wrap_def, throw_statement,
        switch_node, case_variant_node, case_range_node, null_const_node, null_type_node, unsized_array, delegated_method,
        convert_types_function_node, runtime_statement, typed_expression, is_node, as_node, sizeof_operator, typeof_operator,
        exit_procedure, exception_filter, try_block, template_type, common_event, compiled_event, static_event_reference,
        nonstatic_event_reference, array_const, statement_expression_node, question_colon_expression, record_const,
        type_synonym, label, labeled_statement, goto_statement, compiled_static_method_call_node_as_constant, enum_const,
        common_namespace_function_call_node_as_constant, foreach_node, lock_statement, local_block_variable,
        local_block_variable_reference,compiled_constructor_call_as_constant, rethrow_statement, short_string,
        foreach_break_node, foreach_continue_node, generic_indicator, namespace_constant_reference, function_constant_reference,
        common_constructor_call_as_constant, array_initializer, record_initializer, default_operator, attribute_node,
        pinvoke_node, basic_function_call_node_as_constant, compiled_static_field_reference_as_constant,
        common_namespace_event, indefinite_definition_node, indefinite_type, indefinite_function_call, indefinite_reference,
        wrapped_statement, wrapped_expression, default_operator_node_as_constant, double_question_colon_expression,
        typeof_operator_as_constant, common_static_method_call_node_as_constant, sizeof_operator_as_constant
    };

    /// <summary>
    /// Для работы часто необходимо знать лищь обобщенный тип узла.
    /// Например, часто достаточно знать что конкретный узел является выражением и не важно каким.
    /// Каждый узел семантического дерева должен уметь возвращать свой обобщенный тип.
    /// Это список всех возможных обобщенных типов узлов.
    /// </summary>
	public enum general_node_type {type_node, function_node, namespace_node, unit_node, variable_node,
		property_node, constant_definition, statement, expression, program_node, interface_node,
        template_type, event_node, type_synonym, label, generic_indicator, attribute_node};

    /// <summary>
    /// Базовый абстрактный класс для всех выражений.
    /// </summary>
	[Serializable]
	public abstract class semantic_node : SemanticTree.ISemanticNode
	{
        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
		public abstract general_node_type general_node_type
		{
			get;
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public abstract semantic_node_type semantic_node_type
		{
			get;
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public virtual void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}

    /// <summary>
    /// Базовый класс для всех определений в программе.
    /// К определениям относятся переменные, константы, свойства, методы и обычные функции, поля, пространства имен,
    /// сама программа или dll, типы и модули.
    /// </summary>
	[Serializable]
	public abstract class definition_node : semantic_node, SemanticTree.IDefinitionNode
	{
        protected string doc;
		
		protected attributes_list _attributes;
		
		/// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
		
		public attributes_list attributes
		{
			get
			{
				if (_attributes == null)
					_attributes = new attributes_list();
				return _attributes;
			}
		}
		
		public SemanticTree.IAttributeNode[] Attributes
		{
			get
			{
				return attributes.ToArray();
			}
		}
		
		public virtual string documentation
		{
			get
			{
				return doc;
			}
			set
			{
				doc = value;
			}
		}
		
		string SemanticTree.IDefinitionNode.Documentation
		{
			get
			{
				return doc;
			}
		}

        public virtual location location
        {
            get
            {
                return null;
            }
        }

        public virtual semantic_node find_by_location(int line, int col)
        {
            return null;
        }
	}

    /// <summary>
    /// Базовый класс для всех statement-ов.
    /// </summary>
	[Serializable]
	public abstract class statement_node : semantic_node, SemanticTree.IStatementNode
	{
        /// <summary>
        /// Расположение statement-а в программе.
        /// </summary>
		private location _loc;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="loc">Расположение statement-а в программе.</param>
        public statement_node()
        {
        }

        public statement_node(location loc)
        {
            _loc = loc;
        }

        /// <summary>
        /// Расположение statement-а в программе.
        /// </summary>
        public location location
        {
            get
            {
                return _loc;
            }
            set
            {
                _loc=value;
            }
        }

        /// <summary>
        /// Расположение statement-а в программе.
        /// </summary>
		SemanticTree.ILocation SemanticTree.ILocated.Location
		{
			get
			{
				return _loc;
			}
		}

        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
		public override general_node_type general_node_type
		{
			get
			{
				return general_node_type.statement;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}

    /// <summary>
    /// Базовый класс для всех выражений.
    /// </summary>
	[Serializable]
	public abstract class expression_node : statement_node, SemanticTree.IExpressionNode
	{
        /// <summary>
        /// Тип выражения.
        /// </summary>
		private type_node _tn;
		
		private type_node _conversion_tn;
		
        /// <summary>
        /// Конструктор выражения.
        /// </summary>
        /// <param name="tn">Тип выражения.</param>
        /// <param name="loc">Расположение выражения.</param>
        
        public expression_node()//30_01_2010_Tasha
        {
        }

		public expression_node(type_node tn, location loc) : base(loc)
		{
			_tn=tn;
		}
        
        /// <summary>
        /// Тип выражения. Используется при построении дерева.
        /// </summary>
		public virtual type_node type
		{
			get
			{
				return _tn;
			}
            set
            {
                _tn = value;
            }
		}
		
		public virtual type_node conversion_type
		{
			get
			{
				return _conversion_tn;
			}
            set
            {
                _conversion_tn = value;
            }
		}

        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
		public override general_node_type general_node_type
		{
			get
			{
				return general_node_type.expression;
			}
		}

        /// <summary>
        /// Является ли это выражение lvalue.
        /// Т.е. может ли это выражение стоять в левой части оператора присваивания и передаваться по ссылке.
        /// По умолчанию false.
        /// </summary>
		public virtual bool is_addressed
		{
			get
			{
				return false;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        /// <summary>
        /// Тип выражения. Используется посетителем при обходе дерева.
        /// </summary>
		SemanticTree.ITypeNode SemanticTree.IExpressionNode.type
		{
			get
			{
				return this.type;
			}
		}
		
		SemanticTree.ITypeNode SemanticTree.IExpressionNode.conversion_type
		{
			get
			{
				return this.conversion_type;
			}
		}
	}

    [Serializable]
    public class typed_expression : expression_node
    {
        public typed_expression(type_node type, location loc)
            : base(type, loc)
        {
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.typed_expression;
            }
        }
        public override string ToString() => type.ToString();
    }

    /// <summary>
    /// Базовый класс для адресных выражений.
    /// Т.е. тех, которые могут стоять в левой части оператора присваивания и передаваться по ссылке.
    /// </summary>
	[Serializable]
	public abstract class addressed_expression : expression_node, SemanticTree.IAddressedExpressionNode
	{
        /// <summary>
        /// Конструктор адресного выражения.
        /// </summary>
        /// <param name="tn">Тип выражения.</param>
        /// <param name="loc">Расположение выражения.</param>
		public addressed_expression(type_node tn, location loc) : base(tn,loc)
		{
		}

        /// <summary>
        /// Является ли это выражение lvalue.
        /// Т.е. может ли это выражение стоять в левой части оператора присваивания и передаваться по ссылке.
        /// По умолчанию true.
        /// </summary>
		public override bool is_addressed
		{
			get
			{
				return true;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

	}

    /// <summary>
    /// Класс, представляющий ссылку на объект внутри его метода.
    /// </summary>
	[Serializable]
	public class this_node : expression_node, SemanticTree.IThisNode
	{
        /// <summary>
        /// Конструктор this_node.
        /// </summary>
        /// <param name="type">Тип объекта.</param>
        /// <param name="loc">Расположение выражения.</param>
		public this_node(type_node type, location loc) : base(type,loc)
		{
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.this_node;
			}
		}

        public override bool is_addressed
        {
            get
            {
                if (type.is_value_type)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}
	
    /// <summary>
    /// Класс, представляющий операцию получения адреса объекта.
    /// </summary>
	[Serializable]
	public class get_addr_node : expression_node, SemanticTree.IGetAddrNode
	{
        /// <summary>
        /// Выражение, адрес которого мы получаем.
        /// </summary>
		private readonly expression_node _addr_of;
		
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="addr_of">Выражение, адрес которого мы получаем.</param>
        /// <param name="loc">Расположение узла.</param>
		public get_addr_node(expression_node addr_of, location loc) :
            base(addr_of.type.ref_type, loc)
		{
			_addr_of = addr_of;
		}
		
        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.get_addr_node;
			}
		}
		
        /// <summary>
        /// Выражение, адрес которого мы получаем.
        /// Это свойство используется при генерации дерева.
        /// </summary>
		public expression_node addr_of
		{
			get 
			{
				return _addr_of;
			}
		}
		
        /// <summary>
        /// Выражение, адрес которого мы получаем.
        /// Испоьзуется при обходе дерева посетителем.
        /// </summary>
		SemanticTree.IExpressionNode SemanticTree.IGetAddrNode.addr_of_expr
		{
			get
			{
				return _addr_of;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}
	
    /// <summary>
    /// Класс, представляющий операцию разыменования объекта.
    /// </summary>
	[Serializable]
	public class dereference_node : addressed_expression, SemanticTree.IDereferenceNode
	{
        /// <summary>
        /// Выражение-указатель.
        /// </summary>
		private expression_node _deref_expr;
		
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="deref_expr">Выражение-указатель.</param>
        /// <param name="loc">Расположение выражения.</param>
		public dereference_node(expression_node deref_expr, location loc) :
           //base(PascalABCCompiler.SystemLibrary.SystemLibrary.get_pointed_type_by_type(deref_expr.type),loc)
           base( (deref_expr.type as ref_type_node).pointed_type, loc)
		{
			_deref_expr = deref_expr;
		}
		
        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.deref_node;
			}
		}
		
        /// <summary>
        /// Выражение-указатель, которое мы разыменовываем.
        /// Это свойство используется при генерации дерева.
        /// </summary>
		public expression_node deref_expr
		{
			get 
			{
				return _deref_expr;
			}
			set
			{
				_deref_expr = value;
			}
		}
		
        /// <summary>
        /// Выражение-указатель, которое мы разыменовываем.
        /// Это свойство используется при обходе дерева посетителем.
        /// </summary>
		SemanticTree.IExpressionNode SemanticTree.IDereferenceNode.derefered_expr
		{
			get
			{
				return _deref_expr;
			}
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}

    [Serializable]
    public class expression_with_let_semantic : expression_node
    {
        public expression_with_let_semantic(type_node type, location loc)
            : base(type, loc)
        {
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.typed_expression;
            }
        }
        public override string ToString() => type.ToString();
    }

}
