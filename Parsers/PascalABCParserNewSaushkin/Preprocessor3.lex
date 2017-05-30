%{
%}
%scannertype PreprocessorScanner

%namespace GPPGPreprocessor3

%using PascalABCCompiler.SyntaxTree;
%using QUT.Gppg;

DIRECTIVE [^\}]+
NODIRECTIVE [.|\n]+

%x INSIDEDIRECTIVE

%%

"{$" { 
  BEGIN(INSIDEDIRECTIVE);
}

<INSIDEDIRECTIVE> "}" { 
  BEGIN(INITIAL);
}

<INSIDEDIRECTIVE> {DIRECTIVE} {
  //if (yytext.Contains("\n"))
  //  Console.WriteLine("({0},{1})  {2}",tokLin,tokCol,"error: LF into directive");
  //else Console.WriteLine("({0},{1})  {2}",tokLin,tokCol,yytext);
  yylval = new Directive(yytext, new LexLocation(tokLin, tokCol, tokELin, tokECol));
  var t1 = yylloc;
  return (int)Tokens.DIRECTIVE;
}

{NODIRECTIVE} {
  return (int)Tokens.NODIRECTIVE;
}

%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%

  /*public override void yyerror(string format, params object[] args) 
  {
    string errorMsg = parsertools.CreateErrorString(yytext,args);
    parsertools.AddError(errorMsg,new LexLocation(tokLin, tokCol, tokELin, tokECol));
  }*/
  