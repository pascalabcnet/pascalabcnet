cls
@echo off
echo [Haskell] GRM to CS,RES compiler.
echo.

echo Delete old files...
del Haskell_lrparser_rules.cs 
del haskell_parser.cgt 
del HaskellLang.resources
del haskell_parser.tmpl
del haskell_parser.log

echo Compile: GRM to CGT...
start /wait ..\..\Utils\gpbcmd\goldbuilder_main haskell_parser.grm haskell_parser.cgt
copy haskell_parser.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\Utils\gpbcmd\createskelprog_main haskell_parser.cgt haskell_parser.pgt haskell_parser.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\Utils\grmCommentCompiler\bin\Debug\grmCommentCompiler.exe haskell_parser.grm haskell_parser.tmpl Haskell_lrparser_rules.cs
echo.

echo Compile: CGT to RES...
start /wait ..\..\Utils\ResXMaker\ResXMaker.exe haskell_parser.CGT HaskellLanguage HaskellLang.resources
echo.

copy HaskellLang.resources ..\..\Parsers\HaskellParser\HaskellLang.resources
copy Haskell_lrparser_rules.cs ..\..\Parsers\HaskellParser\Haskell_lrparser_rules.cs