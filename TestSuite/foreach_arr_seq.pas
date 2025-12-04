begin
  var a: array of sequence of byte;
  SetLength(a,1);
  a[0] := Seq(byte(1),byte(2));
  
  foreach var b in a do
    Assert(b.GetType.Name='Byte[]');
end.