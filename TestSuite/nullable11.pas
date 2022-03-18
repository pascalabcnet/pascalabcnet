var i: integer;
function f1: boolean?;
begin
  Inc(i);
  Result := false;
end;
begin
  assert(f1 <> true);
  assert(i = 1);
end.
