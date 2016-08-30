function Gen: sequence of integer;
begin
  var i := 1;
  if (i mod 2 = 0) then
  begin
    yield i;
  end;
  i += 1;
  if (i mod 2 = 0) then
  begin
    yield i;
  end;
  i += 1;
  if (i mod 2 = 0) then
  begin
    yield i;
  end;
end; 


begin
  var q := Gen();
  foreach var x in q do
    writeln(x);
end.