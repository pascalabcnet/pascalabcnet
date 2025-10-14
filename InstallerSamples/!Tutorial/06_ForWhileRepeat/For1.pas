// Цикл for

const n = 20;

begin
  Println('Числа от 1 до', n, ':');
  for var i := 1 to n do
    Print(i);
  Println;
  Println;
  Println('Числа от', n, 'до 1:');
  for var i := n downto 1 do
    Print(i);
  Println;
  Println;
  Println('Маленькие английские буквы:');
  for var c := 'a' to 'z' do
    Print(c);
  Println;
end.
