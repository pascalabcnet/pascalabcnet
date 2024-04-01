var i: integer := 0;

// Вместо IntRange - любой тип НЕ из .dll
// Обязательно params
procedure p1(params a: array of IntRange);
begin
  i := a[0].High;
end;

begin
  p1(Lst&<IntRange>(1..3).ToArray);
  assert(i = 3);
end.
