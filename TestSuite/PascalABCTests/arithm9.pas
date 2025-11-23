var b : byte := 1;
      sh : shortint := 1;
      w : word := 1;
      sm : smallint := 1;
      lw : longword := 1;
      i := 1;
      li : int64 := 1;
      ui : uint64 := 1;
      
begin
assert(b+b=2);
assert(b+sh=2);
assert(b+w=2);
assert(b+sm=2);
assert(b+i=2);
assert(b+lw=2);
assert(b+li=2);
assert(b+ui=2);

assert(sh+b=2);
assert(sh+sh=2);
assert(sh+w=2);
assert(sh+sm=2);
assert(sh+i=2);
assert(sh+lw=2);
assert(sh+li=2);
assert(sh+ui=2);

assert(sm+b=2);
assert(sm+sh=2);
assert(sm+w=2);
assert(sm+sm=2);
assert(sm+i=2);
assert(sm+lw=2);
assert(sm+li=2);
assert(sm+ui=2);

assert(w+b=2);
assert(w+sh=2);
assert(w+w=2);
assert(w+sm=2);
assert(w+i=2);
assert(w+lw=2);
assert(w+li=2);
assert(w+ui=2);

assert(lw+b=2);
assert(lw+sh=2);
assert(lw+w=2);
assert(lw+sm=2);
assert(lw+i=2);
assert(lw+lw=2);
assert(lw+li=2);
assert(lw+ui=2);

assert(i+b=2);
assert(i+sh=2);
assert(i+w=2);
assert(i+sm=2);
assert(i+i=2);
assert(i+lw=2);
assert(i+li=2);
assert(i+ui=2);

assert(li+b=2);
assert(li+sh=2);
assert(li+w=2);
assert(li+sm=2);
assert(li+i=2);
assert(li+lw=2);
assert(li+li=2);
assert(li+ui=2);

assert(ui+b=2);
assert(ui+sh=2);
assert(ui+w=2);
assert(ui+sm=2);
assert(ui+i=2);
assert(ui+lw=2);
assert(ui+li=2);
assert(ui+ui=2);

assert(b-b=0);
assert(b-sh=0);
assert(b-w=0);
assert(b-sm=0);
assert(b-i=0);
assert(b-lw=0);
assert(b-li=0);
assert(b-ui=0);

assert(sh-b=0);
assert(sh-sh=0);
assert(sh-w=0);
assert(sh-sm=0);
assert(sh-i=0);
assert(sh-lw=0);
assert(sh-li=0);
assert(sh-ui=0);

assert(sm-b=0);
assert(sm-sh=0);
assert(sm-w=0);
assert(sm-sm=0);
assert(sm-i=0);
assert(sm-lw=0);
assert(sm-li=0);
assert(sm-ui=0);

assert(w-b=0);
assert(w-sh=0);
assert(w-w=0);
assert(w-sm=0);
assert(w-i=0);
assert(w-lw=0);
assert(w-li=0);
assert(w-ui=0);

assert(lw-b=0);
assert(lw-sh=0);
assert(lw-w=0);
assert(lw-sm=0);
assert(lw-i=0);
assert(lw-lw=0);
assert(lw-li=0);
assert(lw-ui=0);

assert(i-b=0);
assert(i-sh=0);
assert(i-w=0);
assert(i-sm=0);
assert(i-i=0);
assert(i-lw=0);
assert(i-li=0);
assert(i-ui=0);

assert(li-b=0);
assert(li-sh=0);
assert(li-w=0);
assert(li-sm=0);
assert(li-i=0);
assert(li-lw=0);
assert(li-li=0);
assert(li-ui=0);

assert(ui-b=0);
assert(ui-sh=0);
assert(ui-w=0);
assert(ui-sm=0);
assert(ui-i=0);
assert(ui-lw=0);
assert(ui-li=0);
assert(ui-ui=0);

assert(b*b=1);
assert(b*sh=1);
assert(b*w=1);
assert(b*sm=1);
assert(b*i=1);
assert(b*lw=1);
assert(b*li=1);
assert(b*ui=1);

assert(sh*b=1);
assert(sh*sh=1);
assert(sh*w=1);
assert(sh*sm=1);
assert(sh*i=1);
assert(sh*lw=1);
assert(sh*li=1);
assert(sh*ui=1);

assert(sm*b=1);
assert(sm*sh=1);
assert(sm*w=1);
assert(sm*sm=1);
assert(sm*i=1);
assert(sm*lw=1);
assert(sm*li=1);
assert(sm*ui=1);

assert(w*b=1);
assert(w*sh=1);
assert(w*w=1);
assert(w*sm=1);
assert(w*i=1);
assert(w*lw=1);
assert(w*li=1);
assert(w*ui=1);

assert(lw*b=1);
assert(lw*sh=1);
assert(lw*w=1);
assert(lw*sm=1);
assert(lw*i=1);
assert(lw*lw=1);
assert(lw*li=1);
assert(lw*ui=1);

assert(i*b=1);
assert(i*sh=1);
assert(i*w=1);
assert(i*sm=1);
assert(i*i=1);
assert(i*lw=1);
assert(i*li=1);
assert(i*ui=1);

assert(li*b=1);
assert(li*sh=1);
assert(li*w=1);
assert(li*sm=1);
assert(li*i=1);
assert(li*lw=1);
assert(li*li=1);
assert(li*ui=1);

assert(ui*b=1);
assert(ui*sh=1);
assert(ui*w=1);
assert(ui*sm=1);
assert(ui*i=1);
assert(ui*lw=1);
assert(ui*li=1);
assert(ui*ui=1);

assert(b div b=1);
assert(b div sh=1);
assert(b div w=1);
assert(b div sm=1);
assert(b div i=1);
assert(b div lw=1);
assert(b div li=1);
assert(b div ui=1);

assert(sh div b=1);
assert(sh div sh=1);
assert(sh div w=1);
assert(sh div sm=1);
assert(sh div i=1);
assert(sh div lw=1);
assert(sh div li=1);
assert(sh div ui=1);

assert(sm div b=1);
assert(sm div sh=1);
assert(sm div w=1);
assert(sm div sm=1);
assert(sm div i=1);
assert(sm div lw=1);
assert(sm div li=1);
assert(sm div ui=1);

assert(w div b=1);
assert(w div sh=1);
assert(w div w=1);
assert(w div sm=1);
assert(w div i=1);
assert(w div lw=1);
assert(w div li=1);
assert(w div ui=1);

assert(lw div b=1);
assert(lw div sh=1);
assert(lw div w=1);
assert(lw div sm=1);
assert(lw div i=1);
assert(lw div lw=1);
assert(lw div li=1);
assert(lw div ui=1);

assert(i div b=1);
assert(i div sh=1);
assert(i div w=1);
assert(i div sm=1);
assert(i div i=1);
assert(i div lw=1);
assert(i div li=1);
assert(i div ui=1);

assert(li div b=1);
assert(li div sh=1);
assert(li div w=1);
assert(li div sm=1);
assert(li div i=1);
assert(li div lw=1);
assert(li div li=1);
assert(li div ui=1);

assert(ui div b=1);
assert(ui div sh=1);
assert(ui div w=1);
assert(ui div sm=1);
assert(ui div i=1);
assert(ui div lw=1);
assert(ui div li=1);
assert(ui div ui=1);

assert(b mod b=0);
assert(b mod sh=0);
assert(b mod w=0);
assert(b mod sm=0);
assert(b mod i=0);
assert(b mod lw=0);
assert(b mod li=0);
assert(b mod ui=0);

assert(sh mod b=0);
assert(sh mod sh=0);
assert(sh mod w=0);
assert(sh mod sm=0);
assert(sh mod i=0);
assert(sh mod lw=0);
assert(sh mod li=0);
assert(sh mod ui=0);

assert(sm mod b=0);
assert(sm mod sh=0);
assert(sm mod w=0);
assert(sm mod sm=0);
assert(sm mod i=0);
assert(sm mod lw=0);
assert(sm mod li=0);
assert(sm mod ui=0);

assert(w mod b=0);
assert(w mod sh=0);
assert(w mod w=0);
assert(w mod sm=0);
assert(w mod i=0);
assert(w mod lw=0);
assert(w mod li=0);
assert(w mod ui=0);

assert(lw mod b=0);
assert(lw mod sh=0);
assert(lw mod w=0);
assert(lw mod sm=0);
assert(lw mod i=0);
assert(lw mod lw=0);
assert(lw mod li=0);
assert(lw mod ui=0);

assert(i mod b=0);
assert(i mod sh=0);
assert(i mod w=0);
assert(i mod sm=0);
assert(i mod i=0);
assert(i mod lw=0);
assert(i mod li=0);
assert(i mod ui=0);

assert(li mod b=0);
assert(li mod sh=0);
assert(li mod w=0);
assert(li mod sm=0);
assert(li mod i=0);
assert(li mod lw=0);
assert(li mod li=0);
assert(li mod ui=0);

assert(ui mod b=0);
assert(ui mod sh=0);
assert(ui mod w=0);
assert(ui mod sm=0);
assert(ui mod i=0);
assert(ui mod lw=0);
assert(ui mod li=0);
assert(ui mod ui=0);

assert(byte(b)=1);
assert(byte(sh)=1);
assert(byte(sm)=1);
assert(byte(w)=1);
assert(byte(i)=1);
assert(byte(lw)=1);
assert(byte(li)=1);
assert(byte(ui)=1);

assert(shortint(b)=1);
assert(shortint(sh)=1);
assert(shortint(sm)=1);
assert(shortint(w)=1);
assert(shortint(i)=1);
assert(shortint(lw)=1);
assert(shortint(li)=1);
assert(shortint(ui)=1);

assert(smallint(b)=1);
assert(smallint(sh)=1);
assert(smallint(sm)=1);
assert(smallint(w)=1);
assert(smallint(i)=1);
assert(smallint(lw)=1);
assert(smallint(li)=1);
assert(smallint(ui)=1);

assert(word(b)=1);
assert(word(sh)=1);
assert(word(sm)=1);
assert(word(w)=1);
assert(word(i)=1);
assert(word(lw)=1);
assert(word(li)=1);
assert(word(ui)=1);

assert(integer(b)=1);
assert(integer(sh)=1);
assert(integer(sm)=1);
assert(integer(w)=1);
assert(integer(i)=1);
assert(integer(lw)=1);
assert(integer(li)=1);
assert(integer(ui)=1);

assert(longword(b)=1);
assert(longword(sh)=1);
assert(longword(sm)=1);
assert(longword(w)=1);
assert(longword(i)=1);
assert(longword(lw)=1);
assert(longword(li)=1);
assert(longword(ui)=1);

assert(int64(b)=1);
assert(int64(sh)=1);
assert(int64(sm)=1);
assert(int64(w)=1);
assert(int64(i)=1);
assert(int64(lw)=1);
assert(int64(li)=1);
assert(int64(ui)=1);

assert(uint64(b)=1);
assert(uint64(sh)=1);
assert(uint64(sm)=1);
assert(uint64(w)=1);
assert(uint64(i)=1);
assert(uint64(lw)=1);
assert(uint64(li)=1);
assert(uint64(ui)=1);
end.