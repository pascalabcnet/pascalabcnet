// Цикл for. Сумма всех двузначных чисел
var s: integer;

begin
  s := 0;
  for var i:=10 to 99 do
    s := s + i; // можно s += i
  writeln('Сумма всех двузначных чисел = ',s);  
end.