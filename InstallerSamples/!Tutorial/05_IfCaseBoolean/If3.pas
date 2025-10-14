// Условный оператор. Логические условия с or и and

begin
  var x := ReadInteger('Введите x (от 1 до 99):');
  if (x >= 1) and (x <= 9) then
    Println('Однозначное число');
  // Odd — функция, возвращающая True, если x нечётное
  if Odd(x) and (x >= 10) and (x <= 99) then
    Println('Нечётное двузначное число');
  if (x = 3) or (x = 9) or (x = 27) or (x = 81) then
    Println('Степень тройки');
end.
