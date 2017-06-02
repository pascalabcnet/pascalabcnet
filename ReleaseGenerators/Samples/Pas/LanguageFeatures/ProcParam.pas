// Иллюстрация процедурных переменных как параметров подпрограмм
// Для процедурного типа в PascalABC.NET реализована структурная эквивалентность типов
// Процедурный тип реализован через делегаты .NET
procedure for_each(a: array of real; p: procedure(var r: real));
begin
  for var i := 0 to a.Length-1 do
    p(a[i]);
end;

procedure mult2(var r: real) := r := 2*r;

procedure print(var r: real) := write(r,' ');

var a: array of real := (1,2,3,6,7);

begin
  for_each(a,print); writeln;
  for_each(a,mult2);
  for_each(a,print);
end.