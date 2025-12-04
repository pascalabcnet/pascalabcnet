begin
  var x:= Arr(1..5);
  x := x[:2] + x;
  Assert(x.SequenceEqual(Arr(1,2,1,2,3,4,5)));
  x := x?[:2] + x;
  Assert(x.SequenceEqual(Arr(1,2,1,2,1,2,3,4,5)));
end.