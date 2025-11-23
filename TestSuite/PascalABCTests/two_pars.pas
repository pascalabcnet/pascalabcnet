type
  TI = (integer, integer);

procedure p(f: (TI,TI) -> integer);
begin
  Assert(f((3, 4), (2, 5)) = 14);
end;

begin
  p((\(y1,y2),\(x,z)) -> x + y1 + y2 + z);
end.