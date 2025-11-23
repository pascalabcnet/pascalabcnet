uses u_property3;

begin
var b: t2<byte>;
var c: t1 := new t1;
c[2] := 2;
assert(c.i = 2);
end.
