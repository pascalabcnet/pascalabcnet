// Строки и столбцы

begin
  var a := MatrRandomInteger(3,4);  
  a.Println;
  Println;
  
  // Цикл для вывода k-той строки
  var k := 1;
  for var j:=0 to a.ColCount-1 do // foreach var x in a.Row(k) do Print(x);
    Print(a[k,j]);
  Println;
    
  // Срез a[k,:] возвращает одномерный массив - k-тую строку
  a[k,:].Println; 
  
  // Цикл для вывода k-того столбца
  k := 2;
  for var i:=0 to a.RowCount-1 do
    Print(a[i,k]);
  Println;
  
  // Срез a[:,k] возвращает одномерный массив - k-тый столбец
  a[:,k].Println;
end.