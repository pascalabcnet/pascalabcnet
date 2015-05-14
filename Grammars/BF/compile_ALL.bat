cls
@echo off
echo [BrainF*ck] GRM to CS,RES compiler. (c) DarkStar 2006
echo.

echo Delete old files...
del BF.cs 
del BF.cgt 
del BF.cgt.zip
del BFLang.resources

echo Compile: GRM to CGT...
start /wait ..\..\Utils\GPBCMD\goldbuilder_main BF.grm BF.cgt
copy BF.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\Utils\GPBCMD\createskelprog_main BF.cgt BF.pgt BF.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\Utils\grmCommentCompiler\bin\Debug\grmCommentCompiler.exe BF.grm BF.tmpl BF.cs
echo.

echo Compile: CGT to RES...
..\..\Utils\ResXMaker\ResXMaker.exe BF.CGT BFLanguage BFLang.resources
echo.

copy BFLang.resources ..\..\Parsers\BFParser\BFLang.resources
copy BF.cs ..\..\Parsers\BFParser\BF_lrparser.cs