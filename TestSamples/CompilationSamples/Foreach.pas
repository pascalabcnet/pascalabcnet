var a:array [1..10] of integer;

begin
  for var i:=1 to 10 do 
    a[i]:=Random(100);
  foreach elem:integer in a do 
    write(elem:3);
end.