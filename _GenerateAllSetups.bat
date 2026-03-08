cd ReleaseGenerators\Samples\
del Pas /s /q
del BF /s  /q
del PL0 /s /q
GetSamples.exe "..\..\InstallerSamples" Pas\
GetSamples.exe _svn\BF BF
GetSamples.exe _svn\PL0\ PL0\
cd ..\..

Utils\IncrementVresion\IncrementVresion.exe Configuration\Version.defs REVISION 1
Utils\ReplaceInFiles\ReplaceInFiles.exe Configuration\Version.defs Configuration\GlobalAssemblyInfo.cs.tmpl Configuration\GlobalAssemblyInfo.cs
Utils\ReplaceInFiles\ReplaceInFiles.exe Configuration\Version.defs ReleaseGenerators\PascalABCNET_version.nsh.tmpl ReleaseGenerators\PascalABCNET_version.nsh
Utils\ReplaceInFiles\ReplaceInFiles.exe Configuration\Version.defs Configuration\pabcversion.txt.tmpl Release\pabcversion.txt

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


cd ..\..\ReleaseGenerators
call PascalABCNET_ALL.bat

GOTO EXIT

:ERROR
PAUSE

:EXIT
