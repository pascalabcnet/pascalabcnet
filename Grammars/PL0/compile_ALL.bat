cls
@echo off
echo [PL/0] GRM to CS,RES compiler. (c) DarkStar 2007
echo.

echo Delete old files...
del PL0.cs 
del PL0.cgt 
del PL0Lang.resources

echo Compile: GRM to CGT...
start /wait ..\..\Utils\gpbcmd\goldbuilder_main PL0.grm PL0.cgt
copy PL0.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\Utils\gpbcmd\createskelprog_main PL0.cgt PL0.pgt PL0.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\Utils\grmCommentCompiler\bin\Debug\grmCommentCompiler.exe PL0.grm PL0.tmpl PL0.cs
echo.

echo Compile: CGT to RES...
..\..\Utils\ResXMaker\ResXMaker.exe PL0.CGT PL0Language PL0Lang.resources
echo.

copy PL0Lang.resources ..\..\Parsers\PL0Parser\PL0Lang.resources
copy PL0.cs ..\..\Parsers\PL0Parser\PL0_lrparser_rules.cs