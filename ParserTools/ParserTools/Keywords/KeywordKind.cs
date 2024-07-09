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

    /*public class Keyword
    {
        string _name;
        KeywordKind _kind = KeywordKind.None;
        public string Name
        {
            get
            {
                return _name;
            }
        }
        KeywordKind Kind
        {
            get
            {
                return _kind;
            }
        }
        public Keyword(string name, KeywordKind kind)
        {
            _name = name;
            _kind = kind;
        }
        public Keyword(string name)
        {
            _name = name;
        }
        public override string ToString()
        {
            return Name;
        } 
    }*/
}
