procedure p := exit;

var i: integer;

procedure p1;
begin
  if p = nil then
    i := 1
  else if nil = p then
    i := -1
  else
    i := 2;
end;
begin
  p1;
  assert(i = 2);
end.