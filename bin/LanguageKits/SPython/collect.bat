@echo off

copy ..\..\Lib\SPythonSystem.pas .\
copy ..\..\Lib\SPythonHidden.pas .\

copy ..\..\Highlighting\SPython.xshd .\
copy ..\..\Highlighting\PythonABC.xshd .\

call ..\..\..\Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" ..\..\..\Parsers\SPythonParserKrylovMovchan.sln
call ..\..\..\Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" ..\..\..\SemanticAnalyzers\SPythonSyntaxTreeVisitor\SPythonSyntaxTreeVisitor.csproj

copy ..\..\SPythonLanguageParser.dll .\
copy ..\..\SPythonSyntaxTreeVisitor.dll .\

copy ..\..\Lng\Rus\SPythonParser.dat .\Lng\Rus\
copy ..\..\Lng\Rus\SPythonSemantic.dat .\Lng\Rus\

copy ..\..\Lng\Eng\SPythonParser.dat .\Lng\Eng\
copy ..\..\Lng\Eng\SPythonSemantic.dat .\Lng\Eng\

powershell Compress-Archive . SPython.zip