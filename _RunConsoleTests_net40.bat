@echo off
call "%~dp0_RunConsoleTests.bat" net40 %*
pause
exit /b %ERRORLEVEL%
