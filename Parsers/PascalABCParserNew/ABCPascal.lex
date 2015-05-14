%{
    public PT parsertools;
%}

%namespace GPPGParserScanner

%using PascalABCNewParser;
%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;
%using QUT.Gppg;

Letter [a-zA-Z_]
Digit [0-9]
LetterDigit {Letter}|{Digit}
ID {Letter}{LetterDigit}* 
HexDigit {Digit}|[abcdefABCDEF]
DotChr [^\r\n]
OneLineCmnt  \/\/{DotChr}*

CHARACTERNUM '[^'\n]'
INTNUM {Digit}+
FLOATNUM {INTNUM}\.{INTNUM}
EXPNUM ({INTNUM}\.)?{INTNUM}[eE][+\-]?{INTNUM}
STRINGNUM \'([^\'\n]|\'\')*\'
HEXNUM ${HexDigit}+

%x COMMENT
%x COMMENT1

%%



"{" { 
  BEGIN(COMMENT);
}

<COMMENT> "}" { 
  BEGIN(INITIAL);
}

<COMMENT>.|\n {
}

"(*" { 
  BEGIN(COMMENT1);
}

<COMMENT1> "*)" { 
  BEGIN(INITIAL);
}

<COMMENT1>.|\n {
}

"&"              { return (int)Tokens.tkAmpersend; }
","              { yylval = new Union(); yylval.ti = new token_info(yytext); return (int)Tokens.tkComma; }
":"              { return (int)Tokens.tkColon; }
".."             { return (int)Tokens.tkDotDot; }
"."              { return (int)Tokens.tkPoint; }
"("              { return (int)Tokens.tkRoundOpen; }
")"              { return (int)Tokens.tkRoundClose; }
";"              { return (int)Tokens.tkSemiColon; }
"["              { return (int)Tokens.tkSquareOpen; }
"]"              { return (int)Tokens.tkSquareClose; }
"?"              { return (int)Tokens.tkQuestion; }
"@"              { yylval = new Union(); yylval.op = new op_type_node(Operators.AddressOf); return (int)Tokens.tkAddressOf; }
":="            { yylval = new Union(); yylval.op = new op_type_node(Operators.Assignment); return (int)Tokens.tkAssign; }
"+="            { yylval = new Union(); yylval.op = new op_type_node(Operators.AssignmentAddition); return (int)Tokens.tkPlusEqual; }
"-="            { yylval = new Union(); yylval.op = new op_type_node(Operators.AssignmentSubtraction); return (int)Tokens.tkMinusEqual; }
"*="            { yylval = new Union(); yylval.op = new op_type_node(Operators.AssignmentMultiplication); return (int)Tokens.tkMultEqual; }
"/="            { yylval = new Union(); yylval.op = new op_type_node(Operators.AssignmentDivision); return (int)Tokens.tkDivEqual; }
"-"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Minus); return (int)Tokens.tkMinus; }
"+"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Plus); return (int)Tokens.tkPlus; }
"/"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Division); return (int)Tokens.tkSlash; }
"*"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Multiplication); return (int)Tokens.tkStar; }
"="             { yylval = new Union(); yylval.op = new op_type_node(Operators.Equal); return (int)Tokens.tkEqual; }
">"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Greater); return (int)Tokens.tkGreater; }
">="            { yylval = new Union(); yylval.op = new op_type_node(Operators.GreaterEqual); return (int)Tokens.tkGreaterEqual; }
"<"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Less); return (int)Tokens.tkLower; }
"<="            { yylval = new Union(); yylval.op = new op_type_node(Operators.LessEqual); return (int)Tokens.tkLowerEqual; }
"<>"            { yylval = new Union(); yylval.op = new op_type_node(Operators.NotEqual); return (int)Tokens.tkNotEqual; }
"^"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Deref); return (int)Tokens.tkDeref; }
"->"            { yylval = new Union(); yylval.ti = new token_info(yytext); return (int)Tokens.tkArrow; }
\<\<expression\>\> { return (int)Tokens.tkParseModeExpression; }
\<\<statement\>\>  { return (int)Tokens.tkParseModeStatement; }

\x01 { return (int)Tokens.INVISIBLE; }

{OneLineCmnt} {

}

[&]?{ID}  { 
  int res = Keywords.KeywordOrIDToken(yytext);
  if (res == (int)Tokens.tkIdentifier)
  {
	yylval = new Union(); 
    yylval.id = parsertools.create_ident(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol));
  }	
  else 
	switch (res)
    {
    case (int)Tokens.tkOr:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalOR);
        break;
    case (int)Tokens.tkXor:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseXOR);
        break;
    case (int)Tokens.tkAnd:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalAND);
        break;
    case (int)Tokens.tkDiv:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.IntegerDivision);
        break;
    case (int)Tokens.tkMod:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.ModulusRemainder);
        break;
    case (int)Tokens.tkShl:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseLeftShift);
        break;
    case (int)Tokens.tkShr:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseRightShift);
        break;
    case (int)Tokens.tkNot:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalNOT);
        break;
    case (int)Tokens.tkAs:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.As);
        break;
    case (int)Tokens.tkIn:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.In);
        break;
    case (int)Tokens.tkIs:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Is);
        break;
    case (int)Tokens.tkImplicit:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Implicit);
        break;
    case (int)Tokens.tkExplicit:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Explicit);
        break;
    case (int)Tokens.tkSizeOf:
    case (int)Tokens.tkTypeOf:
    case (int)Tokens.tkWhere:
    case (int)Tokens.tkArray:
    case (int)Tokens.tkCase:
    case (int)Tokens.tkClass:
    case (int)Tokens.tkConst:
    case (int)Tokens.tkConstructor:
    case (int)Tokens.tkDestructor:
    case (int)Tokens.tkDo:
    case (int)Tokens.tkElse:
    case (int)Tokens.tkExcept:
    case (int)Tokens.tkFile:
    case (int)Tokens.tkFinalization:
    case (int)Tokens.tkFinally:
    case (int)Tokens.tkFor:
    case (int)Tokens.tkForeach:
    case (int)Tokens.tkFunction:
    case (int)Tokens.tkIf:
    case (int)Tokens.tkImplementation:
    case (int)Tokens.tkInherited:
    case (int)Tokens.tkInitialization:
    case (int)Tokens.tkInterface:
    case (int)Tokens.tkProcedure:
    case (int)Tokens.tkOperator:
    case (int)Tokens.tkProperty:
    case (int)Tokens.tkRaise:
    case (int)Tokens.tkRecord:
    case (int)Tokens.tkRepeat:
    case (int)Tokens.tkSet:
    case (int)Tokens.tkType:
    case (int)Tokens.tkThen:
    case (int)Tokens.tkUntil:
    case (int)Tokens.tkUses:
    case (int)Tokens.tkVar:
    case (int)Tokens.tkWhile:
    case (int)Tokens.tkWith:
    case (int)Tokens.tkNil:
    case (int)Tokens.tkGoto:
    case (int)Tokens.tkOf:
    case (int)Tokens.tkLabel:
    case (int)Tokens.tkLock:
    case (int)Tokens.tkProgram:
    case (int)Tokens.tkEvent:
    case (int)Tokens.tkDefault:
    case (int)Tokens.tkTemplate:
    case (int)Tokens.tkPacked:
    case (int)Tokens.tkInline:
    case (int)Tokens.tkExports:
    case (int)Tokens.tkResourceString:
    case (int)Tokens.tkThreadvar:
    case (int)Tokens.tkFinal:
    case (int)Tokens.tkVariant:
    case (int)Tokens.tkOleVariant:
    case (int)Tokens.tkParams:
    case (int)Tokens.tkTo:
    case (int)Tokens.tkDownto:
		yylval = new Union();
        yylval.ti = new token_info(yytext);
        break;
    case (int)Tokens.tkBegin:
    case (int)Tokens.tkEnd:
    case (int)Tokens.tkTry:
		yylval = new Union();
        yylval.ti = new token_info(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol));
        break;
    case (int)Tokens.tkAt:
    case (int)Tokens.tkOn:
    case (int)Tokens.tkContains:
    case (int)Tokens.tkUnit:
    case (int)Tokens.tkLibrary:
    case (int)Tokens.tkOut:
    case (int)Tokens.tkPackage:
    case (int)Tokens.tkRequires:
    case (int)Tokens.tkShortInt:
    case (int)Tokens.tkSmallInt:
    case (int)Tokens.tkOrdInteger:
    case (int)Tokens.tkByte:
    case (int)Tokens.tkLongInt:
    case (int)Tokens.tkInt64:
    case (int)Tokens.tkWord:
    case (int)Tokens.tkBoolean:
    case (int)Tokens.tkChar:
    case (int)Tokens.tkWideChar:
    case (int)Tokens.tkLongWord:
    case (int)Tokens.tkPChar:
    case (int)Tokens.tkCardinal:
    case (int)Tokens.tkReal:
    case (int)Tokens.tkSingle:
    case (int)Tokens.tkDouble:
    case (int)Tokens.tkExtended:
    case (int)Tokens.tkComp:
    case (int)Tokens.tkAbsolute:
    case (int)Tokens.tkAssembler:
    case (int)Tokens.tkAutomated:
    case (int)Tokens.tkDispid:
    case (int)Tokens.tkExternal:
    case (int)Tokens.tkImplements:
    case (int)Tokens.tkIndex:
    case (int)Tokens.tkMessage:
    case (int)Tokens.tkName:
    case (int)Tokens.tkNodefault:
    case (int)Tokens.tkPrivate:
    case (int)Tokens.tkProtected:
    case (int)Tokens.tkPublic:
    case (int)Tokens.tkInternal:
    case (int)Tokens.tkRead:
    case (int)Tokens.tkResident:
    case (int)Tokens.tkStored:
    case (int)Tokens.tkWrite:
    case (int)Tokens.tkReadOnly:
    case (int)Tokens.tkWriteOnly:
		yylval = new Union(); 
        yylval.id = new ident(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol));
        break;
    case (int)Tokens.tkAbstract:
		yylval = new Union(); 
        yylval.pat = new procedure_attribute(proc_attribute.attr_abstract,new LexLocation(tokLin, tokCol, tokELin, tokECol));
        break;
    case (int)Tokens.tkForward:
		yylval = new Union(); 
        yylval.pat = new procedure_attribute(proc_attribute.attr_forward,new LexLocation(tokLin, tokCol, tokELin, tokECol));
        break;
    case (int)Tokens.tkOverload:
		yylval = new Union(); 
        yylval.pat = new procedure_attribute(proc_attribute.attr_overload,new LexLocation(tokLin, tokCol, tokELin, tokECol));
        break;
    case (int)Tokens.tkReintroduce:
		yylval = new Union(); 
        yylval.pat = new procedure_attribute(proc_attribute.attr_reintroduce,new LexLocation(tokLin, tokCol, tokELin, tokECol));
        break;
    case (int)Tokens.tkOverride:
		yylval = new Union(); 
        yylval.pat = new procedure_attribute(proc_attribute.attr_override,new LexLocation(tokLin, tokCol, tokELin, tokECol));
        break;
    case (int)Tokens.tkVirtual:
		yylval = new Union(); 
        yylval.pat = new procedure_attribute(proc_attribute.attr_virtual,new LexLocation(tokLin, tokCol, tokELin, tokECol));
        break;
    }
  return res;
}

{INTNUM} { 
  yylval = new Union(); 
  yylval.con = parsertools.create_int_const(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol)); 
  return (int)Tokens.tkInteger; 
}

{HEXNUM} { 
  yylval = new Union(); 
  yylval.con = parsertools.create_hex_const(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol));
  return (int)Tokens.tkHex; 
}

{FLOATNUM} |
{EXPNUM} {
  yylval = new Union(); 
  yylval.con = parsertools.create_double_const(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol));
  return (int)Tokens.tkFloat;
}

{STRINGNUM} { 
  yylval = new Union(); 
  yylval.con = parsertools.create_string_const(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol)); 
  return (int)Tokens.tkStringLiteral; 
}

'#'{Digit}+ {
  yylval = new Union(); 
  yylval.con = parsertools.create_string_const(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol)); 
  return (int)Tokens.tkAsciiChar; 
}

'#'{ID} {
  yylval = new Union(); 
  yylval.id = parsertools.create_directive_name(yytext,new LexLocation(tokLin, tokCol, tokELin, tokECol));
  return (int)Tokens.tkDirectiveName;
}


%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%

  public override void yyerror(string format, params object[] args) 
  {
    string errorMsg = parsertools.CreateErrorString(args);
    parsertools.AddError(errorMsg,new LexLocation(tokLin, tokCol, tokELin, tokECol));
  }
  
// Статический класс, определяющий ключевые слова языка
public static class Keywords
{
	private static Dictionary<string, int> keywords = new Dictionary<string, int>();

	static Keywords()
	{
        keywords.Add("OR",(int)Tokens.tkOr);
        keywords.Add("XOR",(int)Tokens.tkXor);
        keywords.Add("AND",(int)Tokens.tkAnd);
        keywords.Add("DIV",(int)Tokens.tkDiv);
        keywords.Add("MOD",(int)Tokens.tkMod);
        keywords.Add("SHL",(int)Tokens.tkShl);
        keywords.Add("SHR",(int)Tokens.tkShr);
        keywords.Add("NOT",(int)Tokens.tkNot);
        keywords.Add("AS",(int)Tokens.tkAs);
        keywords.Add("IN",(int)Tokens.tkIn);
        keywords.Add("IS",(int)Tokens.tkIs);
        keywords.Add("IMPLICIT",(int)Tokens.tkImplicit);
        keywords.Add("EXPLICIT",(int)Tokens.tkExplicit);
        keywords.Add("SIZEOF",(int)Tokens.tkSizeOf);
        keywords.Add("TYPEOF",(int)Tokens.tkTypeOf);
        keywords.Add("WHERE",(int)Tokens.tkWhere);
        keywords.Add("ARRAY",(int)Tokens.tkArray);
        keywords.Add("BEGIN",(int)Tokens.tkBegin);
        keywords.Add("CASE",(int)Tokens.tkCase);
        keywords.Add("CLASS",(int)Tokens.tkClass);
        keywords.Add("CONST",(int)Tokens.tkConst);
        keywords.Add("CONSTRUCTOR",(int)Tokens.tkConstructor);
        keywords.Add("DESTRUCTOR",(int)Tokens.tkDestructor);
        keywords.Add("DOWNTO",(int)Tokens.tkDownto);
        keywords.Add("DO",(int)Tokens.tkDo);
        keywords.Add("ELSE",(int)Tokens.tkElse);
        keywords.Add("END",(int)Tokens.tkEnd);
        keywords.Add("EXCEPT",(int)Tokens.tkExcept);
        keywords.Add("FILE",(int)Tokens.tkFile);
        keywords.Add("FINALIZATION",(int)Tokens.tkFinalization);
        keywords.Add("FINALLY",(int)Tokens.tkFinally);
        keywords.Add("FOR",(int)Tokens.tkFor);
        keywords.Add("FOREACH",(int)Tokens.tkForeach);
        keywords.Add("FUNCTION",(int)Tokens.tkFunction);
        keywords.Add("IF",(int)Tokens.tkIf);
        keywords.Add("IMPLEMENTATION",(int)Tokens.tkImplementation);
        keywords.Add("INHERITED",(int)Tokens.tkInherited);
        keywords.Add("INITIALIZATION",(int)Tokens.tkInitialization);
        keywords.Add("INTERFACE",(int)Tokens.tkInterface);
        keywords.Add("PROCEDURE",(int)Tokens.tkProcedure);
        keywords.Add("OPERATOR",(int)Tokens.tkOperator);
        keywords.Add("PROPERTY",(int)Tokens.tkProperty);
        keywords.Add("RAISE",(int)Tokens.tkRaise);
        keywords.Add("RECORD",(int)Tokens.tkRecord);
        keywords.Add("REPEAT",(int)Tokens.tkRepeat);
        keywords.Add("SET",(int)Tokens.tkSet);
        keywords.Add("TRY",(int)Tokens.tkTry);
        keywords.Add("TYPE",(int)Tokens.tkType);
        keywords.Add("THEN",(int)Tokens.tkThen);
        keywords.Add("TO",(int)Tokens.tkTo);
        keywords.Add("UNTIL",(int)Tokens.tkUntil);
        keywords.Add("USES",(int)Tokens.tkUses);
        keywords.Add("VAR",(int)Tokens.tkVar);
        keywords.Add("WHILE",(int)Tokens.tkWhile);
        keywords.Add("WITH",(int)Tokens.tkWith);
        keywords.Add("NIL",(int)Tokens.tkNil);
        keywords.Add("GOTO",(int)Tokens.tkGoto);
        keywords.Add("OF",(int)Tokens.tkOf);
        keywords.Add("LABEL",(int)Tokens.tkLabel);
        keywords.Add("LOCK",(int)Tokens.tkLock);
        keywords.Add("PROGRAM",(int)Tokens.tkProgram);
        keywords.Add("EVENT",(int)Tokens.tkEvent);
        keywords.Add("DEFAULT",(int)Tokens.tkDefault);
        keywords.Add("TEMPLATE",(int)Tokens.tkTemplate);
        keywords.Add("PACKED",(int)Tokens.tkPacked);
        keywords.Add("INLINE",(int)Tokens.tkInline);
        keywords.Add("EXPORTS",(int)Tokens.tkExports);
        keywords.Add("RESOURCESTRING",(int)Tokens.tkResourceString);
        keywords.Add("THREADVAR",(int)Tokens.tkThreadvar);
        keywords.Add("FINAL",(int)Tokens.tkFinal);
        keywords.Add("VARIANT",(int)Tokens.tkVariant);
        keywords.Add("OLEVARIANT",(int)Tokens.tkOleVariant);
        keywords.Add("PARAMS",(int)Tokens.tkParams);
        keywords.Add("AT",(int)Tokens.tkAt);
        keywords.Add("ON",(int)Tokens.tkOn);
        keywords.Add("CONTAINS",(int)Tokens.tkContains);
        keywords.Add("UNIT",(int)Tokens.tkUnit);
        keywords.Add("LIBRARY",(int)Tokens.tkLibrary);
        keywords.Add("OUT",(int)Tokens.tkOut);
        keywords.Add("PACKAGE",(int)Tokens.tkPackage);
        keywords.Add("REQUIRES",(int)Tokens.tkRequires);
        keywords.Add("SHORTINT",(int)Tokens.tkShortInt);
        keywords.Add("SMALLINT",(int)Tokens.tkSmallInt);
        keywords.Add("INTEGER",(int)Tokens.tkOrdInteger);
        keywords.Add("BYTE",(int)Tokens.tkByte);
        keywords.Add("LONGINT",(int)Tokens.tkLongInt);
        keywords.Add("INT64",(int)Tokens.tkInt64);
        keywords.Add("WORD",(int)Tokens.tkWord);
        keywords.Add("BOOLEAN",(int)Tokens.tkBoolean);
        keywords.Add("CHAR",(int)Tokens.tkChar);
        keywords.Add("WIDECHAR",(int)Tokens.tkWideChar);
        keywords.Add("LONGWORD",(int)Tokens.tkLongWord);
        keywords.Add("PCHAR",(int)Tokens.tkPChar);
        keywords.Add("CARDINAL",(int)Tokens.tkCardinal);
        keywords.Add("REAL",(int)Tokens.tkReal);
        keywords.Add("SINGLE",(int)Tokens.tkSingle);
        keywords.Add("DOUBLE",(int)Tokens.tkDouble);
        keywords.Add("EXTENDED",(int)Tokens.tkExtended);
        keywords.Add("COMP",(int)Tokens.tkComp);
        keywords.Add("ABSOLUTE",(int)Tokens.tkAbsolute);
        keywords.Add("ASSEMBLER",(int)Tokens.tkAssembler);
        keywords.Add("AUTOMATED",(int)Tokens.tkAutomated);
        keywords.Add("DISPID",(int)Tokens.tkDispid);
        keywords.Add("EXTERNAL",(int)Tokens.tkExternal);
        keywords.Add("IMPLEMENTS",(int)Tokens.tkImplements);
        keywords.Add("INDEX",(int)Tokens.tkIndex);
        keywords.Add("MESSAGE",(int)Tokens.tkMessage);
        keywords.Add("NAME",(int)Tokens.tkName);
        keywords.Add("NODEFAULT",(int)Tokens.tkNodefault);
        keywords.Add("PRIVATE",(int)Tokens.tkPrivate);
        keywords.Add("PROTECTED",(int)Tokens.tkProtected);
        keywords.Add("PUBLIC",(int)Tokens.tkPublic);
        keywords.Add("INTERNAL",(int)Tokens.tkInternal);
        keywords.Add("READ",(int)Tokens.tkRead);
        keywords.Add("RESIDENT",(int)Tokens.tkResident);
        keywords.Add("STORED",(int)Tokens.tkStored);
        keywords.Add("WRITE",(int)Tokens.tkWrite);
        keywords.Add("READONLY",(int)Tokens.tkReadOnly);
        keywords.Add("WRITEONLY",(int)Tokens.tkWriteOnly);
        keywords.Add("ABSTRACT",(int)Tokens.tkAbstract);
        keywords.Add("FORWARD",(int)Tokens.tkForward);
        keywords.Add("OVERLOAD",(int)Tokens.tkOverload);
        keywords.Add("REINTRODUCE",(int)Tokens.tkReintroduce);
        keywords.Add("OVERRIDE",(int)Tokens.tkOverride);
        keywords.Add("VIRTUAL",(int)Tokens.tkVirtual);
	}
	
	public static int KeywordOrIDToken(string s)
	{
		s = s.ToUpper();
		if (keywords.ContainsKey(s))
			return keywords[s];
		else
			return (int)Tokens.tkIdentifier;
	}
}
  
