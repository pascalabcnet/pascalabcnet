Section $(DESC_SPython_Language) SPythonSection
    SectionIn 1 2
    
    ; Установка файлов SPython
    SetOutPath "$INSTDIR"
    File "..\bin\SPythonParser.dll"
    File "..\bin\SPythonLanguageInfo.dll"
    File "..\bin\SPythonStandardTreeConverter.dll"
    File "..\bin\SPythonSyntaxTreeVisitor.dll"
    
    ${AddFile} "SPythonParser.dll"
    ${AddFile} "SPythonLanguageInfo.dll"
    ${AddFile} "SPythonStandardTreeConverter.dll"
    ${AddFile} "SPythonSyntaxTreeVisitor.dll"
    
    ; Файл подсветки синтаксиса
    SetOutPath "$INSTDIR\Highlighting"
    File "..\bin\Highlighting\SPython.xshd"
    ${AddFile} "SPython.xshd"
    
    ; Оптимизация .NET библиотек через NGEN
    SetOutPath "$INSTDIR"
    Push "SPythonParser.dll"
    Call NGEN
    Push "SPythonLanguageInfo.dll"
    Call NGEN
    Push "SPythonStandardTreeConverter.dll"
    Call NGEN
    Push "SPythonSyntaxTreeVisitor.dll"
    Call NGEN

    DeleteRegKey HKCR "pys"
    ;insalling pys file type
    ReadRegStr $R0 HKCR ".pys" ""
    StrCmp $R0 "PascalABCNET.SPythonParser" 0 +2
      DeleteRegKey HKCR "PascalABCNET.SPythonParser"
    WriteRegStr HKCR ".pys" "" "PascalABCNET.SPythonParser"
    WriteRegStr HKCR "PascalABCNET.SPythonParser" "" "SPython Program"
    WriteRegStr HKCR "PascalABCNET.SPythonParser\DefaultIcon" "" "$INSTDIR\Ico\pas.ico"
    ReadRegStr $R0 HKCR "PascalABCNET.SPythonParser\shell\open\command" "" 
    StrCmp $R0 "" 0 no_nshopen
      WriteRegStr HKCR "PascalABCNET.SPythonParser\shell" "" "open"
      WriteRegStr HKCR "PascalABCNET.SPythonParser\shell\open\command" "" '"$INSTDIR\PascalABCNET.exe" "%1"'
    no_nshopen:
    WriteRegStr HKCR "PascalABCNET.SPythonParser\shell\compile" "" "Компилировать"
    WriteRegStr HKCR "PascalABCNET.SPythonParser\shell\compile\command" "" '"$INSTDIR\pabcnetc.exe" "%1"'

SectionEnd