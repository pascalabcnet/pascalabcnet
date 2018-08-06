var c2: array of array of integer := ((1,2,3),(4,5),(6,7,8));
begin
  var c: array of array of integer := ((1,2,3),(4,5),(6,7,8));
  assert(c[0].Length = 3);
  assert(c[1].Length = 2);
  assert(c[2].Length = 3);
  assert(c2[0].Length = 3);
  assert(c2[1].Length = 2);
  assert(c2[2].Length = 3);
end.