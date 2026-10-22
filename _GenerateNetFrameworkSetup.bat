@echo off
setlocal
rem Prepare, test and package only the ordinary .NET Framework distribution.
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\generate-all-setups.ps1" -Target NetFramework %*
pause
exit /b %ERRORLEVEL%
