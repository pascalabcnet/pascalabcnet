// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;
using System.Reflection;
using System.Text;
using PascalABCCompiler.Parsers;

namespace CodeCompletion
{
	
	public class UnitDocCache
	{
		private static Hashtable ht = new Hashtable();
		private static List<SymScope> elem_cache = new List<SymScope>();
		private static System.Threading.Thread th = null;
		public static AbstractDispatcher dispatcher=null;
		
		public static void Reset()
		{
			if (th != null)
			while (th.ThreadState == ThreadState.Running);
			
			elem_cache.Clear();
			//namespace_elem_cache.Clear();
			//doc_namespace_list.Clear();
		}
		
		public static void Load(SymScope unit, string fileName)
		{
			if (ht[unit] != null) return;
			string xml_loc = CodeCompletionTools.XmlDoc.LookupLocalizedXmlDocForUnit(fileName, CodeCompletionController.currentLanguageISO);
			if (xml_loc != null)
			{
                CodeCompletionTools.XmlDoc xdoc = CodeCompletionTools.XmlDoc.Load(xml_loc, Environment.GetEnvironmentVariable("TEMP"), false);
				ht[unit] = xdoc;
			}
		}
		
		public static void LoadWithSources(SymScope unit, string fileName)
		{
			if (ht[unit] != null) return;
            string xml_loc = CodeCompletionTools.XmlDoc.LookupLocalizedXmlDocForUnitWithSources(fileName, CodeCompletionController.currentLanguageISO);
			if (xml_loc != null)
			{
                CodeCompletionTools.XmlDoc xdoc = CodeCompletionTools.XmlDoc.Load(xml_loc, Environment.GetEnvironmentVariable("TEMP"), false);
				ht[unit] = xdoc;
			}
		}
		
		private static void internalCompleteDocumentation()
		{
			try
			{
				if (dispatcher == null) return;
				for (int i=0; i<elem_cache.Count; i++)
				{
					SymScope ss = elem_cache[i];
					if (ss.si.has_doc) continue;
					if (ss is TypeScope)
					{
						ss.si.description += "\n"+UnitDocCache.GetDocumentation(ss as TypeScope);
						ss.si.has_doc = true;
						dispatcher.Update(ss.si);
					}
					else if (ss is ElementScope)
					{
						ss.si.description += "\n"+UnitDocCache.GetDocumentation(ss as ElementScope);
						ss.si.has_doc = true;
						dispatcher.Update(ss.si);
					}
					else if (ss is ProcScope)
					{
						ss.si.description += "\n"+UnitDocCache.GetDocumentation(ss as ProcScope);
						ss.si.has_doc = true;
						dispatcher.Update(ss.si);
					}
					else if (ss is InterfaceUnitScope)
					{
						ss.si.description += "\n"+UnitDocCache.GetDocumentation(ss as InterfaceUnitScope);
						ss.si.has_doc = true;
						dispatcher.Update(ss.si);
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
		
		private static string GetNormalHint(string s)
		{
			return AssemblyDocCache.GetNormalHint(s);
		}
		
		public static string GetDocumentation(TypeScope t)
		{
			try
			{
                if (t.declaringUnit != null)
                {
                    CodeCompletionTools.XmlDoc xdoc = (CodeCompletionTools.XmlDoc)ht[t.declaringUnit];
                    if (xdoc != null)
                    {
                        string s = GetNormalHint(xdoc.GetDocumentation("T:" + t.si.name, true));
                        return s;
                    }
                }
			}
			catch(Exception e)
			{
				
			}
			return "";
		}
		
		public static string GetDocumentation(ElementScope es)
		{
			try
			{
                if (es.declaringUnit == null)
                    return "";
                CodeCompletionTools.XmlDoc xdoc = (CodeCompletionTools.XmlDoc)ht[es.declaringUnit];
				if (xdoc != null)
				{
					string s = "";
					switch (es.si.kind)
					{
						case SymbolKind.Variable : s = GetNormalHint(xdoc.GetDocumentation("V:"+es.si.name,true)); break;
						case SymbolKind.Field : s = GetNormalHint(xdoc.GetDocumentation("F:"+es.topScope.si.name+"."+es.si.name,true)); break;
						case SymbolKind.Property : s = GetNormalHint(xdoc.GetDocumentation("P:"+es.topScope.si.name+"."+es.si.name,true)); break;
						case SymbolKind.Event : s = GetNormalHint(xdoc.GetDocumentation("E:"+es.topScope.si.name+"."+es.si.name,true)); break;
						case SymbolKind.Constant : s = GetNormalHint(xdoc.GetDocumentation("C:"+es.si.name,true)); break;
					}
					return s;
				}
			}
			catch(Exception e)
			{
				
			}
			return "";
		}
		
		public static string GetDocumentation(ProcScope mi)
		{
			try
			{

                if (mi.declaringUnit != null)
                {
                    CodeCompletionTools.XmlDoc xdoc = (CodeCompletionTools.XmlDoc)ht[mi.declaringUnit];
                    if (xdoc != null)
                    {
                        string s = "";
                        if (mi.declaringType != null)
                        {
                            if (!mi.is_constructor)
                                s = GetNormalHint(xdoc.GetDocumentation("M:" + mi.declaringType.si.name + "." + mi.si.name + GetParamNames(mi), true));
                            else
                                s = GetNormalHint(xdoc.GetDocumentation("M:" + mi.declaringType.si.name + ".#ctor" + GetParamNames(mi), true));
                        }
                        else
                            s = GetNormalHint(xdoc.GetDocumentation("M:" + mi.name + GetParamNames(mi), true));
                        return s;
                    }
                }
			}
			catch(Exception e)
			{
				
			}
			return "";
		}
		
		public static string GetDocumentation(InterfaceUnitScope mi)
		{
			try
			{
                CodeCompletionTools.XmlDoc xdoc = (CodeCompletionTools.XmlDoc)ht[mi];
				if (xdoc != null)
				{
					return GetNormalHint(xdoc.GetDocumentation(mi.si.name,true));
				}
			}
			catch(Exception e)
			{
				
			}
			return "";
		}
		
		public static string GetDocumentation(SymScope unit, string descr)
		{
			try
			{
                CodeCompletionTools.XmlDoc xdoc = (CodeCompletionTools.XmlDoc)ht[unit];
				if (xdoc != null)
				{
					string s = GetNormalHint(xdoc.GetDocumentation(descr,false));
					return s;
				}
			}
			catch(Exception e)
			{
				
			}
			return "";
		}
		
		private static string GetTypeName(TypeScope typ)
		{
			StringBuilder sb = new StringBuilder();
			if (typ == null)
				return "";
			if (typ is CompiledScope)
				sb.Append((typ as CompiledScope).ctn.FullName);
			else if (typ is ArrayScope)
			{
				ArrayScope arr = typ as ArrayScope;
				if (arr.is_dynamic_arr)
				{
					ArrayScope tmp = arr;
					int j=1;
					while (tmp.elementType is ArrayScope && (tmp.elementType as ArrayScope).is_dynamic_arr)
					{
						j++;
						tmp = tmp.elementType as ArrayScope;
					}
					sb.Append(GetTypeName(tmp.elementType));
					for (int k=0; k<j; k++)
						sb.Append("[]");
				}
				else
				{
					sb.Append("@array[");
					for (int j=0; j<arr.indexes.Length; j++)
					{
						sb.Append(GetTypeName(arr.indexes[j]));
						if (j<arr.indexes.Length-1)
							sb.Append(",");
					}
					sb.Append("]");
					sb.Append("["+GetTypeName(arr.elementType)+"]");
				}
			}
			else if (typ is ProcType)
			{
				sb.Append(GetDelegateName((typ as ProcType).target));
			}
			else if (typ is SetScope)
			{
				sb.Append("@set["+GetTypeName((typ as SetScope).elementType)+"]");
			}
			else if (typ is FileScope)
			{
				if (typ.elementType != null)
				sb.Append("@fileof["+GetTypeName(typ.elementType)+"]");
				else
				sb.Append("@file");
			}
			else if (typ is TypeSynonim)
			{
				sb.Append(GetTypeName((typ as TypeSynonim).actType));
			}
			else if (typ is PointerScope)
			{
				PointerScope ptr = typ as PointerScope;
				int j=1;
				while (ptr.ref_type is PointerScope)
				{
					j++;
					ptr = ptr.ref_type as PointerScope;
				}
				for (int k=0; k<j; k++)
					sb.Append("*");
				sb.Append(ptr.ref_type);
			}
			else if (typ is ShortStringScope)
			{
				sb.Append("@string["+(typ as ShortStringScope).Length+"]");
			}
			else if (typ is DiapasonScope)
			{
				sb.Append("@diap["+(typ as DiapasonScope).left+".."+(typ as DiapasonScope).right+"]");
			}
			else
			sb.Append(typ.declaringUnit.si.name+"."+typ.si.name);
			return sb.ToString();
		}
		
		public static string GetParamNames(ProcScope mi)
		{
			StringBuilder sb = new StringBuilder();
			if (mi.parameters.Count > 0)
			{
				sb.Append("(");
				for (int i=0; i<mi.parameters.Count; i++)
				{
					SymScope typ = mi.parameters[i].sc;
					if (typ is TypeScope) 
						sb.Append(GetTypeName(typ as TypeScope));
					if (mi.parameters[i].param_kind == PascalABCCompiler.SyntaxTree.parametr_kind.var_parametr || mi.parameters[i].param_kind == PascalABCCompiler.SyntaxTree.parametr_kind.const_parametr)
						sb.Append('@');
					if (i<mi.parameters.Count-1)
						sb.Append(',');
				}
				sb.Append(")");
			}
			return sb.ToString();
		}
		
		private static string GetDelegateName(ProcScope value)
		{
			StringBuilder sb = new StringBuilder();
			if (value.return_type != null)
				sb.Append("@function");
			else sb.Append("@procedure");
			sb.Append(GetParamNames(value));
			if (value.return_type != null)
				sb.Append(":"+GetTypeName(value.return_type));
			return sb.ToString();
		}
		
		public static void AddDescribeToComplete(SymScope value)
		{
			if (value is TypeScope) AddDescribeToComplete(value as TypeScope);
			else if (value is ProcScope) AddDescribeToComplete(value as ProcScope);
			else if (value is ElementScope) AddDescribeToComplete(value as ElementScope);
			else if (value is InterfaceUnitScope) AddDescribeToComplete(value as InterfaceUnitScope);
		}
		
		public static void AddDescribeToComplete(TypeScope value)
		{
			elem_cache.Add(value);
		}
		
		public static void AddDescribeToComplete(ElementScope value)
		{
			elem_cache.Add(value);
		}
		
		public static void AddDescribeToComplete(ProcScope value)
		{
			elem_cache.Add(value);
		}
		
		public static void AddDescribeToComplete(InterfaceUnitScope value)
		{
			elem_cache.Add(value);
		}
	}
}

