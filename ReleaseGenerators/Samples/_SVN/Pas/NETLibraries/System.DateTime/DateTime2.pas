Uses System;

var 
  d: DateTime;
  s: string;

begin
  s := '27.03.2008 9:58:17';
  //Попытка преобразования строки в объект DateTime
  if not DateTime.TryParse(s, d) then
  begin
    Writeln('Строка не содержит значение даты и времени');
    Exit;
  end;

  //  Проверка високосности заданного года с помощью статического метода
  if DateTime.IsLeapYear(d.Year) then
     Writeln('Год високосный')
  else
     Writeln('Год невисокосный');
  
  // Преобразование даты и времени в строковое представление
  s := d.ToString;
  Writeln(s);
  
  // Преобразование даты в строковое представление
  s := d.ToShortDateString;
  Writeln(s);

  // Преобразование времени в строковое представление
  s := d.ToShortTimeString;
  Writeln(s);

end.