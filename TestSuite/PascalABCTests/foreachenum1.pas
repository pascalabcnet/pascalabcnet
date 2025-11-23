type
  e1 = (ep1, ep2, ep3);

begin
  var i := 0;
  foreach var a in arr(ep1, ep3) do
  begin
    assert(a=(i=0?ep1:ep3));
    Inc(i);
  end;
end.