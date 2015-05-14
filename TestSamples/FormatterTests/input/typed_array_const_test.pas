const
  a: array [1..3] of integer = (1, 2, 4);
  b: array [1..2, 1..2, 1..2] of integer = (((1,2), (3,4)), ((5,6), (7,8)));

var i,j,k : integer;
    c: array [1..3] of integer := (4, 5, 6);

procedure p;
const a: array [1..3] of integer = (10, 11, 12);
var i:=0;
    b: array [1..3] of integer := (7, 8, 9);
begin
  Writeln('Local const');
  for i:=1 to 3 do
    writeln(a[i]);
  Writeln('Local var');
  for i:=1 to 3 do
    writeln(b[i]);
end;

begin
  p;
  Writeln('Global const');
  for i:=1 to 3 do
    writeln(a[i]);
  for i:=1 to 2 do begin
    for j:=1 to 2 do
      for k:=1 to 2 do
        write(b[i,j,k], ' ');
    writeln;
  end;{}
  Writeln('Global var');
  for i:=1 to 3 do
    writeln(c[i]);
  readln;
end.