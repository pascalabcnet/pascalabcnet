function Gen: sequence of integer;
begin
  var i := 1;
  while i<=10 do
  begin
    yield i*i;
    i += 1;    
  end;
end;

begin
  write(1)
end.