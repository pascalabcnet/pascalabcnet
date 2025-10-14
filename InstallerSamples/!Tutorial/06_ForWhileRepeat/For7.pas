// Цикл for. Числа Фибоначчи
const n = 25;

begin
  var a := 1;
  var b := 1;
  Println('Числа Фибоначчи:');
  Print(a,b);
  for var i := 3 to n do
  begin
    var c := a + b;
    Print(c);
    a := b;
    b := c;
  end;
  Println;
end.