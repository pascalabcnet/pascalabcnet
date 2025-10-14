// Цикл repeat. Контроль ввода
begin
  var mark: integer;
  repeat
    Println('Введите оценку (2..5):');
    mark := ReadInteger;
    if (mark < 2) or (mark > 5) then
      Println('Оценка неверная. Повторите ввод');
  until (mark >= 2) and (mark <= 5);
  Println('Вы ввели оценку', mark);
end.
