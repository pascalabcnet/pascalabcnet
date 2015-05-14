!define WindowsInstallerPath "WindowsInstaller-KB893803-v2-x86.exe"
!define DotNet "dotNetFx40_Full_x86_x64.exe"
!define LangPackEXE "dotNetFx40LP_Full_x86ru.exe"

SectionGroup "Microsoft .NET Framework" _DOTNET
Section "Microsoft .NET Framework 4.0" Framework
  SectionIn 1 2
;  IfFileExists "$WINDIR\Microsoft.NET\Framework\v2.0.50727\System.dll" +3 0
;    MessageBox MB_YESNO "Для работы компилятора необходима платформа Microsoft .NET версии 2.0 либо выше. Установить платформу Microsoft .NET Framework 2.0?" IDYES insalldotnet
;  Goto notinsalldotnet
;  Goto dotnetinstalled
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v4.0.30319\System.dll" 0 +2
    ;ReadRegDWORD $1 HKLM "Software\Microsoft\NET Framework Setup\NDP\v2.0.50727" "SP"
	;IntCmp $1 2 dotnetinstalled installdotnet dotnetinstalled
	Goto dotnetinstalled
  installdotnet:
    SetOutPath "$INSTDIR"
    File "DotNet40\${WindowsInstallerPath}"
    ExecWait '"$INSTDIR\${WindowsInstallerPath}" /Quiet /Norestart'    
    Delete "$INSTDIR\${WindowsInstallerPath}"
    File "DotNet40\${DotNet}"
    ExecWait '"$INSTDIR\${DotNet}" /Q'
    Delete "$INSTDIR\${DotNet}"
    Goto exit
  dotnetinstalled:
    ;MessageBox MB_OK "Платформа Microsoft .NET версии 4.0 уже установлена на этом компьютере."
  exit:    
SectionEnd 
Section "Russian Language Pack" LangPack
  SectionIn 1 
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v4.0.30319\ru\mscorlib.resources.dll" 0 +2
	Goto dotnetlangpackinstalled
  installlangpack:
	SetOutPath "$INSTDIR"
    File "DotNet40\${LangPackEXE}"
    ExecWait '"$INSTDIR\${LangPackEXE}" /Q'
    Delete "$INSTDIR\${LangPackEXE}"
  Goto exitlangpack
  dotnetlangpackinstalled:    
  exitlangpack:    
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