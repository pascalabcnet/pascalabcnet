  Section $(DESC_Envr) VisualPABCNET
    SectionIn 1 2 RO
    SetOutPath "$INSTDIR"
    File "..\bin\ICSharpCode.TextEditor.dll" 
	File "..\bin\ICSharpCode.AvalonEdit.dll"	
    File "..\bin\PluginsSupport.dll"    
    File "..\bin\PascalABCNET.exe"   
    ;File "..\bin\PascalABCNET.exe.manifest"   
    File "..\bin\Debugger.Core.dll"
    File "..\bin\SharpDisasm.dll"
    File "..\bin\PascalABCNET.chm"
    File "..\bin\CodeCompletion.dll"
    File "..\bin\WeifenLuo.WinFormsUI.Docking.dll"
    File "..\bin\CodeTemplatesPlugin.dll"
    File "..\bin\HelpExamples.ini"
    File "..\bin\template.pct"
    File "..\bin\samples.pct"
    File "..\bin\school.pct"
    File "..\bin\Pause.exe"
    File "..\bin\nuget.exe"
    File "..\bin\FormatterOptions.ini"
    File "..\bin\ProgrammRunner.exe"
    File "..\bin\RunUnitTests.exe"
    File "..\bin\pabcworknet.ini"
	
	;dobavljaem fajly v uninst.log
    ${AddFile} "ICSharpCode.TextEditor.dll"
    ${AddFile} "ICSharpCode.AvalonEdit.dll"	
    ${AddFile} "PluginsSupport.dll"    
    ${AddFile} "PascalABCNET.exe"   
    ;File "..\bin\PascalABCNET.exe.manifest"   
    ${AddFile} "Debugger.Core.dll"
    ${AddFile} "SharpDisasm.dll"
    ${AddFile} "PascalABCNET.chm"
    ${AddFile} "CodeCompletion.dll"
    ${AddFile} "WeifenLuo.WinFormsUI.Docking.dll"
    ${AddFile} "CodeTemplatesPlugin.dll"
    ${AddFile} "HelpExamples.ini"
    ${AddFile} "template.pct"
    ${AddFile} "samples.pct"
    ${AddFile} "school.pct"
    ${AddFile} "Pause.exe"
    ${AddFile} "nuget.exe"
    ${AddFile} "FormatterOptions.ini"
    ${AddFile} "ProgrammRunner.exe"
    ${AddFile} "RunUnitTests.exe"
	
    SetOutPath "$INSTDIR\Temp"
    SetOutPath "$INSTDIR"
    Push "Pause.exe"
    Call NGEN

    ;Временно
    ;Delete "$INSTDIR\PascalABCNET.ini"

    CreateShortcut "$COMMONSTARTMENU\PascalABC.NET\PascalABC.NET.lnk" "$INSTDIR\PascalABCNET.exe"

    CreateShortCut "$DESKTOP\PascalABCNET.lnk" "$INSTDIR\PascalABCNET.exe"
    CreateShortCut "$SMPROGRAMS\PascalABC.NET\PascalABC.NET.lnk" "$INSTDIR\PascalABCNET.exe" 
    Push "PascalABCNET.exe"
    Call NGEN
    Push "ICSharpCode.TextEditor.dll"
	Call NGEN
    Push "Debugger.Core.dll"
    Call NGEN
    Push "CodeCompletion.dll"
    Call NGEN
    WriteUninstaller "$INSTDIR\Uninstall.exe"
    CreateShortCut "$SMPROGRAMS\PascalABC.NET\$(DESC_Remove) PascalABC.NET.lnk" "$INSTDIR\Uninstall.exe"
    System::Call 'Shell32::SHChangeNotify(i ${SHCNE_ASSOCCHANGED}, i ${SHCNF_IDLIST}, i 0, i 0)'  
    Delete "$PABCWorkNETPath\Output\*.pcu"
	FileOpen $4 "$INSTDIR\pabcworknet.ini" w
    FileWrite $4 "$PABCWorkNETPath"
    FileClose $4
  SectionEnd
