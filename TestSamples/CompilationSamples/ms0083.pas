type pinteger=^integer;

var i1,i2: integer;
    p: pointer;

begin
  i1:=10;i2:=11;
  writeln(integer(@i1));
  writeln(integer(@i2));
  writeln(pinteger(pointer(integer(@i1)+4))^);  
  readln;
end.