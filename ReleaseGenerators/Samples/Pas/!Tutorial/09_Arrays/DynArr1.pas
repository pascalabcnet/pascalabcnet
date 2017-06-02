// Использование динамического массива. 
// Процедура SetLength выделения памяти под динамический массив
// Оператор foreach (массив доступен только на чтение)
var a: array of integer;

begin
  var n := 20;
  SetLength(a,n);
  for var i:=0 to a.Length-1 do
    a[i] := Random(1,99);
  writeln('Содержимое случайного динамического массива целых: ');  
  foreach var x in a do
    Print(x);
end.