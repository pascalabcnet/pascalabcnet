begin
  var a:integer;
  a:=((x,y)=>integer(y)+123+(z=>integer(z))(8))(5,4)+
     ((x,y)=>integer(y)+123+(z=>integer(z))(8))(5,4);
  writeln(a);
end.