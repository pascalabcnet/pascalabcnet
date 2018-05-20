// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.TreeRealization
{

	public enum concrete_parameter_type {cpt_var, cpt_const, cpt_none};

	[Serializable]
	public abstract class var_definition_node : definition_node, SemanticTree.IVAriableDefinitionNode
	{
        public override string ToString() => "var " + name + ": "+ type.ToString();

        private string _name;
		private type_node _type;
        private expression_node _inital_value;
        private bool _is_ret_value;
        private bool _is_special_name;
        private SemanticTree.polymorphic_state _polymorphic_state = PascalABCCompiler.SemanticTree.polymorphic_state.ps_common;

        public virtual SemanticTree.polymorphic_state polymorphic_state
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

        public virtual bool IsStatic
        {
            get
            {
                return polymorphic_state == PascalABCCompiler.SemanticTree.polymorphic_state.ps_static;
            }
        }

        private void check_special_names()
        {
            if (name != null)
            if (_name.Length > 0 && _name[0] == '$')
            {
                if (_name.StartsWith("$rv"))
                {
                    _is_ret_value = true;
                    _is_special_name = true;
                }
                else _is_special_name = true;
            }
        }

        public var_definition_node(string name)
        {
            _name = name;
            check_special_names();
        }

		public var_definition_node(string name,type_node type)
		{
			_name=name;
			_type=type;
            check_special_names();
		}

        public bool is_ret_value
        {
            get
            {
                return _is_ret_value;
            }
            set
            {
                _is_ret_value = value;
            }
        }

        public bool is_special_name
        {
            get
            {
                return _is_special_name;
            }
            set
            {
                _is_special_name = value;
            }
		}

		//Имя переменной.
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		//Тип переменной.
		public type_node type
		{
			get
			{
				return _type;
			}
            set
            {
                _type = value;
            }
		}


        public expression_node inital_value
        {
            get
            {
                return _inital_value;
            }
            set
            {
                _inital_value = value;
            }
        }
        SemanticTree.IExpressionNode SemanticTree.IVAriableDefinitionNode.inital_value
        {
            get
            {
                return _inital_value;
            }
        }

		//Раположение переменной.
		public abstract SemanticTree.node_location_kind node_location_kind
		{
			get;
		}

		public override general_node_type general_node_type
		{
			get
			{
				return general_node_type.variable_node;
			}
		}


		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.ITypeNode SemanticTree.IVAriableDefinitionNode.type
		{
			get
			{
				return _type;
			}
		}
	}

	[Serializable]
	public class local_variable : var_definition_node, SemanticTree.ILocalVariableNode
	{
		private common_function_node _cont_function;

		private location _loc;

		private bool _is_used_as_unlocal;

        public local_variable(string name, common_function_node cont_function, location loc)
            : base(name)
        {
            _cont_function = cont_function;
            _loc = loc;
        }

		public local_variable(string name,type_node type,common_function_node cont_function, location loc) : base(name,type)
		{
			_cont_function=cont_function;
            _loc = loc;
		}

		public location loc
		{
			get
			{
				return _loc;
			}
		}

		public bool is_used_as_unlocal
		{
			get
			{
				return _is_used_as_unlocal;
			}
		}

        public void set_used_as_unlocal()
        {
            _is_used_as_unlocal = true;
        }

		public SemanticTree.ILocation Location
		{
			get
			{
				return _loc;
			}
		}

		//Функция, которая содержит эту переменную.
		public common_function_node function
		{
			get
			{
				return _cont_function;
			}
            set
            {
                _cont_function = value;
            }
		}

		//Раположение переменной - переменная расположена в функции.
		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_function_location;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.local_variable;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        SemanticTree.ICommonFunctionNode SemanticTree.IFunctionMemberNode.function
		{
			get
			{
				return _cont_function;
			}
		}
	}

    [Serializable]
    public class local_block_variable : var_definition_node, SemanticTree.ILocalBlockVariableNode
    {
        private location _loc;
        statements_list stmt_list;

        public local_block_variable(string name, statements_list stmt_list, location loc)
            : base(name)
        {
            this.stmt_list = stmt_list;
            _loc = loc;
        }

        public local_block_variable(string name, type_node type, statements_list stmt_list, location loc)
            : base(name, type)
        {
            this.stmt_list = stmt_list;
            _loc = loc;
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

        //Функция, которая содержит эту переменную.
        public statements_list block
        {
            get
            {
                return stmt_list;
            }
            set
            {
                stmt_list = value;
            }
        }

        //Раположение переменной - переменная расположена в функции.
        public override SemanticTree.node_location_kind node_location_kind
        {
            get
            {
                return SemanticTree.node_location_kind.in_block_location;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.local_block_variable;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        SemanticTree.IStatementsListNode SemanticTree.ILocalBlockVariableNode.Block
        {
            get
            {
                return stmt_list;
            }
        }
    }
    
    [Serializable]
	public class namespace_variable : var_definition_node, SemanticTree.ICommonNamespaceVariableNode
	{
		private common_namespace_node _cont_namespace;

		private location _loc;

        public namespace_variable(string name, common_namespace_node cont_namespace, location loc)
            : base(name)
        {
            _cont_namespace = cont_namespace;
            _loc = loc;
        }

		public namespace_variable(string name,type_node type,common_namespace_node cont_namespace,location loc) : base(name,type)
		{
			_cont_namespace=cont_namespace;
            _loc = loc;
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

		//Пространство имен, в котором описана эта переменная.
		public common_namespace_node namespace_node
		{
			get
			{
				return _cont_namespace;
			}
            set
            {
                _cont_namespace = value;
            }
		}

		//Раположение переменной - переменная расположена непосредственно в пространстве имен.
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
				return semantic_node_type.namespace_variable;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        public SemanticTree.ICommonNamespaceNode comprehensive_namespace
		{
			get
			{
				return _cont_namespace;
			}
		}
	}

	[Serializable]
	public class class_field : var_definition_node, SemanticTree.ICommonClassFieldNode
	{
		private common_type_node _cont_class;
		private SemanticTree.polymorphic_state _polymorphic_state;
		private SemanticTree.field_access_level _field_access_level;

		private location _loc;

        public class_field(string name, common_type_node cont_class,
            SemanticTree.polymorphic_state polymorphic_state, SemanticTree.field_access_level field_access_level,
            location loc) :
            base(name)
        {
            _cont_class = cont_class;
            _polymorphic_state = polymorphic_state;
            _field_access_level = field_access_level;
            _loc = loc;
        }


		public class_field(string name,type_node type,common_type_node cont_class,
			SemanticTree.polymorphic_state polymorphic_state, SemanticTree.field_access_level field_access_level,
            location loc):
			base(name,type)
		{
			_cont_class=cont_class;
			_polymorphic_state=polymorphic_state;
			_field_access_level=field_access_level;
            _loc = loc;
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

		//Класс, в котором описано это поле.
		public common_type_node cont_type
		{
			get
			{
				return _cont_class;
			}
			set
			{
				_cont_class = value;
			}
		}
		
		public bool IsReadOnly
		{
			get
			{
				return false;
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

		public SemanticTree.field_access_level field_access_level
		{
			get
			{
				return _field_access_level;
			}
			set
			{
				_field_access_level = value;
			}
		}

		//Раположение переменной - переменная расположена в классе.
		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_class_location;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.class_field;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        public SemanticTree.ICommonTypeNode common_comprehensive_type
		{
			get
			{
				return _cont_class;
			}
		}

		public SemanticTree.ITypeNode comperehensive_type
		{
			get
			{
				return _cont_class;
			}
		}

		SemanticTree.polymorphic_state SemanticTree.IClassMemberNode.polymorphic_state
		{
			get
			{
				return _polymorphic_state;
			}
		}
	}

	[Serializable]
	public class compiled_variable_definition : var_definition_node, SemanticTree.ICompiledClassFieldNode
	{
		private compiled_type_node _cont_type;
		private System.Reflection.FieldInfo _fi;

        //TODO: Сделать поля одиночками.
		public compiled_variable_definition(System.Reflection.FieldInfo fi) :
			base(fi.Name,compiled_type_node.get_type_node(fi.FieldType))
		{
			_fi=fi;
			_cont_type=compiled_type_node.get_type_node(fi.DeclaringType);
		}

		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_class_location;
			}
		}

		public compiled_type_node cont_type
		{
			get
			{
				return _cont_type;
			}
		}
		
		public System.Reflection.FieldInfo compiled_field
        {
			get
            {
				return _fi;
			}
		}

        //TODO: Вынести в NetHelper.
		public SemanticTree.polymorphic_state polymorphic_state
		{
			get
			{
				if (_fi.IsStatic)
				{
					return SemanticTree.polymorphic_state.ps_static;
				}
				return SemanticTree.polymorphic_state.ps_common;
			}
		}
		
		public bool IsReadOnly
		{
			get
			{
				return _fi.IsInitOnly;
			}
		}
		
		public bool IsLiteral
		{
			get
			{
				return _fi.IsLiteral;
			}
		}
		
        //TODO: Вынести в NetHelper.
		public SemanticTree.field_access_level field_access_level
		{
			get
			{
                return NetHelper.NetHelper.GetFieldAccessLevel(_fi);
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.compiled_variable_definition;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		public SemanticTree.ICompiledTypeNode comprehensive_type
		{
			get
			{
				return _cont_type;
			}
		}

		public SemanticTree.ITypeNode comperehensive_type
		{
			get
			{
				return _cont_type;
			}
		}
	}

	[Serializable]
	public abstract class parameter : var_definition_node, SemanticTree.IParameterNode
	{
		protected SemanticTree.parameter_type _par_type;

        public parameter(string name) : base(name)
        {
        }
        
        public parameter(string name,type_node type) : base(name,type)
		{
		}

		//Тип параметра.
		public abstract SemanticTree.parameter_type parameter_type
		{
			get;
		}

        public abstract bool is_params
        {
            get;
        }

        public abstract bool is_const
        {
            get;
        }

		//Функция, в которой описан этот праметр.
		public abstract function_node function
		{
			get;
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.IFunctionNode SemanticTree.IParameterNode.function
		{
			get
			{
				return this.function;
			}
		}

		SemanticTree.parameter_type SemanticTree.IParameterNode.parameter_type
		{
			get
			{
                return _par_type;
				//return SemanticTree.parameter_type.value;
			}
		}

		public override SemanticTree.node_location_kind node_location_kind
		{
			get
			{
				return SemanticTree.node_location_kind.in_function_location;
			}
		}

        SemanticTree.IExpressionNode SemanticTree.IParameterNode.default_value
        {
            get
            {
                return default_value;
            }
        }

        public virtual expression_node default_value
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

    }

	[Serializable]
	public class common_parameter : parameter, SemanticTree.ICommonParameterNode
	{
		private common_function_node _cont_function;
		
		private concrete_parameter_type _concrete_parameter_type;

		private location _loc;

		private bool _is_used_as_unlocal;

		private expression_node _default_value;

        private bool _is_params;

        public common_parameter(string name, SemanticTree.parameter_type pt,
            common_function_node cont_function, concrete_parameter_type conc_par_type, location loc) :
            base(name)
        {
            _par_type = pt;
            _cont_function = cont_function;
            _concrete_parameter_type = conc_par_type;
            _loc = loc;
        }

        public common_parameter(string name, SemanticTree.parameter_type pt,
            common_function_node cont_function, concrete_parameter_type conc_par_type, expression_node default_value,
            location loc) :
            base(name)
        {
            _par_type = pt;
            _cont_function = cont_function;
            _concrete_parameter_type = conc_par_type;
            _default_value = default_value;
            _loc = loc;
        }

		public common_parameter(string name,type_node tp,SemanticTree.parameter_type pt,
			common_function_node cont_function, concrete_parameter_type conc_par_type,expression_node default_value,
            location loc):
			base(name,tp)
		{
			_par_type=pt;
			_cont_function=cont_function;
            _concrete_parameter_type = conc_par_type;
            _default_value = default_value;
            _loc = loc;
		}

		public override expression_node default_value
		{
			get
			{
				return _default_value;
			}
            set
            {
                _default_value = value;
            }
		}

        public override bool is_const
        {
            get { return this._concrete_parameter_type == concrete_parameter_type.cpt_const; }
        }

        public bool intrenal_is_params
        {
            get
            {
                return _is_params;
            }
            set
            {
                _is_params = value;
            }
        }

        public override bool is_params
        {
            get
            {
                return _is_params;
            }
        }


        public void set_param_is_params(bool is_params_)
        {
            _is_params = is_params_;
        }

		public concrete_parameter_type concrete_parameter_type
		{
			get
			{
				return _concrete_parameter_type;
			}
            set
            {
                _concrete_parameter_type = value;
            }
		}

		public bool is_used_as_unlocal
		{
			get
			{
				return _is_used_as_unlocal;
			}
		}

        public void set_used_as_unlocal()
        {
            _is_used_as_unlocal = true;
        }

		public location loc
		{
			get
			{
				return _loc;
			}
		}

		public SemanticTree.ILocation Location
		{
			get
			{
				return _loc;
			}
		}

		//Тип параметра.
		public override SemanticTree.parameter_type parameter_type
		{
			get
			{
				return _par_type;
			}
		}

		public override function_node function
		{
			get
			{
				return common_function;
			}
		}

		//Функция, в которой определен параметер.
		public common_function_node common_function
		{
			get
			{
				return _cont_function;
			}
            set
            {
                _cont_function = value;
            }
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_parameter;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

        SemanticTree.ICommonFunctionNode SemanticTree.ICommonParameterNode.common_function
		{
			get
			{
				return _cont_function;
			}
		}
	}

	[Serializable]
	public class basic_parameter : parameter, SemanticTree.IBasicParameterNode
	{
		private basic_function_node _cont_function;

		public basic_parameter(string name,type_node type,SemanticTree.parameter_type par_type,
			basic_function_node cont_function) : base(name,type)
		{
			_cont_function=cont_function;
			_par_type=par_type;
		}

		//Функция, в которой определен параметер.
		public override function_node function
		{
			get
			{
				return _cont_function;
			}
		}

        public override bool is_const
        {
            get { return false; }
        }

        public override bool is_params
        {
            get
            {
                return false;
            }
        }

		//Тип параметра (по ссылке или по значению).
		public override SemanticTree.parameter_type parameter_type
		{
			get
			{
				return _par_type;
			}
		}

		public basic_function_node cont_function
		{
			get
			{
				return _cont_function;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.basic_parameter;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}

	[Serializable]
	public class compiled_parameter : parameter, SemanticTree.ICompiledParameterNode
	{
		private readonly System.Reflection.ParameterInfo _par;
        private expression_node _default_value;
        private bool _is_params;
        private bool _is_params_evaluated;

		public compiled_parameter(System.Reflection.ParameterInfo param) : 
			base(param.Name,compiled_type_node.get_type_node(param.ParameterType))
		{
			_par=param;
		}
		
		public System.Reflection.ParameterInfo parameter_info
		{
			get
			{
				return _par;
			}
		}

        public override expression_node default_value
        {
            get
            {
                if (_par.IsOptional && _default_value == null)
                {
                    if (_par.DefaultValue == null)
                        _default_value = new null_const_node(type, null);
                    else
                        _default_value = constant_node.make_constant(_par.DefaultValue);
                }
                return _default_value;
            }
            set
            {
                
            }
        }

        public override bool is_const
        {
            get { return false; }
        }

		public override function_node function
		{
			get
			{
				return compiled_function;
			}
		}

        public override bool is_params
        {
            get
            {
                if (_is_params_evaluated)
                    return _is_params;

                object[] objarr = _par.GetCustomAttributes(typeof(ParamArrayAttribute), true);
                if ((objarr == null) || (objarr.Length == 0))
                {
                    _is_params = false;
                }
                else
                { 
                    _is_params = true;
                }
                _is_params_evaluated = true;
                return _is_params;
            }
        }

		public compiled_function_node compiled_function
		{
			get
			{
#if (DEBUG)
				if (_par.Member.MemberType!=System.Reflection.MemberTypes.Method)
				{
					throw new Exception("Not method");
				}
#endif
                return compiled_function_node.get_compiled_method((System.Reflection.MethodInfo)_par.Member);
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.compiled_parameter;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.ICompiledMethodNode SemanticTree.ICompiledParameterNode.compiled_function
		{
			get
			{
				return compiled_function;
			}
		}

        public void SetParameterType(SemanticTree.parameter_type pt)
        {
            _par_type = pt;
        }

		public override SemanticTree.parameter_type parameter_type
		{
			get
			{
				/*
				if (_par.IsOut)
				{
					return SemanticTree.parameter_type.var;
				}
				return SemanticTree.parameter_type.value;
				*/
				return _par_type;
			}
		}
	}


}
