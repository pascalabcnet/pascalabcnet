// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
using PascalABCCompiler.Parsers;

namespace Languages.Pascal.Frontend.Core
{

    public class PascalABCKeywords : BaseKeywords
    {

        protected override string FileName => "keywordsmap.pabc";

        public PascalABCKeywords() : base(false)
        {
            #region Keywords Initialization
            CreateNewKeyword("or", Tokens.tkOr);
            CreateNewKeyword("xor", Tokens.tkXor);
            CreateNewKeyword("and", Tokens.tkAnd);
            CreateNewKeyword("div", Tokens.tkDiv);
            CreateNewKeyword("mod", Tokens.tkMod);
            CreateNewKeyword("shl", Tokens.tkShl);
            CreateNewKeyword("shr", Tokens.tkShr);
            CreateNewKeyword("not", Tokens.tkNot);
            CreateNewKeyword("as", Tokens.tkAs);
            CreateNewKeyword("in", Tokens.tkIn);
            CreateNewKeyword("is", Tokens.tkIs);
            CreateNewKeyword("implicit", Tokens.tkImplicit);
            CreateNewKeyword("explicit", Tokens.tkExplicit);
            CreateNewKeyword("sizeof", Tokens.tkSizeOf);
            CreateNewKeyword("typeof", Tokens.tkTypeOf);
            CreateNewKeyword("where", Tokens.tkWhere);
            CreateNewKeyword("array", Tokens.tkArray, isTypeKeyword: true);
            CreateNewKeyword("begin", Tokens.tkBegin);
            CreateNewKeyword("case", Tokens.tkCase);
            CreateNewKeyword("class", Tokens.tkClass, isTypeKeyword: true);
            CreateNewKeyword("const", Tokens.tkConst, KeywordKind.Const);
            CreateNewKeyword("constructor", Tokens.tkConstructor, KeywordKind.Constructor);
            CreateNewKeyword("default", Tokens.tkDefault);
            CreateNewKeyword("destructor", Tokens.tkDestructor, KeywordKind.Destructor);
            CreateNewKeyword("downto", Tokens.tkDownto);
            CreateNewKeyword("do", Tokens.tkDo);
            CreateNewKeyword("else", Tokens.tkElse);
            CreateNewKeyword("end", Tokens.tkEnd);
            CreateNewKeyword("event", Tokens.tkEvent);
            CreateNewKeyword("except", Tokens.tkExcept);
            CreateNewKeyword("exports", Tokens.tkExports);
            CreateNewKeyword("file", Tokens.tkFile, isTypeKeyword : true);
            CreateNewKeyword("finalization", Tokens.tkFinalization);
            CreateNewKeyword("finally", Tokens.tkFinally);
            CreateNewKeyword("for", Tokens.tkFor);
            CreateNewKeyword("foreach", Tokens.tkForeach);
            CreateNewKeyword("function", Tokens.tkFunction, KeywordKind.Function, true);
            CreateNewKeyword("goto", Tokens.tkGoto);
            CreateNewKeyword("if", Tokens.tkIf);
            CreateNewKeyword("implementation", Tokens.tkImplementation);
            CreateNewKeyword("inherited", Tokens.tkInherited, KeywordKind.Inherited);
            CreateNewKeyword("initialization", Tokens.tkInitialization);
            CreateNewKeyword("interface", Tokens.tkInterface);
            CreateNewKeyword("label", Tokens.tkLabel);
            CreateNewKeyword("lock", Tokens.tkLock);
            CreateNewKeyword("loop", Tokens.tkLoop);
            CreateNewKeyword("nil", Tokens.tkNil);
            CreateNewKeyword("procedure", Tokens.tkProcedure, KeywordKind.Function, true);
            CreateNewKeyword("of", Tokens.tkOf, KeywordKind.Of);
            CreateNewKeyword("operator", Tokens.tkOperator);
            CreateNewKeyword("property", Tokens.tkProperty);
            CreateNewKeyword("raise", Tokens.tkRaise, KeywordKind.Raise);
            CreateNewKeyword("record", Tokens.tkRecord, isTypeKeyword: true);
            CreateNewKeyword("repeat", Tokens.tkRepeat);
            CreateNewKeyword("set", Tokens.tkSet, isTypeKeyword : true);
            CreateNewKeyword("try", Tokens.tkTry);
            CreateNewKeyword("type", Tokens.tkType, KeywordKind.Type);
            CreateNewKeyword("then", Tokens.tkThen);
            CreateNewKeyword("to", Tokens.tkTo);
            CreateNewKeyword("until", Tokens.tkUntil);
            CreateNewKeyword("uses", Tokens.tkUses, KeywordKind.Uses);
            CreateNewKeyword("var", Tokens.tkVar, KeywordKind.Var);
            CreateNewKeyword("while", Tokens.tkWhile);
            CreateNewKeyword("with", Tokens.tkWith);
            CreateNewKeyword("program", Tokens.tkProgram, KeywordKind.Program);
            CreateNewKeyword("template", Tokens.tkTemplate);
            CreateNewKeyword("resourcestring", Tokens.tkResourceString);
            CreateNewKeyword("threadvar", Tokens.tkThreadvar);
            CreateNewKeyword("sealed", Tokens.tkSealed);
            CreateNewKeyword("partial", Tokens.tkPartial);
            CreateNewKeyword("params", Tokens.tkParams);
            CreateNewKeyword("unit", Tokens.tkUnit, KeywordKind.Unit);
            CreateNewKeyword("library", Tokens.tkLibrary);
            CreateNewKeyword("external", Tokens.tkExternal);
            CreateNewKeyword("name", Tokens.tkName);
            CreateNewKeyword("private", Tokens.tkPrivate);
            CreateNewKeyword("protected", Tokens.tkProtected);
            CreateNewKeyword("public", Tokens.tkPublic);
            CreateNewKeyword("internal", Tokens.tkInternal);
            CreateNewKeyword("read", Tokens.tkRead);
            CreateNewKeyword("write", Tokens.tkWrite);
            CreateNewKeyword("on", Tokens.tkOn);
            CreateNewKeyword("forward", Tokens.tkForward);
            CreateNewKeyword("abstract", Tokens.tkAbstract);
            CreateNewKeyword("overload", Tokens.tkOverload);
            CreateNewKeyword("reintroduce", Tokens.tkReintroduce);
            CreateNewKeyword("override", Tokens.tkOverride);
            CreateNewKeyword("virtual", Tokens.tkVirtual);
            CreateNewKeyword("extensionmethod", Tokens.tkExtensionMethod);
            CreateNewKeyword("new", Tokens.tkNew, KeywordKind.New);
            CreateNewKeyword("auto", Tokens.tkAuto);
            CreateNewKeyword("sequence", Tokens.tkSequence);
            CreateNewKeyword("yield", Tokens.tkYield);
            CreateNewKeyword("match", Tokens.tkMatch);
            CreateNewKeyword("when", Tokens.tkWhen);
            CreateNewKeyword("namespace", Tokens.tkNamespace);
            CreateNewKeyword("static", Tokens.tkStatic);
            CreateNewKeyword("step", Tokens.tkStep);
            CreateNewKeyword("index", Tokens.tkIndex);
            CreateNewKeyword("async", Tokens.tkAsync);
            CreateNewKeyword("await", Tokens.tkAwait);
            #endregion
        }

        protected override int GetIdToken() => (int)Tokens.tkIdentifier;
    }

}
