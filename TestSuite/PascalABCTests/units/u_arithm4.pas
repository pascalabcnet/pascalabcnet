unit u_arithm4;
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
  assert(b div 2 = 1);
  assert(sh div 2 = 1);
  assert(sm div 2 = 1);
  assert(w div 2 = 1);
  assert(lw div 2 = 1);
  assert(i div 2 = 1);
  assert(li div 2 = 1);
  assert(ui div 2 = 1);
  
  assert(b div b = 1);
  assert(sh div b = 1);
  assert(sm div b = 1);
  assert(w div b = 1);
  assert(lw div b = 1);
  assert(i div b = 1);
  assert(li div b = 1);
  assert(ui div b = 1);
  
  assert(b div sh = 1);
  assert(sh div sh = 1);
  assert(sm div sh = 1);
  assert(w div sh = 1);
  assert(lw div sh = 1);
  assert(i div sh = 1);
  assert(li div sh = 1);
  assert(ui div sh = 1);
  
  assert(b div sm = 1);
  assert(sh div sm = 1);
  assert(sm div sm = 1);
  assert(w div sm = 1);
  assert(lw div sm = 1);
  assert(i div sm = 1);
  assert(li div sm = 1);
  assert(ui div sm = 1);
  
  assert(b div w = 1);
  assert(sh div w = 1);
  assert(sm div w = 1);
  assert(w div w = 1);
  assert(lw div w = 1);
  assert(i div w = 1);
  assert(li div w = 1);
  assert(ui div w = 1);
  
  assert(b div lw = 1);
  assert(sh div lw = 1);
  assert(sm div lw = 1);
  assert(w div lw = 1);
  assert(lw div lw = 1);
  assert(i div lw = 1);
  assert(li div lw = 1);
  assert(ui div lw = 1);
  
  assert(b div i = 1);
  assert(sh div i = 1);
  assert(sm div i = 1);
  assert(w div i = 1);
  assert(lw div i = 1);
  assert(i div i = 1);
  assert(li div i = 1);
  assert(ui div i = 1);
  
  assert(b div li = 1);
  assert(sh div li = 1);
  assert(sm div li = 1);
  assert(w div li = 1);
  assert(lw div li = 1);
  assert(i div li = 1);
  assert(li div li = 1);
  assert(ui div li = 1);
  
  assert(b div ui = 1);
  assert(sh div ui = 1);
  assert(sm div ui = 1);
  assert(w div ui = 1);
  assert(lw div ui = 1);
  assert(i div ui = 1);
  assert(li div ui = 1);
  assert(ui div ui = 1);
  
  //mod
  assert(b mod 2 = 0);
  assert(sh mod 2 = 0);
  assert(sm mod 2 = 0);
  assert(w mod 2 = 0);
  assert(lw mod 2 = 0);
  assert(i mod 2 = 0);
  assert(li mod 2 = 0);
  assert(ui mod 2 = 0);
  
  assert(b mod b = 0);
  assert(sh mod b = 0);
  assert(sm mod b = 0);
  assert(w mod b = 0);
  assert(lw mod b = 0);
  assert(i mod b = 0);
  assert(li mod b = 0);
  assert(ui mod b = 0);
  
  assert(b mod sh = 0);
  assert(sh mod sh = 0);
  assert(sm mod sh = 0);
  assert(w mod sh = 0);
  assert(lw mod sh = 0);
  assert(i mod sh = 0);
  assert(li mod sh = 0);
  assert(ui mod sh = 0);
  
  assert(b mod sm = 0);
  assert(sh mod sm = 0);
  assert(sm mod sm = 0);
  assert(w mod sm = 0);
  assert(lw mod sm = 0);
  assert(i mod sm = 0);
  assert(li mod sm = 0);
  assert(ui mod sm = 0);
  
  assert(b mod w = 0);
  assert(sh mod w = 0);
  assert(sm mod w = 0);
  assert(w mod w = 0);
  assert(lw mod w = 0);
  assert(i mod w = 0);
  assert(li mod w = 0);
  assert(ui mod w = 0);
  
  assert(b mod lw = 0);
  assert(sh mod lw = 0);
  assert(sm mod lw = 0);
  assert(w mod lw = 0);
  assert(lw mod lw = 0);
  assert(i mod lw = 0);
  assert(li mod lw = 0);
  assert(ui mod lw = 0);
  
  assert(b mod i = 0);
  assert(sh mod i = 0);
  assert(sm mod i = 0);
  assert(w mod i = 0);
  assert(lw mod i = 0);
  assert(i mod i = 0);
  assert(li mod i = 0);
  assert(ui mod i = 0);
  
  assert(b mod li = 0);
  assert(sh mod li = 0);
  assert(sm mod li = 0);
  assert(w mod li = 0);
  assert(lw mod li = 0);
  assert(i mod li = 0);
  assert(li mod li = 0);
  assert(ui mod li = 0);
  
  assert(b mod ui = 0);
  assert(sh mod ui = 0);
  assert(sm mod ui = 0);
  assert(w mod ui = 0);
  assert(lw mod ui = 0);
  assert(i mod ui = 0);
  assert(li mod ui = 0);
  assert(ui mod ui = 0);
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
  assert(b div 2 = 1);
  assert(sh div 2 = 1);
  assert(sm div 2 = 1);
  assert(w div 2 = 1);
  assert(lw div 2 = 1);
  assert(i div 2 = 1);
  assert(li div 2 = 1);
  assert(ui div 2 = 1);
  
  assert(b div b = 1);
  assert(sh div b = 1);
  assert(sm div b = 1);
  assert(w div b = 1);
  assert(lw div b = 1);
  assert(i div b = 1);
  assert(li div b = 1);
  assert(ui div b = 1);
  
  assert(b div sh = 1);
  assert(sh div sh = 1);
  assert(sm div sh = 1);
  assert(w div sh = 1);
  assert(lw div sh = 1);
  assert(i div sh = 1);
  assert(li div sh = 1);
  assert(ui div sh = 1);
  
  assert(b div sm = 1);
  assert(sh div sm = 1);
  assert(sm div sm = 1);
  assert(w div sm = 1);
  assert(lw div sm = 1);
  assert(i div sm = 1);
  assert(li div sm = 1);
  assert(ui div sm = 1);
  
  assert(b div w = 1);
  assert(sh div w = 1);
  assert(sm div w = 1);
  assert(w div w = 1);
  assert(lw div w = 1);
  assert(i div w = 1);
  assert(li div w = 1);
  assert(ui div w = 1);
  
  assert(b div lw = 1);
  assert(sh div lw = 1);
  assert(sm div lw = 1);
  assert(w div lw = 1);
  assert(lw div lw = 1);
  assert(i div lw = 1);
  assert(li div lw = 1);
  assert(ui div lw = 1);
  
  assert(b div i = 1);
  assert(sh div i = 1);
  assert(sm div i = 1);
  assert(w div i = 1);
  assert(lw div i = 1);
  assert(i div i = 1);
  assert(li div i = 1);
  assert(ui div i = 1);
  
  assert(b div li = 1);
  assert(sh div li = 1);
  assert(sm div li = 1);
  assert(w div li = 1);
  assert(lw div li = 1);
  assert(i div li = 1);
  assert(li div li = 1);
  assert(ui div li = 1);
  
  assert(b div ui = 1);
  assert(sh div ui = 1);
  assert(sm div ui = 1);
  assert(w div ui = 1);
  assert(lw div ui = 1);
  assert(i div ui = 1);
  assert(li div ui = 1);
  assert(ui div ui = 1);
  
  //mod
  assert(b mod 2 = 0);
  assert(sh mod 2 = 0);
  assert(sm mod 2 = 0);
  assert(w mod 2 = 0);
  assert(lw mod 2 = 0);
  assert(i mod 2 = 0);
  assert(li mod 2 = 0);
  assert(ui mod 2 = 0);
  
  assert(b mod b = 0);
  assert(sh mod b = 0);
  assert(sm mod b = 0);
  assert(w mod b = 0);
  assert(lw mod b = 0);
  assert(i mod b = 0);
  assert(li mod b = 0);
  assert(ui mod b = 0);
  
  assert(b mod sh = 0);
  assert(sh mod sh = 0);
  assert(sm mod sh = 0);
  assert(w mod sh = 0);
  assert(lw mod sh = 0);
  assert(i mod sh = 0);
  assert(li mod sh = 0);
  assert(ui mod sh = 0);
  
  assert(b mod sm = 0);
  assert(sh mod sm = 0);
  assert(sm mod sm = 0);
  assert(w mod sm = 0);
  assert(lw mod sm = 0);
  assert(i mod sm = 0);
  assert(li mod sm = 0);
  assert(ui mod sm = 0);
  
  assert(b mod w = 0);
  assert(sh mod w = 0);
  assert(sm mod w = 0);
  assert(w mod w = 0);
  assert(lw mod w = 0);
  assert(i mod w = 0);
  assert(li mod w = 0);
  assert(ui mod w = 0);
  
  assert(b mod lw = 0);
  assert(sh mod lw = 0);
  assert(sm mod lw = 0);
  assert(w mod lw = 0);
  assert(lw mod lw = 0);
  assert(i mod lw = 0);
  assert(li mod lw = 0);
  assert(ui mod lw = 0);
  
  assert(b mod i = 0);
  assert(sh mod i = 0);
  assert(sm mod i = 0);
  assert(w mod i = 0);
  assert(lw mod i = 0);
  assert(i mod i = 0);
  assert(li mod i = 0);
  assert(ui mod i = 0);
  
  assert(b mod li = 0);
  assert(sh mod li = 0);
  assert(sm mod li = 0);
  assert(w mod li = 0);
  assert(lw mod li = 0);
  assert(i mod li = 0);
  assert(li mod li = 0);
  assert(ui mod li = 0);
  
  assert(b mod ui = 0);
  assert(sh mod ui = 0);
  assert(sm mod ui = 0);
  assert(w mod ui = 0);
  assert(lw mod ui = 0);
  assert(i mod ui = 0);
  assert(li mod ui = 0);
  assert(ui mod ui = 0);
  
  Test;
end.