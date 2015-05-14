var a,b:array[1..10,1..10] of integer;
begin
  a[1][2]:=10;
  b:=a;
  a[1][2]:=11;
  writeln(b[1][2]);
  readln;
end.