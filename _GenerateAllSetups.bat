cd ReleaseGenerators\Samples\
del Pas /s /q
del BF /s  /q
del PL0 /s /q
del PNET /s /q
GetSamples.exe "..\..\InstallerSamples" Pas\
GetSamples.exe _svn\BF BF
GetSamples.exe _svn\PL0\ PL0\
GetSamples.exe _svn\PNET\ PNET\
cd ..\..

cd utils\DefaultLanguageResMaker\
LanguageResMaker.exe
cd ..\..

Utils\IncrementVresion\IncrementVresion.exe Configuration\Version.defs REVISION 1
Utils\ReplaceInFiles\ReplaceInFiles.exe Configuration\Version.defs Configuration\GlobalAssemblyInfo.cs.tmpl Configuration\GlobalAssemblyInfo.cs
Utils\ReplaceInFiles\ReplaceInFiles.exe Configuration\Version.defs ReleaseGenerators\PascalABCNET_version.nsh.tmpl ReleaseGenerators\PascalABCNET_version.nsh

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

cd ..\bin
MPGORunner.exe
TestRunner.exe
cd ..\ReleaseGenerators
PascalABCNET_ALL.bat
GOTO EXIT

:ERROR
PAUSE

:EXIT