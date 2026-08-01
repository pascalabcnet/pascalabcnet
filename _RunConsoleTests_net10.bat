@echo off
call "%~dp0_RunConsoleTests.bat" net10 %*
pause
exit /b %ERRORLEVEL%
