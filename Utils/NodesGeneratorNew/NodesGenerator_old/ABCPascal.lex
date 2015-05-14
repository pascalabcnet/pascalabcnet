%{
    public PT parsertools;
    Stack<BufferContext> buffStack = new Stack<BufferContext>();
    Stack<string> fNameStack = new Stack<string>();
	Stack<int> IfStack = new Stack<int>(); // 0 - if, 1 - else
	List<string> Defines = new List<string>();
	int IfExclude;
	string Pars;
	string directivename;
	string directiveparam;
%}

%namespace GPPGParserScanner

%using PascalABCSavParser;
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

DotChr1 [^\r\n}]

NOTASCII [^\x00-x7F]

CHARACTERNUM '[^'\n]'
INTNUM {Digit}+
FLOATNUM {INTNUM}\.{INTNUM}
EXPNUM ({INTNUM}\.)?{INTNUM}[eE][+\-]?{INTNUM}
STRINGNUM \'([^\'\n]|\'\')*\'
HEXNUM ${HexDigit}+
SHARPCHARNUM #{Digit}+
OLDDIRECTIVE #{ID}
IFDEF \{\$ifdef\ {DotChr1}*\} 
IFNDEF \{\$ifndef\ {DotChr1}*\} 
ENDIF \{\$endif\}
DEFINE \{\$define\ {DotChr1}*\} 
DIRECTIVE \{\${DotChr1}*\}

ALPHABET [^ a-zA-Z_0-9\r\n\t\'$#&,:.;@\+\-\*/=<>\^()\[\]\x01]

%x COMMENT
%x COMMENT1
%x COMMENTONELINE
%x INCL
%x EXCLUDETEXT

%%

{OneLineCmnt} {
	
}

{DIRECTIVE} {
	if (parsertools.build_tree_for_brackets)
		break;

	parsertools.DivideDirectiveOn(yytext,out directivename,out directiveparam);
    parsertools.CheckDirectiveParams(directivename,directiveparam); // directivename in UPPERCASE!
	if (directivename == "INCLUDE")
	{
		TryInclude(directiveparam);
	}
	else if (directivename == "IFDEF")
	{
		IfStack.Push(0);
		if (!Defines.Contains(directiveparam))
		{
			BEGIN(EXCLUDETEXT);
			IfExclude = 1;
		}
	}
	else if (directivename == "IFNDEF")
	{
		IfStack.Push(0);
		if (Defines.Contains(directiveparam))
		{
			BEGIN(EXCLUDETEXT);	    
			IfExclude = 1;
		}
	}
	else if (directivename == "ELSE")
	{
		if (IfStack.Count==0)
			parsertools.AddErrorFromResource("UNNECESSARY $else",CurrentLexLocation);
		if (IfStack.Peek()==1) 
			parsertools.AddErrorFromResource("UNNECESSARY $else",CurrentLexLocation);
		IfStack.Pop();	
		IfStack.Push(1);
		BEGIN(EXCLUDETEXT);
		IfExclude = 1;
	}
	else if (directivename == "ENDIF")
	{
		if (IfStack.Count == 0)
			parsertools.AddErrorFromResource("UNNECESSARY $endif",CurrentLexLocation);	   
		IfStack.Pop();
	}
	else if (directivename == "DEFINE")
	{
		if (!Defines.Contains(directiveparam))
			Defines.Add(directiveparam);
	}
}

<EXCLUDETEXT>{OneLineCmnt} {

}

<EXCLUDETEXT>{DIRECTIVE} {
	parsertools.DivideDirectiveOn(yytext,out directivename,out directiveparam);
    parsertools.CheckDirectiveParams(directivename,directiveparam); // directivename in UPPERCASE!
	if (directivename == "IFDEF")
	{
		IfStack.Push(0);
		IfExclude++;
	}
	else if (directivename == "IFNDEF")
	{
		IfStack.Push(0);
		IfExclude++;
	}
	else if (directivename == "ELSE")
	{
		if (IfStack.Peek() == 1) 
			parsertools.AddErrorFromResource("UNNECESSARY $else",CurrentLexLocation);
		IfStack.Pop();	
		IfStack.Push(1);
		if (IfExclude == 1)
			BEGIN(INITIAL);
	}
	else if (directivename == "ENDIF")
	{
		if (IfStack.Count==0)
			parsertools.AddErrorFromResource("UNNECESSARY $endif",CurrentLexLocation);	   
		IfStack.Pop();
		IfExclude--;
		if (IfExclude == 0)
			BEGIN(INITIAL); 		
	}
}

<EXCLUDETEXT>.|\n {
}

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

[&]?{ID}  { 
  int res = Keywords.KeywordOrIDToken(yytext);
  if (res == (int)Tokens.tkIdentifier)
  {
	yylval = new Union(); 
    yylval.id = parsertools.create_ident(yytext,CurrentLexLocation);
  }
  else 
	switch (res)
    {
    case (int)Tokens.tkOr:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalOR,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkXor:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseXOR,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkAnd:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalAND,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkDiv:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.IntegerDivision,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkMod:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.ModulusRemainder,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkShl:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseLeftShift,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkShr:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseRightShift,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkNot:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalNOT,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkAs:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.As,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkIn:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.In,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkIs:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Is,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkImplicit:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Implicit,CurrentLexLocation);
		yylval.op.text = yytext;
        break;
    case (int)Tokens.tkExplicit:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Explicit,CurrentLexLocation);
		yylval.op.text = yytext;
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
    case (int)Tokens.tkExports:
    case (int)Tokens.tkResourceString:
    case (int)Tokens.tkThreadvar:
    case (int)Tokens.tkSealed:
    case (int)Tokens.tkPartial:
    case (int)Tokens.tkParams:
    case (int)Tokens.tkTo:
    case (int)Tokens.tkDownto:
    case (int)Tokens.tkUnit:
    case (int)Tokens.tkLibrary:
    case (int)Tokens.tkExternal:
		yylval = new Union();
        yylval.ti = new token_info(yytext,CurrentLexLocation);
        break;
    case (int)Tokens.tkBegin:
    case (int)Tokens.tkEnd:
    case (int)Tokens.tkTry:
		yylval = new Union();
        yylval.ti = new token_info(yytext,CurrentLexLocation);
        break;
    case (int)Tokens.tkNew:
    case (int)Tokens.tkOn:
    case (int)Tokens.tkName:
    case (int)Tokens.tkPrivate:
    case (int)Tokens.tkProtected:
    case (int)Tokens.tkPublic:
    case (int)Tokens.tkInternal:
    case (int)Tokens.tkRead:
    case (int)Tokens.tkWrite:
		yylval = new Union(); 
        yylval.id = new ident(yytext,CurrentLexLocation);
        break;
    case (int)Tokens.tkAbstract:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_abstract,CurrentLexLocation);
        yylval.id.name = yytext;
        break;
    case (int)Tokens.tkForward:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_forward,CurrentLexLocation);
        yylval.id.name = yytext;
        break;
    case (int)Tokens.tkOverload:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_overload,CurrentLexLocation);
        yylval.id.name = yytext;
        break;
    case (int)Tokens.tkReintroduce:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_reintroduce,CurrentLexLocation);
        yylval.id.name = yytext;
        break;
    case (int)Tokens.tkOverride:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_override,CurrentLexLocation);
        yylval.id.name = yytext;
        break;
    case (int)Tokens.tkVirtual:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_virtual,CurrentLexLocation);
        yylval.id.name = yytext;
        break;
    }
  return res;
}

{INTNUM} { 
  yylval = new Union(); 
  yylval.ex = parsertools.create_int_const(yytext,CurrentLexLocation); 
  return (int)Tokens.tkInteger; 
}

{HEXNUM} { 
  yylval = new Union(); 
  yylval.ex = parsertools.create_hex_const(yytext,CurrentLexLocation);
  return (int)Tokens.tkHex; 
}

{FLOATNUM} |
{EXPNUM} {
  yylval = new Union(); 
  yylval.ex = parsertools.create_double_const(yytext,CurrentLexLocation);
  return (int)Tokens.tkFloat;
}

{STRINGNUM} { 
  yylval = new Union(); 
  yylval.stn = parsertools.create_string_const(yytext,CurrentLexLocation); 
  return (int)Tokens.tkStringLiteral; 
}

{SHARPCHARNUM} {
  yylval = new Union(); 
  yylval.stn = parsertools.create_sharp_char_const(yytext,CurrentLexLocation); 
  return (int)Tokens.tkAsciiChar; 
}

{OLDDIRECTIVE} {
  yylval = new Union(); 
  yylval.id = new ident(yytext,CurrentLexLocation);
  return (int)Tokens.tkDirectiveName;
}

{ALPHABET} {
  parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}",CurrentLexLocation, yytext);
  return -1;
}

%{
  yylloc = CurrentLexLocation;
%}

%%
  public LexLocation CurrentLexLocation
  {
      get {
          return new LexLocation(tokLin, tokCol, tokELin, tokECol, parsertools.CurrentFileName);
	  }
  }
  
    protected override bool yywrap()
    {
	    if (IfStack.Count > 0)
		    parsertools.AddErrorFromResource("ENDIF_ABSENT",CurrentLexLocation);
		BEGIN(INITIAL);	
        if (buffStack.Count == 0) 
		    return true;
        RestoreBuffCtx(buffStack.Pop());
		parsertools.CurrentFileName = fNameStack.Pop();
        return false;     
    }
  
    public override void yyerror(string format, params object[] args) 
    {
		string errorMsg = parsertools.CreateErrorString(yytext,args);
		parsertools.AddError(errorMsg,CurrentLexLocation);
    }
  
    private void TryInclude(string fName)
    {
        if (fName == null || fName.Length == 0)
            parsertools.AddErrorFromResource("INCLUDE_EMPTY_FILE",CurrentLexLocation);
        else 
            try {
                if (fName.StartsWith("'"))
				{
                    fName = fName.Substring(1);
					if (fName.EndsWith("'"))
						fName = fName.Substring(0, fName.Length-1);
				}
                BufferContext savedCtx = MkBuffCtx();
                string full_path = fName;
                if (!Path.IsPathRooted(full_path))
                    full_path = Path.Combine(Path.GetDirectoryName(parsertools.CurrentFileName),fName);
                SetSource(File.ReadAllText(full_path), 0);
				fNameStack.Push(parsertools.CurrentFileName);
				parsertools.CurrentFileName = full_path;
                buffStack.Push(savedCtx); 
            }
            catch
            {
                parsertools.AddErrorFromResource("INCLUDE_COULDNT_OPEN_FILE{0}",CurrentLexLocation,fName);
            }
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
        keywords.Add("EXPORTS",(int)Tokens.tkExports);
        keywords.Add("RESOURCESTRING",(int)Tokens.tkResourceString);
        keywords.Add("THREADVAR",(int)Tokens.tkThreadvar);
        keywords.Add("SEALED",(int)Tokens.tkSealed);
        keywords.Add("PARTIAL",(int)Tokens.tkPartial);
        keywords.Add("PARAMS",(int)Tokens.tkParams);
        keywords.Add("UNIT",(int)Tokens.tkUnit);
        keywords.Add("LIBRARY",(int)Tokens.tkLibrary);
        keywords.Add("EXTERNAL",(int)Tokens.tkExternal);
        keywords.Add("NAME",(int)Tokens.tkName);
        keywords.Add("PRIVATE",(int)Tokens.tkPrivate);
        keywords.Add("PROTECTED",(int)Tokens.tkProtected);
        keywords.Add("PUBLIC",(int)Tokens.tkPublic);
        keywords.Add("INTERNAL",(int)Tokens.tkInternal);
        keywords.Add("READ",(int)Tokens.tkRead);
        keywords.Add("WRITE",(int)Tokens.tkWrite);
        keywords.Add("ON",(int)Tokens.tkOn);
        keywords.Add("FORWARD",(int)Tokens.tkForward);
        keywords.Add("ABSTRACT",(int)Tokens.tkAbstract);
        keywords.Add("OVERLOAD",(int)Tokens.tkOverload);
        keywords.Add("REINTRODUCE",(int)Tokens.tkReintroduce);
        keywords.Add("OVERRIDE",(int)Tokens.tkOverride);
        keywords.Add("VIRTUAL",(int)Tokens.tkVirtual);
        keywords.Add("NEW",(int)Tokens.tkNew);
        //keywords.Add("CYCLE",(int)Tokens.tkCycle);
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
  
