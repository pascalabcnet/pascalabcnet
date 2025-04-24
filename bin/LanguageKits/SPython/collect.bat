@echo off

copy ..\..\Lib\SPython*.pas .\

copy ..\..\Highlighting\SPython.xshd .\

call ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\Gplex.exe /unicode ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\SPythonLexer.lex
call ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\Gppg.exe /no-lines /gplex ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\SPythonParser.y

@if %ERRORLEVEL% NEQ 0 GOTO ERROR

call dotnet build -c Release --no-incremental -v d ..\..\..\LanguagePlugins\SPython\SPythonLanguageInfo\SPythonLanguageInfo.csproj

@if %ERRORLEVEL% NEQ 0 GOTO ERROR

copy ..\..\SPython*.dll .\

copy ..\..\Lng\Rus\SPython*.dat .\Lng\Rus\

copy ..\..\Lng\Eng\SPython*.dat .\Lng\Eng\

copy ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\Examples\*.pys .\Examples\
copy ..\..\..\LanguagePlugins\SPython\SPythonParserKrylovMovchan\Examples\*.pas .\Examples\

powershell Compress-Archive -Force . SPython.zip

GOTO EXIT

:ERROR
PAUSE

:EXIT