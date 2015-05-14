type pint=^integer;
var  p:pint;
     i:integer;
begin
  i:=1;
  p:=@i;
  p^:=2;
  writeln(p^);
end.