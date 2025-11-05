// Использование динамического массива. 
// Процедура SetLength выделения памяти под динамический массив
// Оператор foreach (массив доступен только на чтение)

begin
  var n := 20;
  var a: array of integer;
  SetLength(a,n);
  for var i:=0 to a.Length-1 do
    a[i] := Random(1,99);
  Println('Содержимое случайного динамического массива целых: ');  
  foreach var x in a do
    Print(x);
end.