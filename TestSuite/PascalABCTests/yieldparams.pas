function Get(params x: array of integer): sequence of integer;
begin
  yield x[0];
  yield x[1];
end;

begin
var a := Get(1,2).ToArray;
assert(a[0] = 1);
assert(a[1] = 2);
end.