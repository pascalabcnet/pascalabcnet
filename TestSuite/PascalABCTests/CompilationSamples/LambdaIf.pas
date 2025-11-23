begin
  var p: procedure(x: integer);
  p := procedure(x) -> if True then Print(x);
  p(666);
end.