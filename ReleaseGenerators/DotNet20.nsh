!define WindowsInstallerPath "WindowsInstaller-KB893803-v2-x86.exe"
!define DotNet "NetFx20SP2_x86.exe"
!define LangPackEXE "NetFx20SP2_x86ru.exe"

SectionGroup "Microsoft .NET Framework" _DOTNET
Section "Microsoft .NET Framework 2.0" Framework
  SectionIn 1 2
;  IfFileExists "$WINDIR\Microsoft.NET\Framework\v2.0.50727\System.dll" +3 0
;    MessageBox MB_YESNO "Для работы компилятора необходима платформа Microsoft .NET версии 2.0 либо выше. Установить платформу Microsoft .NET Framework 2.0?" IDYES insalldotnet
;  Goto notinsalldotnet
;  Goto dotnetinstalled
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v2.0.50727\System.dll" 0 +2
    ReadRegDWORD $1 HKLM "Software\Microsoft\NET Framework Setup\NDP\v2.0.50727" "SP"
	IntCmp $1 2 dotnetinstalled installdotnet dotnetinstalled
  installdotnet:
    SetOutPath "$INSTDIR"
    File "DotNet20\${WindowsInstallerPath}"
    ExecWait '"$INSTDIR\${WindowsInstallerPath}" /Quiet /Norestart'    
    Delete "$INSTDIR\${WindowsInstallerPath}"
    File "DotNet20\${DotNet}"
    ExecWait '"$INSTDIR\${DotNet}" /Q'
    Delete "$INSTDIR\${DotNet}"
    Goto exit
  dotnetinstalled:
    MessageBox MB_OK "Платформа Microsoft .NET версии 2.0 Service Pack 2 уже установлена на этом компьютере."
  exit:    
SectionEnd 
Section "Framework Class Library Help" FrameworkHelp
  SectionIn 1
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v2.0.50727\ru\System.xml" 0 +2
    Goto dotnethelpinstalled
  SetOutPath "$WINDIR\Microsoft.NET\Framework\v2.0.50727\ru"
  File DotNet20\Tooltips\*.xml
  File DotNet20\Tooltips\Net30\*.xml
  Goto exithelp
  dotnethelpinstalled:
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v2.0.50727\ru\System.Core.xml" 0 +2
   Goto exithelp
  SetOutPath "$WINDIR\Microsoft.NET\Framework\v2.0.50727\ru"
  File DotNet20\Tooltips\Net30\*.xml  
  exithelp:    
SectionEnd 
Section "Russian Language Pack" LangPack
  SectionIn 1 
  IfFileExists "$WINDIR\Microsoft.NET\Framework\v2.0.50727\ru" 0 +4
    ReadRegDWORD $1 HKLM "Software\Microsoft\NET Framework Setup\NDP\v2.0.50727\1049" "Install"
	IntCmp $1 1 0 installlangpack 0
	ReadRegDWORD $1 HKLM "Software\Microsoft\NET Framework Setup\NDP\v2.0.50727\1049" "SP"
	IntCmp $1 2 dotnetlangpackinstalled installlangpack dotnetlangpackinstalled
  installlangpack:
	SetOutPath "$INSTDIR"
    File "DotNet20\${LangPackEXE}"
    ExecWait '"$INSTDIR\${LangPackEXE}" /Q'
    Delete "$INSTDIR\${LangPackEXE}"
  Goto exitlangpack
  dotnetlangpackinstalled:    
  exitlangpack:    
SectionEnd 
SectionGroupEnd