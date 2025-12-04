var i: integer;
procedure test;
begin
  Inc(i);
end;

procedure Cycle(p: procedure);
begin
  10.Times.ForEach(x->p()); // Всё остальное работает
end;

begin
  Cycle(test);
  assert(i=10);
end.