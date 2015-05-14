type TRec = record 
             a : integer; 
             b : array[1..3] of integer; 
            end;

procedure Test;
const r : TRec = (a:3;b:(1,2,3));
      r2 : TRec = r;
begin
assert(r.a=3);
assert(r.b[2]=2);
assert(r2.a=3);
assert(r2.b[2]=2);
end;

procedure Test2;
const r : TRec = (a:3;b:(1,2,3));
      r2 : TRec = r;

procedure Nested;
begin
assert(r.a=3);
assert(r.b[2]=2);
assert(r2.a=3);
assert(r2.b[2]=2);  
end;

begin
assert(r.a=3);
assert(r.b[2]=2);
assert(r2.a=3);
assert(r2.b[2]=2);
Nested;
end;

const r : TRec = (a:3;b:(1,2,3));
      r2 : TRec = r;
      
begin
assert(r.a=3);
assert(r.b[2]=2);
assert(r2.a=3);
assert(r2.b[2]=2);
Test;
Test2;
end.