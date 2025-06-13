cd ..\bin
del ..\Release\PascalABCNETMono.zip
..\utils\pkzipc\pkzipc.exe -add ..\Release\PascalABCNETMono.zip PascalABCNETMono.exe pabcnetc.exe pabcnetc PascalABCNET Compiler.dll CompilerTools.dll Errors.dll Localization.dll NETGenerator.dll ParserTools.dll SyntaxTreeConverters.dll ICSharpCode.NRefactory.dll SemanticTree.dll SyntaxTree.dll TreeConverter.dll OptimizerConversion.dll PascalABCParser.dll PascalABCLanguageInfo.dll licence.txt PascalABCNET.chm System.Threading.dll PluginsSupport.dll InternalErrorReport.dll ICSharpCode.TextEditor.dll ICSharpCode.SharpZipLib.dll CodeCompletion.dll Debugger.Core.dll Mono.Debugger.Soft.dll Mono.Debugging.dll Mono.Debugging.Soft.dll NETXP.Controls.dll NETXP.Controls.Bars.dll NETXP.Library.dll NETXP.Win32.dll WeifenLuo.WinFormsUI.Docking.dll template.pct YieldHelpers.dll LanguageIntegrator.dll StringConstants.dll
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PascalABCNETMono.zip Lib\*.pcu
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PascalABCNETMono.zip Highlighting\*.xshd
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PascalABCNETMono.zip Ico\*.ico
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PascalABCNETMono.zip Lng\Rus\*.dat Lng\Rus\.LanguageName Lng\Rus\.LanguageName Lng\Rus\_Global Lng\Rus\_Global_loc
cd ..\ReleaseGenerators
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PascalABCNETMono.zip Samples\Pas\*
cd ..\Release
mkdir LibSource
copy ..\bin\Lib\*.pas LibSource
..\utils\pkzipc\pkzipc.exe -add -nozip -dir ..\Release\PascalABCNETMono.zip LibSource\*.pas
