// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
//Инициализация системной библиотеки
using System;

using PascalABCCompiler.TreeRealization;

using PascalABCCompiler.TreeConverter;

using System.Collections;
using System.Collections.Generic;

using System.Linq;

namespace PascalABCCompiler.SystemLibrary
{
        
    //TODO: Все базовые методы должны быть определены здесь как статические и использовать их отсюда, а не создавать их по два раза.
    public static class SystemLibrary
    {
    	
        private static compiled_type_node _bool_type;
        private static compiled_type_node _byte_type;
        private static compiled_type_node _sbyte_type;
        private static compiled_type_node _short_type;
        private static compiled_type_node _ushort_type;
        private static compiled_type_node _integer_type;
        private static compiled_type_node _uint_type;
        private static compiled_type_node _int64_type;
        private static compiled_type_node _uint64_type;
        private static compiled_type_node _double_type; //double
        private static compiled_type_node _float_type;
		private static compiled_type_node _char_type;
        private static compiled_type_node _string_type;
        private static compiled_type_node _object_type;
        private static compiled_type_node _exception_base_type;
        private static compiled_type_node _array_base_type;
        private static compiled_type_node _system_delegate_type;
        private static compiled_type_node _delegate_base_type;
        private static compiled_type_node _enum_base_type;
        private static compiled_type_node _pointer_type;
        private static compiled_type_node _complex_type;
        private static compiled_constructor_node _complex_type_constructor;
        private static compiled_type_node _void_type;
        private static compiled_type_node _value_type;
        private static compiled_type_node _decimal_type;
		private static compiled_type_node _attribute_type;
        private static compiled_type_node _array_of_string;
		private static compiled_type_node _dllimport_type;
		private static compiled_type_node _flags_attribute_type;
		private static compiled_type_node _usage_attribute_type;
		private static compiled_type_node _comimport_type;
		private static compiled_type_node _field_offset_attribute_type;
		private static compiled_type_node _struct_layout_attribute_type;
		
        private static compiled_type_node _icloneable_interface;
        private static compiled_type_node _ilist_interface;
        private static compiled_type_node _icollection_interface;
        private static compiled_type_node _ienumerable_interface;
        private static compiled_type_node _ilist1_interface;
        private static compiled_type_node _icollection1_interface;
        private static compiled_type_node _ireadonlycollection_interface;
        private static compiled_type_node _ireadonlylist_interface;
        private static compiled_type_node _ienumerable1_interface;

        private static compiled_function_node _delegate_combine_method;
        private static compiled_function_node _delegate_remove_method;
		private static compiled_function_node _assert_method;

        private static compiled_function_node _object_equals_method;

        private static bool_const_node _true_constant;
		private static bool_const_node _false_constant;

        private static basic_function_node _empty_method;
        public static expression_node get_empty_method_call(location loc)
        {
            return new basic_function_call(_empty_method, loc);
        }

        //ssyy добавил 02.06.2007
        //system_unit - то же, что и syntax_tree_visitor._system_unit
        //Используется при восстановлении шаблонного класса из PCU
        public static common_unit_node system_unit = null;
        //\ssyy

        //ssyy добавил 26.06.2007
        public static syntax_tree_visitor syn_visitor = null;
        //\ssyy

        //ssyy добавил 05.07.2007
        public static type_constructor type_constructor = null;
        //\ssyy

		private static SymbolTable.TreeConverterSymbolTable _symtab;
        private static System.Collections.Generic.Dictionary<SemanticTree.basic_function_type, basic_function_node> ht = new System.Collections.Generic.Dictionary<SemanticTree.basic_function_type, basic_function_node>();

        private static System.Collections.Hashtable writable_in_typed_files_types = new System.Collections.Hashtable();

        private static string_const_node _empty_string;

        internal static basic_function_node _byte_plusassign;
        internal static basic_function_node _byte_minusassign;
        internal static basic_function_node _byte_multassign;
        internal static basic_function_node _byte_divassign;

        internal static basic_function_node _sbyte_plusassign;
        internal static basic_function_node _sbyte_minusassign;
        internal static basic_function_node _sbyte_multassign;
        internal static basic_function_node _sbyte_divassign;

        internal static basic_function_node _short_plusassign;
        internal static basic_function_node _short_minusassign;
        internal static basic_function_node _short_multassign;
        internal static basic_function_node _short_divassign;

        internal static basic_function_node _ushort_plusassign;
        internal static basic_function_node _ushort_minusassign;
        internal static basic_function_node _ushort_multassign;
        internal static basic_function_node _ushort_divassign;

        internal static basic_function_node _int_plusassign;
        internal static basic_function_node _int_minusassign;
        internal static basic_function_node _int_multassign;
        internal static basic_function_node _int_divassign;

        internal static basic_function_node _uint_plusassign;
        internal static basic_function_node _uint_minusassign;
        internal static basic_function_node _uint_multassign;
        internal static basic_function_node _uint_divassign;

        internal static basic_function_node _long_plusassign;
        internal static basic_function_node _long_minusassign;
        internal static basic_function_node _long_multassign;
        internal static basic_function_node _long_divassign;

        internal static basic_function_node _ulong_plusassign;
        internal static basic_function_node _ulong_minusassign;
        internal static basic_function_node _ulong_multassign;
        internal static basic_function_node _ulong_divassign;

        internal static basic_function_node _float_plusassign;
        internal static basic_function_node _float_minusassign;
        internal static basic_function_node _float_multassign;
        internal static basic_function_node _float_divassign;

        internal static basic_function_node _double_plusassign;
        internal static basic_function_node _double_minusassign;
        internal static basic_function_node _double_multassign;
        internal static basic_function_node _double_divassign;
        
        //byte
        private static basic_function_node _byte_to_sbyte;
        private static basic_function_node _byte_to_short;
        private static basic_function_node _byte_to_ushort;
        private static basic_function_node _byte_to_int;
        private static basic_function_node _byte_to_uint;
        private static basic_function_node _byte_to_long;
        private static basic_function_node _byte_to_ulong;
        private static basic_function_node _byte_to_char;
        private static basic_function_node _byte_to_float;
        private static basic_function_node _byte_to_double;
        private static basic_function_node _byte_assign;
        private static basic_function_node _byte_unmin;
        private static basic_function_node _byte_not;
        private static basic_function_node _byte_gr;
        private static basic_function_node _byte_greq;
        private static basic_function_node _byte_sm;
        private static basic_function_node _byte_smeq;
        private static basic_function_node _byte_eq; 
        private static basic_function_node _byte_noteq; 
        private static basic_function_node _byte_add; 
        private static basic_function_node _byte_sub; 
        private static basic_function_node _byte_mul; 
        private static basic_function_node _byte_idiv; 
        private static basic_function_node _byte_mod; 
        private static basic_function_node _byte_div;
        private static basic_function_node _byte_and;
        private static basic_function_node _byte_or;
        private static basic_function_node _byte_xor; 
        //private static basic_function_node _byte_shl;
        //private static basic_function_node _byte_shr; 

        //sbyte
        private static basic_function_node _sbyte_to_byte;
        private static basic_function_node _sbyte_to_short;
        private static basic_function_node _sbyte_to_ushort;
        private static basic_function_node _sbyte_to_int;
        private static basic_function_node _sbyte_to_uint;
        private static basic_function_node _sbyte_to_long;
        private static basic_function_node _sbyte_to_ulong;
        private static basic_function_node _sbyte_to_char;
        private static basic_function_node _sbyte_to_float;
        private static basic_function_node _sbyte_to_double;
        private static basic_function_node _sbyte_assign;
        private static basic_function_node _sbyte_unmin;
        private static basic_function_node _sbyte_not;
        private static basic_function_node _sbyte_gr;
        private static basic_function_node _sbyte_greq;
        private static basic_function_node _sbyte_sm;
        private static basic_function_node _sbyte_smeq;
        private static basic_function_node _sbyte_eq;
        private static basic_function_node _sbyte_noteq;
        private static basic_function_node _sbyte_add;
        private static basic_function_node _sbyte_sub;
        private static basic_function_node _sbyte_mul;
        private static basic_function_node _sbyte_idiv;
        private static basic_function_node _sbyte_mod;
        private static basic_function_node _sbyte_div;
        private static basic_function_node _sbyte_and;
        private static basic_function_node _sbyte_or;
        private static basic_function_node _sbyte_xor;
        //private static basic_function_node _sbyte_shl;
        //private static basic_function_node _sbyte_shr;

        //short
        private static basic_function_node _short_to_byte;
        private static basic_function_node _short_to_sbyte;
        private static basic_function_node _short_to_ushort;
        private static basic_function_node _short_to_int;
        private static basic_function_node _short_to_uint;
        private static basic_function_node _short_to_long;
        private static basic_function_node _short_to_ulong;
        private static basic_function_node _short_to_char;
        private static basic_function_node _short_to_float;
        private static basic_function_node _short_to_double;
        private static basic_function_node _short_assign;
        private static basic_function_node _short_unmin;
        private static basic_function_node _short_not;
        private static basic_function_node _short_gr;
        private static basic_function_node _short_greq;
        private static basic_function_node _short_sm;
        private static basic_function_node _short_smeq;
        private static basic_function_node _short_eq;
        private static basic_function_node _short_noteq;
        private static basic_function_node _short_add;
        private static basic_function_node _short_sub;
        private static basic_function_node _short_mul;
        private static basic_function_node _short_idiv;
        private static basic_function_node _short_mod;
        private static basic_function_node _short_div;
        private static basic_function_node _short_and;
        private static basic_function_node _short_or;
        private static basic_function_node _short_xor;
        //private static basic_function_node _short_shl;
        //private static basic_function_node _short_shr;

        //ushort
        private static basic_function_node _ushort_to_byte;
        private static basic_function_node _ushort_to_sbyte;
        private static basic_function_node _ushort_to_short;
        private static basic_function_node _ushort_to_int;
        private static basic_function_node _ushort_to_uint;
        private static basic_function_node _ushort_to_long;
        private static basic_function_node _ushort_to_ulong;
        private static basic_function_node _ushort_to_char;
        private static basic_function_node _ushort_to_float;
        private static basic_function_node _ushort_to_double;
        private static basic_function_node _ushort_assign;
        private static basic_function_node _ushort_unmin;
        private static basic_function_node _ushort_not;
        private static basic_function_node _ushort_gr;
        private static basic_function_node _ushort_greq;
        private static basic_function_node _ushort_sm;
        private static basic_function_node _ushort_smeq;
        private static basic_function_node _ushort_eq;
        private static basic_function_node _ushort_noteq;
        private static basic_function_node _ushort_add;
        private static basic_function_node _ushort_sub;
        private static basic_function_node _ushort_mul;
        private static basic_function_node _ushort_idiv;
        private static basic_function_node _ushort_mod;
        private static basic_function_node _ushort_div;
        private static basic_function_node _ushort_and;
        private static basic_function_node _ushort_or;
        private static basic_function_node _ushort_xor;
        //private static basic_function_node _ushort_shl;
        //private static basic_function_node _ushort_shr;

        //int
        private static basic_function_node _int_to_byte;
        private static basic_function_node _int_to_sbyte;
        private static basic_function_node _int_to_short;
        private static basic_function_node _int_to_ushort;
        private static basic_function_node _int_to_uint;
        private static basic_function_node _int_to_long;
        private static basic_function_node _int_to_ulong;
        private static basic_function_node _int_to_char;
        private static basic_function_node _int_to_float;
        private static basic_function_node _int_to_double;
        private static basic_function_node _int_assign;
        private static basic_function_node _int_unmin;
        private static basic_function_node _int_not;
        private static basic_function_node _int_add;
        private static basic_function_node _int_sub;
        private static basic_function_node _int_mul;
        private static basic_function_node _int_div;
        private static basic_function_node _int_mod;
        private static basic_function_node _int_idiv;
        private static basic_function_node _int_gr;
        private static basic_function_node _int_greq;
        private static basic_function_node _int_sm;
        private static basic_function_node _int_smeq;
        private static basic_function_node _int_eq;
        private static basic_function_node _int_noteq;
        private static basic_function_node _int_and;
        private static basic_function_node _int_or;
        private static basic_function_node _int_xor;
        private static basic_function_node _int_shl;
        private static basic_function_node _int_shr;

        //uint
        private static basic_function_node _uint_to_byte;
        private static basic_function_node _uint_to_sbyte;
        private static basic_function_node _uint_to_short;
        private static basic_function_node _uint_to_ushort;
        private static basic_function_node _uint_to_int;
        private static basic_function_node _uint_to_long;
        private static basic_function_node _uint_to_ulong;
        private static basic_function_node _uint_to_char;
        private static basic_function_node _uint_to_float;
        private static basic_function_node _uint_to_double;
        private static basic_function_node _uint_assign;
        private static basic_function_node _uint_unmin;
        private static basic_function_node _uint_not;
        private static basic_function_node _uint_gr;
        private static basic_function_node _uint_greq;
        private static basic_function_node _uint_sm;
        private static basic_function_node _uint_smeq;
        private static basic_function_node _uint_eq;
        private static basic_function_node _uint_noteq;
        private static basic_function_node _uint_add;
        private static basic_function_node _uint_sub;
        private static basic_function_node _uint_mul;
        private static basic_function_node _uint_idiv;
        private static basic_function_node _uint_mod;
        private static basic_function_node _uint_div;
        private static basic_function_node _uint_and;
        private static basic_function_node _uint_or;
        private static basic_function_node _uint_xor;
        private static basic_function_node _uint_shl;
        private static basic_function_node _uint_shr;
        
        //long
        private static basic_function_node _long_to_byte;
        private static basic_function_node _long_to_sbyte;
        private static basic_function_node _long_to_short;
        private static basic_function_node _long_to_ushort;
        private static basic_function_node _long_to_int;
        private static basic_function_node _long_to_uint;
        private static basic_function_node _long_to_ulong;
        private static basic_function_node _long_to_char;
        private static basic_function_node _long_to_float;
        private static basic_function_node _long_to_double;
        private static basic_function_node _long_assign;
        private static basic_function_node _long_unmin;
        private static basic_function_node _long_not;
        private static basic_function_node _long_add;
        private static basic_function_node _long_sub;
        private static basic_function_node _long_mul;
        private static basic_function_node _long_div;
        private static basic_function_node _long_mod;
        private static basic_function_node _long_idiv;
        private static basic_function_node _long_gr;
        private static basic_function_node _long_greq;
        private static basic_function_node _long_sm;
        private static basic_function_node _long_smeq;
        private static basic_function_node _long_eq;
        private static basic_function_node _long_noteq;
        private static basic_function_node _long_and;
        private static basic_function_node _long_or;
        private static basic_function_node _long_xor;
        private static basic_function_node _long_shl;
        private static basic_function_node _long_shr;

        //ulong
        private static basic_function_node _ulong_to_byte;
        private static basic_function_node _ulong_to_sbyte;
        private static basic_function_node _ulong_to_short;
        private static basic_function_node _ulong_to_ushort;
        private static basic_function_node _ulong_to_int;
        private static basic_function_node _ulong_to_uint;
        private static basic_function_node _ulong_to_long;
        private static basic_function_node _ulong_to_char;
        private static basic_function_node _ulong_to_float;
        private static basic_function_node _ulong_to_double;
        private static basic_function_node _ulong_assign;
        private static basic_function_node _ulong_unmin;
        private static basic_function_node _ulong_not;
        private static basic_function_node _ulong_gr;
        private static basic_function_node _ulong_greq;
        private static basic_function_node _ulong_sm;
        private static basic_function_node _ulong_smeq;
        private static basic_function_node _ulong_eq;
        private static basic_function_node _ulong_noteq;
        private static basic_function_node _ulong_add;
        private static basic_function_node _ulong_sub;
        private static basic_function_node _ulong_mul;
        private static basic_function_node _ulong_idiv;
        private static basic_function_node _ulong_mod;
        private static basic_function_node _ulong_div;
        private static basic_function_node _ulong_and;
        private static basic_function_node _ulong_or;
        private static basic_function_node _ulong_xor;
        private static basic_function_node _ulong_shl;
        private static basic_function_node _ulong_shr;

        //float
        private static basic_function_node _float_to_byte;
        private static basic_function_node _float_to_sbyte;
        private static basic_function_node _float_to_short;
        private static basic_function_node _float_to_ushort;
        private static basic_function_node _float_to_int;
        private static basic_function_node _float_to_uint;
        private static basic_function_node _float_to_long;
        private static basic_function_node _float_to_ulong;

        private static basic_function_node _float_to_double;
        private static basic_function_node _float_assign;
        private static basic_function_node _float_unmin;
        private static basic_function_node _float_add;
        private static basic_function_node _float_sub;
        private static basic_function_node _float_mul;
        private static basic_function_node _float_div;
        private static basic_function_node _float_gr;
        private static basic_function_node _float_greq;
        private static basic_function_node _float_sm;
        private static basic_function_node _float_smeq;
        private static basic_function_node _float_eq;
        private static basic_function_node _float_noteq;

        //real  (double)
        private static basic_function_node _double_to_byte;
        private static basic_function_node _double_to_sbyte;
        private static basic_function_node _double_to_short;
        private static basic_function_node _double_to_ushort;
        private static basic_function_node _double_to_int;
        private static basic_function_node _double_to_uint;
        private static basic_function_node _double_to_long;
        private static basic_function_node _double_to_ulong;
        private static basic_function_node _double_to_char;
        private static basic_function_node _double_to_float;
        private static basic_function_node _real_assign;
        private static basic_function_node _real_unmin;
        private static basic_function_node _real_add;
        private static basic_function_node _real_sub;
        private static basic_function_node _real_mul;
        private static basic_function_node _real_div;
        private static basic_function_node _real_gr;
        private static basic_function_node _real_greq;
        private static basic_function_node _real_sm;
        private static basic_function_node _real_smeq;
        private static basic_function_node _real_eq;
        private static basic_function_node _real_noteq;

        //char
        private static basic_function_node _char_to_byte;
        private static basic_function_node _char_to_sbyte;
        private static basic_function_node _char_to_short;
        private static basic_function_node _char_to_ushort;
        private static basic_function_node _char_to_int;
        private static basic_function_node _char_to_uint;
        private static basic_function_node _char_to_long;
        private static basic_function_node _char_to_ulong;
        private static basic_function_node _char_to_float;
        private static basic_function_node _char_to_double;
        private static basic_function_node _char_assign;
        private static basic_function_node _char_gr;
        private static basic_function_node _char_greq;
        private static basic_function_node _char_sm;
        private static basic_function_node _char_smeq;
        private static basic_function_node _char_eq;
        private static basic_function_node _char_noteq;
        private static function_node _char_add;

        //bool
        private static basic_function_node _bool_assign;
        private static basic_function_node _bool_not;
        private static basic_function_node _bool_and;
        private static basic_function_node _bool_or;
        private static basic_function_node _bool_xor;
        private static basic_function_node _bool_gr;
        private static basic_function_node _bool_greq;
        private static basic_function_node _bool_sm;
        private static basic_function_node _bool_smeq;
        private static basic_function_node _bool_eq;
        private static basic_function_node _bool_noteq;
        private static basic_function_node _bool_to_int;
        private static basic_function_node _bool_to_byte;
        private static basic_function_node _bool_to_sbyte;
        private static basic_function_node _bool_to_short;
        private static basic_function_node _bool_to_ushort;
        private static basic_function_node _bool_to_uint;
        private static basic_function_node _bool_to_long;
        private static basic_function_node _bool_to_ulong;
        private static basic_function_node _enum_gr;
        private static basic_function_node _enum_greq;
        private static basic_function_node _enum_sm;
        private static basic_function_node _enum_smeq;
        
        //string
        private static function_node _string_add;
        private static function_node _char_to_string;

        private static basic_function_node _obj_to_obj;

        private static basic_function_node _int64_to_pointer;
        private static basic_function_node _pointer_to_int64;

        private static compiled_function_node _resize_func;

        private static System.StringComparer _string_comparer = StringComparer.Ordinal;

        private static basic_function_node make_unary_function(type_node param_type,
            SemanticTree.basic_function_type bft,type_node ret_val_type)
        {
            basic_function_node bfn = new basic_function_node(bft, ret_val_type, true);
            basic_parameter bpar = new basic_parameter(StringConstants.unary_param_name, param_type,
                SemanticTree.parameter_type.value, bfn);
            bfn.parameters.AddElement(bpar);
            add_stand_type(bft, bfn);
            return bfn;
        }

        public static basic_function_node make_common_binary_operation(string operator_name,
            type_node def_type,type_node left,type_node right, 
            SemanticTree.basic_function_type bft,type_node ret_value_type)
        {
            basic_function_node bfn = new basic_function_node(bft, ret_value_type,true,operator_name);
            basic_parameter par_left = new basic_parameter(StringConstants.left_param_name, left,
                SemanticTree.parameter_type.value, bfn);
            basic_parameter par_right = new basic_parameter(StringConstants.right_param_name, right,
                SemanticTree.parameter_type.value, bfn);
            bfn.parameters.AddElement(par_left);
            bfn.parameters.AddElement(par_right);
            def_type.add_name(operator_name, new SymbolInfo(bfn));
            add_stand_type(bft, bfn);
            return bfn;
        }

        public static basic_function_node make_unary_operator(string operator_name, type_node to,
            SemanticTree.basic_function_type bft, type_node ret_value_type)
        {
            basic_function_node bfn = new basic_function_node(bft, ret_value_type,true, operator_name);
            basic_parameter par = new basic_parameter(StringConstants.unary_param_name, to,
                SemanticTree.parameter_type.value, bfn);
            bfn.parameters.AddElement(par);
            to.add_name(operator_name, new SymbolInfo(bfn));
            add_stand_type(bft, bfn);
            return bfn;
        }

        public static basic_function_node make_unary_operator(string operator_name, type_node to, SemanticTree.basic_function_type bft)
        {
            return make_unary_operator(operator_name, to, bft, to);
        }

        public static basic_function_node make_unary_empty_operator(string operator_name, type_node to,
            type_node ret_value_type)
        {
            basic_function_node bfn = create_emty_function(ret_value_type, operator_name);            
            to.add_name(operator_name,new SymbolInfo(bfn));
            return bfn;
        }

        public static basic_function_node make_binary_operator(string operator_name, type_node to,
            SemanticTree.basic_function_type bft, type_node ret_value_type)
        {
            return make_common_binary_operation(operator_name, to, to, to, bft, ret_value_type);
        }

        public static basic_function_node make_binary_operator(string operator_name, type_node to, SemanticTree.basic_function_type bft)
        {
            return make_binary_operator(operator_name, to, bft, to);
        }

        public static basic_function_node make_binary_operator(string operator_name, type_node to)
        {
            return make_binary_operator(operator_name, to, PascalABCCompiler.SemanticTree.basic_function_type.none, to);
        }

        public static basic_function_node make_type_conversion(type_node from, type_node to, type_compare tc,
            SemanticTree.basic_function_type bft)
        {
            return make_type_conversion(from, to, tc, bft, true);
        }

        public static basic_function_node make_type_conversion(type_node from, type_node to, type_compare tc,
            SemanticTree.basic_function_type bft, bool is_implicit)
        {
            basic_function_node conv_method = new basic_function_node(bft, to,false);
            basic_parameter bp = new basic_parameter(StringConstants.unary_param_name,
                from, SemanticTree.parameter_type.value, conv_method);
            conv_method.parameters.AddElement(bp);

            type_table.add_type_conversion_from_defined(from, to, conv_method, tc, is_implicit);
            //type_intersection_node inter_node = new type_intersection_node(tc);
            //inter_node.this_to_another = new type_conversion(conv_method,!is_implicit);
            //from.add_intersection_node(to, inter_node);
			add_stand_type(bft, conv_method);
            return conv_method;
        }
        public static basic_function_node make_generated_type_conversion(type_node from, type_node to, type_compare tc,
            SemanticTree.basic_function_type bft, bool is_implicit)
        {
            basic_function_node conv_method = new basic_function_node(bft, to, false);
            basic_parameter bp = new basic_parameter(StringConstants.unary_param_name,
                from, SemanticTree.parameter_type.value, conv_method);
            conv_method.parameters.AddElement(bp);
            type_table.add_generated_type_conversion_from_defined(from, to, conv_method, tc, is_implicit);
            return conv_method;
        }

        public static void add_function_to_type(string oper_name, type_node to, function_node fn)
        {
            to.add_name(oper_name, new SymbolInfo(fn));
        }
        public static void add_generated_funtion_to_type(string oper_name, type_node to, function_node fn)
        {
            to.add_generated_name(oper_name, new SymbolInfo(fn));
        }

        private static basic_function_node create_oti_method(SemanticTree.basic_function_type bft, type_node type)
        {
            basic_function_node bfn = new basic_function_node(bft, type,true);
            basic_parameter cp = new basic_parameter(StringConstants.unary_param_name, type,
                SemanticTree.parameter_type.value, bfn);
            bfn.parameters.AddElement(cp);
			add_stand_type(bft, bfn);
            return bfn;
        }

        private static basic_function_node create_oti_method(SemanticTree.basic_function_type bft, type_node type, SemanticTree.parameter_type pt)
        {
            basic_function_node bfn = new basic_function_node(bft, type, true);
            basic_parameter cp = new basic_parameter(StringConstants.unary_param_name, type,
                pt, bfn);
            bfn.parameters.AddElement(cp);
            //TODO: Важен порядок вызовов.
            add_stand_type(bft, bfn);
            return bfn;
        }

        private static bool compare_ordinal_type(constant_node left,constant_node right,compile_time_executor cte,
            location call_location)
        {
            expression_node ret_expr=cte(call_location,left,right);
            if (ret_expr == null)
            {
                throw new CompilerInternalError("Expected compile-time executed expression.");
            }
            bool_const_node bcn = ret_expr as bool_const_node;
            if (bcn == null)
            {
                throw new CompilerInternalError("Expected bool value.");
            }
            return bcn.constant_value;
        }

        private static expression_node inc_value_compile_time_executor(location call_location, expression_node[] expr)
        {
            return null;
            /*
            if (expr.Length != 1)
            {
                return null;
            }
            expression_node value_to_inc = expr[0];
            internal_interface ii=value_to_inc.type.get_internal_interface(internal_interface_kind.ordinal_interface);
            if (ii == null)
            {
                throw new CompilerInternalError("This method must be called only with ordinal types.");
            }
            ordinal_type_interface oti = (ordinal_type_interface)ii;
            constant_node cn = value_to_inc as constant_node;
            if (cn != null)
            {
                if (compare_ordinal_type(cn, oti.upper_value, oti.greater_eq_method.compile_time_executor,call_location))
                {
                    throw new CanNotIncrementOrdinalTypeValue(cn);
                }
                return (oti.internal_inc_value.compile_time_executor(call_location,cn));
            }
            else
            {

            }
            */
        }

        private static expression_node dec_value_compile_time_executor(location call_location, expression_node[] expr)
        {
            return null;
        }

        private static expression_node inc_compile_time_executor(location call_location, expression_node[] expr)
        {
            return null;
        }

        private static expression_node dec_compile_time_executor(location call_location, expression_node[] expr)
        {
            return null;
        }

        private static basic_function_node create_inc_value_method(SemanticTree.basic_function_type bft, type_node type)
        {
            basic_function_node bfn = create_oti_method(bft, type,SemanticTree.parameter_type.value);
            bfn.compile_time_executor = inc_value_compile_time_executor;
            return bfn;
        }

        private static basic_function_node create_dec_value_method(SemanticTree.basic_function_type bft, type_node type)
        {
            basic_function_node bfn = create_oti_method(bft, type, SemanticTree.parameter_type.value);
            bfn.compile_time_executor = dec_value_compile_time_executor;
            return bfn;
        }

        private static basic_function_node create_inc_method(SemanticTree.basic_function_type bft, type_node type)
        {
            basic_function_node bfn = create_oti_method(bft, type, SemanticTree.parameter_type.var);
            bfn.compile_time_executor = inc_compile_time_executor;
            return bfn;
        }

        private static basic_function_node create_dec_method(SemanticTree.basic_function_type bft, type_node type)
        {
            basic_function_node bfn = create_oti_method(bft, type, SemanticTree.parameter_type.var);
            bfn.compile_time_executor = dec_compile_time_executor;
            return bfn;
        }

        private static function_node make_compiled_operator(compiled_type_node declaring_type,
            string method_name,string new_name,compiled_type_node left_type,compiled_type_node right_type)
        {
            compiled_function_node fn=NetHelper.NetHelper.get_compiled_method(declaring_type, method_name, left_type, right_type);
            declaring_type.add_name(new_name,new SymbolInfo(fn));
            return fn;
        }

        private static function_node make_compiled_operator(compiled_type_node declaring_type,
            string method_name, string new_name,compiled_type_node operand_type)
        {
            return make_binary_compiled_operator(declaring_type, declaring_type, method_name, new_name, operand_type);
        }

        private static function_node make_binary_compiled_operator(compiled_type_node declaring_type, compiled_type_node add_to_type,
            string method_name, string new_name, compiled_type_node operand_type)
        {
            compiled_function_node fn = NetHelper.NetHelper.get_compiled_method(declaring_type, method_name, operand_type, operand_type);
            add_to_type.add_name(new_name, new SymbolInfo(fn));
            fn.IsOperator = true;
            return fn;
        }

        private static function_node make_binary_compiled_operator(compiled_type_node operand_type,
            string method_name, string new_name)
        {
            return make_compiled_operator(operand_type, method_name, new_name, operand_type, operand_type);
        }

        private static function_node make_unary_compiled_operator(compiled_type_node operand_type,
            string method_name, string new_name)
        {
            return make_compiled_operator(operand_type, method_name, new_name, operand_type);
        }

        private static int int_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.int_const_node)
                if (cn.semantic_node_type != semantic_node_type.enum_const)
                {
                    throw new CompilerInternalError("Error in static type conversion");
                }
#endif
            int_const_node icn = null;
            if (cn.semantic_node_type == semantic_node_type.enum_const)
                icn = new int_const_node((cn as enum_const_node).constant_value, cn.location);
            else
                icn = (int_const_node)cn;
            return icn.constant_value;
        }

        private static int long_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.long_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            long_const_node icn = null;
            icn = (long_const_node)cn;
            return (int)icn.constant_value;
        }
		
        private static int uint_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.uint_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            uint_const_node icn = null;
            icn = (uint_const_node)cn;
            return (int)icn.constant_value;
        }
        
        private static int ulong_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.ulong_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            ulong_const_node icn = null;
            icn = (ulong_const_node)cn;
            return (int)icn.constant_value;
        }
        
        private static int byte_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.byte_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            byte_const_node icn = (byte_const_node)cn;
            return (int)icn.constant_value;
        }

        private static int char_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.char_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            char_const_node icn = (char_const_node)cn;
            return ((int)icn.constant_value);
        }

        private static int sbyte_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.sbyte_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            sbyte_const_node icn = (sbyte_const_node)cn;
            return ((int)icn.constant_value);
        }

        private static int short_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.short_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            short_const_node icn = (short_const_node)cn;
            return ((int)icn.constant_value);
        }

        private static int ushort_to_int(constant_node cn)
        {
#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.ushort_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            ushort_const_node icn = (ushort_const_node)cn;
            return ((int)icn.constant_value);
        }
      
        
        private static int bool_to_int(constant_node cn)
        {
        	#if (DEBUG)
            if (cn.semantic_node_type != semantic_node_type.bool_const_node)
            {
                throw new CompilerInternalError("Error in static type conversion");
            }
#endif
            bool_const_node icn = (bool_const_node)cn;
            return (Convert.ToInt32(icn.constant_value));
        }
        
        internal static expression_node delegated_empty_method(location call_loc,expression_node[] expr)
        {
            if (expr.Length != 1)
            {
                return null;
            }
            return expr[0];
        }
        /*
        private static expression_node char_to_int(location call_location,expression_node[] expr)
        {
            if (expr.Length != 1)
            {
                return null;
            }
            if (expr[0].type != char_type)
            {
                return null;
            }
            char_const_node ccn = expr[0] as char_const_node;
            if (ccn == null)
            {
                return null;
            }
            return new int_const_node((int)ccn.constant_value, ccn.location);
        }
        */
        private static basic_function_node create_emty_function(type_node ret_type, string name)
        {
            basic_function_node bfn = new basic_function_node(SemanticTree.basic_function_type.none, ret_type, true, name);
            basic_parameter par = new basic_parameter(StringConstants.unary_param_name, ret_type,
                SemanticTree.parameter_type.value, bfn);
            bfn.parameters.AddElement(par);
            bfn.compile_time_executor = delegated_empty_method;
			add_stand_type(SemanticTree.basic_function_type.none, bfn);
            return bfn;
        }
        /*
        private static void mark_byte_as_ordinal()
        {
            basic_function_node inc_value_method = create_inc_value_method(SemanticTree.basic_function_type.binc, _byte_type);
            basic_function_node dec_value_method = create_dec_value_method(SemanticTree.basic_function_type.bdec, _byte_type);

            basic_function_node inc_method = create_inc_method(SemanticTree.basic_function_type.binc, _byte_type);
            basic_function_node dec_method = create_dec_method(SemanticTree.basic_function_type.bdec, _byte_type);

            SymbolInfo si = _byte_type.find_in_type(StringConstants.greq_name);
            basic_function_node greq = (basic_function_node)si.sym_info;

            si = _byte_type.find(StringConstants.smeq_name);
            basic_function_node loeq = (basic_function_node)si.sym_info;

            constant_node cn_max = new byte_const_node(byte.MaxValue, null);
            constant_node cn_min = new byte_const_node(byte.MinValue, null);

            basic_function_node i2i_method = create_emty_function(byte_type);

            ordinal_type_to_int ordinal_type_to_int = byte_to_int;

            ordinal_type_interface oti = new ordinal_type_interface(inc_method, dec_method, inc_value_method, dec_value_method,
                internal_inc_value, internal_dec_value, loeq, greq, cn_min, cn_max, i2i_method, ordinal_type_to_int);

            _byte_type.add_internal_interface(oti);
        }
        */
        private static void mark_type_as_ordinal(type_node type,
            SemanticTree.basic_function_type inc,SemanticTree.basic_function_type dec,
            SemanticTree.basic_function_type vinc, SemanticTree.basic_function_type vdec,
            constant_node lower_value, constant_node upper_value,
            function_node t2i,ordinal_type_to_int t2i_comp)
        {
            basic_function_node inc_value = create_oti_method(inc, type, SemanticTree.parameter_type.value);
            basic_function_node dec_value = create_oti_method(dec, type, SemanticTree.parameter_type.value);

            basic_function_node inc_var = create_oti_method(vinc, type, SemanticTree.parameter_type.var);
            basic_function_node dec_var = create_oti_method(vdec, type, SemanticTree.parameter_type.var);

            SymbolInfo si = type.find_first_in_type(StringConstants.greq_name);
            basic_function_node greq = (basic_function_node)si.sym_info;
        
            si = type.find(StringConstants.smeq_name).FirstOrDefault();
            basic_function_node loeq = (basic_function_node)si.sym_info;
            
			
            si = type.find(StringConstants.sm_name).FirstOrDefault();
            basic_function_node lo = (basic_function_node)si.sym_info;
            
            si = type.find(StringConstants.gr_name).FirstOrDefault();
            basic_function_node gr = (basic_function_node)si.sym_info;

            ordinal_type_interface oti = new ordinal_type_interface(inc_value, dec_value, inc_var, dec_var,
                loeq, greq, lo, gr, lower_value, upper_value, t2i, t2i_comp);

            type.add_internal_interface(oti);
        }
        /*
        private static expression_node runtime_inc_int(location call_location, expression_node[] exprs)
        {
            if (exprs.Length != 1)
            {
                return null;
            }
            int_const_node icn = exprs[0] as int_const_node;
            if (icn == null)
            {
                return null;
            }
            return (new int_const_node(icn.constant_value + 1, call_location));
        }

        private static expression_node runtime_dec_int(location call_location, expression_node[] exprs)
        {
            if (exprs.Length != 1)
            {
                return null;
            }
            int_const_node icn = exprs[0] as int_const_node;
            if (icn == null)
            {
                return null;
            }
            return (new int_const_node(icn.constant_value - 1, call_location));
        }
        
        private static void mark_int_as_ordinal()
        {
            basic_function_node internal_inc_value = create_oti_method(SemanticTree.basic_function_type.iinc, _integer_type);
            internal_inc_value.compile_time_executor = runtime_inc_int;
            basic_function_node internal_dec_value = create_oti_method(SemanticTree.basic_function_type.idec, _integer_type);
            internal_dec_value.compile_time_executor = runtime_dec_int;

            basic_function_node inc_value_method = create_inc_value_method(SemanticTree.basic_function_type.iinc, _integer_type);
            basic_function_node dec_value_method = create_dec_value_method(SemanticTree.basic_function_type.idec, _integer_type);

            basic_function_node inc_method = create_inc_method(SemanticTree.basic_function_type.iinc, _integer_type);
            basic_function_node dec_method = create_dec_method(SemanticTree.basic_function_type.idec, _integer_type);

            //basic_function_node inc_method = create_oti_method(SemanticTree.basic_function_type.iinc, _integer_type);
            //basic_function_node dec_method = create_oti_method(SemanticTree.basic_function_type.idec, _integer_type);

            SymbolInfo si = _integer_type.find_in_type(StringConstants.greq_name);
            basic_function_node greq = (basic_function_node)si.sym_info;

            si = _integer_type.find(StringConstants.smeq_name);
            basic_function_node loeq = (basic_function_node)si.sym_info;

            //DarkStar Changed MinValue->MaxValue
            constant_node cn_max = new int_const_node(int.MaxValue,null);
            constant_node cn_min = new int_const_node(int.MinValue,null);

            basic_function_node i2i_method = create_emty_function(integer_type);

            ordinal_type_to_int ordinal_type_to_int = int_to_int;

            ordinal_type_interface oti = new ordinal_type_interface(inc_method,dec_method,inc_value_method,dec_value_method,
                internal_inc_value,internal_dec_value,loeq,greq,cn_min,cn_max,i2i_method,ordinal_type_to_int);

            _integer_type.add_internal_interface(oti);
        }

        private static expression_node runtime_inc_char(location call_location, expression_node[] exprs)
        {
            if (exprs.Length != 1)
            {
                return null;
            }
            char_const_node icn = exprs[0] as char_const_node;
            if (icn == null)
            {
                return null;
            }
            return (new int_const_node((icn.constant_value+1) , call_location));
        }

        private static expression_node runtime_dec_char(location call_location, expression_node[] exprs)
        {
            if (exprs.Length != 1)
            {
                return null;
            }
            char_const_node icn = exprs[0] as char_const_node;
            if (icn == null)
            {
                return null;
            }
            return (new int_const_node((icn.constant_value-1), call_location));
        }

        private static void mark_char_as_ordinal()
        {
            basic_function_node internal_inc_value = create_oti_method(SemanticTree.basic_function_type.cinc, _integer_type);
            internal_inc_value.compile_time_executor = runtime_inc_char;
            basic_function_node internal_dec_value = create_oti_method(SemanticTree.basic_function_type.cdec, _integer_type);
            internal_dec_value.compile_time_executor = runtime_dec_char;

            basic_function_node inc_value_method = create_inc_value_method(SemanticTree.basic_function_type.iinc, _integer_type);
            basic_function_node dec_value_method = create_dec_value_method(SemanticTree.basic_function_type.idec, _integer_type);

            basic_function_node inc_method = create_inc_method(SemanticTree.basic_function_type.iinc, _integer_type);
            basic_function_node dec_method = create_dec_method(SemanticTree.basic_function_type.iinc, _integer_type);

            SymbolInfo si = _char_type.find(StringConstants.greq_name);
            basic_function_node greq = (basic_function_node)si.sym_info;

            si = _char_type.find(StringConstants.smeq_name);
            basic_function_node loeq = (basic_function_node)si.sym_info;

            constant_node cn_max = new char_const_node(char.MaxValue,null);
            constant_node cn_min = new char_const_node(char.MinValue,null);

            basic_function_node c2i_method = new basic_function_node(SemanticTree.basic_function_type.chartoi, _integer_type,true);
            basic_parameter cp = new basic_parameter(StringConstants.unary_param_name, _char_type,
                SemanticTree.parameter_type.value, c2i_method);
            //TODO: Сделано очень плохо.
            add_stand_type(SemanticTree.basic_function_type.chartoi, c2i_method);
            c2i_method.parameters.AddElement(cp);
            c2i_method.compile_time_executor = char_to_int;

            ordinal_type_to_int ordinal_type_to_int = char_to_int;

            ordinal_type_interface oti = new ordinal_type_interface(inc_method,dec_method,inc_value_method,dec_value_method,
                internal_inc_value,internal_dec_value,loeq,greq,cn_min,cn_max,
                c2i_method,ordinal_type_to_int);

            _char_type.add_internal_interface(oti);
        }
        */
        private static void make_function_comparison(function_node left, function_node right, function_compare comp)
        {
            function_intersection_node fin = new function_intersection_node(left, right);
            fin.function_relation = comp;
            function_node.add_function_intersection(left,right,fin);
        }

        /*public static void Reset()
        {
            (string_type as compiled_type_node).reinit_scope();
        }*/


        //TODO: Замечание к TODO перед этим классом. Просто сохранить ссылки на все объекты, которые мы здесь получаем.
        private static void initialize_types()
        {
            //TODO:  (??? подумать над именами!!)
            /*                     bit
            Shortint	signed      8
            Byte        unsigned    8
            Smallint	signed      16
            Word	   	unsigned    16
            Integer	    signed      32
            Cardinal	unsigned    32
            Longint		signed      64
            ULongint	unsigned    64
            Decimal     signed      128
             */
            _byte_type = compiled_type_node.get_type_node(typeof(byte), symtab);
            _byte_type.SetName(StringConstants.byte_type_name);

            _sbyte_type = compiled_type_node.get_type_node(typeof(sbyte), symtab);
            _sbyte_type.SetName(StringConstants.sbyte_type_name);

            _short_type = compiled_type_node.get_type_node(typeof(short), symtab);
            _short_type.SetName(StringConstants.short_type_name);

            _ushort_type = compiled_type_node.get_type_node(typeof(ushort), symtab);
            _ushort_type.SetName(StringConstants.ushort_type_name);

            _integer_type = compiled_type_node.get_type_node(typeof(int), symtab);
            _integer_type.SetName(StringConstants.integer_type_name);

            _uint_type = compiled_type_node.get_type_node(typeof(uint), symtab);
            _uint_type.SetName(StringConstants.uint_type_name);

            _int64_type = compiled_type_node.get_type_node(typeof(long), symtab);
            _int64_type.SetName(StringConstants.long_type_name);

            _uint64_type = compiled_type_node.get_type_node(typeof(ulong), symtab);
            _uint64_type.SetName(StringConstants.ulong_type_name);

            _icloneable_interface = compiled_type_node.get_type_node(typeof(ICloneable), symtab);
            _ilist_interface = compiled_type_node.get_type_node(typeof(IList), symtab);
            _icollection_interface = compiled_type_node.get_type_node(typeof(ICollection));
            _ienumerable_interface = compiled_type_node.get_type_node(typeof(IEnumerable));
            _ilist1_interface = compiled_type_node.get_type_node(typeof(IList<>));

            var t = Type.GetType("System.Collections.Generic.IReadOnlyCollection`1");
            if (t != null)
                _ireadonlycollection_interface = compiled_type_node.get_type_node(t);

            t = Type.GetType("System.Collections.Generic.IReadOnlyList`1");
            if (t != null)
                _ireadonlylist_interface = compiled_type_node.get_type_node(t);

            _icollection1_interface = compiled_type_node.get_type_node(typeof(ICollection<>));
            _ienumerable1_interface = compiled_type_node.get_type_node(typeof(IEnumerable<>));
            _assert_method = compiled_function_node.get_compiled_method(typeof(System.Diagnostics.Debug).GetMethod("Assert",new Type[1]{typeof(bool)}));
            _decimal_type = compiled_type_node.get_type_node(typeof(decimal), symtab);
            //_decimal_type.SetName(StringConstants.decimal_type_name);
            make_assign_operator(_decimal_type, SemanticTree.basic_function_type.objassign);

            _bool_type = compiled_type_node.get_type_node(typeof(bool), symtab);
            _bool_type.SetName(StringConstants.bool_type_name);

            _double_type = compiled_type_node.get_type_node(typeof(double), symtab);
            _double_type.SetName(StringConstants.real_type_name);

            _float_type = compiled_type_node.get_type_node(typeof(float), symtab);
            _float_type.SetName(StringConstants.float_type_name);

            _char_type = compiled_type_node.get_type_node(typeof(char), symtab);
            _char_type.SetName(StringConstants.char_type_name);

            _string_type = compiled_type_node.get_type_node(typeof(string), symtab);
            _string_type.SetName(StringConstants.string_type_name);

            _pointer_type = compiled_type_node.get_type_node(NetHelper.NetHelper.void_ptr_type, symtab);
            _pointer_type.SetName(StringConstants.pointer_type_name);

            _object_type = compiled_type_node.get_type_node(typeof(object), symtab);
            _object_type.SetName(StringConstants.object_type_name);

            _complex_type = compiled_type_node.get_type_node(typeof(System.Numerics.Complex), symtab);
            _complex_type_constructor = compiled_constructor_node.get_compiled_constructor(_complex_type.compiled_type.GetConstructor(new Type[] { typeof(double), typeof(double)}));
            _enum_base_type = compiled_type_node.get_type_node(typeof(System.Enum), symtab);

            _delegate_base_type = compiled_type_node.get_type_node(typeof(System.MulticastDelegate), symtab);
            _delegate_base_type.SetName(StringConstants.base_delegate_type_name);

            _system_delegate_type = compiled_type_node.get_type_node(typeof(System.Delegate), symtab);

            _array_base_type = compiled_type_node.get_type_node(typeof(System.Array), symtab);
            _array_base_type.SetName(StringConstants.base_array_type_name);

            _void_type = compiled_type_node.get_type_node(typeof(void), symtab);

            _value_type = compiled_type_node.get_type_node(typeof(ValueType),symtab);

            Type delegate_type = typeof(System.Delegate);
            Type[] arr_params=new Type[2];
            arr_params[0]=delegate_type;
            arr_params[1]=delegate_type;
            _delegate_combine_method =
            compiled_function_node.get_compiled_method(delegate_type.GetMethod(StringConstants.combine_method_name, arr_params));

            _delegate_remove_method =
            compiled_function_node.get_compiled_method(delegate_type.GetMethod(StringConstants.remove_method_name, arr_params));

            _object_equals_method =
                compiled_function_node.get_compiled_method(typeof(object).GetMethod(StringConstants.object_equals_name, new Type[2] { typeof(object), typeof(object) }));

            //Это для прикола. Убрать!
            //make_common_binary_operation(StringConstants.plus_name, _integer_type, _integer_type, _byte_type, SemanticTree.basic_function_type.badd, _byte_type);


            //Это нормально? (чтобы работало i:=+15;)
            /*
            make_unary_operator(StringConstants.plus_name, _byte_type, SemanticTree.basic_function_type.none);
            make_unary_operator(StringConstants.plus_name, _integer_type, SemanticTree.basic_function_type.none);
            make_unary_operator(StringConstants.plus_name, _long_type, SemanticTree.basic_function_type.none);
            make_unary_operator(StringConstants.plus_name, _real_type, SemanticTree.basic_function_type.none);
            */            
            
            //integer type
            //Преобразования типов.
            _int_to_byte = make_type_conversion(_integer_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.itob);
            _int_to_sbyte = make_type_conversion(_integer_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.itosb);
            _int_to_short = make_type_conversion(_integer_type, _short_type, type_compare.greater_type, SemanticTree.basic_function_type.itos);
            _int_to_ushort = make_type_conversion(_integer_type, _ushort_type, type_compare.greater_type, SemanticTree.basic_function_type.itous);
            _int_to_uint = make_type_conversion(_integer_type, _uint_type, type_compare.less_type, SemanticTree.basic_function_type.itoui);
            _int_to_long = make_type_conversion(_integer_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.itol);
            _int_to_ulong = make_type_conversion(_integer_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.itoul);
            _int_to_char = make_type_conversion(_integer_type, _char_type, type_compare.greater_type, SemanticTree.basic_function_type.itochar, false);
            _int_to_double=make_type_conversion(_integer_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.itod);
            //make_type_conversion(_integer_type, _long_type, type_compare.less_type, SemanticTree.basic_function_type.itol);
            _int_to_float = make_type_conversion(_integer_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.itof);
            //Присваивание для integer.
            _int_assign=make_assign_operator(_integer_type, SemanticTree.basic_function_type.iassign);
            //Унарные операции.
            _int_unmin=make_unary_operator(StringConstants.minus_name, _integer_type, SemanticTree.basic_function_type.iunmin);
            _int_not = make_unary_operator(StringConstants.not_name, _integer_type, SemanticTree.basic_function_type.inot);
            make_unary_empty_operator(StringConstants.plus_name, _integer_type, _integer_type);
            //Арифметические операции.
            _int_add = make_binary_operator(StringConstants.plus_name, _integer_type, SemanticTree.basic_function_type.iadd);            
            _int_sub = make_binary_operator(StringConstants.minus_name, _integer_type, SemanticTree.basic_function_type.isub);
            _int_mul=make_binary_operator(StringConstants.mul_name, _integer_type, SemanticTree.basic_function_type.imul);
            _int_idiv=make_binary_operator(StringConstants.idiv_name, _integer_type, SemanticTree.basic_function_type.idiv);
            _int_mod=make_binary_operator(StringConstants.mod_name, _integer_type, SemanticTree.basic_function_type.imod);
            //Операция / для integer.
            _int_div = make_common_binary_operation(StringConstants.div_name, _integer_type, _double_type, _double_type,
                SemanticTree.basic_function_type.ddiv, _double_type);
            //_int_div = make_common_binary_operation(StringConstants.div_name, _integer_type, _integer_type, _integer_type,
            //    SemanticTree.basic_function_type.ddiv, _real_type);
            //Опрерации сравнения.
            _int_gr=make_binary_operator(StringConstants.gr_name, _integer_type, SemanticTree.basic_function_type.igr, _bool_type);
            _int_greq=make_binary_operator(StringConstants.greq_name, _integer_type, SemanticTree.basic_function_type.igreq, _bool_type);
            _int_sm=make_binary_operator(StringConstants.sm_name, _integer_type, SemanticTree.basic_function_type.ism, _bool_type);
            _int_smeq=make_binary_operator(StringConstants.smeq_name, _integer_type, SemanticTree.basic_function_type.ismeq, _bool_type);
            _int_eq=make_binary_operator(StringConstants.eq_name, _integer_type, SemanticTree.basic_function_type.ieq, _bool_type);
            _int_noteq=make_binary_operator(StringConstants.noteq_name, _integer_type, SemanticTree.basic_function_type.inoteq, _bool_type);
            //Логические опреции.
            _int_and=make_binary_operator(StringConstants.and_name, _integer_type, SemanticTree.basic_function_type.iand);
            _int_or=make_binary_operator(StringConstants.or_name, _integer_type, SemanticTree.basic_function_type.ior);
            _int_xor=make_binary_operator(StringConstants.xor_name, _integer_type, SemanticTree.basic_function_type.ixor);
            _int_shl=make_binary_operator(StringConstants.shl_name, _integer_type, SemanticTree.basic_function_type.ishl);
            _int_shr=make_binary_operator(StringConstants.shr_name, _integer_type, SemanticTree.basic_function_type.ishr);


            //byte type.
            //Assign.
            _byte_assign = make_assign_operator(_byte_type, SemanticTree.basic_function_type.bassign);
            //Преобразования типов.
            _byte_to_sbyte = make_type_conversion(_byte_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.btosb);
            _byte_to_short = make_type_conversion(_byte_type, _short_type, type_compare.less_type, SemanticTree.basic_function_type.btos);
            _byte_to_ushort = make_type_conversion(_byte_type, _ushort_type, type_compare.less_type, SemanticTree.basic_function_type.btous);
            _byte_to_uint = make_type_conversion(_byte_type, _uint_type, type_compare.less_type, SemanticTree.basic_function_type.btoui);
            _byte_to_long = make_type_conversion(_byte_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.btol);
            _byte_to_ulong = make_type_conversion(_byte_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.btoul);
            _byte_to_int = make_type_conversion(_byte_type, _integer_type, type_compare.less_type, SemanticTree.basic_function_type.btoi);
            _byte_to_char = make_type_conversion(_byte_type, _char_type, type_compare.less_type, SemanticTree.basic_function_type.btochar,false);
            _byte_to_float = make_type_conversion(_byte_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.btof);
            _byte_to_double = make_type_conversion(_byte_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.btod);
            //Унарные операции.
            _byte_unmin = make_unary_operator(StringConstants.minus_name, _byte_type, SemanticTree.basic_function_type.bunmin, _integer_type);
            _byte_not = make_unary_operator(StringConstants.not_name, _byte_type, SemanticTree.basic_function_type.bnot);
            make_unary_empty_operator(StringConstants.plus_name, _byte_type, _byte_type);
            //Опрерации сравнения.
            /*
            _byte_gr = make_binary_operator(StringConstants.gr_name, _byte_type, SemanticTree.basic_function_type.bgr, _bool_type);
            _byte_greq = make_binary_operator(StringConstants.greq_name, _byte_type, SemanticTree.basic_function_type.bgreq, _bool_type);
            _byte_sm = make_binary_operator(StringConstants.sm_name, _byte_type, SemanticTree.basic_function_type.bsm, _bool_type);
            _byte_smeq = make_binary_operator(StringConstants.smeq_name, _byte_type, SemanticTree.basic_function_type.bsmeq, _bool_type);
            _byte_eq = make_binary_operator(StringConstants.eq_name, _byte_type, SemanticTree.basic_function_type.beq, _bool_type);
            _byte_noteq = make_binary_operator(StringConstants.noteq_name, _byte_type, SemanticTree.basic_function_type.bnoteq, _bool_type);
             */

            add_function_to_type(StringConstants.not_name, _byte_type, _int_unmin);

            add_function_to_type(StringConstants.gr_name, _byte_type, _int_gr);
            add_function_to_type(StringConstants.greq_name, _byte_type, _int_greq);
            add_function_to_type(StringConstants.sm_name, _byte_type, _int_sm);
            add_function_to_type(StringConstants.smeq_name, _byte_type, _int_smeq);
            add_function_to_type(StringConstants.eq_name, _byte_type, _int_eq);
            add_function_to_type(StringConstants.noteq_name, _byte_type, _int_noteq);

            add_function_to_type(StringConstants.plus_name, _byte_type, _int_add);
            add_function_to_type(StringConstants.minus_name, _byte_type, _int_sub);
            add_function_to_type(StringConstants.mul_name, _byte_type, _int_mul);
            add_function_to_type(StringConstants.div_name, _byte_type, _int_div);
			add_function_to_type(StringConstants.idiv_name, _byte_type, _int_idiv);
			add_function_to_type(StringConstants.mod_name, _byte_type, _int_mod);
			
            /*add_funtion_to_type(StringConstants.and_name, _byte_type, _int_and);
            add_funtion_to_type(StringConstants.or_name, _byte_type, _int_or);
            add_funtion_to_type(StringConstants.xor_name, _byte_type, _int_xor);*/
            //Арифметические операции.
            //_byte_add = make_common_binary_operation(StringConstants.plus_name, _byte_type, _byte_type,_byte_type, SemanticTree.basic_function_type.badd, _integer_type);
            /*
            _byte_sub = make_common_binary_operation(StringConstants.minus_name, _byte_type, _byte_type, _byte_type, SemanticTree.basic_function_type.bsub, _integer_type);
            _byte_mul = make_common_binary_operation(StringConstants.mul_name, _byte_type, _byte_type, _byte_type, SemanticTree.basic_function_type.bmul, _integer_type);
            _byte_idiv = make_common_binary_operation(StringConstants.idiv_name, _byte_type, _byte_type, _byte_type, SemanticTree.basic_function_type.bdiv, _integer_type);
            _byte_mod = make_common_binary_operation(StringConstants.mod_name, _byte_type, _byte_type, _byte_type, SemanticTree.basic_function_type.bmod, _integer_type);
            */
            //Операция / для byte.
            _byte_div = make_common_binary_operation(StringConstants.div_name, _byte_type, _double_type, _double_type,
                SemanticTree.basic_function_type.ddiv, _double_type);
            //Логические опреции.
            
            _byte_and = make_common_binary_operation(StringConstants.and_name, _byte_type, _byte_type, _byte_type, SemanticTree.basic_function_type.band, _byte_type);
            _byte_or = make_common_binary_operation(StringConstants.or_name, _byte_type, _byte_type, _byte_type, SemanticTree.basic_function_type.bor, _byte_type);
            _byte_xor = make_common_binary_operation(StringConstants.xor_name, _byte_type, _byte_type, _byte_type, SemanticTree.basic_function_type.bxor, _byte_type);
            
            add_function_to_type(StringConstants.shl_name, _byte_type, _int_shl);
            add_function_to_type(StringConstants.shr_name, _byte_type, _int_shr);
            //_byte_shl = make_binary_operator(StringConstants.shl_name, _byte_type, SemanticTree.basic_function_type.bshl);
            //_byte_shr = make_binary_operator(StringConstants.shr_name, _byte_type, SemanticTree.basic_function_type.bshr);

            //sbyte type.
            //Assign.
            _sbyte_assign = make_assign_operator(_sbyte_type, SemanticTree.basic_function_type.sbassign);
            //Преобразования sbyteов.
	        _sbyte_to_byte = make_type_conversion(_sbyte_type, _byte_type, type_compare.less_type, SemanticTree.basic_function_type.sbtob);
            _sbyte_to_short = make_type_conversion(_sbyte_type, _short_type, type_compare.less_type, SemanticTree.basic_function_type.sbtos);
            _sbyte_to_ushort = make_type_conversion(_sbyte_type, _ushort_type, type_compare.less_type, SemanticTree.basic_function_type.sbtous);
            _sbyte_to_uint = make_type_conversion(_sbyte_type, _uint_type, type_compare.less_type, SemanticTree.basic_function_type.sbtoui);
            _sbyte_to_long = make_type_conversion(_sbyte_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.sbtol);
            _sbyte_to_ulong = make_type_conversion(_sbyte_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.sbtoul);
            _sbyte_to_int = make_type_conversion(_sbyte_type, _integer_type, type_compare.less_type, SemanticTree.basic_function_type.sbtoi);
            _sbyte_to_char = make_type_conversion(_sbyte_type, _char_type, type_compare.less_type, SemanticTree.basic_function_type.sbtochar,false);
            _sbyte_to_float = make_type_conversion(_sbyte_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.sbtof);
            _sbyte_to_double = make_type_conversion(_sbyte_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.sbtod);
            //Унарные операции.
            _sbyte_unmin = make_unary_operator(StringConstants.minus_name, _sbyte_type, SemanticTree.basic_function_type.sbunmin, _integer_type);
            _sbyte_not = make_unary_operator(StringConstants.not_name, _sbyte_type, SemanticTree.basic_function_type.sbnot);
            make_unary_empty_operator(StringConstants.plus_name, _sbyte_type, _sbyte_type);

            add_function_to_type(StringConstants.not_name, _sbyte_type, _int_unmin);

            add_function_to_type(StringConstants.gr_name, _sbyte_type, _int_gr);
            add_function_to_type(StringConstants.greq_name, _sbyte_type, _int_greq);
            add_function_to_type(StringConstants.sm_name, _sbyte_type, _int_sm);
            add_function_to_type(StringConstants.smeq_name, _sbyte_type, _int_smeq);
            add_function_to_type(StringConstants.eq_name, _sbyte_type, _int_eq);
            add_function_to_type(StringConstants.noteq_name, _sbyte_type, _int_noteq);

            add_function_to_type(StringConstants.plus_name, _sbyte_type, _int_add);
            add_function_to_type(StringConstants.minus_name, _sbyte_type, _int_sub);
            add_function_to_type(StringConstants.mul_name, _sbyte_type, _int_mul);
            add_function_to_type(StringConstants.div_name, _sbyte_type, _int_div);
			add_function_to_type(StringConstants.idiv_name, _sbyte_type, _int_idiv);
			add_function_to_type(StringConstants.mod_name, _sbyte_type, _int_mod);
			
            //Операция / для byte.
            _sbyte_div = make_common_binary_operation(StringConstants.div_name, _sbyte_type, _double_type, _double_type,
                SemanticTree.basic_function_type.ddiv, _double_type);
            //Логические опреции.
            
            _sbyte_and = make_common_binary_operation(StringConstants.and_name, _sbyte_type, _sbyte_type, _sbyte_type, SemanticTree.basic_function_type.sband, _sbyte_type);
            _sbyte_or = make_common_binary_operation(StringConstants.or_name, _sbyte_type, _sbyte_type, _sbyte_type, SemanticTree.basic_function_type.sbor, _sbyte_type);
            _sbyte_xor = make_common_binary_operation(StringConstants.xor_name, _sbyte_type,_sbyte_type, _sbyte_type, SemanticTree.basic_function_type.sbxor, _sbyte_type);
            
            add_function_to_type(StringConstants.shl_name, _sbyte_type, _int_shl);
            add_function_to_type(StringConstants.shr_name, _sbyte_type, _int_shr);

            	    //short type.
            //Assign.
            _short_assign = make_assign_operator(_short_type, SemanticTree.basic_function_type.sassign);
            //Преобразования shortов.
	        _short_to_byte = make_type_conversion(_short_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.stob);
            _short_to_sbyte = make_type_conversion(_short_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.stosb);
            _short_to_ushort = make_type_conversion(_short_type, _ushort_type, type_compare.less_type, SemanticTree.basic_function_type.stous);
            _short_to_uint = make_type_conversion(_short_type, _uint_type, type_compare.less_type, SemanticTree.basic_function_type.stoui);
            _short_to_long = make_type_conversion(_short_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.stol);
            _short_to_ulong = make_type_conversion(_short_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.stoul);
            _short_to_int = make_type_conversion(_short_type, _integer_type, type_compare.less_type, SemanticTree.basic_function_type.stoi);
            _short_to_char = make_type_conversion(_short_type, _char_type, type_compare.greater_type, SemanticTree.basic_function_type.stochar,false);
            _short_to_float = make_type_conversion(_short_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.stof);
            _short_to_double = make_type_conversion(_short_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.stod);
            //Унарные операции.
            _short_unmin = make_unary_operator(StringConstants.minus_name, _short_type, SemanticTree.basic_function_type.sunmin,_integer_type);
            _short_not = make_unary_operator(StringConstants.not_name, _short_type, SemanticTree.basic_function_type.snot);
            make_unary_empty_operator(StringConstants.plus_name, _short_type, _short_type);

            add_function_to_type(StringConstants.not_name, _short_type, _int_unmin);

            add_function_to_type(StringConstants.gr_name, _short_type, _int_gr);
            add_function_to_type(StringConstants.greq_name, _short_type, _int_greq);
            add_function_to_type(StringConstants.sm_name, _short_type, _int_sm);
            add_function_to_type(StringConstants.smeq_name, _short_type, _int_smeq);
            add_function_to_type(StringConstants.eq_name, _short_type, _int_eq);
            add_function_to_type(StringConstants.noteq_name, _short_type, _int_noteq);

            add_function_to_type(StringConstants.plus_name, _short_type, _int_add);
            add_function_to_type(StringConstants.minus_name, _short_type, _int_sub);
            add_function_to_type(StringConstants.mul_name, _short_type, _int_mul);
            add_function_to_type(StringConstants.div_name, _short_type, _int_div);
			add_function_to_type(StringConstants.idiv_name, _short_type, _int_idiv);
			add_function_to_type(StringConstants.mod_name, _short_type, _int_mod);
			
            /*add_funtion_to_type(StringConstants.and_name, _short_type, _int_and);
            add_funtion_to_type(StringConstants.or_name, _short_type, _int_or);
            add_funtion_to_type(StringConstants.xor_name, _short_type, _int_xor);*/

            //Операция / для byte.
            _short_div = make_common_binary_operation(StringConstants.div_name, _short_type, _double_type, _double_type,
                SemanticTree.basic_function_type.ddiv, _double_type);
            //Логические опреции.
            
            _short_and = make_common_binary_operation(StringConstants.and_name, _short_type, _short_type, _short_type, SemanticTree.basic_function_type.sand, _short_type);
            _short_or = make_common_binary_operation(StringConstants.or_name, _short_type, _short_type, _short_type, SemanticTree.basic_function_type.sor, _short_type);
            _short_xor = make_common_binary_operation(StringConstants.xor_name, _short_type, _short_type, _short_type, SemanticTree.basic_function_type.sxor, _short_type);
            
            add_function_to_type(StringConstants.shl_name, _short_type, _int_shl);
            add_function_to_type(StringConstants.shr_name, _short_type, _int_shr);

            //ushort type.
            //Assign.
            _ushort_assign = make_assign_operator(_ushort_type, SemanticTree.basic_function_type.usassign);
            //Преобразования ushortов.
	        _ushort_to_byte = make_type_conversion(_ushort_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.ustob);
            _ushort_to_sbyte = make_type_conversion(_ushort_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.ustosb);
            _ushort_to_short = make_type_conversion(_ushort_type, _short_type, type_compare.greater_type, SemanticTree.basic_function_type.ustos);
            _ushort_to_uint = make_type_conversion(_ushort_type, _uint_type, type_compare.less_type, SemanticTree.basic_function_type.ustoui);
            _ushort_to_long = make_type_conversion(_ushort_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.ustol);
            _ushort_to_ulong = make_type_conversion(_ushort_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.ustoul);
            _ushort_to_int = make_type_conversion(_ushort_type, _integer_type, type_compare.less_type, SemanticTree.basic_function_type.ustoi);
            _ushort_to_char = make_type_conversion(_ushort_type, _char_type, type_compare.greater_type, SemanticTree.basic_function_type.ustochar,false);
            _ushort_to_float = make_type_conversion(_ushort_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.ustof);
            _ushort_to_double = make_type_conversion(_ushort_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.ustod);
            //Унарные операции.
            _ushort_unmin = make_unary_operator(StringConstants.minus_name, _ushort_type, SemanticTree.basic_function_type.usunmin, _integer_type);
            _ushort_not = make_unary_operator(StringConstants.not_name, _ushort_type, SemanticTree.basic_function_type.usnot);
            make_unary_empty_operator(StringConstants.plus_name, _ushort_type, _ushort_type);

            add_function_to_type(StringConstants.not_name, _ushort_type, _int_unmin);

            add_function_to_type(StringConstants.gr_name, _ushort_type, _int_gr);
            add_function_to_type(StringConstants.greq_name, _ushort_type, _int_greq);
            add_function_to_type(StringConstants.sm_name, _ushort_type, _int_sm);
            add_function_to_type(StringConstants.smeq_name, _ushort_type, _int_smeq);
            add_function_to_type(StringConstants.eq_name, _ushort_type, _int_eq);
            add_function_to_type(StringConstants.noteq_name, _ushort_type, _int_noteq);

            add_function_to_type(StringConstants.plus_name, _ushort_type, _int_add);
            add_function_to_type(StringConstants.minus_name, _ushort_type, _int_sub);
            add_function_to_type(StringConstants.mul_name, _ushort_type, _int_mul);
            add_function_to_type(StringConstants.div_name, _ushort_type, _int_div);
			add_function_to_type(StringConstants.idiv_name, _ushort_type, _int_idiv);
			add_function_to_type(StringConstants.mod_name, _ushort_type, _int_mod);
			
            /*add_funtion_to_type(StringConstants.and_name, _ushort_type, _int_and);
            add_funtion_to_type(StringConstants.or_name, _ushort_type, _int_or);
            add_funtion_to_type(StringConstants.xor_name, _ushort_type, _int_xor);*/

            //Опрерации сравнения.
            /*
            _ushort_gr = make_binary_operator(StringConstants.gr_name, _ushort_type, SemanticTree.basic_function_type.usgr, _bool_type);
            _ushort_greq = make_binary_operator(StringConstants.greq_name, _ushort_type, SemanticTree.basic_function_type.usgreq, _bool_type);
            _ushort_sm = make_binary_operator(StringConstants.sm_name, _ushort_type, SemanticTree.basic_function_type.ussm, _bool_type);
            _ushort_smeq = make_binary_operator(StringConstants.smeq_name, _ushort_type, SemanticTree.basic_function_type.ussmeq, _bool_type);
            _ushort_eq = make_binary_operator(StringConstants.eq_name, _ushort_type, SemanticTree.basic_function_type.useq, _bool_type);
            _ushort_noteq = make_binary_operator(StringConstants.noteq_name, _ushort_type, SemanticTree.basic_function_type.usnoteq, _bool_type);
            */
            //Арифметические операции.
            /*
            _ushort_add = make_common_binary_operation(StringConstants.plus_name, _ushort_type, _ushort_type, _ushort_type, SemanticTree.basic_function_type.usadd, _integer_type);
            _ushort_sub = make_common_binary_operation(StringConstants.minus_name, _ushort_type, _ushort_type, _ushort_type, SemanticTree.basic_function_type.ussub, _integer_type);
            _ushort_mul = make_common_binary_operation(StringConstants.mul_name, _ushort_type, _ushort_type, _ushort_type, SemanticTree.basic_function_type.usmul, _integer_type);
            _ushort_idiv = make_common_binary_operation(StringConstants.idiv_name, _ushort_type, _ushort_type, _ushort_type, SemanticTree.basic_function_type.usdiv, _integer_type);
            _ushort_mod = make_common_binary_operation(StringConstants.mod_name, _ushort_type, _ushort_type, _ushort_type, SemanticTree.basic_function_type.usmod, _integer_type);
            */
            //Операция / для byte.
            _ushort_div = make_common_binary_operation(StringConstants.div_name, _ushort_type, _double_type, _double_type,
                SemanticTree.basic_function_type.ddiv, _double_type);
            //Логические опреции.
            
            _ushort_and = make_common_binary_operation(StringConstants.and_name, _ushort_type, _ushort_type, _ushort_type, SemanticTree.basic_function_type.usand, _ushort_type);
            _ushort_or = make_common_binary_operation(StringConstants.or_name, _ushort_type, _ushort_type, _ushort_type, SemanticTree.basic_function_type.usor, _ushort_type);
            _ushort_xor = make_common_binary_operation(StringConstants.xor_name, _ushort_type, _ushort_type, _ushort_type, SemanticTree.basic_function_type.usxor, _ushort_type);
            
            add_function_to_type(StringConstants.shl_name, _ushort_type, _int_shl);
            add_function_to_type(StringConstants.shr_name, _ushort_type, _int_shr);

       

            //uint type.
            //Assign.
            _uint_assign = make_assign_operator(_uint_type, SemanticTree.basic_function_type.uiassign);
            //Преобразования uintов.
	        _uint_to_byte = make_type_conversion(_uint_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.uitob);
            _uint_to_sbyte = make_type_conversion(_uint_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.uitosb);
            _uint_to_short = make_type_conversion(_uint_type, _short_type, type_compare.greater_type, SemanticTree.basic_function_type.uitos);
            _uint_to_ushort = make_type_conversion(_uint_type, _ushort_type, type_compare.greater_type, SemanticTree.basic_function_type.uitous);
            _uint_to_long = make_type_conversion(_uint_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.uitol);
            _uint_to_ulong = make_type_conversion(_uint_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.uitoul);
            _uint_to_int = make_type_conversion(_uint_type, _integer_type, type_compare.greater_type, SemanticTree.basic_function_type.uitoi);
            _uint_to_char = make_type_conversion(_uint_type, _char_type, type_compare.greater_type, SemanticTree.basic_function_type.uitochar,false);
            _uint_to_float = make_type_conversion(_uint_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.uitof);
            _uint_to_double = make_type_conversion(_uint_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.uitod);
            //Унарные операции.
            _uint_unmin = make_unary_operator(StringConstants.minus_name, _uint_type, SemanticTree.basic_function_type.uiunmin, _int64_type);
            _uint_not = make_unary_operator(StringConstants.not_name, _uint_type, SemanticTree.basic_function_type.uinot);
            make_unary_empty_operator(StringConstants.plus_name, _uint_type, _uint_type);
            //Опрерации сравнения.
            _uint_gr = make_binary_operator(StringConstants.gr_name, _uint_type, SemanticTree.basic_function_type.uigr, _bool_type);
            _uint_greq = make_binary_operator(StringConstants.greq_name, _uint_type, SemanticTree.basic_function_type.uigreq, _bool_type);
            _uint_sm = make_binary_operator(StringConstants.sm_name, _uint_type, SemanticTree.basic_function_type.uism, _bool_type);
            _uint_smeq = make_binary_operator(StringConstants.smeq_name, _uint_type, SemanticTree.basic_function_type.uismeq, _bool_type);
            _uint_eq = make_binary_operator(StringConstants.eq_name, _uint_type, SemanticTree.basic_function_type.uieq, _bool_type);
            _uint_noteq = make_binary_operator(StringConstants.noteq_name, _uint_type, SemanticTree.basic_function_type.uinoteq, _bool_type);
            //Арифметические операции.
            /*_uint_add = make_common_binary_operation(StringConstants.plus_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uiadd, _uint_type);
            _uint_sub = make_common_binary_operation(StringConstants.minus_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uisub, _uint_type);
            _uint_mul = make_common_binary_operation(StringConstants.mul_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uimul, _uint_type);
            _uint_idiv = make_common_binary_operation(StringConstants.idiv_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uidiv, _uint_type);
            _uint_mod = make_common_binary_operation(StringConstants.mod_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uimod, _uint_type);
            //Операция / для byte.
            _uint_div = make_common_binary_operation(StringConstants.div_name, _uint_type, _real_type, _real_type,
                SemanticTree.basic_function_type.ddiv, _real_type);
            //Логические опреции.
            _uint_and = make_common_binary_operation(StringConstants.and_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uiand, _uint_type);
            _uint_or = make_common_binary_operation(StringConstants.or_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uior, _uint_type);
            _uint_xor = make_common_binary_operation(StringConstants.xor_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uixor, _uint_type);
            //add_funtion_to_type(StringConstants.shl_name, _uint_type, _int_shl);
            //add_funtion_to_type(StringConstants.shr_name, _uint_type, _int_shr);
            _uint_shl = make_common_binary_operation(StringConstants.shl_name, _uint_type, _uint_type, _integer_type, SemanticTree.basic_function_type.uishl,_uint_type);
            _uint_shr = make_common_binary_operation(StringConstants.shr_name, _uint_type, _uint_type, _integer_type, SemanticTree.basic_function_type.uishr, _uint_type);*/
			
            _uint_add = make_common_binary_operation(StringConstants.plus_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uiadd, _int64_type);
            _uint_sub = make_common_binary_operation(StringConstants.minus_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uisub, _int64_type);
            _uint_mul = make_common_binary_operation(StringConstants.mul_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uimul, _int64_type);
            _uint_idiv = make_common_binary_operation(StringConstants.idiv_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uidiv, _int64_type);
            _uint_mod = make_common_binary_operation(StringConstants.mod_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uimod, _int64_type);
            //Операция / для byte.
            _uint_div = make_common_binary_operation(StringConstants.div_name, _uint_type, _double_type, _double_type,
                SemanticTree.basic_function_type.ddiv, _double_type);
            //Логические опреции.
            _uint_and = make_common_binary_operation(StringConstants.and_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uiand, _uint_type);
            _uint_or = make_common_binary_operation(StringConstants.or_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uior, _uint_type);
            _uint_xor = make_common_binary_operation(StringConstants.xor_name, _uint_type, _uint_type, _uint_type, SemanticTree.basic_function_type.uixor, _uint_type);
            //add_funtion_to_type(StringConstants.shl_name, _uint_type, _int_shl);
            //add_funtion_to_type(StringConstants.shr_name, _uint_type, _int_shr);
            _uint_shl = make_common_binary_operation(StringConstants.shl_name, _uint_type, _uint_type, _integer_type, SemanticTree.basic_function_type.uishl,_uint_type);
            _uint_shr = make_common_binary_operation(StringConstants.shr_name, _uint_type, _uint_type, _integer_type, SemanticTree.basic_function_type.uishr, _uint_type);
            
            /*make_function_comparison(_int_add, _uint_add, function_compare.greater);
            make_function_comparison(_int_sub, _uint_sub, function_compare.greater);
            make_function_comparison(_int_mul, _uint_mul, function_compare.greater);
            make_function_comparison(_int_idiv, _uint_idiv, function_compare.greater);
            make_function_comparison(_int_mod, _uint_mod, function_compare.greater);
            make_function_comparison(_int_gr, _uint_gr, function_compare.greater);
            make_function_comparison(_int_greq, _uint_greq, function_compare.greater);
            make_function_comparison(_int_sm, _uint_sm, function_compare.greater);
            make_function_comparison(_int_smeq, _uint_smeq, function_compare.greater);
            make_function_comparison(_int_eq, _uint_eq, function_compare.greater);
            make_function_comparison(_int_noteq, _uint_noteq, function_compare.greater);
            make_function_comparison(_int_and, _uint_and, function_compare.greater);
            make_function_comparison(_int_or, _uint_or, function_compare.greater);
            make_function_comparison(_int_xor, _uint_xor, function_compare.greater);*/
            
            make_function_comparison(_int_add, _uint_add, function_compare.less);
            make_function_comparison(_int_sub, _uint_sub, function_compare.less);
            make_function_comparison(_int_mul, _uint_mul, function_compare.less);
            make_function_comparison(_int_idiv, _uint_idiv, function_compare.less);
            make_function_comparison(_int_mod, _uint_mod, function_compare.less);
            make_function_comparison(_int_gr, _uint_gr, function_compare.less);
            make_function_comparison(_int_greq, _uint_greq, function_compare.less);
            make_function_comparison(_int_sm, _uint_sm, function_compare.less);
            make_function_comparison(_int_smeq, _uint_smeq, function_compare.less);
            make_function_comparison(_int_eq, _uint_eq, function_compare.less);
            make_function_comparison(_int_noteq, _uint_noteq, function_compare.less);
            make_function_comparison(_int_and, _uint_and, function_compare.less);
            make_function_comparison(_int_or, _uint_or, function_compare.less);
            make_function_comparison(_int_xor, _uint_xor, function_compare.less);

            //long type.
            //Assign.
            _long_assign = make_assign_operator(_int64_type, SemanticTree.basic_function_type.lassign);
            //Преобразования longов.
	        _long_to_byte = make_type_conversion(_int64_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.ltob);
            _long_to_sbyte = make_type_conversion(_int64_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.ltosb);
            _long_to_short = make_type_conversion(_int64_type, _short_type, type_compare.greater_type, SemanticTree.basic_function_type.ltos);
            _long_to_ushort = make_type_conversion(_int64_type, _ushort_type, type_compare.greater_type, SemanticTree.basic_function_type.ltous);
            _long_to_uint = make_type_conversion(_int64_type, _uint_type, type_compare.greater_type, SemanticTree.basic_function_type.ltoui);
            _long_to_ulong = make_type_conversion(_int64_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.ltoul);
            _long_to_int = make_type_conversion(_int64_type, _integer_type, type_compare.greater_type, SemanticTree.basic_function_type.ltoi);
            _long_to_char = make_type_conversion(_int64_type, _char_type, type_compare.greater_type, SemanticTree.basic_function_type.ltochar,false);
            _long_to_float = make_type_conversion(_int64_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.ltof);
            _long_to_double = make_type_conversion(_int64_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.ltod);
            //Унарные операции.
            _long_unmin = make_unary_operator(StringConstants.minus_name, _int64_type, SemanticTree.basic_function_type.lunmin);
            _long_not = make_unary_operator(StringConstants.not_name, _int64_type, SemanticTree.basic_function_type.lnot);
            make_unary_empty_operator(StringConstants.plus_name, _int64_type, _int64_type);
            //Опрерации сравнения.
            _long_gr = make_binary_operator(StringConstants.gr_name, _int64_type, SemanticTree.basic_function_type.lgr, _bool_type);
            _long_greq = make_binary_operator(StringConstants.greq_name, _int64_type, SemanticTree.basic_function_type.lgreq, _bool_type);
            _long_sm = make_binary_operator(StringConstants.sm_name, _int64_type, SemanticTree.basic_function_type.lsm, _bool_type);
            _long_smeq = make_binary_operator(StringConstants.smeq_name, _int64_type, SemanticTree.basic_function_type.lsmeq, _bool_type);
            _long_eq = make_binary_operator(StringConstants.eq_name, _int64_type, SemanticTree.basic_function_type.leq, _bool_type);
            _long_noteq = make_binary_operator(StringConstants.noteq_name, _int64_type, SemanticTree.basic_function_type.lnoteq, _bool_type);
            //Арифметические операции.
            _long_add = make_common_binary_operation(StringConstants.plus_name, _int64_type, _int64_type, _int64_type, SemanticTree.basic_function_type.ladd, _int64_type);
            _long_sub = make_common_binary_operation(StringConstants.minus_name, _int64_type, _int64_type, _int64_type, SemanticTree.basic_function_type.lsub, _int64_type);
            _long_mul = make_common_binary_operation(StringConstants.mul_name, _int64_type, _int64_type, _int64_type, SemanticTree.basic_function_type.lmul, _int64_type);
            _long_idiv = make_common_binary_operation(StringConstants.idiv_name, _int64_type, _int64_type, _int64_type, SemanticTree.basic_function_type.ldiv, _int64_type);
            _long_mod = make_common_binary_operation(StringConstants.mod_name, _int64_type, _int64_type, _int64_type, SemanticTree.basic_function_type.lmod, _int64_type);
            //Операция / для byte.
            _long_div = make_common_binary_operation(StringConstants.div_name, _int64_type, _double_type, _double_type,
                SemanticTree.basic_function_type.ddiv, _double_type);
            //Логические опреции.
            _long_and = make_common_binary_operation(StringConstants.and_name, _int64_type, _int64_type, _int64_type, SemanticTree.basic_function_type.land, _int64_type);
            _long_or = make_common_binary_operation(StringConstants.or_name, _int64_type, _int64_type, _int64_type, SemanticTree.basic_function_type.lor, _int64_type);
            _long_xor = make_common_binary_operation(StringConstants.xor_name, _int64_type, _int64_type, _int64_type, SemanticTree.basic_function_type.lxor, _int64_type);
            //add_funtion_to_type(StringConstants.shl_name, _long_type, _int_shl);
            //add_funtion_to_type(StringConstants.shr_name, _long_type, _int_shr);
            _long_shl = make_common_binary_operation(StringConstants.shl_name, _int64_type, _int64_type, _integer_type, SemanticTree.basic_function_type.lshl, _int64_type);
            _long_shr = make_common_binary_operation(StringConstants.shr_name, _int64_type, _int64_type, _integer_type, SemanticTree.basic_function_type.lshr, _int64_type);

            make_function_comparison(_long_add, _uint_add, function_compare.greater);
            make_function_comparison(_long_sub, _uint_sub, function_compare.greater);
            make_function_comparison(_long_mul, _uint_mul, function_compare.greater);
            make_function_comparison(_long_idiv, _uint_idiv, function_compare.greater);
            make_function_comparison(_long_mod, _uint_mod, function_compare.greater);
            make_function_comparison(_long_gr, _uint_gr, function_compare.greater);
            make_function_comparison(_long_greq, _uint_greq, function_compare.greater);
            make_function_comparison(_long_sm, _uint_sm, function_compare.greater);
            make_function_comparison(_long_smeq, _uint_smeq, function_compare.greater);
            make_function_comparison(_long_eq, _uint_eq, function_compare.greater);
            make_function_comparison(_long_noteq, _uint_noteq, function_compare.greater);
            make_function_comparison(_long_and, _uint_and, function_compare.greater);
            make_function_comparison(_long_or, _uint_or, function_compare.greater);
            make_function_comparison(_long_xor, _uint_xor, function_compare.greater);

            make_function_comparison(_long_add, _int_add, function_compare.greater);
            make_function_comparison(_long_sub, _int_sub, function_compare.greater);
            make_function_comparison(_long_mul, _int_mul, function_compare.greater);
            make_function_comparison(_long_idiv, _int_idiv, function_compare.greater);
            make_function_comparison(_long_mod, _int_mod, function_compare.greater);
            make_function_comparison(_long_gr, _int_gr, function_compare.greater);
            make_function_comparison(_long_greq, _int_greq, function_compare.greater);
            make_function_comparison(_long_sm, _int_sm, function_compare.greater);
            make_function_comparison(_long_smeq, _int_smeq, function_compare.greater);
            make_function_comparison(_long_eq, _int_eq, function_compare.greater);
            make_function_comparison(_long_noteq, _int_noteq, function_compare.greater);
            make_function_comparison(_long_and, _int_and, function_compare.greater);
            make_function_comparison(_long_or, _int_or, function_compare.greater);
            make_function_comparison(_long_xor, _int_xor, function_compare.greater);

            	    //ulong type.
            //Assign.
            _ulong_assign = make_assign_operator(_uint64_type, SemanticTree.basic_function_type.ulassign);
            //Преобразования ulongов.
	        _ulong_to_byte = make_type_conversion(_uint64_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.ultob);
            _ulong_to_sbyte = make_type_conversion(_uint64_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.ultosb);
            _ulong_to_short = make_type_conversion(_uint64_type, _short_type, type_compare.greater_type, SemanticTree.basic_function_type.ultos);
            _ulong_to_ushort = make_type_conversion(_uint64_type, _ushort_type, type_compare.greater_type, SemanticTree.basic_function_type.ultous);
            _ulong_to_uint = make_type_conversion(_uint64_type, _uint_type, type_compare.greater_type, SemanticTree.basic_function_type.ultoui);
            _ulong_to_long = make_type_conversion(_uint64_type, _int64_type, type_compare.greater_type, SemanticTree.basic_function_type.ultol);
            _ulong_to_int = make_type_conversion(_uint64_type, _integer_type, type_compare.greater_type, SemanticTree.basic_function_type.ultoi);
            _ulong_to_char = make_type_conversion(_uint64_type, _char_type, type_compare.greater_type, SemanticTree.basic_function_type.ultochar,false);
            _ulong_to_float = make_type_conversion(_uint64_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.ultof);
            _ulong_to_double = make_type_conversion(_uint64_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.ultod);
            //Унарные операции.
            _ulong_unmin = make_unary_operator(StringConstants.minus_name, _uint64_type, SemanticTree.basic_function_type.ulunmin, _uint64_type);
            _ulong_not = make_unary_operator(StringConstants.not_name, _uint64_type, SemanticTree.basic_function_type.ulnot);
            make_unary_empty_operator(StringConstants.plus_name, _uint64_type, _uint64_type);
            //Опрерации сравнения.
            _ulong_gr = make_binary_operator(StringConstants.gr_name, _uint64_type, SemanticTree.basic_function_type.ulgr, _bool_type);
            _ulong_greq = make_binary_operator(StringConstants.greq_name, _uint64_type, SemanticTree.basic_function_type.ulgreq, _bool_type);
            _ulong_sm = make_binary_operator(StringConstants.sm_name, _uint64_type, SemanticTree.basic_function_type.ulsm, _bool_type);
            _ulong_smeq = make_binary_operator(StringConstants.smeq_name, _uint64_type, SemanticTree.basic_function_type.ulsmeq, _bool_type);
            _ulong_eq = make_binary_operator(StringConstants.eq_name, _uint64_type, SemanticTree.basic_function_type.uleq, _bool_type);
            _ulong_noteq = make_binary_operator(StringConstants.noteq_name, _uint64_type, SemanticTree.basic_function_type.ulnoteq, _bool_type);
            //Арифметические операции.
            _ulong_add = make_common_binary_operation(StringConstants.plus_name, _uint64_type, _uint64_type, _uint64_type, SemanticTree.basic_function_type.uladd, _uint64_type);
            _ulong_sub = make_common_binary_operation(StringConstants.minus_name, _uint64_type, _uint64_type, _uint64_type, SemanticTree.basic_function_type.ulsub, _uint64_type);
            _ulong_mul = make_common_binary_operation(StringConstants.mul_name, _uint64_type, _uint64_type, _uint64_type, SemanticTree.basic_function_type.ulmul, _uint64_type);
            _ulong_idiv = make_common_binary_operation(StringConstants.idiv_name, _uint64_type, _uint64_type, _uint64_type, SemanticTree.basic_function_type.uldiv, _uint64_type);
            _ulong_mod = make_common_binary_operation(StringConstants.mod_name, _uint64_type, _uint64_type, _uint64_type, SemanticTree.basic_function_type.ulmod, _uint64_type);
            //Операция / для byte.
            _ulong_div = make_common_binary_operation(StringConstants.div_name, _uint64_type, _double_type, _double_type,
                SemanticTree.basic_function_type.ddiv, _double_type);
            //Логические опреции.
            _ulong_and = make_common_binary_operation(StringConstants.and_name, _uint64_type, _uint64_type, _uint64_type, SemanticTree.basic_function_type.uland, _uint64_type);
            _ulong_or = make_common_binary_operation(StringConstants.or_name, _uint64_type, _uint64_type, _uint64_type, SemanticTree.basic_function_type.ulor, _uint64_type);
            _ulong_xor = make_common_binary_operation(StringConstants.xor_name, _uint64_type, _uint64_type, _uint64_type, SemanticTree.basic_function_type.ulxor, _uint64_type);
            //add_funtion_to_type(StringConstants.shl_name, _ulong_type, _int_shl);
            //add_funtion_to_type(StringConstants.shr_name, _ulong_type, _int_shr);
            _ulong_shl = make_common_binary_operation(StringConstants.shl_name, _uint64_type, _uint64_type, _integer_type, SemanticTree.basic_function_type.ulshl, _uint64_type);
            _ulong_shr = make_common_binary_operation(StringConstants.shr_name, _uint64_type, _uint64_type, _integer_type, SemanticTree.basic_function_type.ulshr, _uint64_type);

            /*make_function_comparison(_long_add, _ulong_add, function_compare.greater);
            make_function_comparison(_long_sub, _ulong_sub, function_compare.greater);
            make_function_comparison(_long_mul, _ulong_mul, function_compare.greater);
            make_function_comparison(_long_idiv, _ulong_idiv, function_compare.greater);
            make_function_comparison(_long_mod, _ulong_mod, function_compare.greater);
            make_function_comparison(_long_gr, _ulong_gr, function_compare.greater);
            make_function_comparison(_long_greq, _ulong_greq, function_compare.greater);
            make_function_comparison(_long_sm, _ulong_sm, function_compare.greater);
            make_function_comparison(_long_smeq, _ulong_smeq, function_compare.greater);
            make_function_comparison(_long_eq, _ulong_eq, function_compare.greater);
            make_function_comparison(_long_noteq, _ulong_noteq, function_compare.greater);
            make_function_comparison(_long_and, _ulong_and, function_compare.greater);
            make_function_comparison(_long_or, _ulong_or, function_compare.greater);
            make_function_comparison(_long_xor, _ulong_xor, function_compare.greater);*/
			
            make_function_comparison(_long_add, _ulong_add, function_compare.less);
            make_function_comparison(_long_sub, _ulong_sub, function_compare.less);
            make_function_comparison(_long_mul, _ulong_mul, function_compare.less);
            make_function_comparison(_long_idiv, _ulong_idiv, function_compare.less);
            make_function_comparison(_long_mod, _ulong_mod, function_compare.less);
            make_function_comparison(_long_gr, _ulong_gr, function_compare.less);
            make_function_comparison(_long_greq, _ulong_greq, function_compare.less);
            make_function_comparison(_long_sm, _ulong_sm, function_compare.less);
            make_function_comparison(_long_smeq, _ulong_smeq, function_compare.less);
            make_function_comparison(_long_eq, _ulong_eq, function_compare.less);
            make_function_comparison(_long_noteq, _ulong_noteq, function_compare.less);
            make_function_comparison(_long_and, _ulong_and, function_compare.less);
            make_function_comparison(_long_or, _ulong_or, function_compare.less);
            make_function_comparison(_long_xor, _ulong_xor, function_compare.less);
            
            make_function_comparison(_int_add, _ulong_add, function_compare.less);
            make_function_comparison(_int_sub, _ulong_sub, function_compare.less);
            make_function_comparison(_int_mul, _ulong_mul, function_compare.less);
            make_function_comparison(_int_idiv, _ulong_idiv, function_compare.less);
            make_function_comparison(_int_mod, _ulong_mod, function_compare.less);
            make_function_comparison(_int_gr, _ulong_gr, function_compare.less);
            make_function_comparison(_int_greq, _ulong_greq, function_compare.less);
            make_function_comparison(_int_sm, _ulong_sm, function_compare.less);
            make_function_comparison(_int_smeq, _ulong_smeq, function_compare.less);
            make_function_comparison(_int_eq, _ulong_eq, function_compare.less);
            make_function_comparison(_int_noteq, _ulong_noteq, function_compare.less);
            make_function_comparison(_int_and, _ulong_and, function_compare.less);
            make_function_comparison(_int_or, _ulong_or, function_compare.less);
            make_function_comparison(_int_xor, _ulong_xor, function_compare.less);

           
            
            make_function_comparison(_ulong_add, _uint_add, function_compare.greater);
            make_function_comparison(_ulong_sub, _uint_sub, function_compare.greater);
            make_function_comparison(_ulong_mul, _uint_mul, function_compare.greater);
            make_function_comparison(_ulong_idiv, _uint_idiv, function_compare.greater);
            make_function_comparison(_ulong_mod, _uint_mod, function_compare.greater);
            make_function_comparison(_ulong_gr, _uint_gr, function_compare.greater);
            make_function_comparison(_ulong_greq, _uint_greq, function_compare.greater);
            make_function_comparison(_ulong_sm, _uint_sm, function_compare.greater);
            make_function_comparison(_ulong_smeq, _uint_smeq, function_compare.greater);
            make_function_comparison(_ulong_eq, _uint_eq, function_compare.greater);
            make_function_comparison(_ulong_noteq, _uint_noteq, function_compare.greater);
            make_function_comparison(_ulong_and, _uint_and, function_compare.greater);
            make_function_comparison(_ulong_or, _uint_or, function_compare.greater);
            make_function_comparison(_ulong_xor, _uint_xor, function_compare.greater);

            //real type.
            //Assign.
            _real_assign=make_assign_operator(_double_type, SemanticTree.basic_function_type.dassign);
            /*
            _double_to_byte = make_type_conversion(_ulong_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.ultob);
            _double_to_sbyte = make_type_conversion(_ulong_type, _sbyte_type, type_compare.non_comparable_type, SemanticTree.basic_function_type.ultosb);
            _double_to_short = make_type_conversion(_ulong_type, _short_type, type_compare.non_comparable_type, SemanticTree.basic_function_type.ultos);
            _double_to_ushort = make_type_conversion(_ulong_type, _ushort_type, type_compare.greater_type, SemanticTree.basic_function_type.ultous);
            _double_to_uint = make_type_conversion(_ulong_type, _uint_type, type_compare.greater_type, SemanticTree.basic_function_type.ului);
            _double_to_long = make_type_conversion(_ulong_type, _long_type, type_compare.non_comparable_type, SemanticTree.basic_function_type.ultol);
            _double_to_ulong = make_type_conversion(_ulong_type, _long_type, type_compare.non_comparable_type, SemanticTree.basic_function_type.ultol);
            _double_to_int = make_type_conversion(_ulong_type, _integer_type, type_compare.non_comparable_type, SemanticTree.basic_function_type.ultoi);
            _double_to_char = make_type_conversion(_ulong_type, _char_type, type_compare.greater_type, SemanticTree.basic_function_type.ultochar);
            */
            _float_to_sbyte = make_type_conversion(_float_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.ftosb, false);
            _float_to_byte = make_type_conversion(_float_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.ftob, false);
            _float_to_ushort = make_type_conversion(_float_type, _ushort_type, type_compare.greater_type, SemanticTree.basic_function_type.ftous, false);
            _float_to_short = make_type_conversion(_float_type, _short_type, type_compare.greater_type, SemanticTree.basic_function_type.ftos, false);
            _float_to_ulong = make_type_conversion(_float_type, _uint64_type, type_compare.greater_type, SemanticTree.basic_function_type.ftoul, false);
            _float_to_long = make_type_conversion(_float_type, _int64_type, type_compare.greater_type, SemanticTree.basic_function_type.ftol, false);
            _float_to_uint = make_type_conversion(_float_type, _uint_type, type_compare.greater_type, SemanticTree.basic_function_type.ftoui, false);
            _float_to_int = make_type_conversion(_float_type, _integer_type, type_compare.greater_type, SemanticTree.basic_function_type.ftoi, false);


            _double_to_sbyte = make_type_conversion(_double_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.dtosb, false);
            _double_to_byte = make_type_conversion(_double_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.dtob, false);
            _double_to_ushort = make_type_conversion(_double_type, _ushort_type, type_compare.greater_type, SemanticTree.basic_function_type.dtous, false);
            _double_to_short = make_type_conversion(_double_type, _short_type, type_compare.greater_type, SemanticTree.basic_function_type.dtos, false);
            _double_to_ulong = make_type_conversion(_double_type, _uint64_type, type_compare.greater_type, SemanticTree.basic_function_type.dtoul, false);
            _double_to_long = make_type_conversion(_double_type, _int64_type, type_compare.greater_type, SemanticTree.basic_function_type.dtol, false);
            _double_to_uint = make_type_conversion(_double_type, _uint_type, type_compare.greater_type, SemanticTree.basic_function_type.dtoui, false);
            _double_to_int = make_type_conversion(_double_type, _integer_type, type_compare.greater_type, SemanticTree.basic_function_type.dtoi, false);
            _double_to_float = make_type_conversion(_double_type, _float_type, type_compare.greater_type, SemanticTree.basic_function_type.dtof);
            //Унарные операции.
            _real_unmin = make_unary_operator(StringConstants.minus_name, _double_type, SemanticTree.basic_function_type.dunmin);
            //make_empty_operator(StringConstants.plus_name, _real_type);
            make_unary_empty_operator(StringConstants.plus_name, _double_type, _double_type);
            //Арифметические операции.
            _real_add=make_binary_operator(StringConstants.plus_name, _double_type, SemanticTree.basic_function_type.dadd);
            _real_sub=make_binary_operator(StringConstants.minus_name, _double_type, SemanticTree.basic_function_type.dsub);
            _real_mul=make_binary_operator(StringConstants.mul_name, _double_type, SemanticTree.basic_function_type.dmul);
            _real_div=make_binary_operator(StringConstants.div_name, _double_type, SemanticTree.basic_function_type.ddiv);

            //Опрерации сравнения.
            _real_gr=make_binary_operator(StringConstants.gr_name, _double_type, SemanticTree.basic_function_type.dgr, _bool_type);
            _real_greq=make_binary_operator(StringConstants.greq_name, _double_type, SemanticTree.basic_function_type.dgreq, _bool_type);
            _real_sm=make_binary_operator(StringConstants.sm_name, _double_type, SemanticTree.basic_function_type.dsm, _bool_type);
            _real_smeq=make_binary_operator(StringConstants.smeq_name, _double_type, SemanticTree.basic_function_type.dsmeq, _bool_type);
            _real_eq=make_binary_operator(StringConstants.eq_name, _double_type, SemanticTree.basic_function_type.deq, _bool_type);
            _real_noteq=make_binary_operator(StringConstants.noteq_name, _double_type, SemanticTree.basic_function_type.dnoteq, _bool_type);

            //float type
            _float_assign = make_assign_operator(_float_type, SemanticTree.basic_function_type.fassign);
            _float_to_double = make_type_conversion(_float_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.ftod);
            //Унарные операции.
            _float_unmin = make_unary_operator(StringConstants.minus_name, _float_type, SemanticTree.basic_function_type.funmin);
            //make_empty_operator(StringConstants.plus_name, _real_type);
            make_unary_empty_operator(StringConstants.plus_name, _float_type, _float_type);
            //Арифметические операции.
            _float_add = make_binary_operator(StringConstants.plus_name, _float_type, SemanticTree.basic_function_type.fadd);
            _float_sub = make_binary_operator(StringConstants.minus_name, _float_type, SemanticTree.basic_function_type.fsub);
            _float_mul = make_binary_operator(StringConstants.mul_name, _float_type, SemanticTree.basic_function_type.fmul);
            _float_div = make_binary_operator(StringConstants.div_name, _float_type, SemanticTree.basic_function_type.fdiv);

            //Опрерации сравнения.
            _float_gr = make_binary_operator(StringConstants.gr_name, _float_type, SemanticTree.basic_function_type.fgr, _bool_type);
            _float_greq = make_binary_operator(StringConstants.greq_name, _float_type, SemanticTree.basic_function_type.fgreq, _bool_type);
            _float_sm = make_binary_operator(StringConstants.sm_name, _float_type, SemanticTree.basic_function_type.fsm, _bool_type);
            _float_smeq = make_binary_operator(StringConstants.smeq_name, _float_type, SemanticTree.basic_function_type.fsmeq, _bool_type);
            _float_eq = make_binary_operator(StringConstants.eq_name, _float_type, SemanticTree.basic_function_type.feq, _bool_type);
            _float_noteq = make_binary_operator(StringConstants.noteq_name, _float_type, SemanticTree.basic_function_type.fnoteq, _bool_type);

            make_function_comparison(_real_add, _float_add, function_compare.greater);
            make_function_comparison(_real_sub, _float_sub, function_compare.greater);
            make_function_comparison(_real_mul, _float_mul, function_compare.greater);
            make_function_comparison(_real_gr, _float_gr, function_compare.greater);
            make_function_comparison(_real_greq, _float_greq, function_compare.greater);
            make_function_comparison(_real_sm, _float_sm, function_compare.greater);
            make_function_comparison(_real_smeq, _float_smeq, function_compare.greater);
            make_function_comparison(_real_eq, _float_eq, function_compare.greater);
            make_function_comparison(_real_noteq, _float_noteq, function_compare.greater);
            make_function_comparison(_real_div, _float_div, function_compare.greater);

            //char type.
            //Assign.
            _char_assign = make_assign_operator(_char_type, SemanticTree.basic_function_type.charassign);
            _char_to_byte = make_type_conversion(_char_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.chartob,false);
            _char_to_sbyte = make_type_conversion(_char_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.chartosb,false);
            _char_to_short = make_type_conversion(_char_type, _short_type, type_compare.less_type, SemanticTree.basic_function_type.chartos,false);
            _char_to_ushort = make_type_conversion(_char_type, _ushort_type, type_compare.less_type, SemanticTree.basic_function_type.chartous,false);
            _char_to_uint = make_type_conversion(_char_type, _uint_type, type_compare.less_type, SemanticTree.basic_function_type.chartoui,false);
            _char_to_long = make_type_conversion(_char_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.chartol,false);
            _char_to_ulong = make_type_conversion(_char_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.chartoul,false);
            _char_to_int = make_type_conversion(_char_type, _integer_type, type_compare.less_type, SemanticTree.basic_function_type.chartoi, false);
            _char_to_float = make_type_conversion(_char_type, _float_type, type_compare.less_type, SemanticTree.basic_function_type.chartof, false);
            _char_to_double = make_type_conversion(_char_type, _double_type, type_compare.less_type, SemanticTree.basic_function_type.chartod, false);
            //Опрерации сравнения.
            _char_gr = make_binary_operator(StringConstants.gr_name, _char_type, SemanticTree.basic_function_type.chargr, _bool_type);
            _char_greq = make_binary_operator(StringConstants.greq_name, _char_type, SemanticTree.basic_function_type.chargreq, _bool_type);
            _char_sm = make_binary_operator(StringConstants.sm_name, _char_type, SemanticTree.basic_function_type.charsm, _bool_type);
            _char_smeq = make_binary_operator(StringConstants.smeq_name, _char_type, SemanticTree.basic_function_type.charsmeq, _bool_type);
            _char_eq = make_binary_operator(StringConstants.eq_name, _char_type, SemanticTree.basic_function_type.chareq, _bool_type);
            _char_noteq = make_binary_operator(StringConstants.noteq_name, _char_type, SemanticTree.basic_function_type.charnoteq, _bool_type);

            
            //boolean type.
            //Assign.
            _bool_assign=make_assign_operator(_bool_type, SemanticTree.basic_function_type.boolassign);

            //Логические операции.
            //Унарные операции.
            _bool_not=make_unary_operator(StringConstants.not_name, _bool_type, SemanticTree.basic_function_type.boolnot);

            //Логическме операции.
            _bool_and=make_binary_operator(StringConstants.and_name, _bool_type, SemanticTree.basic_function_type.booland);
            _bool_or=make_binary_operator(StringConstants.or_name, _bool_type, SemanticTree.basic_function_type.boolor);
            _bool_xor=make_binary_operator(StringConstants.xor_name, _bool_type, SemanticTree.basic_function_type.boolxor);

            //Опрерации сравнения.
            _bool_gr=make_binary_operator(StringConstants.gr_name, _bool_type, SemanticTree.basic_function_type.boolgr);
            _bool_greq=make_binary_operator(StringConstants.greq_name, _bool_type, SemanticTree.basic_function_type.boolgreq);
            _bool_sm=make_binary_operator(StringConstants.sm_name, _bool_type, SemanticTree.basic_function_type.boolsm);
            _bool_smeq=make_binary_operator(StringConstants.smeq_name, _bool_type, SemanticTree.basic_function_type.boolsmeq);
            _bool_eq=make_binary_operator(StringConstants.eq_name, _bool_type, SemanticTree.basic_function_type.booleq);
            _bool_noteq=make_binary_operator(StringConstants.noteq_name, _bool_type, SemanticTree.basic_function_type.boolnoteq);
			_bool_to_int=make_type_conversion(_bool_type, _integer_type, type_compare.less_type, SemanticTree.basic_function_type.booltoi, false);
            _bool_to_byte = make_type_conversion(_bool_type, _byte_type, type_compare.greater_type, SemanticTree.basic_function_type.booltob, false);
            _bool_to_sbyte = make_type_conversion(_bool_type, _sbyte_type, type_compare.greater_type, SemanticTree.basic_function_type.booltosb, false);
            _bool_to_short = make_type_conversion(_bool_type, _short_type, type_compare.less_type, SemanticTree.basic_function_type.booltos, false);
            _bool_to_ushort = make_type_conversion(_bool_type, _ushort_type, type_compare.less_type, SemanticTree.basic_function_type.booltous, false);
            _bool_to_uint = make_type_conversion(_bool_type, _uint_type, type_compare.less_type, SemanticTree.basic_function_type.booltoui, false);
            _bool_to_long = make_type_conversion(_bool_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.booltol, false);
            _bool_to_ulong = make_type_conversion(_bool_type, _uint64_type, type_compare.less_type, SemanticTree.basic_function_type.booltoul, false);

            _obj_to_obj=make_unary_function(_object_type, SemanticTree.basic_function_type.objtoobj, _object_type);

            //TODO: Закончить с инициализацией строк.
            make_assign_operator(_string_type, SemanticTree.basic_function_type.objassign);

            _string_add = make_binary_compiled_operator(_string_type, StringConstants.string_concat_method_name, StringConstants.plus_name);
            _char_add = make_binary_compiled_operator(_string_type, _char_type, StringConstants.string_concat_method_name, StringConstants.plus_name, _char_type);
            
            init_reference_type(_object_type);
            
            //mark_byte_as_ordinal();
            //mark_int_as_ordinal();
            //mark_char_as_ordinal();

            
            //+= -= *= /=
            _byte_plusassign = make_binary_operator(StringConstants.plusassign_name, _byte_type);
            _byte_minusassign = make_binary_operator(StringConstants.minusassign_name, _byte_type);
            _byte_multassign = make_binary_operator(StringConstants.multassign_name, _byte_type);
            _byte_divassign = make_binary_operator(StringConstants.divassign_name, _byte_type);

            _sbyte_plusassign = make_binary_operator(StringConstants.plusassign_name, _sbyte_type);
            _sbyte_minusassign = make_binary_operator(StringConstants.minusassign_name, _sbyte_type);
            _sbyte_multassign = make_binary_operator(StringConstants.multassign_name, _sbyte_type);
            _sbyte_divassign = make_binary_operator(StringConstants.divassign_name, _sbyte_type);

            _short_plusassign = make_binary_operator(StringConstants.plusassign_name, _short_type);
            _short_minusassign = make_binary_operator(StringConstants.minusassign_name, _short_type);
            _short_multassign = make_binary_operator(StringConstants.multassign_name, _short_type);
            _short_divassign = make_binary_operator(StringConstants.divassign_name, _short_type);

            _ushort_plusassign = make_binary_operator(StringConstants.plusassign_name, _ushort_type);
            _ushort_minusassign = make_binary_operator(StringConstants.minusassign_name, _ushort_type);
            _ushort_multassign = make_binary_operator(StringConstants.multassign_name, _ushort_type);
            _ushort_divassign = make_binary_operator(StringConstants.divassign_name, _ushort_type);

            _int_plusassign = make_binary_operator(StringConstants.plusassign_name, _integer_type);
            _int_minusassign = make_binary_operator(StringConstants.minusassign_name, _integer_type);
            _int_multassign = make_binary_operator(StringConstants.multassign_name, _integer_type);
            _int_divassign = make_binary_operator(StringConstants.divassign_name, _integer_type);

            _uint_plusassign = make_binary_operator(StringConstants.plusassign_name, _uint_type);
            _uint_minusassign = make_binary_operator(StringConstants.minusassign_name, _uint_type);
            _uint_multassign = make_binary_operator(StringConstants.multassign_name, _uint_type);
            _uint_divassign = make_binary_operator(StringConstants.divassign_name, _uint_type);

            _long_plusassign = make_binary_operator(StringConstants.plusassign_name, _int64_type);
            _long_minusassign = make_binary_operator(StringConstants.minusassign_name, _int64_type);
            _long_multassign = make_binary_operator(StringConstants.multassign_name, _int64_type);
            _long_divassign = make_binary_operator(StringConstants.divassign_name, _int64_type);

            _ulong_plusassign = make_binary_operator(StringConstants.plusassign_name, _uint64_type);
            _ulong_minusassign = make_binary_operator(StringConstants.minusassign_name, _uint64_type);
            _ulong_multassign = make_binary_operator(StringConstants.multassign_name, _uint64_type);
            _ulong_divassign = make_binary_operator(StringConstants.divassign_name, _uint64_type);

            _float_plusassign = make_binary_operator(StringConstants.plusassign_name, _float_type);
            _float_minusassign = make_binary_operator(StringConstants.minusassign_name, _float_type);
            _float_multassign = make_binary_operator(StringConstants.multassign_name, _float_type);
            _float_divassign = make_binary_operator(StringConstants.divassign_name, _float_type);

            _double_plusassign = make_binary_operator(StringConstants.plusassign_name, _double_type);
            _double_minusassign = make_binary_operator(StringConstants.minusassign_name, _double_type);
            _double_multassign = make_binary_operator(StringConstants.multassign_name, _double_type);
            _double_divassign = make_binary_operator(StringConstants.divassign_name, _double_type);




            mark_type_as_ordinal(_integer_type, SemanticTree.basic_function_type.iinc, SemanticTree.basic_function_type.idec,
                SemanticTree.basic_function_type.isinc, SemanticTree.basic_function_type.isdec,
                new int_const_node(int.MinValue,null), new int_const_node(int.MaxValue,null), create_emty_function(_integer_type,null), int_to_int);
			
            mark_type_as_ordinal(_uint_type, SemanticTree.basic_function_type.uiinc, SemanticTree.basic_function_type.uidec,
                SemanticTree.basic_function_type.uisinc, SemanticTree.basic_function_type.uisdec,
                new uint_const_node(uint.MinValue, null), new uint_const_node(uint.MaxValue, null), _uint_to_int, uint_to_int);
            
            mark_type_as_ordinal(_int64_type, SemanticTree.basic_function_type.linc, SemanticTree.basic_function_type.ldec,
                SemanticTree.basic_function_type.lsinc, SemanticTree.basic_function_type.lsdec,
                new long_const_node(long.MinValue, null), new long_const_node(long.MaxValue, null), _long_to_int, long_to_int);
			
            mark_type_as_ordinal(_uint64_type, SemanticTree.basic_function_type.ulinc, SemanticTree.basic_function_type.uldec,
                SemanticTree.basic_function_type.ulsinc, SemanticTree.basic_function_type.ulsdec,
                new ulong_const_node(ulong.MinValue, null), new ulong_const_node(ulong.MaxValue, null), _ulong_to_int, ulong_to_int);
            
            mark_type_as_ordinal(_byte_type, SemanticTree.basic_function_type.binc, SemanticTree.basic_function_type.bdec,
                SemanticTree.basic_function_type.bsinc, SemanticTree.basic_function_type.bsdec,
                new byte_const_node(byte.MinValue, null), new byte_const_node(byte.MaxValue, null), _byte_to_int, byte_to_int);

            mark_type_as_ordinal(_sbyte_type, SemanticTree.basic_function_type.sbinc, SemanticTree.basic_function_type.sbdec,
                SemanticTree.basic_function_type.sbsinc, SemanticTree.basic_function_type.sbsdec,
                new sbyte_const_node(sbyte.MinValue, null), new sbyte_const_node(sbyte.MaxValue, null), _sbyte_to_int, sbyte_to_int);

            mark_type_as_ordinal(_short_type, SemanticTree.basic_function_type.sinc, SemanticTree.basic_function_type.sdec,
                SemanticTree.basic_function_type.ssinc, SemanticTree.basic_function_type.ssdec,
                new short_const_node(short.MinValue, null), new short_const_node(short.MaxValue, null), _short_to_int, short_to_int);

            mark_type_as_ordinal(_ushort_type, SemanticTree.basic_function_type.usinc, SemanticTree.basic_function_type.usdec,
                SemanticTree.basic_function_type.ussinc, SemanticTree.basic_function_type.ussdec,
                new ushort_const_node(ushort.MinValue, null), new ushort_const_node(ushort.MaxValue, null), _ushort_to_int, ushort_to_int);

            mark_type_as_ordinal(_char_type, SemanticTree.basic_function_type.cinc, SemanticTree.basic_function_type.cdec,
                SemanticTree.basic_function_type.csinc, SemanticTree.basic_function_type.csdec,
                new char_const_node(char.MinValue, null), new char_const_node(char.MaxValue, null), _char_to_int, char_to_int);
			
            mark_type_as_ordinal(_bool_type, SemanticTree.basic_function_type.boolinc, SemanticTree.basic_function_type.booldec,
                SemanticTree.basic_function_type.boolsinc, SemanticTree.basic_function_type.boolsdec,
                new bool_const_node(false, null), new bool_const_node(true, null), _bool_to_int, bool_to_int);
            
            _empty_string = new string_const_node(string.Empty, null);
            _dllimport_type = compiled_type_node.get_type_node(typeof(System.Runtime.InteropServices.DllImportAttribute));
            _flags_attribute_type = compiled_type_node.get_type_node(typeof(System.FlagsAttribute));
            _usage_attribute_type = compiled_type_node.get_type_node(typeof(AttributeUsageAttribute));
            _comimport_type = compiled_type_node.get_type_node(typeof(System.Runtime.InteropServices.ComImportAttribute));
            _attribute_type = compiled_type_node.get_type_node(typeof(Attribute));
            _field_offset_attribute_type = compiled_type_node.get_type_node(typeof(System.Runtime.InteropServices.FieldOffsetAttribute));
            _struct_layout_attribute_type = compiled_type_node.get_type_node(typeof(System.Runtime.InteropServices.StructLayoutAttribute));
            string[] s=new string[0];
            _array_of_string = compiled_type_node.get_type_node(s.GetType());

            //это преобразование есть явно в decimal - это уже давно было закомментировано
            //make_type_conversion_use_ctor(_integer_type, _decimal_type, type_compare.less_type, true);

            //make_type_conversion_use_ctor(_float_type, _decimal_type, type_compare.less_type, true);
            //make_type_conversion_use_ctor(_double_type, _decimal_type, type_compare.less_type, true);

            //make_type_conversion_use_ctor(_uint_type, _decimal_type, type_compare.less_type, true);
            //make_type_conversion_use_ctor(_uint64_type, _decimal_type, type_compare.less_type, true);

            // SSM 21.07.18 закомментировал четыре строчки выше, т.к. они давали неоднозначность при decimal(2.5). 
            // Теперь из real в decimal и из single в decimal возможно только явное преобразование
            // Из longword в decimal и из uint64 в decimal существуют как явное, так и неявное преобразования
            // Это полностью соответствует тому, что в C#

            // забавно, но make_type_conversion_use_ctor используется ТОЛЬКО тут и ТОЛЬКО для типа decimal !!!!!!!
            // То есть, это было неправильное исправление 

            writable_in_typed_files_types.Clear();
            writable_in_typed_files_types.Add(_bool_type, _bool_type);
            writable_in_typed_files_types.Add(_byte_type, _byte_type);
            writable_in_typed_files_types.Add(_sbyte_type, _sbyte_type);
            writable_in_typed_files_types.Add(_short_type, _short_type);
            writable_in_typed_files_types.Add(_ushort_type, _ushort_type);
            writable_in_typed_files_types.Add(_integer_type, _integer_type);
            writable_in_typed_files_types.Add(_uint_type, _uint_type);
            writable_in_typed_files_types.Add(_int64_type, _int64_type);
            writable_in_typed_files_types.Add(_uint64_type, _uint64_type);
            writable_in_typed_files_types.Add(_double_type, _double_type);
            writable_in_typed_files_types.Add(_char_type, _char_type);
            writable_in_typed_files_types.Add(_float_type, _float_type);
            
            foreach (type_node tn in wait_add_ref_list)
            	init_reference_type(tn);
            wait_add_ref_list.Clear();
            
            _exception_base_type = compiled_type_node.get_type_node(typeof(System.Exception), symtab);
            _exception_base_type.SetName(StringConstants.base_exception_class_name);
            _int64_to_pointer = make_type_conversion(_int64_type, _pointer_type, type_compare.greater_type, SemanticTree.basic_function_type.ltop, false);
            _pointer_to_int64 = make_type_conversion(_pointer_type, _int64_type, type_compare.less_type, SemanticTree.basic_function_type.ptol, false);
        }
		
        private static List<type_node> wait_add_ref_list = new List<type_node>();
        
        public static compiled_constructor_node make_type_conversion_use_ctor(compiled_type_node from, compiled_type_node to, type_compare tc, bool is_implicit)
        {
            Type[] ctor_params = new Type[1];
            ctor_params[0] = from.compiled_type;
            System.Reflection.ConstructorInfo ci = to.compiled_type.GetConstructor(ctor_params);
            compiled_constructor_node conv_method = null;
            if (ci != null)
            {
                conv_method = compiled_constructor_node.get_compiled_constructor(ci);
                type_table.add_type_conversion_from_defined(from, to, conv_method, tc, is_implicit);
            }
            return conv_method;
        }

        public static bool CanUseThisTypeForTypedFiles(type_node tn)
        {
            return writable_in_typed_files_types[tn] != null;
        }

        private static void add_std_convertions()
        {
            type_intersection_node inter = new type_intersection_node(type_compare.greater_type);

            Type t = typeof(char);
            Type[] types = new Type[1];
            types[0] = typeof(char);
            System.Reflection.MethodInfo mi = t.GetMethod(StringConstants.to_string_method_name, types);
            compiled_function_node cfn = compiled_function_node.get_compiled_method(mi);

            inter.another_to_this = new type_conversion(cfn);
            _string_type.add_intersection_node(_char_type, inter,false);
            _char_to_string = cfn;
        }

        private static void make_types()
        {
            initialize_types();
            add_std_convertions();
            static_executors.init_compiled_type_executors();
        }

        private static expression_node set_length_compile_time_executor(location call_location, params expression_node[] pars)
        {
            
            array_internal_interface aii = (array_internal_interface)
                pars[0].type.get_internal_interface(PascalABCCompiler.TreeRealization.internal_interface_kind.unsized_array_interface);

            if (aii == null)
            {
                throw new SimpleSemanticError(call_location, "CAN_NOT_APPLY_SETLENGTH_TO_THIS_OPERAND_TYPE");
            }

            compiled_static_method_call csmc = new compiled_static_method_call(_resize_func, call_location);
            csmc.parameters.AddRange(pars);
            csmc.template_parametres_list.AddElement(aii.element_type);
            return csmc;
        }

        /*public static void RestoreStandartNames()
        {
            _bool_type.SetName(StringConstants.bool_type_name);
            _byte_type.SetName(StringConstants.byte_type_name);
            _sbyte_type.SetName(StringConstants.sbyte_type_name);
            _short_type.SetName(StringConstants.short_type_name);
            _ushort_type.SetName(StringConstants.ushort_type_name);
            _integer_type.SetName(StringConstants.integer_type_name); ;
            _uint_type.SetName(StringConstants.uint_type_name);
            _int64_type.SetName(StringConstants.long_type_name);
            _uint64_type.SetName(StringConstants.ulong_type_name);
            _double_type.SetName(StringConstants.real_type_name);
            _float_type.SetName(StringConstants.float_type_name);
            _char_type.SetName(StringConstants.char_type_name);
            _string_type.SetName(StringConstants.string_type_name);
            _object_type.SetName(StringConstants.object_type_name);
            _exception_base_type.SetName(StringConstants.base_exception_class_name);
            _array_base_type.SetName(StringConstants.base_array_type_name);
            _delegate_base_type.SetName(StringConstants.base_delegate_type_name);
            _enum_base_type.SetName(StringConstants.base_enum_class_name);
            _pointer_type.SetName(StringConstants.pointer_type_name);
            _void_type.SetName(StringConstants.void_class_name);
        }*/

        private static void make_methods()
        {
            _empty_method = new basic_function_node(SemanticTree.basic_function_type.none, null, true);
            basic_parameter bp = new basic_parameter(StringConstants.unary_param_name, null,
                SemanticTree.parameter_type.value, _empty_method);
            _empty_method.parameters.AddElement(bp);
            _empty_method.compile_time_executor = delegated_empty_method;

            //(ssyy) Убрать подмену параметров?!
            Type arr_type = typeof(System.Array);
            System.Reflection.MethodInfo resize_func_mi = arr_type.GetMethod("Resize");
            _resize_func = compiled_function_node.get_compiled_method(resize_func_mi);
            _resize_func.parameters.Clear();
            common_parameter par0 = new common_parameter("Array", _object_type, SemanticTree.parameter_type.value, null,
                concrete_parameter_type.cpt_none, null, null);
            common_parameter par1 = new common_parameter("Length", _integer_type, SemanticTree.parameter_type.value, null,
                concrete_parameter_type.cpt_none, null, null);
            _resize_func.parameters.AddElement(par0);
            _resize_func.parameters.AddElement(par1);
            resize_func.compile_time_executor = set_length_compile_time_executor;
            
        }

        private static void make_constants()
        {
            _true_constant = new bool_const_node(true, null);
            _false_constant = new bool_const_node(false, null);
        }

        private static basic_function_node make_object_operator(type_node ctn, SemanticTree.basic_function_type bas_ft,
            string name, type_node ret_type, SemanticTree.parameter_type first_parameter_type)
        {
            basic_function_node bfn = new basic_function_node(bas_ft, ret_type,true,name);
            basic_parameter to = new basic_parameter(StringConstants.left_param_name, ctn,
                first_parameter_type, bfn);
            basic_parameter from = new basic_parameter(StringConstants.right_param_name, ctn,
                SemanticTree.parameter_type.value, bfn);
            bfn.parameters.AddElement(to);
            bfn.parameters.AddElement(from);
            ctn.add_name(name, new SymbolInfo(bfn)); 
            add_stand_type(bas_ft, bfn);
            return bfn;
        }

        private static basic_function_node make_object_operator(type_node ctn, SemanticTree.basic_function_type bas_ft, string name,
            type_node ret_type)
        {
            return make_object_operator(ctn, bas_ft, name, ret_type, SemanticTree.parameter_type.value);
        }

        public static basic_function_node make_assign_operator(type_node ctn,SemanticTree.basic_function_type assign_method)
        {
            basic_function_node ret = make_object_operator(ctn, assign_method, StringConstants.assign_name,
                ctn, SemanticTree.parameter_type.var);
            ret.operation_kind = special_operation_kind.assign;
            return ret;
        }

        private static basic_function_node make_assign_operator(type_node ctn)
        {
            return make_assign_operator(ctn, SemanticTree.basic_function_type.objassign);
        }

        public static basic_function_node make_equivalence_operator(type_node ctn)
        {
            return make_object_operator(ctn, SemanticTree.basic_function_type.objeq, StringConstants.eq_name, _bool_type);
        }

        public static basic_function_node make_not_equivalence_operator(type_node ctn)
        {
            return make_object_operator(ctn, SemanticTree.basic_function_type.objnoteq, StringConstants.noteq_name, _bool_type);
        }

        public static void init_reference_type(type_node tn)
        {
        	if (_bool_type == null) 
        	{
        		wait_add_ref_list.Add(tn);
        		return;
        	}
            if (!tn.is_ref_inited)
            {
                make_assign_operator(tn);
                make_equivalence_operator(tn);
                make_not_equivalence_operator(tn);
                tn.is_ref_inited = true;
            }
        }

        public static basic_function_node find_operator(SemanticTree.basic_function_type bft)
        {
            return ht[bft];
        }

        private static void add_stand_type(SemanticTree.basic_function_type bft, basic_function_node bfn)
        {
            ht[bft] = bfn;
        }

        private static void initialize()
        {
            make_types();
            make_methods();
            make_constants();
        }

        public static void InitSystemLib(System.StringComparer string_comparer)
        {
            _string_comparer = string_comparer;
        }

        static SystemLibrary()
        {
            //TODO: Запихнуть этот вызов куда-нибудь в другое место. В принципе должен быть класс, характеризующий язык, в котором делаются все такие вызовы.
            InitSystemLib(System.StringComparer.OrdinalIgnoreCase);
            initialize();
        }

		public static compiled_type_node attribute_type
		{
			get
			{
				return _attribute_type;
			}
		}
		
        public static compiled_type_node array_of_string
        {
            get
            {
                return _array_of_string;
            }
        }

        public static compiled_function_node delegate_combine_method
        {
            get
            {
                return _delegate_combine_method;
            }
        }

        public static compiled_function_node object_equals_method
        {
            get
            {
                return _object_equals_method;
            }
        }

        public static compiled_function_node delegate_remove_method
        {
            get
            {
                return _delegate_remove_method;
            }
        }

        public static System.StringComparer string_comparer
        {
            get
            {
                return _string_comparer;
            }
        }

		public static type_node bool_type
		{
			get
			{
				return _bool_type;
			}
		}

        public static type_node decimal_type
        {
            get
            {
                return _decimal_type;
            }
        }


        public static type_node byte_type
        {
            get
            {
                return _byte_type;
            }
        }

        public static type_node sbyte_type
        {
            get
            {
                return _sbyte_type;
            }
        }

        public static type_node short_type
        {
            get
            {
                return _short_type;
            }
        }

        public static type_node ushort_type
        {
            get
            {
                return _ushort_type;
            }
        }

        public static type_node integer_type
		{
			get
			{
				return _integer_type;
			}
		}

        public static type_node uint_type
        {
            get
            {
                return _uint_type;
            }
        }

        public static type_node int64_type
        {
            get
            {
                return _int64_type;
            }
        }

        public static type_node uint64_type
        {
            get
            {
                return _uint64_type;
            }
        }

        public static type_node double_type
		{
			get
			{
				return _double_type;
			}
		}

        public static type_node float_type
        {
            get
            {
                return _float_type;
            }
        }

		public static type_node char_type
		{
			get
			{
				return _char_type;
			}
		}
        
        public static type_node string_type
		{
			get
			{
				return _string_type;
			}
		}

        public static string_const_node empty_string
        {
            get
            {
                return _empty_string;
            }
        }

        public static type_node object_type
        {
            get
            {
                return _object_type;
            }
        }

        public static type_node complex_type
        {
            get
            {
                return _complex_type;
            }
        }

        public static compiled_constructor_node complex_type_constructor
        {
            get
            {
                return _complex_type_constructor;
            }
        }

        public static type_node flags_attribute_type
        {
        	get
        	{
        		return _flags_attribute_type;
        	}
        }
        
        public static type_node dllimport_type
        {
        	get
        	{
        		return _dllimport_type;
        	}
        }
        
        public static compiled_type_node usage_attribute_type
        {
        	get
        	{
        		return _usage_attribute_type;
        	}
        }
        
        public static type_node comimport_type
        {
        	get
        	{
        		return _comimport_type;
        	}
        }
        
        public static type_node field_offset_attribute_type
        {
        	get
        	{
        		return _field_offset_attribute_type;
        	}
        }
        
        public static type_node struct_layout_attribute_type
        {
        	get
        	{
        		return _struct_layout_attribute_type;
        	}
        }
        
        public static type_node array_base_type
        {
            get
            {
                return _array_base_type;
            }
        }

        public static type_node delegate_base_type
        {
            get
            {
                return _delegate_base_type;
            }
        }

        public static type_node system_delegate_type
        {
            get
            {
                return _system_delegate_type;
            }
        }

        public static type_node void_type
        {
            get
            {
                return _void_type;
            }
        }
        public static type_node value_type
        {
            get
            {
                return _value_type;
            }
        }

        public static type_node exception_base_type
        {
            get
            {
                return _exception_base_type;
            }
        }

        public static type_node enum_base_type
        {
            get
            {
                return _enum_base_type;
            }
        }

		public static type_node pointer_type
		{
			get
			{
				return _pointer_type;
			}
		}

        public static bool_const_node true_constant
        {
            get
            {
                return _true_constant;
            }
        }

        public static bool_const_node false_constant
        {
            get
            {
                return _false_constant;
            }
        }

        public static function_node resize_func
        {
            get
            {
                return _resize_func;
            }
        }

        public static function_node string_add
        {
            get
            {
                return _string_add;
            }
        }

        public static function_node char_add
        {
            get
            {
                return _char_add;
            }
        }

        public static function_node char_to_string
        {
            get
            {
                return _char_to_string;
            }
        }
        
        public static function_node empty_method
        {
            get
            {
                return _empty_method;
            }
        }

        public static SymbolTable.TreeConverterSymbolTable symtab
        {
            get
            {
                return SymbolTable.SymbolTableController.CurrentSymbolTable;
            }
        }

        public static function_node obj_to_obj
        {
            get
            {
                return _obj_to_obj;
            }
        }




        
        //byte

        public static function_node byte_to_sbyte
        {
            get
            {
                return _byte_to_sbyte;
            }
        }

        public static function_node byte_to_short
        {
            get
            {
                return _byte_to_short;
            }
        }

        public static function_node byte_to_ushort
        {
            get
            {
                return _byte_to_ushort;
            }
        }

        public static function_node byte_to_int_bfn
        {
            get
            {
                return _byte_to_int;
            }
        }

        public static function_node byte_to_uint
        {
            get
            {
                return _byte_to_uint;
            }
        }

        public static function_node byte_to_long
        {
            get
            {
                return _byte_to_long;
            }
        }

        public static function_node byte_to_ulong
        {
            get
            {
                return _byte_to_ulong;
            }
        }

        public static function_node byte_to_char
        {
            get
            {
                return _byte_to_char;
            }
        }


        public static function_node byte_to_float
        {
            get
            {
                return _byte_to_float;
            }
        }

        public static function_node byte_to_double
        {
            get
            {
                return _byte_to_double;
            }
        }

        public static function_node byte_assign
        {
            get
            {
                return _byte_assign;
            }
        }

        public static function_node byte_unmin
        {
            get
            {
                return _byte_unmin;
            }
        }

        public static function_node byte_not
        {
            get
            {
                return _byte_not;
            }
        }
        
        public static function_node byte_add
        {
            get
            {
                return _byte_add;
            }
        }

        public static function_node byte_sub
        {
            get
            {
                return _byte_sub;
            }
        }

        public static function_node byte_mul
        {
            get
            {
                return _byte_mul;
            }
        }

        public static function_node byte_div
        {
            get
            {
                return _byte_div;
            }
        }

        public static function_node byte_mod
        {
            get
            {
                return _byte_mod;
            }
        }

        public static function_node byte_idiv
        {
            get
            {
                return _byte_idiv;
            }
        }

        public static function_node byte_gr
        {
            get
            {
                return _byte_gr;
            }
        }

        public static function_node byte_greq
        {
            get
            {
                return _byte_greq;
            }
        }

        public static function_node byte_sm
        {
            get
            {
                return _byte_sm;
            }
        }

        public static function_node byte_smeq
        {
            get
            {
                return _byte_smeq;
            }
        }

        public static function_node byte_eq
        {
            get
            {
                return _byte_eq;
            }
        }

        public static function_node byte_noteq
        {
            get
            {
                return _byte_noteq;
            }
        }

        public static function_node byte_and
        {
            get
            {
                return _byte_and;
            }
        }

        public static function_node byte_or
        {
            get
            {
                return _byte_or;
            }
        }

        public static function_node byte_xor
        {
            get
            {
                return _byte_xor;
            }
        }
        /*
        public static function_node byte_shl
        {
            get
            {
                return _byte_shl;
            }
        }

        public static function_node byte_shr
        {
            get
            {
                return _byte_shr;
            }
        }
        */


        //sbyte
        public static function_node sbyte_to_byte
        {
            get
            {
                return _sbyte_to_byte;
            }
        }

        public static function_node sbyte_to_short
        {
            get
            {
                return _sbyte_to_short;
            }
        }

        public static function_node sbyte_to_ushort
        {
            get
            {
                return _sbyte_to_ushort;
            }
        }

        public static function_node sbyte_to_int_bfn
        {
            get
            {
                return _sbyte_to_int;
            }
        }

        public static function_node sbyte_to_uint
        {
            get
            {
                return _sbyte_to_uint;
            }
        }

        public static function_node sbyte_to_long
        {
            get
            {
                return _sbyte_to_long;
            }
        }

        public static function_node sbyte_to_ulong
        {
            get
            {
                return _sbyte_to_ulong;
            }
        }

        public static function_node sbyte_to_char
        {
            get
            {
                return _sbyte_to_char;
            }
        }

        public static function_node sbyte_to_float
        {
            get
            {
                return _sbyte_to_float;
            }
        }

        public static function_node sbyte_to_double
        {
            get
            {
                return _sbyte_to_double;
            }
        }

        public static function_node sbyte_assign
        {
            get
            {
                return _sbyte_assign;
            }
        }

        public static function_node sbyte_unmin
        {
            get
            {
                return _sbyte_unmin;
            }
        }

        public static function_node sbyte_not
        {
            get
            {
                return _sbyte_not;
            }
        }

        public static function_node sbyte_add
        {
            get
            {
                return _sbyte_add;
            }
        }

        public static function_node sbyte_sub
        {
            get
            {
                return _sbyte_sub;
            }
        }

        public static function_node sbyte_mul
        {
            get
            {
                return _sbyte_mul;
            }
        }

        public static function_node sbyte_div
        {
            get
            {
                return _sbyte_div;
            }
        }

        public static function_node sbyte_mod
        {
            get
            {
                return _sbyte_mod;
            }
        }

        public static function_node sbyte_idiv
        {
            get
            {
                return _sbyte_idiv;
            }
        }

        public static function_node sbyte_gr
        {
            get
            {
                return _sbyte_gr;
            }
        }

        public static function_node sbyte_greq
        {
            get
            {
                return _sbyte_greq;
            }
        }

        public static function_node sbyte_sm
        {
            get
            {
                return _sbyte_sm;
            }
        }

        public static function_node sbyte_smeq
        {
            get
            {
                return _sbyte_smeq;
            }
        }

        public static function_node sbyte_eq
        {
            get
            {
                return _sbyte_eq;
            }
        }

        public static function_node sbyte_noteq
        {
            get
            {
                return _sbyte_noteq;
            }
        }

        public static function_node sbyte_and
        {
            get
            {
                return _sbyte_and;
            }
        }

        public static function_node sbyte_or
        {
            get
            {
                return _sbyte_or;
            }
        }

        public static function_node sbyte_xor
        {
            get
            {
                return _sbyte_xor;
            }
        }
        /*
        public static function_node sbyte_shl
        {
            get
            {
                return _sbyte_shl;
            }
        }

        public static function_node sbyte_shr
        {
            get
            {
                return _sbyte_shr;
            }
        }
        */



        //int
        public static function_node int_to_byte
        {
            get
            {
                return _int_to_byte;
            }
        }

        public static function_node int_to_sbyte
        {
            get
            {
                return _int_to_sbyte;
            }
        }

        public static function_node int_to_short
        {
            get
            {
                return _int_to_short;
            }
        }

        public static function_node int_to_ushort
        {
            get
            {
                return _int_to_ushort;
            }
        }

        public static function_node int_to_uint
        {
            get
            {
                return _int_to_uint;
            }
        }

        public static function_node int_to_long
        {
            get
            {
                return _int_to_long;
            }
        }

        public static function_node int_to_ulong
        {
            get
            {
                return _int_to_ulong;
            }
        }

        public static function_node int_to_char
        {
            get
            {
                return _int_to_char;
            }
        }


        public static function_node int_to_float
        {
            get
            {
                return _int_to_float;
            }
        }

        public static function_node int_to_double
        {
            get
            {
                return _int_to_double;
            }
        }

        public static function_node int_assign
        {
            get
            {
                return _int_assign;
            }
        }

        public static function_node int_unmin
        {
            get
            {
                return _int_unmin;
            }
        }

        public static function_node int_not
        {
            get
            {
                return _int_not;
            }
        }

        public static function_node int_add
        {
            get
            {
                return _int_add;
            }
        }

        public static function_node int_sub
        {
            get
            {
                return _int_sub;
            }
        }

        public static function_node int_mul
        {
            get
            {
                return _int_mul;
            }
        }

        public static function_node int_div
        {
            get
            {
                return _int_div;
            }
        }

        public static function_node int_mod
        {
            get
            {
                return _int_mod;
            }
        }

        public static function_node int_idiv
        {
            get
            {
                return _int_idiv;
            }
        }

        public static function_node int_gr
        {
            get
            {
                return _int_gr;
            }
        }

        public static function_node int_greq
        {
            get
            {
                return _int_greq;
            }
        }

        public static function_node int_sm
        {
            get
            {
                return _int_sm;
            }
        }

        public static function_node int_smeq
        {
            get
            {
                return _int_smeq;
            }
        }

        public static function_node int_eq
        {
            get
            {
                return _int_eq;
            }
        }

        public static function_node int_noteq
        {
            get
            {
                return _int_noteq;
            }
        }

        public static function_node int_and
        {
            get
            {
                return _int_and;
            }
        }

        public static function_node int_or
        {
            get
            {
                return _int_or;
            }
        }

        public static function_node int_xor
        {
            get
            {
                return _int_xor;
            }
        }

        public static function_node int_shl
        {
            get
            {
                return _int_shl;
            }
        }

        public static function_node int_shr
        {
            get
            {
                return _int_shr;
            }
        }



        //short
        public static function_node short_to_byte
        {
            get
            {
                return _short_to_byte;
            }
        }

        public static function_node short_to_sbyte
        {
            get
            {
                return _short_to_sbyte;
            }
        }

        public static function_node short_to_ushort
        {
            get
            {
                return _short_to_ushort;
            }
        }

        public static function_node short_to_int_bfn
        {
            get
            {
                return _short_to_int;
            }
        }

        public static function_node short_to_uint
        {
            get
            {
                return _short_to_uint;
            }
        }

        public static function_node short_to_long
        {
            get
            {
                return _short_to_long;
            }
        }

        public static function_node short_to_ulong
        {
            get
            {
                return _short_to_ulong;
            }
        }

        public static function_node short_to_char
        {
            get
            {
                return _short_to_char;
            }
        }


        public static function_node short_to_float
        {
            get
            {
                return _short_to_float;
            }
        }

        public static function_node short_to_double
        {
            get
            {
                return _short_to_double;
            }
        }

        public static function_node short_assign
        {
            get
            {
                return _short_assign;
            }
        }

        public static function_node short_unmin
        {
            get
            {
                return _short_unmin;
            }
        }

        public static function_node short_not
        {
            get
            {
                return _short_not;
            }
        }

        public static function_node short_add
        {
            get
            {
                return _short_add;
            }
        }

        public static function_node short_sub
        {
            get
            {
                return _short_sub;
            }
        }

        public static function_node short_mul
        {
            get
            {
                return _short_mul;
            }
        }

        public static function_node short_div
        {
            get
            {
                return _short_div;
            }
        }

        public static function_node short_mod
        {
            get
            {
                return _short_mod;
            }
        }

        public static function_node short_idiv
        {
            get
            {
                return _short_idiv;
            }
        }

        public static function_node short_gr
        {
            get
            {
                return _short_gr;
            }
        }

        public static function_node short_greq
        {
            get
            {
                return _short_greq;
            }
        }

        public static function_node short_sm
        {
            get
            {
                return _short_sm;
            }
        }

        public static function_node short_smeq
        {
            get
            {
                return _short_smeq;
            }
        }

        public static function_node short_eq
        {
            get
            {
                return _short_eq;
            }
        }

        public static function_node short_noteq
        {
            get
            {
                return _short_noteq;
            }
        }

        public static function_node short_and
        {
            get
            {
                return _short_and;
            }
        }

        public static function_node short_or
        {
            get
            {
                return _short_or;
            }
        }

        public static function_node short_xor
        {
            get
            {
                return _short_xor;
            }
        }

        /*
        public static function_node short_shl
        {
            get
            {
                return _short_shl;
            }
        }

        public static function_node short_shr
        {
            get
            {
                return _short_shr;
            }
        }
        */




        //ushort
        public static function_node ushort_to_byte
        {
            get
            {
                return _ushort_to_byte;
            }
        }

        public static function_node ushort_to_sbyte
        {
            get
            {
                return _ushort_to_sbyte;
            }
        }

        public static function_node ushort_to_short
        {
            get
            {
                return _ushort_to_short;
            }
        }

        public static function_node ushort_to_int_bfn
        {
            get
            {
                return _ushort_to_int;
            }
        }

        public static function_node ushort_to_uint
        {
            get
            {
                return _ushort_to_uint;
            }
        }

        public static function_node ushort_to_long
        {
            get
            {
                return _ushort_to_long;
            }
        }

        public static function_node ushort_to_ulong
        {
            get
            {
                return _ushort_to_ulong;
            }
        }

        public static function_node ushort_to_char
        {
            get
            {
                return _ushort_to_char;
            }
        }


        public static function_node ushort_to_float
        {
            get
            {
                return _ushort_to_float;
            }
        }

        public static function_node ushort_to_double
        {
            get
            {
                return _ushort_to_double;
            }
        }

        public static function_node ushort_assign
        {
            get
            {
                return _ushort_assign;
            }
        }

        public static function_node ushort_unmin
        {
            get
            {
                return _ushort_unmin;
            }
        }

        public static function_node ushort_not
        {
            get
            {
                return _ushort_not;
            }
        }

        public static function_node ushort_add
        {
            get
            {
                return _ushort_add;
            }
        }

        public static function_node ushort_sub
        {
            get
            {
                return _ushort_sub;
            }
        }

        public static function_node ushort_mul
        {
            get
            {
                return _ushort_mul;
            }
        }

        public static function_node ushort_div
        {
            get
            {
                return _ushort_div;
            }
        }

        public static function_node ushort_mod
        {
            get
            {
                return _ushort_mod;
            }
        }

        public static function_node ushort_idiv
        {
            get
            {
                return _ushort_idiv;
            }
        }

        public static function_node ushort_gr
        {
            get
            {
                return _ushort_gr;
            }
        }

        public static function_node ushort_greq
        {
            get
            {
                return _ushort_greq;
            }
        }

        public static function_node ushort_sm
        {
            get
            {
                return _ushort_sm;
            }
        }

        public static function_node ushort_smeq
        {
            get
            {
                return _ushort_smeq;
            }
        }

        public static function_node ushort_eq
        {
            get
            {
                return _ushort_eq;
            }
        }

        public static function_node ushort_noteq
        {
            get
            {
                return _ushort_noteq;
            }
        }

        public static function_node ushort_and
        {
            get
            {
                return _ushort_and;
            }
        }

        public static function_node ushort_or
        {
            get
            {
                return _ushort_or;
            }
        }

        public static function_node ushort_xor
        {
            get
            {
                return _ushort_xor;
            }
        }

        /*
        public static function_node ushort_shl
        {
            get
            {
                return _ushort_shl;
            }
        }

        public static function_node ushort_shr
        {
            get
            {
                return _ushort_shr;
            }
        }
        */




        //uint
        public static function_node uint_to_byte
        {
            get
            {
                return _uint_to_byte;
            }
        }

        public static function_node uint_to_sbyte
        {
            get
            {
                return _uint_to_sbyte;
            }
        }

        public static function_node uint_to_short
        {
            get
            {
                return _uint_to_short;
            }
        }

        public static function_node uint_to_ushort
        {
            get
            {
                return _uint_to_ushort;
            }
        }

        public static function_node uint_to_int_bfn
        {
            get
            {
                return _uint_to_int;
            }
        }

        public static function_node uint_to_long
        {
            get
            {
                return _uint_to_long;
            }
        }

        public static function_node uint_to_ulong
        {
            get
            {
                return _uint_to_ulong;
            }
        }

        public static function_node uint_to_char
        {
            get
            {
                return _uint_to_char;
            }
        }


        public static function_node uint_to_float
        {
            get
            {
                return _uint_to_float;
            }
        }

        public static function_node uint_to_double
        {
            get
            {
                return _uint_to_double;
            }
        }

        public static function_node uint_assign
        {
            get
            {
                return _uint_assign;
            }
        }

        public static function_node uint_unmin
        {
            get
            {
                return _uint_unmin;
            }
        }

        public static function_node uint_not
        {
            get
            {
                return _uint_not;
            }
        }

        public static function_node uint_add
        {
            get
            {
                return _uint_add;
            }
        }

        public static function_node uint_sub
        {
            get
            {
                return _uint_sub;
            }
        }

        public static function_node uint_mul
        {
            get
            {
                return _uint_mul;
            }
        }

        public static function_node uint_div
        {
            get
            {
                return _uint_div;
            }
        }

        public static function_node uint_mod
        {
            get
            {
                return _uint_mod;
            }
        }

        public static function_node uint_idiv
        {
            get
            {
                return _uint_idiv;
            }
        }

        public static function_node uint_gr
        {
            get
            {
                return _uint_gr;
            }
        }

        public static function_node uint_greq
        {
            get
            {
                return _uint_greq;
            }
        }

        public static function_node uint_sm
        {
            get
            {
                return _uint_sm;
            }
        }

        public static function_node uint_smeq
        {
            get
            {
                return _uint_smeq;
            }
        }

        public static function_node uint_eq
        {
            get
            {
                return _uint_eq;
            }
        }

        public static function_node uint_noteq
        {
            get
            {
                return _uint_noteq;
            }
        }

        public static function_node uint_and
        {
            get
            {
                return _uint_and;
            }
        }

        public static function_node uint_or
        {
            get
            {
                return _uint_or;
            }
        }

        public static function_node uint_xor
        {
            get
            {
                return _uint_xor;
            }
        }

        public static function_node uint_shl
        {
            get
            {
                return _uint_shl;
            }
        }

        public static function_node uint_shr
        {
            get
            {
                return _uint_shr;
            }
        }




        //long
        public static function_node long_to_byte
        {
            get
            {
                return _long_to_byte;
            }
        }

        public static function_node long_to_sbyte
        {
            get
            {
                return _long_to_sbyte;
            }
        }

        public static function_node long_to_short
        {
            get
            {
                return _long_to_short;
            }
        }

        public static function_node long_to_ushort
        {
            get
            {
                return _long_to_ushort;
            }
        }

        public static function_node long_to_int_bfn
        {
            get
            {
                return _long_to_int;
            }
        }

        public static function_node long_to_uint
        {
            get
            {
                return _long_to_uint;
            }
        }

        public static function_node long_to_ulong
        {
            get
            {
                return _long_to_ulong;
            }
        }

        public static function_node long_to_char
        {
            get
            {
                return _long_to_char;
            }
        }


        public static function_node long_to_float
        {
            get
            {
                return _long_to_float;
            }
        }

        public static function_node long_to_double
        {
            get
            {
                return _long_to_double;
            }
        }

        public static function_node long_assign
        {
            get
            {
                return _long_assign;
            }
        }

        public static function_node long_unmin
        {
            get
            {
                return _long_unmin;
            }
        }

        public static function_node long_not
        {
            get
            {
                return _long_not;
            }
        }

        public static function_node long_add
        {
            get
            {
                return _long_add;
            }
        }

        public static function_node long_sub
        {
            get
            {
                return _long_sub;
            }
        }

        public static function_node long_mul
        {
            get
            {
                return _long_mul;
            }
        }

        public static function_node long_div
        {
            get
            {
                return _long_div;
            }
        }

        public static function_node long_mod
        {
            get
            {
                return _long_mod;
            }
        }

        public static function_node long_idiv
        {
            get
            {
                return _long_idiv;
            }
        }

        public static function_node long_gr
        {
            get
            {
                return _long_gr;
            }
        }

        public static function_node long_greq
        {
            get
            {
                return _long_greq;
            }
        }

        public static function_node long_sm
        {
            get
            {
                return _long_sm;
            }
        }

        public static function_node long_smeq
        {
            get
            {
                return _long_smeq;
            }
        }

        public static function_node long_eq
        {
            get
            {
                return _long_eq;
            }
        }

        public static function_node long_noteq
        {
            get
            {
                return _long_noteq;
            }
        }

        public static function_node long_and
        {
            get
            {
                return _long_and;
            }
        }

        public static function_node long_or
        {
            get
            {
                return _long_or;
            }
        }

        public static function_node long_xor
        {
            get
            {
                return _long_xor;
            }
        }

        public static function_node long_shl
        {
            get
            {
                return _long_shl;
            }
        }

        public static function_node long_shr
        {
            get
            {
                return _long_shr;
            }
        }





        //ulong
        public static function_node ulong_to_byte
        {
            get
            {
                return _ulong_to_byte;
            }
        }

        public static function_node ulong_to_sbyte
        {
            get
            {
                return _ulong_to_sbyte;
            }
        }

        public static function_node ulong_to_short
        {
            get
            {
                return _ulong_to_short;
            }
        }

        public static function_node ulong_to_ushort
        {
            get
            {
                return _ulong_to_ushort;
            }
        }

        public static function_node ulong_to_int_bfn
        {
            get
            {
                return _ulong_to_int;
            }
        }

        public static function_node ulong_to_uint
        {
            get
            {
                return _ulong_to_uint;
            }
        }

        public static function_node ulong_to_long
        {
            get
            {
                return _ulong_to_long;
            }
        }

        public static function_node ulong_to_char
        {
            get
            {
                return _ulong_to_char;
            }
        }


        public static function_node ulong_to_float
        {
            get
            {
                return _ulong_to_float;
            }
        }

        public static function_node ulong_to_double
        {
            get
            {
                return _ulong_to_double;
            }
        }

        public static function_node ulong_assign
        {
            get
            {
                return _ulong_assign;
            }
        }
        
        public static function_node ulong_unmin
        {
            get
            {
                return _ulong_unmin;
            }
        }
        
        public static function_node ulong_not
        {
            get
            {
                return _ulong_not;
            }
        }

        public static function_node ulong_add
        {
            get
            {
                return _ulong_add;
            }
        }

        public static function_node ulong_sub
        {
            get
            {
                return _ulong_sub;
            }
        }

        public static function_node ulong_mul
        {
            get
            {
                return _ulong_mul;
            }
        }

        public static function_node ulong_div
        {
            get
            {
                return _ulong_div;
            }
        }

        public static function_node ulong_mod
        {
            get
            {
                return _ulong_mod;
            }
        }

        public static function_node ulong_idiv
        {
            get
            {
                return _ulong_idiv;
            }
        }

        public static function_node ulong_gr
        {
            get
            {
                return _ulong_gr;
            }
        }

        public static function_node ulong_greq
        {
            get
            {
                return _ulong_greq;
            }
        }

        public static function_node ulong_sm
        {
            get
            {
                return _ulong_sm;
            }
        }

        public static function_node ulong_smeq
        {
            get
            {
                return _ulong_smeq;
            }
        }

        public static function_node ulong_eq
        {
            get
            {
                return _ulong_eq;
            }
        }

        public static function_node ulong_noteq
        {
            get
            {
                return _ulong_noteq;
            }
        }

        public static function_node ulong_and
        {
            get
            {
                return _ulong_and;
            }
        }

        public static function_node ulong_or
        {
            get
            {
                return _ulong_or;
            }
        }

        public static function_node ulong_xor
        {
            get
            {
                return _ulong_xor;
            }
        }

        public static function_node ulong_shl
        {
            get
            {
                return _ulong_shl;
            }
        }

        public static function_node ulong_shr
        {
            get
            {
                return _ulong_shr;
            }
        }



        

        //float
        

        public static function_node float_to_double
        {
            get
            {
                return _float_to_double;
            }
        }

        public static function_node float_assign
        {
            get
            {
                return _float_assign;
            }
        }

        public static function_node float_unmin
        {
            get
            {
                return _float_unmin;
            }
        }

        public static function_node float_add
        {
            get
            {
                return _float_add;
            }
        }

        public static function_node float_sub
        {
            get
            {
                return _float_sub;
            }
        }

        public static function_node float_mul
        {
            get
            {
                return _float_mul;
            }
        }

        public static function_node float_div
        {
            get
            {
                return _float_div;
            }
        }

        public static function_node float_gr
        {
            get
            {
                return _float_gr;
            }
        }

        public static function_node float_greq
        {
            get
            {
                return _float_greq;
            }
        }

        public static function_node float_sm
        {
            get
            {
                return _float_sm;
            }
        }

        public static function_node float_smeq
        {
            get
            {
                return _float_smeq;
            }
        }

        public static function_node float_eq
        {
            get
            {
                return _float_eq;
            }
        }

        public static function_node float_noteq
        {
            get
            {
                return _float_noteq;
            }
        }





        //real
        public static function_node double_to_byte
        {
            get
            {
                return _double_to_byte;
            }
        }

        public static function_node double_to_sbyte
        {
            get
            {
                return _double_to_sbyte;
            }
        }

        public static function_node double_to_short
        {
            get
            {
                return _double_to_short;
            }
        }

        public static function_node double_to_ushort
        {
            get
            {
                return _double_to_ushort;
            }
        }

        public static function_node double_to_int_bfn
        {
            get
            {
                return _double_to_int;
            }
        }

        public static function_node double_to_uint
        {
            get
            {
                return _double_to_uint;
            }
        }

        public static function_node double_to_long
        {
            get
            {
                return _double_to_long;
            }
        }

        public static function_node double_to_ulong
        {
            get
            {
                return _double_to_ulong;
            }
        }

        public static function_node double_to_char
        {
            get
            {
                return _double_to_char;
            }
        }


        public static function_node double_to_float
        {
            get
            {
                return _double_to_float;
            }
        }

        public static function_node real_assign
        {
            get
            {
                return _real_assign;
            }
        }

        public static function_node real_unmin
        {
            get
            {
                return _real_unmin;
            }
        }

        public static function_node real_add
        {
            get
            {
                return _real_add;
            }
        }

        public static function_node real_sub
        {
            get
            {
                return _real_sub;
            }
        }

        public static function_node real_mul
        {
            get
            {
                return _real_mul;
            }
        }

        public static function_node real_div
        {
            get
            {
                return _real_div;
            }
        }

        public static function_node real_gr
        {
            get
            {
                return _real_gr;
            }
        }

        public static function_node real_greq
        {
            get
            {
                return _real_greq;
            }
        }

        public static function_node real_sm
        {
            get
            {
                return _real_sm;
            }
        }

        public static function_node real_smeq
        {
            get
            {
                return _real_smeq;
            }
        }

        public static function_node real_eq
        {
            get
            {
                return _real_eq;
            }
        }

        public static function_node real_noteq
        {
            get
            {
                return _real_noteq;
            }
        }






        //char
        public static function_node char_to_byte
        {
            get
            {
                return _char_to_byte;
            }
        }

        public static function_node char_to_sbyte
        {
            get
            {
                return _char_to_sbyte;
            }
        }

        public static function_node char_to_short
        {
            get
            {
                return _char_to_short;
            }
        }

        public static function_node char_to_ushort
        {
            get
            {
                return _char_to_short;
            }
        }

        public static function_node char_to_int_bfn
        {
            get
            {
                return _char_to_int;
            }
        }

        public static function_node char_to_uint
        {
            get
            {
                return _char_to_uint;
            }
        }

        public static function_node char_to_long
        {
            get
            {
                return _char_to_long;
            }
        }

        public static function_node char_to_ulong
        {
            get
            {
                return _char_to_ulong;
            }
        }

        public static function_node char_to_float
        {
            get
            {
                return _char_to_float;
            }
        }

        public static function_node char_to_double
        {
            get
            {
                return _char_to_double;
            }
        }

        public static function_node char_assign
        {
            get
            {
                return _char_assign;
            }
        }

        public static function_node char_gr
        {
            get
            {
                return _char_gr;
            }
        }

        public static function_node char_greq
        {
            get
            {
                return _char_greq;
            }
        }

        public static function_node char_sm
        {
            get
            {
                return _char_sm;
            }
        }

        public static function_node char_smeq
        {
            get
            {
                return _char_smeq;
            }
        }

        public static function_node char_eq
        {
            get
            {
                return _char_eq;
            }
        }

        public static function_node char_noteq
        {
            get
            {
                return _char_noteq;
            }
        }

        public static function_node bool_assign
        {
            get
            {
                return _bool_assign;
            }
        }

        public static function_node bool_not
        {
            get
            {
                return _bool_not;
            }
        }

        public static function_node bool_and
        {
            get
            {
                return _bool_and;
            }
        }

        public static function_node bool_or
        {
            get
            {
                return _bool_or;
            }
        }

        public static function_node bool_xor
        {
            get
            {
                return _bool_xor;
            }
        }

        public static function_node bool_gr
        {
            get
            {
                return _bool_gr;
            }
        }

        public static function_node bool_greq
        {
            get
            {
                return _bool_greq;
            }
        }

        public static function_node bool_sm
        {
            get
            {
                return _bool_sm;
            }
        }

        public static function_node bool_smeq
        {
            get
            {
                return _bool_smeq;
            }
        }

        public static function_node bool_eq
        {
            get
            {
                return _bool_eq;
            }
        }

        public static function_node bool_noteq
        {
            get
            {
                return _bool_noteq;
            }
        }
		
        public static function_node enum_gr
        {
        	get
        	{
        		return _enum_gr;
        	}
        }
        
        public static function_node enum_sm
        {
        	get
        	{
        		return _enum_sm;
        	}
        }
        
        public static function_node enum_greq
        {
        	get
        	{
        		return _enum_greq;
        	}
        }
        
        public static function_node enum_smeq
        {
        	get
        	{
        		return _enum_smeq;
        	}
        }

        public static basic_function_node int64_to_pointer
        {
            get
            {
                return _int64_to_pointer;
            }
        }

        public static basic_function_node pointer_to_int64
        {
            get
            {
                return _pointer_to_int64;
            }
        }

        public static compiled_type_node icloneable_interface
        {
            get
            {
                return _icloneable_interface;
            }
        }

        public static compiled_type_node ilist_interface
        {
            get
            {
                return _ilist_interface;
            }
        }

        public static compiled_type_node ienumerable_interface
        {
            get
            {
                return _ienumerable_interface;
            }
        }

        public static compiled_type_node icollection_interface
        {
            get
            {
                return _icollection_interface;
            }
        }

        public static compiled_type_node ilist1_interface
        {
            get
            {
                return _ilist1_interface;
            }
        }

        public static compiled_type_node icollection1_interface
        {
            get
            {
                return _icollection1_interface;
            }
        }

        public static compiled_type_node ienumerable1_interface
        {
            get
            {
                return _ienumerable1_interface;
            }
        }

        public static compiled_type_node ireadonlycollection_interface
        {
            get
            {
                return _ireadonlycollection_interface;
            }
        }

        public static compiled_type_node ireadonlylist_interface
        {
            get
            {
                return _ireadonlylist_interface;
            }
        }

        public static compiled_function_node assert_method
        {
        	get
        	{
        		return _assert_method;
        	}
        }

    }
}
