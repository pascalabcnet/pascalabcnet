;RequestExecutionLevel admin

!include PascalABCNET_version.nsh

!define REGDIR "Software\PascalABC.NET"
!define INSTALLDIRREGKEY "Install Directory"
!define UninstLog "uninstall.log"

InstType $(DESC_Common)
InstType $(DESC_Minimal)

Var UninstLog
Var PABCWorkNETPath

!macro AddFile FileName
  FileWrite $UninstLog "$OUTDIR\${FileName}$\r$\n"
!macroend
!define AddFile "!insertmacro AddFile"

!macro AddDirectory Path
 FileWrite $UninstLog "${Path}$\r$\n"
!macroend
!define AddDirectory "!insertmacro AddDirectory"

!macro AddInstallerFile FileName
  FileWrite $UninstLog "$INSTDIR\${FileName}$\r$\n"
!macroend
!define AddInstallerFile "!insertmacro AddInstallerFile"

;--------------------------------
;Definitions

!define SHCNE_ASSOCCHANGED 0x8000000
!define SHCNF_IDLIST 0

;--------------------------------

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"
  !include "PABCWorkNETDriveChoose.nsdinc"
;--------------------------------
;General

  ;Name and file
  Name "PascalABC.NET v${VERSION}" "PascalABC.NET"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\PascalABC.NET"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU ${REGDIR} ""


;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING
  !define MUI_COMPONENTSPAGE_SMALLDESC
!define MUI_LANGDLL_ALLLANGUAGES
;--------------------------------
;Language Selection Dialog Settings

  ;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
  !define MUI_LANGDLL_REGISTRY_KEY "Software\PascalABC.NET" 
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

;--------------------------------
;Pages

  ;!insertmacro MUI_PAGE_LICENSE $(MUILicense)
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  Page custom fnc_PABCWorkNETDriveChoose_Show
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "Russian"
  !insertmacro MUI_LANGUAGE "English" 
;  !insertmacro MUI_LANGUAGE "German"

;--------------------------------
;Reserve Files
  
  ;If you are using solid compression, files that are required before
  ;the actual installation should be stored first in the data block,
  ;because this will make your installer start faster.
  
  !insertmacro MUI_RESERVEFILE_LANGDLL

  Section -openlogfile
 CreateDirectory "$INSTDIR"
 CreateDirectory "$PROGRAMFILES\..\PABCWork.NET"
 IfFileExists "$INSTDIR\${UninstLog}" +3
  FileOpen $UninstLog "$INSTDIR\${UninstLog}" w
 Goto +4
  SetFileAttributes "$INSTDIR\${UninstLog}" NORMAL
  FileOpen $UninstLog "$INSTDIR\${UninstLog}" a
  FileSeek $UninstLog 0 END
SectionEnd

Section
	${AddDirectory} "$INSTDIR\LibSource"
  ${AddDirectory} "$INSTDIR\LibSource\SPython"
	${AddDirectory} "$INSTDIR\Lib"
  ${AddDirectory} "$INSTDIR\Lib\SPython"
	${AddDirectory} "$INSTDIR\Highlighting"
	${AddDirectory} "$INSTDIR\PT4"
	${AddDirectory} "$INSTDIR\Ico"
	${AddDirectory} "$INSTDIR\Doc"
	${AddInstallerFile} "Uninstall.exe"
SectionEnd



