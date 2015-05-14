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
    str : string:='aaa';
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
sh := 1 * (-b) * (-i) * (-sm) * (-sh) * (-w) * (-lw) * (-li) * (-ui);
b := 1 + b + i + sm + sh + w + lw + li + ui;
i := 1 - b - i - sm - sh - w - lw - li - ui;
sm := 1 * b * i * sm * sh * w * lw * li * ui;

w := 1 + not b + not i + not sm + not sh + not w + not lw + not li + not ui;
lw := 1 div b div i div sm div sh div w div lw div li div ui;
lw := 1 mod b mod i mod sm mod sh mod w mod lw mod li mod ui;
r := i / r; r := b / r; r := sm / r; r := sh / r; r := w / r; r := li / r; r := lw / r; r := ui / r;
s := i / s; s := b / s; s := sm / s; s := sh / s; s := w / s; s := li / s; s := lw / s; s := ui / s;

d1 := d1 + dsm + db + dsh + dw + dli + dlw + dui + b + i + sm + sh + w + lw + li + ui;
d1 := d1 - dsm - db - dsh - dw - dli - dlw - dui - b - i - sm - sh - w - lw - li - ui;
db := d1 * dsm * db * dsh * dw * dli * dlw * dui * b * i * sm * sh * w * lw * li * ui;
db := -d1 * (-dsm) * (-db) * (-dsh) * (-dw) * (-dli) * (-dlw) * (-dui) * (-b) * (-i) * (-sm) * (-sh) * (-w) * (-lw) * (-li) * (-ui);
dli := not b + not i + not sm + not sh + not w + not lw + not li + not ui + not d1 + not dsm + not db + not dsh + not dw + not dli + not dlw + not dui;
db := d1 div dsm div db div dsh div dw div dli div dlw div dui div b div i div sm div sh div w div lw div li div ui;
db := d1 mod dsm mod db mod dsh mod dw mod dli mod dlw mod dui mod b mod i mod sm mod sh mod w mod lw mod li mod ui;
dsm := d1 and dsm and db and dsh and dw and dli and dlw and dui and b and i and sm and sh and w and lw and li and ui;
dsh := d1 or dsm or db or dsh or dw or dli or dlw or dui or b or i or sm or sh or w or lw or li or ui;
dli := d1 xor dsm xor db xor dsh xor dw xor dli xor dlw xor dui xor b xor i xor sm xor sh xor w xor lw xor li xor ui;

r := d1 / r; r := db / r; r := dsm / r; r := dsh / r; r := dw / r; r := dli / r; r := dlw / r; r := dui / r;
s := d1 / s; s := db / s; s := dsm / s; s := dsh / s; s := dw / s; s := dli / s; s := dlw / s; s := dui / s;
 
f := b < i; f := b <= i; f := b = i; f := b > i; f := b >= i; f := b <> i;
f := b < sh; f := b <= sh; f := b = sh; f := b > sh; f := b >= sh; f := b <> sh;
f := b < sm; f := b <= sm; f := b = sm; f := b > sm; f := b >= sm; f := b <> sm;
f := b < w; f := b <= w; f := b = w; f := b > w; f := b >= w; f := b <> w;
f := b < lw; f := b <= lw; f := b = lw; f := b > lw; f := b >= lw; f := b <> lw;
f := b < li; f := b <= li; f := b = li; f := b > li; f := b >= li; f := b <> li;
f := b < ui; f := b <= ui; f := b = ui; f := b > ui; f := b >= ui; f := b <> ui;

f := b < d1; f := b <= d1; f := b = d1; f := b > d1; f := b >= d1; f := b <> d1;
f := b < db; f := b <= db; f := b = db; f := b > db; f := b >= db; f := b <> db;
f := b < dsh; f := b <= dsh; f := b = dsh; f := b > dsh; f := b >= dsh; f := b <> dsh;
f := b < dsm; f := b <= dsm; f := b = dsm; f := b > dsm; f := b >= dsm; f := b <> dsm;
f := b < dw; f := b <= dw; f := b = dw; f := b > dw; f := b >= dw; f := b <> dw;
f := b < dlw; f := b <= dlw; f := b = dlw; f := b > dlw; f := b >= dlw; f := b <> dlw;
f := b < dli; f := b <= dli; f := b = dli; f := b > dli; f := b >= dli; f := b <> dli;
f := b < dui; f := b <= dui; f := b = dui; f := b > dui; f := b >= dui; f := b <> dui;

f := i < d1; f := i <= d1; f := i = d1; f := i > d1; f := i >= d1; f := i <> d1;
f := i < db; f := i <= db; f := i = db; f := i > db; f := i >= db; f := i <> db;
f := i < dsh; f := i <= dsh; f := i = dsh; f := i > dsh; f := i >= dsh; f := i <> dsh;
f := i < dsm; f := i <= dsm; f := i = dsm; f := i > dsm; f := i >= dsm; f := i <> dsm;
f := i < dw; f := i <= dw; f := i = dw; f := i > dw; f := i >= dw; f := i <> dw;
f := i < dlw; f := i <= dlw; f := i = dlw; f := i > dlw; f := i >= dlw; f := i <> dlw;
f := i < dli; f := i <= dli; f := i = dli; f := i > dli; f := i >= dli; f := i <> dli;
f := i < dui; f := i <= dui; f := i = dui; f := i > dui; f := i >= dui; f := i <> dui;

f := d1 < d1; f := d1 <= d1; f := d1 = d1; f := d1 > d1; f := d1 >= d1; f := d1 <> d1;
f := d1 < db; f := d1 <= db; f := d1 = db; f := d1 > db; f := d1 >= db; f := d1 <> db;
f := d1 < dsh; f := d1 <= dsh; f := d1 = dsh; f := d1 > dsh; f := d1 >= dsh; f := d1 <> dsh;
f := d1 < dsm; f := d1 <= dsm; f := d1 = dsm; f := d1 > dsm; f := d1 >= dsm; f := d1 <> dsm;
f := d1 < dw; f := d1 <= dw; f := d1 = dw; f := d1 > dw; f := d1 >= dw; f := d1 <> dw;
f := d1 < dlw; f := d1 <= dlw; f := d1 = dlw; f := d1 > dlw; f := d1 >= dlw; f := d1 <> dlw;
f := d1 < dli; f := d1 <= dli; f := d1 = dli; f := d1 > dli; f := d1 >= dli; f := d1 <> dli;
f := d1 < dui; f := d1 <= dui; f := d1 = dui; f := d1 > dui; f := d1 >= dui; f := d1 <> dui;

f := (d1 < db) and (db < dsm) or (dsm < dsh) and (dsh < dw) and (dw < dli) and (dli < dui);

i := sm*(dsm+not dli) div 3 + db*(dlw - lw + w* sh);

end.