// Цикл repeat. Контроль ввода
var mark: integer;

begin
  repeat
    writeln('Введите оценку (2..5): ');
    readln(mark);
    if (mark<2) or (mark>5) then
      writeln('Оценка неверная. Повторите ввод');
  until (mark>=2) and (mark<=5);
  writeln('Вы ввели оценку ',mark);
end.