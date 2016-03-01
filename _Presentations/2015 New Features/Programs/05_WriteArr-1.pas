var a: array [1..3] of integer := (2,3,5);

var p: record 
  name: string;
  age: integer;
end;

var s: set of integer := [1,3,7];

begin
  p.name := 'Иванов'; p.age := 20;
  writeln(a); 
  writeln(p);
  writeln(s);
end.