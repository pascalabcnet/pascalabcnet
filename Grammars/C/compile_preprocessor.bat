cls
@echo off
echo [C] GRM to CS,RES compiler. (c) DarkStar 2007
echo.

echo Delete old files...
del CPreprocessor.cs 
del CPreprocessor.cgt 
del CPreprocessor.cgt.zip
del CPreprocessorLang.resources

echo Compile: GRM to CGT...
start /wait ..\..\utils\gpbcmd\goldbuilder_main CPreprocessor.grm CPreprocessor.cgt CPreprocessor.log
copy CPreprocessor.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\utils\gpbcmd\createskelprog_main CPreprocessor.cgt CPreprocessor.pgt CPreprocessor.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\utils\grmCommentCompiler\bin\debug\grmCommentCompiler CPreprocessor.grm CPreprocessor.tmpl CPreprocessor.cs
echo.

echo Compile: CGT to RES...
..\..\utils\ResXMaker\ResXMaker.exe CPreprocessor.CGT CPreprocessorLanguage CPreprocessorLang.resources
echo.

copy CPreprocessorLang.resources ..\..\Parsers\CParser\Preprocessor\CPreprocessorLang.resources
copy CPreprocessor.cs ..\..\Parsers\CParser\Preprocessor\CPreprocessor_lrparser.cs