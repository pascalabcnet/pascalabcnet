uses System;

begin
  // Получение текущей даты - вызов статического метода
  var d1 := DateTime.Now; 
  Println(d1);
  
  // Дата и время через один месяц
  var d2 := d1.AddMonths(1);
  Println(d2);

  // Дата и время на 12 часов раньше
  d2 := d1.AddHours(-12);
  Println(d2);

  // Формирование даты - вызов конструктора объекта (год, месяц,число)
  var d3 := new DateTime(2001, 1, 1);
  Println(d3);
  
  // Определение времени, прошедшего с начала тысячелетия (разность дат)
  var ts: TimeSpan := d1.Subtract(d3);

  // Промежуток времени в днях (результат - вещественное число)
  Println(ts.TotalDays);
  
  // Промежуток времени в днях, часах, минутах и секундах
  Println($'{ts.Days} {ts.Hours}:{ts.Minutes}:{ts.Seconds}');
end.