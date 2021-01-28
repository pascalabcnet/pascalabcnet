begin
  var b := Arr(0, 1);
  Assert(b.SelectMany(x -> b.SelectMany(y -> b.Select(z -> y).Select(w -> 1))).SequenceEqual(Arr(1)*8));
end.
