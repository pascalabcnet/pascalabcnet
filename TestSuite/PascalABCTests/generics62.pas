var i: integer;
procedure p1<T1>(d: T1->()) := exit;
procedure p1<T1,T2>(d: T1->T2);
begin
  i := 1;
end;

function f(b: byte) := 0;
begin
  p1(f);
  assert(i = 1);
end.
