// Процедуры и методы работы с динамическим массивом
uses System;

procedure Print(a: array of integer);
begin
  foreach v: integer in a do
    Write(v, ' ');
  Writeln;
end;    

var a: array of integer;

begin
  SetLength(a,10); // выделение памяти. А можно так: a := new integer[10];
  
  // Заполнение массива
  for var i:=0 to a.Length-1 do
    a[i] := PABCSystem.Random(100);
  
  // Вывод всех элементов массива
  Print(a);
  
  // Сортировка массива (знак & используется для того,
  // чтобы воспользоваться ключевым словом array при обозначении
  // класса Array)
  &Array.Sort(a);
  Print(a);

  // Обращение массива
  &Array.Reverse(a);
  Print(a);

  // Изменение размерамассиав с сохранением элементов. То же делает SetLength
  &Array.Resize(a,a.Length+2);
  Print(a);

  // Поиск вхождения элемента в массив
  var i := &Array.IndexOf(a, 50);
  Writeln(i);
end.