!define DotNet "NDP471-KB4033342-x86-x64-AllOS-ENU.exe"

SectionGroup "Microsoft .NET Framework" _DOTNET
Section "Microsoft .NET Framework 4.7" Framework
  SectionIn 1 2
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v4.0.30319\System.dll" 0 +2
    ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"
	IntCmp $1 460805 dotnetinstalled installdotnet dotnetinstalled
	Goto dotnetinstalled
  installdotnet:
    SetOutPath "$INSTDIR"
    File "DotNet471\${DotNet}"
    ExecWait '"$INSTDIR\${DotNet}" /Q'
    Delete "$INSTDIR\${DotNet}"
    Goto exit
  dotnetinstalled:
    ;MessageBox MB_OK "Платформа Microsoft .NET версии 4.0 уже установлена на этом компьютере."
  exit:    
SectionEnd 
Section "Framework Class Library Help" FrameworkHelp
  SectionIn 1
  IfFileExists "$WINDIR\Microsoft.NET\Framework64" 0 +5
  IfFileExists "$WINDIR\Microsoft.NET\Framework64\v4.0.30319\ru\System.xml" 0 +2
    Goto dotnet40helpinstalled
  SetOutPath "$WINDIR\Microsoft.NET\Framework64\v4.0.30319\ru"
  Goto copyfiles
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v4.0.30319\ru\System.xml" 0 +2
    Goto dotnet40helpinstalled
  SetOutPath "$WINDIR\Microsoft.NET\Framework\v4.0.30319\ru"
  copyfiles:
  File DotNet40\Tooltips\*.xml
  Goto exithelp
  dotnet40helpinstalled:
  exithelp:      
SectionEnd 

SectionGroupEnd