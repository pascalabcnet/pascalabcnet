begin
  var b: byte := 2;
  var i: integer := 3;
  var s: shortint := 4;
  var sm: smallint := 1;
  var w: word := 5;
  var lw: longword := 6;
  var i64: int64 := 7;
  var ui64: uint64 := 8;
  assert(min(b,i)=2);
  assert(min(s,i)=3);
  assert(min(s,sm)=1);
  assert(min(w,s)=4);
  assert(min(b,lw)=2);
  assert(min(i,lw)=3);
  assert(min(s,lw)=4);
  assert(min(b,s)=2);
  assert(min(b,i64)=2);
  assert(min(i64,ui64)=7);
  assert(min(b,255)=2);
  assert(min(s,255)=4);
  
  assert(min(sm,65535)=1);
  assert(min(i,255)=3);
  assert(min(w,65535)=5);
  assert(min(lw,255)=6);
  assert(min(i64,255)=7);
  assert(min(ui64,255)=8);
end.