const n = 6;

var a: array [1..n] of integer;
    i,j,t: integer;
begin
  a[1] := 2; a[2] := 5; a[3] := 3; 
  a[4] := 1; a[5] := 9; a[6] := 7;

  for i:=1 to n do
    write(a[i],' ');
  writeln;

  for i:=1 to n-1 do
    for j:=n downto 2 do
      if a[j-1]>a[j] then
      begin
        t := a[j-1];
        a[j-1] := a[j];
        a[j] := t;
      end;

  for i:=1 to n do
    write(a[i],' ');
  writeln;

  for i:=1 to n div 2 do
  begin
    t := a[i];
    a[i] := a[n-i+1];
    a[n-i+1] := t
  end;

  for i:=1 to n do
    write(a[i],' ');
  writeln;
end.