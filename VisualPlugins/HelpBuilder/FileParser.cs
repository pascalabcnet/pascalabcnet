using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using PascalABCCompiler.SyntaxTree;
using System.Xml;
using System.Text;
using System.IO;
using PascalABCCompiler.Errors;
using PascalABCCompiler.SemanticTree;

namespace VisualPascalABCPlugins
{
	public class FileParser
	{
		IVisualEnvironmentCompiler VisualEnvironmentCompiler;
		Hashtable symtab = new Hashtable();
		IProgramNode tree;
		public ICommonNamespaceNode main_ns;
		string Name;
		internal List<ICommonNamespaceNode> namespaces = new List<ICommonNamespaceNode>();
		
		public FileParser(IVisualEnvironmentCompiler vec)
		{
			this.VisualEnvironmentCompiler = vec;
		}
		
		public void Clear()
		{
			symtab.Clear();
			main_ns = null;
			tree = null;
			namespaces.Clear();
		}
		
		public void set_ns(int i)
		{
			main_ns = namespaces[i];
		}
		
		public bool Parse(string FileName)
		{
			Name = Path.GetFileNameWithoutExtension(FileName);
			bool tmp_code_gen = VisualEnvironmentCompiler.StandartCompiler.CompilerOptions.GenerateCode;
            bool tmp_save_pcu = VisualEnvironmentCompiler.StandartCompiler.CompilerOptions.SavePCU;
			VisualEnvironmentCompiler.StandartCompiler.CompilerOptions.GenerateCode = false;
            VisualEnvironmentCompiler.StandartCompiler.CompilerOptions.SavePCU = false;
            VisualEnvironmentCompiler.StandartCompiler.InternalDebug.AlwaysGenerateXMLDoc = true;
            PascalABCCompiler.CompilerType ct = VisualEnvironmentCompiler.DefaultCompilerType;
            VisualEnvironmentCompiler.DefaultCompilerType = PascalABCCompiler.CompilerType.Standart;
            try
            {
            	VisualEnvironmentCompiler.ExecuteAction(VisualEnvironmentCompilerAction.BuildUnit, FileName);
            }
            catch (System.Exception e)
            {
            	throw;
            }
            VisualEnvironmentCompiler.DefaultCompilerType = ct;
            VisualEnvironmentCompiler.StandartCompiler.CompilerOptions.GenerateCode = tmp_code_gen;
            VisualEnvironmentCompiler.StandartCompiler.CompilerOptions.SavePCU = tmp_save_pcu;
            VisualEnvironmentCompiler.StandartCompiler.InternalDebug.AlwaysGenerateXMLDoc = false;
            if (tree != null)
            {
            	return true;
            }
            return false;
		}
		
		internal void SetSemanticTree(IProgramNode tree)
		{
			this.tree = tree;
			if (tree != null)
			foreach (ICommonNamespaceNode cnn in tree.namespaces)
			if (string.Compare(cnn.namespace_name,Name,true)==0)
			{
				main_ns = cnn;
				namespaces.Add(cnn);
				break;
			}
			
				
		}
		
		public bool InTree(ITypeNode type)
		{
			foreach (ICommonNamespaceNode cnn in namespaces)
			foreach(ITypeNode t in cnn.types)
			{
				if (t == type)
					return true;
				if (t is INamespaceMemberNode && type is INamespaceMemberNode && string.Compare(t.name,type.name,true) == 0 &&
				    string.Compare((t as INamespaceMemberNode).comprehensive_namespace.namespace_name,(t as INamespaceMemberNode).comprehensive_namespace.namespace_name,true) == 0)
					return true;
				
			}
			return false;
		}
		
		public ITypeNode[] GetParentClasses(ITypeNode ctn)
		{
			ctn = ctn.base_type;
			List<ITypeNode> lst = new List<ITypeNode>();
			while (ctn != null)
			{
				lst.Insert(0,ctn);
				ctn = ctn.base_type;
			}
			return lst.ToArray();
		}
		
		public ICommonNamespaceFunctionNode[] GetFunctions()
		{
			List<ICommonNamespaceFunctionNode> lst = new List<ICommonNamespaceFunctionNode>();
			foreach (ICommonNamespaceFunctionNode f in main_ns.functions)
				if (HelpUtils.can_write(f))
				lst.Add(f);
			return lst.ToArray();
		}
		
		public ICommonNamespaceVariableNode[] GetVariables()
		{
			List<ICommonNamespaceVariableNode> lst = new List<ICommonNamespaceVariableNode>();
			foreach (ICommonNamespaceVariableNode f in main_ns.variables)
				if (HelpUtils.can_write(f))
				lst.Add(f);
			return lst.ToArray();
		}
		
		public INamespaceConstantDefinitionNode[] GetNamespaceConstants()
		{
			List<INamespaceConstantDefinitionNode> lst = new List<INamespaceConstantDefinitionNode>();
			foreach (INamespaceConstantDefinitionNode f in main_ns.constants)
				if (HelpUtils.can_write(f))
				lst.Add(f);
			return lst.ToArray();
		}
		
		public ITypeNode[] GetChildClasses(ICommonTypeNode ctn)
		{
			List<ITypeNode> lst = new List<ITypeNode>();
			foreach(ITypeNode t in main_ns.types)
			{
				if (t.base_type == ctn)
					lst.Add(t);
			}
			return lst.ToArray();
		}
		
		public ICommonMethodNode[] GetConstructors(ICommonTypeNode ctn)
		{
			List<ICommonMethodNode> lst = new List<ICommonMethodNode>();
			foreach (ICommonMethodNode m in ctn.methods)
				if (m.is_constructor && m.field_access_level != field_access_level.fal_private && m.polymorphic_state != polymorphic_state.ps_static
				   && HelpUtils.can_write(m))
				lst.Add(m);
			return lst.ToArray();
		}
		
		class EventComparer : IComparer<ICommonEventNode>
		{
			public int Compare(ICommonEventNode x, ICommonEventNode y)
			{
				return string.Compare(x.Name,y.Name,true);
			}
		}
		
		class ConstantComparer : IComparer<IClassConstantDefinitionNode>
		{
			public int Compare(IClassConstantDefinitionNode x, IClassConstantDefinitionNode y)
			{
				return string.Compare(x.name,y.name,true);
			}
		}
		
		class PropertyComparer : IComparer<ICommonPropertyNode>
		{
			public int Compare(ICommonPropertyNode x, ICommonPropertyNode y)
			{
				return string.Compare(x.name,y.name,true);
			}
		}
		
		class MethodComparer : IComparer<ICommonMethodNode>
		{
			public int Compare(ICommonMethodNode x, ICommonMethodNode y)
			{
				return string.Compare(x.name,y.name,true);
			}
		}
		
		class FieldComparer : IComparer<ICommonClassFieldNode>
		{
			public int Compare(ICommonClassFieldNode x, ICommonClassFieldNode y)
			{
				return string.Compare(x.name,y.name,true);
			}
		}
		
		internal bool is_getter_or_setter(ICommonMethodNode meth)
		{
			if (meth.common_comprehensive_type != null)
			{
				ICommonPropertyNode[] props = GetProperties(meth.common_comprehensive_type);
				foreach (ICommonPropertyNode p in props)
				if (p.get_function == meth || p.set_function == meth)
					return true;
			}
			return false;
		}
		
		internal bool is_event_special_method(ICommonMethodNode meth)
		{
			if (meth.common_comprehensive_type != null)
			{
				ICommonEventNode[] events = meth.common_comprehensive_type.events;
				foreach (ICommonEventNode e in events)
					if (e.AddMethod == meth || e.RaiseMethod == meth || e.RemoveMethod == meth)
					return true;
			}
			return false;
		}
		
		private bool is_getter_or_setter(ICommonMethodNode meth, ICommonPropertyNode[] props)
		{
			foreach (ICommonPropertyNode p in props)
			if (p.get_function == meth || p.set_function == meth)
				return true;
			return false;
		}
		
		public int GetConstructorIndex(ICommonMethodNode cmn)
		{
			if (cmn.common_comprehensive_type != null)
			{
				ICommonMethodNode[] meths = cmn.common_comprehensive_type.methods;
				int i=1;
				foreach (ICommonMethodNode m in meths)
				{
					if (m == cmn)
						return i;
					else if (m.is_constructor)
						i++;
				}
			}
			return 1;
		}
		
		public int GetMethodIndex(ICommonMethodNode cmn, ICommonTypeNode ctn)
		{
			int i=1;
			while (ctn != null)
			{
				ICommonMethodNode[] meths = ctn.methods;
				foreach (ICommonMethodNode m in meths)
				{
					if (m == cmn)
						return i;
					else if (string.Compare(m.name,cmn.name,true)==0 && cmn.overrided_method != m)
						i++;
				}
				ctn = ctn.base_type as ICommonTypeNode;
			}
			return 1;
		}
		
		public bool is_overloaded_constructor(ICommonMethodNode meth)
		{
			int count = 1;
			if (meth.common_comprehensive_type != null)
			foreach (ICommonMethodNode m in meth.common_comprehensive_type.methods)
			{
				if (m.is_constructor && meth != m)
					count++;
			}
			return count > 1;
		}
		
		public bool is_overload(ICommonMethodNode meth, ICommonTypeNode ctn)
		{
			int count = 1;
			while (ctn != null)
			{
				foreach (ICommonMethodNode m in ctn.methods)
				{
					if (string.Compare(meth.name,m.name,true)==0 && meth != m && meth.overrided_method != m)
						count++;
				}
				ctn = ctn.base_type as ICommonTypeNode;
			}
			return count > 1;
		}
		
		public bool is_overload(ICommonNamespaceFunctionNode func)
		{
			int count = 1;
			if (func.comprehensive_namespace != null)
			foreach (ICommonNamespaceFunctionNode f in func.comprehensive_namespace.functions)
			{
				if (string.Compare(f.name,func.name,true)==0 && f != func)
					count++;
			}
			return count > 1;
		}
		
		public List<ICommonNamespaceFunctionNode> GetOverloadsList(ICommonNamespaceNode ns, ICommonNamespaceFunctionNode f)
		{
			List<ICommonNamespaceFunctionNode> lst = new List<ICommonNamespaceFunctionNode>();
			foreach (ICommonNamespaceFunctionNode m in ns.functions)
			{
				if (string.Compare(m.name,f.name,true)==0)
					lst.Add(m);
			}
			return lst;
		}
		
		public List<ICommonMethodNode> GetOverloadedConstructors(ICommonTypeNode t)
		{
			List<ICommonMethodNode> lst = new List<ICommonMethodNode>();
			foreach (ICommonMethodNode m in t.methods)
			{
				if (m.is_constructor)
					lst.Add(m);
			}
			return lst;
		}
		
		public List<ICommonMethodNode> GetOverloadsList(ICommonTypeNode t, ICommonMethodNode meth)
		{
			
			List<ICommonMethodNode> lst = new List<ICommonMethodNode>();
			while (t != null)
			{
				foreach (ICommonMethodNode m in t.methods)
				{
					if (string.Compare(m.name,meth.name,true)==0 && meth.overrided_method != m)
						lst.Add(m);
				}
				t = t.base_type as ICommonTypeNode;
			}
			return lst;
		}
		
		public int GetMethodIndex(ICommonNamespaceFunctionNode cmn)
		{
			if (cmn.comprehensive_namespace != null)
			{
				ICommonNamespaceFunctionNode[] meths = cmn.comprehensive_namespace.functions;
				int i=1;
				foreach (ICommonNamespaceFunctionNode m in meths)
				{
					if (m == cmn)
						return i;
					else if (m.name == cmn.name)
						i++;
				}
			}
			return 1;
		}
		
		public ICommonClassFieldNode[] GetDefinedFields(ICommonTypeNode ctn)
		{
			List<ICommonClassFieldNode> lst = new List<ICommonClassFieldNode>();
			foreach (ICommonClassFieldNode fld in ctn.fields)
			{
				if (!fld.name.Contains("$"))
					lst.Add(fld);
			}
			return lst.ToArray();
		}
		
		public ICommonMethodNode[] GetNonPrivateMethods(ICommonTypeNode ctn)
		{
			List<ICommonMethodNode> lst = new List<ICommonMethodNode>();
			ICommonPropertyNode[] props = GetProperties(ctn);
			while (ctn != null)
			{
				foreach (ICommonMethodNode m in ctn.methods)
				if (!m.is_constructor && m.field_access_level != field_access_level.fal_private && m.Location != null && !is_getter_or_setter(m,props)
					    && !is_event_special_method(m) && HelpUtils.can_write(m))
				{
				    lst.Add(m);
				}
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new MethodComparer());
			return lst.ToArray();
		}
		
		public ICommonMethodNode[] GetMethods(ICommonTypeNode ctn, field_access_level fal)
		{
			List<ICommonMethodNode> lst = new List<ICommonMethodNode>();
			ICommonPropertyNode[] props = GetProperties(ctn);
			while (ctn != null)
			{
				foreach (ICommonMethodNode m in ctn.methods)
				if (!m.is_constructor && m.field_access_level == fal && m.Location != null && !is_getter_or_setter(m,props)
					    && !is_event_special_method(m) && HelpUtils.can_write(m))
				{
				    lst.Add(m);
				}
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new MethodComparer());
			return lst.ToArray();
		}
		
		public ICommonTypeNode[] GetClasses()
		{
			ICommonTypeNode[] types = main_ns.types;
			List<ICommonTypeNode> lst = new List<ICommonTypeNode>();
			foreach (ICommonTypeNode t in types)
				if (!t.IsEnum && !t.is_value_type && !t.IsInterface && HelpUtils.can_show_members(t) && HelpUtils.can_write(t))
				lst.Add(t);
			return lst.ToArray();
		}
		
		public ICommonTypeNode[] GetInterfaces()
		{
			ICommonTypeNode[] types = main_ns.types;
			List<ICommonTypeNode> lst = new List<ICommonTypeNode>();
			foreach (ICommonTypeNode t in types)
				if (t.IsInterface && HelpUtils.can_write(t))
				lst.Add(t);
			return lst.ToArray();
		}
		
		public ICommonTypeNode[] GetEnumsRecords()
		{
			ICommonTypeNode[] types = main_ns.types;
			List<ICommonTypeNode> lst = new List<ICommonTypeNode>();
			foreach (ICommonTypeNode t in types)
				if ((t.IsEnum || t.is_value_type || t.type_special_kind == type_special_kind.record) && HelpUtils.can_show_members(t)
				   && HelpUtils.can_write(t))
				lst.Add(t);
			return lst.ToArray();
		}
		
		public ICommonTypeNode[] GetTypes()
		{
			List<ICommonTypeNode> lst = new List<ICommonTypeNode>();
			ICommonTypeNode[] types = main_ns.types;
			foreach (ICommonTypeNode t in types)
				if (!HelpUtils.can_show_members(t) && HelpUtils.can_write(t))
				lst.Add(t);
			return lst.ToArray();
		}
		
		public ICommonPropertyNode[] GetNonPrivateProperties(ICommonTypeNode ctn)
		{
			List<ICommonPropertyNode> lst = new List<ICommonPropertyNode>();
			while (ctn != null)
			{
				foreach (ICommonPropertyNode p in ctn.properties)
					if (p.field_access_level != field_access_level.fal_private && HelpUtils.can_write(p))
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new PropertyComparer());
			return lst.ToArray();
		}
		
		public ICommonPropertyNode[] GetProperties(ICommonTypeNode ctn)
		{
			List<ICommonPropertyNode> lst = new List<ICommonPropertyNode>();
			while (ctn != null)
			{
				foreach (ICommonPropertyNode p in ctn.properties)
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new PropertyComparer());
			return lst.ToArray();
		}
		
		public ICommonPropertyNode[] GetProperties(ICommonTypeNode ctn, field_access_level fal)
		{
			List<ICommonPropertyNode> lst = new List<ICommonPropertyNode>();
			while (ctn != null)
			{
				foreach (ICommonPropertyNode p in ctn.properties)
					if (p.field_access_level == fal && HelpUtils.can_write(p))
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new PropertyComparer());
			return lst.ToArray();
		}
		
		public ICommonEventNode[] GetEvents(ICommonTypeNode ctn, field_access_level fal)
		{
			List<ICommonEventNode> lst = new List<ICommonEventNode>();
			while (ctn != null)
			{
				foreach (ICommonEventNode p in ctn.events)
					if (p.field_access_level == fal && HelpUtils.can_write(p))
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new EventComparer());
			return lst.ToArray();
		}
		
		public ICommonEventNode[] GetNonPrivateEvents(ICommonTypeNode ctn)
		{
			List<ICommonEventNode> lst = new List<ICommonEventNode>();
			while (ctn != null)
			{
				foreach (ICommonEventNode p in ctn.events)
					if (p.field_access_level != field_access_level.fal_private && HelpUtils.can_write(p))
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new EventComparer());
			return lst.ToArray();
		}
		
		public IClassConstantDefinitionNode[] GetConstants(ICommonTypeNode ctn, field_access_level fal)
		{
			List<IClassConstantDefinitionNode> lst = new List<IClassConstantDefinitionNode>();
			while (ctn != null)
			{
				foreach (IClassConstantDefinitionNode p in ctn.constants)
					if (p.field_access_level == fal && HelpUtils.can_write(p))
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new ConstantComparer());
			return lst.ToArray();
		}
		
		public IClassConstantDefinitionNode[] GetNonPrivateClassConstants(ICommonTypeNode ctn)
		{
			List<IClassConstantDefinitionNode> lst = new List<IClassConstantDefinitionNode>();
			while (ctn != null)
			{
				foreach (IClassConstantDefinitionNode p in ctn.constants)
					if (p.field_access_level != field_access_level.fal_private && HelpUtils.can_write(p))
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new ConstantComparer());
			return lst.ToArray();
		}
		
		public IClassConstantDefinitionNode[] GetConstants(ICommonTypeNode ctn, string name)
		{
			List<IClassConstantDefinitionNode> lst = new List<IClassConstantDefinitionNode>();
			foreach (IClassConstantDefinitionNode p in ctn.constants)
				if (string.Compare(p.name,name,true)==0)
				    lst.Add(p);
			return lst.ToArray();
		}
		
		public ICommonClassFieldNode[] GetNonPrivateFields(ICommonTypeNode ctn)
		{
			List<ICommonClassFieldNode> lst = new List<ICommonClassFieldNode>();
			while (ctn != null)
			{
				foreach (ICommonClassFieldNode p in ctn.fields)
					if (p.field_access_level != field_access_level.fal_private && HelpUtils.can_write(p))
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new FieldComparer());
			return lst.ToArray();
		}
		
		public ICommonClassFieldNode[] GetFields(ICommonTypeNode ctn, field_access_level fal)
		{
			List<ICommonClassFieldNode> lst = new List<ICommonClassFieldNode>();
			while (ctn != null)
			{
				foreach (ICommonClassFieldNode p in ctn.fields)
					if (p.field_access_level == fal && HelpUtils.can_write(p))
				    	lst.Add(p);
				ctn = ctn.base_type as ICommonTypeNode;
			}
			lst.Sort(new FieldComparer());
			return lst.ToArray();
		}
		
		public ICommonMethodNode[] GetMethods(ICommonTypeNode t, string name)
		{
			List<ICommonMethodNode> lst = new List<ICommonMethodNode>();
			foreach (ICommonMethodNode m in t.methods)
			if (string.Compare(m.name,name,true)==0)
				lst.Add(m);
			return lst.ToArray();
		}
	}
}

