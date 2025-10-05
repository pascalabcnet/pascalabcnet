rmdir Release\Samples\ /s /q
rmdir Release\PascalABCNETLinux\ /s /q
mkdir Release\PascalABCNETLinux\
mkdir Release\PascalABCNETLinux\Lib\
mkdir Release\PascalABCNETLinux\LibSource\
mkdir Release\PascalABCNETLinux\Lng\Rus
mkdir Release\PascalABCNETLinux\Lng\Eng
mkdir Release\PascalABCNETLinux\Highlighting\
del Release\PascalABCNETLinux.zip


copy bin\CodeCompletion.dll Release\PascalABCNETLinux\CodeCompletion.dll
copy bin\Compiler.dll Release\PascalABCNETLinux\Compiler.dll
copy bin\CompilerTools.dll Release\PascalABCNETLinux\CompilerTools.dll
copy bin\Debugger.Core.dll Release\PascalABCNETLinux\Debugger.Core.dll
copy bin\Errors.dll Release\PascalABCNETLinux\Errors.dll
copy bin\ICSharpCode.Core.dll Release\PascalABCNETLinux\ICSharpCode.Core.dll
copy bin\ICSharpCode.Core.WinForms.dll Release\PascalABCNETLinux\ICSharpCode.Core.WinForms.dll
copy bin\ICSharpCode.NRefactory.dll Release\PascalABCNETLinux\ICSharpCode.NRefactory.dll
copy bin\ICSharpCode.SharpDevelop.dll Release\PascalABCNETLinux\ICSharpCode.SharpDevelop.dll
copy bin\ICSharpCode.SharpDevelop.Dom.dll Release\PascalABCNETLinux\ICSharpCode.SharpDevelop.Dom.dll
copy bin\ICSharpCode.TextEditorLinux.dll Release\PascalABCNETLinux\ICSharpCode.TextEditorLinux.dll
copy bin\Localization.dll Release\PascalABCNETLinux\Localization.dll
copy bin\mono_pabcIDE.bat Release\PascalABCNETLinux\mono_pabcIDE.bat
copy bin\mono_pabcnetc.bat Release\PascalABCNETLinux\mono_pabcnetc.bat
copy bin\NETGenerator.dll Release\PascalABCNETLinux\NETGenerator.dll
copy bin\OptimizerConversion.dll Release\PascalABCNETLinux\OptimizerConversion.dll
copy bin\LanguageIntegrator.dll Release\PascalABCNETLinux\LanguageIntegrator.dll
copy bin\StringConstants.dll Release\PascalABCNETLinux\StringConstants.dll

copy bin\SharpDisasm.dll Release\PascalABCNETLinux\SharpDisasm.dll
copy bin\Mono.Cecil.dll Release\PascalABCNETLinux\Mono.Cecil.dll
copy bin\Mono.Cecil.Mdb.dll Release\PascalABCNETLinux\Mono.Cecil.Mdb.dll
copy bin\Mono.Cecil.Pdb.dll Release\PascalABCNETLinux\Mono.Cecil.Pdb.dll
copy bin\Mono.Cecil.Rocks.dll Release\PascalABCNETLinux\Mono.Cecil.Rocks.dll
copy bin\Microsoft.Bcl.HashCode.dll Release\PascalABCNETLinux\Microsoft.Bcl.HashCode.dll
copy bin\System.Buffers.dll Release\PascalABCNETLinux\System.Buffers.dll
copy bin\System.Collections.Immutable.dll Release\PascalABCNETLinux\System.Collections.Immutable.dll
copy bin\System.Formats.Nrbf.dll Release\PascalABCNETLinux\System.Formats.Nrbf.dll
copy bin\System.Memory.dll Release\PascalABCNETLinux\System.Memory.dll
copy bin\System.Numerics.Vectors.dll Release\PascalABCNETLinux\System.Numerics.Vectors.dll
copy bin\System.Reflection.Metadata.dll Release\PascalABCNETLinux\System.Reflection.Metadata.dll
copy bin\System.Resources.Extensions.dll Release\PascalABCNETLinux\System.Resources.Extensions.dll
copy bin\System.Runtime.CompilerServices.Unsafe.dll Release\PascalABCNETLinux\System.Runtime.CompilerServices.Unsafe.dll
copy bin\System.ValueTuple.dll Release\PascalABCNETLinux\System.ValueTuple.dll

copy bin\pabcnetc.exe Release\PascalABCNETLinux\pabcnetc.exe
copy bin\pabcnetc.exe.config Release\PascalABCNETLinux\pabcnetc.exe.config
copy bin\pabcnetcclear.exe Release\PascalABCNETLinux\pabcnetcclear.exe
copy bin\pabcnetcclear.exe.config Release\PascalABCNETLinux\pabcnetcclear.exe.config
copy bin\ParserTools.dll Release\PascalABCNETLinux\ParserTools.dll
copy bin\PascalABCNET.chm Release\PascalABCNETLinux\PascalABCNET.chm
copy bin\PascalABCNETLinux.exe Release\PascalABCNETLinux\PascalABCNETLinux.exe
copy bin\PascalABCNETLinux.exe.config Release\PascalABCNETLinux\PascalABCNETLinux.exe.config
copy bin\PascalABCParser.dll Release\PascalABCNETLinux\PascalABCParser.dll
copy bin\PascalABCLanguageInfo.dll Release\PascalABCNETLinux\PascalABCLanguageInfo.dll
copy bin\Pause.exe Release\PascalABCNETLinux\Pause.exe
copy bin\PluginsSupportLinux.dll Release\PascalABCNETLinux\PluginsSupportLinux.dll
copy bin\ProgrammRunner.exe Release\PascalABCNETLinux\ProgrammRunner.exe
rem copy bin\rc.exe Release\PascalABCNETLinux\rc.exe
copy bin\SemanticTree.dll Release\PascalABCNETLinux\SemanticTree.dll
copy bin\SyntaxTree.dll Release\PascalABCNETLinux\SyntaxTree.dll
copy bin\SyntaxTreeConverters.dll Release\PascalABCNETLinux\SyntaxTreeConverters.dll
copy bin\SyntaxVisitors.dll Release\PascalABCNETLinux\SyntaxVisitors.dll
copy bin\template.pct Release\PascalABCNETLinux\template.pct
copy bin\TreeConverter.dll Release\PascalABCNETLinux\TreeConverter.dll
copy bin\WeifenLuo.WinFormsUI.Docking.ThemeVS2005Linux.dll Release\PascalABCNETLinux\WeifenLuo.WinFormsUI.Docking.ThemeVS2005Linux.dll
copy bin\WeifenLuo.WinFormsUI.DockingLinux.dll Release\PascalABCNETLinux\WeifenLuo.WinFormsUI.DockingLinux.dll
copy bin\YieldHelpers.dll Release\PascalABCNETLinux\YieldHelpers.dll
copy bin\LambdaAnySynToSemConverter.dll Release\PascalABCNETLinux\LambdaAnySynToSemConverter.dll
copy bin\TeacherControlPlugin.dll Release\PascalABCNETLinux\TeacherControlPlugin.dll
copy bin\Highlighting\PascalABCNET.xshd Release\PascalABCNETLinux\Highlighting\PascalABCNET.xshd

copy bin\Lib\nunit.framework.dll Release\PascalABCNETLinux\Lib\nunit.framework.dll
copy bin\Lib\ABCDatabases.pcu Release\PascalABCNETLinux\Lib\ABCDatabases.pcu
copy bin\Lib\BBCMicroBit.pcu Release\PascalABCNETLinux\Lib\BBCMicroBit.pcu
copy bin\Lib\ClientServer.pcu Release\PascalABCNETLinux\Lib\ClientServer.pcu
copy bin\Lib\Core.pcu Release\PascalABCNETLinux\Lib\Core.pcu
copy bin\Lib\Countries.pcu Release\PascalABCNETLinux\Lib\Countries.pcu
copy bin\Lib\CRT.pcu Release\PascalABCNETLinux\Lib\CRT.pcu
copy bin\Lib\DMCollect.pcu Release\PascalABCNETLinux\Lib\DMCollect.pcu
copy bin\Lib\DMTaskMaker.pcu Release\PascalABCNETLinux\Lib\DMTaskMaker.pcu
copy bin\Lib\DMZadan.pcu Release\PascalABCNETLinux\Lib\DMZadan.pcu
copy bin\Lib\Drawman.pcu Release\PascalABCNETLinux\Lib\Drawman.pcu
copy bin\Lib\DrawManField.pcu Release\PascalABCNETLinux\Lib\DrawManField.pcu
copy bin\Lib\FilesOperations.pcu Release\PascalABCNETLinux\Lib\FilesOperations.pcu
copy bin\Lib\FormsABC.pcu Release\PascalABCNETLinux\Lib\FormsABC.pcu
copy bin\Lib\GraphABC.pcu Release\PascalABCNETLinux\Lib\GraphABC.pcu
copy bin\Lib\ABCObjects.pcu Release\PascalABCNETLinux\Lib\ABCObjects.pcu
copy bin\Lib\GraphABCHelper.pcu Release\PascalABCNETLinux\Lib\GraphABCHelper.pcu
copy bin\Lib\IniFile.pcu Release\PascalABCNETLinux\Lib\IniFile.pcu
copy bin\Lib\LightPT.pcu Release\PascalABCNETLinux\Lib\LightPT.pcu
copy bin\Lib\NumLibABC.pcu Release\PascalABCNETLinux\Lib\NumLibABC.pcu
copy bin\Lib\NUnitABC.pcu Release\PascalABCNETLinux\Lib\NUnitABC.pcu
copy bin\Lib\PABCExtensions.pcu Release\PascalABCNETLinux\Lib\PABCExtensions.pcu
copy bin\Lib\PABCSystem.pcu Release\PascalABCNETLinux\Lib\PABCSystem.pcu
copy bin\Lib\TwoPanelsWindow.pcu Release\PascalABCNETLinux\Lib\TwoPanelsWindow.pcu
copy bin\Lib\XLSX.pcu Release\PascalABCNETLinux\Lib\XLSX.pcu
copy bin\Lib\__RedirectIOMode.pcu Release\PascalABCNETLinux\Lib\__RedirectIOMode.pcu
copy bin\Lib\__RunMode.pcu Release\PascalABCNETLinux\Lib\__RunMode.pcu
copy bin\Lib\PointerTools.pcu Release\PascalABCNETLinux\Lib\PointerTools.pcu
copy bin\Lib\Robot.pcu Release\PascalABCNETLinux\Lib\Robot.pcu
copy bin\Lib\RobotField.pcu Release\PascalABCNETLinux\Lib\RobotField.pcu
copy bin\Lib\RobotTaskMaker.pcu Release\PascalABCNETLinux\Lib\RobotTaskMaker.pcu
copy bin\Lib\RobotZadan.pcu Release\PascalABCNETLinux\Lib\RobotZadan.pcu
copy bin\Lib\Sounds.pcu Release\PascalABCNETLinux\Lib\Sounds.pcu
copy bin\Lib\School.pcu Release\PascalABCNETLinux\Lib\School.pcu
copy bin\Lib\SF.pcu Release\PascalABCNETLinux\Lib\SF.pcu
copy bin\Lib\Speech.pcu Release\PascalABCNETLinux\Lib\Speech.pcu
copy bin\Lib\Tasks.pcu Release\PascalABCNETLinux\Lib\Tasks.pcu
copy bin\Lib\Timers.pcu Release\PascalABCNETLinux\Lib\Timers.pcu
copy bin\Lib\Turtle.pcu Release\PascalABCNETLinux\Lib\Turtle.pcu
copy bin\Lib\TurtleABC.pcu Release\PascalABCNETLinux\Lib\TurtleABC.pcu

copy bin\Lib\ABCDatabases.pas Release\PascalABCNETLinux\LibSource\ABCDatabases.pas
copy bin\Lib\BBCMicrobit.pas Release\PascalABCNETLinux\LibSource\BBCMicrobit.pas
copy bin\Lib\ClientServer.pas Release\PascalABCNETLinux\LibSource\ClientServer.pas
copy bin\Lib\Core.pas Release\PascalABCNETLinux\LibSource\Core.pas
copy bin\Lib\Countries.pas Release\PascalABCNETLinux\LibSource\Countries.pas
copy bin\Lib\CRT.pas Release\PascalABCNETLinux\LibSource\CRT.pas
copy bin\Lib\DMCollect.pas Release\PascalABCNETLinux\LibSource\DMCollect.pas
copy bin\Lib\DMTaskMaker.pas Release\PascalABCNETLinux\LibSource\DMTaskMaker.pas
copy bin\Lib\DMZadan.pas Release\PascalABCNETLinux\LibSource\DMZadan.pas
copy bin\Lib\Drawman.pas Release\PascalABCNETLinux\LibSource\Drawman.pas
copy bin\Lib\DrawManField.pas Release\PascalABCNETLinux\LibSource\DrawManField.pas
copy bin\Lib\FilesOperations.pas Release\PascalABCNETLinux\LibSource\FilesOperations.pas
copy bin\Lib\FormsABC.pas Release\PascalABCNETLinux\LibSource\FormsABC.pas
copy bin\Lib\GraphABC.pas Release\PascalABCNETLinux\LibSource\GraphABC.pas
copy bin\Lib\GraphABCHelper.pas Release\PascalABCNETLinux\LibSource\GraphABCHelper.pas
copy bin\Lib\ABCObjects.pas Release\PascalABCNETLinux\LibSource\ABCObjects.pas
copy bin\Lib\IniFile.pas Release\PascalABCNETLinux\LibSource\IniFile.pas
copy bin\Lib\LightPT.pas Release\PascalABCNETLinux\LibSource\LightPT.pas
copy bin\Lib\NumLibABC.pas Release\PascalABCNETLinux\LibSource\NumLibABC.pas
copy bin\Lib\NUnitABC.pas Release\PascalABCNETLinux\LibSource\NUnitABC.pas
copy bin\Lib\PABCExtensions.pas Release\PascalABCNETLinux\LibSource\PABCExtensions.pas
copy bin\Lib\PABCSystem.pas Release\PascalABCNETLinux\LibSource\PABCSystem.pas
copy bin\Lib\PointerTools.pas Release\PascalABCNETLinux\LibSource\PointerTools.pas
copy bin\Lib\Robot.pas Release\PascalABCNETLinux\LibSource\Robot.pas
copy bin\Lib\RobotField.pas Release\PascalABCNETLinux\LibSource\RobotField.pas
copy bin\Lib\RobotTaskMaker.pas Release\PascalABCNETLinux\LibSource\RobotTaskMaker.pas
copy bin\Lib\RobotZadan.pas Release\PascalABCNETLinux\LibSource\RobotZadan.pas
copy bin\Lib\School.pas Release\PascalABCNETLinux\LibSource\School.pas
copy bin\Lib\SF.pas Release\PascalABCNETLinux\LibSource\SF.pas
copy bin\Lib\Sounds.pas Release\PascalABCNETLinux\LibSource\Sounds.pas
copy bin\Lib\Speech.pas Release\PascalABCNETLinux\LibSource\Speech.pas
copy bin\Lib\Tasks.pas Release\PascalABCNETLinux\LibSource\Tasks.pas
copy bin\Lib\Timers.pas Release\PascalABCNETLinux\LibSource\Timers.pas
copy bin\Lib\Turtle.pas Release\PascalABCNETLinux\LibSource\Turtle.pas
copy bin\Lib\TurtleABC.pas Release\PascalABCNETLinux\LibSource\TurtleABC.pas
copy bin\Lib\TwoPanelsWindow.pas Release\PascalABCNETLinux\LibSource\TwoPanelsWindow.pas
copy bin\Lib\XLSX.pas Release\PascalABCNETLinux\LibSource\XLSX.pas
copy bin\Lib\__RedirectIOMode.pas Release\PascalABCNETLinux\LibSource\__RedirectIOMode.pas
copy bin\Lib\__RunMode.pas Release\PascalABCNETLinux\LibSource\__RunMode.pas
copy bin\Lib\Робот.pas Release\PascalABCNETLinux\LibSource\Робот.pas
copy bin\Lib\Чертежник.pas Release\PascalABCNETLinux\LibSource\Чертежник.pas

copy bin\Lng\Eng\.LanguageName Release\PascalABCNETLinux\Lng\Eng\.LanguageName
copy bin\Lng\Eng\AspectsTree.dat Release\PascalABCNETLinux\Lng\Eng\AspectsTree.dat
copy bin\Lng\Eng\CodeCompletion.dat Release\PascalABCNETLinux\Lng\Eng\CodeCompletion.dat
copy bin\Lng\Eng\CodeTemplates.dat Release\PascalABCNETLinux\Lng\Eng\CodeTemplates.dat
copy bin\Lng\Eng\CompilerController.dat Release\PascalABCNETLinux\Lng\Eng\CompilerController.dat
copy bin\Lng\Eng\CompilerErrors.dat Release\PascalABCNETLinux\Lng\Eng\CompilerErrors.dat
copy bin\Lng\Eng\Global_loc.dat Release\PascalABCNETLinux\Lng\Eng\Global_loc.dat
copy bin\Lng\Eng\InternalErrorReport.dat Release\PascalABCNETLinux\Lng\Eng\InternalErrorReport.dat
copy bin\Lng\Eng\LanguageName.info Release\PascalABCNETLinux\Lng\Eng\LanguageName.info
copy bin\Lng\Eng\OpenMPErrors.dat Release\PascalABCNETLinux\Lng\Eng\OpenMPErrors.dat
copy bin\Lng\Eng\PABCNETC.dat Release\PascalABCNETLinux\Lng\Eng\PABCNETC.dat
copy bin\Lng\Eng\PABCPreprocessor2.dat Release\PascalABCNETLinux\Lng\Eng\PABCPreprocessor2.dat
copy bin\Lng\Eng\ParserErrors.dat Release\PascalABCNETLinux\Lng\Eng\ParserErrors.dat
copy bin\Lng\Eng\PascalABCParser.dat Release\PascalABCNETLinux\Lng\Eng\PascalABCParser.dat
copy bin\Lng\Eng\PT4Provider.dat Release\PascalABCNETLinux\Lng\Eng\PT4Provider.dat
copy bin\Lng\Eng\RuntimeExceptions_ds.dat Release\PascalABCNETLinux\Lng\Eng\RuntimeExceptions_ds.dat
copy bin\Lng\Eng\SemanticErrors_ds.dat Release\PascalABCNETLinux\Lng\Eng\SemanticErrors_ds.dat
copy bin\Lng\Eng\SemanticErrors_ib.dat Release\PascalABCNETLinux\Lng\Eng\SemanticErrors_ib.dat
copy bin\Lng\Eng\SemanticErrors_ms.dat Release\PascalABCNETLinux\Lng\Eng\SemanticErrors_ms.dat
copy bin\Lng\Eng\SemanticErrors_nv.dat Release\PascalABCNETLinux\Lng\Eng\SemanticErrors_nv.dat
copy bin\Lng\Eng\SemanticErrors_rs.dat Release\PascalABCNETLinux\Lng\Eng\SemanticErrors_rs.dat
copy bin\Lng\Eng\SemanticErrors_ssyy.dat Release\PascalABCNETLinux\Lng\Eng\SemanticErrors_ssyy.dat
copy bin\Lng\Eng\SemanticErrors_ws.dat Release\PascalABCNETLinux\Lng\Eng\SemanticErrors_ws.dat
copy bin\Lng\Eng\SyantaxTreeVisualisator.dat Release\PascalABCNETLinux\Lng\Eng\SyantaxTreeVisualisator.dat
copy bin\Lng\Eng\SyntaxTreeVisitorsErrors.dat Release\PascalABCNETLinux\Lng\Eng\SyntaxTreeVisitorsErrors.dat
copy bin\Lng\Eng\VisualPascalABCNET.dat Release\PascalABCNETLinux\Lng\Eng\VisualPascalABCNET.dat
copy bin\Lng\Eng\VisualPascalABCNET_VEC.dat Release\PascalABCNETLinux\Lng\Eng\VisualPascalABCNET_VEC.dat
copy bin\Lng\Eng\Warnings_ds.dat Release\PascalABCNETLinux\Lng\Eng\Warnings_ds.dat
copy bin\Lng\Eng\Warning_ib.dat Release\PascalABCNETLinux\Lng\Eng\Warning_ib.dat
copy bin\Lng\Rus\.LanguageName Release\PascalABCNETLinux\Lng\Rus\.LanguageName
copy bin\Lng\Rus\AspectsTree.dat Release\PascalABCNETLinux\Lng\Rus\AspectsTree.dat
copy bin\Lng\Rus\CodeCompletion.dat Release\PascalABCNETLinux\Lng\Rus\CodeCompletion.dat
copy bin\Lng\Rus\CodeTemplates.dat Release\PascalABCNETLinux\Lng\Rus\CodeTemplates.dat
copy bin\Lng\Rus\CompilerController.dat Release\PascalABCNETLinux\Lng\Rus\CompilerController.dat
copy bin\Lng\Rus\CompilerErrors.dat Release\PascalABCNETLinux\Lng\Rus\CompilerErrors.dat
copy bin\Lng\Rus\HelpBuilder.dat Release\PascalABCNETLinux\Lng\Rus\HelpBuilder.dat
copy bin\Lng\Rus\InternalErrorReport.dat Release\PascalABCNETLinux\Lng\Rus\InternalErrorReport.dat
copy bin\Lng\Rus\KuMirParser.dat Release\PascalABCNETLinux\Lng\Rus\KuMirParser.dat
copy bin\Lng\Rus\LanguageConvertor.dat Release\PascalABCNETLinux\Lng\Rus\LanguageConvertor.dat
copy bin\Lng\Rus\LanguageName.info Release\PascalABCNETLinux\Lng\Rus\LanguageName.info
copy bin\Lng\Rus\OpenMPErrors.dat Release\PascalABCNETLinux\Lng\Rus\OpenMPErrors.dat
copy bin\Lng\Rus\PABCNETC.dat Release\PascalABCNETLinux\Lng\Rus\PABCNETC.dat
copy bin\Lng\Rus\PABCPreprocessor2.dat Release\PascalABCNETLinux\Lng\Rus\PABCPreprocessor2.dat
copy bin\Lng\Rus\ParserErrors.dat Release\PascalABCNETLinux\Lng\Rus\ParserErrors.dat
copy bin\Lng\Rus\PascalABCParser.dat Release\PascalABCNETLinux\Lng\Rus\PascalABCParser.dat
copy bin\Lng\Rus\PluginController.dat Release\PascalABCNETLinux\Lng\Rus\PluginController.dat
copy bin\Lng\Rus\PT4Provider.dat Release\PascalABCNETLinux\Lng\Rus\PT4Provider.dat
copy bin\Lng\Rus\PythonABCParser.dat Release\PascalABCNETLinux\Lng\Rus\PythonABCParser.dat
copy bin\Lng\Rus\RegisterAndControlPlugin.dat Release\PascalABCNETLinux\Lng\Rus\RegisterAndControlPlugin.dat
copy bin\Lng\Rus\RuntimeExceptions_ds.dat Release\PascalABCNETLinux\Lng\Rus\RuntimeExceptions_ds.dat
copy bin\Lng\Rus\SemanticErrors_ds.dat Release\PascalABCNETLinux\Lng\Rus\SemanticErrors_ds.dat
copy bin\Lng\Rus\SemanticErrors_ib.dat Release\PascalABCNETLinux\Lng\Rus\SemanticErrors_ib.dat
copy bin\Lng\Rus\SemanticErrors_ms.dat Release\PascalABCNETLinux\Lng\Rus\SemanticErrors_ms.dat
copy bin\Lng\Rus\SemanticErrors_nv.dat Release\PascalABCNETLinux\Lng\Rus\SemanticErrors_nv.dat
copy bin\Lng\Rus\SemanticErrors_rs.dat Release\PascalABCNETLinux\Lng\Rus\SemanticErrors_rs.dat
copy bin\Lng\Rus\SemanticErrors_ssyy.dat Release\PascalABCNETLinux\Lng\Rus\SemanticErrors_ssyy.dat
copy bin\Lng\Rus\SemanticErrors_ws.dat Release\PascalABCNETLinux\Lng\Rus\SemanticErrors_ws.dat
copy bin\Lng\Rus\SemanticTreeVisualisator.dat Release\PascalABCNETLinux\Lng\Rus\SemanticTreeVisualisator.dat
copy bin\Lng\Rus\SourceTextFormater.dat Release\PascalABCNETLinux\Lng\Rus\SourceTextFormater.dat
copy bin\Lng\Rus\SyntaxTreeVisitorsErrors.dat Release\PascalABCNETLinux\Lng\Rus\SyntaxTreeVisitorsErrors.dat
copy bin\Lng\Rus\SyntaxTreeVisualisator.dat Release\PascalABCNETLinux\Lng\Rus\SyntaxTreeVisualisator.dat
copy bin\Lng\Rus\VisualPascalABCNET.dat Release\PascalABCNETLinux\Lng\Rus\VisualPascalABCNET.dat
copy bin\Lng\Rus\VisualPascalABCNET_VEC.dat Release\PascalABCNETLinux\Lng\Rus\VisualPascalABCNET_VEC.dat
copy bin\Lng\Rus\Warnings_ds.dat Release\PascalABCNETLinux\Lng\Rus\Warnings_ds.dat
copy bin\Lng\Rus\Warning_ib.dat Release\PascalABCNETLinux\Lng\Rus\Warning_ib.dat
copy bin\Lng\Rus\_Global Release\PascalABCNETLinux\Lng\Rus\_Global
copy bin\Lng\Rus\_Global_loc Release\PascalABCNETLinux\Lng\Rus\_Global_loc

xcopy InstallerSamples\!РусскиеИсполнители\ Release\Samples\!РусскиеИсполнители\ /s /e 
xcopy InstallerSamples\!MainFeatures\ Release\Samples\!MainFeatures\ /s /e

xcopy InstallerSamples\!Tutorial\           Release\Samples\!Tutorial\           /s /e
xcopy InstallerSamples\Algorithms\          Release\Samples\Algorithms\          /s /e
xcopy InstallerSamples\Applications\        Release\Samples\Applications\        /s /e
xcopy InstallerSamples\Games\               Release\Samples\Games\               /s /e
xcopy InstallerSamples\LanguageFeatures\    Release\Samples\LanguageFeatures\    /s /e
xcopy InstallerSamples\LINQ\                Release\Samples\LINQ\                /s /e
xcopy InstallerSamples\NETLibraries\        Release\Samples\NETLibraries\        /s /e
xcopy InstallerSamples\Other\               Release\Samples\Other\               /s /e
xcopy InstallerSamples\StandardUnits\       Release\Samples\StandardUnits\       /s /e
xcopy InstallerSamples\WhatsNew\            Release\Samples\WhatsNew\            /s /e

xcopy InstallerSamples\Graphics\GraphABC\   Release\Samples\Graphics\GraphABC\   /s /e
xcopy InstallerSamples\Graphics\ABCObjects\   Release\Samples\Graphics\ABCObjects\   /s /e

cd Release

..\Utils\pkzipc\pkzipc.exe -add -dir=current PascalABCNETLinux.zip PascalABCNETLinux\*.*
..\Utils\pkzipc\pkzipc.exe -add -dir=current PascalABCNETLinux.zip Samples\*.*

cd ..
pause
