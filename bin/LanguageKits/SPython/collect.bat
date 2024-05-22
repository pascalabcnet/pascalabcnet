@echo off

copy ..\..\Lib\SPythonSystem.pas .\
copy ..\..\Lib\SpythonHidden.pas .\

copy ..\..\Highlighting\SPython.xshd .\
copy ..\..\Highlighting\PythonABC.xshd .\

call ..\..\..\Parsers\SPythonParserKrylovMovchan\Gplex.exe /unicode ..\..\..\Parsers\SPythonParserKrylovMovchan\SPythonLexer.lex
call ..\..\..\Parsers\SPythonParserKrylovMovchan\Gppg.exe /no-lines /gplex ..\..\..\Parsers\SPythonParserKrylovMovchan\SPythonParser.y

call ..\..\..\Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" ..\..\..\Parsers\SPythonParserKrylovMovchan\SPythonParser.csproj
call ..\..\..\Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" ..\..\..\SemanticAnalyzers\SPythonSyntaxTreeVisitor\SPythonSyntaxTreeVisitor.csproj

copy ..\..\SPythonLanguageParser.dll .\
copy ..\..\SPythonSyntaxTreeVisitor.dll .\

copy ..\..\Lng\Rus\SPythonParser.dat .\Lng\Rus\
copy ..\..\Lng\Rus\SPythonSemantic.dat .\Lng\Rus\

copy ..\..\Lng\Eng\SPythonParser.dat .\Lng\Eng\
copy ..\..\Lng\Eng\SPythonSemantic.dat .\Lng\Eng\

powershell Compress-Archive . SPython.zip