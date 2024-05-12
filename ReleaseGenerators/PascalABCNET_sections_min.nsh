!include sect_Core.nsh

SectionGroup $(DESC_InputLanguages) Languages
  !include sect_PascalABCLanguage.nsh
SectionGroupEnd

SectionGroup $(DESC_Envr) Envr
  !include sect_PABCNETC.nsh
  !include sect_VisualPABCNET.nsh
  SectionGroup $(DESC_Plugins) Plugins 
    !include sect_PluginInternalErrorReport.nsh
  SectionGroupEnd
SectionGroupEnd

!include sect_Samples.nsh

SectionGroup $(DESC_Localization) Localization
  !include sect_LocRus.nsh
;  !include sect_LocEng.nsh
SectionGroupEnd