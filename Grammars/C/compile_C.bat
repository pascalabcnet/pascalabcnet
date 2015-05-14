cls
@echo off
echo [C] GRM to CS,RES compiler. (c) DarkStar 2007
echo.

echo Delete old files...
del C.cs 
del C.cgt 
del CLang.resources

echo Compile: GRM to CGT...
start /wait ..\..\Utils\GPBCMD\goldbuilder_main C.grm C.cgt
copy C.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\Utils\gpbcmd\createskelprog_main C.cgt C.pgt C.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\Utils\grmCommentCompiler\bin\Debug\grmCommentCompiler.exe C.grm C.tmpl C.cs
echo.

echo Compile: CGT to RES...
..\..\Utils\ResXMaker\ResXMaker.exe C.CGT CLanguage CLang.resources
echo.

copy CLang.resources ..\..\Parsers\CParser\CLang.resources
copy C.cs ..\..\Parsers\CParser\C_lrparser.cs