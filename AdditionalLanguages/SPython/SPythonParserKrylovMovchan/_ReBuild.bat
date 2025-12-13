call generateParser.bat
dotnet build -c Release --no-incremental -v d SPythonParser.csproj

@IF %ERRORLEVEL% NEQ 0 PAUSE