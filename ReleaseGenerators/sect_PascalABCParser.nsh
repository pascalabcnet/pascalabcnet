  Section $(DESC_PascalABCNET_Language) PascalABCParser
    SectionIn 1 2 RO
    SetOutPath "$INSTDIR"
    File "..\bin\PascalABCParser.dll"
    File "..\bin\PascalLanguage.dll"
	${AddFile} "PascalABCParser.dll"
  ${AddFile} "PascalLanguage.dll"
;    File "..\bin\PascalABCPartParser.dll"
    SetOutPath "$INSTDIR\Highlighting"
    File "..\bin\Highlighting\PascalABCNET.xshd"
	${AddFile} "PascalABCNET.xshd"
    Push "PascalABCParser.dll"
    Call NGEN
;    Push "PascalABCPartParser.dll"
;    Call NGEN
    Push "PascalLanguage.dll"
    Call NGEN
    SetOutPath "$INSTDIR\Ico"
    File "..\bin\Ico\pas.ico"
	${AddFile} "pas.ico"
	File "..\bin\Ico\pabcproj.ico"
	${AddFile} "pabcproj.ico"
	
	;remove PAS file type
    DeleteRegKey HKCR "pabcproj"
    ;insalling PAS file type
    ReadRegStr $R0 HKCR ".pabcproj" ""
    StrCmp $R0 "PascalABCNET.PascalABCNETProject" 0 +2
      DeleteRegKey HKCR "PascalABCNET.PascalABCNETProject"
    WriteRegStr HKCR ".pabcproj" "" "PascalABCNET.PascalABCNETProject"
    WriteRegStr HKCR "PascalABCNET.PascalABCNETProject" "" "PascalABC.NET Project"
    WriteRegStr HKCR "PascalABCNET.PascalABCNETProject\DefaultIcon" "" "$INSTDIR\Ico\pabcproj.ico"
    ReadRegStr $R0 HKCR "PascalABCNET.PascalABCNETProject\shell\open\command" "" 
    StrCmp $R0 "" 0 no_nshopen
      WriteRegStr HKCR "PascalABCNET.PascalABCNETProject\shell" "" "open"
      WriteRegStr HKCR "PascalABCNET.PascalABCNETProject\shell\open\command" "" '"$INSTDIR\PascalABCNET.exe" "%1"'
    no_nshopen:
    WriteRegStr HKCR "PascalABCNET.PascalABCNETProject\shell\compile" "" $(DESC_Compile)
    WriteRegStr HKCR "PascalABCNET.PascalABCNETProject\shell\compile\command" "" '"$INSTDIR\pabcnetc.exe" "%1"'
  SectionEnd
  Section $(DESC_PascalABCParserRegistation) PascalABCParserRegistation
    SectionIn 1 2
    ;remove PAS file type
    DeleteRegKey HKCR "pas"
    ;insalling PAS file type
    ReadRegStr $R0 HKCR ".pas" ""
    StrCmp $R0 "PascalABCNET.PascalABCNETParser" 0 +2
      DeleteRegKey HKCR "PascalABCNET.PascalABCNETParser"
    WriteRegStr HKCR ".pas" "" "PascalABCNET.PascalABCNETParser"
    WriteRegStr HKCR "PascalABCNET.PascalABCNETParser" "" "Pascal Program"
    WriteRegStr HKCR "PascalABCNET.PascalABCNETParser\DefaultIcon" "" "$INSTDIR\Ico\pas.ico"
    ReadRegStr $R0 HKCR "PascalABCNET.PascalABCNETParser\shell\open\command" "" 
    StrCmp $R0 "" 0 no_nshopen
      WriteRegStr HKCR "PascalABCNET.PascalABCNETParser\shell" "" "open"
      WriteRegStr HKCR "PascalABCNET.PascalABCNETParser\shell\open\command" "" '"$INSTDIR\PascalABCNET.exe" "%1"'
    no_nshopen:
    WriteRegStr HKCR "PascalABCNET.PascalABCNETParser\shell\compile" "" "Компилировать"
    WriteRegStr HKCR "PascalABCNET.PascalABCNETParser\shell\compile\command" "" '"$INSTDIR\pabcnetc.exe" "%1"'
;    WriteRegStr HKCR "PascalABCNET.PascalABCNETParser\shell\openinpascalabc" "" "Открыть в Pascal ABC"
;    WriteRegStr HKCR "PascalABCNET.PascalABCNETParser\shell\compile\openinpascalabc" "" '"$INSTDIR\pabcnetc.exe" "%1"'
  SectionEnd