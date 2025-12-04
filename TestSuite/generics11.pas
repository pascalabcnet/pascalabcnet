var i: integer;
procedure Right;
begin
  Inc(i);
end;
begin
  var t1 := Arr(Right,Right);
  t1[0];
  t1[1];
  assert(i=2);
end.