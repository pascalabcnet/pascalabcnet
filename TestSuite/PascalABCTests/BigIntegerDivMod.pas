begin
  var a: BigInteger := 11;
  Assert(a div 2 = 5);
  Assert(a mod 2 = 1);
  Assert(a mod BigInteger(2) = 1);
end.