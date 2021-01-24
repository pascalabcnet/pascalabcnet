const
  r = 3 ** 2;

begin
  assert(round(2 ** 3) = 8);
  assert(round(3 * 2 ** 3) = 24);
  assert(round(2 ** 3 * 3) = 24);
  assert(round(2.0 ** 3) = 8);
  assert(round(2 ** 3.0) = 8);
  assert(round(-2 ** 2) = -4);
  assert(round(-(2 ** 2)) = -4);
  var b: byte := 2;
  assert(round(b ** 2) = 4);
  assert(abs(2 ** -2 - 0.25) < 0.0001);
  var i := round(2 ** 3);
  assert(i = 8);
  assert(round(r) = 9);
  var bi := new BigInteger(3);
  assert(bi ** 2 = 9);
  var bi2 := bi ** 2;
end.