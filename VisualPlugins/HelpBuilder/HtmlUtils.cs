using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using PascalABCCompiler.SemanticTree;
using System.Text.RegularExpressions;
using System.Reflection;

namespace VisualPascalABCPlugins
{
	public class HelpUtils
	{
		public static HelpBuilder builder;
		
		public static string HTMLSpace = Convert.ToString(((char)0xA0));
		
		public static string get_meth_name(ICommonNamespaceFunctionNode f)
		{
			return f.comprehensive_namespace.namespace_name + "." + f.name;
		}
		
		public static string get_meth_name(ICommonMethodNode f)
		{
			return HelpUtils.get_type_name(f.comperehensive_type) + "." + (f.is_constructor?"Create":f.name);
		}

        public static string get_meth_name(MethodInfo f)
        {
            return HelpUtils.get_type_name(f.DeclaringType) + "." + (f.IsConstructor ? "Create" : f.Name);
        }

        public static string get_meth_name(ConstructorInfo f)
        {
            return HelpUtils.get_type_name(f.DeclaringType) + "." + "Create";
        }

		public static string get_type_name(ITypeSynonym t)
		{
			return builder.parser.main_ns.namespace_name+"."+t.name;
		}
		
		public static string get_type_name(ITypeNode t)
		{
			if (t is ICommonTypeNode)
			{
				return (t as ICommonTypeNode).comprehensive_namespace.namespace_name + "." + t.name;
			}
			else if (t is ICompiledTypeNode)
				return (t as ICompiledTypeNode).name;
			return t.name;
		}

        public static string get_type_name(Type t)
        {
            return t.Name;
            //return get_compiled_type_text(t);
        }

		public static string get_var_name(ICommonNamespaceVariableNode v)
		{
			return v.comprehensive_namespace.namespace_name + "." + v.name;
		}
		
		public static string get_prop_name(ICommonPropertyNode p)
		{
			return get_type_name(p.comperehensive_type) + "." + p.name;
		}

        public static string get_prop_name(PropertyInfo p)
        {
            return get_type_name(p.DeclaringType) + "." + p.Name;
        }

		public static string get_const_name(INamespaceConstantDefinitionNode c)
		{
			return c.comprehensive_namespace.namespace_name + "." + c.name;
		}
		
		public static string get_const_name(IClassConstantDefinitionNode c)
		{
			return get_type_name(c.comperehensive_type) + "." + c.name;
		}
		
		public static string get_field_name(ICommonClassFieldNode f)
		{
			return get_type_name(f.comperehensive_type) + "." + f.name;
		}

        public static string get_field_name(FieldInfo f)
        {
            return get_type_name(f.DeclaringType) + "." + f.Name;
        }

		public static string get_event_name(ICommonEventNode e)
		{
			return get_type_name(e.comperehensive_type) + "." + e.Name;
		}

        public static string get_event_name(EventInfo e)
        {
            return get_type_name(e.DeclaringType) + "." + e.Name;
        }

		public static bool is_pascal_type(ITypeNode t)
		{
			return
				t.type_special_kind == type_special_kind.set_type
				|| t.type_special_kind == type_special_kind.short_string
				|| t.type_special_kind == type_special_kind.typed_file
				|| t.type_special_kind == type_special_kind.binary_file
				|| t.type_special_kind == type_special_kind.diap_type && t.name.Contains("..");
		}
		
		public static bool can_show_members(ICommonTypeNode t)
		{
			switch (t.type_special_kind)
			{
				case type_special_kind.array_wrapper:
				case type_special_kind.set_type:
				case type_special_kind.typed_file:
				case type_special_kind.text_file:
				case type_special_kind.binary_file:
				case type_special_kind.diap_type:
				return false;
			}
			if (t.IsDelegate /*|| t.IsEnum*/)
				return false;
			return true;
		}

        public static bool can_show_members(Type t)
        {
            if (t.BaseType == typeof(Delegate) || t.BaseType == typeof(MulticastDelegate))
                return false;
            return true;
        }
		
		public static bool can_write(ICommonTypeNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			         || is_pascal_type(t) || user_doc_disabled(t.Documentation));
		}
		
		public static bool can_write(ICommonMethodNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			         || builder.parser.is_getter_or_setter(t) || t.name.StartsWith("op_") || builder.parser.is_event_special_method(t)
			        || user_doc_disabled(t.Documentation));
		}
		
		public static bool can_write(ICommonEventNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.Name.Contains("$")
			        || user_doc_disabled(t.Documentation));
		}
		
		public static bool can_write(ICommonClassFieldNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			        || user_doc_disabled(t.Documentation));
		}
		
		public static bool can_write(ICommonNamespaceVariableNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			        || user_doc_disabled(t.Documentation));
		}
		
		public static bool can_write(ICommonParameterNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			        || user_doc_disabled(t.Documentation));
		}
		
		public static bool can_write(INamespaceConstantDefinitionNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			        || user_doc_disabled(t.Documentation));
		}
		
		public static bool can_write(ICommonPropertyNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			        || user_doc_disabled(t.Documentation));
		}
		
		public static bool can_write(ICommonNamespaceFunctionNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			         || user_doc_disabled(t.Documentation) || !Regex.Match(t.name,@"(\w)+", RegexOptions.ECMAScript).Success);
		}
		
		public static bool can_write(IClassConstantDefinitionNode t)
		{
			return !(string.IsNullOrEmpty(t.Documentation) && !builder.options.ShowNoCommentedElements || t.name.Contains("$")
			        || user_doc_disabled(t.Documentation));
		}

        public static bool can_write(Type t)
        {
            if (!t.IsNested)
            return true;
            return false;
        }

        public static bool can_write(MethodInfo t)
        {
            return true;
        }

        public static bool can_write(FieldInfo t)
        {
            return true;
        }

        public static bool can_write(PropertyInfo t)
        {
            return true;
        }

        public static bool can_write(EventInfo t)
        {
            return true;
        }

        public static bool can_write(ConstructorInfo t)
        {
            return true;
        }

		public static string possible_overload_constructor_symbol(ICommonMethodNode f)
		{
			if (builder.parser.is_overloaded_constructor(f))
			{
				int ind = builder.parser.GetConstructorIndex(f);
				if (ind != 1)
				return "$"+builder.parser.GetConstructorIndex(f);
				else
				return "";
			}
			return "";
		}

		public static string possible_overload_symbol(ICommonMethodNode f)
		{
			if (builder.parser.is_overload(f,f.common_comprehensive_type))
			{
				int ind = builder.parser.GetMethodIndex(f,f.common_comprehensive_type);
				if (ind != 1)
				return "$"+builder.parser.GetMethodIndex(f,f.common_comprehensive_type);
				else
				return "";
			}
			return "";
		}
		
		public static string possible_overload_symbol(ICommonNamespaceFunctionNode f)
		{
			if (builder.parser.is_overload(f))
				return "$"+builder.parser.GetMethodIndex(f);
			return "";
		}
		
		public static string get_span_for_type(string name)
		{
			return "<span style=\"color=#0000ff;\">"+name+"</span>";
		}
		
		public static string get_compiled_type_text(Type ctn)
		{
			TypeCode tc = Type.GetTypeCode(ctn);
			if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition && ctn.IsGenericParameter) 
				return get_span_for_type("T");
			if (!ctn.IsEnum)
			{
			switch (tc)
			{
					case TypeCode.Int32 : return "integer";
					case TypeCode.Double : return "real";
					case TypeCode.Boolean : return "boolean";
					case TypeCode.String : return "string";
					case TypeCode.Char : return "char";
					case TypeCode.Byte : return "byte";
					case TypeCode.SByte : return "shortint";
					case TypeCode.Int16 : return "smallint";
					case TypeCode.Int64 : return "int64";
					case TypeCode.UInt16 : return "word";
					case TypeCode.UInt32 : return "longword";
					case TypeCode.UInt64 : return "uint64";
					case TypeCode.Single : return "single";
			}
			if (ctn.Name.Contains("`"))
			{
				int len = ctn.GetGenericArguments().Length;
				Type[] gen_ps = ctn.GetGenericArguments();
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(ctn.Namespace+"."+ctn.Name.Substring(0,ctn.Name.IndexOf('`')));
				sb.Append('<');
				for (int i=0; i<len; i++)
				{
					sb.Append(gen_ps[i].Name);
					if (i<len-1)
					sb.Append(',');
				}
				sb.Append('>');
				return sb.ToString();
			}
			if (ctn.IsPointer)
				if (ctn.FullName == "System.Void*")
				return "pointer";
				else
					return "^"+get_compiled_type_text(ctn.GetElementType());
			if (ctn.IsArray)
				return "array of "+get_compiled_type_text(ctn.GetElementType());
			else 
				return ctn.Name;
			}
			return ctn.Name;
		}
		
		public static string get_compiled_type_html_text(Type ctn)
		{
			TypeCode tc = Type.GetTypeCode(ctn);
			if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition && ctn.IsGenericParameter) 
				return get_span_for_type("T");
			if (!ctn.IsEnum)
			{
			switch (tc)
			{
					case TypeCode.Int32 : return get_span_for_type("integer");
					case TypeCode.Double : return get_span_for_type("real");
					case TypeCode.Boolean : return get_span_for_type("boolean");
					case TypeCode.String : return get_span_for_type("string");
					case TypeCode.Char : return get_span_for_type("char");
					case TypeCode.Byte : return get_span_for_type("byte");
					case TypeCode.SByte : return get_span_for_type("shortint");
					case TypeCode.Int16 : return get_span_for_type("smallint");
					case TypeCode.Int64 : return get_span_for_type("int64");
					case TypeCode.UInt16 : return get_span_for_type("word");
					case TypeCode.UInt32 : return get_span_for_type("longword");
					case TypeCode.UInt64 : return get_span_for_type("uint64");
					case TypeCode.Single : return get_span_for_type("single");
			}
			if (ctn.Name.Contains("`"))
			{
				int len = ctn.GetGenericArguments().Length;
				Type[] gen_ps = ctn.GetGenericArguments();
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(ctn.Namespace+"."+ctn.Name.Substring(0,ctn.Name.IndexOf('`')));
				sb.Append('<');
				for (int i=0; i<len; i++)
				{
					sb.Append(gen_ps[i].Name);
					if (i<len-1)
					sb.Append(',');
				}
				sb.Append('>');
				return sb.ToString();
			}
			if (ctn.IsPointer)
				if (ctn.FullName == "System.Void*")
				return get_span_for_type("pointer");
				else
					return "^"+get_compiled_type_html_text(ctn.GetElementType());
			if (ctn.IsArray)
				return get_span_for_keyword("array")+" "+ get_span_for_keyword("of")+" "+get_compiled_type_html_text(ctn.GetElementType());
			else 
				return "<a href=\"http://msdn.microsoft.com/ru-ru/library/"+ctn.FullName.ToLower()+"(en-us,VS.80).aspx\">"+ctn.Name+"</a>";
			}
			return "<a href=\"http://msdn.microsoft.com/ru-ru/library/"+ctn.FullName.ToLower()+"(en-us,VS.80).aspx\">"+ctn.Name+"</a>";
		}
		
		public static string get_type_text(ITypeNode t)
		{
			if (t is ICompiledTypeNode)
				return get_compiled_type_text((t as ICompiledTypeNode).compiled_type);
			else if (is_pascal_type(t) || t.name.Contains("$"))
				return get_pascal_type(t,false);
			return build_name_with_possible_generic(t,true);
		}
		
		private static string get_pascal_type(ITypeNode t, bool use_tags)
		{
			switch (t.type_special_kind)
			{
				case type_special_kind.array_kind : return get_dynamic_array_type(t,use_tags);
				case type_special_kind.array_wrapper : return get_static_array_type(t,use_tags);
				case type_special_kind.diap_type : return get_diap_type(t,use_tags);
				case type_special_kind.binary_file : 
				if (use_tags)
				return get_span_for_keyword("file");
				else return "file";
				case type_special_kind.set_type : return get_set_type(t,use_tags);
				case type_special_kind.short_string : return get_short_string(t,use_tags);
				case type_special_kind.typed_file : return get_typed_file(t,use_tags);
				case type_special_kind.record : 
				if (use_tags)
				return get_span_for_keyword("record");
				else return "record";
			}
			if (t.IsEnum)
				return get_enum(t,use_tags);
			if (t.IsDelegate && t is ICommonTypeNode)
				return get_delegate(t as ICommonTypeNode,use_tags);
			return t.name;
		}

        public static string build_name_with_possible_generic(MethodInfo f, bool use_lt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(f.Name);
            if (f.GetGenericArguments().Length > 0)
            {
                if (use_lt)
                    sb.Append("&lt");
                else
                    sb.Append("<");
                Type[] generic_params = f.GetGenericArguments();
                for (int i = 0; i < generic_params.Length; i++)
                {
                    sb.Append(generic_params[i].Name);
                    if (i < generic_params.Length - 1)
                        sb.Append(',');
                }
                if (use_lt)
                    sb.Append("&gt");
                else sb.Append(">");
            }
            return sb.ToString();
        }
        
        public static string build_name_with_possible_generic(ICommonFunctionNode f, bool use_lt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(f.name);
            if (f.generic_params != null)
            {
                if (use_lt)
                    sb.Append("&lt");
                else
                    sb.Append("<");
                for (int i = 0; i < f.generic_params.Count; i++)
                {
                    sb.Append(f.generic_params[i].name);
                    if (i < f.generic_params.Count - 1)
                        sb.Append(',');
                }
                if (use_lt)
                    sb.Append("&gt");
                else sb.Append(">");
            }
            return sb.ToString();
        }

		public static string build_name_with_possible_generic(ITypeNode t, bool use_lt)
		{
			StringBuilder sb = new StringBuilder();
			if (t.is_generic_type_definition)
			{
				ICommonTypeNode ctn = t as ICommonTypeNode;
				sb.Append(ctn.name.Substring(0,ctn.name.IndexOf('`')));
				if (use_lt)
				sb.Append("&lt");
				else
					sb.Append("<");
				for (int i=0; i<ctn.generic_params.Count; i++)
				{
					sb.Append(ctn.generic_params[i].name);
					if (i<ctn.generic_params.Count-1)
						sb.Append(',');
				}
				if (use_lt)
				sb.Append("&gt");
				else sb.Append(">");
			}
			else if (t.is_generic_type_instance)
			{
				IGenericTypeInstance ctn = t as IGenericTypeInstance;
				sb.Append(ctn.name);
				if (use_lt)
				sb.Append("&lt");
				else sb.Append("<");
				for (int i=0; i<ctn.generic_parameters.Count; i++)
				{
					sb.Append(ctn.generic_parameters[i].name);
					if (i<ctn.generic_parameters.Count-1)
						sb.Append(',');
				}
				if (use_lt)
				sb.Append("&gt");
				else sb.Append(">");
			}
			else
				sb.Append(t.name);
			return sb.ToString();
		}

        public static string build_name_with_possible_generic(Type t, bool use_lt)
        {
            StringBuilder sb = new StringBuilder();
            if (t.IsGenericTypeDefinition)
            {
                int ind = t.Name.IndexOf('`');
                sb.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
                if (use_lt)
                    sb.Append("&lt");
                else
                    sb.Append("<");
                for (int i = 0; i < t.GetGenericArguments().Length; i++)
                {
                    sb.Append(t.GetGenericArguments()[i].Name);
                    if (i < t.GetGenericArguments().Length - 1)
                        sb.Append(',');
                }
                if (use_lt)
                    sb.Append("&gt");
                else sb.Append(">");
            }
            else if (t.IsGenericType)
            {
                sb.Append(t.Name);
                if (use_lt)
                    sb.Append("&lt");
                else sb.Append("<");
                for (int i = 0; i < t.GetGenericArguments().Length; i++)
                {
                    sb.Append(t.GetGenericArguments()[i].Name);
                    if (i < t.GetGenericArguments().Length - 1)
                        sb.Append(',');
                }
                if (use_lt)
                    sb.Append("&gt");
                else sb.Append(">");
            }
            else
                sb.Append(t.Name);
            return sb.ToString();
        }

		public static string get_type_html_text(ITypeNode t)
		{
			if (builder.parser.InTree(t))
			{
				if (!t.name.Contains("$"))
				{
					if (t is ICommonTypeNode && is_pascal_type(t))
						return get_pascal_type(t,true);
					else
						return "<a href=\""+HelpUtils.get_type_name(t)+".html"+"\">"+build_name_with_possible_generic(t,true)+"</a>";
				}
				else
				return get_pascal_type(t,true);
			}
			else if (t is ICompiledTypeNode)
				return get_compiled_type_html_text((t as ICompiledTypeNode).compiled_type);
			else
			if (t is IShortStringTypeNode)
				return get_short_string(t,true);
			else
				return get_span_for_type(build_name_with_possible_generic(t,true));
		}

        public static string get_type_html_text(Type t)
        {
            if (t.FullName == null)
                return t.Name;
            return get_compiled_type_html_text(t);
        }

		public static string get_const_value(IConstantNode c)
		{
			if (c is IIntConstantNode)
				return (c as IIntConstantNode).constant_value.ToString();
			else
				if (c is IDoubleConstantNode)
				return (c as IDoubleConstantNode).constant_value.ToString();
			else if (c is IFloatConstantNode)
				return (c as IFloatConstantNode).constant_value.ToString();
			else
				if (c is IStringConstantNode)
				return "'"+(c as IStringConstantNode).constant_value.ToString()+"'";
			else
				if (c is ICharConstantNode)
				return "'"+(c as ICharConstantNode).constant_value.ToString()+"'";
			else
				if (c is IByteConstantNode)
				return (c as IByteConstantNode).constant_value.ToString();
			else
				if (c is ISByteConstantNode)
				return (c as ISByteConstantNode).constant_value.ToString();
			else
				if (c is IShortConstantNode)
				return (c as IShortConstantNode).constant_value.ToString();
			else
				if (c is IUShortConstantNode)
				return (c as IUShortConstantNode).constant_value.ToString();
			else
				if (c is IUIntConstantNode)
				return (c as IUIntConstantNode).constant_value.ToString();
			else
				if (c is ILongConstantNode)
				return (c as ILongConstantNode).constant_value.ToString();
			else
				if (c is IULongConstantNode)
				return (c as IULongConstantNode).constant_value.ToString();
			else
				if (c is IEnumConstNode)
				return (c as IEnumConstNode).constant_value.ToString();
			else
			if (c is IArrayConstantNode)
			{
				StringBuilder sb = new StringBuilder();
				IArrayConstantNode arr = c as IArrayConstantNode;
				sb.Append('(');
				for (int i=0; i<arr.ElementValues.Length; i++)
				{
					sb.Append(get_const_value(arr.ElementValues[i]));
					if (i<arr.ElementValues.Length-1)
						sb.Append(", ");
				}
				sb.Append(')');
				return sb.ToString();
			}
			else
			if (c is IRecordConstantNode && c.type is ICommonTypeNode)
			{
				StringBuilder sb = new StringBuilder();
				IRecordConstantNode rec = c as IRecordConstantNode;
				sb.Append('(');
				ICommonTypeNode ctn = c.type as ICommonTypeNode;
				ICommonClassFieldNode[] fields = builder.parser.GetDefinedFields(ctn);
				for (int i=0; i<rec.FieldValues.Length; i++)
				{
					if (i<fields.Length)
						sb.Append(fields[i].name+": ");
					sb.Append(get_const_value(rec.FieldValues[i]));
					if (i<rec.FieldValues.Length-1)
						sb.Append(", ");
				}
				sb.Append(')');
				return sb.ToString();
			}
			return "value";
		}
		
		public static string get_var_header(ICommonNamespaceVariableNode v)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<div>");
			sb.Append(get_span_for_keyword("var")+" "+get_span_for_identifier(v.name)+": "+get_type_html_text(v.type));
			sb.Append("</div>");
			return sb.ToString();
		}
		
		public static string get_const_header(INamespaceConstantDefinitionNode c)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<div>");
			sb.Append(get_span_for_keyword("const")+" "+get_span_for_identifier(c.name)+": "+get_type_html_text(c.type)+" = "+get_const_value(c.constant_value));
			sb.Append("</div>");
			return sb.ToString();
		}
		
		public static string get_span_for_keyword(string keyw)
		{
			return "<span class=\"keyword\">"+keyw+"</span>";
		}
		
		private static string get_enum(ITypeNode tt, bool use_tags)
		{
			ICommonTypeNode t = tt as ICommonTypeNode;
			IClassConstantDefinitionNode[] consts = t.constants;
			StringBuilder sb = new StringBuilder();
			sb.Append('(');
			for (int i=0; i<consts.Length; i++)
			{
				sb.Append(consts[i].name);
				if (i<consts.Length-1)
					sb.Append(", ");
			}
			sb.Append(')');
			return sb.ToString();
		}

        private static string get_enum(Type t, bool use_tags)
        {
            FieldInfo[] consts = t.GetFields();
            StringBuilder sb = new StringBuilder();
            sb.Append('(');
            for (int i = 0; i < consts.Length; i++)
            {
                sb.Append(consts[i].Name);
                if (i < consts.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(')');
            return sb.ToString();
        }

		private static string get_delegate(ICommonTypeNode t, bool use_tags)
		{
			ICommonMethodNode f = builder.parser.GetMethods(t,"Invoke")[0];
			StringBuilder sb = new StringBuilder();
			if (f.return_value_type == null)
				sb.Append(use_tags?get_span_for_keyword("procedure"):"procedure");
			else
				sb.Append(use_tags?get_span_for_keyword("function"):"function");
            if (f.parameters.Length > 0)
            {
                sb.Append('(');
                for (int i = 0; i < f.parameters.Length; i++)
                {
                    IParameterNode prm = f.parameters[i];
                    if (prm.parameter_type == parameter_type.var)
                        sb.Append((use_tags ? get_span_for_keyword("var") : "var") + " ");
                    else if (prm.is_params)
                        sb.Append((use_tags ? get_span_for_keyword("params") : "params") + " ");
                    if (use_tags)
                        sb.Append(get_span_for_param(prm.name));
                    else
                        sb.Append(prm.name);
                    sb.Append(" : ");
                    if (use_tags)
                        sb.Append(get_type_html_text(prm.type));
                    else
                        sb.Append(get_type_text(prm.type));
                    if (i < f.parameters.Length - 1)
                        sb.Append("; ");
                }
                sb.Append(')');
            }
			
			if (f.return_value_type != null && !f.is_constructor)
				sb.Append(": "+(use_tags?get_type_html_text(f.return_value_type):get_type_text(f.return_value_type)));
			return sb.ToString();
		}
		
		private static string get_set_type(ITypeNode t, bool use_tags)
		{
			if (use_tags)
				return get_span_for_keyword("set")+ " "+get_span_for_keyword("of")+" " + get_type_html_text(t.element_type);
			else return "set of "+get_type_text(t.element_type);
		}
		
		private static string get_typed_file(ITypeNode t, bool use_tags)
		{
			if (use_tags)
				return get_span_for_keyword("file") + " "+get_span_for_keyword("of")+" " + get_type_html_text(t.element_type);
			else
				return "file of "+get_type_text(t.element_type);
		}
		
		private static string get_short_string(ITypeNode t, bool use_tags)
		{
			return (use_tags?get_span_for_type("string"):"string")+ "["+ (t as IShortStringTypeNode).Length.ToString() + "]";
		}
		
		private static string get_static_array_type(ITypeNode t, bool use_tags)
		{
			ICommonTypeNode ctn = t as ICommonTypeNode;
			IClassConstantDefinitionNode low_val = builder.parser.GetConstants(ctn,"LowerIndex")[0];
			IClassConstantDefinitionNode upper_val = builder.parser.GetConstants(ctn,"UpperIndex")[0];
			StringBuilder sb = new StringBuilder();
			if (use_tags)
			sb.Append(get_span_for_keyword("array"));
			else sb.Append("array");
			sb.Append('[');
			sb.Append(get_const_value(low_val.constant_value));
			sb.Append("..");
			sb.Append(get_const_value(upper_val.constant_value));
			sb.Append(']');
			if (use_tags)
			sb.Append(" "+get_span_for_keyword("of")+" ");
			else sb.Append(" of ");
			sb.Append(get_type_html_text(t.element_type));
			return sb.ToString();
		}
		
		private static string get_dynamic_array_type(ITypeNode t, bool use_tags)
		{
			ICommonTypeNode ctn = t as ICommonTypeNode;
			StringBuilder sb = new StringBuilder();
			if (use_tags)
				sb.Append(get_span_for_keyword("array")+" "+get_span_for_keyword("of")+" ");
			else sb.Append("array of ");
			sb.Append(get_type_html_text(t.element_type));
			return sb.ToString();
		}
		
		private static string get_inherited_and_implement(ITypeNode t)
		{
			
			StringBuilder sb = new StringBuilder();
			if (t.base_type != null && string.Compare(t.base_type.name,"object",true)!=0)
			{
				sb.Append('(');
				sb.Append(get_type_html_text(t.base_type));
				for (int i=0; i<t.ImplementingInterfaces.Count; i++)
				{
					sb.Append(", ");
					sb.Append(get_type_html_text(t.ImplementingInterfaces[i]));
				}
				sb.Append(')');
			}
			else
			{
				if (t.ImplementingInterfaces.Count > 0)
				{
					sb.Append('(');
					for (int i=0; i<t.ImplementingInterfaces.Count; i++)
					{
						sb.Append(get_type_html_text(t.ImplementingInterfaces[i]));
						if (i<t.ImplementingInterfaces.Count-1)
							sb.Append(", ");
					}
					sb.Append(')');
				}
			}
			
			return sb.ToString();
		}

        private static string get_inherited_and_implement(Type t)
        {
            StringBuilder sb = new StringBuilder();
            Type[] interfaces = t.GetInterfaces();
            if (t.BaseType != null && t.BaseType != typeof(object))
            {
                sb.Append('(');
                sb.Append(get_type_html_text(t.BaseType));
                
                for (int i = 0; i < interfaces.Length; i++)
                {
                    sb.Append(", ");
                    sb.Append(get_type_html_text(interfaces[i]));
                }
                sb.Append(')');
            }
            else
            {
                if (interfaces.Length > 0)
                {
                    sb.Append('(');
                    for (int i = 0; i < interfaces.Length; i++)
                    {
                        sb.Append(get_type_html_text(interfaces[i]));
                        if (i < interfaces.Length - 1)
                            sb.Append(", ");
                    }
                    sb.Append(')');
                }
            }

            return sb.ToString();
        }

		private static string get_diap_type(ITypeNode t, bool use_tags)
		{
			ICommonTypeNode ctn = t as ICommonTypeNode;
			StringBuilder sb = new StringBuilder();
			sb.Append(get_const_value(ctn.lower_value));
			sb.Append("..");
			sb.Append(get_const_value(ctn.upper_value));
			return sb.ToString();
		}
		
		public static string get_type_header(ITypeSynonym t)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(t.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append("<div>"+hdr+"</div>");
				return sb.ToString();
			}
			sb.Append("<div>");
			sb.Append(get_span_for_keyword("type")+" "+get_span_for_identifier(t.name)+" = "+get_type_html_text(t.original_type));
			sb.Append("</div>");
			return sb.ToString();
		}
		
		public static string get_type_header(ICommonTypeNode t)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(t.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append("<div>"+hdr+"</div>");
				return sb.ToString();
			}
			sb.Append("<div>");
			sb.Append(get_span_for_keyword("type")+" "+get_span_for_identifier(HelpUtils.build_name_with_possible_generic(t,true))+" = ");
			if (t.type_special_kind == type_special_kind.set_type)
				sb.Append(get_set_type(t,true));
			else
			if (t.type_special_kind == type_special_kind.typed_file)
				sb.Append(get_typed_file(t,true));
			else
			if (t.type_special_kind == type_special_kind.binary_file)
				sb.Append(get_span_for_keyword("file"));
			else
			if (t.type_special_kind == type_special_kind.diap_type)
				sb.Append(get_diap_type(t,true));
			else if (t.type_special_kind == type_special_kind.array_wrapper)
				sb.Append(get_static_array_type(t,true));
			else
			if (t.IsInterface)
				sb.Append(get_span_for_keyword("interface"));
			else if (t.is_class)
			{
				sb.Append(get_span_for_keyword("class"));
				sb.Append(get_inherited_and_implement(t));
			}
			else if (t.IsEnum)
				sb.Append(get_enum(t,true));
			else if (t.IsDelegate)
				sb.Append(get_delegate(t,true));
			else if (t.is_value_type)
				sb.Append(get_span_for_keyword("record"));
			sb.Append("</div>");
			return sb.ToString();
		}

        public static string get_type_header(Type t)
        {
            StringBuilder sb = new StringBuilder();
            string hdr = HelpUtils.extract_user_defined_header(GetDocumentation(t));
            if (!string.IsNullOrEmpty(hdr))
            {
                sb.Append("<div>" + hdr + "</div>");
                return sb.ToString();
            }
            sb.Append("<div>");
            sb.Append(get_span_for_keyword("type") + " " + get_span_for_identifier(HelpUtils.build_name_with_possible_generic(t, true)) + " = ");
            if (t.IsInterface)
                sb.Append(get_span_for_keyword("interface"));
            else if (t.IsClass)
            {
                sb.Append(get_span_for_keyword("class"));
                sb.Append(get_inherited_and_implement(t));
            }
            else if (t.IsEnum)
                sb.Append(get_enum(t, true));
            /*else if (t.BaseType == typeof(Delegate) && t.BaseType == typeof(MulticastDelegate))
                sb.Append(get_delegate(t, true));*/
            else if (t.IsValueType)
                sb.Append(get_span_for_keyword("record"));
            sb.Append("</div>");
            return sb.ToString();
        }

		public static string get_text_for_access_level(field_access_level fal)
		{
			switch(fal)
			{
				case field_access_level.fal_public : return "public";
				case field_access_level.fal_protected : return "protected";
				case field_access_level.fal_private : return "private";
				case field_access_level.fal_internal : return "internal";
			}
			return "";
		}

        public static string get_text_for_access_level(Type t)
        {
            if (t.IsPublic)
                return "public";
            if (t.IsNotPublic)
                return "private";
            return "";
        }

        public static string get_text_for_access_level(MethodInfo t)
        {
            if (t.IsPublic)
                return "public";
            if (t.IsPrivate)
                return "private";
            if (t.IsFamily || t.IsFamilyOrAssembly)
                return "protected";
            if (t.IsAssembly || t.IsFamilyAndAssembly)
                return "internal";
            return "";
        }

        public static string get_text_for_access_level(FieldInfo t)
        {
            if (t.IsPublic)
                return "public";
            if (t.IsPrivate)
                return "private";
            if (t.IsFamily || t.IsFamilyOrAssembly)
                return "protected";
            if (t.IsAssembly || t.IsFamilyAndAssembly)
                return "internal";
            return "";
        }

        public static string get_text_for_access_level(PropertyInfo t)
        {
            MethodInfo mi = t.GetGetMethod(true);
            if (mi != null)
            {
                if (mi.IsPublic)
                    return "public";
                if (mi.IsPrivate)
                    return "private";
                if (mi.IsFamily || mi.IsFamilyOrAssembly)
                    return "protected";
                if (mi.IsAssembly || mi.IsFamilyAndAssembly)
                    return "internal";
            }
            return "";
        }

        public static string get_text_for_access_level(EventInfo t)
        {
            MethodInfo mi = t.GetAddMethod(true);
            if (mi != null)
            {
                if (mi.IsPublic)
                    return "public";
                if (mi.IsPrivate)
                    return "private";
                if (mi.IsFamily || mi.IsFamilyOrAssembly)
                    return "protected";
                if (mi.IsAssembly || mi.IsFamilyAndAssembly)
                    return "internal";
            }
            return "";
        }

		public static string get_field_header(ICommonClassFieldNode f)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(f.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append("<div>"+hdr+"</div>");
				return sb.ToString();
			}
			sb.Append("<div>");
			sb.Append(get_span_for_keyword(get_text_for_access_level(f.field_access_level))+" ");
			if (f.polymorphic_state == polymorphic_state.ps_static)
				sb.Append(get_span_for_keyword("class")+" ");
			sb.Append(get_span_for_identifier(f.name));
			sb.Append(" : "+get_type_html_text(f.type));
			sb.Append("</div>");
			return sb.ToString();
		}

        public static string get_field_header(FieldInfo f)
        {
            StringBuilder sb = new StringBuilder();
            string hdr = HelpUtils.extract_user_defined_header(GetDocumentation(f));
            if (!string.IsNullOrEmpty(hdr))
            {
                sb.Append("<div>" + hdr + "</div>");
                return sb.ToString();
            }
            sb.Append("<div>");
            sb.Append(get_span_for_keyword(get_text_for_access_level(f)) + " ");
            if (f.IsStatic)
                sb.Append(get_span_for_keyword("class") + " ");
            sb.Append(get_span_for_identifier(f.Name));
            sb.Append(" : " + get_type_html_text(f.FieldType));
            sb.Append("</div>");
            return sb.ToString();
        }

		public static string get_event_header(ICommonEventNode f)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(f.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append("<div>"+hdr+"</div>");
				return sb.ToString();
			}
			sb.Append("<div>");
			sb.Append(get_span_for_keyword(get_text_for_access_level(f.field_access_level))+" ");
			if (f.polymorphic_state == polymorphic_state.ps_static)
				sb.Append(get_span_for_keyword("class")+" ");
			sb.Append(get_span_for_keyword("event")+" ");
			sb.Append(get_span_for_identifier(f.Name));
			sb.Append(" : "+get_type_html_text(f.DelegateType));
			sb.Append("</div>");
			return sb.ToString();
		}

        public static string get_event_header(EventInfo f)
        {
            StringBuilder sb = new StringBuilder();
            string hdr = HelpUtils.extract_user_defined_header(GetDocumentation(f));
            if (!string.IsNullOrEmpty(hdr))
            {
                sb.Append("<div>" + hdr + "</div>");
                return sb.ToString();
            }
            sb.Append("<div>");
            MethodInfo add_meth = f.GetAddMethod(true);
            sb.Append(get_span_for_keyword(get_text_for_access_level(f)) + " ");
            if (add_meth.IsStatic)
                sb.Append(get_span_for_keyword("class") + " ");
            sb.Append(get_span_for_keyword("event") + " ");
            sb.Append(get_span_for_identifier(f.Name));
            sb.Append(" : " + get_type_html_text(f.EventHandlerType));
            sb.Append("</div>");
            return sb.ToString();
        }

		public static string get_const_header(IClassConstantDefinitionNode c)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(c.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append("<div>"+hdr+"</div>");
				return sb.ToString();
			}
			sb.Append("<div>");
			sb.Append(get_span_for_keyword(get_text_for_access_level(c.field_access_level))+" ");
			if (c.polymorphic_state == polymorphic_state.ps_static)
				sb.Append(get_span_for_keyword("class")+" ");
			sb.Append(get_span_for_keyword("const")+" "+get_span_for_identifier(c.name)+" : "+get_type_html_text(c.type)+" = "+get_const_value(c.constant_value));
			sb.Append("</div>");
			return sb.ToString();
		}
		
		public static string get_property_header(ICommonPropertyNode p)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(p.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append("<div>"+hdr+"</div>");
				return sb.ToString();
			}
			sb.Append("<div>");
			sb.Append(get_span_for_keyword(get_text_for_access_level(p.field_access_level))+" ");
			if (p.polymorphic_state == polymorphic_state.ps_static)
				sb.Append(get_span_for_keyword("class")+" ");
			sb.Append(get_span_for_keyword("property")+" ");
			sb.Append(get_span_for_identifier(p.name));
			sb.Append(get_property_parameters(p.parameters));
			sb.Append(" : "+get_type_html_text(p.property_type));
			if (p.get_function != null)
				sb.Append(" read");
			if (p.set_function != null)
				sb.Append(" write");
			if (p.polymorphic_state == polymorphic_state.ps_virtual)
				sb.Append("; "+get_span_for_keyword("virtual"));
			else if (p.polymorphic_state == polymorphic_state.ps_virtual_abstract)
				sb.Append("; "+get_span_for_keyword("abstract"));
			sb.Append("</div>");
			return sb.ToString();
		}

        public static string get_property_header(PropertyInfo p)
        {
            StringBuilder sb = new StringBuilder();
            string hdr = HelpUtils.extract_user_defined_header(GetDocumentation(p));
            if (!string.IsNullOrEmpty(hdr))
            {
                sb.Append("<div>" + hdr + "</div>");
                return sb.ToString();
            }
            sb.Append("<div>");
            sb.Append(get_span_for_keyword(get_text_for_access_level(p)) + " ");
            MethodInfo get_meth = p.GetGetMethod(true);
            MethodInfo set_meth = p.GetSetMethod(true);
            if (get_meth != null && get_meth.IsStatic || set_meth != null && set_meth.IsStatic)
                sb.Append(get_span_for_keyword("class") + " ");
            sb.Append(get_span_for_keyword("property") + " ");
            sb.Append(get_span_for_identifier(p.Name));
            sb.Append(get_property_parameters(p.GetIndexParameters()));
            sb.Append(" : " + get_type_html_text(p.PropertyType));
            if (get_meth != null && AssemblyHelpBuilder.is_not_private(get_meth))
                sb.Append(" read");
            if (set_meth != null && AssemblyHelpBuilder.is_not_private(set_meth))
                sb.Append(" write");
            if (get_meth != null && get_meth.IsVirtual)
                sb.Append("; " + get_span_for_keyword("virtual"));
            else if (get_meth != null && get_meth.IsAbstract)
                sb.Append("; " + get_span_for_keyword("abstract"));
            sb.Append("</div>");
            return sb.ToString();
        }

		public static string get_func_header(ICommonNamespaceFunctionNode f)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(f.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append("<div>"+hdr+"</div>");
				return sb.ToString();
			}
			sb.Append("<div>");
			if (f.return_value_type == null)
				sb.Append(get_span_for_keyword("procedure")+" ");
			else
				sb.Append(get_span_for_keyword("function")+" ");
			sb.Append(get_span_for_identifier(f.name));
			sb.Append(get_generic_string(f.generic_params));
            if (f.parameters.Length > 0)
            {
                sb.Append('(');
                for (int i = 0; i < f.parameters.Length; i++)
                {
                    IParameterNode prm = f.parameters[i];
                    if (prm.parameter_type == parameter_type.var)
                        sb.Append(get_span_for_keyword("var") + " ");
                    else if (prm.is_params)
                        sb.Append(get_span_for_keyword("params") + " ");
                    sb.Append(get_span_for_param(prm.name));
                    sb.Append(" : ");
                    sb.Append(get_type_html_text(prm.type));
                    if (i < f.parameters.Length - 1)
                        sb.Append("; ");
                }
                sb.Append(')');
            }
			if (f.return_value_type != null)
				sb.Append(": "+get_type_html_text(f.return_value_type));
			sb.Append("</div>");
			return sb.ToString();
		}
		
		public static string get_simple_meth_header(ICommonFunctionNode f)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(f.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append(hdr);
				return sb.ToString();
			}
			sb.Append(f.name);
			sb.Append(get_generic_string(f.generic_params));
            if (f.parameters.Length > 0)
            {
                sb.Append('(');
                for (int i = 0; i < f.parameters.Length; i++)
                {
                    sb.Append(get_type_text(f.parameters[i].type));
                    if (i < f.parameters.Length - 1)
                        sb.Append(", ");
                }
                sb.Append(')');
            }
			return sb.ToString();
		}

        public static string get_simple_meth_header(MethodInfo f)
        {
            StringBuilder sb = new StringBuilder();
            string hdr = HelpUtils.extract_user_defined_header(GetDocumentation(f));
            if (!string.IsNullOrEmpty(hdr))
            {
                sb.Append(hdr);
                return sb.ToString();
            }
            sb.Append(f.Name);
            sb.Append(get_generic_string(f.GetGenericArguments()));
            ParameterInfo[] parameters = f.GetParameters();
            if (parameters.Length > 0)
            {
                sb.Append('(');
                for (int i = 0; i < parameters.Length; i++)
                {
                    sb.Append(get_compiled_type_text(parameters[i].ParameterType));
                    if (i < parameters.Length - 1)
                        sb.Append(", ");
                }
                sb.Append(')');
            }
            return sb.ToString();
        }

        public static string get_simple_meth_header(ConstructorInfo f)
        {
            StringBuilder sb = new StringBuilder();
            string hdr = HelpUtils.extract_user_defined_header(GetDocumentation(f));
            if (!string.IsNullOrEmpty(hdr))
            {
                sb.Append(hdr);
                return sb.ToString();
            }
            sb.Append("Create");
            ParameterInfo[] parameters = f.GetParameters();
            if (parameters.Length > 0)
            {
                sb.Append('(');
                for (int i = 0; i < parameters.Length; i++)
                {
                    sb.Append(get_compiled_type_text(parameters[i].ParameterType));
                    if (i < parameters.Length - 1)
                        sb.Append(", ");
                }
                sb.Append(')');
            }
            return sb.ToString();
        }

		private static string get_generic_string(List<ICommonTypeNode> generics)
		{
			StringBuilder sb = new StringBuilder();
			if (generics != null && generics.Count > 0)
			{
				sb.Append("&lt");
				for (int i=0; i<generics.Count; i++)
				{
					sb.Append(generics[i].name);
					if (i<generics.Count-1)
						sb.Append(',');
				}
				sb.Append("&gt");
			}
			return sb.ToString();
		}

        private static string get_generic_string(Type[] generics)
        {
            StringBuilder sb = new StringBuilder();
            if (generics != null && generics.Length > 0)
            {
                sb.Append("&lt");
                for (int i = 0; i < generics.Length; i++)
                {
                    sb.Append(generics[i].Name);
                    if (i < generics.Length - 1)
                        sb.Append(',');
                }
                sb.Append("&gt");
            }
            return sb.ToString();
        }

		private static string get_property_parameters(IParameterNode[] prms)
		{
			StringBuilder sb = new StringBuilder();
			if (prms.Length > 0)
			{
				sb.Append('[');
				for (int i=0; i<prms.Length; i++)
				{
					IParameterNode prm = prms[i];
					if (prm.parameter_type == parameter_type.var)
						sb.Append(get_span_for_keyword("var")+" ");
					else if (prm.is_params)
						sb.Append(get_span_for_keyword("params")+" ");
					sb.Append(get_span_for_param(prm.name));
					sb.Append(" : ");
					sb.Append(get_type_html_text(prm.type));
					if (i<prms.Length-1)
					sb.Append("; ");
				}
				sb.Append(']');
			}
			return sb.ToString();
		}

        private static string get_property_parameters(ParameterInfo[] prms)
        {
            StringBuilder sb = new StringBuilder();
            if (prms.Length > 0)
            {
                sb.Append('[');
                for (int i = 0; i < prms.Length; i++)
                {
                    ParameterInfo prm = prms[i];
                    if (prm.ParameterType.IsByRef)
                        sb.Append(get_span_for_keyword("var") + " ");
                    else if (PascalABCCompiler.NetHelper.NetHelper.IsParamsParameter(prm))
                        sb.Append(get_span_for_keyword("params") + " ");
                    sb.Append(get_span_for_param(prm.Name));
                    sb.Append(" : ");
                    sb.Append(get_type_html_text(prm.ParameterType));
                    if (i < prms.Length - 1)
                        sb.Append("; ");
                }
                sb.Append(']');
            }
            return sb.ToString();
        }

		private static string get_parameters(IParameterNode[] prms)
		{
			StringBuilder sb = new StringBuilder();
			if (prms.Length > 0)
			{
				sb.Append('(');
				for (int i=0; i<prms.Length; i++)
				{
					IParameterNode prm = prms[i];
					if (prm.parameter_type == parameter_type.var)
						sb.Append(get_span_for_keyword("var")+" ");
					else if (prm.is_params)
						sb.Append(get_span_for_keyword("params")+" ");
					sb.Append(get_span_for_param(prm.name));
					sb.Append(" : ");
					sb.Append(get_type_html_text(prm.type));
					if (i<prms.Length-1)
					sb.Append("; ");
				}
				sb.Append(')');
			}
			return sb.ToString();
		}

        private static string get_parameters(ParameterInfo[] prms)
        {
            StringBuilder sb = new StringBuilder();
            if (prms.Length > 0)
            {
                sb.Append('(');
                for (int i = 0; i < prms.Length; i++)
                {
                    ParameterInfo prm = prms[i];
                    if (prm.ParameterType.IsByRef)
                        sb.Append(get_span_for_keyword("var") + " ");
                    else if (PascalABCCompiler.NetHelper.NetHelper.IsParamsParameter(prm))
                        sb.Append(get_span_for_keyword("params") + " ");
                    sb.Append(get_span_for_param(prm.Name));
                    sb.Append(" : ");
                    sb.Append(get_type_html_text(prm.ParameterType));
                    if (i < prms.Length - 1)
                        sb.Append("; ");
                }
                sb.Append(')');
            }
            return sb.ToString();
        }

		public static string get_meth_header(ICommonMethodNode f)
		{
			StringBuilder sb = new StringBuilder();
			string hdr = HelpUtils.extract_user_defined_header(f.Documentation);
			if (!string.IsNullOrEmpty(hdr))
			{
				sb.Append("<div>"+hdr+"</div>");
				return sb.ToString();
			}
			sb.Append("<div>");
			sb.Append(get_span_for_keyword(get_text_for_access_level(f.field_access_level))+" ");
			if (f.polymorphic_state == polymorphic_state.ps_static)
				sb.Append(get_span_for_keyword("class")+" ");
			if (f.is_constructor)
				sb.Append(get_span_for_keyword("constructor")+" ");
			else
			if (f.return_value_type == null)
				sb.Append(get_span_for_keyword("procedure")+" ");
			else
				sb.Append(get_span_for_keyword("function")+" ");
			sb.Append(get_span_for_identifier(f.name));
			sb.Append(get_generic_string(f.generic_params));
			sb.Append(get_parameters(f.parameters));
			if (f.return_value_type != null && !f.is_constructor)
				sb.Append(": "+get_type_html_text(f.return_value_type));
			if (f.polymorphic_state == polymorphic_state.ps_virtual)
			if (f.overrided_method == null)
				sb.Append("; "+get_span_for_keyword("virtual"));
			else
				sb.Append("; "+get_span_for_keyword("override"));
			else if (f.polymorphic_state == polymorphic_state.ps_virtual_abstract && f.common_comprehensive_type != null && !f.common_comprehensive_type.IsInterface)
				sb.Append("; "+get_span_for_keyword("abstract"));
			sb.Append("</div>");
			return sb.ToString();
		}

        public static string get_meth_header(MethodInfo f)
        {
            StringBuilder sb = new StringBuilder();
            string hdr = HelpUtils.extract_user_defined_header(GetDocumentation(f));
            if (!string.IsNullOrEmpty(hdr))
            {
                sb.Append("<div>" + hdr + "</div>");
                return sb.ToString();
            }
            sb.Append("<div>");
            sb.Append(get_span_for_keyword(get_text_for_access_level(f)) + " ");
            if (f.IsStatic)
                sb.Append(get_span_for_keyword("class") + " ");
            if (f.IsConstructor)
                sb.Append(get_span_for_keyword("constructor") + " ");
            else
                if (f.ReturnType == typeof(void))
                    sb.Append(get_span_for_keyword("procedure") + " ");
                else
                    sb.Append(get_span_for_keyword("function") + " ");
            sb.Append(get_span_for_identifier(f.Name));
            sb.Append(get_generic_string(f.GetGenericArguments()));
            sb.Append(get_parameters(f.GetParameters()));
            if (f.ReturnType != typeof(void) && !f.IsConstructor)
                sb.Append(": " + get_type_html_text(f.ReturnType));
            if (f.IsVirtual)
                if (!is_override(f))
                    sb.Append("; " + get_span_for_keyword("virtual"));
                else
                    sb.Append("; " + get_span_for_keyword("override"));
            else if (f.IsAbstract && !f.DeclaringType.IsInterface)
                sb.Append("; " + get_span_for_keyword("abstract"));
            sb.Append("</div>");
            return sb.ToString();
        }

        private static bool is_override(MethodInfo mi)
        {
            return false;
        }

		public static string get_localization(string name)
		{
			return PascalABCCompiler.StringResources.Get(HelpBuilder_VisualPascalABCPlugin.StringsPrefix+name);
		}
		
		public static string get_return_value_description(string doc)
		{
			string s = null;
			if (doc == null)
				return null;
			int ind = doc.IndexOf("<returns>");
			if (ind == -1)
				return null;
			int end_ind = doc.IndexOf("</returns>",ind);
			if (ind != -1)
			{
				if (end_ind != -1)
					s = doc.Substring(ind+9,end_ind-ind-9).Trim(' ','\n','\t');
				else
					s = doc.Substring(ind+10).Trim(' ','\n','\t');
			}
			return s;
		}
		
		public static List<NameInfo> get_parameters(string doc)
		{
			List<NameInfo> lst = new List<NameInfo>();
			if (doc == null)
				return lst;
			int ind = doc.IndexOf("<param");
			
			while (ind != -1)
			{
				ind = doc.IndexOf("\"",ind);
				if (ind == -1) 
					return lst;
				int end_ind = doc.IndexOf("\"",ind+1);
				if (end_ind == -1)
					return lst;
				string name = doc.Substring(ind+1,end_ind-ind-1).Trim(' ','\n','\t');
				ind = doc.IndexOf(">",end_ind);
				if (ind == -1)
					return lst;
				end_ind = doc.IndexOf("</param>",ind);
				if (end_ind == -1)
					return lst;
				string desc = doc.Substring(ind+1,end_ind-ind-1).Trim(' ','\n','\t');
				ind = doc.IndexOf("<param",end_ind);
				lst.Add(new NameInfo(name,desc));
			}
			return lst;
		}
		
		private static bool user_doc_disabled(string doc)
		{
			if (doc == null) return false;
			if (doc.Length >= 2 && doc[0] == '-' && doc[1] == '-')
				return true;
			return false;
		}
		
		public static string extract_user_defined_header(string doc)
		{
			if (doc == null) return null;
			if (doc.Length >= 2 && doc[0] == '-' && doc[1] != '-')
			{
				int ind = doc.IndexOf("<summary>");
				if (ind != -1)
					return doc.Substring(1,ind-1).Trim(' ','\t','\n');
				ind = doc.IndexOf('\n');
				if (ind != -1)
					return doc.Substring(1,ind-1);
				return null;
			}
			return null;
		}
		
		public static string get_summary(string doc)
		{
			if (doc == null) return Convert.ToString(((char)0xA0));
			string res = doc;
			int ind = doc.IndexOf("<summary>");
			if (ind != -1)
			{
				int end_ind = doc.IndexOf("</summary>");
				if (end_ind != -1)
					res = doc.Substring(ind+9,end_ind-ind-9).Trim(' ','\t','\n');
				else
					res = doc.Substring(ind+9).Trim(' ','\t','\n');				
			}
			else if (doc.Length >= 1 && doc[0] == '-')
			{
				ind = doc.IndexOf('\n');
				if (ind != -1 && ind+1 < doc.Length)
					res = doc.Substring(ind+1);
				else 
					res = "";
			}
			res = Regex.Replace(res,"<see(\\s)*cref(\\s)*=(\\s)*\"(?<type_name>[\\w\\.]+)\"(\\s)*/>(?<text>(.)*)</see>","<a href=\"${type_name}.html\"/>${text}</a>");
			return res;
		}
		
		public static string get_example(string doc)
		{
			if (doc == null) return null;
			string res = null;
			int ind = doc.IndexOf("<example>");
			if (ind != -1)
			{
				int end_ind = doc.IndexOf("</example>");
				if (end_ind != -1)
					res = doc.Substring(ind+9,end_ind-ind-9).Trim(' ','\t','\n');
				else
					res = doc.Substring(ind+9).Trim(' ','\t','\n');
			}
			return res;
		}
		
		public static string get_example_comment(string doc)
		{
			int ind = doc.IndexOf("<code>");
			if (ind != -1)
			{
				return doc.Substring(0,ind).Trim(' ','\n','\t');
			}
			else 
			return doc;
		}
		
		public static string get_example_code(string doc)
		{
			int ind = doc.IndexOf("<code>");
			if (ind != -1)
			{
				int end_ind = doc.IndexOf("</code>",ind);
				if (end_ind != -1)
				return doc.Substring(ind+6,end_ind-ind-6).Trim(' ','\n','\t');
				else
				return doc.Substring(ind+6).Trim(' ','\n','\t');
			}
			else 
			return null;
		}
		
		private static string get_span_for_identifier(string s)
		{
			return "<span class=\"ident\">"+s+"</span>";
		}
		
		private static string get_span_for_param(string s)
		{
			return "<span class=\"paramname\">"+s+"</span>";
		}

        public static string GetDocumentation(Type t)
        {
            return CodeCompletion.AssemblyDocCache.GetFullDocumentation(t);
        }

        public static string GetDocumentation(MethodInfo t)
        {
            return CodeCompletion.AssemblyDocCache.GetFullDocumentation(t);
        }

        public static string GetDocumentation(ConstructorInfo t)
        {
            return CodeCompletion.AssemblyDocCache.GetFullDocumentation(t);
        }

        public static string GetDocumentation(PropertyInfo t)
        {
            return CodeCompletion.AssemblyDocCache.GetFullDocumentation(t);
        }

        public static string GetDocumentation(FieldInfo t)
        {
            return CodeCompletion.AssemblyDocCache.GetFullDocumentation(t);
        }

        public static string GetDocumentation(EventInfo t)
        {
            return CodeCompletion.AssemblyDocCache.GetFullDocumentation(t);
        }
    }
}

