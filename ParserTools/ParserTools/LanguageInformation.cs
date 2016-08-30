﻿// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using System;
using System.Text;
using System.Collections.Generic;
using PascalABCCompiler.SyntaxTree;
using PascalABCCompiler.Errors;

namespace PascalABCCompiler.Parsers
{
	/// <summary>
	/// Интерфейс, предоставляющий информацию интеллисенсу
	/// </summary>
	public interface ILanguageInformation
    {
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
    	string GetShortTypeName(ICompiledTypeScope scope);
        /// <summary>
        /// Получить короткое имя откомпилированного метода
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
    	string GetShortTypeName(ICompiledMethodScope scope);
        /// <summary>
        /// Получить короткое имя откомпилированного конструктора
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
    	string GetShortTypeName(ICompiledConstructorScope scope);
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
        string[] Keywords
        {
            get;
        }
        string[] TypeKeywords
        {
            get;
        }
        string SystemUnitName
        {
            get;
        }
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
        bool CaseSensitive
        {
            get;
        }
        bool IncludeDotNetEntities
        {
            get;
        }
    }

    public interface ICodeFormatter
    {
        string FormatTree(string Text, compilation_unit cu, int cursor_line, int cursor_col);
    }

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
        public string description;
        public SymbolKind kind;
        public bool IsUnitNamespace = false;
        public access_modifer acc_mod;
        public bool has_doc = false;
        public bool not_include = false;

        public SymInfo(string name, SymbolKind kind, string description)
        {
            this.name = name;
            this.kind = kind;
            this.description = description;
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
                return Instance;
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
    	
    	string[] TemplateArguments
    	{
    		get;
    	}

        IProcScope GetConstructor();
    }
    
    public interface ICompiledTypeScope : ITypeScope
    {
    	Type CompiledType
    	{
    		get;
    	}
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
    	ITypeScope[] Indexers
    	{
    		get;
    	}
    	
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
}

