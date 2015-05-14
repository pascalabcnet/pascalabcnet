SetCompressor /SOLID lzma
OutFile "..\Release\PascalABCNETSetup.exe"
!include PascalABCNET_head.nsh
!include PascalABCNET_sections.nsh
!include PascalABCNET_end.nsh

Function .onInit
	IfFileExists "$WINDIR\Microsoft.NET\Framework\v4.0.30319\System.dll" +3 0
	MessageBox MB_OK $(DESC_Framework_Required)
	Abort
	!insertmacro MUI_LANGDLL_DISPLAY
	Goto exit
	exit:
FunctionEnd