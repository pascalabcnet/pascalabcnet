begin
  var b: byte := 254;
  Assert(Sqr(b)=b*b);
  Assert(Abs(b)=b);
  var w: word := 65535;
  Assert(Sqr(w)=w*w);
  Assert(Abs(w)=w);
  var sm: smallint := smallint.MaxValue;
  Assert(Sqr(sm) = sm*sm);
  var sh: shortint := shortint.MaxValue;
  Assert(Sqr(sh) = sh*sh);
  var lw: longword := longword.MaxValue;
  Assert(Sqr(lw) = uint64(lw)*uint64(lw));
  var i: integer := integer.MaxValue;
  Assert(Sqr(i) = int64(i)*int64(i));
  var i64: int64 := integer.MaxValue;
  Assert(Sqr(i64) = i64*i64);
  var ui64: uint64 := integer.MaxValue;
  Assert(Sqr(ui64) = ui64*ui64);
end.