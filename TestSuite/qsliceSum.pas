begin
  var a := |1, 2, 4|;
  var s := a?[:2].Sum;
  Assert(s = 3);
  Assert(a?[:2].Sum = 3);
end.