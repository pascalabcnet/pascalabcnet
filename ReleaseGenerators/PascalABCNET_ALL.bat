@echo off
setlocal
pushd "%~dp0"
if errorlevel 1 exit /b %ERRORLEVEL%
"..\utils\NSIS2\makensis.exe" PascalABCNETStandart.nsi
if errorlevel 1 goto failed
copy /Y "..\Release\PascalABCNETSetup.exe" "..\Release\PascalABCNETWithDotNetSetup.exe" >nul
if errorlevel 1 goto failed
copy /Y "..\Release\PascalABCNETSetup.exe" "..\Release\PascalABCNETMiniSetup.exe" >nul
if errorlevel 1 goto failed
call "%~dp0PascalABCNETConsoleZIP.bat"
if errorlevel 1 goto failed
popd
exit /b 0
:failed
set "PACKAGE_EXIT=%ERRORLEVEL%"
popd
exit /b %PACKAGE_EXIT%
