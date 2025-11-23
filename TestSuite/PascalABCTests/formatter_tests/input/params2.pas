procedure Test(params a : array of ^integer);
begin
assert(a[0]^=1);
assert(a[1]^=2);
end;

var i,j : integer;
  
begin
i := 1; j := 2;
Test(@i,@j);
end.