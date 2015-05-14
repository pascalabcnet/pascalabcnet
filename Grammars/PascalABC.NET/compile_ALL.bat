cls
@echo off
echo [PascalABC.NET] GRM to CS,RES compiler. (c) DarkStar 2006-2008
echo.

echo Delete old files...
del PascalABC.cs 
del PascalABC.cgt 
del PascalABCLang.resources

echo Compile: GRM to CGT...
start /wait ..\..\utils\gpbcmd\goldbuilder_main PascalABC.grm PascalABC.cgt 
copy pascalabc.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\utils\gpbcmd\createskelprog_main PascalABC.cgt PascalABC.pgt PascalABC.tmpl
start /wait ..\..\utils\gpbcmd\createskelprog_main PascalABC.cgt PascalABC_part.pgt PascalABC_part.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\utils\grmCommentCompiler\bin\debug\grmCommentCompiler PascalABC.grm PascalABC.tmpl PascalABC.cs
..\..\utils\grmCommentCompiler\bin\debug\grmCommentCompiler PascalABC.grm PascalABC_part.tmpl PascalABC_part.cs
echo.

echo Compile: CGT to RES...
..\..\utils\ResXMaker\ResXMaker.exe PascalABC.CGT PascalABCLanguage PascalABCLang.resources
echo.

copy PascalABCLang.resources ..\..\Parsers\PascalABCParser\PascalABCLang.resources
copy PascalABCLang.resources ..\..\Parsers\PascalABCPartParser\PascalABCLang.resources
copy PascalABC.cs ..\..\Parsers\PascalABCParser\pascalabc_lrparser_rules.cs
copy PascalABC_part.cs ..\..\Parsers\PascalABCPartParser\_COPY\pascalabc_lrparser_rules.cs