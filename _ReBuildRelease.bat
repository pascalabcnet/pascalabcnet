%windir%\microsoft.net\framework\v4.0.30319\msbuild /t:rebuild /verbosity:d /property:Configuration=Release PascalABCNET.sln
@IF %ERRORLEVEL% NEQ 0 PAUSE