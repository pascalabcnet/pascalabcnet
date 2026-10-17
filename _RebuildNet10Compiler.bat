@echo off
setlocal

set "CONFIGURATION=Release"
if /I "%~1"=="Debug" set "CONFIGURATION=Debug"
if /I not "%~1"=="Debug" if /I not "%~1"=="Release" if not "%~1"=="" (
    echo Usage: %~nx0 [Release^|Debug]
    exit /b 2
)

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0scripts\build-compile-net10-plugin.ps1" -Configuration %CONFIGURATION%
if errorlevel 1 (
    echo.
    echo ERROR: .NET 10 compiler or IDE plugin rebuild failed.
    pause
    exit /b 1
)

echo.
echo .NET 10 compiler updated in "%~dp0bin-net10".
echo IDE plugin updated in "%~dp0bin\CompileNet10Plugin.dll".
exit /b 0
