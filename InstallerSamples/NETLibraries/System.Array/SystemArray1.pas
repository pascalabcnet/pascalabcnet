const Size = 10;

begin
  // Заполнение массива
  var a := ArrRandomInteger(Size);

  // Вывод всех элементов массива
  a.Println;
  
  // Сортировка массива (знак & используется для того,
  // чтобы воспользоваться ключевым словом array при обозначении класса Array)
  Sort(a);

  a.Println;

  // Обращение массива
  Reverse(a);

  a.Println;

  // Поиск вхождения элемента в массив
  var i := a.IndexOf(99);
  Println(i);
end.