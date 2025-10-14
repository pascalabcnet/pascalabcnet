// Оператор выбора

begin
  var day := ReadInteger('Введите номер дня недели (1..7):');
  case day of
    1..5: Println('Будний');
    6,7: Println('Выходной');
  else Println('Неверный день недели');
  end;
end.
