
using System;
using System.Collections;
using System.Collections.Generic;

namespace PascalABCCompiler.SyntaxTree
{
	///<summary>
	///Базовый класс для всех классов синтаксического дерева
	///</summary>
	[Serializable]
	public class syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public syntax_tree_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public syntax_tree_node(SourceContext _source_context)
		{
			this._source_context=_source_context;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public syntax_tree_node(SourceContext _source_context,SourceContext sc)
		{
			this._source_context=_source_context;
			source_context = sc;
		}

		protected SourceContext _source_context;

		///<summary>
		///Позиция в тексте (строка-столбец начала - строка-столбец конца)
		///</summary>
		public SourceContext source_context
		{
			get
			{
				return _source_context;
			}
			set
			{
				_source_context=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public virtual void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Выражение
	///</summary>
	[Serializable]
	public class expression : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public expression()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Оператор
	///</summary>
	[Serializable]
	public class statement : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public statement()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Блок операторов
	///</summary>
	[Serializable]
	public class statement_list : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public statement_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public statement_list(List<statement> _subnodes,syntax_tree_node _left_logical_bracket,syntax_tree_node _right_logical_bracket)
		{
			this._subnodes=_subnodes;
			this._left_logical_bracket=_left_logical_bracket;
			this._right_logical_bracket=_right_logical_bracket;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public statement_list(List<statement> _subnodes,syntax_tree_node _left_logical_bracket,syntax_tree_node _right_logical_bracket,SourceContext sc)
		{
			this._subnodes=_subnodes;
			this._left_logical_bracket=_left_logical_bracket;
			this._right_logical_bracket=_right_logical_bracket;
			source_context = sc;
		}

		protected List<statement> _subnodes=new List<statement>();
		protected syntax_tree_node _left_logical_bracket;
		protected syntax_tree_node _right_logical_bracket;

		///<summary>
		///Список операторов
		///</summary>
		public List<statement> subnodes
		{
			get
			{
				return _subnodes;
			}
			set
			{
				_subnodes=value;
			}
		}

		///<summary>
		///Левая операторная скобка
		///</summary>
		public syntax_tree_node left_logical_bracket
		{
			get
			{
				return _left_logical_bracket;
			}
			set
			{
				_left_logical_bracket=value;
			}
		}

		///<summary>
		///Правая операторная скобка
		///</summary>
		public syntax_tree_node right_logical_bracket
		{
			get
			{
				return _right_logical_bracket;
			}
			set
			{
				_right_logical_bracket=value;
			}
		}


		public statement_list(statement _statement, SourceContext sc)
		{
		    Add(_statement,sc);
		}
		public statement_list Add(statement _statement)
		{
		    subnodes.Add(_statement);
		    return this;
		}
		public statement_list Add(statement _statement, SourceContext sc)
		{
		    subnodes.Add(_statement);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Идентификатор
	///</summary>
	[Serializable]
	public class ident : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public ident()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ident(string _name)
		{
			this._name=_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ident(string _name,SourceContext sc)
		{
			this._name=_name;
			source_context = sc;
		}

		protected string _name;

		///<summary>
		///Строка, представляющая идентификатор
		///</summary>
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Оператор присваивания
	///</summary>
	[Serializable]
	public class assign : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public assign()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public assign(addressed_value _to,expression _from,Operators _operator_type)
		{
			this._to=_to;
			this._from=_from;
			this._operator_type=_operator_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public assign(addressed_value _to,expression _from,Operators _operator_type,SourceContext sc)
		{
			this._to=_to;
			this._from=_from;
			this._operator_type=_operator_type;
			source_context = sc;
		}

		protected addressed_value _to;
		protected expression _from;
		protected Operators _operator_type;

		///<summary>
		///Левый операнд оператора присваивания (чему присваивать).
		///</summary>
		public addressed_value to
		{
			get
			{
				return _to;
			}
			set
			{
				_to=value;
			}
		}

		///<summary>
		///Выражение в правой части
		///</summary>
		public expression from
		{
			get
			{
				return _from;
			}
			set
			{
				_from=value;
			}
		}

		///<summary>
		///Тип оператора присваивания
		///</summary>
		public Operators operator_type
		{
			get
			{
				return _operator_type;
			}
			set
			{
				_operator_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Бинарное выражение
	///</summary>
	[Serializable]
	public class bin_expr : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public bin_expr()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public bin_expr(expression _left,expression _right,Operators _operation_type)
		{
			this._left=_left;
			this._right=_right;
			this._operation_type=_operation_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public bin_expr(expression _left,expression _right,Operators _operation_type,SourceContext sc)
		{
			this._left=_left;
			this._right=_right;
			this._operation_type=_operation_type;
			source_context = sc;
		}

		protected expression _left;
		protected expression _right;
		protected Operators _operation_type;

		///<summary>
		///
		///</summary>
		public expression left
		{
			get
			{
				return _left;
			}
			set
			{
				_left=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression right
		{
			get
			{
				return _right;
			}
			set
			{
				_right=value;
			}
		}

		///<summary>
		///
		///</summary>
		public Operators operation_type
		{
			get
			{
				return _operation_type;
			}
			set
			{
				_operation_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Унарное выражение
	///</summary>
	[Serializable]
	public class un_expr : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public un_expr()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public un_expr(expression _subnode,Operators _operation_type)
		{
			this._subnode=_subnode;
			this._operation_type=_operation_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public un_expr(expression _subnode,Operators _operation_type,SourceContext sc)
		{
			this._subnode=_subnode;
			this._operation_type=_operation_type;
			source_context = sc;
		}

		protected expression _subnode;
		protected Operators _operation_type;

		///<summary>
		///
		///</summary>
		public expression subnode
		{
			get
			{
				return _subnode;
			}
			set
			{
				_subnode=value;
			}
		}

		///<summary>
		///
		///</summary>
		public Operators operation_type
		{
			get
			{
				return _operation_type;
			}
			set
			{
				_operation_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Константа
	///</summary>
	[Serializable]
	public class const_node : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public const_node()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Логическая константа
	///</summary>
	[Serializable]
	public class bool_const : const_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public bool_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public bool_const(bool _val)
		{
			this._val=_val;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public bool_const(bool _val,SourceContext sc)
		{
			this._val=_val;
			source_context = sc;
		}

		protected bool _val;

		///<summary>
		///Значение логической константы
		///</summary>
		public bool val
		{
			get
			{
				return _val;
			}
			set
			{
				_val=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Целая константа
	///</summary>
	[Serializable]
	public class int32_const : const_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public int32_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public int32_const(Int32 _val)
		{
			this._val=_val;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public int32_const(Int32 _val,SourceContext sc)
		{
			this._val=_val;
			source_context = sc;
		}

		protected Int32 _val;

		///<summary>
		///Значение
		///</summary>
		public Int32 val
		{
			get
			{
				return _val;
			}
			set
			{
				_val=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Вещественная константа
	///</summary>
	[Serializable]
	public class double_const : const_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public double_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public double_const(double _val)
		{
			this._val=_val;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public double_const(double _val,SourceContext sc)
		{
			this._val=_val;
			source_context = sc;
		}

		protected double _val;

		///<summary>
		///Значение вещественной константы
		///</summary>
		public double val
		{
			get
			{
				return _val;
			}
			set
			{
				_val=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Тело подпрограммы
	///</summary>
	[Serializable]
	public class subprogram_body : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public subprogram_body()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public subprogram_body(statement_list _subprogram_code,declarations _subprogram_defs)
		{
			this._subprogram_code=_subprogram_code;
			this._subprogram_defs=_subprogram_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public subprogram_body(statement_list _subprogram_code,declarations _subprogram_defs,SourceContext sc)
		{
			this._subprogram_code=_subprogram_code;
			this._subprogram_defs=_subprogram_defs;
			source_context = sc;
		}

		protected statement_list _subprogram_code;
		protected declarations _subprogram_defs;

		///<summary>
		///Блок операторов подпрограммы
		///</summary>
		public statement_list subprogram_code
		{
			get
			{
				return _subprogram_code;
			}
			set
			{
				_subprogram_code=value;
			}
		}

		///<summary>
		///Описания подпрограммы
		///</summary>
		public declarations subprogram_defs
		{
			get
			{
				return _subprogram_defs;
			}
			set
			{
				_subprogram_defs=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Значение, имеющее адрес
	///</summary>
	[Serializable]
	public class addressed_value : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public addressed_value()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Определение типа
	///</summary>
	[Serializable]
	public class type_definition : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public type_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition(type_definition_attr_list _attr_list)
		{
			this._attr_list=_attr_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition(type_definition_attr_list _attr_list,SourceContext sc)
		{
			this._attr_list=_attr_list;
			source_context = sc;
		}

		protected type_definition_attr_list _attr_list;

		///<summary>
		///
		///</summary>
		public type_definition_attr_list attr_list
		{
			get
			{
				return _attr_list;
			}
			set
			{
				_attr_list=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class roof_dereference : dereference
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public roof_dereference()
		{

		}


		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public roof_dereference(addressed_value _dereferencing_value)
		{
			this._dereferencing_value=_dereferencing_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public roof_dereference(addressed_value _dereferencing_value,SourceContext sc)
		{
			this._dereferencing_value=_dereferencing_value;
			source_context = sc;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Именованное определение типа
	///</summary>
	[Serializable]
	public class named_type_reference : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public named_type_reference()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public named_type_reference(List<ident> _names)
		{
			this._names=_names;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public named_type_reference(List<ident> _names,SourceContext sc)
		{
			this._names=_names;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public named_type_reference(type_definition_attr_list _attr_list,List<ident> _names)
		{
			this._attr_list=_attr_list;
			this._names=_names;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public named_type_reference(type_definition_attr_list _attr_list,List<ident> _names,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._names=_names;
			source_context = sc;
		}

		protected List<ident> _names=new List<ident>();

		///<summary>
		///Список имен типа
		///</summary>
		public List<ident> names
		{
			get
			{
				return _names;
			}
			set
			{
				_names=value;
			}
		}


		public void Add(string name)
		{
		  names.Add(new ident(name));
		}
		public void Add(string name, SourceContext sc)
		{
		  names.Add(new ident(name));
		  source_context = sc;
		}
		public named_type_reference(string name, SourceContext sc)
		{
		  Add(name,sc);
		}
		public named_type_reference(ident _ident, SourceContext sc)
		{
		    Add(_ident,sc);
		}
		public named_type_reference Add(ident _ident)
		{
		    names.Add(_ident);
		    return this;
		}
		public named_type_reference Add(ident _ident, SourceContext sc)
		{
		    names.Add(_ident);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Секция описания переменных
	///</summary>
	[Serializable]
	public class variable_definitions : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public variable_definitions()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variable_definitions(List<var_def_statement> _var_definitions)
		{
			this._var_definitions=_var_definitions;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variable_definitions(List<var_def_statement> _var_definitions,SourceContext sc)
		{
			this._var_definitions=_var_definitions;
			source_context = sc;
		}

		protected List<var_def_statement> _var_definitions=new List<var_def_statement>();

		///<summary>
		///Список описаний переменных
		///</summary>
		public List<var_def_statement> var_definitions
		{
			get
			{
				return _var_definitions;
			}
			set
			{
				_var_definitions=value;
			}
		}


		public variable_definitions(var_def_statement _var_def_statement, SourceContext sc)
		{
		    Add(_var_def_statement,sc);
		}
		public variable_definitions Add(var_def_statement _var_def_statement)
		{
		    var_definitions.Add(_var_def_statement);
		    return this;
		}
		public variable_definitions Add(var_def_statement _var_def_statement, SourceContext sc)
		{
		    var_definitions.Add(_var_def_statement);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Список идентификаторов
	///</summary>
	[Serializable]
	public class ident_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public ident_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ident_list(List<ident> _idents)
		{
			this._idents=_idents;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ident_list(List<ident> _idents,SourceContext sc)
		{
			this._idents=_idents;
			source_context = sc;
		}

		protected List<ident> _idents=new List<ident>();

		///<summary>
		///Список идентификаторов
		///</summary>
		public List<ident> idents
		{
			get
			{
				return _idents;
			}
			set
			{
				_idents=value;
			}
		}


		public void Add(string name)
		{
		  idents.Add(new ident(name));
		}
		public void Add(string name, SourceContext sc)
		{
		  Add(new ident(name),sc);
		}
		public ident_list(string name, SourceContext sc)
		{
		  Add(name,sc);
		}
		public ident_list(string name)
		{
		  Add(name);
		}
		public ident_list(ident _ident, SourceContext sc)
		{
		    Add(_ident,sc);
		}
		public ident_list Add(ident _ident)
		{
		    idents.Add(_ident);
		    return this;
		}
		public ident_list Add(ident _ident, SourceContext sc)
		{
		    idents.Add(_ident);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Описание переменных
	///</summary>
	[Serializable]
	public class var_def_statement : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public var_def_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public var_def_statement(ident_list _vars,type_definition _vars_type,expression _inital_value,definition_attribute _var_attr,bool _is_event)
		{
			this._vars=_vars;
			this._vars_type=_vars_type;
			this._inital_value=_inital_value;
			this._var_attr=_var_attr;
			this._is_event=_is_event;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public var_def_statement(ident_list _vars,type_definition _vars_type,expression _inital_value,definition_attribute _var_attr,bool _is_event,SourceContext sc)
		{
			this._vars=_vars;
			this._vars_type=_vars_type;
			this._inital_value=_inital_value;
			this._var_attr=_var_attr;
			this._is_event=_is_event;
			source_context = sc;
		}

		protected ident_list _vars;
		protected type_definition _vars_type;
		protected expression _inital_value;
		protected definition_attribute _var_attr;
		protected bool _is_event;

		///<summary>
		///Список имен переменных
		///</summary>
		public ident_list vars
		{
			get
			{
				return _vars;
			}
			set
			{
				_vars=value;
			}
		}

		///<summary>
		///Тип переменных
		///</summary>
		public type_definition vars_type
		{
			get
			{
				return _vars_type;
			}
			set
			{
				_vars_type=value;
			}
		}

		///<summary>
		///Начальное значение переменных
		///</summary>
		public expression inital_value
		{
			get
			{
				return _inital_value;
			}
			set
			{
				_inital_value=value;
			}
		}

		///<summary>
		///
		///</summary>
		public definition_attribute var_attr
		{
			get
			{
				return _var_attr;
			}
			set
			{
				_var_attr=value;
			}
		}

		///<summary>
		///Являются ли переменные событиями
		///</summary>
		public bool is_event
		{
			get
			{
				return _is_event;
			}
			set
			{
				_is_event=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Описание
	///</summary>
	[Serializable]
	public class declaration : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public declaration()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declaration(attribute_list _attributes)
		{
			this._attributes=_attributes;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declaration(attribute_list _attributes,SourceContext sc)
		{
			this._attributes=_attributes;
			source_context = sc;
		}

		protected attribute_list _attributes;

		///<summary>
		///Список атрибутов описания
		///</summary>
		public attribute_list attributes
		{
			get
			{
				return _attributes;
			}
			set
			{
				_attributes=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Список описаний
	///</summary>
	[Serializable]
	public class declarations : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public declarations()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declarations(List<declaration> _defs)
		{
			this._defs=_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declarations(List<declaration> _defs,SourceContext sc)
		{
			this._defs=_defs;
			source_context = sc;
		}

		protected List<declaration> _defs=new List<declaration>();

		///<summary>
		///Список описаний
		///</summary>
		public List<declaration> defs
		{
			get
			{
				return _defs;
			}
			set
			{
				_defs=value;
			}
		}


		public declarations(declaration _declaration, SourceContext sc)
		{
		    Add(_declaration,sc);
		}
		public declarations Add(declaration _declaration)
		{
		    defs.Add(_declaration);
		    return this;
		}
		public declarations Add(declaration _declaration, SourceContext sc)
		{
		    defs.Add(_declaration);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class program_tree : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public program_tree()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_tree(List<compilation_unit> _compilation_units)
		{
			this._compilation_units=_compilation_units;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_tree(List<compilation_unit> _compilation_units,SourceContext sc)
		{
			this._compilation_units=_compilation_units;
			source_context = sc;
		}

		protected List<compilation_unit> _compilation_units=new List<compilation_unit>();

		///<summary>
		///Список подключенных модулей
		///</summary>
		public List<compilation_unit> compilation_units
		{
			get
			{
				return _compilation_units;
			}
			set
			{
				_compilation_units=value;
			}
		}


		public program_tree(compilation_unit _compilation_unit, SourceContext sc)
		{
		    Add(_compilation_unit,sc);
		}
		public program_tree Add(compilation_unit _compilation_unit)
		{
		    compilation_units.Add(_compilation_unit);
		    return this;
		}
		public program_tree Add(compilation_unit _compilation_unit, SourceContext sc)
		{
		    compilation_units.Add(_compilation_unit);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Имя программы
	///</summary>
	[Serializable]
	public class program_name : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public program_name()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_name(ident _prog_name)
		{
			this._prog_name=_prog_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_name(ident _prog_name,SourceContext sc)
		{
			this._prog_name=_prog_name;
			source_context = sc;
		}

		protected ident _prog_name;

		///<summary>
		///Идентификатор - имя программы
		///</summary>
		public ident prog_name
		{
			get
			{
				return _prog_name;
			}
			set
			{
				_prog_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Строковая константа
	///</summary>
	[Serializable]
	public class string_const : literal
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public string_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public string_const(string _Value)
		{
			this._Value=_Value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public string_const(string _Value,SourceContext sc)
		{
			this._Value=_Value;
			source_context = sc;
		}

		protected string _Value;

		///<summary>
		///Значенеи строковой константы
		///</summary>
		public string Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Список выражений
	///</summary>
	[Serializable]
	public class expression_list : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public expression_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public expression_list(List<expression> _expressions)
		{
			this._expressions=_expressions;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public expression_list(List<expression> _expressions,SourceContext sc)
		{
			this._expressions=_expressions;
			source_context = sc;
		}

		protected List<expression> _expressions=new List<expression>();

		///<summary>
		///Список выражений
		///</summary>
		public List<expression> expressions
		{
			get
			{
				return _expressions;
			}
			set
			{
				_expressions=value;
			}
		}


		public expression_list(expression _expression, SourceContext sc)
		{
		    Add(_expression,sc);
		}
		public expression_list Add(expression _expression)
		{
		    expressions.Add(_expression);
		    return this;
		}
		public expression_list Add(expression _expression, SourceContext sc)
		{
		    expressions.Add(_expression);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class dereference : addressed_value_funcname
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public dereference()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public dereference(addressed_value _dereferencing_value)
		{
			this._dereferencing_value=_dereferencing_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public dereference(addressed_value _dereferencing_value,SourceContext sc)
		{
			this._dereferencing_value=_dereferencing_value;
			source_context = sc;
		}

		protected addressed_value _dereferencing_value;

		///<summary>
		///
		///</summary>
		public addressed_value dereferencing_value
		{
			get
			{
				return _dereferencing_value;
			}
			set
			{
				_dereferencing_value=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Индекс или индексы
	///</summary>
	[Serializable]
	public class indexer : dereference
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public indexer()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public indexer(expression_list _indexes)
		{
			this._indexes=_indexes;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public indexer(expression_list _indexes,SourceContext sc)
		{
			this._indexes=_indexes;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public indexer(addressed_value _dereferencing_value,expression_list _indexes)
		{
			this._dereferencing_value=_dereferencing_value;
			this._indexes=_indexes;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public indexer(addressed_value _dereferencing_value,expression_list _indexes,SourceContext sc)
		{
			this._dereferencing_value=_dereferencing_value;
			this._indexes=_indexes;
			source_context = sc;
		}

		protected expression_list _indexes;

		///<summary>
		///Список индексов
		///</summary>
		public expression_list indexes
		{
			get
			{
				return _indexes;
			}
			set
			{
				_indexes=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Цикл for
	///</summary>
	[Serializable]
	public class for_node : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public for_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public for_node(ident _loop_variable,expression _initial_value,expression _finish_value,statement _statements,for_cycle_type _cycle_type,expression _increment_value,type_definition _type_name,bool _create_loop_variable)
		{
			this._loop_variable=_loop_variable;
			this._initial_value=_initial_value;
			this._finish_value=_finish_value;
			this._statements=_statements;
			this._cycle_type=_cycle_type;
			this._increment_value=_increment_value;
			this._type_name=_type_name;
			this._create_loop_variable=_create_loop_variable;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public for_node(ident _loop_variable,expression _initial_value,expression _finish_value,statement _statements,for_cycle_type _cycle_type,expression _increment_value,type_definition _type_name,bool _create_loop_variable,SourceContext sc)
		{
			this._loop_variable=_loop_variable;
			this._initial_value=_initial_value;
			this._finish_value=_finish_value;
			this._statements=_statements;
			this._cycle_type=_cycle_type;
			this._increment_value=_increment_value;
			this._type_name=_type_name;
			this._create_loop_variable=_create_loop_variable;
			source_context = sc;
		}

		protected ident _loop_variable;
		protected expression _initial_value;
		protected expression _finish_value;
		protected statement _statements;
		protected for_cycle_type _cycle_type;
		protected expression _increment_value;
		protected type_definition _type_name;
		protected bool _create_loop_variable;

		///<summary>
		///Переменная цикла for
		///</summary>
		public ident loop_variable
		{
			get
			{
				return _loop_variable;
			}
			set
			{
				_loop_variable=value;
			}
		}

		///<summary>
		///Начальное значение переменной цикла
		///</summary>
		public expression initial_value
		{
			get
			{
				return _initial_value;
			}
			set
			{
				_initial_value=value;
			}
		}

		///<summary>
		///Конечное значение переменной цикла
		///</summary>
		public expression finish_value
		{
			get
			{
				return _finish_value;
			}
			set
			{
				_finish_value=value;
			}
		}

		///<summary>
		///Тело цикла
		///</summary>
		public statement statements
		{
			get
			{
				return _statements;
			}
			set
			{
				_statements=value;
			}
		}

		///<summary>
		///
		///</summary>
		public for_cycle_type cycle_type
		{
			get
			{
				return _cycle_type;
			}
			set
			{
				_cycle_type=value;
			}
		}

		///<summary>
		///Шаг переменной цикла
		///</summary>
		public expression increment_value
		{
			get
			{
				return _increment_value;
			}
			set
			{
				_increment_value=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public bool create_loop_variable
		{
			get
			{
				return _create_loop_variable;
			}
			set
			{
				_create_loop_variable=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Цикл с постусловием (repeat)
	///</summary>
	[Serializable]
	public class repeat_node : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public repeat_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public repeat_node(statement _statements,expression _expr)
		{
			this._statements=_statements;
			this._expr=_expr;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public repeat_node(statement _statements,expression _expr,SourceContext sc)
		{
			this._statements=_statements;
			this._expr=_expr;
			source_context = sc;
		}

		protected statement _statements;
		protected expression _expr;

		///<summary>
		///Тело цикла
		///</summary>
		public statement statements
		{
			get
			{
				return _statements;
			}
			set
			{
				_statements=value;
			}
		}

		///<summary>
		///Условие завершения цикла
		///</summary>
		public expression expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Цикл ПОКА
	///</summary>
	[Serializable]
	public class while_node : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public while_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public while_node(expression _expr,statement _statements,WhileCycleType _CycleType)
		{
			this._expr=_expr;
			this._statements=_statements;
			this._CycleType=_CycleType;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public while_node(expression _expr,statement _statements,WhileCycleType _CycleType,SourceContext sc)
		{
			this._expr=_expr;
			this._statements=_statements;
			this._CycleType=_CycleType;
			source_context = sc;
		}

		protected expression _expr;
		protected statement _statements;
		protected WhileCycleType _CycleType;

		///<summary>
		///Условие цикла
		///</summary>
		public expression expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}

		///<summary>
		///Тело цикла
		///</summary>
		public statement statements
		{
			get
			{
				return _statements;
			}
			set
			{
				_statements=value;
			}
		}

		///<summary>
		///Тип цикла ПОКА
		///</summary>
		public WhileCycleType CycleType
		{
			get
			{
				return _CycleType;
			}
			set
			{
				_CycleType=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Условный оператор
	///</summary>
	[Serializable]
	public class if_node : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public if_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public if_node(expression _condition,statement _then_body,statement _else_body)
		{
			this._condition=_condition;
			this._then_body=_then_body;
			this._else_body=_else_body;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public if_node(expression _condition,statement _then_body,statement _else_body,SourceContext sc)
		{
			this._condition=_condition;
			this._then_body=_then_body;
			this._else_body=_else_body;
			source_context = sc;
		}

		protected expression _condition;
		protected statement _then_body;
		protected statement _else_body;

		///<summary>
		///Условие
		///</summary>
		public expression condition
		{
			get
			{
				return _condition;
			}
			set
			{
				_condition=value;
			}
		}

		///<summary>
		///Оператор по ветви then
		///</summary>
		public statement then_body
		{
			get
			{
				return _then_body;
			}
			set
			{
				_then_body=value;
			}
		}

		///<summary>
		///Оператор по ветви else
		///</summary>
		public statement else_body
		{
			get
			{
				return _else_body;
			}
			set
			{
				_else_body=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class ref_type : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public ref_type()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ref_type(type_definition _pointed_to)
		{
			this._pointed_to=_pointed_to;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ref_type(type_definition _pointed_to,SourceContext sc)
		{
			this._pointed_to=_pointed_to;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ref_type(type_definition_attr_list _attr_list,type_definition _pointed_to)
		{
			this._attr_list=_attr_list;
			this._pointed_to=_pointed_to;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ref_type(type_definition_attr_list _attr_list,type_definition _pointed_to,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._pointed_to=_pointed_to;
			source_context = sc;
		}

		protected type_definition _pointed_to;

		///<summary>
		///
		///</summary>
		public type_definition pointed_to
		{
			get
			{
				return _pointed_to;
			}
			set
			{
				_pointed_to=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Диапазон
	///</summary>
	[Serializable]
	public class diapason : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public diapason()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public diapason(expression _left,expression _right)
		{
			this._left=_left;
			this._right=_right;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public diapason(expression _left,expression _right,SourceContext sc)
		{
			this._left=_left;
			this._right=_right;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public diapason(type_definition_attr_list _attr_list,expression _left,expression _right)
		{
			this._attr_list=_attr_list;
			this._left=_left;
			this._right=_right;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public diapason(type_definition_attr_list _attr_list,expression _left,expression _right,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._left=_left;
			this._right=_right;
			source_context = sc;
		}

		protected expression _left;
		protected expression _right;

		///<summary>
		///Нижняя граница диапазона
		///</summary>
		public expression left
		{
			get
			{
				return _left;
			}
			set
			{
				_left=value;
			}
		}

		///<summary>
		///Верхняя граница диапазона
		///</summary>
		public expression right
		{
			get
			{
				return _right;
			}
			set
			{
				_right=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Типы индексов
	///</summary>
	[Serializable]
	public class indexers_types : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public indexers_types()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public indexers_types(List<type_definition> _indexers)
		{
			this._indexers=_indexers;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public indexers_types(List<type_definition> _indexers,SourceContext sc)
		{
			this._indexers=_indexers;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public indexers_types(type_definition_attr_list _attr_list,List<type_definition> _indexers)
		{
			this._attr_list=_attr_list;
			this._indexers=_indexers;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public indexers_types(type_definition_attr_list _attr_list,List<type_definition> _indexers,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._indexers=_indexers;
			source_context = sc;
		}

		protected List<type_definition> _indexers=new List<type_definition>();

		///<summary>
		///Список типов индексов
		///</summary>
		public List<type_definition> indexers
		{
			get
			{
				return _indexers;
			}
			set
			{
				_indexers=value;
			}
		}


		public indexers_types(type_definition _type_definition, SourceContext sc)
		{
		    Add(_type_definition,sc);
		}
		public indexers_types Add(type_definition _type_definition)
		{
		    indexers.Add(_type_definition);
		    return this;
		}
		public indexers_types Add(type_definition _type_definition, SourceContext sc)
		{
		    indexers.Add(_type_definition);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Тип массива
	///</summary>
	[Serializable]
	public class array_type : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public array_type()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_type(indexers_types _indexers,type_definition _elemets_types)
		{
			this._indexers=_indexers;
			this._elemets_types=_elemets_types;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_type(indexers_types _indexers,type_definition _elemets_types,SourceContext sc)
		{
			this._indexers=_indexers;
			this._elemets_types=_elemets_types;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_type(type_definition_attr_list _attr_list,indexers_types _indexers,type_definition _elemets_types)
		{
			this._attr_list=_attr_list;
			this._indexers=_indexers;
			this._elemets_types=_elemets_types;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_type(type_definition_attr_list _attr_list,indexers_types _indexers,type_definition _elemets_types,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._indexers=_indexers;
			this._elemets_types=_elemets_types;
			source_context = sc;
		}

		protected indexers_types _indexers;
		protected type_definition _elemets_types;

		///<summary>
		///Типы индексов массива
		///</summary>
		public indexers_types indexers
		{
			get
			{
				return _indexers;
			}
			set
			{
				_indexers=value;
			}
		}

		///<summary>
		///Тип элементов массива
		///</summary>
		public type_definition elemets_types
		{
			get
			{
				return _elemets_types;
			}
			set
			{
				_elemets_types=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Описание меток
	///</summary>
	[Serializable]
	public class label_definitions : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public label_definitions()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public label_definitions(ident_list _labels)
		{
			this._labels=_labels;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public label_definitions(ident_list _labels,SourceContext sc)
		{
			this._labels=_labels;
			source_context = sc;
		}

		protected ident_list _labels;

		///<summary>
		///Список меток
		///</summary>
		public ident_list labels
		{
			get
			{
				return _labels;
			}
			set
			{
				_labels=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class procedure_attribute : ident
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public procedure_attribute()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_attribute(proc_attribute _attribute_type)
		{
			this._attribute_type=_attribute_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_attribute(proc_attribute _attribute_type,SourceContext sc)
		{
			this._attribute_type=_attribute_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_attribute(string _name,proc_attribute _attribute_type)
		{
			this._name=_name;
			this._attribute_type=_attribute_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_attribute(string _name,proc_attribute _attribute_type,SourceContext sc)
		{
			this._name=_name;
			this._attribute_type=_attribute_type;
			source_context = sc;
		}

		protected proc_attribute _attribute_type;

		///<summary>
		///
		///</summary>
		public proc_attribute attribute_type
		{
			get
			{
				return _attribute_type;
			}
			set
			{
				_attribute_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class typed_parameters : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public typed_parameters()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typed_parameters(ident_list _idents,type_definition _vars_type,parametr_kind _param_kind,expression _inital_value)
		{
			this._idents=_idents;
			this._vars_type=_vars_type;
			this._param_kind=_param_kind;
			this._inital_value=_inital_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typed_parameters(ident_list _idents,type_definition _vars_type,parametr_kind _param_kind,expression _inital_value,SourceContext sc)
		{
			this._idents=_idents;
			this._vars_type=_vars_type;
			this._param_kind=_param_kind;
			this._inital_value=_inital_value;
			source_context = sc;
		}

		protected ident_list _idents;
		protected type_definition _vars_type;
		protected parametr_kind _param_kind;
		protected expression _inital_value;

		///<summary>
		///
		///</summary>
		public ident_list idents
		{
			get
			{
				return _idents;
			}
			set
			{
				_idents=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition vars_type
		{
			get
			{
				return _vars_type;
			}
			set
			{
				_vars_type=value;
			}
		}

		///<summary>
		///
		///</summary>
		public parametr_kind param_kind
		{
			get
			{
				return _param_kind;
			}
			set
			{
				_param_kind=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression inital_value
		{
			get
			{
				return _inital_value;
			}
			set
			{
				_inital_value=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class formal_parameters : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public formal_parameters()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public formal_parameters(List<typed_parameters> _params_list)
		{
			this._params_list=_params_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public formal_parameters(List<typed_parameters> _params_list,SourceContext sc)
		{
			this._params_list=_params_list;
			source_context = sc;
		}

		protected List<typed_parameters> _params_list=new List<typed_parameters>();

		///<summary>
		///
		///</summary>
		public List<typed_parameters> params_list
		{
			get
			{
				return _params_list;
			}
			set
			{
				_params_list=value;
			}
		}


		public formal_parameters(typed_parameters _typed_parameters, SourceContext sc)
		{
		    Add(_typed_parameters,sc);
		}
		public formal_parameters Add(typed_parameters _typed_parameters)
		{
		    params_list.Add(_typed_parameters);
		    return this;
		}
		public formal_parameters Add(typed_parameters _typed_parameters, SourceContext sc)
		{
		    params_list.Add(_typed_parameters);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class procedure_attributes_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public procedure_attributes_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_attributes_list(List<procedure_attribute> _proc_attributes)
		{
			this._proc_attributes=_proc_attributes;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_attributes_list(List<procedure_attribute> _proc_attributes,SourceContext sc)
		{
			this._proc_attributes=_proc_attributes;
			source_context = sc;
		}

		protected List<procedure_attribute> _proc_attributes=new List<procedure_attribute>();

		///<summary>
		///
		///</summary>
		public List<procedure_attribute> proc_attributes
		{
			get
			{
				return _proc_attributes;
			}
			set
			{
				_proc_attributes=value;
			}
		}


		public procedure_attributes_list(procedure_attribute _procedure_attribute, SourceContext sc)
		{
		    Add(_procedure_attribute,sc);
		}
		public procedure_attributes_list Add(procedure_attribute _procedure_attribute)
		{
		    proc_attributes.Add(_procedure_attribute);
		    return this;
		}
		public procedure_attributes_list Add(procedure_attribute _procedure_attribute, SourceContext sc)
		{
		    proc_attributes.Add(_procedure_attribute);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class procedure_header : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public procedure_header()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_header(formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs)
		{
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_header(formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs,SourceContext sc)
		{
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_header(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_header(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
			source_context = sc;
		}

		protected formal_parameters _parameters;
		protected procedure_attributes_list _proc_attributes;
		protected method_name _name;
		protected bool _of_object;
		protected bool _class_keyword;
		protected ident_list _template_args;
		protected where_definition_list _where_defs;

		///<summary>
		///
		///</summary>
		public formal_parameters parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters=value;
			}
		}

		///<summary>
		///
		///</summary>
		public procedure_attributes_list proc_attributes
		{
			get
			{
				return _proc_attributes;
			}
			set
			{
				_proc_attributes=value;
			}
		}

		///<summary>
		///
		///</summary>
		public method_name name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public bool of_object
		{
			get
			{
				return _of_object;
			}
			set
			{
				_of_object=value;
			}
		}

		///<summary>
		///class procedure...
		///</summary>
		public bool class_keyword
		{
			get
			{
				return _class_keyword;
			}
			set
			{
				_class_keyword=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident_list template_args
		{
			get
			{
				return _template_args;
			}
			set
			{
				_template_args=value;
			}
		}

		///<summary>
		///
		///</summary>
		public where_definition_list where_defs
		{
			get
			{
				return _where_defs;
			}
			set
			{
				_where_defs=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class function_header : procedure_header
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public function_header()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public function_header(type_definition _return_type)
		{
			this._return_type=_return_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public function_header(type_definition _return_type,SourceContext sc)
		{
			this._return_type=_return_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public function_header(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs,type_definition _return_type)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
			this._return_type=_return_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public function_header(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs,type_definition _return_type,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
			this._return_type=_return_type;
			source_context = sc;
		}

		protected type_definition _return_type;

		///<summary>
		///
		///</summary>
		public type_definition return_type
		{
			get
			{
				return _return_type;
			}
			set
			{
				_return_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class procedure_definition : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public procedure_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_definition(procedure_header _proc_header,proc_block _proc_body)
		{
			this._proc_header=_proc_header;
			this._proc_body=_proc_body;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_definition(procedure_header _proc_header,proc_block _proc_body,SourceContext sc)
		{
			this._proc_header=_proc_header;
			this._proc_body=_proc_body;
			source_context = sc;
		}

		protected procedure_header _proc_header;
		protected proc_block _proc_body;

		///<summary>
		///
		///</summary>
		public procedure_header proc_header
		{
			get
			{
				return _proc_header;
			}
			set
			{
				_proc_header=value;
			}
		}

		///<summary>
		///
		///</summary>
		public proc_block proc_body
		{
			get
			{
				return _proc_body;
			}
			set
			{
				_proc_body=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class type_declaration : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public type_declaration()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_declaration(ident _type_name,type_definition _type_def)
		{
			this._type_name=_type_name;
			this._type_def=_type_def;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_declaration(ident _type_name,type_definition _type_def,SourceContext sc)
		{
			this._type_name=_type_name;
			this._type_def=_type_def;
			source_context = sc;
		}

		protected ident _type_name;
		protected type_definition _type_def;

		///<summary>
		///
		///</summary>
		public ident type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition type_def
		{
			get
			{
				return _type_def;
			}
			set
			{
				_type_def=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Список определений типов
	///</summary>
	[Serializable]
	public class type_declarations : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public type_declarations()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_declarations(List<type_declaration> _types_decl)
		{
			this._types_decl=_types_decl;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_declarations(List<type_declaration> _types_decl,SourceContext sc)
		{
			this._types_decl=_types_decl;
			source_context = sc;
		}

		protected List<type_declaration> _types_decl=new List<type_declaration>();

		///<summary>
		///
		///</summary>
		public List<type_declaration> types_decl
		{
			get
			{
				return _types_decl;
			}
			set
			{
				_types_decl=value;
			}
		}


		public type_declarations(type_declaration _type_declaration, SourceContext sc)
		{
		    Add(_type_declaration,sc);
		}
		public type_declarations Add(type_declaration _type_declaration)
		{
		    types_decl.Add(_type_declaration);
		    return this;
		}
		public type_declarations Add(type_declaration _type_declaration, SourceContext sc)
		{
		    types_decl.Add(_type_declaration);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class simple_const_definition : const_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public simple_const_definition()
		{

		}


		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public simple_const_definition(ident _const_name,expression _const_value)
		{
			this._const_name=_const_name;
			this._const_value=_const_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public simple_const_definition(ident _const_name,expression _const_value,SourceContext sc)
		{
			this._const_name=_const_name;
			this._const_value=_const_value;
			source_context = sc;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class typed_const_definition : const_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public typed_const_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typed_const_definition(type_definition _const_type)
		{
			this._const_type=_const_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typed_const_definition(type_definition _const_type,SourceContext sc)
		{
			this._const_type=_const_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typed_const_definition(ident _const_name,expression _const_value,type_definition _const_type)
		{
			this._const_name=_const_name;
			this._const_value=_const_value;
			this._const_type=_const_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typed_const_definition(ident _const_name,expression _const_value,type_definition _const_type,SourceContext sc)
		{
			this._const_name=_const_name;
			this._const_value=_const_value;
			this._const_type=_const_type;
			source_context = sc;
		}

		protected type_definition _const_type;

		///<summary>
		///
		///</summary>
		public type_definition const_type
		{
			get
			{
				return _const_type;
			}
			set
			{
				_const_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class const_definition : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public const_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public const_definition(ident _const_name,expression _const_value)
		{
			this._const_name=_const_name;
			this._const_value=_const_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public const_definition(ident _const_name,expression _const_value,SourceContext sc)
		{
			this._const_name=_const_name;
			this._const_value=_const_value;
			source_context = sc;
		}

		protected ident _const_name;
		protected expression _const_value;

		///<summary>
		///
		///</summary>
		public ident const_name
		{
			get
			{
				return _const_name;
			}
			set
			{
				_const_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression const_value
		{
			get
			{
				return _const_value;
			}
			set
			{
				_const_value=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class consts_definitions_list : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public consts_definitions_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public consts_definitions_list(List<const_definition> _const_defs)
		{
			this._const_defs=_const_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public consts_definitions_list(List<const_definition> _const_defs,SourceContext sc)
		{
			this._const_defs=_const_defs;
			source_context = sc;
		}

		protected List<const_definition> _const_defs=new List<const_definition>();

		///<summary>
		///
		///</summary>
		public List<const_definition> const_defs
		{
			get
			{
				return _const_defs;
			}
			set
			{
				_const_defs=value;
			}
		}


		public consts_definitions_list(const_definition _const_definition, SourceContext sc)
		{
		    Add(_const_definition,sc);
		}
		public consts_definitions_list Add(const_definition _const_definition)
		{
		    const_defs.Add(_const_definition);
		    return this;
		}
		public consts_definitions_list Add(const_definition _const_definition, SourceContext sc)
		{
		    const_defs.Add(_const_definition);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class unit_name : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public unit_name()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public unit_name(ident _idunit_name,UnitHeaderKeyword _HeaderKeyword)
		{
			this._idunit_name=_idunit_name;
			this._HeaderKeyword=_HeaderKeyword;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public unit_name(ident _idunit_name,UnitHeaderKeyword _HeaderKeyword,SourceContext sc)
		{
			this._idunit_name=_idunit_name;
			this._HeaderKeyword=_HeaderKeyword;
			source_context = sc;
		}

		protected ident _idunit_name;
		protected UnitHeaderKeyword _HeaderKeyword;

		///<summary>
		///
		///</summary>
		public ident idunit_name
		{
			get
			{
				return _idunit_name;
			}
			set
			{
				_idunit_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public UnitHeaderKeyword HeaderKeyword
		{
			get
			{
				return _HeaderKeyword;
			}
			set
			{
				_HeaderKeyword=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class unit_or_namespace : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public unit_or_namespace()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public unit_or_namespace(ident_list _name)
		{
			this._name=_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public unit_or_namespace(ident_list _name,SourceContext sc)
		{
			this._name=_name;
			source_context = sc;
		}

		protected ident_list _name;

		///<summary>
		///
		///</summary>
		public ident_list name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}


		public unit_or_namespace(string name)
		{
		  this.name = new ident_list(name);
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class uses_unit_in : unit_or_namespace
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public uses_unit_in()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public uses_unit_in(string_const _in_file)
		{
			this._in_file=_in_file;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public uses_unit_in(string_const _in_file,SourceContext sc)
		{
			this._in_file=_in_file;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public uses_unit_in(ident_list _name,string_const _in_file)
		{
			this._name=_name;
			this._in_file=_in_file;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public uses_unit_in(ident_list _name,string_const _in_file,SourceContext sc)
		{
			this._name=_name;
			this._in_file=_in_file;
			source_context = sc;
		}

		protected string_const _in_file;

		///<summary>
		///
		///</summary>
		public string_const in_file
		{
			get
			{
				return _in_file;
			}
			set
			{
				_in_file=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class uses_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public uses_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public uses_list(List<unit_or_namespace> _units)
		{
			this._units=_units;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public uses_list(List<unit_or_namespace> _units,SourceContext sc)
		{
			this._units=_units;
			source_context = sc;
		}

		protected List<unit_or_namespace> _units=new List<unit_or_namespace>();

		///<summary>
		///
		///</summary>
		public List<unit_or_namespace> units
		{
			get
			{
				return _units;
			}
			set
			{
				_units=value;
			}
		}


		public uses_list(string name)
		{
		  units.Add(new unit_or_namespace(name));
		}
		public uses_list(unit_or_namespace un)
		{
		  units.Add(un);
		}
		public uses_list(unit_or_namespace _unit_or_namespace, SourceContext sc)
		{
		    Add(_unit_or_namespace,sc);
		}
		public uses_list Add(unit_or_namespace _unit_or_namespace)
		{
		    units.Add(_unit_or_namespace);
		    return this;
		}
		public uses_list Add(unit_or_namespace _unit_or_namespace, SourceContext sc)
		{
		    units.Add(_unit_or_namespace);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class program_body : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public program_body()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_body(uses_list _used_units,declarations _program_definitions,statement_list _program_code,using_list _using_list)
		{
			this._used_units=_used_units;
			this._program_definitions=_program_definitions;
			this._program_code=_program_code;
			this._using_list=_using_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_body(uses_list _used_units,declarations _program_definitions,statement_list _program_code,using_list _using_list,SourceContext sc)
		{
			this._used_units=_used_units;
			this._program_definitions=_program_definitions;
			this._program_code=_program_code;
			this._using_list=_using_list;
			source_context = sc;
		}

		protected uses_list _used_units;
		protected declarations _program_definitions;
		protected statement_list _program_code;
		protected using_list _using_list;

		///<summary>
		///
		///</summary>
		public uses_list used_units
		{
			get
			{
				return _used_units;
			}
			set
			{
				_used_units=value;
			}
		}

		///<summary>
		///
		///</summary>
		public declarations program_definitions
		{
			get
			{
				return _program_definitions;
			}
			set
			{
				_program_definitions=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement_list program_code
		{
			get
			{
				return _program_code;
			}
			set
			{
				_program_code=value;
			}
		}

		///<summary>
		///
		///</summary>
		public using_list using_list
		{
			get
			{
				return _using_list;
			}
			set
			{
				_using_list=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class compilation_unit : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public compilation_unit()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compilation_unit(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compilation_unit(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,SourceContext sc)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			source_context = sc;
		}

		protected string _file_name;
		protected List<compiler_directive> _compiler_directives=new List<compiler_directive>();
		protected LanguageId _Language;

		///<summary>
		///
		///</summary>
		public string file_name
		{
			get
			{
				return _file_name;
			}
			set
			{
				_file_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public List<compiler_directive> compiler_directives
		{
			get
			{
				return _compiler_directives;
			}
			set
			{
				_compiler_directives=value;
			}
		}

		///<summary>
		///
		///</summary>
		public LanguageId Language
		{
			get
			{
				return _Language;
			}
			set
			{
				_Language=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class unit_module : compilation_unit
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public unit_module()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public unit_module(unit_name _unit_name,interface_node _interface_part,implementation_node _implementation_part,statement_list _initialization_part,statement_list _finalization_part)
		{
			this._unit_name=_unit_name;
			this._interface_part=_interface_part;
			this._implementation_part=_implementation_part;
			this._initialization_part=_initialization_part;
			this._finalization_part=_finalization_part;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public unit_module(unit_name _unit_name,interface_node _interface_part,implementation_node _implementation_part,statement_list _initialization_part,statement_list _finalization_part,SourceContext sc)
		{
			this._unit_name=_unit_name;
			this._interface_part=_interface_part;
			this._implementation_part=_implementation_part;
			this._initialization_part=_initialization_part;
			this._finalization_part=_finalization_part;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public unit_module(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,unit_name _unit_name,interface_node _interface_part,implementation_node _implementation_part,statement_list _initialization_part,statement_list _finalization_part)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			this._unit_name=_unit_name;
			this._interface_part=_interface_part;
			this._implementation_part=_implementation_part;
			this._initialization_part=_initialization_part;
			this._finalization_part=_finalization_part;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public unit_module(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,unit_name _unit_name,interface_node _interface_part,implementation_node _implementation_part,statement_list _initialization_part,statement_list _finalization_part,SourceContext sc)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			this._unit_name=_unit_name;
			this._interface_part=_interface_part;
			this._implementation_part=_implementation_part;
			this._initialization_part=_initialization_part;
			this._finalization_part=_finalization_part;
			source_context = sc;
		}

		protected unit_name _unit_name;
		protected interface_node _interface_part;
		protected implementation_node _implementation_part;
		protected statement_list _initialization_part;
		protected statement_list _finalization_part;

		///<summary>
		///
		///</summary>
		public unit_name unit_name
		{
			get
			{
				return _unit_name;
			}
			set
			{
				_unit_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public interface_node interface_part
		{
			get
			{
				return _interface_part;
			}
			set
			{
				_interface_part=value;
			}
		}

		///<summary>
		///
		///</summary>
		public implementation_node implementation_part
		{
			get
			{
				return _implementation_part;
			}
			set
			{
				_implementation_part=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement_list initialization_part
		{
			get
			{
				return _initialization_part;
			}
			set
			{
				_initialization_part=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement_list finalization_part
		{
			get
			{
				return _finalization_part;
			}
			set
			{
				_finalization_part=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class program_module : compilation_unit
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public program_module()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_module(program_name _program_name,uses_list _used_units,block _program_block,using_list _using_namespaces)
		{
			this._program_name=_program_name;
			this._used_units=_used_units;
			this._program_block=_program_block;
			this._using_namespaces=_using_namespaces;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_module(program_name _program_name,uses_list _used_units,block _program_block,using_list _using_namespaces,SourceContext sc)
		{
			this._program_name=_program_name;
			this._used_units=_used_units;
			this._program_block=_program_block;
			this._using_namespaces=_using_namespaces;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_module(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,program_name _program_name,uses_list _used_units,block _program_block,using_list _using_namespaces)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			this._program_name=_program_name;
			this._used_units=_used_units;
			this._program_block=_program_block;
			this._using_namespaces=_using_namespaces;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public program_module(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,program_name _program_name,uses_list _used_units,block _program_block,using_list _using_namespaces,SourceContext sc)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			this._program_name=_program_name;
			this._used_units=_used_units;
			this._program_block=_program_block;
			this._using_namespaces=_using_namespaces;
			source_context = sc;
		}

		protected program_name _program_name;
		protected uses_list _used_units;
		protected block _program_block;
		protected using_list _using_namespaces;

		///<summary>
		///
		///</summary>
		public program_name program_name
		{
			get
			{
				return _program_name;
			}
			set
			{
				_program_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public uses_list used_units
		{
			get
			{
				return _used_units;
			}
			set
			{
				_used_units=value;
			}
		}

		///<summary>
		///
		///</summary>
		public block program_block
		{
			get
			{
				return _program_block;
			}
			set
			{
				_program_block=value;
			}
		}

		///<summary>
		///
		///</summary>
		public using_list using_namespaces
		{
			get
			{
				return _using_namespaces;
			}
			set
			{
				_using_namespaces=value;
			}
		}


		public static program_module create(ident id,uses_list _used_units,block _program_block,using_list _using_namespaces)
		{
		  var r =  new program_module(new program_name(id),_used_units,_program_block,_using_namespaces);
		  r.Language = LanguageId.CommonLanguage;
		  return r;
		}
		
		public static program_module create(ident id,uses_list _used_units,block _program_block,using_list _using_namespaces,SourceContext sc)
		{
		  var r = new program_module(new program_name(id),_used_units,_program_block,_using_namespaces,sc);
		  r.Language = LanguageId.CommonLanguage;
		  return r;
		}
		public static program_module create(ident id,uses_list _used_units,block _program_block)
		{
		  var r = new program_module(new program_name(id),_used_units,_program_block,null);
		  r.Language = LanguageId.CommonLanguage;
		  return r;
		}
		
		public static program_module create(ident id,uses_list _used_units,block _program_block,SourceContext sc)
		{
		  var r = new program_module(new program_name(id),_used_units,_program_block,null,sc);
		  r.Language = LanguageId.CommonLanguage;
		  return r;
		}
		

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class hex_constant : int64_const
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public hex_constant()
		{

		}


		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public hex_constant(Int64 _val)
		{
			this._val=_val;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public hex_constant(Int64 _val,SourceContext sc)
		{
			this._val=_val;
			source_context = sc;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class get_address : addressed_value_funcname
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public get_address()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public get_address(addressed_value _address_of)
		{
			this._address_of=_address_of;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public get_address(addressed_value _address_of,SourceContext sc)
		{
			this._address_of=_address_of;
			source_context = sc;
		}

		protected addressed_value _address_of;

		///<summary>
		///
		///</summary>
		public addressed_value address_of
		{
			get
			{
				return _address_of;
			}
			set
			{
				_address_of=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class case_variant : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public case_variant()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public case_variant(expression_list _conditions,statement _exec_if_true)
		{
			this._conditions=_conditions;
			this._exec_if_true=_exec_if_true;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public case_variant(expression_list _conditions,statement _exec_if_true,SourceContext sc)
		{
			this._conditions=_conditions;
			this._exec_if_true=_exec_if_true;
			source_context = sc;
		}

		protected expression_list _conditions;
		protected statement _exec_if_true;

		///<summary>
		///
		///</summary>
		public expression_list conditions
		{
			get
			{
				return _conditions;
			}
			set
			{
				_conditions=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement exec_if_true
		{
			get
			{
				return _exec_if_true;
			}
			set
			{
				_exec_if_true=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class case_node : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public case_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public case_node(expression _param,case_variants _conditions,statement _else_statement)
		{
			this._param=_param;
			this._conditions=_conditions;
			this._else_statement=_else_statement;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public case_node(expression _param,case_variants _conditions,statement _else_statement,SourceContext sc)
		{
			this._param=_param;
			this._conditions=_conditions;
			this._else_statement=_else_statement;
			source_context = sc;
		}

		protected expression _param;
		protected case_variants _conditions;
		protected statement _else_statement;

		///<summary>
		///
		///</summary>
		public expression param
		{
			get
			{
				return _param;
			}
			set
			{
				_param=value;
			}
		}

		///<summary>
		///
		///</summary>
		public case_variants conditions
		{
			get
			{
				return _conditions;
			}
			set
			{
				_conditions=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement else_statement
		{
			get
			{
				return _else_statement;
			}
			set
			{
				_else_statement=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class method_name : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public method_name()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public method_name(ident _class_name,ident _meth_name,ident _explicit_interface_name)
		{
			this._class_name=_class_name;
			this._meth_name=_meth_name;
			this._explicit_interface_name=_explicit_interface_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public method_name(ident _class_name,ident _meth_name,ident _explicit_interface_name,SourceContext sc)
		{
			this._class_name=_class_name;
			this._meth_name=_meth_name;
			this._explicit_interface_name=_explicit_interface_name;
			source_context = sc;
		}

		protected ident _class_name;
		protected ident _meth_name;
		protected ident _explicit_interface_name;

		///<summary>
		///
		///</summary>
		public ident class_name
		{
			get
			{
				return _class_name;
			}
			set
			{
				_class_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident meth_name
		{
			get
			{
				return _meth_name;
			}
			set
			{
				_meth_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident explicit_interface_name
		{
			get
			{
				return _explicit_interface_name;
			}
			set
			{
				_explicit_interface_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class dot_node : addressed_value_funcname
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public dot_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public dot_node(addressed_value _left,addressed_value _right)
		{
			this._left=_left;
			this._right=_right;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public dot_node(addressed_value _left,addressed_value _right,SourceContext sc)
		{
			this._left=_left;
			this._right=_right;
			source_context = sc;
		}

		protected addressed_value _left;
		protected addressed_value _right;

		///<summary>
		///
		///</summary>
		public addressed_value left
		{
			get
			{
				return _left;
			}
			set
			{
				_left=value;
			}
		}

		///<summary>
		///
		///</summary>
		public addressed_value right
		{
			get
			{
				return _right;
			}
			set
			{
				_right=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class empty_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public empty_statement()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class goto_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public goto_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public goto_statement(ident _label)
		{
			this._label=_label;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public goto_statement(ident _label,SourceContext sc)
		{
			this._label=_label;
			source_context = sc;
		}

		protected ident _label;

		///<summary>
		///
		///</summary>
		public ident label
		{
			get
			{
				return _label;
			}
			set
			{
				_label=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class labeled_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public labeled_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public labeled_statement(ident _label_name,statement _to_statement)
		{
			this._label_name=_label_name;
			this._to_statement=_to_statement;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public labeled_statement(ident _label_name,statement _to_statement,SourceContext sc)
		{
			this._label_name=_label_name;
			this._to_statement=_to_statement;
			source_context = sc;
		}

		protected ident _label_name;
		protected statement _to_statement;

		///<summary>
		///
		///</summary>
		public ident label_name
		{
			get
			{
				return _label_name;
			}
			set
			{
				_label_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement to_statement
		{
			get
			{
				return _to_statement;
			}
			set
			{
				_to_statement=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Представление оператора with в синтаксическом дереве.
	///</summary>
	[Serializable]
	public class with_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public with_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public with_statement(statement _what_do,expression_list _do_with)
		{
			this._what_do=_what_do;
			this._do_with=_do_with;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public with_statement(statement _what_do,expression_list _do_with,SourceContext sc)
		{
			this._what_do=_what_do;
			this._do_with=_do_with;
			source_context = sc;
		}

		protected statement _what_do;
		protected expression_list _do_with;

		///<summary>
		///Что делать.
		///</summary>
		public statement what_do
		{
			get
			{
				return _what_do;
			}
			set
			{
				_what_do=value;
			}
		}

		///<summary>
		///Список объектов, с которыми производить действия.
		///</summary>
		public expression_list do_with
		{
			get
			{
				return _do_with;
			}
			set
			{
				_do_with=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class method_call : dereference
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public method_call()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public method_call(expression_list _parameters)
		{
			this._parameters=_parameters;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public method_call(expression_list _parameters,SourceContext sc)
		{
			this._parameters=_parameters;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public method_call(addressed_value _dereferencing_value,expression_list _parameters)
		{
			this._dereferencing_value=_dereferencing_value;
			this._parameters=_parameters;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public method_call(addressed_value _dereferencing_value,expression_list _parameters,SourceContext sc)
		{
			this._dereferencing_value=_dereferencing_value;
			this._parameters=_parameters;
			source_context = sc;
		}

		protected expression_list _parameters;

		///<summary>
		///
		///</summary>
		public expression_list parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Выражение-константа множество
	///</summary>
	[Serializable]
	public class pascal_set_constant : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public pascal_set_constant()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public pascal_set_constant(expression_list _values)
		{
			this._values=_values;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public pascal_set_constant(expression_list _values,SourceContext sc)
		{
			this._values=_values;
			source_context = sc;
		}

		protected expression_list _values;

		///<summary>
		///
		///</summary>
		public expression_list values
		{
			get
			{
				return _values;
			}
			set
			{
				_values=value;
			}
		}


		public void Add(expression value)
		{
		    values.Add(value);
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class array_const : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public array_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_const(expression_list _elements)
		{
			this._elements=_elements;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_const(expression_list _elements,SourceContext sc)
		{
			this._elements=_elements;
			source_context = sc;
		}

		protected expression_list _elements;

		///<summary>
		///
		///</summary>
		public expression_list elements
		{
			get
			{
				return _elements;
			}
			set
			{
				_elements=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class write_accessor_name : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public write_accessor_name()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public write_accessor_name(ident _accessor_name)
		{
			this._accessor_name=_accessor_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public write_accessor_name(ident _accessor_name,SourceContext sc)
		{
			this._accessor_name=_accessor_name;
			source_context = sc;
		}

		protected ident _accessor_name;

		///<summary>
		///
		///</summary>
		public ident accessor_name
		{
			get
			{
				return _accessor_name;
			}
			set
			{
				_accessor_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class read_accessor_name : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public read_accessor_name()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public read_accessor_name(ident _accessor_name)
		{
			this._accessor_name=_accessor_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public read_accessor_name(ident _accessor_name,SourceContext sc)
		{
			this._accessor_name=_accessor_name;
			source_context = sc;
		}

		protected ident _accessor_name;

		///<summary>
		///
		///</summary>
		public ident accessor_name
		{
			get
			{
				return _accessor_name;
			}
			set
			{
				_accessor_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class property_accessors : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public property_accessors()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public property_accessors(read_accessor_name _read_accessor,write_accessor_name _write_accessor)
		{
			this._read_accessor=_read_accessor;
			this._write_accessor=_write_accessor;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public property_accessors(read_accessor_name _read_accessor,write_accessor_name _write_accessor,SourceContext sc)
		{
			this._read_accessor=_read_accessor;
			this._write_accessor=_write_accessor;
			source_context = sc;
		}

		protected read_accessor_name _read_accessor;
		protected write_accessor_name _write_accessor;

		///<summary>
		///
		///</summary>
		public read_accessor_name read_accessor
		{
			get
			{
				return _read_accessor;
			}
			set
			{
				_read_accessor=value;
			}
		}

		///<summary>
		///
		///</summary>
		public write_accessor_name write_accessor
		{
			get
			{
				return _write_accessor;
			}
			set
			{
				_write_accessor=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///property property_name[parameter_list]:property_type index index_expression accessors;array_default;
	///</summary>
	[Serializable]
	public class simple_property : declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public simple_property()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public simple_property(ident _property_name,type_definition _property_type,expression _index_expression,property_accessors _accessors,property_array_default _array_default,property_parameter_list _parameter_list,definition_attribute _attr)
		{
			this._property_name=_property_name;
			this._property_type=_property_type;
			this._index_expression=_index_expression;
			this._accessors=_accessors;
			this._array_default=_array_default;
			this._parameter_list=_parameter_list;
			this._attr=_attr;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public simple_property(ident _property_name,type_definition _property_type,expression _index_expression,property_accessors _accessors,property_array_default _array_default,property_parameter_list _parameter_list,definition_attribute _attr,SourceContext sc)
		{
			this._property_name=_property_name;
			this._property_type=_property_type;
			this._index_expression=_index_expression;
			this._accessors=_accessors;
			this._array_default=_array_default;
			this._parameter_list=_parameter_list;
			this._attr=_attr;
			source_context = sc;
		}

		protected ident _property_name;
		protected type_definition _property_type;
		protected expression _index_expression;
		protected property_accessors _accessors;
		protected property_array_default _array_default;
		protected property_parameter_list _parameter_list;
		protected definition_attribute _attr;

		///<summary>
		///
		///</summary>
		public ident property_name
		{
			get
			{
				return _property_name;
			}
			set
			{
				_property_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition property_type
		{
			get
			{
				return _property_type;
			}
			set
			{
				_property_type=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression index_expression
		{
			get
			{
				return _index_expression;
			}
			set
			{
				_index_expression=value;
			}
		}

		///<summary>
		///
		///</summary>
		public property_accessors accessors
		{
			get
			{
				return _accessors;
			}
			set
			{
				_accessors=value;
			}
		}

		///<summary>
		///
		///</summary>
		public property_array_default array_default
		{
			get
			{
				return _array_default;
			}
			set
			{
				_array_default=value;
			}
		}

		///<summary>
		///
		///</summary>
		public property_parameter_list parameter_list
		{
			get
			{
				return _parameter_list;
			}
			set
			{
				_parameter_list=value;
			}
		}

		///<summary>
		///
		///</summary>
		public definition_attribute attr
		{
			get
			{
				return _attr;
			}
			set
			{
				_attr=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class index_property : simple_property
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public index_property()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public index_property(formal_parameters _property_parametres,default_indexer_property_node _is_default)
		{
			this._property_parametres=_property_parametres;
			this._is_default=_is_default;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public index_property(formal_parameters _property_parametres,default_indexer_property_node _is_default,SourceContext sc)
		{
			this._property_parametres=_property_parametres;
			this._is_default=_is_default;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public index_property(ident _property_name,type_definition _property_type,expression _index_expression,property_accessors _accessors,property_array_default _array_default,property_parameter_list _parameter_list,definition_attribute _attr,formal_parameters _property_parametres,default_indexer_property_node _is_default)
		{
			this._property_name=_property_name;
			this._property_type=_property_type;
			this._index_expression=_index_expression;
			this._accessors=_accessors;
			this._array_default=_array_default;
			this._parameter_list=_parameter_list;
			this._attr=_attr;
			this._property_parametres=_property_parametres;
			this._is_default=_is_default;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public index_property(ident _property_name,type_definition _property_type,expression _index_expression,property_accessors _accessors,property_array_default _array_default,property_parameter_list _parameter_list,definition_attribute _attr,formal_parameters _property_parametres,default_indexer_property_node _is_default,SourceContext sc)
		{
			this._property_name=_property_name;
			this._property_type=_property_type;
			this._index_expression=_index_expression;
			this._accessors=_accessors;
			this._array_default=_array_default;
			this._parameter_list=_parameter_list;
			this._attr=_attr;
			this._property_parametres=_property_parametres;
			this._is_default=_is_default;
			source_context = sc;
		}

		protected formal_parameters _property_parametres;
		protected default_indexer_property_node _is_default;

		///<summary>
		///
		///</summary>
		public formal_parameters property_parametres
		{
			get
			{
				return _property_parametres;
			}
			set
			{
				_property_parametres=value;
			}
		}

		///<summary>
		///
		///</summary>
		public default_indexer_property_node is_default
		{
			get
			{
				return _is_default;
			}
			set
			{
				_is_default=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class class_members : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public class_members()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_members(List<declaration> _members,access_modifer_node _access_mod)
		{
			this._members=_members;
			this._access_mod=_access_mod;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_members(List<declaration> _members,access_modifer_node _access_mod,SourceContext sc)
		{
			this._members=_members;
			this._access_mod=_access_mod;
			source_context = sc;
		}

		protected List<declaration> _members=new List<declaration>();
		protected access_modifer_node _access_mod;

		///<summary>
		///
		///</summary>
		public List<declaration> members
		{
			get
			{
				return _members;
			}
			set
			{
				_members=value;
			}
		}

		///<summary>
		///
		///</summary>
		public access_modifer_node access_mod
		{
			get
			{
				return _access_mod;
			}
			set
			{
				_access_mod=value;
			}
		}


		public class_members(declaration _declaration, SourceContext sc)
		{
		    Add(_declaration,sc);
		}
		public class_members Add(declaration _declaration)
		{
		    members.Add(_declaration);
		    return this;
		}
		public class_members Add(declaration _declaration, SourceContext sc)
		{
		    members.Add(_declaration);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class access_modifer_node : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public access_modifer_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public access_modifer_node(access_modifer _access_level)
		{
			this._access_level=_access_level;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public access_modifer_node(access_modifer _access_level,SourceContext sc)
		{
			this._access_level=_access_level;
			source_context = sc;
		}

		protected access_modifer _access_level;

		///<summary>
		///
		///</summary>
		public access_modifer access_level
		{
			get
			{
				return _access_level;
			}
			set
			{
				_access_level=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class class_body : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public class_body()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_body(List<class_members> _class_def_blocks)
		{
			this._class_def_blocks=_class_def_blocks;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_body(List<class_members> _class_def_blocks,SourceContext sc)
		{
			this._class_def_blocks=_class_def_blocks;
			source_context = sc;
		}

		protected List<class_members> _class_def_blocks=new List<class_members>();

		///<summary>
		///
		///</summary>
		public List<class_members> class_def_blocks
		{
			get
			{
				return _class_def_blocks;
			}
			set
			{
				_class_def_blocks=value;
			}
		}


		public class_body(class_members _class_members, SourceContext sc)
		{
		    Add(_class_members,sc);
		}
		public class_body Add(class_members _class_members)
		{
		    class_def_blocks.Add(_class_members);
		    return this;
		}
		public class_body Add(class_members _class_members, SourceContext sc)
		{
		    class_def_blocks.Add(_class_members);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class class_definition : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public class_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_definition(named_type_reference_list _class_parents,class_body _body,class_keyword _keyword,ident_list _template_args,where_definition_list _where_section,class_attribute _attribute)
		{
			this._class_parents=_class_parents;
			this._body=_body;
			this._keyword=_keyword;
			this._template_args=_template_args;
			this._where_section=_where_section;
			this._attribute=_attribute;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_definition(named_type_reference_list _class_parents,class_body _body,class_keyword _keyword,ident_list _template_args,where_definition_list _where_section,class_attribute _attribute,SourceContext sc)
		{
			this._class_parents=_class_parents;
			this._body=_body;
			this._keyword=_keyword;
			this._template_args=_template_args;
			this._where_section=_where_section;
			this._attribute=_attribute;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_definition(type_definition_attr_list _attr_list,named_type_reference_list _class_parents,class_body _body,class_keyword _keyword,ident_list _template_args,where_definition_list _where_section,class_attribute _attribute)
		{
			this._attr_list=_attr_list;
			this._class_parents=_class_parents;
			this._body=_body;
			this._keyword=_keyword;
			this._template_args=_template_args;
			this._where_section=_where_section;
			this._attribute=_attribute;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_definition(type_definition_attr_list _attr_list,named_type_reference_list _class_parents,class_body _body,class_keyword _keyword,ident_list _template_args,where_definition_list _where_section,class_attribute _attribute,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._class_parents=_class_parents;
			this._body=_body;
			this._keyword=_keyword;
			this._template_args=_template_args;
			this._where_section=_where_section;
			this._attribute=_attribute;
			source_context = sc;
		}

		protected named_type_reference_list _class_parents;
		protected class_body _body;
		protected class_keyword _keyword;
		protected ident_list _template_args;
		protected where_definition_list _where_section;
		protected class_attribute _attribute;

		///<summary>
		///
		///</summary>
		public named_type_reference_list class_parents
		{
			get
			{
				return _class_parents;
			}
			set
			{
				_class_parents=value;
			}
		}

		///<summary>
		///
		///</summary>
		public class_body body
		{
			get
			{
				return _body;
			}
			set
			{
				_body=value;
			}
		}

		///<summary>
		///
		///</summary>
		public class_keyword keyword
		{
			get
			{
				return _keyword;
			}
			set
			{
				_keyword=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident_list template_args
		{
			get
			{
				return _template_args;
			}
			set
			{
				_template_args=value;
			}
		}

		///<summary>
		///
		///</summary>
		public where_definition_list where_section
		{
			get
			{
				return _where_section;
			}
			set
			{
				_where_section=value;
			}
		}

		///<summary>
		///
		///</summary>
		public class_attribute attribute
		{
			get
			{
				return _attribute;
			}
			set
			{
				_attribute=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class default_indexer_property_node : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public default_indexer_property_node()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class known_type_definition : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public known_type_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public known_type_definition(known_type _tp,ident _unit_name)
		{
			this._tp=_tp;
			this._unit_name=_unit_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public known_type_definition(known_type _tp,ident _unit_name,SourceContext sc)
		{
			this._tp=_tp;
			this._unit_name=_unit_name;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public known_type_definition(type_definition_attr_list _attr_list,known_type _tp,ident _unit_name)
		{
			this._attr_list=_attr_list;
			this._tp=_tp;
			this._unit_name=_unit_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public known_type_definition(type_definition_attr_list _attr_list,known_type _tp,ident _unit_name,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._tp=_tp;
			this._unit_name=_unit_name;
			source_context = sc;
		}

		protected known_type _tp;
		protected ident _unit_name;

		///<summary>
		///
		///</summary>
		public known_type tp
		{
			get
			{
				return _tp;
			}
			set
			{
				_tp=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident unit_name
		{
			get
			{
				return _unit_name;
			}
			set
			{
				_unit_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class set_type_definition : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public set_type_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public set_type_definition(type_definition _of_type)
		{
			this._of_type=_of_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public set_type_definition(type_definition _of_type,SourceContext sc)
		{
			this._of_type=_of_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public set_type_definition(type_definition_attr_list _attr_list,type_definition _of_type)
		{
			this._attr_list=_attr_list;
			this._of_type=_of_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public set_type_definition(type_definition_attr_list _attr_list,type_definition _of_type,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._of_type=_of_type;
			source_context = sc;
		}

		protected type_definition _of_type;

		///<summary>
		///
		///</summary>
		public type_definition of_type
		{
			get
			{
				return _of_type;
			}
			set
			{
				_of_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class try_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public try_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_statement(statement_list _statements)
		{
			this._statements=_statements;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_statement(statement_list _statements,SourceContext sc)
		{
			this._statements=_statements;
			source_context = sc;
		}

		protected statement_list _statements;

		///<summary>
		///
		///</summary>
		public statement_list statements
		{
			get
			{
				return _statements;
			}
			set
			{
				_statements=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class on_exception : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public on_exception()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public on_exception(ident _exception_var_name,ident _exception_type_name,statement _stat)
		{
			this._exception_var_name=_exception_var_name;
			this._exception_type_name=_exception_type_name;
			this._stat=_stat;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public on_exception(ident _exception_var_name,ident _exception_type_name,statement _stat,SourceContext sc)
		{
			this._exception_var_name=_exception_var_name;
			this._exception_type_name=_exception_type_name;
			this._stat=_stat;
			source_context = sc;
		}

		protected ident _exception_var_name;
		protected ident _exception_type_name;
		protected statement _stat;

		///<summary>
		///
		///</summary>
		public ident exception_var_name
		{
			get
			{
				return _exception_var_name;
			}
			set
			{
				_exception_var_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident exception_type_name
		{
			get
			{
				return _exception_type_name;
			}
			set
			{
				_exception_type_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement stat
		{
			get
			{
				return _stat;
			}
			set
			{
				_stat=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class on_exception_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public on_exception_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public on_exception_list(List<on_exception> _on_exceptions)
		{
			this._on_exceptions=_on_exceptions;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public on_exception_list(List<on_exception> _on_exceptions,SourceContext sc)
		{
			this._on_exceptions=_on_exceptions;
			source_context = sc;
		}

		protected List<on_exception> _on_exceptions=new List<on_exception>();

		///<summary>
		///
		///</summary>
		public List<on_exception> on_exceptions
		{
			get
			{
				return _on_exceptions;
			}
			set
			{
				_on_exceptions=value;
			}
		}


		public on_exception_list(on_exception _on_exception, SourceContext sc)
		{
		    Add(_on_exception,sc);
		}
		public on_exception_list Add(on_exception _on_exception)
		{
		    on_exceptions.Add(_on_exception);
		    return this;
		}
		public on_exception_list Add(on_exception _on_exception, SourceContext sc)
		{
		    on_exceptions.Add(_on_exception);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class try_finally_statement : try_statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public try_finally_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_finally_statement(statement_list _finally_statements)
		{
			this._finally_statements=_finally_statements;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_finally_statement(statement_list _finally_statements,SourceContext sc)
		{
			this._finally_statements=_finally_statements;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_finally_statement(statement_list _statements,statement_list _finally_statements)
		{
			this._statements=_statements;
			this._finally_statements=_finally_statements;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_finally_statement(statement_list _statements,statement_list _finally_statements,SourceContext sc)
		{
			this._statements=_statements;
			this._finally_statements=_finally_statements;
			source_context = sc;
		}

		protected statement_list _finally_statements;

		///<summary>
		///
		///</summary>
		public statement_list finally_statements
		{
			get
			{
				return _finally_statements;
			}
			set
			{
				_finally_statements=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class try_except_statement : try_statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public try_except_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_except_statement(on_exception_list _on_except,statement_list _else_statements)
		{
			this._on_except=_on_except;
			this._else_statements=_else_statements;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_except_statement(on_exception_list _on_except,statement_list _else_statements,SourceContext sc)
		{
			this._on_except=_on_except;
			this._else_statements=_else_statements;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_except_statement(statement_list _statements,on_exception_list _on_except,statement_list _else_statements)
		{
			this._statements=_statements;
			this._on_except=_on_except;
			this._else_statements=_else_statements;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_except_statement(statement_list _statements,on_exception_list _on_except,statement_list _else_statements,SourceContext sc)
		{
			this._statements=_statements;
			this._on_except=_on_except;
			this._else_statements=_else_statements;
			source_context = sc;
		}

		protected on_exception_list _on_except;
		protected statement_list _else_statements;

		///<summary>
		///
		///</summary>
		public on_exception_list on_except
		{
			get
			{
				return _on_except;
			}
			set
			{
				_on_except=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement_list else_statements
		{
			get
			{
				return _else_statements;
			}
			set
			{
				_else_statements=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class record_const_definition : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public record_const_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_const_definition(ident _name,expression _val)
		{
			this._name=_name;
			this._val=_val;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_const_definition(ident _name,expression _val,SourceContext sc)
		{
			this._name=_name;
			this._val=_val;
			source_context = sc;
		}

		protected ident _name;
		protected expression _val;

		///<summary>
		///
		///</summary>
		public ident name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression val
		{
			get
			{
				return _val;
			}
			set
			{
				_val=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class record_const : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public record_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_const(List<record_const_definition> _rec_consts)
		{
			this._rec_consts=_rec_consts;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_const(List<record_const_definition> _rec_consts,SourceContext sc)
		{
			this._rec_consts=_rec_consts;
			source_context = sc;
		}

		protected List<record_const_definition> _rec_consts=new List<record_const_definition>();

		///<summary>
		///
		///</summary>
		public List<record_const_definition> rec_consts
		{
			get
			{
				return _rec_consts;
			}
			set
			{
				_rec_consts=value;
			}
		}


		public record_const(record_const_definition _record_const_definition, SourceContext sc)
		{
		    Add(_record_const_definition,sc);
		}
		public record_const Add(record_const_definition _record_const_definition)
		{
		    rec_consts.Add(_record_const_definition);
		    return this;
		}
		public record_const Add(record_const_definition _record_const_definition, SourceContext sc)
		{
		    rec_consts.Add(_record_const_definition);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class record_type : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public record_type()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_type(record_type_parts _parts,type_definition _base_type)
		{
			this._parts=_parts;
			this._base_type=_base_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_type(record_type_parts _parts,type_definition _base_type,SourceContext sc)
		{
			this._parts=_parts;
			this._base_type=_base_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_type(type_definition_attr_list _attr_list,record_type_parts _parts,type_definition _base_type)
		{
			this._attr_list=_attr_list;
			this._parts=_parts;
			this._base_type=_base_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_type(type_definition_attr_list _attr_list,record_type_parts _parts,type_definition _base_type,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._parts=_parts;
			this._base_type=_base_type;
			source_context = sc;
		}

		protected record_type_parts _parts;
		protected type_definition _base_type;

		///<summary>
		///
		///</summary>
		public record_type_parts parts
		{
			get
			{
				return _parts;
			}
			set
			{
				_parts=value;
			}
		}

		///<summary>
		///in oberon2
		///</summary>
		public type_definition base_type
		{
			get
			{
				return _base_type;
			}
			set
			{
				_base_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class enum_type_definition : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public enum_type_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public enum_type_definition(enumerator_list _enumerators)
		{
			this._enumerators=_enumerators;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public enum_type_definition(enumerator_list _enumerators,SourceContext sc)
		{
			this._enumerators=_enumerators;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public enum_type_definition(type_definition_attr_list _attr_list,enumerator_list _enumerators)
		{
			this._attr_list=_attr_list;
			this._enumerators=_enumerators;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public enum_type_definition(type_definition_attr_list _attr_list,enumerator_list _enumerators,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._enumerators=_enumerators;
			source_context = sc;
		}

		protected enumerator_list _enumerators;

		///<summary>
		///
		///</summary>
		public enumerator_list enumerators
		{
			get
			{
				return _enumerators;
			}
			set
			{
				_enumerators=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Класс, представляющий символьную константу в синтаксическом дереве программы.
	///</summary>
	[Serializable]
	public class char_const : literal
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public char_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public char_const(char _cconst)
		{
			this._cconst=_cconst;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public char_const(char _cconst,SourceContext sc)
		{
			this._cconst=_cconst;
			source_context = sc;
		}

		protected char _cconst;

		///<summary>
		///
		///</summary>
		public char cconst
		{
			get
			{
				return _cconst;
			}
			set
			{
				_cconst=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Выкидывание исключения.
	///</summary>
	[Serializable]
	public class raise_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public raise_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public raise_statement(expression _excep)
		{
			this._excep=_excep;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public raise_statement(expression _excep,SourceContext sc)
		{
			this._excep=_excep;
			source_context = sc;
		}

		protected expression _excep;

		///<summary>
		///Выкидываемое исключение.
		///</summary>
		public expression excep
		{
			get
			{
				return _excep;
			}
			set
			{
				_excep=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Представление в синтаксичеком дереве символьной константы вида #100.
	///</summary>
	[Serializable]
	public class sharp_char_const : literal
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public sharp_char_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public sharp_char_const(int _char_num)
		{
			this._char_num=_char_num;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public sharp_char_const(int _char_num,SourceContext sc)
		{
			this._char_num=_char_num;
			source_context = sc;
		}

		protected int _char_num;

		///<summary>
		///
		///</summary>
		public int char_num
		{
			get
			{
				return _char_num;
			}
			set
			{
				_char_num=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Представляет в синтаксическом дереве строку вида #123'abc'#124#125.
	///</summary>
	[Serializable]
	public class literal_const_line : literal
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public literal_const_line()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public literal_const_line(List<literal> _literals)
		{
			this._literals=_literals;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public literal_const_line(List<literal> _literals,SourceContext sc)
		{
			this._literals=_literals;
			source_context = sc;
		}

		protected List<literal> _literals=new List<literal>();

		///<summary>
		///
		///</summary>
		public List<literal> literals
		{
			get
			{
				return _literals;
			}
			set
			{
				_literals=value;
			}
		}


		public literal_const_line(literal _literal, SourceContext sc)
		{
		    Add(_literal,sc);
		}
		public literal_const_line Add(literal _literal)
		{
		    literals.Add(_literal);
		    return this;
		}
		public literal_const_line Add(literal _literal, SourceContext sc)
		{
		    literals.Add(_literal);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Представление для класса строки вида string[256].
	///</summary>
	[Serializable]
	public class string_num_definition : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public string_num_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public string_num_definition(expression _num_of_symbols,ident _name)
		{
			this._num_of_symbols=_num_of_symbols;
			this._name=_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public string_num_definition(expression _num_of_symbols,ident _name,SourceContext sc)
		{
			this._num_of_symbols=_num_of_symbols;
			this._name=_name;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public string_num_definition(type_definition_attr_list _attr_list,expression _num_of_symbols,ident _name)
		{
			this._attr_list=_attr_list;
			this._num_of_symbols=_num_of_symbols;
			this._name=_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public string_num_definition(type_definition_attr_list _attr_list,expression _num_of_symbols,ident _name,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._num_of_symbols=_num_of_symbols;
			this._name=_name;
			source_context = sc;
		}

		protected expression _num_of_symbols;
		protected ident _name;

		///<summary>
		///Число символов в строке вида string[256].
		///</summary>
		public expression num_of_symbols
		{
			get
			{
				return _num_of_symbols;
			}
			set
			{
				_num_of_symbols=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class variant : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public variant()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant(ident_list _vars,type_definition _vars_type)
		{
			this._vars=_vars;
			this._vars_type=_vars_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant(ident_list _vars,type_definition _vars_type,SourceContext sc)
		{
			this._vars=_vars;
			this._vars_type=_vars_type;
			source_context = sc;
		}

		protected ident_list _vars;
		protected type_definition _vars_type;

		///<summary>
		///
		///</summary>
		public ident_list vars
		{
			get
			{
				return _vars;
			}
			set
			{
				_vars=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition vars_type
		{
			get
			{
				return _vars_type;
			}
			set
			{
				_vars_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class variant_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public variant_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant_list(List<variant> _vars)
		{
			this._vars=_vars;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant_list(List<variant> _vars,SourceContext sc)
		{
			this._vars=_vars;
			source_context = sc;
		}

		protected List<variant> _vars=new List<variant>();

		///<summary>
		///
		///</summary>
		public List<variant> vars
		{
			get
			{
				return _vars;
			}
			set
			{
				_vars=value;
			}
		}


		public variant_list(variant _variant, SourceContext sc)
		{
		    Add(_variant,sc);
		}
		public variant_list Add(variant _variant)
		{
		    vars.Add(_variant);
		    return this;
		}
		public variant_list Add(variant _variant, SourceContext sc)
		{
		    vars.Add(_variant);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class variant_type : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public variant_type()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant_type(expression_list _case_exprs,record_type_parts _parts)
		{
			this._case_exprs=_case_exprs;
			this._parts=_parts;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant_type(expression_list _case_exprs,record_type_parts _parts,SourceContext sc)
		{
			this._case_exprs=_case_exprs;
			this._parts=_parts;
			source_context = sc;
		}

		protected expression_list _case_exprs;
		protected record_type_parts _parts;

		///<summary>
		///
		///</summary>
		public expression_list case_exprs
		{
			get
			{
				return _case_exprs;
			}
			set
			{
				_case_exprs=value;
			}
		}

		///<summary>
		///
		///</summary>
		public record_type_parts parts
		{
			get
			{
				return _parts;
			}
			set
			{
				_parts=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class variant_types : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public variant_types()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant_types(List<variant_type> _vars)
		{
			this._vars=_vars;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant_types(List<variant_type> _vars,SourceContext sc)
		{
			this._vars=_vars;
			source_context = sc;
		}

		protected List<variant_type> _vars=new List<variant_type>();

		///<summary>
		///
		///</summary>
		public List<variant_type> vars
		{
			get
			{
				return _vars;
			}
			set
			{
				_vars=value;
			}
		}


		public variant_types(variant_type _variant_type, SourceContext sc)
		{
		    Add(_variant_type,sc);
		}
		public variant_types Add(variant_type _variant_type)
		{
		    vars.Add(_variant_type);
		    return this;
		}
		public variant_types Add(variant_type _variant_type, SourceContext sc)
		{
		    vars.Add(_variant_type);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class variant_record_type : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public variant_record_type()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant_record_type(ident _var_name,type_definition _var_type,variant_types _vars)
		{
			this._var_name=_var_name;
			this._var_type=_var_type;
			this._vars=_vars;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public variant_record_type(ident _var_name,type_definition _var_type,variant_types _vars,SourceContext sc)
		{
			this._var_name=_var_name;
			this._var_type=_var_type;
			this._vars=_vars;
			source_context = sc;
		}

		protected ident _var_name;
		protected type_definition _var_type;
		protected variant_types _vars;

		///<summary>
		///
		///</summary>
		public ident var_name
		{
			get
			{
				return _var_name;
			}
			set
			{
				_var_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition var_type
		{
			get
			{
				return _var_type;
			}
			set
			{
				_var_type=value;
			}
		}

		///<summary>
		///
		///</summary>
		public variant_types vars
		{
			get
			{
				return _vars;
			}
			set
			{
				_vars=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class procedure_call : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public procedure_call()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_call(addressed_value _func_name)
		{
			this._func_name=_func_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public procedure_call(addressed_value _func_name,SourceContext sc)
		{
			this._func_name=_func_name;
			source_context = sc;
		}

		protected addressed_value _func_name;

		///<summary>
		///
		///</summary>
		public addressed_value func_name
		{
			get
			{
				return _func_name;
			}
			set
			{
				_func_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class class_predefinition : type_declaration
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public class_predefinition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_predefinition(ident _class_name)
		{
			this._class_name=_class_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_predefinition(ident _class_name,SourceContext sc)
		{
			this._class_name=_class_name;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_predefinition(ident _type_name,type_definition _type_def,ident _class_name)
		{
			this._type_name=_type_name;
			this._type_def=_type_def;
			this._class_name=_class_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public class_predefinition(ident _type_name,type_definition _type_def,ident _class_name,SourceContext sc)
		{
			this._type_name=_type_name;
			this._type_def=_type_def;
			this._class_name=_class_name;
			source_context = sc;
		}

		protected ident _class_name;

		///<summary>
		///
		///</summary>
		public ident class_name
		{
			get
			{
				return _class_name;
			}
			set
			{
				_class_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class nil_const : const_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public nil_const()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class file_type_definition : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public file_type_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public file_type_definition(type_definition _elem_type)
		{
			this._elem_type=_elem_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public file_type_definition(type_definition _elem_type,SourceContext sc)
		{
			this._elem_type=_elem_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public file_type_definition(type_definition_attr_list _attr_list,type_definition _elem_type)
		{
			this._attr_list=_attr_list;
			this._elem_type=_elem_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public file_type_definition(type_definition_attr_list _attr_list,type_definition _elem_type,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._elem_type=_elem_type;
			source_context = sc;
		}

		protected type_definition _elem_type;

		///<summary>
		///
		///</summary>
		public type_definition elem_type
		{
			get
			{
				return _elem_type;
			}
			set
			{
				_elem_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class constructor : procedure_header
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public constructor()
		{

		}


		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public constructor(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public constructor(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
			source_context = sc;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class destructor : procedure_header
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public destructor()
		{

		}


		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public destructor(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public destructor(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
			source_context = sc;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class inherited_method_call : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public inherited_method_call()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public inherited_method_call(ident _method_name,expression_list _exprs)
		{
			this._method_name=_method_name;
			this._exprs=_exprs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public inherited_method_call(ident _method_name,expression_list _exprs,SourceContext sc)
		{
			this._method_name=_method_name;
			this._exprs=_exprs;
			source_context = sc;
		}

		protected ident _method_name;
		protected expression_list _exprs;

		///<summary>
		///
		///</summary>
		public ident method_name
		{
			get
			{
				return _method_name;
			}
			set
			{
				_method_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression_list exprs
		{
			get
			{
				return _exprs;
			}
			set
			{
				_exprs=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class typecast_node : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public typecast_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typecast_node(addressed_value _expr,type_definition _type_def,op_typecast _cast_op)
		{
			this._expr=_expr;
			this._type_def=_type_def;
			this._cast_op=_cast_op;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typecast_node(addressed_value _expr,type_definition _type_def,op_typecast _cast_op,SourceContext sc)
		{
			this._expr=_expr;
			this._type_def=_type_def;
			this._cast_op=_cast_op;
			source_context = sc;
		}

		protected addressed_value _expr;
		protected type_definition _type_def;
		protected op_typecast _cast_op;

		///<summary>
		///
		///</summary>
		public addressed_value expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition type_def
		{
			get
			{
				return _type_def;
			}
			set
			{
				_type_def=value;
			}
		}

		///<summary>
		///
		///</summary>
		public op_typecast cast_op
		{
			get
			{
				return _cast_op;
			}
			set
			{
				_cast_op=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class interface_node : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public interface_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public interface_node(declarations _interface_definitions,uses_list _uses_modules,using_list _using_namespaces)
		{
			this._interface_definitions=_interface_definitions;
			this._uses_modules=_uses_modules;
			this._using_namespaces=_using_namespaces;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public interface_node(declarations _interface_definitions,uses_list _uses_modules,using_list _using_namespaces,SourceContext sc)
		{
			this._interface_definitions=_interface_definitions;
			this._uses_modules=_uses_modules;
			this._using_namespaces=_using_namespaces;
			source_context = sc;
		}

		protected declarations _interface_definitions;
		protected uses_list _uses_modules;
		protected using_list _using_namespaces;

		///<summary>
		///
		///</summary>
		public declarations interface_definitions
		{
			get
			{
				return _interface_definitions;
			}
			set
			{
				_interface_definitions=value;
			}
		}

		///<summary>
		///
		///</summary>
		public uses_list uses_modules
		{
			get
			{
				return _uses_modules;
			}
			set
			{
				_uses_modules=value;
			}
		}

		///<summary>
		///
		///</summary>
		public using_list using_namespaces
		{
			get
			{
				return _using_namespaces;
			}
			set
			{
				_using_namespaces=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class implementation_node : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public implementation_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public implementation_node(uses_list _uses_modules,declarations _implementation_definitions,using_list _using_namespaces)
		{
			this._uses_modules=_uses_modules;
			this._implementation_definitions=_implementation_definitions;
			this._using_namespaces=_using_namespaces;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public implementation_node(uses_list _uses_modules,declarations _implementation_definitions,using_list _using_namespaces,SourceContext sc)
		{
			this._uses_modules=_uses_modules;
			this._implementation_definitions=_implementation_definitions;
			this._using_namespaces=_using_namespaces;
			source_context = sc;
		}

		protected uses_list _uses_modules;
		protected declarations _implementation_definitions;
		protected using_list _using_namespaces;

		///<summary>
		///
		///</summary>
		public uses_list uses_modules
		{
			get
			{
				return _uses_modules;
			}
			set
			{
				_uses_modules=value;
			}
		}

		///<summary>
		///
		///</summary>
		public declarations implementation_definitions
		{
			get
			{
				return _implementation_definitions;
			}
			set
			{
				_implementation_definitions=value;
			}
		}

		///<summary>
		///
		///</summary>
		public using_list using_namespaces
		{
			get
			{
				return _using_namespaces;
			}
			set
			{
				_using_namespaces=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class diap_expr : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public diap_expr()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public diap_expr(expression _left,expression _right)
		{
			this._left=_left;
			this._right=_right;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public diap_expr(expression _left,expression _right,SourceContext sc)
		{
			this._left=_left;
			this._right=_right;
			source_context = sc;
		}

		protected expression _left;
		protected expression _right;

		///<summary>
		///
		///</summary>
		public expression left
		{
			get
			{
				return _left;
			}
			set
			{
				_left=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression right
		{
			get
			{
				return _right;
			}
			set
			{
				_right=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class block : proc_block
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public block()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public block(declarations _defs,statement_list _program_code)
		{
			this._defs=_defs;
			this._program_code=_program_code;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public block(declarations _defs,statement_list _program_code,SourceContext sc)
		{
			this._defs=_defs;
			this._program_code=_program_code;
			source_context = sc;
		}

		protected declarations _defs;
		protected statement_list _program_code;

		///<summary>
		///
		///</summary>
		public declarations defs
		{
			get
			{
				return _defs;
			}
			set
			{
				_defs=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement_list program_code
		{
			get
			{
				return _program_code;
			}
			set
			{
				_program_code=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class proc_block : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public proc_block()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///array of type_name
	///</summary>
	[Serializable]
	public class array_of_named_type_definition : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public array_of_named_type_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_of_named_type_definition(named_type_reference _type_name)
		{
			this._type_name=_type_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_of_named_type_definition(named_type_reference _type_name,SourceContext sc)
		{
			this._type_name=_type_name;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_of_named_type_definition(type_definition_attr_list _attr_list,named_type_reference _type_name)
		{
			this._attr_list=_attr_list;
			this._type_name=_type_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_of_named_type_definition(type_definition_attr_list _attr_list,named_type_reference _type_name,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._type_name=_type_name;
			source_context = sc;
		}

		protected named_type_reference _type_name;

		///<summary>
		///
		///</summary>
		public named_type_reference type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///array of const
	///</summary>
	[Serializable]
	public class array_of_const_type_definition : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public array_of_const_type_definition()
		{

		}


		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_of_const_type_definition(type_definition_attr_list _attr_list)
		{
			this._attr_list=_attr_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_of_const_type_definition(type_definition_attr_list _attr_list,SourceContext sc)
		{
			this._attr_list=_attr_list;
			source_context = sc;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///#12 либо 'abc'
	///</summary>
	[Serializable]
	public class literal : const_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public literal()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class case_variants : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public case_variants()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public case_variants(List<case_variant> _variants)
		{
			this._variants=_variants;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public case_variants(List<case_variant> _variants,SourceContext sc)
		{
			this._variants=_variants;
			source_context = sc;
		}

		protected List<case_variant> _variants=new List<case_variant>();

		///<summary>
		///
		///</summary>
		public List<case_variant> variants
		{
			get
			{
				return _variants;
			}
			set
			{
				_variants=value;
			}
		}


		public case_variants(case_variant _case_variant, SourceContext sc)
		{
		    Add(_case_variant,sc);
		}
		public case_variants Add(case_variant _case_variant)
		{
		    variants.Add(_case_variant);
		    return this;
		}
		public case_variants Add(case_variant _case_variant, SourceContext sc)
		{
		    variants.Add(_case_variant);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class diapason_expr : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public diapason_expr()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public diapason_expr(expression _left,expression _right)
		{
			this._left=_left;
			this._right=_right;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public diapason_expr(expression _left,expression _right,SourceContext sc)
		{
			this._left=_left;
			this._right=_right;
			source_context = sc;
		}

		protected expression _left;
		protected expression _right;

		///<summary>
		///
		///</summary>
		public expression left
		{
			get
			{
				return _left;
			}
			set
			{
				_left=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression right
		{
			get
			{
				return _right;
			}
			set
			{
				_right=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class var_def_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public var_def_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public var_def_list(List<var_def_statement> _vars)
		{
			this._vars=_vars;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public var_def_list(List<var_def_statement> _vars,SourceContext sc)
		{
			this._vars=_vars;
			source_context = sc;
		}

		protected List<var_def_statement> _vars=new List<var_def_statement>();

		///<summary>
		///
		///</summary>
		public List<var_def_statement> vars
		{
			get
			{
				return _vars;
			}
			set
			{
				_vars=value;
			}
		}


		public var_def_list(var_def_statement _var_def_statement, SourceContext sc)
		{
		    Add(_var_def_statement,sc);
		}
		public var_def_list Add(var_def_statement _var_def_statement)
		{
		    vars.Add(_var_def_statement);
		    return this;
		}
		public var_def_list Add(var_def_statement _var_def_statement, SourceContext sc)
		{
		    vars.Add(_var_def_statement);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class record_type_parts : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public record_type_parts()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_type_parts(var_def_list _fixed_part,variant_record_type _variant_part)
		{
			this._fixed_part=_fixed_part;
			this._variant_part=_variant_part;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public record_type_parts(var_def_list _fixed_part,variant_record_type _variant_part,SourceContext sc)
		{
			this._fixed_part=_fixed_part;
			this._variant_part=_variant_part;
			source_context = sc;
		}

		protected var_def_list _fixed_part;
		protected variant_record_type _variant_part;

		///<summary>
		///
		///</summary>
		public var_def_list fixed_part
		{
			get
			{
				return _fixed_part;
			}
			set
			{
				_fixed_part=value;
			}
		}

		///<summary>
		///
		///</summary>
		public variant_record_type variant_part
		{
			get
			{
				return _variant_part;
			}
			set
			{
				_variant_part=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class property_array_default : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public property_array_default()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class property_interface : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public property_interface()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public property_interface(property_parameter_list _parameter_list,type_definition _property_type,expression _index_expression)
		{
			this._parameter_list=_parameter_list;
			this._property_type=_property_type;
			this._index_expression=_index_expression;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public property_interface(property_parameter_list _parameter_list,type_definition _property_type,expression _index_expression,SourceContext sc)
		{
			this._parameter_list=_parameter_list;
			this._property_type=_property_type;
			this._index_expression=_index_expression;
			source_context = sc;
		}

		protected property_parameter_list _parameter_list;
		protected type_definition _property_type;
		protected expression _index_expression;

		///<summary>
		///
		///</summary>
		public property_parameter_list parameter_list
		{
			get
			{
				return _parameter_list;
			}
			set
			{
				_parameter_list=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition property_type
		{
			get
			{
				return _property_type;
			}
			set
			{
				_property_type=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression index_expression
		{
			get
			{
				return _index_expression;
			}
			set
			{
				_index_expression=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class property_parameter : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public property_parameter()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public property_parameter(ident_list _names,type_definition _type)
		{
			this._names=_names;
			this._type=_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public property_parameter(ident_list _names,type_definition _type,SourceContext sc)
		{
			this._names=_names;
			this._type=_type;
			source_context = sc;
		}

		protected ident_list _names;
		protected type_definition _type;

		///<summary>
		///
		///</summary>
		public ident_list names
		{
			get
			{
				return _names;
			}
			set
			{
				_names=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition type
		{
			get
			{
				return _type;
			}
			set
			{
				_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class property_parameter_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public property_parameter_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public property_parameter_list(List<property_parameter> _parameters)
		{
			this._parameters=_parameters;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public property_parameter_list(List<property_parameter> _parameters,SourceContext sc)
		{
			this._parameters=_parameters;
			source_context = sc;
		}

		protected List<property_parameter> _parameters=new List<property_parameter>();

		///<summary>
		///
		///</summary>
		public List<property_parameter> parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters=value;
			}
		}


		public property_parameter_list(property_parameter _property_parameter, SourceContext sc)
		{
		    Add(_property_parameter,sc);
		}
		public property_parameter_list Add(property_parameter _property_parameter)
		{
		    parameters.Add(_property_parameter);
		    return this;
		}
		public property_parameter_list Add(property_parameter _property_parameter, SourceContext sc)
		{
		    parameters.Add(_property_parameter);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class inherited_ident : ident
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public inherited_ident()
		{

		}


		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public inherited_ident(string _name)
		{
			this._name=_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public inherited_ident(string _name,SourceContext sc)
		{
			this._name=_name;
			source_context = sc;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///expr:1:2
	///</summary>
	[Serializable]
	public class format_expr : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public format_expr()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public format_expr(expression _expr,expression _format1,expression _format2)
		{
			this._expr=_expr;
			this._format1=_format1;
			this._format2=_format2;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public format_expr(expression _expr,expression _format1,expression _format2,SourceContext sc)
		{
			this._expr=_expr;
			this._format1=_format1;
			this._format2=_format2;
			source_context = sc;
		}

		protected expression _expr;
		protected expression _format1;
		protected expression _format2;

		///<summary>
		///
		///</summary>
		public expression expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression format1
		{
			get
			{
				return _format1;
			}
			set
			{
				_format1=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression format2
		{
			get
			{
				return _format2;
			}
			set
			{
				_format2=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class initfinal_part : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public initfinal_part()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public initfinal_part(statement_list _initialization_sect,statement_list _finalization_sect)
		{
			this._initialization_sect=_initialization_sect;
			this._finalization_sect=_finalization_sect;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public initfinal_part(statement_list _initialization_sect,statement_list _finalization_sect,SourceContext sc)
		{
			this._initialization_sect=_initialization_sect;
			this._finalization_sect=_finalization_sect;
			source_context = sc;
		}

		protected statement_list _initialization_sect;
		protected statement_list _finalization_sect;

		///<summary>
		///
		///</summary>
		public statement_list initialization_sect
		{
			get
			{
				return _initialization_sect;
			}
			set
			{
				_initialization_sect=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement_list finalization_sect
		{
			get
			{
				return _finalization_sect;
			}
			set
			{
				_finalization_sect=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class token_info : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public token_info()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public token_info(string _text)
		{
			this._text=_text;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public token_info(string _text,SourceContext sc)
		{
			this._text=_text;
			source_context = sc;
		}

		protected string _text;

		///<summary>
		///
		///</summary>
		public string text
		{
			get
			{
				return _text;
			}
			set
			{
				_text=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///raise [expr [at address]]
	///</summary>
	[Serializable]
	public class raise_stmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public raise_stmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public raise_stmt(expression _expr,expression _address)
		{
			this._expr=_expr;
			this._address=_address;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public raise_stmt(expression _expr,expression _address,SourceContext sc)
		{
			this._expr=_expr;
			this._address=_address;
			source_context = sc;
		}

		protected expression _expr;
		protected expression _address;

		///<summary>
		///
		///</summary>
		public expression expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression address
		{
			get
			{
				return _address;
			}
			set
			{
				_address=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class op_type_node : token_info
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public op_type_node()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public op_type_node(Operators _type)
		{
			this._type=_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public op_type_node(Operators _type,SourceContext sc)
		{
			this._type=_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public op_type_node(string _text,Operators _type)
		{
			this._text=_text;
			this._type=_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public op_type_node(string _text,Operators _type,SourceContext sc)
		{
			this._text=_text;
			this._type=_type;
			source_context = sc;
		}

		protected Operators _type;

		///<summary>
		///
		///</summary>
		public Operators type
		{
			get
			{
				return _type;
			}
			set
			{
				_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class file_type : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public file_type()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public file_type(type_definition _file_of_type)
		{
			this._file_of_type=_file_of_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public file_type(type_definition _file_of_type,SourceContext sc)
		{
			this._file_of_type=_file_of_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public file_type(type_definition_attr_list _attr_list,type_definition _file_of_type)
		{
			this._attr_list=_attr_list;
			this._file_of_type=_file_of_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public file_type(type_definition_attr_list _attr_list,type_definition _file_of_type,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._file_of_type=_file_of_type;
			source_context = sc;
		}

		protected type_definition _file_of_type;

		///<summary>
		///
		///</summary>
		public type_definition file_of_type
		{
			get
			{
				return _file_of_type;
			}
			set
			{
				_file_of_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class known_type_ident : ident
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public known_type_ident()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public known_type_ident(known_type _type)
		{
			this._type=_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public known_type_ident(known_type _type,SourceContext sc)
		{
			this._type=_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public known_type_ident(string _name,known_type _type)
		{
			this._name=_name;
			this._type=_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public known_type_ident(string _name,known_type _type,SourceContext sc)
		{
			this._name=_name;
			this._type=_type;
			source_context = sc;
		}

		protected known_type _type;

		///<summary>
		///
		///</summary>
		public known_type type
		{
			get
			{
				return _type;
			}
			set
			{
				_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class exception_handler : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public exception_handler()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public exception_handler(ident _variable,named_type_reference _type_name,statement _statements)
		{
			this._variable=_variable;
			this._type_name=_type_name;
			this._statements=_statements;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public exception_handler(ident _variable,named_type_reference _type_name,statement _statements,SourceContext sc)
		{
			this._variable=_variable;
			this._type_name=_type_name;
			this._statements=_statements;
			source_context = sc;
		}

		protected ident _variable;
		protected named_type_reference _type_name;
		protected statement _statements;

		///<summary>
		///
		///</summary>
		public ident variable
		{
			get
			{
				return _variable;
			}
			set
			{
				_variable=value;
			}
		}

		///<summary>
		///
		///</summary>
		public named_type_reference type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement statements
		{
			get
			{
				return _statements;
			}
			set
			{
				_statements=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class exception_ident : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public exception_ident()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public exception_ident(ident _variable,named_type_reference _type_name)
		{
			this._variable=_variable;
			this._type_name=_type_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public exception_ident(ident _variable,named_type_reference _type_name,SourceContext sc)
		{
			this._variable=_variable;
			this._type_name=_type_name;
			source_context = sc;
		}

		protected ident _variable;
		protected named_type_reference _type_name;

		///<summary>
		///
		///</summary>
		public ident variable
		{
			get
			{
				return _variable;
			}
			set
			{
				_variable=value;
			}
		}

		///<summary>
		///
		///</summary>
		public named_type_reference type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class exception_handler_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public exception_handler_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public exception_handler_list(List<exception_handler> _handlers)
		{
			this._handlers=_handlers;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public exception_handler_list(List<exception_handler> _handlers,SourceContext sc)
		{
			this._handlers=_handlers;
			source_context = sc;
		}

		protected List<exception_handler> _handlers=new List<exception_handler>();

		///<summary>
		///
		///</summary>
		public List<exception_handler> handlers
		{
			get
			{
				return _handlers;
			}
			set
			{
				_handlers=value;
			}
		}


		public exception_handler_list(exception_handler _exception_handler, SourceContext sc)
		{
		    Add(_exception_handler,sc);
		}
		public exception_handler_list Add(exception_handler _exception_handler)
		{
		    handlers.Add(_exception_handler);
		    return this;
		}
		public exception_handler_list Add(exception_handler _exception_handler, SourceContext sc)
		{
		    handlers.Add(_exception_handler);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class exception_block : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public exception_block()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public exception_block(statement_list _stmt_list,exception_handler_list _handlers,statement_list _else_stmt_list)
		{
			this._stmt_list=_stmt_list;
			this._handlers=_handlers;
			this._else_stmt_list=_else_stmt_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public exception_block(statement_list _stmt_list,exception_handler_list _handlers,statement_list _else_stmt_list,SourceContext sc)
		{
			this._stmt_list=_stmt_list;
			this._handlers=_handlers;
			this._else_stmt_list=_else_stmt_list;
			source_context = sc;
		}

		protected statement_list _stmt_list;
		protected exception_handler_list _handlers;
		protected statement_list _else_stmt_list;

		///<summary>
		///
		///</summary>
		public statement_list stmt_list
		{
			get
			{
				return _stmt_list;
			}
			set
			{
				_stmt_list=value;
			}
		}

		///<summary>
		///
		///</summary>
		public exception_handler_list handlers
		{
			get
			{
				return _handlers;
			}
			set
			{
				_handlers=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement_list else_stmt_list
		{
			get
			{
				return _else_stmt_list;
			}
			set
			{
				_else_stmt_list=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class try_handler : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public try_handler()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class try_handler_finally : try_handler
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public try_handler_finally()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_handler_finally(statement_list _stmt_list)
		{
			this._stmt_list=_stmt_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_handler_finally(statement_list _stmt_list,SourceContext sc)
		{
			this._stmt_list=_stmt_list;
			source_context = sc;
		}

		protected statement_list _stmt_list;

		///<summary>
		///
		///</summary>
		public statement_list stmt_list
		{
			get
			{
				return _stmt_list;
			}
			set
			{
				_stmt_list=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class try_handler_except : try_handler
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public try_handler_except()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_handler_except(exception_block _except_block)
		{
			this._except_block=_except_block;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_handler_except(exception_block _except_block,SourceContext sc)
		{
			this._except_block=_except_block;
			source_context = sc;
		}

		protected exception_block _except_block;

		///<summary>
		///
		///</summary>
		public exception_block except_block
		{
			get
			{
				return _except_block;
			}
			set
			{
				_except_block=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class try_stmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public try_stmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_stmt(statement_list _stmt_list,try_handler _handler)
		{
			this._stmt_list=_stmt_list;
			this._handler=_handler;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public try_stmt(statement_list _stmt_list,try_handler _handler,SourceContext sc)
		{
			this._stmt_list=_stmt_list;
			this._handler=_handler;
			source_context = sc;
		}

		protected statement_list _stmt_list;
		protected try_handler _handler;

		///<summary>
		///
		///</summary>
		public statement_list stmt_list
		{
			get
			{
				return _stmt_list;
			}
			set
			{
				_stmt_list=value;
			}
		}

		///<summary>
		///
		///</summary>
		public try_handler handler
		{
			get
			{
				return _handler;
			}
			set
			{
				_handler=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class inherited_message : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public inherited_message()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///expression может быть literal или ident
	///</summary>
	[Serializable]
	public class external_directive : proc_block
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public external_directive()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public external_directive(expression _modulename,expression _name)
		{
			this._modulename=_modulename;
			this._name=_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public external_directive(expression _modulename,expression _name,SourceContext sc)
		{
			this._modulename=_modulename;
			this._name=_name;
			source_context = sc;
		}

		protected expression _modulename;
		protected expression _name;

		///<summary>
		///
		///</summary>
		public expression modulename
		{
			get
			{
				return _modulename;
			}
			set
			{
				_modulename=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class using_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public using_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public using_list(List<unit_or_namespace> _namespaces)
		{
			this._namespaces=_namespaces;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public using_list(List<unit_or_namespace> _namespaces,SourceContext sc)
		{
			this._namespaces=_namespaces;
			source_context = sc;
		}

		protected List<unit_or_namespace> _namespaces=new List<unit_or_namespace>();

		///<summary>
		///
		///</summary>
		public List<unit_or_namespace> namespaces
		{
			get
			{
				return _namespaces;
			}
			set
			{
				_namespaces=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_import_module : unit_or_namespace
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_import_module()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_import_module(ident _new_name)
		{
			this._new_name=_new_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_import_module(ident _new_name,SourceContext sc)
		{
			this._new_name=_new_name;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_import_module(ident_list _name,ident _new_name)
		{
			this._name=_name;
			this._new_name=_new_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_import_module(ident_list _name,ident _new_name,SourceContext sc)
		{
			this._name=_name;
			this._new_name=_new_name;
			source_context = sc;
		}

		protected ident _new_name;

		///<summary>
		///import new_name:=unit_name;
		///</summary>
		public ident new_name
		{
			get
			{
				return _new_name;
			}
			set
			{
				_new_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_module : compilation_unit
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_module()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_module(ident _first_name,ident _second_name,uses_list _import_list,declarations _definitions,statement_list _module_code)
		{
			this._first_name=_first_name;
			this._second_name=_second_name;
			this._import_list=_import_list;
			this._definitions=_definitions;
			this._module_code=_module_code;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_module(ident _first_name,ident _second_name,uses_list _import_list,declarations _definitions,statement_list _module_code,SourceContext sc)
		{
			this._first_name=_first_name;
			this._second_name=_second_name;
			this._import_list=_import_list;
			this._definitions=_definitions;
			this._module_code=_module_code;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_module(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,ident _first_name,ident _second_name,uses_list _import_list,declarations _definitions,statement_list _module_code)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			this._first_name=_first_name;
			this._second_name=_second_name;
			this._import_list=_import_list;
			this._definitions=_definitions;
			this._module_code=_module_code;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_module(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,ident _first_name,ident _second_name,uses_list _import_list,declarations _definitions,statement_list _module_code,SourceContext sc)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			this._first_name=_first_name;
			this._second_name=_second_name;
			this._import_list=_import_list;
			this._definitions=_definitions;
			this._module_code=_module_code;
			source_context = sc;
		}

		protected ident _first_name;
		protected ident _second_name;
		protected uses_list _import_list;
		protected declarations _definitions;
		protected statement_list _module_code;

		///<summary>
		///
		///</summary>
		public ident first_name
		{
			get
			{
				return _first_name;
			}
			set
			{
				_first_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident second_name
		{
			get
			{
				return _second_name;
			}
			set
			{
				_second_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public uses_list import_list
		{
			get
			{
				return _import_list;
			}
			set
			{
				_import_list=value;
			}
		}

		///<summary>
		///
		///</summary>
		public declarations definitions
		{
			get
			{
				return _definitions;
			}
			set
			{
				_definitions=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement_list module_code
		{
			get
			{
				return _module_code;
			}
			set
			{
				_module_code=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_ident_with_export_marker : ident
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_ident_with_export_marker()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_ident_with_export_marker(oberon_export_marker _marker)
		{
			this._marker=_marker;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_ident_with_export_marker(oberon_export_marker _marker,SourceContext sc)
		{
			this._marker=_marker;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_ident_with_export_marker(string _name,oberon_export_marker _marker)
		{
			this._name=_name;
			this._marker=_marker;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_ident_with_export_marker(string _name,oberon_export_marker _marker,SourceContext sc)
		{
			this._name=_name;
			this._marker=_marker;
			source_context = sc;
		}

		protected oberon_export_marker _marker;

		///<summary>
		///
		///</summary>
		public oberon_export_marker marker
		{
			get
			{
				return _marker;
			}
			set
			{
				_marker=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_exit_stmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_exit_stmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_exit_stmt(string _text)
		{
			this._text=_text;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_exit_stmt(string _text,SourceContext sc)
		{
			this._text=_text;
			source_context = sc;
		}

		protected string _text;

		///<summary>
		///
		///</summary>
		public string text
		{
			get
			{
				return _text;
			}
			set
			{
				_text=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class jump_stmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public jump_stmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public jump_stmt(expression _expr,JumpStmtType _JumpType)
		{
			this._expr=_expr;
			this._JumpType=_JumpType;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public jump_stmt(expression _expr,JumpStmtType _JumpType,SourceContext sc)
		{
			this._expr=_expr;
			this._JumpType=_JumpType;
			source_context = sc;
		}

		protected expression _expr;
		protected JumpStmtType _JumpType;

		///<summary>
		///
		///</summary>
		public expression expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}

		///<summary>
		///
		///</summary>
		public JumpStmtType JumpType
		{
			get
			{
				return _JumpType;
			}
			set
			{
				_JumpType=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_procedure_receiver : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_procedure_receiver()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_procedure_receiver(parametr_kind _param_kind,ident _receiver_name,ident _receiver_typename)
		{
			this._param_kind=_param_kind;
			this._receiver_name=_receiver_name;
			this._receiver_typename=_receiver_typename;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_procedure_receiver(parametr_kind _param_kind,ident _receiver_name,ident _receiver_typename,SourceContext sc)
		{
			this._param_kind=_param_kind;
			this._receiver_name=_receiver_name;
			this._receiver_typename=_receiver_typename;
			source_context = sc;
		}

		protected parametr_kind _param_kind;
		protected ident _receiver_name;
		protected ident _receiver_typename;

		///<summary>
		///
		///</summary>
		public parametr_kind param_kind
		{
			get
			{
				return _param_kind;
			}
			set
			{
				_param_kind=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident receiver_name
		{
			get
			{
				return _receiver_name;
			}
			set
			{
				_receiver_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident receiver_typename
		{
			get
			{
				return _receiver_typename;
			}
			set
			{
				_receiver_typename=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_procedure_header : function_header
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_procedure_header()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_procedure_header(oberon_procedure_receiver _receiver,ident _first_name,ident _second_name)
		{
			this._receiver=_receiver;
			this._first_name=_first_name;
			this._second_name=_second_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_procedure_header(oberon_procedure_receiver _receiver,ident _first_name,ident _second_name,SourceContext sc)
		{
			this._receiver=_receiver;
			this._first_name=_first_name;
			this._second_name=_second_name;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_procedure_header(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs,type_definition _return_type,oberon_procedure_receiver _receiver,ident _first_name,ident _second_name)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
			this._return_type=_return_type;
			this._receiver=_receiver;
			this._first_name=_first_name;
			this._second_name=_second_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_procedure_header(type_definition_attr_list _attr_list,formal_parameters _parameters,procedure_attributes_list _proc_attributes,method_name _name,bool _of_object,bool _class_keyword,ident_list _template_args,where_definition_list _where_defs,type_definition _return_type,oberon_procedure_receiver _receiver,ident _first_name,ident _second_name,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._parameters=_parameters;
			this._proc_attributes=_proc_attributes;
			this._name=_name;
			this._of_object=_of_object;
			this._class_keyword=_class_keyword;
			this._template_args=_template_args;
			this._where_defs=_where_defs;
			this._return_type=_return_type;
			this._receiver=_receiver;
			this._first_name=_first_name;
			this._second_name=_second_name;
			source_context = sc;
		}

		protected oberon_procedure_receiver _receiver;
		protected ident _first_name;
		protected ident _second_name;

		///<summary>
		///
		///</summary>
		public oberon_procedure_receiver receiver
		{
			get
			{
				return _receiver;
			}
			set
			{
				_receiver=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident first_name
		{
			get
			{
				return _first_name;
			}
			set
			{
				_first_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public ident second_name
		{
			get
			{
				return _second_name;
			}
			set
			{
				_second_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_withstmt_guardstat : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_withstmt_guardstat()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_withstmt_guardstat(addressed_value _name,type_definition _type_name,statement _stmt)
		{
			this._name=_name;
			this._type_name=_type_name;
			this._stmt=_stmt;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_withstmt_guardstat(addressed_value _name,type_definition _type_name,statement _stmt,SourceContext sc)
		{
			this._name=_name;
			this._type_name=_type_name;
			this._stmt=_stmt;
			source_context = sc;
		}

		protected addressed_value _name;
		protected type_definition _type_name;
		protected statement _stmt;

		///<summary>
		///
		///</summary>
		public addressed_value name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement stmt
		{
			get
			{
				return _stmt;
			}
			set
			{
				_stmt=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_withstmt_guardstat_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_withstmt_guardstat_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_withstmt_guardstat_list(List<oberon_withstmt_guardstat> _guardstats)
		{
			this._guardstats=_guardstats;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_withstmt_guardstat_list(List<oberon_withstmt_guardstat> _guardstats,SourceContext sc)
		{
			this._guardstats=_guardstats;
			source_context = sc;
		}

		protected List<oberon_withstmt_guardstat> _guardstats=new List<oberon_withstmt_guardstat>();

		///<summary>
		///
		///</summary>
		public List<oberon_withstmt_guardstat> guardstats
		{
			get
			{
				return _guardstats;
			}
			set
			{
				_guardstats=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class oberon_withstmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public oberon_withstmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_withstmt(oberon_withstmt_guardstat_list _quardstat_list,statement _else_stmt)
		{
			this._quardstat_list=_quardstat_list;
			this._else_stmt=_else_stmt;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public oberon_withstmt(oberon_withstmt_guardstat_list _quardstat_list,statement _else_stmt,SourceContext sc)
		{
			this._quardstat_list=_quardstat_list;
			this._else_stmt=_else_stmt;
			source_context = sc;
		}

		protected oberon_withstmt_guardstat_list _quardstat_list;
		protected statement _else_stmt;

		///<summary>
		///
		///</summary>
		public oberon_withstmt_guardstat_list quardstat_list
		{
			get
			{
				return _quardstat_list;
			}
			set
			{
				_quardstat_list=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement else_stmt
		{
			get
			{
				return _else_stmt;
			}
			set
			{
				_else_stmt=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class loop_stmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public loop_stmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public loop_stmt(statement _stmt)
		{
			this._stmt=_stmt;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public loop_stmt(statement _stmt,SourceContext sc)
		{
			this._stmt=_stmt;
			source_context = sc;
		}

		protected statement _stmt;

		///<summary>
		///
		///</summary>
		public statement stmt
		{
			get
			{
				return _stmt;
			}
			set
			{
				_stmt=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class foreach_stmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public foreach_stmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public foreach_stmt(ident _identifier,type_definition _type_name,expression _in_what,statement _stmt)
		{
			this._identifier=_identifier;
			this._type_name=_type_name;
			this._in_what=_in_what;
			this._stmt=_stmt;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public foreach_stmt(ident _identifier,type_definition _type_name,expression _in_what,statement _stmt,SourceContext sc)
		{
			this._identifier=_identifier;
			this._type_name=_type_name;
			this._in_what=_in_what;
			this._stmt=_stmt;
			source_context = sc;
		}

		protected ident _identifier;
		protected type_definition _type_name;
		protected expression _in_what;
		protected statement _stmt;

		///<summary>
		///
		///</summary>
		public ident identifier
		{
			get
			{
				return _identifier;
			}
			set
			{
				_identifier=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression in_what
		{
			get
			{
				return _in_what;
			}
			set
			{
				_in_what=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement stmt
		{
			get
			{
				return _stmt;
			}
			set
			{
				_stmt=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class addressed_value_funcname : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public addressed_value_funcname()
		{

		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class named_type_reference_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public named_type_reference_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public named_type_reference_list(List<named_type_reference> _types)
		{
			this._types=_types;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public named_type_reference_list(List<named_type_reference> _types,SourceContext sc)
		{
			this._types=_types;
			source_context = sc;
		}

		protected List<named_type_reference> _types=new List<named_type_reference>();

		///<summary>
		///
		///</summary>
		public List<named_type_reference> types
		{
			get
			{
				return _types;
			}
			set
			{
				_types=value;
			}
		}


		public named_type_reference_list(named_type_reference _named_type_reference, SourceContext sc)
		{
		    Add(_named_type_reference,sc);
		}
		public named_type_reference_list Add(named_type_reference _named_type_reference)
		{
		    types.Add(_named_type_reference);
		    return this;
		}
		public named_type_reference_list Add(named_type_reference _named_type_reference, SourceContext sc)
		{
		    types.Add(_named_type_reference);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class template_param_list : dereference
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public template_param_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_param_list(List<type_definition> _params_list)
		{
			this._params_list=_params_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_param_list(List<type_definition> _params_list,SourceContext sc)
		{
			this._params_list=_params_list;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_param_list(addressed_value _dereferencing_value,List<type_definition> _params_list)
		{
			this._dereferencing_value=_dereferencing_value;
			this._params_list=_params_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_param_list(addressed_value _dereferencing_value,List<type_definition> _params_list,SourceContext sc)
		{
			this._dereferencing_value=_dereferencing_value;
			this._params_list=_params_list;
			source_context = sc;
		}

		protected List<type_definition> _params_list=new List<type_definition>();

		///<summary>
		///
		///</summary>
		public List<type_definition> params_list
		{
			get
			{
				return _params_list;
			}
			set
			{
				_params_list=value;
			}
		}


		public template_param_list(type_definition _type_definition, SourceContext sc)
		{
		    Add(_type_definition,sc);
		}
		public template_param_list Add(type_definition _type_definition)
		{
		    params_list.Add(_type_definition);
		    return this;
		}
		public template_param_list Add(type_definition _type_definition, SourceContext sc)
		{
		    params_list.Add(_type_definition);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class template_type_reference : named_type_reference
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public template_type_reference()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_type_reference(named_type_reference _name,template_param_list _params_list)
		{
			this._name=_name;
			this._params_list=_params_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_type_reference(named_type_reference _name,template_param_list _params_list,SourceContext sc)
		{
			this._name=_name;
			this._params_list=_params_list;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_type_reference(type_definition_attr_list _attr_list,List<ident> _names,named_type_reference _name,template_param_list _params_list)
		{
			this._attr_list=_attr_list;
			this._names=_names;
			this._name=_name;
			this._params_list=_params_list;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_type_reference(type_definition_attr_list _attr_list,List<ident> _names,named_type_reference _name,template_param_list _params_list,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._names=_names;
			this._name=_name;
			this._params_list=_params_list;
			source_context = sc;
		}

		protected named_type_reference _name;
		protected template_param_list _params_list;

		///<summary>
		///
		///</summary>
		public named_type_reference name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public template_param_list params_list
		{
			get
			{
				return _params_list;
			}
			set
			{
				_params_list=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class int64_const : const_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public int64_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public int64_const(Int64 _val)
		{
			this._val=_val;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public int64_const(Int64 _val,SourceContext sc)
		{
			this._val=_val;
			source_context = sc;
		}

		protected Int64 _val;

		///<summary>
		///
		///</summary>
		public Int64 val
		{
			get
			{
				return _val;
			}
			set
			{
				_val=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class uint64_const : const_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public uint64_const()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public uint64_const(UInt64 _val)
		{
			this._val=_val;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public uint64_const(UInt64 _val,SourceContext sc)
		{
			this._val=_val;
			source_context = sc;
		}

		protected UInt64 _val;

		///<summary>
		///
		///</summary>
		public UInt64 val
		{
			get
			{
				return _val;
			}
			set
			{
				_val=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class new_expr : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public new_expr()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public new_expr(type_definition _type,expression_list _params_list,bool _new_array,array_const _array_init_expr)
		{
			this._type=_type;
			this._params_list=_params_list;
			this._new_array=_new_array;
			this._array_init_expr=_array_init_expr;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public new_expr(type_definition _type,expression_list _params_list,bool _new_array,array_const _array_init_expr,SourceContext sc)
		{
			this._type=_type;
			this._params_list=_params_list;
			this._new_array=_new_array;
			this._array_init_expr=_array_init_expr;
			source_context = sc;
		}

		protected type_definition _type;
		protected expression_list _params_list;
		protected bool _new_array;
		protected array_const _array_init_expr;

		///<summary>
		///
		///</summary>
		public type_definition type
		{
			get
			{
				return _type;
			}
			set
			{
				_type=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression_list params_list
		{
			get
			{
				return _params_list;
			}
			set
			{
				_params_list=value;
			}
		}

		///<summary>
		///
		///</summary>
		public bool new_array
		{
			get
			{
				return _new_array;
			}
			set
			{
				_new_array=value;
			}
		}

		///<summary>
		///
		///</summary>
		public array_const array_init_expr
		{
			get
			{
				return _array_init_expr;
			}
			set
			{
				_array_init_expr=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class type_definition_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public type_definition_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition_list(List<type_definition> _defs)
		{
			this._defs=_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition_list(List<type_definition> _defs,SourceContext sc)
		{
			this._defs=_defs;
			source_context = sc;
		}

		protected List<type_definition> _defs=new List<type_definition>();

		///<summary>
		///
		///</summary>
		public List<type_definition> defs
		{
			get
			{
				return _defs;
			}
			set
			{
				_defs=value;
			}
		}


		public type_definition_list(type_definition _type_definition, SourceContext sc)
		{
		    Add(_type_definition,sc);
		}
		public type_definition_list Add(type_definition _type_definition)
		{
		    defs.Add(_type_definition);
		    return this;
		}
		public type_definition_list Add(type_definition _type_definition, SourceContext sc)
		{
		    defs.Add(_type_definition);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class where_definition : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public where_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public where_definition(ident_list _names,type_definition_list _types)
		{
			this._names=_names;
			this._types=_types;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public where_definition(ident_list _names,type_definition_list _types,SourceContext sc)
		{
			this._names=_names;
			this._types=_types;
			source_context = sc;
		}

		protected ident_list _names;
		protected type_definition_list _types;

		///<summary>
		///
		///</summary>
		public ident_list names
		{
			get
			{
				return _names;
			}
			set
			{
				_names=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition_list types
		{
			get
			{
				return _types;
			}
			set
			{
				_types=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class where_definition_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public where_definition_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public where_definition_list(List<where_definition> _defs)
		{
			this._defs=_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public where_definition_list(List<where_definition> _defs,SourceContext sc)
		{
			this._defs=_defs;
			source_context = sc;
		}

		protected List<where_definition> _defs=new List<where_definition>();

		///<summary>
		///
		///</summary>
		public List<where_definition> defs
		{
			get
			{
				return _defs;
			}
			set
			{
				_defs=value;
			}
		}


		public where_definition_list(where_definition _where_definition, SourceContext sc)
		{
		    Add(_where_definition,sc);
		}
		public where_definition_list Add(where_definition _where_definition)
		{
		    defs.Add(_where_definition);
		    return this;
		}
		public where_definition_list Add(where_definition _where_definition, SourceContext sc)
		{
		    defs.Add(_where_definition);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class sizeof_operator : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public sizeof_operator()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public sizeof_operator(type_definition _type_def,expression _expr)
		{
			this._type_def=_type_def;
			this._expr=_expr;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public sizeof_operator(type_definition _type_def,expression _expr,SourceContext sc)
		{
			this._type_def=_type_def;
			this._expr=_expr;
			source_context = sc;
		}

		protected type_definition _type_def;
		protected expression _expr;

		///<summary>
		///
		///</summary>
		public type_definition type_def
		{
			get
			{
				return _type_def;
			}
			set
			{
				_type_def=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class typeof_operator : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public typeof_operator()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typeof_operator(named_type_reference _type_name)
		{
			this._type_name=_type_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public typeof_operator(named_type_reference _type_name,SourceContext sc)
		{
			this._type_name=_type_name;
			source_context = sc;
		}

		protected named_type_reference _type_name;

		///<summary>
		///
		///</summary>
		public named_type_reference type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class compiler_directive : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public compiler_directive()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive(token_info _Name,token_info _Directive)
		{
			this._Name=_Name;
			this._Directive=_Directive;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive(token_info _Name,token_info _Directive,SourceContext sc)
		{
			this._Name=_Name;
			this._Directive=_Directive;
			source_context = sc;
		}

		protected token_info _Name;
		protected token_info _Directive;

		///<summary>
		///
		///</summary>
		public token_info Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public token_info Directive
		{
			get
			{
				return _Directive;
			}
			set
			{
				_Directive=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class operator_name_ident : ident
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public operator_name_ident()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public operator_name_ident(Operators _operator_type)
		{
			this._operator_type=_operator_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public operator_name_ident(Operators _operator_type,SourceContext sc)
		{
			this._operator_type=_operator_type;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public operator_name_ident(string _name,Operators _operator_type)
		{
			this._name=_name;
			this._operator_type=_operator_type;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public operator_name_ident(string _name,Operators _operator_type,SourceContext sc)
		{
			this._name=_name;
			this._operator_type=_operator_type;
			source_context = sc;
		}

		protected Operators _operator_type;

		///<summary>
		///
		///</summary>
		public Operators operator_type
		{
			get
			{
				return _operator_type;
			}
			set
			{
				_operator_type=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class var_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public var_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public var_statement(var_def_statement _var_def)
		{
			this._var_def=_var_def;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public var_statement(var_def_statement _var_def,SourceContext sc)
		{
			this._var_def=_var_def;
			source_context = sc;
		}

		protected var_def_statement _var_def;

		///<summary>
		///
		///</summary>
		public var_def_statement var_def
		{
			get
			{
				return _var_def;
			}
			set
			{
				_var_def=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class question_colon_expression : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public question_colon_expression()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public question_colon_expression(expression _condition,expression _ret_if_true,expression _ret_if_false)
		{
			this._condition=_condition;
			this._ret_if_true=_ret_if_true;
			this._ret_if_false=_ret_if_false;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public question_colon_expression(expression _condition,expression _ret_if_true,expression _ret_if_false,SourceContext sc)
		{
			this._condition=_condition;
			this._ret_if_true=_ret_if_true;
			this._ret_if_false=_ret_if_false;
			source_context = sc;
		}

		protected expression _condition;
		protected expression _ret_if_true;
		protected expression _ret_if_false;

		///<summary>
		///
		///</summary>
		public expression condition
		{
			get
			{
				return _condition;
			}
			set
			{
				_condition=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression ret_if_true
		{
			get
			{
				return _ret_if_true;
			}
			set
			{
				_ret_if_true=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression ret_if_false
		{
			get
			{
				return _ret_if_false;
			}
			set
			{
				_ret_if_false=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class expression_as_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public expression_as_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public expression_as_statement(expression _expr)
		{
			this._expr=_expr;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public expression_as_statement(expression _expr,SourceContext sc)
		{
			this._expr=_expr;
			source_context = sc;
		}

		protected expression _expr;

		///<summary>
		///
		///</summary>
		public expression expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class c_scalar_type : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public c_scalar_type()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_scalar_type(c_scalar_type_name _scalar_name,c_scalar_sign _sign)
		{
			this._scalar_name=_scalar_name;
			this._sign=_sign;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_scalar_type(c_scalar_type_name _scalar_name,c_scalar_sign _sign,SourceContext sc)
		{
			this._scalar_name=_scalar_name;
			this._sign=_sign;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_scalar_type(type_definition_attr_list _attr_list,c_scalar_type_name _scalar_name,c_scalar_sign _sign)
		{
			this._attr_list=_attr_list;
			this._scalar_name=_scalar_name;
			this._sign=_sign;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_scalar_type(type_definition_attr_list _attr_list,c_scalar_type_name _scalar_name,c_scalar_sign _sign,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._scalar_name=_scalar_name;
			this._sign=_sign;
			source_context = sc;
		}

		protected c_scalar_type_name _scalar_name;
		protected c_scalar_sign _sign;

		///<summary>
		///
		///</summary>
		public c_scalar_type_name scalar_name
		{
			get
			{
				return _scalar_name;
			}
			set
			{
				_scalar_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public c_scalar_sign sign
		{
			get
			{
				return _sign;
			}
			set
			{
				_sign=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class c_module : compilation_unit
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public c_module()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_module(declarations _defs,uses_list _used_units)
		{
			this._defs=_defs;
			this._used_units=_used_units;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_module(declarations _defs,uses_list _used_units,SourceContext sc)
		{
			this._defs=_defs;
			this._used_units=_used_units;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_module(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,declarations _defs,uses_list _used_units)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			this._defs=_defs;
			this._used_units=_used_units;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_module(string _file_name,List<compiler_directive> _compiler_directives,LanguageId _Language,declarations _defs,uses_list _used_units,SourceContext sc)
		{
			this._file_name=_file_name;
			this._compiler_directives=_compiler_directives;
			this._Language=_Language;
			this._defs=_defs;
			this._used_units=_used_units;
			source_context = sc;
		}

		protected declarations _defs;
		protected uses_list _used_units;

		///<summary>
		///
		///</summary>
		public declarations defs
		{
			get
			{
				return _defs;
			}
			set
			{
				_defs=value;
			}
		}

		///<summary>
		///
		///</summary>
		public uses_list used_units
		{
			get
			{
				return _used_units;
			}
			set
			{
				_used_units=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class declarations_as_statement : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public declarations_as_statement()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declarations_as_statement(declarations _defs)
		{
			this._defs=_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declarations_as_statement(declarations _defs,SourceContext sc)
		{
			this._defs=_defs;
			source_context = sc;
		}

		protected declarations _defs;

		///<summary>
		///
		///</summary>
		public declarations defs
		{
			get
			{
				return _defs;
			}
			set
			{
				_defs=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class array_size : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public array_size()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_size(expression _max_value)
		{
			this._max_value=_max_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_size(expression _max_value,SourceContext sc)
		{
			this._max_value=_max_value;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_size(type_definition_attr_list _attr_list,expression _max_value)
		{
			this._attr_list=_attr_list;
			this._max_value=_max_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public array_size(type_definition_attr_list _attr_list,expression _max_value,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._max_value=_max_value;
			source_context = sc;
		}

		protected expression _max_value;

		///<summary>
		///
		///</summary>
		public expression max_value
		{
			get
			{
				return _max_value;
			}
			set
			{
				_max_value=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class enumerator : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public enumerator()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public enumerator(ident _name,expression _value)
		{
			this._name=_name;
			this._value=_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public enumerator(ident _name,expression _value,SourceContext sc)
		{
			this._name=_name;
			this._value=_value;
			source_context = sc;
		}

		protected ident _name;
		protected expression _value;

		///<summary>
		///
		///</summary>
		public ident name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression value
		{
			get
			{
				return _value;
			}
			set
			{
				_value=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class enumerator_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public enumerator_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public enumerator_list(List<enumerator> _enumerators)
		{
			this._enumerators=_enumerators;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public enumerator_list(List<enumerator> _enumerators,SourceContext sc)
		{
			this._enumerators=_enumerators;
			source_context = sc;
		}

		protected List<enumerator> _enumerators=new List<enumerator>();

		///<summary>
		///
		///</summary>
		public List<enumerator> enumerators
		{
			get
			{
				return _enumerators;
			}
			set
			{
				_enumerators=value;
			}
		}


		public enumerator_list(enumerator _enumerator, SourceContext sc)
		{
		    Add(_enumerator,sc);
		}
		public enumerator_list Add(enumerator _enumerator)
		{
		    enumerators.Add(_enumerator);
		    return this;
		}
		public enumerator_list Add(enumerator _enumerator, SourceContext sc)
		{
		    enumerators.Add(_enumerator);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class c_for_cycle : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public c_for_cycle()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_for_cycle(statement _expr1,expression _expr2,expression _expr3,statement _stmt)
		{
			this._expr1=_expr1;
			this._expr2=_expr2;
			this._expr3=_expr3;
			this._stmt=_stmt;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public c_for_cycle(statement _expr1,expression _expr2,expression _expr3,statement _stmt,SourceContext sc)
		{
			this._expr1=_expr1;
			this._expr2=_expr2;
			this._expr3=_expr3;
			this._stmt=_stmt;
			source_context = sc;
		}

		protected statement _expr1;
		protected expression _expr2;
		protected expression _expr3;
		protected statement _stmt;

		///<summary>
		///
		///</summary>
		public statement expr1
		{
			get
			{
				return _expr1;
			}
			set
			{
				_expr1=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression expr2
		{
			get
			{
				return _expr2;
			}
			set
			{
				_expr2=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression expr3
		{
			get
			{
				return _expr3;
			}
			set
			{
				_expr3=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement stmt
		{
			get
			{
				return _stmt;
			}
			set
			{
				_stmt=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class switch_stmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public switch_stmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public switch_stmt(expression _condition,statement _stmt,SwitchPartType _Part)
		{
			this._condition=_condition;
			this._stmt=_stmt;
			this._Part=_Part;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public switch_stmt(expression _condition,statement _stmt,SwitchPartType _Part,SourceContext sc)
		{
			this._condition=_condition;
			this._stmt=_stmt;
			this._Part=_Part;
			source_context = sc;
		}

		protected expression _condition;
		protected statement _stmt;
		protected SwitchPartType _Part;

		///<summary>
		///
		///</summary>
		public expression condition
		{
			get
			{
				return _condition;
			}
			set
			{
				_condition=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement stmt
		{
			get
			{
				return _stmt;
			}
			set
			{
				_stmt=value;
			}
		}

		///<summary>
		///
		///</summary>
		public SwitchPartType Part
		{
			get
			{
				return _Part;
			}
			set
			{
				_Part=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class type_definition_attr_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public type_definition_attr_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition_attr_list(List<type_definition_attr> _attributes)
		{
			this._attributes=_attributes;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition_attr_list(List<type_definition_attr> _attributes,SourceContext sc)
		{
			this._attributes=_attributes;
			source_context = sc;
		}

		protected List<type_definition_attr> _attributes=new List<type_definition_attr>();

		///<summary>
		///
		///</summary>
		public List<type_definition_attr> attributes
		{
			get
			{
				return _attributes;
			}
			set
			{
				_attributes=value;
			}
		}


		public type_definition_attr_list(type_definition_attr _type_definition_attr, SourceContext sc)
		{
		    Add(_type_definition_attr,sc);
		}
		public type_definition_attr_list Add(type_definition_attr _type_definition_attr)
		{
		    attributes.Add(_type_definition_attr);
		    return this;
		}
		public type_definition_attr_list Add(type_definition_attr _type_definition_attr, SourceContext sc)
		{
		    attributes.Add(_type_definition_attr);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class type_definition_attr : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public type_definition_attr()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition_attr(definition_attribute _attr)
		{
			this._attr=_attr;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition_attr(definition_attribute _attr,SourceContext sc)
		{
			this._attr=_attr;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition_attr(type_definition_attr_list _attr_list,definition_attribute _attr)
		{
			this._attr_list=_attr_list;
			this._attr=_attr;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public type_definition_attr(type_definition_attr_list _attr_list,definition_attribute _attr,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._attr=_attr;
			source_context = sc;
		}

		protected definition_attribute _attr;

		///<summary>
		///
		///</summary>
		public definition_attribute attr
		{
			get
			{
				return _attr;
			}
			set
			{
				_attr=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class lock_stmt : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public lock_stmt()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public lock_stmt(expression _lock_object,statement _stmt)
		{
			this._lock_object=_lock_object;
			this._stmt=_stmt;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public lock_stmt(expression _lock_object,statement _stmt,SourceContext sc)
		{
			this._lock_object=_lock_object;
			this._stmt=_stmt;
			source_context = sc;
		}

		protected expression _lock_object;
		protected statement _stmt;

		///<summary>
		///
		///</summary>
		public expression lock_object
		{
			get
			{
				return _lock_object;
			}
			set
			{
				_lock_object=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement stmt
		{
			get
			{
				return _stmt;
			}
			set
			{
				_stmt=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class compiler_directive_list : compiler_directive
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public compiler_directive_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive_list(List<compiler_directive> _directives)
		{
			this._directives=_directives;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive_list(List<compiler_directive> _directives,SourceContext sc)
		{
			this._directives=_directives;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive_list(token_info _Name,token_info _Directive,List<compiler_directive> _directives)
		{
			this._Name=_Name;
			this._Directive=_Directive;
			this._directives=_directives;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive_list(token_info _Name,token_info _Directive,List<compiler_directive> _directives,SourceContext sc)
		{
			this._Name=_Name;
			this._Directive=_Directive;
			this._directives=_directives;
			source_context = sc;
		}

		protected List<compiler_directive> _directives=new List<compiler_directive>();

		///<summary>
		///
		///</summary>
		public List<compiler_directive> directives
		{
			get
			{
				return _directives;
			}
			set
			{
				_directives=value;
			}
		}


		public compiler_directive_list(compiler_directive _compiler_directive, SourceContext sc)
		{
		    Add(_compiler_directive,sc);
		}
		public compiler_directive_list Add(compiler_directive _compiler_directive)
		{
		    directives.Add(_compiler_directive);
		    return this;
		}
		public compiler_directive_list Add(compiler_directive _compiler_directive, SourceContext sc)
		{
		    directives.Add(_compiler_directive);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class compiler_directive_if : compiler_directive
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public compiler_directive_if()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive_if(compiler_directive _if_part,compiler_directive _elseif_part)
		{
			this._if_part=_if_part;
			this._elseif_part=_elseif_part;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive_if(compiler_directive _if_part,compiler_directive _elseif_part,SourceContext sc)
		{
			this._if_part=_if_part;
			this._elseif_part=_elseif_part;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive_if(token_info _Name,token_info _Directive,compiler_directive _if_part,compiler_directive _elseif_part)
		{
			this._Name=_Name;
			this._Directive=_Directive;
			this._if_part=_if_part;
			this._elseif_part=_elseif_part;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public compiler_directive_if(token_info _Name,token_info _Directive,compiler_directive _if_part,compiler_directive _elseif_part,SourceContext sc)
		{
			this._Name=_Name;
			this._Directive=_Directive;
			this._if_part=_if_part;
			this._elseif_part=_elseif_part;
			source_context = sc;
		}

		protected compiler_directive _if_part;
		protected compiler_directive _elseif_part;

		///<summary>
		///
		///</summary>
		public compiler_directive if_part
		{
			get
			{
				return _if_part;
			}
			set
			{
				_if_part=value;
			}
		}

		///<summary>
		///
		///</summary>
		public compiler_directive elseif_part
		{
			get
			{
				return _elseif_part;
			}
			set
			{
				_elseif_part=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class documentation_comment_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public documentation_comment_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public documentation_comment_list(List<documentation_comment_section> _sections)
		{
			this._sections=_sections;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public documentation_comment_list(List<documentation_comment_section> _sections,SourceContext sc)
		{
			this._sections=_sections;
			source_context = sc;
		}

		protected List<documentation_comment_section> _sections=new List<documentation_comment_section>();

		///<summary>
		///
		///</summary>
		public List<documentation_comment_section> sections
		{
			get
			{
				return _sections;
			}
			set
			{
				_sections=value;
			}
		}


		public documentation_comment_list(documentation_comment_section _documentation_comment_section, SourceContext sc)
		{
		    Add(_documentation_comment_section,sc);
		}
		public documentation_comment_list Add(documentation_comment_section _documentation_comment_section)
		{
		    sections.Add(_documentation_comment_section);
		    return this;
		}
		public documentation_comment_list Add(documentation_comment_section _documentation_comment_section, SourceContext sc)
		{
		    sections.Add(_documentation_comment_section);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class documentation_comment_tag : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public documentation_comment_tag()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public documentation_comment_tag(string _name,List<documentation_comment_tag_param> _parameters,string _text)
		{
			this._name=_name;
			this._parameters=_parameters;
			this._text=_text;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public documentation_comment_tag(string _name,List<documentation_comment_tag_param> _parameters,string _text,SourceContext sc)
		{
			this._name=_name;
			this._parameters=_parameters;
			this._text=_text;
			source_context = sc;
		}

		protected string _name;
		protected List<documentation_comment_tag_param> _parameters=new List<documentation_comment_tag_param>();
		protected string _text;

		///<summary>
		///
		///</summary>
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public List<documentation_comment_tag_param> parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters=value;
			}
		}

		///<summary>
		///
		///</summary>
		public string text
		{
			get
			{
				return _text;
			}
			set
			{
				_text=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class documentation_comment_tag_param : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public documentation_comment_tag_param()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public documentation_comment_tag_param(string _name,string _value)
		{
			this._name=_name;
			this._value=_value;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public documentation_comment_tag_param(string _name,string _value,SourceContext sc)
		{
			this._name=_name;
			this._value=_value;
			source_context = sc;
		}

		protected string _name;
		protected string _value;

		///<summary>
		///
		///</summary>
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public string value
		{
			get
			{
				return _value;
			}
			set
			{
				_value=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class documentation_comment_section : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public documentation_comment_section()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public documentation_comment_section(List<documentation_comment_tag> _tags,string _text)
		{
			this._tags=_tags;
			this._text=_text;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public documentation_comment_section(List<documentation_comment_tag> _tags,string _text,SourceContext sc)
		{
			this._tags=_tags;
			this._text=_text;
			source_context = sc;
		}

		protected List<documentation_comment_tag> _tags=new List<documentation_comment_tag>();
		protected string _text;

		///<summary>
		///
		///</summary>
		public List<documentation_comment_tag> tags
		{
			get
			{
				return _tags;
			}
			set
			{
				_tags=value;
			}
		}

		///<summary>
		///
		///</summary>
		public string text
		{
			get
			{
				return _text;
			}
			set
			{
				_text=value;
			}
		}


		public documentation_comment_section(documentation_comment_tag _documentation_comment_tag, SourceContext sc)
		{
		    Add(_documentation_comment_tag,sc);
		}
		public documentation_comment_section Add(documentation_comment_tag _documentation_comment_tag)
		{
		    tags.Add(_documentation_comment_tag);
		    return this;
		}
		public documentation_comment_section Add(documentation_comment_tag _documentation_comment_tag, SourceContext sc)
		{
		    tags.Add(_documentation_comment_tag);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class token_taginfo : token_info
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public token_taginfo()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public token_taginfo(object _tag)
		{
			this._tag=_tag;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public token_taginfo(object _tag,SourceContext sc)
		{
			this._tag=_tag;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public token_taginfo(string _text,object _tag)
		{
			this._text=_text;
			this._tag=_tag;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public token_taginfo(string _text,object _tag,SourceContext sc)
		{
			this._text=_text;
			this._tag=_tag;
			source_context = sc;
		}

		protected object _tag;

		///<summary>
		///
		///</summary>
		public object tag
		{
			get
			{
				return _tag;
			}
			set
			{
				_tag=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class declaration_specificator : type_definition
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public declaration_specificator()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declaration_specificator(DeclarationSpecificator _specificator,string _name)
		{
			this._specificator=_specificator;
			this._name=_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declaration_specificator(DeclarationSpecificator _specificator,string _name,SourceContext sc)
		{
			this._specificator=_specificator;
			this._name=_name;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declaration_specificator(type_definition_attr_list _attr_list,DeclarationSpecificator _specificator,string _name)
		{
			this._attr_list=_attr_list;
			this._specificator=_specificator;
			this._name=_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public declaration_specificator(type_definition_attr_list _attr_list,DeclarationSpecificator _specificator,string _name,SourceContext sc)
		{
			this._attr_list=_attr_list;
			this._specificator=_specificator;
			this._name=_name;
			source_context = sc;
		}

		protected DeclarationSpecificator _specificator;
		protected string _name;

		///<summary>
		///
		///</summary>
		public DeclarationSpecificator specificator
		{
			get
			{
				return _specificator;
			}
			set
			{
				_specificator=value;
			}
		}

		///<summary>
		///
		///</summary>
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class ident_with_templateparams : addressed_value_funcname
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public ident_with_templateparams()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ident_with_templateparams(addressed_value _name,template_param_list _template_params)
		{
			this._name=_name;
			this._template_params=_template_params;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public ident_with_templateparams(addressed_value _name,template_param_list _template_params,SourceContext sc)
		{
			this._name=_name;
			this._template_params=_template_params;
			source_context = sc;
		}

		protected addressed_value _name;
		protected template_param_list _template_params;

		///<summary>
		///
		///</summary>
		public addressed_value name
		{
			get
			{
				return _name;
			}
			set
			{
				_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public template_param_list template_params
		{
			get
			{
				return _template_params;
			}
			set
			{
				_template_params=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class template_type_name : ident
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public template_type_name()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_type_name(ident_list _template_args)
		{
			this._template_args=_template_args;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_type_name(ident_list _template_args,SourceContext sc)
		{
			this._template_args=_template_args;
			source_context = sc;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_type_name(string _name,ident_list _template_args)
		{
			this._name=_name;
			this._template_args=_template_args;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public template_type_name(string _name,ident_list _template_args,SourceContext sc)
		{
			this._name=_name;
			this._template_args=_template_args;
			source_context = sc;
		}

		protected ident_list _template_args;

		///<summary>
		///
		///</summary>
		public ident_list template_args
		{
			get
			{
				return _template_args;
			}
			set
			{
				_template_args=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class default_operator : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public default_operator()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public default_operator(named_type_reference _type_name)
		{
			this._type_name=_type_name;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public default_operator(named_type_reference _type_name,SourceContext sc)
		{
			this._type_name=_type_name;
			source_context = sc;
		}

		protected named_type_reference _type_name;

		///<summary>
		///
		///</summary>
		public named_type_reference type_name
		{
			get
			{
				return _type_name;
			}
			set
			{
				_type_name=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class bracket_expr : addressed_value
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public bracket_expr()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public bracket_expr(expression _expr)
		{
			this._expr=_expr;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public bracket_expr(expression _expr,SourceContext sc)
		{
			this._expr=_expr;
			source_context = sc;
		}

		protected expression _expr;

		///<summary>
		///
		///</summary>
		public expression expr
		{
			get
			{
				return _expr;
			}
			set
			{
				_expr=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class attribute : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public attribute()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public attribute(ident _qualifier,named_type_reference _type,expression_list _arguments)
		{
			this._qualifier=_qualifier;
			this._type=_type;
			this._arguments=_arguments;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public attribute(ident _qualifier,named_type_reference _type,expression_list _arguments,SourceContext sc)
		{
			this._qualifier=_qualifier;
			this._type=_type;
			this._arguments=_arguments;
			source_context = sc;
		}

		protected ident _qualifier;
		protected named_type_reference _type;
		protected expression_list _arguments;

		///<summary>
		///
		///</summary>
		public ident qualifier
		{
			get
			{
				return _qualifier;
			}
			set
			{
				_qualifier=value;
			}
		}

		///<summary>
		///
		///</summary>
		public named_type_reference type
		{
			get
			{
				return _type;
			}
			set
			{
				_type=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression_list arguments
		{
			get
			{
				return _arguments;
			}
			set
			{
				_arguments=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class simple_attribute_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public simple_attribute_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public simple_attribute_list(List<attribute> _attributes)
		{
			this._attributes=_attributes;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public simple_attribute_list(List<attribute> _attributes,SourceContext sc)
		{
			this._attributes=_attributes;
			source_context = sc;
		}

		protected List<attribute> _attributes=new List<attribute>();

		///<summary>
		///
		///</summary>
		public List<attribute> attributes
		{
			get
			{
				return _attributes;
			}
			set
			{
				_attributes=value;
			}
		}


		public simple_attribute_list(attribute _attribute, SourceContext sc)
		{
		    Add(_attribute,sc);
		}
		public simple_attribute_list Add(attribute _attribute)
		{
		    attributes.Add(_attribute);
		    return this;
		}
		public simple_attribute_list Add(attribute _attribute, SourceContext sc)
		{
		    attributes.Add(_attribute);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class attribute_list : syntax_tree_node
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public attribute_list()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public attribute_list(List<simple_attribute_list> _attributes)
		{
			this._attributes=_attributes;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public attribute_list(List<simple_attribute_list> _attributes,SourceContext sc)
		{
			this._attributes=_attributes;
			source_context = sc;
		}

		protected List<simple_attribute_list> _attributes=new List<simple_attribute_list>();

		///<summary>
		///
		///</summary>
		public List<simple_attribute_list> attributes
		{
			get
			{
				return _attributes;
			}
			set
			{
				_attributes=value;
			}
		}


		public attribute_list(simple_attribute_list _simple_attribute_list, SourceContext sc)
		{
		    Add(_simple_attribute_list,sc);
		}
		public attribute_list Add(simple_attribute_list _simple_attribute_list)
		{
		    attributes.Add(_simple_attribute_list);
		    return this;
		}
		public attribute_list Add(simple_attribute_list _simple_attribute_list, SourceContext sc)
		{
		    attributes.Add(_simple_attribute_list);
		    source_context = sc;
		    return this;
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class function_lambda_definition : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public function_lambda_definition()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public function_lambda_definition(ident_list _ident_list,type_definition _return_type,formal_parameters _formal_parameters,statement _proc_body,procedure_definition _proc_definition,expression_list _parameters,string _lambda_name,List<declaration> _defs)
		{
			this._ident_list=_ident_list;
			this._return_type=_return_type;
			this._formal_parameters=_formal_parameters;
			this._proc_body=_proc_body;
			this._proc_definition=_proc_definition;
			this._parameters=_parameters;
			this._lambda_name=_lambda_name;
			this._defs=_defs;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public function_lambda_definition(ident_list _ident_list,type_definition _return_type,formal_parameters _formal_parameters,statement _proc_body,procedure_definition _proc_definition,expression_list _parameters,string _lambda_name,List<declaration> _defs,SourceContext sc)
		{
			this._ident_list=_ident_list;
			this._return_type=_return_type;
			this._formal_parameters=_formal_parameters;
			this._proc_body=_proc_body;
			this._proc_definition=_proc_definition;
			this._parameters=_parameters;
			this._lambda_name=_lambda_name;
			this._defs=_defs;
			source_context = sc;
		}

		protected ident_list _ident_list;
		protected type_definition _return_type;
		protected formal_parameters _formal_parameters;
		protected statement _proc_body;
		protected procedure_definition _proc_definition;
		protected expression_list _parameters;
		protected string _lambda_name;
		protected List<declaration> _defs;

		///<summary>
		///
		///</summary>
		public ident_list ident_list
		{
			get
			{
				return _ident_list;
			}
			set
			{
				_ident_list=value;
			}
		}

		///<summary>
		///
		///</summary>
		public type_definition return_type
		{
			get
			{
				return _return_type;
			}
			set
			{
				_return_type=value;
			}
		}

		///<summary>
		///
		///</summary>
		public formal_parameters formal_parameters
		{
			get
			{
				return _formal_parameters;
			}
			set
			{
				_formal_parameters=value;
			}
		}

		///<summary>
		///
		///</summary>
		public statement proc_body
		{
			get
			{
				return _proc_body;
			}
			set
			{
				_proc_body=value;
			}
		}

		///<summary>
		///
		///</summary>
		public procedure_definition proc_definition
		{
			get
			{
				return _proc_definition;
			}
			set
			{
				_proc_definition=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression_list parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters=value;
			}
		}

		///<summary>
		///
		///</summary>
		public string lambda_name
		{
			get
			{
				return _lambda_name;
			}
			set
			{
				_lambda_name=value;
			}
		}

		///<summary>
		///
		///</summary>
		public List<declaration> defs
		{
			get
			{
				return _defs;
			}
			set
			{
				_defs=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///
	///</summary>
	[Serializable]
	public class function_lambda_call : expression
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public function_lambda_call()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public function_lambda_call(function_lambda_definition _f_lambda_def,expression_list _parameters)
		{
			this._f_lambda_def=_f_lambda_def;
			this._parameters=_parameters;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public function_lambda_call(function_lambda_definition _f_lambda_def,expression_list _parameters,SourceContext sc)
		{
			this._f_lambda_def=_f_lambda_def;
			this._parameters=_parameters;
			source_context = sc;
		}

		protected function_lambda_definition _f_lambda_def;
		protected expression_list _parameters;

		///<summary>
		///
		///</summary>
		public function_lambda_definition f_lambda_def
		{
			get
			{
				return _f_lambda_def;
			}
			set
			{
				_f_lambda_def=value;
			}
		}

		///<summary>
		///
		///</summary>
		public expression_list parameters
		{
			get
			{
				return _parameters;
			}
			set
			{
				_parameters=value;
			}
		}


		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}


	///<summary>
	///Узел для семантических проверок на этапе построения семантического дерева. Сделан для узлов синтаксического дерева, реализующих синтаксический сахар. Может, видимо, использоваться и для обычных семантических проверок
	///</summary>
	[Serializable]
	public class semantic_check : statement
	{

		///<summary>
		///Конструктор без параметров.
		///</summary>
		public semantic_check()
		{

		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public semantic_check(string _CheckName,List<syntax_tree_node> _param,int _fictive)
		{
			this._CheckName=_CheckName;
			this._param=_param;
			this._fictive=_fictive;
		}

		///<summary>
		///Конструктор с параметрами.
		///</summary>
		public semantic_check(string _CheckName,List<syntax_tree_node> _param,int _fictive,SourceContext sc)
		{
			this._CheckName=_CheckName;
			this._param=_param;
			this._fictive=_fictive;
			source_context = sc;
		}

		protected string _CheckName;
		protected List<syntax_tree_node> _param=new List<syntax_tree_node>();
		protected int _fictive;

		///<summary>
		///Тип проверки. Пока строковый. Например, является ли выражение целым
		///</summary>
		public string CheckName
		{
			get
			{
				return _CheckName;
			}
			set
			{
				_CheckName=value;
			}
		}

		///<summary>
		///Параметры - синтаксические узлы для проверки
		///</summary>
		public List<syntax_tree_node> param
		{
			get
			{
				return _param;
			}
			set
			{
				_param=value;
			}
		}

		///<summary>
		///Фиктивное поле - чтобы можно было вручную написать конструктор с params
		///</summary>
		public int fictive
		{
			get
			{
				return _fictive;
			}
			set
			{
				_fictive=value;
			}
		}


		public semantic_check(string name, params syntax_tree_node[] pars)
		{
		  CheckName = name;
		  param.AddRange(pars);
		}

		///<summary>
		///Метод для обхода дерева посетителем
		///</summary>
		///<param name="visitor">Объект-посетитель.</param>
		///<returns>Return value is void</returns>
		public override void visit(IVisitor visitor)
		{
			visitor.visit(this);
		}

	}



}

