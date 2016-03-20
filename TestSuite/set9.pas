begin
  var s: set of integer := [1,2,3];
  var hs: HashSet<integer>;
  hs := s;
  assert(3 in hs);
  hs.Println;
  var set1: set of integer := hs;
  assert(2 in set1);
end.