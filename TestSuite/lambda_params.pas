procedure p1(params fs: array of Action0);
begin
  fs[0];  
end;

begin
  var i := 0;
  p1(()->
  begin
    i := 1;
  end);
  assert(i = 1);
end.