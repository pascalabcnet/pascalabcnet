  Section $(DESC_ConsoleCompiler) PABCNETC
    SectionIn 1 2 RO
    SetOutPath "$INSTDIR"
    File "..\bin\pabcnetc.exe"
	${AddFile} "pabcnetc.exe"
    Push "pabcnetc.exe"
    Call NGEN
    File "..\bin\pabcnetcclear.exe"
     ${AddFile} "pabcnetcclear.exe"
    Push "pabcnetcclear.exe"
    Call NGEN

;    CreateShortCut "$SMPROGRAMS\PascalABC.NET\pabcnetc.lnk" "$INSTDIR\pabcnetc.exe"      
  SectionEnd
