function Gen: sequence of integer;
begin
  var i := 3;
  if (i mod 2 <> 0) then
  begin
    if (i mod 3 = 0) then
      i *= 4;
    yield i * 33;

    if (i mod 3 = 0) then
      yield i * 2;

    yield i;

    if (i mod 3 = 0) then 
      yield i * 2;
  end
  else yield i * 10;

  if (i mod 12 <> 0) then
  begin
    i += 1
  end
  else 
  begin 
    if (i mod 4 = 0) then
      yield 888
    else 
      yield 777
  end;
  

  if (i mod 2 = 0) then
  begin
    for var x := 1 to 10 do
      yield x;
  end;
  i += 1;
  if (i mod 5 = 0) then
    yield i;
end; 


begin
  var q := Gen();
  q.Println;
end.