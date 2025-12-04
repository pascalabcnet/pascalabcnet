function f1 := 1;

function p1(n: integer?): integer;
begin
  Result := (n??f1).Value;
end;

begin
  // OK
  assert(p1(nil) = 1);
  // AccessViolationException
  assert(p1(5) = 5);
end.