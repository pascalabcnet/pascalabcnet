var i: integer;

procedure p0(o: object);
begin
  i := integer(o);
end;

procedure p1<T>(a: T);
begin
  p0(object(a));
end;

begin
  p1(1);
  assert(i = 1);
end.