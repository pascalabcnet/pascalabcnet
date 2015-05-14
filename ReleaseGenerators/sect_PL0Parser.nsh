  Section "Парсер языка PL/0" PL0Parser
    SectionIn 1 2
    SetOutPath "$INSTDIR"
    File "..\bin\PL0Parser.dll"
	${AddFile} "PL0Parser.dll"
    SetOutPath "$INSTDIR\Highlighting"
    File "..\bin\Highlighting\PL0.xshd"
	${AddFile} "PL0.xshd"
    Push "PL0Parser.dll"
    Call NGEN
    SetOutPath "$INSTDIR\Ico"
    File "..\bin\Ico\pl0.ico"
	${AddFile} "pl0.ico"
    ;insalling PL0 file type
    ReadRegStr $R0 HKCR ".pl0" ""
    StrCmp $R0 "PascalABCNET.PL0Parser" 0 +2
      DeleteRegKey HKCR "PascalABCNET.PL0Parser"
    WriteRegStr HKCR ".pl0" "" "PascalABCNET.PL0Parser"
    WriteRegStr HKCR "PascalABCNET.PL0Parser" "" "PL/0 Program"
    WriteRegStr HKCR "PascalABCNET.PL0Parser\DefaultIcon" "" "$INSTDIR\Ico\pl0.ico"
    ReadRegStr $R0 HKCR "PascalABCNET.PL0NETParser\shell\open\command" "" 
    StrCmp $R0 "" 0 no_nshopen
      WriteRegStr HKCR "PascalABCNET.PL0Parser\shell" "" "open"
      WriteRegStr HKCR "PascalABCNET.PL0Parser\shell\open\command" "" '"$INSTDIR\PascalABCNET.exe" "%1"'
    no_nshopen:
    WriteRegStr HKCR "PascalABCNET.PL0Parser\shell\compile" "" "Компилировать"
    WriteRegStr HKCR "PascalABCNET.PL0Parser\shell\compile\command" "" '"$INSTDIR\pabcnetc.exe" "%1"'
    SetOutPath "${PABCWORK}\Samples\PL0"
    File /r "Samples\PL0\*"
  SectionEnd
