// Работа с классом System.DateTime
// Вычисление времени выполнения фрагмента кода

begin
  var dt: DateTime := DateTime.Now;

  // Делаем случайную паузу
  var ms := Random(1000);
  Writeln('Sleep','(',ms,')');
  Sleep(ms);
  
  // Сравниваем с показаниями DateTime.Now
  Println((DateTime.Now-dt).TotalMilliseconds,'ms');
end.