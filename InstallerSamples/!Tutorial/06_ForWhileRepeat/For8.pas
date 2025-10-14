// Цикл for. Максимум из введенных чисел

const n = 10;

begin
  Println('Введите', n, 'чисел');
  var max := real.MinValue; // самое маленькое вещественное число
  for var i := 1 to n do
  begin
    var x := ReadReal;
    if x > max then
      max := x;
  end;
  Println('Максимальное равно', max);
end.
