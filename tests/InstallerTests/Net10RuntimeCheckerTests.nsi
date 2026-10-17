; Headless test executable. Does not install anything or write to the registry.
Unicode true
RequestExecutionLevel user
SilentInstall silent
AutoCloseWindow true
!ifndef TEST_OUTPUT
!error "Pass /DTEST_OUTPUT=<isolated output exe>"
!endif
OutFile "${TEST_OUTPUT}"
!include "..\..\ReleaseGenerators\Net10RuntimeChecker.nsh"
Var TestLog
Var Failures

!macro CheckVersion Version Expected
  Push "${Version}"
  Call Net10.IsStableVersion
  Pop $0
  StrCmp $0 "${Expected}" +3
    IntOp $Failures $Failures + 1
    FileWrite $TestLog "FAIL version ${Version}: $0$\r$\n"
  FileWrite $TestLog "version ${Version}: $0$\r$\n"
!macroend

!macro Touch Path
  FileOpen $0 "${Path}" w
  FileWrite $0 "test fixture"
  FileClose $0
!macroend

!macro CheckRoot Expected Name
  Call Net10.CheckRuntimeAtRoot
  StrCmp $Net10RuntimeFound "${Expected}" +3
    IntOp $Failures $Failures + 1
    FileWrite $TestLog "FAIL ${Name}: $Net10RuntimeFound$\r$\n"
  FileWrite $TestLog "${Name}: $Net10RuntimeFound$\r$\n"
!macroend

Section
  StrCpy $Failures 0
  FileOpen $TestLog "$EXEDIR\runtime-checker-results.txt" w
  !insertmacro CheckVersion "10.0.0" 1
  !insertmacro CheckVersion "10.0.19" 1
  !insertmacro CheckVersion "9.0.10" 0
  !insertmacro CheckVersion "11.0.0" 0
  !insertmacro CheckVersion "10.0.0-preview.1" 0
  !insertmacro CheckVersion "10.0.0-rc.2" 0
  !insertmacro CheckVersion "10.0." 0
  !insertmacro CheckVersion "10.0.1x" 0
  !insertmacro CheckVersion "10.0.1.2" 0
  !insertmacro CheckVersion "" 0
  StrCpy $Net10DotnetRoot "$EXEDIR\fixture"
  CreateDirectory "$Net10DotnetRoot"
  !insertmacro CheckRoot 0 "no host"
  !insertmacro Touch "$Net10DotnetRoot\dotnet.exe"
  !insertmacro CheckRoot 0 "host only"
  CreateDirectory "$Net10DotnetRoot\shared\Microsoft.NETCore.App\10.0.19"
  !insertmacro Touch "$Net10DotnetRoot\shared\Microsoft.NETCore.App\10.0.19\coreclr.dll"
  !insertmacro CheckRoot 0 "core runtime without desktop"
  CreateDirectory "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\9.0.10"
  !insertmacro Touch "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\9.0.10\System.Windows.Forms.dll"
  !insertmacro Touch "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\9.0.10\Microsoft.WindowsDesktop.App.runtimeconfig.json"
  !insertmacro CheckRoot 0 "desktop 9 only"
  CreateDirectory "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\10.0.0-rc.2"
  !insertmacro Touch "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\10.0.0-rc.2\System.Windows.Forms.dll"
  !insertmacro Touch "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\10.0.0-rc.2\Microsoft.WindowsDesktop.App.runtimeconfig.json"
  !insertmacro CheckRoot 0 "desktop 10 preview only"
  CreateDirectory "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\10.0.7"
  !insertmacro Touch "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\10.0.7\System.Windows.Forms.dll"
  !insertmacro CheckRoot 0 "incomplete desktop installation"
  !insertmacro Touch "$Net10DotnetRoot\shared\Microsoft.WindowsDesktop.App\10.0.7\Microsoft.WindowsDesktop.App.runtimeconfig.json"
  !insertmacro CheckRoot 1 "stable desktop 10 with independently patched core"
  SetErrors
  !insertmacro CheckRoot 1 "stable runtime after a preceding installer error"
  Delete "$Net10DotnetRoot\dotnet.exe"
  !insertmacro CheckRoot 0 "stale installation without host"
  Call Net10.DetectRuntime
  FileWrite $TestLog "machine detection (read-only): $Net10RuntimeFound; $Net10DotnetRoot$\r$\n"
  StrCpy $1 $Net10RuntimeFound
  SetErrors
  Call Net10.DetectRuntime
  StrCmp $Net10RuntimeFound $1 +3
    IntOp $Failures $Failures + 1
    FileWrite $TestLog "FAIL machine detection depends on preceding installer error$\r$\n"
  FileWrite $TestLog "machine detection after SetErrors: $Net10RuntimeFound$\r$\n"
  FileWrite $TestLog "Failures: $Failures$\r$\n"
  FileClose $TestLog
  SetErrorLevel $Failures
SectionEnd
