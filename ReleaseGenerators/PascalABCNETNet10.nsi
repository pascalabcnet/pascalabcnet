; The same IDE, legacy compiler and installer sections, plus the shared modern kit.
Unicode true
; Render controls and fonts at the system DPI instead of Windows bitmap scaling.
ManifestDPIAware true
!define PABC_WITH_NET10
!define PABC_UNICODE_UNINSTALL_LOG
!ifndef PABC_INSTALLER_OUTFILE
!define PABC_INSTALLER_OUTFILE "..\Release\PascalABCNETSetupNet10.exe"
!endif
!ifndef PABC_NET10_PAYLOAD_DIR
!error "Prepare the payload with scripts/build-net10-ide-installer.ps1 first."
!endif
!include PascalABCNETStandart.nsi
