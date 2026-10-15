@echo off
setlocal
set "ROOT=%~dp0.."
pushd "%ROOT%\bin"
if errorlevel 1 exit /b %ERRORLEVEL%
rem Recreate the archive: updating an old ZIP retains removed files.
if exist "..\Release\PABCNETC.zip" del /Q "..\Release\PABCNETC.zip"
if errorlevel 1 goto failed
"..\utils\pkzipc\pkzipc.exe" -add "..\Release\PABCNETC.zip" pabcnetc.exe pabcnetcclear.exe Compiler.dll CompilerTools.dll Errors.dll Localization.dll NETGenerator.dll ParserTools.dll ICSharpCode.NRefactory.dll SemanticTree.dll SyntaxTree.dll TreeConverter.dll OptimizerConversion.dll PascalABCParser.dll PascalABCLanguageInfo.dll licence.txt PascalABCNET.chm System.Threading.dll SyntaxTreeConverters.dll PABCCoreUtils.dll SyntaxVisitors.dll LambdaAnySynToSemConverter.dll LanguageIntegrator.dll StringConstants.dll
if errorlevel 1 goto failed
for %%D in ("Lib\*.pcu" "Lib\SPython\*.pcu" "Lng\*.dat" "Lng\*.LanguageName" "doc\*.*") do (
  "..\utils\pkzipc\pkzipc.exe" -add -nozip -dir "..\Release\PABCNETC.zip" %%D
  if errorlevel 1 goto failed
)
popd
pushd "%~dp0"
if errorlevel 1 exit /b %ERRORLEVEL%
"..\utils\pkzipc\pkzipc.exe" -add -nozip -dir "..\Release\PABCNETC.zip" install_pabcnetc.bat
if errorlevel 1 goto failed
popd
exit /b 0
:failed
set "PACKAGE_EXIT=%ERRORLEVEL%"
popd
exit /b %PACKAGE_EXIT%
