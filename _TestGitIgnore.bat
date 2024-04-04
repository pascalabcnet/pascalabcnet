


cd bin
pabcnetcclear GitIgnoreTester.pas
GitIgnoreTester.exe NoWait
@IF %ERRORLEVEL% NEQ 0 GOTO ERROR
cd ..



GOTO EXIT

:ERROR
PAUSE

:EXIT