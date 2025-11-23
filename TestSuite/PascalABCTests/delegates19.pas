var i: integer;
procedure p1;
begin
  
end;
procedure p2;
begin
  
end;
procedure p3(p: procedure);
begin
  i := 1;
end;
begin
  p3(true?p1:p2);
  assert(i = 1);
end.