dotnet build -c Release PascalABCNET.sln

@IF %ERRORLEVEL% NEQ 0 GOTO ERROR

cd ReleaseGenerators
..\bin\pabcnetc RebuildStandartModules.pas /rebuild /noconsole
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR


cd PABCRtl
..\..\bin\pabcnetc PABCRtl.pas /rebuild /noconsole
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\sn.exe -Vr PABCRtl.dll
..\sn.exe -R PABCRtl.dll KeyPair.snk
..\sn.exe -Vu PABCRtl.dll
copy PABCRtl.dll ..\..\bin\Lib

cd ..
ExecHide.exe gacutil.exe /u PABCRtl
ExecHide.exe gacutil.exe /i ..\bin\Lib\PABCRtl.dll

..\bin\pabcnetc RebuildStandartModules.pas /rebuild /noconsole
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR


cd ..\TestSuite
REM MPGORunner.exe
..\bin\TestRunner.exe 1 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\bin\TestRunner.exe 2 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\bin\TestRunner.exe 3 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\bin\TestRunner.exe 4 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\bin\TestRunner.exe 5 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\bin\TestRunner.exe 6 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR

cd ..\TestSuiteAdditionalLanguages\SPythonTests
..\..\bin\TestRunner.exe 1 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\..\bin\TestRunner.exe 2 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\..\bin\TestRunner.exe 3 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\..\bin\TestRunner.exe 4 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\..\bin\TestRunner.exe 5 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\..\bin\TestRunner.exe 6 1
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR

cd ..\..\bin
pabcnetcclear GitIgnoreTester.pas
GitIgnoreTester.exe NoWait
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
cd ..

GOTO EXIT

:ERROR
PAUSE

:EXIT