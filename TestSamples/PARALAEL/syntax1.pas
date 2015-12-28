
//синхронизация
// длянный код вводит 3 имени - GetResult SetX SetY
function GetResult:real set SetX(x: real), SetY(y: real);
begin
  //зайдет сюда когда будут вызваны SetX и SetY
  result := x*y;
end;


//асинхронные процедуры
procedure CalcX(z:real); async;
begin
  z := {логика(z)};
  SetX(z);
end;
procedure CalcY(z:real); async;
begin
  z := {логика(z)};
  SetY(z);
end;

begin
  CalcX(3.14);//запускается в потоке 1
  CalcY(2.71);//запускается в потоке 2
  writeln(GetResult);
end.