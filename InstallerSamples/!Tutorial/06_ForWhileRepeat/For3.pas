// Цикл for. Последовательность случайных чисел
const n = 20;

begin
  Println('Случайные целые от 2 до 5:');
  for var i := 1 to n do
    Print(Random(2, 5));
  Println;
  Println;
  Println('Случайные целые от 1 до 99:');
  for var i := n downto 1 do
    Print(Random(1, 99));
end.
