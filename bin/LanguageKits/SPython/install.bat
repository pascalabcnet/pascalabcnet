@echo off
 
set path_to_pas=%1
if [%1] == [] (
	$p="C:\Program Files (x86)\PascalABC.NET\*"
	if not exist $p (
	echo "C:\Program Files (x86)\PascalABC.NET\* is not a valid path. Enter a valid path as an argument."
	exit
)
)

copy *.dll %path_to_pas%

%path_to_pas%\pabcnetc.exe SpythonSystem.pas
%path_to_pas%\pabcnetc.exe SpythonHidden.pas

copy *.pas %path_to_pas%\LibSource
copy *.pcu %path_to_pas%\Lib


copy *.xshd %path_to_pas%\Highlighting
copy *.dat %path_to_pas%\Lng\Rus