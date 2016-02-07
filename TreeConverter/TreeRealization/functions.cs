// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeRealization
{
    public enum function_compare { non_comparable, less, greater };

    public class function_intersection_node
    {
        private function_node _left;
        private function_node _right;

        private function_compare _function_relation;

        public function_compare function_relation
        {
            get
            {
                return _function_relation;
            }
            set
            {
                _function_relation = value;
            }
        }

        public function_node left
        {
            get
            {
                return _left;
            }
        }

        public function_node right
        {
            get
            {
                return _right;
            }
        }

        public function_intersection_node(function_node left, function_node right)
        {
            _left = left;
            _right = right;
        }
    }

    /// <summary>
    /// Делегат, который вызывается при создании узла вызова метода. Если результат не null, то вместо узла вызова метода в дерево попадает возвращаемое этим длегатом значение. Таким образом он позволяет вычислять значения функции во время компиляции.
    /// </summary>
    /// <param name="call_location">Место вызова метода.</param>
    /// <param name="parameters">Список параметровю</param>
    /// <returns>Если значение функции с этими параметрами может быть вычислено во время компиляции. null иначе.</returns>
    public delegate expression_node compile_time_executor(location call_location,params expression_node[] parameters);

    /// <summary>
    /// Базовый класс для описания функций.
    /// </summary>
	[Serializable]
	public abstract class function_node : definition_node, SemanticTree.IFunctionNode
	{
        private parameter_list _parameters = new parameter_list();

		private type_node _ret_type;

        private compile_time_executor _compile_time_executor;

        private bool _is_operator = false;

        public bool IsOperator
        {
            get { return _is_operator; }
            set { _is_operator = value; }
        }

        private System.Collections.Generic.Dictionary<function_node, function_intersection_node> _func_intersections =
            new System.Collections.Generic.Dictionary<function_node, function_intersection_node>();

        private void add_intersection(function_node _func,function_intersection_node _func_intersec)
        {
            _func_intersections.Add(_func, _func_intersec);
        }

        private function_intersection_node get_intersecion(function_node _func)
        {
            function_intersection_node _fint = null;
            bool ret = _func_intersections.TryGetValue(_func, out _fint);
            if (!ret)
            {
                return null;
            }
            return _fint;
        }

        public virtual bool is_extension_method
        {
            get
            {
                return false;
            }
        }

        public virtual SemanticTree.polymorphic_state polymorphic_state
        {
            get
            {
                return PascalABCCompiler.SemanticTree.polymorphic_state.ps_common;
            }
            set
            {
            }
        }
        public virtual SemanticTree.field_access_level field_access_level
        {
            get
            {
                return SemanticTree.field_access_level.fal_public;
            }
            set
            {
            }
        }

        public virtual bool is_generic_function
        {
            get
            {
                return false;
            }
        }

        public virtual function_node original_function
        {
            get
            {
                return null;
            }
        }

        public virtual bool is_generic_function_instance
        {
            get
            {
                return false;
            }
        }

        public virtual int num_of_default_parameters
        {
            get
            {
                return 0;
            }
        }

        //ограничения на generic-параметры
        public virtual List<generic_parameter_eliminations> parameters_eliminations
        {
            get
            {
                return null;
            }
        }

        public virtual List<type_node> get_generic_params_list()
        {
            return null;
        }

        public virtual bool is_final
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public virtual int generic_parameters_count
        {
            get
            {
                return 0;
            }
        }

        public static void add_function_intersection(function_node left, function_node right,
            function_intersection_node intersec)
        {
#if DEBUG
            function_intersection_node func_int_left = left.get_intersecion(right);
            if (func_int_left != null)
            {
                throw new TreeConverter.CompilerInternalError("Duplicate function intersection");
            }
#endif
            left.add_intersection(right, intersec);
        }

        public static function_intersection_node get_function_intersection(function_node left, function_node right)
        {
            return left.get_intersecion(right);
        }

        public static function_compare compare_functions(function_node left,function_node right)
        {
            function_intersection_node func_int_left = left.get_intersecion(right);
            function_intersection_node func_int_right = right.get_intersecion(left);
            if ((func_int_left == null) && (func_int_right == null))
            {
                return function_compare.non_comparable;
            }
            if ((func_int_left != null) && (func_int_right == null))
            {
                return func_int_left.function_relation;
            }
            if ((func_int_left == null) && (func_int_right != null))
            {
                if (func_int_right.function_relation == function_compare.non_comparable)
                {
                    return function_compare.non_comparable;
                }
                if (func_int_right.function_relation == function_compare.greater)
                {
                    return function_compare.less;
                }
                if (func_int_right.function_relation == function_compare.less)
                {
                    return function_compare.greater;
                }
            }
            if ((func_int_left != null) && (func_int_right != null))
            {
                if ((func_int_left.function_relation == function_compare.less) &&
                    (func_int_right.function_relation == function_compare.greater))
                {
                    return function_compare.less;
                }
                if ((func_int_left.function_relation == function_compare.greater) &&
                    (func_int_right.function_relation == function_compare.less))
                {
                    return function_compare.greater;
                }
                throw new TreeConverter.CompilerInternalError("Contradiction in function relations");
            }
            throw new TreeConverter.CompilerInternalError("Error in function relations");
        }

        /// <summary>
        /// Делегат, который вызывается при создании узла вызова метода. Если результат не null, то вместо узла вызова метода в дерево попадает возвращаемое этим длегатом значение. Таким образом он позволяет вычислять значения функции во время компиляции.
        /// </summary>
        public compile_time_executor compile_time_executor
        {
            get
            {
                return _compile_time_executor;
            }
            set
            {
                _compile_time_executor = value;
            }
        }

        public virtual function_node get_instance(List<type_node> param_types, bool stop_on_error, location loc)
        {
            return null;
        }

        public function_node()
        {
        }

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="ret_type">Тип возвращаемого значения функции.</param>
		public function_node(type_node ret_type)
		{
			_ret_type=ret_type;
		}

        /*
        public function_node(parameter_list parameters,type_node ret_type)
        {
            _ret_type = ret_type;
            _parameters.AddRange(parameters);
        }
        */
        /// <summary>
        /// Вид узла - базовый (basic), обычный (common) или экспортируемый (compiled).
        /// </summary>
		public abstract SemanticTree.node_kind node_kind
		{
			get;
		}

		/// <summary>
        /// Список формальных параметров функции.
		/// </summary>
		public parameter_list parameters
		{
			get
			{
				return _parameters;
			}
		}

        /// <summary>
        /// Тип возвращаемого значения функции.
        /// </summary>
		public type_node return_value_type
		{
			get
			{
				return _ret_type;
			}
            set
            {
                _ret_type = value;
            }
		}

        /// <summary>
        /// Имя функции.
        /// </summary>
		public abstract string name
		{
			get;
		}

        //TODO: Может is_overload оставить только в информации связанной с common_function_node.
		/*
        public abstract bool is_overload
		{
			get;
		}
        */

        /// <summary>
        /// Расположение узла - в пространстве имен, классе или функции.
        /// </summary>
		public abstract SemanticTree.node_location_kind node_location_kind
		{
			get;
		}

        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
		public override general_node_type general_node_type
		{
			get
			{
				return general_node_type.function_node;
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
        /// Тип возвращаеого значения.
        /// </summary>
		SemanticTree.ITypeNode SemanticTree.IFunctionNode.return_value_type
		{
			get
			{
				return _ret_type;
			}
		}

        /// <summary>
        /// Список формальных параметров функции.
        /// </summary>
		SemanticTree.IParameterNode[] SemanticTree.IFunctionNode.parameters
		{
			get
			{
				return (_parameters.ToArray());
			}
		}

	}

    [Serializable]
    public class convert_types_function_node : function_node
    {
        private bool _good = false;

        public convert_types_function_node(compile_time_executor ctn, bool good)
        {
            compile_time_executor = ctn;
            this._good = good;
        }

        public override SemanticTree.node_kind node_kind
        {
            get
            {
                return SemanticTree.node_kind.basic;
            }
        }

        public override string name
        {
            get
            {
                return ("$InternalName");
            }
        }

        public override SemanticTree.node_location_kind node_location_kind
        {
            get
            {
                return SemanticTree.node_location_kind.in_namespace_location;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.convert_types_function_node;
            }
        }

        public bool Good
        {
            get
            {
                return _good;
            }
        }
    }

    public enum special_operation_kind { none, assign };

    /// <summary>
    /// Класс, базовый, нигде не определенный метод, например операцию сложения двух целых чисел.
    /// </summary>
	[Serializable]
	public class basic_function_node : function_node, SemanticTree.IBasicFunctionNode
	{
        private string _name = null;

		private SemanticTree.basic_function_type _basic_function_type;

		private bool _overload;

        private special_operation_kind _operation_kind = special_operation_kind.none;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="bft">Тип базовой функции.</param>
        /// <param name="ret_type">Тип возвращаемого значения.</param>
        /// <param name="is_overload">Перегружена-ли функция.</param>
		public basic_function_node(SemanticTree.basic_function_type bft,type_node ret_type,
            bool is_overload) :
			base(ret_type)
		{
			_basic_function_type=bft;
            _overload = is_overload;
		}

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="bft">Тип базовой функции.</param>
        /// <param name="ret_type">Тип возвращаемого значения.</param>
        /// <param name="is_overload">Перегружена-ли функция.</param>
        /// <param name="name">Имя</param>
        public basic_function_node(SemanticTree.basic_function_type bft, type_node ret_type,
            bool is_overload, string name)
            :
            base(ret_type)
        {
            _basic_function_type = bft;
            _overload = is_overload;
            _name = name;
        }

        /// <summary>
        /// Какая именно это базовая функция.
        /// </summary>
		public SemanticTree.basic_function_type basic_function_type
		{
			get
			{
				return _basic_function_type;
			}
		}

        public special_operation_kind operation_kind
        {
            get
            {
                return _operation_kind;
            }
            set
            {
                _operation_kind = value;
            }
        }

        /// <summary>
        /// Имя функции.
        /// </summary>
		public override string name
		{
			get
			{
                if (_name != null)
                    return _name;
				return _basic_function_type.ToString();
			}
		}

        //TODO: Нужно ли is_overload для basic_function_node?
		public bool is_overload
		{
			get
			{
				return _overload;
			}
            /*
            set
            {
                _overload = value;
            }
            */
		}

        /// <summary>
        /// Расположение узла - в пространстве имен, классе или функции.
        /// </summary>
		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_namespace_location;
			}
		}

        /// <summary>
        /// Вид узла - обычный, базовый или откомпилированный.
        /// </summary>
		public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.basic;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.basic_function_node;
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
    /// Обычная, определенная пользователем функция.
    /// </summary>
	[Serializable]
	public abstract class common_function_node : function_node, SemanticTree.ICommonFunctionNode
	{
		protected string _name;

        protected readonly local_variable_list _var_defs = new local_variable_list();

        protected readonly List<label_node> _label_defs = new List<label_node>();

        protected readonly function_constant_definition_list _const_defs = new function_constant_definition_list();

        protected SemanticTree.SpecialFunctionKind specialFunctionKind = SemanticTree.SpecialFunctionKind.None;

        public SemanticTree.SpecialFunctionKind SpecialFunctionKind
        {
            get { return specialFunctionKind; }
            set { specialFunctionKind = value; }
        }

        //TODO: Возможно вынести в ассоциированную с функцией информацию.
		//Только указание - какая переменная возвтащается. Ссылка на эту-же переменную есть в _var_defs.
		private local_variable _return_variable;

        private readonly common_in_function_function_node_list _fnl = new common_in_function_function_node_list();
		private statement_node _function_code;

        //TODO: Возможно вынести оба этих поля в ассоциированную с функцией информацию.
		private bool _overload=false;
		private bool _is_forward=false;

        //TODO: Вынести в ассоциированную с функцией информацию.
		private statement_node_stack _cycles_stack=new statement_node_stack();

		private SymbolTable.Scope _scope;

		private location _loc;

        //TODO: Возможно вынести оба этих поля в ассоциированную с функцией информацию.
		private int _num_of_default_variables;
		private int _num_of_for_cycles;

        protected List<SemanticTree.ICommonTypeNode> _generic_params = null;

        public List<SemanticTree.ICommonTypeNode> generic_params
        {
            get { return _generic_params; }
            set { _generic_params = value; }
        }
        
        public override List<type_node> get_generic_params_list()
        {
            if (_generic_params == null)
            {
                return null;
            }
            List<type_node> rez = new List<type_node>(_generic_params.Count);
            foreach (type_node t in _generic_params)
            {
                rez.Add(t);
            }
            return rez;
        }

        public override int generic_parameters_count
        {
            get
            {
                if (_generic_params == null)
                {
                    return 0;
                }
                return _generic_params.Count;
            }
        }

        public override bool is_generic_function
        {
            get
            {
                return _generic_params != null;
            }
        }

        private List<generic_parameter_eliminations> _parameters_eliminations = null;

        public override List<generic_parameter_eliminations> parameters_eliminations
        {
            get
            {
                if (_parameters_eliminations != null)
                {
                    return _parameters_eliminations;
                }
                if (!this.is_generic_function)
                {
                    throw new TreeConverter.CompilerInternalError("Function is not generic.");
                }
                _parameters_eliminations = generic_parameter_eliminations.make_eliminations_common(generic_params);
                return _parameters_eliminations;
            }
        }

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="loc">Расположение функции.</param>
        /// <param name="scope">Пространство имен этой функции.</param>
        public common_function_node(string name, location loc,SymbolTable.Scope scope)
        {
            _name = name;
            _loc = loc;
            _scope = scope;
        }

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="ret_type">Тип возвращаемого значения функции.</param>
        /// <param name="loc">Расположение функции.</param>
        /// <param name="scope">Пространство имен этой функции.</param>
		public common_function_node(string name,type_node ret_type,location loc, SymbolTable.Scope scope) :
			base(ret_type)
		{
			_name=name;
            _loc = loc;
            _scope = scope;
		}

        /// <summary>
        /// Число циклов for в функции. Временно хранится здесь. Будет удалено.
        /// </summary>
		public int num_of_for_cycles
		{
			get
			{
				return _num_of_for_cycles;
			}
            set
            {
                _num_of_for_cycles = value;
            }
		}

        /*
        public int inc_num_of_for_cycles()
        {
            _num_of_for_cycles++;
        }
        */

        /// <summary>
        /// Стек циклов. Временно хранится здесь. Будет удален. Используется для реализации break и continue.
        /// </summary>
		public statement_node_stack cycles_stack
		{
			get
			{
				return _cycles_stack;
			}
		}

        /// <summary>
        /// Является ли это предописанием функции. Скорее всего будет удалено.
        /// </summary>
		public bool is_forward
		{
			get
			{
				return _is_forward;
			}
			set
			{
				_is_forward=value;
			}
		}

        /// <summary>
        /// Список констант определенных в функции.
        /// </summary>
        public function_constant_definition_list constants
        {
            get
            {
                return _const_defs;
            }
        }

        /// <summary>
        /// Поиск символа в функции. Осуществляет поиск и в объемлющих пространствах имен.
        /// </summary>
        /// <param name="name">Имя символа.</param>
        /// <returns>Информация о найленном символе. null, если ни чего не найдено.</returns>
        public PascalABCCompiler.TreeConverter.SymbolInfo find(string name, SymbolTable.Scope CurrentScope)
        {
            return _scope.Find(name, CurrentScope);//y
        }

        /// <summary>
        /// Поиск символа в функции. Осуществляет поиск только в пространстве имен функции. Используется для обнаружения повторного определения символа в пространстве имен.
        /// </summary>
        /// <param name="name">Имя символа.</param>
        /// <returns>Информация о найленном символе. null, если ни чего не найдено.</returns>
        //public PascalABCCompiler.TreeConverter.SymbolInfo find_only_in_namespace(string name)
        //{
         //   return _scope.FindOnlyInScopeAndBlocks(name);
        //}

        /// <summary>
        /// Пространство имен функции.
        /// </summary>
		public SymbolTable.Scope scope
		{
			get
			{
				return _scope;
			}
            //TODO: Может хоть это закрыть.
			set
			{
				_scope=value;
			}
		}

        /// <summary>
        /// Число переменных, имеющих параметры по умолчанию. Возможно будет удалено.
        /// </summary>
		public int num_of_default_variables
		{
			get
			{
				return _num_of_default_variables;
			}
			set
			{
				_num_of_default_variables=value;
			}
		}

        /// <summary>
        /// Расположение имени функции в программе.
        /// </summary>
		public location loc
		{
			get
			{
				return _loc;
			}
            set
            {
                _loc = value;
            }
		}

        /// <summary>
        /// Расположение имени функции в программе.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		public SemanticTree.ILocation Location
		{
			get
			{
				return _loc;
			}
		}

        /// <summary>
        /// Перегружена ли функция. Возможно будет удалено.
        /// </summary>
		public bool is_overload
		{
			get
			{
				return _overload;
			}
			set
			{
				_overload=value;
			}
		}


        /// <summary>
        /// Список переменных, определяемых в функции.
        /// </summary>
		public local_variable_list var_definition_nodes_list
		{
			get
			{
				return _var_defs;
			}
		}

        /// <summary>
        /// Список меток, определяемых в функции.
        /// </summary>
        public List<label_node> label_nodes_list
        {
            get
            {
                return _label_defs;
            }
        }

        /// <summary>
        /// Список вложенных функций.
        /// </summary>
		public common_in_function_function_node_list functions_nodes_list
		{
			get
			{
				return _fnl;
			}
		}

        /// <summary>
        /// Код функции.
        /// </summary>
		public statement_node function_code
		{
			get
			{
				return _function_code;
			}
            set
            {
                _function_code = value;
            }
		}

        /// <summary>
        /// Имя функции. Для языков не чувствительных к регистру - в том виде, в котором функция определена.
        /// </summary>
		public override string name
		{
			get
			{
				return _name;
			}
		}

		public void SetName(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Переменная, которая содержит возвращаемое значение функции. Скорее всего будет удалено.
        /// </summary>
		public local_variable return_variable
		{
			get
			{
				return _return_variable;
			}
			set
			{
				_return_variable=value;
			}
		}

        /// <summary>
        /// Вид узла - обычный, базовый или откомпилированный.
        /// </summary>
		public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.common;
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
        /// Код функции.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		SemanticTree.IStatementNode SemanticTree.ICommonFunctionNode.function_code
		{
			get
			{
				return _function_code;
			}
		}

        /// <summary>
        /// Возвращаемая переменная функции.
        /// Используется при обходе дерева посетителем.
        /// Хотя не должно использоваться. Скорее всего будет удалено.
        /// </summary>
		SemanticTree.ILocalVariableNode SemanticTree.ICommonFunctionNode.return_variable
		{
			get
			{
				return _return_variable;
			}
		}

        /// <summary>
        /// Массив функций определенных в этой функции.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		public SemanticTree.ICommonNestedInFunctionFunctionNode[] functions_nodes
		{
			get
			{
				return (_fnl.ToArray());
			}
		}

        /// <summary>
        /// Массив переменных, определенных в функции.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		public SemanticTree.ILocalVariableNode[] var_definition_nodes
		{
			get
			{
				return (_var_defs.ToArray());
			}
		}

        /// <summary>
        /// Массив констант, определенных в функции.
        /// Используется при обходе дерева посетителем.
        /// </summary>
        SemanticTree.ICommonFunctionConstantDefinitionNode[] SemanticTree.ICommonFunctionNode.constants
        {
            get
            {
                return _const_defs.ToArray();
            }
        }

    }

    /// <summary>
    /// Функция, определенная в пространстве имен.
    /// </summary>
	[Serializable]
	public class common_namespace_function_node : common_function_node, SemanticTree.ICommonNamespaceFunctionNode
	{
		private common_namespace_node _namespace;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="loc">Расположение функции.</param>
        /// <param name="nsp">Пространство имен, в котором определена функция.</param>
        /// <param name="scope">Пространство имен функции.</param>
        public common_namespace_function_node(string name, location loc, common_namespace_node nsp,SymbolTable.Scope scope) :
            base(name, loc,scope)
        {
            _namespace = nsp;
        }

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="ret_type">Тип возвращааемого знчения функции.</param>
        /// <param name="loc">Расположение функции.</param>
        /// <param name="nsp">Пространство имен, в котором определена функция.</param>
        /// <param name="scope">Пространство имен функции.</param>
        public common_namespace_function_node(string name, type_node ret_type, location loc, common_namespace_node nsp, SymbolTable.Scope scope) :
			base(name,ret_type,loc,scope)
		{
			_namespace=nsp;
		}

        public type_node ConnectedToType = null;

        public override bool is_extension_method
        {
            get
            {
                return ConnectedToType != null;
            }
        }

        SemanticTree.ITypeNode SemanticTree.ICommonNamespaceFunctionNode.ConnectedToType
        {
            get
            {
                return ConnectedToType;
            }
        }

        /// <summary>
        /// Пространство имен, в котором определена эта функция.
        /// </summary>
		public common_namespace_node namespace_node
		{
			get
			{
				return _namespace;
			}
            set
            {
                _namespace = value;
            }
		}

        /// <summary>
        /// Расположение функции - функция определена непосредственно в namespace.
        /// </summary>
		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_namespace_location;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_namespace_function_node;
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
        /// Пространство имен в котором определена функция.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		SemanticTree.ICommonNamespaceNode SemanticTree.ICommonNamespaceFunctionNode.namespace_node
		{
			get
			{
				return _namespace;
			}
		}

        /// <summary>
        /// Пространство имен в котором определена функция.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		public SemanticTree.ICommonNamespaceNode comprehensive_namespace
		{
			get
			{
				return _namespace;
			}
		}

        public override function_node get_instance(List<type_node> param_types, bool stop_on_error, location loc)
        {
            if (generic_parameters_count != param_types.Count)
            {
                if (stop_on_error)
                {
                    throw new TreeConverter.SimpleSemanticError(loc, "FUNCTION_{0}_DEPEND_FROM_{1}_TYPE_PARAMS", name, this.generic_parameters_count);
                }
                return null;
            }
            int num;
            TreeConverter.CompilationErrorWithLocation err = generic_parameter_eliminations.check_type_list(param_types, parameters_eliminations, true, out num);
            if (err != null)
            {
                if (stop_on_error)
                {
                    err.loc = loc;
                    throw err;
                }
                return null;
            }
            return generic_convertions.get_function_instance(this, param_types);
        }
	}

    /// <summary>
    /// Функция, определенная в другой функции.
    /// </summary>
	[Serializable]
	public class common_in_function_function_node : common_function_node, SemanticTree.ICommonNestedInFunctionFunctionNode
	{
		private readonly common_function_node _up_function;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="loc">Расположение имени функции в программе.</param>
        /// <param name="up_function">Функция, в которой определена эта функция.</param>
        /// <param name="scope">Протсранство имен этой функции.</param>
        public common_in_function_function_node(string name, location loc, common_function_node up_function, SymbolTable.Scope scope) :
            base(name, loc,scope)
        {
            _up_function = up_function;
        }

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="ret_type">Тип возвращаемого значения функции.</param>
        /// <param name="loc">Расположение имени функции в программе.</param>
        /// <param name="up_function">Функция, в которой определена эта функция.</param>
        /// <param name="scope">Протсранство имен этой функции.</param>
        public common_in_function_function_node(string name, type_node ret_type, location loc, common_function_node up_function, SymbolTable.Scope scope) :
			base(name,ret_type,loc,scope)
		{
			_up_function=up_function;
		}

        /// <summary>
        /// Функция, в которой определена эта функция.
        /// </summary>
		public common_function_node function
		{
			get
			{
				return _up_function;
			}
		}

        /// <summary>
        /// Расположение функции - функция определена в дргой функции.
        /// </summary>
		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_function_location;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_in_function_function_node;
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
        /// Функция в которой определена эта функция.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		SemanticTree.ICommonFunctionNode SemanticTree.IFunctionMemberNode.function
		{
			get
			{
				return _up_function;
			}
		}
	}

    /// <summary>
    /// Метод класса.
    /// </summary>
	[Serializable]
	public class common_method_node : common_function_node, SemanticTree.ICommonMethodNode
	{
		private common_type_node _cont_type;
		private SemanticTree.field_access_level _field_access_level;
		private SemanticTree.polymorphic_state _polymorphic_state;

        private type_node _explicit_interface;
		private bool _is_constructor;
        public bool first_statement = true;
        public bool inherited_ctor_called = false;
        public bool name_case_fixed = false;

        private bool _is_final = false;

        public type_node explicit_interface
        {
            get
            {
                return _explicit_interface;
            }
            set
            {
                _explicit_interface = value;
            }
        }

        public bool IsStatic
        {
            get
            {
                return _polymorphic_state == PascalABCCompiler.SemanticTree.polymorphic_state.ps_static;
            }
        }

        public override bool is_final
        {
            get
            {
                return _is_final;
            }
            set
            {
                _is_final = value;
            }
        }

        private bool _newslot_awaited = false;

        public virtual bool newslot_awaited
        {
            get
            {
                return _newslot_awaited;
            }
            set
            {
                _newslot_awaited = value;
            }
        }

        //\ssyy

        private function_node _overrided_method;

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="loc">Расположение имени метода в программе.</param>
        /// <param name="cont_type">Тип, который содержит этот метод.</param>
        /// <param name="polymorphic_state">Обычный, виртуальный или статический метод.</param>
        /// <param name="field_access_level">Уровень доступа к методу.</param>
        /// <param name="scope">Пространство имен функции.</param>
        public common_method_node(string name, location loc, common_type_node cont_type,
            SemanticTree.polymorphic_state polymorphic_state, SemanticTree.field_access_level field_access_level, SymbolTable.Scope scope) :
            base(name, loc,scope)
        {
            _cont_type = cont_type;
            _field_access_level = field_access_level;
            _polymorphic_state = polymorphic_state;
        }

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="name">Имя функции.</param>
        /// <param name="ret_type"></param>
        /// <param name="loc">Расположение имени метода в программе.</param>
        /// <param name="cont_type">Тип, который содержит этот метод.</param>
        /// <param name="polymorphic_state">Обычный, виртуальный или статический метод.</param>
        /// <param name="field_access_level">Уровень доступа к методу.</param>
        /// <param name="scope">Пространство имен функции.</param>
		public common_method_node(string name,type_node ret_type, location loc, common_type_node cont_type,
            SemanticTree.polymorphic_state polymorphic_state, SemanticTree.field_access_level field_access_level, SymbolTable.Scope scope) :
			base(name,ret_type,loc,scope)
		{
			_cont_type=cont_type;
            _field_access_level = field_access_level;
            _polymorphic_state = polymorphic_state;
		}
		
		public common_method_node(string name,location loc,SymbolTable.Scope scope) :
            base(name,loc,scope)
        {
        }

        /// <summary>
        /// Тип, который содержит этот метод.
        /// </summary>
		public common_type_node cont_type
		{
			get
			{
				return _cont_type;
			}
			set
            {
                _cont_type = value;
            }
		}

        public function_node overrided_method
        {
            get
            {
                return _overrided_method;
            }
            set
            {
                _overrided_method = value;
            }
        }

        private bool _is_reintroduce = false;

        /// <summary>
        /// Не ипольззутеся для генерации кода. информативная.
        /// </summary>
        public bool IsReintroduce
        {
            get
            {
                return _is_reintroduce;
            }
            set
            {
                _is_reintroduce = value;
            }
        }



        private local_variable _self_variable;
        public local_variable self_variable
        {
            get
            {
                return _self_variable;
            }
            set
            {
                _self_variable = value;
            }
        }

        //ssyy-
        /*
        /// <summary>
        /// Если данный метод определен в программе с ключевым словом constructor то это поле указывает на метод, соответствующий данному методу. См. компиляцию кода для конструкторов pascal.
        /// Возможно следует переместить это поле в ассоциированную с методом информацию.
        /// </summary>
		public common_method_node pascal_associated_constructor
		{
			get
			{
				return _pascal_associated_constructor;
			}
			set
			{
				_pascal_associated_constructor=value;
			}
		}
         */

        /// <summary>
        /// Обычный, виртуальный или статический метод.
        /// </summary>
		public override SemanticTree.polymorphic_state polymorphic_state
		{
			get
			{
				return _polymorphic_state;
			}
			set
            {
                _polymorphic_state = value;
            }
		}

        /// <summary>
        /// Уровень доступа к методу.
        /// </summary>
		public override SemanticTree.field_access_level field_access_level
		{
			get
			{
				return _field_access_level;
			}
            //ssyy
			set
            {
                _field_access_level = value;
            }
            //\ssyy
		}
        public void set_access_level(SemanticTree.field_access_level fal)
        {
            _field_access_level = fal;
        }
		
        /// <summary>
        /// Является ли этот метод конструктором.
        /// </summary>
		public bool is_constructor
		{
			get
			{
				return _is_constructor;
			}
			set
			{
				_is_constructor=value;
			}
		}

        /// <summary>
        /// Расположение функции - функция определена в классе.
        /// </summary>
		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_class_location;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_method_node;
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
        /// Тип, который содержит этот метод.
        /// </summary>
		public SemanticTree.ICommonTypeNode common_comprehensive_type
		{
			get
			{
				return _cont_type;
			}
		}

        /// <summary>
        /// Тип, который содержит этот метод.
        /// </summary>
		public virtual SemanticTree.ITypeNode comperehensive_type
		{
			get
			{
				return _cont_type;
			}
		}

        SemanticTree.IFunctionNode SemanticTree.ICommonMethodNode.overrided_method
        {
            get
            {
                return _overrided_method;
            }
        }

        public override function_node get_instance(List<type_node> param_types, bool stop_on_error, location loc)
        {
            if (generic_parameters_count != param_types.Count)
            {
                if (stop_on_error)
                {
                    throw new TreeConverter.SimpleSemanticError(loc, "FUNCTION_{0}_DEPEND_FROM_{1}_TYPE_PARAMS", name, this.generic_parameters_count);
                }
                return null;
            }
            int num;
            TreeConverter.CompilationErrorWithLocation err = generic_parameter_eliminations.check_type_list(param_types, parameters_eliminations, true, out num);
            if (err != null)
            {
                if (stop_on_error)
                {
                    err.loc = loc;
                    throw err;
                }
                return null;
            }
            return generic_convertions.get_function_instance(this, param_types);
        }
	}

    /// <summary>
    /// Обертка для откомпилированного метода.
    /// </summary>
	[Serializable]
	public class compiled_function_node : function_node, SemanticTree.ICompiledMethodNode
	{
		private readonly System.Reflection.MethodInfo _mi;
		private compiled_type_node _cont_type;
        private int _generic_params_count;
        private int _num_of_default_parameters;
        private bool _is_extension_method=false;
        private compiled_type_node connected_to_type;
        private List<compiled_type_node> _generic_params;
        private static System.Collections.Generic.Dictionary<System.Reflection.MethodInfo, compiled_function_node> compiled_methods =
            new System.Collections.Generic.Dictionary<System.Reflection.MethodInfo, compiled_function_node>();

        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="mi">Оборачиваемый метод.</param>
		private compiled_function_node(System.Reflection.MethodInfo mi)
		{
			_mi=mi;
			type_node ret_val=null;
			if (_mi.ReturnType!=null)
			{
				ret_val=compiled_type_node.get_type_node(mi.ReturnType);
                if (ret_val == SystemLibrary.SystemLibrary.void_type)
                {
                    ret_val = null;
                }
			}
			System.Reflection.ParameterInfo[] pinf=mi.GetParameters();
            parameter_list pal = new parameter_list();
            //if (!(_mi.IsGenericMethod))
            {
                int i = 1;
                foreach (System.Reflection.ParameterInfo pi in pinf)
                {
                    Type t = null;
                    
                    type_node par_type = null;
                    SemanticTree.parameter_type pt = SemanticTree.parameter_type.value;
//                    if (pi.ParameterType.Name.EndsWith("&") == true)
                    //(ssyy) Лучше так:
                    if (pi.ParameterType.IsByRef)
                    {
                        //t = pi.ParameterType.Assembly.GetType(pi.ParameterType.FullName.Substring(0, pi.ParameterType.FullName.Length - 1));
                        //(ssyy) Лучше так:
                        t = pi.ParameterType.GetElementType();
                        par_type = compiled_type_node.get_type_node(t);
                        pt = SemanticTree.parameter_type.var;
                    }
                    else
                    {
                        if (pi.Position == 0)
                        {
                            par_type = compiled_type_node.get_type_node(pi.ParameterType);
                            if (NetHelper.NetHelper.IsExtensionMethod(mi))
                            {
                                connected_to_type = par_type as compiled_type_node;
                            }
                        }
                    }
                    string name = pi.Name;
                    compiled_parameter crpar = new compiled_parameter(pi);
                    crpar.SetParameterType(pt);
                    pal.AddElement(crpar);
                    if (pi.IsOptional && pi.DefaultValue != null)
                        _num_of_default_parameters++;
                    i++;
                }
            }
            //else
            if (_mi.IsGenericMethod)
            {
                _generic_params_count = mi.GetGenericArguments().Length;
            }
            _is_extension_method = NetHelper.NetHelper.IsExtensionMethod(mi);
			this.return_value_type=ret_val;
			this.parameters.AddRange(pal);
		}

        /// <summary>
        /// Метод создания узла. Позволяет обеспичивать уникальность обертки для каждого метода.
        /// </summary>
        /// <param name="mi">Оборачиваемый метод.</param>
        /// <returns>Класс-обертка на метод.</returns>
        public static compiled_function_node get_compiled_method(System.Reflection.MethodInfo mi)
        {
            compiled_function_node cfn = null;
            if (compiled_methods.TryGetValue(mi, out cfn))
            {
                return cfn;
            }
            cfn = new compiled_function_node(mi);
            compiled_methods[mi] = cfn;
            return cfn;
        }

        /// <summary>
        /// Откомпилированный метод.
        /// </summary>
		public System.Reflection.MethodInfo method_info
		{
			get
			{
				return _mi;
			}
		}

        public override bool is_extension_method
        {
            get
            {
                return _is_extension_method;
            }
        }

        bool SemanticTree.ICompiledMethodNode.is_extension
        {
            get
            {
                return is_extension_method;
            }
        }

        public compiled_type_node ConnectedToType
        {
            get
            {
                return connected_to_type;
            }
        }

        public override int num_of_default_parameters
        {
            get
            {
                return _num_of_default_parameters;
            }
        }

        /// <summary>
        /// Класс, содержащий функцию.
        /// </summary>
		public compiled_type_node cont_type
		{
			get
			{
				if (_cont_type!=null)
				{
					return _cont_type;
				}
				_cont_type=compiled_type_node.get_type_node(_mi.DeclaringType);
				return _cont_type;
			}
		}
		
		public bool IsFinal
		{
			get
			{
				return _mi.IsFinal;
			}
		}
		
        /// <summary>
        /// Имя функции.
        /// </summary>
		public override string name
		{
			get
			{
				return _mi.Name;
			}
		}

        /// <summary>
        /// Функция - метод кдасса.
        /// </summary>
		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_class_location;
			}
		}

        /// <summary>
        /// Вид узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.compiled_function_node;
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
        /// Обычный, виртуальный или статический метод.
        /// </summary>
		public override SemanticTree.polymorphic_state polymorphic_state
		{
			get
			{
				if (_mi.IsAbstract)
				{
					return SemanticTree.polymorphic_state.ps_virtual_abstract;
				}
				if (_mi.IsStatic)
				{
					return SemanticTree.polymorphic_state.ps_static;
				}
				if (_mi.IsVirtual)
				{
					return SemanticTree.polymorphic_state.ps_virtual;
				}
				return SemanticTree.polymorphic_state.ps_common;
			}
		}
		
		public bool IsSpecialName
		{
			get
			{
				return _mi.IsSpecialName;
			}
		}

        public override bool is_final
        {
            get
            {
                return _mi.IsFinal;
            }
        }

        //public override 

        //Ограничения, налагаемые на параметры generic-типа
        private List<generic_parameter_eliminations> _parameters_eliminations = null;

        public override List<generic_parameter_eliminations> parameters_eliminations
        {
            get
            {
                if (_parameters_eliminations != null)
                {
                    return _parameters_eliminations;
                }
                if (!_mi.IsGenericMethodDefinition)
                {
                    throw new TreeConverter.CompilerInternalError("Method is not generic.");
                }
                _parameters_eliminations = generic_parameter_eliminations.make_eliminations_compiled(_mi.GetGenericArguments());
                return _parameters_eliminations;
            }
        }

        public override List<type_node> get_generic_params_list()
        {
            if (is_generic_function || is_generic_function_instance)
            {
                Type[] tp = _mi.GetGenericArguments();
                List<type_node> rez = new List<type_node>(tp.Length);
                foreach (Type t in tp)
                {
                    rez.Add(compiled_type_node.get_type_node(t));
                }
                return rez;
            }
            return null;
        }

        /// <summary>
        /// Уровень доступа к методу.
        /// </summary>
		public override SemanticTree.field_access_level field_access_level
		{
			get
			{
                return NetHelper.NetHelper.get_access_level(_mi);
			}
		}

	    /// <summary>
	    /// Вид узла - откомпилированный метод.
	    /// </summary>
		public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.compiled;
			}
		}

        /// <summary>
        /// Класс, содержащий этот метод.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		public SemanticTree.ICompiledTypeNode comprehensive_type
		{
			get
			{
				return cont_type;
			}
		}

        /// <summary>
        /// Класс, содержащий этот метод.
        /// Используется при обходе дерева посетителем.
        /// </summary>
		public SemanticTree.ITypeNode comperehensive_type
		{
			get
			{
				return cont_type;
			}
		}

        public override bool is_generic_function
        {
            get
            {
                return (this != SystemLibrary.SystemLibrary.resize_func) && _mi.IsGenericMethodDefinition;
            }
        }

        public override function_node original_function
        {
            get
            {
                if (is_generic_function_instance)
                {
                    return compiled_function_node.get_compiled_method(_mi.GetGenericMethodDefinition());
                }
                return null;
            }
        }

        public override int generic_parameters_count
        {
            get
            {
                return _generic_params_count;
            }
        }

        public override bool is_generic_function_instance
        {
            get
            {
                return _mi.IsGenericMethod && !_mi.IsGenericMethodDefinition;
            }
        }

        public override function_node get_instance(List<type_node> param_types, bool stop_on_error, location loc)
        {
            int count = param_types.Count;
            if (count != _generic_params_count)
            {
                if (stop_on_error)
                {
                    throw new TreeConverter.SimpleSemanticError(loc, "FUNCTION_{0}_DEPEND_FROM_{1}_TYPE_PARAMS", name, _generic_params_count);
                }
                return null;
            }
            int num;
            TreeConverter.CompilationErrorWithLocation err = generic_parameter_eliminations.check_type_list(param_types, parameters_eliminations, true, out num);
            if (err != null)
            {
                if (stop_on_error)
                {
                    err.loc = loc;
                    throw err;
                }
                return null;
            }
            bool all_compiled = true;
            List<Type> ts = new List<Type>(count);
            int k = 0;
            while (k < count && all_compiled)
            {
                compiled_type_node compt = param_types[k] as compiled_type_node;
                if (compt == null)
                {
                    all_compiled = false;
                }
                else
                {
                    ts.Add(compt._compiled_type);
                }
                ++k;
            }
            if (all_compiled)
            {
                System.Reflection.MethodInfo rez_t = _mi.MakeGenericMethod(ts.ToArray());
                compiled_function_node rez = compiled_function_node.get_compiled_method(rez_t);
                return rez;
            }
            return generic_convertions.get_function_instance(this, param_types);
        }
	}

	[Serializable]
	public class return_node : statement_node, SemanticTree.IReturnNode
	{
		private expression_node _return_expr;

		public return_node(expression_node return_expr,location loc) : base(loc)
		{
			_return_expr=return_expr;
		}

		public expression_node return_expr
		{
			get
			{
				return _return_expr;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.return_node;
			}
		}

		public SemanticTree.IExpressionNode return_value
		{
			get
			{
				return _return_expr;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}

/*    //ssyy
    [Serializable]
    public class ctor_return_node : statement_node, SemanticTree.ICtorReturnNode
    {
        public ctor_return_node(location loc)
            : base(loc)
        {
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit((SemanticTree.ICtorReturnNode)this);
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.ctor_return_node;
            }
        }
    }
    //\ssyy*/

    //TODO: Связать с compiled_function_node.
	[Serializable]
	public class compiled_constructor_node : function_node, SemanticTree.ICompiledConstructorNode
	{
        private static System.Collections.Generic.Dictionary<System.Reflection.ConstructorInfo, compiled_constructor_node> compiled_constructors =
            new System.Collections.Generic.Dictionary<System.Reflection.ConstructorInfo, compiled_constructor_node>();

        private int _num_of_default_parameters;
        private System.Reflection.ConstructorInfo _con_info;

        public static compiled_constructor_node get_compiled_constructor(System.Reflection.ConstructorInfo ci)
        {
            compiled_constructor_node ccn = null;
            if (compiled_constructors.TryGetValue(ci, out ccn))
            {
                return ccn;
            }
            ccn = new compiled_constructor_node(ci);
            //compiled_constructors[ci] = ccn;
            return ccn;
        }        
        
        //TODO: Возможно объеденить с compiled_function_node
		private compiled_constructor_node(System.Reflection.ConstructorInfo con_info)
		{
			_con_info=con_info;
            compiled_constructors[con_info] = this;
            //type_node ret_val=null;
			System.Reflection.ParameterInfo[] pinf=_con_info.GetParameters();
			parameter_list pal=new parameter_list();
			foreach(System.Reflection.ParameterInfo pi in pinf)
			{
				Type t=null;
				type_node par_type=null;
				SemanticTree.parameter_type pt=SemanticTree.parameter_type.value;
				if (pi.ParameterType.IsByRef) 
				{
					t=pi.ParameterType.GetElementType();
					par_type=compiled_type_node.get_type_node(t);
					pt=SemanticTree.parameter_type.var;
				}
				else
				{
					par_type=compiled_type_node.get_type_node(pi.ParameterType);
				}
				string name=pi.Name;
				compiled_parameter crpar=new compiled_parameter(pi);
				crpar.SetParameterType(pt);
				pal.AddElement(crpar);
                if (pi.IsOptional && pi.DefaultValue != null)
                    _num_of_default_parameters++;
			}
			this.return_value_type=compiled_type_node.get_type_node(_con_info.DeclaringType);
			this.parameters.AddRange(pal);
		}

        /*
		public override bool is_overload
		{
			get
			{
				return false;
			}
		}
        */

		public override string name
		{
			get
			{
				return _con_info.Name;
			}
		}

		public compiled_type_node compiled_type
		{
			get
			{
				return compiled_type_node.get_type_node(_con_info.DeclaringType);
			}
		}

		public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.compiled;
			}
		}

		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_class_location;
			}
		}

        public override int num_of_default_parameters
        {
            get
            {
                return _num_of_default_parameters;
            }
        }

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.compiled_constructor_node;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		public System.Reflection.ConstructorInfo constructor_info
		{
			get
			{
				return _con_info;
			}
		}

		public SemanticTree.ICompiledTypeNode comprehensive_type
		{
			get
			{
				return compiled_type_node.get_type_node(_con_info.DeclaringType);
			}
		}

		public SemanticTree.ITypeNode comperehensive_type
		{
			get
			{
				return compiled_type_node.get_type_node(_con_info.DeclaringType);
			}
		}

		public override SemanticTree.field_access_level field_access_level
		{
			get
			{
				if (_con_info.IsPublic)
				{
					return SemanticTree.field_access_level.fal_public;
				}
				if (_con_info.IsPrivate)
				{
					return SemanticTree.field_access_level.fal_private;
				}
				//Как в System.Reflection отображается то, что метод protected?
				return SemanticTree.field_access_level.fal_protected;
			}
		}

		public SemanticTree.polymorphic_state polymorphic_state
		{
			get
			{
				return SemanticTree.polymorphic_state.ps_common;
			}
		}
	}

    [Serializable]
    public class function_lambda_node : expression_node, SemanticTree.ILambdaFunctionNode
    {
        private parameter_list _parameters = new parameter_list();

        private type_node _ret_type;

        private statement_node _body;

        private function_node _function;

        //private compile_time_executor _compile_time_executor;

        //private bool _is_operator = false;

        /*public bool IsOperator
        {
            get { return _is_operator; }
            set { _is_operator = value; }
        }*/

        /*private System.Collections.Generic.Dictionary<function_lambda_node, function_intersection_node> _func_intersections =
            new System.Collections.Generic.Dictionary<function_lambda_node, function_intersection_node>();*/

        /*private void add_intersection(function_lambda_node _func, function_intersection_node _func_intersec)
        {
            _func_intersections.Add(_func, _func_intersec);
        }*/

        /*private function_intersection_node get_intersecion(function_lambda_node _func)
        {
            function_intersection_node _fint = null;
            bool ret = _func_intersections.TryGetValue(_func, out _fint);
            if (!ret)
            {
                return null;
            }
            return _fint;
        }*/

        /*public virtual SemanticTree.polymorphic_state polymorphic_state
        {
            get
            {
                return PascalABCCompiler.SemanticTree.polymorphic_state.ps_common;
            }
            set
            {
            }
        }*/
        /*public virtual SemanticTree.field_access_level field_access_level
        {
            get
            {
                return SemanticTree.field_access_level.fal_public;
            }
            set
            {
            }
        }*/

        public virtual bool is_generic_function
        {
            get
            {
                return false;
            }
        }

        /*public virtual function_lambda_node original_function
        {
            get
            {
                return null;
            }
        }*/

        /*public virtual bool is_generic_function_instance
        {
            get
            {
                return false;
            }
        }*/

        //ограничения на generic-параметры
        /*public virtual List<generic_parameter_eliminations> parameters_eliminations
        {
            get
            {
                return null;
            }
        }*/

        /*public virtual List<type_node> get_generic_params_list()
        {
            return null;
        }*/

        /*public virtual bool is_final
        {
            get
            {
                return false;
            }
            set
            {
            }
        }*/

        public virtual int generic_parameters_count
        {
            get
            {
                return 0;
            }
        }

        /*public static void add_function_intersection(function_lambda_node left, function_lambda_node right,
            function_intersection_node intersec)
        {
#if DEBUG
            function_intersection_node func_int_left = left.get_intersecion(right);
            if (func_int_left != null)
            {
                throw new TreeConverter.CompilerInternalError("Duplicate function intersection");
            }
#endif
            left.add_intersection(right, intersec);
        }*/

        /*public static function_intersection_node get_function_intersection(function_lambda_node left, function_lambda_node right)
        {
            return left.get_intersecion(right);
        }*/

        /*public static function_compare compare_functions(function_lambda_node left, function_lambda_node right)
        {
            function_intersection_node func_int_left = left.get_intersecion(right);
            function_intersection_node func_int_right = right.get_intersecion(left);
            if ((func_int_left == null) && (func_int_right == null))
            {
                return function_compare.non_comparable;
            }
            if ((func_int_left != null) && (func_int_right == null))
            {
                return func_int_left.function_relation;
            }
            if ((func_int_left == null) && (func_int_right != null))
            {
                if (func_int_right.function_relation == function_compare.non_comparable)
                {
                    return function_compare.non_comparable;
                }
                if (func_int_right.function_relation == function_compare.greater)
                {
                    return function_compare.less;
                }
                if (func_int_right.function_relation == function_compare.less)
                {
                    return function_compare.greater;
                }
            }
            if ((func_int_left != null) && (func_int_right != null))
            {
                if ((func_int_left.function_relation == function_compare.less) &&
                    (func_int_right.function_relation == function_compare.greater))
                {
                    return function_compare.less;
                }
                if ((func_int_left.function_relation == function_compare.greater) &&
                    (func_int_right.function_relation == function_compare.less))
                {
                    return function_compare.greater;
                }
                throw new TreeConverter.CompilerInternalError("Contradiction in function relations");
            }
            throw new TreeConverter.CompilerInternalError("Error in function relations");
        }*/

        /// <summary>
        /// Делегат, который вызывается при создании узла вызова метода. Если результат не null, то вместо узла вызова метода в дерево попадает возвращаемое этим длегатом значение. Таким образом он позволяет вычислять значения функции во время компиляции.
        /// </summary>
        /*public compile_time_executor compile_time_executor
        {
            get
            {
                return _compile_time_executor;
            }
            set
            {
                _compile_time_executor = value;
            }
        }*/

        /*public virtual function_lambda_node get_instance(List<type_node> param_types, bool stop_on_error, location loc)
        {
            return null;
        }*/

        public function_lambda_node()
        {
        }

        public function_lambda_node(type_node ret_type)
        {
            _ret_type = ret_type;
        }
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="ret_type">Тип возвращаемого значения функции.</param>
        public function_lambda_node(parameter_list parameters, type_node ret_type, statement_node body)
        {
            _parameters = parameters;
            _ret_type = ret_type;
            _body = body;
        }

        /*
        public function_node(parameter_list parameters,type_node ret_type)
        {
            _ret_type = ret_type;
            _parameters.AddRange(parameters);
        }
        */
        /// <summary>
        /// Вид узла - базовый (basic), обычный (common) или экспортируемый (compiled).
        /// </summary>
        public SemanticTree.node_kind node_kind
        {
            get { return node_kind; }
        }

        /// <summary>
        /// Список формальных параметров функции.
        /// </summary>
        public parameter_list parameters
        {
            get
            {
                return _parameters;
            }
        }

        /// <summary>
        /// Тип возвращаемого значения функции.
        /// </summary>
        public type_node return_value_type
        {
            get
            {
                return _ret_type;
            }
            set
            {
                _ret_type = value;
            }
        }

        public function_node function
        {
            get
            {
                return _function;
            }
            set
            {
                _function = value;
            }
        }

        /// <summary>
        /// Имя функции.
        /// </summary>
        /*public string name
        {
            get { return name; }
        }*/

        //TODO: Может is_overload оставить только в информации связанной с common_function_node.
        /*
        public abstract bool is_overload
        {
            get;
        }
        */

        /// <summary>
        /// Расположение узла - в пространстве имен, классе или функции.
        /// </summary>
        public SemanticTree.node_location_kind node_location_kind
        {
            get { return node_location_kind; }
        }

        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
        public override general_node_type general_node_type
        {
            get
            {
                return general_node_type.function_node;
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
        /// Тип возвращаеого значения.
        /// </summary>
        SemanticTree.ITypeNode SemanticTree.ILambdaFunctionNode.return_value_type
        {
            get
            {
                return _ret_type;
            }
        }

        /// <summary>
        /// Список формальных параметров функции.
        /// </summary>
        SemanticTree.IParameterNode[] SemanticTree.ILambdaFunctionNode.parameters
        {
            get
            {
                return (_parameters.ToArray());
            }
        }

        SemanticTree.IStatementNode SemanticTree.ILambdaFunctionNode.body
        {
            get
            {
                return _body;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.return_node;
            }
        }

        SemanticTree.IFunctionNode SemanticTree.ILambdaFunctionNode.function
        {
            get
            {
                return _function;
            }
        }

    }
    [Serializable]
    public class function_lambda_call_node : expression_node, SemanticTree.ILambdaFunctionCallNode
    {
        private expressions_list _parameters = new expressions_list();

        private function_lambda_node _fln;

        public function_lambda_call_node()
        {
        }

        /*public function_lambda_call_node(type_node ret_type)
        {
            _ret_type = ret_type;
        }*/
        /// <summary>
        /// Конструктор узла.
        /// </summary>
        /// <param name="ret_type">Тип возвращаемого значения функции.</param>
        public function_lambda_call_node(expressions_list parameters, function_lambda_node fln)
        {
            _parameters = parameters;
            _fln = fln;
        }
        public expressions_list parameters
        {
            get
            {
                return _parameters;
            }
        }
        public SemanticTree.node_kind node_kind
        {
            get { return node_kind; }
        }
        public function_lambda_node fln
        {
            get
            {
                return _fln;
            }
        }
        public SemanticTree.node_location_kind node_location_kind
        {
            get { return node_location_kind; }
        }

        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
        public override general_node_type general_node_type
        {
            get
            {
                return general_node_type.function_node;
            }
        }

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.return_node;
            }
        }
        SemanticTree.IExpressionNode[] SemanticTree.ILambdaFunctionCallNode.parameters
        {
            get
            {
                return (_parameters.ToArray());
            }
        }
        SemanticTree.ILambdaFunctionNode SemanticTree.ILambdaFunctionCallNode.lambda
        {
            get
            {
                return fln;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }
    }

}
