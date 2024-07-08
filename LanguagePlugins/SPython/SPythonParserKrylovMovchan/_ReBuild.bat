call generateParser.bat
call Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" SPythonParser.sln

@IF %ERRORLEVEL% NEQ 0 PAUSE
move bin\Release\SPythonLanguageParser.dll ..\..\bin\
move bin\Release\SPythonLanguageParser.pdb ..\..\bin\