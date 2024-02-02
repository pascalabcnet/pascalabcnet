call generateParser.bat
call Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" VeryBasicParser.sln

@IF %ERRORLEVEL% NEQ 0 PAUSE
move bin\Release\VeryBasicLanguageParser.dll ..\..\bin\
move bin\Release\VeryBasicLanguageParser.pdb ..\..\bin\