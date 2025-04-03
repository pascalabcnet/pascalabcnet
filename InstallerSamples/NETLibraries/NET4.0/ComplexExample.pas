// Использование комплексных чисел
begin
  var a := new Complex(0,1);
  Println('Мнимая единица: ', a);
  
  var b := Sqrt(a);
  Println('Главный корень из мнимой единицы: ', b);
  
  var c := a*a;
  Println('Квадрат мнимой единицы: ', c);
  
  var d := Cplx(1,2);
  Println($'Модуль {ObjectToString(d)} равен {d.Magnitude}');
end.