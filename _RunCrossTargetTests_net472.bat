@echo off
call "%~dp0_RunCrossTargetTests.bat" net472 %*
pause
exit /b %ERRORLEVEL%
