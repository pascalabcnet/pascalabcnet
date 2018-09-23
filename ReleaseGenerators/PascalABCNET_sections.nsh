!include sect_Core.nsh

SectionGroup $(DESC_InputLanguages) Languages
  !include sect_PascalABCParser.nsh
SectionGroupEnd

SectionGroup $(DESC_Envr) Envr
  !include sect_PABCNETC.nsh
  !include sect_VisualPABCNET.nsh
  SectionGroup $(DESC_Plugins) Plugins 
    !include sect_PluginInternalErrorReport.nsh
	;!include sect_HelpBuilder.nsh
;    !include sect_PluginSTV.nsh  
  SectionGroupEnd
SectionGroupEnd

!include sect_PT4.nsh

!include sect_Files.nsh

!include sect_Samples.nsh

SectionGroup $(DESC_Localization) Localization
  !include sect_LocRus.nsh
  !include sect_LocEng.nsh
  !include sect_LocUkr.nsh
SectionGroupEnd


