using PascalABCCompiler.SyntaxTree;
using System;
using System.Collections.Generic;

namespace PascalABCCompiler.Contracts
{
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
}
