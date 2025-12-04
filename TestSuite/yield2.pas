function g: sequence of integer;
begin
  yield 1;
  yield 2;
end;
function f: sequence of integer;
begin
  foreach var i in g do
  begin
    yield i;
  end;
end;
begin
  assert(f.ToArray[1] = 2);
end.