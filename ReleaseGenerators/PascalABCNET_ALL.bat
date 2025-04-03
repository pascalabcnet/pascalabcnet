rem "..\utils\NSIS\Unicode\makensis.exe" PascalABCNETMini.nsi
"..\utils\NSIS2\makensis.exe" PascalABCNETStandart.nsi
copy ..\Release\PascalABCNETSetup.exe ..\Release\PascalABCNETWithDotNetSetup.exe 
copy ..\Release\PascalABCNETSetup.exe ..\Release\PascalABCNETMiniSetup.exe 
rem "..\utils\NSIS\Unicode\makensis.exe" PascalABCNETWithDotNet.nsi
PascalABCNETConsoleZIP.bat
rem PascalABCNETMonoZIP.bat
