// Оператор выбора
var x: integer;

begin
  writeln('Введите оценку (1..5): ');
  readln(x);
  case x of
    1: writeln('Единица');
    2: writeln('Двойка');
    3: writeln('Тройка');
    4: writeln('Четверка');
    5: writeln('Пятерка');  
  else writeln('Такой оценки нет');
  end;
end.