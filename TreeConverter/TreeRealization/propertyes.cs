// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.TreeRealization
{
    /// <summary>
    /// Базовый класс для свойств классов.
    /// </summary>
	[Serializable]
	public abstract class property_node : definition_node, SemanticTree.IPropertyNode
	{
        /// <summary>
        /// Список параметров свойства.
        /// </summary>
        private parameter_list _parametres = new parameter_list();

        /// <summary>
        /// Вид объекта.
        /// </summary>
		public abstract SemanticTree.node_kind node_kind
		{
			get;
		}

        /// <summary>
        /// Имя свойства.
        /// </summary>
		public abstract string name
		{
			get;
		}

        /// <summary>
        /// Тип, который содержит это свойство.
        /// </summary>
		public abstract type_node comprehensive_type
		{
			get;
		}

        /// <summary>
        /// Тип свойства.
        /// </summary>
		public abstract type_node property_type
		{
			get;
		}

        /// <summary>
        /// Уровень доступа к свойству.
        /// </summary>
		public abstract SemanticTree.field_access_level field_access_level
		{
			get;
		}

        /// <summary>
        /// Тип свойства (виртуальное, статическое или обычное).
        /// </summary>
		public virtual SemanticTree.polymorphic_state polymorphic_state
		{
			get
			{
				return SemanticTree.polymorphic_state.ps_common;
			}
			set
			{
				
			}
		}

        /// <summary>
        /// Список параметров свойства.
        /// </summary>
		public parameter_list parameters
		{
			get
			{
				return _parametres;
			}
		}

        /// <summary>
        /// Функция, которая возвращает значение свойства.
        /// </summary>
		public abstract function_node get_function
		{
			get;
		}

		/// <summary>
        /// Функция, которая устанавливает значение свойства.
		/// </summary>
		public abstract function_node set_function
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
				return general_node_type.property_node;
			}
		}

		SemanticTree.IParameterNode[] SemanticTree.IPropertyNode.parameters
		{
			get
			{
				return (_parametres.ToArray());
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

		SemanticTree.IFunctionNode SemanticTree.IPropertyNode.get_function
		{
			get
			{
				return this.get_function;
			}
		}

		SemanticTree.IFunctionNode SemanticTree.IPropertyNode.set_function
		{
			get
			{
				return this.set_function;
			}
		}

		SemanticTree.ITypeNode SemanticTree.IPropertyNode.property_type
		{
			get
			{
				return this.property_type;
			}
		}

		SemanticTree.ITypeNode SemanticTree.IPropertyNode.comprehensive_type
		{
            get
            {
                return this.comprehensive_type;
            }
		}
    }

    /// <summary>
    /// Свойство, определенное в обычном классе.
    /// </summary>
	[Serializable]
	public class common_property_node : property_node, SemanticTree.ICommonPropertyNode
	{
        /// <summary>
        /// Тип, в котором определено свойство.
        /// </summary>
		private common_type_node _comprehensive_type;

        /// <summary>
        /// Тип свойства.
        /// </summary>
		private type_node _property_type;

        /// <summary>
        /// Акцессор get свойства.
        /// </summary>
		private function_node _get_function;

        /// <summary>
        /// Акцессор set свойства.
        /// </summary>
		private function_node _set_function;

        /// <summary>
        /// Имя свойства.
        /// </summary>
		private string _name;

        /// <summary>
        /// Уровень доступа к свойству.
        /// </summary>
		private SemanticTree.field_access_level _field_access_level;

        /// <summary>
        /// Тип свойства (виртуальное, статическое или обычное).
        /// </summary>
		private SemanticTree.polymorphic_state _polymorphic_state;

        /// <summary>
        /// Расположение свойства.
        /// </summary>
		private location _loc;

        public common_property_node(string name, common_type_node comprehensive_type, location loc,
            SemanticTree.field_access_level field_access_level, SemanticTree.polymorphic_state polymorphic_state)
        {
            _name = name;
            _comprehensive_type = comprehensive_type;
            _loc = loc;
            _field_access_level = field_access_level;
            _polymorphic_state = polymorphic_state;
        }

		public common_property_node(string name,common_type_node comprehensive_type,type_node property_type,
			function_node get_function,function_node set_function,location loc,
            SemanticTree.field_access_level field_access_level, SemanticTree.polymorphic_state polymorphic_state)
		{
			_name=name;
			_comprehensive_type=comprehensive_type;
			_property_type=property_type;
			_get_function=get_function;
			_set_function=set_function;
            _loc = loc;
            _field_access_level = field_access_level;
            _polymorphic_state = polymorphic_state;
		}

        /// <summary>
        /// Расположение свойства.
        /// </summary>
		public location loc
		{
			get
			{
				return _loc;
			}
		}

        /// <summary>
        /// Расположение свойства.
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
        /// Имя свойства.
        /// </summary>
		public override string name
		{
			get
			{
				return _name;
			}
		}

        /// <summary>
        /// Уровень доступа к свойству.
        /// </summary>
		public override SemanticTree.field_access_level field_access_level
		{
			get
			{
				return _field_access_level;
			}
		}

        /// <summary>
        /// Тип, в котором определено свойство.
        /// </summary>
		public override type_node comprehensive_type
		{
			get
			{
                return common_comprehensive_type;
			}
		}

        /// <summary>
        /// Тип, который содержит это свойство.
        /// </summary>
		public common_type_node common_comprehensive_type
		{
			get
			{
				return _comprehensive_type;
			}
		}

        public type_node internal_property_type
        {
            get
            {
                return _property_type;
            }
            set
            {
                _property_type = value;
            }
        }

        /// <summary>
        /// Тип свойства.
        /// </summary>
		public override type_node property_type
		{
			get
			{
				return _property_type;
			}
		}

        public function_node internal_get_function
        {
            get
            {
                return _get_function;
            }
            set
            {
                _get_function = value;
            }
        }

        /// <summary>
        /// Акцессор get свойства.
        /// </summary>
		public override function_node get_function
		{
			get
			{
				return _get_function;
			}
		}

        public function_node internal_set_function
        {
            get
            {
                return _set_function;
            }
            set
            {
                _set_function = value;
            }
        }

        /// <summary>
        /// Акцессор set свойства.
        /// </summary>
		public override function_node set_function
		{
			get
			{
				return _set_function;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_property_node;
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

        //SemanticTree.ICommonTypeNode SemanticTree.ICommonPropertyNode.common_comprehensive_type
        //{
        //    get
        //    {
        //        return _comprehensive_type;
        //    }
        //}

		SemanticTree.ICommonTypeNode SemanticTree.ICommonClassMemberNode.common_comprehensive_type
		{
			get
			{
				return _comprehensive_type;
			}
		}

		SemanticTree.ITypeNode SemanticTree.IClassMemberNode.comperehensive_type
		{
			get
			{
				return _comprehensive_type;
			}
		}

		SemanticTree.field_access_level SemanticTree.IClassMemberNode.field_access_level
		{
			get
			{
				return _field_access_level;
			}
		}

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

        public override SemanticTree.node_kind node_kind
        {
            get
            {
                return SemanticTree.node_kind.common;
            }
        }
    }

    /// <summary>
    /// Свойство откомпилированного класса.
    /// </summary>
	[Serializable]
	public class compiled_property_node : property_node, SemanticTree.ICompiledPropertyNode
	{
        /// <summary>
        /// Оборачиваемое свойство.
        /// </summary>
		private System.Reflection.PropertyInfo _pi;

        //TODO: Поговорить с Ваней на тему того, чтобы разместить храилища функций, свойств и т.д. в этих классах. Тогда конструкторы можно будет сделать закрытыми.
        /// <summary>
        /// Конструктор класса.
        /// </summary>
        /// <param name="prop_info">Оборачиваемое свойство.</param>
		public compiled_property_node(System.Reflection.PropertyInfo prop_info)
		{
			_pi=prop_info;
            System.Reflection.ParameterInfo[] pas = _pi.GetIndexParameters();
            if ((pas != null) && (pas.Length > 0))
            {
                foreach (System.Reflection.ParameterInfo parinfo in pas)
                {
                    parameters.AddElement(new compiled_parameter(parinfo));
                }
            }
		}

        //TODO: Закрыть бы его от любопытных и оставить только для интерфейсов.
        /// <summary>
        /// Оборачиваемое свойство.
        /// </summary>
		public System.Reflection.PropertyInfo prop_info
		{
			get
			{
				return _pi;
			}
		}

        /// <summary>
        /// Имя свойства.
        /// </summary>
		public override string name
		{
			get
			{
				return _pi.Name;
			}
		}

        /// <summary>
        /// Тип, который содержит это свойство.
        /// </summary>
		public override type_node comprehensive_type
		{
			get
			{
				return compiled_comprehensive_type;
			}
		}

        /// <summary>
        /// Тип, который содержит это свойство.
        /// </summary>
		public compiled_type_node compiled_comprehensive_type
		{
			get
			{
				return compiled_type_node.get_type_node(_pi.DeclaringType);
			}
		}

        /// <summary>
        /// Тип свойства.
        /// </summary>
		public override type_node property_type
		{
			get
			{
				return compiled_type_node.get_type_node(_pi.PropertyType);
			}
		}

        /// <summary>
        /// Акцессор get.
        /// </summary>
        public compiled_function_node compiled_get_accessor
        {
            get
            {
                System.Reflection.MethodInfo mi=PascalABCCompiler.NetHelper.NetHelper.GetReadAccessor(_pi);
				if (mi==null)
				{
					return null;
				}
                return compiled_function_node.get_compiled_method(mi);
            }
        }

        /// <summary>
        /// Акцессор get.
        /// </summary>
		public override function_node get_function
		{
			get
			{
				return compiled_get_accessor;
			}
		}

        public compiled_function_node compiled_set_accessor
        {
            get
            {
                System.Reflection.MethodInfo mi=PascalABCCompiler.NetHelper.NetHelper.GetWriteAccessor(_pi);
				if (mi==null)
				{
					return null;
				}
                return compiled_function_node.get_compiled_method(mi);
            }
        }

        /// <summary>
        /// Акцессор set.
        /// </summary>
		public override function_node set_function
		{
			get
			{
				return compiled_set_accessor;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.basic_property_node;
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

        //TODO: Сделать меьлды в NetHelper-е, которые будут возвращать уровень доступа и polymorphic_state для всего.
        /// <summary>
        /// Виртуальное, статическое или обычное свойство.
        /// </summary>
		public override SemanticTree.polymorphic_state polymorphic_state
		{
			get
			{
                System.Reflection.MethodInfo _mi=PascalABCCompiler.NetHelper.NetHelper.GetAnyAccessor(_pi);
				if (_mi==null)
				{
					return SemanticTree.polymorphic_state.ps_common;
				}
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
				//Как с остальными - override,new? Наверно они не нужны?
				return SemanticTree.polymorphic_state.ps_common;
			}
		}

        /// <summary>
        /// Уровень доступа к свойству. Мало что значит, т.к. надо проверять акцессоры 
        /// по отдельности при обращении к ним, т.к. могут быть акцессоры с разными уровнями доступа.
        /// </summary>
		public override SemanticTree.field_access_level field_access_level
		{
			get
			{
				System.Reflection.MethodInfo _mi=PascalABCCompiler.NetHelper.NetHelper.GetAnyAccessor(_pi);
				if (_mi==null)
				{
					return SemanticTree.field_access_level.fal_private;
				}
				if (_mi.IsPublic)
				{
					return SemanticTree.field_access_level.fal_public;
				}
				if (_mi.IsPrivate)
				{
					return SemanticTree.field_access_level.fal_private;
				}
				//Как в System.Reflection отображается то, что метод protected?
				return SemanticTree.field_access_level.fal_protected;
			}
		}

        //TODO: Лучше эту информацию вообще никому кроме интерфейсов не выдавать.
        /// <summary>
        /// Информация о свойстве.
        /// </summary>
		public System.Reflection.PropertyInfo property_info
		{
			get
			{
				return _pi;
			}
		}

		SemanticTree.ICompiledMethodNode SemanticTree.ICompiledPropertyNode.compiled_get_method
		{
			get
			{
                return this.compiled_get_accessor;
			}
		}

		SemanticTree.ICompiledMethodNode SemanticTree.ICompiledPropertyNode.compiled_set_method
		{
			get
			{
                return this.compiled_set_accessor;
			}
		}

        SemanticTree.ICompiledTypeNode SemanticTree.ICompiledPropertyNode.compiled_comprehensive_type
        {
            get
            {
                return this.compiled_comprehensive_type;
            }
        }

		public override SemanticTree.node_kind node_kind
		{
			get
			{
				return SemanticTree.node_kind.compiled;
			}
		}

		SemanticTree.ICompiledTypeNode SemanticTree.ICompiledClassMemberNode.comprehensive_type
		{
			get
			{
				return compiled_comprehensive_type;
			}
		}

		public SemanticTree.ITypeNode comperehensive_type
		{
			get
			{
				return compiled_comprehensive_type;
			}
		}
	}

    /// <summary>
    /// Класс, представляющий обращение к статическому свойству.
    /// Реально в выходной код обращения к свойствам не идут. Они заменяются на вызовы акцессоров.
    /// Эти классы используются как промежуточные.
    /// </summary>
	[Serializable]
	public class static_property_reference : addressed_expression
	{
        /// <summary>
        /// Свойство.
        /// </summary>
		private property_node _prop;

        /// <summary>
        /// Список фактических параметров.
        /// </summary>
        private expressions_list _fact_parametres = new expressions_list();

		public static_property_reference(property_node pn, location loc) :
			base(pn.property_type,loc)
		{
			_prop=pn;
		}

        /// <summary>
        /// Свойство, к которому мы обращаемся.
        /// </summary>
		public property_node property
		{
			get
			{
				return _prop;
			}
		}

        /// <summary>
        /// Список фактических параметров.
        /// </summary>
		public expressions_list fact_parametres
		{
			get
			{
				return _fact_parametres;
			}
		}

        /// <summary>
        /// Выражение не адресное.
        /// </summary>
		public override bool is_addressed
		{
			get
			{
				return false;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.static_property_reference;
			}
		}

	}

    //TODO: Не очень хорошая структура наследования для этих двух классов (static_property_reference и non_static_property_reference).
    /// <summary>
    /// Класс, представляющий обращение к не статическому свойству.
    /// Реально в выходной код обращения к свойствам не идут. Они заменяются на вызовы акцессоров.
    /// Эти классы используются как промежуточные.
    /// </summary>
	[Serializable]
    public class non_static_property_reference : static_property_reference
	{
		/// <summary>
		/// Выражение, к свойству которого мы обращаемся.
		/// </summary>
		private expression_node _en;

		public non_static_property_reference(property_node pn,expression_node obj,location loc) :
			base(pn,loc)
		{
			_en=obj;
		}

        /// <summary>
        /// Выражение, к свойству которого мы обращаемся.
        /// </summary>
		public expression_node expression
		{
			get
			{
				return _en;
			}
		}

        /// <summary>
        /// Тип узла.
        /// </summary>
		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.non_static_property_reference;
			}
		}

	}
}
