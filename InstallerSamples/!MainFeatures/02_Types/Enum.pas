// Перечислимый тип
type Months = (January,February,March,April,May,June,
  July,August,September,October,November,December);

begin
  var m: Months := February;
  writeln(m);
  // Использование констант перечислимого типа после имени типа удобно: после точки intellisense показывает список констант
  m := Months.April;
  Println('Следующий месяц:',m);
  Inc(m);
  Println('Следующий месяц:',m);
  m := Succ(m);  
  Println('Следующий месяц:',m);
  m := Pred(m);  
  Println('Предыдующий месяц:',m);
  Dec(m);
  Println('Предыдующий месяц:',m);
  Println('Его порядковый номер (нумерация - с нуля):',Ord(m));
  // Ошибки нет
  Println('Месяц перед январем - выход за границы:',Pred(Months.January));
  // Ошибки нет
  Println('Месяц после декабря - выход за границы:',Succ(Months.December));
end.  