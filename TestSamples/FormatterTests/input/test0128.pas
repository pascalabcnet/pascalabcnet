var b : byte:=1;
    sh : shortint:=1;
    sm : smallint:=1;
    w : word:=1;
    i : integer:=1;
    lw : longword:=1;
    li : int64:=1;
    ui : uint64:=1;
    
    s1 : set of byte;
    s2 : set of shortint;
    s3 : set of smallint;
    s4 : set of word;
    s5 : set of integer;
    s6 : set of longword;
    s7 : set of int64;
    s8 : set of uint64;
    
begin
Include(s1,b);
Include(s2,sh);
Include(s3,sm);
Include(s4,w);
Include(s5,i);
Include(s6,lw);
Include(s7,li);
Include(s8,ui);
assert(sh in s1);
assert(sm in s1);
assert(w in s1);
assert(i in s1);
assert(lw in s1);
assert(li in s1);
assert(ui in s1);
s2 := [1,3];
s4 := [1,2];
assert(s2*s4=s1);
assert(s2*s4=s3);
assert(s2*s4=s5);
assert(s2*s4=s7);
assert(s2*s4=s8);
end.