%namespace GPPGParserScanner

%using PascalABCCompiler.KuMir00Parser;
%using PascalABCCompiler.SyntaxTree;

Alpha [[:IsLetter:]_]
Digit [0-9]
AlphaDigit [[:IsLetterOrDigit:]_]
INTNUM {Digit}+
ID {Alpha}{AlphaDigit}*
REAL {INTNUM}\.{INTNUM}
 
LINECOMMENT |.*
STRING \'[^'\n]*\'|\"[^"\n]*\"
CHAR '[^"\\\r\n]{1}(?:\\.[^"\\\r\n]*)*'
ENDL [\n|\r\n?]*

%%

{LINECOMMENT} { 
		BEGIN(INITIAL);
	}


\x02 { return (int)Tokens.TK_NEWLINE; }
"**" { return (int)Tokens.TK_POWER; }

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
"<>" { return (int)Tokens.NE; }
"(" { return (int)Tokens.LPAREN; }
")" { return (int)Tokens.RPAREN; }
"," { return (int)Tokens.COLUMN; }
"." { return (int)Tokens.COMMA; }
":" { return (int)Tokens.COLON; }
"[" { return (int)Tokens.TK_SQUARE_OPEN; }
"]" { return (int)Tokens.TK_SQUARE_CLOSE; }
\x01 { return (int)Tokens.INVISIBLE; }

{ID}  { 
  int res = ScannerHelper.GetIDToken(yytext);
  if (res == (int)Tokens.ID)
	yylval.sVal = yytext;
  return res;
}

{INTNUM} { 
  yylval.iVal = int.Parse(yytext); 
  return (int)Tokens.INTNUM; 
}

{REAL} { 
	//yylval.rVal = PT.create_double_const(yytext, yylloc); 
		
	yylval.rVal = double.Parse(yytext,new System.Globalization.CultureInfo("en-US"));
	return (int)Tokens.REAL; 
}

{CHAR} { 
  yylval.chVal = yytext[1]; 
  return (int)Tokens.CHAR; 
}

{STRING} { 
  yylval.sVal = yytext.Substring(1,yytext.Length - 2); 
  return (int)Tokens.STRING; 
}

{ENDL} { 
  yylval.sVal = "\n"; 
  return (int)Tokens.ENDL; 
}

%{
  yylloc = new QUT.Gppg.LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%

  public override void yyerror(string format, params object[] args) 
  {
    string [] ww = new string[args.Length-1];
	for (int i=1; i<args.Length; i++)
	  ww[i-1]=(string)args[i];
	string w = string.Join(" ��� ",ww);  
    string errorMsg = string.Format("�������������� ������: ��������� {0}, � ��������� {1}", args[0], w);
    PT.AddError(errorMsg,yylloc);
  }

class ScannerHelper 
{
  private static Dictionary<string,int> keywords;

  static ScannerHelper() 
  {
    keywords = new Dictionary<string,int>();

    keywords.Add("��",(int)Tokens.TK_TRUE);
    keywords.Add("���",(int)Tokens.TK_FALSE);    
    keywords.Add("���",(int)Tokens.TK_ALG);
    keywords.Add("���",(int)Tokens.TK_USES);
    keywords.Add("���",(int)Tokens.TK_ARG);
    keywords.Add("���",(int)Tokens.TK_VAR);
    keywords.Add("����",(int)Tokens.TK_FUNC_VAL);
    keywords.Add("���",(int)Tokens.TK_BEGIN);
    keywords.Add("���",(int)Tokens.TK_END);
    keywords.Add("��",(int)Tokens.TK_BEGIN_CYCLE);
    keywords.Add("��",(int)Tokens.TK_END_CYCLE);
    keywords.Add("���",(int)Tokens.TK_ASSERT);
    keywords.Add("����",(int)Tokens.TK_READ);
    keywords.Add("�����",(int)Tokens.TK_WRITE);
    keywords.Add("��",(int)Tokens.TK_EOL);
    keywords.Add("����",(int)Tokens.TK_IF);
    keywords.Add("��",(int)Tokens.TK_THEN);
    keywords.Add("�����",(int)Tokens.TK_ELSE);
    keywords.Add("���",(int)Tokens.TK_END_ALL);
    keywords.Add("�����",(int)Tokens.TK_CASE);
    keywords.Add("���",(int)Tokens.TK_CASE_V);
    keywords.Add("��",(int)Tokens.TK_NOT);
    keywords.Add("�",(int)Tokens.TK_AND);
    keywords.Add("���",(int)Tokens.TK_OR);
    keywords.Add("����",(int)Tokens.TK_WHILE);
    keywords.Add("���",(int)Tokens.TK_FOR);
    keywords.Add("��",(int)Tokens.TK_FROM);
    keywords.Add("��",(int)Tokens.TK_TO);
    keywords.Add("���",(int)Tokens.TK_INTEGER_TYPE);
    keywords.Add("���",(int)Tokens.TK_REAL_TYPE);
    keywords.Add("���",(int)Tokens.TK_BOOLEAN_TYPE);
    keywords.Add("���",(int)Tokens.TK_CHAR_TYPE);
    keywords.Add("���",(int)Tokens.TK_STRING_TYPE);
    keywords.Add("���",(int)Tokens.TK_ARRAY);
    keywords.Add("���",(int)Tokens.TK_ISP);   
    keywords.Add("���",(int)Tokens.TK_RAZ);
  }
  public static int GetIDToken(string s)
  {
	if (keywords.ContainsKey(s))
	  return keywords[s];
	else
      return (int)Tokens.ID;
  }
  
}
