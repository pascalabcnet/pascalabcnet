// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Reflection;
using System.Text;
using System.Globalization;
using PascalABCCompiler.Parsers;

namespace CodeCompletion
{
	public class AbstractDispatcher
	{
        public virtual void Update(PascalABCCompiler.Parsers.SymInfo si)
		{
			
		}
	}
	
	public class AssemblyDocCache
	{
		private static Hashtable ht=new Hashtable();

        public static string Load(Assembly a, string path)
        {
            return CodeCompletionTools.AssemblyDocCache.Load(a, path);
        }

        internal static string GetNormalHint(string s)
        {
            return CodeCompletionTools.AssemblyDocCache.GetNormalHint(s);
        }
		
		public static string GetDocumentation(Type t)
		{
            return CodeCompletionTools.AssemblyDocCache.GetDocumentation(t);
		}

        public static string GetFullDocumentation(Type t)
        {
            return CodeCompletionTools.AssemblyDocCache.GetFullDocumentation(t);
        }

		public static string GetParamNames(ConstructorInfo mi)
		{
            return CodeCompletionTools.AssemblyDocCache.GetParamNames(mi);
		}
		
		public static string GetParamNames(MethodInfo mi)
		{
            return CodeCompletionTools.AssemblyDocCache.GetParamNames(mi);
		}

        private static string GetTypeName(Type t)
        {
            return CodeCompletionTools.AssemblyDocCache.GetTypeName(t);
        }
		
		private static Dictionary<MemberInfo,SymInfo> doc_wait_list = new Dictionary<MemberInfo,SymInfo>();
		private static Dictionary<string,SymInfo> doc_namespace_list = new Dictionary<string,SymInfo>();
		private static List<MemberInfo> elem_cache = new List<MemberInfo>();
		private static List<Type> namespace_elem_cache = new List<Type>();
		private static System.Threading.Thread th = null;
		public static AbstractDispatcher dispatcher=null;
		
		public static void Reset()
		{
			if (th != null)
			while (th.ThreadState == ThreadState.Running);
			
			doc_wait_list.Clear();
			elem_cache.Clear();
			//namespace_elem_cache.Clear();
			//doc_namespace_list.Clear();
		}

        private static void internalCompleteDocumentation()
        {
            try
            {
                if (dispatcher == null) return;
                for (int i = 0; i < elem_cache.Count; i++)
                {
                    MemberInfo mi = elem_cache[i];
                    SymInfo si = null;
                    switch (mi.MemberType)
                    {
                        case MemberTypes.TypeInfo:
                            si = doc_wait_list[mi];
                            si.description += "\n" + AssemblyDocCache.GetDocumentation(mi as Type);
                            si.has_doc = true;
                            dispatcher.Update(si);
                            break;
                        case MemberTypes.Method:
                            si = doc_wait_list[mi];
                            MethodInfo meth = mi as MethodInfo;
                            si.description += "\n" + AssemblyDocCache.GetDocumentation(meth);//AssemblyDocCache.GetDocumentation(meth.DeclaringType.Assembly,"M:"+meth.DeclaringType.FullName+"."+meth.Name+GetParamNames(meth));
                            si.has_doc = true;
                            dispatcher.Update(si);
                            break;
                        case MemberTypes.Property:
                            si = doc_wait_list[mi];
                            PropertyInfo pi = mi as PropertyInfo;
                            si.description += "\n" + AssemblyDocCache.GetDocumentation(pi);//AssemblyDocCache.GetDocumentation(pi.DeclaringType.Assembly,"P:"+pi.DeclaringType.FullName+"."+pi.Name);
                            si.has_doc = true;
                            dispatcher.Update(si);
                            break;
                        case MemberTypes.Event:
                            si = doc_wait_list[mi];
                            EventInfo ei = mi as EventInfo;
                            si.description += "\n" + AssemblyDocCache.GetDocumentation(ei);//AssemblyDocCache.GetDocumentation(ei.DeclaringType.Assembly,"E:"+ei.DeclaringType.FullName+"."+ei.Name);
                            si.has_doc = true;
                            dispatcher.Update(si);
                            break;
                        case MemberTypes.Constructor:
                            si = doc_wait_list[mi];
                            ConstructorInfo ci = mi as ConstructorInfo;
                            si.description += "\n" + AssemblyDocCache.GetDocumentation(ci);//AssemblyDocCache.GetDocumentation(ci.DeclaringType.Assembly,"M:"+ci.DeclaringType.FullName+".#ctor"+GetParamNames(ci));
                            si.has_doc = true;
                            dispatcher.Update(si);
                            break;
                        case MemberTypes.Field:
                            si = doc_wait_list[mi];
                            FieldInfo fi = mi as FieldInfo;
                            si.description += "\n" + AssemblyDocCache.GetDocumentation(fi);//AssemblyDocCache.GetDocumentation(fi.DeclaringType.Assembly,"F:"+fi.DeclaringType.FullName+"."+fi.Name);
                            si.has_doc = true;
                            dispatcher.Update(si);
                            break;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
		
		public static void CompleteDocumentation()
		{
			th = new System.Threading.Thread(new System.Threading.ThreadStart(internalCompleteDocumentation));
			th.Start();
		}
		
		public static void AddDescribeToComplete(SymInfo si, Type t)
		{
			elem_cache.Add(t);
			doc_wait_list[t] = si;
		}
		
		public static void AddDescribeToComplete(SymInfo si, Type t, string ns)
		{
			string s=ns;
			if (!doc_namespace_list.ContainsKey(t.Namespace))
			{
				namespace_elem_cache.Add(t);
				doc_namespace_list[ns] = si;
			}
		}
		
		public static void AddDescribeToComplete(SymInfo si, MethodInfo mi)
		{
			elem_cache.Add(mi);
			doc_wait_list[mi] = si;
		}
		
		public static void AddDescribeToComplete(SymInfo si, FieldInfo fi)
		{
			elem_cache.Add(fi);
			doc_wait_list[fi] = si;
		}
		
		public static void AddDescribeToComplete(SymInfo si, EventInfo ei)
		{
			elem_cache.Add(ei);	
			doc_wait_list[ei] = si;
		}
		
		public static void AddDescribeToComplete(SymInfo si, PropertyInfo pi)
		{
			elem_cache.Add(pi);
			doc_wait_list[pi] = si;
		}
		
		public static void AddDescribeToComplete(SymInfo si, ConstructorInfo ci)
		{
			elem_cache.Add(ci);
			doc_wait_list[ci] = si;
		}
		
		public static string GetDocumentation(ConstructorInfo mi)
		{
            return CodeCompletionTools.AssemblyDocCache.GetDocumentation(mi);
		}

        public static string GetFullDocumentation(ConstructorInfo mi)
        {
            return CodeCompletionTools.AssemblyDocCache.GetFullDocumentation(mi);
        }

		public static string GetDocumentation(FieldInfo fi)
		{
            return CodeCompletionTools.AssemblyDocCache.GetDocumentation(fi);
		}

        public static string GetFullDocumentation(FieldInfo fi)
        {
            return CodeCompletionTools.AssemblyDocCache.GetFullDocumentation(fi);
        }

		public static string GetDocumentation(PropertyInfo pi)
		{
            return CodeCompletionTools.AssemblyDocCache.GetDocumentation(pi);
		}

        public static string GetFullDocumentation(PropertyInfo pi)
        {
            return CodeCompletionTools.AssemblyDocCache.GetFullDocumentation(pi);
        }

		public static string GetDocumentation(EventInfo ei)
		{
            return CodeCompletionTools.AssemblyDocCache.GetDocumentation(ei);
		}

        public static string GetFullDocumentation(EventInfo ei)
        {
            return CodeCompletionTools.AssemblyDocCache.GetFullDocumentation(ei);
        }

		private static string GetGenericAddString(MethodInfo mi)
		{
			if (mi.ContainsGenericParameters && mi.IsGenericMethodDefinition)
				return "``"+Convert.ToString(mi.GetGenericArguments().Length);
			else
				return "";
            
		}
		
		public static string GetDocumentation(MethodInfo mi)
		{
            return CodeCompletionTools.AssemblyDocCache.GetDocumentation(mi);
		}

        public static string GetFullDocumentation(MethodInfo mi)
        {
            return CodeCompletionTools.AssemblyDocCache.GetFullDocumentation(mi);
        }

        public static string GetDocumentationForNamespace(string name)
        {
            return "";
        }
		
		public static string GetDocumentation(Assembly a, string descr)
		{
            return CodeCompletionTools.AssemblyDocCache.GetDocumentation(a, descr);
		}
	}
}
