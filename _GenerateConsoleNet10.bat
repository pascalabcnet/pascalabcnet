@echo off
setlocal

rem Build, test and package the PascalABC.NET console distribution for .NET 10.

set "ROOT=%~dp0"

call "%ROOT%_RebuildStandartModules_net10.bat" Release
if errorlevel 1 goto error

rem The preceding step prepared and verified the complete Release runtime.
rem Test and package that exact kit without rebuilding individual DLLs.
call "%ROOT%_RunCrossTargetTests.bat" net10 all --skip-build
if errorlevel 1 goto error

call "%ROOT%_BuildConsoleNet10Distribution.bat" --skip-build
if errorlevel 1 goto error

echo.
echo .NET 10 console tests passed.
echo Distribution created: %ROOT%Release\PascalABCNET-Console-net10.zip
exit /b 0

:error
set "GENERATION_EXIT=%ERRORLEVEL%"
echo.
echo ERROR: .NET 10 console distribution was not created.
pause
exit /b %GENERATION_EXIT%
