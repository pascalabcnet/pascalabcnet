function Test(params a : array of integer) : integer;
begin
Result := a[0]-a[1];
end;

function Test(a,b: integer) : integer;
begin
Result := a+b;
end;

function Test2(params a : array of real) : integer;
begin
Result := round(a[0]-a[1]);
end;

function Test2(a,b: integer) : integer;
begin
Result := a+b;
end;

begin
assert(Test(2,3)=5);
assert(Test2(2.2,1.1)=1);
end.