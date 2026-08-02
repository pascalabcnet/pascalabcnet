dotnet build -c Release --no-incremental -v d PascalABCNET.sln -p:PABCNET_LEGACY_ONLY=true

@IF %ERRORLEVEL% NEQ 0 PAUSE
