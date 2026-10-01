begin
  Assert((1..100).Count(x -> x.Divs(3)) = 33);
  Assert((1..100).Count = 100);
  Assert((5..3).Count = 0);
  Assert(Arr(1..10).Count(x -> x.IsEven) = 5);
end.
