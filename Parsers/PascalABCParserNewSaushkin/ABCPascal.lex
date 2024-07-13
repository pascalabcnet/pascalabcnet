%{
  public PascalParserTools parserTools;
  private PascalABCCompiler.Parsers.BaseKeywords keywords;
  private Stack<BufferContext> buffStack = new Stack<BufferContext>();
  private Stack<string> fNameStack = new Stack<string>();
	private Stack<bool> IfDefInElseBranch = new Stack<bool>();
	private Stack<string> IfDefVar = new Stack<string>();
	public List<string> Defines = new List<string>();
	private int IfExclude;
	private string Pars;
	private LexLocation currentLexLocation;
	private bool HiddenIdents = false;
	private bool ExprMode = false;

  public Scanner(PascalABCCompiler.Parsers.BaseKeywords keywords) { this.keywords = keywords; }
%}

%namespace Languages.Pascal.Frontend.Core

%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;
%using QUT.Gppg;

Letter [[:IsLetter:]_]
Digit [0-9]
Digit_ [0-9_]
LetterDigit {Letter}|{Digit}
ID {Letter}{LetterDigit}* 
HexDigit {Digit}|[abcdefABCDEF]
HexDigit_ {Digit}|[abcdefABCDEF_]
DotChr [^\r\n]
OneLineCmnt  \/\/{DotChr}*

DotChr1 [^\r\n}]

NOTASCII [^\x00-x7F]

CHARACTERNUM '[^'\n]'
INTNUM {Digit}{Digit_}*
BIGINTNUM {INTNUM}[bB][iI]
FLOATNUM {INTNUM}\.{INTNUM}
EXPNUM ({INTNUM}\.)?{INTNUM}[eE][+\-]?{INTNUM}
STRINGNUM \'([^\'\n]|\'\')*\'
MULTILINESTRINGNUM \'\'\'[ ]*\r?\n([^']|\'[^']|\'\'[^'])*\'\'\'
FORMATSTRINGNUM \$\'([^\'\n]|\'\')*\'
HEXNUM ${HexDigit}{HexDigit_}*
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
	if (parserTools.buildTreeForFormatter)
		break;

  parserTools.ParseDirective(yytext, CurrentLexLocation, out var directiveName, out var directiveParams);
  var orgDirectiveName = directiveName;
  
  if (directiveName == "") // случай пустой директивы
    break;

  directiveName = directiveName.ToUpper();

	if (directiveName == "HIDDENIDENTS")
	{
		HiddenIdents = true;
	}
	else if (directiveName == "INCLUDE")
	{
		TryInclude(directiveParams[0]);
	}
	else if (directiveName == "IFDEF")
	{
		IfDefInElseBranch.Push(false);
        IfDefVar.Push(directiveParams[0]);
		if (!Defines.Contains(directiveParams[0]))
		{
			BEGIN(EXCLUDETEXT);
			IfExclude = 1;
		}
	}
	else if (directiveName == "IFNDEF")
	{
		IfDefInElseBranch.Push(false);
        IfDefVar.Push(directiveParams[0]);
		if (Defines.Contains(directiveParams[0]))
		{
			BEGIN(EXCLUDETEXT);	    
			IfExclude = 1;
		}
	}
	else if (directiveName == "ELSE")
	{
        if (directiveParams.Count!=0 && directiveParams[0]!=IfDefVar.Peek())
            parserTools.AddWarningFromResource("DIFF_DEFINE_NAME", CurrentLexLocation, orgDirectiveName, IfDefVar.Peek(), directiveParams[0]);
        if (IfDefInElseBranch.Count==0 || IfDefInElseBranch.Pop())
			parserTools.AddErrorFromResource("UNNECESSARY $else",CurrentLexLocation);
		IfDefInElseBranch.Push(true);
		BEGIN(EXCLUDETEXT);
		IfExclude = 1;
	}
	else if (directiveName == "ENDIF")
	{
		if (IfDefInElseBranch.Count == 0)
			parserTools.AddErrorFromResource("UNNECESSARY $endif",CurrentLexLocation);	   
		IfDefInElseBranch.Pop();
        var define_name = IfDefVar.Pop();
        if (directiveParams.Count!=0 && directiveParams[0]!=define_name)
            parserTools.AddWarningFromResource("DIFF_DEFINE_NAME", CurrentLexLocation, orgDirectiveName, define_name, directiveParams[0]);
	}
	else if (directiveName == "DEFINE")
	{
		if (!Defines.Contains(directiveParams[0]))
			Defines.Add(directiveParams[0]);
	}
	else if (directiveName == "UNDEF")
	{
		if (Defines.Contains(directiveParams[0]))
			Defines.Remove(directiveParams[0]);
	}
  parserTools.compilerDirectives.Add(new compiler_directive(new token_info(directiveName), new token_info(string.Join(" ", directiveParams)), CurrentLexLocation));
}

<EXCLUDETEXT>{OneLineCmnt} {

}

<EXCLUDETEXT>{DIRECTIVE} {
	
  parserTools.ParseDirective(yytext, CurrentLexLocation, out directiveName, out directiveParams);
  orgDirectiveName = directiveName;

  if (directiveName == "") // случай пустой директивы
    break;
	
  directiveName = directiveName.ToUpper();

  bool addDirective = false;

  if (directiveName == "IFDEF")
	{
		IfDefInElseBranch.Push(false);
        IfDefVar.Push(directiveParams[0]);
		IfExclude++;
	}
	else if (directiveName == "IFNDEF")
	{
		IfDefInElseBranch.Push(false);
        IfDefVar.Push(directiveParams[0]);
		IfExclude++;
	}
	else if (directiveName == "ELSE")
	{
        if (directiveParams.Count!=0 && directiveParams[0]!=IfDefVar.Peek())
            parserTools.AddWarningFromResource("DIFF_DEFINE_NAME", CurrentLexLocation, orgDirectiveName, IfDefVar.Peek(), directiveParams[0]);
        if (IfDefInElseBranch.Count==0 || IfDefInElseBranch.Pop())
			parserTools.AddErrorFromResource("UNNECESSARY $else",CurrentLexLocation);
		IfDefInElseBranch.Push(true);
		if (IfExclude == 1)
    {
        BEGIN(INITIAL);
        addDirective = true;
    }
			
	}
	else if (directiveName == "ENDIF")
	{
		if (IfDefInElseBranch.Count == 0)
			parserTools.AddErrorFromResource("UNNECESSARY $endif",CurrentLexLocation);	   
		IfDefInElseBranch.Pop();
        var define_name = IfDefVar.Pop();
        if (directiveParams.Count!=0 && directiveParams[0]!=define_name)
            parserTools.AddWarningFromResource("DIFF_DEFINE_NAME", CurrentLexLocation, orgDirectiveName, define_name, directiveParams[0]);
        IfExclude--;
		if (IfExclude == 0)
    {
        BEGIN(INITIAL);
        addDirective = true;
    }
			
	}

  if (addDirective)
    parserTools.compilerDirectives.Add(new compiler_directive(new token_info(directiveName), new token_info(string.Join(" ", directiveParams)), CurrentLexLocation));
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

"|"              { return (int)Tokens.tkVertParen; }
[#][#][ \t\r\n]+  { yylval = new Union(); yylval.ti = new token_info("##",CurrentLexLocation);	return (int)Tokens.tkShortProgram; }
[#][#][#][ \t\r\n]+ { yylval = new Union(); yylval.ti = new token_info("###",CurrentLexLocation); return (int)Tokens.tkShortSFProgram; 
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
"_"				 { return (int)Tokens.tkUnderscore; }
"?."              { return (int)Tokens.tkQuestionPoint; }
"??"              { return (int)Tokens.tkDoubleQuestion; }
"?["              { return (int)Tokens.tkQuestionSquareOpen; }
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
"**"            { yylval = new Union(); yylval.op = new op_type_node(Operators.Power); return (int)Tokens.tkStarStar; }
"="             { yylval = new Union(); yylval.op = new op_type_node(Operators.Equal); return (int)Tokens.tkEqual; }
">"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Greater); return (int)Tokens.tkGreater; }
">="            { yylval = new Union(); yylval.op = new op_type_node(Operators.GreaterEqual); return (int)Tokens.tkGreaterEqual; }
"<"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Less); return (int)Tokens.tkLower; }
"<="            { yylval = new Union(); yylval.op = new op_type_node(Operators.LessEqual); return (int)Tokens.tkLowerEqual; }
"<>"            { yylval = new Union(); yylval.op = new op_type_node(Operators.NotEqual); return (int)Tokens.tkNotEqual; }
"^"             { yylval = new Union(); yylval.op = new op_type_node(Operators.Deref); return (int)Tokens.tkDeref; }
"->"            { yylval = new Union(); yylval.ti = new token_info(yytext); return (int)Tokens.tkArrow; }
\\[(]           { yylval = new Union(); yylval.ti = new token_info(yytext); return (int)Tokens.tkBackSlashRoundOpen; }


\u2192 			{ yylval = new Union(); yylval.ti = new token_info(yytext); return (int)Tokens.tkArrow; }

\<\<expression\>\> { ExprMode = true; return (int)Tokens.tkParseModeExpression; }
\<\<statement\>\>  { ExprMode = true; return (int)Tokens.tkParseModeStatement; }
\<\<type\>\>  { ExprMode = true; return (int)Tokens.tkParseModeType; }

\x01 { return (int)Tokens.INVISIBLE; }

[&]?[!]?{ID}  { 
  string cur_yytext = yytext;
  int res = keywords.KeywordOrIDToken(cur_yytext);
  currentLexLocation = CurrentLexLocation;
  if (res == (int)Tokens.tkIdentifier)
  {
    if (cur_yytext[0] == '!' && !HiddenIdents && !ExprMode)
    	parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}",CurrentLexLocation, ""+cur_yytext[0]);
	yylval = new Union(); 
    yylval.id = parserTools.create_ident(cur_yytext,currentLexLocation);
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
    case (int)Tokens.tkLoop:
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
    case (int)Tokens.tkExports:
    case (int)Tokens.tkResourceString:
    case (int)Tokens.tkThreadvar:
    case (int)Tokens.tkSealed:
    case (int)Tokens.tkPartial:
    case (int)Tokens.tkParams:
    case (int)Tokens.tkTo:
    case (int)Tokens.tkDownto:
    case (int)Tokens.tkUnit:
    case (int)Tokens.tkNamespace:
    case (int)Tokens.tkLibrary:
    case (int)Tokens.tkExternal:
    case (int)Tokens.tkYield:
    case (int)Tokens.tkSequence:
    case (int)Tokens.tkMatch:
    case (int)Tokens.tkWhen:
    case (int)Tokens.tkStatic:
    case (int)Tokens.tkStep:
    case (int)Tokens.tkAsync:
    case (int)Tokens.tkAwait:
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
    case (int)Tokens.tkIndex:
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
  yylval.ex = parserTools.create_int_const(yytext,currentLexLocation); 
  return (int)Tokens.tkInteger; 
}

{BIGINTNUM} { 
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parserTools.create_bigint_const(yytext,currentLexLocation); 
  return (int)Tokens.tkBigInteger; 
}


{HEXNUM} { 
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parserTools.create_hex_const(yytext,currentLexLocation);
  return (int)Tokens.tkHex; 
}

{FLOATNUM} |
{EXPNUM} {
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parserTools.create_double_const(yytext,currentLexLocation);
  return (int)Tokens.tkFloat;
}

{STRINGNUM} { 
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parserTools.create_string_const(yytext,currentLexLocation); 
  return (int)Tokens.tkStringLiteral; 
}

{MULTILINESTRINGNUM} { 
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parserTools.create_multiline_string_const(yytext,currentLexLocation); 
  return (int)Tokens.tkMultilineStringLiteral; 
}

{FORMATSTRINGNUM} { 
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parserTools.create_format_string_const(yytext,currentLexLocation); 
  return (int)Tokens.tkFormatStringLiteral; 
}

{SHARPCHARNUM} {
  yylval = new Union();
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parserTools.create_sharp_char_const(yytext,currentLexLocation); 
  return (int)Tokens.tkAsciiChar; 
}

{OLDDIRECTIVE} {
  yylval = new Union(); 
  yylval.id = new ident(yytext,CurrentLexLocation);
  return (int)Tokens.tkDirectiveName;
}

[^ \r\n\t] {
  parserTools.AddErrorFromResource("UNEXPECTED_SYMBOL{0}",CurrentLexLocation, yytext);
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
          return new LexLocation(tokLin, tokCol, tokELin, tokECol, parserTools.currentFileName);
	  }
  }
  
    protected override bool yywrap()
    {
	    if (IfDefInElseBranch.Count != 0)
		    parserTools.AddErrorFromResource("ENDIF_ABSENT",CurrentLexLocation);
		BEGIN(INITIAL);	
        if (buffStack.Count == 0) 
		    return true;
        RestoreBuffCtx(buffStack.Pop());
		parserTools.currentFileName = fNameStack.Pop();
        return false;     
    }
  
    public override void yyerror(string format, params object[] args) 
    {
		string errorMsg = parserTools.CreateErrorString(yytext,args);
		parserTools.AddError(errorMsg,CurrentLexLocation);
    }
  
    private void TryInclude(string fName)
    {
        if (fName == null || fName.Length == 0)
            parserTools.AddErrorFromResource("INCLUDE_EMPTY_FILE",CurrentLexLocation);
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
                    full_path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(parserTools.currentFileName), fName));
                    
                if (fNameStack.Contains(full_path))
                {
                    parserTools.AddErrorFromResource("RECUR_INCLUDE", CurrentLexLocation, fName);
                    return;
                }
                    
                SetSource(File.ReadAllText(full_path), 0);
				fNameStack.Push(parserTools.currentFileName);
				parserTools.currentFileName = full_path;
                buffStack.Push(savedCtx); 
            }
            catch
            {
                parserTools.AddErrorFromResource("INCLUDE_COULDNT_OPEN_FILE{0}",CurrentLexLocation,fName);
            }
    }

// Класс, определяющий ключевые слова языка, находится в файле Keywords.cs
