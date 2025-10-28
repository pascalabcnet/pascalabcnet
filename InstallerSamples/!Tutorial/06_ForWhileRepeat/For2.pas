// Цикл for. Последовательность случайных чисел
const n = 20;

begin
  Print('Случайные оценки:');
  for var i := 1 to n do
    Print(Random(2, 5));
  Println;
  
  Print('Случайные целые от 1 до 99:');
  for var i := n downto 1 do
    Print(Random(1, 99));
  Println;
end.
