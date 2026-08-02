@echo off
setlocal

rem NET10-TESTFIX infrastructure: rebuild modern standard PCU files from sources.

set "ROOT=%~dp0"
set "MODERN_BIN=%ROOT%bin-net10"
set "MODERN_LIB=%MODERN_BIN%\Lib"
set "REBUILD_DIR=%ROOT%ReleaseGenerators"
set "REBUILD_NAME=RebuildStandartModulesNet10"

echo Building console compiler for net10.0...
dotnet build "%ROOT%pabcnetc.sln" -c Debug -p:TargetFramework=net10.0 -m:1
if errorlevel 1 exit /b %ERRORLEVEL%

echo Removing existing net10 PCU files...
if exist "%MODERN_LIB%" (
  for /R "%MODERN_LIB%" %%F in (*.pcu) do del /Q "%%F"
)

echo Rebuilding base standard modules for net10.0...
pushd "%REBUILD_DIR%"
rem pabcnetcclear has no legacy /rebuild switch; deleting PCU files above forces a source rebuild.
call "%MODERN_BIN%\pabcnetcclear.exe" "%REBUILD_NAME%.pas"
set "REBUILD_EXIT=%ERRORLEVEL%"
popd
if not "%REBUILD_EXIT%"=="0" exit /b %REBUILD_EXIT%

for %%M in (
  PABCSystem PABCExtensions __RunMode NumLibABC IniFile Utils Timers Countries
  ABCDatabases School SF DataFrameABC DataFrameABCCore LinearAlgebraML
  PreprocessorABC MetricsABC MLABC MLCoreABC MLModelsABC ValidationML
  MLExceptions InspectionML MLPipelineABC MLDatasets DataAdapters MLUtilsABC
) do (
  call :require_pcu "%%M"
  if errorlevel 1 exit /b 1
)

del /Q "%REBUILD_DIR%\%REBUILD_NAME%.exe" >nul 2>nul
del /Q "%REBUILD_DIR%\%REBUILD_NAME%.exe.config" >nul 2>nul
del /Q "%REBUILD_DIR%\%REBUILD_NAME%.pdb" >nul 2>nul
del /Q "%REBUILD_DIR%\%REBUILD_NAME%.runtimeconfig.json" >nul 2>nul

echo Base net10.0 standard modules rebuilt successfully.
exit /b 0

:require_pcu
dir /B /S "%MODERN_LIB%\%~1.pcu" >nul 2>nul
if errorlevel 1 (
  echo ERROR: %~1.pcu was not created.
  exit /b 1
)
exit /b 0
