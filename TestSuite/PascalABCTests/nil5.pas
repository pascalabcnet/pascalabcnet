var i: integer;
procedure p1<T>; where T:class;
begin
  var o: T;
  if o = nil then
    i := 1;
  if o <> nil then
    i := 2;
end;

begin
  p1&<object>;
  assert(i = 1);
end.