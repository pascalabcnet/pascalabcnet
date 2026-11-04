@echo off
pushd "%~dp0"
if errorlevel 1 exit /b 1
call _GenerateLinuxVersion.bat --net10
set "LinuxNet10ExitCode=%ERRORLEVEL%"
popd
exit /b %LinuxNet10ExitCode%
