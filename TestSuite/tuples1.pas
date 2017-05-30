type
  Point = (integer, integer);

begin
  var a: Point := (1, 2);
  assert(a.Item1 = 1);
  assert(a.Item2 = 2);
end.