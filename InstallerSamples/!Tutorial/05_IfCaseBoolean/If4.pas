// Вложенные условные операторы

begin
  var x := ReadInteger('Введите оценку (1..5):');
  if x = 1 then
    Println('Единица')
  else if x = 2 then
    Println('Двойка')
  else if x = 3 then
    Println('Тройка')
  else if x = 4 then
    Println('Четвёрка')
  else if x = 5 then
    Println('Пятёрка')
  else Println('Такой оценки нет');
end.

