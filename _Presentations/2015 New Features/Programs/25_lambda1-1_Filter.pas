const n = 5;

var a,b: array [1..n] of integer;
    i,bn: integer;

function p(x: integer): boolean;
begin
  p := x mod 2 <> 0;
end;    
    
begin
  a[1] := 1; a[2] := 5; a[3] := 2; 
  a[4] := 6; a[5] := 7; 

  for i:=1 to n do
    write(a[i],' ');
  writeln;

  bn := 0;
  for i:=1 to n do
    if p(a[i]) then
    begin
      bn := bn + 1;
      b[bn] := a[i];
    end;

  for i:=1 to bn do
    write(b[i],' ');
end.