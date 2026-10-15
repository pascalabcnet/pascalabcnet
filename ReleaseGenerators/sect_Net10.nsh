!include Net10RuntimeChecker.nsh

LangString DESC_Net10 ${LANG_RUSSIAN} ".NET 10: компилятор и IntelliSense"
LangString DESC_Net10 ${LANG_ENGLISH} ".NET 10: compiler and IntelliSense"
LangString DESC_Net10_Desc ${LANG_RUSSIAN} "Дополнительный режим .NET 10. Требуется Microsoft .NET 10 Desktop Runtime x64; SDK не требуется. Обычный режим .NET Framework сохраняется."
LangString DESC_Net10_Desc ${LANG_ENGLISH} "Additional .NET 10 mode. Requires Microsoft .NET 10 Desktop Runtime x64, not the SDK. The ordinary .NET Framework mode remains available."
LangString DESC_Net10_Missing ${LANG_RUSSIAN} "Microsoft .NET 10 Desktop Runtime x64 не найден.$\r$\nОткрыть страницу Microsoft для скачивания и установки?$\r$\n$\r$\nВыберите Desktop Runtime 10 для Windows x64, не SDK.$\r$\nМожно продолжить без него: обычный режим .NET Framework будет работать."
LangString DESC_Net10_Missing ${LANG_ENGLISH} "Microsoft .NET 10 Desktop Runtime x64 was not found.$\r$\nOpen Microsoft's download page to install it?$\r$\n$\r$\nChoose Desktop Runtime 10 for Windows x64, not the SDK.$\r$\nYou may continue without it: the ordinary .NET Framework mode will still work."
LangString DESC_Net10_Retry ${LANG_RUSSIAN} "После установки Desktop Runtime нажмите Повторить.$\r$\nНажмите Отмена, чтобы продолжить установку PascalABC.NET без него."
LangString DESC_Net10_Retry ${LANG_ENGLISH} "After installing Desktop Runtime, click Retry.$\r$\nClick Cancel to continue installing PascalABC.NET without it."
LangString DESC_Net10_Ready ${LANG_RUSSIAN} "Microsoft .NET 10 Desktop Runtime x64 найден."
LangString DESC_Net10_Ready ${LANG_ENGLISH} "Microsoft .NET 10 Desktop Runtime x64 was found."
LangString DESC_Net10_OrdinaryOnly ${LANG_RUSSIAN} "Desktop Runtime 10 x64 отсутствует. Режим .NET 10 требует его установки; обычный режим доступен."
LangString DESC_Net10_OrdinaryOnly ${LANG_ENGLISH} "Desktop Runtime 10 x64 is missing. Install it to use .NET 10; the ordinary mode remains available."

Section $(DESC_Net10) Net10
  SectionIn 1 2 RO
  Call Net10.DetectRuntime
  StrCmp $Net10RuntimeFound 1 ready
  IfSilent missing
  MessageBox MB_YESNO|MB_ICONINFORMATION $(DESC_Net10_Missing) /SD IDNO IDNO missing
  ; No runtime bytes are served by SFedU or embedded in this installer.
  ExecShell "open" "https://dotnet.microsoft.com/en-us/download/dotnet/10.0"
retry:
  MessageBox MB_RETRYCANCEL|MB_ICONINFORMATION $(DESC_Net10_Retry) /SD IDCANCEL IDCANCEL missing
  Call Net10.DetectRuntime
  StrCmp $Net10RuntimeFound 1 ready retry
missing:
  DetailPrint $(DESC_Net10_OrdinaryOnly)
  Goto files
ready:
  DetailPrint $(DESC_Net10_Ready)
files:
  SetOutPath "$INSTDIR"
  File "${PABC_NET10_PAYLOAD_DIR}\CompileNet10Plugin.dll"
  ${AddFile} "CompileNet10Plugin.dll"
  ; Preserve overrides on upgrade. The installer-specific default points inside INSTDIR.
  SetOverwrite off
  File "${PABC_NET10_PAYLOAD_DIR}\CompileNet10Plugin.ini"
  SetOverwrite on
  ; User configuration deliberately survives uninstall; do not add it to the file log.
  !include "${PABC_NET10_PAYLOAD_DIR}\net10-payload.nsh"
SectionEnd
