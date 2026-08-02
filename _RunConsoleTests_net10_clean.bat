@echo off
setlocal

rem NET10-TESTFIX infrastructure: verify tests from freshly rebuilt modern standard PCU files.
call "%~dp0_RebuildStandartModules_net10.bat"
set "REBUILD_EXIT=%ERRORLEVEL%"
if not "%REBUILD_EXIT%"=="0" goto finish

call "%~dp0_RunConsoleTests.bat" net10 %*
set "TEST_EXIT=%ERRORLEVEL%"

:finish
if not defined TEST_EXIT set "TEST_EXIT=%REBUILD_EXIT%"
pause
exit /b %TEST_EXIT%
