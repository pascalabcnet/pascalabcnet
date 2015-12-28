var a: array [1..3] of integer;

var p: record 
  name: string;
  age: integer;
end;

var s: set of byte;

var i: integer;

begin
  a[1] := 2; a[2] := 3; a[3] := 5;
  p.name := 'Иванов'; p.age := 20;
  s := [1,3,7];

  for i:=1 to 3 do
    write(a[i],' ');
  writeln;
  
  writeln(p.name,' ',p.age);
  
  for i:=0 to 255 do
    if i in s then
      write(i,' ');
end.