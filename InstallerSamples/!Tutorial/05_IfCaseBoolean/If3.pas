// Условный оператор. Логические условия с or и and
var x: integer;

begin
  writeln('Введите x (от 1 до 99): ');
  readln(x);
  if (x>=1) and (x<=9) then 
    writeln('Однозначное число');
  // Odd - функция, возвращающая True только если x - нечетно
  if Odd(x) and (x>=10) and (x<=99) then 
    writeln('Нечетное двузначное число');
  if (x=3) or (x=9) or (x=27) or (x=81) then 
    writeln('Степень тройки');
end.