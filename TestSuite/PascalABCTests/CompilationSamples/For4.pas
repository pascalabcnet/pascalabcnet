// ÷икл for. —умма всех двузначных чисел
var s: integer;

begin
  s := 0;
  for var i:=10 to 99 do
    s := s + i; // можно s += i
  writeln('—умма всех двузначных чисел = ',s);  
end.