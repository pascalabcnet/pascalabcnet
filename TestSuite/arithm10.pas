var 
    sh : shortint;
    sm : smallint;
    i : integer;
    li : int64;
    f: single;
    r: real;
    
begin
sh := 1; 
sm := 1; 
i := 1; 
li := 1;
f := 1;
r := 1;
sh := -sh;
sm := -sm;
i := -i;
li := -li;
f := -f;
r := -r;
assert(sh = -1);
assert(sm = -1);
assert(i = -1);
assert(li = -1);
assert(abs(r+1) < 0.00001);
assert(abs(f+1) < 0.00001);
end.