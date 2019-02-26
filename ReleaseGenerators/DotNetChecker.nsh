!macro CheckNetFramework FrameworkVersion
	Var /GLOBAL dotNetUrl${FrameworkVersion}
	Var /GLOBAL dotNetReadableVersion${FrameworkVersion}

	!ifndef DOTNET472_URL
	!define DOTNET472_URL       "https://go.microsoft.com/fwlink/?LinkId=863265"
	!define DOTNET471_URL       "https://go.microsoft.com/fwlink/?LinkId=852104"
	!define DOTNET47_URL        "https://go.microsoft.com/fwlink/?LinkId=825302"
	!define DOTNET462_URL       "https://go.microsoft.com/fwlink/?LinkId=780600"
	!define DOTNET461_URL       "https://go.microsoft.com/fwlink/?LinkId=671743"
	!define DOTNET46_URL        "https://go.microsoft.com/fwlink/?LinkId=528232"
	!define DOTNET452_URL       "https://go.microsoft.com/fwlink/?LinkId=397708"
	!define DOTNET451_URL       "https://go.microsoft.com/fwlink/?LinkId=322116"
	!define DOTNET45_URL        "https://go.microsoft.com/fwlink/?LinkId=225702"
	!define DOTNET40Full_URL    "https://www.microsoft.com/downloads/info.aspx?na=41&srcfamilyid=0a391abd-25c1-4fc0-919f-b21f31ab88b7&srcdisplaylang=en&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2f9%2f5%2fA%2f95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE%2fdotNetFx40_Full_x86_x64.exe"
	!define DOTNET40Client_URL  "https://www.microsoft.com/downloads/info.aspx?na=41&srcfamilyid=e5ad0459-cbcc-4b4f-97b6-fb17111cf544&srcdisplaylang=en&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2f5%2f6%2f2%2f562A10F9-C9F4-4313-A044-9C94E0A8FAC8%2fdotNetFx40_Client_x86_x64.exe"
	!define DOTNET35_URL        "https://download.microsoft.com/download/2/0/e/20e90413-712f-438c-988e-fdaa79a8ac3d/dotnetfx35.exe"
	!define DOTNET30_URL        "https://download.microsoft.com/download/2/0/e/20e90413-712f-438c-988e-fdaa79a8ac3d/dotnetfx35.exe"
	!define DOTNET20_URL        "https://www.microsoft.com/downloads/info.aspx?na=41&srcfamilyid=0856eacb-4362-4b0d-8edd-aab15c5e04f5&srcdisplaylang=en&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2f5%2f6%2f7%2f567758a3-759e-473e-bf8f-52154438565a%2fdotnetfx.exe"
	!define DOTNET11_URL        "https://www.microsoft.com/downloads/info.aspx?na=41&srcfamilyid=262d25e3-f589-4842-8157-034d1e7cf3a3&srcdisplaylang=en&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2fa%2fa%2fc%2faac39226-8825-44ce-90e3-bf8203e74006%2fdotnetfx.exe"
	!define DOTNET10_URL        "https://www.microsoft.com/downloads/info.aspx?na=41&srcfamilyid=262d25e3-f589-4842-8157-034d1e7cf3a3&srcdisplaylang=en&u=http%3a%2f%2fdownload.microsoft.com%2fdownload%2fa%2fa%2fc%2faac39226-8825-44ce-90e3-bf8203e74006%2fdotnetfx.exe"
	!endif

	${If} ${FrameworkVersion} == "472"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET472_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.7.2"
	${ElseIf} ${FrameworkVersion} == "471"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET471_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.7.1"
	${ElseIf} ${FrameworkVersion} == "47"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET47_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.7"
	${ElseIf} ${FrameworkVersion} == "462"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET462_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.6.2"
	${ElseIf} ${FrameworkVersion} == "461"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET461_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.6.1"
	${ElseIf} ${FrameworkVersion} == "46"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET46_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.6"
	${ElseIf} ${FrameworkVersion} == "452"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET452_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.52"
	${ElseIf} ${FrameworkVersion} == "451"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET451_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.51"
	${ElseIf} ${FrameworkVersion} == "45"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET45_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.5"
	${ElseIf} ${FrameworkVersion} == "40Full"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET40Full_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.0 Full"
	${ElseIf} ${FrameworkVersion} == "40Client"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET40Client_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "4.0 Client"
	${ElseIf} ${FrameworkVersion} == "35"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET35_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "3.5"
	${ElseIf} ${FrameworkVersion} == "30"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET30_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "3.0"
	${ElseIf} ${FrameworkVersion} == "20"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET20_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "2.0"
	${ElseIf} ${FrameworkVersion} == "11"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET11_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "1.1"
	${ElseIf} ${FrameworkVersion} == "10"
		StrCpy $dotNetUrl${FrameworkVersion} ${DOTNET10_URL}
		StrCpy $dotNetReadableVersion${FrameworkVersion} "1.0"
	${EndIf}

	DetailPrint "Checking .NET Framework version..."

	Push $0
	Push $1
	Push $2
	Push $3
	Push $4
	Push $5
	Push $6
	Push $7

	DotNetChecker::IsDotNet${FrameworkVersion}Installed
	Pop $0

	${If} $0 == "false"
	${OrIf} $0 == "f"  ; if script is compiled in ANSI mode then we get only an "f"  https://github.com/ReVolly/NsisDotNetChecker/issues/4
		DetailPrint ".NET Framework $dotNetReadableVersion${FrameworkVersion} not found, download is required for program to run."
		Goto NoDotNET${FrameworkVersion}
	${Else}
		DetailPrint ".NET Framework $dotNetReadableVersion${FrameworkVersion} found, no need to install."
		Goto NewDotNET${FrameworkVersion}
	${EndIf}

NoDotNET${FrameworkVersion}:
	MessageBox MB_YESNOCANCEL|MB_ICONEXCLAMATION \
	".NET Framework not installed. Required version: $dotNetReadableVersion${FrameworkVersion}.$\nDownload .NET Framework $dotNetReadableVersion${FrameworkVersion} from microsoft.com?" \
	/SD IDYES IDYES DownloadDotNET${FrameworkVersion} IDNO NewDotNET${FrameworkVersion}
	goto GiveUpDotNET${FrameworkVersion} ;IDCANCEL

DownloadDotNET${FrameworkVersion}:
	DetailPrint "Beginning download of .NET Framework $dotNetReadableVersion${FrameworkVersion}."
	NSISDL::download $dotNetUrl${FrameworkVersion} "$TEMP\dotnetfx.exe"
	DetailPrint "Completed download."

	Pop $0
	${If} $0 == "cancel"
		MessageBox MB_YESNO|MB_ICONEXCLAMATION \
		"Download canceled.  Continue Installation?" \
		IDYES NewDotNET${FrameworkVersion} IDNO GiveUpDotNET${FrameworkVersion}
	${ElseIf} $0 != "success"
		MessageBox MB_YESNO|MB_ICONEXCLAMATION \
		"Download failed:$\n$0$\n$\nContinue Installation?" \
		IDYES NewDotNET${FrameworkVersion} IDNO GiveUpDotNET${FrameworkVersion}
	${EndIf}

	DetailPrint "Pausing installation while downloaded .NET Framework installer runs."
	ExecWait '$TEMP\dotnetfx.exe /q /c:"install /q"'

	DetailPrint "Completed .NET Framework install/update. Removing .NET Framework installer."
	Delete "$TEMP\dotnetfx.exe"
	DetailPrint ".NET Framework installer removed."
	goto NewDotNet${FrameworkVersion}

GiveUpDotNET${FrameworkVersion}:
	Abort "Installation canceled by user."

NewDotNET${FrameworkVersion}:
	DetailPrint "Proceeding with remainder of installation."
	Pop $7
	Pop $6
	Pop $5
	Pop $4
	Pop $3
	Pop $2
	Pop $1
	Pop $0

!macroend
