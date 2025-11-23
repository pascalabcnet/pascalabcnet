procedure mywriteln(i: integer);
begin
  assert(i = 4);
end;

begin
  var s: string[4] := '2222';
  assert(s.Count = 4);
  mywriteln(s.Count);
end.