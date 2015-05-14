function Test(params arr: array of integer): array of integer;
begin
  Result := new integer[3](4,4,4);  
end;

begin
  var arr := Test;
  assert(arr[0] = 4);
  arr := Test;
  assert(arr[0] = 4);
  foreach i : integer in Test do
    assert(i = 4);
end.