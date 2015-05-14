cls
@echo off
echo [KuMir] GRM to CS,RES compiler.
echo.

echo Delete old files...
del KuMir.cs 
del KuMir.cgt 
del KuMirLang.resources
del KuMir.tmpl
del KuMir.log

echo Compile: GRM to CGT...
start /wait ..\..\Utils\gpbcmd\goldbuilder_main KuMir.grm KuMir.cgt
copy KuMir.log con
echo.

echo Compile: CGT,PGT to TEMPLATE...
start /wait ..\..\Utils\gpbcmd\createskelprog_main KuMir.cgt KuMir.pgt KuMir.tmpl
echo.

echo Compile: GRM,TEMPLATE to CS...
..\..\Utils\grmCommentCompiler\bin\Debug\grmCommentCompiler.exe KuMir.grm KuMir.tmpl KuMir.cs
echo.

echo Compile: CGT to RES...
start /wait ..\..\Utils\ResXMaker\ResXMaker.exe KuMir.CGT KuMirLanguage KuMirLang.resources
echo.

copy KuMirLang.resources ..\..\Parsers\KuMirParser\KuMirLang.resources
copy KuMir.cs ..\..\Parsers\KuMirParser\KuMir_lrparser_rules.cs