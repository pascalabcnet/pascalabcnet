var i: integer;

procedure p(x: Object);
begin
  i := 1;
end;
procedure p(x: procedure);
begin
  i := 2;
end;

procedure x := exit;
procedure xx(a: integer) := exit;
begin
  var proc: procedure := x;
  p(x);
  assert(i = 2);
  p(xx);
  assert(i = 1);
end.