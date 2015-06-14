begin
  var l := Range(2,1000).Where(x->Range(2,Round(sqrt(x))).All(i->x mod i <> 0)).ToList();
  assert(l.Count = 168);
  assert(l.Contains(3));
  assert(l.Contains(7));
  assert(l.Contains(11));
end.