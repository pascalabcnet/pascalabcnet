function Sq(n: integer): sequence of integer;
begin
  var i:=1;
  while i<=n do
  begin
    yield i;
    i += 1;
  end;
  repeat
    yield i;
    i -= 1;
  until i<=0;
end;


begin
  Assert(Sq(3).SequenceEqual(Seq(1,2,3,4,3,2,1)),Sq(3).JoinIntoString);
end.