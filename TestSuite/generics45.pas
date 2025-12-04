begin
  var s: sequence of integer := Arr(3,2,1).AsEnumerable();
  var e: IEnumerator<integer> := s.OrderBy(b->b).GetEnumerator;
  e.MoveNext();
  assert(e.Current = 1);
  var e2: IEnumerator<integer> := s.OrderBy(b->b).GetEnumerator();
  e2.MoveNext();
  assert(e2.Current = 1);
end.