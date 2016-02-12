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
	LexLocation currentLexLocation;
%}

%namespace GPPGParserScanner

%using PascalABCSavParser;
%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;
%using QUT.Gppg;

Letter [[:IsLetter:]_]
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

UNICODEARROW \x890

%x COMMENT
%x COMMENT1
%x COMMENTONELINE
%x INCL
%x EXCLUDETEXT

%%

{OneLineCmnt} {
	
}

{DIRECTIVE} {
	if (parsertools.build_tree_for_formatter)
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
"?."              { return (int)Tokens.tkQuestionPoint; }
"=>"			 { return (int)Tokens.tkMatching; }
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
\x2192 			{ yylval = new Union(); yylval.ti = new token_info(yytext); return (int)Tokens.tkArrow; }
\<\<expression\>\> { return (int)Tokens.tkParseModeExpression; }
\<\<statement\>\>  { return (int)Tokens.tkParseModeStatement; }
\<\<type\>\>  { return (int)Tokens.tkParseModeType; }

\x01 { return (int)Tokens.INVISIBLE; }

[&]?{ID}  { 
  string cur_yytext = yytext;
  int res = Keywords.KeywordOrIDToken(cur_yytext);
  currentLexLocation = CurrentLexLocation;
  if (res == (int)Tokens.tkIdentifier)
  {
	yylval = new Union(); 
    yylval.id = parsertools.create_ident(cur_yytext,currentLexLocation);
  }
  else 
	switch (res)
    {
    case (int)Tokens.tkOr:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalOR,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkXor:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseXOR,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkAnd:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalAND,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkDiv:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.IntegerDivision,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkMod:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.ModulusRemainder,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkShl:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseLeftShift,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkShr:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.BitwiseRightShift,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkNot:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.LogicalNOT,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkAs:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.As,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkIn:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.In,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkIs:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Is,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkImplicit:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Implicit,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkExplicit:
		yylval = new Union(); 
        yylval.op = new op_type_node(Operators.Explicit,currentLexLocation);
		yylval.op.text = cur_yytext;
        break;
    case (int)Tokens.tkSizeOf:
    case (int)Tokens.tkTypeOf:
    case (int)Tokens.tkWhere:
    case (int)Tokens.tkArray:
    case (int)Tokens.tkCase:
    case (int)Tokens.tkClass:
    case (int)Tokens.tkAuto:
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
    case (int)Tokens.tkYield:
    case (int)Tokens.tkSequence:
		yylval = new Union();
        yylval.ti = new token_info(cur_yytext,currentLexLocation);
        break;
    case (int)Tokens.tkBegin:
    case (int)Tokens.tkEnd:
    case (int)Tokens.tkTry:
		yylval = new Union();
        yylval.ti = new token_info(cur_yytext,currentLexLocation);
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
        yylval.id = new ident(cur_yytext,currentLexLocation);
        break;
    case (int)Tokens.tkAbstract:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_abstract,currentLexLocation);
        yylval.id.name = cur_yytext;
        break;
    case (int)Tokens.tkForward:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_forward,currentLexLocation);
        yylval.id.name = cur_yytext;
        break;
    case (int)Tokens.tkOverload:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_overload,currentLexLocation);
        yylval.id.name = cur_yytext;
        break;
    case (int)Tokens.tkReintroduce:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_reintroduce,currentLexLocation);
        yylval.id.name = cur_yytext;
        break;
    case (int)Tokens.tkOverride:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_override,currentLexLocation);
        yylval.id.name = cur_yytext;
        break;
    case (int)Tokens.tkExtensionMethod:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_extension,currentLexLocation);
        yylval.id.name = cur_yytext;
        break;
    case (int)Tokens.tkVirtual:
		yylval = new Union(); 
        yylval.id = new procedure_attribute(proc_attribute.attr_virtual,currentLexLocation);
        yylval.id.name = cur_yytext;
        break;
    }
  return res;
}

{INTNUM} { 
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parsertools.create_int_const(yytext,currentLexLocation); 
  return (int)Tokens.tkInteger; 
}

{HEXNUM} { 
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parsertools.create_hex_const(yytext,currentLexLocation);
  return (int)Tokens.tkHex; 
}

{FLOATNUM} |
{EXPNUM} {
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parsertools.create_double_const(yytext,currentLexLocation);
  return (int)Tokens.tkFloat;
}

{STRINGNUM} { 
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parsertools.create_string_const(yytext,currentLexLocation); 
  return (int)Tokens.tkStringLiteral; 
}

{SHARPCHARNUM} {
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parsertools.create_sharp_char_const(yytext,currentLexLocation); 
  return (int)Tokens.tkAsciiChar; 
}

{OLDDIRECTIVE} {
  yylval = new Union(); 
  yylval.id = new ident(yytext,CurrentLexLocation);
  return (int)Tokens.tkDirectiveName;
}

[^ \r\n\t] {
  parsertools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}",CurrentLexLocation, yytext);
  return -1;
}

%{
  if (currentLexLocation != null)
    yylloc = currentLexLocation;
  else
	yylloc = CurrentLexLocation;
  currentLexLocation = null;
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
                    full_path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(parsertools.CurrentFileName), fName));
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
	private static Dictionary<string, int> keywords = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

	static Keywords()
	{
        keywords.Add("or",(int)Tokens.tkOr);
        keywords.Add("xor",(int)Tokens.tkXor);
        keywords.Add("and",(int)Tokens.tkAnd);
        keywords.Add("div",(int)Tokens.tkDiv);
        keywords.Add("mod",(int)Tokens.tkMod);
        keywords.Add("shl",(int)Tokens.tkShl);
        keywords.Add("shr",(int)Tokens.tkShr);
        keywords.Add("not",(int)Tokens.tkNot);
        keywords.Add("as",(int)Tokens.tkAs);
        keywords.Add("in",(int)Tokens.tkIn);
        keywords.Add("is",(int)Tokens.tkIs);
        keywords.Add("implicit",(int)Tokens.tkImplicit);
        keywords.Add("explicit",(int)Tokens.tkExplicit);
        keywords.Add("sizeof",(int)Tokens.tkSizeOf);
        keywords.Add("typeof",(int)Tokens.tkTypeOf);
        keywords.Add("where",(int)Tokens.tkWhere);
        keywords.Add("array",(int)Tokens.tkArray);
        keywords.Add("begin",(int)Tokens.tkBegin);
        keywords.Add("case",(int)Tokens.tkCase);
        keywords.Add("class",(int)Tokens.tkClass);
        keywords.Add("const",(int)Tokens.tkConst);
        keywords.Add("constructor",(int)Tokens.tkConstructor);
        keywords.Add("destructor",(int)Tokens.tkDestructor);
        keywords.Add("downto",(int)Tokens.tkDownto);
        keywords.Add("do",(int)Tokens.tkDo);
        keywords.Add("else",(int)Tokens.tkElse);
        keywords.Add("end",(int)Tokens.tkEnd);
        keywords.Add("except",(int)Tokens.tkExcept);
        keywords.Add("file",(int)Tokens.tkFile);
        keywords.Add("finalization",(int)Tokens.tkFinalization);
        keywords.Add("finally",(int)Tokens.tkFinally);
        keywords.Add("for",(int)Tokens.tkFor);
        keywords.Add("foreach",(int)Tokens.tkForeach);
        keywords.Add("function",(int)Tokens.tkFunction);
        keywords.Add("if",(int)Tokens.tkIf);
        keywords.Add("implementation",(int)Tokens.tkImplementation);
        keywords.Add("inherited",(int)Tokens.tkInherited);
        keywords.Add("initialization",(int)Tokens.tkInitialization);
        keywords.Add("interface",(int)Tokens.tkInterface);
        keywords.Add("procedure",(int)Tokens.tkProcedure);
        keywords.Add("operator",(int)Tokens.tkOperator);
        keywords.Add("property",(int)Tokens.tkProperty);
        keywords.Add("raise",(int)Tokens.tkRaise);
        keywords.Add("record",(int)Tokens.tkRecord);
        keywords.Add("repeat",(int)Tokens.tkRepeat);
        keywords.Add("set",(int)Tokens.tkSet);
        keywords.Add("try",(int)Tokens.tkTry);
        keywords.Add("type",(int)Tokens.tkType);
        keywords.Add("then",(int)Tokens.tkThen);
        keywords.Add("to",(int)Tokens.tkTo);
        keywords.Add("until",(int)Tokens.tkUntil);
        keywords.Add("uses",(int)Tokens.tkUses);
        keywords.Add("var",(int)Tokens.tkVar);
        keywords.Add("while",(int)Tokens.tkWhile);
        keywords.Add("with",(int)Tokens.tkWith);
        keywords.Add("nil",(int)Tokens.tkNil);
        keywords.Add("goto",(int)Tokens.tkGoto);
        keywords.Add("of",(int)Tokens.tkOf);
        keywords.Add("label",(int)Tokens.tkLabel);
        keywords.Add("lock",(int)Tokens.tkLock);
        keywords.Add("program",(int)Tokens.tkProgram);
        keywords.Add("event",(int)Tokens.tkEvent);
        keywords.Add("default",(int)Tokens.tkDefault);
        keywords.Add("template",(int)Tokens.tkTemplate);
        keywords.Add("packed",(int)Tokens.tkPacked);
        keywords.Add("exports",(int)Tokens.tkExports);
        keywords.Add("resourcestring",(int)Tokens.tkResourceString);
        keywords.Add("threadvar",(int)Tokens.tkThreadvar);
        keywords.Add("sealed",(int)Tokens.tkSealed);
        keywords.Add("partial",(int)Tokens.tkPartial);
        keywords.Add("params",(int)Tokens.tkParams);
        keywords.Add("unit",(int)Tokens.tkUnit);
        keywords.Add("library",(int)Tokens.tkLibrary);
        keywords.Add("external",(int)Tokens.tkExternal);
        keywords.Add("name",(int)Tokens.tkName);
        keywords.Add("private",(int)Tokens.tkPrivate);
        keywords.Add("protected",(int)Tokens.tkProtected);
        keywords.Add("public",(int)Tokens.tkPublic);
        keywords.Add("internal",(int)Tokens.tkInternal);
        keywords.Add("read",(int)Tokens.tkRead);
        keywords.Add("write",(int)Tokens.tkWrite);
        keywords.Add("on",(int)Tokens.tkOn);
        keywords.Add("forward",(int)Tokens.tkForward);
        keywords.Add("abstract",(int)Tokens.tkAbstract);
        keywords.Add("overload",(int)Tokens.tkOverload);
        keywords.Add("reintroduce",(int)Tokens.tkReintroduce);
        keywords.Add("override",(int)Tokens.tkOverride);
        keywords.Add("virtual",(int)Tokens.tkVirtual);
        keywords.Add("extensionmethod",(int)Tokens.tkExtensionMethod);
        keywords.Add("new",(int)Tokens.tkNew);
        keywords.Add("auto",(int)Tokens.tkAuto);
        keywords.Add("sequence",(int)Tokens.tkSequence);
        keywords.Add("yield",(int)Tokens.tkYield);
	}
	
	public static int KeywordOrIDToken(string s)
	{
		//s = s.ToUpper();
		int keyword = 0;
		if (keywords.TryGetValue(s, out keyword))
			return keyword;
		else
			return (int)Tokens.tkIdentifier;
	}
}
  
