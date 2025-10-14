// Оператор выбора

begin
  var x := ReadInteger('Введите оценку (1..5):');
  case x of
    1: Println('Единица');
    2: Println('Двойка');
    3: Println('Тройка');
    4: Println('Четвёрка');
    5: Println('Пятёрка');
  else
    Println('Такой оценки нет');
  end;
end.