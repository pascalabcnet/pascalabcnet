dotnet build -c Release PascalABCNET.sln

@IF %ERRORLEVEL% NEQ 0 GOTO ERROR

cd ReleaseGenerators
..\bin\pabcnetc RebuildStandartModules.pas /rebuild /noconsole

cd ..\bin
pabcnetc PcuDiffTester.pas /noconsole
PcuDiffTester.exe
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
cd ..

GOTO EXIT

:ERROR
PAUSE

:EXIT