﻿	Section $(DESC_Core) Core
    SectionIn 1 2 RO
    SetOutPath "$INSTDIR"
    File ExecHide.exe
    File "..\bin\Compiler.dll"
    File "..\bin\CompilerTools.dll"
    File "..\bin\Errors.dll"
    File "..\bin\Localization.dll"
    File "..\bin\NETGenerator.dll"
    File "..\bin\ParserTools.dll"
    File "..\bin\SemanticTree.dll"
    File "..\bin\SyntaxTree.dll"
    File "..\bin\SyntaxTreeConverters.dll"
    File "..\bin\SyntaxVisitors.dll"
    File "..\bin\YieldHelpers.dll"
    File "..\bin\ICSharpCode.NRefactory.dll"
    File "..\bin\TreeConverter.dll"
    File "..\bin\OptimizerConversion.dll"
    File "..\bin\ICSharpCode.AvalonEdit.dll"
    File "..\bin\ICSharpCode.SharpDevelop.dll"
    File "..\bin\ICSharpCode.SharpDevelop.Dom.dll"
    File "..\bin\ICSharpCode.Core.WinForms.dll"
    File "..\bin\ICSharpCode.Core.dll"
    File "..\bin\ICSharpCode.Core.Presentation.dll"
    File "..\bin\ICSharpCode.SharpDevelop.Widgets.dll"
    File "..\bin\ControlLibrary.sdcl"
    File "..\bin\AvalonDock.dll"
    File "..\bin\Mono.Cecil.dll"
	;File "libs\System.Core.dll"
    File "gacutil.exe"
    File "gacutil.exe.config"
    File "gacutlrc.dll"
    File "License.txt"
    File "License_en.txt"
    File "copyright.txt"
    File "..\bin\pabcnetc.exe.config"
    File "..\bin\pabcnetcclear.exe.config"


; main config - only .NET 4.7.1 and above
	DotNetChecker::IsDotNet471Installed
	Pop $0

	${If} $0 == "false"
	${OrIf} $0 == "f"  ; if script is compiled in ANSI mode then we get only an "f"  https://github.com/ReVolly/NsisDotNetChecker/issues/4
	${Else}
	    File "..\bin\PascalABCNET.exe.config"
	    ${AddFile} "PascalABCNET.exe.config"
	${EndIf}
	
	
	;dobavljaem fajly v uninst.log
	${AddFile} "Compiler.dll"
    ${AddFile} "CompilerTools.dll"
    ${AddFile} "Errors.dll"
    ${AddFile} "Localization.dll"
    ${AddFile} "NETGenerator.dll"
    ${AddFile} "ParserTools.dll"
    ${AddFile} "SemanticTree.dll"
    ${AddFile} "SyntaxTree.dll"
    ${AddFile} "SyntaxTreeConverters.dll"
    ${AddFile} "YieldHelpers.dll"
    ${AddFile} "SyntaxVisitors.dll"
    ${AddFile} "ICSharpCode.NRefactory.dll"
    ${AddFile} "TreeConverter.dll"
    ${AddFile} "OptimizerConversion.dll"
    ${AddFile} "Mono.Cecil.dll"
    ${AddFile} "License.txt"
    ${AddFile} "copyright.txt"
    ${AddFile} "pabcnetc.exe.config"
    ${AddFile} "pabcnetcclear.exe.config"

    Delete "$INSTDIR\Lib\*.pas"
    SetOutPath "$INSTDIR\Lib"
    ;File ..\bin\Lib\*.pcu; eto ploho nuzhno kazhdyj pcu raspisyvat
	
    File ..\bin\Lib\__RedirectIOMode.pcu
    File ..\bin\Lib\__RunMode.pcu
    File ..\bin\Lib\ABCButtons.pcu
    File ..\bin\Lib\ABCHouse.pcu
    File ..\bin\Lib\ABCObjects.pcu
    File ..\bin\Lib\ABCSprites.pcu
    File ..\bin\Lib\ABCDatabases.pcu
    File ..\bin\Lib\PT4Databases.pcu
    File ..\bin\Lib\Arrays.pcu
    ;File ..\bin\Lib\Colors.pcu
    File ..\bin\Lib\CRT.pcu
    File ..\bin\Lib\DMCollect.pcu
    File ..\bin\Lib\DMTaskMaker.pcu
    File ..\bin\Lib\DMZadan.pcu
    File ..\bin\Lib\Drawman.pcu
    File ..\bin\Lib\DrawManField.pcu
    File ..\bin\Lib\Events.pcu
    File ..\bin\Lib\FilesOperations.pcu
    File ..\bin\Lib\FormsABC.pcu
    File ..\bin\Lib\GraphABC.pcu
    File ..\bin\Lib\NumLibABC.pcu
    File ..\bin\Lib\GraphWPFBase.pcu
    File ..\bin\Lib\GraphWPF.pcu
    File ..\bin\Lib\WPFObjects.pcu
    File ..\bin\Lib\Controls.pcu
    File ..\bin\Lib\Countries.pcu
    File ..\bin\Lib\Graph3D.pcu
    File ..\bin\Lib\GraphABCHelper.pcu
    File ..\bin\Lib\BBCMicroBit.pcu
    File ..\bin\Lib\IniFile.pcu
    File ..\bin\Lib\PABCSystem.pcu
    File ..\bin\Lib\PABCExtensions.pcu
    File ..\bin\Lib\PointerTools.pcu
    File ..\bin\Lib\PointRect.pcu
    File ..\bin\Lib\PT4.pcu
    File ..\bin\Lib\Robot.pcu
    File ..\bin\Lib\RobotField.pcu
    File ..\bin\Lib\RobotTaskMaker.pcu
    File ..\bin\Lib\RobotZadan.pcu
    File ..\bin\Lib\Sockets.pcu
    File ..\bin\Lib\Timers.pcu
    File ..\bin\Lib\Utils.pcu
    File ..\bin\Lib\VCL.pcu
    File ..\bin\Lib\PT4Exam.pcu
    File ..\bin\Lib\PT4TaskMakerNET.pcu
    File ..\bin\Lib\RBDMUtils.pcu
    File ..\bin\Lib\Collections.pcu
    File ..\bin\Lib\Core.pcu
    File ..\bin\Lib\MPI.pcu
    File ..\bin\Lib\ClientServer.pcu
    File ..\bin\Lib\OpenGL.pcu
    File ..\bin\Lib\PT4MakerNetX.pcu
    File ..\bin\Lib\Speech.pcu
    File ..\bin\Lib\Sounds.pcu
    File ..\bin\Lib\BlockFileOfT.pcu
    File ..\bin\Lib\OpenCL.pcu
    File ..\bin\Lib\OpenCLABC.pcu
    File ..\bin\Lib\OpenGL.pcu
    File ..\bin\Lib\OpenGLABC.pcu
    File ..\bin\Lib\School.pcu
    File ..\bin\Lib\SF.pcu
    File ..\bin\Lib\Turtle.pcu
    File ..\bin\Lib\TwoPanelsWindow.pcu
    File ..\bin\Lib\NUnitABC.pcu

    File ..\bin\Lib\PABCRtl.dll
    File ..\bin\Lib\HelixToolkit.Wpf.dll
    File ..\bin\Lib\HelixToolkit.dll
    File ..\bin\Lib\nunit.framework.dll 
	
    ${AddFile} "__RedirectIOMode.pcu"
    ${AddFile} "__RunMode.pcu"
    ${AddFile} "ABCButtons.pcu"
    ${AddFile} "ABCHouse.pcu"
    ${AddFile} "ABCObjects.pcu"
    ${AddFile} "ABCDatabases.pcu"
    ${AddFile} "PT4Databases.pcu"
    ${AddFile} "ABCSprites.pcu"
    ${AddFile} "Arrays.pcu"
    ${AddFile} "BFSystem.pcu"
    ;${AddFile} "Colors.pcu"
    ${AddFile} "CRT.pcu"
    ${AddFile} "DMCollect.pcu"
    ${AddFile} "DMTaskMaker.pcu"
    ${AddFile} "DMZadan.pcu"
    ${AddFile} "Drawman.pcu"
    ${AddFile} "DrawManField.pcu"
    ${AddFile} "Events.pcu"
    ${AddFile} "FilesOperations.pcu"
    ${AddFile} "FormsABC.pcu"
    ${AddFile} "GOLDParserEngine.pcu"
    ${AddFile} "GraphABC.pcu"
    ${AddFile} "NumLibABC.pcu"
    ${AddFile} "GraphWPFBase.pcu"
    ${AddFile} "GraphWPF.pcu"
    ${AddFile} "WPFObjects.pcu"
    ${AddFile} "Controls.pcu"
    ${AddFile} "Countries.pcu"
    ${AddFile} "Graph3D.pcu"
    ${AddFile} "GraphABCHelper.pcu"
    ${AddFile} "BBCMicroBit.pcu"
    ${AddFile} "IniFile.pcu"
    ${AddFile} "PABCSystem.pcu"
    ${AddFile} "PABCExtensions.pcu"
    ${AddFile} "PointerTools.pcu"
    ${AddFile} "PointRect.pcu"
    ${AddFile} "PT4.pcu"
    ${AddFile} "Robot.pcu"
    ${AddFile} "RobotField.pcu"
    ${AddFile} "RobotTaskMaker.pcu"
    ${AddFile} "RobotZadan.pcu"
    ${AddFile} "Sockets.pcu"
    ${AddFile} "Timers.pcu"
    ${AddFile} "Utils.pcu"
    ${AddFile} "VCL.pcu"
    ${AddFile} "PT4TaskMakerNET.pcu"
    ${AddFile} "PT4Exam.pcu"
    ${AddFile} "RBDMUtils.pcu"
    ${AddFile} "Collections.pcu"
    ${AddFile} "Core.pcu"
    ${AddFile} "MPI.pcu"
    ${AddFile} "ClientServer.pcu"
    ${AddFile} "OpenGL.pcu"
    ${AddFile} "PT4MakerNetX.pcu"
    ${AddFile} "Speech.pcu"
    ${AddFile} "Sounds.pcu"
    ${AddFile} "BlockFileOfT.pcu"
    ${AddFile} "OpenCL.pcu"
    ${AddFile} "OpenCLABC.pcu"
    ${AddFile} "OpenGL.pcu"
    ${AddFile} "OpenGLABC.pcu"
    ${AddFile} "School.pcu"
    ${AddFile} "SF.pcu"
    ${AddFile} "Turtle.pcu"
    ${AddFile} "TwoPanelsWindow.pcu"
    ${AddFile} "NUnitABC.pcu"


    ${AddFile} "PABCRtl.dll"
    ${AddFile} "HelixToolkit.Wpf.dll"
    ${AddFile} "HelixToolkit.dll"
    ${AddFile} "nunit.framework.dll"
    
    ${AddFile} "PABCRtl.pdb"

    SetOutPath "$INSTDIR\Doc"
    File ..\doc\NumLibABC.pdf
    ${AddFile} "NumLibABC.pdf"


    Push "Lib\PABCRtl.dll"
    Call NGEN
    Push "Lib\HelixToolkit.Wpf.dll"
    Call NGEN
    Push "Lib\HelixToolkit.dll"
    Call NGEN
    Push "Lib\nunit.framework.dll"
    Call NGEN
	
    SetOutPath "$INSTDIR\LibSource"
    File ..\bin\Lib\__RedirectIOMode.pas
    File ..\bin\Lib\__RunMode.pas
    File ..\bin\Lib\ABCButtons.pas
    File ..\bin\Lib\ABCHouse.pas
    File ..\bin\Lib\ABCObjects.pas
    File ..\bin\Lib\ABCSprites.pas
    File ..\bin\Lib\ABCDatabases.pas
    File ..\bin\Lib\PT4Databases.pas
    File ..\bin\Lib\Arrays.pas
    ;File ..\bin\Lib\Colors.pas
    File ..\bin\Lib\CRT.pas
    File ..\bin\Lib\DMCollect.pas
    File ..\bin\Lib\DMTaskMaker.pas
;    File ..\bin\Lib\DMZadan.pas
    File ..\bin\Lib\Drawman.pas
    File ..\bin\Lib\DrawManField.pas
    File ..\bin\Lib\Events.pas
    File ..\bin\Lib\FilesOperations.pas
    File ..\bin\Lib\FormsABC.pas
    File ..\bin\Lib\GraphABC.pas
    File ..\bin\Lib\NumLibABC.pas
    File ..\bin\Lib\GraphWPFBase.pas
    File ..\bin\Lib\GraphWPF.pas
    File ..\bin\Lib\WPFObjects.pas
    File ..\bin\Lib\Controls.pas
    File ..\bin\Lib\Countries.pas
    File ..\bin\Lib\Graph3D.pas
    File ..\bin\Lib\GraphABCHelper.pas
    File ..\bin\Lib\BBCMicroBit.pas
    File ..\bin\Lib\IniFile.pas
    File ..\bin\Lib\PABCSystem.pas
    File ..\bin\Lib\PABCExtensions.pas
    File ..\bin\Lib\PointerTools.pas
    File ..\bin\Lib\PointRect.pas
    File ..\bin\Lib\PT4.pas
    File ..\bin\Lib\PT4Exam.pas
    File ..\bin\Lib\Robot.pas
    File ..\bin\Lib\RobotField.pas
    File ..\bin\Lib\RobotTaskMaker.pas
;    File ..\bin\Lib\RobotZadan.pas
    File ..\bin\Lib\Sockets.pas
    File ..\bin\Lib\Timers.pas
    File ..\bin\Lib\Utils.pas
    File ..\bin\Lib\VCL.pas
    File ..\bin\Lib\PT4TaskMakerNET.pas
    File ..\bin\Lib\RBDMUtils.pas
    File ..\bin\Lib\Collections.pas
    File ..\bin\Lib\Core.pas
    File ..\bin\Lib\MPI.pas
    File ..\bin\Lib\ClientServer.pas
    File ..\bin\Lib\OpenGL.pas
    File ..\bin\Lib\PT4MakerNetX.pas
    File ..\bin\Lib\Speech.pas
    File ..\bin\Lib\Sounds.pas
    File ..\bin\Lib\BlockFileOfT.pas
    File ..\bin\Lib\OpenCL.pas
    File ..\bin\Lib\OpenCLABC.pas
    File ..\bin\Lib\OpenGL.pas
    File ..\bin\Lib\OpenGLABC.pas
    File ..\bin\Lib\School.pas
    File ..\bin\Lib\SF.pas
    File ..\bin\Lib\Turtle.pas
    File ..\bin\Lib\TwoPanelsWindow.pas
    File ..\bin\Lib\NUnitABC.pas

	File ..\bin\Lib\__RedirectIOMode.vb
	File ..\bin\Lib\VBSystem.vb
	
	;dobavljaem fajly v uninst.log
	${AddFile} "__RedirectIOMode.pas"
    ${AddFile} "__RunMode.pas"
    ${AddFile} "ABCButtons.pas"
    ${AddFile} "ABCHouse.pas"
    ${AddFile} "ABCObjects.pas"
    ${AddFile} "ABCSprites.pas"
    ${AddFile} "ABCDatabases.pas"
    ${AddFile} "PT4Databases.pas"
    ${AddFile} "Arrays.pas"
    ;${AddFile} "Colors.pas"
    ${AddFile} "CRT.pas"
    ${AddFile} "DMCollect.pas"
    ${AddFile} "DMTaskMaker.pas"
;    File ..\bin\Lib\DMZadan.pas
    ${AddFile} "Drawman.pas"
    ${AddFile} "DrawManField.pas"
    ${AddFile} "Events.pas"
    ${AddFile} "FilesOperations.pas"
    ${AddFile} "FormsABC.pas"
    ${AddFile} "GraphABC.pas"
    ${AddFile} "NumLibABC.pas"
    ${AddFile} "GraphWPFBase.pas"
    ${AddFile} "GraphWPF.pas"
    ${AddFile} "WPFObjects.pas"
    ${AddFile} "Controls.pas"
    ${AddFile} "Countries.pas"
    ${AddFile} "Graph3D.pas"
    ${AddFile} "GraphABCHelper.pas"
    ${AddFile} "BBCMicroBit.pas"
    ${AddFile} "IniFile.pas"
    ${AddFile} "PABCSystem.pas"
    ${AddFile} "PABCExtensions.pas"
    ${AddFile} "PointerTools.pas"
    ${AddFile} "PointRect.pas"
    ${AddFile} "PT4.pas"
    ${AddFile} "PT4Exam.pas"
    ${AddFile} "Robot.pas"
    ${AddFile} "RobotField.pas"
    ${AddFile} "RobotTaskMaker.pas"
;    File ..\bin\Lib\RobotZadan.pas
    ${AddFile} "Sockets.pas"
	${AddFile} "Timers.pas"
    ${AddFile} "Utils.pas"
    ${AddFile} "VCL.pas"
    ${AddFile} "PT4TaskMakerNET.pas"
    ${AddFile} "RBDMUtils.pas"
    ${AddFile} "Collections.pas"
    ${AddFile} "Core.pas"
    ${AddFile} "MPI.pas"
    ${AddFile} "ClientServer.pas"
    ${AddFile} "PT4MakerNetX.pas"
    ${AddFile} "Speech.pas"
    ${AddFile} "Sounds.pas"
    ${AddFile} "BlockFileOfT.pas"
    ${AddFile} "OpenCL.pas"
    ${AddFile} "OpenCLABC.pas"
    ${AddFile} "OpenGL.pas"
    ${AddFile} "OpenGLABC.pas"
    ${AddFile} "School.pas"
    ${AddFile} "SF.pas"
    ${AddFile} "Turtle.pas"
    ${AddFile} "TwoPanelsWindow.pas"
    ${AddFile} "NUnitABC.pas"


	${AddFile} "__RedirectIOMode.vb"
    ${AddFile} "VBSystem.vb"
	
    CreateDirectory "$SMPROGRAMS\PascalABC.NET"
    Push "OptimizerConversion.dll"
    Call NGEN
	Push "SyntaxVisitors.dll"
    Call NGEN
    
;    SetOutPath "$INSTDIR\Output"
SectionEnd
