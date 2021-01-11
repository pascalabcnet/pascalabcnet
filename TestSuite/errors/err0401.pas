begin
  var s: sequence of byte;
  var e: IEnumerator<char> := s.OrderBy(b->b).GetEnumerator;
end.