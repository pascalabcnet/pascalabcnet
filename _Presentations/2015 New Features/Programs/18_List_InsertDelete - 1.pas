const m = 20;

var a: array [1..m] of integer;
    n: integer;

begin
  n := 10;
  for var i:=1 to n do
    a[i] := Random(100);
  for var i:=1 to n do
    write(a[i],' ');
  writeln;  

  n += 1;
  for var i:=n downto 5 do
    a[i+1] := a[i];
  a[5] := 777777;
  
  for var i:=1 to n do
    write(a[i],' ');
  writeln;  

  for var i:=1 to n-1 do
    a[i] := a[i+1];
  n -= 1;  

  for var i:=1 to n do
    write(a[i],' ');
end.