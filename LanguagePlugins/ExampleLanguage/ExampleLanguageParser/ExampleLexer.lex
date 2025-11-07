%using QUT.Gppg;
%using PascalABCCompiler.SyntaxTree;
%using PascalABCCompiler.ParserTools;

%namespace Languages.Example.Frontend.Core

Letter [[:IsLetter:]_]
Digit [0-9]
Digit_ [0-9_]
LetterDigit {Letter}|{Digit}
ID {Letter}{LetterDigit}*

INTNUM {Digit}{Digit_}*

%{
  private ExampleKeywords keywords;
  internal ExampleParserTools parserTools;
  internal List<string> Defines = new List<string>();

  internal Scanner(string text, ExampleParserTools parserTools, PascalABCCompiler.Parsers.BaseKeywords keywords, List<string> defines = null) 
  {
    this.parserTools = parserTools;
    this.keywords = (ExampleKeywords)keywords;
    if (defines != null)
      this.Defines.AddRange(defines);
    SetSource(text, 0);
  }
%}

%%

{INTNUM} {
  yylval = new Union();
  yylval.ex = parserTools.create_int_const(yytext, CurrentLexLocation);
  return (int)Tokens.INTNUM;
}

{ID}  {
  string curYytext = yytext;
  int res = keywords.KeywordOrIDToken(curYytext);
  yylval = new Union();
  
  switch (res)
  {
    case (int)Tokens.ID:
      yylval.id = parserTools.create_ident(curYytext, CurrentLexLocation);
      break;
    // ...
  }

  return res;
}

"="  { yylval = new Union(); yylval.op = new op_type_node(Operators.Assignment); return (int)Tokens.ASSIGN; }
";"  { yylval = new Union(); return (int)Tokens.SEMICOLON; }
"("  { yylval = new Union(); return (int)Tokens.LPAR; }
")"  { yylval = new Union(); return (int)Tokens.RPAR; }

%%

public LexLocation CurrentLexLocation
{
  get {
    return new LexLocation(tokLin, tokCol, tokELin, tokECol, parserTools.currentFileName);
	}
}