unit u_setinoverload1;
var i : integer;

procedure Test(a : set of integer);
begin
i := 1;
end;

procedure Test(a : set of byte);
begin
i := 2;
end;

begin
Test([1..3]);
assert(i=1);
end.