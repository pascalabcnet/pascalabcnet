begin
  var a := Arr(1,3,2);
  a.Sort((row1, row2) -> 1);
  assert(a[0] = 1);
end.