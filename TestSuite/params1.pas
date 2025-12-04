function Test(params a : array of integer) : integer;
begin
Result := a[0]+a[1];
end;

var t : integer := Test(1,2); 
  
begin
assert(t=3);
end.