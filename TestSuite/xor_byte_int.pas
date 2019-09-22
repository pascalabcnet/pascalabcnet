begin
  var b: byte := 255;
  var i: int64 := 256;
  Assert(b xor i = 511);
  Assert(i xor b = 511);
end.