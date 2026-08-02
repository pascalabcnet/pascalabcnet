@echo off
setlocal

set "ROOT=%~dp0"

echo Building full legacy TestRunner...
pushd "%ROOT%bin"
call pabcnetcclear.exe TestRunner.pas
set "BUILD_EXIT=%ERRORLEVEL%"
popd
if not "%BUILD_EXIT%"=="0" exit /b %BUILD_EXIT%

echo Full legacy TestRunner built successfully.
exit /b 0
