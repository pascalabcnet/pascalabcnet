// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;

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
			_conversion_method=conversion;
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

	public enum type_compare {less_type,greater_type,non_comparable_type};

    /// <summary>
    /// Узел пересечения типов. Хранит информацию, общую для пары типов. Например, операции приведения от одного типа к другому.
    /// </summary>
    [Serializable]
	public class type_intersection_node
	{
		private type_compare _type_compare=type_compare.non_comparable_type;

		private type_conversion _this_to_another;
		private type_conversion _another_to_this;

		public type_intersection_node()
		{
		}

		public type_intersection_node(type_compare type_compare)
		{
			_type_compare=type_compare;
		}

		public type_conversion this_to_another
		{
			get
			{
				return _this_to_another;
			}
			set
			{
				_this_to_another=value;
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
				_another_to_this=value;
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
		public type_conversion first=null;
		public type_conversion second=null;
		public type_node from;
		public type_node to;
	}

    /// <summary>
    /// Таблица типов.
    /// </summary>
	public static class type_table
	{
		private static type_intersection_node[] get_type_intersections_in_specific_order(type_node left,type_node right)
		{
			type_intersection_node tin1=left.get_type_intersection(right);
			type_intersection_node tin2=right.get_type_intersection(left);

			if (tin1==null)
			{
				if (tin2==null)
				{
					return new type_intersection_node[0];
				}
				else
				{
					if (tin2.another_to_this != null && tin2.another_to_this.is_explicit)
						return new type_intersection_node[0];
					type_intersection_node[] tinarr=new type_intersection_node[1];
					type_intersection_node new_tin = null;
					if (tin2.type_compare == type_compare.greater_type)
						new_tin = new type_intersection_node(type_compare.less_type);
					else if (tin2.type_compare == type_compare.less_type)
						new_tin = new type_intersection_node(type_compare.greater_type);
					else
						new_tin = new type_intersection_node(type_compare.non_comparable_type);
					new_tin.another_to_this = tin2.another_to_this;
					new_tin.this_to_another = tin2.this_to_another;
					tinarr[0]=new_tin;
					return tinarr;
				}
			}
			else
			{
				if (tin2==null)
				{
					if (tin1.this_to_another != null && tin1.this_to_another.is_explicit)
						return new type_intersection_node[0];
					type_intersection_node[] tinarr2=new type_intersection_node[1];
					/*type_intersection_node new_tin = null;
					if (tin1.type_compare == type_compare.greater_type)
						new_tin = new type_intersection_node(type_compare.less_type);
					else if (tin1.type_compare == type_compare.less_type)
						new_tin = new type_intersection_node(type_compare.greater_type);
					else
						new_tin = new type_intersection_node(type_compare.non_comparable_type);
					new_tin.another_to_this = tin1.another_to_this;
					new_tin.this_to_another = tin1.this_to_another;
					tinarr2[0]=new_tin;*/
					tinarr2[0]=tin1;
					return tinarr2;
				}
				else
				{
					type_intersection_node[] tinarr3=new type_intersection_node[2];
					tinarr3[0]=tin1;
					tinarr3[1]=tin2;
					return tinarr3;
				}
			}
		}
		
		private static type_intersection_node[] get_type_intersections(type_node left,type_node right)
		{
			type_intersection_node tin1=left.get_type_intersection(right);
			type_intersection_node tin2=right.get_type_intersection(left);

			if (tin1==null)
			{
				if (tin2==null)
				{
					return new type_intersection_node[0];
				}
				else
				{
					type_intersection_node[] tinarr=new type_intersection_node[1];
                    type_intersection_node new_tin = new type_intersection_node();

                    new_tin.another_to_this = tin2.another_to_this;
                    new_tin.this_to_another = tin2.this_to_another;
                    if (tin2.type_compare == type_compare.greater_type)
                        new_tin.type_compare = type_compare.less_type;
                    else
                        new_tin.type_compare = type_compare.greater_type;
					tinarr[0]= new_tin;
					return tinarr;
				}
			}
			else
			{
				if (tin2==null)
				{
					type_intersection_node[] tinarr2=new type_intersection_node[1];
					tinarr2[0]=tin1;
					return tinarr2;
				}
				else
				{
					type_intersection_node[] tinarr3=new type_intersection_node[2];
					tinarr3[0]=tin1;
					tinarr3[1]=tin2;
					return tinarr3;
				}
			}
		}

        //(ssyy) Это не используется - я закомментировал.
		/*private static void add_type_intersection(type_node left,type_node right,type_intersection_node intersection)
		{
			left.add_intersection_node(right,intersection);
		}*/

        //(ssyy) Это не используется - я закомментировал.
        /*public static void add_type_conversion_from_defined(type_node from, type_node to,
            function_node convertion_method, type_compare comp)
        {
            add_type_conversion_from_defined(from, to, convertion_method, comp, true);
        }*/

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
			function_node convertion_method,type_compare comp,bool is_implicit,bool is_generated)
		{
			type_intersection_node tin=from.get_type_intersection(to);
			if (tin==null)
			{
				tin=new type_intersection_node(comp);
				from.add_intersection_node(to,tin,is_generated);
			}
#if (DEBUG)
			else
			{
				if (tin.this_to_another!=null)
				{
					throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Duplicate type conversion added");
				}
			}
#endif
			tin.this_to_another=new type_conversion(convertion_method,!is_implicit);		
		}

        //(ssyy) Это не используется - я закомментировал.
        /*public static void add_type_conversion_to_defined(type_node from, type_node to,
            function_node convertion_method, type_compare comp)
        {
            add_type_conversion_to_defined(from, to, convertion_method, comp, true);
        }*/

        //(ssyy) Это не используется - я закомментировал.
        /*public static void add_type_conversion_to_defined(type_node from,type_node to,
			function_node convertion_method,type_compare comp,bool is_implicit)
		{
			type_intersection_node tin=to.get_type_intersection(from);
			if (tin==null)
			{
				tin=new type_intersection_node(comp);
				to.add_intersection_node(from,tin);
			}
#if (DEBUG)
			else
			{
				if (tin.this_to_another!=null)
				{
                    throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Duplicate type conversion added");
				}
			}
#endif
			tin.this_to_another=new type_conversion(convertion_method,!is_implicit);		
		}*/

        //(ssyy) Это не используется - я закомментировал.
        /*public static void add_type_conversion_from_defined(type_node from,type_node to,function_node convertion_method)
		{
			add_type_conversion_from_defined(from,to,convertion_method,type_compare.non_comparable_type);
		}*/

        //(ssyy) Это не используется - я закомментировал.
		/*public static void add_type_conversion_to_defined(type_node from,type_node to,function_node convertion_method)
		{
			add_type_conversion_to_defined(from,to,convertion_method,type_compare.non_comparable_type);
		}*/
		
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
                        return (tn.is_class || tn.base_type != SystemLibrary.SystemLibrary.object_type && tn.base_type.is_class);
                    }
                    else if (tn is ref_type_node)
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
            //if (_interface == cnode)
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
            if (!cnode.ImplementingInterfaces.Contains(_interface))
            {
                cnode.ImplementingInterfaces.Add(_interface);
                foreach (type_node tn in _interface.ImplementingInterfaces)
                {
                    if (!cnode.ImplementingInterfaces.Contains(tn))
                    {
                        cnode.ImplementingInterfaces.Add(tn);
                    }
                }
            }
        }
		
		public static bool is_derived(type_node base_class,type_node derived_class)
		{
            if (derived_class == null)  //void?
                return false;
            if (base_class == null)
                return false;
			type_node tn=derived_class.base_type;
            //TODO: Проверить на ссылочный и размерный тип.
			if (derived_class.semantic_node_type == semantic_node_type.null_type_node) 
			if (is_with_nil_allowed(base_class) || base_class.IsPointer)
				return true;
			else
				return false;
			/*if (base_class.semantic_node_type == semantic_node_type.null_type_node) 
				if (!derived_class.is_value || derived_class.IsPointer)
				return true;
			else if (!is_with_nil_allowed(derived_class)) return false;
			else return true;*/
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
				type_compare tc = get_table_type_compare(base_class.element_type,derived_class.element_type);
				if (tc == type_compare.non_comparable_type)
				{
					if (base_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
					{
						if (derived_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
						{
							if (base_class.element_type.base_type == derived_class.element_type.base_type) return true;
							tc = get_table_type_compare(base_class.element_type.base_type,derived_class.element_type.base_type);
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
							tc = get_table_type_compare(base_class.element_type.base_type,derived_class.element_type);
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
						tc = get_table_type_compare(base_class.element_type,derived_class.element_type.base_type);
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
					type_intersection_node tin = base_class.element_type.get_type_intersection(derived_class.element_type);
					if (tin == null || tin.this_to_another == null) 
					{
						//proverka na diapasony
						if (base_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
						{
						if (derived_class.element_type.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
						{
							if (base_class.element_type.base_type == derived_class.element_type.base_type) return true;
							tc = get_table_type_compare(base_class.element_type.base_type,derived_class.element_type.base_type);
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
							tc = get_table_type_compare(base_class.element_type.base_type,derived_class.element_type);
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
						tc = get_table_type_compare(base_class.element_type,derived_class.element_type.base_type);
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
            // if (base_class is common_type_node && (base_class as common_type_node).IsEnum && derived_class == SystemLibrary.SystemLibrary.integer_type) 
           //     return true;
            //ssyy Рассматриваем случай интерфейса
            if (base_class.IsInterface)
            {
                bool implements = false;
                type_node tnode = derived_class;
                while (!implements && tnode != null && tnode.ImplementingInterfaces!=null)
                {
                    implements = tnode.ImplementingInterfaces.Contains(base_class);
                    tnode = tnode.base_type;
                }
                return implements;
                //return derived_class.ImplementingInterfaces.Contains(base_class);
            }
            //\ssyy
            /*if (base_class.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
            {
            	if (derived_class.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
            		return is_derived(base_class.base_type,derived_class.base_type);
            	else return is_derived(base_class.base_type,derived_class);
            }
            else if (derived_class.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
            {
            	//if (derived_class.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
            		return is_derived(base_class,derived_class.base_type);
            	//else return is_derived(base_class.base_type,derived_class);
            }*/
            
            if (base_class.type_special_kind == SemanticTree.type_special_kind.diap_type && derived_class.type_special_kind == SemanticTree.type_special_kind.diap_type)
			{
				if (base_class.base_type == derived_class.base_type) return true;
				type_compare tc = get_table_type_compare(base_class.base_type,derived_class.base_type);
				if (tc == type_compare.non_comparable_type) return false;
				type_intersection_node tin = base_class.base_type.get_type_intersection(derived_class.base_type);
				if (tin == null || tin.this_to_another == null)
				{
					return false;
				}
				return !tin.this_to_another.is_explicit;
            }
            else if (base_class.type_special_kind != SemanticTree.type_special_kind.diap_type && derived_class.type_special_kind == SemanticTree.type_special_kind.diap_type)
            {
            	/*if (base_class == derived_class.base_type) return true;
				type_compare tc = get_table_type_compare(base_class,derived_class.base_type);
				if (tc == type_compare.non_comparable_type) return false;
				type_intersection_node tin = base_class.get_type_intersection(derived_class.base_type);
				if (tin == null || tin.this_to_another == null)
				{
					if ((base_class == SystemLibrary.SystemLibrary.real_type || base_class == SystemLibrary.SystemLibrary.float_type) && derived_class.base_type != SystemLibrary.SystemLibrary.char_type)
					{
						return true;
					}
					else if (base_class == SystemLibrary.SystemLibrary.string_type && derived_class.base_type == SystemLibrary.SystemLibrary.char_type)
					{
						return true;
					}
					//else if (base_class == SystemLibrary.SystemLibrary.char_type) return 
					return false;
				}
				return true;*///!tin.this_to_another.is_explicit;
            }
            else if (base_class.type_special_kind == SemanticTree.type_special_kind.diap_type && derived_class.type_special_kind != SemanticTree.type_special_kind.diap_type)
            {
            	/*if (base_class.base_type == derived_class) return true;
				type_compare tc = get_table_type_compare(base_class.base_type,derived_class);
				if (tc == type_compare.non_comparable_type) return false;
				type_intersection_node tin = base_class.base_type.get_type_intersection(derived_class);
				if (tin == null || tin.this_to_another == null)
				{
					return false;
				}
				return true;*/// !tin.this_to_another.is_explicit;
            }
				
//            if (base_class.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
//            {
////            	if (derived_class.type_special_kind != PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
////            	{
////            		if (base_class.base_type == derived_class) return true;
////            		return get_table_type_compare(base_class.base_type,derived_class) == type_compare.greater_type || get_table_type_compare(base_class.base_type,derived_class) == type_compare.less_type ;
////            	}
////            	else
////            	{
////            		if (base_class.base_type == derived_class.base_type) return true;
////            		return get_table_type_compare(base_class.base_type,derived_class.base_type) == type_compare.greater_type || get_table_type_compare(base_class.base_type,derived_class.base_type) == type_compare.less_type;
////            	}
//            	if (derived_class.type_special_kind != PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
//            	{
//            		if (base_class.base_type == derived_class) return true;
//            		if (base_class.base_type != SystemLibrary.SystemLibrary.bool_type && base_class.base_type != SystemLibrary.SystemLibrary.char_type)
//            		{
//            			if (derived_class == SystemLibrary.SystemLibrary.integer_type || derived_class == SystemLibrary.SystemLibrary.byte_type //|| derived_class == SystemLibrary.SystemLibrary.sbyte_type
//            			   || derived_class == SystemLibrary.SystemLibrary.short_type || derived_class == SystemLibrary.SystemLibrary.ushort_type || derived_class == SystemLibrary.SystemLibrary.uint_type
//            			  || derived_class == SystemLibrary.SystemLibrary.long_type || derived_class == SystemLibrary.SystemLibrary.ulong_type)
//            				return true;
//            		}
//            		else
//            		return get_table_type_compare(base_class,derived_class.base_type) == type_compare.less_type || get_table_type_compare(base_class,derived_class.base_type) == type_compare.greater_type;
//            	}
//            	else
//            	{
//            		if (base_class.base_type == derived_class.base_type) return true;
//            		return is_derived(base_class.base_type,derived_class);
//            	}
//            }
//            else if (derived_class.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
//            {
//            	if (base_class.type_special_kind != PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
//            	{
//            		if (base_class == derived_class.base_type) return true;
//            		if (base_class == SystemLibrary.SystemLibrary.bool_type || base_class == SystemLibrary.SystemLibrary.sbyte_type) return false;
//            		else if (derived_class.base_type != SystemLibrary.SystemLibrary.bool_type && derived_class.base_type != SystemLibrary.SystemLibrary.char_type)
//            		{
//            			if (base_class == SystemLibrary.SystemLibrary.integer_type || base_class == SystemLibrary.SystemLibrary.byte_type //|| base_class == SystemLibrary.SystemLibrary.sbyte_type
//            			   || base_class == SystemLibrary.SystemLibrary.short_type || base_class == SystemLibrary.SystemLibrary.ushort_type || base_class == SystemLibrary.SystemLibrary.uint_type
//            			  || base_class == SystemLibrary.SystemLibrary.long_type || base_class == SystemLibrary.SystemLibrary.ulong_type)
//            				return true;
//            		}
//            		else
//            		return get_table_type_compare(base_class,derived_class.base_type) == type_compare.less_type || get_table_type_compare(base_class,derived_class.base_type) == type_compare.greater_type;
//            	}
//            	else
//            	{
//            		if (base_class.base_type == derived_class.base_type) return true;
////            		if (base_class.base_type == SystemLibrary.SystemLibrary.bool_type || base_class.base_type == SystemLibrary.SystemLibrary.sbyte_type) return false;
////            		else if (base_class.base_type != SystemLibrary.SystemLibrary.bool_type && base_class.base_type != SystemLibrary.SystemLibrary.char_type)
////            		{
////            			if (base_class == SystemLibrary.SystemLibrary.integer_type || base_class == SystemLibrary.SystemLibrary.byte_type || base_class == SystemLibrary.SystemLibrary.sbyte_type
////            			   || base_class == SystemLibrary.SystemLibrary.short_type || base_class == SystemLibrary.SystemLibrary.ushort_type || base_class == SystemLibrary.SystemLibrary.uint_type
////            			  || base_class == SystemLibrary.SystemLibrary.long_type || base_class == SystemLibrary.SystemLibrary.ulong_type)
////            				return true;
////            		}
////            		else
//            		return is_derived(base_class.base_type,derived_class);
//            		//return get_table_type_compare(base_class.base_type,derived_class.base_type) == type_compare.less_type || get_table_type_compare(base_class.base_type,derived_class.base_type) == type_compare.greater_type;
//            	}
//            }
			while((tn!=null)&&(tn!=base_class))
			{
				tn=tn.base_type;
			}
			if (tn==null)
			{
				return false;
			}
			return true;
		}
		
		private static type_compare get_table_type_compare_in_specific_order(type_node left, type_node right)
		{
			type_intersection_node[] tins=get_type_intersections_in_specific_order(left,right);
			if (tins.Length==0)
			{
				return type_compare.non_comparable_type;
			}
			if (tins.Length==1)
			{
				return tins[0].type_compare;
			}
			if (tins.Length==2)
			{
				type_compare tc1=tins[0].type_compare;
				type_compare tc2=tins[1].type_compare;
				if ((tc1==type_compare.non_comparable_type)&&(tc2==type_compare.non_comparable_type))
				{
					return type_compare.non_comparable_type;
				}
				if ((tc1==type_compare.greater_type)&&(tc2==type_compare.less_type))
				{
					return type_compare.greater_type;
				}
				if ((tc1==type_compare.less_type)&&(tc2==type_compare.greater_type))
				{
					return type_compare.less_type;
				}
                throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Conflicting type comparsion");
			}
			return type_compare.non_comparable_type;
		}
		
		private static type_compare get_table_type_compare(type_node left,type_node right)
		{
			type_intersection_node[] tins=get_type_intersections(left,right);
			if (tins.Length==0)
			{
				return type_compare.non_comparable_type;
			}
			if (tins.Length==1)
			{
				return tins[0].type_compare;
			}
			if (tins.Length==2)
			{
				type_compare tc1=tins[0].type_compare;
				type_compare tc2=tins[1].type_compare;
				if ((tc1==type_compare.non_comparable_type)&&(tc2==type_compare.non_comparable_type))
				{
					return type_compare.non_comparable_type;
				}
				if ((tc1==type_compare.greater_type)&&(tc2==type_compare.less_type))
				{
					return type_compare.greater_type;
				}
				if ((tc1==type_compare.less_type)&&(tc2==type_compare.greater_type))
				{
					return type_compare.less_type;
				}
                throw new PascalABCCompiler.TreeConverter.CompilerInternalError("Conflicting type comparsion");
			}
			return type_compare.non_comparable_type;
		}
		
		public static type_compare compare_types_in_specific_order(type_node left, type_node right)
		{
			type_compare ret=get_table_type_compare_in_specific_order(left,right);
			if (ret!=type_compare.non_comparable_type)
			{
				return ret;
			}
			if (is_derived(left,right))
			{
				return type_compare.greater_type;
			}
			if (is_derived(right,left))
			{
				return type_compare.less_type;
			}
			/*if (left.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type || right.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
			{
				return type_compare.less_type;
			}*/
			return type_compare.non_comparable_type;
		}
		
		public static type_compare compare_types(type_node left,type_node right)
		{
			type_compare ret=get_table_type_compare(left,right);
			if (ret!=type_compare.non_comparable_type)
			{
				return ret;
			}
			if (is_derived(left,right))
			{
				return type_compare.greater_type;
			}
			if (is_derived(right,left))
			{
				return type_compare.less_type;
			}
			/*if (left.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type || right.type_special_kind == PascalABCCompiler.SemanticTree.type_special_kind.diap_type)
			{
				return type_compare.less_type;
			}*/
			return type_compare.non_comparable_type;
		}

        /*
        public static void add_explicit_type_conversion(type_node from, type_node to, function_node conversion_method)
        {
            //В этом методе преобразование типа добавляется только в один из них.
            //Возможно следует сделать пару методов, для добавления преобразования к типу из которого мы преобразуем 
            //и для добавления преобразования типа к которому мы преобразуем.
            to.add_explicit_type_conversion(from, conversion_method);
        }

        public static function_node get_explicit_type_conversion(type_node from,type_node to)
        {
            //Возможно этот метод должен возвращать possible_type_conversions.
            return to.get_explicit_type_conversion(from);
        }
        */

        private static void add_conversion(possible_type_convertions ptc, function_node fn,type_node from,type_node to)
        {
            if (fn==null)
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

        /*
        if (ctn_to != null)
                    {
                        function_node ccfn2 = ctn_to.get_implicit_conversion_from(from);
                        if (ccfn2 != null)
                        {
                            if (ret.first == null)
                            {
                                ret.first = new type_conversion(ccfn2);
                            }
                            else
                            {
                                ret.second = new type_conversion(ccfn2);
                            }
                        }
                        else
                        {
                            ccfn2 = ctn_from.get_implicit_conversion_to(to);
                            if (ccfn2 != null)
                            {
                                if (ret.first == null)
                                {
                                    ret.first = new type_conversion(ccfn2);
                                }
                                else
                                {
                                    ret.second = new type_conversion(ccfn2);
                                }
                            }
                        }
                    }
                    else
                    {
                        function_node ccfn2 = ctn_from.get_implicit_conversion_to(to);
                        if (ccfn2 != null)
                        {
                            if (ret.first == null)
                            {
                                ret.first = new type_conversion(ccfn2);
                            }
                            else
                            {
                                ret.second = new type_conversion(ccfn2);
                            }
                        }
                        else
                        {
                            ccfn2 = ctn_to.get_implicit_conversion_from(from);
                            if (ret.first == null)
                            {
                                ret.first = new type_conversion(ccfn2);
                            }
                            else
                            {
                                ret.second = new type_conversion(ccfn2);
                            }
                        }
                    } 
        */

        public static expression_node convert_delegate_to_return_value_type(location call_location, params expression_node[] parameters)
        {
            expression_node par=parameters[0];
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
                expression_node del=parameters[0];
                expression_node par=convert_delegate_to_return_value_type(call_location,del);
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
                delegate_internal_interface dii_to=
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

                PascalABCCompiler.TreeConverter.SymbolInfo si = pr.type.find_in_type(PascalABCCompiler.TreeConverter.compiler_string_consts.eq_name);

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

        //TODO: Возможно стоит если пересечение типов найдено в откомпилированных типах, добавлять к нашим структурам и не искать повторно.
		public static possible_type_convertions get_convertions(type_node from,type_node to, bool is_implicit)
		{
            possible_type_convertions ret = new possible_type_convertions();
            ret.first = null;
            ret.second = null;

            if ((from == null) || (to == null))
            {
                return ret;
            }

			type_intersection_node tin_from=from.get_type_intersection(to);
			type_intersection_node tin_to=to.get_type_intersection(from);

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

            if (is_derived(to, from) || (from.IsInterface && to == SystemLibrary.SystemLibrary.object_type))
            {
                add_conversion(ret, TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(from, to, true), from, to);
                //add_conversion(ret, SystemLibrary.SystemLibrary.empty_method, from, to);
            }

            if (ret.second != null)
            {
                return ret;
            }

			wrapped_type ctn_to=to as wrapped_type;
			wrapped_type ctn_from=from as wrapped_type;

            if (ctn_to != null)
            {
                function_node fnode1 = null;
                fnode1 = ctn_to.get_implicit_conversion_from(from);
                add_conversion(ret, fnode1, from, to);
                if (ret.second != null)
                {
                    return ret;
                }
                fnode1 = null;
                if (!is_implicit)
                {
                    fnode1 = ctn_to.get_explicit_conversion_from(from);
                }
                add_conversion(ret, fnode1, from, to);
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
                    return ret;
                }
                fnode2 = null;
                if (!is_implicit)
                {
                    fnode2 = ctn_from.get_explicit_conversion_to(to);
                }
                add_conversion(ret, fnode2, from, to);
                if (ret.second != null)
                {
                    return ret;
                }
            }

            //TODO: Вот это должно быть в каком нибудь другом месте.
            internal_interface ii = from.get_internal_interface(internal_interface_kind.delegate_interface);
//            if (ii == null)
//            {
//            	delegate_methods dels = ctn_from as delegated_methods;
//            	if (dels != null && (ctn_to is compiled_type_node) && (ctn_to as compiled_type_node).compiled_type == NetHelper.NetHelper.DelegateType)
//            	{
//            		PascalABCCompiler.TreeConverter.type_constructor.instance.create_delegate();
//            	}
//            }
//            else
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
                        /*bool eq = true;
                        int i = 0;
                        while ((eq)&&(i<dii.parameters.Count))
                        {
                            eq=(dii.parameters[i].type==to_dii.parameters[i].type);
                            i++;
                        }*/
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
                        add_conversion(ret, new convert_types_function_node(convert_delegate_to_return_value_type, true), from, to);
                    }
                    else
                    {
                        possible_type_convertions ptcc = get_convertions(dii.return_value_type, to);
                        if ((ptcc.first != null) && (ptcc.first.convertion_method != null))
                        {
                            delegate_type_converter dtc = new delegate_type_converter(ptcc.first.convertion_method);
                            add_conversion(ret, new convert_types_function_node(dtc.convert_delegate_to_return_value_type_with_convertion, false), from, to);
                        }
                        if ((ptcc.second != null) && (ptcc.second.convertion_method != null))
                        {
                            delegate_type_converter dtc = new delegate_type_converter(ptcc.second.convertion_method);
                            add_conversion(ret, new convert_types_function_node(dtc.convert_delegate_to_return_value_type_with_convertion, false), from, to);
                        }
                    }
                }
            }

			return ret;
		}
	}

}
