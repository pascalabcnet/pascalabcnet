function f<T>(params a: array of T): array of T;
begin
  Result := a;
end;

function f2<T>(b: T; params a: array of T): array of T;
begin
  Result := a;
end;

begin
  var a := new integer[3](1,2,3);
  var b: array of integer;
  b := f(a);
  assert(b[0] = 1);
  b := f2(2, a);
  assert(b[1] = 2);
end.