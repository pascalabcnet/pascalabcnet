SetCompressor /SOLID lzma

InstType "Полная"

;--------------------------------
;Definitions

!define SHCNE_ASSOCCHANGED 0x8000000
!define SHCNF_IDLIST 0

;--------------------------------

;--------------------------------
;Include Modern UI

  !include "MUI.nsh"

;--------------------------------
;General

  ;Name and file
  Name "BF Languge Parser"
  OutFile "..\Release\BFParserSetup.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\PascalABC.NET"
  
  ;Get installation folder from registry if available
  ;InstallDirRegKey HKCU "Software\PascalABC.NET" ""


;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING
  !define MUI_COMPONENTSPAGE_SMALLDESC

;--------------------------------
;Language Selection Dialog Settings

  ;Remember the installer language
;  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
;  !define MUI_LANGDLL_REGISTRY_KEY "Software\PascalABC.NET" 
;  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

;--------------------------------
;Pages

;  !insertmacro MUI_PAGE_LICENSE "License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "Russian"
;  !insertmacro MUI_LANGUAGE "English" 
;  !insertmacro MUI_LANGUAGE "German"

;--------------------------------
;Reserve Files
  
  ;If you are using solid compression, files that are required before
  ;the actual installation should be stored first in the data block,
  ;because this will make your installer start faster.
  
  !insertmacro MUI_RESERVEFILE_LANGDLL





;--------------------------------
;Installer Sections


  Section "Парсер языка BF" BFParser
    SectionIn 1 RO
    SetOutPath "$INSTDIR"
    File "..\bin\BFParser.dll"
    SetOutPath "$INSTDIR\Lib"
    File "..\bin\lib\BFSystem.pas"
    SetOutPath "$INSTDIR\Highlighting"
    File "..\bin\Highlighting\BF.xshd"
    File ExecHide.exe
    Push "BFParser.dll"
    Call NGEN
    Delete ExecHide.exe
    SetOutPath "$INSTDIR\Ico"
    File "..\bin\Ico\bf.ico"
    ;insalling BF file type
    ReadRegStr $R0 HKCR ".bf" ""
    StrCmp $R0 "PascalABCNET.BFParser" 0 +2
      DeleteRegKey HKCR "PascalABCNET.BFParser"
    WriteRegStr HKCR ".bf" "" "PascalABCNET.BFParser"
    WriteRegStr HKCR "PascalABCNET.BFParser" "" "BF Program"
    WriteRegStr HKCR "PascalABCNET.BFParser\DefaultIcon" "" "$INSTDIR\Ico\bf.ico"
    ReadRegStr $R0 HKCR "PascalABCNET.BFParser\shell\open\command" "" 
    StrCmp $R0 "" 0 no_nshopen
      WriteRegStr HKCR "PascalABCNET.BFParser\shell" "" "open"
      WriteRegStr HKCR "PascalABCNET.BFParser\shell\open\command" "" '"$INSTDIR\PascalABCNET.exe" "%1"'
    no_nshopen:
    WriteRegStr HKCR "PascalABCNET.BFParser\shell\compile" "" "Компилировать"
    WriteRegStr HKCR "PascalABCNET.BFParser\shell\compile\command" "" '"$INSTDIR\pabcnetc.exe" "%1"'
    CreateDirectory "$SMPROGRAMS\PascalABC.NET\Парсер языка BF"
    WriteUninstaller "$INSTDIR\BFParserUninstall.exe"
    CreateShortCut "$SMPROGRAMS\PascalABC.NET\Парсер языка BF\Удаление парсера BF.lnk" "$INSTDIR\BFParserUninstall.exe"
    System::Call 'Shell32::SHChangeNotify(i ${SHCNE_ASSOCCHANGED}, i ${SHCNF_IDLIST}, i 0, i 0)'  
  SectionEnd


Section "Примеры" Samples
  SectionIn 1
  SetOutPath "${PABCWORK}\BFSamples"
  File /r "Samples\BF\*"  
  CreateShortCut "$SMPROGRAMS\PascalABC.NET\Парсер языка BF\Samples.lnk" "$INSTDIR\BFSamples"
SectionEnd



Section Uninstall
  Delete "$INSTDIR\Ico\BF.ico"
  Delete "$INSTDIR\BFParser.dll"
  Delete "$INSTDIR\Highlighting\BF.xshd"
  RMDir /r $INSTDIR\BFSamples
  RMDir /r "$SMPROGRAMS\PascalABC.NET\Парсер языка BF"
  ReadRegStr $R0 HKCR ".bf" ""
  StrCmp $R0 "PascalABCNET.BFParser" 0 +2
    DeleteRegKey HKCR ".bf"
  DeleteRegKey HKCR "PascalABCNET.BFParser"
  System::Call 'Shell32::SHChangeNotify(i ${SHCNE_ASSOCCHANGED}, i ${SHCNF_IDLIST}, i 0, i 0)'
SectionEnd

;--------------------------------
;Installer Functions

Function .onInit

;  !insertmacro MUI_LANGDLL_DISPLAY

FunctionEnd


;--------------------------------
;Descriptions

  ;USE A LANGUAGE STRING IF YOU WANT YOUR DESCRIPTIONS TO BE LANGAUGE SPECIFIC

  ;Assign descriptions to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${BFParser} "Парсер языка BF"
    !insertmacro MUI_DESCRIPTION_TEXT ${Samples} "Файлы примеров"
  !insertmacro MUI_FUNCTION_DESCRIPTION_END


Icon Install.ico
UninstallIcon Uninstall.ico
 
;--------------------------------
;Uninstaller Section


;--------------------------------
;Uninstaller Functions

Function un.onInit

  ;!insertmacro MUI_UNGETLANGUAGE
  
FunctionEnd

Function NGEN
  Pop $0
  ExecWait '"$INSTDIR\ExecHide.exe" "$WINDIR\Microsoft.NET\Framework\v2.0.50727\ngen.exe" install "$INSTDIR\$0"'
FunctionEnd
;!include Functions.nsh