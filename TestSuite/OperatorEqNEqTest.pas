begin
  var a := (1,2,3,4,5,6,7);
  var b := (1,2,3,4,5,6,7);
  Assert(b=a);
  Assert(not (b<>a));
  
  var s1 := SSet(1,2,3);
  var s2 := SSet(1,2,3);
  Assert(s1=s2);
  Assert(not (s1<>s2));

  var h1 := HSet(1,2,3);
  var h2 := HSet(1,2,3);
  Assert(h1=h2);
  Assert(not (h1<>h2));
  
  var b1: BigInteger := 2;
  var b2: Integer := 2;
  Assert(b1=b2);
  Assert(not (b1<>b2));
  Assert(b2=b1);
  Assert(not (b2<>b1));
end.