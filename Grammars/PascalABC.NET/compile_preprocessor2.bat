cls
@echo off
echo [C] GRM to CS,RES compiler. (c) DarkStar 2007
echo.

echo Delete old files...
del Preprocessor2.cs 
del Preprocessor2.cgt 
del Preprocessor2.cgt.zip
del PABCPreprocessor2Lang.resources

echo Compile: GRM to CGT...
start /wait ..\..\utils\gpbcmd\goldbuilder_main Preprocessor2.grm Preprocessor2.cgt Preprocessor2.log
copy Preprocessor2.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\utils\gpbcmd\createskelprog_main Preprocessor2.cgt Preprocessor2.pgt Preprocessor2.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\utils\grmCommentCompiler\bin\debug\grmCommentCompiler Preprocessor2.grm Preprocessor2.tmpl Preprocessor2.cs
echo.

echo Compile: CGT to RES...
..\..\utils\ResXMaker\ResXMaker.exe Preprocessor2.CGT PABCPreprocessor2Language PABCPreprocessor2Lang.resources
echo.

copy PABCPreprocessor2Lang.resources ..\..\Parsers\PascalABCParser\Preprocessor_2\PABCPreprocessor2Lang.resources
copy Preprocessor2.cs ..\..\Parsers\PascalABCParser\Preprocessor_2\PABCPreprocessor2_lrparser.cs