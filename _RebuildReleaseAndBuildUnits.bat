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

rem ..\bin\pabcnetc RebuildStandartModules.pas /rebuild
rem @IF %ERRORLEVEL% NEQ 0 GOTO ERROR


cd ..\bin
REM MPGORunner.exe
cd ..
GOTO EXIT

:ERROR
PAUSE

:EXIT