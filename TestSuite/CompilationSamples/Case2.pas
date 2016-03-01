// Оператор выбора
var day: integer;

begin
  writeln('Введите номер дня недели (1..7): ');
  readln(day);
  case day of
    1..5: writeln('Будний');
    6,7: writeln('Выходной');
  else writeln('Неверный день недели');
  end;
end.