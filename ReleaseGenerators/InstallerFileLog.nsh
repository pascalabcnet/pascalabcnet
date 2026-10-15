!ifndef PABC_INSTALLER_FILE_LOG
!define PABC_INSTALLER_FILE_LOG
!macro PabcWriteLoggedPath Path
 !ifdef PABC_UNICODE_UNINSTALL_LOG
   FileWriteUTF16LE /BOM $UninstLog "${Path}$\r$\n"
 !else
   FileWrite $UninstLog "${Path}$\r$\n"
 !endif
!macroend

!macro PabcReadLoggedPath Handle Output
 !ifdef PABC_UNICODE_UNINSTALL_LOG
   FileReadUTF16LE ${Handle} ${Output}
 !else
   FileRead ${Handle} ${Output}
 !endif
!macroend

!macro AddFile FileName
 !insertmacro PabcWriteLoggedPath "$OUTDIR\${FileName}"
!macroend
!define AddFile "!insertmacro AddFile"

!macro AddDirectory Path
 !insertmacro PabcWriteLoggedPath "${Path}"
!macroend
!define AddDirectory "!insertmacro AddDirectory"

!macro AddInstallerFile FileName
 !insertmacro PabcWriteLoggedPath "$INSTDIR\${FileName}"
!macroend
!define AddInstallerFile "!insertmacro AddInstallerFile"
!endif
