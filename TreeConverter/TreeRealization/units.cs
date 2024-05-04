// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.TreeRealization
{

    [Serializable]
    public abstract class unit_node : definition_node
    {
        public abstract List<TreeConverter.SymbolInfo> find_only_in_namespace(string name);

        public override general_node_type general_node_type
        {
            get
            {
                return general_node_type.unit_node;
            }
        }
    }

    [Serializable]
    public class namespace_unit_node : unit_node
    {
        public using_namespace namespace_name;
        public SymbolTable.Scope scope;
        public location loc;
        public namespace_unit_node(using_namespace namespace_name)
        {
            this.namespace_name = namespace_name;
        }
        public namespace_unit_node(using_namespace namespace_name, location loc)
        {
            this.namespace_name = namespace_name;
            this.loc = loc;
        }
        public override List<TreeConverter.SymbolInfo> find_only_in_namespace(string name)
        {
            throw new NotSupportedException();
        }
        public override location location
        {
            get
            {
                return loc;
            }
        }
        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_unit_node;
            }
        }
    }

    [Serializable]
    public class dot_net_unit_node : unit_node
    {
        //private SymbolTable.DotNETScope _dotNetScope;
        private NetHelper.NetScope _dotNetScope;

        public dot_net_unit_node(NetHelper.NetScope dotNetScope)
        {
            _dotNetScope = dotNetScope;
        }

        public NetHelper.NetScope dotNetScope
        {
            get
            {
                return _dotNetScope;
            }
        }

        public override semantic_node_type semantic_node_type
        {
            get
            {
                return semantic_node_type.compiled_unit_node;
            }
        }

        public override List<TreeConverter.SymbolInfo> find_only_in_namespace(string name)
        {
            return _dotNetScope.Find(name);
        }
    }

	[Serializable]
    public sealed class common_unit_node : unit_node
	{
        private readonly common_namespace_node_list _namespaces = new common_namespace_node_list();

		private SymbolTable.UnitInterfaceScope _scope;
		private SymbolTable.UnitImplementationScope _implementation_scope;

        public System.Collections.Generic.List<compiler_directive> compiler_directives;

		private common_namespace_function_node _main_function;
        private common_namespace_function_node _finalization_method;
        public string unit_name { get; set; }

        internal common_parameter MainArgsParameter = null;
        internal expression_node MainArgsAssignExpr = null;

        internal bool IsSystemUnit
        {
            get
            {
                TreeConverter.SymbolInfo si = namespaces[0].findFirstOnlyInNamespace(StringConstants.system_unit_marker);
                if (si == null)
                    return false;
                if (si.sym_info is constant_definition_node)
                    if ((si.sym_info as constant_definition_node).const_value is bool_const_node)
                        return ((si.sym_info as constant_definition_node).const_value as bool_const_node).constant_value;
                return false;
            }
        }

        internal bool_const_node IsConsoleApplicationVariableValue = new bool_const_node(true, null);
        internal expression_node IsConsoleApplicationVariableAssignExpr = null;

        ///<summary>
        ///Задает значение переменной IsConsoleApplication в системном модуле. Имеет значение только для системного модуля.
        ///</summary>
        public bool IsConsoleApplicationVariable
        {
            get
            {
                return IsConsoleApplicationVariableValue.constant_value;
            }
            set
            {
                IsConsoleApplicationVariableValue.constant_value = value;
            }
        }

        public common_unit_node(SymbolTable.UnitInterfaceScope scope,
            SymbolTable.UnitImplementationScope implementation_scope,
            common_namespace_function_node main_function,
            common_namespace_function_node finalization_method)
        {
            _scope = scope;
            _implementation_scope = implementation_scope;
            _main_function = main_function;
            _finalization_method = finalization_method;
        }

        public common_unit_node()
        {
        }

        public common_unit_node(string name)
        {
            this.unit_name = name;
        }

        public void add_unit_name_to_namespace()
        {
            this.scope.AddSymbol(unit_name, new TreeConverter.SymbolInfo(this));
        }

		public common_namespace_function_node main_function
		{
			get
			{
				return _main_function;
			}
            set
            {
                _main_function = value;
            }
		}

        public common_namespace_function_node finalization_method
        {
            get
            {
                return _finalization_method;
            }
            set
            {
                _finalization_method = value;
            }
        }

		public common_namespace_node_list namespaces
		{
			get
			{
				return _namespaces;
			}
		}

		public SymbolTable.UnitInterfaceScope scope
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
		
		private List<string> _used_namespaces = new List<string>();
		
		public List<string> used_namespaces
		{
			get
			{
				return _used_namespaces;
			}
		}
		
		public SymbolTable.UnitImplementationScope implementation_scope
		{
			get
			{
				return _implementation_scope;
			}
            set
            {
                _implementation_scope = value;
            }
		}

		public override semantic_node_type semantic_node_type
		{
			get
			{
				return semantic_node_type.common_unit_node;
			}
		}

        public override List<TreeConverter.SymbolInfo> find_only_in_namespace(string name)
		{
			return _scope.FindOnlyInScope(name);
		}
	}

}
