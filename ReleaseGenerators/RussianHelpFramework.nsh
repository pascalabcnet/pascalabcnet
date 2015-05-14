
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
