﻿begin
  var L := Lst(1..10);
  // Меняет местами элементы списка
  L.Swap(0,9).Swap(1,8).Println;

  var a := Arr(1..10);
  // Меняет местами элементы массива
  a.Swap(0,9).Swap(1,8).Print;
end.