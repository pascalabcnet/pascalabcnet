begin
  var a := 6;
  var f : function(x: integer): integer := function(x) -> begin a += 1; result := x + a end;
  
  var t := f(a);
  assert(t = 13);
  assert(a = 7);
  t := f(a);
  assert(t = 15);
  assert(a = 8);
end.