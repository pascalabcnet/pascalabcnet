%using VeryBasicParser;
%using QUT.Gppg;
%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;
%using VeryBasicParserYacc;
%using VeryBasicParser;

%namespace VeryBasicParser

Alpha         [a-zA-Z_]
NonZeroDigit  [1-9]
Digit         0|{NonZeroDigit}
AlphaDigit {Alpha}|{Digit}
UInt ({NonZeroDigit}{Digit}*)|0
INTNUM  -?{UInt}
REALNUM -?(({UInt}?\.{UInt})|({UInt}\.))
STRINGNUM (\'([^\'\n\\]|\\.)*\')|(\"([^\"\n\\]|\\.)*\")
ID {Alpha}{AlphaDigit}*

%{
  public VeryBasicParserTools parsertools;
  public List<string> Defines = new List<string>();
  LexLocation currentLexLocation;
%}

%%

{INTNUM} {
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parsertools.create_int_const(yytext,currentLexLocation);
  return (int)Tokens.INTNUM;
}

{REALNUM} {
  currentLexLocation = CurrentLexLocation;
  yylval.ex = parsertools.create_double_const(yytext,currentLexLocation);
  return (int)Tokens.REALNUM;
}

{STRINGNUM} {
  currentLexLocation = CurrentLexLocation;
  yylval.stn = parsertools.create_string_const(yytext,currentLexLocation); 
  return (int)Tokens.STRINGNUM;
}

"and" { yylval.op = new op_type_node(Operators.LogicalAND); return (int)Tokens.AND; }
"or"  { yylval.op = new op_type_node(Operators.LogicalOR); return (int)Tokens.OR; }

{ID}  {
  string cur_yytext = yytext;
  int res = Keywords.KeywordOrIDToken(cur_yytext);
  currentLexLocation = CurrentLexLocation;
  if (res == (int)Tokens.ID)
  {
    yylval.id = parsertools.create_ident(cur_yytext,currentLexLocation);
  }
  return res;
}

"+"  { yylval.op = new op_type_node(Operators.Plus); return (int)Tokens.PLUS; }
"-"  { yylval.op = new op_type_node(Operators.Minus); return (int)Tokens.MINUS; }
"*"  { yylval.op = new op_type_node(Operators.Multiplication); return (int)Tokens.MULTIPLY; }
"/"  { yylval.op = new op_type_node(Operators.Division); return (int)Tokens.DIVIDE; }

"<="  { yylval.op = new op_type_node(Operators.LessEqual); return (int)Tokens.LESSEQUAL; }
">="  { yylval.op = new op_type_node(Operators.GreaterEqual); return (int)Tokens.GREATEREQUAL; }
"<"  { yylval.op = new op_type_node(Operators.Less); return (int)Tokens.LESS; }
">"  { yylval.op = new op_type_node(Operators.Greater); return (int)Tokens.GREATER; }
"=="  { yylval.op = new op_type_node(Operators.Equal); return (int)Tokens.EQUAL; }
"!="  { yylval.op = new op_type_node(Operators.NotEqual); return (int)Tokens.NOTEQUAL; }

"#{"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.INDENT; }
"#}"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.UNINDENT; }
"{"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.LBRACE; }
"}"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.RBRACE; }
"["  { currentLexLocation = CurrentLexLocation; return (int)Tokens.LBRACKET; }
"]"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.RBRACKET; }

"->"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.ARROW; }

"."  { currentLexLocation = CurrentLexLocation; return (int)Tokens.DOT; }
","  { currentLexLocation = CurrentLexLocation; return (int)Tokens.COMMA; }
":"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.COLON; }
";"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.SEMICOLON; }
"("  { currentLexLocation = CurrentLexLocation; return (int)Tokens.LPAR; }
")"  { currentLexLocation = CurrentLexLocation; return (int)Tokens.RPAR; }
"="  { yylval.op = new op_type_node(Operators.Assignment); currentLexLocation = CurrentLexLocation; return (int)Tokens.ASSIGN; }


[^ \r\n] {
  parsertools.AddError("Unexpected token!", CurrentLexLocation);
	return (int)Tokens.EOF;
}

%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol); // ������� ������� (������������� ��� ���������������), ������������ @1 @2 � �.�.
%}

%%

public LexLocation CurrentLexLocation
{
  get {
    return new LexLocation(tokLin, tokCol, tokELin, tokECol, parsertools.CurrentFileName);
	}
}

public override void yyerror(string format, params object[] args)
{
	string errorMsg = parsertools.CreateErrorString(yytext, yylloc, args);
	parsertools.AddError(errorMsg,CurrentLexLocation);
}

