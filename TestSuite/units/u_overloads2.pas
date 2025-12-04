unit u_overloads2;
var i : integer;

procedure Test(a : set of 1..3);
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