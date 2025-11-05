// Заполнение и вывод случайной матрицы

begin
  var a := new integer[3,4];
  
  for var i:=0 to a.RowCount-1 do
  for var j:=0 to a.ColCount-1 do
    a[i,j] := i + j;  
  
  a.Println;
  Println(a);
  // 3 позиции под каждое значение
  a.Println(3);
  
  var mr := MatrRandomReal(3,4);
  mr.Println;
  Println(mr);
  // 6 позиций под каждое значение и 1 под дробную часть
  mr.Println(6,1);
end.