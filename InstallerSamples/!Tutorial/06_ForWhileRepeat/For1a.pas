// Цикл for. Шаг цикла 2
const n = 25;

begin
  Println('Чётные числа от 1 до', 2 * n);
  for var i := 1 to n do
    Print(2 * i);
  Println;
  Println;

  Println('Чётные числа от 1 до', 2 * n);
  var x := 2;
  for var i := 1 to n do
  begin
    Print(x);
    x += 2;
  end;
  Println;
  Println;

  Println('Нечётные числа от 1 до', 2 * n);
  for var i := 1 to n do
    Print(2 * i - 1);
  Println;
  Println;

  Println('Нечётные числа от 1 до', 2 * n);
  x := 1;
  for var i := 1 to n do
  begin
    Print(x);
    x += 2;
  end;
  Println;
  Println;
end.
