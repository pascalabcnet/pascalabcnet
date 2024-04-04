cd ..\bin
del ..\Release\PACNETConsole.zip
..\utils\pkzipc\pkzipc.exe -add ..\Release\PABCNETC.zip pabcnetc.exe pabcnetcclear.exe Compiler.dll CompilerTools.dll Errors.dll Localization.dll NETGenerator.dll ParserTools.dll ICSharpCode.NRefactory.dll SemanticTree.dll SyntaxTree.dll TreeConverter.dll OptimizerConversion.dll PascalABCParser.dll licence.txt PascalABCNET.chm System.Threading.dll SyntaxTreeConverters.dll YieldHelpers.dll SyntaxVisitors.dll LambdaAnySynToSemConverter.dll LanguageIntegrator.dll
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PABCNETC.zip Lib\*.pcu
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PABCNETC.zip Lng\*.dat
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PABCNETC.zip Lng\*.LanguageName
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PABCNETC.zip doc\*.*
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PABCNETC.zip -dir LanguageKits
cd ..\ReleaseGenerators
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PABCNETC.zip install_pabcnetc.bat