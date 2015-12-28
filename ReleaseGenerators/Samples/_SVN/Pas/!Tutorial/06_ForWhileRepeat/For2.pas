// Цикл for. Последовательность случайных чисел

const n = 20;

var i: integer;

begin
  write('Случайные оценки: ');
  for i:=1 to n do
    write(Random(2,5),' ');
  writeln;
  write('Случайные целые от 1 до 99: ');
  for i:=n downto 1 do
    write(Random(1,99),' ');
end.