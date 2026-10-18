@echo off
setlocal

set "ROOT=%~dp0"
set "CROSS_TARGET_RUNNER_SOURCE=%ROOT%CrossTargetTestRunner\CrossTargetTestRunner.pas"
set "TARGET=%~1"
set "MODE=%~2"
set "FILTER=%~3"
set "SKIP_BUILD=%~4"
rem Also allow the switch without an empty native argument (PowerShell 5.1 drops it).
if /I "%FILTER%"=="--skip-build" (
  set "FILTER="
  set "SKIP_BUILD=--skip-build"
)
if not "%SKIP_BUILD%"=="" if /I not "%SKIP_BUILD%"=="--skip-build" goto usage

if "%TARGET%"=="" set "TARGET=net472"
if "%MODE%"=="" set "MODE=all"

if /I "%TARGET%"=="all" goto run_all
if /I "%TARGET%"=="net472" goto run_net472
if /I "%TARGET%"=="net10" goto run_net10

:usage
echo Usage: %~nx0 [net472^|net10^|all] [all^|core^|units^|errors] [file-name-filter] [--skip-build]
exit /b 2

:run_all
call "%~f0" net472 "%MODE%" "%FILTER%" "%SKIP_BUILD%"
set "NET472_EXIT=%ERRORLEVEL%"
call "%~f0" net10 "%MODE%" "%FILTER%" "%SKIP_BUILD%"
set "NET10_EXIT=%ERRORLEVEL%"
if not "%NET472_EXIT%"=="0" exit /b %NET472_EXIT%
exit /b %NET10_EXIT%

:run_net472
echo Building CrossTargetTestRunner for net472...
copy /Y "%CROSS_TARGET_RUNNER_SOURCE%" "%ROOT%bin\CrossTargetTestRunner.pas" >nul
if errorlevel 1 exit /b %ERRORLEVEL%
pushd "%ROOT%bin"
if errorlevel 1 exit /b %ERRORLEVEL%
call pabcnetcclear.exe CrossTargetTestRunner.pas
set "BUILD_EXIT=%ERRORLEVEL%"
popd
del /Q "%ROOT%bin\CrossTargetTestRunner.pas" >nul 2>nul
if not "%BUILD_EXIT%"=="0" exit /b %BUILD_EXIT%
set "RUNNER=%ROOT%bin\CrossTargetTestRunner.exe"
goto run_tests

:run_net10
if /I "%SKIP_BUILD%"=="--skip-build" goto prepared_net10
echo Building CodeCompletion for net10...
dotnet build "%ROOT%CodeCompletion\CodeCompletion.csproj" -c Release -p:TargetFramework=net10.0 -p:PABCNET_LEGACY_ONLY=false -m:1
if errorlevel 1 exit /b %ERRORLEVEL%
goto build_net10_runner

:prepared_net10
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%ROOT%scripts\assert-net10-runtime.ps1" -Configuration Release -RequireCompiledUnits
if errorlevel 1 exit /b %ERRORLEVEL%

:build_net10_runner
echo Building CrossTargetTestRunner for net10...
copy /Y "%CROSS_TARGET_RUNNER_SOURCE%" "%ROOT%bin-net10\CrossTargetTestRunner.pas" >nul
if errorlevel 1 exit /b %ERRORLEVEL%
pushd "%ROOT%bin-net10"
if errorlevel 1 exit /b %ERRORLEVEL%
dotnet "%ROOT%bin-net10\pabcnetcclear.dll" CrossTargetTestRunner.pas
set "BUILD_EXIT=%ERRORLEVEL%"
popd
del /Q "%ROOT%bin-net10\CrossTargetTestRunner.pas" >nul 2>nul
if not "%BUILD_EXIT%"=="0" exit /b %BUILD_EXIT%
set "RUNNER=%ROOT%bin-net10\CrossTargetTestRunner.exe"

:run_tests
echo Running %TARGET% tests: %MODE% %FILTER%
pushd "%ROOT%TestSuite"
if errorlevel 1 exit /b %ERRORLEVEL%
if /I "%TARGET%"=="net10" (
  dotnet "%RUNNER%" "%MODE%" "%FILTER%"
) else (
  call "%RUNNER%" "%MODE%" "%FILTER%"
)
set "TEST_EXIT=%ERRORLEVEL%"
popd
exit /b %TEST_EXIT%
