// Процедуры и методы работы с динамическим массивом

begin
  var a := ArrRandom(10);
  
  // Вывод всех элементов массива
  a.Println;
  
  // Сортировка массива 
  a.Sort;
  a.Println;

  // Обращение массива
  a.Reverse;
  a.Println;

  // Изменение размерамассиав с сохранением элементов. То же делает SetLength
  a.Resize(a.Length+2);
  a.Println;

  // Поиск вхождения элемента в массив
  var i := a.IndexOf(50);
  Writeln(i);
end.