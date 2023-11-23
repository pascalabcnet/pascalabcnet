if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\MsBuild.exe" (
"%ProgramFiles%\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\MsBuild.exe" %1 %2 %3 %4
) else (
	ECHO Visual Studio not found
	SET ERRORLEVEL=1
)
