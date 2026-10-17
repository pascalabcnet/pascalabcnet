; Installs ONLY the modern payload into this test EXE's isolated directory.
; Never includes the product's GAC/shortcuts/associations sections.
Unicode true
RequestExecutionLevel user
SilentInstall silent
SilentUnInstall silent
AutoCloseWindow true
!ifndef TEST_OUTPUT
!error "Pass /DTEST_OUTPUT=<isolated output exe>"
!endif
OutFile "${TEST_OUTPUT}"
!addincludedir "${__FILEDIR__}\..\..\ReleaseGenerators"
!include MUI2.nsh
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_LANGUAGE "Russian"
!define UninstLog "uninstall.log"
!define PABC_UNICODE_UNINSTALL_LOG
Var UninstLog
Var PABCWorkNETPath
!include InstallerFileLog.nsh

Function .onInit
  StrCpy $INSTDIR "$EXEDIR\installed"
  StrCpy $LANGUAGE 1049
FunctionEnd

Section -start
  CreateDirectory "$INSTDIR"
  FileOpen $UninstLog "$INSTDIR\${UninstLog}" w
SectionEnd

!include sect_Net10.nsh

Section -finish
  FileOpen $0 "$INSTDIR\localized-label.txt" w
  FileWriteUTF16LE /BOM $0 "$(DESC_Net10_Missing)"
  FileClose $0
  ${AddInstallerFile} "localized-label.txt"
  WriteUninstaller "$INSTDIR\payload-uninstall.exe"
  ${AddInstallerFile} "payload-uninstall.exe"
  FileClose $UninstLog
SectionEnd

Section Uninstall
  Call un.DeleteFiles
SectionEnd
!include Functions.nsh
