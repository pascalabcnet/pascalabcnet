var i: integer;

procedure p1(params ps: array of ()->());
begin
  ps[0];
  ps[1];
end;

begin
  p1(
    ()->begin Inc(i); end,
    ()->begin Inc(i); end
  );
  assert(i = 2);
end.