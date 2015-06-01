// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeRealization
{
	[Serializable]
	public class attribute_node : semantic_node, SemanticTree.IAttributeNode, SemanticTree.ILocated
	{
		private function_node _attr_constr;
		private type_node _attr_type;
		private List<constant_node> _args = new List<constant_node>();
		private List<property_node> _prop_names = new List<property_node>();
		private List<constant_node> _prop_initializers = new List<constant_node>();
		private List<var_definition_node> _field_names = new List<var_definition_node>();
		private List<constant_node> _field_initializers = new List<constant_node>();
		private location _loc;
		private SemanticTree.attribute_qualifier_kind _qualifier;
		
		public attribute_node(function_node _attr_constr, type_node _attr_type, location _loc)
		{
			this._attr_constr = _attr_constr;
			this._attr_type = _attr_type;
			this._loc = _loc;
		}
		
		public type_node attribute_type
		{
			get
			{
				return _attr_type;
			}
			set
			{
				_attr_type = value;
			}
		}
		
		public SemanticTree.attribute_qualifier_kind qualifier
		{
			get
			{
				return _qualifier;
			}
			set
			{
				_qualifier = value;
			}
		}
		
		public function_node attribute_constr
		{
			get
			{
				return _attr_constr;
			}
			set
			{
				_attr_constr = value;
			}
		}
		
		public List<constant_node> args
		{
			get
			{
				return _args;
			}
		}
		
		public List<property_node> prop_names
		{
			get
			{
				return _prop_names;
			}
		}
		
		public List<constant_node> prop_initializers
		{
			get
			{
				return _prop_initializers;
			}
		}
		
		public List<var_definition_node> field_names
		{
			get
			{
				return _field_names;
			}
		}
		
		public List<constant_node> field_initializers
		{
			get
			{
				return _field_initializers;
			}
		}
		
		public SemanticTree.IConstantNode[] Arguments
		{
			get
			{
				return _args.ToArray();
			}
		}
		
		public SemanticTree.IPropertyNode[] PropertyNames
		{
			get
			{
				return _prop_names.ToArray();
			}
		}
		
		public SemanticTree.IConstantNode[] PropertyInitializers
		{
			get
			{
				return _prop_initializers.ToArray();
			}
		}
		
		public SemanticTree.IVAriableDefinitionNode[] FieldNames
		{
			get
			{
				return _field_names.ToArray();
			}
		}
		
		public SemanticTree.IConstantNode[] FieldInitializers
		{
			get
			{
				return _field_initializers.ToArray();
			}
		}
		
		public SemanticTree.ITypeNode AttributeType
		{
			get
			{
				return _attr_type;
			}
		}
		
		public SemanticTree.IFunctionNode AttributeConstructor
		{
			get
			{
				return _attr_constr;
			}
		}
		
		public location location
        {
            get
            {
                return _loc;
            }
            set
            {
                _loc=value;
            }
        }
		
		SemanticTree.ILocation SemanticTree.ILocated.Location
		{
			get
			{
				return _loc;
			}
		}
		
		public override general_node_type general_node_type
		{
			get
			{
				return general_node_type.attribute_node;
			}
		}
		
		public override semantic_node_type semantic_node_type {
			get {
				return semantic_node_type.attribute_node;
			}
		}
		
		public override void visit(PascalABCCompiler.SemanticTree.ISemanticVisitor visitor)
		{
			visitor.visit(this);
		}
	}
}
