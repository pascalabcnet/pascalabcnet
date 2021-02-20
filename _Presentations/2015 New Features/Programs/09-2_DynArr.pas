var a: array [1..10] of integer;
    b: array [1..6] of char;
    i: integer;
begin
  a[1] := 2; a[2] := 5; 
  a[3] := 7; a[4] := 10;
  for i:=1 to 4 do
    write(a[i],' ');
  writeln;
  b[1] := 'a'; b[2] := 'e'; b[3] := 'i'; 
  b[4] := 'o'; b[5] := 'u'; b[6] := 'y'; 
  for i:=1 to 6 do
    write(b[i],' ');
  writeln;
  Randomize;
  for i:=1 to 10 do
    a[i] := Random(100);
  for i:=1 to 10 do
    write(a[i],' ');
  writeln;
  for i:=1 to 5 do
    a[i] := 1;
  for i:=1 to 5 do
    write(a[i],' ');
  writeln;
  for i:=1 to 5 do
    read(a[i]);
  for i:=1 to 5 do
    write(a[i],' ');
end.