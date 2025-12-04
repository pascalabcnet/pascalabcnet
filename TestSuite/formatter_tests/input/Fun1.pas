// Определение функции. Вывод таблицы ее значений

function MyFun(x: real): real;
begin
  Result := x*sin(x);
end;

const 
  a = 0.0;
  b = 2*Pi;
  n = 10;

begin
  var h := (b-a)/n;
  var x := a;
  writeln('Таблица значений функции MyFun:');
  for var i := 0 to n do
  begin
    writeln(x:5:2,MyFun(x):10:4);
    x += h;
  end;  
end.  