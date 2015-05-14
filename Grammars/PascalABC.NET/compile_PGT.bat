del PABCParser.cs 
start /wait ..\..\utils\gpbcmd\createskelprog_main PascalABC.cgt PascalABC.pgt PascalABC.tmpl
..\..\grmCommentCompiler\bin\debug\grmCommentCompiler PascalABC.grm PascalABC.tmpl PascalABC.cs
copy PascalABC.cs ..\..\Parsers\PascalABCParser\pascalabc_lrparser_rules.cs