@echo off
setlocal

set "ROOT=%~dp0"
set "MODERN_BIN=%ROOT%bin-net10"
set "PACKAGE_NAME=PascalABCNET-Console-net10"
set "RELEASE_DIR=%ROOT%Release"
set "STAGE_DIR=%RELEASE_DIR%\%PACKAGE_NAME%"
set "ZIP_FILE=%RELEASE_DIR%\%PACKAGE_NAME%.zip"
set "ASSETS_DIR=%ROOT%ConsoleDistribution"

call "%ROOT%_RebuildStandartModules_net10.bat" Release
if errorlevel 1 exit /b %ERRORLEVEL%

if exist "%STAGE_DIR%" rmdir /S /Q "%STAGE_DIR%"
mkdir "%STAGE_DIR%"
if errorlevel 1 exit /b %ERRORLEVEL%

copy /Y "%MODERN_BIN%\*.dll" "%STAGE_DIR%\" >nul
copy /Y "%MODERN_BIN%\pabcnetc.exe" "%STAGE_DIR%\" >nul
copy /Y "%MODERN_BIN%\pabcnetc.deps.json" "%STAGE_DIR%\" >nul
copy /Y "%MODERN_BIN%\pabcnetc.runtimeconfig.json" "%STAGE_DIR%\" >nul
copy /Y "%MODERN_BIN%\pabcnetc.dll.config" "%STAGE_DIR%\" >nul
copy /Y "%MODERN_BIN%\pabcnetcclear.exe" "%STAGE_DIR%\" >nul
copy /Y "%MODERN_BIN%\pabcnetcclear.deps.json" "%STAGE_DIR%\" >nul
copy /Y "%MODERN_BIN%\pabcnetcclear.runtimeconfig.json" "%STAGE_DIR%\" >nul
copy /Y "%MODERN_BIN%\pabcnetcclear.dll.config" "%STAGE_DIR%\" >nul

xcopy "%MODERN_BIN%\Lib" "%STAGE_DIR%\Lib\" /E /I /Y /Q >nul
if errorlevel 1 exit /b %ERRORLEVEL%
xcopy "%MODERN_BIN%\Lng" "%STAGE_DIR%\Lng\" /E /I /Y /Q >nul
if errorlevel 1 exit /b %ERRORLEVEL%
xcopy "%ASSETS_DIR%\Examples" "%STAGE_DIR%\Examples\" /E /I /Y /Q >nul
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
