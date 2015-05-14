del PABCParser.cs 
start /wait ..\..\Utils\gpbcmd\createskelprog_main PNET.cgt PNET.pgt PNET.tmpl
..\..\Utils\grmCommentCompiler\bin\Debug\grmCommentCompiler.exe PNET.grm PNET.tmpl PNET.cs
copy PNET.cs ..\..\..\PABCSVN\Parsers\PNETParser\PNET_lrparser_rules.cs