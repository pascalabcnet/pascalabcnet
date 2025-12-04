var i: integer;

procedure p1(o: real; d: char->());
begin
  Inc(i);
end;

procedure p1(o: byte; d: ()->word) := exit;

begin
  p1(0, procedure(x) ->x.ToString());
  p1(0, x -> x.ToString());
  assert(i = 2);
end.