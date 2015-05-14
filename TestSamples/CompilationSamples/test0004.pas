var b : byte:=10;
    c : char:='s';
    i : integer:=23123;
    sm : smallint:=134;
    sh : shortint:=454;
    w : word:=212;
    lw : longword:=15567897;
    li : longint:=10000000000;
    ui : uint64:=10000000000;
    s : single:=3.14;
    r : real:=2.71;
    f : boolean:=false;
    str : string:='bbb';
    d1 : 1..17;
    d2 : 'a'..'z';
    db : byte(1)..byte(5);
    dsm : smallint(1)..smallint(5);
    dsh : shortint(1)..shortint(5);
    dw : word(1)..word(5);
    dli : longint(1)..longint(10);
    dlw : longword(1)..longword(10);
    dui : uint64(1)..uint64(10);
    
begin
d1 := b; d1 := sm; d1 := sh; d1 := i; d1 := w; d1 := lw; d1 := li; d1 := ui;
db := b; db := sm; db := sh; db := i; db := w; db := lw; db := li; db := ui;
dsm := b; dsm := sm; dsm := sh; dsm := i; dsm := w; dsm := lw; dsm := li; dsm := ui;
dsh := b; dsh := sm; dsh := sh; dsh := i; dsh := w; dsh := lw; dsh := li; dsh := ui;
dw := b; dw := sm; dw := sh; dw := i; dw := w; dw := lw; dw := li; dw := ui;
dli := b; dli := sm; dli := sh; dli := i; dli := w; dli := lw; dli := li; dli := ui;
dlw := b; dlw := sm; dlw := sh; dlw := i; dlw := w; dlw := lw; dlw := li; dlw := ui;
dui := b; dui := sm; dui := sh; dui := i; dui := w; dui := lw; dui := li; dui := ui;

d2 := c; c := d2;
b := d1; b := db; b := dsm; b := dsh; b := dw; b := dli; b := dlw; b := dui;
i := d1; i := db; i := dsm; i := dsh; i := dw; i := dli; i := dlw; i := dui;
sm := d1; sm := db; sm := dsm; sm := dsh; sm := dw; sm := dli; sm := dlw; sm := dui;
sh := d1; sh := db; sh := dsm; sh := dsh; sh := dw; sh := dli; sh := dlw; sh := dui;
w := d1; w := db; w := dsm; w := dsh; w := dw; w := dli; w := dlw; w := dui;
lw := d1; lw := db; lw := dsm; lw := dsh; lw := dw; lw := dli; lw := dlw; lw := dui;
li := d1; li := db; li := dsm; li := dsh; li := dw; li := dli; li := dlw; li := dui;
ui := d1; ui := db; ui := dsm; ui := dsh; ui := dw; ui := dli; ui := dlw; ui := dui;
r := d1; r := db; r := dsm; r := dsh; r := dw; r := dli; r := dlw; r := dui;
s := d1; s := db; s := dsm; s := dsh; s := dw; s := dli; s := dlw; s := dui;

b := byte(d2); i := integer(d2); sm := smallint(d2); sh := shortint(d2); w := word(d2);
lw := longword(d2); li := longint(li); ui := uint64(d2);
c := char(d1); c := char(db); c := char(dsh); c := char(dsm); c := char(dw); c := char(dlw); c := char(dli);
c := char(dui);
end.