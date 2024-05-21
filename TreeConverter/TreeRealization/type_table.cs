// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Метод преобразования одного типа к другому.
    /// </summary>
    [Serializable]
    public class type_conversion
    {
        private function_node _conversion_method;
        private bool _is_explicit = false;

        public type_conversion(function_node conversion)
        {
            _conversion_method = conversion;
        }

        public type_conversion(function_node conversion, bool is_explicit)
        {
            _conversion_method = conversion;
            _is_explicit = is_explicit;
        }

        public bool is_explicit
        {
            get
            {
                return _is_explicit;
            }
        }

        public type_node from
        {
            get
            {
                return _conversion_method.parameters[0].type;
            }
        }

        public type_node to
        {
            get
            {
                return _conversion_method.return_value_type;
            }
        }

        public function_node convertion_method
        {
            get
            {
                return _conversion_method;
            }
        }
    }

    public enum type_compare { less_type, greater_type, non_comparable_type };

    /// <summary>
    /// Узел пересечения типов. Хранит информацию, общую для пары типов. Например, операции приведения от одного типа к другому.
    /// </summary>
    [Serializable]
    public class type_intersection_node
    {
        private type_compare _type_compare = type_compare.non_comparable_type;

        private type_conversion _this_to_another;
        private type_conversion _another_to_this;

        public type_intersection_node()
        {
        }

        public type_intersection_node(type_compare type_compare)
        {
            _type_compare = type_compare;
        }

        public type_conversion this_to_another
        {
            get
            {
                return _this_to_another;
            }
            set
            {
                _this_to_another = value;
            }
        }

        public type_conversion another_to_this
        {
            get
            {
                return _another_to_this;
            }
            set
            {
                _another_to_this = value;
            }
        }

        public type_compare type_compare
        {
            get
            {
                return _type_compare;
            }
            set
            {
                _type_compare = value;
            }
        }
    }

    public class possible_type_convertions
    {
        public type_conversion first = null;
        public type_conversion second = null;
        public type_node from;
        public type_node to;
    }

    /// <summary>
    /// Таблица типов.
    /// </summary>
	public static class type_table
    {
        private static type_intersection_node change_to_opposite(type_intersection_node tin2)
        {
            type_intersection_node new_tin = null;
            if (tin2.type_compare == type_compare.greater_type)
                new_tin = new type_intersection_node(type_compare.less_type);
            else if (tin2.type_compare == type_compare.less_type)
                new_tin = new type_intersection_node(type_compare.greater_type);
            else
                new_tin = new type_intersection_node(type_compare.non_comparable_type);
            new_tin.another_to_this = tin2.another_to_this;
            new_tin.this_to_another = tin2.this_to_another;
            return new_tin;
        }
        private static type_intersection_node[] get_type_intersections_in_specific_order(type_node left, type_node right)
        {
            type_intersection_node tin1 = left.get_type_intersection(right);
            type_intersection_node tin2 = right.get_type_intersection(left);

            if (tin1 == null)
            {
                if (tin2 == null)
                {
                    return new type_intersection_node[0];
                }
                else
                {
                    if (tin2.another_to_this != null && tin2.another_to_this.is_explicit)
                        return new type_intersection_node[0];
                    type_intersection_node[] tinarr = new type_intersection_node[1];

                    /*type_intersection_node new_tin = null;
                    if (tin2.type_compare == type_compare.greater_type)
                        new_tin = new type_intersection_node(type_compare.less_type);
                    else if (tin2.type_compare == type_compare.less_type)
                        new_tin = new type_intersection_node(type_compare.greater_type);
                    else
                        new_tin = new type_intersection_node(type_compare.non_comparable_type);
                    new_tin.another_to_this = tin2.another_to_this;
                    new_tin.this_to_another = tin2.this_to_another;*/

                    tinarr[0] = change_to_opposite(tin2);
                    return tinarr;
                }
            }
            else
            {
                if (tin2 == null)
                {
                    if (tin1.this_to_another != null && tin1.this_to_another.is_explicit)
                        return new type_intersection_node[0];
                    type_intersection_node[] tinarr2 = new type_intersection_node[1];
                    tinarr2[0] = tin1;
                    return tinarr2;
                }
                else
                {
                    type_intersection_node[] tinarr3 = new type_intersection_node[2];
                    tinarr3[0] = tin1;
                    tinarr3[1] = tin2;
                    return tinarr3;
                }
            }
        }

        private static type_intersection_node[] get_type_intersections(type_node left, type_node right)
        {
            type_intersection_node tin1 = left.get_type_intersection(right);
            type_intersection_node tin2 = right.get_type_intersection(left);

            if (tin1 == null)
            {
                if (tin2 == null)
                {
                    return new type_intersection_node[0];
                }
                else
                {
                    type_intersection_node[] tinarr = new type_intersection_node[1];
                    type_intersection_node new_tin = new type_intersection_node();

                    new_tin.another_to_this = tin2.another_to_this;
                    new_tin.this_to_another = tin2.this_to_another;
                    if (tin2.type_compare == type_compare.greater_type)
                        new_tin.type_compare = type_compare.less_type;
                    else
                        new_tin.type_compare = type_compare.greater_type;
                    tinarr[0] = new_tin;
                    return tinarr;
                }
            }
            else
            {
                if (tin2 == null)
                {
                    type_intersection_node[] tinarr2 = new type_intersection_node[1];
                    tinarr2[0] = tin1;
                    return tinarr2;
                }
                else
                {
                    type_intersection_node[] tinarr3 = new type_intersection_node[2];
                    tinarr3[0] = tin1;
                    tinarr3[1] = tin2;
                    return tinarr3;
                }
            }
        }

        public static void add_type_conversion_from_defined(type_node from, type_node to,
            function_node convertion_method, type_compare comp, bool is_implicit)
        {
            add_type_conversion_from_defined(from, to, convertion_method, comp, is_implicit, false);
        }
        public static void add_generated_type_conversion_from_defined(type_node from, type_node to,
            function_node convertion_method, type_compare comp, bool is_implicit)
        {
            add_type_conversion_from_defined(from, to, convertion_method, comp, is_implicit, true);
        }
        public static void add_type_conversion_from_defined(type_node from, type_node to,
            function_node convertion_method, type_compare comp, bool is_implicit, bool is_generated)
        {
            type_intersection_node tin = from.get_type_intersection(to);
            if (tin == null)
            {
                tin = new type_intersection_node(comp);
                from.add_intersection_node(to, tin, is_generated);
            }
#if (DEBUG)
            else
            {
                if (tin.this_to_another != null)
                {
                    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Duplicate type conversion added");
                }
            }
#endif
            tin.this_to_another = new type_conversion(convertion_method, !is_implicit);
        }

        public static bool is_with_nil_allowed(type_node tn)
        {
            //(ssyy) переписал нормально, через switch

            switch (tn.type_special_kind)
            {
                case SemanticTree.type_special_kind.array_wrapper:
                case SemanticTree.type_special_kind.binary_file:
                case SemanticTree.type_special_kind.diap_type:
                case SemanticTree.type_special_kind.enum_kind:
                case SemanticTree.type_special_kind.set_type:
                case SemanticTree.type_special_kind.short_string:
                case SemanticTree.type_special_kind.text_file:
                case SemanticTree.type_special_kind.typed_file:
                    return false;
                default:
                    if (tn == SystemLibrary.SystemLibInitializer.AbstractBinaryFileNode)
                    {
                        return false;
                    }
                    if (tn.is_generic_parameter)
                    {
                        if (tn.is_class)
                            return true;
                        if (tn.base_type.is_value || tn.is_value_type)
                            return false;
                        return true;
                        //return (tn.is_class || tn.base_type != SystemLibrary.SystemLibrary.object_type && tn.base_type.is_class);
                    }
                    else if (tn is ref_type_node)
                    {
                        return true;
                    }
                    else if (tn.is_nullable_type)
                    {
                    	return true;
                    }
                    else
                    {
                        return !tn.is_value || tn is null_type_node;
                    }
            }
        }

        public static bool original_types_equals(type_node tn1, type_node tn2)
        {
            if (tn1 == tn2)
            {
                return true;
            }
            if (tn1.original_generic != null)
            {
                if (tn1.original_generic == tn2.original_generic || tn1.original_generic == tn2)
                {
                    return true;
                }
                return false;
            }
            if (tn2.original_generic != null)
            {
                if (tn2.original_generic == tn1)
                {
                    return true;
                }
            }
            return false;
        }

        //Добавляет интерфейс и всех его предков в список реализуемых данным классом.
        public static void AddInterface(common_type_node cnode, type_node _interface, location loc)
        {
            if (original_types_equals(_interface, cnode))
                throw new TreeConverter.UndefinedNameReference(_interface.name, loc);
            if (!_interface.IsInterface)
            {
                throw new TreeConverter.SimpleSemanticError(loc, "{0}_IS_NOT_INTERFACE", _interface.name);
            }
            if (_interface.ForwardDeclarationOnly)
            {
                throw new TreeConverter.SimpleSemanticError(loc, "FORWARD_DECLARATION_{0}_AS_IMPLEMENTING_INTERFACE", _interface.name);
            }

            if (cnode.ImplementingInterfaces.Contains(_interface))
                return;
            cnode.ImplementingInterfaces.Add(_interface);
            foreach (type_node tn in _interface.ImplementingInterfaces)
            {
                if (cnode.ImplementingInterfaces.Contains(tn))
                    continue;
                cnode.ImplementingInterfaces.Add(tn);
            }
        }

        public static bool is_derived(type_node base_class, type_node derived_class, bool ignore_generic_instances=false)
        {
            if (derived_class == null)  //void?
                return false;
            if (base_class == null)
                return false;
            type_node tn = derived_class.base_type;
            if (tn is compiled_generic_instance_type_node && ignore_generic_instances)
                tn = tn.original_generic;
            //TODO: Проверить на ссылочный и размерный тип.
            if (derived_class.semantic_node_type == semantic_node_type.null_type_node)
                if (is_with_nil_allowed(base_class) || base_class.IsPointer)
                    return true;
                else
                    return false;
            if (base_class.type_special_kind == SemanticTree.type_special_kind.short_string && derived_class.type_special_kind == SemanticTree.type_special_kind.short_string) return true;
            else if (derived_class == SystemLibrary.SystemLibrary.string_type && base_class.type_special_kind == SemanticTree.type_special_kind.short_string) return true;
            else if (base_class == SystemLibrary.SystemLibrary.string_type && derived_class.type_special_kind == SemanticTree.type_special_kind.short_string) return true;
            if (SystemLibrary.SystemLibInitializer.TypedSetType != null && SystemLibrary.SystemLibInitializer.TypedSetType.Found &&
                base_class.type_special_kind == SemanticTree.type_special_kind.set_type && derived_class == SystemLibrary.SystemLibInitializer.TypedSetType.sym_info as type_node
                )
                return true;
            if (base_class.type_special_kind == SemanticTree.type_special_kind.set_type && derived_class.type_special_kind == SemanticTree.type_special_kind.set_type)
            {
                if (base_class.element_type == derived_class.element_type) return true;
                type_compare tc = get_table_type_compare(base_class.element_type, derived_class.element_type);
                if (tc == type_compare.non_comparable_type)
                {
                    if (base_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
                    {
                        if (derived_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
                        {
                            if (base_class.element_type.base_type == derived_class.element_type.base_type) return true;
                            tc = get_table_type_compare(base_class.element_type.base_type, derived_class.element_type.base_type);
                            if (tc == type_compare.non_comparable_type) return false;
                            type_intersection_node tin = base_class.element_type.base_type.get_type_intersection(derived_class.element_type.base_type);
                            if (tin == null || tin.this_to_another == null)
                            {
                                if (base_class.element_type.base_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type.base_type != SystemLibrary.SystemLibrary.float_type)
                                {
                                    if ((derived_class.element_type.base_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type.base_type == SystemLibrary.SystemLibrary.float_type))
                                        return false;
                                    else return false;
                                }
                                else return true;
                            }
                            if (base_class.element_type.base_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type.base_type != SystemLibrary.SystemLibrary.float_type
                                 && (derived_class.element_type.base_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type.base_type == SystemLibrary.SystemLibrary.float_type))
                                return false;
                            return !tin.this_to_another.is_explicit;
                        }
                        else
                        {
                            if (base_class.element_type.base_type == derived_class.element_type) return true;
                            tc = get_table_type_compare(base_class.element_type.base_type, derived_class.element_type);
                            if (tc == type_compare.non_comparable_type) return false;
                            type_intersection_node tin = base_class.element_type.base_type.get_type_intersection(derived_class.element_type);
                            if (tin == null || tin.this_to_another == null)
                            {
                                if (base_class.element_type.base_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type.base_type != SystemLibrary.SystemLibrary.float_type)
                                {
                                    if ((derived_class.element_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type == SystemLibrary.SystemLibrary.float_type))
                                        return false;
                                    else return false;
                                }
                                else return true;
                            }
                            if (base_class.element_type.base_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type.base_type != SystemLibrary.SystemLibrary.float_type
                                 && (derived_class.element_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type == SystemLibrary.SystemLibrary.float_type))
                                return false;
                            return !tin.this_to_another.is_explicit;
                        }
                    }
                    else
                     if (derived_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
                    {
                        if (base_class.element_type == derived_class.element_type.base_type) return true;
                        tc = get_table_type_compare(base_class.element_type, derived_class.element_type.base_type);
                        if (tc == type_compare.non_comparable_type) return false;
                        type_intersection_node tin = base_class.element_type.get_type_intersection(derived_class.element_type.base_type);
                        if (tin == null || tin.this_to_another == null)
                        {
                            if (base_class.element_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type != SystemLibrary.SystemLibrary.float_type)
                            {
                                if ((derived_class.element_type.base_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type.base_type == SystemLibrary.SystemLibrary.float_type))
                                    return false;
                                else return false;
                            }
                            else return true;
                        }
                        if (base_class.element_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type != SystemLibrary.SystemLibrary.float_type
                                 && (derived_class.element_type.base_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type.base_type == SystemLibrary.SystemLibrary.float_type))
                            return false;
                        return !tin.this_to_another.is_explicit;
                    }
                    else if (base_class.element_type.type_special_kind == SemanticTree.type_special_kind.short_string && derived_class.element_type == SystemLibrary.SystemLibrary.string_type
                            || derived_class.element_type.type_special_kind == SemanticTree.type_special_kind.short_string && base_class.element_type == SystemLibrary.SystemLibrary.string_type)
                        return true;
                    else return false;

                }
                else
                {
                    if (tc == type_compare.less_type) // нет - тогда не работает set of integer -> set of 1..4
                    {
                        var bti = PascalABCCompiler.TreeConverter.convertion_data_and_alghoritms.is_value_int_type(base_class.element_type);
                        var dtr = PascalABCCompiler.TreeConverter.convertion_data_and_alghoritms.is_value_real_type(derived_class.element_type);
                        if (bti && dtr)
                            return false;
                    }

                    type_intersection_node tin = base_class.element_type.get_type_intersection(derived_class.element_type);
                    // вот тут ошибка. Сравниваются элементы. По идее, надо всегда выдавать false кроме случаев если оба - integer и оба - real
                    if (tin == null || tin.this_to_another == null)
                    {
                        //proverka na diapasony
                        if (base_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
                        {
                            if (derived_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
                            {
                                if (base_class.element_type.base_type == derived_class.element_type.base_type) return true;
                                tc = get_table_type_compare(base_class.element_type.base_type, derived_class.element_type.base_type);
                                if (tc == type_compare.non_comparable_type) return false;
                                type_intersection_node tin2 = base_class.element_type.base_type.get_type_intersection(derived_class.element_type.base_type);
                                if (tin == null || tin.this_to_another == null)
                                {
                                    if (base_class.element_type.base_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type.base_type != SystemLibrary.SystemLibrary.float_type)
                                    {
                                        if ((derived_class.element_type.base_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type.base_type == SystemLibrary.SystemLibrary.float_type))
                                            return false;
                                        else return false;
                                    }
                                    else return true;
                                }
                                if (base_class.element_type.base_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type.base_type != SystemLibrary.SystemLibrary.float_type
                                     && (derived_class.element_type.base_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type.base_type == SystemLibrary.SystemLibrary.float_type))
                                    return false;
                                return !tin.this_to_another.is_explicit;
                            }
                            else
                            {
                                if (base_class.element_type.base_type == derived_class.element_type) return true;
                                tc = get_table_type_compare(base_class.element_type.base_type, derived_class.element_type);
                                if (tc == type_compare.non_comparable_type) return false;
                                type_intersection_node tin2 = base_class.element_type.base_type.get_type_intersection(derived_class.element_type);
                                if (tin == null || tin.this_to_another == null)
                                {
                                    if (base_class.element_type.base_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type.base_type != SystemLibrary.SystemLibrary.float_type)
                                    {
                                        if ((derived_class.element_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type == SystemLibrary.SystemLibrary.float_type))
                                            return false;
                                        else return false;
                                    }
                                    else return true;
                                }
                                if (base_class.element_type.base_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type.base_type != SystemLibrary.SystemLibrary.float_type
                                     && (derived_class.element_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type == SystemLibrary.SystemLibrary.float_type))
                                    return false;
                                return !tin.this_to_another.is_explicit;
                            }
                        }
                        else
                        if (derived_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
                        {
                            if (base_class.element_type == derived_class.element_type.base_type) return true;
                            tc = get_table_type_compare(base_class.element_type, derived_class.element_type.base_type);
                            if (tc == type_compare.non_comparable_type) return false;
                            type_intersection_node tin2 = base_class.element_type.get_type_intersection(derived_class.element_type.base_type);
                            if (tin == null || tin.this_to_another == null)
                            {
                                if (base_class.element_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type != SystemLibrary.SystemLibrary.float_type)
                                {
                                    if ((derived_class.element_type.base_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type.base_type == SystemLibrary.SystemLibrary.float_type))
                                        return false;
                                    else return false;
                                }
                                else return true;
                            }
                            if (base_class.element_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type != SystemLibrary.SystemLibrary.float_type
                                     && (derived_class.element_type.base_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type.base_type == SystemLibrary.SystemLibrary.float_type))
                                return false;
                            return !tin.this_to_another.is_explicit;
                        }
                        if (base_class.element_type != SystemLibrary.SystemLibrary.double_type && base_class.element_type != SystemLibrary.SystemLibrary.float_type)
                        {
                            if ((derived_class.element_type == SystemLibrary.SystemLibrary.double_type || derived_class.element_type == SystemLibrary.SystemLibrary.float_type))
                                return false;
                            else return false;
                        }
                        else return true;
                    }
                    return !tin.this_to_another.is_explicit;
                }
            }
            //ssyy Рассматриваем случай интерфейса
            if (base_class.IsInterface)
            {
                bool implements = false;
                type_node tnode = derived_class;
                while (!implements && tnode != null && tnode.ImplementingInterfaces != null)
                {
                    //implements = tnode.ImplementingInterfaces.Contains(base_class);

                    implements = false;
                    // Цикл по всем реализуемым интерфейсам. А если tnode сам является интерфейсом?
                    var ImplementingInterfaces = tnode.ImplementingInterfaces.ToList();
                    // Из-за этой строки возникла ошибка https://github.com/pascalabcnet/pascalabcnet/issues/2872
                    // Но без этой строчки не работает преобразование sequence of Student к sequence of Person и sequence of object
                    // SSM 19/07/23 - снова раскомментировал - теперь не работало
                    // CheckOutputSeq(a); в Tasks где var a: array of char
                    //if (tnode.IsInterface) // SSM закомментировал опять 17.09.23 - single - неточные вычисления!
                    //    ImplementingInterfaces.Add(tnode);
                    foreach (var interf in ImplementingInterfaces)
                    {
                        var ctn = interf as compiled_type_node;
                        // if (ctn.generic_params == null || ctn.generic_params.Count == 0)
                        //    ctn = null;
                        var bcctn = base_class as compiled_type_node; // мне кажется, здесь надо проверять на наличие generic-параметров
                        //if (bcctn.generic_params == null || bcctn.generic_params.Count == 0)
                        //    bcctn = null;
                        var cgitn = interf as compiled_generic_instance_type_node;
                        var bcgitn = base_class as compiled_generic_instance_type_node;
                        // interf - IEnumerable<Student>, base_class - IEnumerable<Person>
                        if (interf == base_class)
                        {
                            implements = true;
                            break;
                        }
                        // Справка:
                        // IEnumerable<object> - compiled_type_node
                        // IEnumerable<Student>, List<Student> - compiled_generic_instance_type_node
                        // MyIEnumerable<Student>, MyIEnumerable<object> - common_generic_instance_type_node
                        // Здесь common быть не может поскольку ковариантность у нас - только для откомпилированных NET-типов
                        // Тут можно проверить на ковариантность
                        // еще где то надо проверять, что IEnumerable<Derived> -> IEnumerable<Base>, но здесь base_class предполагает, 
                        // что это - класс, и рассматривает все его интерфейсы, упуская ситуацию, когда base_class - это и есть интерфейс
                        //else if (cgitn != null && (bcgitn != null || bcctn != null))
                        //else if ((cgitn != null || ctn != null) && bcgitn != null)
                        //else if (cgitn != null && bcgitn != null)
                        //else if (cgitn != null && bcgitn != null || ctn != null && bcctn != null) // SSM 18/04 немного большие ограничения
                        else if ((cgitn != null || ctn != null) && (bcgitn != null || bcctn != null)) // SSM 19/04
                        {
                            compiled_type_node interf_original_generic = cgitn != null ? cgitn.original_generic as compiled_type_node : ctn.original_generic as compiled_type_node;
                            compiled_type_node base_original_generic = bcgitn != null ? bcgitn.original_generic as compiled_type_node : bcctn.original_generic as compiled_type_node;
                            List<type_node> interf_instance_params = cgitn != null ? cgitn.instance_params : ctn.instance_params;
                            List<type_node> base_instance_params = bcgitn != null ? bcgitn.instance_params : bcctn.instance_params;

                            //compiled_type_node interf_original_generic = cgitn.original_generic as compiled_type_node;
                            //compiled_type_node base_original_generic = bcgitn.original_generic as compiled_type_node;
                            //List<type_node> interf_instance_params = cgitn.instance_params;
                            //List<type_node> base_instance_params = bcgitn.instance_params;

                            if (interf_original_generic != null && interf_original_generic == base_original_generic)
                            {
                                // Нам нужно два original_generic as compiled_type_node и два instance_params

                                //if (ctcgi != null) // по идее это всегда так! потому что cgitn - compiled - поэтому закомментировал SSM 14/02/23
                                // теперь надо проверить параметры на ковариантность - все
                                var n = base_instance_params.Count;
                                var n1 = interf_instance_params.Count;

                                // перенес сюда. cgitn точно не null
                                var interf_compiled_type = interf_original_generic.compiled_type; // Пр: IEnumerable<T>
                                var impl = true;
                                if (n != n1)
                                    impl = false;
                                else
                                    for (int i = 0; i < n; i++)
                                    {
                                        // ctcgi.compiled_type - это System.Type
                                        if ((interf_compiled_type.GetGenericArguments()[i].GenericParameterAttributes & System.Reflection.GenericParameterAttributes.Covariant) != 0)
                                        {
                                            if (is_derived(base_instance_params[i], interf_instance_params[i], false)) // is_derived(object , T, false) = true, что неверно - T - не наследник object!!!
                                            {
                                                // OK
                                            }
                                            else // SSM 19/04/23 -вот эта ветка была решающей
                                            {
                                                impl = false;
                                                break;
                                            }
                                        }
                                        // Почему то когда это закомментировано, то работает. А так не вызывается ??!
                                        /*else if (base_instance_params[i] == interf_instance_params[i])
                                        {
                                            n = n;
                                            // Тоже OK
                                        }*/
                                        else
                                        {
                                            impl = false;
                                            break;
                                        }
                                    }
                                // то есть если все контравариантные или неважно какие, но равные то implements = true иначе false
                                implements = impl;
                                if (implements)
                                    break;
                            }
                        }
                        else if (interf is common_generic_instance_type_node cictn && base_class is common_generic_instance_type_node cbctn)
                        {
                            foreach (type_node impltn in cictn.ImplementingInterfaces)
                            {
                                if (is_type_or_original_generics_equal(impltn, cbctn))
                                {
                                    implements = true;
                                    break;
                                }
                            }
                        }
                        else if (interf is compiled_type_node ictn && base_class is compiled_type_node bctn)
                        {
                            if (ictn.compiled_type.AssemblyQualifiedName != null && ictn.compiled_type.AssemblyQualifiedName == bctn.compiled_type.AssemblyQualifiedName)
                            {
                                implements = true;
                                break;
                            }
                        }
                    }

                    tnode = tnode.base_type;
                }
                return implements;
            }
            //\ssyy

            if (base_class.type_special_kind == SemanticTree.type_special_kind.diap_type && derived_class.type_special_kind == SemanticTree.type_special_kind.diap_type)
            {
                if (base_class.base_type == derived_class.base_type) return true;
                type_compare tc = get_table_type_compare(base_class.base_type, derived_class.base_type);
                if (tc == type_compare.non_comparable_type) return false;
                type_intersection_node tin = base_class.base_type.get_type_intersection(derived_class.base_type);
                if (tin == null || tin.this_to_another == null)
                {
                    return false;
                }
                return !tin.this_to_another.is_explicit;
            }

            while (tn != null)
            {
                if (tn == base_class) // точное совпадение
                    break;
                if ((tn is compiled_type_node ctn1) && (base_class is compiled_type_node ctn2) // в случае если этот тип определен в одной dll, а используется в другой
                    && ctn1.compiled_type.AssemblyQualifiedName == ctn2.compiled_type.AssemblyQualifiedName)
                    break;
                tn = tn.base_type;
            }
            if (tn == null)
            {
                return false;
            }
            return true;
        }

        private static type_compare get_table_type_compare_in_specific_order(type_node left, type_node right, bool only_implicit = false)
        {
            type_intersection_node[] tins = get_type_intersections_in_specific_order(left, right);
            if (only_implicit)
            {
                var tinsv = tins.Where(t => (t.this_to_another != null && t.this_to_another.is_explicit == false) || (t.another_to_this != null && t.another_to_this.is_explicit == false)).ToArray();
                if (tins.Length==2 && tinsv.Length==1 && tins[1]==tinsv[0])
                {
                    tinsv[0] = change_to_opposite(tinsv[0]);
                }
                tins = tinsv;
            }
            if (tins.Length == 0)
            {
                return type_compare.non_comparable_type;
            }

            if (tins.Length == 1)
            {
                return tins[0].type_compare;
            }
            if (tins.Length == 2)
            {
                type_compare tc1 = tins[0].type_compare;
                type_compare tc2 = tins[1].type_compare;
                if ((tc1 == type_compare.non_comparable_type) && (tc2 == type_compare.non_comparable_type))
                {
                    return type_compare.non_comparable_type;
                }
                if ((tc1 == type_compare.greater_type) && (tc2 == type_compare.less_type))
                {
                    return type_compare.greater_type;
                }
                if ((tc1 == type_compare.less_type) && (tc2 == type_compare.greater_type))
                {
                    return type_compare.less_type;
                }
                throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Conflicting type comparsion");
            }
            return type_compare.non_comparable_type;
        }

        private static type_compare get_table_type_compare(type_node left, type_node right)
        {
            type_intersection_node[] tins = get_type_intersections(left, right);
            if (tins.Length == 0)
            {
                return type_compare.non_comparable_type;
            }
            if (tins.Length == 1 && tins[0].this_to_another != null && tins[0].this_to_another.is_explicit) // SSM bug fix #1441
            {
                return type_compare.non_comparable_type;
            }
            if (tins.Length == 1)
            {
                return tins[0].type_compare;
            }
            if (tins.Length == 2)
            {
                type_compare tc1 = tins[0].type_compare;
                type_compare tc2 = tins[1].type_compare;
                if ((tc1 == type_compare.non_comparable_type) && (tc2 == type_compare.non_comparable_type))
                {
                    return type_compare.non_comparable_type;
                }
                if ((tc1 == type_compare.greater_type) && (tc2 == type_compare.less_type))
                {
                    return type_compare.greater_type;
                }
                if ((tc1 == type_compare.less_type) && (tc2 == type_compare.greater_type))
                {
                    return type_compare.less_type;
                }
                throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Conflicting type comparsion");
            }
            return type_compare.non_comparable_type;
        }

        public static type_compare compare_types_in_specific_order(type_node left, type_node right, bool only_implicit = false)
        {
            type_compare ret = get_table_type_compare_in_specific_order(left, right, only_implicit);
            if (ret != type_compare.non_comparable_type)
            {
                return ret;
            }
            if (is_derived(left, right)) // left-base, right-derived, Person > Student
            {
                return type_compare.greater_type;
            }
            if (is_derived(right, left))
            {
                return type_compare.less_type;
            }
            return type_compare.non_comparable_type;
        }

        public static type_compare compare_types(type_node left, type_node right)
        {
            type_compare ret = get_table_type_compare(left, right);
            // SSM 18.09.23 исправление #2872 (частное!!! только первый параметр!)
            var ctnl = left as compiled_type_node;
            var ctnr = right as compiled_type_node;
            if (ctnl != null && ctnr != null && ctnl.original_generic == ctnr.original_generic
                && ctnl.compiled_type.IsInterface && ctnl.generic_params != null && ctnl.generic_params.Count == 1)
            {
                var interf_compiled_type = (ctnr.original_generic as compiled_type_node).compiled_type;
                if ((interf_compiled_type.GetGenericArguments()[0].GenericParameterAttributes & System.Reflection.GenericParameterAttributes.Covariant) != 0)
                    return compare_types(ctnl.generic_params[0], ctnr.generic_params[0]);
            }
            // IEnumerable<object> д.б. < IEnumerable<integer>
            // Проверить, что оба - IEnumerable. И если object < integer, то вернуть type_compare.less_type
            if (ret != type_compare.non_comparable_type)
            {
                return ret;
            }
            if (is_derived(left, right))
            {
                return type_compare.greater_type;
            }
            if (is_derived(right, left))
            {
                return type_compare.less_type;
            }
            return type_compare.non_comparable_type;
        }

        private static void add_conversion(possible_type_convertions ptc, function_node fn, type_node from, type_node to)
        {
            if (fn == null)
            {
                return;
            }
            if (ptc.first == null)
            {
                ptc.first = new type_conversion(fn);
            }
            else
            {
                ptc.second = new type_conversion(fn);
            }
            ptc.from = from;
            ptc.to = to;
        }

        public static expression_node convert_delegate_to_return_value_type(location call_location, params expression_node[] parameters)
        {
            expression_node par = parameters[0];
            internal_interface ii = par.type.get_internal_interface(internal_interface_kind.delegate_interface);
            delegate_internal_interface dii = (delegate_internal_interface)ii;
            common_method_node cmn = dii.invoke_method as common_method_node;
            if (cmn != null)
            {
                expression_node exp = new common_method_call(cmn, par, call_location);
                return exp;
            }
            compiled_function_node cfn = dii.invoke_method as compiled_function_node;
            if (cfn != null)
            {
                expression_node exp = new compiled_function_call(cfn, par, call_location);
                return exp;
            }
            return null;
        }

        public delegate expression_node function_call_maker(function_node fn, location loc, params expression_node[] exprs);
        public static function_call_maker type_table_function_call_maker;

        public class delegate_type_converter
        {
            private function_node convert_function;

            public delegate_type_converter(function_node convert_function)
            {
                this.convert_function = convert_function;
            }

            public expression_node convert_delegate_to_return_value_type_with_convertion(location call_location, params expression_node[] parameters)
            {
                expression_node del = parameters[0];
                expression_node par = convert_delegate_to_return_value_type(call_location, del);
                return type_table_function_call_maker(convert_function, call_location, par);
            }
        }

        public static possible_type_convertions get_convertions(type_node from, type_node to)
        {
            return get_convertions(from, to, true);
        }

        private class delegate_to_delegate_type_converter
        {
            private type_node _to;

            public delegate_to_delegate_type_converter(type_node to)
            {
                _to = to;
            }

            public expression_node convert_delegates_to_delegates(location call_location, expression_node[] parameters)
            {
                if (parameters.Length != 1)
                {
                    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Invalid delegates convertion");
                }
                delegate_internal_interface dii_to =
                    (delegate_internal_interface)_to.get_internal_interface(internal_interface_kind.delegate_interface);
                delegate_internal_interface dii =
                    (delegate_internal_interface)parameters[0].type.get_internal_interface(internal_interface_kind.delegate_interface);

                expression_node pr = parameters[0];

                base_function_call ifnotnull = null;
                if (_to.semantic_node_type == semantic_node_type.compiled_type_node)
                {
                    ifnotnull = new compiled_constructor_call((compiled_constructor_node)dii_to.constructor, call_location);
                }
                else
                {
                    ifnotnull = new common_constructor_call((common_method_node)dii_to.constructor, call_location);
                }
                //ccc = new common_constructor_call(dii_to.constructor, call_location);

                expression_node par = null;
                if (parameters[0].type.semantic_node_type == semantic_node_type.compiled_type_node)
                {
                    par = new compiled_function_call((compiled_function_node)dii.invoke_method, parameters[0], call_location);
                }
                else
                {
                    par = new common_method_call((common_method_node)dii.invoke_method, parameters[0], call_location);
                }
                ifnotnull.parameters.AddElement(par);

                null_const_node ncn = new null_const_node(_to, call_location);
                null_const_node ncn2 = new null_const_node(_to, call_location);

                PascalABCCompiler.TreeConverter.SymbolInfo si = pr.type.find_first_in_type(StringConstants.eq_name);

                basic_function_node fn = si.sym_info as basic_function_node;
                expression_node condition = null;
                if (fn != null)
                {
                    basic_function_call condition_bfc = new basic_function_call(fn, call_location);
                    condition_bfc.parameters.AddElement(pr);
                    condition_bfc.parameters.AddElement(ncn);
                    condition = condition_bfc;
                }
                else if (si.sym_info is compiled_function_node)
                {
                    compiled_static_method_call condition_cfc = new compiled_static_method_call(si.sym_info as compiled_function_node, call_location);
                    condition_cfc.parameters.AddElement(pr);
                    condition_cfc.parameters.AddElement(ncn);
                    condition = condition_cfc;
                }

                question_colon_expression qce = new question_colon_expression(condition, ncn2, ifnotnull, call_location);

                return qce;
            }
        }

        static bool not_contains_type(type_node right, type_node left) // вызывать только если left - generic parameter
        {
            System.Diagnostics.Debug.Assert(left.is_generic_parameter); // вроде рекурсивно не надо просматривать right т.к. это обрезается другой проверкой - один из типов должен быть t1<T>
            if (right.is_generic_type_instance)
                for (int i = 0; i < right.instance_params.Count; i++)
                {
                    if (right.instance_params[i] == left)
                        return false;
                }
            return true;
        }
        public static bool is_type_or_original_generics_equal(type_node left, type_node right)
        {
            if (left == right)
                return true;
            if (left.is_generic_type_instance && right.is_generic_type_instance)
            {
                if (left.original_generic != right.original_generic)
                    return false;
                if (left.instance_params.Count != right.instance_params.Count)
                    return false;
                for (int i = 0; i < left.instance_params.Count; i++)
                {
                    if (!is_type_or_original_generics_equal(left.instance_params[i], right.instance_params[i]))
                        return false;
                }
                return true;
            }
            if (left.is_generic_parameter) // SSM 29.05.19 надо проверять, что right не содержит T
                return not_contains_type(right, left); // нет, другие тесты падают
                //return true;
            if (left.type_special_kind == SemanticTree.type_special_kind.set_type && right == SystemLibrary.SystemLibInitializer.TypedSetType.sym_info
                || right.type_special_kind == SemanticTree.type_special_kind.set_type && left == SystemLibrary.SystemLibInitializer.TypedSetType.sym_info)
                return true;
            if (left is ref_type_node && right is ref_type_node)
                return is_type_or_original_generics_equal((left as ref_type_node).pointed_type, (right as ref_type_node).pointed_type);
            if (left.type_special_kind == SemanticTree.type_special_kind.array_kind && right.type_special_kind == SemanticTree.type_special_kind.array_kind)
                return is_type_or_original_generics_equal(left.element_type, right.element_type);
            return left == right;
        }

        //TODO: Возможно стоит если пересечение типов найдено в откомпилированных типах, добавлять к нашим структурам и не искать повторно.
        public static possible_type_convertions get_convertions(type_node from, type_node to, bool is_implicit)
        {
            possible_type_convertions ret = new possible_type_convertions();
            ret.first = null;
            ret.second = null;

            if ((from == null) || (to == null))
            {
                return ret;
            }

            type_intersection_node tin_from = from.get_type_intersection(to);
            type_intersection_node tin_to = to.get_type_intersection(from);

            if (tin_from != null)
            {
                if (tin_from.this_to_another != null)
                {
                    if ((!is_implicit) || (!(tin_from.this_to_another.is_explicit)))
                    {
                        add_conversion(ret, tin_from.this_to_another.convertion_method, from, to);
                    }
                }
            }
            if (tin_to != null)
            {
                if (tin_to.another_to_this != null)
                {
                    if ((!is_implicit) || (!(tin_to.another_to_this.is_explicit)))
                    {
                        add_conversion(ret, tin_to.another_to_this.convertion_method, from, to);
                    }
                }
            }
            if (ret.second != null)
            {
                return ret;
            }

            if (is_derived(to, from)
                || (from.IsInterface && to == SystemLibrary.SystemLibrary.object_type)
                || from.is_generic_type_instance && to == SystemLibrary.SystemLibrary.object_type
                )
            {
                add_conversion(ret, TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(from, to, true), from, to);
                //add_conversion(ret, SystemLibrary.SystemLibrary.empty_method, from, to);
            }

            if (ret.second != null)
            {
                return ret;
            }

            wrapped_type ctn_to = to as wrapped_type;
            wrapped_type ctn_from = from as wrapped_type;

            function_node fnode1 = null; // SSM вынес чтобы посмотреть значения 02.06.20
            function_node fnode1_1 = null;
            if (ctn_to != null)
            {
                fnode1 = ctn_to.get_implicit_conversion_from(from);
                add_conversion(ret, fnode1, from, to);
                if (ret.second != null)
                {
                    return ret;
                }
                fnode1_1 = null;
                if (!is_implicit)
                {
                    fnode1_1 = ctn_to.get_explicit_conversion_from(from);
                }
                add_conversion(ret, fnode1_1, from, to);
                if (ret.second != null)
                {
                    return ret;
                }
            }
            if (ctn_from != null)
            {
                function_node fnode2 = null;
                fnode2 = ctn_from.get_implicit_conversion_to(to);
                add_conversion(ret, fnode2, from, to);
                if (ret.second != null)
                {
                    var a = ret.first == ret.second;
                    return ret;
                }
                function_node fnode2_1 = null;
                if (!is_implicit)
                {
                    fnode2_1 = ctn_from.get_explicit_conversion_to(to);
                }
                add_conversion(ret, fnode2_1, from, to);
                if (ret.second != null)
                {
                    return ret;
                }
            }

            //TODO: Вот это должно быть в каком нибудь другом месте.
            internal_interface ii = from.get_internal_interface(internal_interface_kind.delegate_interface);
            if (ii != null)
            {
                delegate_internal_interface dii = (delegate_internal_interface)ii;

                internal_interface to_ii = to.get_internal_interface(internal_interface_kind.delegate_interface);
                if (to_ii != null)
                {
                    delegate_internal_interface to_dii = (delegate_internal_interface)to_ii;
                    if (dii.parameters.Count == to_dii.parameters.Count)
                    {
                        //ms100 error fixed (DS)
                        bool eq = TreeConverter.convertion_data_and_alghoritms.function_eq_params_and_result(dii.invoke_method, to_dii.invoke_method);
                        if (eq)
                        {
                            delegate_to_delegate_type_converter dtdtc = new delegate_to_delegate_type_converter(to);
                            add_conversion(ret, new convert_types_function_node(dtdtc.convert_delegates_to_delegates, false), from, to);
                        }
                    }
                }
                else if (to is delegated_methods && (to as delegated_methods).proper_methods.Count > 0)
                {
                    var proper_meth = (to as delegated_methods).proper_methods[0].simple_function_node;
                    if (dii.parameters.Count == proper_meth.parameters.Count)
                    {
                        //ms100 error fixed (DS)
                        bool eq = TreeConverter.convertion_data_and_alghoritms.function_eq_params_and_result(dii.invoke_method, proper_meth);
                        if (eq)
                        {
                            delegate_to_delegate_type_converter dtdtc = new delegate_to_delegate_type_converter(to);
                            add_conversion(ret, new convert_types_function_node(dtdtc.convert_delegates_to_delegates, false), from, to);
                        }
                    }
                }

                if (dii.parameters.Count == 0)
                {
                    if (dii.return_value_type == to)
                    {
                        if (ret.first != null) // SSM 09.08.18
                            ret.first = null;
                        add_conversion(ret, new convert_types_function_node(convert_delegate_to_return_value_type, true), from, to);
                    }
                    else
                    {
                        possible_type_convertions ptcc = get_convertions(dii.return_value_type, to);
                        if ((ptcc.first != null) && (ptcc.first.convertion_method != null))
                        {
                            delegate_type_converter dtc = new delegate_type_converter(ptcc.first.convertion_method);
                            if (ret.first != null) // SSM 09.08.18
                                ret.first = null;
                            add_conversion(ret, new convert_types_function_node(dtc.convert_delegate_to_return_value_type_with_convertion, false), from, to);
                        }
                        if ((ptcc.second != null) && (ptcc.second.convertion_method != null))
                        {
                            delegate_type_converter dtc = new delegate_type_converter(ptcc.second.convertion_method);
                            if (ret.first != null) // SSM 09.08.18
                                ret.first = null;
                            add_conversion(ret, new convert_types_function_node(dtc.convert_delegate_to_return_value_type_with_convertion, false), from, to);
                        }
                    }
                }
            }

            if (from.is_generic_type_instance && to.is_generic_type_instance && from.original_generic == to.original_generic && from.name == to.name
                && from.original_generic is common_type_node && to.original_generic is common_type_node && (from.original_generic as common_type_node).comprehensive_namespace == (to.original_generic as common_type_node).comprehensive_namespace
                && (to.original_generic as common_type_node).IsPartial && (from.original_generic as common_type_node).IsPartial)
                add_conversion(ret, TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(from, to, true), from, to);
            return ret;
        }
    }

}