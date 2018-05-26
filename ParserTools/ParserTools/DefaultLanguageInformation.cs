// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PascalABCCompiler.Parsers
{
	public class DefaultLanguageInformation : ILanguageInformation
	{
		protected IParser parser;
		protected string[] type_keywords_array;
		protected string[] keywords_array;
		protected Dictionary<string, string> keywords = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		protected Hashtable ignored_keywords = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
		protected Hashtable keyword_kinds = new Hashtable(StringComparer.CurrentCultureIgnoreCase);
		
		public DefaultLanguageInformation()
		{
			
		}
		
		public DefaultLanguageInformation(IParser p)
		{
			this.parser = p;
			InitKeywords();
		}
		
		protected virtual void InitKeywords()
		{
			List<string> keys = new List<string>();
            List<string> type_keys = new List<string>();
            keywords.Add("and", "and"); keys.Add("and");
            keywords.Add("or", "or"); keys.Add("or");
            keywords.Add("xor", "xor"); keys.Add("xor");
            keywords.Add("begin", "begin"); keys.Add("begin");
            keywords.Add("end", "end"); keys.Add("end");
            keywords.Add("for", "for"); keys.Add("for");
            keywords.Add("while", "while"); keys.Add("while");
            keywords.Add("repeat", "repeat"); keys.Add("repeat");
            keywords.Add("until", "until"); keys.Add("until");
            keywords.Add("do", "do"); keys.Add("do");
            keywords.Add("to", "to"); keys.Add("to");
            keywords.Add("downto", "downto"); keys.Add("downto");
            keywords.Add("class", "class"); keys.Add("class"); type_keys.Add("class");
            keywords.Add("record", "record"); keys.Add("record"); type_keys.Add("record");
            keywords.Add("set", "set"); keys.Add("set"); type_keys.Add("set");
            keywords.Add("file", "file"); keys.Add("file"); type_keys.Add("file");
            keywords.Add("type", "type"); keys.Add("type"); keyword_kinds.Add("type", KeywordKind.Type);
            keywords.Add("array", "array"); keys.Add("array"); type_keys.Add("array");
            keywords.Add("of", "of"); keys.Add("of"); keyword_kinds.Add("of", KeywordKind.Of);
            keywords.Add("private", "private"); keys.Add("private");
            keywords.Add("protected", "protected"); keys.Add("protected");
            keywords.Add("public", "public"); keys.Add("public");
            keywords.Add("inherited", "inherited"); keys.Add("inherited"); keyword_kinds.Add("inherited", KeywordKind.Inherited);
            keywords.Add("foreach", "foreach"); keys.Add("foreach");
            keywords.Add("try", "try"); keys.Add("try");
            keywords.Add("except", "except"); keys.Add("except");
            keywords.Add("finally", "finally"); keys.Add("finally");
            keywords.Add("uses", "uses"); keys.Add("uses"); keyword_kinds.Add("uses", KeywordKind.Uses);
            keywords.Add("unit", "unit"); keys.Add("unit"); keyword_kinds.Add("unit", KeywordKind.Unit);

            keywords.Add("procedure", "procedure"); keys.Add("procedure"); keyword_kinds.Add("procedure", KeywordKind.Function); type_keys.Add("procedure");
            keywords.Add("function", "function"); keys.Add("function"); keyword_kinds.Add("function", KeywordKind.Function); type_keys.Add("function");
            keywords.Add("constructor", "constructor"); keys.Add("constructor"); keyword_kinds.Add("constructor", KeywordKind.Constructor);
            keywords.Add("destructor", "destructor"); keys.Add("destructor"); keyword_kinds.Add("destructor", KeywordKind.Destructor);
            keywords.Add("interface", "interface"); keys.Add("interface");
            keywords.Add("implementation", "implementation"); keys.Add("implementation");
            keywords.Add("initialization", "initialization"); keys.Add("initialization");
            keywords.Add("finalization", "finalization"); keys.Add("finalization");
            keywords.Add("with", "with"); keys.Add("with");
            keywords.Add("abstract","abstract"); keys.Add("abstract");
            keywords.Add("virtual", "virtual"); keys.Add("virtual");
            keywords.Add("override", "override"); keys.Add("override");
            keywords.Add("reintroduce", "reintroduce"); keys.Add("reintroduce");
            keywords.Add("sealed", "sealed"); keys.Add("sealed");
            keywords.Add("case", "case"); keys.Add("case");
            keywords.Add("var", "var"); keys.Add("var"); keyword_kinds.Add("var", KeywordKind.Var);
            keywords.Add("const", "const"); keys.Add("const"); keyword_kinds.Add("const", KeywordKind.Const);
            keywords.Add("if", "if"); keys.Add("if");
            keywords.Add("then", "then"); keys.Add("then");
            keywords.Add("else", "else"); keys.Add("else");
            keywords.Add("in", "in"); keys.Add("in");
            keywords.Add("operator", "operator"); keys.Add("operator");
            keywords.Add("raise", "raise"); keys.Add("raise"); keyword_kinds.Add("raise", KeywordKind.Raise);
            keywords.Add("program", "program"); keys.Add("program"); keyword_kinds.Add("program", KeywordKind.Program);
            keywords.Add("new", "new"); keys.Add("new"); keyword_kinds.Add("new", KeywordKind.New);
            keywords.Add("nil", "nil"); keys.Add("nil");
            keywords.Add("loop", "loop"); keys.Add("loop");
            keywords.Add("yield", "yield"); keys.Add("yield");
            keywords.Add("sequence", "sequence"); keys.Add("sequence");
            keywords.Add("extensionmethod", "extensionmethod"); keys.Add("extensionmethod");
            keywords.Add("params", "params"); keys.Add("params");
            keywords.Add("implicit", "implicit"); keys.Add("implicit");
            keywords.Add("explicit", "explicit"); keys.Add("explicit");
            keywords.Add("forward", "forward"); keys.Add("forward");
            keywords.Add("break", "break"); keys.Add("break");
            keywords.Add("continue", "continue"); keys.Add("continue");
            keywords.Add("default", "default"); keys.Add("default");
            keywords.Add("label", "label"); keys.Add("label");
            keywords.Add("property", "property"); keys.Add("property");
            keywords.Add("auto", "auto"); keys.Add("auto");
            keywords.Add("external", "external"); keys.Add("external");
            keywords.Add("lock", "lock"); keys.Add("lock");
            keywords.Add("where", "where"); keys.Add("where");
            keywords.Add("library", "library"); keys.Add("library");
            keywords.Add("div", "div"); keys.Add("div");
            keywords.Add("mod", "mod"); keys.Add("mod");
            keywords.Add("shl", "shl"); keys.Add("shl");
            keywords.Add("shr", "shr"); keys.Add("shr");
            keywords.Add("not", "not"); keys.Add("not");
            keywords.Add("as", "as"); keys.Add("as");
            keywords.Add("is", "is"); keys.Add("is");
            keywords.Add("on", "on"); keys.Add("on");
            keywords.Add("goto", "goto"); keys.Add("goto");
            keywords.Add("overload", "overload"); keys.Add("overload");
            keywords.Add("internal", "internal"); keys.Add("internal");
            keywords.Add("template", "template"); keys.Add("template");
            keywords.Add("namespace", "namespace"); keys.Add("namespace");
            keywords.Add("exit", "exit"); keys.Add("exit");
            keywords.Add("event", "event"); keys.Add("event");
            keywords.Add("match", "match"); keys.Add("match");
            //keywords.Add("typeof", "typeof"); //keys.Add("typeof");
            //keywords.Add("sizeof", "sizeof"); //keys.Add("sizeof");
            keywords_array = new string[keywords.Count + 2];
            keywords_array[0] = "typeof";
            keywords_array[1] = "sizeof";
            keywords.Values.CopyTo(keywords_array, 2);
            type_keywords_array = type_keys.ToArray();
		}

        public virtual string BodyStartBracket
        {
            get
            {
                return "begin";
            }
        }

        public virtual string BodyEndBracket
        {
            get
            {
                return "end";
            }
        }

		public virtual string SystemUnitName
		{
            get
            {
                return "PABCSystem";
            }
		}

        public string[] Keywords
        {
            get
            {
                return keywords_array;
            }
        }

        public string[] TypeKeywords
        {
            get
            {
                return type_keywords_array;
            }
        }

        public virtual string ParameterDelimiter
        {
            get
            {
                return ";";
            }
        }

        public virtual bool CaseSensitive
        {
            get
            {
                return false;
            }
        }

        public virtual bool IncludeDotNetEntities
        {
            get
            {
                return true;
            }
        }

		public virtual string GetDescription(IBaseScope scope)
		{
			switch (scope.Kind)
			{
				case ScopeKind.Type : return GetDescriptionForType(scope as ITypeScope);
				case ScopeKind.CompiledType : return GetDescriptionForCompiledType(scope as ICompiledTypeScope);
				case ScopeKind.Delegate : return GetDescriptionForDelegate(scope as IProcType);
				case ScopeKind.TypeSynonim : return GetSynonimDescription(scope as ITypeSynonimScope);
				case ScopeKind.Array : return GetDescriptionForArray(scope as IArrayScope);
				case ScopeKind.Diapason : return GetDescriptionForDiapason(scope as IDiapasonScope);
				case ScopeKind.File : return GetDescriptionForFile(scope as IFileScope);
				case ScopeKind.Pointer : return GetDescriptionForPointer(scope as IPointerScope);
				case ScopeKind.Enum : return GetDescriptionForEnum(scope as IEnumScope);
				case ScopeKind.Set : return GetDescriptionForSet(scope as ISetScope);
				case ScopeKind.ElementScope : return GetDescriptionForElementScope(scope as IElementScope);
				case ScopeKind.CompiledField : return GetDescriptionForCompiledField(scope as ICompiledFieldScope);
				case ScopeKind.CompiledProperty : return GetDescriptionForCompiledProperty(scope as ICompiledPropertyScope);
				case ScopeKind.CompiledMethod : return GetDescriptionForCompiledMethod(scope as ICompiledMethodScope);
				case ScopeKind.Namespace : return GetDescriptionForNamespace(scope as INamespaceScope);
				case ScopeKind.Procedure : return GetDescriptionForProcedure(scope as IProcScope);
				case ScopeKind.CompiledEvent : return GetDescriptionForCompiledEvent(scope as ICompiledEventScope);
				case ScopeKind.CompiledConstructor : return GetDescriptionForCompiledConstructor(scope as ICompiledConstructorScope);
				case ScopeKind.ShortString : return GetDescriptionForShortString(scope as IShortStringScope);
				//case ScopeKind.Procedure : return GetDescriptionForProcedure(scope as IProcScope);
			}
			return "";
		}
		
		public virtual string GetSimpleDescription(IBaseScope scope)
		{
			if (scope == null)
				return "";
			switch (scope.Kind)
			{
				case ScopeKind.Delegate : 
				case ScopeKind.Array :
				case ScopeKind.Diapason : 
				case ScopeKind.File : 
				case ScopeKind.Pointer :
				case ScopeKind.Enum : 
				case ScopeKind.Set : 
				case ScopeKind.ShortString : 
					if (scope is ITypeScope && (scope as ITypeScope).Aliased)
						return scope.Name;
				break;
			}
			switch (scope.Kind)
			{
				case ScopeKind.Type : return GetSimpleDescriptionForType(scope as ITypeScope);
				case ScopeKind.CompiledType : return GetSimpleDescriptionForCompiledType(scope as ICompiledTypeScope);
				case ScopeKind.Delegate : return GetDescriptionForDelegate(scope as IProcType);
				case ScopeKind.TypeSynonim : return GetSimpleSynonimDescription(scope as ITypeSynonimScope);
				case ScopeKind.Array : return GetDescriptionForArray(scope as IArrayScope);
				case ScopeKind.Diapason : return GetDescriptionForDiapason(scope as IDiapasonScope);
				case ScopeKind.File : return GetDescriptionForFile(scope as IFileScope);
				case ScopeKind.Pointer : return GetDescriptionForPointer(scope as IPointerScope);
				case ScopeKind.Enum : return GetDescriptionForEnum(scope as IEnumScope);
				case ScopeKind.Set : return GetDescriptionForSet(scope as ISetScope);
				case ScopeKind.ElementScope : return GetSimpleDescriptionForElementScope(scope as IElementScope);
				case ScopeKind.CompiledField : return GetDescriptionForCompiledField(scope as ICompiledFieldScope);
				case ScopeKind.CompiledProperty : return GetDescriptionForCompiledProperty(scope as ICompiledPropertyScope);
				case ScopeKind.CompiledMethod : return GetDescriptionForCompiledMethod(scope as ICompiledMethodScope);
				case ScopeKind.Namespace : return GetDescriptionForNamespace(scope as INamespaceScope);
				case ScopeKind.Procedure : return GetSimpleDescriptionForProcedure(scope as IProcScope);
				case ScopeKind.CompiledEvent : return GetDescriptionForCompiledEvent(scope as ICompiledEventScope);
				case ScopeKind.CompiledConstructor : return GetDescriptionForCompiledConstructor(scope as ICompiledConstructorScope);
				case ScopeKind.ShortString : return GetDescriptionForShortString(scope as IShortStringScope);
				case ScopeKind.UnitInterface : return GetDescriptionForModule(scope as IInterfaceUnitScope);
				//case ScopeKind.Procedure : return GetDescriptionForProcedure(scope as IProcScope);
			}
			return "";
		}
		
		public virtual string GetDescriptionForModule(IInterfaceUnitScope scope)
		{
			return "unit "+scope.Name;
		}
		
		public virtual string GetShortName(ICompiledTypeScope scope)
		{
			return GetShortTypeName(scope.CompiledType);
		}
		
		public virtual string GetShortName(ICompiledMethodScope scope)
		{
			return GetShortTypeName(scope.CompiledMethod);
		}
		
		public virtual string GetShortName(ICompiledConstructorScope scope)
		{
			return "Create";
		}

        public virtual string GetShortName(IProcScope scope)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(scope.Name);
            if (scope.TemplateParameters != null && scope.TemplateParameters.Length > 0)
                sb.Append("<>");
            return sb.ToString();
        }

        public virtual string GetFullTypeName(ICompiledTypeScope scope, bool no_alias=true)
		{
			return GetFullTypeName(scope.CompiledType);
		}
		
		public virtual string GetKeyword(SymbolKind kind)
		{
			switch (kind)
			{
				case SymbolKind.Class : return "class";
				case SymbolKind.Enum : return "enum";
				case SymbolKind.Struct : return "record";
				case SymbolKind.Type : return "type";
				case SymbolKind.Interface : return "interface";
				case SymbolKind.Null : return "nil";
			}
			return "";
		}
		
		public virtual string GetArrayDescription(string elementType, int rank)
		{
			if (rank == 1)
			return "array of "+elementType;
			else
			{
				StringBuilder sb = new StringBuilder();
				sb.Append('[');
				sb.Append(',',rank-1);
				sb.Append(']');
				return "array"+sb.ToString()+" of "+elementType;
			}
		}
		
		public string GetCompiledTypeTextRepresentation(Type t)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("type "+t.Name+" = ");
			if (t.IsEnum)
			{
				//sb.Append(GetEnum(t));
				sb.AppendLine(";");
				return sb.ToString();
			}
			else if (t.BaseType == typeof(MulticastDelegate))
			{
				//sb.Append(GetDelegate(t));
				sb.AppendLine(";");
				return sb.ToString();
			}
			else if (t.IsInterface)
				sb.Append("interface");
			else if (t.IsValueType)
				sb.Append("record");
			else if (t.IsClass)
				sb.Append("class");
			if (t.BaseType != null)
			{
				sb.Append('(');
				sb.Append(GetFullTypeName(t.BaseType));
			}
			Type[] intfs = t.GetInterfaces();
			if (intfs.Length > 0)
			{
				if (t.BaseType == null)
					sb.Append('(');
				for (int i=0; i<intfs.Length; i++)
				{
					if (i != 0 || t.BaseType != null)
						sb.Append(", ");
					sb.Append(GetFullTypeName(intfs[i]));
				}
			}
			return sb.ToString();
		}
		
		public virtual string GetStandardTypeByKeyword(KeywordKind keyw)
		{
			switch (keyw)
			{
				case KeywordKind.ByteType : return "byte";
				case KeywordKind.SByteType : return "shortint";
				case KeywordKind.ShortType : return "smallint";
				case KeywordKind.UShortType : return "word";
				case KeywordKind.IntType : return "integer";
				case KeywordKind.UIntType : return "longword";
				case KeywordKind.Int64Type : return "int64";
				case KeywordKind.UInt64Type : return "uint64";
				case KeywordKind.DoubleType : return "real";
				case KeywordKind.FloatType : return "single";
				case KeywordKind.CharType : return "char";
				case KeywordKind.BoolType : return "boolean";
				case KeywordKind.PointerType : return "pointer";
			}
			return null;
		}
		
		protected virtual string GetDescriptionForDelegate(IProcType t)
		{
			StringBuilder sb = new StringBuilder();
			if (t.Target.ReturnType != null)
				sb.Append("function");
			else sb.Append("procedure");
			IElementScope[] prms = t.Target.Parameters;
			if (prms.Length > 0)
			{
				sb.Append('(');
				for (int i=0; i<prms.Length; i++)
				{
					sb.Append(GetSimpleDescription(prms[i]));
					if (i < prms.Length - 1)
					{
						sb.Append("; ");
					}
				}
				sb.Append(')');
			}
			if (t.Target.ReturnType != null)
			{
				sb.Append(": "+GetSimpleDescription(t.Target.ReturnType));
			}
			return sb.ToString();
		}
		
		protected virtual string GetFullTypeName(Type ctn, bool no_alias=true)
		{
			TypeCode tc = Type.GetTypeCode(ctn);
			if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition && ctn.IsGenericParameter) 
				return ctn.Name;
            if (!ctn.IsEnum)
            {
                switch (tc)
                {
                    case TypeCode.Int32: return "integer";
                    case TypeCode.Double: return "real";
                    case TypeCode.Boolean: return "boolean";
                    case TypeCode.String: return "string";
                    case TypeCode.Char: return "char";
                    case TypeCode.Byte: return "byte";
                    case TypeCode.SByte: return "shortint";
                    case TypeCode.Int16: return "smallint";
                    case TypeCode.Int64: return "int64";
                    case TypeCode.UInt16: return "word";
                    case TypeCode.UInt32: return "longword";
                    case TypeCode.UInt64: return "uint64";
                    case TypeCode.Single: return "single";
                }
                if (ctn.IsPointer)
                    if (ctn.FullName == "System.Void*")
                        return "pointer";
                    else
                        return "^" + GetFullTypeName(ctn.GetElementType());
            }
            else 
                return ctn.FullName;
            if (!no_alias)
            {
                if (ctn.Name.Contains("Func`") || ctn.Name.Contains("Predicate`"))
                    return getLambdaRepresentation(ctn, true, new List<string>());
                else if (ctn.Name.Contains("Action`"))
                    return getLambdaRepresentation(ctn, false, new List<string>());
                else if (ctn.Name == "IEnumerable`1")
                    return "sequence of " + GetShortTypeName(ctn.GetGenericArguments()[0], false);
                else if (ctn.Name.Contains("Tuple`"))
                    return get_tuple_string(ctn);
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
			if (ctn.IsArray) return "array of "+GetFullTypeName(ctn.GetElementType());
			if (ctn == Type.GetType("System.Void*")) return "pointer";
			if (ctn.IsNested) 
				return ctn.Name;
			return ctn.FullName;
		}

        private string get_tuple_string(ITypeScope[] generic_args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < generic_args.Length; i++)
            {
                sb.Append(GetSimpleDescriptionWithoutNamespace(generic_args[i]));
                if (i < generic_args.Length - 1)
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }

        private string get_tuple_string(Type t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(");
            Type[] generic_args = t.GetGenericArguments();
            for (int i = 0; i<generic_args.Length; i++)
            {
                sb.Append(GetShortTypeName(generic_args[i], false));
                if (i < generic_args.Length - 1)
                    sb.Append(",");
            }
            sb.Append(")");
            return sb.ToString();
        }

        private string get_enum_constants(Type t)
        {
            FieldInfo[] fields = t.GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("(");
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Name != "value__")
                {
                    sb.Append(' ', 4);
                    sb.Append(fields[i].Name);
                    if (i < fields.Length - 1)
                        sb.AppendLine(",");
                    else
                        sb.AppendLine();
                }
            }
            sb.Append(");");

            return sb.ToString();
        }

        public virtual string GetCompiledTypeRepresentation(Type t, MemberInfo mi, ref int line, ref int col)
        {
            StringBuilder sb = new StringBuilder();
            int ln = 1;
            if (t.Namespace == "System")
                sb.AppendLine("unit SystemUnit;");
            else
                sb.AppendLine("unit " + t.Namespace.Replace(".", "_") + ";"); 
            ln++;
            sb.AppendLine(); ln++;
            sb.AppendLine("type "); ln++;

            if (mi == t)
            {
                line = ln;
                col = 1;
            }
            string doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(t);
            if (!string.IsNullOrEmpty(doc))
            {
                doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "  /// ");
                doc = doc.Replace("<returns>", StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                doc = doc.Replace("<params>", "");
                doc = doc.Replace("<param>", StringResources.Get("CODE_COMPLETION_PARAMETER"));
                doc = doc.Replace("</param>", "");
                sb.AppendLine("  /// " + doc);
            }
            if (mi == t)
            {
                line = get_line_nr(sb);
                col = 1;
            }
            int ind = t.Name.IndexOf('`');
            if (ind != -1)
            {
                sb.Append(prepare_member_name(t.Name.Substring(0, ind)));
                sb.Append('<');
                Type[] gen_args = t.GetGenericArguments();
                for (int i = 0; i < gen_args.Length; i++)
                {
                    sb.Append(gen_args[i].Name);
                    if (i < gen_args.Length - 1)
                        sb.Append(",");
                }
                sb.Append('>');
            }
            else
                sb.Append(prepare_member_name(t.Name));
            sb.Append(" = " + GetClassKeyword(t));
            bool bracket = false;
            if (t.IsEnum)
            {
                sb.AppendLine(get_enum_constants(t));
                sb.AppendLine();
                sb.AppendLine("end.");
                return sb.ToString();
            }
            if (t.BaseType != null && t.BaseType != typeof(object))
            {
                sb.Append("("); 
                bracket = true;
                sb.Append(GetFullTypeName(t.BaseType));
            }
            Type[] intfs = t.GetInterfaces();
            if (intfs.Length > 0)
                if (!bracket)
                {
                    sb.Append("(");
                    bracket = true;
                }
                else
                    sb.Append(", ");
            for (int i = 0; i < intfs.Length; i++)
            {
                sb.Append(GetFullTypeName(intfs[i]));
                if (i < intfs.Length - 1)
                    sb.Append(", ");
            }
            if (bracket)
                sb.Append(")");
            sb.AppendLine(); ln++;
            FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (fields.Length > 0)
            {
                sb.AppendLine("{$region " + StringResources.Get("CODE_COMPLETION_FIELDS") + "}");
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].DeclaringType == t && !fields[i].IsPrivate && !fields[i].IsAssembly)
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(fields[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "  /// ");
                            doc = doc.Replace("<returns>", StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("  /// " + doc);
                            ln++;
                        }
                        if (fields[i] == mi || fields[i].Name == mi.Name)
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("  ");
                        sb.Append(GetDescriptionForCompiledField(fields[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("{$endregion}");
                sb.AppendLine();
            }
            EventInfo[] events = t.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (events.Length > 0)
            {
                sb.AppendLine("{$region " + StringResources.Get("CODE_COMPLETION_EVENTS") + "}");
                for (int i = 0; i < events.Length; i++)
                {
                    if (events[i].DeclaringType != t)
                        continue;
                    MethodInfo add_meth = events[i].GetAddMethod(true);
                    if (add_meth != null && add_meth.DeclaringType == t && !add_meth.IsPrivate && !add_meth.IsAssembly)
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(events[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "  /// ");
                            doc = doc.Replace("<returns>", StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("  /// " + doc);
                        }

                        if (events[i] == mi || events[i].Name == mi.Name)
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("  ");
                        sb.Append(GetDescriptionForCompiledEvent(events[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("{$endregion}");
                sb.AppendLine();
            }
            PropertyInfo[] props = t.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (props.Length > 0)
            {
                sb.AppendLine("{$region " + StringResources.Get("CODE_COMPLETION_PROPERTIES") + "}");
                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].DeclaringType != t)
                        continue;
                    MethodInfo get_meth = props[i].GetGetMethod(true);
                    if (get_meth != null && get_meth.DeclaringType == t && !get_meth.IsPrivate && !get_meth.IsAssembly)
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(props[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "  /// ");
                            doc = doc.Replace("<returns>", StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("  /// " + doc);
                        }
                        if (props[i] == mi || props[i].Name == mi.Name)
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("  ");
                        sb.Append(GetDescriptionForCompiledProperty(props[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("{$endregion}");
                sb.AppendLine();
            }
            
            ConstructorInfo[] constrs = t.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (constrs.Length > 0)
            {
                sb.AppendLine("{$region " + StringResources.Get("CODE_COMPLETION_CONSTRUCTORS") + "}");
                for (int i = 0; i < constrs.Length; i++)
                {
                    if (constrs[i].DeclaringType == t && !constrs[i].IsPrivate && !constrs[i].IsAssembly)
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(constrs[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "  /// ");
                            doc = doc.Replace("<returns>", StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("  /// " + doc);
                        }
                        if (constrs[i] == mi)
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("  ");
                        sb.Append(GetDescriptionForCompiledConstructor(constrs[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("{$endregion}");
                sb.AppendLine();
            }
            MethodInfo[] meths = t.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (meths.Length > 0)
            {
                sb.AppendLine("{$region " + StringResources.Get("CODE_COMPLETION_METHODS") + "}");
                for (int i = 0; i < meths.Length; i++)
                {
                    if (meths[i].DeclaringType == t && !meths[i].IsPrivate && !meths[i].IsAssembly && !meths[i].Name.StartsWith("get_") && !meths[i].Name.StartsWith("set_") && !meths[i].Name.StartsWith("add_") && !meths[i].Name.StartsWith("remove_"))
                    {
                        doc = CodeCompletionTools.AssemblyDocCache.GetDocumentation(meths[i]);
                        if (!string.IsNullOrEmpty(doc))
                        {
                            doc = doc.Trim(' ', '\n', '\t', '\r').Replace(Environment.NewLine, Environment.NewLine + "  /// ");
                            doc = doc.Replace("<returns>", StringResources.Get("CODE_COMPLETION_RETURN_VALUE"));
                            doc = doc.Replace("<params>", "");
                            doc = doc.Replace("<param>", StringResources.Get("CODE_COMPLETION_PARAMETER"));
                            doc = doc.Replace("</param>", "");
                            sb.AppendLine("  /// " + doc);
                        }
                        if (meths[i] == mi || (meths[i].Name == mi.Name))
                        {
                            line = get_line_nr(sb);
                            col = 3;
                        }
                        sb.Append("  ");
                        sb.Append(GetDescriptionForCompiledMethod(meths[i]));
                        sb.AppendLine(); ln++;
                        sb.AppendLine(); ln++;
                    }
                }
                sb.AppendLine("{$endregion}");
                sb.AppendLine();
            }
            
            sb.AppendLine("end;");
            sb.AppendLine();
            sb.AppendLine("end.");
            return sb.ToString();
        }

        private int get_line_nr(StringBuilder sb)
        {
            int line = 1;
            for (int i = 0; i < sb.Length; i++)
                if (sb[i] == '\n')
                    line++;
            return line;
        }

        public virtual string GetCompiledMethodRepresentation(MethodInfo mi)
        {
            return null;
        }

        public virtual string GetCompiledFieldRepresentation(FieldInfo mi)
        {
            return null;
        }

        public virtual string GetCompiledPropertyRepresentation(PropertyInfo mi)
        {
            return null;
        }

        public virtual string GetCompiledEventRepresentation(EventInfo mi)
        {
            return null;
        }

        public virtual string GetCompiledConstructorRepresentation(ConstructorInfo mi)
        {
            return null;
        }

        public virtual string GetClassKeyword(Type t)
        {
            if (t.IsInterface)
                return "interface";
            if (t.IsEnum)
                return "";
            if (t.IsValueType)
                return "record";
            if (t.IsClass)
                return "class";
            return "";
        }

		public virtual string GetClassKeyword(PascalABCCompiler.SyntaxTree.class_keyword keyw)
		{
			switch(keyw)
			{
				case PascalABCCompiler.SyntaxTree.class_keyword.Class : return "class";
				case PascalABCCompiler.SyntaxTree.class_keyword.Interface : return "interface";
				case PascalABCCompiler.SyntaxTree.class_keyword.Record : return "record";
				case PascalABCCompiler.SyntaxTree.class_keyword.TemplateClass : return "template class";
				case PascalABCCompiler.SyntaxTree.class_keyword.TemplateRecord : return "template record";
				case PascalABCCompiler.SyntaxTree.class_keyword.TemplateInterface : return "template interface";
			}
			return null;
		}
		
		protected virtual string GetShortTypeName(MethodInfo mi)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(mi.Name);
			if (mi.GetGenericArguments().Length > 0)
				sb.Append("<>");
			return sb.ToString();
		}
		
		public virtual string GetShortTypeName(Type ctn, bool noalias=true)
		{
			TypeCode tc = Type.GetTypeCode(ctn);
			if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition && ctn.IsGenericParameter) 
				return ctn.Name;
            if (!ctn.IsEnum)
            {
                switch (tc)
                {
                    case TypeCode.Int32: return "integer";
                    case TypeCode.Double: return "real";
                    case TypeCode.Boolean: return "boolean";
                    case TypeCode.String: return "string";
                    case TypeCode.Char: return "char";
                    case TypeCode.Byte: return "byte";
                    case TypeCode.SByte: return "shortint";
                    case TypeCode.Int16: return "smallint";
                    case TypeCode.Int64: return "int64";
                    case TypeCode.UInt16: return "word";
                    case TypeCode.UInt32: return "longword";
                    case TypeCode.UInt64: return "uint64";
                    case TypeCode.Single: return "single";
                }
                if (ctn.IsPointer)
                    if (ctn.FullName == "System.Void*")
                        return "pointer";
                    else
                        return "^" + GetShortTypeName(ctn.GetElementType(), noalias);
            }
            else return ctn.Name;
			if (ctn.Name.Contains("`"))
			{
                if (!noalias)
                {
                    if (ctn.Name.Contains("Func`") || ctn.Name.Contains("Predicate`"))
                        return getLambdaRepresentation(ctn, true, new List<string>());
                    else if (ctn.Name.Contains("Action`"))
                        return getLambdaRepresentation(ctn, false, new List<string>());
                    else if (ctn.Name == "IEnumerable`1")
                        return "sequence of " + GetShortTypeName(ctn.GetGenericArguments()[0], false);
                    else if (ctn.Name.Contains("Tuple`"))
                        return get_tuple_string(ctn);
                }
				int len = ctn.GetGenericArguments().Length;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(ctn.Name.Substring(0,ctn.Name.IndexOf('`')));
				sb.Append('<');
                if (!noalias)
                {
                    Type[] gen_ps = ctn.GetGenericArguments();
                    for (int i = 0; i < len; i++)
                    {
                        sb.Append(gen_ps[i].Name);
                        if (i < len - 1)
                            sb.Append(',');
                    }
                }
				sb.Append('>');
				/*sb.Append('<');
				for (int i=0; i<len; i++)
				{
					sb.Append('T');
					if (i<len-1)
					sb.Append(',');
				}
				sb.Append('>');*/
				return sb.ToString();
			}
			if (ctn.IsArray) return "array of "+GetShortTypeName(ctn.GetElementType());
			//if (ctn == Type.GetType("System.Void*")) return PascalABCCompiler.TreeConverter.compiler_string_consts.pointer_type_name;
			return ctn.Name;
		}
		
		protected virtual string GetTopScopeName(IBaseScope sc)
		{
			if (sc == null) return "";
			if (sc.Name == "" || sc.Name.Contains("$") || sc.Name == "PABCSystem") return "";
			if (sc is IProcScope) return "";
			if (sc is ITypeScope)
			{
				return sc.Name+(((sc as ITypeScope).TemplateArguments != null && !sc.Name.EndsWith("<>"))?"<>":"")+".";
			}
			return sc.Name + ".";
		}
		
		private string GetTopScopeNameWithoutDot(IBaseScope sc)
		{
			if (sc == null) return "";
			if (sc.Name == "" || sc.Name.Contains("$") || sc.Name == "PABCSystem") return "";
			if (sc is IProcScope) return "";
			if (sc is ITypeScope)
			{
				return sc.Name+(((sc as ITypeScope).TemplateArguments != null && !sc.Name.EndsWith("<>"))? "<>":"");
			}
			return sc.Name;
		}
		
		protected virtual string GetTemplateString(ITypeScope scope)
		{
			string[] generic_params = scope.TemplateArguments;
			if (generic_params != null && generic_params.Length > 0)
			{
				ITypeScope[] gen_insts = scope.GenericInstances;
				if (gen_insts == null || gen_insts.Length == 0)
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					sb.Append('<');
					for (int i=0; i<generic_params.Length; i++)
					{
						sb.Append(generic_params[i]);
						if (i < generic_params.Length -1)
						sb.Append(',');
					}
					sb.Append('>');
					return sb.ToString();
				}
				else
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					sb.Append('<');
					for (int i=0; i<gen_insts.Length; i++)
					{
						sb.Append(GetSimpleDescriptionWithoutNamespace(gen_insts[i]));
						if (i < gen_insts.Length -1)
						sb.Append(',');
					}
					sb.Append('>');
					return sb.ToString();
				}
			}
			return null;
		}
		
		protected virtual string GetDescriptionForType(ITypeScope scope)
		{
			string template_str=GetTemplateString(scope);
			switch(scope.ElemKind)
			{
				case SymbolKind.Class : 
					if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
						return (scope.IsAbstract ? "abstract " : "") + (scope.IsFinal?"sealed ":"")+"class "+scope.TopScope.Name + "." +scope.Name+template_str;
					else return (scope.IsAbstract ? "abstract " : "") + (scope.IsFinal?"sealed ":"")+"class "+scope.Name+template_str;
				case SymbolKind.Interface :
					if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
					return "interface "+scope.TopScope.Name + "." +scope.Name+template_str;
					else return "interface "+scope.Name+template_str;
				case SymbolKind.Enum :
					if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
					return "enum "+scope.TopScope.Name + "." +scope.Name;
					else return "enum "+scope.Name;
				case SymbolKind.Delegate :
					if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
					return "delegate "+scope.TopScope.Name + "." +scope.Name+template_str;
					else return "delegate "+scope.Name+template_str;
				case SymbolKind.Struct :
					if (scope.TopScope != null && scope.TopScope.Name != "" && !scope.TopScope.Name.Contains("$"))
					return "record "+scope.TopScope.Name + "." +scope.Name+template_str;
					else return "record "+scope.Name+template_str;
			}
			
			if (scope.TopScope != null)
			return scope.TopScope.Name + "." +scope.Name;
			else return scope.Name;
		}
		
		protected virtual string GetSimpleDescriptionForType(ITypeScope scope)
		{
			string template_str=GetTemplateString(scope);
			if (scope.Name.StartsWith("$"))
				return scope.Name.Substring(1,scope.Name.Length-1)+template_str;
			return scope.Name+template_str;
		}
		
		protected virtual string GetDescriptionForCompiledType(ICompiledTypeScope scope)
		{
			string s = GetFullTypeName(scope.CompiledType);
			ITypeScope[] instances = scope.GenericInstances;
			if (instances.Length > 0)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				int ind = s.IndexOf('<');
				if (ind != -1) sb.Append(s.Substring(0,ind));
				else
				sb.Append(s);
				sb.Append('<');
				for (int i=0; i<instances.Length; i++)
				{
					sb.Append(GetSimpleDescriptionWithoutNamespace(instances[i]));
					//sb.Append(instances[i].Name);
					if (i < instances.Length - 1) sb.Append(',');
				}
				sb.Append('>');
				s = sb.ToString();
			}
			
			switch(scope.ElemKind)
			{
				case SymbolKind.Class : 					
					return (scope.IsAbstract ? "abstract " : "")+(scope.IsFinal?"sealed ":"")+"class "+s;
				case SymbolKind.Interface :
					return "interface "+s;
				case SymbolKind.Enum :
					return "enum "+s;
				case SymbolKind.Delegate :
					return "delegate "+s;
				case SymbolKind.Struct :
					return "record "+s;
				case SymbolKind.Type :
					return "type "+s;
			}
			return s;
		}
		
		
		
		public virtual string GetSimpleDescriptionWithoutNamespace(ITypeScope scope)
		{
			ICompiledTypeScope cts = scope as ICompiledTypeScope;
			if (cts == null)
				return GetSimpleDescription(scope);
			string s = GetShortName(cts);
			ITypeScope[] instances = scope.GenericInstances;
			if (instances != null && instances.Length > 0)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				int ind = s.IndexOf('<');
				if (ind != -1) sb.Append(s.Substring(0,ind));
				else
				sb.Append(s);
				sb.Append('<');
				for (int i=0; i<instances.Length; i++)
				{
					sb.Append(GetSimpleDescriptionWithoutNamespace(instances[i]));
					if (i < instances.Length - 1) sb.Append(',');
				}
				sb.Append('>');
				s = sb.ToString();
			}
			return s;
		}

        protected bool isIdentifier(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }

        protected string getLambdaRepresentation(Type t, bool has_return_value, List<string> parameters, Dictionary<string, string> generic_param_args=null)
        {
            StringBuilder sb = new StringBuilder();
            if (parameters.Count != t.GetGenericArguments().Length || true)
            {
                List<string> old_parameters = parameters;
                parameters = new List<string>();
                int ind = 0;
                foreach (Type generic_arg in t.GetGenericArguments())
                {
                    if (generic_arg.IsGenericParameter)
                    {
                        if (generic_param_args != null)
                        {
                            if (generic_param_args.ContainsKey(generic_arg.Name))
                                parameters.Add(generic_param_args[generic_arg.Name]);
                            else
                                parameters.Add(generic_arg.Name);
                        }
                        else if (ind < old_parameters.Count)
                        {
                            parameters.Add(old_parameters[ind]);
                            ind++;
                        }
                        else
                            parameters.Add(generic_arg.Name);
                    }
                    else
                        //parameters.Add(GetShortTypeName(generic_arg, false));
                        parameters.Add(get_type_instance(generic_arg,parameters,generic_param_args));
                }
            }
            foreach (string parameter in parameters)
            {
                if (!isIdentifier(parameter))
                    return get_type_instance(t, parameters, generic_param_args, true);
            }
            if (has_return_value)
            {
                if (parameters.Count > 2)
                    sb.Append("(" + string.Join(",", parameters.GetRange(0, parameters.Count - 1).ToArray()) + ")->" + parameters[parameters.Count - 1]);
                else if (parameters.Count > 1)
                    sb.Append(parameters[0] + "->" + parameters[1]);
                else if (parameters.Count == 1)
                {
                    if (t.FullName == "System.Predicate`1" || t.Name == "Predicate`1")
                        sb.Append(parameters[0] + "->boolean");
                    else
                        sb.Append("()->" + parameters[0]);
                }

            }
            else if (parameters.Count > 0)
            {
                if (t.FullName == "System.Predicate`1" || t.Name == "Predicate`1")
                    sb.Append(parameters[0] + "->boolean");
                else if (parameters.Count > 1)
                    sb.Append("(" + string.Join(",", parameters.ToArray()) + ")->()");
                else
                    sb.Append(parameters[0] + "->()");
            }
            if (sb.Length == 0)
                sb.Append(t.Name);
            return sb.ToString();
        }

        protected string getLambdaRepresentation(ICompiledTypeScope scope, bool has_return_value)
        {
            StringBuilder sb = new StringBuilder();
            ITypeScope[] instances = scope.GenericInstances;
            List<string> parameters = new List<string>();
            if (instances != null && instances.Length > 0)
            {
                foreach (ITypeScope ts in instances)
                    parameters.Add(GetSimpleDescriptionWithoutNamespace(ts));
            }
            /*else
            {
                foreach (Type t in scope.CompiledType.GetGenericArguments())
                    parameters.Add(t.Name);
            }*/
            return getLambdaRepresentation(scope.CompiledType, has_return_value, parameters);
        }

        protected string GetSimpleDescriptionForCompiledType(ICompiledTypeScope scope, bool fullName)
        {
            if (scope.CompiledType.Name != null && scope.CompiledType.Name.Contains("Func`"))
            {
                return getLambdaRepresentation(scope, true);
            }
            else if (scope.CompiledType.Name != null && scope.CompiledType.Name.Contains("Action`"))
            {
                return getLambdaRepresentation(scope, false);
            }
            else if (scope.CompiledType.Name != null && scope.CompiledType.Name.Contains("Predicate`1"))
            {
                return getLambdaRepresentation(scope, false);
            }
            else if (scope.CompiledType.Name == "IEnumerable`1")
            {
                ITypeScope[] instances = scope.GenericInstances;
                if (instances != null && instances.Length > 0)
                {
                    return "sequence of " + GetSimpleDescriptionWithoutNamespace(instances[0]);
                }
                else
                    return "sequence of T";
            }
            else if (scope.CompiledType.Name != null && scope.CompiledType.Name.Contains("Tuple`"))
            {
                ITypeScope[] instances = scope.GenericInstances;
                if (instances != null && instances.Length > 0)
                {
                    return get_tuple_string(instances);
                }
                else
                    return "(T1,...)";
            }
            else
            {
                string s = !fullName?GetShortTypeName(scope.CompiledType):GetFullTypeName(scope.CompiledType);
                ITypeScope[] instances = scope.GenericInstances;
                if (instances != null && instances.Length > 0)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    int ind = s.IndexOf('<');
                    if (ind != -1) sb.Append(s.Substring(0, ind));
                    else
                        sb.Append(s);
                    sb.Append('<');
                    for (int i = 0; i < instances.Length; i++)
                    {
                        sb.Append(GetSimpleDescriptionWithoutNamespace(instances[i]));
                        if (i < instances.Length - 1) sb.Append(',');
                    }
                    sb.Append('>');
                    s = sb.ToString();
                }
                return s;
            }
        }

        protected virtual string GetSimpleDescriptionForCompiledType(ICompiledTypeScope scope)
		{
            return GetSimpleDescriptionForCompiledType(scope, false);
		}
		
		protected virtual string GetDescriptionForArray(IArrayScope scope)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("array");
			ITypeScope[] inds = scope.Indexers;
			if (!scope.IsDynamic)
			{
				sb.Append('[');
				for (int i=0; i<inds.Length; i++)
				{
					sb.Append(GetSimpleDescription(inds[i]));
					if (i<inds.Length-1) sb.Append(',');
				}
				sb.Append(']');
			}
			if (scope.ElementType != null)
			{
				string s = GetSimpleDescription(scope.ElementType);
				if (s.Length > 0 && s[0] == '$') s = s.Substring(1,s.Length-1);
				sb.Append(" of "+s);
			}
			return sb.ToString();
		}
		
		protected virtual string GetDescriptionForDiapason(IDiapasonScope scope)
		{
			return scope.Left.ToString() + ".." + scope.Right.ToString();
		}
		
		protected virtual string GetDescriptionForFile(IFileScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("file");
			if (scope.ElementType != null)
			{
				string s = GetSimpleDescription(scope.ElementType);
				if (s.Length > 0 && s[0] == '$') s = s.Substring(1,s.Length-1);
				sb.Append(" of "+s);
			}
			return sb.ToString();
		}
		
		private Stack<ITypeScope> typs = new Stack<ITypeScope>();
		
		protected virtual string GetDescriptionForPointer(IPointerScope scope)
		{
			string s = "";
			if (scope.ElementType != null)
			{
				if (!typs.Contains(scope))
				{
					typs.Push(scope);
					s = "^"+GetSimpleDescription(scope.ElementType);
				}
				else 
				s = "";
			}
			else
			s = "pointer";
			if (typs.Count > 0)
				typs.Pop();
			return s;
		}
		
		protected virtual string GetDescriptionForEnum(IEnumScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append('(');
			for (int i=0; i<scope.EnumConsts.Length; i++)
			{
				sb.Append(scope.EnumConsts[i]);
				if (i < scope.EnumConsts.Length-1)
				sb.Append(',');
				if (i >= 2) 
				{
					sb.Append("...");
					break;
				}
			}
			sb.Append(')');
			return sb.ToString();
		}
		
		protected virtual string GetDescriptionForSet(ISetScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("set of ");
			if (scope.ElementType != null)
			{
				string s = GetSimpleDescription(scope.ElementType);
				if (s.Length > 0 && s[0] == '$') s = s.Substring(1,s.Length-1);
				sb.Append(s);
			}
			return sb.ToString();
		}
		
		private string kind_of_param(IElementScope scope)
		{
			switch (scope.ParamKind)
			{
				case PascalABCCompiler.SyntaxTree.parametr_kind.const_parametr : return "const ";
				case PascalABCCompiler.SyntaxTree.parametr_kind.var_parametr : return "var ";
				case PascalABCCompiler.SyntaxTree.parametr_kind.params_parametr : return "params ";
				case PascalABCCompiler.SyntaxTree.parametr_kind.out_parametr : return "out ";
			}
			return "";
		}
		
		protected virtual string get_index_description(IElementScope scope)
		{
			ITypeScope[] indexers = scope.Indexers;
			if (indexers == null || indexers.Length == 0) return "";
			StringBuilder sb = new StringBuilder();
			sb.Append('[');
			for (int i=0; i<indexers.Length; i++)
			{
				sb.Append(indexers[i].Name);
				if (i <indexers.Length-1)
					sb.Append(',');
			}
			sb.Append(']');
			return sb.ToString();
		}
		
		private void append_modifiers(StringBuilder sb, IElementScope scope)
		{
			if (scope.IsVirtual) sb.Append("; virtual");
			if (scope.IsAbstract) sb.Append("; abstract");
			if (scope.IsOverride) sb.Append("; override");
			//if (scope.IsStatic) sb.Append("; static");
			if (scope.IsReintroduce) sb.Append("; reintroduce");
		}

        protected virtual string GetDescriptionForElementScope(IElementScope scope)
        {
            string type_name = null;
            StringBuilder sb = new StringBuilder();
            if (scope.Type == null) type_name = "";
            else
                type_name = GetSimpleDescription(scope.Type);
            if (type_name.StartsWith("$"))
                type_name = type_name.Substring(1, type_name.Length - 1);
            switch (scope.ElemKind)
            {
                case SymbolKind.Variable: sb.Append("var " + GetTopScopeName(scope.TopScope) + scope.Name + ": " + type_name); break;
                case SymbolKind.Parameter: sb.Append(kind_of_param(scope) + "parameter " + scope.Name + ": " + type_name + (scope.ConstantValue != null ? (":=" + scope.ConstantValue.ToString()) : "")); break;
                case SymbolKind.Constant:
                    {
                        if (scope.ConstantValue == null)
                            sb.Append("const " + GetTopScopeName(scope.TopScope) + scope.Name + ": " + type_name);
                        else sb.Append("const " + GetTopScopeName(scope.TopScope) + scope.Name + ": " + type_name + " = " + scope.ConstantValue.ToString());
                    }
                    break;
                case SymbolKind.Event:
                    if (scope.IsStatic) sb.Append("class ");
                    sb.Append("event " + GetTopScopeName(scope.TopScope) + scope.Name + ": " + type_name);
                    append_modifiers(sb, scope);
                    break;
                case SymbolKind.Field:
                    if (scope.IsStatic)
                        sb.Append("class ");
                    else
                        sb.Append("var ");
                    sb.Append(GetTopScopeName(scope.TopScope) + scope.Name + ": " + type_name);
                    append_modifiers(sb, scope);
                    //if (scope.IsStatic) sb.Append("; static");
                    if (scope.IsReadOnly) sb.Append("; readonly");
                    break;
                case SymbolKind.Property:
                    if (scope.IsStatic)
                        sb.Append("class ");
                    sb.Append("property " + GetTopScopeName(scope.TopScope) + scope.Name + get_index_description(scope) + ": " + type_name);
                    if (scope.IsReadOnly)
                        sb.Append("; readonly");
                    append_modifiers(sb, scope);
                    break;

            }
            sb.Append(';');
            return sb.ToString();
        }
		
		protected virtual string GetSimpleDescriptionForElementScope(IElementScope scope)
		{
			string type_name = GetSimpleDescription(scope.Type);
			if (type_name.StartsWith("$")) type_name = type_name.Substring(1,type_name.Length-1);
			return kind_of_param(scope) + scope.Name + ": "+type_name + (scope.ConstantValue!=null?(":="+scope.ConstantValue.ToString()):"");
		}

        public virtual string GetDescriptionForCompiledField(FieldInfo fi)
        {
            StringBuilder sb = new StringBuilder();
            if (fi.IsPublic)
                sb.Append("public ");
            else if (fi.IsFamily)
                sb.Append("protected ");
            if (!fi.IsLiteral)
                if (fi.IsStatic) sb.Append("class ");
            if (!fi.IsLiteral)
            {
                sb.Append(prepare_member_name(fi.Name));
                sb.Append(" : " + GetFullTypeName(fi.FieldType));
            }
            else
            {
                sb.Append("const " + fi.Name + " : " + GetFullTypeName(fi.FieldType));
                sb.Append(" = " + fi.GetRawConstantValue().ToString());
            }
            sb.Append(";");
            return sb.ToString();
        }

		protected virtual string GetDescriptionForCompiledField(ICompiledFieldScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (!scope.CompiledField.IsLiteral)
			if (scope.CompiledField.IsStatic && !scope.IsGlobal) sb.Append("class ");
			else sb.Append("var ");
			string inst_type = null;
			if (scope.GenericArgs != null)
			{
				inst_type = get_type_instance(scope.CompiledField.FieldType,scope.GenericArgs);
			}
			if (!scope.CompiledField.IsLiteral)
				sb.Append(GetShortTypeName(scope.CompiledField.DeclaringType) +"."+ scope.CompiledField.Name + ": "+(inst_type != null?inst_type:GetSimpleDescription(scope.Type)));
			else
				sb.Append("const "+GetShortTypeName(scope.CompiledField.DeclaringType) +"."+ scope.CompiledField.Name + ": "+GetSimpleDescription(scope.Type));
			//if (scope.CompiledField.IsStatic) sb.Append("; static");
			if (scope.IsReadOnly) sb.Append("; readonly");
			sb.Append(';');
			return sb.ToString();
		}
		
		private string get_indexer_for_prop(ICompiledPropertyScope scope)
		{
			ITypeScope[] indexers = scope.Indexers;
			if (indexers.Length == 0) return "";
			StringBuilder sb = new StringBuilder();
			sb.Append('[');
			for (int i=0; i<indexers.Length; i++)
			{
				sb.Append(GetSimpleDescriptionWithoutNamespace(indexers[i]));
				if (i<indexers.Length-1)
					sb.Append(',');
			}
			sb.Append(']');
			return sb.ToString();
		}

        public virtual string GetDescriptionForCompiledProperty(PropertyInfo pi)
        {
            StringBuilder sb = new StringBuilder();
            MethodInfo get_meth = pi.GetGetMethod(true);
            MethodInfo set_meth = pi.GetSetMethod(true);
            if (get_meth.IsPublic)
                sb.Append("public ");
            else if (get_meth.IsFamily)
                sb.Append("protected ");
            if (get_meth.IsStatic) sb.Append("class ");
            sb.Append("property " + prepare_member_name(pi.Name));
            ParameterInfo[] prms = get_meth.GetParameters();
            if (prms.Length > 0)
            {
                sb.Append('[');
                for (int i = 0; i < prms.Length; i++)
                {
                    sb.Append(prepare_member_name(prms[i].Name));
                    sb.Append(": ");
                    sb.Append(GetFullTypeName(prms[i].ParameterType));
                    if (i < prms.Length - 1)
                        sb.Append("; ");
                }
                sb.Append(']');
            }
            sb.Append(" : " + GetFullTypeName(pi.PropertyType));
            if (set_meth == null)
                sb.Append("; readonly");
            sb.Append(";");
            return sb.ToString();
        }

		protected virtual string GetDescriptionForCompiledProperty(ICompiledPropertyScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			MethodInfo acc = scope.CompiledProperty.GetGetMethod();
			string inst_type = null;
			if (acc != null)
			if (acc.IsStatic) sb.Append("class ");
			if (scope.Type is ICompiledTypeScope && scope.GenericArgs != null)
			{
				Type t = (scope.Type as ICompiledTypeScope).CompiledType;
				inst_type = get_type_instance(t,scope.GenericArgs);
			}
			sb.Append("property "+ GetShortTypeName(scope.CompiledProperty.DeclaringType) +"."+ scope.CompiledProperty.Name + get_indexer_for_prop(scope)+ ": "+(inst_type != null?inst_type:GetSimpleDescription(scope.Type)));
			if (acc != null)
			//if (acc.IsStatic) sb.Append("; static");
			if (acc.IsVirtual) sb.Append("; virtual");
			else if (acc.IsAbstract) sb.Append("; abstract");
			if (scope.IsReadOnly) sb.Append("; readonly");
			sb.Append(';');
			return sb.ToString();
		}
		
		private bool is_params(ParameterInfo _par)
		{
			object[] objarr = _par.GetCustomAttributes(typeof(ParamArrayAttribute), true);
            if ((objarr == null) || (objarr.Length == 0))
            {
                return false;
            }
            return true;
		}

        private string get_type_instance(Type t, List<string> generic_args, Dictionary<string, string> generic_param_args=null, bool no_alias=false)
        {
            if (t.IsGenericParameter)
            {
                if (generic_param_args != null)
                {
                    if (generic_param_args.ContainsKey(t.Name))
                        return generic_param_args[t.Name];
                    else
                        return t.Name;
                }
                else if (t.GenericParameterPosition < generic_args.Count)
                    return generic_args[t.GenericParameterPosition];
                else
                    return t.Name;
            }
            if (t.IsArray)
                return "array of " + get_type_instance(t.GetElementType(), generic_args, generic_param_args, no_alias);
            if (t.ContainsGenericParameters)
            {
                if (!no_alias)
                {
                    if (t.Name == "IEnumerable`1")
                    {
                        Type tt = t.GetGenericArguments()[0];
                        if (generic_param_args != null)
                        {
                            if (tt.IsGenericParameter && generic_param_args.ContainsKey(tt.Name))
                                return "sequence of " + generic_param_args[tt.Name];
                            else
                                return "sequence of " + GetShortTypeName(tt, false);
                        }
                        else 
                            return "sequence of " + (generic_args.Count>0?generic_args[0]:GetShortTypeName(tt, false));
                    }
                    else if (t.Name.Contains("Func`") || t.Name.Contains("Predicate`"))
                        return getLambdaRepresentation(t, true, generic_args, generic_param_args);
                    else if (t.Name.Contains("Action`") )
                        return getLambdaRepresentation(t, false, generic_args, generic_param_args);
                    else if (t.Name.Contains("Tuple`"))
                        return get_tuple_string(t);
                }
                string name = GetShortTypeName(t);
                StringBuilder sb = new StringBuilder();
                int ind = name.IndexOf('<');
                if (ind == -1)
                    return name;
                sb.Append(name.Substring(0, ind));
                Type[] args = t.GetGenericArguments();
                sb.Append('<');
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].IsGenericParameter)
                    {
                        if (generic_param_args != null)
                        {
                            if (generic_param_args.ContainsKey(args[i].Name))
                                sb.Append(generic_param_args[args[i].Name]);
                            else
                                sb.Append(args[i].Name);
                        }
                        else if (args[i].GenericParameterPosition < generic_args.Count)
                            sb.Append(generic_args[args[i].GenericParameterPosition]);
                        else
                            sb.Append(args[i].Name);
                    }
                    else
                        sb.Append(get_type_instance(args[i], generic_args, generic_param_args));
                    if (i < args.Length - 1)
                        sb.Append(',');
                }
                sb.Append('>');
                return sb.ToString();
            }
            return GetFullTypeName(t, no_alias);
        }

        public virtual string GetDescriptionForCompiledMethod(MethodInfo mi)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (mi.IsPublic)
                sb.Append("public ");
            else if (mi.IsFamily)
                sb.Append("protected ");
            if (mi.IsStatic) sb.Append("class ");
            if (mi.ReturnType == typeof(void))
                sb.Append("procedure ");
            else
                sb.Append("function ");
            sb.Append(prepare_member_name(mi.Name));
            if (mi.GetGenericArguments().Length > 0)
            {
                Type[] tt = mi.GetGenericArguments();
                sb.Append('<');
                for (int i = 0; i < tt.Length; i++)
                {
                    sb.Append(tt[i].Name);
                    if (i < tt.Length - 1) sb.Append(',');
                }
                sb.Append('>');
            }
            sb.Append('(');
            ParameterInfo[] pis = mi.GetParameters();
            for (int i = 0; i < pis.Length; i++)
            {
                if (pis[i].ParameterType.IsByRef)
                    sb.Append("var ");
                else if (is_params(pis[i]))
                    sb.Append("params ");
                sb.Append(prepare_member_name(pis[i].Name));
                sb.Append(": ");
                string inst_type = null;
                if (!pis[i].ParameterType.IsByRef)
                {
                    sb.Append(GetFullTypeName(pis[i].ParameterType));
                    if (pis[i].IsOptional)
                        sb.Append(":=" + (pis[i].DefaultValue != null ? pis[i].DefaultValue.ToString() : "nil"));
                }
                else
                {
                    Type t = pis[i].ParameterType.GetElementType();
                    sb.Append(GetFullTypeName(t));
                }
                
                if (i < pis.Length - 1)
                    sb.Append("; ");
            }
            sb.Append(')');
            string ret_inst_type = null;
            if (mi.ReturnType != typeof(void))
            {
                sb.Append(": " + GetFullTypeName(mi.ReturnType));
            }
            //if (scope.CompiledMethod.IsStatic) sb.Append("; static");
            if (mi.IsVirtual && !mi.IsFinal) sb.Append("; virtual");
            else if (mi.IsAbstract) sb.Append("; abstract");
            //else if (scope.CompiledMethod.IsHideBySig) sb.Append("; reintroduce");
            sb.Append(';');
            return sb.ToString();
        }

        protected virtual string GetDescriptionForCompiledMethod(ICompiledMethodScope scope)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string extensionType = null;
            if (scope.IsExtension && scope.Parameters.Length > 0)
            {
                extensionType = GetSimpleDescription(scope.Parameters[0].Type);
            }
            if (scope.IsExtension)
            {
                if (extensionType.IndexOf(' ') != -1)
                {
                    sb.Append("(" + StringResources.Get("CODE_COMPLETION_EXTENSION") + " " + extensionType + ") ");
                }
                else
                {
                    sb.Append("(" + StringResources.Get("CODE_COMPLETION_EXTENSION") + ") ");
                    extensionType = null;
                }
            }
                
            if (scope.IsStatic && !scope.IsGlobal) sb.Append("class ");
            if (scope.ReturnType == null)
                sb.Append("procedure ");
            else
                sb.Append("function ");
            Dictionary<string, string> generic_param_args = null;
            Dictionary<string, int> class_generic_table = new Dictionary<string, int>();
            ParameterInfo[] pis = scope.CompiledMethod.GetParameters();
            Type[] tt = scope.CompiledMethod.GetGenericArguments();
            int gen_ind = 0;
            if (!scope.IsExtension)
            {
                sb.Append(GetShortTypeName(scope.CompiledMethod.DeclaringType));
                int ind = 0;
                foreach (Type gen_arg in scope.CompiledMethod.DeclaringType.GetGenericArguments())
                {
                    if (gen_arg.IsGenericParameter)
                    {
                        if (generic_param_args == null)
                            generic_param_args = new Dictionary<string, string>();
                        if (scope.GenericArgs != null && scope.GenericArgs.Count > ind)
                            generic_param_args.Add(gen_arg.Name, scope.GenericArgs[ind]);
                        else if (scope.DeclaringType.TemplateArguments != null && scope.DeclaringType.TemplateArguments.Length > ind)
                            generic_param_args.Add(gen_arg.Name, scope.DeclaringType.TemplateArguments[ind]);
                    }
                    ind++;
                }
                    
            }
            else
            {
                gen_ind = 0;
                generic_param_args = new Dictionary<string, string>();
                for (int i = 0; i < pis.Length; i++)
                {
                    if (i == 0)
                    {
                        Type[] class_generic_args = pis[i].ParameterType.GetGenericArguments();
                        for (int j = 0; j < class_generic_args.Length; j++)
                        {
                            if (!class_generic_table.ContainsKey(class_generic_args[i].Name))
                                class_generic_table.Add(class_generic_args[i].Name, j);
                            if (scope.GenericArgs != null && scope.GenericArgs.Count > j)
                            {
                                if (scope.DeclaringType.GenericInstances.Length > 0)
                                    generic_param_args[class_generic_args[i].Name] = GetSimpleDescription(scope.DeclaringType.GenericInstances[0]);
                                else if (scope.DeclaringType is ICompiledTypeScope)
                                {
                                    Type ctn = (scope.DeclaringType as ICompiledTypeScope).CompiledType;
                                    if (ctn.IsGenericType && !ctn.IsGenericTypeDefinition)
                                        generic_param_args[class_generic_args[i].Name] = GetSimpleDescriptionForCompiledType((scope.DeclaringType as ICompiledTypeScope).GetCompiledGenericArguments()[0]);
                                }
                            }
                            else if (scope.DeclaringType.TemplateArguments != null && scope.DeclaringType.TemplateArguments.Length > j)
                                generic_param_args[class_generic_args[i].Name] = scope.DeclaringType.TemplateArguments[j];
                        }
                        break;
                    }
                }
                if (extensionType == null)
                    sb.Append(GetShortTypeName(scope.CompiledMethod.GetParameters()[0].ParameterType));
            }
            //if (scope.Name != "Invoke")
            {
                if (extensionType == null)
                    sb.Append(".");
                sb.Append(scope.Name);
            }

            if (scope.CompiledMethod.GetGenericArguments().Length > 0)
            {
                sb.Append('<');
                for (int i = gen_ind; i < tt.Length; i++)
                {
                    if (class_generic_table.ContainsKey(tt[i].Name))
                    {
                        int ind = class_generic_table[tt[i].Name];
                        if (scope.GenericArgs != null && scope.GenericArgs.Count > ind)
                        {
                            if (scope.DeclaringType.GenericInstances.Length > ind)
                            {
                                sb.Append(GetSimpleDescription(scope.DeclaringType.GenericInstances[ind]));
                                if (!generic_param_args.ContainsKey(tt[i].Name))
                                    generic_param_args.Add(tt[i].Name, GetSimpleDescription(scope.DeclaringType.GenericInstances[ind]));
                            }
                            else if (scope.DeclaringType is ICompiledTypeScope)
                            {
                                Type ctn = (scope.DeclaringType as ICompiledTypeScope).CompiledType;
                                if (ctn.IsGenericType && !ctn.IsGenericTypeDefinition)
                                {
                                    sb.Append(GetSimpleDescription((scope.DeclaringType as ICompiledTypeScope).GetCompiledGenericArguments()[ind]));
                                    if (!generic_param_args.ContainsKey(tt[i].Name))
                                        generic_param_args.Add(tt[i].Name, GetSimpleDescription((scope.DeclaringType as ICompiledTypeScope).GetCompiledGenericArguments()[ind]));
                                }
                            }
                        }
                    }
                    
                    else
                        sb.Append(tt[i].Name);
                    if (i < tt.Length - 1) sb.Append(',');
                }
                sb.Append('>');
            }
            sb.Append('(');
            
            for (int i = 0; i < pis.Length; i++)
            {
                if (i == 0 && scope.IsExtension)
                    continue;
                if (pis[i].ParameterType.IsByRef)
                    sb.Append("var ");
                else if (is_params(pis[i]))
                    sb.Append("params ");
                sb.Append(pis[i].Name);
                sb.Append(": ");
                string inst_type = null;
                if (!pis[i].ParameterType.IsByRef)
                {
                    if (scope.GenericArgs != null)
                    {
                        inst_type = get_type_instance(pis[i].ParameterType, scope.GenericArgs, generic_param_args);
                    }
                    if (inst_type == null)
                        sb.Append(GetShortTypeName(pis[i].ParameterType, false));
                    else
                        sb.Append(inst_type);
                    if (pis[i].IsOptional)
                        sb.Append(":=" + (pis[i].DefaultValue != null ? pis[i].DefaultValue.ToString() : "nil"));
                }
                else
                {
                    Type t = pis[i].ParameterType.GetElementType();
                    if (scope.GenericArgs != null)
                    {
                        inst_type = get_type_instance(t, scope.GenericArgs, generic_param_args);
                    }
                    if (inst_type == null)
                        sb.Append(GetShortTypeName(t, false));
                    else
                        sb.Append(inst_type);
                }
                if (i < pis.Length - 1)
                    sb.Append("; ");
            }
            sb.Append(')');
            string ret_inst_type = null;
            if (scope.ReturnType != null)
            {
                if (scope.GenericArgs != null)
                {
                    ret_inst_type = get_type_instance(scope.CompiledMethod.ReturnType, scope.GenericArgs, generic_param_args);
                }
                if (ret_inst_type == null)
                {
                    if (scope.ReturnType is ICompiledTypeScope)
                        sb.Append(": " + GetFullTypeName((scope.ReturnType as ICompiledTypeScope).CompiledType, false));
                    else
                        sb.Append(": " + GetSimpleDescription(scope.ReturnType));
                }
                else
                    sb.Append(": " + ret_inst_type);
            }
            //if (scope.CompiledMethod.IsStatic) sb.Append("; static");
            if (scope.IsVirtual) sb.Append("; virtual");
            else if (scope.IsAbstract) sb.Append("; abstract");
            //else if (scope.CompiledMethod.IsHideBySig) sb.Append("; reintroduce");
            sb.Append(';');
            return sb.ToString();
        }
		
		protected virtual string GetDescriptionForNamespace(INamespaceScope scope)
		{
			return "namespace "+scope.Name;
		}
		
		protected virtual string GetDescriptionForProcedure(IProcScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string extensionType = null;
            if (scope.IsExtension && scope.Parameters.Length > 0)
            {
                extensionType = GetSimpleDescription(scope.Parameters[0].Type);
            }
            if (scope.IsExtension)
            {
                if (extensionType != null && extensionType.IndexOf(' ') != -1)
                {
                    sb.Append("(" + StringResources.Get("CODE_COMPLETION_EXTENSION") + " " + extensionType + ") ");
                    extensionType = null;
                }
                else
                {
                    sb.Append("(" + StringResources.Get("CODE_COMPLETION_EXTENSION")+ ") ");
                }   
            }
              
			if (scope.IsStatic) sb.Append("class ");
			if (scope.IsConstructor())
				sb.Append("constructor ");
			else
			if (scope.ReturnType == null)
				sb.Append("procedure ");
			else
				sb.Append("function ");
			if (!scope.IsConstructor())
			{
                if (extensionType != null)
                {
                    sb.Append(extensionType+".");
                    sb.Append(scope.Name);
                }
                else
                {
                    sb.Append(GetTopScopeName(scope.TopScope));
                    sb.Append(scope.Name);
                }
			}
			else
			{
				sb.Append(GetTopScopeNameWithoutDot(scope.TopScope));
			}
			/*string[] template_args = scope.TemplateParameters;
			if (template_args != null)
			{
				sb.Append('<');
				for (int i=0; i<template_args.Length; i++)
				{
					sb.Append(template_args[i]);
					if (i < template_args.Length-1)
						sb.Append(',');
				}
				sb.Append('>');
			}*/
			sb.Append(GetGenericString(scope.TemplateParameters));
			sb.Append('(');
			IElementScope[] parameters = scope.Parameters;
			for (int i=0; i<parameters.Length; i++)
			{
                if (scope.IsExtension && i == 0)
                    continue;
				sb.Append(GetSimpleDescription(parameters[i]));
				if (i < parameters.Length - 1)
				{
					sb.Append("; ");
				}
			}
			sb.Append(')');
			if (scope.ReturnType != null && !scope.IsConstructor())
				sb.Append(": "+GetSimpleDescription(scope.ReturnType));
			//if (scope.IsStatic) sb.Append("; static");
			if (scope.IsVirtual) sb.Append("; virtual");
			else if (scope.IsAbstract) sb.Append("; abstract");
			else if (scope.IsOverride) sb.Append("; override");
			else if (scope.IsReintroduce) sb.Append("; reintroduce");
			sb.Append(';');
			return sb.ToString();
		}
		
		protected virtual string GetGenericString(string[] template_args)
		{
			StringBuilder sb = new StringBuilder();
			if (template_args != null)
			{
				sb.Append('<');
				for (int i=0; i<template_args.Length; i++)
				{
					sb.Append(template_args[i]);
					if (i < template_args.Length-1)
						sb.Append(',');
				}
				sb.Append('>');
			}
			return sb.ToString();
		}
		
		protected virtual string GetSimpleDescriptionForProcedure(IProcScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (scope.TopScope is ITypeScope && scope.Realization == null)
				sb.Append(GetTopScopeName(scope.TopScope));
			sb.Append(scope.Name); 
			sb.Append(GetGenericString(scope.TemplateParameters));
			sb.Append('(');
			IElementScope[] parameters = scope.Parameters;
			for (int i=0; i<parameters.Length; i++)
			{
				sb.Append(GetSimpleDescription(parameters[i]));
				if (i < parameters.Length - 1)
				{
					sb.Append("; ");
				}
			}
			sb.Append(')');
			return sb.ToString();
		}

        private string prepare_member_name(string s)
        {
            if (keywords.ContainsKey(s))
                return "&" + s;
            return s;
        }

        protected virtual string GetDescriptionForCompiledEvent(EventInfo ei)
        {
            MethodInfo add_meth = ei.GetAddMethod(true);
            StringBuilder sb = new StringBuilder();
            if (add_meth.IsPublic)
                sb.Append("public ");
            else if (add_meth.IsFamily)
                sb.Append("protected ");
            sb.Append((add_meth.IsStatic ? "class " : "") + "event " + prepare_member_name(ei.Name) + ": " + GetFullTypeName(ei.EventHandlerType) + ";");
            return sb.ToString();
        }

		protected virtual string GetDescriptionForCompiledEvent(ICompiledEventScope scope)
		{
			return (scope.IsStatic?"class ":"")+"event "+ GetShortTypeName(scope.CompiledEvent.DeclaringType, true) +"."+ scope.CompiledEvent.Name + ": "+GetSimpleDescription(scope.Type)+ ";";
		}

        protected virtual string GetDescriptionForCompiledConstructor(ConstructorInfo ci)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (ci.IsPublic)
                sb.Append("public ");
            else if (ci.IsFamily)
                sb.Append("protected ");
            if (ci.IsStatic)
                sb.Append("class ");
            sb.Append("constructor ");
            //sb.Append(".");
            //sb.Append("Create");
            sb.Append('(');
            ParameterInfo[] pis = ci.GetParameters();
            for (int i = 0; i < pis.Length; i++)
            {
                if (pis[i].ParameterType.IsByRef)
                    sb.Append("var ");
                else if (is_params(pis[i]))
                    sb.Append("params ");
                sb.Append(prepare_member_name(pis[i].Name));
                sb.Append(": ");
                if (!pis[i].ParameterType.IsByRef)
                    sb.Append(GetFullTypeName(pis[i].ParameterType));
                else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
                if (i < pis.Length - 1)
                    sb.Append("; ");
            }
            sb.Append(')');
            sb.Append(';');
            return sb.ToString();
        }

		protected virtual string GetDescriptionForCompiledConstructor(ICompiledConstructorScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("constructor ");
			sb.Append(GetShortTypeName(scope.CompiledConstructor.DeclaringType));
			//sb.Append(".");
			//sb.Append("Create");
			sb.Append('(');
			ParameterInfo[] pis = scope.CompiledConstructor.GetParameters();
			for (int i=0; i<pis.Length; i++)
			{
				if (pis[i].ParameterType.IsByRef)
					sb.Append("var ");
				else if (is_params(pis[i]))
					sb.Append("params ");
				sb.Append(pis[i].Name);
				sb.Append(": ");
				if (!pis[i].ParameterType.IsByRef)
				sb.Append(GetFullTypeName(pis[i].ParameterType));
				else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
				if (i < pis.Length - 1)
				sb.Append("; ");
			}
			sb.Append(')');
			sb.Append(';');
			return sb.ToString();
		}
		
		protected virtual string GetDescriptionForShortString(IShortStringScope scope)
		{
			return "string"+"["+scope.Length+"]";
		}
		
		public string GetSynonimDescription(ITypeScope scope)
		{
			return "type "+scope.Name + GetGenericString(scope.TemplateArguments)+" = " +scope.Description;
		}
		
		public string GetSynonimDescription(ITypeSynonimScope scope)
		{
            if (scope.ActType is ICompiledTypeScope && !(scope.ActType as ICompiledTypeScope).Aliased)
                return "type " + scope.Name + GetGenericString(scope.TemplateArguments) + " = " + GetSimpleDescriptionForCompiledType(scope.ActType as ICompiledTypeScope, true);
            else
			    return "type " + scope.Name+GetGenericString(scope.TemplateArguments) + " = " + GetSimpleDescription(scope.ActType);
		}
		
		public string GetSynonimDescription(IProcScope scope)
		{
			return "type "+scope.Name+" = "+scope.Description;
		}
		
		public virtual string[] GetIndexerString(IBaseScope scope)
		{
			IBaseScope tmp_si = scope;
        	if (scope == null) return null;
        	if (scope is IElementScope) 
        		if ((scope as IElementScope).Indexers.Length == 0)
        		scope = (scope as IElementScope).Type;
        	if (scope is IProcScope) scope = (scope as IProcScope).ReturnType;
            if (!(scope is IElementScope))
            {
                ITypeScope ts = scope as ITypeScope;
                if (ts == null) return null;
                if (tmp_si is ITypeScope) return null;
                ITypeScope[] indexers = ts.Indexers;
                if ((indexers == null || indexers.Length == 0) && !(ts is IArrayScope))
                    return null;
                StringBuilder sb = new StringBuilder();
                sb.Append("this");
                sb.Append('[');
                if (indexers != null)
                    for (int i = 0; i < indexers.Length; i++)
                    {
                        sb.Append(GetSimpleDescriptionWithoutNamespace(indexers[i]));
                        if (i < indexers.Length - 1)
                            sb.Append(',');
                    }
                else
                    sb.Append("integer");
        		sb.Append("] : ");
        		sb.Append(GetSimpleDescriptionWithoutNamespace(ts.ElementType));
        		return new string[1]{sb.ToString()};
        	}
        	else
        	{
        		IElementScope es = scope as IElementScope;
        		ITypeScope[] indexers = es.Indexers;
        		if (indexers == null || indexers.Length == 0 || es.ElementType == null) return null;
        		StringBuilder sb = new StringBuilder();
        		sb.Append(es.Name);
        		sb.Append('[');
        		for (int i=0; i<indexers.Length; i++)
        		{
        			sb.Append(GetSimpleDescriptionWithoutNamespace(indexers[i]));
        			if (i < indexers.Length - 1)
        				sb.Append(',');
        		}
        		sb.Append("] : ");
        		sb.Append(GetSimpleDescriptionWithoutNamespace(es.ElementType));
        		return new string[1]{sb.ToString()};
        	}
		}
		
		protected virtual string GetSimpleSynonimDescription(ITypeSynonimScope scope)
		{
			return scope.Name;
		}
		
		public virtual string GetStringForChar(char c)
		{
			return "'"+c.ToString()+"'";
		}
		
		public virtual string GetStringForSharpChar(int num)
		{
			return "#"+num.ToString();
		}
		
		public virtual string GetStringForString(string s)
		{
			return "'"+s+"'";
		}
		
		public KeywordKind GetKeywordKind(string name)
        {
            object o = keyword_kinds[name];
            if (o != null) return (KeywordKind)o;
            else return KeywordKind.None;
        }

       
        
        protected virtual string GetAccessModifier(access_modifer mod)
        {
        	switch (mod)
        	{
        		case access_modifer.private_modifer : return "private";
        		case access_modifer.protected_modifer : return "protected";
        		case access_modifer.public_modifer : return "public";
        		case access_modifer.published_modifer : return "published";
        		case access_modifer.internal_modifer : return "internal";
        	}
        	return "";
        }
        
        public virtual string ConstructOverridedMethodHeader(IProcScope scope, out int off)
        {
        	System.Text.StringBuilder sb = new System.Text.StringBuilder();
        	if (scope.AccessModifier != access_modifer.internal_modifer)
        	sb.Append(GetAccessModifier(scope.AccessModifier)+" ");
        	if (scope.ReturnType == null)
				sb.Append("procedure ");
			else
				sb.Append("function ");
			off = sb.Length;
			sb.Append(scope.Name); 
			sb.Append(GetGenericString(scope.TemplateParameters));
			if (!(scope is ICompiledMethodScope))
			{
				IElementScope[] parameters = scope.Parameters;
				if (parameters != null && parameters.Length > 0)
				{
					sb.Append('(');
				for (int i=0; i<scope.Parameters.Length; i++)
				{
					sb.Append(GetSimpleDescription(parameters[i]));
					if (i < parameters.Length - 1)
					{
						sb.Append("; ");
					}
				}
				sb.Append(')');
				}
			}
			else
			{
				ParameterInfo[] pis = (scope as ICompiledMethodScope).CompiledMethod.GetParameters();
                if (pis.Length > 0)
                {
                    sb.Append('(');
                    for (int i = 0; i < pis.Length; i++)
                    {
                        if (pis[i].ParameterType.IsByRef)
                            sb.Append("var ");
                        else if (is_params(pis[i]))
                            sb.Append("params ");
                        sb.Append(pis[i].Name);
                        sb.Append(": ");
                        if (!pis[i].ParameterType.IsByRef)
                            sb.Append(GetFullTypeName(pis[i].ParameterType));
                        else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
                        if (i < pis.Length - 1)
                            sb.Append("; ");
                    }
                    sb.Append(')');
                }
			}
			if (scope.ReturnType != null)
				sb.Append(": "+GetSimpleDescription(scope.ReturnType));
			sb.Append("; override;");
			return sb.ToString();
        }
        
        public virtual string ConstructHeader(IProcRealizationScope scope, int tabCount)
        {
        	System.Text.StringBuilder sb = new System.Text.StringBuilder();
        	sb.Append('\t');
        	if (!scope.DefProc.IsAbstract)
        	sb.Append(GetAccessModifier(access_modifer.public_modifer)+" ");
        	else
        	if (scope.DefProc.AccessModifier != access_modifer.none && scope.DefProc.AccessModifier != access_modifer.internal_modifer)
        		sb.Append(GetAccessModifier(scope.DefProc.AccessModifier)+" ");
        	if (scope.DefProc.ReturnType == null)
				sb.Append("procedure ");
			else
				sb.Append("function ");
			sb.Append(scope.Name); 
			sb.Append(GetGenericString(scope.TemplateParameters));
			
			if (!(scope.DefProc is ICompiledMethodScope))
			{
				IElementScope[] parameters = scope.DefProc.Parameters;
                if (parameters != null && parameters.Length > 0)
                {
                    sb.Append('(');
                    for (int i = 0; i < scope.DefProc.Parameters.Length; i++)
                    {
                        sb.Append(GetSimpleDescription(parameters[i]));
                        if (i < parameters.Length - 1)
                        {
                            sb.Append("; ");
                        }
                    }
                    sb.Append(')');
                }
			}
			else
			{
				ParameterInfo[] pis = (scope.DefProc as ICompiledMethodScope).CompiledMethod.GetParameters();
				if (pis.Length > 0)
				{
					sb.Append('(');
				for (int i=0; i<pis.Length; i++)
				{
				if (pis[i].ParameterType.IsByRef)
					sb.Append("var ");
				else if (is_params(pis[i]))
					sb.Append("params ");
				sb.Append(pis[i].Name);
				sb.Append(": ");
				if (!pis[i].ParameterType.IsByRef)
				sb.Append(GetFullTypeName(pis[i].ParameterType));
				else sb.Append(GetFullTypeName(pis[i].ParameterType.GetElementType()));
				if (i < pis.Length - 1)
				sb.Append("; ");
				}
				sb.Append(')');
				}
			}
			
			if (scope.DefProc.ReturnType != null)
				sb.Append(": "+GetSimpleDescription(scope.DefProc.ReturnType));
			sb.Append(';');
			if (scope.DefProc.IsAbstract)
				sb.Append("override;");
			return sb.ToString();
        }
        
        private void get_procedure_template(procedure_header header, StringBuilder res, int col)
        {
        	if (header.parameters != null)
        		for (int i=0; i<header.parameters.params_list.Count; i++)
        		for (int j=0; j<header.parameters.params_list[i].idents.idents.Count; j++)
        		{
        			res.AppendLine();
        			for (int k=0;k<col-3; k++)
						res.Append(' ');
        			res.Append("/// <param name=\""+header.parameters.params_list[i].idents.idents[j].name+"\"></param>");
        		}
        		if (header is function_header)
        		{
        			res.AppendLine();
        			for (int k=0;k<col-3; k++)
						res.Append(' ');
        			res.Append("/// <returns></returns>");
        		}
        }
        
        public virtual string GetDocumentTemplate(string lineText, string Text, int line, int col, int pos)
        {
            try
            {
                if (lineText == null) return "";
                StringBuilder res = new StringBuilder();
                if (lineText.StartsWith("procedure") || lineText.StartsWith("function") || lineText.StartsWith("constructor") || lineText.StartsWith("destructor"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(lineText);
                    sb.AppendLine("begin end;");
                    sb.AppendLine("begin end.");
                    program_module node = this.parser.BuildTree("test.pas", sb.ToString(), ParseMode.Special) as program_module;
                    procedure_header header = (node.program_block.defs.defs[0] as procedure_definition).proc_header;
                    get_procedure_template(header, res, col);
                    return res.ToString();
                }
                else if (lineText.StartsWith("class") || lineText.StartsWith("public") || lineText.StartsWith("private") || lineText.StartsWith("protected") || lineText.StartsWith("internal"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(lineText);
                    sb.AppendLine("begin end;");
                    sb.AppendLine("begin end.");
                    program_module node = this.parser.BuildTree("test.pas", sb.ToString(), ParseMode.Special) as program_module;
                    if (node.program_block.defs != null && node.program_block.defs.defs.Count > 0 && node.program_block.defs.defs[0] is procedure_definition)
                    {
                        procedure_header header = (node.program_block.defs.defs[0] as procedure_definition).proc_header;
                        get_procedure_template(header, res, col);
                        return res.ToString();
                    }
                    return "";
                }
                else return "";
            }
            catch
            {

            }
        	return "";
        }
        
		public virtual string ConstructHeader(string meth, IProcScope scope, int tabCount)
		{
			int i=0;
        	bool is_cnstr = false;
        	StringBuilder sb = new StringBuilder();
            if (scope.IsStatic)
                sb.Append("class ");
        	while (i < meth.Length && char.IsLetterOrDigit(meth[i]))
        	{
        		sb.Append(meth[i++]);
        	}
        	if (sb.ToString().ToLower() == "constructor") is_cnstr = true;
        	sb.Append(' ');
        	while (i < meth.Length && !(char.IsLetterOrDigit(meth[i]) || meth[i] == '_' || meth[i] == '(' || meth[i] == ';'))
        	if (meth[i] == '{')
        		while (i<meth.Length && meth[i] != '}') i++;
        	else
        		i++;
        	if (i < meth.Length)
        	{
        		if (scope.TopScope is ITypeScope && ((scope.TopScope as ITypeScope).ElemKind == SymbolKind.Class || (scope.TopScope as ITypeScope).ElemKind == SymbolKind.Struct))
        			sb.Append(GetSimpleDescriptionForType(scope.TopScope as ITypeScope)+".");
        			//if (meth[i] == '(' || meth[i] == ';')
        		sb.Append(scope.Name);
        		sb.Append(GetGenericString(scope.TemplateParameters));
                
        		while (i < meth.Length && (char.IsLetterOrDigit(meth[i]) || meth[i] == '_')) i++;
        		while (i < meth.Length && meth[i] != ';' && meth[i] != '(' && meth[i] != ':')
        			if (meth[i] == '{') while (i<meth.Length && meth[i] != '}') i++;
        			else i++;
        		if (meth[i] == '(')
        		{
        			sb.Append('(');
        			bool in_kav = false;
        			Stack<char> sk_stack = new Stack<char>();
        			sk_stack.Push('(');i++;
        			bool default_value = false;
        			while (i < meth.Length && sk_stack.Count > 0)
        			{
        				if (meth[i] == '\'') in_kav = !in_kav;
        				else if (meth[i] == '(') {if (!in_kav) sk_stack.Push('(');}
        				else if (meth[i] == ')') {if (!in_kav) sk_stack.Pop();}
        				if (meth[i] == ':' && meth[i+1] == '=' && !in_kav)
        					default_value = true;
        				else if (meth[i] == ';' && !in_kav)
        					default_value = false;
        				if (!default_value || meth[i] == ')' && sk_stack.Count == 0)
        				{
        					if (meth[i] == ')' && sk_stack.Count == 0 && default_value)
        						sb = new StringBuilder(sb.ToString().TrimEnd());
        					sb.Append(meth[i]);
        				}
        				i++;
        			}
        			while (i<meth.Length && meth[i] != ':' && meth[i] != ';')
        				if (meth[i] == '{') while (i<meth.Length && meth[i] != '}') i++;
        				else sb.Append(meth[i++]);
        				
        				//sb.Append(')');
        		}
        		if (meth[i] == ':')
        		{
        			bool in_kav = false;
        			while (i < meth.Length && !(meth[i] == ';' && !in_kav))
        			{
        				if (meth[i] == '{' && !in_kav) while (i<meth.Length && meth[i] != '}') i++;
        				else if (meth[i] == '\'') in_kav = !in_kav;
        				sb.Append(meth[i]);
        				i++;
        			}
        		}
        		sb.Append(';');
        	}
        	sb.AppendLine();
        	sb.AppendLine("begin");
        	for (int j=0; j<tabCount; j++)
        		sb.Append(' ');
        	sb.AppendLine();
        	sb.AppendLine("end;");
        	return sb.ToString();
		}

        private bool isOperator(string Text, int i, out int next)
        {
            next = i;
            if (i >= 3)
            {
                string op = Text.Substring(i - 2, 3).ToLower().Trim();
                if (op == "and" || op == "div" || op == "mod" || op == "xor")
                {
                    if (!char.IsLetterOrDigit(Text[i - 3]) && Text[i - 3] != '_' && Text[i - 3] != '&' && !(i + 1 < Text.Length && char.IsLetterOrDigit(Text[i + 1])))
                    {
                        next = i - 3;
                        return true;
                    }
                    return false;
                }
            }
            if (i >= 2)
            {
                string op = Text.Substring(i - 1, 2).ToLower().Trim();
                if (op == "or")
                {
                    if (!char.IsLetterOrDigit(Text[i - 2]) && Text[i - 2] != '_' && Text[i - 2] != '&')
                    {
                        next = i - 2;
                        return true;
                    }   
                    return false;
                }
            }
            return false;
        }

        private void TestForKeyword(string Text, int i, ref int bound, bool sym_punkt, out KeywordKind keyword)
        {
            StringBuilder sb = new StringBuilder();
            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
            {
                sb.Insert(0, Text[i]);
                i--;
            }
            while (i >= 0 && char.IsWhiteSpace(Text[i]))
            {
                i--;
            }
            if (i >= 0 && (Text[i] == '.' || Text[i] == '&'))
                sb.Insert(0, Text[i]);
            string s = sb.ToString().ToLower();
            if (s == "new")
            {
                bound = i + 1;
                keyword = KeywordKind.New;
            }
            else if ((s == "procedure" || s == "function") && !sym_punkt)
            {
                keyword = KeywordKind.Function;
            }
            else if (s == "constructor" && !sym_punkt)
            {
                keyword = KeywordKind.Constructor;
            }
            else if (s == "destructor" && !sym_punkt)
            {
                keyword = KeywordKind.Destructor;
            }
            else if (s == "uses")
            {
                keyword = KeywordKind.Uses;
            }
            else if (s == "inherited")
            {
                bound = i + 1;
                keyword = KeywordKind.Inherited;
            }
            else if (s == "raise")
            {
                keyword = KeywordKind.Raise;
            }
            else if (keywords.ContainsKey(s))
            {
                keyword = KeywordKind.CommonKeyword;
            }

            else keyword = KeywordKind.None;
        }
		
		private bool CheckForComment(string Text, int off, out int comment_position, out bool one_line_comment)
		{
			int i = off;
            one_line_comment = false;
            comment_position = -1;
			Stack<char> kav = new Stack<char>();
			bool is_comm = false;
			while (i>=0 && !is_comm && Text[i] != '\n')
			{
				if (Text[i] == '\'')
				{
					if (kav.Count == 0) kav.Push('\'');
					else kav.Pop();
				}
				else if (Text[i] == '{')
				{
					if (kav.Count == 0)
                    {
                        is_comm = true;
                        comment_position = i;
                    }  
				}
				else if (Text[i] == '}')
				{
					return false;
				}
				else if (Text[i] == '/')
					if (i > 0 && Text[i-1] == '/' && kav.Count == 0)
                    {
                        is_comm = true;
                        one_line_comment = true;
                        comment_position = i - 1;
                    }
					
				i--;
			}
			return is_comm;
		}

        public virtual string FindExpression(int off, string Text, int line, int col, out KeywordKind keyw)
        {
            int i = off - 1;
            int bound = 0;
            bool punkt_sym = false;
            keyw = KeywordKind.None;
            System.Text.StringBuilder sb = new StringBuilder();
            Stack<char> tokens = new Stack<char>();
            Stack<char> kav = new Stack<char>();
            Stack<char> ugl_skobki = new Stack<char>();
            int num_in_ident = -1;
            keyw = TestForKeyword(Text, i);
            if (keyw == KeywordKind.Punkt) return "";
            while (i >= bound)
            {
                bool end = false;
                char ch = Text[i];
                if (kav.Count == 0 && (char.IsLetterOrDigit(ch) || ch == '_' || ch == '&' || ch == '\''))
                {
                    num_in_ident = i;
                    if (kav.Count == 0 && tokens.Count == 0)
                    {
                        int tmp = i;
                        if (ch == '\'')
                        {
                            i--;
                            if (kav.Count == 0)
                                kav.Push('\'');
                            while (i >= 0 && Text[i] != '\'')
                                i--;
                            if (i >= 0)
                                i--;
                        }
                        else
                            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                            {
                                i--;
                            }
                        while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]) || Text[i] == '}'))
                        {
                            if (Text[i] != '}')
                                i--;
                            else
                            {
                                while (i >= 0 && Text[i] != '{') //propusk kommentariev
                                    i--;
                                if (i >= 0)
                                    i--;
                            }
                        }
                        if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                        {
                            bound = i + 1;
                            TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                            for (int j = tmp; j > bound; j--)
                            {
                                sb.Insert(0, Text[j]);
                            }
                            i = bound;
                            continue;
                        }
                        else if (i >= 0 && Text[i] == '\'') return "";
                        i = tmp;
                    }
                    else
                        if (ch == '\'')
                        kav.Push('\'');
                    sb.Insert(0, ch);//.Append(Text[i]);
                }
                else if (ch == '.' || ch == '^' || ch == '&')
                {
                    if (ch == '.' && i >= 1 && Text[i - 1] == '.') end = true; else sb.Insert(0, ch);
                    if (ch != '.') punkt_sym = true;
                }
                else if (ch == '}')
                {
                    if (kav.Count == 0)
                    {
                        while (i >= 0 && Text[i] != '{')
                        {
                            if (Text[i] != '$')//skip {$
                                sb.Insert(0, Text[i]);
                            i--;
                        }
                        if (i < 0)
                        {
                            break;
                        }
                        else if (Text[i] == '{')
                        {
                            sb.Insert(0, '{');
                        }
                    }
                    else
                        sb.Insert(0, ch);
                }
                else if (ch == '{')
                {
                    if (kav.Count == 0)
                    {
                        if (keyw == KeywordKind.None)
                            return sb.ToString();
                        sb.Insert(0, ch);
                        break;
                    }
                    else sb.Insert(0, ch);
                }
                else
                    switch (ch)
                    {
                        case ')':
                        case ']':
                            if (kav.Count == 0)
                            {
                                string tmps = sb.ToString().Trim(' ', '\r', '\t', '\n');
                                if (tmps.Length >= 1 && (char.IsLetter(tmps[0]) || tmps[0] == '_' || tmps[0] == '&') && tokens.Count == 0)
                                    end = true;
                                else
                                    tokens.Push(ch);
                            }
                            if (!end)
                            {
                                sb.Insert(0, ch);
                                punkt_sym = true;
                            }
                            break;
                        case '>':
                            if (tokens.Count == 0)
                            {
                                if (ugl_skobki.Count > 0 || i == off - 1 || i + 1 < Text.Length && Text[i - 1] != '-' && (Text[i + 1] == '.' || Text[i + 1] == '('))
                                {
                                    ugl_skobki.Push('>');
                                    sb.Insert(0, ch);
                                }
                                else if (i >= 1 && Text[i - 1] == '-')
                                {
                                    if (!(kav.Count == 0 && tokens.Count == 0))
                                        sb.Insert(0, ch);
                                }
                                else
                                    end = true;
                            }
                            else
                                sb.Insert(0, ch);
                            break;
                        case '<':
                            if (tokens.Count == 0)
                            {
                                if (ugl_skobki.Count > 0)
                                {
                                    ugl_skobki.Pop();
                                    sb.Insert(0, ch);
                                }
                                else
                                    end = true;
                            }
                            else
                                sb.Insert(0, ch);
                            break;
                        case '[':
                        case '(':
                            if (kav.Count == 0)
                            {
                                if (tokens.Count > 0)
                                {
                                    tokens.Pop();
                                    punkt_sym = true;
                                    sb.Insert(0, ch);
                                    if (ch == '(')
                                    {
                                        int tmp = i--;
                                        /*while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                                        {
                                            i--;
                                        }*/
                                        while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]) || Text[i] == '}'))
                                        {
                                            if (Text[i] != '}')
                                                i--;
                                            else
                                            {
                                                while (i >= 0 && Text[i] != '{') //propusk kommentariev
                                                    i--;
                                                if (i >= 0)
                                                    i--;
                                            }
                                        }
                                        if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                                        {
                                            bound = i + 1;
                                            TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                            if (keyw != KeywordKind.None && tokens.Count == 0)
                                            {
                                                end = true;
                                            }
                                            else bound = 0;
                                        }
                                        else if (i >= 0 && Text[i] == '\'') return "";
                                        i = tmp;
                                    }
                                }
                                else
                                    end = true;
                            }
                            else sb.Insert(0, ch); punkt_sym = true;
                            break;
                        case '\'':
                            if (kav.Count == 0) kav.Push(ch); else kav.Pop();
                            sb.Insert(0, ch);
                            punkt_sym = true; break;
                        default:
                            if (!(ch == ' ' || char.IsControl(ch)))
                            {
                                if (kav.Count == 0)
                                {
                                    if (ch == ',' && ugl_skobki.Count > 0)
                                        sb.Insert(0, ch);
                                    else
                                    if (tokens.Count == 0)
                                        end = true;
                                    else
                                        sb.Insert(0, ch);
                                }
                                else
                                    sb.Insert(0, ch);
                            }
                            else
                            {
                                if (Text[i] == '\n')
                                {
                                    bool one_line_comment = false;
                                    int comment_position = -1;
                                    if (CheckForComment(Text, i - 1, out comment_position, out one_line_comment))
                                    {
                                        if (!one_line_comment)
                                            end = true;
                                        else
                                        {
                                            sb.Insert(0, ch);
                                            i = comment_position;
                                        }
                                            
                                    }    
                                    else
                                        sb.Insert(0, ch);
                                }
                                else
                                    sb.Insert(0, ch);
                            }
                            punkt_sym = true;
                            break;
                    }

                if (end)
                {
                    bool one_line_comment = false;
                    int comment_position = -1;
                    if (CheckForComment(Text, i, out comment_position, out one_line_comment))
                    {
                        int new_line_ind = sb.ToString().IndexOf('\n');
                        if (new_line_ind != -1) sb = sb.Remove(0, new_line_ind + 1);
                        else sb = sb.Remove(0, sb.Length);
                    }
                    break;
                }
                i--;
            }

            //return RemovePossibleKeywords(sb);
            return sb.ToString();

        }

        public virtual string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out string expr_without_brackets)
        {
            KeywordKind keyw = KeywordKind.None;
            return FindExpressionFromAnyPosition(off, Text, line, col, out keyw, out expr_without_brackets);
        }

        public virtual string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out KeywordKind keyw, out string expr_without_brackets)
        {
            int i = off - 1;
            expr_without_brackets = null;
            keyw = KeywordKind.None;
            if (i < 0)
                return "";
            bool is_char = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Text[i] != ' ' && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
            {
                //sb.Remove(0,sb.Length);
                while (i >= 0 && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                {
                    //sb.Insert(0,Text[i]);//.Append(Text[i]);
                    i--;
                }
                is_char = true;
            }
            i = off;
            if (i < Text.Length && Text[i] != ' ' && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
            {
                while (i < Text.Length && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                {
                    //sb.Append(Text[i]);//.Append(Text[i]);
                    i++;
                }
                is_char = true;
            }
            if (is_char)
            {
                expr_without_brackets = FindExpression(i, Text, line, col, out keyw);
            }
            bool is_new = keyw == KeywordKind.New;
            bool meth_call = false;
            KeywordKind new_keyw = KeywordKind.None;
            int j = i;
            bool in_comment = false;
            bool brackets = false;
            while (j < Text.Length)
            {
                char c = Text[j];

                if (c == '(' && !in_comment)
                {
                    Stack<char> sk_stack = new Stack<char>();
                    in_comment = false;
                    bool in_kav = false;
                    sk_stack.Push('(');
                    j++;
                    while (j < Text.Length)
                    {
                        c = Text[j];
                        if (c == '(' && !in_kav)
                            sk_stack.Push('(');
                        else
                        if (c == ')' && !in_kav)
                        {
                            if (sk_stack.Count == 0)
                            {
                                break;
                            }
                            else
                            {
                                sk_stack.Pop();
                                if (sk_stack.Count == 0)
                                {
                                    i = j + 1;
                                    meth_call = true;
                                    break;
                                }
                            }
                        }
                        else if (c == '\'' && !in_comment)
                        {
                            in_kav = !in_kav;
                        }
                        else if (c == '{' && !in_kav)
                        {
                            in_comment = true;
                        }
                        else if (c == '}' && !in_kav)
                        {
                            in_comment = false;
                        }
                        else if (c == '/' && !in_kav && !in_comment)
                        {
                            if (j + 1 < Text.Length && Text[j + 1] == '/')
                                break;
                        }
                        j++;
                    }
                    break;
                }
                else if (c == '<' && !in_comment)
                {
                    Stack<char> sk_stack = new Stack<char>();
                    sk_stack.Push('<');
                    j++;
                    bool generic = false;
                    while (j < Text.Length)
                    {
                        c = Text[j];
                        if (c == '>')
                        {
                            sk_stack.Pop();
                            if (sk_stack.Count == 0)
                            {
                                i = j + 1;
                                generic = true;
                                break;
                            }
                        }
                        else if (c == '<')
                        {
                            sk_stack.Push('<');

                        }
                        else if (!char.IsLetterOrDigit(c) && c != '&' && c != '.' && c != ' ' && c != '\t' && c != '\n' && c != ',')
                        {
                            break;
                        }
                        j++;
                    }
                    if (generic)
                    {
                        break;
                    }
                }
                else if (c == '[' && !in_comment)
                {
                    brackets = true;
                    break;
                }
                else if (c == '{')
                {
                    in_comment = true;
                }
                else if (c == '}')
                {
                    if (!in_comment)
                        break;
                    else
                        in_comment = false;
                }
                else if (c == ' ' || c == '\t' || c == '\n' || c == '\r')
                {

                }
                else
                {
                    if (!in_comment)
                        break;
                }
                j++;
            }
            if (is_new && string.Compare(expr_without_brackets.Trim(' ', '\n', '\t', '\r'), "new", true) == 0 && !meth_call)
            {
                expr_without_brackets = null;
                return null;
            }
            if (is_char)
            {
                string ss = FindExpression(i, Text, line, col, out new_keyw);
                if (brackets && is_new)
                {
                    int ind = ss.ToLower().IndexOf("new");
                    if (ind != -1)
                        return ss.Substring(ind + 3);
                }
                return ss;
            }
            return null;
        }

        public virtual KeywordKind TestForKeyword(string Text, int i)
        {
            StringBuilder sb = new StringBuilder();
            int j = i;
            bool in_keyw = false;
            while (j >= 0 && Text[j] != '\n')
                j--;
            Stack<char> kav_stack = new Stack<char>();
            j++;
            bool in_format_str = false;
            while (j <= i)
            {
                if (Text[j] == '\'')
                {
                    if (kav_stack.Count == 0 && !in_keyw)
                    {
                        if (j == 0 || Text[j - 1] != '$')
                            kav_stack.Push('\'');
                        else
                        {
                            in_keyw = false;
                            in_format_str = true;
                        }
                            
                    }   
                    else if (kav_stack.Count > 0)
                        kav_stack.Pop();
                }
                else if (Text[j] == '{' && kav_stack.Count == 0 && !in_format_str)
                    in_keyw = true;
                else
                    if (Text[j] == '}')
                    in_keyw = false;
                j++;
            }
            j = i;
            if (kav_stack.Count != 0 || in_keyw) return PascalABCCompiler.Parsers.KeywordKind.Punkt;
            if (j >= 0 && Text[j] == '.') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
            while (j >= 0)
            {
                //if (Text[j] == '{') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
                if (!in_keyw && (Text[j] == '\'' || Text[j] == '\n'))
                    break;
                if (Text[j] == '}')
                    in_keyw = true;
                else
                if (Text[j] == '/' && !in_keyw)
                    if (j > 0 && Text[j - 1] == '/') return PascalABCCompiler.Parsers.KeywordKind.Punkt;
                j--;
            }
            //if (j>= 0 && Text[j] == '\'') return CodeCompletion.KeywordKind.kw_punkt;
            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]))) i--;
            if (i >= 0 && Text[i] == ':') return PascalABCCompiler.Parsers.KeywordKind.Colon;

            if (i >= 0 && Text[i] == ',')
            {

                while (i >= 0 && Text[i] != '\n')
                {
                    if (char.IsLetterOrDigit(Text[i]))
                        sb.Insert(0, Text[i]);
                    else
                    {
                        PascalABCCompiler.Parsers.KeywordKind keyw = GetKeywordKind(sb.ToString());
                        if (keyw == PascalABCCompiler.Parsers.KeywordKind.Uses)
                            return PascalABCCompiler.Parsers.KeywordKind.Uses;
                        else if (keyw == PascalABCCompiler.Parsers.KeywordKind.Var)
                            return PascalABCCompiler.Parsers.KeywordKind.Var;
                        else sb.Remove(0, sb.Length);
                    }
                    i--;
                }
            }
            else
                while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
                {
                    sb.Insert(0, Text[i]);
                    i--;
                }
            string s = sb.ToString().ToLower();

            return GetKeywordKind(s);
        }
		
		public virtual string SkipNew(int off, string Text, ref KeywordKind keyw)
        {
        	int tmp = off;
        	string expr = null;
        	while (off >= 0 && Char.IsLetterOrDigit(Text[off])) off--;
        	while (off >= 0 && (Text[off] == ' ' || char.IsControl(Text[off]))) off--;
        	if (off >= 1 && Text[off] == '=' && Text[off-1] == ':')
        	{
        		off -= 2;
        		while (off >= 0 && (Text[off] == ' ' || char.IsControl(Text[off]))) off--;
        		if (off >= 0 && (Text[off] == '_' || char.IsLetterOrDigit(Text[off]) || Text[off] == ']' || Text[off] == '>'))
        			expr = FindExpression(off+1,Text,0,0,out keyw);
        	}
        	return expr;
        }

        public virtual string FindExpressionForMethod(int off, string Text, int line, int col, char pressed_key, ref int num_param)
        {
            int i = off - 1;
            string pattern = null;
            int bound = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Stack<char> tokens = new Stack<char>();
            Stack<char> kav = new Stack<char>();
            Stack<char> skobki = new Stack<char>();
            Stack<char> ugl_skobki = new Stack<char>();
            bool comma_pressed = pressed_key == ',';
            int num_in_ident = -1;
            bool punkt_sym = false;
            int next;
            KeywordKind keyw = TestForKeyword(Text, i);
            bool on_brace = false;
            if (keyw == KeywordKind.Punkt)
                return "";
            try
            {
                while (i >= bound)
                {
                    bool end = false;
                    char ch = Text[i];
                    if ((char.IsLetterOrDigit(ch) || ch == '_' || ch == '&') && !isOperator(Text, i, out next))
                    {
                        num_in_ident = i;
                        if (kav.Count == 0 && tokens.Count == 0)
                        {
                            int tmp = i;
                            while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&') && !isOperator(Text, i, out next))
                            {
                                i--;
                            }
                            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]) || Text[i] == '}'))
                            {
                                if (Text[i] != '}')
                                    i--;
                                else
                                {
                                    while (i >= 0 && Text[i] != '{') //propusk kommentariev
                                        i--;
                                    if (i >= 0)
                                        i--;
                                }
                            }
                            if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&') && !isOperator(Text, i, out next))
                            {
                                bound = i + 1;
                                TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                if (keyw == KeywordKind.New && comma_pressed)
                                    bound = 0;
                                if (keyw == KeywordKind.Function || keyw == KeywordKind.Constructor || keyw == KeywordKind.Destructor)
                                    return "";
                            }
                            else if (i >= 0 && Text[i] == '\'') return "";
                            i = tmp;
                        }
                        sb.Insert(0, ch);//.Append(Text[i]);
                    }
                    else if (ch == '.') sb.Insert(0, ch);
                    else if (ch == '}')
                    {
                        if (kav.Count == 0)
                        {
                            while (i >= 0 && Text[i] != '{')
                            {
                                sb.Insert(0, Text[i]);
                                i--;
                            }
                            if (i < 0)
                            {
                                break;
                            }
                            else if (Text[i] == '{')
                            {
                                sb.Insert(0, '{');
                            }
                        }
                        else
                            sb.Insert(0, ch);
                    }
                    else if (ch == '{')
                    {
                        if (kav.Count == 0)
                        {
                            sb.Insert(0, ch);
                            break;
                        }
                        else sb.Insert(0, ch);
                    }
                    else
                        switch (ch)
                        {
                            case ')':
                            case ']':
                            case '>':
                                if (kav.Count == 0)
                                {
                                    if (ch != '>')
                                        tokens.Push(ch);
                                    if (ch == ')')
                                        skobki.Push(ch);
                                    if (tokens.Count > 0 || pressed_key == ',')
                                        sb.Insert(0, ch);
                                    else if (i == off - 1 || ugl_skobki.Count > 0 || i + 1 < Text.Length && Text[i-1] != '-' && (Text[i + 1] == '.' || Text[i + 1] == '('))
                                    {
                                        tokens.Push(ch);
                                        ugl_skobki.Push(ch);
                                        sb.Insert(0, ch);
                                    }
                                    else
                                        end = true;
                                }
                                else
                                    sb.Insert(0, ch); break;
                            case '[':
                            case '<':
                            case '(':
                                if (kav.Count == 0)//esli ne v kavychkah
                                {
                                    if (ch == '(')
                                        if (skobki.Count > 0)
                                            skobki.Pop();
                                        else skobki.Push('(');
                                    //esli byli zakryvaushie tokeny (polagaem, chto skobki korrektny, esli net, to parser v lubom sluchae ne proparsit
                                    if (tokens.Count > 0)
                                    {
                                        if (ch != '<')
                                            tokens.Pop();
                                        else if (ugl_skobki.Count > 0)
                                        {
                                            tokens.Pop();
                                            ugl_skobki.Pop();
                                        }
                                        sb.Insert(0, ch);//dobavljaem k stroke
                                        if (ch == '(')
                                        {
                                            int tmp = i--;
                                            /*while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                                            {
                                                i--;
                                            }*/
                                            while (i >= 0 && (Text[i] == ' ' || char.IsControl(Text[i]) || Text[i] == '}'))
                                            {
                                                if (Text[i] != '}')
                                                {
                                                    i--;
                                                }
                                                else
                                                {
                                                    while (i >= 0 && Text[i] != '{')
                                                    {
                                                        //propusk kommentariev
                                                        i--;
                                                    }
                                                    if (i >= 0)
                                                        i--;
                                                }
                                            }

                                            if (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
                                            {
                                                bound = i + 1;
                                                TestForKeyword(Text, i, ref bound, punkt_sym, out keyw);
                                                if (keyw == KeywordKind.New)
                                                    bound = 0;
                                                else
                                                if (keyw != KeywordKind.None && tokens.Count == 0)
                                                    end = true;
                                                else
                                                    bound = 0;
                                            }
                                            else if (i >= 0 && Text[i] == '\'') return "";
                                            i = tmp;
                                        }
                                    }
                                    else if (ch == '<' || ch == '>')
                                    {
                                        if (tokens.Count > 0 || pressed_key == ',')
                                            sb.Insert(0, ch);
                                        else
                                            end = true;
                                    }
                                    else
                                    if (!comma_pressed) //esli my ne v parametrah
                                        end = true; //zakonchili, tak kak doshli do pervoj skobki
                                    else
                                    {
                                        sb.Remove(0, sb.Length);//doshli do skobki, byla nazhata zapjataja, poetomu udaljaem vse parametry
                                        if (ch == '(')
                                            on_brace = true;
                                        //on_skobka = true;
                                        comma_pressed = false;
                                    }
                                }
                                else sb.Insert(0, ch);
                                break;
                            case '\'':
                                if (kav.Count == 0)
                                    kav.Push(ch);
                                else
                                    kav.Pop();
                                sb.Insert(0, ch);
                                break;
                            default:
                                if (!(ch == ' ' || char.IsControl(ch)))
                                {
                                    if (ch == '^')
                                        sb.Insert(0, ch);
                                    else
                                    if (kav.Count == 0)//ne v kavychkah
                                    {
                                        if (tokens.Count == 0)
                                        {
                                            if (ch != ',' && !comma_pressed)
                                                end = true;//esli ne na zapjatoj i ne v parametrah, to finish
                                            else if (ch == ',' && !comma_pressed)
                                                end = true;
                                            else if (ch == ',' && (pressed_key == '(' || pressed_key == '['))
                                                end = true;
                                            else
                                            {
                                                sb.Insert(0, ch);//prodolzhaem
                                                if (isOperator(Text, i, out next))
                                                {
                                                    i = next;
                                                    continue;
                                                }
                                            }
                                                
                                        }
                                        else
                                            sb.Insert(0, ch);//esli est skobki, prodolzhaem
                                        if (!end && ch == ',')
                                        {
                                            if (tokens.Count == 0)
                                                num_param++;//esli na zapjatoj, uvelichivaem nomer parametra
                                        }
                                    }
                                    else
                                        sb.Insert(0, ch);//prodolzhaem

                                }
                                else
                                if (Text[i] == '\n')
                                {
                                    bool one_line_comment = false;
                                    int comment_position = -1;
                                    if (CheckForComment(Text, i - 1, out comment_position, out one_line_comment)) //proverjaem, net li kommenta ne predydushej stroke
                                    {
                                        if (!one_line_comment)
                                            end = true;
                                        else
                                        {
                                            sb.Insert(0, ch);
                                            i = comment_position;
                                        }
                                            
                                    }
                                    else
                                        sb.Insert(0, ch);//a inache vyrazhenie na neskolkih strokah
                                }
                                else
                                    sb.Insert(0, ch);
                                break;
                        }

                    if (end)
                    {
                        if (comma_pressed && !on_brace)
                            return "";
                        bool one_line_comment = false;
                        int comment_position = -1;
                        if (CheckForComment(Text, i, out comment_position, out one_line_comment))//proverka na kommentarii
                        {
                            int new_line_ind = sb.ToString().IndexOf('\n');
                            if (new_line_ind != -1) sb = sb.Remove(0, new_line_ind + 1);
                            else sb = sb.Remove(0, sb.Length);
                        }
                        break;
                    }
                    i--;
                }
                if (pressed_key == ',')
                    num_param++;
            }
            catch (Exception e)
            {

            }
            if (pressed_key == ',' && (!on_brace || skobki.Count == 0))
                return "";
            //return RemovePossibleKeywords(sb);
            return sb.ToString();
        }
		
		public virtual string FindOnlyIdentifier(int off, string Text, int line, int col, ref string name)
		{
			int i = off-1;
            if (i < 0)
                return "";
            bool is_char = false;
        	System.Text.StringBuilder sb = new System.Text.StringBuilder();
        	if (Text[i] != ' ' && !char.IsControl(Text[i]))
        	{
        		sb.Remove(0,sb.Length);
        		while (i >= 0 && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[i] == '&'))
        		{
        			//sb.Insert(0,Text[i]);//.Append(Text[i]);
        			i--;
        		}
        		is_char = true;
        	}
        	i++;
            if (i < Text.Length && Text[i] != ' ' && !char.IsControl(Text[i]))
        	{
        		while (i < Text.Length && (Char.IsLetterOrDigit(Text[i]) || Text[i] == '_'))
        		{
        			sb.Append(Text[i]);//.Append(Text[i]);
        			i++;
        		}
        		is_char = true;
        	}
            name = sb.ToString();
            KeywordKind keyw = KeywordKind.None;
        	if (is_char) 
        	{
        		return FindExpression(i,Text,line,col,out keyw);
        	}
            return null;
		}
		
		public virtual string FindPattern(int off, string Text, out bool is_pattern)
		{
			System.Text.StringBuilder sb=null;
			is_pattern = false;
            if (off > 0 && (char.IsLetterOrDigit(Text[off - 1]) || Text[off - 1] == '_' || Text[off-1] == '&'))
            {
                sb = new System.Text.StringBuilder();
                int i = off - 1;
                is_pattern = true;
                while (i >= 0 && (char.IsLetterOrDigit(Text[i]) || Text[i] == '_' || Text[off-1] == '&'))
                  sb.Insert(0, Text[i--]);
                return sb.ToString();
            }
            return null;
		}
		
		public virtual bool IsMethodCallParameterSeparator(char key)
		{
			return key == ',';
		}
		
		public virtual bool IsOpenBracketForMethodCall(char key)
		{
			return key == '(';
		}
		
		public virtual bool IsOpenBracketForIndex(char key)
		{
			return key == '[';
		}
		
		public virtual bool IsDefinitionIdentifierAfterKeyword(KeywordKind keyw)
		{
			if (keyw == PascalABCCompiler.Parsers.KeywordKind.Function || keyw == PascalABCCompiler.Parsers.KeywordKind.Constructor || keyw == PascalABCCompiler.Parsers.KeywordKind.Destructor || keyw == PascalABCCompiler.Parsers.KeywordKind.Type || keyw == PascalABCCompiler.Parsers.KeywordKind.Var
           		|| keyw == PascalABCCompiler.Parsers.KeywordKind.Unit || keyw == PascalABCCompiler.Parsers.KeywordKind.Const || keyw == PascalABCCompiler.Parsers.KeywordKind.Program || keyw == PascalABCCompiler.Parsers.KeywordKind.Punkt)
           	return true;
			return false;
		}
		
		public virtual bool IsTypeAfterKeyword(KeywordKind keyw)
		{
			if (keyw == PascalABCCompiler.Parsers.KeywordKind.Colon || keyw == PascalABCCompiler.Parsers.KeywordKind.Of || keyw == PascalABCCompiler.Parsers.KeywordKind.TypeDecl)
				return true;
			return false;
		}
		
		public virtual bool IsNamespaceAfterKeyword(KeywordKind keyw)
		{
			if (keyw == PascalABCCompiler.Parsers.KeywordKind.Uses) return true;
			return false;
		}
		
	}
	
	public class CLanguageInformation : DefaultLanguageInformation
	{
		public CLanguageInformation(IParser p)
		{
			this.parser = p;
			InitKeywords();
		}
		
		protected override void InitKeywords()
		{
			List<string> keys = new List<string>();
            List<string> type_keys = new List<string>();
            keywords.Add("for", "for"); keys.Add("for");
            keywords.Add("while", "while"); keys.Add("while");
            keywords.Add("do", "do"); keys.Add("do");
//            keywords.Add("class", "class"); keys.Add("class"); type_keys.Add("class");
            keywords.Add("struct", "struct"); keys.Add("struct"); type_keys.Add("struct");
            keywords.Add("typedef", "typedef"); keys.Add("typedef"); keyword_kinds.Add("typedef", KeywordKind.Type);
//            keywords.Add("private", "private"); keys.Add("private");
//            keywords.Add("protected", "protected"); keys.Add("protected");
//            keywords.Add("public", "public"); keys.Add("public");
//            keywords.Add("try", "try"); keys.Add("try");
//            keywords.Add("catch", "catch"); keys.Add("catch");
//            keywords.Add("finally", "finally"); keys.Add("finally");
//            keywords.Add("using", "using"); keys.Add("using"); keyword_kinds.Add("using", KeywordKind.Uses);

            //keywords.Add("virtual", "virtual"); keys.Add("virtual");
            keywords.Add("static", "static"); keys.Add("static");
			
            keywords.Add("switch", "switch"); keys.Add("switch");
            keywords.Add("case", "case"); keys.Add("case");
            
            keywords.Add("const", "const"); keys.Add("const"); keyword_kinds.Add("const", KeywordKind.Const);
            keywords.Add("if", "if"); keys.Add("if");
            keywords.Add("else", "else"); keys.Add("else");
            keywords.Add("new", "new"); keys.Add("new"); keyword_kinds.Add("new", KeywordKind.New);
            keywords.Add("char","char"); keys.Add("char"); type_keys.Add("char");
            keywords.Add("short","short"); keys.Add("short"); type_keys.Add("short");
            keywords.Add("int","int"); keys.Add("int"); type_keys.Add("int");
            keywords.Add("long","long"); keys.Add("long"); type_keys.Add("long");
            keywords.Add("float","float"); keys.Add("float"); type_keys.Add("float");
            keywords.Add("double","double"); keys.Add("double"); type_keys.Add("double");
            keywords.Add("bool","bool"); keys.Add("bool"); type_keys.Add("bool");
            keywords.Add("unsigned","unsigned"); keys.Add("unsigned");
            keywords_array = keys.ToArray();
            type_keywords_array = type_keys.ToArray();
		}
		
		public override string SystemUnitName
		{
            get
            {
                return null;
            }
		}

        public override bool CaseSensitive
        {
            get
            {
                return true;
            }
        }

        public override bool IncludeDotNetEntities
        {
            get
            {
                return false;
            }
        }

		public override string GetKeyword(SymbolKind kind)
		{
			switch (kind)
			{
				case SymbolKind.Class : return "class";
				case SymbolKind.Enum : return "enum";
				case SymbolKind.Struct : return "struct";
				case SymbolKind.Type : return "typedef";
			}
			return "";
		}
		
		public override string GetStandardTypeByKeyword(KeywordKind keyw)
		{
			
			return null;
		}
		
		protected override string GetFullTypeName(Type ctn, bool no_alias=true)
		{
			TypeCode tc = Type.GetTypeCode(ctn);
			if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition) 
				return "T";
			if (!ctn.IsEnum)
			switch (tc)
			{
				case TypeCode.Int32 : return "int";
				case TypeCode.Double : return "double";
				case TypeCode.Boolean : return "bool";
				case TypeCode.String : return "string";
				case TypeCode.Char : return "char";
				case TypeCode.Byte : return "unsigned char";
				case TypeCode.SByte : return "char";
				case TypeCode.Int16 : return "short";
				case TypeCode.UInt16 : return "unsigned short";
				case TypeCode.UInt32 : return "unsigned int";
				case TypeCode.Single : return "float";
				default : return ctn.FullName;
			}
			else return ctn.FullName;
			
			if (ctn.Name.Contains("`"))
			{
				int len = ctn.GetGenericArguments().Length;
				Type[] gen_ps = ctn.GetGenericArguments();
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(ctn.Namespace+"::"+ctn.Name.Substring(0,ctn.Name.IndexOf('`')));
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
			if (ctn.IsArray) return GetFullTypeName(ctn.GetElementType())+"[]";
			if (ctn == Type.GetType("System.Void*")) return "void*";
			return ctn.FullName;
		}
		
		public override string GetShortTypeName(Type ctn, bool noalias=false)
		{
			TypeCode tc = Type.GetTypeCode(ctn);
			if (ctn.FullName == null && !ctn.IsArray && !ctn.IsGenericTypeDefinition) 
				return "T";
			if (ctn.IsEnum) return ctn.Name;
			
			if (ctn.Name.Contains("`"))
			{
				int len = ctn.GetGenericArguments().Length;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(ctn.Name.Substring(0,ctn.Name.IndexOf('`')));
				sb.Append('<');
				sb.Append('>');
				/*sb.Append('<');
				for (int i=0; i<len; i++)
				{
					sb.Append('T');
					if (i<len-1)
					sb.Append(',');
				}
				sb.Append('>');*/
				return sb.ToString();
			}
			//if (ctn.IsArray) return "array of "+GetTypeName(ctn.GetElementType());
			//if (ctn == Type.GetType("System.Void*")) return PascalABCCompiler.TreeConverter.compiler_string_consts.pointer_type_name;
			return ctn.Name;
		}
		
		protected override string GetTopScopeName(IBaseScope sc)
		{
			if (sc == null) return "";
			if (sc.Name == "" || sc.Name.Contains("$") || sc.Name == "PABCSystem") return "";
			if (sc is IProcScope) return "";
			return sc.Name + "::";
		}
		
		protected override string GetDescriptionForType(ITypeScope scope)
		{
			string template_str=GetTemplateString(scope);
			switch(scope.ElemKind)
			{
				case SymbolKind.Class : 
					if (scope.TopScope != null && scope.TopScope.Name != "")
						return (scope.IsAbstract ? "abstract " : "") + (scope.IsFinal?"sealed ":"")+"class "+scope.TopScope.Name + "::" +scope.Name+template_str;
					else return (scope.IsAbstract ? "abstract " : "") + (scope.IsFinal?"sealed ":"")+"class "+scope.Name+template_str;
				case SymbolKind.Interface :
					if (scope.TopScope != null && scope.TopScope.Name != "")
					return "interface class"+scope.TopScope.Name + "::" +scope.Name+template_str;
					else return "interface class"+scope.Name+template_str;
				case SymbolKind.Enum :
					if (scope.TopScope != null && scope.TopScope.Name != "")
					return "enum "+scope.TopScope.Name + "::" +scope.Name;
					else return "enum "+scope.Name;
				case SymbolKind.Delegate :
					if (scope.TopScope != null && scope.TopScope.Name != "")
					return "delegate "+scope.TopScope.Name + "::" +scope.Name+template_str;
					else return "delegate "+scope.Name+template_str;
				case SymbolKind.Struct :
					if (scope.TopScope != null && scope.TopScope.Name != "")
					return "struct "+scope.TopScope.Name + "::" +scope.Name+template_str;
					else return "struct "+scope.Name+template_str;
			}
			
			if (scope.TopScope != null)
			return scope.TopScope.Name + "::" +scope.Name;
			else return scope.Name;
		}
		
		protected override string GetSimpleDescriptionForType(ITypeScope scope)
		{
			string template_str=GetTemplateString(scope);
			return scope.Name+template_str;
		}
		
		protected override string GetDescriptionForCompiledType(ICompiledTypeScope scope)
		{
			string s = GetFullTypeName(scope.CompiledType);
			ITypeScope[] instances = scope.GenericInstances;
			if (instances.Length > 0)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				int ind = s.IndexOf('<');
				if (ind != -1) sb.Append(s.Substring(0,ind));
				else
				sb.Append(s);
				sb.Append('<');
				for (int i=0; i<instances.Length; i++)
				{
					sb.Append(instances[i].Name);
					if (i < instances.Length - 1) sb.Append(',');
				}
				sb.Append('>');
				s = sb.ToString();
			}
			
			switch(scope.ElemKind)
			{
				case SymbolKind.Class : 					
					return (scope.IsFinal?"sealed ":"")+"class "+s;
				case SymbolKind.Interface :
					return "class interface "+s;
				case SymbolKind.Enum :
					return "enum "+s;
				case SymbolKind.Delegate :
					return "delegate "+s;
				case SymbolKind.Struct :
					return "struct "+s;
				case SymbolKind.Type :
					return "typedef "+s;
			}
			return s;
		}
		
		protected override string GetSimpleDescriptionForCompiledType(ICompiledTypeScope scope)
		{
			string s = GetFullTypeName(scope.CompiledType);
			ITypeScope[] instances = scope.GenericInstances;
			if (instances.Length > 0)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				int ind = s.IndexOf('<');
				if (ind != -1) sb.Append(s.Substring(0,ind));
				else
				sb.Append(s);
				sb.Append('<');
				for (int i=0; i<instances.Length; i++)
				{
					sb.Append(instances[i].Name);
					if (i < instances.Length - 1) sb.Append(',');
				}
				sb.Append('>');
				s = sb.ToString();
			}
			return s;
		}
		
		protected override string GetDescriptionForArray(IArrayScope scope)
		{
			StringBuilder sb = new StringBuilder();
			ITypeScope[] inds = scope.Indexers;
			if (scope.ElementType != null)
			{
				string s = GetSimpleDescription(scope.ElementType);
				if (s.Length > 0 && s[0] == '$') s = s.Substring(1,s.Length-1);
				sb.Append(s);
			}
			if (!scope.IsDynamic)
			{
				sb.Append('[');
				for (int i=0; i<inds.Length; i++)
				{
					sb.Append(GetSimpleDescription(inds[i]));
					if (i<inds.Length-1) sb.Append(',');
				}
				sb.Append(']');
			}
			return sb.ToString();
		}
		
		protected override string GetDescriptionForPointer(IPointerScope scope)
		{
			if (scope.ElementType != null)
				return GetSimpleDescription(scope.ElementType)+"*";
			return "void*";
		}
		
		private void append_modifiers(StringBuilder sb, IElementScope scope)
		{
			if (scope.IsVirtual) sb.Append("virtual ");
			if (scope.IsStatic) sb.Append("static ");
		}
		
		private string get_access_modifier(access_modifer mod)
		{
			switch (mod)
			{
				case access_modifer.private_modifer : return "private ";
				case access_modifer.public_modifer : return "public ";
				case access_modifer.protected_modifer : return "protected ";
				case access_modifer.none : return "";
			}
			return "";
		}
		
		protected override string GetDescriptionForElementScope(IElementScope scope)
		{
			string type_name=null;
			string s="";
			StringBuilder sb = new StringBuilder();
			if (scope.Type == null) type_name = "";
			else
				type_name = GetSimpleDescription(scope.Type);
			if (type_name.StartsWith("$")) 
				type_name = type_name.Substring(1,type_name.Length-1);
			switch (scope.ElemKind)
			{
				case SymbolKind.Variable : sb.Append(type_name+" "+scope.Name); break;
				case SymbolKind.Parameter : sb.Append("parameter "+type_name+" "+scope.Name); break;
				case SymbolKind.Constant : 
				{
					if (scope.ConstantValue == null)
						sb.Append("const "+ scope.Name);
					else sb.Append("const "+ scope.Name+" = "+scope.ConstantValue.ToString());
				}
				break;
				case SymbolKind.Event :
					sb.Append(get_access_modifier(scope.AccessModifier));
					append_modifiers(sb,scope);
					sb.Append("event "+ type_name + " "+GetTopScopeName(scope.TopScope)+scope.Name);
					break;
				case SymbolKind.Field :
					sb.Append(get_access_modifier(scope.AccessModifier));
					append_modifiers(sb,scope);
					sb.Append(type_name+" "+GetTopScopeName(scope.TopScope)+scope.Name );
					break;
				case SymbolKind.Property :
					sb.Append(get_access_modifier(scope.AccessModifier));
					append_modifiers(sb,scope);
					sb.Append(type_name + " "+GetTopScopeName(scope.TopScope)+scope.Name + get_index_description(scope)); 
					break;
					
			}
			return sb.ToString();
		}
		
		protected override string GetSimpleDescriptionForElementScope(IElementScope scope)
		{
			string type_name = GetSimpleDescription(scope.Type);
			if (type_name.StartsWith("$")) type_name = type_name.Substring(1,type_name.Length-1);
			return type_name + " " + scope.Name;
		}
		
		protected override string GetDescriptionForProcedure(IProcScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (scope.ReturnType != null)
				sb.Append(GetSimpleDescription(scope.ReturnType)+" ");
			else if (!scope.IsConstructor())
				sb.Append("void ");
			sb.Append(scope.Name); sb.Append('(');
			IElementScope[] parameters = scope.Parameters;
			for (int i=0; i<parameters.Length; i++)
			{
				sb.Append(GetSimpleDescription(parameters[i]));
				if (i < parameters.Length - 1)
				{
					sb.Append(", ");
				}
			}
			sb.Append(')');
			return sb.ToString();
		}
		
		protected override string GetSimpleDescriptionForProcedure(IProcScope scope)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (scope.TopScope is ITypeScope && scope.Realization == null)
				sb.Append(GetTopScopeName(scope.TopScope));
			sb.Append(scope.Name); sb.Append('(');
			IElementScope[] parameters = scope.Parameters;
			for (int i=0; i<parameters.Length; i++)
			{
				sb.Append(GetSimpleDescription(parameters[i]));
				if (i < parameters.Length - 1)
				{
					sb.Append(", ");
				}
			}
			sb.Append(')');
			return sb.ToString();
		}
	}
}

