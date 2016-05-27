function Gen(n: integer): sequence of integer;
begin
  for var i := 1 to n do
  begin
    yield i;
  end;
end;

begin
  foreach var x in Gen(10) do
    Print(x);
end.