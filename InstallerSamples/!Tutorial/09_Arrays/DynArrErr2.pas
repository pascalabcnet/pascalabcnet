// Ошибка выхода за границы диапазона для динамического массива

begin
  var a: array of integer;
  SetLength(a,10);
  a[10] := 666;
end.