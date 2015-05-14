var b : boolean;
    i : integer;
    b2 : false..true;
    
begin
b := true; i := integer(b);
assert(i=1);
i := 0;
for var j:= false to true do
begin
assert(integer(j)=i);
Inc(i);
end;
i := 1;
for var j:= true to false do
begin
assert(integer(j)=i);
Dec(i);
end;
i := 0;
for var j:= false to false do
begin
assert(integer(j)=i);
end;
i := 1;
for var j:= true to true do
begin
assert(integer(j)=i);
end;
assert(true in [false..true]);
assert(false in [false..true]);
assert(true in [true]);
assert(false in [false]);
b := false; Inc(b);
assert(b=true);
Inc(b);
assert(b=false);
b := true;
Dec(b); assert(b=false);
Dec(b); assert(b=true);
b := false;
Inc(b,1); assert(b=true);
Dec(b,1); assert(b=false);
Inc(b,2); assert(b=false);
Inc(b,10000000001); assert(b=true);
end.