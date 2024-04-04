call Studio.bat /t:rebuild "/property:Configuration=Release" "/p:Platform=Any CPU" PascalABCNET.sln

@IF %ERRORLEVEL% NEQ 0 GOTO ERROR

cd ReleaseGenerators
..\bin\pabcnetc RebuildStandartModules.pas /rebuild
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
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR


cd ..\bin
REM MPGORunner.exe
TestRunner.exe 1
TestRunner.exe 2
TestRunner.exe 3
TestRunner.exe 4
TestRunner.exe 5
TestRunner.exe 6
cd ..

GOTO EXIT

:ERROR
PAUSE

:EXIT