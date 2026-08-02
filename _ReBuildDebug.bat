dotnet build --no-incremental PascalABCNET.sln -p:PABCNET_LEGACY_ONLY=true

@IF %ERRORLEVEL% NEQ 0 PAUSE
