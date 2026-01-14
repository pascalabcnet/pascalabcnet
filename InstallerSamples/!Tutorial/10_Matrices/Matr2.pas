// Заполнение матрицы

begin
  // Создающая функция Matr
  var a := Matr([1,2,3,4],[5,6,7,8],[9,10,11,12]);  
  a.Println(3);  
  Println;
  
  // Создающая функция MatrGen
  var a1 := MatrGen(3,4,(i,j)->i+j+0.5);
  a1.Println(5,1);
end.