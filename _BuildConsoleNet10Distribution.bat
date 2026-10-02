@echo off
setlocal

rem Build the complete PascalABC.NET console distribution for .NET 10.

set "ROOT=%~dp0"
set "MODERN_BIN=%ROOT%bin-net10"
set "PACKAGE_NAME=PascalABCNET-Console-net10"
set "RELEASE_DIR=%ROOT%Release"
set "STAGE_DIR=%RELEASE_DIR%\%PACKAGE_NAME%"
set "ZIP_FILE=%RELEASE_DIR%\%PACKAGE_NAME%.zip"
set "ASSETS_DIR=%ROOT%ConsoleDistribution"
set "SAMPLES_DIR=%ROOT%InstallerSamples"

call "%ROOT%_RebuildStandartModules_net10.bat" Release
if errorlevel 1 exit /b %ERRORLEVEL%

if exist "%STAGE_DIR%" rmdir /S /Q "%STAGE_DIR%"
mkdir "%STAGE_DIR%"
if errorlevel 1 exit /b %ERRORLEVEL%

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%ROOT%scripts\export-net10-runtime.ps1" -Destination "%STAGE_DIR%" -IncludeCompiledUnits
if errorlevel 1 exit /b %ERRORLEVEL%
xcopy "%SAMPLES_DIR%" "%STAGE_DIR%\Samples\" /E /I /Y /Q >nul
if errorlevel 1 exit /b %ERRORLEVEL%
copy /Y "%ASSETS_DIR%\README.txt" "%STAGE_DIR%\README.txt" >nul

if exist "%ZIP_FILE%" del /Q "%ZIP_FILE%"
powershell.exe -NoProfile -Command "Compress-Archive -LiteralPath '%STAGE_DIR%' -DestinationPath '%ZIP_FILE%' -CompressionLevel Optimal"
if errorlevel 1 exit /b %ERRORLEVEL%

if not exist "%ZIP_FILE%" (
  echo ERROR: ZIP file was not created.
  exit /b 1
)

echo Created: %ZIP_FILE%
exit /b 0
