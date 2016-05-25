"%ProgramFiles(x86)%\MSBuild\14.0\Bin\msbuild.exe" /t:rebuild /verbosity:d /property:Configuration=Release PascalABCNET.sln
@IF %ERRORLEVEL% NEQ 0 PAUSE