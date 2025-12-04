unit u_compare1;
var i : uint64;
    j : longword;
    k : word;
    l : byte;
    
begin
i := int64.MaxValue;
Inc(i);
assert(i>int64.MaxValue);
Dec(i,2);
assert(i=int64.MaxValue-1);
j := integer.MaxValue;
Inc(j);
assert(j>integer.MaxValue);
Dec(j,2);
assert(j=integer.MaxValue-1);
k := smallint.MaxValue;
Inc(k);
assert(k>smallint.MaxValue);
assert(k=smallint.MaxValue+1);
l := shortint.MaxValue;
Inc(l);
assert(l>shortint.MaxValue);
assert(l=shortint.MaxValue+1);
end.