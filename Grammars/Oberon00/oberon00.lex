%namespace GPPGParserScanner

%using PascalABCCompiler.Oberon00Parser;

Alpha [a-zA-Z_]
Digit [0-9]
AlphaDigit {Alpha}|{Digit}
INTNUM {Digit}+
ID {Alpha}{AlphaDigit}* 

%%

":=" { return (int)Tokens.ASSIGN; }
";" { return (int)Tokens.SEMICOLUMN; }
"-" { return (int)Tokens.MINUS; }
"+" { return (int)Tokens.PLUS; }
"*" { return (int)Tokens.MULT; }
"/" { return (int)Tokens.DIVIDE; }
"<" { return (int)Tokens.LT; }
">" { return (int)Tokens.GT; }
"<=" { return (int)Tokens.LE; }
">=" { return (int)Tokens.GE; }
"=" { return (int)Tokens.EQ; }
"#" { return (int)Tokens.NE; }
"(" { return (int)Tokens.LPAREN; }
")" { return (int)Tokens.RPAREN; }
"," { return (int)Tokens.COLUMN; }
"~" { return (int)Tokens.NOT; }
"&" { return (int)Tokens.AND; }
"." { return (int)Tokens.COMMA; }
":" { return (int)Tokens.COLON; }
"!" { return (int)Tokens.EXCLAMATION; }
\x01 { return (int)Tokens.INVISIBLE; }

{ID}  { 
  int res = Keywords.KeywordOrIDToken(yytext);
  if (res == (int)Tokens.ID)
    yylval.sVal = yytext;
  return res;
}

{INTNUM} { 
  yylval.iVal = int.Parse(yytext); 
  return (int)Tokens.INTNUM; 
}

%{
  yylloc = new QUT.Gppg.LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%

  public override void yyerror(string format, params object[] args) 
  {
    string errorMsg = PT.CreateErrorString(args);
    PT.AddError(errorMsg,yylloc);
  }

