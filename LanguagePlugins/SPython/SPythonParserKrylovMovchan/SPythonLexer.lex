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
  LexLocation currentLexLocation;

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
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parserTools.create_int_const(yytext,currentLexLocation);
  return (int)Tokens.INTNUM;
}

{REALNUM} {
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parserTools.create_double_const(yytext,currentLexLocation);
  return (int)Tokens.REALNUM;
}

{STRINGNUM} {
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parserTools.create_string_const(yytext,currentLexLocation);
  return (int)Tokens.STRINGNUM;
}

{ID}  {
  string cur_yytext = yytext;
  int res = keywords.KeywordOrIDToken(cur_yytext);
  currentLexLocation = CurrentLexLocation;
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
  }

  return res;
}

"+=" { yylval.op = new op_type_node(Operators.AssignmentAddition); return (int)Tokens.PLUSEQUAL; }
"-=" { yylval.op = new op_type_node(Operators.AssignmentSubtraction); return (int)Tokens.MINUSEQUAL; }
"*=" { yylval.op = new op_type_node(Operators.AssignmentMultiplication); return (int)Tokens.STAREQUAL; }
"/=" { yylval.op = new op_type_node(Operators.AssignmentDivision); return (int)Tokens.DIVEQUAL; }

"+"  { yylval.op = new op_type_node(Operators.Plus); return (int)Tokens.PLUS; }
"-"  { yylval.op = new op_type_node(Operators.Minus); return (int)Tokens.MINUS; }
"*"  { yylval.op = new op_type_node(Operators.Multiplication); return (int)Tokens.STAR; }
"//" { yylval.op = new op_type_node(Operators.IntegerDivision); return (int)Tokens.SLASHSLASH; }
"/"  { yylval.op = new op_type_node(Operators.Division); return (int)Tokens.DIVIDE; }
"%"  { yylval.op = new op_type_node(Operators.ModulusRemainder); return (int)Tokens.PERCENTAGE; }

"<=" { yylval.op = new op_type_node(Operators.LessEqual); return (int)Tokens.LESSEQUAL; }
">=" { yylval.op = new op_type_node(Operators.GreaterEqual); return (int)Tokens.GREATEREQUAL; }
"<"  { yylval.op = new op_type_node(Operators.Less); return (int)Tokens.LESS; }
">"  { yylval.op = new op_type_node(Operators.Greater); return (int)Tokens.GREATER; }
"==" { yylval.op = new op_type_node(Operators.Equal); return (int)Tokens.EQUAL; }
"!=" { yylval.op = new op_type_node(Operators.NotEqual); return (int)Tokens.NOTEQUAL; }
"="  { yylval.op = new op_type_node(Operators.Assignment); return (int)Tokens.ASSIGN; }

"#{" { return (int)Tokens.INDENT; }
"#}" { return (int)Tokens.UNINDENT; }
"{"  { return (int)Tokens.LBRACE; }
"}"  { return (int)Tokens.RBRACE; }
"["  { return (int)Tokens.LBRACKET; }
"]"  { return (int)Tokens.RBRACKET; }

"->" { yylval.ti = new token_info(yytext); return (int)Tokens.ARROW; }

"."  { return (int)Tokens.DOT; }
","  { return (int)Tokens.COMMA; }
":"  { return (int)Tokens.COLON; }
";"  { return (int)Tokens.SEMICOLON; }
"("  { return (int)Tokens.LPAR; }
")"  { return (int)Tokens.RPAR; }
"**" { return (int)Tokens.STARSTAR; }

"##" {
  parserTools.AddErrorFromResource("WRONG_INDENT", new LexLocation(CurrentLexLocation.StartLine + 1, 0, CurrentLexLocation.StartLine + 1, 0));
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
	string errorMsg = parserTools.CreateErrorString(yytext, args);
	parserTools.AddError(errorMsg, CurrentLexLocation);
}

protected override bool yywrap()
{
  return true;
}

