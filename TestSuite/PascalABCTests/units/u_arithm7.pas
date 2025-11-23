unit u_arithm7;
var sh : shortint;
    b : byte;
    sm : smallint;
    w : word;
    lw : longword;
    i : integer;
    li : int64;
    ui : uint64;
    s : integer;
    
begin
s := 0;
for sh := 1 to 3 do
 s += sh;
assert(s=6);
s := 0;
for b := 1 to 3 do
 s += b;
assert(s=6);
s := 0;
for sm := 1 to 3 do
 s += sm;
assert(s=6);
s := 0;
for w := 1 to 3 do
 s += w;
assert(s=6);
s := 0;
for i := 1 to 3 do
 s += i;
assert(s=6);
s := 0;
for lw := 1 to 3 do
 s += lw;
assert(s=6); 
s := 0;
for li := 1 to 3 do
 s += li;
assert(s=6);
s := 0;
for ui := 1 to 3 do
 s += ui;
assert(s=6);
end.