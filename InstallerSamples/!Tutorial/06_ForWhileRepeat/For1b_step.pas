// Цикл for с шагом 2
const n = 25;

begin
  Println('Чётные числа от 1 до', 2 * n);
  for var i := 2 to 2 * n step 2 do
    Print(i);
  Println;
  Println;

  Println('Нечётные числа от 1 до', 2 * n);
  for var i := 1 to 2 * n step 2 do
    Print(i);
end.
