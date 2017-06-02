// Перечислимый тип
type Months = (January,February,March,April,May,June,July,August,September,October,November,December);

var m: Months;
  
begin
  m := February;
  writeln(m);
  // Использование констант перечислимого типа после имени типа удобно: после точки intellisense показывает список констант
  m := Months.April;
  writeln('Следующий месяц: ',m);
  Inc(m);
  writeln('Следующий месяц: ',m);
  m := Succ(m);  
  writeln('Следующий месяц: ',m);
  m := Pred(m);  
  writeln('Предыдующий месяц: ',m);
  Dec(m);
  writeln('Предыдующий месяц: ',m);
  writeln('Его порядковый номер (нумерация - с нуля): ',Ord(m));
  // Ошибки нет
  writeln('Месяц перед январем - выход за границы: ',pred(Months.January));
  // Ошибки нет
  writeln('Месяц после декабря - выход за границы: ',succ(Months.December));
end.  