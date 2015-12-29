%windir%\microsoft.net\framework\v4.0.30319\msbuild /t:rebuild /property:Configuration=Release PascalABCNET.sln
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

..\..\bin\pabcnetc PABCRtl32.pas /rebuild
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
..\sn.exe -Vr PABCRtl32.dll
..\sn.exe -R PABCRtl32.dll KeyPair32.snk
..\sn.exe -Vu PABCRtl32.dll
copy PABCRtl32.dll ..\..\bin\Lib
cd ..
ExecHide.exe gacutil.exe /u PABCRtl
ExecHide.exe gacutil.exe /i ..\bin\Lib\PABCRtl.dll

..\bin\pabcnetc RebuildStandartModules.pas /rebuild
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR

cd ..\bin
REM MPGORunner.exe
TestRunner.exe
cd ..
GOTO EXIT

:ERROR
PAUSE

:EXIT