// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Вид внутреннего интерфейса, реализуемого типом.
    /// </summary>
    public enum internal_interface_kind { ordinal_interface, delegate_interface, unsized_array_interface ,
        bounded_array_interface};

    /// <summary>
    /// Внутренний интерфейс. Это не интерфейс, который будет потом доступен из среды исполнения,
    /// а используемый о внутренних целях компилятора.
    /// </summary>
    [Serializable]
	public abstract class internal_interface
	{
		public abstract internal_interface_kind internal_interface_kind
		{
			get;
		}
	}

    /*
    /// <summary>
    /// Внутренний интерфейс, который должны предоставлять все порядковые типы.
    /// </summary>
    [Serializable]
    public abstract class ordinal_type_interface : internal_interface
    {
        /// <summary>
        /// Тип внутреннего интерфейса.
        /// </summary>
        public override internal_interface_kind internal_interface_kind
        {
            get
            {
                return internal_interface_kind.ordinal_interface;
            }
        }

        /// <summary>
        /// Возвращает узел дерева, с операцией увеличения expr на 1.
        /// </summary>
        /// <param name="expr">Выражение для инкремента.</param>
        /// <returns>Инкрементированное выражение.</returns>
        public abstract expression_node inc_node(expression_node expr);

        /// <summary>
        /// Возвращает узел дерева, с операцией уменьшения expr на 1.
        /// </summary>
        /// <param name="expr">Выражение для декремента.</param>
        /// <returns>Декрементированное выражение.</returns>
        public abstract expression_node dec_node(expression_node expr);

        /// <summary>
        /// Возвращает булевское выражение, представляющие операцию сравнения двух выражений.
        /// </summary>
        /// <param name="left">Левое выражение перечеслимого типа.</param>
        /// <param name="right">Правое выражение перечеслимого типа.</param>
        /// <returns>Возвращаит true если левое меньше или равро правого, иначе false.</returns>
        public abstract expression_node lower_eq_node(expression_node left, expression_node right);

        /// <summary>
        /// Возвращает булевское выражение, представляющие операцию сравнения двух выражений.
        /// </summary>
        /// <param name="left">Левое выражение перечеслимого типа.</param>
        /// <param name="right">Правое выражение перечеслимого типа.</param>
        /// <returns>Возвращаит true если левое больше или равро правого, иначе false.</returns>
        public abstract expression_node greater_eq_node(expression_node left, expression_node right);

        /// <summary>
        /// Наименьшее значение перечеслимого типа.
        /// </summary>
        public abstract constant_node lower_value
        {
            get;
        }

        /// <summary>
        /// Наибольшее значение перечеслимого типа.
        /// </summary>
        public abstract constant_node upper_value
        {
            get;
        }

        /// <summary>
        /// Возвращает выражение с операцией приведения данного типа к целому.
        /// </summary>
        /// <param name="expr">Выражение для приведения.</param>
        /// <returns>Выражение с операцией приведения типа.</returns>
        public abstract expression_node expression_to_int(expression_node expr);

        /// <summary>
        /// Приводит тип к целому. Используется при компиляции.
        /// </summary>
        /// <param name="cn">Константа для приведения.</param>
        /// <returns>Целое число, соответствующее константе перечеслимого типа.</returns>
        public abstract int ordinal_type_to_int(constant_node cn);
    }
    */

    /// <summary>
    /// Делегат, преобразующий константу указанного типа в целое число.
    /// </summary>
    /// <param name="cn">Значение константы.</param>
    /// <returns>Целое число к которому приведена константа.</returns>
	public delegate int ordinal_type_to_int(constant_node cn);
    
    [Serializable]
	public class ordinal_type_interface : internal_interface
	{
        /// <summary>
        /// Метод инкрементирования переменной порядкового типа.
        /// </summary>
		private function_node _inc_method;

        /// <summary>
        /// Метод декрементирования переменной порядкового типа.
        /// </summary>
		private function_node _dec_method;

        /// <summary>
        /// Метод получения следующего значения порядкового типа. Должен принимать одно значение данного типа и возвращать следующее.
        /// </summary>
        private function_node _inc_value_method;

        /// <summary>
        /// Метод получения предыдущего значения порядкового типа. Должен принимать одно значение данного типа и возвращать предыдущее.
        /// </summary>
        private function_node _dec_value_method;

        //private function_node _internal_inc_value;

        //private function_node _internal_dec_value;

        /// <summary>
        /// Метод сравнения двух значений данного типа. Принимает два значения перечислимого типа и 
        /// возвращаит true если левое меньше или равро правого. Иначе false.
        /// </summary>
		private function_node _lower_eq_method;

        /// <summary>
        /// Метод сравнения двух значений данного типа. Принимает два значения перечислимого типа и возвращаит true 
        /// если левое больше или равро правого. Иначе false.
        /// </summary>
		private function_node _greater_eq_method;
		
		private function_node _lower_method;
		
		private function_node _greater_method;
		
        /// <summary>
        /// Наименьшее значени перечислимого типа.
        /// </summary>
		private constant_node _lower_value;

        /// <summary>
        /// Наибольшее значение перечеслимого типа.
        /// </summary>
		private constant_node _upper_value;

        /// <summary>
        /// Метод преобразования значения перечислимого типа в целое число.
        /// Должен принимать одно значение перечислимого типа и возвращать значение целого типа.
        /// </summary>
		private function_node _value_to_int;

        /// <summary>
        /// Делегат, который позволяет преобразовывать значение перечислимого типа в целое во время компиляции.
        /// </summary>
		private ordinal_type_to_int _ordinal_type_to_int;
        
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="inc_method">Метод получения следующего значения порядкового типа.</param>
        /// <param name="dec_method">Метод получения предыдущего значения порядкового типа.</param>
        /// <param name="lower_eq_method">Метод сравнения двух значений данного типа. Принимает два значения перечислимого типа и возвращаит true если левое меньше или равро правого. Иначе false.</param>
        /// <param name="greater_eq_method">Метод сравнения двух значений данного типа. Принимает два значения перечислимого типа и возвращаит true если левое больше или равро правого. Иначе false.</param>
        /// <param name="lower_value">Наименьшее значени перечислимого типа.</param>
        /// <param name="upper_value">Наибольшее значение перечеслимого типа.</param>
        /// <param name="value_to_int">Метод преобразования значения перечислимого типа в целое число.</param>
        /// <param name="ordinal_type_to_int_method">Делегат, который позволяет преобразовывать значение перечислимого типа в целое во время компиляции.</param>
        public ordinal_type_interface(function_node inc_method, function_node dec_method,
            function_node inc_value_method, function_node dec_value_method,
            function_node lower_eq_method, function_node greater_eq_method,
            function_node lower_method, function_node greater_method,
            constant_node lower_value, constant_node upper_value,
            function_node value_to_int, ordinal_type_to_int ordinal_type_to_int_method)
        {
            _inc_method = inc_method;
            _dec_method = dec_method;
            _inc_value_method = inc_value_method;
            _dec_value_method = dec_value_method;
            _lower_eq_method = lower_eq_method;
            _greater_eq_method = greater_eq_method;
            _lower_method = lower_method;
            _greater_method = greater_method;
            _lower_value = lower_value;
            _upper_value = upper_value;
            _value_to_int = value_to_int;
            _ordinal_type_to_int = ordinal_type_to_int_method;
        }

        /*
        public function_node internal_inc_value
        {
            get
            {
                return _internal_inc_value;
            }
        }

        public function_node internal_dec_value
        {
            get
            {
                return _internal_dec_value;
            }
        }
        */
        /// <summary>
        /// Метод инкрементирования переменной порядкового типа.
        /// </summary>
		public function_node inc_method
		{
			get
			{
				return _inc_method;
			}
		}

        /// <summary>
        /// Метод декрементирования переменной порядкового типа.
        /// </summary>
		public function_node dec_method
		{
			get
			{
				return _dec_method;
			}
		}

        /// <summary>
        /// Метод получения следующего значения порядкового типа. Должен принимать одно значение данного типа и возвращать следующее.
        /// </summary>
        public function_node inc_value_method
        {
            get
            {
                return _inc_value_method;
            }
        }

        /// <summary>
        /// Метод получения предыдущего значения порядкового типа. Должен принимать одно значение данного типа и возвращать предыдущее.
        /// </summary>
        public function_node dec_value_method
        {
            get
            {
                return _dec_value_method;
            }
        }

        /// <summary>
        /// Метод сравнения двух значений данного типа. Принимает два значения перечислимого типа и 
        /// возвращаит true если левое меньше или равро правого. Иначе false.
        /// </summary>
		public function_node lower_eq_method
		{
			get
			{
				return _lower_eq_method;
			}
		}
		
		public function_node lower_method
		{
			get
			{
				return _lower_method;
			}
		}
		
        /// <summary>
        /// Метод сравнения двух значений данного типа. Принимает два значения перечислимого типа и возвращаит true 
        /// если левое больше или равро правого. Иначе false.
        /// </summary>
		public function_node greater_eq_method
		{
			get
			{
				return _greater_eq_method;
			}
		}
		
		public function_node greater_method
		{
			get
			{
				return _greater_method;
			}
		}

        /// <summary>
        /// Наименьшее значени перечислимого типа.
        /// </summary>
		public constant_node lower_value
		{
			get
			{
				return _lower_value;
			}
		}

        /// <summary>
        /// Наибольшее значение перечеслимого типа.
        /// </summary>
		public constant_node upper_value
		{
			get
			{
				return _upper_value;
			}
		}

        /// <summary>
        /// Метод преобразования значения перечислимого типа в целое число.
        /// Должен принимать одно значение перечислимого типа и возвращать значение целого типа.
        /// </summary>
		public function_node value_to_int
		{
			get
			{
				return _value_to_int;
			}
		}

        /// <summary>
        /// Делегат, который позволяет преобразовывать значение перечислимого типа в целое во время компиляции.
        /// </summary>
		public ordinal_type_to_int ordinal_type_to_int
		{
			get
			{
				return _ordinal_type_to_int;
			}
		}

        public override internal_interface_kind internal_interface_kind
        {
            get
            {
                return internal_interface_kind.ordinal_interface;
            }
        }

        public type_node elems_type
        {
            get
            {
                return lower_value.type;
            }
        }
    
    }

    [Serializable]
    public class delegate_internal_interface : internal_interface
    {
        private parameter_list _parametres=new parameter_list();
        private type_node _return_value_type;
        private function_node _invoke_method;
        private function_node _constructor;

        public delegate_internal_interface(type_node return_value_type, function_node invoke_method,
            function_node constructor)
        {
            _return_value_type = return_value_type;
            _invoke_method = invoke_method;
            _constructor = constructor;
        }

        public parameter_list parameters
        {
            get
            {
                return _parametres;
            }
        }

        public type_node return_value_type
        {
            get
            {
                return _return_value_type;
            }
        }

        public function_node invoke_method
        {
            get
            {
                return _invoke_method;
            }
        }

        public function_node constructor
        {
            get
            {
                return _constructor;
            }
        }

        public override internal_interface_kind internal_interface_kind
        {
            get
            {
                return (internal_interface_kind.delegate_interface);
            }
        }
    }

    [Serializable]
    public class array_internal_interface : internal_interface
    {
        private type_node _element_type;
		private int _rank=1;
		
        public array_internal_interface(type_node element_type, int rank)
        {
            _element_type = element_type;
            _rank = rank;
        }

        public type_node element_type
        {
            get
            {
                return _element_type;
            }
        }
		
        public int rank
        {
        	get
        	{
        		return _rank;
        	}
        }
        
        public override internal_interface_kind internal_interface_kind
        {
            get
            {
                return (internal_interface_kind.unsized_array_interface);
            }
        }

    }
	/*
    [Serializable]
    public class pascal_array_internal_interface : internal_interface
    {
        private type_node _index_type;
        private constant_node _lower_index_value;
        private constant_node _upper_index_value;
        private int _int_upper_index_value;
        private int _int_lower_index_value;
        private type_node _elem_type;

        public pascal_array_internal_interface(type_node index_type, constant_node lower_index_value, constant_node upper_index_value,
            int int_lower_index_value, int int_upper_index_value, type_node elem_type)
        {
            _index_type = index_type;
            _lower_index_value = lower_index_value;
            _upper_index_value = upper_index_value;
            _int_lower_index_value = int_lower_index_value;
            _int_upper_index_value = int_upper_index_value;
            _elem_type = elem_type;
        }

        public type_node elem_type
        {
            get
            {
                return _elem_type;
            }
        }

        public int int_upper_index_value
        {
            get
            {
                return _int_upper_index_value;
            }
        }

        public int int_lower_index_value
        {
            get
            {
                return _int_lower_index_value;
            }
        }

        public int length
        {
            get
            {
                return (_int_upper_index_value - _int_lower_index_value + 1);
            }
        }

        public type_node index_type
        {
            get
            {
                return _index_type;
            }
        }

        public constant_node lower_index_value
        {
            get
            {
                return _lower_index_value;
            }
        }

        public constant_node upper_index_value
        {
            get
            {
                return _upper_index_value;
            }
        }

        public override internal_interface_kind internal_interface_kind
        {
            get
            {
                return internal_interface_kind.pascal_array_internal_interface;
            }
        }

    }
	*/
    /*
    [Serializable]
    public class diap_internal_interface : internal_interface
    {
        private constant_node _lower_value;
        private constant_node _upper_value;

        public diap_internal_interface(constant_node lower_value, constant_node upper_value)
        {
            _lower_value = lower_value;
            _upper_value = upper_value;
        }

        public constant_node lower_value
        {
            get
            {
                return _lower_value;
            }
        }

        public constant_node upper_value
        {
            get
            {
                return _upper_value;
            }
        }

        public override internal_interface_kind internal_interface_kind
        {
            get
            {
                return internal_interface_kind.diap_internal_interface;
            }
        }
    }
    */

    [Serializable]
    public class bounded_array_interface : internal_interface
    {
        private ordinal_type_interface _oti;
        private type_node _element_type;
        private common_property_node _index_property;
        private type_node _index_type;
        private class_field _int_array;

        public bounded_array_interface(ordinal_type_interface oti, type_node element_type,
            common_property_node index_property, type_node index_type, class_field int_array)
        {
            _oti = oti;
            _element_type = element_type;
            _index_property = index_property;
            _index_type = index_type;
            _int_array = int_array;
        }

        public class_field int_array
        {
            get
            {
                return _int_array;
            }
        }

        public ordinal_type_interface ordinal_type_interface
        {
            get
            {
                return _oti;
            }
        }

        public common_property_node index_property
        {
            get
            {
                return _index_property;
            }
        }

        public type_node index_type
        {
            get
            {
                return _index_type;
            }
        }

        public type_node element_type
        {
            get
            {
                return _element_type;
            }
        }

        public override internal_interface_kind internal_interface_kind
        {
            get
            {
                return internal_interface_kind.bounded_array_interface;
            }
        }
    }

}
