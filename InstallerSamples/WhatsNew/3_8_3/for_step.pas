type Days = (Mon,Tue,Wed,Thi,Fri,sat);

begin
  for var i:=1 to 5 step 2 do
    Print(i);
  Println;
  
  for var i:='5' to '2' step -2 do
    Print(i);
  Println;
  
  for var i:=Mon to Sat step 2 do
    Print(i);
  Println;
end.