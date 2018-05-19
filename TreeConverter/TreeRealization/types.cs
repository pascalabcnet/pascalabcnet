// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PascalABCCompiler.TreeConverter;
using System.Linq;

namespace PascalABCCompiler.TreeRealization
{

    /// <summary>
    /// Базовый класс, для представления типов.
    /// </summary>
	[Serializable]
	public abstract class type_node : definition_node, SemanticTree.ITypeNode
	{
        public override string ToString()
        {
            if (type_special_kind == SemanticTree.type_special_kind.record)
                return "record " + PrintableName;
            if (IsInterface)
                return "interface " + PrintableName;
            if (is_class)
                return "class " + PrintableName;
            
            return PrintableName;
        }
        private System.Collections.Generic.Dictionary<type_node, type_intersection_node> type_intersections;
            //new System.Collections.Generic.Dictionary<type_node, type_intersection_node>();
        private List<type_node> generated_type_intersections = null;//new List<type_node>();
        private System.Collections.Generic.Dictionary<internal_interface_kind, internal_interface> internal_interfaces;
        //new System.Collections.Generic.Dictionary<internal_interface_kind, internal_interface>();

        //private System.Collections.Generic.Dictionary<type_node, type_conversion> explicit_type_conversions =
        //    new System.Collections.Generic.Dictionary<type_node, type_conversion>();
        /*
        public void add_explicit_type_conversion(type_node from_type,function_node conversion_method)
        {
#if DEBUG
            type_conversion tc = null;
            bool b = explicit_type_conversions.TryGetValue(from_type, out tc);
            if (b == true)
            {
                throw new CompilerInternalError("Duplicate explicit type conversion");
            }
#endif
            type_conversion atc = new type_conversion(conversion_method, true);
            //atc.from = from_type;
            //atc.to = this;
            explicit_type_conversions.Add(from_type, atc);
        }

        public function_node get_explicit_type_conversion(type_node from_type)
        {
            type_conversion tc = null;
            bool b = explicit_type_conversions.TryGetValue(from_type, out tc);
            if (b == false)
            {
                return null;
            }
            return tc.convertion_method;
        }
        */

        internal bool is_ref_inited;
        internal bool is_nullable_inited;
        private bool _is_nullable_type;
        
        public virtual bool IsSealed
        {
            get { return false; }
        }
        
        public virtual bool IsAbstract
        {
        	get { return false; }
        }
        
        public virtual bool is_standard_type
        {
        	get { return false; }
        }
        
        public bool is_nullable_type
        {
        	get
        	{
        		if (is_nullable_inited)
        			return _is_nullable_type;
        		string fname = full_name;
        		if (this is compiled_type_node)
        			_is_nullable_type = fname != null && fname.StartsWith("System.Nullable");
        		else if (original_generic != null)
        			return original_generic.is_nullable_type;
        		is_nullable_inited = true;
        		return _is_nullable_type;
        	}
        }
        
        public virtual bool IsInterface
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public virtual bool is_generic_parameter
        {
            get
            {
                return false;
            }
        }

        public virtual int generic_param_index
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
        
        public virtual SemanticTree.ICommonTypeNode generic_type_container
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual function_node generic_function_container
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        SemanticTree.ICommonFunctionNode SemanticTree.ITypeNode.common_generic_function_container
        {
            get
            {
                return generic_function_container as SemanticTree.ICommonFunctionNode;
            }
        }

        //Ближайший предок, являющийся инстанцией generic-типа
        public virtual generic_instance_type_node base_generic_instance
        {
            get
            {
                return null;
            }
        }

        public virtual bool IsPointer
        {
            get
            {
                return false;
            }
        }
		
        public virtual bool IsEnum
        {
        	get
        	{
        		return false;
        	}
        }

        public virtual bool IsDelegate
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public virtual List<SemanticTree.ITypeNode> ImplementingInterfaces
        {
            get
            {
                return null;
            }
        }

        //true, если на данный момент имеется только предописание класса
        public virtual bool ForwardDeclarationOnly
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public virtual bool is_generic_type_definition
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        //Является ли инстанцией дженерика
        public virtual bool is_generic_type_instance
        {
            get
            {
                return false;
            }
        }

        //Список параметров для инстанции дженерика
        public virtual List<type_node> instance_params
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        //Тип, являющийся описанием дженерика
        public virtual type_node original_generic
        {
            get
            {
                return null;
            }
        }

        public virtual type_node get_instance(List<type_node> param_types)
        {
            return null;
        }

        public virtual bool depended_from_indefinite
        {
            get { return false; }
        }

		protected ref_type_node _ref_type;
		
        /// <summary>
        /// Добавляет узел пересечения типов.
        /// </summary>
        /// <param name="tn">Тип с которым нужно добавить узел пересечения типов.</param>
        /// <param name="intersection">Узел пересечения типов.</param>
		public void add_intersection_node(type_node tn, type_intersection_node intersection, bool is_generated)
		{
			if (type_intersections == null)
				type_intersections = new System.Collections.Generic.Dictionary<type_node, type_intersection_node>(32);
#if (DEBUG)
            type_intersection_node ti;
			if (type_intersections.TryGetValue(tn,out ti))
			{
				throw new CompilerInternalError("Duplicate type intersection");
			}
#endif
			type_intersections[tn]=intersection;
            if (is_generated)
            {
            	if (generated_type_intersections == null)
            		generated_type_intersections = new List<type_node>(32);
            	
            	generated_type_intersections.Add(tn);
            }
		}
        public void clear_generated_intersections()
        {
            if (generated_type_intersections == null || type_intersections == null)
            		return;
        	foreach (type_node tn in generated_type_intersections)
            {
                type_intersections.Remove(tn);
            }
            generated_type_intersections.Clear();
        }

        /// <summary>
        /// Получаем узел пересечения типов.
        /// </summary>
        /// <param name="tn">Тип с которым пересекается этот тип.</param>
        /// <returns>Узел пересечения типов.</returns>
		public type_intersection_node get_type_intersection(type_node tn)
		{
            type_intersection_node tin;
            if (type_intersections != null)
            if (type_intersections.TryGetValue(tn, out tin))
            {
                return tin;
            }
			return null;
		}

        /// <summary>
        /// Базоый тип.
        /// </summary>
        public abstract type_node base_type
        {
            get;
        }

        //public bool need_ref_type = false;
		public virtual ref_type_node ref_type
		{
			get 
			{
                //need_ref_type = true;
                if (_ref_type == null)
                {
                    _ref_type = new ref_type_node(this);
                }
				return _ref_type;
			}
			set
			{
				_ref_type = value;
			}
			//protected set
			//{
			//	_ref_type = value;
			//}
		}
		
        /// <summary>
        /// Имя типа.
        /// </summary>
		public abstract string name
		{
			get;
		}
		
		public abstract string full_name
		{
			get;
		}
		
        public virtual string PrintableName
        {
            get
            {
                return name;
            }
        }

        public virtual string BaseFullName
        {
            get
            {
                return full_name;
            }
        }

        /// <summary>
        /// Свойство по умолчанию.
        /// </summary>
		public abstract property_node default_property_node
		{
			get;
		}

		public abstract SemanticTree.node_kind node_kind
		{
			get;
		}

        /// <summary>
        /// Метод для обхода дерева посетителем.
        /// </summary>
        /// <param name="visitor">Класс - посетитель дерева.</param>
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        SemanticTree.ITypeNode SemanticTree.ITypeNode.base_type
		{
			get
			{
				return this.base_type;
			}
		}
		
        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
		public override general_node_type general_node_type
		{
			get
			{
				return general_node_type.type_node;
			}
		}

        /// <summary>
        /// Добавляем внутренний интерфейс к типу.
        /// </summary>
        /// <param name="ii">Внутренний интерфейс.</param>
		public void add_internal_interface(internal_interface ii)
		{
			if (internal_interfaces == null)
				internal_interfaces = new System.Collections.Generic.Dictionary<internal_interface_kind, internal_interface>();
#if (DEBUG)
			internal_interface int_inf;//=internal_interfaces[ii.internal_interface_kind];
			if (internal_interfaces.TryGetValue(ii.internal_interface_kind,out int_inf))
			{
				throw new CompilerInternalError("Duplicate internal interface addition");
			}
#endif
			internal_interfaces[ii.internal_interface_kind]=ii;
		}

        /// <summary>
        /// Получаем внутренний интерфейс для типа.
        /// </summary>
        /// <param name="iik">Тип внутреннего интерфейса.</param>
        /// <returns>Внутренний интерфейс. null, если ничего не найдено.</returns>
		public internal_interface get_internal_interface(internal_interface_kind iik)
		{
            if (internal_interfaces == null)
				return null;
			internal_interface ret;
            if (internal_interfaces.TryGetValue(iik, out ret))
            {
                return ret;
            }
            return null;
		}

        public abstract List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false);
        public virtual List<SymbolInfo> find(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            return find(name);
        }
        public abstract SymbolInfo find_first_in_type(string name, bool no_search_in_extension_methods = false);
        public virtual SymbolInfo find_first_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            return find_first_in_type(name);
        }
        public abstract List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false);   
        public virtual List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            return find_in_type(name);
        }
        public abstract void add_name(string name, PascalABCCompiler.TreeConverter.SymbolInfo si);
        public abstract void add_generated_name(string name, PascalABCCompiler.TreeConverter.SymbolInfo si);
        public abstract void clear_generated_names();
        public abstract void SetName(string name);

        public abstract SymbolTable.Scope Scope
        {
            get;
        }

        public abstract bool is_value
        {
            get;
        }

        public bool is_value_type
        {
            get
            {
                return is_value;
            }
        }

		public abstract bool is_class
        {
            get;
            set;
        }

        public abstract SemanticTree.type_special_kind type_special_kind
        {
            get;
            set;
        }

        SemanticTree.type_special_kind SemanticTree.ITypeNode.type_special_kind
        {
            get
            {
                return this.type_special_kind;
            }
        }

        private type_node _element_type = null;
        private bool element_type_recived = false;
        public virtual type_node element_type
        {
            get
            {
                if (element_type_recived)
                {
                    return _element_type;
                }
                internal_interface ii = this.get_internal_interface(internal_interface_kind.unsized_array_interface);
                if (ii != null)
                    _element_type = ((array_internal_interface)ii).element_type;
                ii = this.get_internal_interface(internal_interface_kind.bounded_array_interface);
                if (ii != null)
                    _element_type = ((bounded_array_interface)ii).element_type;
                element_type_recived = true;
                return _element_type;
            }
            set
            {
                _element_type = value;
            }
        }

        SemanticTree.ITypeNode SemanticTree.ITypeNode.element_type
        {
            get
            {
                return element_type;
            }
        }


    }
	
	//lroman//
    [Serializable]
    public class lambda_any_type_node : wrapped_type
    {
        public lambda_any_type_node()
        {
        }

        public override function_node get_implicit_conversion_from(type_node ctn)
        {
            return null;
        }

        public override function_node get_implicit_conversion_to(type_node ctn)
        {
            return null;
        }

        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            return null;
        }

        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            return null;
        }

        public override property_node default_property_node
        {
            get
            {
                return null;
            }
        }

        public override type_node base_type
        {
            get
            {
                return null;
            }
        }

        public override SymbolTable.Scope Scope
        {
            get
            {
                return null;
            }
        }

        public override SemanticTree.node_kind node_kind
        {
            get
            {
                return SemanticTree.node_kind.common;
            }
        }

        public override void SetName(string name)
        {
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_type_node;///////////////////????
            }
        }

        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override void add_name(string name, SymbolInfo si)
        {

        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override bool is_value
        {
            get
            {
                return false;
            }
        }

        public override bool is_class
        {
            get
            {
                return false;
            }
            set
            {

            }
        }
    }
	
    [Serializable]
    public abstract class wrapped_type : type_node
    {
        private string _name;

        private System.Collections.Generic.Dictionary<string, List<SymbolInfo>> additional_names =
            new System.Collections.Generic.Dictionary<string, List<SymbolInfo>>(SystemLibrary.SystemLibrary.string_comparer);
        private System.Collections.Generic.Dictionary<SymbolInfo, string> additional_generated_names =
            new System.Collections.Generic.Dictionary<SymbolInfo, string>();

        protected void add_additional_name(string name, PascalABCCompiler.TreeConverter.SymbolInfo si, bool is_generated)
        {
            List<SymbolInfo> sil;// = additional_names[name];
            if (!(additional_names.TryGetValue(name,out sil)))
            {
                sil = new List<SymbolInfo>();
                additional_names.Add(name, sil);
            }
            sil.Add(si);
            if (is_generated)
                additional_generated_names.Add(si, name);
        }

        public override void clear_generated_names()
        {
            foreach (SymbolInfo si in additional_generated_names.Keys)
                additional_names[additional_generated_names[si]].Remove(si);
            additional_generated_names.Clear();
            
            //Незнаю нужно ли это, возможно да — Уже не нужно
            /*foreach(List<SymbolInfoUnit> l in additional_names.Values)
                foreach (SymbolInfoUnit sii in l)
                    sii.Next = null;*/

        }

        protected List<SymbolInfo> find_in_additional_names(string name)
        {
            List<SymbolInfo> sil;// = additional_names[name];
            if (!(additional_names.TryGetValue(name, out sil)))
            {
                return null;
            }
            if (sil.Count == 0)
            {
                return null;
            }
            List<SymbolInfo> new_sil = new List<SymbolInfo>();
            for (int j = 0; j < sil.Count; j++)
            {
                if (sil[j].access_level == access_level.al_protected || sil[j].access_level == access_level.al_internal || sil[j].access_level == access_level.al_private)
                {
                    if (sil[j].sym_info is compiled_constructor_node)
                    {
                        if (NetHelper.NetHelper.is_visible((sil[j].sym_info as compiled_constructor_node).constructor_info))
                            new_sil.Add(sil[j]);
                    }
                    else
                        new_sil.Add(sil[j]);
                }
                else
                    new_sil.Add(sil[j]);
            }
            if (new_sil.Count == 0)
            {
                return null;
            }
            
            return new_sil;
        }

        public override string name
        {
            get
            {
                return _name;
            }
        }
		
		public override string full_name {
			get 
			{ 
				return name; 
			}
		}
        
        public override void SetName(string name)
        {
            _name = name;
        }

        public override void add_name(string name, PascalABCCompiler.TreeConverter.SymbolInfo si)
        {
            add_additional_name(name, si, false);
        }

        public override void add_generated_name(string name, PascalABCCompiler.TreeConverter.SymbolInfo si)
        {
            add_additional_name(name, si, true);
        }

        public override type_node base_type
        {
            get
            {
                return null;
            }
        }

        public override property_node default_property_node
        {
            get
            {
                return null;
            }
        }

        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override SymbolInfo find_first_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override SymbolTable.Scope Scope
        {
            get
            {
                return null;
            }
        }

        public abstract function_node get_implicit_conversion_to(type_node ctn);

        public abstract function_node get_implicit_conversion_from(type_node ctn);

        public abstract function_node get_explicit_conversion_to(type_node ctn);

        public abstract function_node get_explicit_conversion_from(type_node ctn);

        SemanticTree.type_special_kind _type_special_kind = SemanticTree.type_special_kind.none_kind;
        public override SemanticTree.type_special_kind type_special_kind
        {
            get
            {
                return _type_special_kind;
            }
            set
            {
                _type_special_kind = value;
            }
        }

    }
	
	[Serializable]
	public class ref_type_node : wrapped_type, SemanticTree.IRefTypeNode
	{
		private type_node _pointed_type;
        private location _loc;
        private string _pointed_type_name;

        public override string ToString() => this.GetType().Name + ", " + this.PrintableName + ", " + this.Scope;

        public override bool IsPointer
        {
            get
            {
                return true;
            }
        }

        public string PointedTypeName
        {
            get { return _pointed_type_name; }
            set { _pointed_type_name = value; }
        }

        public location loc
        {
            get { return _loc; }
            set { _loc = value; }
        }

		public ref_type_node(type_node pointed_type)
		{
			_pointed_type = pointed_type;
			SystemLibrary.SystemLibrary.init_reference_type(this);
			/*
            ref_type_node rtn = _pointed_type as ref_type_node;
			compiled_type_node ctn = _pointed_type as compiled_type_node;
			bool is_pointer=false;
            if (ctn != null)
            {
                is_pointer = ctn.compiled_type == NetHelper.NetHelper.void_ptr_type;
            }
			string stars = "*";
			while (rtn != null)
			{
				stars += "*";
				ctn = rtn.pointed_type as compiled_type_node;
				if (ctn != null && ctn.compiled_type == typeof(void))
					is_pointer = true;
				rtn = rtn.pointed_type as ref_type_node;
			}
			if (is_pointer == false)
			{
                //TODO: Спростиь у Вани почему здесь имя типа-System.Void***, а не compiled_type_node.name***?
				_base_type = compiled_type_node.get_type_node(NetHelper.NetHelper.FindTypeOrCreate("System.Void"+stars));
				_base_type.base_type = SystemLibrary.SystemLibrary.pointer_type;
			}
			else _base_type = SystemLibrary.SystemLibrary.pointer_type;
            */
		}
		
		public type_node pointed_type
		{
			get
			{
				return _pointed_type;
			}
		}
        internal void SetPointedType(type_node tn)
        {
            _pointed_type = tn;
            tn.ref_type = this;
        }


        //TODO: Не надо наследовать типы указателей друг от друга.
        public override type_node base_type
        {
            get
            {
                return null;//SystemLibrary.SystemLibrary.pointer_type;
            }
        }
		
		SemanticTree.ITypeNode SemanticTree.IRefTypeNode.pointed_type
		{
			get
			{
				return _pointed_type;
			}
		}
		
		private bool check_for_circularity(type_node _pointed_type, type_node tn)
		{
			if ( _pointed_type == null) return false;
			if (_pointed_type == tn) return true;
			if (_pointed_type is ref_type_node) return check_for_circularity((_pointed_type as ref_type_node).pointed_type,tn);
			if (_pointed_type.type_special_kind == SemanticTree.type_special_kind.array_kind || _pointed_type.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
				return check_for_circularity(_pointed_type.element_type,tn);
			return false;
		}
		
		public override string name
		{
			get
			{
				if (base.name != null)
                    return base.name;
				if (_pointed_type != null && !check_for_circularity(_pointed_type,this))
                return PascalABCCompiler.TreeConverter.compiler_string_consts.get_pointer_type_name_by_type_name(_pointed_type.name);
				return "";
			}
		}
		
		public override string full_name {
			get { return name; }
		}
		
		public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
		{
			return find_in_additional_names(name);
		}

        public override SymbolInfo find_first_in_type(string name, bool no_search_in_extension_methods = false)
        {
            var temp = find_in_additional_names(name);
            if (temp != null)
                return temp.FirstOrDefault();
            return null;
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return find_in_additional_names(name);
        }

        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            return find_in_additional_names(name);
        }

        public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.common;
			}
		}
		
        //TODO: Доопределить.
        public override property_node default_property_node
		{
			get
			{
				return null;
			}
		}
		
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.ref_type_node;
			}
		}
		
		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            /*if (ctn == SystemLibrary.SystemLibrary.integer_type)
            {
                return TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(ctn, this);
            }*/
            return null;
        }

        //TODO: К чему еще можно привести указатель явно?
        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            if (ctn == SystemLibrary.SystemLibrary.integer_type)
            {
                return PascalABCCompiler.TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(this, ctn, false);
            }
            return null;
        }

        public override function_node get_implicit_conversion_from(type_node ctn)
        {
            if (SemanticRules.ImplicitConversionFormPointerToTypedPointer)
            {
                if (ctn == SystemLibrary.SystemLibrary.pointer_type)
                {
                    return TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(ctn, this, false);
                }

            }
            return null;
        }

        //TODO: Возможно доделать неявные преобразования типов. К чему еще можно привести указутель неявно?
        public override function_node get_implicit_conversion_to(type_node ctn)
        {            
            ref_type_node rtn = ctn as ref_type_node;
            if (ctn == SystemLibrary.SystemLibrary.pointer_type)
            {
            	return PascalABCCompiler.TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(this, ctn, false);
            }
            if (rtn == null)
            {
            	return null;
            }
            if (rtn.pointed_type == null || this.pointed_type == null)
                return null;
            if (type_table.is_derived(rtn.pointed_type, this.pointed_type))
            {
                return PascalABCCompiler.TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(this, ctn, false);
            }
            return null;
        }

        /*
        public override SymbolTable.Scope Scope
        {
            get
            {
                return null;
            }
        }
        */

        public override bool is_value
        {
            get
            {
                return true;
            }
        }

        public override bool is_class
        {
            get
            {
                return false;
            }
            set
            {
               
            }
        }
	}

    //ssyy

    /// <summary>
    /// Синоним типа.
    /// </summary>
    [Serializable]
    public class type_synonym : definition_node, SemanticTree.ITypeSynonym
    {
        /// <summary>
        /// Имя синонима.
        /// </summary>
        private string _name;

        /// <summary>
        /// Тип, которому даётся новое имя.
        /// </summary>
        private type_node _original_type;

        /// <summary>
        /// Расположение синонима.
        /// </summary>
        private location _loc;

        public type_synonym(string v_name, type_node v_original_type, location loc)
        {
            _name = v_name;
            _original_type = v_original_type;
            _loc = loc;
        }

        public string name
        {
            get
            {
                return _name;
            }
        }
		
        public type_node original_type
        {
            get
            {
                return _original_type;
            }
        }

        SemanticTree.ITypeNode SemanticTree.ITypeSynonym.original_type
        {
            get
            {
                return _original_type;
            }
        }

        /// <summary>
        /// Обобщенный тип узла.
        /// </summary>
        public override general_node_type general_node_type
        {
            get
            {
                return general_node_type.type_synonym;
            }
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.type_synonym;
            }
        }

        public SemanticTree.ILocation Location
        {
            get
            {
                return _loc;
            }
        }

    }

    //\ssyy

    /// <summary>
    /// Обычный тип.
    /// </summary>
	[Serializable]
    public class common_type_node : wrapped_type, SemanticTree.ICommonTypeNode
	{
        /// <summary>
        /// Имя типа.
        /// </summary>
		private string _name;

        /// <summary>
        /// Уровень доступа к типу.
        /// </summary>
		private SemanticTree.type_access_level _type_access_level;

        /// <summary>
        /// Базовый тип.
        /// </summary>
		private type_node _base_type;

        /// <summary>
        /// Пространство имен, которое содержит этот тип.
        /// </summary>
		private common_namespace_node _comprehensive_namespace;

        /// <summary>
        /// Методы, определенные в типе.
        /// </summary>
        private readonly common_method_node_list _methods = new common_method_node_list();

        /// <summary>
        /// Поля класса.
        /// </summary>
        private readonly class_field_list _fields = new class_field_list();

        /// <summary>
        /// Свойства класса.
        /// </summary>
        private readonly common_property_node_list _properties = new common_property_node_list();

        /// <summary>
        /// Список констант, определенных в классе.
        /// </summary>
        private readonly class_constant_definition_list _const_defs = new class_constant_definition_list();
		
        private readonly common_event_list _events = new common_event_list();
        
        /// <summary>
        /// Свойство по умолчанию.
        /// </summary>
		private common_property_node _default_property_node;

        /// <summary>
        /// Расположение класса.
        /// </summary>
		private location _loc;

        internal SymbolTable.Scope defined_in_scope;

        private bool _is_value=false;
        private bool _is_class = false;
		private SymbolTable.ClassScope _scope;

        public bool has_default_constructor = false;
        public bool has_user_defined_constructor = false;

        private bool _has_static_constructor = false;

        public bool has_static_constructor
        {
            get
            {
                return _has_static_constructor;
            }
            set
            {
                _has_static_constructor = value;
            }
        }
		
        private common_method_node _static_constr=null;
        
        public common_method_node static_constr
        {
        	get
        	{
        		return _static_constr;
        	}
        	set
        	{
        		_static_constr = value;
        	}
        }
        
        SemanticTree.ICommonMethodNode SemanticTree.ICommonTypeNode.static_constructor
        {
        	get
        	{
        		return _static_constr;
        	}
        }
        
        private bool _is_generic_type_definition = false;

        public override bool is_generic_type_definition
        {
            get { return _is_generic_type_definition; }
            set { _is_generic_type_definition = value; }
        }

        protected generic_instance_type_node _base_generic_instance = null;

        //Ближайший предок, являющийся инстанцией generic-типа
        public override generic_instance_type_node base_generic_instance
        {
            get
            {
                return _base_generic_instance;
            }
        }

        //Статическое поле, используемое для указания типа аргумента generic-типа.
        //Необходимо для инициализации переменных типа этого аргумента.
        private class_field _runtime_initialization_marker = null;

        public class_field runtime_initialization_marker
        {
            get
            {
                return _runtime_initialization_marker;
            }
            set
            {
                _runtime_initialization_marker = value;
            }
        }

        SemanticTree.ICommonClassFieldNode SemanticTree.ICommonTypeNode.runtime_initialization_marker
        {
            get
            {
                return _runtime_initialization_marker;
            }
        }

        private SemanticTree.ICommonTypeNode _generic_type_container = null;

        public override SemanticTree.ICommonTypeNode generic_type_container
        {
            get
            {
                return _generic_type_container;
            }
            set
            {
                _generic_type_container = value;
            }
        }

        private common_function_node _generic_function_container = null;

        public override function_node generic_function_container
        {
            get { return _generic_function_container; }
            set { _generic_function_container = value as common_function_node; }
        }

        public override bool is_generic_parameter
        {
            get { return _generic_type_container != null || _generic_function_container != null; }
        }

        public override int generic_param_index
        {
            get
            {
                if (_generic_type_container != null)
                {
                    return generic_type_container.generic_params.IndexOf(this);
                }
                else
                {
                    return (generic_function_container as common_function_node).generic_params.IndexOf(this);
                }
            }
        }

        private List<SemanticTree.ICommonTypeNode> _generic_params = null;

        public List<SemanticTree.ICommonTypeNode> generic_params
        {
            get { return _generic_params; }
            set { _generic_params = value; }
        }

        private bool _is_interface = false;

        public override bool IsInterface
        {
            get
            {
                return _is_interface;
            }
            set
            {
                _is_interface = value;
            }
        }

        //ivan
        public override bool IsEnum
        {
            get
            {
                compiled_type_node bt = _base_type as compiled_type_node;
                if (bt == null) return false;
                return (bt.compiled_type == NetHelper.NetHelper.EnumType);
            }
        }

        private bool _is_delegate = false;

        public override bool IsDelegate
        {
            get
            {
                return _is_delegate;
            }
            set
            {
                _is_delegate = value;
            }
        }

        private bool _forward_declaration_only = false;

        public override bool ForwardDeclarationOnly
        {
            get
            {
                return _forward_declaration_only;
            }
            set
            {
                _forward_declaration_only = value;
            }
        }

        private List<SemanticTree.ITypeNode> _implementing_interfaces = new List<SemanticTree.ITypeNode>();
        public override List<SemanticTree.ITypeNode> ImplementingInterfaces
        {
            get
            {
                return _implementing_interfaces;
            }
        }

        public void SetImplementingInterfaces(List<SemanticTree.ITypeNode> interfs)
        {
            _implementing_interfaces = interfs;
        }

        private template_class _original_template = null;
        public template_class original_template
        {
            get
            {
                return _original_template;
            }
            set
            {
                _original_template = value;
            }
        }

        private bool _depended_from_indefinite = false;

        public override bool depended_from_indefinite
        {
            get
            {
                return _depended_from_indefinite;
            }
        }

        public void SetDependedFromIndefinite(bool depended)
        {
            _depended_from_indefinite = depended;
        }


        private SemanticTree.type_special_kind _type_special_kind;

        public common_type_node(string name, SemanticTree.type_access_level type_access_level,
            common_namespace_node comprehensive_namespace, SymbolTable.ClassScope cs, location loc)
        {
            _name = name;
            _type_access_level = type_access_level;
            _comprehensive_namespace = comprehensive_namespace;
            defined_in_scope = (comprehensive_namespace == null)?null:comprehensive_namespace.scope;
            //_ref_type = new ref_type_node(this);
            _scope = cs;
            _loc = loc;
        }

		public common_type_node(type_node base_type,string name,SemanticTree.type_access_level type_access_level,
			common_namespace_node comprehensive_namespace, SymbolTable.ClassScope cs, location loc)
		{
			_name=name;
			_type_access_level=type_access_level;
			_comprehensive_namespace=comprehensive_namespace;
			//_ref_type = new ref_type_node(this);
            _scope = cs;
            _loc = loc;
            this.SetBaseType(base_type);
		}
        bool _sealed = false;
        bool _is_abstract = false;
        bool _is_partial = false;
        public void SetIsSealed(bool val)
        {
            _sealed=val;
        }
        
        public void SetIsAbstract(bool val)
        {
        	_is_abstract = val;
        }

        public bool IsPartial
        {
            get
            {
                return _is_partial;
            }
            set
            {
                _is_partial = value;
            }
        }

        public override bool IsSealed
        {
        	get
        	{
        		/*return _type_special_kind == SemanticTree.type_special_kind.array_kind || _type_special_kind == SemanticTree.type_special_kind.array_wrapper
        			|| _type_special_kind == SemanticTree.type_special_kind.base_set_type || _type_special_kind == SemanticTree.type_special_kind.binary_file
        			|| _type_special_kind == SemanticTree.type_special_kind.typed_file || _type_special_kind == SemanticTree.type_special_kind.diap_type 
        			|| _type_special_kind == SemanticTree.type_special_kind.enum_kind || _type_special_kind == SemanticTree.type_special_kind.set_type*/
        		return _type_special_kind != SemanticTree.type_special_kind.none_kind || _sealed || IsEnum;
        	}
        }
        
        public override bool IsAbstract
        {
        	get
        	{
        		return _is_abstract;
        	}
        }
        
        public int rank
        {
        	get
        	{
        		array_internal_interface aii = get_internal_interface(internal_interface_kind.unsized_array_interface) as array_internal_interface;
        		if (aii != null)
        		return aii.rank;
        		return 0;
        	}
        }
        
        public override SemanticTree.type_special_kind type_special_kind
        {
            get
            {
                return _type_special_kind;
            }
            set
            {
                _type_special_kind = value;
            }
        }

        public SemanticTree.type_special_kind internal_type_special_kind
        {
            get
            {
                return _type_special_kind;
            }
            set
            {
                _type_special_kind = value;
            }
        }
		
        private bool has_flags_attribute()
        {
        	foreach (attribute_node attr in attributes)
        	{
        		if (attr.attribute_type == SystemLibrary.SystemLibrary.flags_attribute_type)
        			return true;
        	}
        	return false;
        }

        public void Merge(common_type_node ctn)
        {
            this.methods.AddRange(ctn.methods.ToArray());
            this.fields.AddRange(ctn.fields.ToArray());
            this.properties.AddRange(ctn.properties.ToArray());
            this.const_defs.AddRange(ctn.const_defs);
            this.events.AddRange(ctn.events);                
        }

        public void add_additional_enum_operations()
        {
        	if (!has_flags_attribute())
        		return;
        	basic_function_node _int_and = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.and_name, this, SemanticTree.basic_function_type.iand);
            basic_function_node _int_or = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.or_name, this, SemanticTree.basic_function_type.ior);
            basic_function_node _int_xor = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.xor_name, this, SemanticTree.basic_function_type.ixor);
            scope.AddSymbol(compiler_string_consts.and_name, new SymbolInfo(_int_and));
            scope.AddSymbol(compiler_string_consts.or_name, new SymbolInfo(_int_or));
            scope.AddSymbol(compiler_string_consts.xor_name, new SymbolInfo(_int_xor));
        }
        
		public common_property_node default_property
		{
			get
			{
				return _default_property_node;
			}
			set
			{
				_default_property_node=value;
			}
		}

		public override property_node default_property_node
		{
			get
			{
                if(_default_property_node!=null)
				    return _default_property_node;
                type_node tn = this.base_type;
                while (tn != null)
                {
                    if (tn.default_property_node != null)
                        return tn.default_property_node;
                    tn = tn.base_type;
                }
                return null;
			}
		}

		public SymbolTable.ClassScope scope
		{
			get
			{
				return _scope;
			}
		}

		public class_constant_definition_list const_defs
		{
			get
			{
				return _const_defs;
			}
		}

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

		public SemanticTree.ILocation Location
		{
			get
			{
				return _loc;
			}
		}

        //(ssyy) Что значат такие комментарии?
		// .
		public common_method_node_list methods
		{
			get
			{
				return _methods;
			}
		}

		// .
		public class_field_list fields
		{
			get
			{
				return _fields;
			}
		}

		// .
		public common_property_node_list properties
		{
			get
			{
				return _properties;
			}
		}
		
		public common_event_list events
		{
			get
			{
				return _events;
			}
		}
		
        public bool internal_is_value
        {
            get
            {
                return _is_value;
            }
            set
            {
                _is_value = value;
            }
        }

		// public  internal.
		public SemanticTree.type_access_level type_access_level
		{
			get
			{
				return _type_access_level;
			}
		}

		// .
		public override type_node base_type
		{
			get
			{
				return _base_type;
			}
            /*
			set
			{
				_base_type=value;
				_scope.BaseClassScope=_base_type.Scope;
			}
            */
		}

        public void SetBaseType(type_node base_type)
        {
            if (base_type == null)
            {
                _base_type = null;
                _scope.BaseClassScope = null;
                return;
            }
            _base_type = base_type;
            if (_scope != null)
            {
                _scope.BaseClassScope = _base_type.Scope;
            }
            _base_generic_instance = base_type.base_generic_instance;
        }

        public void SetBaseTypeIgnoringScope(type_node base_type)
        {
            _base_type = base_type;
            if (base_type != null)
            {
                _base_generic_instance = base_type.base_generic_instance;
            }
        }

        public override SymbolTable.Scope Scope
        {
            get
            {
                return this.scope;
            }
        }

		// .           ,    .
		public override string name
		{
			get
			{
                //if (type_special_kind == SemanticTree.type_special_kind.typed_file)
                //    return compiler_string_consts.GetTypedFileTypeName(element_type.name);
				return _name;
			}
		}
		
		public override string full_name {
			get 
			{ 
				if (_comprehensive_namespace != null && !string.IsNullOrEmpty(_comprehensive_namespace.namespace_name))
				return _comprehensive_namespace.namespace_name+"."+_name;
				return _name;
			}
		}
		
        public override string BaseFullName
        {
            get
            {
                return (this.comprehensive_namespace != null?this.comprehensive_namespace.namespace_full_name + ".":"") + name;
            }
        }

        public override string PrintableName
        {
            get
            {
                if (this.is_generic_parameter)
                {
                    return name;
                }
                if (this.is_generic_type_definition)
                {
                    int pos = _name.IndexOf(compiler_string_consts.generic_params_infix);
                    string rez_name = _name.Substring(0, pos) + "<";
                    rez_name += _generic_params[0].name;
                    for (int i = 1; i < _generic_params.Count; i++)
                    {
                        rez_name += "," + _generic_params[i].name;
                    }
                    rez_name += ">";
                    return rez_name;
                }
                if (this.IsEnum)
                {
                	StringBuilder sb = new StringBuilder();
                	int cnt = _const_defs.Count;
                	if (cnt > 5) cnt = 5;
                	sb.Append('(');
                	int i=0;
                	while (i<cnt)
                	{
                		sb.Append(_const_defs[i].name);
                		if (i<cnt-1)
                			sb.Append(',');
                		i++;
                	}
                	if (cnt < _const_defs.Count)
                		sb.Append("...");
                	sb.Append(')');
                	return sb.ToString();
                }
                if (this.is_value_type && name.Contains("$"))
                {
                    return string.Format(compiler_string_consts.recort_printable_name_template, "...");
                }
                if (this.type_special_kind == SemanticTree.type_special_kind.array_wrapper)
                {
                    bounded_array_interface bai = get_internal_interface(internal_interface_kind.bounded_array_interface) as bounded_array_interface;
                    if (bai != null)
                    {
                        string UpperValue=null, LowerValue=null;
                        if (bai.ordinal_type_interface.upper_value is int_const_node)
                        {
                            UpperValue = (bai.ordinal_type_interface.upper_value as int_const_node).constant_value.ToString();
                            LowerValue = (bai.ordinal_type_interface.lower_value as int_const_node).constant_value.ToString();
                        }
                        else
                        if (bai.ordinal_type_interface.upper_value is char_const_node)
                        {
                            UpperValue = "'" + (bai.ordinal_type_interface.upper_value as char_const_node).constant_value + "'";
                            LowerValue = "'" + (bai.ordinal_type_interface.lower_value as char_const_node).constant_value + "'";
                        }
                        else
                        {
                            UpperValue = bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.upper_value).ToString();
                            LowerValue = bai.ordinal_type_interface.ordinal_type_to_int(bai.ordinal_type_interface.lower_value).ToString();
                        }
                    return string.Format(compiler_string_consts.bounded_array_printable_name_template, LowerValue, UpperValue, bai.element_type.PrintableName);
                    }
                }
                if (this.type_special_kind == SemanticTree.type_special_kind.array_kind)
                {
                    return string.Format(compiler_string_consts.array_printable_name_template, element_type.PrintableName);
                }
                return base.PrintableName;
            }
        }
        
        public constant_node low_bound
        {
        	get
        	{
        		ordinal_type_interface oti = null;
        		if (this.type_special_kind == SemanticTree.type_special_kind.set_type)
        			oti = element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
        		else oti = get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
        		if (oti == null) return null;
        		return oti.lower_value;
        	}
        }
        
        public constant_node upper_bound
        {
        	get
        	{
        		ordinal_type_interface oti = null;
        		if (this.type_special_kind == SemanticTree.type_special_kind.set_type)
        			oti = element_type.get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
        		else oti = get_internal_interface(internal_interface_kind.ordinal_interface) as ordinal_type_interface;
        		if (oti == null) return null;
        		return oti.upper_value;
        	}
        }
        
        
		// ,    .
		public common_namespace_node comprehensive_namespace
		{
			get
			{
				return _comprehensive_namespace;
			}
		}

		public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.common;
			}
		}

        public override void SetName(string name)
        {
            this._name = name;
        }

        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
           return Scope.Find(name, null);//int,:=
        }

        public override List<SymbolInfo> find(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            return Scope.Find(name, CurrentScope);//int,:=
        }

        public override SymbolInfo find_first_in_type(string name, bool no_search_in_extension_methods = false)
        {
            var temp = Scope.FindOnlyInType(name, null);//:=,create,x
            if (temp != null)
                return temp.FirstOrDefault();
            return null;
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return Scope.FindOnlyInType(name, null);//:=,create,x
        }

        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            List<SymbolInfo> sil = Scope.FindOnlyInType(name, CurrentScope);//:=,create,x

            // SSM 2018.04.05 
            if (base_type is compiled_generic_instance_type_node)
            {
                //sil = (base_type as compiled_generic_instance_type_node).original_generic.find_in_type(name, CurrentScope);
                return sil;
            }
                

            if (sil == null && base_type is compiled_type_node && string.Compare(name, "create", true) != 0)
            {
                sil = (base_type as compiled_type_node).find_in_type(name, CurrentScope);
            }
            if (this.is_generic_parameter && sil != null)
            {
                sil = sil?.Select(x => x.copy()).ToList();
                //удаляем повторяющиеся символы
                for (int i = 0; i < sil.Count; ++i)
                {
                    for (int j = i + 1; j < sil.Count; ++j)
                    {
                        if (sil[i].sym_info == sil[j].sym_info)
                        {
                            sil.RemoveAt(j);
                            --j;
                        }
                    }
                }
                //конвертируем
                int index = 0;
                while (index < sil.Count())
                {
                    SemanticTree.IClassMemberNode imn = sil.FirstOrDefault().sym_info as SemanticTree.IClassMemberNode;
                    if (imn != null && imn.comperehensive_type.IsInterface && imn.comperehensive_type.is_generic_type_definition)
                    {
                        List<SymbolInfo> start = new List<SymbolInfo>();
                        foreach (type_node interf in ImplementingInterfaces)
                        {
                            generic_instance_type_node gitn = interf as generic_instance_type_node;
                            if (gitn == null)
                                continue;
                            if (interf.original_generic == imn.comperehensive_type)
                            {
                                definition_node dn = gitn.ConvertMember(sil[index].sym_info);
                                SymbolInfo new_si = new SymbolInfo(dn, sil[index].access_level, sil[index].symbol_kind);
                                start.Add(new_si);
                            }
                        }
                        if (start.Count() > 0)
                        {
                            sil[index].sym_info = start.FirstOrDefault().sym_info;
                            if (start.Count() > 1)
                                sil.Insert(index + 1, start[1]);
                        }
                    }
                    ++index;
                }
            }
            if (this.base_generic_instance != null && sil != null)
            {
                //if (name == "IndexOf" || name == "Add") SSM 5/05/18 Proba
                //    return sil;
                return base_generic_instance.ConvertSymbolInfo(sil);
            }
            else // if (sil == null)
            {
                if (ImplementingInterfaces != null)
                {
                    Dictionary<definition_node, definition_node> cache = new Dictionary<definition_node, definition_node>();
                    List<SymbolInfo> props = new List<SymbolInfo>();
                    foreach (type_node ii_tn in ImplementingInterfaces)
                    {
                        List<SymbolInfo> isi = ii_tn.find_in_type(name, CurrentScope);
                        if (isi != null)
                        {
                            if (sil == null)
                                sil = new List<SymbolInfo>();
                            foreach (SymbolInfo si in isi)
                            {
                                if (!cache.ContainsKey(si.sym_info))
                                {
                                    if (si.sym_info is function_node && (si.sym_info as function_node).is_extension_method)
                                        sil.Add(si);
                                    cache.Add(si.sym_info, si.sym_info);
                                }
                            }


                        }
                    }
                    if (sil != null && sil.Count() == 0)
                        sil = null;
                }
            }
            return sil;
        }

        //(ssyy) Они нигде не используются
		/*public SymbolInfo find_in_comprehensive_scope(string name)
		{
            //TODO: Как это сделать. Спросить у Саши.
			return _scope.Find(name);
		}*/
        /*public SymbolInfo find_in_comprehensive_scope(string name, SymbolTable.Scope CurrentScope)
        {
            //TODO: Как это сделать. Спросить у Саши.
            return _scope.Find(name, CurrentScope);
        }*/

        public override void add_name(string name, PascalABCCompiler.TreeConverter.SymbolInfo si)
        {
            scope.AddSymbol(name, si);
        }
        public override void add_generated_name(string name, PascalABCCompiler.TreeConverter.SymbolInfo si)
        {
            scope.AddSymbol(name, si);
        }

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_type_node;
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

        SemanticTree.ICommonMethodNode[] SemanticTree.ICommonTypeNode.methods
		{
			get
			{
				return (this.methods.ToArray());
			}
		}

		SemanticTree.ICommonClassFieldNode[] SemanticTree.ICommonTypeNode.fields
		{
			get
			{
				return (this.fields.ToArray());
			}
		}

		SemanticTree.ICommonPropertyNode[] SemanticTree.ICommonTypeNode.properties
		{
			get
			{
				return (this.properties.ToArray());
			}
		}
		
		SemanticTree.ICommonEventNode[] SemanticTree.ICommonTypeNode.events
		{
			get
			{
				return (this.events.ToArray());
			}
		}
		
		SemanticTree.ITypeNode SemanticTree.ITypeNode.base_type
		{
			get
			{
				return _base_type;
			}
		}

		SemanticTree.ICommonNamespaceNode SemanticTree.INamespaceMemberNode.comprehensive_namespace
		{
			get
			{
				return _comprehensive_namespace;
			}
		}
		
		
        public SemanticTree.IClassConstantDefinitionNode[] constants
        {
            get
            {
                return _const_defs.ToArray();
            }
        }

        SemanticTree.IPropertyNode SemanticTree.ICommonTypeNode.default_property
        {
            //TODO: Переделать. Нужен поиск default_property_node в базовых классах.
            get
            {
                return default_property_node;
            }
        }
		
        SemanticTree.IConstantNode SemanticTree.ICommonTypeNode.lower_value
        {
        	get
        	{
        		return low_bound;
        	}
        }
        
        SemanticTree.IConstantNode SemanticTree.ICommonTypeNode.upper_value
        {
        	get
        	{
        		return upper_bound;
        	}
        }
        
        public override bool is_value
        {
            get
            {
                return _is_value;
            }
        }

        public override bool is_class
        {
            get
            {
                return _is_class;
            }
            set
            {
                _is_class = value;
            }
        }

        bool _is_ficive=false;
        public bool IsFictive
        {
            get
            {
                return _is_ficive;
            }
            set
            {
                _is_ficive = value;
            }
        }

        public override function_node get_implicit_conversion_to(type_node ctn)
        {
            List<SymbolInfo> sil = this.find_in_type(compiler_string_consts.implicit_operator_name);
        	if (sil != null)
        	{
        		function_node fn = null;
        		foreach(var si in sil)
        		{
        			fn = si.sym_info as function_node;
                    if (fn != null && type_table.is_type_or_original_generics_equal(fn.return_value_type, ctn) && type_table.is_type_or_original_generics_equal(fn.parameters[0].type, this))
                    {
                        if (fn.is_generic_function)
                        {
                            if (this.instance_params != null && this.instance_params.Count > 0)
                            {
                                fn = fn.get_instance(this.instance_params, true, null);
                            }
                            else if (ctn.instance_params != null && ctn.instance_params.Count > 0)
                            {
                                fn = fn.get_instance(ctn.instance_params, true, null);
                            }
                        }
                        return fn;
                    }
        		}
        		return null;
        	}
        	else
        		return null;
        }

        public override function_node get_implicit_conversion_from(type_node ctn)
        {
        	List<SymbolInfo> sil = this.find_in_type(compiler_string_consts.implicit_operator_name);
        	if (sil != null)
        	{
        		function_node fn = null;
        		foreach(var si in sil)
        		{
        			fn = si.sym_info as function_node;
        			if (fn != null && fn.parameters.Count == 1 && type_table.is_type_or_original_generics_equal(fn.parameters[0].type, ctn) && type_table.is_type_or_original_generics_equal(fn.return_value_type, this))
                    {
                        if (fn.is_generic_function)
                        {
                            if (this.instance_params != null && this.instance_params.Count > 0)
                            {
                                fn = fn.get_instance(this.instance_params, true, null);
                            }
                            else if (ctn.instance_params != null && ctn.instance_params.Count > 0)
                            {
                                fn = fn.get_instance(ctn.instance_params, true, null);
                            }
                        }
                        return fn;
                    }
        		}
        		return null;
        	}
            return null;
        }

        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            List<SymbolInfo> sil = this.find_in_type(compiler_string_consts.explicit_operator_name);
        	if (sil != null)
        	{
        		function_node fn = null;
        		foreach(var si in sil)
        		{
        			fn = si.sym_info as function_node;
        			if (fn != null && fn.return_value_type == ctn)
        				return fn;
        		}
        		return null;
        	}
        	else
        		return null;
        }

        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            List<SymbolInfo> sil = this.find_in_type(compiler_string_consts.explicit_operator_name);
        	if (sil != null)
        	{
        		function_node fn = null;
        		foreach(var si in sil)
        		{
        			fn = si.sym_info as function_node;
                    if (fn != null && fn.parameters.Count == 1 && fn.parameters[0].type == ctn)
                        return fn;
        		}
        		return null;
        	}
        	else
        		return null;
        }

        public override type_node get_instance(List<type_node> param_types)
        {
            int count = param_types.Count;
            List<generic_type_instance_info> _generic_instances = generic_convertions.get_type_instances(this);
            type_node founded_inst = generic_convertions.find_type_instance(_generic_instances, param_types);
            if (founded_inst != null) return founded_inst;
            //Создаём новую псевдо-инстанцию
            //SymbolTable.ClassScope ct_scope = SystemLibrary.SystemLibrary.syn_visitor.convertion_data_and_alghoritms.symbol_table.CreateInterfaceOrClassScope(this.IsInterface);
            common_generic_instance_type_node ctnode = new common_generic_instance_type_node(this, param_types, SystemLibrary.SystemLibrary.object_type,
                generic_convertions.MakePseudoInstanceName(name, param_types, true),
                SemanticTree.type_access_level.tal_public, null, this.loc);
            _generic_instances.Add(new generic_type_instance_info(param_types, ctnode));
            generic_convertions.init_generic_instance(this, ctnode, param_types);
            return ctnode;
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
                if (!this.is_generic_type_definition)
                {
                    throw new CompilerInternalError("Type is not generic.");
                }
                _parameters_eliminations = generic_parameter_eliminations.make_eliminations_common(generic_params);
                return _parameters_eliminations;
            }
        }

    }

   

    public class short_string_type_node: wrapped_type, SemanticTree.IShortStringTypeNode
    {
        private SymbolTable.Scope _scope;
        private location _loc;
        private property_node _default_property_node;
        
    	private int length;
        public int Length
        {
            get
            {
                return length;
            }
        }
		
		public SymbolTable.Scope scope {
			get { return _scope; }
		}
        
        public override SymbolTable.Scope Scope
        {
            get
            {
                return this.scope;
            }
        }
         
        public location loc
        {
            get { return _loc; }
            set { _loc = value; }
        }
        
        /*public short_string_type_node(SemanticTree.type_access_level type_access_level,
            common_namespace_node comprehensive_namespace, SymbolTable.ClassScope cs, location loc, int length)
        :base(null,type_access_level,comprehensive_namespace,cs,loc)
        {
            this.length = length;
            this.type_special_kind = SemanticTree.type_special_kind.short_string;
        }*/
        
        public short_string_type_node(SymbolTable.ClassScope cs, location loc, int length)
        {
        	this._scope = cs;
        	this._loc = loc;
        	this.length = length;
        	this.type_special_kind = SemanticTree.type_special_kind.short_string;
        }
		
        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
		{
            //return this.find_in_additional_names(name);
            return _scope.Find(name);
		}
		
		public property_node default_property {
			get { return _default_property_node; }
			set { _default_property_node = value;}
		}
		
        public override property_node default_property_node {
			get { return _default_property_node; }
		}
        
        public override type_node base_type
		{
			get
			{
				return SystemLibrary.SystemLibrary.object_type;
			}
		}
        
        public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.common;
			}
		}
        
		public override bool IsSealed {
			get { return true; }
		}
        
        public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.short_string;
			}
		}
        
        public override bool is_value
        {
            get
            {
                return false;
            }
        }

        public override bool is_class
        {
            get
            {
                return true;
            }
            set
            {
               
            }
        }
        
		public override List<PascalABCCompiler.SemanticTree.ITypeNode> ImplementingInterfaces {
			get { return SystemLibrary.SystemLibrary.string_type.ImplementingInterfaces; }
		}
        
        public override string name
        {
            get
            {
                return compiler_string_consts.GetShortStringTypeName(length);
            }
        }
        
		public override string full_name {
			get { return name; }
		}
        
        public override void add_name(string name, SymbolInfo si)
        {
            _scope.AddSymbol(name, si);
        }

        public override void add_generated_name(string name, SymbolInfo si)
        {
            _scope.AddSymbol(name, si);
        }

        public override SymbolInfo find_first_in_type(string name, bool no_search_in_extension_methods = false)
        {
            if (name == compiler_string_consts.assign_name || name == compiler_string_consts.plusassign_name)
            {
                var temp = _scope.FindOnlyInType(name, null);
                return temp?.FirstOrDefault();
            }
            return SystemLibrary.SystemLibrary.string_type.find_first_in_type(name);
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            //return this.find_in_additional_names(name)
            //SymbolInfo si = _scope.FindOnlyInType(name, null);
            //if (name )
            if (name == compiler_string_consts.assign_name || name == compiler_string_consts.plusassign_name)
                return _scope.FindOnlyInType(name, null);
            return SystemLibrary.SystemLibrary.string_type.find_in_type(name);
        }

        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            //return this.find_in_additional_names(name);
            //SymbolInfo si = _scope.FindOnlyInType(name, CurrentScope);
            if (name == compiler_string_consts.assign_name || name == compiler_string_consts.plusassign_name)
            {
                return _scope.FindOnlyInType(name, null);
            }
            return SystemLibrary.SystemLibrary.string_type.find_in_type(name);
        }

        public override string PrintableName
        {
            get
            {
                return name;
            }
        }
        //Dictionary<type_node,function_node> cv=new Dictionary<type_node,function_node>();
        public override function_node get_implicit_conversion_to(type_node ctn)
        {
            /*if (ctn == base_type)
            {
                if(cv.ContainsKey(ctn))
                    return cv[ctn];
                function_node fn =PascalABCCompiler.TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(ctn, this, false);
                cv.Add(ctn,fn);
                return fn;
            }*/
            /*else if (ctn == PascalABCCompiler.SystemLibrary.SystemLibrary.string_type)
            {
            	
            }*/
            return null;//s:=ss;
        }

        public override function_node get_implicit_conversion_from(type_node ctn)
        {
            //if (ctn == base_type)
            //    return TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(ctn, this, false);
            //SymbolInfo si = find_in_type(compiler_string_consts.implicit_operator_name);
            return null;//ss:=s
        }

        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            return null;
        }

        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            //if (ctn == base_type)
            //    return TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(ctn, this, false);
            return null;//s:=ss;
        }

    }

    

    /// <summary>
    /// Откомпилированный тип.
    /// </summary>
	[Serializable]
	public class compiled_type_node : wrapped_type, SemanticTree.ICompiledTypeNode , SemanticTree.ILocated
	{
        internal System.Type _compiled_type;
		protected compiled_type_node _base_type;

        //Если это не чистить, будет ошибка. Т.к. при следуйщей компиляции области видимости могут изменится.
        internal static System.Collections.Generic.Dictionary<System.Type, compiled_type_node> compiled_types =
            new System.Collections.Generic.Dictionary<Type, compiled_type_node>();

        private NetHelper.NetTypeScope _net_type_scope;

        private PascalABCCompiler.SemanticTree.type_special_kind _type_special_kind = PascalABCCompiler.SemanticTree.type_special_kind.not_set_kind;

        private Dictionary<type_node, function_node> _implicit_convertions_to = new Dictionary<type_node, function_node>();
        private Dictionary<type_node, function_node> _implicit_convertions_from = new Dictionary<type_node, function_node>();
        private Dictionary<type_node, function_node> _explicit_convertions_from = new Dictionary<type_node, function_node>();
        private Dictionary<type_node, function_node> _explicit_convertions_to = new Dictionary<type_node, function_node>();
        private bool enum_operations_inited;

        public override bool IsSealed
        {
            get { return _compiled_type.IsSealed || _compiled_type.IsPointer || _compiled_type == NetHelper.NetHelper.ArrayType || _compiled_type == NetHelper.NetHelper.EnumType; }
        }
		
        public override bool IsAbstract
        {
        	get
        	{
        		return _compiled_type.IsAbstract;
        	}
        }
        
        public bool IsPrimitive
        {
            get { return _compiled_type.IsPrimitive; }
        }
		
        public int rank
        {
        	get
        	{
        		if (_compiled_type.IsArray)
        		return _compiled_type.GetArrayRank();
        		return 0;
        	}
        }
        
        public override bool IsPointer
        {
            get
            {                
                return _compiled_type.IsPointer;
            }
        }
        private int is_interface;

        //ssyy
        public override bool IsInterface
        {
            get
            {
                if (is_interface == 0)
                    is_interface = _compiled_type.IsInterface ? 1 : -1;
                return is_interface == 1;
            }
            set
            {
            }
        }
        
        public override bool is_standard_type
        {
        	get
        	{
        		return NetHelper.NetHelper.IsStandType(_compiled_type);
        	}
        }
        
        public override function_node generic_function_container
        {
            get
            {
                if (!_compiled_type.IsGenericParameter || _compiled_type.DeclaringMethod == null)
                {
                    return null;
                }
                return compiled_function_node.get_compiled_method(_compiled_type.DeclaringMethod as System.Reflection.MethodInfo);
            }
        }

        public override bool is_generic_type_definition
        {
            get
            {
                return _compiled_type.IsGenericTypeDefinition;
            }
        }

        public override int generic_param_index
        {
            get
            {
                return _compiled_type.GenericParameterPosition;
            }
        }

        public override bool is_generic_type_instance
        {
            get
            {
                return _compiled_type.IsGenericType && !_compiled_type.IsGenericTypeDefinition;
            }
        }

        List<compiled_type_node> _generic_params = null;

        public List<compiled_type_node> generic_params
        {
            get
            {
                if (_generic_params == null)
                {
                    _generic_params = new List<compiled_type_node>();
                    foreach (Type t in _compiled_type.GetGenericArguments())
                    {
                        _generic_params.Add(compiled_type_node.get_type_node(t));
                    }
                }
                return _generic_params;
            }
        }

        public override bool is_generic_parameter
        {
            get
            {
                return _compiled_type.IsGenericParameter;
            }
        }

		public override string full_name {
			get 
			{ 
				if (!_compiled_type.IsGenericType)
				return _compiled_type.FullName;
				StringBuilder sb = new StringBuilder();
				sb.Append(_compiled_type.GetGenericTypeDefinition().Namespace+"."+_compiled_type.GetGenericTypeDefinition().Name.Substring(0,_compiled_type.GetGenericTypeDefinition().Name.IndexOf('`')));
				if (!_compiled_type.IsGenericTypeDefinition)
				{
					sb.Append("{");
					Type[] tt = _compiled_type.GetGenericArguments();
					for (int i=0; i<tt.Length; i++)
					{
						sb.Append(tt[i].FullName);
						if (i<tt.Length-1)
							sb.Append(",");
					}
                    sb.Append("}");
				}
				return sb.ToString();
			}
		}
        
        public override type_node  original_generic
        {
            get
            {
                if (!is_generic_type_instance)
                {
                    return null;
                }
                Type orig = _compiled_type.GetGenericTypeDefinition();
                if (orig == null)
                {
                    return null;
                }
                return compiled_type_node.get_type_node(orig);
            }
        }

        private List<type_node> _instance_params = null;
        public override List<type_node> instance_params
        {
            get
            {
                if (_instance_params != null)
                {
                    return _instance_params;
                }
                Type[] pars = _compiled_type.GetGenericArguments();
                _instance_params = new List<type_node>(pars.Length);
                foreach (Type t in pars)
                {
                    _instance_params.Add(compiled_type_node.get_type_node(t));
                }
                return _instance_params;
            }
        }
		
        public override bool IsDelegate
        {
        	get
        	{
        		return _compiled_type == NetHelper.NetHelper.DelegateType || _compiled_type == NetHelper.NetHelper.MulticastDelegateType
        			|| _compiled_type.BaseType == NetHelper.NetHelper.DelegateType || _compiled_type.BaseType == NetHelper.NetHelper.MulticastDelegateType;
        	}
        }

        private int is_enum;

        public override bool IsEnum
        {
        	get
        	{
                if (is_enum == 0)
                    is_enum = (_compiled_type.IsEnum || compiled_type.BaseType != null && compiled_type.BaseType == NetHelper.NetHelper.EnumType)?1:-1;
                return is_enum == 1;
            }
        }

		public override ref_type_node ref_type {
			get 
			{ 
				if (_compiled_type == NetHelper.NetHelper.void_type) return null;
				return base.ref_type;
			}
		}
        
        private List<SemanticTree.ITypeNode> _implementing_interfaces = null;
        public override List<SemanticTree.ITypeNode> ImplementingInterfaces
        {
            get
            {
                if (_implementing_interfaces == null)
                {
                    Type[] interf = _compiled_type.GetInterfaces();
                    _implementing_interfaces = new List<SemanticTree.ITypeNode>(interf.Length);
                    for (int i = 0; i < interf.Length; i++)
                    {
                        _implementing_interfaces.Add(get_type_node(interf[i]));
                    }
                }
                return _implementing_interfaces;
            }
        }
        //\ssyy

        //ssyy
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
                if (!_compiled_type.IsGenericTypeDefinition)
                {
                    throw new CompilerInternalError("Type is not generic.");
                }
                _parameters_eliminations = generic_parameter_eliminations.make_eliminations_compiled(_compiled_type.GetGenericArguments());
                return _parameters_eliminations;
            }
        }

        public override type_node get_instance(List<type_node> param_types)
        {
            int count = param_types.Count;
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
                Type rez_t = _compiled_type.MakeGenericType(ts.ToArray());
                compiled_type_node rez = compiled_type_node.get_type_node(rez_t);
                if (rez.scope == null)
                {
                    rez.scope = new NetHelper.NetTypeScope(rez_t, SystemLibrary.SystemLibrary.symtab);
                }
                return rez;
            }
            List<generic_type_instance_info> _generic_instances = generic_convertions.get_type_instances(this);
            type_node founded_inst = generic_convertions.find_type_instance(_generic_instances, param_types);
            if (founded_inst != null) return founded_inst;
            //Создаём новую псевдо-инстанцию
            //SymbolTable.ClassScope ct_scope = SystemLibrary.SystemLibrary.syn_visitor.convertion_data_and_alghoritms.symbol_table.CreateInterfaceOrClassScope(this.IsInterface);
            compiled_generic_instance_type_node ctnode = new compiled_generic_instance_type_node(this, param_types, SystemLibrary.SystemLibrary.object_type,
                generic_convertions.MakePseudoInstanceName(name, param_types, true),
                SemanticTree.type_access_level.tal_public, null, /*ct_scope,*/ this.loc);
            _generic_instances.Add(new generic_type_instance_info(param_types, ctnode));
            generic_convertions.init_generic_instance(this, ctnode, /*ct_scope,*/ param_types);
            return ctnode;
        }
        //\ssyy

        private string MakePseudoInstanceName(List<type_node> param_types)
        {
            int last = name.LastIndexOf(compiler_string_consts.generic_params_infix);
            string rez = name.Substring(0, last);
            bool first = true;
            foreach (type_node tnode in param_types)
            {
                rez += ((first) ? "<" : ",") + tnode.name;
                first = false;
            }
            rez += ">";
            return rez;
        }
		
        private location loc=null;
        public SemanticTree.ILocation Location
        {
            get { return loc;}
            set { loc = (location)value; }
        }

        public override SymbolTable.Scope Scope
        {
            get
            {
                return _net_type_scope;
            }
        }

        public NetHelper.NetTypeScope scope
        {
            get
            {
                return _net_type_scope;
            }
            set
            {
            	_net_type_scope = value;
            }
        }

		private compiled_type_node(System.Type st)
		{
			_compiled_type=st;
			_ref_type = new ref_type_node(this);
            //TODO: Почему?
			//if (st != NetHelper.NetHelper.void_ptr_type && st.BaseType == null) _base_type = (compiled_type_node)_sli.pointer_type;
		}

        private void mark_if_delegate()
        {
            //До того как мы добавили в стандартную библиотеку MulticastDelegate других делегатов не добавляем.
            //Это важно при инициализации стандартной библиотеки.
            if (SystemLibrary.SystemLibrary.delegate_base_type != null)
            {
                if (base_type == SystemLibrary.SystemLibrary.delegate_base_type)
                {
                    SymbolInfo si_int = this.find_first_in_type(compiler_string_consts.invoke_method_name);
#if DEBUG
                    if (si_int == null)
                    {
                        throw new CompilerInternalError("No invoke method in class derived from MulticastDelegate");
                    }
#endif
                    compiled_function_node invoke = si_int.sym_info as compiled_function_node;
#if DEBUG
                    if (invoke == null)
                    {
                        throw new CompilerInternalError("No invoke method in class derived from MulticastDelegate");
                    }
#endif
                    SymbolInfo si_cons = this.find_first_in_type(compiler_string_consts.net_constructor_name);
                    compiled_constructor_node ctor = si_cons.sym_info as compiled_constructor_node;
#if DEBUG
                    if (ctor == null)
                    {
                        throw new CompilerInternalError("No constructor in class derived from MulticastDelegate");
                    }
#endif
                    delegate_internal_interface dii = new delegate_internal_interface(invoke.return_value_type,
                        invoke, ctor);
                    dii.parameters.AddRange(invoke.parameters);
                    this.add_internal_interface(dii);
                    add_delegate_operator(compiler_string_consts.plusassign_name, type_constructor.instance.delegate_add_assign_compile_time_executor);
                    add_delegate_operator(compiler_string_consts.plus_name, type_constructor.instance.delegate_add_compile_time_executor);
                    add_delegate_operator(compiler_string_consts.minusassign_name, type_constructor.instance.delegate_sub_assign_compile_time_executor);
                    add_delegate_operator(compiler_string_consts.minus_name, type_constructor.instance.delegate_sub_compile_time_executor);
                }
            }
        }

        private void add_delegate_operator(string name, compile_time_executor executor)
        {
            common_namespace_function_node cnfn = new common_namespace_function_node(name, this, null, null, null);
            cnfn.ConnectedToType = this;
            cnfn.compile_time_executor = executor;
            add_name(name, new SymbolInfo(cnfn));
            common_parameter cp1 = new common_parameter(compiler_string_consts.left_param_name, this, SemanticTree.parameter_type.value,
                                                        cnfn, concrete_parameter_type.cpt_none, null, null);
            common_parameter cp2 = new common_parameter(compiler_string_consts.right_param_name, this, SemanticTree.parameter_type.value,
                                                        cnfn, concrete_parameter_type.cpt_none, null, null);
            cnfn.parameters.AddElement(cp1);
            cnfn.parameters.AddElement(cp2);
        }

        //private bool ctors_inited = false;

		private void init_constructors()
		{
            //if (ctors_inited) return;
            //ctors_inited = true;
			System.Reflection.ConstructorInfo[] cons_arr=_compiled_type.GetConstructors();
			System.Reflection.ConstructorInfo[] prot_consttr_arr = _compiled_type.GetConstructors(BindingFlags.FlattenHierarchy|BindingFlags.Instance|BindingFlags.Public | BindingFlags.NonPublic);
			List<System.Reflection.ConstructorInfo> cnstrs = new List<System.Reflection.ConstructorInfo>();
			cnstrs.AddRange(cons_arr);
			for (int i=0; i<prot_consttr_arr.Length; i++)
				if (!cnstrs.Contains(prot_consttr_arr[i]))
				cnstrs.Add(prot_consttr_arr[i]);
			cons_arr = cnstrs.ToArray();
			foreach(System.Reflection.ConstructorInfo ci in cons_arr)
			{
				//if (ci.IsPrivate || ci.IsAssembly) 
				//	continue;
				compiled_constructor_node ccn=compiled_constructor_node.get_compiled_constructor(ci);
				SymbolInfo si=new SymbolInfo(ccn);
				NetHelper.NetHelper.AddConstructor(ci, ccn);
				//add_additional_name(compiler_string_consts.standart_constructor_name,si);
                add_name(compiler_string_consts.default_constructor_name, si);
			}
		}


        public static compiled_type_node get_type_node(System.Type st, SymbolTable.TreeConverterSymbolTable tcst)
        {
            compiled_type_node ctn = get_type_node(st);
            if (ctn.scope == null)
            {
                ctn.scope = new NetHelper.NetTypeScope(ctn.compiled_type, tcst);
                //ivan added and, or for enums
                if (ctn.compiled_type.IsEnum)
                {
                    InitEnumOperations(ctn);
                }
            }
            return ctn;
        }

        private static void InitEnumOperations(compiled_type_node ctn)
        {
            if (ctn.compiled_type.GetCustomAttributes(typeof(FlagsAttribute), true).Length == 0) return;
            basic_function_node _int_and = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.and_name, ctn, SemanticTree.basic_function_type.iand);
            basic_function_node _int_or = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.or_name, ctn, SemanticTree.basic_function_type.ior);
            basic_function_node _int_xor = SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.xor_name, ctn, SemanticTree.basic_function_type.ixor);
            if (ctn.scope != null)
            {
                ctn.scope.AddSymbol(compiler_string_consts.and_name, new SymbolInfo(_int_and));
                ctn.scope.AddSymbol(compiler_string_consts.or_name, new SymbolInfo(_int_or));
                ctn.scope.AddSymbol(compiler_string_consts.xor_name, new SymbolInfo(_int_xor));
            }
        }

        public void reinit_scope()
        {
            if (scope != null)
            {
                scope = new NetHelper.NetTypeScope(compiled_type, scope.SymbolTable);
                if (compiled_type.IsEnum && !enum_operations_inited)
                {
                    InitEnumOperations(this);
                    enum_operations_inited = true;
                }
            }
        }
		
        public void init_scope()
        {
        	this.scope = new NetHelper.NetTypeScope(_compiled_type, SystemLibrary.SystemLibrary.symtab);
        }
        
        public static compiled_type_node get_type_node(System.Type st)
		{
            //(ssyy) Обрабатываем параметры generic-типов
            //Сделаю потом, если это понадобится.
            //if (st.IsGenericParameter)
            //{
            //}
            //if (st.Name.EndsWith("&") == true)
            
            //(ssyy) Лучше так
			if (st.IsByRef)
			{
				//return get_type_node(st.Module.GetType(st.FullName.Substring(0,st.FullName.Length-1)));
                return get_type_node(st.GetElementType());
            }
			compiled_type_node ctn;//=compiled_types[st];
            if (compiled_types.TryGetValue(st, out ctn))
			{
                //ctn.reinit_scope();
                return ctn;
			}
			ctn=new compiled_type_node(st);
            
            //Если это не чистить, будет ошибка. Т.к. при следующей компиляции области видимости могут изменится.
            //Но если это чистить то тоже ошибка. нужна еще одна статическая таблица для стандартных типов
			compiled_types[st] = ctn;
			
            ctn.init_constructors();
            ctn.mark_if_delegate();
            ctn.mark_if_array();
            if (st.IsEnum)
            {
                internal_interface ii = SystemLibrary.SystemLibrary.integer_type.get_internal_interface(internal_interface_kind.ordinal_interface);
                ordinal_type_interface oti_old = (ordinal_type_interface)ii;
                enum_const_node lower_value = new enum_const_node(0, ctn, ctn.loc);
                enum_const_node upper_value = new enum_const_node(st.GetFields().Length-2, ctn, ctn.loc);
                ordinal_type_interface oti_new = new ordinal_type_interface(oti_old.inc_method, oti_old.dec_method,
                    oti_old.inc_value_method, oti_old.dec_value_method,
                    oti_old.lower_eq_method, oti_old.greater_eq_method, 
                    oti_old.lower_method, oti_old.greater_method,
                    lower_value, upper_value, oti_old.value_to_int, oti_old.ordinal_type_to_int);

                ctn.add_internal_interface(oti_new);
                SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.gr_name, ctn, SemanticTree.basic_function_type.enumgr, SystemLibrary.SystemLibrary.bool_type);
                SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.greq_name, ctn, SemanticTree.basic_function_type.enumgreq, SystemLibrary.SystemLibrary.bool_type);
                SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.sm_name, ctn, SemanticTree.basic_function_type.enumsm, SystemLibrary.SystemLibrary.bool_type);
                SystemLibrary.SystemLibrary.make_binary_operator(compiler_string_consts.smeq_name, ctn, SemanticTree.basic_function_type.enumsmeq, SystemLibrary.SystemLibrary.bool_type);
                InitEnumOperations(ctn);
            }
            //ctn.init_scope();
            //TODO: Тут надо подумать. Может как-то сделать по другому?
            if (!NetHelper.NetHelper.IsStandType(st))
			{
				SystemLibrary.SystemLibrary.init_reference_type(ctn);
			}
			return ctn;
		}

        private void mark_if_array()
        {
            if (!(compiled_type.IsArray))
            {
                return;
            }
            type_node elem_type=compiled_type_node.get_type_node(compiled_type.GetElementType());
            array_internal_interface aii = new array_internal_interface(elem_type,_compiled_type.GetArrayRank());
            add_internal_interface(aii);
            return;
        }

		//TODO: неплохо было-бы закрыть от всех, кроме интерфейсов.
		public System.Type compiled_type
		{
			get
			{
				return _compiled_type;
			}
		}

        public override SemanticTree.type_special_kind type_special_kind
        {
            get
            {
                if (_type_special_kind != PascalABCCompiler.SemanticTree.type_special_kind.not_set_kind)
                    return _type_special_kind;
                if (compiled_type.IsArray)
                {
                    _type_special_kind = SemanticTree.type_special_kind.array_kind;
                }
                else if (compiled_type.IsEnum || compiled_type.BaseType != null && compiled_type.BaseType == NetHelper.NetHelper.EnumType)
                {
                    _type_special_kind = SemanticTree.type_special_kind.enum_kind;
                }
                else if (compiled_type.IsValueType && !NetHelper.NetHelper.IsStandType(compiled_type))
                {
                    _type_special_kind = SemanticTree.type_special_kind.record;
                }
                else
                    _type_special_kind = SemanticTree.type_special_kind.none_kind;
                return _type_special_kind;
            }
            set
            {
                this._type_special_kind = value;
            }
        }

        private bool base_type_is_null;

		// .
		public override type_node base_type
        {
            get
            {
                if (_base_type != null)
                {
                    return _base_type;
                }
                if (base_type_is_null)
                    return null;
                System.Type bn = _compiled_type.BaseType;
                if (bn == null)
                {
                    base_type_is_null = true;
                    return null;
                }
                _base_type = get_type_node(bn, SystemLibrary.SystemLibrary.symtab);
                return _base_type;
            }
        }

		// .
		public override string name
		{
			get
			{
                if (base.name != null)
                {
                    return base.name;
                }
				return _compiled_type.Name;
			}
		}

        private compiled_property_node _default_property = null;
        private bool _default_property_recived = false;

        //TODO: Доопределить.
        //DarkStar: доопределил как смог. Коля проверь. 
        private compiled_property_node _default_property_node = null;
        public override property_node default_property_node
		{
			get
			{
				//return null;
                if (_default_property_recived)
                {
                    return _default_property;
                }
                System.Reflection.MemberInfo[] def_members = _compiled_type.GetDefaultMembers();
                if ((def_members != null) && (def_members.Length > 0))
                {
                    foreach (System.Reflection.MemberInfo mi in def_members)
                    {
                        System.Reflection.PropertyInfo pi = mi as System.Reflection.PropertyInfo;
                        if (pi != null)
                        {
                            _default_property = new compiled_property_node(pi);
                            break;
                        }
                    }
                }
                _default_property_recived = true;
                return _default_property;
			}
		}

        private List<SymbolInfo> AddToSymbolInfo(List<SymbolInfo> to, List<SymbolInfo> from, bool clone = false)
        {
            if (to == null)
            {
                return from;
            }
            if (from == null)
            {
                return to;
            }
            if (clone)
            {
                List<SymbolInfo> old = to;
                to = new List<SymbolInfo>(old);
            }
            Dictionary<definition_node, definition_node> defs = new Dictionary<definition_node, definition_node>();
            foreach (SymbolInfo si in to)
                defs[si.sym_info] = si.sym_info;
            foreach (SymbolInfo si in from)
                if (!defs.ContainsKey(si.sym_info))
                    to.Add(si);
            return to;
        }

        //(ssyy) 24.10.2007. Эта функция работает неверно, возвращая дважды найденную функцию.
        //Попытался написать правильно.
        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
            /*//Старый код
            SymbolInfo si = AddToSymbolInfo(compiled_find(name), find_in_additional_names(name));
            if(scope!=null)
                si = AddToSymbolInfo(si, scope.SymbolTable.Find(scope,name));
			return si;
            */
            if (this.type_special_kind == SemanticTree.type_special_kind.array_kind && scope == null)
                this.init_scope();
            if (scope == null)
            {
                List<SymbolInfo> sil = compiled_find(name);
                List<SymbolInfo> sil2 = find_in_additional_names(name);
                if (sil == null && sil2 == null && string.Compare(name,"Create",true) != 0)
                {
                    compiled_type_node bas_type = base_type as compiled_type_node;
                    while (sil == null && bas_type != null && bas_type.scope != null)
                    {
                        sil = bas_type.scope.SymbolTable.Find(bas_type.scope, name);
                        bas_type = bas_type.base_type as compiled_type_node;
                    }
                    if (sil == null)
                    {
                        for (int i = 0; i < ImplementingInterfaces.Count; i++)
                        {
                            compiled_type_node ctn = ImplementingInterfaces[i] as compiled_type_node;
                            if (ctn.is_generic_type_instance)
                                ctn = ctn.original_generic as compiled_type_node;
                            if (ctn != null && ctn.scope != null)
                            {
                                sil = ctn.scope.SymbolTable.Find(ctn.scope, name);
                            }
                            if (sil != null)
                                break;
                        }
                    }

                }
                return AddToSymbolInfo(sil, sil2);
            }
            else
            {
                List<SymbolInfo> sil = scope.SymbolTable.Find(scope, name);
                List<SymbolInfo> sil2 = find_in_additional_names(name);
                List<SymbolInfo> sil3 = compiled_find(name);
                bool clone = false;
                if (!no_search_in_extension_methods || this._compiled_type.IsGenericType)
                {
                    
                    if (this.type_special_kind == SemanticTree.type_special_kind.array_kind && this.base_type.Scope != null)
                    {
                        List<SymbolInfo> tmp_si = this.base_type.Scope.SymbolTable.Find(this.base_type.Scope, name);
                        if (tmp_si != null)
                        {
                            if (sil == null)
                                sil = tmp_si;
                            else if (sil.FirstOrDefault().sym_info != tmp_si.FirstOrDefault().sym_info)
                                sil = AddToSymbolInfo(tmp_si, sil, true);
                            clone = true;
                        }
                    }
                    if (this.compiled_type.IsGenericType && !this.compiled_type.IsGenericTypeDefinition)
                    {
                        compiled_type_node ctn = compiled_type_node.get_type_node(this.compiled_type.GetGenericTypeDefinition());
                        if (ctn.scope != null)
                        {
                            List<SymbolInfo> tmp_si = ctn.scope.SymbolTable.Find(ctn.scope, name);
                            if (tmp_si != null && tmp_si.FirstOrDefault().sym_info is function_node && (tmp_si.FirstOrDefault().sym_info as function_node).is_extension_method)
                            {
                                if (sil == null)
                                    sil = tmp_si;
                                else if (sil.FirstOrDefault().sym_info != tmp_si.FirstOrDefault().sym_info)
                                    sil = AddToSymbolInfo(sil, tmp_si, true);
                                clone = true;
                            }
                        }
                    }
                    compiled_type_node bas_type = base_type as compiled_type_node;
                    if (bas_type == null)
                    {
                        bas_type = compiled_type_node.get_type_node(typeof(object));
                    }
                    while (bas_type != null && bas_type.scope != null)
                    {
                        List<SymbolInfo> tmp_si = bas_type.scope.SymbolTable.Find(bas_type.scope, name);
                        if (tmp_si != null && tmp_si.FirstOrDefault().sym_info is function_node && (tmp_si.FirstOrDefault().sym_info as function_node).is_extension_method)
                        {
                            if (sil == null)
                                sil = tmp_si;
                            else
                            {
                                if (sil.FirstOrDefault().sym_info != tmp_si.FirstOrDefault().sym_info)
                                    sil = AddToSymbolInfo(sil, tmp_si, true);
                            }
                            clone = true;
                        }
                        bas_type = bas_type.base_type as compiled_type_node;
                    }
                    for (int i = 0; i < ImplementingInterfaces.Count; i++)
                    {
                        compiled_type_node ctn = ImplementingInterfaces[i] as compiled_type_node;
                        if (ctn.is_generic_type_instance)
                            ctn = ctn.original_generic as compiled_type_node;
                        if (ctn != null && ctn.scope != null)
                        {
                            List<SymbolInfo> tmp_si = ctn.scope.SymbolTable.Find(ctn.scope, name);
                            if (tmp_si != null && tmp_si.FirstOrDefault().sym_info is function_node && (tmp_si.FirstOrDefault().sym_info as function_node).is_extension_method)
                            {
                                if (sil == null)
                                    sil = tmp_si;
                                else
                                {
                                    if (sil.FirstOrDefault().sym_info != tmp_si.FirstOrDefault().sym_info)
                                        sil = AddToSymbolInfo(sil, tmp_si, true);
                                }
                                clone = true;
                            }
                        }
                    }
                }
                if (sil != null)
                {
                    sil = AddToSymbolInfo(sil, sil3, false);
                }
                else if (sil3 != null)
                    sil = sil3;
                
                if (sil == null && sil2 == null && this == SystemLibrary.SystemLibrary.string_type && SystemLibrary.SystemLibrary.syn_visitor.from_pabc_dll)
                {
                    sil = compiled_type_node.get_type_node(NetHelper.NetHelper.PABCSystemType).find_in_type(name);
                }
                return AddToSymbolInfo(sil, sil2, clone);
            }
		}

        public override SymbolInfo find_first_in_type(string name, bool no_search_in_extension_methods = false)
        {
            var temp = find(name, no_search_in_extension_methods);
            if (temp != null)
                return temp.FirstOrDefault();
            return null;
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return find(name, no_search_in_extension_methods);
        }

        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            //return find(name);
            List<SymbolInfo> result = find(name, no_search_in_extension_methods);
            //Поищем также среди extentions-методов.
            if (is_generic_type_instance)
            {
                List<SymbolInfo> ext = original_generic.find_in_type(name);
                List<SymbolInfo> start = null;
                if (ext != null)
                {
                    for(int cur_index = 0; cur_index < ext.Count(); cur_index++)
                    {
                        if (ext == result)
                            continue;
                        if (ext[cur_index].sym_info is wrapped_definition_node)
                            BasePCUReader.RestoreSymbols(ext, name, cur_index);
                        if (ext[cur_index].sym_info is common_namespace_function_node)
                        {
                            if (start == null)
                            {
                                start = ext.GetRange(cur_index, ext.Count() - cur_index);
                                if (result != null)
                                {
                                    result.RemoveRange(1, result.Count() - 1);
                                    result.AddRange(start);
                                }
                            }
                        }
                    }
                }
                //Построили список extentions-методов. Теперь сольём его с основным списком.
                if (result == null)
                {
                    result = start;
                }
                else
                {
                    List<SymbolInfo> si_lst = new List<SymbolInfo>();
                    List<definition_node> lst = new List<definition_node>();
                    foreach (var current_unit in result)
                    {
                        lst.Add(current_unit.sym_info);
                        //si_lst.Add(current);
                    }

                    foreach (var current_unit in result)
                    {
                        if (!lst.Contains(current_unit.sym_info))
                        {
                            lst.Add(current_unit.sym_info);
                            si_lst.Add(current_unit);
                        }
                    }
                    if (si_lst.Count > 0)
                    {
                        result.RemoveRange(1, result.Count() - 1);
                        for (int i = 0; i < si_lst.Count; i++)
                        {
                            result.Add(si_lst[i]);
                        }
                    }
                }
            }
            return result;
        }

        private List<SymbolInfo> compiled_find(string name)
		{
            return PascalABCCompiler.NetHelper.NetHelper.FindName(_compiled_type, name);
		}

        public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.compiled_type_node;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.ITypeNode SemanticTree.ITypeNode.base_type
		{
			get
			{
				return _base_type;
			}
		}

		public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.compiled;
			}
		}

        //TODO: Доопределить.
        public override function_node get_implicit_conversion_to(type_node ctn)
        {
            compiled_type_node cctn = ctn as compiled_type_node;
            if (cctn == null)
            {
                return null;
            }
            function_node fn = null;
            if (!_implicit_convertions_to.TryGetValue(cctn, out fn))
            {
                fn = NetHelper.NetHelper.get_implicit_conversion(this, this, cctn, scope);
                if (fn is compiled_function_node)
                    _implicit_convertions_to.Add(cctn, fn);
                else if (fn == null && (this.is_generic_type_instance || cctn.is_generic_type_instance))
                {
                    List<type_node> instance_params1 = this.instance_params;
                    List<type_node> instance_params2 = cctn.instance_params;
                    compiled_type_node orig_generic = this;
                    if (this.is_generic_type_instance)
                        orig_generic = compiled_type_node.get_type_node(_compiled_type.GetGenericTypeDefinition());
                    compiled_type_node orig_generic2 = cctn;
                    if (cctn.is_generic_type_instance)
                        orig_generic2 = compiled_type_node.get_type_node(cctn.compiled_type.GetGenericTypeDefinition());
                    fn = NetHelper.NetHelper.get_implicit_conversion(orig_generic, orig_generic, orig_generic2, orig_generic.scope);
                    if (fn != null)
                    {
                        List<type_node> instance_params;
                        if (instance_params1.Count > 0)
                            instance_params = instance_params1;
                        else
                            instance_params = instance_params2;
                        fn = fn.get_instance(instance_params, false, null);
                        return fn;
                    } 
                }
            }
            
            return fn;
        }

        //НЕЯВНО
        public override function_node get_implicit_conversion_from(type_node ctn)
        {
            compiled_type_node cctn = ctn as compiled_type_node;
            if (cctn == null)
            {
                return null;
            }
            function_node fn = null;
            if (!_implicit_convertions_from.TryGetValue(cctn, out fn))
            {
                fn = NetHelper.NetHelper.get_implicit_conversion(this, cctn, this, scope);
                if (fn is compiled_function_node)
                    _implicit_convertions_from.Add(cctn, fn);
                else if (fn == null && (cctn.is_generic_type_instance || this.is_generic_type_instance))
                {
                    List<type_node> instance_params1 = this.instance_params;
                    List<type_node> instance_params2 = cctn.instance_params;
                    compiled_type_node orig_generic = cctn;
                    if (cctn.is_generic_type_instance)
                        orig_generic = compiled_type_node.get_type_node(cctn.compiled_type.GetGenericTypeDefinition());
                    compiled_type_node orig_generic2 = this;
                    if (this.is_generic_type_instance)
                        orig_generic2 = compiled_type_node.get_type_node(this.compiled_type.GetGenericTypeDefinition());
                    fn = NetHelper.NetHelper.get_implicit_conversion(orig_generic2, orig_generic, orig_generic2, orig_generic2.scope);
                    if (fn != null)
                    {
                        List<type_node> instance_params;
                        if (instance_params1.Count > 0)
                            instance_params = instance_params1;
                        else
                            instance_params = instance_params2;
                        fn = fn.get_instance(instance_params, false, null);
                        return fn;
                    }
                }
            }
            return fn;
        }

        //ЯВНО
        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            compiled_type_node cctn = ctn as compiled_type_node;
            if (cctn == null)
            {
                //if (ctn is common_type_node && (ctn as common_type_node).IsEnum && this == SystemLibrary.SystemLibrary.integer_type)
                //    return TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(this, ctn, false);
                return null;
            }
            function_node fn = null;
            if (_explicit_convertions_to.TryGetValue(ctn, out fn))
                return fn;
            fn = NetHelper.NetHelper.get_explicit_conversion(this, this, cctn, scope);
            if (fn is compiled_function_node)
                _explicit_convertions_to.Add(ctn, fn);
            return fn;
        }

        //TODO: Доопределить.
        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            compiled_type_node cctn = ctn as compiled_type_node;
            if (cctn == null)
            {
                //if (ctn is common_type_node && (ctn as common_type_node).IsEnum && this == SystemLibrary.SystemLibrary.integer_type)
                //    return TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(ctn, this, false);
                return null;
            }
            function_node fn = null;
            if (_explicit_convertions_from.TryGetValue(ctn, out fn))
                return fn;
            if (SemanticRules.PoinerRealization == PoinerRealization.VoidStar)
                if ((this == SystemLibrary.SystemLibrary.integer_type && ctn == SystemLibrary.SystemLibrary.pointer_type) ||
                    (this == SystemLibrary.SystemLibrary.pointer_type && ctn == SystemLibrary.SystemLibrary.integer_type))
                {
                    fn = TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(ctn, this, false);
                    _explicit_convertions_from.Add(ctn, fn);
                    return fn;
                }

            //enum->int32, int32->enum
            //TODO переделать это. Наверно это делается както нетак. Enum Conversion
            if ((ctn.base_type == SystemLibrary.SystemLibrary.enum_base_type && this == SystemLibrary.SystemLibrary.integer_type)
              || (this.base_type == SystemLibrary.SystemLibrary.enum_base_type && ctn == SystemLibrary.SystemLibrary.integer_type))
            {
                fn = TreeConverter.convertion_data_and_alghoritms.get_empty_conversion(ctn, this, false);
                _explicit_convertions_from.Add(ctn, fn);
                return fn;
            }
            
            fn = NetHelper.NetHelper.get_explicit_conversion(this, cctn, this, scope);
            if (fn is compiled_function_node)
                _explicit_convertions_from.Add(ctn, fn);
            return fn;
        }

        public override bool is_value
        {
            get
            {
                return (_compiled_type.IsValueType);
            }
        }

        public override bool is_class
        {
            get
            {
                return _compiled_type.IsClass;
            }
            set
            {
                
            }
        }

        public override string PrintableName
        {
            get
            {
                if (_compiled_type.IsGenericType)
                {
                    Type[] tpars = _compiled_type.GetGenericArguments();
                    List<type_node> ctypes = new List<type_node>(tpars.Length);
                    foreach (Type par in tpars)
                    {
                        ctypes.Add(compiled_type_node.get_type_node(par));
                    }
                    return generic_convertions.MakePseudoInstanceName(name, ctypes, true);
                }
                if (!_compiled_type.IsPrimitive && base.PrintableName == _compiled_type.Name)
                    return _compiled_type.FullName;
                return base.PrintableName;
            }
        }

        public override string BaseFullName
        {
            get
            {
                Type t = _compiled_type;
                if (t.IsGenericType)
                    t = t.GetGenericTypeDefinition();
                return t.FullName;
            }
        }
    }

	[Serializable]
	public class simple_array : wrapped_type, SemanticTree.ISimpleArrayNode
	{
		private type_node _element_type;
		private int _length;
        private SymbolTable.Scope _scope;

        public simple_array()
        {
        }

        public simple_array(type_node element_type,int length)
        {            
            _element_type = element_type;
            _length = length;
            _scope = base_type.Scope;
        }

		public override type_node base_type
		{
			get
			{
				return SystemLibrary.SystemLibrary.array_base_type;
			}
		}

		public override type_node element_type
		{
			get
			{
				return _element_type;
			}
		}

		public int length
		{
			get
			{
				return _length;
			}
		}
		
		public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.basic;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.simple_array;
			}
		}

		public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
		{
            //return this.find_in_additional_names(name);
            return _scope.Find(name);
		}

		public override string name
		{
			get
			{
                if (base.name != null)
                {
                    return base.name;
                }
				return PascalABCCompiler.TreeConverter.compiler_string_consts.simple_array_name;
			}
		}
		
		public override string full_name {
			get { return name; }
		}
		
        public override SemanticTree.type_special_kind type_special_kind
        {
            get
            {
                return SemanticTree.type_special_kind.none_kind;
            }
        }

        //TODO: Доопределить.
        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            return null;
        }

        //TODO: Доопределить.
        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            return null;
        }

        //TODO: Доопределить.
        public override function_node get_implicit_conversion_from(type_node ctn)
        {
            return null;
        }

        //TODO: Доопределить.
        public override function_node get_implicit_conversion_to(type_node ctn)
        {
            return null;
        }

        //TODO: Подумать, что-бы это могло значить. Наверно здесь следует создавать функцию, которая при вычислении во время компиляции возвращает simple_array_indexing.
		public override property_node default_property_node
		{
			get
			{
				return null;
			}
		}

		public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
		{
			//return this.find_in_additional_names(name);
            return _scope.FindOnlyInType(name, null);
		}

        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            //return this.find_in_additional_names(name);
            return _scope.FindOnlyInType(name, CurrentScope);
        }
        
        public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.ITypeNode SemanticTree.ISimpleArrayNode.element_type
		{
			get
			{
				return _element_type;
			}
		}

		SemanticTree.ITypeNode SemanticTree.ITypeNode.base_type
		{
			get
			{
				return base_type;
			}
		}

        public override SymbolTable.Scope Scope
        {
            get
            {
                return _scope;
            }
        }

        public override bool is_value
        {
            get
            {
                return false;
            }
        }

        public override bool is_class
        {
            get
            {
                return false;
            }
            set
            {
               
            }
        }
	}

	[Serializable]
	public class simple_array_indexing : addressed_expression, SemanticTree.ISimpleArrayIndexingNode
	{
		private expression_node _simple_arr_expr;
		private expression_node _ind_expr;
		private expression_node[] _indices;
		
		public simple_array_indexing(expression_node simple_arr_expr,expression_node ind_expr,type_node element_type,location loc) :
			base(element_type,loc)
		{
			_simple_arr_expr=simple_arr_expr;
			_ind_expr=ind_expr;
		}
        
		public expression_node simple_arr_expr
		{
			get
			{
				return _simple_arr_expr;
			}
			set
			{
				_simple_arr_expr = value;
			}
		}
		
		public expression_node[] expr_indices
		{
			get
			{
				return _indices;
			}
			set
			{
				_indices = value;
			}
		}
		
		public expression_node ind_expr
		{
			get
			{
				return _ind_expr;
			}
			set
			{
				_ind_expr = value;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.simple_array_indexing;
			}
		}

		public SemanticTree.IExpressionNode array
		{
			get
			{
				return _simple_arr_expr;
			}
		}
		
		public SemanticTree.IExpressionNode[] indices
		{
			get
			{
				return _indices;
			}
		}
		
		public SemanticTree.IExpressionNode index
		{
			get
			{
				return _ind_expr;
			}
		}

	}

    [Serializable]
    public class null_type_node : wrapped_type
    {
        private static null_type_node typ;
		private SymbolTable.ClassScope _scope;
		
        private null_type_node()
        {
        	_scope = compilation_context.instance.convertion_data_and_alghoritms.symbol_table.CreateClassScope(null, null, name);
        }

        public static type_node get_type_node()
        {
            if (typ == null) 
            {
            	typ = new null_type_node();
            	SystemLibrary.SystemLibrary.init_reference_type(typ);
            }
            return typ;
        }
		
        public static void reset()
        {
        	typ = null;
        }
        
        public override function_node get_implicit_conversion_from(type_node ctn)
        {
            return null;
        }

        //TODO: Доопределить.
        public override function_node get_implicit_conversion_to(type_node ctn)
        {
            return null;
        }

        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            return null;
        }

        //TODO: Доопределить.
        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            return null;
        }

        public override property_node default_property_node
        {
            get
            {
                return null;
            }
        }

        public override type_node base_type
        {
            get
            {
                return null;
            }
        }

        public override SymbolTable.Scope Scope
        {
            get
            {
                return null;
            }
        }

        public override SemanticTree.node_kind node_kind
        {
            get
            {
                return SemanticTree.node_kind.common;
            }
        }

        public override void SetName(string name)
        {
            //this._name = name;
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.null_type_node;
            }
        }

        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }
		
		public override void add_name(string name, SymbolInfo si)
		{
			_scope.AddSymbol(name, si);
		}
		
        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return _scope.FindOnlyInScope(name);
        }
        
        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            return _scope.FindOnlyInScope(name);
        }

        public override bool is_value
        {
            get
            {
                return false;
            }
        }
        
        public override bool is_class
        {
            get
            {
                return true;
            }
            set
            {
              
            }
        }
        
		public override string name {
			get {
				return "nil";
			}
		}
        
		public override string full_name {
			get {
				return "nil";
			}
		}
    }

    public class convert_function_to_function_call
    {
        private expression_node _bfc;

        public convert_function_to_function_call(expression_node bfc)
        {
            _bfc = bfc;
        }

        public expression_node compile_time_executor(location call_location, params expression_node[] parameters)
        {
            return _bfc;
        }
    }


    //Псевдотип. Не идет в выходное дерево. Используется как промежуточный при работе с делегатами.
    [Serializable]
    public class delegated_methods : wrapped_type
    {

        private base_function_call_list _proper_methods = new base_function_call_list();

        public delegated_methods()
        {
        }

        public base_function_call_list proper_methods
        {
            get
            {
                return _proper_methods;
            }
        }

        public function_node[] empty_param_methods
        {
            get
            {
                List<function_node> funcs = new List<function_node>();
                foreach (base_function_call bfc in _proper_methods)
                {
                    if (bfc.simple_function_node.parameters.Count == 0 || bfc.simple_function_node.parameters.Count == 1 && (bfc.simple_function_node.parameters[0].default_value != null || bfc.simple_function_node.parameters[0].is_params))
                    {
                        funcs.Add(bfc.simple_function_node);
                    }
                    else if (bfc.function is common_namespace_function_node)
                    {
                        common_namespace_function_node cnfn = bfc.function as common_namespace_function_node;
                        if (cnfn.ConnectedToType != null && (bfc.simple_function_node.parameters.Count == 1 || bfc.simple_function_node.parameters.Count == 2 && (bfc.simple_function_node.parameters[1].is_params || bfc.simple_function_node.parameters[1].default_value != null)))
                        {
                            funcs.Add(bfc.simple_function_node);
                        }
                        if (cnfn.num_of_default_parameters == cnfn.parameters.Count)
                            funcs.Add(bfc.simple_function_node);
                    }
                    else if (bfc.function is compiled_function_node)
                    {
                        compiled_function_node cfn = bfc.function as compiled_function_node;
                        if (cfn.ConnectedToType != null && (bfc.simple_function_node.parameters.Count == 1 || bfc.simple_function_node.parameters.Count == 2 && (bfc.simple_function_node.parameters[1].is_params || bfc.simple_function_node.parameters[1].default_value != null)))
                            funcs.Add(bfc.simple_function_node);
                        if (cfn.num_of_default_parameters == cfn.parameters.Count)
                            funcs.Add(bfc.simple_function_node);
                    }
                }
                return funcs.ToArray();
            }
        }

        public base_function_call empty_param_method
        {
            get
            {
                foreach (base_function_call bfc in _proper_methods)
                {
                    if (bfc.simple_function_node.parameters.Count == 0 || bfc.simple_function_node.parameters.Count == 1 && (bfc.simple_function_node.parameters[0].default_value != null || bfc.simple_function_node.parameters[0].is_params))
                    {
                        return bfc;
                    }
                    else if (bfc.function is common_namespace_function_node)
                    {
                        common_namespace_function_node cnfn = bfc.function as common_namespace_function_node;
                        if (cnfn.ConnectedToType != null && (bfc.simple_function_node.parameters.Count == 1 || bfc.simple_function_node.parameters.Count == 2 && (bfc.simple_function_node.parameters[1].is_params || bfc.simple_function_node.parameters[1].default_value != null)))
                        {
                            return bfc;
                        }
                        if (cnfn.num_of_default_parameters == cnfn.parameters.Count)
                            return bfc;
                    }
                    else if (bfc.function is common_method_node)
                    {
                        common_method_node cmn = bfc.function as common_method_node;
                        if (cmn.num_of_default_parameters == cmn.parameters.Count)
                            return bfc;
                    }
                    else if (bfc.function is compiled_function_node)
                    {
                        compiled_function_node cfn = bfc.function as compiled_function_node;
                        if (cfn.ConnectedToType != null && (bfc.simple_function_node.parameters.Count == 1 || bfc.simple_function_node.parameters.Count == 2 && (bfc.simple_function_node.parameters[1].is_params || bfc.simple_function_node.parameters[1].default_value != null)))
                            return bfc;
                        if (cfn.num_of_default_parameters == cfn.parameters.Count)
                            return bfc;
                    }
                }
                return null;
            }
        }

        private base_function_call get_function_call_copy(base_function_call bfc)
        {
            return bfc.copy();
        }

        public override function_node get_implicit_conversion_to(type_node ctn)
        {
            foreach (base_function_call fn in _proper_methods)
            {
                if (fn.simple_function_node.parameters.Count == 0
                    || (fn.simple_function_node is common_namespace_function_node && (fn.simple_function_node as common_namespace_function_node).ConnectedToType != null 
                            || fn.simple_function_node is compiled_function_node && (fn.simple_function_node as compiled_function_node).ConnectedToType != null)
                    && fn.simple_function_node.parameters.Count == 1 && !fn.simple_function_node.parameters[0].is_params)
                {
                    if (fn.simple_function_node.return_value_type == ctn)
                    {
                        convert_function_to_function_call cftfc = new convert_function_to_function_call(fn);
                        return (new convert_types_function_node(cftfc.compile_time_executor, true));
                    }
                    if (fn.simple_function_node.return_value_type == null)
                    {
                        continue;
                    }
                    //TODO: Очень внимательно рассмотреть. Если преобразование типов должно идти через compile_time_executor.
                    possible_type_convertions ptc=type_table.get_convertions(fn.simple_function_node.return_value_type, ctn);
                    if ((ptc.first == null) || (ptc.first.convertion_method == null))
                    {
                        continue;
                    }
                    expression_node ennew=type_table.type_table_function_call_maker(ptc.first.convertion_method, null, fn);
                    convert_function_to_function_call cftfc2 = new convert_function_to_function_call(ennew);
                    return (new convert_types_function_node(cftfc2.compile_time_executor, false));
                }
                else if (fn.simple_function_node.parameters.Count == 1 && fn.simple_function_node.parameters[0].is_params ||
                    (fn.simple_function_node is common_namespace_function_node && (fn.simple_function_node as common_namespace_function_node).ConnectedToType != null || fn.simple_function_node is compiled_function_node && (fn.simple_function_node as compiled_function_node).ConnectedToType != null) 
                    && fn.simple_function_node.parameters.Count == 2 && fn.simple_function_node.parameters[1].is_params)
                {
                    base_function_call copy_fn = get_function_call_copy(fn);
                    int param_num = (fn.simple_function_node is common_namespace_function_node && (fn.simple_function_node as common_namespace_function_node).ConnectedToType != null || fn.simple_function_node is compiled_function_node && (fn.simple_function_node as compiled_function_node).ConnectedToType != null) ? 1 : 0;
                	if (fn.simple_function_node.return_value_type == ctn)
                    {
                		common_namespace_function_call cnfc = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.NewArrayProcedureDecl,null);
                        cnfc.parameters.AddElement(new typeof_operator(fn.simple_function_node.parameters[param_num].type, null));
                		cnfc.parameters.AddElement(new int_const_node(0,null));
                        if (copy_fn.parameters.Count < fn.simple_function_node.parameters.Count)
                            copy_fn.parameters.AddElement(cnfc);
                		convert_function_to_function_call cftfc = new convert_function_to_function_call(copy_fn);
                        return (new convert_types_function_node(cftfc.compile_time_executor, true));
                        
                    }
                    if (fn.simple_function_node.return_value_type == null)
                    {
                        continue;
                    }
                    //TODO: Очень внимательно рассмотреть. Если преобразование типов должно идти через compile_time_executor.
                    possible_type_convertions ptc=type_table.get_convertions(fn.simple_function_node.return_value_type, ctn);
                    if ((ptc.first == null) || (ptc.first.convertion_method == null))
                    {
                        continue;
                    }
                    common_namespace_function_call cnfc2 = new common_namespace_function_call(SystemLibrary.SystemLibInitializer.NewArrayProcedureDecl,null);
                    cnfc2.parameters.AddElement(new typeof_operator(fn.simple_function_node.parameters[param_num].type, null));
                	cnfc2.parameters.AddElement(new int_const_node(0,null));
                    if (copy_fn.parameters.Count < fn.simple_function_node.parameters.Count)
                        copy_fn.parameters.AddElement(cnfc2);
                    expression_node ennew=type_table.type_table_function_call_maker(ptc.first.convertion_method, null, copy_fn);
                    convert_function_to_function_call cftfc2 = new convert_function_to_function_call(ennew);
                    return (new convert_types_function_node(cftfc2.compile_time_executor, false));
                }
                else if (fn.simple_function_node.parameters.Count == 1 && fn.simple_function_node.parameters[0].default_value != null ||
                    (fn.simple_function_node is common_namespace_function_node && (fn.simple_function_node as common_namespace_function_node).ConnectedToType != null 
                    || fn.simple_function_node is compiled_function_node && (fn.simple_function_node as compiled_function_node).ConnectedToType != null)
                    && fn.simple_function_node.parameters.Count == 2 && fn.simple_function_node.parameters[1].default_value != null)
                {
                    int param_num = (fn.simple_function_node is common_namespace_function_node && (fn.simple_function_node as common_namespace_function_node).ConnectedToType != null || fn.simple_function_node is compiled_function_node && (fn.simple_function_node as compiled_function_node).ConnectedToType != null) ? 1 : 0;
                    base_function_call copy_fn = null;
                    if (fn.simple_function_node.return_value_type == ctn)
                    {
                        copy_fn = get_function_call_copy(fn);
                        copy_fn.parameters.AddElement(fn.simple_function_node.parameters[param_num].default_value);
                        convert_function_to_function_call cftfc = new convert_function_to_function_call(copy_fn);
                        return (new convert_types_function_node(cftfc.compile_time_executor, true));

                    }
                    if (fn.simple_function_node.return_value_type == null)
                    {
                        continue;
                    }
                    //TODO: Очень внимательно рассмотреть. Если преобразование типов должно идти через compile_time_executor.
                    possible_type_convertions ptc = type_table.get_convertions(fn.simple_function_node.return_value_type, ctn);
                    if ((ptc.first == null) || (ptc.first.convertion_method == null))
                    {
                        continue;
                    }
                    copy_fn = get_function_call_copy(fn);
                    copy_fn.parameters.AddElement(fn.simple_function_node.parameters[param_num].default_value);
                    expression_node ennew = type_table.type_table_function_call_maker(ptc.first.convertion_method, null, copy_fn);
                    convert_function_to_function_call cftfc2 = new convert_function_to_function_call(ennew);
                    return (new convert_types_function_node(cftfc2.compile_time_executor, false));
                }
                else if (fn.simple_function_node.parameters.Count == fn.simple_function_node.num_of_default_parameters)
                {
                    base_function_call copy_fn = get_function_call_copy(fn);
                    if (fn.simple_function_node.return_value_type == ctn)
                    {
                        foreach (parameter p in fn.simple_function_node.parameters)
                            copy_fn.parameters.AddElement(p.default_value);
                        convert_function_to_function_call cftfc = new convert_function_to_function_call(copy_fn);
                        return (new convert_types_function_node(cftfc.compile_time_executor, true));
                    }
                    if (fn.simple_function_node.return_value_type == null)
                    {
                        continue;
                    }
                    //TODO: Очень внимательно рассмотреть. Если преобразование типов должно идти через compile_time_executor.
                    possible_type_convertions ptc = type_table.get_convertions(fn.simple_function_node.return_value_type, ctn);
                    if ((ptc.first == null) || (ptc.first.convertion_method == null))
                    {
                        continue;
                    }
                    foreach (parameter p in fn.simple_function_node.parameters)
                        copy_fn.parameters.AddElement(p.default_value);
                    expression_node ennew = type_table.type_table_function_call_maker(ptc.first.convertion_method, null, copy_fn);
                    convert_function_to_function_call cftfc2 = new convert_function_to_function_call(ennew);
                    return (new convert_types_function_node(cftfc2.compile_time_executor, false));
                }
            }

            internal_interface ii = ctn.get_internal_interface(internal_interface_kind.delegate_interface);
            if (ii == null)
            {
            	if (ctn is compiled_type_node && (ctn as compiled_type_node).IsDelegate)
            	{
            		common_type_node del =
            			type_constructor.instance.create_delegate(compilation_context.instance.get_delegate_type_name(), this.proper_methods[0].simple_function_node.return_value_type, this.proper_methods[0].simple_function_node.parameters, compilation_context.instance.converted_namespace, null);
            		compilation_context.instance.converted_namespace.types.AddElement(del);
            		ii = del.get_internal_interface(internal_interface_kind.delegate_interface);
            	}
            	else
            	return null;
            }

            delegate_internal_interface dii = (delegate_internal_interface)ii;
			
            base_function_call _finded_call = null;

            foreach(base_function_call bfn in _proper_methods)
            {
                if (bfn.simple_function_node.parameters.Count != dii.parameters.Count)
                {
                    continue;
                }
                if (bfn.simple_function_node.return_value_type != dii.return_value_type)
                {
                    if (!type_table.is_derived(dii.return_value_type, bfn.simple_function_node.return_value_type)) // SSM 21/05/15
                        if (!(bfn.ret_type is lambda_any_type_node)) //lroman//
                            continue;
                }
                bool find_eq = true;
                int i=0;
                while ((find_eq == true) && (i<bfn.simple_function_node.parameters.Count))
                {
                    /*var a1 = bfn.simple_function_node.parameters[i].type != dii.parameters[i].type;
                    var a2 = bfn.simple_function_node.parameters[i].parameter_type != dii.parameters[i].parameter_type;
                    var a3 = bfn.simple_function_node.parameters[i].is_params != dii.parameters[i].is_params;*/
                    //lroman// Добавил все, что связано с lambda_any_type_node. Считаем, что этот тип равен всем типам. Это используется при выводе параметров лямбды
                    if ((bfn.simple_function_node.parameters[i].type != dii.parameters[i].type && !(bfn.simple_function_node.parameters[i].type is lambda_any_type_node)) ||
                        (bfn.simple_function_node.parameters[i].parameter_type != dii.parameters[i].parameter_type && !(bfn.simple_function_node.parameters[i].parameter_type is lambda_any_type_node)) 
                	    || bfn.simple_function_node.parameters[i].is_params != dii.parameters[i].is_params)
                    {
                        find_eq = false;
                    }
                    i++;
                }
                if (find_eq)
                {
                    _finded_call = bfn;
                    break;
                }
            }

            //Вообщето если мы можем найти более одного вызова функции в цикле выше, то это внутренняя ошибка компилятора, но мы не будем ругаться.
            if (_finded_call == null)
            {
                return null;
            }

            if ((_finded_call is common_constructor_call) || (_finded_call is compiled_constructor_call))
            {
                return null;
            }

            common_method_node cmn = dii.constructor as common_method_node;
            if (cmn != null)
            {
                common_constructor_call ccc = new common_constructor_call(cmn, null);
                ccc.parameters.AddElement(_finded_call);
                convert_function_to_function_call cftfc = new convert_function_to_function_call(ccc);
                return (new convert_types_function_node(cftfc.compile_time_executor, false));
            }

            compiled_constructor_node ccn = dii.constructor as compiled_constructor_node;
            if (ccn != null)
            {
                compiled_constructor_call ccc = new compiled_constructor_call(ccn, null);
                ccc.parameters.AddElement(_finded_call);
                convert_function_to_function_call cftfc = new convert_function_to_function_call(ccc);
                return (new convert_types_function_node(cftfc.compile_time_executor, false));
            }

            //Тут вообщето тоже внутренняя ошибка компилятора. (Не откомпилированный и не обычный конструктор).

            return null;
        }

        public override function_node get_implicit_conversion_from(type_node ctn)
        {
            return null;
        }

        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            return null;
        }

        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            return null;
        }

        public override type_node base_type
        {
            get
            {
                return null;
            }
        }

        public override property_node default_property_node
        {
            get
            {
                return null;
            }
        }

        public override SemanticTree.node_kind node_kind
        {
            get
            {
                return SemanticTree.node_kind.basic;
            }
        }
		
        private string _name=null;
        private string make_name()
        {
        	if (_proper_methods.Count == 0) return compiler_string_consts.method_group_type_name;
        	base_function_call bfc = _proper_methods[0];
        	System.Text.StringBuilder sb = new System.Text.StringBuilder();
        	if (bfc.function.return_value_type == null)
        		sb.Append("procedure");
        	else
        		sb.Append("function");
        	if (bfc.function.parameters != null && bfc.function.parameters.Length > 0)
        	{
        		sb.Append('(');
        		for (int i=0; i<bfc.function.parameters.Length; i++)
        		{
        			SemanticTree.IParameterNode prm = bfc.function.parameters[i];
        			switch (prm.parameter_type)
        			{
        				case SemanticTree.parameter_type.var : sb.Append("var "); break;
        			}
        			if (prm.is_params) sb.Append("params ");
        			sb.Append(prm.name+": ");
        			sb.Append(prm.type.name);
        			if (i<bfc.function.parameters.Length-1)
        			sb.Append(';');
        		}
        		sb.Append(')');
        	}
        	if (bfc.function.return_value_type != null)
        		sb.Append(": "+bfc.function.return_value_type.name);
        	return sb.ToString();
        }
        
        public override string name
        {
            get
            {
            	if (_name == null) _name = make_name();
            	return _name;
            	//return compiler_string_consts.method_group_type_name;
            }
        }

        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
            return find_in_type(name);
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
        	if (name != compiler_string_consts.plusassign_name && name != compiler_string_consts.minusassign_name)
        	    foreach (base_function_call fn in _proper_methods)
                {
                    if (fn.simple_function_node.parameters.Count == 0)
                    {
                        if (fn.simple_function_node.return_value_type != null)
                        {
                            return fn.simple_function_node.return_value_type.find_in_type(name);
                        }
                    }
                }
            return compiled_type_node.get_type_node(typeof(Delegate)).find(name);
            //return null;
        }
        public override List<SymbolInfo> find_in_type(string name, SymbolTable.Scope CurrentScope, bool no_search_in_extension_methods = false)
        {
            return find_in_type(name);
        }

        public override bool is_class
        {
            get
            {
                return false;
            }
            set
            {
               
            }
        }

        public override SymbolTable.Scope Scope
        {
            get
            {
                return null;
            }
        }

        public override bool is_value
        {
            get
            {
                return false;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.delegated_method;
            }
        }
    }

    /*public class typed_file : common_type_node
    {

        public typed_file(common_type_node _associated_type, type_node _element_type, location loc)
            : base(_associated_type, _associated_type.name, _associated_type.type_access_level, _associated_type.comprehensive_namespace, _associated_type.scope, loc)
        {
            this._associated_type = _associated_type;
            this.element_type = _element_type;
            type_special_kind = SemanticTree.type_special_kind.typed_file;
        }

        public override SemanticTree.type_special_kind type_special_kind
        {
            get
            {
                return SemanticTree.type_special_kind.typed_file;
            }
        }

        public override string name
        {
            get { return compiler_string_consts.GetTypedFileTypeName(element_type.name); }
        }

        

        public override bool is_value
        {
            get { return _associated_type.is_value; }
        }

        public override bool is_class
        {
            get
            {
                return _associated_type.is_class;
            }
            set
            {
                
            }
        }

        //public override semantic_node_type semantic_node_type
        //{
        //    get { return semantic_node_type.none; }
        //}


    }
    */

    /*
    [Serializable]
    public class unsized_array : wrapped_type, SemanticTree.IUnsizedArray
    {

        private type_node _element_type;
        private type_node _base_type;
        private NetHelper.NetTypeScope _scope;

        public unsized_array(type_node element_type)
        {
            _element_type = element_type;
        }

        public type_node internal_element_type
        {
            get
            {
                return _element_type;
            }
            set
            {
                _element_type = value;
            }
        }

        public type_node internal_base_type
        {
            get
            {
                return _base_type;
            }
            set
            {
                _base_type = value;
            }
        }

        public NetHelper.NetTypeScope internal_scope
        {
            get
            {
                return _scope;
            }
            set
            {
                _scope = value;
            }
        }

        public override function_node get_implicit_conversion_to(type_node ctn)
        {
            return null;
        }

        public override function_node get_implicit_conversion_from(type_node ctn)
        {
            return null;
        }

        public override function_node get_explicit_conversion_to(type_node ctn)
        {
            return null;
        }

        public override function_node get_explicit_conversion_from(type_node ctn)
        {
            return null;
        }

        public override type_node base_type
        {
            get
            {
                return _base_type;
            }
        }

        public override property_node default_property_node
        {
            get
            {
                return null;
            }
        }

        public override SemanticTree.node_kind node_kind
        {
            get
            {
                return SemanticTree.node_kind.basic;
            }
        }

        public override SymbolInfo find(string name)
        {
            return _scope.Find(name);
        }

        public override SymbolInfo find_in_type(string name)
        {
            return _scope.Find(name);
        }

        public override SymbolTable.Scope Scope
        {
            get
            {
                return null;
            }
        }

        public override bool is_value
        {
            get
            {
                return true;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.unsized_array;
            }
        }

        public SemanticTree.ITypeNode element_type
        {
            get
            {
                return _element_type;
            }
        }
    }
    */

    public class undefined_type : type_node
    {
        private string _name;
        private location _loc;
        public undefined_type(string name, location loc)
        {
            _name = name;
            _loc = loc;
        }

        public location loc
        {
            get
            {
                return _loc;
            }
        }


        public override type_node base_type
        {
            get { return null; }
        }

        public override string name
        {
            get { return _name; }
        }
		
		public override string full_name {
			get { return name; }
		}
        
        public override property_node default_property_node
        {
            get { return null; }
        }

        public override SemanticTree.node_kind node_kind
        {
            get { return SemanticTree.node_kind.basic; }
        }

        public override List<SymbolInfo> find(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override SymbolInfo find_first_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override List<SymbolInfo> find_in_type(string name, bool no_search_in_extension_methods = false)
        {
            return null;
        }

        public override void add_name(string name, SymbolInfo si)
        {

        }
        public override void add_generated_name(string name, SymbolInfo si)
        {

        }
        public override void  clear_generated_names()
        {

        }

        public override void SetName(string name)
        {
            _name = name;
        }

        public override SymbolTable.Scope Scope
        {
            get { return null; }
        }

        public override bool is_value
        {
            get { return false; }
        }

        public override bool is_class
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public override SemanticTree.type_special_kind type_special_kind
        {
            get
            {
                return SemanticTree.type_special_kind.none_kind;
            }
            set
            {
                
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get { return semantic_node_type.none; }
        }
    }

    public class ArrayConstType : undefined_type
    {
        private int _length;
        private type_node _element_type;
        public ArrayConstType(type_node element_type, int length, location loc)
            :base(compiler_string_consts.array_const_type_name, loc)
        {
            _length = length;
            _element_type = element_type;
        }

        public override type_node element_type
        {
            get
            {
                return _element_type;
            }
            set
            {
                _element_type = value;
            }
        }
    }

    public class RecordConstType : undefined_type
    {
        public RecordConstType(location loc)
            : base(compiler_string_consts.record_const_type_name, loc)
        {
        }
    }

    // тип, который объявляется как auto и определяется при компиляции в момент первого присваивания
    // На 04.07.16 нужен только для генерации кода yield
    public class auto_type : undefined_type 
    {
        //public type_node real_type = null;
        public auto_type(location loc)
            : base(compiler_string_consts.auto_type_name, loc) { }
    }

    // тип, который объявляется как ienumerable_auto и определяется при компиляции в момент первого присваивания
    // На 04.07.16 нужен только для генерации кода yield
    public class ienumerable_auto_type : undefined_type 
    {
        //public type_node real_type = null;
        public ienumerable_auto_type(location loc)
            : base(compiler_string_consts.ienumerable_auto_type_name, loc) { }
    }

}
