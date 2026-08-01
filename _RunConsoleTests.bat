@echo off
setlocal

set "ROOT=%~dp0"
set "TARGET=%~1"
set "MODE=%~2"
set "FILTER=%~3"

if "%TARGET%"=="" set "TARGET=net40"
if "%MODE%"=="" set "MODE=all"

if /I "%TARGET%"=="all" goto run_all
if /I "%TARGET%"=="net40" goto run_net40
if /I "%TARGET%"=="net10" goto run_net10

:usage
echo Usage: %~nx0 [net40^|net10^|all] [all^|core^|units^|errors] [file-name-filter]
exit /b 2

:run_all
call "%~f0" net40 "%MODE%" "%FILTER%"
set "NET40_EXIT=%ERRORLEVEL%"
call "%~f0" net10 "%MODE%" "%FILTER%"
set "NET10_EXIT=%ERRORLEVEL%"
if not "%NET40_EXIT%"=="0" exit /b %NET40_EXIT%
exit /b %NET10_EXIT%

:run_net40
echo Building TestRunner for net40...
pushd "%ROOT%bin"
call pabcnetcclear.exe TestRunner.pas
set "BUILD_EXIT=%ERRORLEVEL%"
popd
if not "%BUILD_EXIT%"=="0" exit /b %BUILD_EXIT%
set "RUNNER=%ROOT%bin\TestRunner.exe"
goto run_tests

:run_net10
echo Building TestRunner for net10...
copy /Y "%ROOT%bin\TestRunner.pas" "%ROOT%bin-net10\TestRunner.pas" >nul
pushd "%ROOT%bin-net10"
call pabcnetcclear.exe TestRunner.pas
set "BUILD_EXIT=%ERRORLEVEL%"
popd
del /Q "%ROOT%bin-net10\TestRunner.pas" >nul 2>nul
if not "%BUILD_EXIT%"=="0" exit /b %BUILD_EXIT%
set "RUNNER=%ROOT%bin-net10\TestRunner.exe"

:run_tests
echo Running %TARGET% tests: %MODE% %FILTER%
pushd "%ROOT%TestSuite"
if /I "%TARGET%"=="net10" (
  dotnet "%RUNNER%" "%MODE%" "%FILTER%"
) else (
  call "%RUNNER%" "%MODE%" "%FILTER%"
)
set "TEST_EXIT=%ERRORLEVEL%"
popd
exit /b %TEST_EXIT%
