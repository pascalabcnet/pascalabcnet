%windir%\microsoft.net\framework\v4.0.30319\msbuild /t:rebuild PascalABCNET.sln
@IF %ERRORLEVEL% NEQ 0 PAUSE