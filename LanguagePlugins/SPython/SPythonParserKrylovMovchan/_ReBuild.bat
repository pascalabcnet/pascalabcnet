call generateParser.bat
dotnet build -c Release --no-incremental -v d SPythonParser.sln

@IF %ERRORLEVEL% NEQ 0 PAUSE
move obj\Release\SPythonParser.dll ..\..\..\bin\
move obj\Release\SPythonParser.pdb ..\..\..\bin\
@IF %ERRORLEVEL% NEQ 0 PAUSE