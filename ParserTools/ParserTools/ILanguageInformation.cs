// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;

namespace PascalABCCompiler.Parsers
{
	/// <summary>
	/// Интерфейс, предоставляющий информацию интеллисенсу
	/// </summary>
	public interface ILanguageInformation
    {
        /// <summary>
        /// Название языка
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Версия языка
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Авторское право
        /// </summary>
        string Copyright { get; }

        /// <summary>
        /// Чувствительность к регистру
        /// </summary>
        bool CaseSensitive { get; }

        /// <summary>
        /// Расширения файлов, относящиеся к языку
        /// </summary>
        string[] FilesExtensions { get; }

        /// <summary>
        /// Названия системных модулей (стандартной библиотеки и др.)
        /// </summary>
        string[] SystemUnitNames { get; }

    	/// <summary>
    	/// Получить полное описание элемента (в желтой подсказке)
    	/// </summary>
		string GetDescription(IBaseScope scope);
        /// <summary>
        /// Получить краткое описание элемента (без ключевых слов)
        /// </summary>
        string GetSimpleDescription(IBaseScope scope);
    	/// <summary>
    	/// Получить короткое имя откомпилированного типа
    	/// </summary>
    	string GetShortName(ICompiledTypeScope scope);
        /// <summary>
        /// Получить короткое имя откомпилированного метода
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
    	string GetShortName(ICompiledMethodScope scope);
        /// <summary>
        /// Получить короткое имя откомпилированного конструктора
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
    	string GetShortName(ICompiledConstructorScope scope);
    	string GetShortName(IProcScope scope);
        string GetShortTypeName(Type t, bool noalias = true);
        /// <summary>
        /// Получить представление массива рамерности rank
        /// </summary>
        /// <param name="elementType"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
    	string GetArrayDescription(string elementType, int rank);
    	/// <summary>
    	/// Получить длинное имя откомпилированного типа
    	/// </summary>
    	string GetFullTypeName(ICompiledTypeScope scope, bool no_alias=true);
    	string GetSynonimDescription(ITypeScope scope);
    	string GetSynonimDescription(ITypeSynonimScope scope);
    	string GetSynonimDescription(IProcScope scope);
    	string GetKeyword(SymbolKind kind);
    	string ConstructOverridedMethodHeader(IProcScope scope, out int off);
    	string GetSimpleDescriptionWithoutNamespace(ITypeScope scope);
    	string GetClassKeyword(SyntaxTree.class_keyword keyw);
    	
    	string[] GetIndexerString(IBaseScope scope);
    	/// <summary>
    	/// Получить строковое представление символа
    	/// </summary>
    	string GetStringForChar(char c);
    	/// <summary>
    	/// Получить строковое представление литерального символа
    	/// </summary>
    	string GetStringForSharpChar(int num);
    	/// <summary>
    	/// Получить строковое представление строки
    	/// </summary>    	
    	string GetStringForString(string s);
    	/// <summary>
    	/// Сконструировать реализацию метода
    	/// </summary>
    	string ConstructHeader(string meth, IProcScope scope, int tabCount);
    	string ConstructHeader(IProcRealizationScope scope, int tabCount);
    	/// <summary>
    	/// Получить выражение в строке Text, до смещения off (используется при нажатии точки)
    	/// </summary>
    	string FindExpression(int off, string Text, int line, int col, out KeywordKind keyw);
    	/// <summary>
    	/// Получить выражение в строке Text, до смещения off и возможно после смещения до конца идентификатора (используется при наведении мыши)
    	/// </summary>
    	string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out KeywordKind keyw, out string expr_without_brackets);
        /// <summary>
    	/// Получить выражение в строке Text, до смещения off и возможно после смещения до конца идентификатора (используется при наведении мыши)
    	/// </summary>
    	string FindExpressionFromAnyPosition(int off, string Text, int line, int col, out string expr_without_brackets);
        /// <summary>
        /// Получить выражение в строке Text, до смещения off (используется при нажатии (, [, запятой)
        /// </summary>
        string FindExpressionForMethod(int off, string Text, int line, int col, char pressed_key, ref int num_param);
    	/// <summary>
    	/// Получить идентификатор
    	/// </summary>
    	string FindOnlyIdentifier(int off, string Text, int line, int col, ref string name);
    	string FindPattern(int off, string Text, out bool is_pattern);
    	string GetDocumentTemplate(string lineText, string Text, int line, int col, int pos);
    	/// <summary>
    	/// Проверить на ключевое слово
    	/// </summary>
    	KeywordKind TestForKeyword(string Text, int i);
    	KeywordKind GetKeywordKind(string name);
    	/*/// <summary>
    	/// Получить все ключевые слова
    	/// </summary>
    	string[] GetKeywords();
    	/// <summary>
    	/// Получить все ключевые слова такого рода (var, type,...), т.е. при вводе идентификатора после них не выдавать всплывающую подсказку 
    	/// </summary>
    	string[] GetTypeKeywords();*/
    	bool IsDefinitionIdentifierAfterKeyword(KeywordKind keyw);
    	/// <summary>
    	/// Является ли символ разделителем при вызове функции
    	/// </summary>
    	bool IsMethodCallParameterSeparator(char key);
    	/// <summary>
    	/// Является ли символ открывающей скобкой при вызове функции
    	/// </summary>
    	bool IsOpenBracketForMethodCall(char key);
    	bool IsOpenBracketForIndex(char key);
    	bool IsTypeAfterKeyword(KeywordKind keyw);
    	bool IsNamespaceAfterKeyword(KeywordKind keyw);
    	string GetStandardTypeByKeyword(KeywordKind keyw);
    	/*string GetSystemUnitName();
    	string GetBodyStartBracket();
    	string GetBodyEndBracket();*/
    	string SkipNew(int off, string Text, ref KeywordKind keyw);
    	//char GetParameterDelimiter();
    	string GetCompiledTypeRepresentation(Type t, System.Reflection.MemberInfo mi, ref int line, ref int col);
        bool IsKeyword(string value);

        void RenameOrExcludeSpecialNames(SymInfo[] symInfos);

        bool IsParams(string paramDescription);

        int FindClosingParenthesis(string descriptionAfterOpeningParenthesis, char parenthesis);

        int FindParamDelim(string descriptionAfterOpeningParenthesis, int number);

        int FindParamDelimForIndexer(string descriptionAfterOpeningParenthesis, int number);

        /// <summary>
        /// Нужно ли вызывать конверторы синтаксического дерева, срабатывающие после компиляции зависимостей
        /// </summary>
        bool SyntaxTreeIsConvertedAfterUsedModulesCompilation { get; }

        /// <summary>
        /// Вызывать ли преобразователей синтаксического дерева в работе Intellisense
        /// </summary>
        bool ApplySyntaxTreeConvertersForIntellisense { get; }

        Dictionary<string, string> SpecialModulesAliases { get; }

        BaseKeywords KeywordsStorage
        {
            get;
        }

        /// <summary>
        /// Данные о всех поддерживаемых директивах компилятора
        /// </summary>
        Dictionary<string, ParserTools.Directives.DirectiveInfo> ValidDirectives { get; }

        string BodyStartBracket
        {
            get;
        }
        string BodyEndBracket
        {
            get;
        }
        string ParameterDelimiter
        {
            get;
        }
        string DelimiterInIndexer
        { 
            get; 
        }
        string ResultVariableName
        {
            get;
        }
        bool IncludeDotNetEntities
        {
            get;
        }
        bool AddStandardUnitNamesToUserScope
        {
            get;
        }

        /// <summary>
        /// Добавить пространства имен .NET, подключаемые по умолчанию. Полной поддержки false нет (см. комментарий)
        /// </summary>
        // Если поставить false, то CorrectTreeWithSemantic из DomSyntaxTreeVisitor будет поправлять типы неправильно
        // из-за того, что полное имя типа integer, например, System.Int32,
        // а имя System не будет добавлено в Scope и значит выведется Int32 без преобразования в кастомное название типа для языка
        bool AddStandardNetNamespacesToUserScope
        {
            get;
        }

        /// <summary>
        /// Может ли source context функций быть вложен в source context других функций
        /// </summary>
        bool UsesFunctionsOverlappingSourceContext
        {
            get;
        }
    }

    /*public interface ICodeFormatter
    {
        string FormatTree(string Text, compilation_unit cu, int cursor_line, int cursor_col);
    }*/

    public struct Position
    {
        public int line;
        public int column;
        public int end_line;
        public int end_column;
        public string file_name;
        public bool from_metadata;
        public string metadata;
        public string metadata_title;
        public string full_metadata_title;
        public string fold_text;
        public MetadataType metadata_type;

        public Position(int line, int column, int end_line, int end_column, string file_name)
        {
            this.line = line;
            this.column = column;
            this.end_line = end_line;
            this.end_column = end_column;
            this.file_name = file_name;
            this.from_metadata = false;
            this.metadata = null;
            this.metadata_title = null;
            this.fold_text = null;
            this.metadata_type = MetadataType.Unknown;
            this.full_metadata_title = null;
        }

        public Position(int line, int column, int end_line, int end_column, string file_name, string fold_text)
        {
            this.line = line;
            this.column = column;
            this.end_line = end_line;
            this.end_column = end_column;
            this.file_name = file_name;
            this.from_metadata = false;
            this.metadata = null;
            this.metadata_title = null;
            this.fold_text = fold_text;
            this.metadata_type = MetadataType.Unknown;
            this.full_metadata_title = null;
        }
    }

	public enum ScopeKind
	{
		None,
		Procedure,
		CompiledMethod,
		Type,
		CompiledType,
		Array,
		Set,
		ShortString,
		Enum,
		CompiledProperty,
		CompiledField,
		CompiledEvent,
		CompiledConstructor,
		Namespace,
		Pointer,
		ProcRealization,
		TypeSynonim,
		Diapason,
		Block,
		UnitInterface,
		UnitImplementation,
		File,
		ElementScope,
		Delegate,
		NamespaceTypeScope
	}

    public enum MetadataType
    {
        Field,
        Method,
        Constructor,
        Class,
        Interface,
        Property,
        Struct,
        Event,
        EnumerationMember,
        Delegate,
        Enumeration,
        Unknown
    }

	public enum SymKind
	{
		Field,
		Method,
		Property,
		Event,
		Block,
		Type,
		Namespace,
		Struct,
		Interface,
		Delegate,
		Enum,
		Variable,
		Parameter,
		Constant,
		Class,
		Null
	}

    /*public interface ISymInfo
    {
        string Name
        {
            get;
        }

        string AdditionalName
        {
            get;
        }

        string Description
        {
            get;
        }

        SymbolKind Kind
        {
            get;
        }

        bool IsUnitNamespace = false;
        
        access_modifer AccessModifier
        {
            get;
        }

        bool HasDocumentation
        {
            get;
        }

        bool NotInclude
        {
            get;
        }
    }*/

    public enum SymbolKind
    {
        Field,
        Method,
        Property,
        Event,
        Block,
        Type,
        Namespace,
        Struct,
        Interface,
        Delegate,
        Enum,
        Variable,
        Parameter,
        Constant,
        Class,
        GenericIndicator,
        Null
    }

    public class SymInfo
    {
        public string name;
        public string addit_name;
        public string aliasName;
        public string description;
        public SymbolKind kind;
        public bool IsUnitNamespace;
        public access_modifer acc_mod;
        public bool is_static;
        public bool has_doc;
        public bool not_include;

        public SymInfo(string name, SymbolKind kind, string description)
        {
            this.name = name;
            this.kind = kind;
            this.description = description;
        }

        public SymInfo(SymInfo si)
        {
            this.name = si.name;
            this.addit_name = si.addit_name;
            this.aliasName = si.aliasName;
            this.description = si.description;
            this.kind = si.kind;
            this.IsUnitNamespace = si.IsUnitNamespace;
            this.acc_mod = si.acc_mod;
            this.has_doc = si.has_doc;
            this.not_include = si.not_include;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public SymbolKind Kind
        {
            get
            {
                return kind;
            }
            set
            {
                kind = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
    }

    public interface IBaseScope
    {
    	ScopeKind Kind
    	{
    		get;
    	}
    	
    	SymbolKind ElemKind
    	{
    		get;
    	}
    	
    	string Name
    	{
    		get;
    	}
    	
    	string Description
    	{
    		get;
    	}
    	
    	PascalABCCompiler.SyntaxTree.access_modifer AccessModifier
    	{
    		get;
    	}
    	
    	IBaseScope TopScope
    	{
    		get;
    	}

        SymInfo SymbolInfo
        {
            get;
        }

        IList<Position> Regions
        {
            get;
        }

        IBaseScope[] Members
        {
            get;
        }

        bool IsEqual(IBaseScope scope);
        ITypeScope GetElementType();
        Position GetHeaderPosition();
        Position GetBodyPosition();
        Position GetPosition();
        string GetDescriptionWithoutDoc();
        IBaseScope FindScopeByLocation(int line, int column);
        IBaseScope FindNameInAnyOrder(string name);
        IBaseScope FindNameOnlyInType(string name);
        IElementScope MakeElementScope();
    }

    public interface IImplementationUnitScope : IBaseScope
    {
    }

    public interface IProcScope : IBaseScope
    {
    	bool IsVirtual
    	{
    		get;
    	}
    	
    	bool IsOverride
    	{
    		get;
    	}
    	
    	bool IsStatic
    	{
    		get;
    	}
    	
    	bool IsAbstract
    	{
    		get;
    	}

        bool IsExtension
        {
            get;
        }

    	bool IsReintroduce
    	{
    		get;
    	}
    	
    	bool IsConstructor();
    	
 		IProcRealizationScope Realization
 		{
 			get;
 		}
 		
        ITypeScope DeclaringType
        {
            get;
        }

    	IElementScope[] Parameters
    	{
    		get;
    	}
    	
    	string[] TemplateParameters
    	{
    		get;
    	}
    	
    	ITypeScope ReturnType
    	{
    		get;
    	}
        IProcScope NextFunction
        {
            get;
        }
    }

    public interface IDOMService
    {
        IElementScope NewElementScope(IBaseScope scope);
    }

    public class DOMService
    {
        IDOMService service;
        static DOMService instance;

        public IDOMService Service
        {
            get
            {
                return service;
            }
        }

        public static DOMService Instance
        {
            get
            {
                if (instance == null)
                    instance = new DOMService();
                return instance;
            }
        }
    }

    public interface IElementScope : IBaseScope
    {
    	IBaseScope Type
    	{
    		get;
    	}
    	
    	PascalABCCompiler.SyntaxTree.parametr_kind ParamKind
    	{
    		get;
    	}
    	
    	object ConstantValue
    	{
    		get;
    	}
    	
    	ITypeScope ElementType
    	{
    		get;
    	}
    	
    	ITypeScope[] Indexers
    	{
    		get;
    	}
    	
    	bool IsVirtual
    	{
    		get;
    	}
    	
    	bool IsOverride
    	{
    		get;
    	}
    	
    	bool IsStatic
    	{
    		get;
    	}
    	
    	bool IsAbstract
    	{
    		get;
    	}
    	
    	bool IsReintroduce
    	{
    		get;
    	}
    	
    	bool IsReadOnly
    	{
    		get;
    	}

        
    }
    
    public interface ITypeScope : IBaseScope
    {
    	IBaseScope BaseType
    	{
    		get;
    	}
    	
    	ITypeScope ElementType
    	{
    		get;
    	}
    	
    	ITypeScope[] Indexers
    	{
    		get;
    	}

        ITypeScope[] StaticIndexers
        {
            get;
        }

        ITypeScope[] GenericInstances
    	{
    		get;
    	}
    	
    	bool Aliased
    	{
    		get;
    	}
    	
    	bool IsFinal
    	{
    		get;
    	}

        bool IsAbstract
        {
            get;
        }

        bool IsStatic
        {
            get;
        }

        string[] TemplateArguments
    	{
    		get;
    	}

        IProcScope FindExtensionMethod(string name);
        IProcScope GetConstructor();
    }
    
    public interface ICompiledTypeScope : ITypeScope
    {
    	Type CompiledType
    	{
    		get;
    	}

        ICompiledTypeScope[] GetCompiledGenericArguments();
    }
    
    public interface ICompiledMethodScope : IProcScope
    {
    	System.Reflection.MethodInfo CompiledMethod
    	{
    		get;
    	}
    	
    	List<string> GenericArgs
		{
			get;
		}
    	
    	bool IsGlobal
    	{
    		get;
    	}
    }
    
    public interface ICompiledFieldScope : IElementScope
    {
    	System.Reflection.FieldInfo CompiledField
    	{
    		get;
    	}
    	
    	List<string> GenericArgs
		{
			get;
		}
    	
    	bool IsGlobal
    	{
    		get;
    	}
    }
    
    public interface IProcType : ITypeScope
    {
    	IProcScope Target
    	{
    		get;
    	}
    }
    
    public interface ICompiledPropertyScope : IElementScope
    {
    	System.Reflection.PropertyInfo CompiledProperty
    	{
    		get;
    	}
    	
    	List<string> GenericArgs
		{
			get;
		}
    }
    
    public interface ICompiledEventScope : IElementScope
    {
    	System.Reflection.EventInfo CompiledEvent
    	{
    		get;
    	}
    	
    	List<string> GenericArgs
		{
			get;
		}
    }
    
    public interface IArrayScope : ITypeScope
    {	
    	bool IsDynamic
    	{
    		get;
    	}
    }
    
    public interface IFileScope : ITypeScope
    {
    	
    }
    
    public interface ISetScope : ITypeScope
    {
    	
    }
    
    public interface IDiapasonScope : ITypeScope
    {
    	object Left
    	{
    		get;
    	}
    	
    	object Right
    	{
    		get;
    	}
    }
    
    public interface IEnumScope : ITypeScope
    {
    	string[] EnumConsts
    	{
    		get;
    	}
    }
    
    public interface IPointerScope : ITypeScope
    {
    }
    
    public interface INamespaceScope : IBaseScope
    {
    	
    }
    
    public interface INamespaceTypeScope : IBaseScope
    {
    	
    }
    
    public interface IProcRealizationScope : IProcScope
    {
    	IProcScope DefProc
    	{
    		get;
    	}
    }
    
    public interface IInterfaceUnitScope : IBaseScope
    {
        IImplementationUnitScope ImplementationUnitScope
        {
            get;
        }

        bool IsNamespaceUnit
        {
            get;
        }
    }
    
    public interface IShortStringScope : ITypeScope
    {
    	object Length
    	{
    		get;
    	}
    }
    
    public interface ITypeSynonimScope : ITypeScope
    {
    	ITypeScope ActType
    	{
    		get;
    	}
    }
    
    public interface ICompiledConstructorScope : IProcScope
    {
    	System.Reflection.ConstructorInfo CompiledConstructor
    	{
    		get;
    	}
    }
    
    public interface IBlockScope : IBaseScope
    {
    	
    }
    
    public interface ITemplateParameterScope : ITypeScope
    {
    	
    }

    public interface ICodeCompletionDomConverter
    {
        bool IsCompiled
        {
            get;
        }
        IBaseScope EntryScope
        {
            get;
        }
    }
}

