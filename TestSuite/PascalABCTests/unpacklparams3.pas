begin
  var pairs := Arr(1..3).Pairwise;
  var pairs2 := pairs.ZipTuple(pairs.Skip(1));
  
  Assert(pairs2.Select(\(\(a,b),\(c,d)) -> (a, b, c, d)).SequenceEqual(Seq((1,2,2,3))));
end.