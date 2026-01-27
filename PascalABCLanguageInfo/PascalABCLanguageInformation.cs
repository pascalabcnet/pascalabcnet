// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;

using PascalABCCompiler.Parsers;
using PascalABCCompiler;
using PascalABCCompiler.ParserTools.Directives;
using static PascalABCCompiler.ParserTools.Directives.DirectiveHelper;

namespace Languages.Pascal.Frontend.Data
{
    public class PascalABCLanguageInformation : ILanguageInformation
    {
        public string Name => StringConstants.pascalLanguageName;

        public string Version => "1.2";

        public string Copyright => "Copyright © 2005-2026 by Ivan Bondarev, Stanislav Mikhalkovich";

		public bool CaseSensitive => false;

        public string[] FilesExtensions => new string[] { StringConstants.pascalSourceFileExtension };

        public string[] SystemUnitNames => StringConstants.pascalDefaultStandardModules;

        public bool SyntaxTreeIsConvertedAfterUsedModulesCompilation => false;

        public string CommentSymbol => "//";

		public Dictionary<string, string> SpecialModulesAliases => null;

        public BaseKeywords KeywordsStorage { get; } = new Core.PascalABCKeywords();

        public Dictionary<string, DirectiveInfo> ValidDirectives { get; protected set; }

        public PascalABCLanguageInformation()
        {
            InitializeValidDirectives();
        }

        /// <summary>
        /// Здесь записываются все директивы, поддерживаемые языком, а также правила для проверки их параметров (ограничения, накладываемые со стороны языка)
        /// </summary>
        private void InitializeValidDirectives()
        {
            #region VALID DIRECTIVES
            ValidDirectives = new Dictionary<string, DirectiveInfo>(StringComparer.CurrentCultureIgnoreCase)
            {
                [StringConstants.compiler_directive_apptype] = new DirectiveInfo(SingleAnyOfCheck("console", "windows", "dll", "pcu")),
                [StringConstants.compiler_directive_reference] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_include_namespace] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_savepcu] = new DirectiveInfo(SingleAnyOfCheck("true", "false")),
                [StringConstants.compiler_directive_zerobasedstrings] = new DirectiveInfo(SingleAnyOfCheck("on", "off"), paramsNums: new int[2] { 0, 1 }),
                [StringConstants.compiler_directive_zerobasedstrings_ON] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_zerobasedstrings_OFF] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_nullbasedstrings_ON] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_nullbasedstrings_OFF] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_initstring_as_empty_ON] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_initstring_as_empty_OFF] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_resource] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_platformtarget] = new DirectiveInfo(SingleAnyOfCheck("x86", "x64", "anycpu", "dotnet5win", "dotnet5linux", "dotnet5macos", "native")),
                [StringConstants.compiler_directive_faststrings] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_gendoc] = new DirectiveInfo(SingleAnyOfCheck("true", "false")),
                [StringConstants.compiler_directive_region] = new DirectiveInfo(checkParamsNumNeeded: false),
                [StringConstants.compiler_directive_endregion] = new DirectiveInfo(checkParamsNumNeeded: false),
                [StringConstants.compiler_directive_ifdef] = new DirectiveInfo(SingleIsValidIdCheck()),
                [StringConstants.compiler_directive_endif] = new DirectiveInfo(SingleIsValidIdCheck(), paramsNums: new int[2] { 0, 1 }),
                [StringConstants.compiler_directive_ifndef] = new DirectiveInfo(SingleIsValidIdCheck()),
                [StringConstants.compiler_directive_else] = new DirectiveInfo(SingleIsValidIdCheck(), paramsNums: new int[2] { 0, 1 }),
                [StringConstants.compiler_directive_undef] = new DirectiveInfo(SingleIsValidIdCheck()),
                [StringConstants.compiler_directive_define] = new DirectiveInfo(SingleIsValidIdCheck()),
                [StringConstants.compiler_directive_include] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_targetframework] = new DirectiveInfo(),
                [StringConstants.compiler_directive_hidden_idents] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_disable_standard_units] = NoParamsDirectiveInfo(),
                [StringConstants.compiler_directive_version_string] = new DirectiveInfo(IsValidVersionCheck()),
                [StringConstants.compiler_directive_product_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_company_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_copyright_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_trademark_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_main_resource_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_title_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_description_string] = new DirectiveInfo(quotesAreSpecialSymbols: true),
                [StringConstants.compiler_directive_omp] = new DirectiveInfo(SingleAnyOfCheck("critical", "parallel"), checkParamsNumNeeded: false)
            };
            #endregion
        }
    }
    #region LEGACY - C language
    /*public class CLanguageInformation : DefaultLanguageInformation
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

            //keywords.Add("", ""); keys.Add("");
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
				*//*sb.Append('<');
				for (int i=0; i<len; i++)
				{
					sb.Append('T');
					if (i<len-1)
					sb.Append(',');
				}
				sb.Append('>');*//*
				return sb.ToString();
			}
			//if (ctn.IsArray) return "array of "+GetTypeName(ctn.GetElementType());
			//if (ctn == Type.GetType("System.Void*")) return PascalABCCompiler.StringConstants.pointer_type_name;
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
	}*/
    #endregion
}

