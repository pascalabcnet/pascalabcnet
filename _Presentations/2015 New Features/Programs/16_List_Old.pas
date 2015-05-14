const n = 10;
var a,b: array [1..n] of integer;
    nb: integer;
begin
  for var i:=1 to n do
    a[i] := Random(10);
  for var i:=1 to n do
    write(a[i],' ');
  writeln;  
  
  nb := 0;
  for var i:=1 to n do
    if a[i] mod 2 = 0 then
    begin
      nb := nb + 1;
      b[nb] := a[i]
    end;
      
  for var i:=1 to nb do
    write(b[i],' ');
end.