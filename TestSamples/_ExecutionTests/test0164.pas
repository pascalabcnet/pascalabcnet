uses test0164u;
const c1 = c;

var v := c;
begin

assert(c=ord('k')-ord('a'));
assert(c1=c);
assert(v=c);
end. 