begin
  var a := MatrRandomInteger(3,4,1,9);  
  a.Println;
  Println;
  var b := a.Cols;
  Sort(b, col -> col{@parameter col: array of integer;@}.Sum); // Сортировка с проекцией на ключ
  a := MatrByCol(b); // собрать матрицу из массива массивов
  a.Println;
end.