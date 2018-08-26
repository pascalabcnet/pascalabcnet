begin
  var r1 := Rec&<byte, byte>(1, 1);
  var r2 := Rec&<byte, byte, byte>(1, 1, 1);
  var r3 := Rec&<byte, byte, byte, byte>(1, 1, 1, 1);
  var r4 := Rec&<byte, byte, byte, byte, byte>(1, 1, 1, 1, 1);
  var r5 := Rec&<byte, byte, byte, byte, byte, byte>(1, 1, 1, 1, 1, 1);
  var r6 := Rec&<byte, byte, byte, byte, byte, byte, byte>(1, 1, 1, 1, 1, 1, 1);
  assert(r1.Item1 = 1);
  assert(r2.Item1 = 1);
  assert(r3.Item1 = 1);
  assert(r4.Item1 = 1);
  assert(r5.Item1 = 1);
  assert(r6.Item1 = 1);
end.