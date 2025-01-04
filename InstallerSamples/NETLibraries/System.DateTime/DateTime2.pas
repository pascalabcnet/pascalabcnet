Uses System;

begin
  var s := '03.27.2008 9:58:17';
  //Попытка преобразования строки в объект DateTime
  var d: DateTime;
  if not DateTime.TryParse(s, d) then
  begin
    Println('Строка не содержит значение даты и времени');
    Exit;
  end;

  //  Проверка високосности заданного года с помощью статического метода
  if DateTime.IsLeapYear(d.Year) then
     Println('Год високосный')
  else
     Println('Год невисокосный');
  
  // Преобразование даты и времени в строковое представление
  s := d.ToString;
  Println(s);
  
  // Преобразование даты в строковое представление
  s := d.ToShortDateString;
  Println(s);

  // Преобразование времени в строковое представление
  s := d.ToShortTimeString;
  Println(s);
end.