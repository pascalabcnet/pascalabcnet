//Ошибка чтения из PCU
uses DS0010_unit;

var c: DS0010_unit.cc;

begin
  c := new DS0010_unit.cc;
  writeln(c);
  readln;
end.