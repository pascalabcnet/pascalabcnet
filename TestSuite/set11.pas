begin
  var tupl := (1,2,[3,4,5]);
  assert(tupl.Item1 = 1);
  assert(tupl.Item3 = SetOf(3,4,5));
  assert(4 in tupl.Item3);
  var s: set of integer := tupl.Item3;
  assert(4 in s);
end.