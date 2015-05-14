procedure Test;
var b : byte;
    sh : shortint;
    sm : smallint;
    w : word;
    lw : longword;
    i : integer;
    li : int64;
    ui : uint64;
    
begin
  b := 2; sh := 2; sm := 2; w := 2; lw := 2; i := 2; li := 2; ui := 2;
  assert(b = 2);
  assert(sh = 2);
  assert(sm = 2);
  assert(w = 2);
  assert(lw = 2);
  assert(i = 2);
  assert(li = 2);
  assert(ui = 2);
  
  assert(b = b);
  assert(sh = b);
  assert(sm = b);
  assert(w = b );
  assert(lw = b );
  assert(i = b );
  assert(li = b );
  assert(ui = b );
  
  assert(b = sh );
  assert(sh = sh );
  assert(sm = sh );
  assert(w = sh );
  assert(lw = sh );
  assert(i = sh );
  assert(li = sh );
  assert(ui = sh );
  
  assert(b = sm );
  assert(sh = sm );
  assert(sm = sm );
  assert(w = sm );
  assert(lw = sm );
  assert(i = sm );
  assert(li = sm );
  assert(ui = sm );
  
  assert(b = w );
  assert(sh = w );
  assert(sm = w );
  assert(w = w );
  assert(lw = w );
  assert(i = w );
  assert(li = w );
  assert(ui = w );
  
  assert(b = lw );
  assert(sh = lw );
  assert(sm = lw );
  assert(w = lw );
  assert(lw = lw );
  assert(i = lw );
  assert(li = lw );
  assert(ui = lw );
  
  assert(b = i );
  assert(sh = i );
  assert(sm = i );
  assert(w = i );
  assert(lw = i );
  assert(i = i );
  assert(li = i );
  assert(ui = i );
  
  assert(b = li );
  assert(sh = li );
  assert(sm = li );
  assert(w = li );
  assert(lw = li );
  assert(i = li );
  assert(li = li );
  assert(ui = li );
  
  assert(b = ui );
  assert(sh = ui );
  assert(sm = ui );
  assert(w = ui );
  assert(lw = ui );
  assert(i = ui );
  assert(li = ui );
  assert(ui = ui );
  
  b := 2; sh := 3; sm := 4; w := 5; lw := 6; i := 7; li := 8; ui := 9;
  assert(b > 1);
  assert(sh > 1);
  assert(sm > 1);
  assert(w > 1);
  assert(lw > 1);
  assert(i > 1);
  assert(li > 1);
  assert(ui > 1);

  assert(b >= 1);
  assert(sh >= 1);
  assert(sm >= 1);
  assert(w >= 1);
  assert(lw >= 1);
  assert(i >= 1);
  assert(li >= 1);
  assert(ui >= 1);
  
  assert(sh > b);
  assert(sm > b);
  assert(w > b );
  assert(lw > b );
  assert(i > b );
  assert(li > b );
  assert(ui > b );

  assert(sh >= b);
  assert(sm >= b);
  assert(w >= b );
  assert(lw >= b );
  assert(i >= b );
  assert(li >= b );
  assert(ui >= b );
  
  assert(b < sh );
  assert(sm > sh );
  assert(w > sh );
  assert(lw > sh );
  assert(i > sh );
  assert(li > sh );
  assert(ui > sh );
  
  assert(b <= sh );
  assert(sm >= sh );
  assert(w >= sh );
  assert(lw >= sh );
  assert(i >= sh );
  assert(li >= sh );
  assert(ui >= sh );
  
  assert(b < sm );
  assert(sh < sm );
  assert(w > sm );
  assert(lw > sm );
  assert(i > sm );
  assert(li > sm );
  assert(ui > sm );
  
  assert(b <= sm );
  assert(sh <= sm );
  assert(w >= sm );
  assert(lw >= sm );
  assert(i >= sm );
  assert(li >= sm );
  assert(ui >= sm );
  
  assert(b < w );
  assert(sh < w );
  assert(sm < w );
  assert(lw > w );
  assert(i > w );
  assert(li > w );
  assert(ui > w );
  
  assert(b <= w );
  assert(sh <= w );
  assert(sm <= w );
  assert(lw >= w );
  assert(i >= w );
  assert(li >= w );
  assert(ui >= w );
  
  assert(b < lw );
  assert(sh < lw );
  assert(sm < lw );
  assert(w < lw );
  assert(i > lw );
  assert(li > lw );
  assert(ui > lw );
  
  assert(b <= lw );
  assert(sh <= lw );
  assert(sm <= lw );
  assert(w <= lw );
  assert(i >= lw );
  assert(li >= lw );
  assert(ui >= lw );
  
  assert(b < i );
  assert(sh < i );
  assert(sm < i );
  assert(w < i );
  assert(lw < i );
  assert(li > i );
  assert(ui > i );
  
  assert(b <= i );
  assert(sh <= i );
  assert(sm <= i );
  assert(w <= i );
  assert(lw <= i );
  assert(li >= i );
  assert(ui >= i );
  
  assert(b < li );
  assert(sh < li );
  assert(sm < li );
  assert(w < li );
  assert(lw < li );
  assert(i < li );
  assert(ui > li );

  assert(b <= li );
  assert(sh <= li );
  assert(sm <= li );
  assert(w <= li );
  assert(lw <= li );
  assert(i <= li );
  assert(ui >= li );
  
  assert(b < ui );
  assert(sh < ui );
  assert(sm < ui );
  assert(w < ui );
  assert(lw < ui );
  assert(i < ui );
  assert(li < ui );
  
  assert(b <= ui );
  assert(sh <= ui );
  assert(sm <= ui );
  assert(w <= ui );
  assert(lw <= ui );
  assert(i <= ui );
  assert(li <= ui );
end;

var b : byte;
    sh : shortint;
    sm : smallint;
    w : word;
    lw : longword;
    i : integer;
    li : int64;
    ui : uint64;
    
begin
  b := 2; sh := 2; sm := 2; w := 2; lw := 2; i := 2; li := 2; ui := 2;
  assert(b = 2);
  assert(sh = 2);
  assert(sm = 2);
  assert(w = 2);
  assert(lw = 2);
  assert(i = 2);
  assert(li = 2);
  assert(ui = 2);
  
  assert(b = b);
  assert(sh = b);
  assert(sm = b);
  assert(w = b );
  assert(lw = b );
  assert(i = b );
  assert(li = b );
  assert(ui = b );
  
  assert(b = sh );
  assert(sh = sh );
  assert(sm = sh );
  assert(w = sh );
  assert(lw = sh );
  assert(i = sh );
  assert(li = sh );
  assert(ui = sh );
  
  assert(b = sm );
  assert(sh = sm );
  assert(sm = sm );
  assert(w = sm );
  assert(lw = sm );
  assert(i = sm );
  assert(li = sm );
  assert(ui = sm );
  
  assert(b = w );
  assert(sh = w );
  assert(sm = w );
  assert(w = w );
  assert(lw = w );
  assert(i = w );
  assert(li = w );
  assert(ui = w );
  
  assert(b = lw );
  assert(sh = lw );
  assert(sm = lw );
  assert(w = lw );
  assert(lw = lw );
  assert(i = lw );
  assert(li = lw );
  assert(ui = lw );
  
  assert(b = i );
  assert(sh = i );
  assert(sm = i );
  assert(w = i );
  assert(lw = i );
  assert(i = i );
  assert(li = i );
  assert(ui = i );
  
  assert(b = li );
  assert(sh = li );
  assert(sm = li );
  assert(w = li );
  assert(lw = li );
  assert(i = li );
  assert(li = li );
  assert(ui = li );
  
  assert(b = ui );
  assert(sh = ui );
  assert(sm = ui );
  assert(w = ui );
  assert(lw = ui );
  assert(i = ui );
  assert(li = ui );
  assert(ui = ui );
  
  Test;
  
  b := 2; sh := 3; sm := 4; w := 5; lw := 6; i := 7; li := 8; ui := 9;
  assert(b > 1);
  assert(sh > 1);
  assert(sm > 1);
  assert(w > 1);
  assert(lw > 1);
  assert(i > 1);
  assert(li > 1);
  assert(ui > 1);

  assert(b >= 1);
  assert(sh >= 1);
  assert(sm >= 1);
  assert(w >= 1);
  assert(lw >= 1);
  assert(i >= 1);
  assert(li >= 1);
  assert(ui >= 1);
  
  assert(sh > b);
  assert(sm > b);
  assert(w > b );
  assert(lw > b );
  assert(i > b );
  assert(li > b );
  assert(ui > b );

  assert(sh >= b);
  assert(sm >= b);
  assert(w >= b );
  assert(lw >= b );
  assert(i >= b );
  assert(li >= b );
  assert(ui >= b );
  
  assert(b < sh );
  assert(sm > sh );
  assert(w > sh );
  assert(lw > sh );
  assert(i > sh );
  assert(li > sh );
  assert(ui > sh );
  
  assert(b <= sh );
  assert(sm >= sh );
  assert(w >= sh );
  assert(lw >= sh );
  assert(i >= sh );
  assert(li >= sh );
  assert(ui >= sh );
  
  assert(b < sm );
  assert(sh < sm );
  assert(w > sm );
  assert(lw > sm );
  assert(i > sm );
  assert(li > sm );
  assert(ui > sm );
  
  assert(b <= sm );
  assert(sh <= sm );
  assert(w >= sm );
  assert(lw >= sm );
  assert(i >= sm );
  assert(li >= sm );
  assert(ui >= sm );
  
  assert(b < w );
  assert(sh < w );
  assert(sm < w );
  assert(lw > w );
  assert(i > w );
  assert(li > w );
  assert(ui > w );
  
  assert(b <= w );
  assert(sh <= w );
  assert(sm <= w );
  assert(lw >= w );
  assert(i >= w );
  assert(li >= w );
  assert(ui >= w );
  
  assert(b < lw );
  assert(sh < lw );
  assert(sm < lw );
  assert(w < lw );
  assert(i > lw );
  assert(li > lw );
  assert(ui > lw );
  
  assert(b <= lw );
  assert(sh <= lw );
  assert(sm <= lw );
  assert(w <= lw );
  assert(i >= lw );
  assert(li >= lw );
  assert(ui >= lw );
  
  assert(b < i );
  assert(sh < i );
  assert(sm < i );
  assert(w < i );
  assert(lw < i );
  assert(li > i );
  assert(ui > i );
  
  assert(b <= i );
  assert(sh <= i );
  assert(sm <= i );
  assert(w <= i );
  assert(lw <= i );
  assert(li >= i );
  assert(ui >= i );
  
  assert(b < li );
  assert(sh < li );
  assert(sm < li );
  assert(w < li );
  assert(lw < li );
  assert(i < li );
  assert(ui > li );

  assert(b <= li );
  assert(sh <= li );
  assert(sm <= li );
  assert(w <= li );
  assert(lw <= li );
  assert(i <= li );
  assert(ui >= li );
  
  assert(b < ui );
  assert(sh < ui );
  assert(sm < ui );
  assert(w < ui );
  assert(lw < ui );
  assert(i < ui );
  assert(li < ui );
  
  assert(b <= ui );
  assert(sh <= ui );
  assert(sm <= ui );
  assert(w <= ui );
  assert(lw <= ui );
  assert(i <= ui );
  assert(li <= ui );
  
end.