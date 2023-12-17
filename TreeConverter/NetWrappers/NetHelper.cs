// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using PascalABCCompiler.SemanticTree;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

using PascalABCCompiler.TreeConverter;
using PascalABCCompiler.TreeRealization;

namespace PascalABCCompiler.NetHelper 
{
    class TypeInfo
    {
        public Type type;
        public string FullName;

        public TypeInfo(Type type, string FullName)
        {
            this.type = type;
            this.FullName = FullName;
        }
    }

    class TypeNamespaceInfo
    {
        public TypeInfo type_info;
        public using_namespace us_ns;

        public TypeNamespaceInfo(TypeInfo type_info, using_namespace us_ns)
        {
            this.type_info = type_info;
            this.us_ns = us_ns;
        }
    }

    class FoundInfo
    {
        public bool exists;
        public List<TypeNamespaceInfo> type_infos;

        public FoundInfo(bool exists, TypeInfo TypeInfo=null, using_namespace un=null)
        {
            this.exists = exists;
            if (exists)
            {
                type_infos = new List<TypeNamespaceInfo>();
                type_infos.Add(new TypeNamespaceInfo(TypeInfo,un));
            }
        }

        public TypeInfo GetTypeByNamespaceList(using_namespace_list unl)
        {
            foreach (TypeNamespaceInfo tni in type_infos)
            {
                foreach (using_namespace un in unl)
                {
                    if (tni.us_ns != null && string.Compare(un.namespace_name, tni.us_ns.namespace_name, true) == 0)
                    {
                        return tni.type_info;
                    }
                }
                if (tni.us_ns == null)
                    return tni.type_info;
            }
            return null;
        }
        
        public List<TypeInfo> GetTypesByNamespaceList(using_namespace_list unl)
        {
            List<TypeInfo> types = new List<TypeInfo>();
            foreach (TypeNamespaceInfo tni in type_infos)
            {
                foreach (using_namespace un in unl)
                {
                    if (tni.us_ns != null && string.Compare(un.namespace_name, tni.us_ns.namespace_name, true) == 0)
                    {
                        types.Add(tni.type_info);
                    }
                }
                if (tni.us_ns == null)
                    types.Add(tni.type_info);
            }
            return types;
        }
    }

	public class NetScope : SymbolTable.DotNETScope {
		//private base_scope _up_scope;

        private PascalABCCompiler.TreeRealization.using_namespace_list _unar;
		internal System.Reflection.Assembly _assembly;

		private SymbolTable.TreeConverterSymbolTable _tcst;
		internal Type entry_type = null;
        private List<Type> UnitTypes = null;

        public NetScope(PascalABCCompiler.TreeRealization.using_namespace_list unar,
            SymbolTable.TreeConverterSymbolTable tcst) : base(tcst)
        {
            _unar = unar;

            _tcst = tcst;
        }

        public NetScope(PascalABCCompiler.TreeRealization.using_namespace_list unar,System.Reflection.Assembly assembly,
			SymbolTable.TreeConverterSymbolTable tcst) : base(tcst)
		{
			_unar=unar;
			_assembly=assembly;
			UnitTypes = NetHelper.init_namespaces(assembly);
            if (UnitTypes.Count > 0)
            {
                entry_type = GetEntryType();
            }

			_tcst=tcst;
		}

        private Type GetEntryType()
        {
            if (_unar.Count > 0)
            {
                for (int i = 0; i < UnitTypes.Count; i++)
                {
                    if (string.Compare(UnitTypes[i].Name, _unar[0].namespace_name, true) == 0)
                        return UnitTypes[i];
                }
                return UnitTypes[0];
            }
            else
            {
                return UnitTypes[0];
            }
        }

        public Assembly Assembly
        {
            get
            {
                return _assembly;
            }
        }

        public PascalABCCompiler.TreeRealization.using_namespace_list used_namespaces
		{
			get
			{
				return _unar;
			}
            set
            {
                _unar = value;
            }
		}
		
		/*public override base_scope top_scope
		{
			get
			{
				return _up_scope;
			}
		}*/

        public override List<SymbolInfo> Find(string name, SymbolTable.Scope CurrentScope)
        {
            List<SymbolInfo> sil = null;
            string full_ns = null;
            if (NetHelper.IsNetNamespace(name, _unar, out full_ns) == true)
            {
                compiled_namespace_node cnn = compiled_namespace_node.get_compiled_namespace(full_ns, _tcst);//new compiled_namespace_node(full_ns, _tcst);
                sil = new List<SymbolInfo> { new SymbolInfo(cnn) };
            }
            else
            {
                //Type t = Type.GetType("System."+name,false,true);
                Type t = null;
                t = NetHelper.FindType(name, _unar);
                if (t != null)
                {
                    compiled_type_node ctn = compiled_type_node.get_type_node(t, _tcst);
                    sil = new List<SymbolInfo> { new SymbolInfo(ctn) };
                }
                else
                {
                    if (entry_type != null)
                    {
                        type_node pas_tn = NetHelper.FindCompiledPascalType(entry_type.Namespace + "." + name);
                        if (pas_tn != null)
                        {
                            sil = new List<SymbolInfo> { new SymbolInfo(pas_tn) };
                            return sil;
                        }
                        else
                        {
                            template_class tc = NetHelper.FindCompiledTemplateType(entry_type.Namespace + "." + name);
                            if (tc != null)
                            {
                                sil = new List<SymbolInfo> { new SymbolInfo(tc) };
                                return sil;
                            }
                        }
                    }
                    if (SemanticRules.AllowGlobalVisibilityForPABCDll && entry_type != null)
                    {
                        t = NetHelper.FindType(entry_type.Namespace + "." + name);
                        if (t != null) sil = new List<SymbolInfo> { new SymbolInfo(compiled_type_node.get_type_node(t)) };
                        else
                        {
                            object[] attrs = entry_type.GetCustomAttributes(false);
                            for (int j = 0; j < attrs.Length; j++)
                                if (attrs[j].GetType().Name == PascalABCCompiler.TreeConverter.compiler_string_consts.global_attr_name)
                                {
                                    sil = NetHelper.FindName(entry_type, name);
                                    if (sil != null) break;
                                }
                        }
                        if (sil == null && UnitTypes.Count > 1)
                        {
                            for (int j = 0; j < _unar.Count; j++)
                            {
                                t = in_type_list(_unar[j].namespace_name);
                                if (t != null)
                                {
                                    sil = NetHelper.FindName(t, name);
                                    if (sil != null)
                                        break;
                                }
                            }
                        }
                    }
                }
            }
			return sil;
		}

        private Type in_type_list(string name)
        {
            for (int i = 1; i < UnitTypes.Count; i++)
            {
                if (string.Compare(UnitTypes[i].Name, name, true) == 0)
                    return UnitTypes[i];
            }
            return null;
        }
        public override string ToString()
        {
            return Assembly.ToString();
        }
    }
	
	public class NetTypeScope : SymbolTable.DotNETScope {
		//private base_scope _up_scope;
		private Type type_info;

		public NetTypeScope(Type type_info,SymbolTable.DSSymbolTable tcst) : base(tcst)
		{
			this.type_info = type_info;
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
		
        public override List<SymbolInfo> Find(string name, SymbolTable.Scope CurrentScope)
        {
            return NetHelper.FindName(type_info, name);
        }
    }
	
	public static class NetHelper {
		private static Hashtable namespaces;
		private static Hashtable types;
        private static Dictionary<string,FoundInfo> type_search_cache;
		private static Hashtable compiled_pascal_types;
		/*private static Hashtable methods;
		private static Hashtable properties;
		private static Hashtable fields;*/
        //ssyy-
		//private static Hashtable interfaces;
        //\ssyy-
		private static Dictionary<Type, Dictionary<string, List<MemberInfo>>> members;
		//private static Hashtable meth_nodes;
		private static Dictionary<PropertyInfo, compiled_property_node> prop_nodes;
		private static Hashtable field_nodes;
        private static Hashtable constr_nodes;
		private static Hashtable stand_types;
        private static Hashtable type_handles;
        private static Hashtable method_handles;
        private static Hashtable constr_handles;
        private static Hashtable field_handles;
		private static Hashtable assemblies;
        private static Hashtable special_types;
        private static Hashtable ns_types;
		private static Type memberInfo;
		private static Hashtable namespace_assemblies;
		public static Hashtable cur_used_assemblies;
        private static Hashtable cached_type_extensions;
		public static Type DelegateType;
        public static Type EnumType;
        public static Type ArrayType;
		public static Type void_type;
		public static Type MulticastDelegateType;
        public static Type ExtensionAttributeType;
        public static MethodInfo AddToDictionaryMethod;
        public static bool UsePABCRtl;
        public static Assembly SystemCoreAssembly;
        private static Dictionary<Type,MethodInfo[]> extension_methods = new Dictionary<Type,MethodInfo[]>();
        private static Dictionary<Type,List<MethodInfo>> type_extensions = new Dictionary<Type, List<MethodInfo>>();
        private static Dictionary<Type, Type> arrays_with_extension_methods = new Dictionary<Type, Type>();
        private static Dictionary<Type, Type> generics_with_extension_methods = new Dictionary<Type, Type>();
        private static Dictionary<int, List<MethodInfo>> generic_array_type_extensions = new Dictionary<int, List<MethodInfo>>();
        private static List<MethodInfo> generic_parameter_type_extensions = new List<MethodInfo>();
        public static Type PABCSystemType = null;
        public static Type PT4Type = null;
        public static Type StringType = typeof(string);
        public static Dictionary<string, List<int>> generics_names = new Dictionary<string, List<int>>();
        private static Dictionary<string, MemberInfo> member_cache = new Dictionary<string, MemberInfo>();

		public static void reset()
		{
            if (cur_used_assemblies == null)
                cur_used_assemblies = new Hashtable();
            cur_used_assemblies.Clear();
			cur_used_assemblies[typeof(string).Assembly] = typeof(string).Assembly;
			cur_used_assemblies[typeof(Microsoft.CSharp.CSharpCodeProvider).Assembly] = typeof(Microsoft.CSharp.CSharpCodeProvider).Assembly;
            type_search_cache.Clear();

		}
		
		private static Hashtable ass_name_cache;
		private static Hashtable file_dates;
        private static Dictionary<Assembly, string> assm_full_paths;


        public static bool IsAssemblyChanged(string name)
		{
			if (name == null) return false;
			Assembly a = ass_name_cache[name] as Assembly;
			if (a != null)
			{
				if (System.IO.File.GetLastWriteTime(name) != (DateTime)file_dates[a])
					return true;
				return false;
			}
			else 
			return false;
		}

		public static Assembly LoadAssembly(string name, bool use_load_from = false)
		{
            if (name == null) return null;
			Assembly a = ass_name_cache[name] as Assembly;
            if (a == null && name.IndexOf(System.IO.Path.DirectorySeparatorChar) == -1)
                a = ass_name_cache[System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), name)] as Assembly;
            if (a != null)
			{
				if (System.IO.File.GetLastWriteTime(name) == (DateTime)file_dates[a])
					return a;
				namespace_assemblies.Remove(a);
				assemblies.Remove(a);
				cur_used_assemblies.Remove(a);
				ns_types.Clear();
                
				Type[] tarr = a.GetTypes();
			    foreach(Type t in tarr)
			    {
				    if (t.Namespace != "" && t.Namespace != null)
				    {
					    string s = t.Namespace;
					    int pos = s.LastIndexOf('.');
					    namespaces.Remove(s);
                        
					    while (pos != -1) 
					    {
					    	s = s.Substring(0,pos);
					    	pos = s.LastIndexOf('.');
					    	//if (pos == -1)
					    	namespaces.Remove(s);
					    }
				    }
				    if (t.Name.StartsWith("%"))
				    {
				    	types.Remove(t.Namespace+"."+t.Name.Substring(1));
				    }
				    else
				    types.Remove(t.FullName);
                    if (t == ExtensionAttributeType)
                    {
                        ExtensionAttributeType = null;
                        extension_methods.Clear();
                    }
                    if (type_extensions.ContainsKey(t))
                        type_extensions.Remove(t);
			    }
			}
			//if (!System.IO.Path.GetFileName(name).ToLower().Contains("microsoft.directx"))
            try
            {
                var bytes = File.ReadAllBytes(name);
                a = Assembly.Load(bytes);
            }
            catch (Exception ex)
            {
                a = System.Reflection.Assembly.LoadFrom(name);
            }
            ass_name_cache[name] = a;
            assm_full_paths[a] = name;
            file_dates[a] = System.IO.File.GetLastWriteTime(name);
            return a;
		}
		
		private static void collect_internal()
		{
			
		}
		
		
		public static Hashtable entry_types = new Hashtable();
        private static string curr_inited_assm_path = null;

        public static List<Type> init_namespaces(System.Reflection.Assembly _assembly)
        {
            Assembly assembly = (Assembly)assemblies[_assembly];
            List<string> nss = new List<string>();
            cur_used_assemblies[_assembly] = _assembly;
            Type entry_type = null;
            List<Type> unit_types = entry_types[_assembly] as List<Type>;
            if (unit_types == null)
                unit_types = new List<Type>();
            if (assembly == null)
            {

                Type[] tarr = _assembly.GetTypes();
                //Hashtable ns_ht = new Hashtable(CaseInsensitiveHashCodeProvider.Default,CaseInsensitiveComparer.Default);
                Hashtable ns_ht = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
                foreach (Type t in tarr)
                {
                    if (t.IsNotPublic)
                        continue;
                    if (t.Namespace != "" && t.Namespace != null && t.FullName.IndexOf('$') == -1)
                    {
                        string s = t.Namespace;
                        int pos = s.LastIndexOf('.');
                        //if (pos == -1) 
                        //if (namespaces[s] == null) ns_ht.Add(s,s);
                        ns_ht[s] = s;
                        namespaces[s] = t;
                        // SSM 17.05.19 Запретил uses Reflection: https://github.com/pascalabcnet/pascalabcnet/issues/1941
                        /*if (pos != -1)
                        {
                            string[] sub_ns_arr = s.Split('.');
                            string sub_ns_str = sub_ns_arr[sub_ns_arr.Length - 1];
                            namespaces[sub_ns_str] = t;
                            ns_ht[sub_ns_str] = s;
                            int ind = sub_ns_arr.Length - 2;
                            while (ind != 0)
                            {
                                sub_ns_str = sub_ns_arr[ind] + "." + sub_ns_str;
                                ind--;
                            }
                        }*/
                        while (pos != -1)
                        {
                            s = s.Substring(0, pos);
                            pos = s.LastIndexOf('.');
                            //if (pos == -1)
                            if (namespaces[s] == null)
                                namespaces[s] = t;
                            ns_ht[s] = s;
                        }
                    }
                    TypeInfo ti = new TypeInfo(t, t.FullName);
                    types[t.FullName] = ti;
                    string short_name = t.Name;
                    object o2 = types[short_name];

                    if (o2 == null)
                        types[short_name] = ti;
                    else
                    {
                        if (o2 is TypeInfo)
                        {
                            List<TypeInfo> type_lst = new List<TypeInfo>();
                            type_lst.Add(o2 as TypeInfo);
                            type_lst.Add(ti);
                            types[short_name] = type_lst;
                        }
                        else if (o2 is List<TypeInfo>)
                        {
                            (o2 as List<TypeInfo>).Add(ti);
                        }
                    }

                    AddGenericInfo(t);
                    if (ExtensionAttributeType != null && t.GetCustomAttributes(ExtensionAttributeType, false).Length > 0)
                    {
                        MethodInfo[] meths = t.GetMethods(BindingFlags.Public | BindingFlags.Static);
                        List<MethodInfo> ext_meths = new List<MethodInfo>();
                        if (!extension_methods.ContainsKey(t))
                        {
                            foreach (MethodInfo mi in meths)
                                if (mi.GetCustomAttributes(ExtensionAttributeType, false).Length > 0)
                                {
                                    ext_meths.Add(mi);
                                    ParameterInfo[] prms = mi.GetParameters();
                                    if (prms.Length > 0)
                                    {

                                        List<MethodInfo> mths = null;
                                        List<MethodInfo> mths2 = null;
                                        Type tmp = prms[0].ParameterType;
                                        if (tmp.IsGenericType)
                                        {
                                            tmp = tmp.GetGenericTypeDefinition();
                                        }
                                        if (!type_extensions.TryGetValue(tmp, out mths))
                                        {
                                            mths = new List<MethodInfo>();
                                            type_extensions.Add(tmp, mths);
                                        }
                                        if ((tmp.BaseType == DelegateType || tmp.BaseType == MulticastDelegateType))
                                        {
                                            List<MethodInfo> mths3 = null;
                                            if (!type_extensions.TryGetValue(DelegateType, out mths3))
                                            {
                                                mths3 = new List<MethodInfo>();
                                                type_extensions.Add(DelegateType, mths3);
                                            }
                                            mths3.Add(mi);
                                        }
                                        mths.Add(mi);
                                        if (tmp.IsArray && !generic_array_type_extensions.TryGetValue(tmp.GetArrayRank(), out mths2))
                                        {
                                            mths2 = new List<MethodInfo>();
                                            generic_array_type_extensions.Add(tmp.GetArrayRank(), mths2);
                                        }
                                        if (mths2 != null)
                                            mths2.Add(mi);
                                        
                                        Dictionary<string, List<MemberInfo>> mht;
                                        if (members.TryGetValue(tmp, out mht))
                                        {
                                            List<MemberInfo> mis2 = null;
                                            string name = compiler_string_consts.GetNETOperName(mi.Name);
                                            if (name == null)
                                                name = mi.Name;
                                            if (!mht.TryGetValue(name, out mis2))
                                            {
                                                mis2 = new List<MemberInfo>();
                                                mht.Add(name, mis2);
                                            }
                                            if (!mis2.Contains(mi))
                                                mis2.Add(mi);
                                        }
                                        if (tmp.IsGenericParameter && !generic_parameter_type_extensions.Contains(mi))
                                        {
                                            generic_parameter_type_extensions.Add(mi);
                                            foreach (Type tt in members.Keys)
                                            {
                                                if (members.TryGetValue(tt, out mht))
                                                {
                                                    List<MemberInfo> mis2 = null;
                                                    string name = compiler_string_consts.GetNETOperName(mi.Name);
                                                    if (name == null)
                                                        name = mi.Name;
                                                    if (!mht.TryGetValue(name, out mis2))
                                                    {
                                                        mis2 = new List<MemberInfo>();
                                                        mht.Add(name, mis2);
                                                    }
                                                    if (!mis2.Contains(mi))
                                                        mis2.Add(mi);
                                                }
                                            }
                                        }
                                        
                                        foreach (Type arr_t in arrays_with_extension_methods.Keys)
                                        {
                                            if (members.TryGetValue(arr_t, out mht))
                                            {
                                                List<MemberInfo> mis2 = null;
                                                string name = compiler_string_consts.GetNETOperName(mi.Name);
                                                if (name == null)
                                                    name = mi.Name;
                                                if (!mht.TryGetValue(name, out mis2))
                                                {
                                                    mis2 = new List<MemberInfo>();
                                                    mht.Add(name, mis2);
                                                }
                                                if (!mis2.Contains(mi))
                                                    mis2.Add(mi);
                                            }
                                        }

                                        foreach (Type gen_t in generics_with_extension_methods.Keys)
                                        {
                                            if (members.TryGetValue(gen_t, out mht))
                                            {
                                                List<MemberInfo> mis2 = null;
                                                string name = compiler_string_consts.GetNETOperName(mi.Name);
                                                if (name == null)
                                                    name = mi.Name;
                                                if (!mht.TryGetValue(name, out mis2))
                                                {
                                                    mis2 = new List<MemberInfo>();
                                                    mht.Add(name, mis2);
                                                }
                                                if (!mis2.Contains(mi))
                                                    mis2.Add(mi);
                                            }
                                        }
                                    }
                                }
                            extension_methods.Add(t, ext_meths.ToArray());
                        }
                    }
                    if (t.Name == t.Namespace)
                    {
                        object[] attrs = t.GetCustomAttributes(false);
                        foreach (Attribute attr in attrs)
                        {
                            if (attr.GetType().Name == PascalABCCompiler.TreeConverter.compiler_string_consts.global_attr_name)
                            {
                                entry_type = t;
                                unit_types.Insert(0, t);
                                entry_types[_assembly] = unit_types;

                                break;
                            }
                            else if (attr.GetType().Name == PascalABCCompiler.TreeConverter.compiler_string_consts.class_unit_attr_name)
                            {
                                if (_assembly.ManifestModule.ScopeName == compiler_string_consts.pabc_rtl_dll_name)
                                {
                                    if (t.Name == compiler_string_consts.system_unit_file_name)
                                        PABCSystemType = t;
                                    else if (t.Name == "PT4")
                                        PT4Type = t;
                                }
                                unit_types.Add(t);
                                break;
                            }
                        }
                    }
                    else if (t.Name.StartsWith("%"))
                    {
                        object[] attrs = t.GetCustomAttributes(false);
                        if (attrs.Length == 1)
                        {
                            Type attr_t = attrs[0].GetType();
                            if (attr_t.FullName == compiler_string_consts.file_of_attr_name)
                            {
                                object o = attr_t.GetField("Type", BindingFlags.Public | BindingFlags.Instance).GetValue(attrs[0]);
                                if (o is Type)
                                    attr_t = o as Type;
                                else
                                    attr_t = _assembly.GetType(o as string, false);
                                if (PascalABCCompiler.TreeConverter.compilation_context.instance != null && PascalABCCompiler.TreeConverter.compilation_context.instance.syntax_tree_visitor.compiled_unit != null && PascalABCCompiler.TreeConverter.compilation_context.instance.converted_namespace != null)
                                {
                                    type_node tn = CreatePascalType(attr_t);
                                    if (tn != null)
                                        tn = PascalABCCompiler.TreeConverter.compilation_context.instance.create_typed_file_type(tn, null);
                                    else
                                        tn = PascalABCCompiler.TreeConverter.compilation_context.instance.create_typed_file_type(compiled_type_node.get_type_node(attr_t), null);
                                    compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = tn;
                                }
                                else
                                {
                                    compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = t;
                                }
                            }
                            else if (attr_t.FullName == compiler_string_consts.set_of_attr_name)
                            {
                                object o = attr_t.GetField("Type", BindingFlags.Public | BindingFlags.Instance).GetValue(attrs[0]);
                                if (o is Type)
                                    attr_t = o as Type;
                                else
                                {
                                    attr_t = t.Assembly.GetType(o as string, false);

                                }
                                if (PascalABCCompiler.TreeConverter.compilation_context.instance != null && PascalABCCompiler.TreeConverter.compilation_context.instance.syntax_tree_visitor.compiled_unit != null && PascalABCCompiler.TreeConverter.compilation_context.instance.converted_namespace != null)
                                {
                                    type_node tn = CreatePascalType(attr_t);
                                    if (tn != null)
                                        tn = PascalABCCompiler.TreeConverter.compilation_context.instance.create_set_type(tn, null);
                                    else
                                        tn = PascalABCCompiler.TreeConverter.compilation_context.instance.create_set_type(compiled_type_node.get_type_node(attr_t), null);
                                    compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = tn;
                                }
                                else
                                {
                                    compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = t;
                                }
                            }
                            else if (attr_t.FullName == compiler_string_consts.short_string_attr_name)
                            {
                                int len = (int)attr_t.GetField("Length", BindingFlags.Public | BindingFlags.Instance).GetValue(attrs[0]);
                                if (PascalABCCompiler.TreeConverter.compilation_context.instance != null && PascalABCCompiler.TreeConverter.compilation_context.instance.syntax_tree_visitor.compiled_unit != null && PascalABCCompiler.TreeConverter.compilation_context.instance.converted_namespace != null)
                                {
                                    type_node tn = PascalABCCompiler.TreeConverter.compilation_context.instance.create_short_string_type(len, null);
                                    compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = tn;
                                }
                                else
                                {
                                    compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = t;
                                }
                            }
                            else if (attr_t.FullName == compiler_string_consts.template_class_attr_name)
                            {
                                compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = t;
                            }
                            else if (attr_t.FullName == compiler_string_consts.type_synonim_attr_name)
                            {
                                object o = attr_t.GetField("Type", BindingFlags.Public | BindingFlags.Instance).GetValue(attrs[0]);
                                if (o is Type)
                                    attr_t = o as Type;
                                else
                                    attr_t = _assembly.GetType(o as string, false);
                                if (attr_t != null)
                                {
                                    type_node tn = CreatePascalType(attr_t);
                                    if (tn != null)
                                        compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = tn;
                                    else
                                        types[t.Namespace + "." + t.Name.Substring(1)] = new TypeInfo(attr_t, attr_t.FullName);
                                }
                            }
                        }
                    }
                }
                namespace_assemblies[_assembly] = ns_ht;
                assemblies[_assembly] = _assembly;
            }
            return unit_types;
        }

        private static template_class CreateTemplateClassType(Type t)
        {
            if (t == null)
                return null;
            object[] attrs = t.GetCustomAttributes(false);
            if (attrs.Length == 1)
            {
                Type attr_t = attrs[0].GetType();
                byte[] tree = (byte[])attr_t.GetField("Tree", BindingFlags.Public | BindingFlags.Instance).GetValue(attrs[0]);
                return PascalABCCompiler.TreeConverter.compilation_context.instance.create_template_class(t.FullName, tree);
            }
            return null;
        }

        private static type_node CreatePascalType(Type t)
        {
            if (t == null) 
                return null;
            object[] attrs = t.GetCustomAttributes(false);
            bool not_pascal_type = false;
            if (attrs.Length == 1)
            {
                Type attr_t = attrs[0].GetType();
                if (attr_t.FullName == compiler_string_consts.file_of_attr_name)
                {
                    object o = attr_t.GetField("Type", BindingFlags.Public | BindingFlags.Instance).GetValue(attrs[0]);
                    if (o is Type)
                        attr_t = o as Type;
                    else
                    {
                        attr_t = t.Assembly.GetType(o as string, false);
                        if (attr_t != null)
                        {
                            type_node tn = CreatePascalType(attr_t);
                            if (tn != null)
                                return PascalABCCompiler.TreeConverter.compilation_context.instance.create_typed_file_type(tn, null);
                        }
                    }
                    return PascalABCCompiler.TreeConverter.compilation_context.instance.create_typed_file_type(compiled_type_node.get_type_node(attr_t), null);
                }
                else if (attr_t.FullName == compiler_string_consts.set_of_attr_name)
                {
                    object o = attr_t.GetField("Type", BindingFlags.Public | BindingFlags.Instance).GetValue(attrs[0]);
                    if (o is Type)
                        attr_t = o as Type;
                    else
                    {
                        attr_t = t.Assembly.GetType(o as string, false);
                        if (attr_t != null)
                        {
                            type_node tn = CreatePascalType(attr_t);
                            if (tn != null)
                                return PascalABCCompiler.TreeConverter.compilation_context.instance.create_set_type(tn, null);
                        }
                    }
                    return PascalABCCompiler.TreeConverter.compilation_context.instance.create_set_type(compiled_type_node.get_type_node(attr_t), null);
                }
                else if (attr_t.FullName == compiler_string_consts.short_string_attr_name)
                {
                    int len = (int)attr_t.GetField("Length", BindingFlags.Public | BindingFlags.Instance).GetValue(attrs[0]);
                    return PascalABCCompiler.TreeConverter.compilation_context.instance.create_short_string_type(len, null);
                }
                else if (attr_t.FullName == compiler_string_consts.template_class_attr_name)
                {
                    return null;
                    
                }
                else
                    not_pascal_type = true;
            }
            else
                not_pascal_type = true;
            if (not_pascal_type)
            {
                if (t.IsPointer)
                {
                    if (t.GetElementType() == void_type)
                        return SystemLibrary.SystemLibrary.pointer_type;
                    else
                    {
                        type_node elem_t = CreatePascalType(t.GetElementType());
                        return elem_t.ref_type;
                    }
                }
                else
                    return compiled_type_node.get_type_node(t, SystemLibrary.SystemLibrary.syn_visitor.SymbolTable);
            }
            return null;
        }
                    	
		public static bool IsEntryType(Type t)
		{
			object[] attrs = t.GetCustomAttributes(false);
			for (int j=0; j<attrs.Length; j++)
			if (attrs[j].GetType().Name == PascalABCCompiler.TreeConverter.compiler_string_consts.global_attr_name
                || attrs[j].GetType().Name == PascalABCCompiler.TreeConverter.compiler_string_consts.class_unit_attr_name)
			{
				return true;
			}
			return false;
		}
		
        public static bool NamespaceExists(string Namespace)
        {
        	Type t = (Type)namespaces[Namespace];
       		
        	if (t != null && cur_used_assemblies.ContainsKey(t.Assembly)) return true;
        	foreach (Assembly a in namespace_assemblies.Keys)
        		if (cur_used_assemblies.ContainsKey(a) && (namespace_assemblies[a] as Hashtable).ContainsKey(Namespace))
                    return true;
        	return false;
        }

        static NetHelper()
        {
            //scope = new NetScope(null,null);
            //namespaces = new Hashtable(1024, CaseInsensitiveHashCodeProvider.Default,CaseInsensitiveComparer.Default);
            //ssyy-
            //interfaces = new Hashtable();
            //\ssyy-
            //types = new Hashtable(1024, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            types = new Hashtable(8096, StringComparer.CurrentCultureIgnoreCase);
            compiled_pascal_types = new Hashtable(1024, StringComparer.CurrentCultureIgnoreCase);
            namespaces = new Hashtable(1024, StringComparer.CurrentCultureIgnoreCase);
            ass_name_cache = new Hashtable(1024, StringComparer.CurrentCultureIgnoreCase);
            assm_full_paths = new Dictionary<Assembly, string>();
            //ass_name_cache = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            file_dates = new Hashtable();
            //methods = new Hashtable();
            //properties = new Hashtable();
            members = new Dictionary<Type, Dictionary<string, List<MemberInfo>>>(128);
            //fields = new Hashtable();
            assemblies = new Hashtable();
            //meth_nodes = new Hashtable();
            prop_nodes = new Dictionary<PropertyInfo,compiled_property_node>();
            field_nodes = new Hashtable();
            constr_nodes = new Hashtable();
            stand_types = new Hashtable();
            type_handles = new Hashtable();
            method_handles = new Hashtable();
            constr_handles = new Hashtable();
            field_handles = new Hashtable();
            special_types = new Hashtable();
            type_search_cache = new Dictionary<string,FoundInfo>(300);
            cached_type_extensions = new Hashtable();
            namespace_assemblies = new Hashtable();
            cur_used_assemblies = new Hashtable();
            //ns_types = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            ns_types = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
            memberInfo = typeof(MemberInfo);

            stand_types[typeof(int)] = stand_types;
            stand_types[typeof(byte)] = stand_types;
            stand_types[typeof(bool)] = stand_types;
            stand_types[typeof(sbyte)] = stand_types;
            stand_types[typeof(short)] = stand_types;
            stand_types[typeof(ushort)] = stand_types;
            stand_types[typeof(uint)] = stand_types;
            stand_types[typeof(long)] = stand_types;
            stand_types[typeof(ulong)] = stand_types;
            stand_types[typeof(char)] = stand_types;
            stand_types[typeof(float)] = stand_types;
            stand_types[typeof(double)] = stand_types;
            //stand_types[typeof(decimal)]=stand_types;
            //stand_types[NetHelper.void_ptr_type] = stand_types;
            DelegateType = typeof(Delegate);
            MulticastDelegateType = typeof(MulticastDelegate);
            void_type = typeof(void);
            AddSpecialType(void_ptr_type);
            AppDomain.CurrentDomain.AssemblyResolve +=new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            ExtensionAttributeType = typeof(System.Runtime.CompilerServices.ExtensionAttribute);
            SystemCoreAssembly = ExtensionAttributeType.Assembly;
            EnumType = typeof(Enum);
            ArrayType = typeof(Array);
            AddToDictionaryMethod = typeof(Dictionary<string, object>).GetMethod("Add");
        }

        public static string GetAssemblyDirectory(Assembly assm)
        {
            string path;
            if (assm_full_paths.TryGetValue(assm, out path))
            {
                return Path.GetDirectoryName(path);
            }
            return null;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                
                if (curr_inited_assm_path != null)
                {
                    Assembly assm = null;
                    string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(curr_inited_assm_path), args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");
                    if (!System.IO.File.Exists(path))
                        path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");
                    if (System.IO.File.Exists(path))
                        assm = LoadAssembly(path, true);
                    curr_inited_assm_path = null;

                    //return LoadAssembly(args.Name);
                    return assm;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private static void AddSpecialType(Type t)
        {
            special_types[t.MetadataToken] = t;
        }

		public static bool IsStandType(Type t)
		{
			if (stand_types[t] != null) return true;
			return false;
		}
		
		public static bool IsNetNamespace(string name, Type tt = null)
		{
            Type t = null;
            if (tt != null)
                t = tt;
            else
                t = namespaces[name] as Type;
        	if (t != null && cur_used_assemblies.ContainsKey(t.Assembly)) return true;
        	foreach (Assembly a in namespace_assemblies.Keys)
        		if (cur_used_assemblies != null && cur_used_assemblies.ContainsKey(a) && (namespace_assemblies[a] as Hashtable).ContainsKey(name))
                    return true;
        	return false;
		}

        public static bool IsNetNamespaceInAssembly(string name, Assembly a)
        {
            if (cur_used_assemblies != null && cur_used_assemblies.ContainsKey(a) && (namespace_assemblies[a] as Hashtable).ContainsKey(name))
                return true;
            return false;
        }

		public static bool IsNetNamespace(string name,PascalABCCompiler.TreeRealization.using_namespace_list _unar, out string full_ns)
		{
			Type t = namespaces[name] as Type;
			full_ns = name;
			if (t != null)
			{
                if (PABCSystemType != null && t.Assembly == PABCSystemType.Assembly && !UsePABCRtl)
                    return false;
                Type tt = t;
                if (string.Compare(t.Namespace, name, true) == 0)
                {
                    full_ns = t.Namespace;
                    return true;
                }
                for (int i = 0; i < _unar.Count; i++)
                {
                    string full_name = _unar[i].namespace_name + "." + name;
                    if (string.Compare(full_name, t.Namespace, true) == 0)
                    {
                        full_ns = full_name;
                        return IsNetNamespace(name, t);
                    }
                }
                for (int i = 0; i < _unar.Count; i++)
                {
                    string full_name = _unar[i].namespace_name + "." + name;
                    t = namespaces[full_name] as Type;
                    if (t != null)
                    {
                        full_ns = full_name;
                        return IsNetNamespace(full_ns, t);
                    }
                }
                if (tt.Namespace.IndexOf(name, 0, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
			}
			return false;
		}
		
        public static Type void_ptr_type
        {
            get
            {
                if(SemanticRules.PoinerRealization == PoinerRealization.IntPtr)
                    return FindTypeOrCreate(compiler_string_consts.pointer_net_type_name_intptr);
                return FindTypeOrCreate(compiler_string_consts.pointer_net_type_name_void);
            }
        }

        public static IEnumerable<MemberInfo> GetExtensionMethods(Type t)
        {
            List<MethodInfo> meths = null;
            if (SystemCoreAssembly == null || !cur_used_assemblies.ContainsKey(SystemCoreAssembly))
                return new List<MemberInfo>();
            if (type_extensions.TryGetValue(t, out meths) || t.IsGenericType && type_extensions.TryGetValue(t.GetGenericTypeDefinition(), out meths))
            {
                //return meths.ToArray();
            }
            else
            {
                meths = new List<MethodInfo>();
            }
            //meths = new List<MethodInfo>();
            Type[] tt = t.GetInterfaces();
            for (int i = 0; i < tt.Length; i++)
            {
                meths.AddRange((MethodInfo[])GetExtensionMethodsNoRecursive(tt[i]));
            }
            if (t.BaseType != null)
                meths.AddRange((MethodInfo[])GetExtensionMethods(t.BaseType));
            return meths.ToArray();

        }

        private static IEnumerable<MemberInfo> GetExtensionMethodsNoRecursive(Type t)
        {
            List<MethodInfo> meths = null;
            if (type_extensions.TryGetValue(t, out meths) || t.IsGenericType && type_extensions.TryGetValue(t.GetGenericTypeDefinition(), out meths))
            {
                return meths.ToArray();
            }
            return new List<MethodInfo>().ToArray();
        }

        private static function_node get_conversion(compiled_type_node in_type,compiled_type_node from,
            type_node to, string op_name, NetTypeScope scope)
        {
            //MethodInfo[] mia = in_type.compiled_type.GetMethods();
            List<MemberInfo> mia = GetMembers(in_type.compiled_type, op_name);
           
            foreach (MemberInfo mbi in mia)
            {
                if (!(mbi is MethodInfo))
                    continue;
                if (!(to is compiled_type_node))
                    continue;
                MethodInfo mi = mbi as MethodInfo;
                if (mi.ReturnType != (to as compiled_type_node).compiled_type)
                {
                    continue;
                }
                ParameterInfo[] piarr = mi.GetParameters();
                if (piarr.Length != 1)
                {
                    continue;
                }
                if (piarr[0].ParameterType != from.compiled_type)
                {
                    continue;
                }
                return compiled_function_node.get_compiled_method(mi);
            }
            if (scope != null)
            {
                List<SymbolInfo> sil = scope.FindOnlyInType(op_name, scope);
                if(sil != null)
                    foreach(SymbolInfo si in sil)
                    {
                        if (si.sym_info is common_namespace_function_node)
                        {
                            function_node fn = si.sym_info as function_node;
                            if ((fn.return_value_type == to || fn.return_value_type.original_generic == to 
                                || to.type_special_kind == type_special_kind.array_kind && fn.return_value_type != null && fn.return_value_type.type_special_kind == type_special_kind.array_kind 
                                && fn.return_value_type.element_type.is_generic_parameter) && 
                                fn.parameters.Count == 1 && 
                                (fn.parameters[0].type == from || fn.parameters[0].type.original_generic == from)
                                || fn.parameters[0].type.type_special_kind == type_special_kind.array_kind && fn.parameters[0].type.element_type.is_generic_parameter)
                            {
                                return fn;
                            }
                        }
                    }
            }
            return null;
        }

        public static function_node get_implicit_conversion(compiled_type_node in_type, compiled_type_node from,
            type_node to, NetTypeScope scope)
        {
            return get_conversion(in_type, from, to, compiler_string_consts.implicit_operator_name, scope);
        }

        public static function_node get_explicit_conversion(compiled_type_node in_type, compiled_type_node from,
            compiled_type_node to, NetTypeScope scope)
        {
            return get_conversion(in_type, from, to, compiler_string_consts.explicit_operator_name, scope);
        }

        public static compiled_type_node get_array_type(compiled_type_node element_type)
        {
            return (compiled_type_node.get_type_node(element_type.compiled_type.MakeArrayType()));
        }
        
        public static compiled_type_node get_array_type(compiled_type_node element_type, int rank)
        {
        	if (rank == 1)
        	return (compiled_type_node.get_type_node(element_type.compiled_type.MakeArrayType()));
        	else
        	return (compiled_type_node.get_type_node(element_type.compiled_type.MakeArrayType(rank)));
        }
		
		public static string[] GetNamespaces(Assembly a)
		{
			Hashtable ht = (Hashtable)namespace_assemblies[a];
			List<string> lst = new List<string>();
			foreach (string s in ht.Values)
				//if (s.IndexOf('.') == -1)
				lst.Add(s);
			return lst.ToArray();
		}
		
        public static void FormBaseInterfacesList(Type t, List<Type> interfaces)
        {
            if (interfaces.IndexOf(t) < 0)
            {
                interfaces.Add(t);
                Type[] inters = t.GetInterfaces();
                int count = inters.Length;
                for (int i = inters.Length - 1; i > -1; --i)
                {
                    FormBaseInterfacesList(inters[i], interfaces);
                }
            }
        }

        public static bool IsExtensionMethod(MethodInfo mi)
        {
            if (ExtensionAttributeType != null && mi.GetCustomAttributes(ExtensionAttributeType, false).Length > 0)
                return true;
            return false;
        }

		public static List<MemberInfo> GetMembers(Type t, string name)
		{
            Dictionary<string, List<MemberInfo>> mht = null;
            if (members.TryGetValue(t, out mht))
            {
                List<MemberInfo> mis2 = null;

                if (!mht.TryGetValue(name, out mis2))
                {
                    return EmptyMemberInfoList;
                }
                return mis2;
            }
            else
			{
                BindingFlags bf = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
				MemberInfo[] mis;
                if (t.IsInterface)
                {
                    List<Type> all_interfaces = new List<Type>();
                    FormBaseInterfacesList(t, all_interfaces);
                    List<MemberInfo> mem_info = new List<MemberInfo>();
                    foreach (Type interf in all_interfaces)
                    {
                        mem_info.AddRange(interf.GetMembers(bf));
                    }
                    mem_info.AddRange(typeof(object).GetMembers(bf));
                    mis = mem_info.ToArray();
                }
                else
                {
                    mis = t.GetMembers(bf);
                }
                //(ssyy) DarkStar, что за предупреждение по следующей строке?
				var ht = new Dictionary<string,List<MemberInfo>>(StringComparer.CurrentCultureIgnoreCase);
                //(ssyy) DarkStar, может быть эффективнее слить следующие 2 цикла в один?
                foreach (MemberInfo mi2 in mis)
                {
                    //Console.WriteLine(mi2.Name.ToLower());
                    string s = mi2.Name;//.ToLower();
                    if (!ht.ContainsKey(s))
                        ht[s] = new List<MemberInfo>();
                }
				foreach (MemberInfo mi in mis)
				{
                    ht[mi.Name].Add(mi);
				}
                if (ExtensionAttributeType != null)
                {
                    List<MethodInfo> meths = null;
                    Type tmp_t = t;
                    if (tmp_t.IsArray)
                        arrays_with_extension_methods[tmp_t] = tmp_t;
                    if (tmp_t.IsGenericTypeDefinition)
                        generics_with_extension_methods[tmp_t] = tmp_t;
                    cached_type_extensions[t] = t;
                    while (tmp_t != null)
                    {
                        List<MethodInfo> meths1 = null;
                        List<MethodInfo> meths2 = null;
                        List<MethodInfo> meths3 = generic_parameter_type_extensions;
                        if (tmp_t.IsGenericType && !tmp_t.IsGenericTypeDefinition)
                            type_extensions.TryGetValue(tmp_t.GetGenericTypeDefinition(), out meths1);
                        if (tmp_t.IsArray)
                            generic_array_type_extensions.TryGetValue(tmp_t.GetArrayRank(), out meths2);
                        if (type_extensions.TryGetValue(tmp_t, out meths) || meths1 != null || meths2 != null || meths3 != null)
                        {
                            List<MethodInfo> all_meths = new List<MethodInfo>();
                            if (meths != null)
                                all_meths.AddRange(meths);
                            if (meths1 != null)
                                all_meths.AddRange(meths1);
                            if (meths2 != null)
                                all_meths.AddRange(meths2);
                            if (meths3 != null)
                                all_meths.AddRange(meths3);
                            foreach (MethodInfo mi in all_meths)
                            {
                                if (cur_used_assemblies.ContainsKey(mi.DeclaringType.Assembly))
                                {
                                    List<MemberInfo> al = null;
                                    string s = compiler_string_consts.GetNETOperName(mi.Name);
                                    if (s == null)
                                        s = mi.Name;
                                    if (!ht.TryGetValue(s, out al))
                                    {
                                        al = new List<MemberInfo>();
                                        ht[s] = al;
                                    }
                                    al.Insert(0, mi);
                                }
                            }
                        }
                        tmp_t = tmp_t.BaseType;
                    }
                    
                    Type[] intfs = t.GetInterfaces();
                    foreach (Type intf_t in intfs)
                    {
                        Type tmp = intf_t;
                        if (tmp.IsGenericType)
                            tmp = tmp.GetGenericTypeDefinition();
                        if (type_extensions.TryGetValue(tmp, out meths))
                        {
                            foreach (MethodInfo mi in meths)
                            {
                                if (cur_used_assemblies.ContainsKey(mi.DeclaringType.Assembly))
                                {
                                    List<MemberInfo> al = null;
                                    if (!ht.TryGetValue(mi.Name, out al))
                                    {
                                        al = new List<MemberInfo>();
                                        ht[mi.Name] = al;
                                    }
                                    al.Insert(0,mi);
                                }
                            }
                        }
                    }
                }
				members[t] = ht;
                List<MemberInfo> lst = null;
				if (!ht.TryGetValue(name, out lst)) 
                    return EmptyMemberInfoList;	
				return lst;
			}
			
		}

        private static List<MemberInfo> EmptyMemberInfoList = new List<MemberInfo>();

        //(ssyy) Является ли член класса видимым.
        public static field_access_level get_access_level(MemberInfo mi)
        {
            field_access_level amod = field_access_level.fal_public;
            switch (mi.MemberType)
            {
                case MemberTypes.Constructor:
                case MemberTypes.Method:
                    {
                        MethodBase mb = mi as MethodBase;
                        MethodAttributes attrs = mb.Attributes;
                        if (attrs == (attrs | MethodAttributes.Public))
                        {
                            amod = field_access_level.fal_public;
                        }
                        else
                        {
                            if ((attrs == (attrs | MethodAttributes.Family) ||
                                attrs == (attrs | MethodAttributes.FamORAssem)) &&
                                ((mi.MemberType == MemberTypes.Constructor) ? true : (attrs != (attrs | MethodAttributes.SpecialName))))
                            {
                                amod = field_access_level.fal_protected;
                            }
                            else
                            {
                                amod = field_access_level.fal_private;
                            }
                        }
                    }
                    break;
                case MemberTypes.Field:
                    FieldInfo fi = mi as FieldInfo;
                    if (fi.Attributes == (fi.Attributes | FieldAttributes.Public)
                        && (fi.Attributes != (fi.Attributes | FieldAttributes.SpecialName)))
                    {
                        amod = field_access_level.fal_public;
                    }
                    else
                    {
                        if (fi.Attributes == (fi.Attributes | FieldAttributes.Family)
                            || fi.Attributes == (fi.Attributes | FieldAttributes.FamORAssem))
                        {
                            amod = field_access_level.fal_protected;
                        }
                        else
                        {
                            amod = field_access_level.fal_private;
                        }
                    }
                    break;
                case MemberTypes.Property:
                    //PropertyInfo pi = mi as PropertyInfo;
                    MethodInfo mi2 = GetAnyAccessor(mi as PropertyInfo);
                    if (mi2 != null)
                    if (mi2.Attributes == (mi2.Attributes | MethodAttributes.Public))
                    {
                        amod = field_access_level.fal_public;
                    }
                    else
                    {
                        if ((mi2.Attributes == (mi2.Attributes | MethodAttributes.Family) ||
                            mi2.Attributes == (mi2.Attributes | MethodAttributes.FamORAssem)))
                        {
                            amod = field_access_level.fal_protected;
                        }
                        else
                        {
                            amod = field_access_level.fal_private;
                        }
                    }
                    break;

            }
            return amod;
        }
        
        public static bool is_visible(MemberInfo mi)
        {
            field_access_level amod = get_access_level(mi);
            if (amod == field_access_level.fal_public)
            {
                return true;
            }
            if (amod == field_access_level.fal_private)
            {
                return false;
            }
            common_type_node curr_type = SystemLibrary.SystemLibrary.syn_visitor.context.converted_type;
            compiled_type_node comp_node = compiled_type_node.get_type_node(mi.DeclaringType);
            return curr_type != null && type_table.is_derived(comp_node, curr_type, true);
        }
		
        public static List<SymbolInfo> GetConstructor(Type t)
        {
        	ConstructorInfo[] constrs = t.GetConstructors(BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);
        	SymbolInfo si = null;
        	List<SymbolInfo> res_si = null;
        	foreach (ConstructorInfo ci in constrs)
        	{
        		field_access_level fal = get_access_level(ci);
            	if (fal != field_access_level.fal_private && fal != field_access_level.fal_internal)
                {
            		si = new SymbolInfo(compiled_constructor_node.get_compiled_constructor(ci));
            		if (res_si == null)
            			res_si = new List<SymbolInfo>();
                    res_si.Add(si);
            	}
        	}
        	return res_si;
        }
        
        public static bool HasFlagAttribute(Type t)
        {
        	return t.GetCustomAttributes(typeof(FlagsAttribute), true).Length != 0;
        }

        public static bool IsParamsParameter(ParameterInfo pi)
        {
            return pi.GetCustomAttributes(typeof(ParamArrayAttribute), true).Length > 0;
        }

        public static bool IsType(string name)
        {
        	return FindType(name) != null;
        }
        
        public static List<SymbolInfo> FindNameIncludeProtected(Type t, string name)
        {
        	if (name == null) return null;
			if (name == compiler_string_consts.assign_name) return null;
            string s = compiler_string_consts.GetNETOperName(name);
			if (s != null) 
			{
				if (IsStandType(t)) return null;
				name = s;
			}
			List<SymbolInfo> sil=null;
			List<MemberInfo> mis = GetMembers(t,name);
        	
            foreach (MemberInfo mi in mis)
            {
                if (mi.DeclaringType != null && PABCSystemType != null && mi.DeclaringType.Assembly == PABCSystemType.Assembly && !UsePABCRtl)
                    continue;
            	field_access_level fal = get_access_level(mi);
            	if (fal != field_access_level.fal_private && fal != field_access_level.fal_internal)
                {
                    SymbolInfo temp = null;
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method:
                            temp = new SymbolInfo(compiled_function_node.get_compiled_method((MethodInfo)mi));
                            break;
                        case MemberTypes.Constructor:
                            temp = new SymbolInfo(compiled_constructor_node.get_compiled_constructor((ConstructorInfo)mi));
                            break;
                        case MemberTypes.Property:
                            temp = new SymbolInfo(GetPropertyNode((PropertyInfo)mi));
                            break;
                        case MemberTypes.Field:
                            temp = GetSymbolInfoForFieldNode((FieldInfo)mi);
                            break;
                        case MemberTypes.Event:
                            temp = new SymbolInfo(GetEvent((EventInfo)mi));
                            break;
                        default:
                            continue;
                    }
                    if (sil == null)
                        sil = new List<SymbolInfo>();
                    sil.Insert(0, temp);
                }
        	}
            Type nested_t = null;
            foreach (Type nt in t.GetNestedTypes())
            {
                if (string.Compare(nt.Name, name, true) == 0)
                {
                    nested_t = nt;
                    break;
                }
            }
            if (nested_t != null)
            {
            	SymbolInfo temp = new SymbolInfo(compiled_type_node.get_type_node(nested_t));
                if (sil == null)
                    sil = new List<SymbolInfo>();
                sil.Insert(0, temp);
            }
            return sil;
        }

        public static List<SymbolInfo> FindName(Type t, string name)
        {
            if (name == null) return null;
            if (name == compiler_string_consts.assign_name) return null;
            string s = compiler_string_consts.GetNETOperName(name);
            string tmp_name = name;
            if (s != null)
            {
                if (IsStandType(t)) return null;
                if (t != NetHelper.PABCSystemType)
                    name = s;
            }
            List<SymbolInfo> sil = null;

            List<MemberInfo> mis = GetMembers(t, name);
            //(ssyy) Изменил алгоритм.
            //У нас некоторые алгоритмы базируются на том, что возвращённые
            //сущности будут одной природы (например, все - методы). Это неверно,
            //так как в случае наличия функции Ident и поля ident оба должны попасть
            //в список.

            //TODO: проанализировать и изменить алгоритмы, использующие поиск.
            //List<SymbolInfo> si_list = new List<SymbolInfo>();
            foreach (MemberInfo mi in mis)
            {
                if (mi.DeclaringType != null && PABCSystemType != null && mi.DeclaringType.Assembly == PABCSystemType.Assembly && !UsePABCRtl)
                    continue;
                if (is_visible(mi))
                {
                    SymbolInfo temp = null;
                    switch (mi.MemberType)
                    {
                        case MemberTypes.Method:
                            temp = new SymbolInfo(compiled_function_node.get_compiled_method(mi as MethodInfo));
                            // SSM 2018.05.05 исправляет bug #815
                            temp.symbol_kind = symbol_kind.sk_overload_function;
                            break;
                        case MemberTypes.Constructor:
                            temp = new SymbolInfo(compiled_constructor_node.get_compiled_constructor(mi as ConstructorInfo));
                            break;
                        case MemberTypes.Property:
                            temp = new SymbolInfo(GetPropertyNode(mi as PropertyInfo));
                            break;
                        case MemberTypes.Field:
                            temp = GetSymbolInfoForFieldNode(mi as FieldInfo);
                            break;
                        case MemberTypes.Event:
                            temp = new SymbolInfo(GetEvent(mi as EventInfo));
                            break;
                        default:
                            continue;
                    }
                    if (sil == null)
                        sil = new List<SymbolInfo> { temp };
                    else
                        sil.Add(temp);

                    //temp.Next = si;
                    //si = temp;
                }
            }
            Type nested_t = null;
            foreach (Type nt in t.GetNestedTypes())
            {
                if (string.Compare(nt.Name, name, true) == 0)
                {
                    nested_t = nt;
                    break;
                }
            }
            if (nested_t != null)
            {
                List<SymbolInfo> temp = new List<SymbolInfo> { new SymbolInfo(compiled_type_node.get_type_node(nested_t)) };
                if(sil != null)
                    temp.AddRange(sil);
                sil = temp;
            }

            return sil;
        }

        public static SymbolInfo GetSymbolInfoForFieldNode(FieldInfo fi)
        {
            SymbolInfo si = null;
            if (fi.IsLiteral && !fi.FieldType.IsEnum)
            {
                compiled_class_constant_definition cd = GetConstantFieldNode(fi);
                if (cd != null)
                    si = new SymbolInfo(cd);
            }
            if (si == null)
                si = new SymbolInfo(GetFieldNode(fi));
            return si;
        }

        /*
		public static compiled_function_node GetMethodNode(MethodInfo mi)
		{
			compiled_function_node cfn = (compiled_function_node)meth_nodes[mi];
			if (cfn != null) return cfn;
			cfn = new compiled_function_node(mi);
			meth_nodes[mi] = cfn;
			return cfn;
		}
		*/

		public static compiled_property_node GetPropertyNode(PropertyInfo pi)
		{
            compiled_property_node cpn = null;
            if (prop_nodes.TryGetValue(pi, out cpn))
                return cpn;
			cpn = new compiled_property_node(pi);
			
			prop_nodes[pi] = cpn;
			return cpn;
		}

		public static compiled_variable_definition GetFieldNode(FieldInfo pi)
		{
            compiled_variable_definition cpn = field_nodes[pi] as compiled_variable_definition;
			if (cpn != null) 
			{
				if (cpn.type is compiled_type_node)
				return cpn;
				if (PascalABCCompiler.TreeConverter.compilation_context.instance != null && PascalABCCompiler.TreeConverter.compilation_context.instance.syntax_tree_visitor.compiled_unit != null && PascalABCCompiler.TreeConverter.compilation_context.instance.converted_namespace != null)
				{
					if (cpn.type.type_special_kind == type_special_kind.typed_file &&
				    (!PascalABCCompiler.TreeConverter.compilation_context.instance.TypedFiles.ContainsKey(cpn.type.element_type) || PascalABCCompiler.TreeConverter.compilation_context.instance.TypedFiles[cpn.type.element_type] != cpn.type))
					{
						cpn.type = PascalABCCompiler.TreeConverter.compilation_context.instance.create_typed_file_type(cpn.type.element_type,null);
						return cpn;
					}
					else if (cpn.type.type_special_kind == type_special_kind.set_type &&
				    (!PascalABCCompiler.TreeConverter.compilation_context.instance.TypedSets.ContainsKey(cpn.type.element_type) || PascalABCCompiler.TreeConverter.compilation_context.instance.TypedSets[cpn.type.element_type] != cpn.type))
					{
						cpn.type = PascalABCCompiler.TreeConverter.compilation_context.instance.create_set_type(cpn.type.element_type,null);
						return cpn;
					}
					else if (cpn.type.type_special_kind == type_special_kind.short_string &&
					        (!PascalABCCompiler.TreeConverter.compilation_context.instance.ShortStringTypes.ContainsKey((cpn.type as short_string_type_node).Length) || PascalABCCompiler.TreeConverter.compilation_context.instance.ShortStringTypes[(cpn.type as short_string_type_node).Length] != cpn.type))
					{
						cpn.type = PascalABCCompiler.TreeConverter.compilation_context.instance.create_short_string_type((cpn.type as short_string_type_node).Length,null);
						return cpn;
					}
				}
			}
			cpn = new compiled_variable_definition(pi);
			
			if (PascalABCCompiler.TreeConverter.compilation_context.instance != null && PascalABCCompiler.TreeConverter.compilation_context.instance.syntax_tree_visitor.compiled_unit != null && PascalABCCompiler.TreeConverter.compilation_context.instance.converted_namespace != null)
			{
				object[] attrs = pi.GetCustomAttributes(false);
				for (int i=0; i<attrs.Length; i++)
				{
					Type t = attrs[i].GetType();
					if (t.FullName == compiler_string_consts.file_of_attr_name)
					{
						object o = t.GetField("Type",BindingFlags.Public|BindingFlags.Instance).GetValue(attrs[i]);
						type_node tn = null;
                    	if (o is Type)
                    		t = o as Type;
                    	else
                   		{
                    		t = t.Assembly.GetType(o as string,false);
                    		tn = CreatePascalType(t);
                   		}
                    	if (tn != null)
                    	cpn.type = PascalABCCompiler.TreeConverter.compilation_context.instance.create_typed_file_type(tn,null);
                    	else
						cpn.type = PascalABCCompiler.TreeConverter.compilation_context.instance.create_typed_file_type(compiled_type_node.get_type_node(t),null);
					}
					else if (t.FullName == compiler_string_consts.set_of_attr_name)
					{
						object o = t.GetField("Type",BindingFlags.Public|BindingFlags.Instance).GetValue(attrs[i]);
						type_node tn = null;
                    	if (o is Type)
                    		t = o as Type;
                    	else
                   		{
                    		t = t.Assembly.GetType(o as string,false);
                    		tn = CreatePascalType(t);
                   		}
                    	if (tn != null)
                    	cpn.type = PascalABCCompiler.TreeConverter.compilation_context.instance.create_set_type(tn,null);
                    	else
						cpn.type = PascalABCCompiler.TreeConverter.compilation_context.instance.create_set_type(compiled_type_node.get_type_node(t),null);
					}
					else if (t.FullName == compiler_string_consts.short_string_attr_name)
					{
						int len = (int)t.GetField("Length",BindingFlags.Public|BindingFlags.Instance).GetValue(attrs[i]);
						cpn.type = PascalABCCompiler.TreeConverter.compilation_context.instance.create_short_string_type(len,null);
					}
				}
			}
			else
				return cpn;
			field_nodes[pi] = cpn;
			return cpn;
		}

        private static constant_node CreateConstantNode(object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                    return new TreeRealization.bool_const_node((Boolean)value, null);
                case TypeCode.SByte:
                    return new TreeRealization.sbyte_const_node((SByte)value, null);
                case TypeCode.Byte:
                    return new TreeRealization.byte_const_node((Byte)value, null);
                case TypeCode.Char:
                    return new TreeRealization.char_const_node((Char)value, null);
                case TypeCode.Int16:
                    return new TreeRealization.short_const_node((Int16)value, null);
                case TypeCode.UInt16:
                    return new TreeRealization.ushort_const_node((UInt16)value, null);
                case TypeCode.Int32:
                    return new TreeRealization.int_const_node((Int32)value, null);
                case TypeCode.UInt32:
                    return new TreeRealization.uint_const_node((UInt32)value, null);
                case TypeCode.Int64:
                    return new TreeRealization.long_const_node((Int64)value, null);
                case TypeCode.UInt64:
                    return new TreeRealization.ulong_const_node((UInt64)value, null);
                case TypeCode.Single:
                    return new TreeRealization.float_const_node((Single)value, null);
                case TypeCode.Double:
                    return new TreeRealization.double_const_node((Double)value, null);
                case TypeCode.String:
                    return new TreeRealization.string_const_node((String)value, null);
                default:
                    return null;
            }
        }

        public static compiled_class_constant_definition ConvertToConstant(compiled_variable_definition cvd)
        {
            if (cvd.compiled_field.IsLiteral && cvd.compiled_field.FieldType.IsEnum)
                return new compiled_class_constant_definition(cvd.compiled_field, new enum_const_node((int)cvd.compiled_field.GetRawConstantValue(), cvd.type, null));
            else
            	return null;
        }
        public static compiled_class_constant_definition GetConstantFieldNode(FieldInfo fi)
        {
            compiled_class_constant_definition cccd = field_nodes[fi] as compiled_class_constant_definition;
            if (cccd != null) return cccd;
            constant_node cn = CreateConstantNode(fi.GetRawConstantValue());
            if (cn == null)
                return null;
            cccd = new compiled_class_constant_definition(fi, cn);
            field_nodes[fi] = cccd;
            return cccd;
        }

        public static field_access_level GetFieldAccessLevel(FieldInfo fi)
        {
            if (fi.IsPublic)
            {
                return SemanticTree.field_access_level.fal_public;
            }
            if (fi.IsPrivate)
            {
                return SemanticTree.field_access_level.fal_private;
            }
            return SemanticTree.field_access_level.fal_protected;
        }

        public static compiled_event GetEvent(EventInfo ei)
        {
            compiled_event ce = field_nodes[ei] as compiled_event;
            if (ce != null)
            {
                return ce;
            }
            ce = new compiled_event(ei);
            field_nodes[ei] = ce;
            return ce;
        }

        public static System.Reflection.MethodInfo GetReadAccessor(PropertyInfo pi)
        {
            MethodInfo mi = pi.GetGetMethod(true);
            if (mi != null && !mi.IsPrivate && !mi.IsAssembly)
            	return mi;
            return null;
        }

        public static System.Reflection.MethodInfo GetWriteAccessor(PropertyInfo pi)
        {
            MethodInfo mi = pi.GetSetMethod(true);
            if (mi != null && !mi.IsPrivate && !mi.IsAssembly)
            	return mi;
            return null;
        }

        public static compiled_function_node get_compiled_method(compiled_type_node declaring_type, string method_name,
            params compiled_type_node[] operands)
        {
            Type[] arr = new Type[operands.Length];
            for (int i = 0; i < operands.Length; i++)
            {
                arr[i] = operands[i].compiled_type;
            }
            MethodInfo mi = declaring_type.compiled_type.GetMethod(method_name, arr);
            compiled_function_node cfn = compiled_function_node.get_compiled_method(mi);
            return cfn;
        }

        //Возвращает акцессор get, если есть. Иначе возвращает акцессор set, если есть. Иначе возвращает null.
        public static System.Reflection.MethodInfo GetAnyAccessor(PropertyInfo pi)
        {
            MethodInfo get = pi.GetGetMethod(true);
            MethodInfo set = pi.GetSetMethod(true);
            if (get != null) 
            if(get.IsPrivate)
            {
            	if (set != null && !set.IsPrivate)
            		return set;
            	else
            		return get;
            }
            else if (get.IsFamily)
            {
            	if (set != null && set.IsPublic)
            		return set;
            	else
            		return get;
            }
            else if (get.IsAssembly)
            {
            	if (set != null && (set.IsPublic||set.IsFamily))
            		return set;
            	else
            		return get;
            }
            else
            return get;
            //return pi.GetSetMethod(true);
            return set;
        }

        public static compiled_constructor_node GetConstructorNode(ConstructorInfo ci)
        {
        	return (compiled_constructor_node)constr_nodes[ci];
        }

        public static void AddConstructor(ConstructorInfo ci, compiled_constructor_node ccn)
        {
            constr_nodes[ci] = ccn;
        }

		public static Type FindType(string name)
		{
            FoundInfo fi = null;
            if (type_search_cache.TryGetValue(name, out fi))
            {
                if (!fi.exists)
                    return null;
                TypeInfo ti = fi.type_infos[0].type_info;
                if (ti != null)
                    return ti.type;
            }
			TypeInfo t = (TypeInfo)types[name];
			if (t != null)
            {
                fi = new FoundInfo(true, t);
                type_search_cache[name] = fi;
                return t.type;
            }
            else
            {
                fi = new FoundInfo(false);
                type_search_cache[name] = fi;
            }
            
			return null;
		}

        public static Type FindRtlType(string name)
        {
            return PABCSystemType.Assembly.GetType(name, false, true);
        }

        public static template_class FindCompiledTemplateType(string name)
        {
            object o = compiled_pascal_types[name];
            if (o != null)
            {
                template_class tc = o as template_class;
                if (tc == null && PascalABCCompiler.TreeConverter.compilation_context.instance != null && PascalABCCompiler.TreeConverter.compilation_context.instance.syntax_tree_visitor.compiled_unit != null && PascalABCCompiler.TreeConverter.compilation_context.instance.converted_namespace != null)
                {
                    Type t = o as Type;
                    if (t != null)
                    {
                        tc = CreateTemplateClassType(t);
                        compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = tc;
                    }
                    else
                        return null;
                }
                return tc;
            }
            return null;
        }

        public static type_node FindCompiledPascalType(string name)
        {
            object o = compiled_pascal_types[name];
            if (o != null)
            {
                type_node tn = o as type_node;
                if (tn == null && PascalABCCompiler.TreeConverter.compilation_context.instance != null && PascalABCCompiler.TreeConverter.compilation_context.instance.syntax_tree_visitor.compiled_unit != null && PascalABCCompiler.TreeConverter.compilation_context.instance.converted_namespace != null)
                {
                    Type t = o as Type;
                    if (t != null)
                    {
                        tn = CreatePascalType(t);
                        if (tn != null)
                            compiled_pascal_types[t.Namespace + "." + t.Name.Substring(1)] = tn;
                        else
                            return null;
                    }
                    else
                        return null;
                }
                else
                {
                    if (PascalABCCompiler.TreeConverter.compilation_context.instance != null && PascalABCCompiler.TreeConverter.compilation_context.instance.syntax_tree_visitor.compiled_unit != null && PascalABCCompiler.TreeConverter.compilation_context.instance.converted_namespace != null)
                    {
                        if (tn.type_special_kind == type_special_kind.typed_file &&
                        (!PascalABCCompiler.TreeConverter.compilation_context.instance.TypedFiles.ContainsKey(tn.element_type) || PascalABCCompiler.TreeConverter.compilation_context.instance.TypedFiles[tn.element_type] != tn))
                        {
                            tn = PascalABCCompiler.TreeConverter.compilation_context.instance.create_typed_file_type(tn.element_type, null);
                            return tn;
                        }
                        else if (tn.type_special_kind == type_special_kind.set_type &&
                            (!PascalABCCompiler.TreeConverter.compilation_context.instance.TypedSets.ContainsKey(tn.element_type) || PascalABCCompiler.TreeConverter.compilation_context.instance.TypedSets[tn.element_type] != tn))
                        {
                            tn = PascalABCCompiler.TreeConverter.compilation_context.instance.create_set_type(tn.element_type, null);
                            return tn;
                        }
                        else if (tn.type_special_kind == type_special_kind.short_string &&
                            (!PascalABCCompiler.TreeConverter.compilation_context.instance.ShortStringTypes.ContainsKey((tn as short_string_type_node).Length) || PascalABCCompiler.TreeConverter.compilation_context.instance.ShortStringTypes[(tn as short_string_type_node).Length] != tn))
                        {
                            tn = PascalABCCompiler.TreeConverter.compilation_context.instance.create_short_string_type((tn as short_string_type_node).Length, null);
                            return tn;
                        }
                    }
                    return tn;
                }
            }
            return null;
        }
		
		public static Type FindType(string name, PascalABCCompiler.TreeRealization.using_namespace_list _unar)
        {
            FoundInfo fi = null;
            if (type_search_cache.TryGetValue(name, out fi))
            {
                if (!fi.exists)
                    return null;
                List<TypeInfo> ti_list = fi.GetTypesByNamespaceList(_unar);
                foreach (TypeInfo ti in ti_list)
                    if (cur_used_assemblies.ContainsKey(ti.type.Assembly))
                        return ti.type;
            }
            object o = types[name];
            if (o == null)
            {
                type_search_cache[name] = new FoundInfo(false);
                return null;
            }
            if (o is TypeInfo)
            {
                TypeInfo t = o as TypeInfo;
                if (t.type.FullName != null && t.type.FullName.IndexOf('.') == -1 && t.type.FullName.IndexOf('+') == -1)
                {
                    if (!type_search_cache.TryGetValue(name, out fi))
                    {
                        fi = new FoundInfo(true, t);
                        type_search_cache[name] = fi;
                    }
                    else
                    {
                        fi.type_infos.Add(new TypeNamespaceInfo(t, null));
                    }
                    if (cur_used_assemblies.ContainsKey(t.type.Assembly))
                        return t.type;
                    else
                        return null;
                }
                for (int i = 0; i < _unar.Count; i++)
                {
                    if (string.Compare(_unar[i].namespace_name + "." + name, t.FullName, true) == 0)
                    {
                        if (!type_search_cache.TryGetValue(name, out fi))
                        {
                            fi = new FoundInfo(true, t, _unar[i]);
                            type_search_cache[name] = fi;
                        }
                        else
                        {
                            fi.type_infos.Add(new TypeNamespaceInfo(t, _unar[i]));
                        }
                        if (cur_used_assemblies.ContainsKey(t.type.Assembly))
                            return t.type;
                        else
                            return null;
                    }
                    
                }
                return null;
            }
            else if (o is List<TypeInfo>)
            {
                foreach (TypeInfo t in o as List<TypeInfo>)
                {
                    if (t.type.FullName != null && t.type.FullName.IndexOf('.') == -1 && t.type.FullName.IndexOf('+') == -1)
                    {
                        if (!type_search_cache.TryGetValue(name, out fi))
                        {
                            fi = new FoundInfo(true, t, null);
                            type_search_cache[name] = fi;
                        }
                        else
                        {
                            fi.type_infos.Add(new TypeNamespaceInfo(t, null));
                        }
                        if (cur_used_assemblies.ContainsKey(t.type.Assembly))
                            return t.type;
                    }
                    for (int i = 0; i < _unar.Count; i++)
                    {
                        if (string.Compare(_unar[i].namespace_name + "." + name, t.FullName, true) == 0)
                        {
                            if (!type_search_cache.TryGetValue(name, out fi))
                            {
                                fi = new FoundInfo(true, t, _unar[i]);
                                type_search_cache[name] = fi;
                            }
                            else
                            {
                                fi.type_infos.Add(new TypeNamespaceInfo(t, _unar[i]));
                            }
                            if (cur_used_assemblies.ContainsKey(t.type.Assembly))
                                return t.type;
                            //else
                            //    return null;
                        }
                    }
                }
                return null;
            }
            for (int i = 0; i < _unar.Count; i++)
            {
                o = types[_unar[i].namespace_name + "." + name];
                if (o == null)
                    continue;
                if (o is TypeInfo)
                    return (o as TypeInfo).type;
                List<TypeInfo> typs = o as List<TypeInfo>;
                List<TypeInfo> founded_types = new List<TypeInfo>();
                foreach (TypeInfo t in typs)
                {
                    if (cur_used_assemblies.ContainsKey(t.type.Assembly))
                        founded_types.Add(t);
                }
                if (founded_types.Count == 1 && cur_used_assemblies.ContainsKey(founded_types[0].type.Assembly))
                    return founded_types[0].type;
                else
                    return null;
            }
            return null;
        }

		public static void AddType(string name, Type t)
		{
			types[name] = new TypeInfo(t, t.FullName);
            AddGenericInfo(t);
		}
		
		public static Type FindTypeOrCreate(string name)
		{
			TypeInfo ti = types[name] as TypeInfo;
			if (ti != null /*&& cur_used_assemblies.ContainsKey(t.Assembly)*/) return ti.type;
			//ivan added - runtime types adding
			Type t = Type.GetType(name, false, true);
            if (t == null)
                foreach (Assembly a in assemblies.Values)
                {
                    t = a.GetType(name, false, true);
                    if (t != null) break;
                }
            if (t != null)
            {
                types[name] = new TypeInfo(t, t.FullName);
                AddGenericInfo(t);
            }
			return t;
		}
		
		public static Type FindAnyTypeInNamespace(string name)
		{
			foreach (string s in types.Keys)
			{
				TypeInfo ti = types[s] as TypeInfo;
				if (string.Compare(ti.type.Namespace,name, true) == 0)
				{
					return ti.type;
				}
			}
			return null;
		}
		
		private static void AddTypeToNamespace(string name, Type[] typs)
		{
			ns_types[name] = typs;
		}
		
		public static Type[] FindTypesInNamespace(string name)
		{
			List<Type> lst = new List<Type>();
			Type[] typs = (Type[])ns_types[name];
			if (typs != null) return typs;
			foreach (string s in types.Keys)
			{
				TypeInfo ti = types[s] as TypeInfo;
				if (ti != null)
				if (string.Compare(ti.type.Namespace,name, true) == 0)
				{
					lst.Add(ti.type);
				}
			}
			if (lst.Count == 0) 
			{
				AddTypeToNamespace(name,new Type[0]);
				return null;
			}
			typs = lst.ToArray();
			AddTypeToNamespace(name,typs);
			return typs;
		}
		
		public static string[] FindSubNamespaces(string name)
		{
			List<string> lst = new List<string>();
			foreach (string s in namespaces.Keys)
			{
				if (s != name && s.StartsWith(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (s.Length >= name.Length + 1)
                        lst.Add(s.Substring(name.Length + 1));
                }
					
			}
			if (lst == null) return null;
			return lst.ToArray();
		}
		
        private static Hashtable InitHandles(Assembly a)
        {
            Type[] types = a.GetTypes();
            Hashtable ht = new Hashtable();
            foreach (Type t in types)
                ht[(int)t.MetadataToken] = t;
            type_handles[a] = ht;
            return ht;
        }

        private static Hashtable InitMethodHandles(Type t)
        {
            MethodInfo[] mis = t.GetMethods();
            Hashtable ht = new Hashtable();
            foreach (MethodInfo mi in mis)
                ht[(int)mi.MetadataToken] = mi;
            method_handles[t] = ht;
            return ht;
        }

        private static Hashtable InitConstructorHandles(Type t)
        {
            ConstructorInfo[] cis = t.GetConstructors();
            Hashtable ht = new Hashtable();
            foreach (ConstructorInfo ci in cis)
                ht[(int)ci.MetadataToken] = ci;
            constr_handles[t] = ht;
            return ht;
        }

        private static Hashtable InitFieldHandles(Type t)
        {
            FieldInfo[] fis = t.GetFields();
            Hashtable ht = new Hashtable();
            foreach (FieldInfo fi in fis)
                ht[(int)fi.MetadataToken] = fi;
            field_handles[t] = ht;
            return ht;
        }

        public static Type FindTypeByHandle(Assembly a, int handle)
        {
            Hashtable ht = (Hashtable)type_handles[a];
            if (ht == null) ht = InitHandles(a);
            Type t = (Type)ht[handle];
            if (t == null)
                return (Type)special_types[handle];
            return t;
        }

        public static MethodInfo FindMethodByHandle(Type t, int handle)
        {
            Hashtable ht = (Hashtable)method_handles[t];
            if (ht == null) ht = InitMethodHandles(t);
            return (MethodInfo)ht[handle];
        }

        public static ConstructorInfo FindConstructorByHandle(Type t, int handle)
        {
            Hashtable ht = (Hashtable)constr_handles[t];
            if (ht == null) ht = InitConstructorHandles(t);
            return (ConstructorInfo)ht[handle];
        }

        public static FieldInfo FindFieldByHandle(Type t, int handle)
        {
            Hashtable ht = (Hashtable)field_handles[t];
            if (ht == null) ht = InitFieldHandles(t);
            return (FieldInfo)ht[handle];
        }

        public static void AddGenericInfo(Type t)
        {
            if (t == null || !t.IsGenericTypeDefinition)
                return;
            int n;
            string s = compiler_string_consts.GetGenericTypeInformation(t.Name, out n).ToLower();
            if (n == 0)
            {
                return;
            }
            List<int> counts;
            bool exists = generics_names.TryGetValue(s, out counts);
            if (!exists)
            {
                counts = new List<int>();
                generics_names[s] = counts;
            }
            if (counts.IndexOf(n) < 0)
            {
                counts.Add(n);
            }
        }

        public static constant_node make_constant(object val)
        {
            try
            {
                Type t = val.GetType();
                if (t == typeof(int))
                    return new int_const_node((int)val, null);
                if (t == typeof(byte))
                    return new byte_const_node((byte)val, null);
                if (t == typeof(sbyte))
                    return new sbyte_const_node((sbyte)val, null);
                if (t == typeof(short))
                    return new short_const_node((short)val, null);
                if (t == typeof(ushort))
                    return new ushort_const_node((ushort)val, null);
                if (t == typeof(uint))
                    return new uint_const_node((uint)val, null);
                if (t == typeof(long))
                    return new long_const_node((long)val, null);
                if (t == typeof(ulong))
                    return new ulong_const_node((ulong)val, null);
                if (t == typeof(char))
                    return new char_const_node((char)val, null);
                if (t == typeof(string))
                    return new string_const_node((string)val, null);
                if (t == typeof(double))
                    return new double_const_node((double)val, null);
                if (t == typeof(float))
                    return new double_const_node((float)val, null);
                if (t == typeof(bool))
                    return new bool_const_node((bool)val, null);
                if (t.IsEnum)
                    return new enum_const_node(Convert.ToInt32(val), compiled_type_node.get_type_node(t), null);
                if (t.FullName == "System.Reflection.Missing")
                    return new compiled_static_field_reference_as_constant(new static_compiled_variable_reference(new compiled_variable_definition(t.GetField("Value", BindingFlags.Public | BindingFlags.Static)), null), null);
            }
            catch
            {
            }
            return null;
        }
	}
}

