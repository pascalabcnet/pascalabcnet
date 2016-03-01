type Pupil = record
  name: string;
  age: integer;
end;

var p: Pupil;

begin
  p.name := 'Петрова';
  p.age := 18;
  writeln(p.name,' ',p.age);
end.