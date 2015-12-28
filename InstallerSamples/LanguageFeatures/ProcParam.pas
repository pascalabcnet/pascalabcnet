// Иллюстрация процедурных переменных как параметров подпрограмм
// Для процедурного типа в PascalABC.NET реализована структурная эквивалентность типов
// Процедурный тип реализован через делегаты .NET
procedure for_each(var a: array of real; p: procedure(var r: real));
var i: integer;
begin
  for i := 0 to a.Length-1 do
    p(a[i]);
end;

procedure mult2(var r: real);
begin
  r := 2*r
end;

procedure print(var r: real);
begin
  write(r,' ');
end;

var a: array of real := (1,2,3,6,7);

begin
  for_each(a,print); writeln;
  for_each(a,mult2);
  for_each(a,print);
end.