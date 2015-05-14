cls
@echo off
echo [C] GRM to CS,RES compiler. (c) DarkStar 2007
echo.

echo Delete old files...
del PascalPreprocessor.cs 
del PascalPreprocessor.cgt 
del PascalPreprocessor.cgt.zip
del PascalPreprocessorLang.resources

echo Compile: GRM to CGT...
start /wait ..\..\utils\gpbcmd\goldbuilder_main PascalPreprocessor.grm PascalPreprocessor.cgt PascalPreprocessor.log
copy PascalPreprocessor.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\utils\gpbcmd\createskelprog_main PascalPreprocessor.cgt PascalPreprocessor.pgt PascalPreprocessor.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\utils\grmCommentCompiler\bin\debug\grmCommentCompiler PascalPreprocessor.grm PascalPreprocessor.tmpl PascalPreprocessor.cs
echo.

echo Compile: CGT to RES...
..\..\utils\ResXMaker\ResXMaker.exe PascalPreprocessor.CGT PascalPreprocessorLanguage PascalPreprocessorLang.resources
echo.

copy PascalPreprocessorLang.resources ..\..\Parsers\PascalABCParser\Preprocessor\PascalPreprocessorLang.resources
copy PascalPreprocessor.cs ..\..\Parsers\PascalABCParser\Preprocessor\PascalPreprocessor_lrparser.cs