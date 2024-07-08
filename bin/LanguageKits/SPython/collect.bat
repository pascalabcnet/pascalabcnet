@echo off

copy ..\..\Lib\SPythonSystem.pas .\
copy ..\..\Lib\SpythonHidden.pas .\

copy ..\..\Highlighting\SPython.xshd .\
copy ..\..\Highlighting\PythonABC.xshd .\

call ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\Gplex.exe /unicode ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\SPythonLexer.lex
call ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\Gppg.exe /no-lines /gplex ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\SPythonParser.y

@if %ERRORLEVEL% NEQ 0 GOTO ERROR

call ..\..\..\Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\SPythonParser.csproj
call ..\..\..\Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" ..\..\..\LanguagePlugins\SPython\SemanticAnalyzers\SPythonSyntaxTreeVisitor\SPythonSyntaxTreeVisitor.csproj

@if %ERRORLEVEL% NEQ 0 GOTO ERROR

copy ..\..\SPythonLanguageInfo.dll .\
copy ..\..\SPythonLanguageParser.dll .\
copy ..\..\SPythonSyntaxTreeVisitor.dll .\
copy ..\..\SPythonStandardTreeConverter.dll .\

copy ..\..\Lng\Rus\SPythonParser.dat .\Lng\Rus\
copy ..\..\Lng\Rus\SPythonSemantic.dat .\Lng\Rus\

copy ..\..\Lng\Eng\SPythonParser.dat .\Lng\Eng\
copy ..\..\Lng\Eng\SPythonSemantic.dat .\Lng\Eng\

powershell Compress-Archive -Force . SPython.zip

GOTO EXIT

:ERROR
PAUSE

:EXIT