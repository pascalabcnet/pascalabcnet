begin
  var a := Arr(1, 2, 3);
  Assert(6 not in a);
  Assert(4 not in a);
  Assert(not (1 not in a));
end.