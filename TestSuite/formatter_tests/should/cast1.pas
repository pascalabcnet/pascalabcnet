type
  TColor = (red, green, blue);
  TDiap = red..blue;
  TIntDiap = 1..4;

var
  t: TColor;
  t2: TDiap;
  b: byte;
  sm: smallint;
  sh: shortint;
  w: word;
  lw: longword;
  i: integer;
  li: int64;
  ui: uint64;
  d: TIntDiap;

begin
  t := t2; t2 := t;
  b := 1; sm := 1; sh := 1; w := 1; lw := 1; i := 1; li := 1; ui := 1; d := 1;
  t := TColor(b); assert(t = green);
  t := TColor(sm); assert(t = green);
  t := TColor(sh); assert(t = green);
  t := TColor(w); assert(t = green);
  t := TColor(lw); assert(t = green);
  t := TColor(i); assert(t = green);
  t := TColor(li); assert(t = green);
  t := TColor(ui); assert(t = green);
  //t := TColor(d); assert(t=green);
  t := blue;
  assert(byte(t) = 2);
  assert(smallint(t) = 2);
  assert(shortint(t) = 2);
  assert(word(t) = 2);
  assert(longword(t) = 2);
  assert(integer(t) = 2);
  assert(int64(t) = 2);
  assert(uint64(t) = 2);
  //assert(TIntDiap(t)=2);
  t2 := TDiap(b); assert(t2 = green);
  t2 := TDiap(sm); assert(t2 = green);
  t2 := TDiap(sh); assert(t2 = green);
  t2 := TDiap(w); assert(t2 = green);
  t2 := TDiap(lw); assert(t2 = green);
  t2 := TDiap(i); assert(t2 = green);
  t2 := TDiap(li); assert(t2 = green);
  t2 := TDiap(ui); assert(t2 = green);
  t2 := blue;
  assert(byte(t2) = 2);
  assert(smallint(t2) = 2);
  assert(shortint(t2) = 2);
  assert(word(t2) = 2);
  assert(longword(t2) = 2);
  assert(integer(t2) = 2);
  assert(int64(t2) = 2);
  assert(uint64(t2) = 2);
end.