Section
  ;MikhailoMMX - регистрируем PABCRtl.dll в Global Assembly Cache
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /u "PABCRtl"'
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /i "$INSTDIR\Lib\PABCRtl.dll"'
  
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /u "HelixToolkit.Wpf"'
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /i "$INSTDIR\Lib\HelixToolkit.Wpf.dll"'

  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /u "HelixToolkit"'
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /i "$INSTDIR\Lib\HelixToolkit.dll"'

  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /u "nunit.framework"'
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /i "$INSTDIR\Lib\nunit.framework.dll"'

  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /u "InteractiveDataDisplay.WPF"'
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /i "$INSTDIR\Lib\InteractiveDataDisplay.WPF.dll"'
  
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /u "MathNet.Numerics.dll"'
  ExecWait '"$INSTDIR\ExecHide.exe" "$INSTDIR\gacutil.exe" /i "$INSTDIR\Lib\MathNet.Numerics.dll"'


  ;\MikhailoMMX
  Delete "$INSTDIR\gacutil.exe"
  Delete "$INSTDIR\gacutil.exe.config"
  Delete "$INSTDIR\gacutlrc.dll"
  Delete "$INSTDIR\ExecHide.exe"
  WriteRegStr HKCU "${REGDIR}" "${INSTALLDIRREGKEY}" "$INSTDIR"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\PascalABCNET" \
                 "DisplayName" "PascalABC.NET"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\PascalABCNET" \
                 "UninstallString" "$\"$INSTDIR\Uninstall.exe$\""
SectionEnd

Section -closelogfile
 FileClose $UninstLog
 SetFileAttributes "$INSTDIR\${UninstLog}" READONLY|SYSTEM|HIDDEN
SectionEnd
 
Section Uninstall
  Delete "$DESKTOP\PascalABCNET.lnk"
  RMDir /r "$SMPROGRAMS\PascalABC.NET"; 
  ;RMDir /r $INSTDIR
  
  IfFileExists "$INSTDIR\${UninstLog}" +3
  ;MessageBox MB_OK|MB_ICONSTOP "$(UninstLogMissing)"
   ;Abort
   RMDir /r $INSTDIR
   Goto +2
  
  Call un.DeleteFiles
 
  ReadRegStr $R0 HKCR ".pas" ""
  StrCmp $R0 "PascalABCNET.PascalABCNETParser" 0 +2
    DeleteRegKey HKCR ".pas"
  DeleteRegKey HKCR "PascalABCNET.PascalABCNETParser"

  DeleteRegKey HKCU "Software\PascalABC.NET"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\PascalABCNET"
  System::Call 'Shell32::SHChangeNotify(i ${SHCNE_ASSOCCHANGED}, i ${SHCNF_IDLIST}, i 0, i 0)'

SectionEnd

;--------------------------------
;Installer Functions

;Function .onInit


  
;FunctionEnd


;--------------------------------
;Descriptions

  ;USE A LANGUAGE STRING IF YOU WANT YOUR DESCRIPTIONS TO BE LANGAUGE SPECIFIC
  LangString DESC_Common ${LANG_RUSSIAN} "Обычная"
  LangString DESC_Common ${LANG_ENGLISH} "Common"
  LangString DESC_Minimal ${LANG_RUSSIAN} "Минимальная"
  LangString DESC_Minimal ${LANG_ENGLISH} "Minimal"
  LangString DESC_PascalABCNET_Language ${LANG_RUSSIAN} "Язык PascalABC.NET"
  LangString DESC_PascalABCNET_Language ${LANG_ENGLISH} "PascalABC.NET language"
  LangString DESC_Compiler ${LANG_RUSSIAN} "Компилятор"
  LangString DESC_Compiler ${LANG_ENGLISH} "Compiler"
  LangString DESC_Envr ${LANG_RUSSIAN} "Оболочка"
  LangString DESC_Envr ${LANG_ENGLISH} "IDE"
  LangString DESC_IDEPlugins ${LANG_RUSSIAN} "Плагины к визуальной оболочке"
  LangString DESC_IDEPlugins ${LANG_ENGLISH} "IDE plugins"
  LangString DESC_Plugins ${LANG_RUSSIAN} "Плагины"
  LangString DESC_Plugins ${LANG_ENGLISH} "Plugins"
  LangString DESC_ConsoleCompiler ${LANG_RUSSIAN} "Консольная оболочка"
  LangString DESC_ConsoleCompiler ${LANG_ENGLISH} "Console compiler"
  LangString DESC_Localization ${LANG_RUSSIAN} "Локализация"
  LangString DESC_Localization ${LANG_ENGLISH} "Localization"
  LangString DESC_Samples ${LANG_RUSSIAN} "Примеры"
  LangString DESC_Samples ${LANG_ENGLISH} "Samples"
  LangString DESC_Core ${LANG_RUSSIAN} "Ядро"
  LangString DESC_Core ${LANG_ENGLISH} "Core"
  LangString DESC_PT4 ${LANG_RUSSIAN} "Задачник PT4"
  LangString DESC_PT4 ${LANG_ENGLISH} "PT4 Taskbook"
  LangString DESC_Files ${LANG_RUSSIAN} "Дополнительные файлы"
  LangString DESC_Files ${LANG_ENGLISH} "Additional files"
  LangString DESC_PROGRAMMING_LANGUAGES ${LANG_RUSSIAN} "Входные языки"
  LangString DESC_PROGRAMMING_LANGUAGES ${LANG_ENGLISH} "Programming languages"
  LangString DESC_INTERNAL_ERROR_REPORT ${LANG_RUSSIAN} "Контроль внутренних ошибок"
  LangString DESC_INTERNAL_ERROR_REPORT ${LANG_ENGLISH} "Internal error reporting"
  LangString DESC_Core_Desc ${LANG_RUSSIAN} "Файлы, необходимые для работы компилятора"
  LangString DESC_Core_Desc ${LANG_ENGLISH} "Compiler core files"
  LangString DESC_Localization_Desc ${LANG_RUSSIAN} "Варианты локализации"
  LangString DESC_Localization_Desc ${LANG_ENGLISH} "Available languages"
  LangString DESC_PABCParser_Desc ${LANG_RUSSIAN} "Язык PascalABC.NET"
  LangString DESC_PABCParser_Desc ${LANG_ENGLISH} "PascalABC.NET language"
  LangString DESC_Envr_Desc ${LANG_RUSSIAN} "Визуальная оболочка компилятора"
  LangString DESC_Envr_Desc ${LANG_ENGLISH} "PascalABC.NET IDE"
  LangString DESC_ConsoleCompiler_Desc ${LANG_RUSSIAN} "Консольная оболочка компилятора"
  LangString DESC_ConsoleCompiler_Desc ${LANG_ENGLISH} " "
  LangString DESC_INTERNAL_ERROR_REPORT_Desc ${LANG_RUSSIAN} "Позволяет отправить разработчикам отчет о ошибках в работе компилятора"
  LangString DESC_INTERNAL_ERROR_REPORT_Desc ${LANG_ENGLISH} " "
  LangString DESC_PT4_Desc ${LANG_RUSSIAN} "Электронный задачник Programming Taskbook Copyright (c)М.Э.Абрамян, 1998-2017"
  LangString DESC_PT4_Desc ${LANG_ENGLISH} "Programming Taskbook Copyright (c) M.E. Abramyan, 1998-2019"
  LangString DESC_Samples_Desc ${LANG_RUSSIAN} "Файлы примеров"
  LangString DESC_Samples_Desc ${LANG_ENGLISH} "Samples"
  LangString DESC_RusLoc_Desc ${LANG_RUSSIAN} "Поддержка русcкого языка"
  LangString DESC_RusLoc_Desc ${LANG_ENGLISH} " "
  LangString DESC_RusLoc_Desc ${LANG_UKR} "Поддержка украинского языка"
  LangString DESC_EngLoc_Desc ${LANG_RUSSIAN} "Поддержка английского языка"
  LangString DESC_EngLoc_Desc ${LANG_ENGLISH} " "
  LangString DESC_Framework_Desc ${LANG_RUSSIAN} "Платформа необходима для работы компилятора и программ, работающих на этой платформе"
  LangString DESC_Framework_Desc ${LANG_ENGLISH} " "
  LangString DESC_Uninstaller_Desc ${LANG_RUSSIAN} "Деинсталлятор"
  LangString DESC_Uninstaller_Desc ${LANG_ENGLISH} "Uninstaller"
  LangString DESC_FrameworkHelp_Desc ${LANG_RUSSIAN} "xml файлы для Framework Class Library, содержащие краткую справку"
  LangString DESC_FrameworkHelp_Desc ${LANG_ENGLISH} "xml documentation files for .NET Framework"
  LangString DESC_LangPack_Desc ${LANG_RUSSIAN} "Содержит текст, переведенный на русский язык, например сообщения об ошибках"
  LangString DESC_LangPack_Desc ${LANG_ENGLISH} "Russian language pack for .NET Framework"
  LangString DESC_PascalABCParserRegistation_Desc ${LANG_RUSSIAN} "Ассоциировать PascalABC.NET с расширением .pas"
  LangString DESC_PascalABCParserRegistation_Desc ${LANG_ENGLISH} "Associate PascalABC.NET with .pas files"
  LangString DESC_PascalABCParserRegistation ${LANG_RUSSIAN} "Ассоциировать с расширением .pas"
  LangString DESC_PascalABCParserRegistation ${LANG_ENGLISH} "Associate with .pas files"
  LangString DESC_InputLanguages ${LANG_RUSSIAN} "Входные языки"
  LangString DESC_InputLanguages ${LANG_ENGLISH} "Programming languages"
  LangString DESC_Framework_Required ${LANG_RUSSIAN} "Для работы компилятора необходима платформа Microsoft .NET версии 4.0. Скачайте и установите полную версию PascalABC.NET"
  LangString DESC_Framework_Required ${LANG_ENGLISH} "Microsoft .NET Framework 4.0 is required to be installed on your computer. Download and install the full version of PascalABC.NET"
  LangString DESC_PABCWORK ${LANG_RUSSIAN} "Рабочая папка PABCWork.NET"
  LangString DESC_PABCWORK ${LANG_ENGLISH} "PABCWork.NET working directory"
  LangString DESC_PABCWORK_Path ${LANG_RUSSIAN} "Путь к рабочей папке PABCWork.NET"
  LangString DESC_PABCWORK_Path ${LANG_ENGLISH} "Path to PABCWork.NET working directory"
  LangString DESC_PABCWORK_CHOOSE ${LANG_RUSSIAN} "Выберите путь к рабочей папке PABCWork.NET"
  LangString DESC_PABCWORK_CHOOSE ${LANG_ENGLISH} "Choose the path to PABCWork.NET working directory"
  LangString DESC_Compile ${LANG_RUSSIAN} "Компилировать"
  LangString DESC_Compile ${LANG_ENGLISH} "Compile"
  LangString DESC_Remove ${LANG_RUSSIAN} "Удаление"
  LangString DESC_Remove ${LANG_ENGLISH} "Remove"
  LangString DESC_PT4Setup ${LANG_RUSSIAN} "Настройка задачника PT4"
  LangString DESC_PT4Setup ${LANG_ENGLISH} "PT4 Setup"
  LangString DESC_IsRunning ${LANG_RUSSIAN} "Невозможно начать установку, так как запущена среда PascalABC.NET. Закройте PascalABC.NET и нажмите Повторить"
  LangString DESC_IsRunning ${LANG_ENGLISH} "PascalABC.NET is running. Please close it first"
  LicenseLangString MUILicense ${LANG_RUSSIAN} "License.rtf"
  LicenseLangString MUILicense ${LANG_ENGLISH} "License_en.rtf"

  ;Assign descriptions to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${Compiler} $(DESC_Compiler)
    !insertmacro MUI_DESCRIPTION_TEXT ${Envr} $(DESC_Envr)
    !insertmacro MUI_DESCRIPTION_TEXT ${Plugins} $(DESC_IDEPlugins)
    !insertmacro MUI_DESCRIPTION_TEXT ${Localization} $(DESC_Localization_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${Core} $(DESC_Core_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${PascalABCParser} $(DESC_PABCParser_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${VisualPABCNET} $(DESC_Envr_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${PABCNETC} $(DESC_ConsoleCompiler_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${InternalErrorReport} $(DESC_INTERNAL_ERROR_REPORT_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${SyantaxTreeVisualisator} "Визуализатор синтаксического дерева программы"
    !insertmacro MUI_DESCRIPTION_TEXT ${PT4} $(DESC_PT4_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${Samples} $(DESC_Samples_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${RusLoc} $(DESC_RusLoc_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${EngLoc} $(DESC_EngLoc_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${Framework} $(DESC_Framework_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${Uninstaller} $(DESC_Uninstaller_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${FrameworkHelp} $(DESC_FrameworkHelp_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${LangPack} $(DESC_LangPack_Desc)
    !insertmacro MUI_DESCRIPTION_TEXT ${PascalABCParserRegistation} $(DESC_PascalABCParserRegistation_Desc)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END


Icon Install.ico
UninstallIcon Uninstall.ico
 
;--------------------------------
;Uninstaller Section


;--------------------------------
;Uninstaller Functions

Function un.onInit

  !insertmacro MUI_UNGETLANGUAGE
  
FunctionEnd

!include Functions.nsh