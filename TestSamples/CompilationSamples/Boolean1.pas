// Логический тип. Логические выражения с and, or и not
var 
  b: boolean;
  x: integer;
begin
  write('Введите x (от 1 до 9): ');
  readln(x);
  b := x=5;
  writeln('x=5? ',b);
  b := (x>=3) and (x<=5);
  writeln('x=3,4 или 5? ',b);
  b := (x=3) or (x=4) or (x=5);
  writeln('x=3,4 или 5? ',b);
  b := not Odd(x);
  writeln('x - четное? ',b);
end.