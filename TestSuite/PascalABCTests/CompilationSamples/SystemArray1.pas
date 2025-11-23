Uses System, PABCSystem;

const Size = 10;

procedure PrintArray<T>(a: array of T);
begin
  foreach v: T in a do
    Write(v, ' ');
  Writeln;
end;    

var a: array of integer;

begin
  SetLength(a, Size);
  // Заполнение массива
  for var i:=0 to a.Length-1 do
    a[i] := Random(100);
  
  // Вывод всех элементов массива
  PrintArray(a);
  
  // Сортировка массива (знак & используется для того,
  // чтобы воспользоваться ключевым словом array при обозначении класса Array)
  &Array.Sort(a);

  PrintArray(a);

  // Обращение массива
  &Array.Reverse(a);

  PrintArray(a);

  // Поиск вхождения элемента в массив
  var i := &Array.IndexOf(a, 99);
  Writeln(i);
end.