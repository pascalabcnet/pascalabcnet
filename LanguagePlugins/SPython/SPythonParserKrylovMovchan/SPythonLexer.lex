%using SPythonParser;
%using QUT.Gppg;
%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;
%using SPythonParserYacc;
%using SPythonParser;

%namespace SPythonParser

Alpha         [[:IsLetter:]_]
NonZeroDigit  [1-9]
Digit         0|{NonZeroDigit}
AlphaDigit {Alpha}|{Digit}
INTNUM  ({NonZeroDigit}{Digit}*)|0
REALNUM ({INTNUM}?\.{INTNUM})|({INTNUM}\.)
STRINGNUM (\'([^\'\n\\]|\\.)*\')|(\"([^\"\n\\]|\\.)*\")
ID {Alpha}{AlphaDigit}*

%{
  private SPythonKeywords keywords;
  public SPythonParserTools parserTools;
  public List<string> Defines = new List<string>();
  private int last_line_needed_colon = -1;
  private LexLocation currentLexLocation;
  private LexLocation prevLexLocation 
  = new LexLocation(-1, -1, -1, -1, "");

  public Scanner(string text, SPythonParserTools parserTools, PascalABCCompiler.Parsers.BaseKeywords keywords, List<string> defines = null) 
  {
    this.parserTools = parserTools;
    this.keywords = (SPythonKeywords)keywords;
    if (defines != null)
      this.Defines.AddRange(defines);
    SetSource(text, 0);
  }
%}

%%

{INTNUM} {
  yylval = new SPythonParserYacc.ValueType();
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parserTools.create_int_const(yytext,currentLexLocation);
  return (int)Tokens.INTNUM;
}

{REALNUM} {
  yylval = new SPythonParserYacc.ValueType();
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parserTools.create_double_const(yytext,currentLexLocation);
  return (int)Tokens.REALNUM;
}

{STRINGNUM} {
  yylval = new SPythonParserYacc.ValueType();
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parserTools.create_string_const(yytext,currentLexLocation);
  return (int)Tokens.STRINGNUM;
}

{ID}  {
  string cur_yytext = yytext;
  int res = keywords.KeywordOrIDToken(cur_yytext);
  currentLexLocation = CurrentLexLocation;
  yylval = new SPythonParserYacc.ValueType();
  switch (res)
  {
    case (int)Tokens.ID:
      yylval.id = parserTools.create_ident(cur_yytext, currentLexLocation);
      break;
    case (int)Tokens.AND:
      yylval.op = new op_type_node(Operators.LogicalAND, currentLexLocation);
      yylval.op.text = cur_yytext;
      break;
    case (int)Tokens.OR:
      yylval.op = new op_type_node(Operators.LogicalOR, currentLexLocation);
      yylval.op.text = cur_yytext;
      break;
    case (int)Tokens.NOT:
      yylval.op = new op_type_node(Operators.LogicalNOT, currentLexLocation);
      yylval.op.text = cur_yytext;
      break;
    case (int)Tokens.IF:
    case (int)Tokens.ELIF:
    case (int)Tokens.ELSE:
    case (int)Tokens.WHILE:
    case (int)Tokens.FOR:
    case (int)Tokens.DEF:
      last_line_needed_colon = CurrentLexLocation.StartLine;
      break;
    case (int)Tokens.CLASS:
    case (int)Tokens.LAMBDA:
      parserTools.AddErrorFromResource("UNSUPPORTED_CONSTRUCTION_{0}", currentLexLocation, yytext);
      break;
  }

  return res;
}

"+=" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.AssignmentAddition); return (int)Tokens.PLUSEQUAL; }
"-=" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.AssignmentSubtraction); return (int)Tokens.MINUSEQUAL; }
"*=" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.AssignmentMultiplication); return (int)Tokens.STAREQUAL; }
"/=" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.AssignmentDivision); return (int)Tokens.DIVEQUAL; }

"+"  { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.Plus); return (int)Tokens.PLUS; }
"-"  { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.Minus); return (int)Tokens.MINUS; }
"*"  { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.Multiplication); return (int)Tokens.STAR; }
"//" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.IntegerDivision); return (int)Tokens.SLASHSLASH; }
"/"  { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.Division); return (int)Tokens.DIVIDE; }
"%"  { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.ModulusRemainder); return (int)Tokens.PERCENTAGE; }

"<=" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.LessEqual); return (int)Tokens.LESSEQUAL; }
">=" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.GreaterEqual); return (int)Tokens.GREATEREQUAL; }
"<"  { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.Less); return (int)Tokens.LESS; }
">"  { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.Greater); return (int)Tokens.GREATER; }
"==" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.Equal); return (int)Tokens.EQUAL; }
"!=" { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.NotEqual); return (int)Tokens.NOTEQUAL; }
"="  { yylval = new SPythonParserYacc.ValueType(); yylval.op = new op_type_node(Operators.Assignment); return (int)Tokens.ASSIGN; }

"#{" { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.INDENT; }
"#}" { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.UNINDENT; }
"#;" { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.END_OF_LINE; }

"{"  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.LBRACE; }
"}"  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.RBRACE; }
"["  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.LBRACKET; }
"]"  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.RBRACKET; }

"->" { yylval = new SPythonParserYacc.ValueType(); yylval.ti = new token_info(yytext); return (int)Tokens.ARROW; }

"."  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.DOT; }
","  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.COMMA; }
":"  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.COLON; }
";"  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.SEMICOLON; }
"("  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.LPAR; }
")"  { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.RPAR; }
"**" { yylval = new SPythonParserYacc.ValueType(); return (int)Tokens.STARSTAR; }

"#!" {
  parserTools.AddErrorFromResource("WRONG_INDENT", 
    new LexLocation(CurrentLexLocation.StartLine + 1, 0, CurrentLexLocation.StartLine + 1, 0));
	return (int)Tokens.EOF;
}

"#b" {
  parserTools.AddErrorFromResource("PROGRAM_BEGIN_WITH_INDENT", 
    CurrentLexLocation);
	return (int)Tokens.EOF;
}

"#$" {
  return (int)Tokens.END_OF_FILE;
}

[^ \r\n] {
  parserTools.AddErrorFromResource("UNEXPECTED_TOKEN_{0}", CurrentLexLocation, yytext);
	return (int)Tokens.EOF;
}

%{
  //yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
  prevLexLocation = yylloc;
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

public override void yyerror(string format, params object[] args)
{
    string expected = parserTools.ExpectedToken(last_line_needed_colon == prevLexLocation.StartLine, args);
    LexLocation lexLocation = parserTools.GetLexLocation(
      yytext, expected, prevLexLocation, CurrentLexLocation);
    
    if (yytext == "#{" && expected == "END_OF_LINE") {
      parserTools.AddErrorFromResource("WRONG_INDENT", lexLocation);
    }
    else {
      string errorMsg = parserTools.CreateErrorString(yytext, expected);
      parserTools.AddError(errorMsg, lexLocation);
    }
}

protected override bool yywrap()
{
  return true;
}

