begin
  var b: byte := 254;
  Assert(Sqr(b)=b*b);
  Assert(Abs(b)=b);
  var w: word := 65535;
  Assert(Sqr(w)=w*w);
  Assert(Abs(w)=w);
end.