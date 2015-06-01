// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

using PascalABCCompiler.Collections;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Класс, представляющий список выражений. Используется для представления фактических параметров функции.
    /// </summary>
    [Serializable]
    public class expressions_list : extended_collection<expression_node>
    {
    }
	
    [Serializable]
    public class attributes_list : extended_collection<attribute_node>
    {
    }
    
    /// <summary>
    /// Список типов, определенных в программе.
    /// </summary>
    [Serializable]
    public class common_type_node_list : extended_collection<common_type_node>
    {
    }

    //ssyy добавил
    /// <summary>
    /// Список шаблонных классов, определенных в программе.
    /// </summary>
    [Serializable]
    public class template_class_list : extended_collection<template_class>
    {
    }
    //\ssyy

    /// <summary>
    /// Список runtime типов, определенных в программе.
    /// </summary>
    [Serializable]
    public class compiled_type_node_list : extended_collection<compiled_type_node>
    {
    }

    /// <summary>
    /// Список переменных, определенных в пространстве имен.
    /// </summary>
    [Serializable]
    public class namespace_variable_list : extendable_collection<namespace_variable>
    {
    }

    [Serializable]
    public class namespace_event_list : extendable_collection<common_namespace_event>
    {
    }

    /// <summary>
    /// Список функций, определенных в пространстве имен.
    /// </summary>
    [Serializable]
    public class common_namespace_function_node_list : extended_collection<common_namespace_function_node>
    {
    }

    /// <summary>
    /// Список вложенных пространств имен.
    /// </summary>
    [Serializable]
    public class common_namespace_node_list : extendable_collection<common_namespace_node>
    {
    }

    /// <summary>
    /// Список вложенных в пространство имен констант.
    /// </summary>
    [Serializable]
    public class namespace_constant_definition_list : extendable_collection<namespace_constant_definition>
    {
    }

    /// <summary>
    /// Список вложенных в класс.
    /// </summary>
    [Serializable]
    public class class_constant_definition_list : extendable_collection<class_constant_definition>
    {
    }

    /// <summary>
    /// Список констант, вложенных в класс.
    /// </summary>
    [Serializable]
    public class function_constant_definition_list : extendable_collection<function_constant_definition>
    {
    }

    /// <summary>
    /// Список модулей.
    /// </summary>
    [Serializable]
    public class unit_node_list : extended_collection<unit_node>
    {
    }

    /// <summary>
    /// Список параметров.
    /// </summary>
    [Serializable]
    public class parameter_list : extended_collection<parameter>
    {
    }

    /// <summary>
    /// Список выражений.
    /// </summary>
    [Serializable]
    public class statement_node_list : extendable_collection<statement_node>
    {
    }

    /// <summary>
    /// Список методов.
    /// </summary>
    [Serializable]
    public class common_method_node_list : extended_collection<common_method_node>
    {
    }

    /// <summary>
    /// Список полей класса.
    /// </summary>
    [Serializable]
    public class class_field_list : extendable_collection<class_field>
    {
    }

    /// <summary>
    /// Список свойств класса.
    /// </summary>
    [Serializable]
    public class common_property_node_list : extendable_collection<common_property_node>
    {
    }


    [Serializable]
    public class common_event_list : extendable_collection<common_event>
    {
    	
    }
    
    /// <summary>
    /// Список локальных переменных.
    /// </summary>
    [Serializable]
    public class local_variable_list : extendable_collection<local_variable>
    {
    }

    /// <summary>
    /// Список функций, определенных в функции.
    /// </summary>
    [Serializable]
    public class common_in_function_function_node_list : extended_collection<common_in_function_function_node>
    {
    }

    [Serializable]
    public class statement_node_stack : stack<statement_node>
    {
    }

    [Serializable]
    public class possible_type_convertions_list : extended_collection<possible_type_convertions>
    {
        private statement_node_list _snl;
        private expression_node _var_ref;

        public statement_node_list snl
        {
            get
            {
                return _snl;
            }
            set
            {
                _snl = value;
            }
        }

        public expression_node var_ref
        {
            get
            {
                return _var_ref;
            }
            set
            {
                _var_ref = value;
            }
        }
        
    }

    [Serializable]
    public class possible_type_convertions_list_list : extended_collection<possible_type_convertions_list>
    {
    }

    [Serializable]
    public class function_node_list : extended_collection<function_node>
    {
    }

    [Serializable]
    public class using_namespace_list : extended_collection<using_namespace>
    {
    }

    [Serializable]
    public class type_node_list : extended_collection<type_node>
    {
    }

    [Serializable]
    public class common_unit_node_list : extended_collection<common_unit_node>
    {
    }

    [Serializable]
    public class constant_node_list : extendable_collection<constant_node>
    {
    }

    [Serializable]
    public class case_range_node_list : extendable_collection<case_range_node>
    {
    }

    [Serializable]
    public class case_variant_node_list : extendable_collection<case_variant_node>
    {
    }

    [Serializable]
    public class int_const_node_list : extendable_collection<int_const_node>
    {
    }

    [Serializable]
    public class base_function_call_list : extended_collection<base_function_call>
    {
    }

    [Serializable]
    public class exception_filters_list : extended_collection<exception_filter>
    {
    }
}
