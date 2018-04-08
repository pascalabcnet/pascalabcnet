var
  i: integer;

function test: integer;
begin
  Result := 2;
  i += 1;
end;

begin
  var a: array[1..13] of integer;
  a[test] += 1;
  assert(i = 1);
  var a2 := Arr(0, 0, 0);
  a2[test] += 1;
  assert(i = 2);
  a2[test] += a2[test];
  assert(i = 4);
end.