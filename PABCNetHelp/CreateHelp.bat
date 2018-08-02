cd PABCSystemDocGen
!AllSteps.exe
cd ..
"C:\Program Files (x86)\HTML Help Workshop\hhc.exe" PascalABCNET.hhp
copy PascalABCNET.chm ..\bin\PascalABCNET.chm 
copy PascalABCNET.chm ..\Release\PascalABCNET.chm