var i: integer;
procedure p0<T>(p: T->());
begin
  p(default(T));
end;

procedure p1(v: byte);
begin
  i := 1;
end;
procedure p1(a,b,c: word) := exit;

begin
  p0(p1);
  assert(i = 1);
end.