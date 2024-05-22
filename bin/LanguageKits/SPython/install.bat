@echo off

cd /d "%~dp0"

if "%~1"=="" (
    set "path_to_pas=C:\Program Files (x86)\PascalABC.NET"
) else (
    set "path_to_pas=%~1"
)

if not exist "%path_to_pas%" (
	echo Error: path "%path_to_pas%" is not valid
	pause
	exit
)

copy *.dll "%path_to_pas%"

call "%path_to_pas%\pabcnetc.exe" SpythonSystem.pas
call "%path_to_pas%\pabcnetc.exe" SpythonHidden.pas

copy *.pas "%path_to_pas%\LibSource"
move *.pcu "%path_to_pas%\Lib"


copy *.xshd "%path_to_pas%\Highlighting"
copy Lng\Rus\*.dat "%path_to_pas%\Lng\Rus"
:: copy Lng\Eng\*.dat "%path_to_pas%\Lng\Eng"
