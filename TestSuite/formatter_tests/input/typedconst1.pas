type TArr = array[1..3] of integer;
     TRec = record a : integer; b : real; end;
     TSet = set of byte;
     
procedure Test;
const a : TArr = (1,2,3);
      b : TRec = (a:2;b:1.4);
      c : TSet = [1,2,3];
      
begin
assert(a[2]=2);
assert(b.b=1.4);
assert(2 in c);
end;

procedure Test2;
const a : TArr = (1,2,3);
      b : TRec = (a:2;b:1.4);
      c : TSet = [1,2,3];
      
procedure Nested;
begin
assert(a[2]=2);
assert(b.b=1.4);
assert(2 in c);
end;

begin
assert(a[2]=2);
assert(b.b=1.4);
assert(2 in c);
Nested;
end;

const a : TArr = (1,2,3);
      b : TRec = (a:2;b:1.4);
      c : TSet = [1,2,3];
      
begin
assert(a[2]=2);
assert(b.b=1.4);
assert(2 in c);
Test;
Test2;
end.