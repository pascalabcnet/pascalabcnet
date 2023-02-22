var i: integer;
procedure test(o: object);
begin
  i := integer(o);
end;
begin
  var x: Object := nil;
  var y:= x ?? 10;
  test(y);
  assert(i = 10);
end.