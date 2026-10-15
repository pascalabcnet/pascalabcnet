!ifndef PABC_NET10_RUNTIME_CHECKER
!define PABC_NET10_RUNTIME_CHECKER
!include x64.nsh

Var Net10DotnetRoot
Var Net10RuntimeFound

; Input: version on stack; output: 1 for a stable 10.0.<digits>, otherwise 0.
; Preview/RC and later major versions do not satisfy the requirement.
Function Net10.IsStableVersion
  Exch $R0
  Push $R1
  Push $R2
  StrCpy $R1 $R0 5
  StrCmp $R1 "10.0." 0 invalid
  StrCpy $R2 5
  StrCpy $R1 $R0 1 $R2
  StrCmp $R1 "" invalid
digit:
  StrCmp $R1 "0" next
  StrCmp $R1 "1" next
  StrCmp $R1 "2" next
  StrCmp $R1 "3" next
  StrCmp $R1 "4" next
  StrCmp $R1 "5" next
  StrCmp $R1 "6" next
  StrCmp $R1 "7" next
  StrCmp $R1 "8" next
  StrCmp $R1 "9" next invalid
next:
  IntOp $R2 $R2 + 1
  StrCpy $R1 $R0 1 $R2
  StrCmp $R1 "" valid digit
valid:
  StrCpy $R0 1
  Goto done
invalid:
  StrCpy $R0 0
done:
  Pop $R2
  Pop $R1
  Exch $R0
FunctionEnd

; Check real files as well: registry leftovers alone must not enable the mode.
Function Net10.CheckRuntimeAtRoot
  Push $R0
  Push $R1
  Push $R2
  StrCpy $Net10RuntimeFound 0
  IfFileExists "$Net10DotnetRoot\dotnet.exe" 0 done
  ; Successful FindFirst does not clear an error left by another installer section.
  ClearErrors
  FindFirst $R0 $R1 "$Net10DotnetRoot\shared\Microsoft.NETCore.App\10.0.*"
  IfErrors done
core_loop:
  Push $R1
  Call Net10.IsStableVersion
  Pop $R2
  StrCmp $R2 1 0 core_next
  IfFileExists "$Net10DotnetRoot\shared\Microsoft.NETCore.App\$R1\coreclr.dll" core_found
core_next:
  FindNext $R0 $R1
  IfErrors core_missing core_loop
core_missing:
  FindClose $R0
  Goto done
core_found:
  FindClose $R0
  ClearErrors
  FindFirst $R0 $R1 "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\10.0.*"
  IfErrors done
desktop_loop:
  Push $R1
  Call Net10.IsStableVersion
  Pop $R2
  StrCmp $R2 1 0 desktop_next
  IfFileExists "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\$R1\System.Windows.Forms.dll" 0 desktop_next
  IfFileExists "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\$R1\Microsoft.WindowsDesktop.App.runtimeconfig.json" 0 desktop_next
  StrCpy $Net10RuntimeFound 1
  FindClose $R0
  Goto done
desktop_next:
  FindNext $R0 $R1
  IfErrors desktop_missing desktop_loop
desktop_missing:
  FindClose $R0
done:
  Pop $R2
  Pop $R1
  Pop $R0
FunctionEnd

Function Net10.DetectRuntime
  StrCpy $Net10RuntimeFound 0
  StrCpy $Net10DotnetRoot ""
  ${IfNot} ${RunningX64}
    Return
  ${EndIf}
  ; Microsoft registers InstallLocation in the 32-bit registry view, even for x64.
  SetRegView 32
  ClearErrors
  ReadRegStr $Net10DotnetRoot HKLM "SOFTWARE\dotnet\Setup\InstalledVersions\x64" "InstallLocation"
  SetRegView lastused
  StrCmp $Net10DotnetRoot "" 0 check
  StrCpy $Net10DotnetRoot "$PROGRAMFILES64\dotnet"
check:
  Call Net10.CheckRuntimeAtRoot
FunctionEnd
!endif
