begin
  var arr := Arr(1,2,3);
  assert(5 in arr = false);
  assert(2 in arr = true);
  var l := new List<integer>();
  l += 4;
  l += 5;
  assert(4 in l = true);
end.