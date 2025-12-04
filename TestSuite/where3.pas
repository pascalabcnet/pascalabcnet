function Test<T>(a: T): T; where T:constructor;
begin
  Result := new T;
end;

function Test2<T>(a: T): T; where T:record;
begin
  Result := a;
end;

begin
  var obj := Test(new StringBuilder);
  assert(obj.Length = 0);
  assert(Test(new StringBuilder).Length = 0);
  assert(Test2(2) = 2);
end.