// Демонстрация функции Milliseconds
uses Utils;

const n = 5000;

var a: array [1..n,1..n] of real;

begin
  for var i:=1 to n do
  for var j:=1 to n do
    a[i,j] := 1;
  writeln(Milliseconds/1000);
end.
