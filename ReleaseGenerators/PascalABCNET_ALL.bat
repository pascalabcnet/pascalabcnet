"..\utils\NSIS\Unicode\makensis.exe" PascalABCNETMini.nsi
"..\utils\NSIS\Unicode\makensis.exe" PascalABCNETStandart.nsi
copy ..\Release\PascalABCNETSetup.exe ..\Release\PascalABCNETWithDotNetSetup.exe 
rem "..\utils\NSIS\Unicode\makensis.exe" PascalABCNETWithDotNet.nsi
PascalABCNETConsoleZIP.bat
rem PascalABCNETMonoZIP.bat