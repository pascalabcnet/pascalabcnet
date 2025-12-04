// Цикл for. Числа Фибоначчи
const n = 25;

begin
  var a := 1;
  var b := 1;
  writeln('Числа Фибоначчи:');
  write(a,' ',b,' ');
  
  for var i:=3 to n do
  begin
    var c := a + b;
    write(c,' ');
    a := b;
    b := c;
  end;
end.