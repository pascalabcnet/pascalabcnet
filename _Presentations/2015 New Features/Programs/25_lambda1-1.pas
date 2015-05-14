const n = 5;

var a,b: array [1..n] of integer;
    i: integer;

function f(x: integer): integer;
begin
  f := x*x;
end;    
    
begin
  a[1] := 1; a[2] := 5; a[3] := 2; 
  a[4] := 6; a[5] := 7; 

  for i:=1 to n do
    write(a[i],' ');
  writeln;

  for i:=1 to n do
    b[i] := f(a[i]);

  for i:=1 to n do
    write(b[i],' ');
end.