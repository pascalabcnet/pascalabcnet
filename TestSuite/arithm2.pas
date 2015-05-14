var b : byte;
    sh : shortint;
    sm : smallint;
    w : word;
    i : integer;
    lw : longword;
    li : int64;
    ui : uint64;
    
begin
b := 1; sh := 1; sm := 1; w := 1; i := 1; lw := 1; li := 1; ui := 1;
assert(b+sh+sm+w+i+lw+li=7);
b := b+sh+sm+w+i+lw+li; assert(b=7); b:=1;
sh := b+sh+sm+w+i+lw+li; assert(sh=7); sh:=1;
sm := b+sh+sm+w+i+lw+li; assert(sm=7); sm:=1;
w := b+sh+sm+w+i+lw+li; assert(w=7); w:=1;
i := b+sh+sm+w+i+lw+li; assert(i=7); i:=1;
lw := b+sh+sm+w+i+lw+li; assert(lw=7); lw:=1;
li := b+sh+sm+w+i+lw+li; assert(li=7); li:=1;
ui := b+sh+sm+w+i+lw+li; assert(ui=7); ui:=1;
end.