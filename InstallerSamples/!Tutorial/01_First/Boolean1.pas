// Логический тип. Логические выражения с and, or и not

begin
  var x: integer := ReadInteger('Введите x (от 1 до 9):');
  var b: boolean := x=5;
  Println('x=5?',b);
  b := (x>=3) and (x<=5);
  Println('x=3,4 или 5?',b);
  b := (x=3) or (x=4) or (x=5);
  Println('x=3,4 или 5?',b);
  b := not Odd(x);
  Println('x - четное?',b);
end.