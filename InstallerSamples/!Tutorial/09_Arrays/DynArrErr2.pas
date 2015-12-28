// Ошибка выхода за границы диапазона для динамического массива
var a: array of integer;

begin
  SetLength(a,10);
  a[10] := 666;
end.