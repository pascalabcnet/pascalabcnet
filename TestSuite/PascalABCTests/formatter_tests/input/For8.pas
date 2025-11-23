// Цикл for. Максимум из введенных чисел
const n = 10;

var max: real;

begin
  writelnFormat('Введите {0} чисел',n);
  max := integer.MinValue; // самое маленькое целое число
  for var i:=1 to n do
  begin
    var x: real;
    read(x);
    if x>max then
      max := x;
  end;  
  writeln('Максимальное равно ',max);
end.