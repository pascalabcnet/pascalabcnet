var i: integer;
procedure p1<T>;
begin
  var o: T;
  if o = nil then
    i := 1;
  if o <> nil then
    i := 2;
end;

begin
  p1&<byte?>;
  assert(i = 1);
end.