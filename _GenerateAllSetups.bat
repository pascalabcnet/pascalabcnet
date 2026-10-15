@echo off
setlocal
rem One preparation pass, then tests and packaging. Arguments are PowerShell options.
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\generate-all-setups.ps1" %*
exit /b %ERRORLEVEL%
