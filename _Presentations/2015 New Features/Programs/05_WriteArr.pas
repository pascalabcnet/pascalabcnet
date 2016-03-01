var a: array [1..3] of integer;

var p: record 
  name: string;
  age: integer;
end;

begin
  a[1] := 2; a[2] := 3; a[3] := 5;
  p.name := 'Иванов'; p.age := 20;
  writeln(a); 
  writeln(p);
  
  var points := Seq(Rec(1,2),Rec(7,3),Rec(9,4));
  writeln(points);
end.