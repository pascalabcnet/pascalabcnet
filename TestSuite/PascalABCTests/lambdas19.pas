var i: integer;

procedure p1<T>(p: procedure);
begin
  var pp := procedure->
  begin
    // Обязательно захватить p
    p := p;
    Inc(i);
  end;
  pp;
end;

begin 
  p1&<integer>(()->begin
                     
                   end);
  assert(i = 1);
end.