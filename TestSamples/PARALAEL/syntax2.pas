//синтаксис 2

procedure CalcX(z:real); async;
begin
  GetResult.x := {логика(z)};
end;
procedure CalcY(z:real); async;
begin
  GetResult.y := {логика(z)};
end;

function GetResult:real;
asyncparam x,y:real;
var z:real;
begin
  //зайдет сюда когда будут вызваны GetResult.X:= и GetResult.Y:=
  //Выполнится в основном потоке 
  z := x*y;
  result := z;
end;

begin
  CalcX(3.14);//запускается в потоке 1
  CalcY(2.71);//запускается в потоке 2
  writeln(GetResult);
end.