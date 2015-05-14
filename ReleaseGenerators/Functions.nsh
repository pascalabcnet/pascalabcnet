Function NGEN
  Pop $0
  IfFileExists "$WINDIR\Microsoft.NET\Framework64\v4.0.30319\ngen.exe" 0 +3
	ExecWait '"$INSTDIR\ExecHide.exe" "$WINDIR\Microsoft.NET\Framework64\v4.0.30319\ngen.exe" install "$INSTDIR\$0"'
	Goto exitngen
  ExecWait '"$INSTDIR\ExecHide.exe" "$WINDIR\Microsoft.NET\Framework\v4.0.30319\ngen.exe" install "$INSTDIR\$0"'
  exitngen:
FunctionEnd

Function un.UNGEN
 ExecWait '"$WINDIR\Microsoft.NET\Framework\v4.0.30319\ngen.exe" uninstall "$INSTDIR\$0"'
FunctionEnd

Function PABCWorkNET_Path_Changed
Pop $PABCWorkNETPath
FunctionEnd

Function un.DeleteFiles
 Push $R0
 Push $R1
 Push $R2
 
 ;RMDir /r "$INSTDIR\Lib"
 RMDir /r "$INSTDIR\Lng"
 RMDir /r "$INSTDIR\Temp"
 
 ;IfFileExists "$INSTDIR\PT4\*.*" 0 +3
  ;CopyFiles "$INSTDIR\PT4\PT.ini" "$INSTDIR"
  ;CopyFiles "$INSTDIR\PT4\Results.abc" "$INSTDIR"
  ;RMDir /r "$INSTDIR\PT4"
  
 SetFileAttributes "$INSTDIR\${UninstLog}" NORMAL
 FileOpen $UninstLog "$INSTDIR\${UninstLog}" r
 StrCpy $R1 0
 
 GetLineCount:
  ClearErrors
   FileRead $UninstLog $R0
   IntOp $R1 $R1 + 1
   IfErrors 0 GetLineCount
 
 LoopRead:
  FileSeek $UninstLog 0 SET
  StrCpy $R2 0
  FindLine:
   FileRead $UninstLog $R0
   IntOp $R2 $R2 + 1
   StrCmp $R1 $R2 0 FindLine
 
   StrCpy $R0 $R0 -2
   IfFileExists "$R0\*.*" 0 +3
    RMDir $R0  #is dir
   Goto +3
   IfFileExists $R0 0 +2
    Delete $R0 #is file
	
  IntOp $R1 $R1 - 1
  StrCmp $R1 0 LoopDone
  Goto LoopRead
 LoopDone:
 FileClose $UninstLog
 Delete "$INSTDIR\${UninstLog}"
 RMDir "$INSTDIR"
 Pop $R2
 Pop $R1
 Pop $R0
FunctionEnd