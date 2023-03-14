
begin
  var i := new BigInteger(1);
  var s := (if false then i else new BigInteger(2)).ToString;
  assert(s = '2');
  s := (if true then i else new BigInteger(2)).ToString;
  assert(s = '1');
end.