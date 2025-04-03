uses typedconst2u;
begin
assert(a=[1..4]);
assert(2 in a);
assert(b.b[1]=3);
assert(c[2]='c');
assert(d[2]=1.5);
assert('b' in e);
assert(e=['a','b','c']);
assert(f=two);
assert(one in g);
assert(g=SetOf(one,three));
assert(h=one);
assert(i=3);
assert(j[3]=2.6);
assert(k=two);
assert(three in l);
assert(l=SetOf(one,three));
assert(m.b[1]=3);
assert(n[1,2]=2);
end.