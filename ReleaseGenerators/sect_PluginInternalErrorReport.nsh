    Section $(DESC_INTERNAL_ERROR_REPORT) InternalErrorReport
      SectionIn 1 2
      SetOutPath "$INSTDIR"
      File "..\bin\ICSharpCode.SharpZipLib.dll"    
      File "..\bin\InternalErrorReport.dll"
	  ${AddFile} "ICSharpCode.SharpZipLib.dll"
	  ${AddFile} "InternalErrorReport.dll"
    SectionEnd    