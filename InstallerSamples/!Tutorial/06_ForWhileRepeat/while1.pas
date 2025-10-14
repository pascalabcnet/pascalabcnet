// Цикл while. Вывод чисел с шагом 3
const n = 60;

begin
  Println($'Числа от 1 до {n}, кратные 3');
  var x := 3;
  while x <= n do
  begin
    Print(x);
    x += 3;
  end;
  Println;
end.
