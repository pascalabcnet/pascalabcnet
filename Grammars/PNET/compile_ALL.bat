cls
@echo off
echo [PNET] GRM to CS,RES compiler. (c) DarkStar 2008
echo.

echo Delete old files...
del PNET.cs 
del PNET.cgt 
del PNETLang.resources

echo Compile: GRM to CGT...
start /wait ..\..\Utils\gpbcmd\goldbuilder_main PNET.grm PNET.cgt
copy PNET.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\Utils\gpbcmd\createskelprog_main PNET.cgt PNET.pgt PNET.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\Utils\grmCommentCompiler\bin\Debug\grmCommentCompiler.exe PNET.grm PNET.tmpl PNET.cs
echo.

echo Compile: CGT to RES...
..\..\Utils\ResXMaker\ResXMaker.exe PNET.CGT PNETLanguage PNETLang.resources
echo.

copy PNETLang.resources ..\..\Parsers\PNETParser\PNETLang.resources
copy PNET.cs ..\..\Parsers\PNETParser\PNET_lrparser_rules.cs