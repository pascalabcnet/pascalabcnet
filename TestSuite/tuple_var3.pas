type c1 = class end;

begin
  var i := 0;
  // OK
  foreach var (a,b) in ||new object, new object|| do
  //Ошибка: Ожидалась последовательность кортежей или последовательностей
  foreach var (c,d) in ||new c1, new c1|| do
  begin
    assert(c <> nil);
    assert(d <> nil);
    assert(c <> d);
    i := 1;
  end;
  assert(i = 1);
end.