


if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" (
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" %1 %2 %3 %4

) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe" (
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\msbuild.exe" %1 %2 %3 %4

) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe" (
"%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe" %1 %2 %3 %4



) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe" (
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe" %1 %2 %3 %4

) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\msbuild.exe" (
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\msbuild.exe" %1 %2 %3 %4

) else if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\msbuild.exe" (
"%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\msbuild.exe" %1 %2 %3 %4

) else if exist "D:\apps for coding\VisualStudio\MSBuild\Current\Bin\MSBuild.exe" (
"D:\apps for coding\VisualStudio\MSBuild\Current\Bin\MSBuild.exe" %1 %2 %3 %4

) else (
	ECHO Visual Studio not found
	SET ERRORLEVEL=1
)


