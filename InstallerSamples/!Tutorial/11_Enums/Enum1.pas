// ѕеречислимый тип
type Months = (January,February,March,April,May,June,July,August,September,October,November,December);

var m: Months;
  
begin
  m := February;
  writeln(m);
  // »спользование констант перечислимого типа после имени типа удобно: после точки intellisense показывает список констант
  m := Months.April;
  writeln('—ледующий мес€ц: ',m);
  Inc(m);
  writeln('—ледующий мес€ц: ',m);
  m := Succ(m);  
  writeln('—ледующий мес€ц: ',m);
  m := Pred(m);  
  writeln('ѕредыдующий мес€ц: ',m);
  Dec(m);
  writeln('ѕредыдующий мес€ц: ',m);
  writeln('≈го пор€дковый номер (нумераци€ - с нул€): ',Ord(m));
  // ќшибки нет
  writeln('ћес€ц перед €нварем - выход за границы: ',pred(Months.January));
  // ќшибки нет
  writeln('ћес€ц после декабр€ - выход за границы: ',succ(Months.December));
end.  