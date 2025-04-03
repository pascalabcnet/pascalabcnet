// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

namespace PascalABCCompiler.Parsers
{
    public enum KeywordKind
    {
        None,
        New,
        Function,
        Constructor,
        Destructor,
        Uses,
        Inherited,
        Raise,
        Type,
        Var,
        Unit,
        Of,
        As,
        Program,
        Const,
        TypeDecl,
        BinaryOperator,
        VisibleModificator,
        MethodModificator,
        PublicModificator,
        PrivateModificator,
        ProtectedModificator,
        InternalModificator,
        StaticModificator,
        VirtualModificator,
        OverrideModificator,
        ReintroduceModificator,
        AbstractModificator,
        UnaryOperator,
        ByteType,
        SByteType,
        ShortType,
        UShortType,
        IntType,
        UIntType,
        Int64Type,
        UInt64Type,
        CharType,
        DoubleType,
        FloatType,
        BoolType,
        PointerType,
        Colon,
        Punkt,
        SquareBracket,
        ThrowNew,
        CommonKeyword,
        CommonExpressionKeyword
    }

    /*public struct Keyword
    {
        public string Name { get; set; }
        public KeywordKind Kind { get; set; }
        
        public bool IsTypeKeyword { get; set; }

        public System.Enum Token { get; set; }

        public Keyword(string name, System.Enum token, KeywordKind kind = KeywordKind.None, bool isTypeKeyword = false)
        {
            Name = name;
            Token = token;
            Kind = kind;
            IsTypeKeyword = isTypeKeyword;
        }

        public override string ToString()
        {
            return Name;
        } 
    }*/
}
