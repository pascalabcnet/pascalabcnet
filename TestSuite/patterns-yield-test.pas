function Y1(): sequence of integer;
begin
  if 1 is integer(i) then 
    yield i;
end;

begin
  Assert(System.Linq.Enumerable.SequenceEqual(Seq(1), Y1));
end.