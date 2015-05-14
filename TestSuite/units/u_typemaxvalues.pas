unit u_typemaxvalues;

var b : byte;
    sh : shortint;
    sm : smallint;
    i : integer;
    w : word;
    lw : longword;
    l : int64;
    ui : uint64;
    
begin
  b := byte.MaxValue; assert(b=byte.MaxValue);
  sh := shortint.MaxValue; assert(sh=shortint.MaxValue);
  sm := smallint.MaxValue; assert(sm = smallint.MaxValue);
  w := word.MaxValue; assert(w = word.MaxValue);
  i := integer.MaxValue; assert(i = integer.MaxValue);
  lw := longword.MaxValue; assert(lw = longword.MaxValue);
  l := int64.MaxValue; assert(l = int64.MaxValue);
  ui := uint64.MaxValue; assert(ui = uint64.MaxValue);
  sh := b; assert(sh = -1);
  sm := w; assert(sm = -1);
  i := lw; assert(i = -1);
  l := ui; assert(l = ui);
end.