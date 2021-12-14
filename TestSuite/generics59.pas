procedure p1<T>(params a: array of T);
begin
  assert(typeof(T) = typeof(integer));
end;

function f1: array of integer := nil;

begin
  p1(f1);
  p1(f1());
end.