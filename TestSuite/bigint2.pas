begin
  var b := BigInteger(1);
  var b1 := b / 2;
  assert(TypeName(b1) = 'real');
  var b2 := b div 2;
  assert(TypeName(b2) = 'BigInteger');
end.