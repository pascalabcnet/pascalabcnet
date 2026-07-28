// Использование динамического массива. 
// Выделение памяти под массив с помощью new
// Оператор foreach (массив доступен только на чтение)

begin
  var n := 20;
  var a: array of integer;

  a := new integer[n];

  for var i := 0 to a.Length - 1 do
    a[i] := Random(1, 99);

  Println('Содержимое случайного массива целых:');  
  foreach var x in a do
    Print(x);
end.
