unit u_constparam2;
procedure Test2(const n : integer);
begin
assert(n=10);
end;
procedure Test(const m : integer);
begin
  Test2(m);
end;

var i : integer;    
begin
i := 10;
Test(i);
end.