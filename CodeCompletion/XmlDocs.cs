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
        private static readonly object pendingSync = new object();
		private static List<Type> namespace_elem_cache = new List<Type>();
		private static System.Threading.Thread th = null;
		public static AbstractDispatcher dispatcher=null;
		
		public static void Reset()
		{
			if (th != null)
			while (th.ThreadState == ThreadState.Running);
			
            lock (pendingSync)
            {
                doc_wait_list.Clear();
                elem_cache.Clear();
            }
			//namespace_elem_cache.Clear();
			//doc_namespace_list.Clear();
		}

#if PABCNET_MODERN
        public static void ReleaseReferenceGeneration()
        {
            Reset();
            namespace_elem_cache.Clear();
            doc_namespace_list.Clear();
            ht.Clear();
        }
#endif

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

        /// <summary>
        /// Detach pending descriptions on the calling thread. The returned work
        /// uses a snapshot and leaves publication/cancellation to its host.
        /// Use instead of the legacy CompleteDocumentation/Reset pair for this host.
        /// </summary>
        public static Action TakeDocumentationWork(Action<SymInfo, string> publish, Func<bool> isCancelled)
        {
            var pending = new List<KeyValuePair<MemberInfo, SymInfo>>();
            lock (pendingSync)
            {
                foreach (MemberInfo member in elem_cache)
                {
                    SymInfo symbol;
                    if (doc_wait_list.TryGetValue(member, out symbol))
                        pending.Add(new KeyValuePair<MemberInfo, SymInfo>(member, symbol));
                }
                elem_cache.Clear();
                doc_wait_list.Clear();
            }
            return () => {
                foreach (var item in pending)
                {
                    if (isCancelled()) return;
                    if (item.Value.has_doc) continue;
                    try
                    {
                        string text;
                        MemberInfo member = item.Key;
                        if (member is Type) text = GetDocumentation((Type)member);
                        else if (member is MethodInfo) text = GetDocumentation((MethodInfo)member);
                        else if (member is PropertyInfo) text = GetDocumentation((PropertyInfo)member);
                        else if (member is EventInfo) text = GetDocumentation((EventInfo)member);
                        else if (member is ConstructorInfo) text = GetDocumentation((ConstructorInfo)member);
                        else if (member is FieldInfo) text = GetDocumentation((FieldInfo)member);
                        else continue;
                        if (isCancelled()) return;
                        publish(item.Value, text);
                    }
                    catch (Exception) { /* A missing description must not stop the remaining items. */ }
                }
            };
        }
		
		public static void AddDescribeToComplete(SymInfo si, Type t)
		{
            AddPendingDescription(si, t);
		}
		
		public static void AddDescribeToComplete(SymInfo si, Type t, string ns)
		{
			string s=ns;
            lock (pendingSync)
                if (!doc_namespace_list.ContainsKey(t.Namespace))
                {
                    namespace_elem_cache.Add(t);
                    doc_namespace_list[ns] = si;
                }
		}
		
		public static void AddDescribeToComplete(SymInfo si, MethodInfo mi)
		{
            AddPendingDescription(si, mi);
		}
		
		public static void AddDescribeToComplete(SymInfo si, FieldInfo fi)
		{
            AddPendingDescription(si, fi);
		}
		
		public static void AddDescribeToComplete(SymInfo si, EventInfo ei)
		{
            AddPendingDescription(si, ei);
		}
		
		public static void AddDescribeToComplete(SymInfo si, PropertyInfo pi)
		{
            AddPendingDescription(si, pi);
		}
		
		public static void AddDescribeToComplete(SymInfo si, ConstructorInfo ci)
		{
            AddPendingDescription(si, ci);
		}

        private static void AddPendingDescription(SymInfo symbol, MemberInfo member)
        {
            lock (pendingSync)
            {
                elem_cache.Add(member);
                doc_wait_list[member] = symbol;
            }
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
