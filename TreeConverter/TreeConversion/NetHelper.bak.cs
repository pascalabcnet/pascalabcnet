using System;
using SemanticTree;
using System.Collections;
using System.Reflection;

namespace TreeConverter {

	public class NetScope : SymbolTable.DotNETScope {
		//private base_scope _up_scope;
		
		private using_namespaceArrayList _unar;
		private System.Reflection.Assembly _assembly;

		//For SymbolTable DEBUGGING
		private NetIntScope _nis;

		public NetScope(using_namespaceArrayList unar,System.Reflection.Assembly assembly)
		{
			_unar=unar;
			_assembly=assembly;
			NetHelper.init_namespaces(assembly);

			//For SymbolTable DEBUGGING
			_nis=new NetIntScope(this);
			_sc=_nis;
		}

		public using_namespaceArrayList used_namespaces
		{
			get
			{
				return _unar;
			}
		}
		
		/*public override base_scope top_scope
		{
			get
			{
				return _up_scope;
			}
		}*/

		public override SymbolInfo Find(string name)
		{
			SymbolInfo si=null;
			if (NetHelper.IsNetNamespace(name.ToLower()) == true)
			{
				compiled_namespace_node cnn = new compiled_namespace_node(name);
				si = new SymbolInfo(cnn);
			}
			else
			{
				//Type t = Type.GetType("System."+name,false,true);
				Type t=null;
				int i=0;
				t = NetHelper.FindType(name);
				if (t != null)
				{
					compiled_type_node ctn = compiled_type_node.get_type_node(t);
					ctn.Scope = new NetTypeScope(ctn.compiled_type);
					si = new SymbolInfo(ctn);
				}
				else {
				while ((t==null)&&(i<_unar.Count))
				{
					t=_assembly.GetType(_unar[i].namespace_name+"."+name,false,true);
					i++;
				}
				if (t != null)
				{
					compiled_type_node ctn = compiled_type_node.get_type_node(t);
					ctn.Scope = new NetTypeScope(ctn.compiled_type);
					si = new SymbolInfo(ctn);
					NetHelper.AddType(name,t);
				}
				}
			}
			return si;
		}
	}
	
	public class NetTypeScope : SymbolTable.DotNETScope {
		//private base_scope _up_scope;
		private Type type_info;

		//For SymbolTable DEBUGGING
		private NetIntTypeScope _nits;
		
		public NetTypeScope(Type type_info)
		{
			this.type_info = type_info;

			//For SymbolTable DEBUGGING
			_nits=new NetIntTypeScope(this);
			_sc=_nits;
		}
		
		/*public override base_scope top_scope
		{
			get
			{
				return _up_scope;
			}
		}*/
		
		public Type TypeInfo {
			get {
				return type_info;
			}
			
			set {
				type_info = value;
			}
		}
		
		public override SymbolInfo Find(string name)
		{
			/*MethodInfo[] mis = NetHelper.FindMethods(type_info,"get_"+name);
			if (mis.Length == 0)
			mis = NetHelper.FindMethods(type_info,name);
			SymbolInfo si=null;
			if (mis.Length != 0)
			{
				si = new SymbolInfo(NetHelper.GetMethodNode(mis[0]));
				SymbolInfo temp=si;
			 	for (int i=1; i<mis.Length; i++)
			 	{
			 		temp.Next = new SymbolInfo(NetHelper.GetMethodNode(mis[i]));
					temp=temp.Next;
			 	}
			}*/
			return NetHelper.FindName(type_info,name);
			//return si;
		}
	}
	
	public class NetHelper {
		private static Hashtable namespaces; 
		private static Hashtable types;
		/*private static Hashtable methods;
		private static Hashtable properties;
		private static Hashtable fields;*/
		private static Hashtable members;
		private static Hashtable meth_nodes;
		private static Hashtable prop_nodes;
		private static Hashtable field_nodes;
		
		private static Hashtable stand_types;
		private static Hashtable oper_names;
		
		public static void init_namespaces(System.Reflection.Assembly _assembly)
		{
			Type[] tarr=_assembly.GetTypes();
			foreach(Type t in tarr)
			{
				if (t.Namespace != "" && t.Namespace != null)
				{
					string s = t.Namespace.ToLower();
					namespaces[s] = t;
					int pos = s.LastIndexOf('.');
					if (pos != -1) namespaces[s.Substring(0,pos)]=t;
				}
			}
		}

		static NetHelper() {
			//scope = new NetScope(null,null);
			namespaces = new Hashtable(1024);
			types = new Hashtable(128);
			//methods = new Hashtable();
			//properties = new Hashtable();
			members = new Hashtable(128);
			//fields = new Hashtable();
			meth_nodes = new Hashtable();
			prop_nodes = new Hashtable();
			field_nodes = new Hashtable();
			stand_types = new Hashtable();
			oper_names = new Hashtable();
			
			stand_types[typeof(int)]=stand_types;
			stand_types[typeof(byte)]=stand_types;
			stand_types[typeof(bool)]=stand_types;
			stand_types[typeof(sbyte)]=stand_types;
			stand_types[typeof(short)]=stand_types;
			stand_types[typeof(ushort)]=stand_types;
			stand_types[typeof(uint)]=stand_types;
			stand_types[typeof(long)]=stand_types;
			stand_types[typeof(ulong)]=stand_types;
			stand_types[typeof(char)]=stand_types;
			stand_types[typeof(float)]=stand_types;
			stand_types[typeof(double)]=stand_types;
			stand_types[typeof(decimal)]=stand_types;
			
			oper_names[compiler_string_consts.plus_name] = "op_Addition";
			oper_names[compiler_string_consts.minus_name] = "op_Subtraction";
			oper_names[compiler_string_consts.mul_name] = "op_Multiply";
			oper_names[compiler_string_consts.div_name] = "op_Division";
			oper_names[compiler_string_consts.idiv_name] = "op_Division";
			oper_names[compiler_string_consts.and_name] = "op_BitwiseAnd";
			oper_names[compiler_string_consts.or_name] = "op_BitwiseOr";
			oper_names[compiler_string_consts.eq_name] = "op_Equality";
			oper_names[compiler_string_consts.gr_name] = "op_GreaterThan";
			oper_names[compiler_string_consts.greq_name] = "op_GreaterThanOrEqual";
			oper_names[compiler_string_consts.sm_name] = "op_LessThan";
			oper_names[compiler_string_consts.smeq_name] = "op_LessThanOrEqual";
			oper_names[compiler_string_consts.mod_name] = "op_Modulus";
			oper_names[compiler_string_consts.not_name] = "op_LogicalNot";
			oper_names[compiler_string_consts.noteq_name] = "op_Inequality";
		}
		
		public static bool IsStandType(Type t)
		{
			if (stand_types[t] != null) return true;
			return false;
		}
		public static bool IsNetNamespace(string name)
		{
			if (namespaces[name] != null)
				return true;
			return false;
		}
		
		/*public static SymbolInfo FindField(Type t, string name, Type[] interfaces)
		{
			SymbolInfo si=null;
			FieldInfo[] mis = FindFields(t, name);
			if (mis.Length == 0) {
				foreach (Type ti in interfaces)
				{
					mis = FindFields(ti,name);
					if (mis.Length>0) break;
				}
			}
			if (mis.Length != 0)
			{
				//Console.WriteLine(mis[0].GetAccessors()[0].Name);
				si = new SymbolInfo(GetFieldNode(mis[0]));
				SymbolInfo temp=si;
			 	for (int i=1; i<mis.Length; i++)
			 	{
			 		temp.Next = new SymbolInfo(GetFieldNode(mis[i]));
					temp=temp.Next;
			 	}
			 	//Console.WriteLine(name);
			}
			return si;
		}
		
		public static SymbolInfo FindProperty(Type t, string name, Type[] interfaces)
		{
			SymbolInfo si=null;
			PropertyInfo[] mis = FindProperties(t, name);
			if (mis.Length == 0) {
				foreach (Type ti in interfaces)
				{
					mis = FindProperties(ti,name);
					if (mis.Length>0) break;
				}
			}
			if (mis.Length != 0)
			{
				//Console.WriteLine(mis[0].GetAccessors()[0].Name);
				si = new SymbolInfo(GetPropertyNode(mis[0]));
				SymbolInfo temp=si;
			 	for (int i=1; i<mis.Length; i++)
			 	{
			 		temp.Next = new SymbolInfo(GetPropertyNode(mis[i]));
					temp=temp.Next;
			 	}
			}
			return si;
		}
		
		public static SymbolInfo FindName(Type t, string name)
		{
			SymbolInfo si=null;
			Type[] tis = t.GetInterfaces();
			si = FindProperty(t,name,tis);
			if (si != null) return si;
			si = FindField(t,name,tis);
			if (si != null) return si;
			MethodInfo[] mis = FindMethods(t,name);
			if (mis.Length == 0)
			{
				foreach (Type ti in tis)
				{
					mis = FindMethods(ti,name);
					if (mis.Length>0) break;
				}
			}
			if (mis.Length != 0)
			{
				si = new SymbolInfo(GetMethodNode(mis[0]));
				SymbolInfo temp=si;
			 	for (int i=1; i<mis.Length; i++)
			 	{
			 		temp.Next = new SymbolInfo(GetMethodNode(mis[i]));
					temp=temp.Next;
			 	}
			}
			return si;
		}*/
		
		public static MemberInfo[] GetMembers(Type t, string name)
		{
			object o = members[t];
			if (o == null)
			{
				MemberInfo[] mis = t.GetMembers();
				Hashtable ht = new Hashtable();
				foreach (MemberInfo mi2 in mis)
				{
					//Console.WriteLine(mi2.Name.ToLower());
					string s = mi2.Name.ToLower();
					if (ht[s] == null)
					 ht[s]=new ArrayList();
				}
				foreach (MemberInfo mi in mis)
				{
				 	((ArrayList)ht[mi.Name.ToLower()]).Add(mi);
				}
				members[t] = ht;
				ArrayList lst = (ArrayList)ht[name.ToLower()];
				if (lst == null) return new MemberInfo[0];	
				return (MemberInfo[])lst.ToArray(typeof(MemberInfo));
			}
			else
			{
				Hashtable ht = (Hashtable)o;
				ArrayList mis2 = (ArrayList)ht[name.ToLower()];
				if (mis2 == null) return new MemberInfo[0];
				MemberInfo[] mi2 = (MemberInfo[])mis2.ToArray(typeof(MemberInfo));
				return mi2;
			}
		}
		
		public static SymbolInfo FindName(Type t, string name)
		{
			string s = (string)oper_names[name];
			if (s != null) name = s; 
			SymbolInfo si=null;
			Type[] tis = t.GetInterfaces();
			MemberInfo[] mis = GetMembers(t,name);
			if (mis.Length == 0)
			{
				foreach (Type tt in tis)
				{
					mis = GetMembers(tt,name);
					if (mis.Length > 0) break;
				}
			}
			if (mis.Length != 0)
			{
				//Console.WriteLine(mis[0].GetAccessors()[0].Name);
				if (mis[0] is MethodInfo)
				{
					si = new SymbolInfo(GetMethodNode((MethodInfo)mis[0]));
					SymbolInfo temp=si;
			 		for (int i=1; i<mis.Length; i++)
			 		{
			 			temp.Next = new SymbolInfo(GetMethodNode((MethodInfo)mis[i]));
						temp=temp.Next;
			 		}
				}
				else if (mis[0] is PropertyInfo)
				{
					si = new SymbolInfo(GetPropertyNode((PropertyInfo)mis[0]));
					SymbolInfo temp=si;
			 		for (int i=1; i<mis.Length; i++)
			 		{
			 			temp.Next = new SymbolInfo(GetPropertyNode((PropertyInfo)mis[i]));
						temp=temp.Next;
			 		}
				}
				else if (mis[0] is FieldInfo)
				{
					si = new SymbolInfo(GetFieldNode((FieldInfo)mis[0]));
					SymbolInfo temp=si;
			 		for (int i=1; i<mis.Length; i++)
			 		{
			 			temp.Next = new SymbolInfo(GetFieldNode((FieldInfo)mis[i]));
						temp=temp.Next;
			 		}
				}
			}
			return si;
		}
		
		public static compiled_function_node GetMethodNode(MethodInfo mi)
		{
			compiled_function_node cfn = (compiled_function_node)meth_nodes[mi];
			if (cfn != null) return cfn;
			cfn = new compiled_function_node(mi);
			meth_nodes[mi] = cfn;
			return cfn;
		}
		
		public static compiled_property_node GetPropertyNode(PropertyInfo pi)
		{
			compiled_property_node cpn = (compiled_property_node)prop_nodes[pi];
			if (cpn != null) return cpn;
			cpn = new compiled_property_node(pi);
			
			prop_nodes[pi] = cpn;
			return cpn;
		}
		
		public static compiled_variable_definition GetFieldNode(FieldInfo pi)
		{
			compiled_variable_definition cpn = (compiled_variable_definition)field_nodes[pi];
			if (cpn != null) return cpn;
			cpn = new compiled_variable_definition(pi);
			
			field_nodes[pi] = cpn;
			return cpn;
		}
		
		public static Type FindType(string name)
		{
			return (Type)types[name];
		}
		
		public static void AddType(string name, Type t)
		{
			types[name] = t;
		}
		
		public static Type FindTypeOrCreate(string name)
		{
			Type t = (Type)types[name];
			if (t != null) return t;
			t = Type.GetType(name);
			types[name] = t;
			return t;
		}
		/*public static MethodInfo[] FindMethods(Type t, string name)
		{
			object o = methods[t];
			if (o == null)
			{
				Hashtable ht = new Hashtable();
				MethodInfo[] mis = t.GetMethods();
				//Console.WriteLine(mis.Length);
				foreach (MethodInfo mi2 in mis)
					ht[mi2.Name.ToLower()]=new ArrayList();
				foreach (MethodInfo mi in mis)
				{
				 	((ArrayList)ht[mi.Name.ToLower()]).Add(mi);
				}
				methods[t] = ht;
				ArrayList lst = (ArrayList)ht[name.ToLower()];
				if (lst == null) return new MethodInfo[0];	
				return (MethodInfo[])lst.ToArray(typeof(MethodInfo));
			}
			else
			{
				Hashtable ht = (Hashtable)o;
				ArrayList mis = (ArrayList)ht[name.ToLower()];
				if (mis == null) return new MethodInfo[0];
				MethodInfo[] mi2 = (MethodInfo[])mis.ToArray(typeof(MethodInfo));
				return mi2;
			}
			//return null;
		}
		
		public static PropertyInfo[] FindProperties(Type t, string name)
		{
			object o = properties[t];
			if (o == null)
			{
				Hashtable ht = new Hashtable();
				PropertyInfo[] mis = t.GetProperties();
				//Console.WriteLine(mis.Length);
				foreach (PropertyInfo mi2 in mis)
					ht[mi2.Name.ToLower()]=new ArrayList();
				foreach (PropertyInfo mi in mis)
				{
				 	((ArrayList)ht[mi.Name.ToLower()]).Add(mi);
				}
				properties[t] = ht;
				ArrayList lst = (ArrayList)ht[name.ToLower()];
				if (lst == null) return new PropertyInfo[0];	
				return (PropertyInfo[])lst.ToArray(typeof(PropertyInfo));
			}
			else
			{
				Hashtable ht = (Hashtable)o;
				ArrayList mis = (ArrayList)ht[name.ToLower()];
				if (mis == null) return new PropertyInfo[0];
				PropertyInfo[] mi2 = (PropertyInfo[])mis.ToArray(typeof(PropertyInfo));
				return mi2;
			}
			//return null;
		}
		
		public static FieldInfo[] FindFields(Type t, string name)
		{
			object o = fields[t];
			if (o == null)
			{
				Hashtable ht = new Hashtable();
				FieldInfo[] mis = t.GetFields();
				//Console.WriteLine(mis.Length);
				foreach (FieldInfo mi2 in mis)
					ht[mi2.Name.ToLower()]=new ArrayList();
				foreach (FieldInfo mi in mis)
				{
				 	((ArrayList)ht[mi.Name.ToLower()]).Add(mi);
				}
				fields[t] = ht;
				ArrayList lst = (ArrayList)ht[name.ToLower()];
				if (lst == null) return new FieldInfo[0];	
				return (FieldInfo[])lst.ToArray(typeof(FieldInfo));
			}
			else
			{
				Hashtable ht = (Hashtable)o;
				ArrayList mis = (ArrayList)ht[name.ToLower()];
				if (mis == null) return new FieldInfo[0];
				FieldInfo[] mi2 = (FieldInfo[])mis.ToArray(typeof(FieldInfo));
				return mi2;
			}
			//return null;
		}*/
		
		/*public static compiled_type_node GetTypeNode(Type t)
		{
			object o = types[t];
			if (o != null) return (compiled_type_node)o;
			compiled_type_node ctn = new compiled_type_node(t);
			types[t] = ctn;
			return ctn;
		}*/
		
		/*public compiled_namespace_node GetNamespaceNode(Type t)
		{
			object o = types[t];
			if (o != null) return (compiled_namespace_node)o;
			compiled_namespace_node ctn = new compiled_type_node(t);
			types[t] = ctn;
			return ctn;
		}*/
	}
}

