type
  t1 = class(List<integer>) end;
  
begin
  var l := new t1;
  l.Add(1);
  l.Add(2);
  var i := 1;
  foreach var x in l do
  begin
    assert(i = x);
    Inc(i);
  end;
end.