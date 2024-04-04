var i: integer;

procedure p1(s: string) :=
case s of
  else Inc(i);
end;

begin
  p1('abc');
  p1('');
  p1(nil);
  assert(i = 3);
end.