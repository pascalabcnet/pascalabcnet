begin
var res := Seq('234 456','222 333').Select(s->s.ToWords).Select(ss->(ss[0],ss[1].ToInteger));
var ind := 0;
foreach tup: System.Tuple<string, integer> in res do
begin
  if ind = 0 then
    assert(tup.Item2 = 456)
  else
    assert(tup.Item2 = 333);
  Inc(ind);
end;
end.