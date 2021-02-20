cls
GPLex_GPPG\gplex.exe /unicode oberon00.lex
GPLex_GPPG\gppg.exe /no-lines /gplex oberon00.y

copy ..\..\bin\SyntaxTree.dll DLL\SyntaxTree.dll
%windir%\microsoft.net\framework\v3.5\msbuild /t:rebuild /property:Configuration=Release ObrProba.sln

copy bin\Release\Oberon00Parser.dll Install\PascalABC.NET\Oberon00Parser.dll
copy bin\Release\Oberon00Parser.dll ..\..\bin\Oberon00Parser.dll
