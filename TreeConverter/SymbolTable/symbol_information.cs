// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.TreeConverter
{

	public enum access_level {al_none, al_private, al_protected, al_public, al_internal};
	
	public enum symbol_kind {sk_none, sk_overload_function, sk_overload_procedure, sk_indefinite};

	public abstract class base_scope
	{
		public abstract SymbolInfo find(string name);

		public abstract base_scope top_scope
		{
			get;
		}
	}

	public class BaseScope
	{
		//public virtual SymbolInfo Find(string name)
		//{
		//	return null;
		//}
	}

	//Тип записи в таблице символов.
	/*public enum name_information_type 
	{
		nit_base_type, //Пока нигде не используется.
		nit_common_type, //Помещается в Сашину таблицу символов в тот момент, когда TreeConverter встречает определение типа.
		nit_compiled_type, //Информация, об откомпилированном типе. Находится Ваней в сборках.
		nit_basic_function, //Информация о базовом методе. Создается Колей при инициализации модуля System.
		nit_common_namespace_function, //Функция, определенная в пространстве имен. Помещается Колей в Сашину таблицу символов.
		nit_common_in_function_function, //Функция, определенная в другой функции. Помещается Колей в Сашину таблицу символов.
		nit_common_method, //Метод класса. Помещается Колей в Сашину таюлицу символов.
		nit_compiled_function, //Метод откомпилированного класса. Находится Ванией в сборках.
		nit_common_namespace, //Пространство имен. Пока нигде не добавляется в таблицу символов.
		nit_unit, //Помещается Колей в Сашину таблицу символов.
		nit_local_variable, //Помещается Колей в Сашину таблицу символов.
		nit_namespace_variable, //Помещается Колей в Сашину таблицу символов.
		nit_class_field,  //Помещается Колей в Сашину таблицу символов.
		nit_common_parameter,  //Помещается Колей в Сашину таблицу символов.
		nit_basic_parameter, //Помещается в таблицу символов при инициализации модуля System.
		nit_compiled_parameter, //Не связано с таблицей символов. О этих параметрах знают только откомпилированный функции.
		nit_constant_defnition, //Помещается Колей в Сашину таблицу символов.
		nit_common_property, //Помещается Колей в Сашину таблицу символов.
		nit_compiled_property, //Свойство откомпилированного класса. Находится Ванией в сборках.
		nit_basic_property, //Пока нигде не используется.
		nit_compiled_variable, //Поле откомпилированного класса. Находится Ванией в сборках.
		nit_compiled_namespace
	};*/

    public class PCUSymbolInfo : SymbolInfo
    {
        private semantic_node_type _semantic_node_type;

        private bool _virtual_slot;

        public semantic_node_type semantic_node_type
        {
            get
            {
                return _semantic_node_type;
            }
            set
            {
                _semantic_node_type = value;
            }
        }

        public bool virtual_slot
        {
            get
            {
                return _virtual_slot;
            }
            set
            {
                _virtual_slot = value;
            }
        }
    }

	public class SymbolInfo
	{
		//private readonly name_information_type _name_information_type;
		private definition_node _sym_info;

        

		private access_level _access_level;

		private symbol_kind _symbol_kind;

		public SymbolInfo Next;

		public SymbolTable.Scope scope;

        
		public access_level access_level
		{
			get
			{
				return _access_level;
			}
            set
            {
                _access_level = value;
            }

		}

		public symbol_kind symbol_kind
		{
			get
			{
				return _symbol_kind;
			}
			set
			{
				_symbol_kind=value;
			}
		}

		/*public name_information_type name_information_type
		{
			get
			{
				return _name_information_type;
			}
		}*/

		public definition_node sym_info
		{
			get
			{
				return _sym_info;
			}
			set
			{
				_sym_info=value;
			}
		}

		public SymbolInfo()
		{
		}

        public SymbolInfo copy()
        {
            SymbolInfo si = new SymbolInfo();
            si._access_level = this.access_level;
            si._sym_info = this._sym_info;
            si._symbol_kind = this._symbol_kind;
            si.scope = this.scope;
            si.Next = this.Next;
            return si;
        }
        private symbol_kind get_function_kind(function_node fn,bool is_overload)
        {
            symbol_kind sk;
            if (is_overload)
            {
                if (fn.return_value_type == null)
                {
                    sk = symbol_kind.sk_overload_procedure;
                }
                else
                {
                    sk = symbol_kind.sk_overload_function;
                }
            }
            else
            {
                sk = symbol_kind.sk_none;
            }
            return sk;
        }

		private symbol_kind get_function_kind(function_node fn)
		{
            common_function_node cfn = fn as common_function_node;
            if (cfn != null)
            {
                return get_function_kind(cfn, cfn.is_overload);
            }
            basic_function_node bfn = fn as basic_function_node;
            if (bfn != null)
            {
                return get_function_kind(bfn, bfn.is_overload);
            }
            return symbol_kind.sk_none;
		}

		private access_level get_class_member_access_level(SemanticTree.IClassMemberNode icmn)
		{
			access_level al;
			switch (icmn.field_access_level)
			{
				case SemanticTree.field_access_level.fal_public: al=access_level.al_public;break;
				case SemanticTree.field_access_level.fal_protected: al=access_level.al_protected;break;
				case SemanticTree.field_access_level.fal_private: al=access_level.al_private;break;
                case SemanticTree.field_access_level.fal_internal: al = access_level.al_internal; break;
				default:
					al=access_level.al_private;break;
			}
			return al;
		}

        public SymbolInfo(template_class tc)
        {
            _sym_info = tc;
            _access_level = access_level.al_public;
            _symbol_kind = symbol_kind.sk_none;
        }

        public SymbolInfo(label_node lab)
        {
            _sym_info = lab;
            _access_level = access_level.al_public;
            _symbol_kind = symbol_kind.sk_none;
        }

        public SymbolInfo(compiled_event ce)
        {
            _sym_info = ce;
            _access_level = access_level.al_public;
            _symbol_kind = symbol_kind.sk_none;
        }

		public SymbolInfo(compiled_type_node value)
		{
			//_name_information_type=name_information_type.nit_compiled_type;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}
		
		public SymbolInfo(common_event value)
		{
			_sym_info = value;
			_access_level = get_class_member_access_level(value);
			_symbol_kind = symbol_kind.sk_none;
		}

        public SymbolInfo(common_namespace_event value)
        {
            _sym_info = value;
            _access_level = access_level.al_public;
            _symbol_kind = symbol_kind.sk_none;
        }

        public SymbolInfo(function_node value)
        {
            _sym_info = value;
            _access_level = access_level.al_public;
            _symbol_kind = symbol_kind.sk_none;
        }

		public SymbolInfo(compiled_function_node value)
		{
			//_name_information_type=name_information_type.nit_compiled_function;
			_sym_info=value;
			_access_level=get_class_member_access_level(value);
			_symbol_kind=get_function_kind(value);
		}

		public SymbolInfo(compiled_constructor_node value)
		{
			//_name_information_type=name_information_type.nit_compiled_function;
			_sym_info=value;
			_access_level=get_class_member_access_level(value);
			_symbol_kind=get_function_kind(value);
		}

		public SymbolInfo(compiled_property_node value)
		{
			//_name_information_type=name_information_type.nit_compiled_property;
			_sym_info=value;
			_access_level=get_class_member_access_level(value);
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(compiled_variable_definition value)
		{
			//_name_information_type=name_information_type.nit_compiled_variable;
			_sym_info=value;
			_access_level=get_class_member_access_level(value);
			_symbol_kind=symbol_kind.sk_none;
		}

        public SymbolInfo(compiled_class_constant_definition value)
        {
            _sym_info = value;
            _access_level = get_class_member_access_level(value);
            _symbol_kind = symbol_kind.sk_none;
        }

		public SymbolInfo(compiled_namespace_node value)
		{
			//_name_information_type=name_information_type.nit_compiled_namespace;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(common_type_node value)
		{
			//_name_information_type=name_information_type.nit_common_type;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(basic_function_node value)
		{
			//_name_information_type=name_information_type.nit_basic_function;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=get_function_kind(value);
		}

		public SymbolInfo(common_namespace_function_node value)
		{
			//_name_information_type=name_information_type.nit_common_namespace_function;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=get_function_kind(value);
		}

		public SymbolInfo(common_in_function_function_node value)
		{
			//_name_information_type=name_information_type.nit_common_in_function_function;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=get_function_kind(value);
		}

		public SymbolInfo(common_method_node value)
		{
			//_name_information_type=name_information_type.nit_common_method;
			_sym_info=value;
			_access_level=get_class_member_access_level(value);
			_symbol_kind=get_function_kind(value);
		}

		public SymbolInfo(common_namespace_node value)
		{
			//_name_information_type=name_information_type.nit_common_namespace;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(unit_node value)
		{
			//_name_information_type=name_information_type.nit_unit;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(local_variable value)
		{
			//_name_information_type=name_information_type.nit_local_variable;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

        public SymbolInfo(local_block_variable value)
        {
            //_name_information_type=name_information_type.nit_local_variable;
            _sym_info = value;
            _access_level = access_level.al_public;
            _symbol_kind = symbol_kind.sk_none;
        }

        
        public SymbolInfo(namespace_variable value)
		{
			//_name_information_type=name_information_type.nit_namespace_variable;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(class_field value)
		{
			//_name_information_type=name_information_type.nit_class_field;
			_sym_info=value;
			_access_level=get_class_member_access_level(value);
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(common_parameter value)
		{
			//_name_information_type=name_information_type.nit_common_parameter;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(basic_parameter value)
		{
			//_name_information_type=name_information_type.nit_basic_parameter;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(constant_definition_node value)
		{
			//_name_information_type=name_information_type.nit_constant_defnition;
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(common_property_node value)
		{
			//_name_information_type=name_information_type.nit_common_property;
			_sym_info=value;
			_access_level=get_class_member_access_level(value);
			_symbol_kind=symbol_kind.sk_none;
		}

		public SymbolInfo(type_node value)
		{
			_sym_info=value;
			_access_level=access_level.al_public;
			_symbol_kind=symbol_kind.sk_none;
		}

        public SymbolInfo(definition_node value, access_level alevel, symbol_kind skind)
        {
            _sym_info = value;
            _access_level = alevel;
            _symbol_kind = skind;
        }
	}

}
