type Fun1 = function(i: integer;k:integer): real;
var a:real;
begin
  var ff: Fun1;
  ff :=(x,y)=>x*y+0.1;
  writeln(ff(5,8));
end.