begin
  var a := Arr(1,2,3);
  var (k,m) := a[1:3];
  Assert((k=2) and (m=3));
end.