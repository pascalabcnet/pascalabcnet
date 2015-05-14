begin
  var a:=(x:integer, y:real)=>x*y;
  writeln(a(5,4.5));
  var b:=(x, y:real)=>integer(x)*y;
  writeln(b(5,4.5));
  var d:=(x:integer, y)=>x*real(y);
  writeln(d(5,4.5));
  var c:=(x, y)=>integer(x)*real(y);
  writeln(c(5,4.5));
end.