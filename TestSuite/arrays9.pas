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
  a2[test mod 3] += 1;
  assert(i = 5);
  a2[a[test]] := 0;
  a2[a[test]] += 1;
  assert(a2[a[test]] = 1);
  assert(i = 8);
end.