type pint = ^integer;

var  
      p:pint;
     i:integer;
begin
  i:=1;
  p:=@i;
  p^:=2;
  assert(p^=2);
end.