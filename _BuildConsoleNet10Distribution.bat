@echo off
setlocal
if /I "%~1"=="--skip-build" goto prepared
if not "%~1"=="" exit /b 2
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\build-console-net10-distribution.ps1"
exit /b %ERRORLEVEL%
:prepared
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\build-console-net10-distribution.ps1" -SkipBuild
exit /b %ERRORLEVEL%
