// Расширенный foreach с распаковкой значений в переменные
begin
  var a := Arr((1,2),(3,4),(5,6));
  foreach var (x,y) in a do // Распаковка кортежей
    Print(x,y);
  Println;
  var b := Arr(|1,2,3|,|4,5|,|6,7,8,9|);
  foreach var (x,y) in b do // Распаковка последовательностей
    Print(x,y);
end.
