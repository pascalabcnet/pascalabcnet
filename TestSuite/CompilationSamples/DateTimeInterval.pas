// Работа с классом System.DateTime
// Вычисление времени выполнения фрагмента кода

uses System, PABCSystem;

var dt: DateTime := DateTime.Now;

begin
  // Делаем случайную паузу
  var ms := Random(1000);
  Writeln('Sleep','(',ms,')');
  Sleep(ms);
  
  // Сравниваем с показаниями DateTime.Now
  Writeln((DateTime.Now-dt).TotalMilliseconds,'ms');
end.