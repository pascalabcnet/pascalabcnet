cd DocGenForStandardUnits
!GenDocPABCSystem.exe
!GenDocGraphWPF.exe
!GenDocWPFObjects.exe
!GenDocGraph3D.exe
!GenDocCRT.exe
cd ..
"C:\Program Files (x86)\HTML Help Workshop\hhc.exe" PascalABCNETNew.hhp
copy PascalABCNET.chm ..\bin\PascalABCNET.chm 
copy PascalABCNET.chm ..\Release\PascalABCNET.chm
