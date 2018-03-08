begin
  var a: BigInteger := 1;
  var s := byte(a).ToString;
  var s2 := object(a).ToString;
  var s3 := BigInteger(1).ToString;
  assert(s='1');
  assert(s2='1');
  assert(s3='1');
end.