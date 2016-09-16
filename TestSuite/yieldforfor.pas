function Sq(n: integer): sequence of integer;
begin
  for var i:=1 to n do
  for var j:=1 to n do
  begin
    yield i*j;
  end;
end;


begin
  Assert(Sq(3).SequenceEqual(Seq(1,2,3,2,4,6,3,6,9)),Sq(3).JoinIntoString);
end.