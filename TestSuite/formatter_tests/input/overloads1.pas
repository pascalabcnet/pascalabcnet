var i : integer;

procedure Test(a : 1..4);
begin
i := 1;
end;

procedure Test(a : byte);
begin
i := 2;
end;

begin
Test(2);
assert(i=2);
end.