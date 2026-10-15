@echo off
setlocal
set "CONFIGURATION=%~1"
if "%CONFIGURATION%"=="" set "CONFIGURATION=Debug"
if /I "%~2"=="--skip-build" goto prepared
if not "%~2"=="" exit /b 2
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\rebuild-net10-standard-units.ps1" -Configuration "%CONFIGURATION%"
exit /b %ERRORLEVEL%
:prepared
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\rebuild-net10-standard-units.ps1" -Configuration "%CONFIGURATION%" -SkipBuild
exit /b %ERRORLEVEL%
