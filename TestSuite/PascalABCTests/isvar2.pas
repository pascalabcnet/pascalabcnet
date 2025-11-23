function f1(o: object): sequence of object;
begin
  if o is object(var a) then
  begin
    yield a;
    Assert(a <> nil); // nil
  end;
end;

begin
  foreach var o in f1(new object) do
end.