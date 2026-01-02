dotnet build -c Release --no-incremental PascalABCNET.sln

@IF %ERRORLEVEL% NEQ 0 GOTO ERROR

cd ReleaseGenerators
..\bin\pabcnetc RebuildStandartModules.pas /rebuild
..\bin\pabcnetc RebuildStandartModulesSPython.pas /rebuild
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR


cd PABCRtl
..\..\bin\pabcnetc PABCRtl.pas /rebuild
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\sn.exe -Vr PABCRtl.dll
..\sn.exe -R PABCRtl.dll KeyPair.snk
..\sn.exe -Vu PABCRtl.dll
copy PABCRtl.dll ..\..\bin\Lib

cd ..
ExecHide.exe gacutil.exe /u PABCRtl
ExecHide.exe gacutil.exe /i ..\bin\Lib\PABCRtl.dll

..\bin\pabcnetc RebuildStandartModules.pas /rebuild
..\bin\pabcnetc RebuildStandartModulesSPython.pas /rebuild
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR


cd ..\TestSuite
REM ..\bin\MPGORunner.exe
..\bin\TestRunner.exe 1
..\bin\TestRunner.exe 2
..\bin\TestRunner.exe 3
..\bin\TestRunner.exe 4
..\bin\TestRunner.exe 5
..\bin\TestRunner.exe 6

cd ..\TestSuiteAdditionalLanguages\SPythonTests
..\..\bin\TestRunner.exe 1
..\..\bin\TestRunner.exe 2
..\..\bin\TestRunner.exe 3
..\..\bin\TestRunner.exe 4
..\..\bin\TestRunner.exe 5
..\..\bin\TestRunner.exe 6
cd ..\..

GOTO EXIT

:ERROR
PAUSE

:EXIT