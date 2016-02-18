begin
  {var b: byte := 2;
  var i: integer := 3;
  var s: shortint := 4;
  var w: word := 5;
  var lw: longword := 6;
  var i64: int64 := 7;
  var ui64: uint64 := 8;
  assert(min(b,i)=2);
  assert(min(s,i)=3);
  assert(min(w,s)=4);
  assert(min(b,lw)=2);
  assert(min(i,lw)=3);
  assert(min(b,i64)=2);
  assert(min(i64,ui64)=7);}
end.