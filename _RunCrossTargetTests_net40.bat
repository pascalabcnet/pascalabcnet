@echo off
call "%~dp0_RunCrossTargetTests.bat" net40 %*
pause
exit /b %ERRORLEVEL%
