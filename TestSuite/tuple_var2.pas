type c1 = class end;

begin
  
  // OK
  var (a,b) := |new object, new object|;
  //Ошибка: Ожидался кортеж или последовательность
  var (c,d) := |new c1, new c1|;
  assert(c <> nil);
  assert(d <> nil);
  assert(c <> d);
end.