// Процедуры и методы работы с динамическим массивом

begin
  var a := ArrRandomInteger(10);
  
  // Вывод всех элементов массива
  a.Println;
  
  // Сортировка массива 
  a.Sort;
  a.Println;

  // Обращение массива
  a.Reverse;
  a.Println;

  // Изменение размера массива с сохранением элементов
  SetLength(a,a.Length+2);
  a.Println;

  // Поиск вхождения элемента в массив
  var i := a.IndexOf(50);
  Println(i);
end.