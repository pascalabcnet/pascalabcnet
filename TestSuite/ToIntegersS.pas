begin
  var s := '12 34 56';
  Assert(s.ToIntegers(3).ArrEqual(Arr(12,34,56)));
  Assert(s.ToIntegers.ArrEqual(Arr(12,34,56)));
  s := ' 12 34 56   ';
  Assert(s.ToIntegers(3).ArrEqual(Arr(12,34,56)));
  Assert(s.ToIntegers.ArrEqual(Arr(12,34,56)));
end.