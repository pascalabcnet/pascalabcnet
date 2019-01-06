call Studio.bat /t:rebuild /verbosity:d "/property:Configuration=Release" PascalABCNET.sln

@IF %ERRORLEVEL% NEQ 0 PAUSE