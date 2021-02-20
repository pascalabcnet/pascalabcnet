SetCompressor /SOLID lzma
OutFile "..\Release\PascalABCNETSetup.exe"
!include PascalABCNET_head.nsh
!include DotNetChecker.nsh
!include DotNet47.nsh
!include PascalABCNET_sections.nsh
!include PascalABCNET_end.nsh
!include "nsProcess.nsh"

Function .onInit
	IfFileExists "$WINDIR\Microsoft.NET\Framework\v4.0.30319\System.dll" +3 0
	MessageBox MB_OK $(DESC_Framework_Required)
	Abort
	!insertmacro MUI_LANGDLL_DISPLAY
	Goto exit
exit:
checkprocess:
    ${nsProcess::FindProcess} "PascalABCNET.exe" $R0
    StrCmp $R0 0 0 notRunning
    MessageBox MB_RETRYCANCEL|MB_ICONEXCLAMATION $(DESC_IsRunning) IDRETRY checkprocess IDCANCEL end
    end: 
    Abort
notRunning:
FunctionEnd