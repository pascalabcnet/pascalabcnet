// Вложенные условные операторы
var x: integer;

begin
  writeln('Введите оценку (1..5): ');
  readln(x);
  if (x=1) then 
    writeln('Единица')
  else if (x=2) then 
    writeln('Двойка')
  else if (x=3) then 
    writeln('Тройка')
  else if (x=4) then 
    writeln('Четверка')
  else if (x=5) then 
    writeln('Пятерка')  
  else writeln('Такой оценки нет')  
end.