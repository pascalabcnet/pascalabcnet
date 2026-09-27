@echo off
call "%~dp0_RunCrossTargetTests.bat" net10 %*
pause
exit /b %ERRORLEVEL%
