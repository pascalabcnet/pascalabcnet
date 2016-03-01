type Pupil = record
  name: string;
  age: integer;
  procedure Init(n: string; a: integer);
  begin
    name := n;
    age := a;
  end;
end;

var p: Pupil;

begin
  p.Init('Петрова',18);
  writeln(p);
  var r := Rec('Иванов',13);
  Println(r, r.Item1, r.Item2);
end.