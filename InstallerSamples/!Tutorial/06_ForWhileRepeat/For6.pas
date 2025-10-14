// Цикл for. Таблица функции
var 
  a: real := 0;
  b: real := 4;
  n: integer := 16;

begin
  var h := (b-a)/n;
  var x := a;
  for var i:=0 to n do
  begin
    Println($'{x:f2}  {Sqrt(x):f4}');  
    x += h;
  end;
end.