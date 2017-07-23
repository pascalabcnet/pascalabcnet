var a, b: integer;

procedure p();
begin
  a := 1;
end;

procedure r();
begin
  b := 2;
end;

begin
  var q: procedure := p+r;
  q;
  assert(a = 1);
  assert(b = 2);
end.