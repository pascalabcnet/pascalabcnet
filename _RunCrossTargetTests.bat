@echo off
setlocal

set "ROOT=%~dp0"
set "CROSS_TARGET_RUNNER_SOURCE=%ROOT%CrossTargetTestRunner\CrossTargetTestRunner.pas"
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
echo Building CrossTargetTestRunner for net40...
copy /Y "%CROSS_TARGET_RUNNER_SOURCE%" "%ROOT%bin\CrossTargetTestRunner.pas" >nul
pushd "%ROOT%bin"
call pabcnetcclear.exe CrossTargetTestRunner.pas
set "BUILD_EXIT=%ERRORLEVEL%"
popd
del /Q "%ROOT%bin\CrossTargetTestRunner.pas" >nul 2>nul
if not "%BUILD_EXIT%"=="0" exit /b %BUILD_EXIT%
set "RUNNER=%ROOT%bin\CrossTargetTestRunner.exe"
goto run_tests

:run_net10
echo Building CrossTargetTestRunner for net10...
copy /Y "%CROSS_TARGET_RUNNER_SOURCE%" "%ROOT%bin-net10\CrossTargetTestRunner.pas" >nul
pushd "%ROOT%bin-net10"
call pabcnetcclear.exe CrossTargetTestRunner.pas
set "BUILD_EXIT=%ERRORLEVEL%"
popd
del /Q "%ROOT%bin-net10\CrossTargetTestRunner.pas" >nul 2>nul
if not "%BUILD_EXIT%"=="0" exit /b %BUILD_EXIT%
set "RUNNER=%ROOT%bin-net10\CrossTargetTestRunner.exe"

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
