cd ReleaseGenerators\Samples\
del Pas /s /q
del BF /s  /q
del PL0 /s /q
GetSamples.exe "..\..\InstallerSamples" Pas\
GetSamples.exe _svn\BF BF
GetSamples.exe _svn\PL0\ PL0\
cd ..\..

cd utils\DefaultLanguageResMaker\
LanguageResMaker.exe
cd ..\..

Utils\IncrementVresion\IncrementVresion.exe Configuration\Version.defs REVISION 1
Utils\ReplaceInFiles\ReplaceInFiles.exe Configuration\Version.defs Configuration\GlobalAssemblyInfo.cs.tmpl Configuration\GlobalAssemblyInfo.cs
Utils\ReplaceInFiles\ReplaceInFiles.exe Configuration\Version.defs ReleaseGenerators\PascalABCNET_version.nsh.tmpl ReleaseGenerators\PascalABCNET_version.nsh
Utils\ReplaceInFiles\ReplaceInFiles.exe Configuration\Version.defs Configuration\pabcversion.txt.tmpl Release\pabcversion.txt

call Studio.bat /t:rebuild "/property:Configuration=Release" PascalABCNET.sln

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
TestRunner.exe 1
TestRunner.exe 2
TestRunner.exe 3
TestRunner.exe 4
TestRunner.exe 5
TestRunner.exe 6

cd ..\ReleaseGenerators
call PascalABCNET_ALL.bat

cd ..
call Studio.bat /t:rebuild "/property:Configuration=Release" PascalABCNET_40.sln
cd ReleaseGenerators
call PascalABCNETWithDotNet40.bat

cd ..
call Studio.bat /t:rebuild "/property:Configuration=Release" PascalABCNET.sln

GOTO EXIT

:ERROR
PAUSE

:EXIT
