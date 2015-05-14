// Демонстрация функции Milliseconds
uses Utils;
const n=5000;
var
  a: array [1..n,1..n] of real;
  i,j,k,m: integer;
begin
  for i:=1 to n do
  for j:=1 to n do
    a[i,j]:=0;
  writeln(Milliseconds/1000);
end.
