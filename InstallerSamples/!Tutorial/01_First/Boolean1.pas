// Логический тип. Логические выражения с and, or и not
var 
  b: boolean;
  x: integer;
begin
  Write('Введите x (от 1 до 9): ');
  Readln(x);
  b := x=5;
  Writeln('x=5? ',b);
  b := (x>=3) and (x<=5);
  Writeln('x=3,4 или 5? ',b);
  b := (x=3) or (x=4) or (x=5);
  Writeln('x=3,4 или 5? ',b);
  b := not Odd(x);
  Writeln('x - четное? ',b);
end.