// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.TreeRealization
{

	[Serializable]
    public class local_variable_reference : variable_reference, SemanticTree.ILocalVariableReferenceNode
	{
		private local_variable _var;
		private int _static_depth;

		public local_variable_reference(local_variable var,int static_depth,location loc) : base(var.type,loc)
		{
			_var=var;
			this.static_depth=static_depth;   
		}

        SemanticTree.IVAriableDefinitionNode SemanticTree.IReferenceNode.Variable
        {
            get
            {
                return _var;
            }
        }

		//Определение локальной переменной.
		public local_variable var
		{
			get
			{
				return _var;
			}
		}

		//Разность статических глубин, между определением и вхождением.
		public int static_depth
		{
			get
			{
				return _static_depth;
			}
			private set
			{
				_static_depth=value;
                if (value > 0)
                {
                    _var.set_used_as_unlocal();
                }
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.local_variable_reference;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		public SemanticTree.ILocalVariableNode variable
		{
			get
			{
				return _var;
			}
		}

        public override var_definition_node VariableDefinition
        {
            get
            {
                return _var;
            }
        }

	}

    [Serializable]
    public class local_block_variable_reference : variable_reference, SemanticTree.ILocalBlockVariableReferenceNode
    {
        private local_block_variable _var;

        public local_block_variable_reference(local_block_variable var, location loc)
            : base(var.type, loc)
        {
            _var = var;
        }

        SemanticTree.IVAriableDefinitionNode SemanticTree.IReferenceNode.Variable
        {
            get
            {
                return _var;
            }
        }


        //Определение локальной переменной.
        public local_block_variable var
        {
            get
            {
                return _var;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.local_block_variable_reference;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        public SemanticTree.ILocalBlockVariableNode Variable
        {
            get
            {
                return _var;
            }
        }

        public override var_definition_node VariableDefinition
        {
            get
            {
                return _var;
            }
        }

    }


    public abstract class variable_reference : addressed_expression
    {
        public variable_reference(type_node tn, location loc)
            : base(tn, loc)
        {
        }

        public virtual var_definition_node VariableDefinition
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
    
    [Serializable]
    public class namespace_variable_reference : variable_reference, SemanticTree.INamespaceVariableReferenceNode
	{
		private namespace_variable _var;

		public namespace_variable_reference(namespace_variable var,location loc) :
			base(var.type,loc)
		{
			_var=var;
		}


        SemanticTree.IVAriableDefinitionNode SemanticTree.IReferenceNode.Variable
        {
            get
            {
                return _var;
            }
        }

		//Переменная.
		public namespace_variable var
		{
			get
			{
				return _var;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.namespace_variable_reference;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		public SemanticTree.ICommonNamespaceVariableNode variable
		{
			get
			{
				return _var;
			}
		}

        public override var_definition_node VariableDefinition
        {
            get
            {
                return _var;
            }
        }
	}

	[Serializable]
    public class class_field_reference : variable_reference, SemanticTree.ICommonClassFieldReferenceNode
	{
		private class_field _field;
		private expression_node _obj;

		public class_field_reference(class_field field,expression_node obj,location loc) :
			base(field.type,loc)
		{
			_field=field;
			_obj=obj;
		}

        public override var_definition_node VariableDefinition
        {
            get
            {
                return _field;
            }
        }

        SemanticTree.IVAriableDefinitionNode SemanticTree.IReferenceNode.Variable
        {
            get
            {
                return _field;
            }
        }

		//Поле класса.
		public class_field field
		{
			get
			{
				return _field;
			}
		}

		//Объект класса.
		public expression_node obj
		{
			get
			{
				return _obj;
			}
			set
			{
				_obj = value;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.class_field_reference;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.ICommonClassFieldNode SemanticTree.ICommonClassFieldReferenceNode.field
		{
			get
			{
				return _field;
			}
		}

		SemanticTree.IExpressionNode SemanticTree.ICommonClassFieldReferenceNode.obj
		{
			get
			{
				return _obj;
			}
		}

        public override bool is_addressed
        {
            get
            {
                bool flag;
                general_node_type gnd;
            	return !TreeConverter.convertion_data_and_alghoritms.check_for_constant_or_readonly(this,out flag, out gnd);
            }
        }
	}

	[Serializable]
    public class static_class_field_reference : variable_reference, SemanticTree.IStaticCommonClassFieldReferenceNode
	{
		private class_field _field;
		private common_type_node _type;

        /*
		public static_class_field_reference(class_field static_field,common_type_node type,location loc) :
			base(static_field.type,loc)
		{
			_field=static_field;
			_type=type;
		}
        */

		public static_class_field_reference(class_field static_field,location loc) :
			base(static_field.type,loc)
		{
			_field=static_field;
			_type=static_field.cont_type;
		}

        SemanticTree.IVAriableDefinitionNode SemanticTree.IReferenceNode.Variable
        {
            get
            {
                return _field;
            }
        }

		//Статическое поле класса.
		public class_field static_field
		{
			get
			{
				return _field;
			}
		}

		//Класс, к статическому методу которого мы обращаемся.
		public common_type_node class_type
		{
			get
			{
				return _type;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.static_class_field_reference;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		SemanticTree.ICommonClassFieldNode SemanticTree.IStaticCommonClassFieldReferenceNode.static_field
		{
			get
			{
				return _field;
			}
		}

		SemanticTree.ICommonTypeNode SemanticTree.IStaticCommonClassFieldReferenceNode.class_type
		{
			get
			{
				return _type;
			}
		}

        public override var_definition_node VariableDefinition
        {
            get
            {
                return _field;
            }
        }

	}

	[Serializable]
    public class compiled_variable_reference : variable_reference, SemanticTree.ICompiledFieldReferenceNode
	{
		private compiled_variable_definition _var;
		private expression_node _obj;

		public compiled_variable_reference(compiled_variable_definition var,expression_node obj,location loc) :
			base(var.type,loc)
		{
			_var=var;
			_obj=obj;
		}

        public override var_definition_node VariableDefinition
        {
            get
            {
                return _var;
            }
        }

		//Поле класса.
		public compiled_variable_definition var
		{
			get
			{
				return _var;
			}
		}

        SemanticTree.IVAriableDefinitionNode SemanticTree.IReferenceNode.Variable
        {
            get
            {
                return _var;
            }
        }


		//Объект класса.
		public expression_node obj
		{
			get
			{
				return _obj;
			}
			set
			{
				_obj = value;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.compiled_variable_reference;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		public SemanticTree.ICompiledClassFieldNode field
		{
			get
			{
				return _var;
			}
		}

		SemanticTree.IExpressionNode SemanticTree.ICompiledFieldReferenceNode.obj
		{
			get
			{
				return _obj;
			}
		}
	}

	[Serializable]
    public class static_compiled_variable_reference : variable_reference, SemanticTree.IStaticCompiledFieldReferenceNode
	{
		private compiled_variable_definition _var;
		private compiled_type_node _type;

		public static_compiled_variable_reference(compiled_variable_definition var,compiled_type_node type,location loc) :
			base(var.type,loc)
		{
			_var=var;
			_type=type;
		}

		public static_compiled_variable_reference(compiled_variable_definition var,location loc) :
			base(var.type,loc)
		{
			_var=var;
			_type=_var.cont_type;
		}

		//Поле класса.
		public compiled_variable_definition var
		{
			get
			{
				return _var;
			}
		}

        SemanticTree.IVAriableDefinitionNode SemanticTree.IReferenceNode.Variable
        {
            get
            {
                return _var;
            }
        }


		//Класс, к статическому полю которого мы обращаемся.
		public compiled_type_node class_type
		{
			get
			{
				return _type;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.static_compiled_variable_reference;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		public SemanticTree.ICompiledClassFieldNode static_field
		{
			get
			{
				return _var;
			}
		}

		SemanticTree.ICompiledTypeNode SemanticTree.IStaticCompiledFieldReferenceNode.class_type
		{
			get
			{
				return _type;
			}
		}

        public override var_definition_node VariableDefinition
        {
            get
            {
                return _var;
            }
        }

	}

	[Serializable]
    public class common_parameter_reference : variable_reference, SemanticTree.ICommonParameterReferenceNode
	{
		private common_parameter _par;
		private int _static_depth;

		public common_parameter_reference(common_parameter par,int static_depth,location loc) :
			base(par.type,loc)
		{
			_par=par;
			this.static_depth=static_depth;
		}

        public override var_definition_node VariableDefinition
        {
            get
            {
                return _par;
            }
        }

		//Параметр метода, к которому мы обращаемся.
		public common_parameter par
		{
			get
			{
				return _par;
			}
		}

        SemanticTree.IVAriableDefinitionNode SemanticTree.IReferenceNode.Variable
        {
            get
            {
                return _par;
            }
        }

		//Разность статических глубин, между обращением к параметру и методом в котором он объявлен.
		public int static_depth
		{
			get
			{
				return _static_depth;
			}
			set
			{
				_static_depth=value;
				if (_static_depth>0)
				{
                    _par.set_used_as_unlocal();
				}
			}
		}

		public override bool is_addressed
		{
			get
			{
				if (_par.concrete_parameter_type==concrete_parameter_type.cpt_const)
				{
					return false;
				}
				return true;
			}
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_parameter_reference;
			}
		}

		public override void visit(SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}

		int SemanticTree.ICommonParameterReferenceNode.static_depth
		{
			get
			{
				return _static_depth;
			}
		}

		public SemanticTree.ICommonParameterNode parameter
		{
			get
			{
				return _par;
			}
		}
	}

}
