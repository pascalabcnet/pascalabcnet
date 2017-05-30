begin
  var p: procedure(x: integer);
  p := x -> if True then Print(x);
  p(666);
end.