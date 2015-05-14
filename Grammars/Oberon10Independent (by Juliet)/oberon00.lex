%namespace GPPGParserScanner

%using PascalABCCompiler.Oberon00Parser;

Digit 		[0-9] 
HexDigit	{Digit}|[A-F]
Alpha 		[a-zA-Z_]
EndLine 	\n									// Конец строки
AlphaDigit 	{Alpha}|{Digit}
ScaleFactor	[ED][\+\-]?{Digit}+
HEXINTNUM	{Digit}{HexDigit}*H
INTNUM 		{Digit}+
REALNUM		{Digit}+\.{Digit}*{ScaleFactor}?
ID 			{Alpha}{AlphaDigit}*
CHARACTER 	'[^'\n]'|\"[^\"\n]\"|{Digit}{HexDigit}*X
STRING 		'[^'\n]*'|\"[^\"\n]*\"
 

%x COMMENT									// Состояние для комментариев
%x COMMENT_S								// Состояние для однострочных комментариев				

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
"{" { return (int)Tokens.LBRACE; }
"}" { return (int)Tokens.RBRACE; }
"[" { return (int)Tokens.LBRACKET; }
"]" { return (int)Tokens.RBRACKET; }
".." { return (int)Tokens.DOUBLEPOINT; }
"|" { return (int)Tokens.PIPE; }
\x01 { return (int)Tokens.INVISIBLE; }

"//" { BEGIN(COMMENT_S);}					// Однострочные комментарии
<COMMENT_S> {EndLine} { BEGIN(INITIAL);}
"(*" { 										// Многострочные вложенные комментарии
	BEGIN(COMMENT);
	mlCommentCnt = 1;
}
<COMMENT> "(*" { ++mlCommentCnt; }
<COMMENT> "*)" { 
	--mlCommentCnt;
	if (mlCommentCnt == 0)
		BEGIN(INITIAL);
}
<COMMENT> <<EOF>> { 
	PT.AddError("Комментарий не закрыт", yylloc); 
}

{ID}  { 
  int res = Keywords.KeywordOrIDToken(yytext);
  if (res == (int)Tokens.ID){
    yylval.sVal = yytext;
	PT.LastIdentificator = yytext;
  }
  return res;
}

{INTNUM} { 
	int tryParseInt;
	if (int.TryParse(yytext, out tryParseInt)){
		yylval.iVal = tryParseInt;
		return (int)Tokens.INTNUM; 
	}
	System.Int64 tryParseLong;
	if (System.Int64.TryParse(yytext, out tryParseLong)){
		yylval.lVal = tryParseLong;
		return (int)Tokens.LONGINTNUM; 
	}
	PT.AddError("Слишком длинное целое", yylloc);
}

{HEXINTNUM} {
	var _yytext = yytext.Substring(0, yytext.Length - 1); 
	int tryParseHexInt;
	if (int.TryParse(_yytext, System.Globalization.NumberStyles.AllowHexSpecifier, null, out tryParseHexInt)){
		yylval.iVal = tryParseHexInt;
		return (int)Tokens.INTNUM; 
	}
	System.Int64 tryParseHexLong;
	if (System.Int64.TryParse(_yytext, System.Globalization.NumberStyles.AllowHexSpecifier, null, out tryParseHexLong)){
		yylval.lVal = tryParseHexLong;
		return (int)Tokens.LONGINTNUM; 
	}
	PT.AddError("Слишком длинное шестнадцатеричное целое", yylloc);
}

{REALNUM} {
	double tryParseDouble;
	var correctDouble = PT.GetCorrectDoubleStr(yytext);
	if (double.TryParse(correctDouble, out tryParseDouble)){
		yylval.rVal = tryParseDouble;
		return (int)Tokens.REALNUM; 
	}
	PT.AddError("Слишком длинное вещественное", yylloc);
}

{CHARACTER} {
	try{
		yylval.cVal = PT.GetStringContent(yytext)[0];
		return (int)Tokens.CHAR_CONST;
	}
	catch (System.ArgumentException){
		PT.AddError("Некорректный код символа", yylloc);
	}
}
{STRING} { 
	yylval.sVal = PT.GetStringContent(yytext);
	return (int)Tokens.STRING_CONST; 
}

%{
  yylloc = new QUT.Gppg.LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%

// Счетчик вложенности для многострочных вложенных комментариев
static int mlCommentCnt = 0;				

  public override void yyerror(string format, params object[] args) 
  {
    string errorMsg = PT.CreateErrorString(args);
    PT.AddError(errorMsg,yylloc);
  }

// Статический класс, определяющий ключевые слова языка
public static class Keywords
{
	private static Dictionary<string, int> keywords = new Dictionary<string, int>();

	static Keywords()
	{
		keywords.Add("TRUE", (int)Tokens.TRUE);
		keywords.Add("FALSE", (int)Tokens.FALSE);
		keywords.Add("ODD", (int)Tokens.ODD);
		keywords.Add("OR", (int)Tokens.OR);
		keywords.Add("DIV", (int)Tokens.DIV);
		keywords.Add("MOD", (int)Tokens.MOD);
		keywords.Add("BEGIN", (int)Tokens.BEGIN);
		keywords.Add("END", (int)Tokens.END);
		keywords.Add("MODULE", (int)Tokens.MODULE);
		keywords.Add("CONST", (int)Tokens.CONST);
		keywords.Add("VAR", (int)Tokens.VAR);
		keywords.Add("TYPE", (int)Tokens.TYPE);
		keywords.Add("ARRAY", (int)Tokens.ARRAY);
		keywords.Add("OF", (int)Tokens.OF);
		keywords.Add("RECORD", (int)Tokens.RECORD);
		keywords.Add("POINTER", (int)Tokens.POINTER);
		keywords.Add("IF", (int)Tokens.IF);
		keywords.Add("THEN", (int)Tokens.THEN);
		keywords.Add("ELSE", (int)Tokens.ELSE);
		keywords.Add("ELSEIF", (int)Tokens.ELSEIF);
		keywords.Add("WHILE", (int)Tokens.WHILE);
		keywords.Add("DO", (int)Tokens.DO);
		keywords.Add("REPEAT", (int)Tokens.REPEAT);
		keywords.Add("UNTIL", (int)Tokens.UNTIL);
		keywords.Add("FOR", (int)Tokens.FOR);
		keywords.Add("TO", (int)Tokens.TO);
		keywords.Add("BY", (int)Tokens.BY);
		keywords.Add("CASE", (int)Tokens.CASE);
		keywords.Add("PROCEDURE", (int)Tokens.PROCEDURE);
	}
	public static int KeywordOrIDToken(string s)
	{
		if (keywords.ContainsKey(s))
			return keywords[s];
		else
			return (int)Tokens.ID;
	}
}
  
