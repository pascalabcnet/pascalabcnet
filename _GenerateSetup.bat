%windir%\microsoft.net\framework\v4.0.30319\msbuild /t:rebuild /property:Configuration=Release PascalABCNET.sln
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR

cd ReleaseGenerators
..\bin\pabcnetc RebuildStandartModules.pas /rebuild
@IF %ERRORLEVEL% NEQ 0 GOTO EXIT

PascalABCNETStandart.bat
GOTO EXIT

:ERROR
PAUSE

:EXIT