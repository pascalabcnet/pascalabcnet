// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;

namespace PascalABCCompiler.TreeRealization
{
    public abstract class event_node : definition_node, SemanticTree.IEventNode
    {
        public override general_node_type general_node_type
        {
            get
            {
                return general_node_type.event_node;
            }
        }

        public abstract bool is_static
        {
            get;
        }

        public abstract string name
        {
        	get;
        }
        
        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        public abstract type_node comprehensive_type
        {
            get;
        }

        public abstract function_node add_method
        {
            get;
        }

        public abstract function_node raise_method
        {
            get;
        }

        public abstract function_node remove_method
        {
            get;
        }

        public abstract type_node delegate_type
        {
            get;
        }
    }
	
    public class common_event : event_node, SemanticTree.ICommonEventNode
    {
    	private string _name;
    	private type_node del_type;
    	private location _loc;
    	private SemanticTree.field_access_level _field_access_level;
    	private SemanticTree.polymorphic_state _polymorphic_state;
    	private common_type_node _cont_type;
    	private common_method_node _add_method;
    	private common_method_node _remove_method;
    	private common_method_node _raise_method;
    	private class_field _field;
    	
    	public common_event(string name, type_node del_type, common_type_node cont_type, common_method_node add_method, common_method_node remove_method, common_method_node raise_method, 
    	                    SemanticTree.field_access_level fal, SemanticTree.polymorphic_state ps, location loc)
    	{
    		this._name = name;
    		this.del_type = del_type;
    		this._add_method = add_method;
    		this._remove_method = remove_method;
    		this._raise_method = raise_method;
    		this._field_access_level = fal;
    		this._polymorphic_state = ps;
    		this._cont_type = cont_type;
    		this._loc = loc;
    	}
    	
    	public override string name
    	{
    		get
    		{
    			return _name;
    		}
    	}
    	
    	public class_field field
    	{
    		get
    		{
    			return _field;
    		}
    		set
    		{
    			_field = value;
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
    	
		public override type_node delegate_type {
			get { return del_type; }
		}
    	
		public override bool is_static {
			get { return _polymorphic_state == SemanticTree.polymorphic_state.ps_static; }
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
    	
    	/// <summary>
        /// Тип, который содержит этот метод.
        /// </summary>
		public common_type_node cont_type
		{
			get
			{
				return _cont_type;
			}
			set
            {
                _cont_type = value;
            }
		}
		
		public void set_add_method(common_method_node meth)
		{
			_add_method = meth;
		}
		
		public void set_remove_method(common_method_node meth)
		{
			_remove_method = meth;
		}
		
		public void set_raise_method(common_method_node meth)
		{
			_raise_method = meth;
		}
		
    	public override function_node add_method
        {
            get
            {
            	return _add_method;
            }
        }

        public override function_node raise_method
        {
            get
            {
            	return _raise_method;
            }
        }

        public override function_node remove_method
        {
            get
            {
            	return _remove_method;
            }
        }

        public override type_node comprehensive_type
        {
            get { return _cont_type; }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_event;
            }
        }
        
        string SemanticTree.ICommonEventNode.Name
        {
        	get
        	{
        		return _name;
        	}
        }

        SemanticTree.ICommonClassFieldNode SemanticTree.ICommonEventNode.Field
        {
            get
            {
                return _field;
            }
        }

        SemanticTree.ICommonMethodNode SemanticTree.ICommonEventNode.AddMethod
        {
        	get
        	{
        		return _add_method;
        	}
        }
        
        SemanticTree.ICommonMethodNode SemanticTree.ICommonEventNode.RemoveMethod
        {
        	get
        	{
        		return _remove_method;
        	}
        }
        
        SemanticTree.ICommonMethodNode SemanticTree.ICommonEventNode.RaiseMethod
        {
        	get
        	{
        		return _raise_method;
        	}
        }
        
        SemanticTree.ITypeNode SemanticTree.ICommonEventNode.DelegateType
        {
        	get
        	{
        		return del_type;
        	}
        }
        
        bool SemanticTree.ICommonEventNode.IsStatic
        {
        	get
        	{
        		return is_static;
        	}
        }
        
		public PascalABCCompiler.SemanticTree.ICommonTypeNode common_comprehensive_type {
			get {
				return _cont_type;
			}
		}
    	
		public PascalABCCompiler.SemanticTree.ITypeNode comperehensive_type {
			get {
				return _cont_type;
			}
		}
    	
		public PascalABCCompiler.SemanticTree.polymorphic_state polymorphic_state {
			get {
        		return _polymorphic_state;
			}
        	set
        	{
        		_polymorphic_state = value;
        	}
		}
        
        public SemanticTree.ILocation Location
		{
			get
			{
				return _loc;
			}
		}
        
    	public override void visit(PascalABCCompiler.SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
    }

    public class common_namespace_event : event_node, SemanticTree.ICommonNamespaceEventNode
    {
        private string _name;
    	private type_node del_type;
    	private location _loc;
    	private common_namespace_function_node _add_function;
    	private common_namespace_function_node _remove_function;
    	private common_namespace_function_node _raise_function;
    	private namespace_variable _field;
        private common_namespace_node _cont_namespace;

        public common_namespace_event(string name, type_node del_type, common_namespace_node cont_namespace, common_namespace_function_node add_function, common_namespace_function_node remove_function, common_namespace_function_node raise_function, 
    	                     location loc)
    	{
    		this._name = name;
    		this.del_type = del_type;
    		this._add_function = add_function;
    		this._remove_function = remove_function;
    		this._raise_function = raise_function;
            this._cont_namespace = cont_namespace;
    		this._loc = loc;
    	}
    	
    	public override string name
    	{
    		get
    		{
    			return _name;
    		}
    	}

        public override type_node comprehensive_type
        {
            get { return null; }
        }

    	public namespace_variable field
    	{
    		get
    		{
    			return _field;
    		}
    		set
    		{
    			_field = value;
    		}
    	}
    	
		public override type_node delegate_type {
			get { return del_type; }
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

        public void set_event_type(type_node tn)
        {
            del_type = tn;
        }

		public void set_add_function(common_namespace_function_node meth)
		{
			_add_function = meth;
		}

        public void set_remove_function(common_namespace_function_node meth)
		{
			_remove_function = meth;
		}

        public void set_raise_function(common_namespace_function_node meth)
		{
			_raise_function = meth;
		}

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

    	public override function_node add_method
        {
            get
            {
            	return _add_function;
            }
        }

        public override function_node raise_method
        {
            get
            {
            	return _raise_function;
            }
        }

        public override function_node remove_method
        {
            get
            {
            	return _remove_function;
            }
        }

        public override bool is_static
        {
            get { return true; }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.common_namespace_event;
            }
        }
        
        string SemanticTree.ICommonNamespaceEventNode.Name
        {
        	get
        	{
        		return _name;
        	}
        }

        SemanticTree.ICommonNamespaceVariableNode SemanticTree.ICommonNamespaceEventNode.Field
        {
            get
            {
                return _field;
            }
        }

        SemanticTree.ICommonNamespaceFunctionNode SemanticTree.ICommonNamespaceEventNode.AddFunction
        {
        	get
        	{
        		return _add_function;
        	}
        }

        SemanticTree.ICommonNamespaceFunctionNode SemanticTree.ICommonNamespaceEventNode.RemoveFunction
        {
        	get
        	{
        		return _remove_function;
        	}
        }

        SemanticTree.ICommonNamespaceFunctionNode SemanticTree.ICommonNamespaceEventNode.RaiseFunction
        {
        	get
        	{
        		return _raise_function;
        	}
        }
        
        SemanticTree.ITypeNode SemanticTree.ICommonNamespaceEventNode.DelegateType
        {
        	get
        	{
        		return del_type;
        	}
        }
        
        public SemanticTree.ILocation Location
		{
			get
			{
				return _loc;
			}
		}
        
    	public override void visit(PascalABCCompiler.SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
    }

    public class compiled_event : event_node, SemanticTree.ICompiledEventNode
    {

        private System.Reflection.EventInfo _ei;
       
        public compiled_event(System.Reflection.EventInfo ei)
        {
            _ei = ei;
        }

        public override bool is_static
        {
            get
            {
                return _ei.GetAddMethod().IsStatic;
            }
        }
		
		public override string name {
			get { return _ei.Name; }
		}
        
        public System.Reflection.EventInfo event_info
        {
        	get
        	{
        		return _ei;
        	}
        }

        public override type_node comprehensive_type
        {
            get { return compiled_comprehensive_type; }
        }

        public compiled_type_node compiled_comprehensive_type
        {
            get
            {
                return compiled_type_node.get_type_node(_ei.DeclaringType);
            }
        }

        public System.Reflection.EventInfo CompiledEvent
        {
            get
            {
                return _ei;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_event;
            }
        }

        public override void visit(SemanticTree.ISemanticVisitor visitor)
        {
            visitor.visit(this);
        }

        public override function_node add_method
        {
            get
            {
                return compiled_function_node.get_compiled_method(_ei.GetAddMethod());
            }
        }

        public override function_node raise_method
        {
            get
            {
                return compiled_function_node.get_compiled_method(_ei.GetRaiseMethod());
            }
        }

        public override function_node remove_method
        {
            get
            {
                return compiled_function_node.get_compiled_method(_ei.GetRemoveMethod());
            }
        }

        public override type_node delegate_type
        {
	        get
            {
                return compiled_type_node.get_type_node(_ei.EventHandlerType);
            }
        }

    }

    public class static_event_reference : addressed_expression, SemanticTree.IStaticEventReference
    {
        private event_node _en;

        public static_event_reference(event_node en, location loc)
            :base(en.delegate_type, loc)
        {
            _en = en;
        }

        public event_node en
        {
            get
            {
                return _en;
            }
        }
		
        SemanticTree.IEventNode SemanticTree.IStaticEventReference.Event
        {
        	get
        	{
        		return _en;
        	}
        }
        
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.static_event_reference;
            }
        }
        
		public override void visit(PascalABCCompiler.SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
    }
	 
    public class nonstatic_event_reference : static_event_reference, SemanticTree.INonStaticEventReference
    {
        private expression_node _obj;

        public nonstatic_event_reference(expression_node obj, event_node en, location loc)
            : base(en, loc)
        {
            _obj = obj;
        }

        public expression_node obj
        {
            get
            {
                return _obj;
            }
        }
		
        SemanticTree.IExpressionNode SemanticTree.INonStaticEventReference.obj
        {
        	get
        	{
        		return _obj;
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
                return semantic_node_type.nonstatic_event_reference;
            }
        }
    }
}
