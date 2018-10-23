// Цикл for. Вычисление a в степени n

begin
  var a := ReadlnReal('Введите a:');
  var n := ReadlnInteger('Введите n:');

  var p := 1.0;
  for var i:=1 to n do
    p *= a; 
  Println($'{a} в степени {n} = {p}');
  
  Println($'Стандартная операция {a} ** {n} = {a ** n}');  
end.