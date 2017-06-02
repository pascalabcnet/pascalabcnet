// Цикл for

const n = 20;

var i: integer;

begin
  writeln('Числа от 1 до ',n,':');
  for i:=1 to n do
    write(i,' ');
  writeln;
  writeln;
  writeln('Числа от ',n,' до 1:');
  for i:=n downto 1 do
    write(i,' ');
  writeln;
  writeln;
  writeln('Маленькие английские буквы:');
  for c: char := 'a' to 'z' do
    write(c,' ');
  writeln;
end.