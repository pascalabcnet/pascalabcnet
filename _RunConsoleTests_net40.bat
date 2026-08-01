@echo off
call "%~dp0_RunConsoleTests.bat" net40 %*
exit /b %ERRORLEVEL%
