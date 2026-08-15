@echo off
setlocal

rem Build, test and package the PascalABC.NET console distribution for .NET 10.

set "ROOT=%~dp0"

call "%ROOT%_RebuildStandartModules_net10.bat" Release
if errorlevel 1 goto error

call "%ROOT%_RunConsoleTests.bat" net10
if errorlevel 1 goto error

call "%ROOT%_BuildConsoleNet10Distribution.bat"
if errorlevel 1 goto error

echo.
echo .NET 10 console tests passed.
echo Distribution created: %ROOT%Release\PascalABCNET-Console-net10.zip
exit /b 0

:error
set "GENERATION_EXIT=%ERRORLEVEL%"
echo.
echo ERROR: .NET 10 console distribution was not created.
exit /b %GENERATION_EXIT%