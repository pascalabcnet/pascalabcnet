var i: integer;
procedure p1<T>;
begin
  var o: T;
  if nil = o then
    i := 1;
  if nil <> o then
    i := 2;
end;

begin
  p1&<integer?>;
  assert(i = 1);
end.