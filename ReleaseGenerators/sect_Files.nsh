Section $(DESC_Files) Files
  SectionIn 1

  SetOutPath "$INSTDIR\Files\Databases"
  File "Files\Databases\*.csv"  
  File "Files\Databases\*.txt"  
  File "Files\Words\*.txt"  

  SetOutPath "$INSTDIR\Files\Images"
  File "Files\Images\*.png"  
SectionEnd
